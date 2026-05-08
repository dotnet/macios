#if __MACOS__
using AVFoundation;
using Xamarin.Utils;

namespace MonoTouchFixtures.AVFoundation {

	[TestFixture]
	[Preserve (AllMembers = true)]
	public class AVCaptionSizeTest {

		[SetUp]
		public void SetUp ()
		{
			TestRuntime.AssertSystemVersion (ApplePlatform.MacOSX, 12, 0);
		}

		[Test]
		public void CreateTest ()
		{
			nfloat val = 10;
			var units = AVCaptionUnitsType.Cells;
			var firstDimension = AVCaptionDimension.Create (val, units);
			var secondDimension = AVCaptionDimension.Create (val, units);

			var size = AVCaptionSize.Create (firstDimension, secondDimension);
			Assert.That (size.Width.Value, Is.EqualTo (val), "Width");
			Assert.That (size.Height.Value, Is.EqualTo (val), "Height");
		}
	}
}
#endif
