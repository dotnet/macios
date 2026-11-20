using System;
using System.Runtime.InteropServices;
using System.Net.Http;

using Foundation;

namespace MySimpleApp {
	public class Program {
		static int Main (string [] args)
		{
			GC.KeepAlive (typeof (NSObject)); // prevent linking away the platform assembly

			Console.WriteLine (Environment.GetEnvironmentVariable ("MAGIC_WORD"));
#if HttpClientHandlerFeatureTrimmedAway
			var client = new System.Net.Http.HttpClient ();
			client.GetStringAsync ("https://microsoft.com");
#elif KEEP_LARIGHT
			if (OperatingSystem.IsIOSVersionAtLeast (17))
				GC.KeepAlive (typeof (LocalAuthentication.LARight));
#elif KEEP_LAPERSISTEDRIGHT
			if (OperatingSystem.IsIOSVersionAtLeast (17))
				GC.KeepAlive (typeof (LocalAuthentication.LAPersistedRight));
#elif KEEP_LACONTEXT
			using (var context = new LocalAuthentication.LAContext ())
				Console.WriteLine (context.BiometryType);
#elif KEEP_APPLICATION_DELEGATE
			using (var appDelegate = new OptionalProtocolDelegate ()) {
#if IMPLEMENT_OPTIONAL_CALLBACKS
				SendCallback (appDelegate.Handle, ObjCRuntime.Selector.GetHandle ("application:handlerForIntent:"), IntPtr.Zero, IntPtr.Zero);
				SendShareCallback (appDelegate.Handle, ObjCRuntime.Selector.GetHandle ("application:userDidAcceptCloudKitShareWithMetadata:"), IntPtr.Zero, IntPtr.Zero);
				if (OptionalProtocolDelegate.CallbackCount != 2)
					throw new InvalidOperationException ($"Expected two optional protocol callbacks, got {OptionalProtocolDelegate.CallbackCount}.");
				Console.WriteLine ("Optional protocol callbacks invoked.");
#endif
			}
#endif

			return args.Length;
		}

#if KEEP_APPLICATION_DELEGATE && IMPLEMENT_OPTIONAL_CALLBACKS
		[DllImport (ObjCRuntime.Constants.ObjectiveCLibrary, EntryPoint = "objc_msgSend")]
		static extern IntPtr SendCallback (IntPtr receiver, IntPtr selector, IntPtr application, IntPtr argument);

		[DllImport (ObjCRuntime.Constants.ObjectiveCLibrary, EntryPoint = "objc_msgSend")]
		static extern void SendShareCallback (IntPtr receiver, IntPtr selector, IntPtr application, IntPtr argument);
#endif
	}

#if KEEP_APPLICATION_DELEGATE
	[Register ("OptionalProtocolDelegate")]
#if IMPLEMENT_OPTIONAL_CALLBACKS
	public class OptionalProtocolDelegate : NSObject, AppKit.INSApplicationDelegate {
		public static int CallbackCount;

		public NSObject? GetHandler (AppKit.NSApplication application, Intents.INIntent intent)
		{
			CallbackCount++;
			return null;
		}

		public void UserDidAcceptCloudKitShare (AppKit.NSApplication application, CloudKit.CKShareMetadata metadata)
		{
			CallbackCount++;
		}
	}
#else
	public class OptionalProtocolDelegate : AppKit.NSApplicationDelegate {
	}
#endif
#endif
}
