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

			ClassicAssert.IsTrue (rect.X == 0, "NSClipViewConstrainBoundsRect - X value was not 0");
			ClassicAssert.IsTrue (rect.Y == 0, "NSClipViewConstrainBoundsRect - Y value was not 0");
			ClassicAssert.IsTrue (rect.Width == 30, "NSClipViewConstrainBoundsRect - Width value was not 30");
			ClassicAssert.IsTrue (rect.Height == 30, "NSClipViewConstrainBoundsRect - Height value was not 30");
		}
	}
}
#endif // __MACOS__
