using System.Collections;

namespace MonoTouchFixtures.Foundation {

	[TestFixture]
	[Preserve (AllMembers = true)]
	public class NSMutableSet1Test {

		[Test]
		public void Ctor ()
		{
			using (var arr = new NSMutableSet<NSDate> ()) {
				Assert.AreEqual ((nuint) 0, arr.Count, "Count");
			}
		}

		[Test]
		public void Ctor_Params ()
		{
			using (var arr = new NSMutableSet<NSString> ((NSString) "foo")) {
				Assert.AreEqual ((nuint) 1, arr.Count, "Count");
			}
			using (var arr = new NSMutableSet<NSString> ((NSString) "foo", (NSString) "bar")) {
				Assert.AreEqual ((nuint) 2, arr.Count, "Count");
			}
		}

		[Test]
		public void Ctor_OtherSet ()
		{
			var v1 = (NSString) "1";

			using (var first = new NSSet<NSString> (v1)) {
				using (var second = new NSMutableSet<NSString> (first)) {
					Assert.AreEqual ((nuint) 1, first.Count, "1 count");
					Assert.AreEqual ((nuint) 1, second.Count, "2 count");
				}
			}
		}

		[Test]
		public void Ctor_OtherMutableSet ()
		{
			var v1 = (NSString) "1";

			using (var first = new NSMutableSet<NSString> (v1)) {
				using (var second = new NSMutableSet<NSString> (first)) {
					Assert.AreEqual ((nuint) 1, first.Count, "1 count");
					Assert.AreEqual ((nuint) 1, second.Count, "2 count");
				}
			}
		}

		[Test]
		public void LookupMemberTest ()
		{
			var v1 = (NSString) "1";
			var v2 = (NSString) "2";

			using (var st = new NSMutableSet<NSString> (v1)) {
				Assert.Throws<ArgumentNullException> (() => st.LookupMember ((NSString) null), "LookupMember ANE 1");
				Assert.AreSame (v1, st.LookupMember (v1), "LookupMember 1");
				Assert.IsNull (st.LookupMember (v2), "LookupMember 2");
			}
		}

		[Test]
		public void AnyObjectTest ()
		{
			var v1 = (NSString) "1";
			var v2 = (NSString) "2";

			using (var st = new NSMutableSet<NSString> ()) {
				Assert.IsNull (st.AnyObject, "AnyObject 1");
			}

			using (var st = new NSMutableSet<NSString> (v1)) {
				Assert.AreSame (v1, st.AnyObject, "AnyObject 2");
			}
		}

		[Test]
		public void ContainsTest ()
		{
			var v1 = (NSString) "1";
			var v2 = (NSString) "2";

			using (var st = new NSMutableSet<NSString> (v1)) {
				Assert.Throws<ArgumentNullException> (() => st.Contains ((NSString) null), "Contains ANE 1");
				Assert.IsTrue (st.Contains (v1), "Contains 1");
				Assert.IsFalse (st.Contains (v2), "Contains 2");
			}
		}

		[Test]
		public void ToArrayTest ()
		{
			var v1 = (NSString) "1";

			using (var st = new NSMutableSet<NSString> (v1)) {
				var arr = st.ToArray ();
				Assert.AreEqual (1, arr.Length, "ToArray Length");
				Assert.AreSame (v1, arr [0], "ToArray () [0]");
			}
		}

		[Test]
		public void OperatorAddTest ()
		{
			var v1 = (NSString) "1";
			var v2 = (NSString) "2";

			using (var first = new NSMutableSet<NSString> (v1)) {
				using (var second = new NSMutableSet<NSString> (v2)) {
					using (var third = first + second) {
						Assert.AreEqual ((nuint) 2, third.Count, "+ Count");
						Assert.IsTrue (third.Contains (v1), "+ 1");
						Assert.IsTrue (third.Contains (v2), "+ 2");
					}
				}
			}
		}

		[Test]
		public void OperatorSubtractTest ()
		{
			var v1 = (NSString) "1";
			var v2 = (NSString) "2";

			using (var first = new NSMutableSet<NSString> (v1, v2)) {
				using (var second = new NSMutableSet<NSString> (v2)) {
					using (var third = first - second) {
						Assert.AreEqual ((nuint) 1, third.Count, "- Count");
						Assert.IsTrue (third.Contains (v1), "- 1");
					}
				}
			}
		}

