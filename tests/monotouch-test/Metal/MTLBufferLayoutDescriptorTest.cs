
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
			Assert.That (descriptor.Stride, Is.EqualTo ((nuint) stride));
		}

		[Test]
		public void GetSetStepFunctionTest ()
		{
			var func = MTLStepFunction.Constant;
			var descriptor = new MTLBufferLayoutDescriptor ();
			descriptor.StepFunction = func;
			Assert.That (descriptor.StepFunction, Is.EqualTo (func));
		}

		[Test]
		public void GetSetStepRate ()
		{
			uint step = 8;
			var descriptor = new MTLBufferLayoutDescriptor ();
			descriptor.StepRate = step;
			Assert.That (descriptor.StepRate, Is.EqualTo ((nuint) step));
		}
	}
}
