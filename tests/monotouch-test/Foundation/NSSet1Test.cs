using System.Collections;
using System.Linq;

namespace MonoTouchFixtures.Foundation {

	[TestFixture]
	[Preserve (AllMembers = true)]
	public class NSSet1Test {

		[Test]
		public void Ctor ()
		{
			using (var arr = new NSSet<NSDate> ()) {
				ClassicAssert.AreEqual ((nuint) 0, arr.Count, "Count");
			}
		}

		[Test]
		public void Ctor_Params ()
		{
			using (var arr = new NSSet<NSString> ((NSString) "foo")) {
				ClassicAssert.AreEqual ((nuint) 1, arr.Count, "Count");
			}
			using (var arr = new NSSet<NSString> ((NSString) "foo", (NSString) "bar")) {
				ClassicAssert.AreEqual ((nuint) 2, arr.Count, "Count");
			}
		}

		[Test]
		public void Ctor_OtherSet ()
		{
			var v1 = (NSString) "1";

			using (var first = new NSSet<NSString> (v1)) {
				using (var second = new NSSet<NSString> (first)) {
					ClassicAssert.AreEqual ((nuint) 1, first.Count, "1 count");
					ClassicAssert.AreEqual ((nuint) 1, second.Count, "2 count");
				}
			}
		}

		[Test]
		public void Ctor_OtherMutableSet ()
		{
			var v1 = (NSString) "1";

			using (var first = new NSMutableSet<NSString> (v1)) {
				using (var second = new NSSet<NSString> (first)) {
					ClassicAssert.AreEqual ((nuint) 1, first.Count, "1 count");
					ClassicAssert.AreEqual ((nuint) 1, second.Count, "2 count");
				}
			}
		}

		[Test]
		public void LookupMemberTest ()
		{
			var v1 = (NSString) "1";
			var v2 = (NSString) "2";

			using (var st = new NSSet<NSString> (v1)) {
				Assert.Throws<ArgumentNullException> (() => st.LookupMember ((NSString) null), "LookupMember ANE 1");
				ClassicAssert.AreSame (v1, st.LookupMember (v1), "LookupMember 1");
				ClassicAssert.IsNull (st.LookupMember (v2), "LookupMember 2");
			}
		}

		[Test]
		public void AnyObjectTest ()
		{
			var v1 = (NSString) "1";
			var v2 = (NSString) "2";

			using (var st = new NSSet<NSString> ()) {
				ClassicAssert.IsNull (st.AnyObject, "AnyObject 1");
			}

			using (var st = new NSSet<NSString> (v1)) {
				ClassicAssert.AreSame (v1, st.AnyObject, "AnyObject 2");
			}
		}

		[Test]
		public void ContainsTest ()
		{
			var v1 = (NSString) "1";
			var v2 = (NSString) "2";

			using (var st = new NSSet<NSString> (v1)) {
				Assert.Throws<ArgumentNullException> (() => st.Contains ((NSString) null), "Contains ANE 1");
				ClassicAssert.IsTrue (st.Contains (v1), "Contains 1");
				ClassicAssert.IsFalse (st.Contains (v2), "Contains 2");
			}
		}

		[Test]
		public void ToArrayTest ()
		{
			var v1 = (NSString) "1";

			using (var st = new NSSet<NSString> (v1)) {
				var arr = st.ToArray ();
				ClassicAssert.AreEqual (1, arr.Length, "ToArray Length");
				ClassicAssert.AreSame (v1, arr [0], "ToArray () [0]");
			}
		}

		[Test]
		public void OperatorAddTest ()
		{
			var v1 = (NSString) "1";
			var v2 = (NSString) "2";

			using (var first = new NSSet<NSString> (v1)) {
				using (var second = new NSSet<NSString> (v2)) {
					using (var third = first + second) {
						ClassicAssert.AreEqual ((nuint) 2, third.Count, "+ Count");
						ClassicAssert.IsTrue (third.Contains (v1), "+ 1");
						ClassicAssert.IsTrue (third.Contains (v2), "+ 2");
					}
				}
			}
		}

