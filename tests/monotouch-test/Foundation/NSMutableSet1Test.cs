using System.Collections;

namespace MonoTouchFixtures.Foundation {

	[TestFixture]
	[Preserve (AllMembers = true)]
	public class NSMutableSet1Test {

		[Test]
		public void Ctor ()
		{
			using (var arr = new NSMutableSet<NSDate> ()) {
				Assert.That (arr.Count, Is.EqualTo ((nuint) 0), "Count");
			}
		}

		[Test]
		public void Ctor_Params ()
		{
			using (var arr = new NSMutableSet<NSString> ((NSString) "foo")) {
				Assert.That (arr.Count, Is.EqualTo ((nuint) 1), "Count");
			}
			using (var arr = new NSMutableSet<NSString> ((NSString) "foo", (NSString) "bar")) {
				Assert.That (arr.Count, Is.EqualTo ((nuint) 2), "Count");
			}
		}

		[Test]
		public void Ctor_OtherSet ()
		{
			var v1 = (NSString) "1";

			using (var first = new NSSet<NSString> (v1)) {
				using (var second = new NSMutableSet<NSString> (first)) {
					Assert.That (first.Count, Is.EqualTo ((nuint) 1), "1 count");
					Assert.That (second.Count, Is.EqualTo ((nuint) 1), "2 count");
				}
			}
		}

