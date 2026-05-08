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
				ClassicAssert.AreEqual ((nuint) 3, result.Count, "AddTest Count");
				ClassicAssert.IsTrue (result.Contains (str1), "AddTest Contains 1");
				ClassicAssert.IsTrue (result.Contains (str2), "AddTest Contains 2");
				ClassicAssert.IsTrue (result.Contains (str3), "AddTest Contains 3");
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

			ClassicAssert.AreEqual ((nuint) 2, third.Count, "OperatorSubtract Count");
			ClassicAssert.IsTrue (third.Contains (str1), "OperatorSubtract 1");
			ClassicAssert.IsTrue (third.Contains (str2), "OperatorSubtract 2");
			ClassicAssert.IsFalse (third.Contains (str3), "OperatorSubtract 3");
			ClassicAssert.IsFalse (third.Contains (str4), "OperatorSubtract 4");
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
			ClassicAssert.AreNotEqual (IntPtr.Zero, one.Handle, "Handle must be != IntPtr.Zero");
			ClassicAssert.AreNotEqual (IntPtr.Zero, two.Handle, "Handle must be != IntPtr.Zero");
		}

		[Test]
		public void OperatorAdd_BothNull ()
		{
			NSMutableSet first = null;
			NSMutableSet second = null;
			var result = first + second;
			ClassicAssert.IsNull (result, "BothNull should return null");
		}

		[Test]
		public void OperatorAdd_FirstNull_SecondNonEmpty ()
		{
			NSMutableSet first = null;
			using (var second = new NSMutableSet ("1", "2"))
			using (var result = first + second) {
				ClassicAssert.IsNotNull (result, "FirstNull should return new set");
				ClassicAssert.AreEqual ((nuint) 2, result.Count, "FirstNull Count");
				ClassicAssert.IsTrue (result.Contains ("1"), "FirstNull Contains 1");
				ClassicAssert.IsTrue (result.Contains ("2"), "FirstNull Contains 2");
			}
		}

		[Test]
		public void OperatorAdd_FirstNull_SecondEmpty ()
		{
			NSMutableSet first = null;
			using (var second = new NSMutableSet ())
			using (var result = first + second) {
				ClassicAssert.IsNotNull (result, "FirstNull SecondEmpty should return new set");
				ClassicAssert.AreEqual ((nuint) 0, result.Count, "FirstNull SecondEmpty Count");
			}
		}

		[Test]
		public void OperatorAdd_FirstNonEmpty_SecondNull ()
		{
			using (var first = new NSMutableSet ("1", "2"))
			using (var result = first + null) {
				ClassicAssert.IsNotNull (result, "SecondNull should return new set");
				ClassicAssert.AreEqual ((nuint) 2, result.Count, "SecondNull Count");
				ClassicAssert.IsTrue (result.Contains ("1"), "SecondNull Contains 1");
				ClassicAssert.IsTrue (result.Contains ("2"), "SecondNull Contains 2");
			}
		}

		[Test]
		public void OperatorAdd_FirstEmpty_SecondNull ()
		{
			using (var first = new NSMutableSet ())
			using (var result = first + null) {
				ClassicAssert.IsNotNull (result, "FirstEmpty SecondNull should return new set");
				ClassicAssert.AreEqual ((nuint) 0, result.Count, "FirstEmpty SecondNull Count");
			}
		}

		[Test]
		public void OperatorAdd_FirstEmpty_SecondNonEmpty ()
		{
			using (var first = new NSMutableSet ())
			using (var second = new NSMutableSet ("1", "2"))
			using (var result = first + second) {
				ClassicAssert.IsNotNull (result, "FirstEmpty should return copy of second");
				ClassicAssert.AreEqual ((nuint) 2, result.Count, "FirstEmpty Count");
				ClassicAssert.IsTrue (result.Contains ("1"), "FirstEmpty Contains 1");
				ClassicAssert.IsTrue (result.Contains ("2"), "FirstEmpty Contains 2");
			}
		}

		[Test]
		public void OperatorAdd_FirstNonEmpty_SecondEmpty ()
		{
			using (var first = new NSMutableSet ("1", "2"))
			using (var second = new NSMutableSet ())
			using (var result = first + second) {
				ClassicAssert.IsNotNull (result, "SecondEmpty should return copy of first");
				ClassicAssert.AreEqual ((nuint) 2, result.Count, "SecondEmpty Count");
				ClassicAssert.IsTrue (result.Contains ("1"), "SecondEmpty Contains 1");
				ClassicAssert.IsTrue (result.Contains ("2"), "SecondEmpty Contains 2");
			}
		}

		[Test]
		public void OperatorAdd_BothEmpty ()
		{
			using (var first = new NSMutableSet ())
			using (var second = new NSMutableSet ())
			using (var result = first + second) {
				ClassicAssert.IsNotNull (result, "BothEmpty should return new empty set");
				ClassicAssert.AreEqual ((nuint) 0, result.Count, "BothEmpty Count");
			}
		}

		[Test]
		public void OperatorAdd_WithOverlappingElements ()
		{
			using (var first = new NSMutableSet ("1", "2", "3"))
			using (var second = new NSMutableSet ("2", "3", "4"))
			using (var result = first + second) {
				ClassicAssert.IsNotNull (result, "Overlapping should return new set");
				ClassicAssert.AreEqual ((nuint) 4, result.Count, "Overlapping Count");
				ClassicAssert.IsTrue (result.Contains ("1"), "Overlapping Contains 1");
				ClassicAssert.IsTrue (result.Contains ("2"), "Overlapping Contains 2");
				ClassicAssert.IsTrue (result.Contains ("3"), "Overlapping Contains 3");
				ClassicAssert.IsTrue (result.Contains ("4"), "Overlapping Contains 4");
			}
		}

		[Test]
		public void OperatorSubtract_FirstNull ()
		{
			NSMutableSet first = null;
			using (var second = new NSMutableSet ("1", "2")) {
				var result = first - second;
				ClassicAssert.IsNull (result, "FirstNull should return null");
			}
		}

		[Test]
		public void OperatorSubtract_SecondNull ()
		{
			using (var first = new NSMutableSet ("1", "2"))
			using (var result = first - null) {
				ClassicAssert.IsNotNull (result, "SecondNull should return copy of first");
				ClassicAssert.AreEqual ((nuint) 2, result.Count, "SecondNull Count");
				ClassicAssert.IsTrue (result.Contains ("1"), "SecondNull Contains 1");
				ClassicAssert.IsTrue (result.Contains ("2"), "SecondNull Contains 2");
			}
		}

		[Test]
		public void OperatorSubtract_BothNull ()
		{
			NSMutableSet first = null;
			NSMutableSet second = null;
			var result = first - second;
			ClassicAssert.IsNull (result, "BothNull should return null");
		}

		[Test]
		public void OperatorSubtract_FirstEmpty ()
		{
			using (var first = new NSMutableSet ())
			using (var second = new NSMutableSet ("1", "2"))
			using (var result = first - second) {
				ClassicAssert.IsNotNull (result, "FirstEmpty should return empty set");
				ClassicAssert.AreEqual ((nuint) 0, result.Count, "FirstEmpty Count");
			}
		}

		[Test]
		public void OperatorSubtract_SecondEmpty ()
		{
			using (var first = new NSMutableSet ("1", "2"))
			using (var second = new NSMutableSet ())
			using (var result = first - second) {
				ClassicAssert.IsNotNull (result, "SecondEmpty should return copy of first");
				ClassicAssert.AreEqual ((nuint) 2, result.Count, "SecondEmpty Count");
				ClassicAssert.IsTrue (result.Contains ("1"), "SecondEmpty Contains 1");
				ClassicAssert.IsTrue (result.Contains ("2"), "SecondEmpty Contains 2");
			}
		}

		[Test]
		public void OperatorSubtract_BothEmpty ()
		{
			using (var first = new NSMutableSet ())
			using (var second = new NSMutableSet ())
			using (var result = first - second) {
				ClassicAssert.IsNotNull (result, "BothEmpty should return empty set");
				ClassicAssert.AreEqual ((nuint) 0, result.Count, "BothEmpty Count");
			}
		}

		[Test]
		public void OperatorSubtract_NoOverlap ()
		{
			using (var first = new NSMutableSet ("1", "2"))
			using (var second = new NSMutableSet ("3", "4"))
			using (var result = first - second) {
				ClassicAssert.IsNotNull (result, "NoOverlap should return copy of first");
				ClassicAssert.AreEqual ((nuint) 2, result.Count, "NoOverlap Count");
				ClassicAssert.IsTrue (result.Contains ("1"), "NoOverlap Contains 1");
				ClassicAssert.IsTrue (result.Contains ("2"), "NoOverlap Contains 2");
			}
		}

		[Test]
		public void OperatorSubtract_PartialOverlap ()
		{
			using (var first = new NSMutableSet ("1", "2", "3"))
			using (var second = new NSMutableSet ("2", "3", "4"))
			using (var result = first - second) {
				ClassicAssert.IsNotNull (result, "PartialOverlap should return difference");
				ClassicAssert.AreEqual ((nuint) 1, result.Count, "PartialOverlap Count");
				ClassicAssert.IsTrue (result.Contains ("1"), "PartialOverlap Contains 1");
				ClassicAssert.IsFalse (result.Contains ("2"), "PartialOverlap Not Contains 2");
				ClassicAssert.IsFalse (result.Contains ("3"), "PartialOverlap Not Contains 3");
			}
		}

		[Test]
		public void OperatorSubtract_CompleteOverlap ()
		{
			using (var first = new NSMutableSet ("1", "2", "3"))
			using (var second = new NSMutableSet ("1", "2", "3"))
			using (var result = first - second) {
				ClassicAssert.IsNotNull (result, "CompleteOverlap should return empty set");
				ClassicAssert.AreEqual ((nuint) 0, result.Count, "CompleteOverlap Count");
			}
		}

		[Test]
		public void OperatorSubtract_SecondIsSupersetOfFirst ()
		{
			using (var first = new NSMutableSet ("1", "2"))
			using (var second = new NSMutableSet ("1", "2", "3", "4"))
			using (var result = first - second) {
				ClassicAssert.IsNotNull (result, "Superset should return empty set");
				ClassicAssert.AreEqual ((nuint) 0, result.Count, "Superset Count");
			}
		}

		[Test]
		public void Ctor_WithNull ()
		{
			var str1 = (NSString) "1";
			NSObject? nullObj = null;
			using (var set = new NSMutableSet (str1, nullObj)) {
				ClassicAssert.AreEqual ((nuint) 2, set.Count, "Count should include null");
				ClassicAssert.IsTrue (set.Contains (str1), "Should contain string");
				ClassicAssert.IsTrue (set.Contains (NSNull.Null), "Should contain NSNull");
			}
		}

		[Test]
		public void Ctor_NullArray ()
		{
			NSObject []? objs = null;
			using (var set = new NSMutableSet (objs)) {
				ClassicAssert.AreEqual ((nuint) 0, set.Count, "Null array should create empty set");
			}
		}
	}
}
