
using Metal;

namespace MonoTouchFixtures.Metal {

	[TestFixture]
	[Preserve (AllMembers = true)]
	public class MTLIndirectCommandBufferDescriptorTest {
		MTLIndirectCommandBufferDescriptor descriptor = null;

		[SetUp]
		public void SetUp ()
		{
			TestRuntime.AssertXcodeVersion (10, 0);
			descriptor = new MTLIndirectCommandBufferDescriptor ();
		}

		[TearDown]
		public void TearDown ()
		{
			if (descriptor is not null)
				descriptor.Dispose ();
			descriptor = null;
		}

		[Test]
		public void GetSetCommandTypesTest ()
		{
			descriptor.CommandTypes = MTLIndirectCommandType.Draw;
			Assert.That (descriptor.CommandTypes, Is.EqualTo (MTLIndirectCommandType.Draw));
		}

#if MONOMAC
		[Test]
		public void GetSetInheritPipelineStateTest ()
		{
			descriptor.InheritPipelineState = true;
			Assert.That (descriptor.InheritPipelineState, Is.EqualTo (true));
		}
#endif

		[Test]
		public void GetSetInheritBuffersTest ()
		{
			descriptor.InheritBuffers = true;
			Assert.That (descriptor.InheritBuffers, Is.EqualTo (true));
		}

		[Test]
		public void GetSetMaxVertexBufferBindCountTest ()
		{
			descriptor.MaxVertexBufferBindCount = 1;
			Assert.That (descriptor.MaxVertexBufferBindCount, Is.EqualTo ((nuint) 1));
		}

		[Test]
		public void GetSetMaxFragmentBufferBindCountTest ()
		{
			descriptor.MaxFragmentBufferBindCount = 1;
			Assert.That (descriptor.MaxFragmentBufferBindCount, Is.EqualTo ((nuint) 1));
		}
	}
}
