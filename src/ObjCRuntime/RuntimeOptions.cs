using System;
using System.IO;
using System.Text;

#if MTOUCH || MMP || BUNDLER
using Mono.Cecil;
using Xamarin.Linker;
#else
using System.Net.Http;
using Foundation;
using ObjCRuntime;
#endif

#nullable enable

#if MMP || MMP_TEST || MTOUCH || BUNDLER
namespace Xamarin.Bundler {
#else
namespace ObjCRuntime {
#endif
	class RuntimeOptions {
		const string SocketsHandlerValue = "SocketsHttpHandler";
		const string CFNetworkHandlerValue = "CFNetworkHandler";
		const string NSUrlSessionHandlerValue = "NSUrlSessionHandler";

		string? http_message_handler;

#if MTOUCH || MMP || BUNDLER
		/*
		 * This section is only used by the tools
		 */
		internal static RuntimeOptions Create (Application app, string? http_message_handler, string? tls_provider)
		{
			var options = new RuntimeOptions ();
			options.http_message_handler = ParseHttpMessageHandler (app, http_message_handler);
			return options;
		}

		static string ParseHttpMessageHandler (Application app, string? value)
		{
			switch (value) {
			// default
			case null:
				return NSUrlSessionHandlerValue;
			case CFNetworkHandlerValue:
			case SocketsHandlerValue:
				return value;
			case NSUrlSessionHandlerValue:
				return value;
			default:
				throw ErrorHelper.CreateError (2010, Errors.MT2010, value);
			}
		}

		internal void Write (string app_dir)
		{
			// note: we always create the file because the simulator won't remove old files
			// that might become useful if we add new options in the future
			var content = new StringBuilder ();
			content.AppendLine ("<?xml version=\"1.0\" encoding=\"UTF-8\"?>");
			content.AppendLine ("<!DOCTYPE plist PUBLIC \\\"-//Apple//DTD PLIST 1.0//EN\\\" \\\"http://www.apple.com/DTDs/PropertyList-1.0.dtd\\\">");
			content.AppendLine ("<plist version=\\\"1.0\\\">");
			content.AppendLine ("<dict>");
			content.AppendLine ("<key>HttpMessageHandler</key>");
			content.Append ("<string>");
			content.Append (http_message_handler);
			content.AppendLine ("</string>");
			content.AppendLine ("</dict>");
			content.AppendLine ("</plist>");

			var file_name = GetFileName (app_dir);
			Xamarin.Bundler.Driver.WriteIfDifferent (file_name, content.ToString ());
		}

		// Called from CoreHttpMessageHandler
		internal static TypeDefinition GetHttpMessageHandler (Application app, RuntimeOptions options, ModuleDefinition httpModule, ModuleDefinition? platformModule = null)
		{
			string? handler;

			if (options is not null) {
				handler = options.http_message_handler;
			} else {
				if (Runtime.UseNSUrlSessionHandler)
					handler = NSUrlSessionHandlerValue;

				if (UseCFNetworkHandler)
					handler = CFNetworkHandlerValue;
				handler = HttpClientHandlerValue;
			}
			TypeDefinition type;
			switch (handler) {
			case CFNetworkHandlerValue:
				type = platformModule!.GetType ("System.Net.Http", "CFNetworkHandler");
				break;
#if MONOMAC
			case HttpClientHandlerValue:
				type = httpModule.GetType ("System.Net.Http", "HttpClientHandler");
				break;
			case NSUrlSessionHandlerValue:
				type = platformModule!.GetType ("Foundation", "NSUrlSessionHandler");
				break;
#else
			case SocketsHandlerValue:
				type = httpModule.GetType ("System.Net.Http", "SocketsHttpHandler");
				break;
			case NSUrlSessionHandlerValue:
				type = platformModule!.GetType ("System.Net.Http", "NSUrlSessionHandler");
				break;
#endif
			default:
				throw new InvalidOperationException (string.Format ("Unknown HttpMessageHandler `{0}`.", handler));
			}
			if (type is null)
				throw new InvalidOperationException (string.Format ("Cannot load HttpMessageHandler `{0}`.", handler));
			return type;
		}
#else

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

			if (UseCFNetworkHandler)
				handler = CFNetworkHandler ();

			handler = HttpClientHandler ();
		}
#endif

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
