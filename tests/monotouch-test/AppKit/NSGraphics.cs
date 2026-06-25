#if __MACOS__
using System.Threading.Tasks;

using AppKit;

namespace Xamarin.Mac.Tests {
	[TestFixture]
	[Preserve (AllMembers = true)]
	public class NSGraphicsTest {
#if !XAMCORE_5_0
		[Test]
		public void BestDepth ()
		{
			bool exactMatch = false;
			var rv = NSGraphics.BestDepth (NSColorSpace.DeviceRGB, 8, 8, false, ref exactMatch);
			Assert.That (rv, Is.EqualTo (NSWindowDepth.TwentyfourBitRgb), "BestDepth");
			Assert.That (exactMatch, Is.True, "ExactMatch");
		}
#endif

		[Test]
		public void GetBestDepth ()
		{
			var rv = NSGraphics.GetBestDepth (NSColorSpace.DeviceRGB, 8, 8, false, out var exactMatch);
			Assert.That (rv, Is.EqualTo (NSWindowDepth.TwentyfourBitRgb), "GetBestDepth");
			Assert.That (exactMatch, Is.True, "ExactMatch");
		}
	}
}

#endif // __MACOS__