		[Test]
		public void Ctor_OtherMutableSet ()
		{
			var v1 = (NSString) "1";

			using (var first = new NSMutableSet<NSString> (v1)) {
				using (var second = new NSMutableSet<NSString> (first)) {
					Assert.That (first.Count, Is.EqualTo ((nuint) 1), "1 count");
					Assert.That (second.Count, Is.EqualTo ((nuint) 1), "2 count");
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
				Assert.That (st.LookupMember (v1), Is.SameAs (v1), "LookupMember 1");
				Assert.That (st.LookupMember (v2), Is.Null, "LookupMember 2");
			}
		}

		[Test]
		public void AnyObjectTest ()
		{
			var v1 = (NSString) "1";
			var v2 = (NSString) "2";

			using (var st = new NSMutableSet<NSString> ()) {
				Assert.That (st.AnyObject, Is.Null, "AnyObject 1");
			}

			using (var st = new NSMutableSet<NSString> (v1)) {
				Assert.That (st.AnyObject, Is.SameAs (v1), "AnyObject 2");
			}
		}

		[Test]
		public void ContainsTest ()
		{
			var v1 = (NSString) "1";
			var v2 = (NSString) "2";

			using (var st = new NSMutableSet<NSString> (v1)) {
				Assert.Throws<ArgumentNullException> (() => st.Contains ((NSString) null), "Contains ANE 1");
				Assert.That (st.Contains (v1), Is.True, "Contains 1");
				Assert.That (st.Contains (v2), Is.False, "Contains 2");
			}
		}

		[Test]
		public void ToArrayTest ()
		{
			var v1 = (NSString) "1";

			using (var st = new NSMutableSet<NSString> (v1)) {
				var arr = st.ToArray ();
				Assert.That (arr.Length, Is.EqualTo (1), "ToArray Length");
				Assert.That (arr [0], Is.SameAs (v1), "ToArray () [0]");
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
						Assert.That (third.Count, Is.EqualTo ((nuint) 2), "+ Count");
						Assert.That (third.Contains (v1), Is.True, "+ 1");
						Assert.That (third.Contains (v2), Is.True, "+ 2");
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
						Assert.That (third.Count, Is.EqualTo ((nuint) 1), "- Count");
						Assert.That (third.Contains (v1), Is.True, "- 1");
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
				Assert.That (st.Contains (v1), Is.True, "Add 1");
				Assert.That (st.AnyObject, Is.SameAs (v1), "Add 2");
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
				Assert.That (st.Count, Is.EqualTo ((nuint) 1), "Remove 1 Count");
				Assert.That (st.Contains (v1), Is.True, "Remove 1 Contains");
				Assert.That (st.AnyObject, Is.SameAs (v1), "Remove 1 AnyObject");

				st.Remove (v1);
				Assert.That (st.Count, Is.EqualTo ((nuint) 0), "Remove 2 Count");
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
				Assert.That (st.Count, Is.EqualTo ((nuint) 1), "AddObjects 1 Count");
				Assert.That (st.Contains (v1), Is.True, "AddObjects 1 Contains");

				st.RemoveAll ();
				st.AddObjects (v1, v1);
				Assert.That (st.Count, Is.EqualTo ((nuint) 1), "AddObjects 2 Count");
				Assert.That (st.Contains (v1), Is.True, "AddObjects 2 Contains");

				st.RemoveAll ();
				st.AddObjects (v2, v1);
				Assert.That (st.Count, Is.EqualTo ((nuint) 2), "AddObjects 3 Count");
				Assert.That (st.Contains (v1), Is.True, "AddObjects 3 Contains a");
				Assert.That (st.Contains (v2), Is.True, "AddObjects 3 Contains b");
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
				Assert.That (st.Count, Is.EqualTo ((nuint) C), "Count 1");

				var lst = new List<NSString> ();
				foreach (var a in (IEnumerable<NSString>) st) {
					Assert.That (a, Is.Not.Null, "null item iterator");
					Assert.That (lst.Contains (a), Is.False, "duplicated item iterator");
					lst.Add (a);
					Assert.That (Array.IndexOf (values, a) >= 0, Is.True, "different object");
				}
				Assert.That (lst.Count, Is.EqualTo (C), "iterator count");
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
				Assert.That (st.Count, Is.EqualTo ((nuint) C), "Count 1");

				var lst = new List<NSString> ();
				foreach (NSString a in (IEnumerable) st) {
					Assert.That (a, Is.Not.Null, "null item iterator");
					Assert.That (lst.Contains (a), Is.False, "duplicated item iterator");
					lst.Add (a);
					Assert.That (Array.IndexOf (values, a) >= 0, Is.True, "different object");
				}
				Assert.That (lst.Count, Is.EqualTo (C), "iterator count");
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
			Assert.That (one.Handle, Is.Not.EqualTo (IntPtr.Zero), "Handle must be != IntPtr.Zero");
			Assert.That (two.Handle, Is.Not.EqualTo (IntPtr.Zero), "Handle must be != IntPtr.Zero");
		}

		[Test]
		public void OperatorPlus_BothNull ()
		{
			NSMutableSet<NSString> first = null;
			NSMutableSet<NSString> second = null;
			var result = first + second;
			Assert.That (result, Is.Null, "Both null should return null");
		}

		[Test]
		public void OperatorPlus_FirstNull ()
		{
			NSMutableSet<NSString> first = null;
			using (var second = new NSMutableSet<NSString> ((NSString) "1", (NSString) "2")) {
				using (var result = first + second) {
					Assert.That (result, Is.Not.Null, "Result should not be null");
					Assert.That (result.Count, Is.EqualTo ((nuint) 2), "Count");
					Assert.That (result.Contains ((NSString) "1"), Is.True, "Contains 1");
					Assert.That (result.Contains ((NSString) "2"), Is.True, "Contains 2");
				}
			}
		}

		[Test]
		public void OperatorPlus_SecondNull ()
		{
			using (var first = new NSMutableSet<NSString> ((NSString) "1", (NSString) "2")) {
				NSMutableSet<NSString> second = null;
				using (var result = first + second) {
					Assert.That (result, Is.Not.Null, "Result should not be null");
					Assert.That (result.Count, Is.EqualTo ((nuint) 2), "Count");
					Assert.That (result.Contains ((NSString) "1"), Is.True, "Contains 1");
					Assert.That (result.Contains ((NSString) "2"), Is.True, "Contains 2");
				}
			}
		}

		[Test]
		public void OperatorPlus_BothEmpty ()
		{
			using (var first = new NSMutableSet<NSString> ())
			using (var second = new NSMutableSet<NSString> ())
			using (var result = first + second) {
				Assert.That (result, Is.Not.Null, "Result should not be null");
				Assert.That (result.Count, Is.EqualTo ((nuint) 0), "Count should be 0");
			}
		}

		[Test]
		public void OperatorPlus_FirstEmpty ()
		{
			using (var first = new NSMutableSet<NSString> ())
			using (var second = new NSMutableSet<NSString> ((NSString) "1", (NSString) "2"))
			using (var result = first + second) {
				Assert.That (result, Is.Not.Null, "Result should not be null");
				Assert.That (result.Count, Is.EqualTo ((nuint) 2), "Count");
				Assert.That (result.Contains ((NSString) "1"), Is.True, "Contains 1");
				Assert.That (result.Contains ((NSString) "2"), Is.True, "Contains 2");
			}
		}

		[Test]
		public void OperatorPlus_SecondEmpty ()
		{
			using (var first = new NSMutableSet<NSString> ((NSString) "1", (NSString) "2"))
			using (var second = new NSMutableSet<NSString> ())
			using (var result = first + second) {
				Assert.That (result, Is.Not.Null, "Result should not be null");
				Assert.That (result.Count, Is.EqualTo ((nuint) 2), "Count");
				Assert.That (result.Contains ((NSString) "1"), Is.True, "Contains 1");
				Assert.That (result.Contains ((NSString) "2"), Is.True, "Contains 2");
			}
		}

		[Test]
		public void OperatorPlus_Overlapping ()
		{
			using (var first = new NSMutableSet<NSString> ((NSString) "1", (NSString) "2"))
			using (var second = new NSMutableSet<NSString> ((NSString) "2", (NSString) "3"))
			using (var result = first + second) {
				Assert.That (result, Is.Not.Null, "Result should not be null");
				Assert.That (result.Count, Is.EqualTo ((nuint) 3), "Count should be 3 (set union)");
				Assert.That (result.Contains ((NSString) "1"), Is.True, "Contains 1");
				Assert.That (result.Contains ((NSString) "2"), Is.True, "Contains 2");
				Assert.That (result.Contains ((NSString) "3"), Is.True, "Contains 3");
			}
		}

		[Test]
		public void OperatorMinus_BothNull ()
		{
			NSMutableSet<NSString> first = null;
			NSMutableSet<NSString> second = null;
			var result = first - second;
			Assert.That (result, Is.Null, "Both null should return null");
		}

		[Test]
		public void OperatorMinus_FirstNull ()
		{
			NSMutableSet<NSString> first = null;
			using (var second = new NSMutableSet<NSString> ((NSString) "1", (NSString) "2")) {
				var result = first - second;
				Assert.That (result, Is.Null, "First null should return null");
			}
		}

		[Test]
		public void OperatorMinus_SecondNull ()
		{
			using (var first = new NSMutableSet<NSString> ((NSString) "1", (NSString) "2")) {
				NSMutableSet<NSString> second = null;
				using (var result = first - second) {
					Assert.That (result, Is.Not.Null, "Result should not be null");
					Assert.That (result.Count, Is.EqualTo ((nuint) 2), "Count");
					Assert.That (result.Contains ((NSString) "1"), Is.True, "Contains 1");
					Assert.That (result.Contains ((NSString) "2"), Is.True, "Contains 2");
				}
			}
		}

		[Test]
		public void OperatorMinus_FirstEmpty ()
		{
			using (var first = new NSMutableSet<NSString> ())
			using (var second = new NSMutableSet<NSString> ((NSString) "1", (NSString) "2")) {
				var result = first - second;
				Assert.That (result, Is.Null, "Empty first should return null");
			}
		}

		[Test]
		public void OperatorMinus_SecondEmpty ()
		{
			using (var first = new NSMutableSet<NSString> ((NSString) "1", (NSString) "2"))
			using (var second = new NSMutableSet<NSString> ())
			using (var result = first - second) {
				Assert.That (result, Is.Not.Null, "Result should not be null");
				Assert.That (result.Count, Is.EqualTo ((nuint) 2), "Count");
				Assert.That (result.Contains ((NSString) "1"), Is.True, "Contains 1");
				Assert.That (result.Contains ((NSString) "2"), Is.True, "Contains 2");
			}
		}

		[Test]
		public void OperatorMinus_BothEmpty ()
		{
			using (var first = new NSMutableSet<NSString> ())
			using (var second = new NSMutableSet<NSString> ()) {
				var result = first - second;
				Assert.That (result, Is.Null, "Both empty should return null");
			}
		}

		[Test]
		public void OperatorMinus_NoOverlap ()
		{
			using (var first = new NSMutableSet<NSString> ((NSString) "1", (NSString) "2"))
			using (var second = new NSMutableSet<NSString> ((NSString) "3", (NSString) "4"))
			using (var result = first - second) {
				Assert.That (result, Is.Not.Null, "Result should not be null");
				Assert.That (result.Count, Is.EqualTo ((nuint) 2), "Count");
				Assert.That (result.Contains ((NSString) "1"), Is.True, "Contains 1");
				Assert.That (result.Contains ((NSString) "2"), Is.True, "Contains 2");
			}
		}

		[Test]
		public void OperatorMinus_PartialOverlap ()
		{
			using (var first = new NSMutableSet<NSString> ((NSString) "1", (NSString) "2", (NSString) "3"))
			using (var second = new NSMutableSet<NSString> ((NSString) "2", (NSString) "4"))
			using (var result = first - second) {
				Assert.That (result, Is.Not.Null, "Result should not be null");
				Assert.That (result.Count, Is.EqualTo ((nuint) 2), "Count");
				Assert.That (result.Contains ((NSString) "1"), Is.True, "Contains 1");
				Assert.That (result.Contains ((NSString) "3"), Is.True, "Contains 3");
				Assert.That (result.Contains ((NSString) "2"), Is.False, "Should not contain 2");
			}
		}

		[Test]
		public void OperatorMinus_CompleteOverlap ()
		{
			using (var first = new NSMutableSet<NSString> ((NSString) "1", (NSString) "2"))
			using (var second = new NSMutableSet<NSString> ((NSString) "1", (NSString) "2"))
			using (var result = first - second) {
				Assert.That (result, Is.Not.Null, "Result should not be null");
				Assert.That (result.Count, Is.EqualTo ((nuint) 0), "Count should be 0");
			}
		}

		[Test]
		public void Ctor_Capacity ()
		{
			using (var set = new NSMutableSet<NSString> (10)) {
				Assert.That (set.Count, Is.EqualTo ((nuint) 0), "Empty with capacity");
			}
		}

		[Test]
		public void ToArray_Empty ()
		{
			using (var set = new NSMutableSet<NSString> ()) {
				var arr = set.ToArray ();
				Assert.That (arr, Is.Not.Null, "Array should not be null");
				Assert.That (arr.Length, Is.EqualTo (0), "Length should be 0");
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
				Assert.That (arr.Length, Is.EqualTo (3), "Length");
				Assert.That (arr, Has.Member (v1), "Contains v1");
				Assert.That (arr, Has.Member (v2), "Contains v2");
				Assert.That (arr, Has.Member (v3), "Contains v3");
			}
		}

		[Test]
		public void Add_Duplicate ()
		{
			var v1 = (NSString) "1";

			using (var set = new NSMutableSet<NSString> ()) {
				set.Add (v1);
				Assert.That (set.Count, Is.EqualTo ((nuint) 1), "Count after first add");

				set.Add (v1);
				Assert.That (set.Count, Is.EqualTo ((nuint) 1), "Count after duplicate add");
			}
		}

		[Test]
		public void Remove_NonExistent ()
		{
			var v1 = (NSString) "1";
			var v2 = (NSString) "2";

			using (var set = new NSMutableSet<NSString> (v1)) {
				set.Remove (v2);
				Assert.That (set.Count, Is.EqualTo ((nuint) 1), "Count should remain 1");
				Assert.That (set.Contains (v1), Is.True, "Should still contain v1");
			}
		}

		[Test]
		public void AddObjects_Empty ()
		{
			using (var set = new NSMutableSet<NSString> ()) {
				set.AddObjects ();
				Assert.That (set.Count, Is.EqualTo ((nuint) 0), "Count should be 0");
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
				Assert.That (result, Is.Null, "Should return null for empty set");
			}
		}

		[Test]
		public void Contains_Empty ()
		{
			var v1 = (NSString) "1";

			using (var set = new NSMutableSet<NSString> ()) {
				Assert.That (set.Contains (v1), Is.False, "Empty set should not contain any element");
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
				Assert.That (count, Is.EqualTo (0), "Should not enumerate any items");
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
				Assert.That (count, Is.EqualTo (1), "Should enumerate one item");
				Assert.That (found, Is.SameAs (v1), "Should find v1");
			}
		}

		[Test]
		public void Ctor_Params_Empty ()
		{
			using (var set = new NSMutableSet<NSString> ()) {
				Assert.That (set.Count, Is.EqualTo ((nuint) 0), "Empty params");
			}
		}
	}
}
