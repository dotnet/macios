// Copyright (c) Microsoft Corporation.
// Licensed under the MIT License.

using System.Collections.Generic;
using System.Linq;

namespace MonoTouchFixtures.Foundation {

	[TestFixture]
	[Preserve (AllMembers = true)]
	public class NSSetOperatorTest {
		[Test]
		public void Set ()
		{
			TestOperators (values => new NSSet (values), values => new NSSet (values),
				(first, second) => first + second, (first, second) => first - second);
		}

		[Test]
		public void SetWithOrderedSet ()
		{
			TestOperators (values => new NSSet (values), values => new NSOrderedSet (values),
				(first, second) => first + second, (first, second) => first - second);
		}

		[Test]
		public void GenericSet ()
		{
			TestOperators (values => new NSSet<NSString> (values), values => new NSSet<NSString> (values),
				(first, second) => first + second, (first, second) => first - second);
		}

		[Test]
		public void MutableSet ()
		{
			TestOperators (values => new NSMutableSet (values), values => new NSMutableSet (values),
				(first, second) => first + second, (first, second) => first - second);
		}

		[Test]
		public void GenericMutableSet ()
		{
			TestOperators (values => new NSMutableSet<NSString> (values), values => new NSMutableSet<NSString> (values),
				(first, second) => first + second, (first, second) => first - second);
		}

		[Test]
		public void OrderedSet ()
		{
			TestOperators (values => new NSOrderedSet (values), values => new NSOrderedSet (values),
				(first, second) => first + second, (first, second) => first - second);
		}

		[Test]
		public void OrderedSetWithSet ()
		{
			TestOperators (values => new NSOrderedSet (values), values => new NSSet (values),
				(first, second) => first + second, (first, second) => first - second);
		}

		[Test]
		public void GenericOrderedSet ()
		{
			TestOperators (values => new NSOrderedSet<NSString> (values), values => new NSOrderedSet<NSString> (values),
				(first, second) => first + second, (first, second) => first - second);
		}

		[Test]
		public void GenericOrderedSetWithSet ()
		{
			TestOperators (values => new NSOrderedSet<NSString> (values), values => new NSSet<NSString> (values),
				(first, second) => first + second, (first, second) => first - second);
		}

		[Test]
		public void MutableOrderedSet ()
		{
			TestOperators (values => new NSMutableOrderedSet (values), values => new NSMutableOrderedSet (values),
				(first, second) => first + second, (first, second) => first - second);
		}

		[Test]
		public void MutableOrderedSetWithSet ()
		{
			TestOperators (values => new NSMutableOrderedSet (values), values => new NSSet (values),
				(first, second) => first + second, (first, second) => first - second);
		}

		[Test]
		public void GenericMutableOrderedSet ()
		{
			TestOperators (values => new NSMutableOrderedSet<NSString> (values), values => new NSMutableOrderedSet<NSString> (values),
				(first, second) => first + second, (first, second) => first - second);
		}

		[Test]
		public void GenericMutableOrderedSetWithSet ()
		{
			TestOperators (values => new NSMutableOrderedSet<NSString> (values), values => new NSSet<NSString> (values),
				(first, second) => first + second, (first, second) => first - second);
		}

		[Test]
		public void GenericMutableOrderedSetWithOrderedSet ()
		{
			TestOperators (values => new NSMutableOrderedSet<NSString> (values), values => new NSOrderedSet<NSString> (values),
				(first, second) => first + second, (first, second) => first - second);
		}

		static void TestOperators<TFirst, TSecond, TResult> (Func<NSString [], TFirst> createFirst, Func<NSString [], TSecond> createSecond,
			Func<TFirst, TSecond, TResult> add, Func<TFirst, TSecond, TResult> subtract)
			where TFirst : NSObject, IEnumerable<NSObject>
			where TSecond : NSObject, IEnumerable<NSObject>
			where TResult : NSObject, IEnumerable<NSObject>
		{
			using var one = new NSString ("1");
			using var two = new NSString ("2");
			using var three = new NSString ("3");
			using var four = new NSString ("4");
			NSString [] [] inputs = [null, [], [one, two], [two, three], [three, four]];

			for (var i = 0; i < inputs.Length; i++) {
				for (var j = 0; j < inputs.Length; j++) {
					using var first = inputs [i] is null ? null : createFirst (inputs [i]);
					using var second = inputs [j] is null ? null : createSecond (inputs [j]);
					NSObject [] firstValues = first is null ? [] : first.ToArray ();
					NSObject [] secondValues = second is null ? [] : second.ToArray ();
					var message = $"Inputs {i}, {j}";

					using (var sum = add (first, second)) {
						if (first is null && second is null) {
							Assert.That (sum, Is.Null, $"{message}: sum");
						} else {
							AssertResult (sum, firstValues.Union (secondValues).ToArray (), first, second, $"{message}: sum");
							if (sum is NSOrderedSet) {
								Assert.That (sum.Take (firstValues.Length), Is.EqualTo (firstValues), $"{message}: union preserves first order");
								if (second is NSOrderedSet)
									Assert.That (sum.ToArray (), Is.EqualTo (firstValues.Union (secondValues).ToArray ()), $"{message}: union order");
							}
						}
					}

					using (var difference = subtract (first, second)) {
						if (first is null) {
							Assert.That (difference, Is.Null, $"{message}: difference");
						} else {
							var expected = firstValues.Except (secondValues).ToArray ();
							AssertResult (difference, expected, first, second, $"{message}: difference");
							if (difference is NSOrderedSet)
								Assert.That (difference.ToArray (), Is.EqualTo (expected), $"{message}: difference order");
						}
					}

					if (first is not null)
						Assert.That (first.ToArray (), Is.EqualTo (firstValues), $"{message}: first unchanged");
					if (second is not null)
						Assert.That (second.ToArray (), Is.EqualTo (secondValues), $"{message}: second unchanged");
				}
			}
		}

		static void AssertResult<T> (T result, NSObject [] expected, NSObject first, NSObject second, string message)
			where T : NSObject, IEnumerable<NSObject>
		{
			Assert.That (result, Is.Not.Null, message);
			Assert.That (result, Is.Not.SameAs (first), $"{message}: not first");
			Assert.That (result, Is.Not.SameAs (second), $"{message}: not second");
			Assert.That (result.ToArray (), Is.EquivalentTo (expected), $"{message}: contents");

			using var extra = new NSString ("5");
			if (result is NSMutableSet mutableSet) {
				mutableSet.Add (extra);
				Assert.That (mutableSet.Contains (extra), Is.True, $"{message}: mutable");
				mutableSet.Remove (extra);
			} else if (result is NSMutableOrderedSet mutableOrderedSet) {
				mutableOrderedSet.Add (extra);
				Assert.That (mutableOrderedSet.Contains (extra), Is.True, $"{message}: mutable");
				mutableOrderedSet.RemoveObject (extra);
			}
		}
	}
}
