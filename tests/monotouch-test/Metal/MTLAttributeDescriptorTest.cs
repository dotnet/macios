
using Metal;

namespace MonoTouchFixtures.Metal {

	[TestFixture]
	[Preserve (AllMembers = true)]
	public class MTLAttributeDescriptorTest {
		MTLAttributeDescriptor descriptor = null;

		[SetUp]
		public void SetUp ()
		{
			TestRuntime.AssertXcodeVersion (8, 0);
			descriptor = new MTLAttributeDescriptor ();
		}

		[TearDown]
		public void TearDown ()
		{
			if (descriptor is not null)
				descriptor.Dispose ();
			descriptor = null;
		}

		[Test]
		public void GetSetFormatTest ()
		{
			descriptor.Format = MTLAttributeFormat.Invalid;
			Assert.That (descriptor.Format, Is.EqualTo (MTLAttributeFormat.Invalid));
		}

		[Test]
		public void GetSetOffsetTest ()
		{
			uint offset = 0; // must be 0, other value will crash the test.
			descriptor.Offset = offset;
			Assert.That (descriptor.Offset, Is.EqualTo ((nuint) offset));
		}

		[Test]
		public void GetSetBufferIndexTest ()
		{
			uint index = 0; // must be 0, other value will crash the test.
			descriptor.BufferIndex = index;
			Assert.That (descriptor.BufferIndex, Is.EqualTo ((nuint) index));
		}
	}
}
