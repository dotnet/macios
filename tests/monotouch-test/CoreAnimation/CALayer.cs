#if __MACOS__
using System.Threading.Tasks;

using AppKit;
using CoreAnimation;
using CoreGraphics;

namespace Xamarin.Mac.Tests {
	[TestFixture]
	[Preserve (AllMembers = true)]
	public class CALayerTests {
		[Test]
		public void CALayer_ValuesTests ()
		{
			CALayer layer = new CALayer ();
			CGRect frame = new CGRect (10, 10, 10, 10);
			using var provider = new CGDataProvider (new byte [(int) frame.Width * (int) frame.Height * 4]);
			using var colorSpace = CGColorSpace.CreateDeviceRGB ();
			using var image = new CGImage ((int) frame.Width, (int) frame.Height, 8, 32, 4 * (int) frame.Width, colorSpace,
				CGBitmapFlags.ByteOrderDefault | CGBitmapFlags.Last, provider, null, false, CGColorRenderingIntent.Default);

			NSImage NSImage = new NSImage ();

			layer.Contents = image;
			CGImage arrayImage = layer.Contents;
			Assert.That (arrayImage.Handle, Is.EqualTo (image.Handle));

			layer.SetContents (NSImage);
			NSImage arrayNSImage = layer.GetContentsAs<NSImage> ();
			Assert.That (arrayNSImage.Handle, Is.EqualTo (NSImage.Handle));

			layer.SetContents (null); // Should not throw
			layer.Contents = null; // Should not throw
		}
	}
}

#endif // __MACOS__
