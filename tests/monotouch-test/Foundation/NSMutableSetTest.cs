//
// Unit tests for NSMutableSet
//
// Authors:
//	Alex Soto <alex.soto@xamarin.com>
//	
//
// Copyright 2015 Xamarin Inc. All rights reserved.
//

namespace MonoTouchFixtures.Foundation {

	[TestFixture]
	[Preserve (AllMembers = true)]
	public class NSMutableSetTest {

		[Test]
		public void OperatorAddTest ()
		{
			var str1 = "1";
			var str2 = "2";
			var str3 = "3";

			using (var set1 = new NSMutableSet (str1))
			using (var set2 = new NSMutableSet (str2, str3))
			using (var result = set1 + set2) {
				Assert.That (result.Count, Is.EqualTo ((nuint) 3), "AddTest Count");
				Assert.That (result.Contains (str1), Is.True, "AddTest Contains 1");
				Assert.That (result.Contains (str2), Is.True, "AddTest Contains 2");
				Assert.That (result.Contains (str3), Is.True, "AddTest Contains 3");
			}
		}

		[Test]
		public void OperatorSubtractTest ()
		{
			var str1 = "1";
			var str2 = "2";
			var str3 = "3";
			var str4 = "4";

			var first = new NSMutableSet (str1, str2, str3, str4);
			var second = new NSMutableSet (str3, str4);
			var third = first - second;

			Assert.That (third.Count, Is.EqualTo ((nuint) 2), "OperatorSubtract Count");
			Assert.That (third.Contains (str1), Is.True, "OperatorSubtract 1");
			Assert.That (third.Contains (str2), Is.True, "OperatorSubtract 2");
			Assert.That (third.Contains (str3), Is.False, "OperatorSubtract 3");
			Assert.That (third.Contains (str4), Is.False, "OperatorSubtract 4");
		}

		[Test]
		public void OperatorPlusReferenceTest ()
		{
			var one = new NSMutableSet ("1", "2", "3");
			var two = new NSMutableSet ("4", "5", "6");
			NSMutableSet nil = null;
			using (var sum = one + nil)
			using (var sum2 = two + one)
			using (var sum3 = one + two) {

			}
			Assert.That (one.Handle, Is.Not.EqualTo (IntPtr.Zero), "Handle must be != IntPtr.Zero");
			Assert.That (two.Handle, Is.Not.EqualTo (IntPtr.Zero), "Handle must be != IntPtr.Zero");
		}

		[Test]
		public void OperatorAdd_BothNull ()
		{
			NSMutableSet first = null;
			NSMutableSet second = null;
			var result = first + second;
			Assert.That (result, Is.Null, "BothNull should return null");
		}

		[Test]
		public void OperatorAdd_FirstNull_SecondNonEmpty ()
		{
			NSMutableSet first = null;
			using (var second = new NSMutableSet ("1", "2"))
			using (var result = first + second) {
				Assert.That (result, Is.Not.Null, "FirstNull should return new set");
				Assert.That (result.Count, Is.EqualTo ((nuint) 2), "FirstNull Count");
				Assert.That (result.Contains ("1"), Is.True, "FirstNull Contains 1");
				Assert.That (result.Contains ("2"), Is.True, "FirstNull Contains 2");
			}
		}

		[Test]
		public void OperatorAdd_FirstNull_SecondEmpty ()
		{
			NSMutableSet first = null;
			using (var second = new NSMutableSet ())
			using (var result = first + second) {
				Assert.That (result, Is.Not.Null, "FirstNull SecondEmpty should return new set");
				Assert.That (result.Count, Is.EqualTo ((nuint) 0), "FirstNull SecondEmpty Count");
			}
		}

		[Test]
		public void OperatorAdd_FirstNonEmpty_SecondNull ()
		{
			using (var first = new NSMutableSet ("1", "2"))
			using (var result = first + null) {
				Assert.That (result, Is.Not.Null, "SecondNull should return new set");
				Assert.That (result.Count, Is.EqualTo ((nuint) 2), "SecondNull Count");
				Assert.That (result.Contains ("1"), Is.True, "SecondNull Contains 1");
				Assert.That (result.Contains ("2"), Is.True, "SecondNull Contains 2");
			}
		}

