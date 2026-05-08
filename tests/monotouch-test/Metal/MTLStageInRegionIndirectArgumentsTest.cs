
using Metal;

namespace MonoTouchFixtures.Metal {

	[TestFixture]
	[Preserve (AllMembers = true)]
	public class MTLStageInRegionIndirectArgumentsTest {

		[SetUp]
		public void SetUp ()
		{
			TestRuntime.AssertXcodeVersion (10, 0);
		}

		[Test]
		public void SizeOfMTLStageInRegionIndirectArgumentsTest ()
		{
			unsafe {
				Assert.That (24, Is.EqualTo (sizeof (MTLStageInRegionIndirectArguments))); // 24 is the size of the native struct
			}
		}
	}
}
