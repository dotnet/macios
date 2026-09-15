
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
			Assert.That (attr.Name, Is.Null, $"Name default value is {attr.Name}");
		}

		[Test]
		public void GetAttributeIndexTest ()
		{
			Assert.That (attr.AttributeIndex, Is.EqualTo ((nuint) 0), $"AttributeIndex default value is {attr.AttributeIndex}");
		}

		[Test]
		public void GetAttributeTypeTest ()
		{
			Assert.That (attr.AttributeType, Is.EqualTo (MTLDataType.None), $"AttributeType default value is {attr.AttributeType}");
		}

		[Test]
		public void GetActiveTest ()
		{
			Assert.That (attr.Active, Is.False);
		}

		[Test]
		public void GetIsPatchDataTest ()
		{
			Assert.That (attr.IsPatchData, Is.False);
		}

		[Test]
		public void GetIsPatchControlPointDataTest ()
		{
			Assert.That (attr.IsPatchControlPointData, Is.False);
		}
	}
}
