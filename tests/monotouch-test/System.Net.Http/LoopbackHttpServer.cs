// Copyright (c) Microsoft Corporation.
// Licensed under the MIT License.

//
// A minimal in-process HTTP/1.1 server for tests, listening on the loopback interface.
//
// This uses a TcpListener instead of an HttpListener, because adding a prefix to an HttpListener does a DNS lookup
// of the machine's host name (System.Net.ServiceNameStore.BuildServiceNames), which takes 5 seconds on the CI bots.
//
// Each connection handles a single request, and the response is sent with 'Connection: close'.
//

using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.IO;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;
using System.Web;

#nullable enable

namespace MonoTests.System.Net.Http {
	[Preserve (AllMembers = true)]
	sealed class LoopbackHttpServer : IDisposable {
		static readonly TimeSpan ClientTimeout = TimeSpan.FromSeconds (30);

		readonly TcpListener listener;
		readonly Action<LoopbackHttpContext> handler;
		readonly HashSet<TcpClient> clients = new HashSet<TcpClient> ();
		bool disposed;

		public int Port { get; }

		public string BaseUrl => $"http://127.0.0.1:{Port}";

		public LoopbackHttpServer (Action<LoopbackHttpContext> handler)
		{
			this.handler = handler;
			listener = new TcpListener (IPAddress.Loopback, 0);
			listener.Start ();
			Port = ((IPEndPoint) listener.LocalEndpoint).Port;
			_ = Task.Run (AcceptLoopAsync);
		}

		async Task AcceptLoopAsync ()
		{
			while (true) {
				TcpClient client;
				try {
					Task<TcpClient> acceptTask;
					lock (clients) {
						if (disposed)
							return;
						acceptTask = listener.AcceptTcpClientAsync ();
					}
					client = await acceptTask.ConfigureAwait (false);
				} catch (ObjectDisposedException) {
					return;
				} catch (SocketException) {
					return;
				} catch (InvalidOperationException) {
					return;
				}
				lock (clients) {
					if (disposed) {
						client.Dispose ();
						return;
					}
					clients.Add (client);
				}
				_ = Task.Run (() => HandleClient (client));
			}
		}

		void HandleClient (TcpClient client)
		{
			try {
				BufferedStream stream;
				lock (clients) {
					if (disposed)
						return;
					client.ReceiveTimeout = (int) ClientTimeout.TotalMilliseconds;
					client.SendTimeout = (int) ClientTimeout.TotalMilliseconds;
					stream = new BufferedStream (client.GetStream ());
				}

				using (stream) {
					var request = LoopbackHttpRequest.Read (stream, BaseUrl);
					if (request is null)
						return;

					var context = new LoopbackHttpContext (request);
					try {
						handler (context);
					} catch (Exception e) {
						context.Response = new LoopbackHttpResponse ();
						context.Response.StatusCode = 500;
						var body = Encoding.UTF8.GetBytes (e.ToString ());
						context.Response.OutputStream.Write (body, 0, body.Length); // CodeQL [SM00430] This is a test server that only listens on localhost.
					}

					context.Response.Send (stream, request.HttpMethod);
				}
			} catch (Exception e) when (e is IOException || e is SocketException || e is ObjectDisposedException) {
				// The client closed the connection, or the server was stopped.
			} catch (Exception e) {
				// Don't let errors bring down the process, but log them so test failures are actionable.
				Console.WriteLine ($"LoopbackHttpServer: error handling client: {e}");
			} finally {
				lock (clients) {
					clients.Remove (client);
					client.Dispose ();
				}
			}
		}

		public void Dispose ()
		{
			lock (clients) {
				if (disposed)
					return;
				disposed = true;
				listener.Stop ();
				foreach (var client in clients)
					client.Dispose ();
				clients.Clear ();
			}
		}
	}

	[Preserve (AllMembers = true)]
	sealed class LoopbackHttpContext {
		public LoopbackHttpRequest Request { get; }
		public LoopbackHttpResponse Response { get; set; } = new LoopbackHttpResponse ();

		public LoopbackHttpContext (LoopbackHttpRequest request)
		{
			Request = request;
		}
	}

	[Preserve (AllMembers = true)]
	sealed class LoopbackHttpRequest {
		public string HttpMethod { get; }
		public string RawUrl { get; }
		public Uri Url { get; }
		public NameValueCollection Headers { get; }
		public NameValueCollection QueryString { get; }
		public Stream InputStream { get; }
		public Encoding ContentEncoding => Encoding.UTF8;

		LoopbackHttpRequest (string httpMethod, string rawUrl, Uri url, NameValueCollection headers, byte [] body)
		{
			HttpMethod = httpMethod;
			RawUrl = rawUrl;
			Url = url;
			Headers = headers;
			QueryString = HttpUtility.ParseQueryString (url.Query);
			InputStream = new MemoryStream (body, false);
		}

