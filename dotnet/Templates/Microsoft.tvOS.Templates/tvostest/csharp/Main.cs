using Microsoft.Testing.Extensions;
using Microsoft.Testing.Platform.Builder;
using tvOSTest1;

// UIApplication.Main() provides a proper UIKit run loop,
// preventing tvOS watchdog kills during long test runs.
UIApplication.Main (args, null, typeof (AppDelegate));

[Register ("AppDelegate")]
class AppDelegate : UIApplicationDelegate {
	public override UIWindow? Window { get; set; }

	public override bool FinishedLaunching (UIApplication application, NSDictionary? launchOptions)
	{
		Window = new UIWindow (UIScreen.MainScreen.Bounds);
		var vc = new UIViewController ();
		var view = vc.View!;
		view.BackgroundColor = UIColor.Black;

		var label = new UILabel {
			Text = "Running tests...\n",
			TextAlignment = UITextAlignment.Left,
			Lines = 0,
			Font = UIFont.GetMonospacedSystemFont (24, UIFontWeight.Regular)!,
			TextColor = UIColor.White,
			TranslatesAutoresizingMaskIntoConstraints = false,
		};
		view.AddSubview (label);
		label.TopAnchor.ConstraintEqualTo (view.SafeAreaLayoutGuide.TopAnchor, 40).Active = true;
		label.LeadingAnchor.ConstraintEqualTo (view.SafeAreaLayoutGuide.LeadingAnchor, 40).Active = true;
		label.TrailingAnchor.ConstraintLessThanOrEqualTo (view.SafeAreaLayoutGuide.TrailingAnchor, -40).Active = true;

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

		return true;
	}
}
