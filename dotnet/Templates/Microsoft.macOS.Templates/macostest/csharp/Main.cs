using Microsoft.Testing.Extensions;
using Microsoft.Testing.Platform.Builder;
using macOSTest1;

// NSApplication.Main() provides a proper AppKit run loop.
NSApplication.Init ();

var app = NSApplication.SharedApplication;
app.Delegate = new AppDelegate ();
app.Run ();

[Register ("AppDelegate")]
class AppDelegate : NSApplicationDelegate {
	NSWindow? _window;

	public override void DidFinishLaunching (NSNotification notification)
	{
		_window = new NSWindow (
			new CoreGraphics.CGRect (0, 0, 600, 400),
			NSWindowStyle.Titled | NSWindowStyle.Closable | NSWindowStyle.Resizable,
			NSBackingStore.Buffered,
			false
		) {
			Title = "Running tests...",
		};

		var label = new NSTextField {
			Editable = false,
			Bordered = false,
			BackgroundColor = NSColor.WindowBackground,
			StringValue = "Running tests...\n",
			TranslatesAutoresizingMaskIntoConstraints = false,
		};
		_window.ContentView!.AddSubview (label);
		label.TopAnchor.ConstraintEqualTo (_window.ContentView.TopAnchor, 8).Active = true;
		label.LeadingAnchor.ConstraintEqualTo (_window.ContentView.LeadingAnchor, 8).Active = true;
		label.TrailingAnchor.ConstraintEqualTo (_window.ContentView.TrailingAnchor, -8).Active = true;

		_window.Center ();
		_window.MakeKeyAndOrderFront (this);

		var consumer = new ResultConsumer ();
		consumer.StatusChanged += line =>
			NSRunLoop.Main.InvokeOnMainThread (() => label.StringValue += line + "\n");

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

				using ITestApplication testApp = await builder.BuildAsync ();
				await testApp.RunAsync ();
				// NSApplication.Run() keeps the process alive, so exit explicitly
				Environment.Exit (consumer.Failed > 0 ? 1 : 0);
			} catch (Exception ex) {
				Console.WriteLine ($"Error running tests: {ex}");
				Environment.Exit (1);
			}
		});
	}
}
