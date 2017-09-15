﻿#if !__WATCHOS__

using System;

#if XAMCORE_2_0
using Metal;
#else
using MonoTouch.Metal;
#endif

using NUnit.Framework;

namespace MonoTouchFixtures.Metal {

	[TestFixture]
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
			if (reference != null)
				reference.Dispose ();
			reference = null;
		}

		[Test]
		public void GetAccessTest ()
		{
			Assert.AreEqual (MTLArgumentAccess.ReadOnly, reference.Access);
		}

		[Test]
		public void GetIsDepthTextureTest ()
		{
			Assert.False (reference.IsDepthTexture);
		}

		[Test]
		public void GetTextureDataType ()
		{
			Assert.AreEqual (MTLDataType.None, reference.TextureDataType);
		}

		[Test]
		public void GetTextureType ()
		{
			Assert.AreEqual (MTLTextureType.k1D, reference.TextureType);
		}
	}
}

#endif // !__WATCHOS__
