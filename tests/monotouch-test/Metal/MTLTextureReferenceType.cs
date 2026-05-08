
using Metal;

namespace MonoTouchFixtures.Metal {

	[TestFixture]
	[Preserve (AllMembers = true)]
	public class MTLTextureReferenceTypeTests {
		MTLTextureReferenceType reference = null;

		[SetUp]
		public void SetUp ()
		{
			TestRuntime.AssertXcodeVersion (9, 0);
			reference = new MTLTextureReferenceType ();
		}

		[TearDown]
		public void TearDown ()
		{
			if (reference is not null)
				reference.Dispose ();
			reference = null;
		}

		[Test]
		public void GetAccessTest ()
		{
			ClassicAssert.AreEqual (MTLArgumentAccess.ReadOnly, reference.Access);
		}

		[Test]
		public void GetIsDepthTextureTest ()
		{
			ClassicAssert.False (reference.IsDepthTexture);
		}

		[Test]
		public void GetTextureDataType ()
		{
			ClassicAssert.AreEqual (MTLDataType.None, reference.TextureDataType);
		}

		[Test]
		public void GetTextureType ()
		{
			ClassicAssert.AreEqual (MTLTextureType.k1D, reference.TextureType);
		}
	}
}