		internal static LoopbackHttpRequest? Read (Stream stream, string baseUrl)
		{
			// Request line: "GET /path HTTP/1.1"
			var requestLine = ReadLine (stream);
			if (string.IsNullOrEmpty (requestLine))
				return null;
			var parts = requestLine.Split (' ');
			if (parts.Length < 3)
				return null;

			var headers = new NameValueCollection (StringComparer.OrdinalIgnoreCase);
			string? line;
			while (true) {
				line = ReadLine (stream);
				if (line is null)
					return null;
				if (line.Length == 0)
					break;
				var colon = line.IndexOf (':');
				if (colon > 0)
					headers.Add (line.Substring (0, colon).Trim (), line.Substring (colon + 1).Trim ());
			}

			var rawUrl = parts [1];
			var url = rawUrl.StartsWith ('/') ? new Uri (baseUrl + rawUrl) : new Uri (rawUrl);
			return new LoopbackHttpRequest (parts [0], rawUrl, url, headers, ReadBody (stream, headers));
		}

		static byte [] ReadBody (Stream stream, NameValueCollection headers)
		{
			var body = new MemoryStream ();
			if (string.Equals (headers ["Transfer-Encoding"], "chunked", StringComparison.OrdinalIgnoreCase)) {
				while (true) {
					var sizeLine = ReadLine (stream) ?? "";
					var semicolon = sizeLine.IndexOf (';');
					if (semicolon >= 0)
						sizeLine = sizeLine.Substring (0, semicolon);
					var size = Convert.ToInt32 (sizeLine.Trim (), 16);
					if (size == 0) {
						// Skip any trailers.
						while (!string.IsNullOrEmpty (ReadLine (stream))) {
						}
						break;
					}
					CopyBytes (stream, body, size);
					ReadLine (stream);
				}
			} else if (long.TryParse (headers ["Content-Length"], out var contentLength) && contentLength > 0) {
				CopyBytes (stream, body, contentLength);
			}
			return body.ToArray ();
		}

		static void CopyBytes (Stream source, Stream destination, long count)
		{
			var buffer = new byte [4096];
			while (count > 0) {
				var read = source.Read (buffer, 0, (int) Math.Min (buffer.Length, count));
				if (read == 0)
					throw new IOException ("Unexpected end of request body.");
				destination.Write (buffer, 0, read);
				count -= read;
			}
		}

		static string? ReadLine (Stream stream)
		{
			var sb = new StringBuilder ();
			while (true) {
				var b = stream.ReadByte ();
				if (b == -1)
					return sb.Length == 0 ? null : sb.ToString ();
				if (b == '\n')
					break;
				if (b != '\r')
					sb.Append ((char) b);
			}
			return sb.ToString ();
		}
	}

	[Preserve (AllMembers = true)]
	sealed class LoopbackHttpResponse {
		readonly List<KeyValuePair<string, string>> headers = new List<KeyValuePair<string, string>> ();

		public int StatusCode { get; set; } = 200;
		public string? ContentType { get; set; }
		public string? RedirectLocation { get; set; }

		// The Content-Length header is computed from the contents of this stream.
		public MemoryStream OutputStream { get; } = new MemoryStream ();

		public void AddHeader (string name, string value)
		{
			headers.RemoveAll (v => string.Equals (v.Key, name, StringComparison.OrdinalIgnoreCase));
			headers.Add (new KeyValuePair<string, string> (name, value));
		}

		public void AppendHeader (string name, string value)
		{
			headers.Add (new KeyValuePair<string, string> (name, value));
		}

		internal void Send (Stream stream, string requestMethod)
		{
			var hasBody = !(StatusCode < 200 || StatusCode == 204 || StatusCode == 304);
			var body = OutputStream.ToArray ();

			var sb = new StringBuilder ();
			sb.Append ($"HTTP/1.1 {StatusCode} {GetReasonPhrase (StatusCode)}\r\n");
			if (ContentType is not null)
				sb.Append ($"Content-Type: {ContentType}\r\n");
			if (RedirectLocation is not null)
				sb.Append ($"Location: {RedirectLocation}\r\n");
			foreach (var header in headers)
				sb.Append ($"{header.Key}: {header.Value}\r\n");
			if (hasBody)
				sb.Append ($"Content-Length: {body.Length}\r\n");
			sb.Append ("Connection: close\r\n");
			sb.Append ("\r\n");

			var headerBytes = Encoding.ASCII.GetBytes (sb.ToString ());
			stream.Write (headerBytes, 0, headerBytes.Length);
			if (hasBody && !string.Equals (requestMethod, "HEAD", StringComparison.OrdinalIgnoreCase))
				stream.Write (body, 0, body.Length);
			stream.Flush ();
		}

		static string GetReasonPhrase (int statusCode)
		{
			switch (statusCode) {
			case 200: return "OK";
			case 204: return "No Content";
			case 301: return "Moved Permanently";
			case 302: return "Found";
			case 304: return "Not Modified";
			case 307: return "Temporary Redirect";
			case 308: return "Permanent Redirect";
			case 400: return "Bad Request";
			case 401: return "Unauthorized";
			case 403: return "Forbidden";
			case 404: return "Not Found";
			case 500: return "Internal Server Error";
			default: return "Unknown";
			}
		}
	}
}
