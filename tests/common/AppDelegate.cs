using System;
using System.Collections.Generic;
using System.Reflection;

using Foundation;
#if !__MACOS__
using UIKit;
#endif

using MonoTouch.NUnit.UI;
using NUnit.Framework;
using NUnit.Framework.Internal;

// Disable until we get around to enable + fix any issues.
#nullable disable

[Register ("AppDelegate")]
public partial class AppDelegate : UIApplicationDelegate {

	static internal UIWindow MainWindow;
	public static TouchRunner Runner { get; set; }

	partial void PostFinishedLaunching ();

	public override bool FinishedLaunching (UIApplication application, NSDictionary launchOptions)
	{
#if __MACCATALYST__ || __MACOS__
		TestRuntime.NotifyLaunchCompleted ();
#endif

#if __MACOS__
		var window = new UIWindow (UIScreen.MainScreen.Bounds);

		var runner = new TouchRunner (window);
		foreach (var assembly in TestLoader.GetTestAssemblies ())
			runner.Add (assembly);

		Window = window;
		Runner = runner;

		window.RootViewController = new UINavigationController (runner.GetViewController ());
		window.MakeKeyAndVisible ();
#endif

		PostFinishedLaunching ();

		return true;
	}

#if !__MACOS__
	public override UISceneConfiguration GetConfiguration (UIApplication application, UISceneSession connectingSceneSession,
		UISceneConnectionOptions options)
	{
		return new UISceneConfiguration ("Default Configuration", connectingSceneSession.Role);
	}
#endif
}

#if !__MACOS__
[Register ("SceneDelegate")]
public partial class SceneDelegate : UIResponder, IUIWindowSceneDelegate {

	[Export ("window")]
	public UIWindow Window { get; set; }

	[Export ("scene:willConnectToSession:options:")]
	public void WillConnect (UIScene scene, UISceneSession session, UISceneConnectionOptions connectionOptions)
	{
		if (scene is UIWindowScene windowScene) {
			Window ??= new UIWindow (windowScene);

			var runner = new TouchRunner (Window);
			foreach (var assembly in TestLoader.GetTestAssemblies ())
				runner.Add (assembly);

			AppDelegate.Runner = runner;
			AppDelegate.MainWindow = Window;

			Window.RootViewController = new UINavigationController (runner.GetViewController ());
			Window.MakeKeyAndVisible ();
		}
	}
}
#endif

public static class MainClass {
	static void Main (string [] args)
	{
#if !__MACOS__
		UIApplication.Main (args, null, typeof (AppDelegate));
#endif
	}
}
