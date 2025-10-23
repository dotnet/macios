using System;
using System.Runtime.InteropServices;

using Foundation;
#if HAS_UIKIT
using UIKit;
#endif

namespace MySimpleApp {
	public class Program {
		static int Main (string [] args)
		{
#if HAS_UIKIT

			UIApplication.Main (args, null, typeof (AppDelegate));
#else
			NSApplication.Init ();
			NSApplication.Main (args);
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
#else
#error This test app has not been implemented for AppKit yet.
#endif
}
