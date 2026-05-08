#if __MACOS__
using AVFoundation;
using Xamarin.Utils;

namespace MonoTouchFixtures.AVFoundation {

	[TestFixture]
	[Preserve (AllMembers = true)]
	public class AVCaptionPointTest {

		[SetUp]
		public void SetUp ()
		{
			TestRuntime.AssertSystemVersion (ApplePlatform.MacOSX, 12, 0);
		}

		[TestCase]
		public void CreateTest ()
		{
			nfloat val = 10;
			var units = AVCaptionUnitsType.Cells;
			var firstDimension = AVCaptionDimension.Create (val, units);
			var secondDimension = AVCaptionDimension.Create (val, units);

			var point = AVCaptionPoint.Create (firstDimension, secondDimension);
			Assert.That (point.X.Value, Is.EqualTo (val), "X");
			Assert.That (point.Y.Value, Is.EqualTo (val), "Y");
		}
	}
}
#endif
