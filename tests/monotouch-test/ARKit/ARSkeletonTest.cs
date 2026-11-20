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
		public void UnknownPointTest ()
		{
			using (var notKnownPoint = new NSString ("nariz"))
				Assert.That (ARSkeleton.CreateJointName (notKnownPoint), Is.Null);
		}

		[TestCase (VNHumanBodyPoseObservationJointName.Root)]
		[TestCase (VNHumanBodyPoseObservationJointName.LeftShoulder)]
		[TestCase (VNHumanBodyPoseObservationJointName.RightShoulder)]
		public void CreateJointNameTest (VNHumanBodyPoseObservationJointName recognizedPointKey)
		{
			using var expected = ARSkeleton.CreateJointName (recognizedPointKey.GetConstant ());
			using var actual = ARSkeleton.CreateJointName (recognizedPointKey);
			Assert.That (actual, Is.EqualTo (expected), "NSString overload");
		}

		[TestCase (VNHumanBodyPoseObservationJointName.None)]
		[TestCase ((VNHumanBodyPoseObservationJointName) (-1))]
		public void InvalidJointNameTest (VNHumanBodyPoseObservationJointName recognizedPointKey)
		{
			var exception = Assert.Throws<ArgumentOutOfRangeException> (() => ARSkeleton.CreateJointName (recognizedPointKey));
			Assert.That (exception.ParamName, Is.EqualTo ("recognizedPointKey"));
		}

	}
}
#endif // HAS_ARKIT
