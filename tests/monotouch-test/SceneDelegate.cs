#if !MONOMAC
using System;
using System.Collections.Generic;
using System.Reflection;
using System.Threading.Tasks;

using CoreFoundation;
using Foundation;
using UIKit;
using MonoTouch.NUnit.UI;
using NUnit.Framework.Internal;
using MonoTouchFixtures.BackgroundTasks;

public partial class SceneDelegate : UIResponder, IUIWindowSceneDelegate {
	// class-level declarations
	TouchRunner runner => AppDelegate.Runner;

#if __IOS__ && !__MACCATALYST__
	public override bool AccessibilityPerformMagicTap ()
	{
		try {
			runner.OpenWriter ("Magic Tap");
			runner.Run (runner.LoadedTest as TestSuite);
		} finally {
			runner.CloseWriter ();
		}
		return true;
	}
#endif
}

public partial class AppDelegate : UIApplicationDelegate {
	partial void PostFinishedLaunching ()
	{
		// required for the background tasks tests, we can only register the tasks in this method
		BGTaskSchedulerTest.RegisterTestTasks ();
	}

	public static void PresentModalViewController (UIViewController vc, double duration)
	{
		var bckp = MainWindow.RootViewController;
		MainWindow.RootViewController = vc;
		try {
			NSRunLoop.Main.RunUntil (NSDate.Now.AddSeconds (duration));
		} finally {
			MainWindow.RootViewController = bckp;
		}
	}
}


#endif // !MONOMAC
