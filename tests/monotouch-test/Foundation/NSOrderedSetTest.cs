//
// Unit tests for NSOrderedSet
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
	public class NSOrderedSetTest {

		[Test]
		public void OperatorAddTest ()
		{
			var str1 = "1";
			var str2 = "2";
			var str3 = "3";

			using (var set1 = new NSOrderedSet (str1))
			using (var set2 = new NSOrderedSet (str2, str3))
			using (var result = set1 + set2) {
				Assert.That (result.Count, Is.EqualTo ((nint) 3), "AddTest Count");
				Assert.That (result.Contains (str1), Is.True, "AddTest Contains 1");
				Assert.That (result.Contains (str2), Is.True, "AddTest Contains 2");
				Assert.That (result.Contains (str3), Is.True, "AddTest Contains 3");
			}
		}

		[Test]
		public void OperatorAddTest2 ()
		{
			var str1 = "1";
			var str2 = "2";
			var str3 = "3";

			using (var set1 = new NSOrderedSet (str1))
			using (var set2 = new NSSet (str2, str3))
			using (var result = set1 + set2) {
				Assert.That (result.Count, Is.EqualTo ((nint) 3), "AddTest Count");
				Assert.That (result.Contains (str1), Is.True, "AddTest Contains 1");
				Assert.That (result.Contains (str2), Is.True, "AddTest Contains 2");
				Assert.That (result.Contains (str3), Is.True, "AddTest Contains 3");
			}
		}

		[Test]
		public void OperatorAddTest3 ()
		{
			var str1 = "1";
			var str2 = "2";
			var str3 = "3";

			using (var set1 = new NSOrderedSet (str1))
			using (var set2 = new NSMutableSet (str2, str3))
			using (var result = set1 + set2) {
				Assert.That (result.Count, Is.EqualTo ((nint) 3), "AddTest Count");
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

			using (var first = new NSOrderedSet (str1, str2, str3, str4))
			using (var second = new NSOrderedSet (str3, str4))
			using (var third = first - second) {

				Assert.That (third.Count, Is.EqualTo ((nint) 2), "OperatorSubtract Count");
				Assert.That (third.Contains (str1), Is.True, "OperatorSubtract 1");
				Assert.That (third.Contains (str2), Is.True, "OperatorSubtract 2");
				Assert.That (third.Contains (str3), Is.False, "OperatorSubtract 3");
				Assert.That (third.Contains (str4), Is.False, "OperatorSubtract 4");
			}
		}

		[Test]
		public void OperatorSubtractTest2 ()
		{
			var str1 = "1";
			var str2 = "2";
			var str3 = "3";
			var str4 = "4";

			using (var first = new NSOrderedSet (str1, str2, str3, str4))
			using (var second = new NSSet (str3, str4))
			using (var third = first - second) {

				Assert.That (third.Count, Is.EqualTo ((nint) 2), "OperatorSubtract Count");
				Assert.That (third.Contains (str1), Is.True, "OperatorSubtract 1");
				Assert.That (third.Contains (str2), Is.True, "OperatorSubtract 2");
				Assert.That (third.Contains (str3), Is.False, "OperatorSubtract 3");
				Assert.That (third.Contains (str4), Is.False, "OperatorSubtract 4");
			}
		}

		[Test]
		public void OperatorSubtractTest3 ()
		{
			var str1 = "1";
			var str2 = "2";
			var str3 = "3";
			var str4 = "4";

			using (var first = new NSOrderedSet (str1, str2, str3, str4))
			using (var second = new NSMutableSet (str3, str4))
			using (var third = first - second) {

				Assert.That (third.Count, Is.EqualTo ((nint) 2), "OperatorSubtract Count");
				Assert.That (third.Contains (str1), Is.True, "OperatorSubtract 1");
				Assert.That (third.Contains (str2), Is.True, "OperatorSubtract 2");
				Assert.That (third.Contains (str3), Is.False, "OperatorSubtract 3");
				Assert.That (third.Contains (str4), Is.False, "OperatorSubtract 4");
			}
		}

		[Test]
		public void OperatorPlusReferenceTest ()
		{
			var one = new NSOrderedSet ("1", "2", "3");
			var two = new NSOrderedSet ("4", "5", "6");
			NSOrderedSet nil = null;
			using (var sum = one + nil)
			using (var sum2 = two + one)
			using (var sum3 = one + two) {

			}
			Assert.That (one.Handle, Is.Not.EqualTo (IntPtr.Zero), "Handle must be != IntPtr.Zero");
			Assert.That (two.Handle, Is.Not.EqualTo (IntPtr.Zero), "Handle must be != IntPtr.Zero");
		}

		[Test]
		public void OperatorEqualTest ()
		{
			var str1 = "1";
			var str2 = "2";
			var str3 = "3";

			using (var oSet = new NSOrderedSet (str1, str2, str3))
			using (var oSet2 = new NSOrderedSet (str1, str2, str3)) {
				Assert.That (oSet == oSet2, Is.True, "NSOrderedSetTest == must be true");
				Assert.That (oSet.Equals (oSet2), Is.True, "NSOrderedSetTest Equals must be true");
			}
		}

		[Test]
		public void OperatorDifferentTest ()
		{
			var str1 = "1";
			var str2 = "2";
			var str3 = "3";

			using (var oSet = new NSOrderedSet (str1, str2, str3))
			using (var oSet2 = new NSOrderedSet (str3, str2, str1)) {
				Assert.That (oSet != oSet2, Is.True, "NSOrderedSetTest != must be true");
				Assert.That (oSet.Equals (oSet2), Is.False, "NSOrderedSetTest Equals must be false");
			}
		}

		[Test]
		public void Ctor_WithNull ()
		{
			var str1 = (NSString) "1";
			NSObject? nullObj = null;
			using (var set = new NSOrderedSet (str1, nullObj)) {
				Assert.That ((int) set.Count, Is.EqualTo (2), "Count should include null");
				Assert.That (set [0], Is.EqualTo (str1), "First item");
				Assert.That (set [1], Is.InstanceOf<NSNull> (), "Second item should be NSNull");
			}
		}

		[Test]
		public void Ctor_NullArray ()
		{
			NSObject []? objs = null;
			using (var set = new NSOrderedSet (objs)) {
				Assert.That ((int) set.Count, Is.EqualTo (0), "Null array should create empty set");
			}
		}

		[Test]
		public void MakeNSOrderedSet_WithNull ()
		{
			var str1 = (NSString) "1";
			var str2 = (NSString) "2";
			var values = new NSString? [] { str1, null, str2 };
			using (var set = NSOrderedSet.MakeNSOrderedSet (values)) {
				Assert.That ((int) set.Count, Is.EqualTo (3), "Count should include null");
				Assert.That (set [0], Is.EqualTo (str1), "First item");
				Assert.That (set [1], Is.InstanceOf<NSNull> (), "Second item should be NSNull");
				Assert.That (set [2], Is.EqualTo (str2), "Third item");
			}
		}

		[Test]
		public void MakeNSOrderedSet_NullArray ()
		{
			NSString []? values = null;
			using (var set = NSOrderedSet.MakeNSOrderedSet (values)) {
				Assert.That (set, Is.Not.Null, "Should create a set");
				Assert.That ((int) set.Count, Is.EqualTo (0), "Null array should create empty set");
			}
		}
	}
}
