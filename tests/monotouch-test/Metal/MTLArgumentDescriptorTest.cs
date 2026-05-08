
using Metal;

namespace MonoTouchFixtures.Metal {

	[TestFixture]
	[Preserve (AllMembers = true)]
	public class MTLArgumentDescriptorTest {
		MTLArgumentDescriptor descriptor = null;

		[SetUp]
		public void SetUp ()
		{
			TestRuntime.AssertXcodeVersion (9, 0);
			descriptor = new MTLArgumentDescriptor ();
		}

		[TearDown]
		public void TearDown ()
		{
			if (descriptor is not null)
				descriptor.Dispose ();
			descriptor = null;
		}

		[Test]
		public void GetSetAccessTest ()
		{
			descriptor.Access = MTLArgumentAccess.ReadWrite;
			Assert.That (descriptor.Access, Is.EqualTo (MTLArgumentAccess.ReadWrite));
		}

		[Test]
		public void GetSetArrayLengthTest ()
		{
			descriptor.ArrayLength = 1;
			Assert.That (descriptor.ArrayLength, Is.EqualTo ((nuint) 1));
		}

		[Test]
		public void GetSetConstantBlockAlignmentTest ()
		{
			descriptor.ConstantBlockAlignment = 1;
			Assert.That (descriptor.ConstantBlockAlignment, Is.EqualTo ((nuint) 1));
		}

		[Test]
		public void GetSetDataTypeTest ()
		{
			descriptor.DataType = MTLDataType.Half4;
			Assert.That (descriptor.DataType, Is.EqualTo (MTLDataType.Half4));
		}

		[Test]
		public void GetSetIndexTest ()
		{
			descriptor.Index = 1;
			Assert.That (descriptor.Index, Is.EqualTo ((nuint) 1));
		}

		[Test]
		public void GetSetTextureTypeTest ()
		{
			descriptor.TextureType = MTLTextureType.k2DArray;
			Assert.That (descriptor.TextureType, Is.EqualTo (MTLTextureType.k2DArray));
		}
	}
}
