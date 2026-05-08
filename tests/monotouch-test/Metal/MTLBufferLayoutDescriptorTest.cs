
using Metal;

namespace MonoTouchFixtures.Metal {

	[TestFixture]
	[Preserve (AllMembers = true)]
	public class MTLBufferLayoutDescriptorTest {

		[SetUp]
		public void Setup ()
		{
			TestRuntime.AssertXcodeVersion (8, 0);
		}

		[Test]
		public void GetSetStrideTest ()
		{
			uint stride = 8;
			var descriptor = new MTLBufferLayoutDescriptor ();
			descriptor.Stride = stride;
			ClassicAssert.AreEqual ((nuint) stride, descriptor.Stride);
		}

		[Test]
		public void GetSetStepFunctionTest ()
		{
			var func = MTLStepFunction.Constant;
			var descriptor = new MTLBufferLayoutDescriptor ();
			descriptor.StepFunction = func;
			ClassicAssert.AreEqual (func, descriptor.StepFunction);
		}

		[Test]
		public void GetSetStepRate ()
		{
			uint step = 8;
			var descriptor = new MTLBufferLayoutDescriptor ();
			descriptor.StepRate = step;
			ClassicAssert.AreEqual ((nuint) step, descriptor.StepRate);
		}
	}
}
