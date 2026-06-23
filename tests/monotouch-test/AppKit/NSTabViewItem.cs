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

			Assert.That (item.Image == image, Is.False, "NSTabViewItemShouldChangeImage - Failed to set the Image property");
		}

		[Test]
		public void NSTabViewItemShouldChangeViewController ()
		{
			var vc = item.ViewController;
			item.ViewController = new NSViewController ();

			Assert.That (item.ViewController == vc, Is.False, "NSTabViewItemShouldChangeViewController - Failed to set the ViewController property");
		}
	}
}

#endif // __MACOS__
