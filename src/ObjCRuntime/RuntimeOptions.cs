using System;
using System.IO;
using System.Text;

using System.Net.Http;
using Foundation;
using ObjCRuntime;

#nullable enable

namespace ObjCRuntime
{
	class RuntimeOptions {
		const string SocketsHandlerValue = "SocketsHttpHandler";
		const string CFNetworkHandlerValue = "CFNetworkHandler";
		const string NSUrlSessionHandlerValue = "NSUrlSessionHandler";

		string? http_message_handler;

		internal static RuntimeOptions? Read ()
		{
			// for iOS NSBundle.ResourcePath returns the path to the root of the app bundle
			// for macOS apps NSBundle.ResourcePath returns foo.app/Contents/Resources
			// for macOS frameworks NSBundle.ResourcePath returns foo.app/Versions/Current/Resources
			Class bundle_finder = new Class (typeof (NSObject.NSObject_Disposer));
			var resource_dir = NSBundle.FromClass (bundle_finder).ResourcePath;
			var plist_path = GetFileName (resource_dir);

			if (!File.Exists (plist_path))
				return null;

			using (var plist = NSMutableDictionary.FromFile (plist_path)) {
				var options = new RuntimeOptions ();
				options.http_message_handler = (NSString) plist ["HttpMessageHandler"];
				return options;
			}
		}

		// This is invoked by
		// System.Net.Http.dll!System.Net.Http.HttpClient.cctor
		internal static HttpMessageHandler GetHttpMessageHandler ()
		{
			if (Runtime.UseNSUrlSessionHandler)
				return new NSUrlSessionHandler ();

			if (Runtime.UseCFNetworkHandler)
				return new CFNetworkHandler ();

			return new HttpClientHandler ();
		}

		// Use either Create() or Read().
		RuntimeOptions ()
		{
		}

		static string GetFileName (string resource_dir)
		{
			return Path.Combine (resource_dir, "runtime-options.plist");
		}
	}
}
