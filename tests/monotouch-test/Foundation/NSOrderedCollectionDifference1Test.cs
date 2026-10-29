using System.Collections;
using CoreGraphics;

namespace MonoTouchFixtures.Foundation {
	[TestFixture]
	[Preserve (AllMembers = true)]
	public class NSOrderedCollectionDifference1Test {

		[Test]
		public void InsertionsAndRemovalsTest ()
		{
			using var data = new NSString ("Foo");
			using var change = NSOrderedCollectionChange<NSString>.ChangeWithObject (data, NSCollectionChangeType.Insert, 0);
			var changes = new NSOrderedCollectionChange<NSString> [] { change };
			using var diff = new NSOrderedCollectionDifference<NSString> (changes);
			Assert.That (diff.Insertions.Length, Is.EqualTo (1), "Insertions");
			Assert.That (diff.Insertions [0].Object, Is.EqualTo (data), "Inserted object");
			Assert.That (diff.Removals, Is.Empty, "Removals");
		}

		[Test]
		public void TransformNativeObjects ()
		{
			using var original = new CGColor (1, 0, 0);
			using var replacement = new CGColor (0, 1, 0);
			using var change = new NSOrderedCollectionChange<CGColor> (original, NSCollectionChangeType.Insert, 0);
			using var diff = new NSOrderedCollectionDifference<CGColor> (new [] { change });
			var calls = 0;
			using var transformed = diff.GetDifference (item => {
				calls++;
				Assert.That (item.Object?.Handle, Is.EqualTo (original.Handle), "Callback object");
				return new NSOrderedCollectionChange<CGColor> (replacement, item.ChangeType, item.Index, item.AssociatedIndex);
			});
			Assert.That (calls, Is.EqualTo (1), "Callback count");
			Assert.That (transformed?.Insertions [0].Object?.Handle, Is.EqualTo (replacement.Handle), "Transformed object");
			using var inverse = transformed?.GetInverseDifference ();
			Assert.That (inverse?.Removals [0].Object?.Handle, Is.EqualTo (replacement.Handle), "Inverse object");
		}

		[Test]
		public void ArrayAndOrderedSetDifferences ()
		{
			using var first = new NSString ("First");
			using var second = new NSString ("Second");
			using var originalArray = NSArray<NSString>.FromNSObjects (first);
			using var changedArray = NSArray<NSString>.FromNSObjects (first, second);
			var arrayCalls = 0;
			using var arrayDiff = changedArray.GetDifferenceFromArray (originalArray, 0, (a, b) => {
				arrayCalls++;
				return a?.ToString () == b?.ToString ();
			});
			Assert.That (arrayCalls, Is.GreaterThan (0), "Array callback count");
			Assert.That (arrayDiff?.Insertions [0].Object, Is.EqualTo (second), "Array insertion");
			Assert.That (originalArray.GetArrayByApplyingDifference (arrayDiff), Is.EqualTo (new [] { first, second }), "Applied array");
			using var originalSet = new NSOrderedSet<NSString> (first);
			using var changedSet = new NSOrderedSet<NSString> (first, second);
			var setCalls = 0;
			using var setDiff = changedSet.GetDifference (originalSet, 0, (a, b) => {
				setCalls++;
				return a?.ToString () == b?.ToString ();
			});
			Assert.That (setCalls, Is.GreaterThan (0), "Set callback count");
			Assert.That (setDiff?.Insertions [0].Object, Is.EqualTo (second), "Set insertion");
			using var appliedSet = originalSet.GetOrderedSet (setDiff);
			Assert.That (appliedSet?.Count, Is.EqualTo ((nint) 2), "Applied set count");
			Assert.That (appliedSet? [1], Is.EqualTo (second), "Applied set object");
		}

		[Test]
		public void NativeObjectEquivalence ()
		{
			using var first = new CGColor (1, 0, 0);
			using var second = new CGColor (0, 1, 0);
			using var original = NSArray<CGColor>.FromNSObjects (first);
			using var changed = NSArray<CGColor>.FromNSObjects (first, second);
			var calls = 0;
			using var difference = changed.GetDifferenceFromArray (original, 0, (a, b) => {
				calls++;
				return a?.Handle == b?.Handle;
			});
			Assert.That (calls, Is.GreaterThan (0), "Callback count");
			Assert.That (difference?.Insertions [0].Object?.Handle, Is.EqualTo (second.Handle), "Inserted object");
		}

		[Test]
		public void ApplyDifferences ()
		{
			using var first = new NSString ("First");
			using var second = new NSString ("Second");
			using var array = new NSMutableArray<NSString> (first);
			using var changed = NSArray<NSString>.FromNSObjects (first, second);
			using var difference = changed.GetDifference (new [] { first });
			array.ApplyDifference (difference);
			Assert.That (array.ToArray (), Is.EqualTo (new [] { first, second }), "Mutable array");
			using var set = new NSMutableOrderedSet<NSString> (first);
			set.ApplyDifference (difference);
			Assert.That (set.Count, Is.EqualTo ((nint) 2), "Mutable set count");
			Assert.That (set [1], Is.EqualTo (second), "Mutable set object");
		}

		[Test]
		public void OmittedObjects ()
		{
			using var insertIndexes = new NSIndexSet (0);
			using var removeIndexes = new NSIndexSet ();
			using var difference = new NSOrderedCollectionDifference<NSString> (insertIndexes, null, removeIndexes, null);
			Assert.That (difference.Insertions.Length, Is.EqualTo (1), "Insertion count");
			Assert.That (difference.Insertions [0].Object, Is.Null, "Omitted insertion");
			using var withChanges = new NSOrderedCollectionDifference<NSString> (insertIndexes, null, removeIndexes, null, []);
			Assert.That (withChanges.Insertions.Length, Is.EqualTo (1), "Additional changes insertion count");
			Assert.That (withChanges.Insertions [0].Object, Is.Null, "Additional changes omitted insertion");
		}
	}
}
