#if __MACOS__

using AppKit;
using CoreGraphics;

namespace Xamarin.Mac.Tests {
	[TestFixture]
	[Preserve (AllMembers = true)]
	public class NSClipViewTests {
		[Test]
		public void NSClipViewConstrainBoundsRect ()
		{
			var clipView = new NSClipView (new CGRect (0, 0, 50, 50));
			var rect = clipView.ConstrainBoundsRect (new CGRect (10, 10, 30, 30));

			Assert.That (rect.X == 0, Is.True, "NSClipViewConstrainBoundsRect - X value was not 0");
			Assert.That (rect.Y == 0, Is.True, "NSClipViewConstrainBoundsRect - Y value was not 0");
			Assert.That (rect.Width == 30, Is.True, "NSClipViewConstrainBoundsRect - Width value was not 30");
			Assert.That (rect.Height == 30, Is.True, "NSClipViewConstrainBoundsRect - Height value was not 30");
		}
	}
}
#endif // __MACOS__