		[Test]
		public void AddTest ()
		{
			var v1 = (NSString) "1";

			using (var st = new NSMutableSet<NSString> ()) {
				Assert.Throws<ArgumentNullException> (() => st.Add ((NSString) null), "Add ANE 1");

				st.Add (v1);
				Assert.IsTrue (st.Contains (v1), "Add 1");
				Assert.AreSame (v1, st.AnyObject, "Add 2");
			}
		}

		[Test]
		public void RemoveTest ()
		{
			var v1 = (NSString) "1";
			var v2 = (NSString) "2";

			using (var st = new NSMutableSet<NSString> (v1)) {
				Assert.Throws<ArgumentNullException> (() => st.Remove ((NSString) null), "Remove ANE 1");

				st.Remove (v2);
				Assert.AreEqual ((nuint) 1, st.Count, "Remove 1 Count");
				Assert.IsTrue (st.Contains (v1), "Remove 1 Contains");
				Assert.AreSame (v1, st.AnyObject, "Remove 1 AnyObject");

				st.Remove (v1);
				Assert.AreEqual ((nuint) 0, st.Count, "Remove 2 Count");
			}
		}

		[Test]
		public void AddObjectsTest ()
		{
			var v1 = (NSString) "1";
			var v2 = (NSString) "2";

			using (var st = new NSMutableSet<NSString> ()) {
				Assert.Throws<ArgumentNullException> (() => st.AddObjects ((NSString) null), "AddObjects ANE 1");
				Assert.Throws<ArgumentNullException> (() => st.AddObjects ((NSString []) null), "AddObjects ANE 2");

				st.AddObjects (v1);
				Assert.AreEqual ((nuint) 1, st.Count, "AddObjects 1 Count");
				Assert.IsTrue (st.Contains (v1), "AddObjects 1 Contains");

				st.RemoveAll ();
				st.AddObjects (v1, v1);
				Assert.AreEqual ((nuint) 1, st.Count, "AddObjects 2 Count");
				Assert.IsTrue (st.Contains (v1), "AddObjects 2 Contains");

				st.RemoveAll ();
				st.AddObjects (v2, v1);
				Assert.AreEqual ((nuint) 2, st.Count, "AddObjects 3 Count");
				Assert.IsTrue (st.Contains (v1), "AddObjects 3 Contains a");
				Assert.IsTrue (st.Contains (v2), "AddObjects 3 Contains b");
			}
		}

		[Test]
		public void IEnumerable1Test ()
		{
			const int C = 16 * 2 + 3; // NSFastEnumerator has a array of size 16, use more than that, and not an exact multiple.
			var values = new NSString [C];
			for (int i = 0; i < C; i++)
				values [i] = (NSString) i.ToString ();

			using (var st = new NSMutableSet<NSString> (values)) {
				Assert.AreEqual ((nuint) C, st.Count, "Count 1");

				var lst = new List<NSString> ();
				foreach (var a in (IEnumerable<NSString>) st) {
					Assert.IsNotNull (a, "null item iterator");
					Assert.IsFalse (lst.Contains (a), "duplicated item iterator");
					lst.Add (a);
					Assert.IsTrue (Array.IndexOf (values, a) >= 0, "different object");
				}
				Assert.AreEqual (C, lst.Count, "iterator count");
			}
		}

		[Test]
		public void IEnumerable1Test_EnumeratorType ()
		{
			var myEnumerable = new NSMutableSet<NSNumber> ();
			foreach (var item in myEnumerable) {
				// The point of this test is to verify that the compiler finds the correct enumerator (the one returning NSNumbers, and not the one from the non-generic NSSet class returning NSObjects).
				// This means that we don't have to actually execute this code, it's enough to make it compile.
				Console.WriteLine (item.LongValue);
			}
		}

