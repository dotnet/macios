
using CoreAnimation;
namespace MonoTouchFixtures.CoreAnimation {

	[TestFixture]
	[Preserve (AllMembers = true)]
	public class CAFrameRateRangeTest {

		[SetUp]
		public void SetUp ()
		{
			TestRuntime.AssertXcodeVersion (13, 0);
		}

		[Test]
		public void IsEqualToTest ()
			=> ClassicAssert.True (CAFrameRateRange.Default.IsEqualTo (CAFrameRateRange.Default));

		[Test]
		public void DefaultTest ()
			=> ClassicAssert.IsNotNull (CAFrameRateRange.Default, "Default");
	}
}
