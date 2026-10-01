namespace tvOSApp1;

[Register ("AppDelegate")]
public class AppDelegate : UIApplicationDelegate {
	public override bool FinishedLaunching (UIApplication application, NSDictionary? launchOptions)
	{
		// Override point for customization after application launch.
		// If not required for your application you can safely delete this method

		return true;
	}

	public override UISceneConfiguration GetConfiguration (UIApplication application, UISceneSession connectingSceneSession, UISceneConnectionOptions options)
	{
		// Called when a new scene session is being created.
		// Use this method to select a configuration to create the new scene with.
		// "Default Configuration" is defined in the Info.plist's 'UISceneConfigurationName' key.
		return new UISceneConfiguration ("Default Configuration", connectingSceneSession.Role);
	}
}