		[Test]
		public void IEnumerableTest ()
		{
			const int C = 16 * 2 + 3; // NSFastEnumerator has a array of size 16, use more than that, and not an exact multiple.
			var values = new NSString [C];
			for (int i = 0; i < C; i++)
				values [i] = (NSString) i.ToString ();

			using (var st = new NSMutableSet<NSString> (values)) {
				Assert.AreEqual ((nuint) C, st.Count, "Count 1");

				var lst = new List<NSString> ();
				foreach (NSString a in (IEnumerable) st) {
					Assert.IsNotNull (a, "null item iterator");
					Assert.IsFalse (lst.Contains (a), "duplicated item iterator");
					lst.Add (a);
					Assert.IsTrue (Array.IndexOf (values, a) >= 0, "different object");
				}
				Assert.AreEqual (C, lst.Count, "iterator count");
			}
		}

		[Test]
		public void OperatorPlusReferenceTest ()
		{
			var one = new NSMutableSet<NSString> ((NSString) "1", (NSString) "2", (NSString) "3");
			var two = new NSMutableSet<NSString> ((NSString) "4", (NSString) "5", (NSString) "6");
			NSMutableSet<NSString> nil = null;
			using (var sum = one + nil)
			using (var sum2 = two + one)
			using (var sum3 = one + two) {

			}
			Assert.AreNotEqual (IntPtr.Zero, one.Handle, "Handle must be != IntPtr.Zero");
			Assert.AreNotEqual (IntPtr.Zero, two.Handle, "Handle must be != IntPtr.Zero");
		}

		[Test]
		public void OperatorPlus_BothNull ()
		{
			NSMutableSet<NSString> first = null;
			NSMutableSet<NSString> second = null;
			var result = first + second;
			Assert.IsNull (result, "Both null should return null");
		}

		[Test]
		public void OperatorPlus_FirstNull ()
		{
			NSMutableSet<NSString> first = null;
			using (var second = new NSMutableSet<NSString> ((NSString) "1", (NSString) "2")) {
				using (var result = first + second) {
					Assert.IsNotNull (result, "Result should not be null");
					Assert.AreEqual ((nuint) 2, result.Count, "Count");
					Assert.IsTrue (result.Contains ((NSString) "1"), "Contains 1");
					Assert.IsTrue (result.Contains ((NSString) "2"), "Contains 2");
				}
			}
		}

		[Test]
		public void OperatorPlus_SecondNull ()
		{
			using (var first = new NSMutableSet<NSString> ((NSString) "1", (NSString) "2")) {
				NSMutableSet<NSString> second = null;
				using (var result = first + second) {
					Assert.IsNotNull (result, "Result should not be null");
					Assert.AreEqual ((nuint) 2, result.Count, "Count");
					Assert.IsTrue (result.Contains ((NSString) "1"), "Contains 1");
					Assert.IsTrue (result.Contains ((NSString) "2"), "Contains 2");
				}
			}
		}

		[Test]
		public void OperatorPlus_BothEmpty ()
		{
			using (var first = new NSMutableSet<NSString> ())
			using (var second = new NSMutableSet<NSString> ())
			using (var result = first + second) {
				Assert.IsNotNull (result, "Result should not be null");
				Assert.AreEqual ((nuint) 0, result.Count, "Count should be 0");
			}
		}

		[Test]
		public void OperatorPlus_FirstEmpty ()
		{
			using (var first = new NSMutableSet<NSString> ())
			using (var second = new NSMutableSet<NSString> ((NSString) "1", (NSString) "2"))
			using (var result = first + second) {
				Assert.IsNotNull (result, "Result should not be null");
				Assert.AreEqual ((nuint) 2, result.Count, "Count");
				Assert.IsTrue (result.Contains ((NSString) "1"), "Contains 1");
				Assert.IsTrue (result.Contains ((NSString) "2"), "Contains 2");
			}
		}

		[Test]
		public void OperatorPlus_SecondEmpty ()
		{
			using (var first = new NSMutableSet<NSString> ((NSString) "1", (NSString) "2"))
			using (var second = new NSMutableSet<NSString> ())
			using (var result = first + second) {
				Assert.IsNotNull (result, "Result should not be null");
				Assert.AreEqual ((nuint) 2, result.Count, "Count");
				Assert.IsTrue (result.Contains ((NSString) "1"), "Contains 1");
				Assert.IsTrue (result.Contains ((NSString) "2"), "Contains 2");
			}
		}

