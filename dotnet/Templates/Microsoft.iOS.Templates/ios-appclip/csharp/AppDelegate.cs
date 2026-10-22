namespace iOSAppClip1;

[Register ("AppDelegate")]
public class AppDelegate : UIApplicationDelegate {
	public override UIWindow? Window { get; set; }

	public override bool FinishedLaunching (UIApplication application, NSDictionary? launchOptions)
	{
		Window = new UIWindow (UIScreen.MainScreen.Bounds);
		Window.RootViewController = new UIViewController ();
		Window.RootViewController.View!.BackgroundColor = UIColor.SystemBackground;
		Window.MakeKeyAndVisible ();
		return true;
	}

	public override bool ContinueUserActivity (UIApplication application, NSUserActivity userActivity, UIApplicationRestorationHandler completionHandler)
	{
		// An App Clip is launched with a URL, which is delivered here.
		return userActivity.ActivityType == NSUserActivityType.BrowsingWeb;
	}
}
