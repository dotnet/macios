
using Metal;

namespace MonoTouchFixtures.Metal {

	[TestFixture]
	[Preserve (AllMembers = true)]
	public class MTLPointerTypeTests {
		MTLPointerType ptrType = null;

		[SetUp]
		public void SetUp ()
		{
			TestRuntime.AssertXcodeVersion (9, 0);
			ptrType = new MTLPointerType ();
		}

		[TearDown]
		public void TearDown ()
		{
			if (ptrType is not null)
				ptrType.Dispose ();
			ptrType = null;
		}

		[Test]
		public void GetAccessTest ()
		{
			ClassicAssert.AreEqual (MTLArgumentAccess.ReadOnly, ptrType.Access);
		}

		[Test]
		public void GetAlignmentTest ()
		{
			ClassicAssert.AreEqual ((nuint) 0, ptrType.Alignment);
		}

		[Test]
		public void GetDataSizeTest ()
		{
			ClassicAssert.AreEqual ((nuint) 0, ptrType.DataSize);
		}

		[Test]
		public void GetElementIsArgumentBufferTest ()
		{
			ClassicAssert.False (ptrType.ElementIsArgumentBuffer);
		}

		[Test]
		public void GetElementTypeTest ()
		{
			ClassicAssert.AreEqual (MTLDataType.None, ptrType.ElementType);
		}
	}
}