		[Test]
		public void OperatorSubtractTest ()
		{
			var v1 = (NSString) "1";
			var v2 = (NSString) "2";

			using (var first = new NSSet<NSString> (v1, v2)) {
				using (var second = new NSSet<NSString> (v2)) {
					using (var third = first - second) {
						ClassicAssert.AreEqual ((nuint) 1, third.Count, "- Count");
						ClassicAssert.IsTrue (third.Contains (v1), "- 1");
					}
				}
			}
		}

		[Test]
		public void OperatorAddNullTest ()
		{
			var v1 = (NSString) "1";
			var v2 = (NSString) "2";
			NSSet<NSString> nullSet = null;

			// Both null -> null
			var result1 = nullSet + nullSet;
			ClassicAssert.IsNull (result1, "null + null");

			// First null, second non-null -> copy of second
			using (var second = new NSSet<NSString> (v2)) {
				using (var result2 = nullSet + second) {
					ClassicAssert.IsNotNull (result2, "null + non-null");
					ClassicAssert.AreEqual ((nuint) 1, result2.Count, "null + non-null Count");
					ClassicAssert.IsTrue (result2.Contains (v2), "null + non-null contains");
				}
			}

			// First non-null, second null -> copy of first
			using (var first = new NSSet<NSString> (v1)) {
				using (var result3 = first + nullSet) {
					ClassicAssert.IsNotNull (result3, "non-null + null");
					ClassicAssert.AreEqual ((nuint) 1, result3.Count, "non-null + null Count");
					ClassicAssert.IsTrue (result3.Contains (v1), "non-null + null contains");
				}
			}
		}

		[Test]
		public void OperatorAddEmptyTest ()
		{
			var v1 = (NSString) "1";
			var v2 = (NSString) "2";

			// First empty, second non-empty -> copy of second
			using (var first = new NSSet<NSString> ()) {
				using (var second = new NSSet<NSString> (v2)) {
					using (var result = first + second) {
						ClassicAssert.IsNotNull (result, "empty + non-empty");
						ClassicAssert.AreEqual ((nuint) 1, result.Count, "empty + non-empty Count");
						ClassicAssert.IsTrue (result.Contains (v2), "empty + non-empty contains");
					}
				}
			}

			// First non-empty, second empty -> copy of first
			using (var first = new NSSet<NSString> (v1)) {
				using (var second = new NSSet<NSString> ()) {
					using (var result = first + second) {
						ClassicAssert.IsNotNull (result, "non-empty + empty");
						ClassicAssert.AreEqual ((nuint) 1, result.Count, "non-empty + empty Count");
						ClassicAssert.IsTrue (result.Contains (v1), "non-empty + empty contains");
					}
				}
			}
		}

		[Test]
		public void OperatorSubtractNullTest ()
		{
			var v1 = (NSString) "1";
			var v2 = (NSString) "2";
			NSSet<NSString> nullSet = null;

			// null - null -> null
			var result1 = nullSet - nullSet;
			ClassicAssert.IsNull (result1, "null - null");

			// null - non-null -> null
			using (var second = new NSSet<NSString> (v2)) {
				var result2 = nullSet - second;
				ClassicAssert.IsNull (result2, "null - non-null");
			}

			// non-null - null -> copy of first
			using (var first = new NSSet<NSString> (v1, v2)) {
				using (var result3 = first - nullSet) {
					ClassicAssert.IsNotNull (result3, "non-null - null");
					ClassicAssert.AreEqual ((nuint) 2, result3.Count, "non-null - null Count");
					ClassicAssert.IsTrue (result3.Contains (v1), "non-null - null contains v1");
					ClassicAssert.IsTrue (result3.Contains (v2), "non-null - null contains v2");
				}
			}
		}

		[Test]
		public void OperatorSubtractEmptyTest ()
		{
			var v1 = (NSString) "1";
			var v2 = (NSString) "2";

			// empty - non-empty -> null
			using (var first = new NSSet<NSString> ()) {
				using (var second = new NSSet<NSString> (v2)) {
					var result = first - second;
					ClassicAssert.IsNull (result, "empty - non-empty");
				}
			}

			// non-empty - empty -> copy of first
			using (var first = new NSSet<NSString> (v1, v2)) {
				using (var second = new NSSet<NSString> ()) {
					using (var result = first - second) {
						ClassicAssert.IsNotNull (result, "non-empty - empty");
						ClassicAssert.AreEqual ((nuint) 2, result.Count, "non-empty - empty Count");
						ClassicAssert.IsTrue (result.Contains (v1), "non-empty - empty contains v1");
						ClassicAssert.IsTrue (result.Contains (v2), "non-empty - empty contains v2");
					}
				}
			}

			// Result is empty after subtraction -> null
			using (var first = new NSSet<NSString> (v1)) {
				using (var second = new NSSet<NSString> (v1)) {
					var result = first - second;
					ClassicAssert.IsNotNull (result, "result is not null");
					ClassicAssert.AreEqual ((nuint) 0, result.Count, "result is empty");
				}
			}
		}

