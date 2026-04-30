#if __MACOS__

using AppKit;

namespace Xamarin.Mac.Tests {
	[Preserve (AllMembers = true)]
	public class NSTabViewItemTests {
		NSTabViewItem item;

		[SetUp]
		public void SetUp ()
		{
			item = new NSTabViewItem ();
		}

		[Test]
		public void NSTabViewItemShouldChangeImage ()
		{
			var image = item.Image;
			item.Image = new NSImage ();

			Assert.IsFalse (item.Image == image, "NSTabViewItemShouldChangeImage - Failed to set the Image property");
		}

		[Test]
		public void NSTabViewItemShouldChangeViewController ()
		{
			var vc = item.ViewController;
			item.ViewController = new NSViewController ();

			Assert.IsFalse (item.ViewController == vc, "NSTabViewItemShouldChangeViewController - Failed to set the ViewController property");
		}
	}
}

#endif // __MACOS__
