using AppKit;
using CoreGraphics;
using Foundation;

namespace SecInterfaceExample;

[Register ("AppDelegate")]
public class AppDelegate : NSApplicationDelegate {
	NSWindow? window;

	public override void DidFinishLaunching (NSNotification notification)
	{
		window = new NSWindow (
			new CGRect (100, 100, 960, 720),
			NSWindowStyle.Titled | NSWindowStyle.Closable | NSWindowStyle.Resizable | NSWindowStyle.Miniaturizable,
			NSBackingStore.Buffered, false) {
			Title = "SecurityInterface Framework Demo",
			ReleasedWhenClosed = false,
		};
		window.ContentViewController = new DemoViewController ();
		window.Center ();
		window.MakeKeyAndOrderFront (this);
	}

	public override void WillTerminate (NSNotification notification) { }
}
