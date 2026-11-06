#if HAS_ARKIT

using System.Threading.Tasks;
using ARKit;
using Vision;
using Xamarin.Utils;

namespace monotouchtest.ARKit {
	[TestFixture]
	[Preserve (AllMembers = true)]
	public class ARSkeletonTest {

		[SetUp]
		public void Setup ()
		{
			TestRuntime.AssertXcodeVersion (12, 0);
			// The API here was introduced to Mac Catalyst later than for the other frameworks, so we have this additional check
			TestRuntime.AssertSystemVersion (ApplePlatform.MacCatalyst, 14, 0, throwIfOtherPlatform: false);
		}

		[Test]
		public void KnownPointTest ()
		{
			TestRuntime.AssertNotSimulator ("Joint name conversion is stubbed out to return null in the simulator.");

			var jointName = ARSkeleton.CreateJointName (VNHumanBodyPoseObservationJointName.LeftShoulder.GetConstant ());
			Assert.That (jointName, Is.Not.Null, "JointName");
			Assert.That (jointName, Is.EqualTo (ARSkeletonJointName.LeftShoulder.GetConstant ()), "LeftShoulder");
		}

		[Test]
		public void UnknownPointTest ()
		{
			using (var notKnownPoint = new NSString ("nariz"))
				Assert.That (ARSkeleton.CreateJointName (notKnownPoint), Is.Null);
		}

	}
}
#endif // HAS_ARKIT
