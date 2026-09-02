#if __MACOS__

using AppKit;
using CoreGraphics;

namespace Xamarin.Mac.Tests {
	[TestFixture]
	[Preserve (AllMembers = true)]
	public class NSImageTests {
		[Test]
		public void ImageWithSize ()
		{
			var image = NSImage.ImageWithSize (new CGSize (50, 50), false, rect => {
				return true;
			});
			Assert.That (image, Is.Not.Null);
		}

		[Test]
		public void NSImageCapInsets ()
		{
			var image = new NSImage ();
			image.CapInsets = new NSEdgeInsets (5f, 6f, 7f, 8f);

			Assert.That (image.CapInsets, Is.Not.Null);
			Assert.That (image.CapInsets.Top == 5f, Is.True, "NSImageCapInsets - Top value was not 5");
			Assert.That (image.CapInsets.Left == 6f, Is.True, "NSImageCapInsets - Left value was not 6");
			Assert.That (image.CapInsets.Bottom == 7f, Is.True, "NSImageCapInsets - Bottom value was not 7");
			Assert.That (image.CapInsets.Right == 8f, Is.True, "NSImageCapInsets - Right value was not 8");
		}

		[Test]
		public void NSImageResizingModeShouldChange ()
		{
			var image = new NSImage ();
			image.ResizingMode = NSImageResizingMode.Stretch;
			Assert.That (NSImageResizingMode.Stretch, Is.EqualTo (image.ResizingMode), "NSImageResizingMode - Was not equal to Stretch");
			Assert.That (NSImageResizingMode.Tile, Is.Not.EqualTo (image.ResizingMode), "NSImageResizingMode - Was incorrectly equal to Tile");
		}
	}
}
#endif // __MACOS__
