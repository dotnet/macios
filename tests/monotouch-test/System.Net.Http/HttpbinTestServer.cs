// Copyright (c) Microsoft Corporation.
// Licensed under the MIT License.

//
// An in-process HTTP server that mimics httpbin.org endpoints for testing,
// eliminating external network dependencies and the associated flakiness.
//

using System;
using System.Collections.Generic;
using System.IO;
using System.IO.Compression;
using System.Linq;
using System.Net;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace MonoTests.System.Net.Http {
	[Preserve (AllMembers = true)]
	static class HttpbinTestServer {
		static readonly Lazy<string> lazyBaseUrl = new Lazy<string> (Start);

		public static string BaseUrl => lazyBaseUrl.Value;

		static string Start ()
		{
			// IANA suggested range for dynamic/private ports
			const int MinPort = 49152;
			const int MaxPort = 65535;

			HttpListener? listener = null;
			int port = -1;

			for (var p = MinPort; p < MaxPort; p++) {
				listener = new HttpListener ();
				listener.Prefixes.Add ($"http://127.0.0.1:{p}/");
				try {
					listener.Start ();
					port = p;
					break;
				} catch {
					// port in use, try next
					listener.Close ();
					listener = null;
				}
			}

			if (listener is null || port == -1)
				throw new InvalidOperationException ("HttpbinTestServer: Could not find an available port.");

			Task.Run (async () => {
				try {
					while (listener.IsListening) {
						var context = await listener.GetContextAsync ();
						_ = Task.Run (() => {
							try {
								HandleRequest (context);
							} catch (Exception ex) {
								try {
									context.Response.StatusCode = 500;
									var body = Encoding.UTF8.GetBytes (ex.ToString ());
									context.Response.OutputStream.Write (body, 0, body.Length);
								} catch {
									// nothing we can do
								}
							} finally {
								try {
									context.Response.Close ();
								} catch {
									// nothing we can do
								}
							}
						});
					}
				} catch (ObjectDisposedException) {
					// listener was stopped
				} catch (HttpListenerException) {
					// listener was stopped
				}
			});

			return $"http://127.0.0.1:{port}";
		}

		static void HandleRequest (HttpListenerContext context)
		{
			var path = context.Request.Url!.AbsolutePath;

			if (path == "/get" || path == "/") {
				HandleGet (context);
			} else if (path == "/post") {
				HandlePost (context);
			} else if (path == "/cookies") {
				HandleCookies (context);
			} else if (path.StartsWith ("/cookies/set", StringComparison.Ordinal)) {
				HandleSetCookies (context);
			} else if (path.StartsWith ("/redirect-to", StringComparison.Ordinal)) {
				HandleRedirectTo (context);
			} else if (path.StartsWith ("/redirect/", StringComparison.Ordinal)) {
				HandleRedirect (context);
			} else if (path.StartsWith ("/basic-auth/", StringComparison.Ordinal)) {
				HandleBasicAuth (context);
			} else if (path.StartsWith ("/digest-auth/", StringComparison.Ordinal)) {
				HandleDigestAuth (context);
			} else if (path == "/gzip") {
				HandleGzip (context);
			} else if (path == "/html") {
				HandleHtml (context);
			} else if (path.StartsWith ("/status/", StringComparison.Ordinal)) {
				HandleStatus (context);
			} else {
				// Default: 200 OK with empty JSON
				WriteJsonResponse (context, "{}");
			}
		}

		// GET / or GET /get: return request info as JSON (including headers)
		static void HandleGet (HttpListenerContext context)
		{
			var headerLines = new List<string> ();
			foreach (string key in context.Request.Headers) {
				var value = context.Request.Headers [key]!;
				headerLines.Add ($"    \"{key}\": \"{EscapeJsonString (value)}\"");
			}

			var json = "{\n  \"headers\": {\n" + string.Join (",\n", headerLines) + "\n  }\n}";
			WriteJsonResponse (context, json);
		}

		// POST /post: echo the posted body and request info
		static void HandlePost (HttpListenerContext context)
		{
			string data;
			using (var reader = new StreamReader (context.Request.InputStream, context.Request.ContentEncoding)) {
				data = reader.ReadToEnd ();
			}

			var json = "{\n  \"data\": \"" + EscapeJsonString (data) + "\",\n  \"url\": \"" + EscapeJsonString (context.Request.Url!.ToString ()) + "\"\n}";
			WriteJsonResponse (context, json);
		}

		// GET /cookies: echo cookies from request as JSON
		static void HandleCookies (HttpListenerContext context)
		{
			var cookieLines = new List<string> ();
			var cookieHeader = context.Request.Headers ["Cookie"];
			if (cookieHeader is not null) {
				var pairs = cookieHeader.Split (';');
				foreach (var pair in pairs) {
					var trimmed = pair.Trim ();
					var eqIdx = trimmed.IndexOf ('=');
					if (eqIdx > 0) {
						var name = trimmed.Substring (0, eqIdx);
						var value = trimmed.Substring (eqIdx + 1);
						cookieLines.Add ($"    \"{name}\": \"{EscapeJsonString (value)}\"");
					}
				}
			}

			var json = "{\n  \"cookies\": {\n" + string.Join (",\n", cookieLines) + "\n  }\n}";
			WriteJsonResponse (context, json);
		}

		// GET /cookies/set?name=value: set cookie(s) via Set-Cookie header and redirect to /cookies
		static void HandleSetCookies (HttpListenerContext context)
		{
			var queryString = context.Request.QueryString;
			foreach (string? key in queryString) {
				if (key is not null) {
					context.Response.AppendHeader ("Set-Cookie", $"{key}={queryString [key]}; Path=/");
				}
			}

			context.Response.StatusCode = 302;
			context.Response.RedirectLocation = $"{BaseUrl}/cookies";
		}

		// GET /redirect/{n}: chain of redirects, ending at /get
		static void HandleRedirect (HttpListenerContext context)
		{
			var path = context.Request.Url!.AbsolutePath;
			var countStr = path.Substring ("/redirect/".Length);
			if (!int.TryParse (countStr, out var count) || count < 1) {
				context.Response.StatusCode = 400;
				return;
			}

			context.Response.StatusCode = 302;
			if (count > 1)
				context.Response.RedirectLocation = $"{BaseUrl}/redirect/{count - 1}";
			else
				context.Response.RedirectLocation = $"{BaseUrl}/get";
		}

		// GET /redirect-to?url={url}: single redirect to the specified URL
		static void HandleRedirectTo (HttpListenerContext context)
		{
			var url = context.Request.QueryString ["url"];
			if (url is null) {
				context.Response.StatusCode = 400;
				return;
			}

			context.Response.StatusCode = 302;
			context.Response.RedirectLocation = url;
		}

		// GET /basic-auth/{user}/{pass}: HTTP Basic authentication
		static void HandleBasicAuth (HttpListenerContext context)
		{
			var path = context.Request.Url!.AbsolutePath;
			var parts = path.Substring ("/basic-auth/".Length).Split ('/');
			if (parts.Length != 2) {
				context.Response.StatusCode = 400;
				return;
			}

			var validUser = Uri.UnescapeDataString (parts [0]);
			var validPass = Uri.UnescapeDataString (parts [1]);

			var authHeader = context.Request.Headers ["Authorization"];
			if (authHeader is not null && authHeader.StartsWith ("Basic ", StringComparison.Ordinal)) {
				try {
					var credentials = Encoding.UTF8.GetString (Convert.FromBase64String (authHeader.Substring (6)));
					var colonIdx = credentials.IndexOf (':');
					if (colonIdx > 0) {
						var user = credentials.Substring (0, colonIdx);
						var pass = credentials.Substring (colonIdx + 1);
						if (user == validUser && pass == validPass) {
							WriteJsonResponse (context, $"{{\"authenticated\": true, \"user\": \"{EscapeJsonString (user)}\"}}");
							return;
						}
					}
				} catch {
					// bad base64 or encoding
				}
			}

			context.Response.StatusCode = 401;
			context.Response.AddHeader ("WWW-Authenticate", "Basic realm=\"Fake Realm\"");
		}

		// GET /digest-auth/auth/{user}/{pass}: HTTP Digest authentication
		static void HandleDigestAuth (HttpListenerContext context)
		{
			var path = context.Request.Url!.AbsolutePath;
			var parts = path.Substring ("/digest-auth/auth/".Length).Split ('/');
			if (parts.Length != 2) {
				context.Response.StatusCode = 400;
				return;
			}

			var validUser = Uri.UnescapeDataString (parts [0]);
			var validPass = Uri.UnescapeDataString (parts [1]);
			const string realm = "test@example.org";

			var authHeader = context.Request.Headers ["Authorization"];
			if (authHeader is not null && authHeader.StartsWith ("Digest ", StringComparison.Ordinal)) {
				var authParams = ParseDigestAuthHeader (authHeader);

				if (authParams.TryGetValue ("username", out var username) &&
					authParams.TryGetValue ("nonce", out var nonce) &&
					authParams.TryGetValue ("uri", out var uri) &&
					authParams.TryGetValue ("response", out var response)) {

					authParams.TryGetValue ("nc", out var nc);
					authParams.TryGetValue ("cnonce", out var cnonce);
					authParams.TryGetValue ("qop", out var qop);

					var ha1 = ComputeMD5 ($"{username}:{realm}:{validPass}");
					var ha2 = ComputeMD5 ($"GET:{uri}");

					string expected;
					if (!string.IsNullOrEmpty (qop))
						expected = ComputeMD5 ($"{ha1}:{nonce}:{nc}:{cnonce}:{qop}:{ha2}");
					else
						expected = ComputeMD5 ($"{ha1}:{nonce}:{ha2}");

					if (username == validUser && response == expected) {
						WriteJsonResponse (context, $"{{\"authenticated\": true, \"user\": \"{EscapeJsonString (username)}\"}}");
						return;
					}
				}
			}

			// Send digest challenge
			var newNonce = Guid.NewGuid ().ToString ("N");
			context.Response.StatusCode = 401;
			context.Response.AddHeader ("WWW-Authenticate",
				$"Digest realm=\"{realm}\", nonce=\"{newNonce}\", qop=\"auth\", opaque=\"{Guid.NewGuid ():N}\", algorithm=MD5, stale=FALSE");
		}

		// GET /gzip: return gzip-compressed JSON response
		static void HandleGzip (HttpListenerContext context)
		{
			var json = "{\"gzipped\": true, \"method\": \"GET\", \"origin\": \"127.0.0.1\"}";
			var jsonBytes = Encoding.UTF8.GetBytes (json);

			using var ms = new MemoryStream ();
			using (var gzip = new GZipStream (ms, CompressionMode.Compress, leaveOpen: true)) {
				gzip.Write (jsonBytes, 0, jsonBytes.Length);
			}

			var compressed = ms.ToArray ();
			context.Response.ContentType = "application/json";
			context.Response.AddHeader ("Content-Encoding", "gzip");
			context.Response.ContentLength64 = compressed.Length;
			context.Response.OutputStream.Write (compressed, 0, compressed.Length);
		}

		// GET /html: return an HTML response with Content-Length
		static void HandleHtml (HttpListenerContext context)
		{
			const string html = "<html><head><title>Test</title></head><body><h1>Herman Melville - Moby Dick</h1></body></html>";
			var bytes = Encoding.UTF8.GetBytes (html);
			context.Response.ContentType = "text/html; charset=utf-8";
			context.Response.ContentLength64 = bytes.Length;
			context.Response.OutputStream.Write (bytes, 0, bytes.Length);
		}

		// GET /status/{code}: return a response with the specified HTTP status code
		static void HandleStatus (HttpListenerContext context)
		{
			var path = context.Request.Url!.AbsolutePath;
			var codeStr = path.Substring ("/status/".Length);
			if (int.TryParse (codeStr, out var code))
				context.Response.StatusCode = code;
			else
				context.Response.StatusCode = 400;
		}

		static void WriteJsonResponse (HttpListenerContext context, string json)
		{
			var bytes = Encoding.UTF8.GetBytes (json);
			context.Response.ContentType = "application/json";
			context.Response.ContentLength64 = bytes.Length;
			context.Response.StatusCode = 200;
			context.Response.OutputStream.Write (bytes, 0, bytes.Length);
		}

		static string EscapeJsonString (string value)
		{
			return value
				.Replace ("\\", "\\\\")
				.Replace ("\"", "\\\"")
				.Replace ("\n", "\\n")
				.Replace ("\r", "\\r")
				.Replace ("\t", "\\t");
		}

		static Dictionary<string, string> ParseDigestAuthHeader (string header)
		{
			var result = new Dictionary<string, string> (StringComparer.OrdinalIgnoreCase);
			var content = header.Substring ("Digest ".Length);

			var i = 0;
			while (i < content.Length) {
				// Skip whitespace and commas
				while (i < content.Length && (content [i] == ' ' || content [i] == ','))
					i++;

				if (i >= content.Length)
					break;

				// Read key
				var keyStart = i;
				while (i < content.Length && content [i] != '=')
					i++;

				if (i >= content.Length)
					break;

				var key = content.Substring (keyStart, i - keyStart).Trim ();
				i++; // skip '='

				// Read value (quoted or unquoted)
				string value;
				if (i < content.Length && content [i] == '"') {
					i++; // skip opening quote
					var valueStart = i;
					while (i < content.Length && content [i] != '"')
						i++;
					value = content.Substring (valueStart, i - valueStart);
					if (i < content.Length)
						i++; // skip closing quote
				} else {
					var valueStart = i;
					while (i < content.Length && content [i] != ',' && content [i] != ' ')
						i++;
					value = content.Substring (valueStart, i - valueStart);
				}

				result [key] = value;
			}

			return result;
		}

		static string ComputeMD5 (string input)
		{
			var bytes = MD5.HashData (Encoding.UTF8.GetBytes (input));
			return Convert.ToHexStringLower (bytes);
		}
	}
}
