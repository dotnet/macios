
using Metal;

namespace MonoTouchFixtures.Metal {

	[TestFixture]
	[Preserve (AllMembers = true)]
	public class MTLFunctionConstantTest {

		[SetUp]
		public void Setup ()
		{
			TestRuntime.AssertXcodeVersion (8, 0);
		}

		[Test]
		public void GetNameTest ()
		{
			var constant = new MTLFunctionConstant ();
			Assert.That (constant.Name, Is.Null); // defualt value is null
		}

		[Test]
		public void GetTypeTest ()
		{
			var constant = new MTLFunctionConstant ();
			Assert.That (constant.Type, Is.EqualTo (MTLDataType.None)); // default value is none
		}

		[Test]
		public void GetIndexTest ()
		{
			var constant = new MTLFunctionConstant ();
			Assert.That (constant.Index, Is.EqualTo ((nuint) 0), $"Index is {constant.Index}"); // default value is 0
		}

		[Test]
		public void GetIsRequiredTest ()
		{
			var constant = new MTLFunctionConstant ();
			Assert.That (constant.IsRequired, Is.False); // defualt value is false
		}
	}
}
