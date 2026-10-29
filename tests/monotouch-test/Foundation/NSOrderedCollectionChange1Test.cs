using CoreGraphics;

namespace MonoTouchFixtures.Foundation {
	[TestFixture]
	[Preserve (AllMembers = true)]
	public class NSOrderedCollectionChange1Test {
		
		[Test]
		public void ChangeWithObjectTest ()
		{
			TestRuntime.AssertXcodeVersion (13,0);

			using var str = new NSString ("Test");
			using var change = NSOrderedCollectionChange<NSString>.ChangeWithObject (str, NSCollectionChangeType.Insert, 0);
			Assert.That (change.Object, Is.EqualTo (str), "Content");
			Assert.That (change.Index, Is.EqualTo ((nuint)0), "Index");
		}

		[Test]
		public void ChangeWithObjectWithAssociatedIndexTest ()
		{
			TestRuntime.AssertXcodeVersion (13,0);

			using var str = new NSString ("Test");
			using var change = NSOrderedCollectionChange<NSString>.ChangeWithObject (str, NSCollectionChangeType.Insert, 0, 1);
			Assert.That (change.Object, Is.EqualTo (str), "Content");
			Assert.That (change.Index, Is.EqualTo ((nuint)0), "Index");
			Assert.That (change.AssociatedIndex, Is.EqualTo ((nuint)1));
		}

		[Test]
		public void NullObject ()
		{
			using var change = new NSOrderedCollectionChange<NSString> (null, NSCollectionChangeType.Insert, 0);
			using var associatedChange = new NSOrderedCollectionChange<NSString> (null, NSCollectionChangeType.Remove, 1, 0);
			Assert.That (change.Object, Is.Null, "Constructor");
			Assert.That (associatedChange.Object, Is.Null, "Associated constructor");
		}

		[Test]
		public void NativeObject ()
		{
			using var color = new CGColor (1, 0, 0);
			using var change = new NSOrderedCollectionChange<CGColor> (color, NSCollectionChangeType.Insert, 0);
			using var factoryChange = NSOrderedCollectionChange<CGColor>.ChangeWithObject (color, NSCollectionChangeType.Insert, 0);
			Assert.That (change.Object?.Handle, Is.EqualTo (color.Handle), "Constructor");
			Assert.That (factoryChange.Object?.Handle, Is.EqualTo (color.Handle), "Factory");
		}
	}
}
