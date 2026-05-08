
using Metal;

namespace MonoTouchFixtures.Metal {

	[TestFixture]
	[Preserve (AllMembers = true)]
	public class MTLAttributeTest {
		MTLAttribute attr = null;

		[SetUp]
		public void SetUp ()
		{
			TestRuntime.AssertXcodeVersion (8, 0);
			attr = new MTLAttribute ();
		}

		[TearDown]
		public void TearDown ()
		{
			if (attr is not null)
				attr.Dispose ();
			attr = null;
		}

		[Test]
		public void GetNameTest ()
		{
			ClassicAssert.Null (attr.Name, $"Name default value is {attr.Name}");
		}

		[Test]
		public void GetAttributeIndexTest ()
		{
			ClassicAssert.AreEqual ((nuint) 0, attr.AttributeIndex, $"AttributeIndex default value is {attr.AttributeIndex}");
		}

		[Test]
		public void GetAttributeTypeTest ()
		{
			ClassicAssert.AreEqual (MTLDataType.None, attr.AttributeType, $"AttributeType default value is {attr.AttributeType}");
		}

		[Test]
		public void GetActiveTest ()
		{
			ClassicAssert.False (attr.Active);
		}

		[Test]
		public void GetIsPatchDataTest ()
		{
			ClassicAssert.False (attr.IsPatchData);
		}

		[Test]
		public void GetIsPatchControlPointDataTest ()
		{
			ClassicAssert.False (attr.IsPatchControlPointData);
		}
	}
}
