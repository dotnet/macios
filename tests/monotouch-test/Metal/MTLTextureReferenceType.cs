
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
			Assert.That (reference.Access, Is.EqualTo (MTLArgumentAccess.ReadOnly));
		}

		[Test]
		public void GetIsDepthTextureTest ()
		{
			Assert.That (reference.IsDepthTexture, Is.False);
		}

		[Test]
		public void GetTextureDataType ()
		{
			Assert.That (reference.TextureDataType, Is.EqualTo (MTLDataType.None));
		}

		[Test]
		public void GetTextureType ()
		{
			Assert.That (reference.TextureType, Is.EqualTo (MTLTextureType.k1D));
		}
	}
}
