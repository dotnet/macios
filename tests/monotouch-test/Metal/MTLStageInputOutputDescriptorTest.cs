
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
			ClassicAssert.NotNull (descriptor.Layouts); // default value
		}

		[Test]
		public void GetAttributesTest ()
		{
			ClassicAssert.NotNull (descriptor.Attributes); // default value
		}

		[Test]
		public void GetSetIndexType ()
		{
			descriptor.IndexType = MTLIndexType.UInt32;
			ClassicAssert.AreEqual (MTLIndexType.UInt32, descriptor.IndexType);
		}

		[Test]
		public void GetSetIndexBufferTest ()
		{
			uint index = 5;
			descriptor.IndexBufferIndex = 5;
			ClassicAssert.AreEqual (descriptor.IndexBufferIndex, (nuint) index);
		}
	}
}
