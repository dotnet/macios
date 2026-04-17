using System;
using System.Runtime.InteropServices;

using Foundation;
#if HAS_UIKIT
using UIKit;
#endif
#if HAS_APPKIT
using AppKit;
#endif

namespace MySimpleApp {
	public class Program {
		static int Main (string [] args)
		{
#if HAS_UIKIT
			UIApplication.Main (args, null, typeof (AppDelegate));
#elif HAS_APPKIT
			NSApplication.Init ();
			NSApplication.Main (args);
#else
#error This test app has not been implemented for this platform.
#endif
			return 0;
		}
	}

#if HAS_UIKIT
	public partial class AppDelegate : UIApplicationDelegate {
		UIWindow window;

		public override bool FinishedLaunching (UIApplication app, NSDictionary options)
		{
			window = new UIWindow (UIScreen.MainScreen.Bounds);

			var dvc = new UIViewController ();
			var button = new UIButton (window.Bounds);
			button.SetTitle ("Hello .NET!", UIControlState.Normal);
			dvc.Add (button);

			window.RootViewController = dvc;
			window.MakeKeyAndVisible ();

			return true;
		}
	}
#elif HAS_APPKIT
	[Register ("AppDelegate")]
	public class AppDelegate : NSApplicationDelegate {
	}
#else
#error This test app has not been implemented for this platform.
#endif
}
