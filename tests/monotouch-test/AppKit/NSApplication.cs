#if __MACOS__

using AppKit;

namespace Xamarin.Mac.Tests {
	[TestFixture]
	[Preserve (AllMembers = true)]
	public class NSApplicationTests {
		[Test]
		public void NSApplication_SendActionNullTest ()
		{
			NSApplication.SharedApplication.SendAction (new Selector ("undo:"), null, new NSObject ());
		}

		[Test]
		public void NSApplication_ApplicationIconImageNullTest ()
		{
			Assert.DoesNotThrow (() => NSApplication.SharedApplication.ApplicationIconImage = null);
		}

		[Test]
		public void NSApplication_DockTileContentViewNullTest ()
		{
			Assert.DoesNotThrow (() => NSApplication.SharedApplication.DockTile.ContentView = null);
		}
	}
}
#endif // __MACOS__
