
using Metal;

namespace MonoTouchFixtures.Metal {

	[TestFixture]
	[Preserve (AllMembers = true)]
	public class MTLStageInputOutputDescriptorTest {
		MTLStageInputOutputDescriptor descriptor = null;

		[SetUp]
		public void SetUp ()
		{
			TestRuntime.AssertXcodeVersion (8, 0);
			descriptor = MTLStageInputOutputDescriptor.Create ();
		}

		[TearDown]
		public void TearDown ()
		{
			if (descriptor is not null)
				descriptor.Dispose ();
			descriptor = null;
		}

		[Test]
		public void GetLayoutsTest ()
		{
			Assert.That (descriptor.Layouts, Is.Not.Null); // default value
		}

		[Test]
		public void GetAttributesTest ()
		{
			Assert.That (descriptor.Attributes, Is.Not.Null); // default value
		}

		[Test]
		public void GetSetIndexType ()
		{
			descriptor.IndexType = MTLIndexType.UInt32;
			Assert.That (descriptor.IndexType, Is.EqualTo (MTLIndexType.UInt32));
		}

		[Test]
		public void GetSetIndexBufferTest ()
		{
			uint index = 5;
			descriptor.IndexBufferIndex = 5;
			Assert.That ((nuint) index, Is.EqualTo (descriptor.IndexBufferIndex));
		}
	}
}
