namespace iOSApp1;

[Register ("AppDelegate")]
public class AppDelegate : UIApplicationDelegate {
	public override bool FinishedLaunching (UIApplication application, NSDictionary? launchOptions)
	{
		// Override point for customization after application launch.
		return true;
	}

	public override UISceneConfiguration GetConfiguration (UIApplication application, UISceneSession connectingSceneSession, UISceneConnectionOptions options)
	{
		// Called when a new scene session is being created.
		// Use this method to select a configuration to create the new scene with.
		// "Default Configuration" is defined in the Info.plist's 'UISceneConfigurationName' key.
		return new UISceneConfiguration ("Default Configuration", connectingSceneSession.Role);
	}

	public override void DidDiscardSceneSessions (UIApplication application, NSSet<UISceneSession> sceneSessions)
	{
		// Called when the user discards a scene session.
		// If any sessions were discarded while the application was not running, this will be called shortly after 'FinishedLaunching'.
		// Use this method to release any resources that were specific to the discarded scenes, as they will not return.
	}
}
