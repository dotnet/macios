
namespace MonoTouchFixtures.Foundation {
	[TestFixture]
	[Preserve (AllMembers = true)]
	public class NSOrderedCollectionChangeTest {

		[Test]
		public void ChangeWithObjectTest ()
		{
			TestRuntime.AssertXcodeVersion (13, 0);

			using var str = new NSString ("Test");
			using var change = NSOrderedCollectionChange.ChangeWithObject (str, NSCollectionChangeType.Insert, 0);
			Assert.That (change.Object, Is.EqualTo (str), "Content");
			Assert.That (change.Index, Is.EqualTo ((nuint) 0), "Index");
		}

		[Test]
		public void ChangeWithObjectTestWithAssociatedindex ()
		{
			TestRuntime.AssertXcodeVersion (13, 0);

			using var str = new NSString ("Test");
			using var change = NSOrderedCollectionChange.ChangeWithObject (str, NSCollectionChangeType.Insert, 0, 1);
			Assert.That (change.Object, Is.EqualTo (str), "Content");
			Assert.That (change.Index, Is.EqualTo ((nuint) 0), "Index");
			Assert.That (change.AssociatedIndex, Is.EqualTo ((nuint) 1));
		}

		[Test]
		public void NullObject ()
		{
			using var change = new NSOrderedCollectionChange (null, NSCollectionChangeType.Insert, 0);
			using var associatedChange = new NSOrderedCollectionChange (null, NSCollectionChangeType.Remove, 1, 0);
			using var factoryChange = NSOrderedCollectionChange.ChangeWithObject (null, NSCollectionChangeType.Insert, 0);
			Assert.That (change.Object, Is.Null, "Constructor");
			Assert.That (associatedChange.Object, Is.Null, "Associated constructor");
			Assert.That (factoryChange.Object, Is.Null, "Factory");
		}
	}
}
