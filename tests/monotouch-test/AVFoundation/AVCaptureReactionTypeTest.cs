//
// Unit tests for AVCaptureReactionType

using AVFoundation;

#nullable enable

namespace MonoTouchFixtures.AVFoundation {

	[TestFixture]
	[Preserve (AllMembers = true)]
	public class AVCaptureReactionTypeTest {
		[Test]
		public void GetSystemImage ()
		{
			TestRuntime.AssertXcodeVersion (15, 0);
			Assert.IsNotNull (AVCaptureReactionType.ThumbsUp.GetSystemImage (), "GetSystemImage");
		}
	}
}