		[Test]
		public void OperatorPlus_Overlapping ()
		{
			using (var first = new NSMutableSet<NSString> ((NSString) "1", (NSString) "2"))
			using (var second = new NSMutableSet<NSString> ((NSString) "2", (NSString) "3"))
			using (var result = first + second) {
				Assert.IsNotNull (result, "Result should not be null");
				Assert.AreEqual ((nuint) 3, result.Count, "Count should be 3 (set union)");
				Assert.IsTrue (result.Contains ((NSString) "1"), "Contains 1");
				Assert.IsTrue (result.Contains ((NSString) "2"), "Contains 2");
				Assert.IsTrue (result.Contains ((NSString) "3"), "Contains 3");
			}
		}

		[Test]
		public void OperatorMinus_BothNull ()
		{
			NSMutableSet<NSString> first = null;
			NSMutableSet<NSString> second = null;
			var result = first - second;
			Assert.IsNull (result, "Both null should return null");
		}

		[Test]
		public void OperatorMinus_FirstNull ()
		{
			NSMutableSet<NSString> first = null;
			using (var second = new NSMutableSet<NSString> ((NSString) "1", (NSString) "2")) {
				var result = first - second;
				Assert.IsNull (result, "First null should return null");
			}
		}

		[Test]
		public void OperatorMinus_SecondNull ()
		{
			using (var first = new NSMutableSet<NSString> ((NSString) "1", (NSString) "2")) {
				NSMutableSet<NSString> second = null;
				using (var result = first - second) {
					Assert.IsNotNull (result, "Result should not be null");
					Assert.AreEqual ((nuint) 2, result.Count, "Count");
					Assert.IsTrue (result.Contains ((NSString) "1"), "Contains 1");
					Assert.IsTrue (result.Contains ((NSString) "2"), "Contains 2");
				}
			}
		}

		[Test]
		public void OperatorMinus_FirstEmpty ()
		{
			using (var first = new NSMutableSet<NSString> ())
			using (var second = new NSMutableSet<NSString> ((NSString) "1", (NSString) "2")) {
				var result = first - second;
				Assert.IsNull (result, "Empty first should return null");
			}
		}

		[Test]
		public void OperatorMinus_SecondEmpty ()
		{
			using (var first = new NSMutableSet<NSString> ((NSString) "1", (NSString) "2"))
			using (var second = new NSMutableSet<NSString> ())
			using (var result = first - second) {
				Assert.IsNotNull (result, "Result should not be null");
				Assert.AreEqual ((nuint) 2, result.Count, "Count");
				Assert.IsTrue (result.Contains ((NSString) "1"), "Contains 1");
				Assert.IsTrue (result.Contains ((NSString) "2"), "Contains 2");
			}
		}

		[Test]
		public void OperatorMinus_BothEmpty ()
		{
			using (var first = new NSMutableSet<NSString> ())
			using (var second = new NSMutableSet<NSString> ()) {
				var result = first - second;
				Assert.IsNull (result, "Both empty should return null");
			}
		}

		[Test]
		public void OperatorMinus_NoOverlap ()
		{
			using (var first = new NSMutableSet<NSString> ((NSString) "1", (NSString) "2"))
			using (var second = new NSMutableSet<NSString> ((NSString) "3", (NSString) "4"))
			using (var result = first - second) {
				Assert.IsNotNull (result, "Result should not be null");
				Assert.AreEqual ((nuint) 2, result.Count, "Count");
				Assert.IsTrue (result.Contains ((NSString) "1"), "Contains 1");
				Assert.IsTrue (result.Contains ((NSString) "2"), "Contains 2");
			}
		}

		[Test]
		public void OperatorMinus_PartialOverlap ()
		{
			using (var first = new NSMutableSet<NSString> ((NSString) "1", (NSString) "2", (NSString) "3"))
			using (var second = new NSMutableSet<NSString> ((NSString) "2", (NSString) "4"))
			using (var result = first - second) {
				Assert.IsNotNull (result, "Result should not be null");
				Assert.AreEqual ((nuint) 2, result.Count, "Count");
				Assert.IsTrue (result.Contains ((NSString) "1"), "Contains 1");
				Assert.IsTrue (result.Contains ((NSString) "3"), "Contains 3");
				Assert.IsFalse (result.Contains ((NSString) "2"), "Should not contain 2");
			}
		}

