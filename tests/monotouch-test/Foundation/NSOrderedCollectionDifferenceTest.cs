using System.Collections;

namespace MonoTouchFixtures.Foundation {
	[TestFixture]
	[Preserve (AllMembers = true)]
	public class NSOrderedCollectionDifferenceTest {
		
		[Test]
		public void InsertionsAndRemovalsTest ()
		{
			using var data = new NSString ("Foo");
			using var change = NSOrderedCollectionChange.ChangeWithObject (data, NSCollectionChangeType.Insert, 0);
			var changes = new NSOrderedCollectionChange [] { change };
			using var diff = new NSOrderedCollectionDifference (changes);
			Assert.That (diff.Insertions.Length, Is.EqualTo (1), "Insertions");
			Assert.That (diff.Insertions [0].Object, Is.EqualTo (data), "Inserted object");
			Assert.That (diff.Removals, Is.Empty, "Removals");
			Assert.That (diff.HasChanges, Is.True, "HasChanges");
		}

		[Test]
		public void TransformChanges ()
		{
			using var original = new NSString ("Original");
			using var replacement = new NSString ("Replacement");
			using var change = new NSOrderedCollectionChange (original, NSCollectionChangeType.Insert, 0);
			using var diff = new NSOrderedCollectionDifference (new [] { change });
			var calls = 0;
			using var transformed = diff.GetDifference (item => {
				calls++;
				Assert.That (item.Object, Is.EqualTo (original), "Callback object");
				return new NSOrderedCollectionChange (replacement, item.ChangeType, item.Index, item.AssociatedIndex);
			});
			Assert.That (calls, Is.EqualTo (1), "Callback count");
			Assert.That (transformed?.Insertions [0].Object, Is.EqualTo (replacement), "Transformed object");
			using var inverse = transformed?.GetInverseDifference ();
			Assert.That (inverse?.Removals [0].Object, Is.EqualTo (replacement), "Inverse object");
		}

		[Test]
		public void ArrayArguments ()
		{
			using var value = new NSString ("Value");
			using var array = NSArray.FromNSObjects (value);
			Assert.Throws<ArgumentNullException> (() => array.GetDifference ((NSObject []) null), "Null array");
			Assert.Throws<ArgumentNullException> (() => array.GetDifference ((NSObject []) null, 0), "Null array with options");
			using var difference = array.GetDifference (new NSObject [] { value });
			Assert.That (difference?.HasChanges, Is.False, "Equivalent array");
		}

		[Test]
		public void OmittedObjects ()
		{
			using var insertIndexes = new NSIndexSet (0);
			using var removeIndexes = new NSIndexSet ();
			using var difference = new NSOrderedCollectionDifference (insertIndexes, null, removeIndexes, null);
			Assert.That (difference.Insertions.Length, Is.EqualTo (1), "Insertion count");
			Assert.That (difference.Insertions [0].Object, Is.Null, "Omitted insertion");
			using var withChanges = new NSOrderedCollectionDifference (insertIndexes, null, removeIndexes, null, []);
			Assert.That (withChanges.Insertions.Length, Is.EqualTo (1), "Additional changes insertion count");
			Assert.That (withChanges.Insertions [0].Object, Is.Null, "Additional changes omitted insertion");
		}

		[Test]
		public void OrderedSetEquivalence ()
		{
			using var first = new NSString ("First");
			using var second = new NSString ("Second");
			using var original = new NSOrderedSet ((NSObject) first);
			using var changed = new NSOrderedSet (new NSObject [] { first, second });
			var calls = 0;
			using var difference = changed.GetDifference (original, 0, (a, b) => {
				calls++;
				return a?.ToString () == b?.ToString ();
			});
			Assert.That (calls, Is.GreaterThan (0), "Callback count");
			Assert.That (difference?.Insertions [0].Object, Is.EqualTo (second), "Inserted object");
			using var mutable = new NSMutableOrderedSet ((NSObject) first);
			mutable.ApplyDifference (difference);
			Assert.That (mutable.Count, Is.EqualTo ((nint) 2), "Mutable set count");
			Assert.That (mutable [1], Is.EqualTo (second), "Mutable set object");
		}
	}
}
