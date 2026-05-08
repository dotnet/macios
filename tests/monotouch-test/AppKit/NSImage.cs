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
			ClassicAssert.IsNotNull (image);
		}

		[Test]
		public void NSImageCapInsets ()
		{
			var image = new NSImage ();
			image.CapInsets = new NSEdgeInsets (5f, 6f, 7f, 8f);

			ClassicAssert.IsNotNull (image.CapInsets);
			ClassicAssert.IsTrue (image.CapInsets.Top == 5f, "NSImageCapInsets - Top value was not 5");
			ClassicAssert.IsTrue (image.CapInsets.Left == 6f, "NSImageCapInsets - Left value was not 6");
			ClassicAssert.IsTrue (image.CapInsets.Bottom == 7f, "NSImageCapInsets - Bottom value was not 7");
			ClassicAssert.IsTrue (image.CapInsets.Right == 8f, "NSImageCapInsets - Right value was not 8");
		}

		[Test]
		public void NSImageResizingModeShouldChange ()
		{
			var image = new NSImage ();
			image.ResizingMode = NSImageResizingMode.Stretch;
			ClassicAssert.AreEqual (image.ResizingMode, NSImageResizingMode.Stretch, "NSImageResizingMode - Was not equal to Stretch");
			ClassicAssert.AreNotEqual (image.ResizingMode, NSImageResizingMode.Tile, "NSImageResizingMode - Was incorrectly equal to Tile");
		}
	}
}
#endif // __MACOS__