		[Test]
		public void IEnumerable1Test ()
		{
			const int C = 16 * 2 + 3; // NSFastEnumerator has a array of size 16, use more than that, and not an exact multiple.
			var values = new NSString [C];
			for (int i = 0; i < C; i++)
				values [i] = (NSString) i.ToString ();

			using (var st = new NSSet<NSString> (values)) {
				ClassicAssert.AreEqual ((nuint) C, st.Count, "Count 1");

				var lst = new List<NSString> ();
				foreach (var a in (IEnumerable<NSString>) st) {
					ClassicAssert.IsNotNull (a, "null item iterator");
					ClassicAssert.IsFalse (lst.Contains (a), "duplicated item iterator");
					lst.Add (a);
					ClassicAssert.IsTrue (Array.IndexOf (values, a) >= 0, "different object");
				}
				ClassicAssert.AreEqual (C, lst.Count, "iterator count");
			}
		}

		[Test]
		public void IEnumerable1Test_EnumeratorType ()
		{
			var myEnumerable = new NSSet<NSNumber> ();
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

			using (var st = new NSSet<NSString> (values)) {
				ClassicAssert.AreEqual ((nuint) C, st.Count, "Count 1");

				var lst = new List<NSString> ();
				foreach (NSString a in (IEnumerable) st) {
					ClassicAssert.IsNotNull (a, "null item iterator");
					ClassicAssert.IsFalse (lst.Contains (a), "duplicated item iterator");
					lst.Add (a);
					ClassicAssert.IsTrue (Array.IndexOf (values, a) >= 0, "different object");
				}
				ClassicAssert.AreEqual (C, lst.Count, "iterator count");
			}
		}

		[Test]
		public void OperatorPlusReferenceTest ()
		{
			var one = new NSSet<NSString> ((NSString) "1", (NSString) "2", (NSString) "3");
			var two = new NSSet<NSString> ((NSString) "4", (NSString) "5", (NSString) "6");
			NSSet<NSString> nil = null;
			using (var sum = one + nil)
			using (var sum2 = two + one)
			using (var sum3 = one + two) {

			}
			ClassicAssert.AreNotEqual (IntPtr.Zero, one.Handle, "Handle must be != IntPtr.Zero");
			ClassicAssert.AreNotEqual (IntPtr.Zero, two.Handle, "Handle must be != IntPtr.Zero");
		}

		[Test]
		public void CreateTest ()
		{
			var input = new string [] { "1", "2", "3" };
			using var s = NSSet<NSString>.Create (input, (str) => (NSString) str);
			Assert.That (s.Count, Is.EqualTo ((nuint) 3), "Count");
			Assert.That (s.ToArray ().Select (v => v.ToString ()).OrderBy (v => v), Is.EqualTo (input), "Input");
		}

		[Test]
		public void ToHashSetTest ()
		{
			using var s = new NSSet<NSString> ((NSString) "1", (NSString) "2", (NSString) "3");
			{
				var hashSet = s.ToHashSet ((v) => v);
				var sorted = hashSet.OrderBy (v => v.ToString ()).ToArray ();
				Assert.That (sorted [0].ToString (), Is.EqualTo ("1"), "1 A");
				Assert.That (sorted [1].ToString (), Is.EqualTo ("2"), "2 A");
				Assert.That (sorted [2].ToString (), Is.EqualTo ("3"), "3 A");
			}

			{
				var hashSet = s.ToHashSet ((v) => v.ToString ());
				var sorted = hashSet.OrderBy (v => v).ToArray ();
				Assert.That (sorted [0], Is.EqualTo ("1"), "1 B");
				Assert.That (sorted [1], Is.EqualTo ("2"), "2 B");
				Assert.That (sorted [2], Is.EqualTo ("3"), "3 B");
			}
		}
	}
}