		[Test]
		public void OperatorAdd_FirstEmpty_SecondNull ()
		{
			using (var first = new NSMutableSet ())
			using (var result = first + null) {
				Assert.That (result, Is.Not.Null, "FirstEmpty SecondNull should return new set");
				Assert.That (result.Count, Is.EqualTo ((nuint) 0), "FirstEmpty SecondNull Count");
			}
		}

		[Test]
		public void OperatorAdd_FirstEmpty_SecondNonEmpty ()
		{
			using (var first = new NSMutableSet ())
			using (var second = new NSMutableSet ("1", "2"))
			using (var result = first + second) {
				Assert.That (result, Is.Not.Null, "FirstEmpty should return copy of second");
				Assert.That (result.Count, Is.EqualTo ((nuint) 2), "FirstEmpty Count");
				Assert.That (result.Contains ("1"), Is.True, "FirstEmpty Contains 1");
				Assert.That (result.Contains ("2"), Is.True, "FirstEmpty Contains 2");
			}
		}

		[Test]
		public void OperatorAdd_FirstNonEmpty_SecondEmpty ()
		{
			using (var first = new NSMutableSet ("1", "2"))
			using (var second = new NSMutableSet ())
			using (var result = first + second) {
				Assert.That (result, Is.Not.Null, "SecondEmpty should return copy of first");
				Assert.That (result.Count, Is.EqualTo ((nuint) 2), "SecondEmpty Count");
				Assert.That (result.Contains ("1"), Is.True, "SecondEmpty Contains 1");
				Assert.That (result.Contains ("2"), Is.True, "SecondEmpty Contains 2");
			}
		}

		[Test]
		public void OperatorAdd_BothEmpty ()
		{
			using (var first = new NSMutableSet ())
			using (var second = new NSMutableSet ())
			using (var result = first + second) {
				Assert.That (result, Is.Not.Null, "BothEmpty should return new empty set");
				Assert.That (result.Count, Is.EqualTo ((nuint) 0), "BothEmpty Count");
			}
		}

		[Test]
		public void OperatorAdd_WithOverlappingElements ()
		{
			using (var first = new NSMutableSet ("1", "2", "3"))
			using (var second = new NSMutableSet ("2", "3", "4"))
			using (var result = first + second) {
				Assert.That (result, Is.Not.Null, "Overlapping should return new set");
				Assert.That (result.Count, Is.EqualTo ((nuint) 4), "Overlapping Count");
				Assert.That (result.Contains ("1"), Is.True, "Overlapping Contains 1");
				Assert.That (result.Contains ("2"), Is.True, "Overlapping Contains 2");
				Assert.That (result.Contains ("3"), Is.True, "Overlapping Contains 3");
				Assert.That (result.Contains ("4"), Is.True, "Overlapping Contains 4");
			}
		}

		[Test]
		public void OperatorSubtract_FirstNull ()
		{
			NSMutableSet first = null;
			using (var second = new NSMutableSet ("1", "2")) {
				var result = first - second;
				Assert.That (result, Is.Null, "FirstNull should return null");
			}
		}

		[Test]
		public void OperatorSubtract_SecondNull ()
		{
			using (var first = new NSMutableSet ("1", "2"))
			using (var result = first - null) {
				Assert.That (result, Is.Not.Null, "SecondNull should return copy of first");
				Assert.That (result.Count, Is.EqualTo ((nuint) 2), "SecondNull Count");
				Assert.That (result.Contains ("1"), Is.True, "SecondNull Contains 1");
				Assert.That (result.Contains ("2"), Is.True, "SecondNull Contains 2");
			}
		}

		[Test]
		public void OperatorSubtract_BothNull ()
		{
			NSMutableSet first = null;
			NSMutableSet second = null;
			var result = first - second;
			Assert.That (result, Is.Null, "BothNull should return null");
		}

		[Test]
		public void OperatorSubtract_FirstEmpty ()
		{
			using (var first = new NSMutableSet ())
			using (var second = new NSMutableSet ("1", "2"))
			using (var result = first - second) {
				Assert.That (result, Is.Not.Null, "FirstEmpty should return empty set");
				Assert.That (result.Count, Is.EqualTo ((nuint) 0), "FirstEmpty Count");
			}
		}

