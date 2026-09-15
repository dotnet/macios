using CoreData;

namespace MonoTouchFixtures.CoreData {

	[TestFixture]
	[Preserve (AllMembers = true)]
	public class ExpressionDescriptionTest {

		[Test]
		public void WeakFramework ()
		{
			using (var exp = new NSExpressionDescription ())
				Assert.That (exp.Handle, Is.Not.EqualTo (IntPtr.Zero), "Handle");
		}

		[Test]
		public void GetSetExpression ()
		{
			using (var exp = new NSExpressionDescription ()) {
				exp.Name = "Test";
				Assert.That (exp.Expression, Is.Null, "An unset Expression should be null.");
				exp.Expression = new NSExpression (NSExpressionType.Block);
				Assert.That (exp.Expression, Is.Not.Null, "Expression was not correctly set.");
			}
		}

		[Test]
		public void GetSetResultType ()
		{
			using (var exp = new NSExpressionDescription ()) {
				exp.Name = "Test";
				Assert.That (NSAttributeType.Undefined, Is.EqualTo (exp.ResultType), "The default value of an unset ResultType should be 'Undefined'");
				exp.ResultType = NSAttributeType.Boolean;
				Assert.That (exp.ResultType, Is.EqualTo (NSAttributeType.Boolean), "ResultType was not correctly set.");
			}
		}
	}
}
