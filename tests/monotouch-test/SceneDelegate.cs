#if !MONOMAC
using System.Reflection;
using System.Threading.Tasks;

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

		LogStateChanges ();
	}

	// Log app and scene state changes, to tell whether the app was deactivated or sent to the background when a test run stops making progress.
	static void LogStateChanges ()
	{
		NSString [] names = [
			UIApplication.DidBecomeActiveNotification,
			UIApplication.WillResignActiveNotification,
			UIApplication.DidEnterBackgroundNotification,
			UIApplication.WillEnterForegroundNotification,
			UIApplication.WillTerminateNotification,
			UIScene.WillConnectNotification,
			UIScene.DidDisconnectNotification,
			UIScene.DidActivateNotification,
			UIScene.WillDeactivateNotification,
			UIScene.WillEnterForegroundNotification,
			UIScene.DidEnterBackgroundNotification,
		];
		foreach (var name in names) {
			NSNotificationCenter.DefaultCenter.AddObserver (name, (notification) => {
				TestRuntime.NSLog ($"State change: {notification.Name} (application state: {UIApplication.SharedApplication.ApplicationState})");
			});
		}
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