		[Test]
		public void OperatorSubtract_SecondEmpty ()
		{
			using (var first = new NSMutableSet ("1", "2"))
			using (var second = new NSMutableSet ())
			using (var result = first - second) {
				Assert.That (result, Is.Not.Null, "SecondEmpty should return copy of first");
				Assert.That (result.Count, Is.EqualTo ((nuint) 2), "SecondEmpty Count");
				Assert.That (result.Contains ("1"), Is.True, "SecondEmpty Contains 1");
				Assert.That (result.Contains ("2"), Is.True, "SecondEmpty Contains 2");
			}
		}

		[Test]
		public void OperatorSubtract_BothEmpty ()
		{
			using (var first = new NSMutableSet ())
			using (var second = new NSMutableSet ())
			using (var result = first - second) {
				Assert.That (result, Is.Not.Null, "BothEmpty should return empty set");
				Assert.That (result.Count, Is.EqualTo ((nuint) 0), "BothEmpty Count");
			}
		}

		[Test]
		public void OperatorSubtract_NoOverlap ()
		{
			using (var first = new NSMutableSet ("1", "2"))
			using (var second = new NSMutableSet ("3", "4"))
			using (var result = first - second) {
				Assert.That (result, Is.Not.Null, "NoOverlap should return copy of first");
				Assert.That (result.Count, Is.EqualTo ((nuint) 2), "NoOverlap Count");
				Assert.That (result.Contains ("1"), Is.True, "NoOverlap Contains 1");
				Assert.That (result.Contains ("2"), Is.True, "NoOverlap Contains 2");
			}
		}

		[Test]
		public void OperatorSubtract_PartialOverlap ()
		{
			using (var first = new NSMutableSet ("1", "2", "3"))
			using (var second = new NSMutableSet ("2", "3", "4"))
			using (var result = first - second) {
				Assert.That (result, Is.Not.Null, "PartialOverlap should return difference");
				Assert.That (result.Count, Is.EqualTo ((nuint) 1), "PartialOverlap Count");
				Assert.That (result.Contains ("1"), Is.True, "PartialOverlap Contains 1");
				Assert.That (result.Contains ("2"), Is.False, "PartialOverlap Not Contains 2");
				Assert.That (result.Contains ("3"), Is.False, "PartialOverlap Not Contains 3");
			}
		}

		[Test]
		public void OperatorSubtract_CompleteOverlap ()
		{
			using (var first = new NSMutableSet ("1", "2", "3"))
			using (var second = new NSMutableSet ("1", "2", "3"))
			using (var result = first - second) {
				Assert.That (result, Is.Not.Null, "CompleteOverlap should return empty set");
				Assert.That (result.Count, Is.EqualTo ((nuint) 0), "CompleteOverlap Count");
			}
		}

		[Test]
		public void OperatorSubtract_SecondIsSupersetOfFirst ()
		{
			using (var first = new NSMutableSet ("1", "2"))
			using (var second = new NSMutableSet ("1", "2", "3", "4"))
			using (var result = first - second) {
				Assert.That (result, Is.Not.Null, "Superset should return empty set");
				Assert.That (result.Count, Is.EqualTo ((nuint) 0), "Superset Count");
			}
		}

		[Test]
		public void Ctor_WithNull ()
		{
			var str1 = (NSString) "1";
			NSObject? nullObj = null;
			using (var set = new NSMutableSet (str1, nullObj)) {
				Assert.That (set.Count, Is.EqualTo ((nuint) 2), "Count should include null");
				Assert.That (set.Contains (str1), Is.True, "Should contain string");
				Assert.That (set.Contains (NSNull.Null), Is.True, "Should contain NSNull");
			}
		}

		[Test]
		public void Ctor_NullArray ()
		{
			NSObject []? objs = null;
			using (var set = new NSMutableSet (objs)) {
				Assert.That (set.Count, Is.EqualTo ((nuint) 0), "Null array should create empty set");
			}
		}
	}
}
