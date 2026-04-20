using System.Diagnostics;
using Microsoft.Testing.Extensions;
using Microsoft.Testing.Platform.Builder;
using iOSTest1;

// UIApplication.Main() provides a proper UIKit run loop,
// preventing iOS watchdog kills during long test runs.
UIApplication.Main (args, null, typeof (AppDelegate));

[Register ("AppDelegate")]
class AppDelegate : UIApplicationDelegate {
	public override UISceneConfiguration GetConfiguration (UIApplication application,
		UISceneSession connectingSceneSession, UISceneConnectionOptions options)
	{
		return new UISceneConfiguration ("Default Configuration", connectingSceneSession.Role);
	}
}

[Register ("SceneDelegate")]
class SceneDelegate : UIResponder, IUIWindowSceneDelegate {
	[Export ("window")]
	public UIWindow? Window { get; set; }

	[Export ("scene:willConnectToSession:options:")]
	public void WillConnect (UIScene scene, UISceneSession session, UISceneConnectionOptions connectionOptions)
	{
		if (scene is not UIWindowScene windowScene)
			return;

		Window = new UIWindow (windowScene);
		var vc = new UIViewController ();
		var view = vc.View;
		Debug.Assert (view is not null, "UIViewController.View should not be null");
		view.BackgroundColor = UIColor.SystemBackground;

		var label = new UILabel {
			Text = "Running tests...\n",
			TextAlignment = UITextAlignment.Left,
			Lines = 0,
			Font = UIFont.GetMonospacedSystemFont (12, UIFontWeight.Regular),
			TextColor = UIColor.Label,
			TranslatesAutoresizingMaskIntoConstraints = false,
		};
		view.AddSubview (label);
		var guide = view.SafeAreaLayoutGuide;
		label.TopAnchor.ConstraintEqualTo (guide.TopAnchor, 8).Active = true;
		label.LeadingAnchor.ConstraintEqualTo (guide.LeadingAnchor, 8).Active = true;
		label.TrailingAnchor.ConstraintLessThanOrEqualTo (guide.TrailingAnchor, -8).Active = true;

		Window.RootViewController = vc;
		Window.MakeKeyAndVisible ();

		var consumer = new ResultConsumer ();
		consumer.StatusChanged += line =>
			vc.InvokeOnMainThread (() => label.Text += line + "\n");

		Task.Run (async () => {
			try {
				var documentsPath = Environment.GetFolderPath (Environment.SpecialFolder.MyDocuments);
				var resultsPath = Path.Combine (documentsPath, "TestResults");

				var builder = await TestApplication.CreateBuilderAsync ([
					"--results-directory", resultsPath,
					"--report-trx"
				]);
				builder.AddMSTest (() => [typeof (Test1).Assembly]);
				builder.AddTrxReportProvider ();
				builder.TestHost.AddDataConsumer (_ => consumer);

				using ITestApplication app = await builder.BuildAsync ();
				await app.RunAsync ();
				// UIApplication.Main() keeps the process alive, so exit explicitly
				Environment.Exit (consumer.Failed > 0 ? 1 : 0);
			} catch (Exception ex) {
				Console.WriteLine ($"Error running tests: {ex}");
				Environment.Exit (1);
			}
		});
	}
}
