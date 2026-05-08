#if __MACOS__
using System.Threading.Tasks;

using AppKit;
using CoreAnimation;
using CoreGraphics;

namespace Xamarin.Mac.Tests {
	[TestFixture]
	[Preserve (AllMembers = true)]
	public class CAKeyFrameAnimationTests {
		[Test]
		public void CAKeyFrameAnimation_ValuesTests ()
		{
			CAKeyFrameAnimation keyFrameAni = new CAKeyFrameAnimation ();
			keyFrameAni.Values = new NSObject [] { new NSNumber (5) };
			Assert.That (keyFrameAni.Values.Length, Is.EqualTo (1));
			NSNumber arrayNumber = (NSNumber) keyFrameAni.Values [0];
			Assert.That (arrayNumber.Int32Value, Is.EqualTo (5));

			CGRect frame = new CGRect (10, 10, 10, 10);

			using var provider = new CGDataProvider (new byte [(int) frame.Width * (int) frame.Height * 4]);
			using var colorSpace = CGColorSpace.CreateDeviceRGB ();
			using var image = new CGImage ((int) frame.Width, (int) frame.Height, 8, 32, 4 * (int) frame.Width, colorSpace,
				CGBitmapFlags.ByteOrderDefault | CGBitmapFlags.Last, provider, null, false, CGColorRenderingIntent.Default);

			keyFrameAni.SetValues (new CGImage [] { image, image });
			Assert.That (keyFrameAni.Values.Length, Is.EqualTo (2));
			CGImage arrayImage = (CGImage) keyFrameAni.GetValuesAs<CGImage> () [1];
			Assert.That (arrayImage.Handle, Is.EqualTo (image.Handle));
		}
	}
}

#endif // __MACOS__
