#if HAS_ARKIT

using System.Reflection;
using ARKit;
using Xamarin.Utils;

namespace MonoTouchFixtures.ARKit {

	[TestFixture]
	[Preserve (AllMembers = true)]
	public class ARCondigurationTest {

		[SetUp]
		public void Setup ()
		{
			TestRuntime.AssertXcodeVersion (9, 3);
			// The API here was introduced to Mac Catalyst later than for the other frameworks, so we have this additional check
			TestRuntime.AssertSystemVersion (ApplePlatform.MacCatalyst, 14, 0, throwIfOtherPlatform: false);
		}

		[Test]
		public void GetSupportedVideoFormats_9_3 ()
		{
			Assert.That (ARWorldTrackingConfiguration.GetSupportedVideoFormats (), Is.Not.Null, "ARWorldTrackingConfiguration");
			Assert.That (AROrientationTrackingConfiguration.GetSupportedVideoFormats (), Is.Not.Null, "AROrientationTrackingConfiguration");
			Assert.That (ARFaceTrackingConfiguration.GetSupportedVideoFormats (), Is.Not.Null, "ARFaceTrackingConfiguration");
		}

		[Test]
		public void GetSupportedVideoFormats_10_0 ()
		{
			TestRuntime.AssertXcodeVersion (10, 0);
			Assert.That (ARImageTrackingConfiguration.GetSupportedVideoFormats (), Is.Not.Null, "ARImageTrackingConfiguration");
			Assert.That (ARObjectScanningConfiguration.GetSupportedVideoFormats (), Is.Not.Null, "ARObjectScanningConfiguration");
		}
	}
}

#endif // HAS_ARKIT