		[Test]
		public void OperatorMinus_CompleteOverlap ()
		{
			using (var first = new NSMutableSet<NSString> ((NSString) "1", (NSString) "2"))
			using (var second = new NSMutableSet<NSString> ((NSString) "1", (NSString) "2"))
			using (var result = first - second) {
				Assert.IsNotNull (result, "Result should not be null");
				Assert.AreEqual ((nuint) 0, result.Count, "Count should be 0");
			}
		}

		[Test]
		public void Ctor_Capacity ()
		{
			using (var set = new NSMutableSet<NSString> (10)) {
				Assert.AreEqual ((nuint) 0, set.Count, "Empty with capacity");
			}
		}

		[Test]
		public void ToArray_Empty ()
		{
			using (var set = new NSMutableSet<NSString> ()) {
				var arr = set.ToArray ();
				Assert.IsNotNull (arr, "Array should not be null");
				Assert.AreEqual (0, arr.Length, "Length should be 0");
			}
		}

		[Test]
		public void ToArray_Multiple ()
		{
			var v1 = (NSString) "1";
			var v2 = (NSString) "2";
			var v3 = (NSString) "3";

			using (var set = new NSMutableSet<NSString> (v1, v2, v3)) {
				var arr = set.ToArray ();
				Assert.AreEqual (3, arr.Length, "Length");
				Assert.Contains (v1, arr, "Contains v1");
				Assert.Contains (v2, arr, "Contains v2");
				Assert.Contains (v3, arr, "Contains v3");
			}
		}

		[Test]
		public void Add_Duplicate ()
		{
			var v1 = (NSString) "1";

			using (var set = new NSMutableSet<NSString> ()) {
				set.Add (v1);
				Assert.AreEqual ((nuint) 1, set.Count, "Count after first add");

				set.Add (v1);
				Assert.AreEqual ((nuint) 1, set.Count, "Count after duplicate add");
			}
		}

		[Test]
		public void Remove_NonExistent ()
		{
			var v1 = (NSString) "1";
			var v2 = (NSString) "2";

			using (var set = new NSMutableSet<NSString> (v1)) {
				set.Remove (v2);
				Assert.AreEqual ((nuint) 1, set.Count, "Count should remain 1");
				Assert.IsTrue (set.Contains (v1), "Should still contain v1");
			}
		}

		[Test]
		public void AddObjects_Empty ()
		{
			using (var set = new NSMutableSet<NSString> ()) {
				set.AddObjects ();
				Assert.AreEqual ((nuint) 0, set.Count, "Count should be 0");
			}
		}

		[Test]
		public void AddObjects_WithNullElement ()
		{
			var v1 = (NSString) "1";

			using (var set = new NSMutableSet<NSString> ()) {
				Assert.Throws<ArgumentNullException> (() => set.AddObjects (v1, null), "Should throw on null element");
			}
		}

		[Test]
		public void LookupMember_Empty ()
		{
			var v1 = (NSString) "1";

			using (var set = new NSMutableSet<NSString> ()) {
				var result = set.LookupMember (v1);
				Assert.IsNull (result, "Should return null for empty set");
			}
		}

		[Test]
		public void Contains_Empty ()
		{
			var v1 = (NSString) "1";

			using (var set = new NSMutableSet<NSString> ()) {
				Assert.IsFalse (set.Contains (v1), "Empty set should not contain any element");
			}
		}

		[Test]
		public void Enumeration_Empty ()
		{
			using (var set = new NSMutableSet<NSString> ()) {
				var count = 0;
				foreach (var item in set) {
					count++;
				}
				Assert.AreEqual (0, count, "Should not enumerate any items");
			}
		}

		[Test]
		public void Enumeration_Single ()
		{
			var v1 = (NSString) "1";

			using (var set = new NSMutableSet<NSString> (v1)) {
				var count = 0;
				NSString found = null;
				foreach (var item in set) {
					count++;
					found = item;
				}
				Assert.AreEqual (1, count, "Should enumerate one item");
				Assert.AreSame (v1, found, "Should find v1");
			}
		}

		[Test]
		public void Ctor_Params_Empty ()
		{
			using (var set = new NSMutableSet<NSString> ()) {
				Assert.AreEqual ((nuint) 0, set.Count, "Empty params");
			}
		}
	}
}
