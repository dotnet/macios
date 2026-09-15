
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
			Assert.That (ptrType.Access, Is.EqualTo (MTLArgumentAccess.ReadOnly));
		}

		[Test]
		public void GetAlignmentTest ()
		{
			Assert.That (ptrType.Alignment, Is.EqualTo ((nuint) 0));
		}

		[Test]
		public void GetDataSizeTest ()
		{
			Assert.That (ptrType.DataSize, Is.EqualTo ((nuint) 0));
		}

		[Test]
		public void GetElementIsArgumentBufferTest ()
		{
			Assert.That (ptrType.ElementIsArgumentBuffer, Is.False);
		}

		[Test]
		public void GetElementTypeTest ()
		{
			Assert.That (ptrType.ElementType, Is.EqualTo (MTLDataType.None));
		}
	}
}
