#if __MACOS__
using System.Threading;

using AppKit;

namespace Xamarin.Mac.Tests {
	[TestFixture]
	[Preserve (AllMembers = true)]
	public class NSSCreenTests {
		[Test]
		public void ScreensNotMainThread ()
		{
			var called = new AutoResetEvent (false);
			var screensCount = 0;
			var backgroundThread = new Thread (() => {
				screensCount = NSScreen.Screens.Length;
				called.Set ();
			});
			backgroundThread.Start ();
			Assert.That (called.WaitOne (1000), Is.True, "called");
			Assert.That (screensCount > 0, Is.True, "screens count");
		}

		[Test]
		public void MainScreenNotMainThread ()
		{
			var called = new AutoResetEvent (false);
			NSScreen main = null;
			var backgroundThread = new Thread (() => {
				main = NSScreen.MainScreen;
				called.Set ();
			});
			backgroundThread.Start ();
			Assert.That (called.WaitOne (1000), Is.True, "called");
			Assert.That (main, Is.Not.Null, "main screen");
		}

		[Test]
		public void DeepScreenNotMainThread ()
		{
			var called = new AutoResetEvent (false);
			NSScreen deepScreen = null;
			var screenCount = 0;

			var backgroundThread = new Thread (() => {
				screenCount = NSScreen.Screens.Length;
				deepScreen = NSScreen.DeepestScreen;
				called.Set ();
			});
			backgroundThread.Start ();
			Assert.That (called.WaitOne (1000), Is.True, "called");
			if (screenCount > 1) {
				Assert.That (deepScreen, Is.Not.Null, "deep screen");
			} else {
				Assert.Inconclusive ("Only one screen detected.");
			}
		}
	}
}
#endif // __MACOS__
