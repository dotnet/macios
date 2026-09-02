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
				Assert.That (arr.Count, Is.EqualTo ((nuint) 0), "Count");
			}
		}

		[Test]
		public void Ctor_Params ()
		{
			using (var arr = new NSSet<NSString> ((NSString) "foo")) {
				Assert.That (arr.Count, Is.EqualTo ((nuint) 1), "Count");
			}
			using (var arr = new NSSet<NSString> ((NSString) "foo", (NSString) "bar")) {
				Assert.That (arr.Count, Is.EqualTo ((nuint) 2), "Count");
			}
		}

		[Test]
		public void Ctor_OtherSet ()
		{
			var v1 = (NSString) "1";

			using (var first = new NSSet<NSString> (v1)) {
				using (var second = new NSSet<NSString> (first)) {
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
				using (var second = new NSSet<NSString> (first)) {
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

			using (var st = new NSSet<NSString> (v1)) {
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

			using (var st = new NSSet<NSString> ()) {
				Assert.That (st.AnyObject, Is.Null, "AnyObject 1");
			}

			using (var st = new NSSet<NSString> (v1)) {
				Assert.That (st.AnyObject, Is.SameAs (v1), "AnyObject 2");
			}
		}

		[Test]
		public void ContainsTest ()
		{
			var v1 = (NSString) "1";
			var v2 = (NSString) "2";

			using (var st = new NSSet<NSString> (v1)) {
				Assert.Throws<ArgumentNullException> (() => st.Contains ((NSString) null), "Contains ANE 1");
				Assert.That (st.Contains (v1), Is.True, "Contains 1");
				Assert.That (st.Contains (v2), Is.False, "Contains 2");
			}
		}

		[Test]
		public void ToArrayTest ()
		{
			var v1 = (NSString) "1";

			using (var st = new NSSet<NSString> (v1)) {
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

			using (var first = new NSSet<NSString> (v1)) {
				using (var second = new NSSet<NSString> (v2)) {
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

			using (var first = new NSSet<NSString> (v1, v2)) {
				using (var second = new NSSet<NSString> (v2)) {
					using (var third = first - second) {
						Assert.That (third.Count, Is.EqualTo ((nuint) 1), "- Count");
						Assert.That (third.Contains (v1), Is.True, "- 1");
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
			Assert.That (result1, Is.Null, "null + null");

			// First null, second non-null -> copy of second
			using (var second = new NSSet<NSString> (v2)) {
				using (var result2 = nullSet + second) {
					Assert.That (result2, Is.Not.Null, "null + non-null");
					Assert.That (result2.Count, Is.EqualTo ((nuint) 1), "null + non-null Count");
					Assert.That (result2.Contains (v2), Is.True, "null + non-null contains");
				}
			}

			// First non-null, second null -> copy of first
			using (var first = new NSSet<NSString> (v1)) {
				using (var result3 = first + nullSet) {
					Assert.That (result3, Is.Not.Null, "non-null + null");
					Assert.That (result3.Count, Is.EqualTo ((nuint) 1), "non-null + null Count");
					Assert.That (result3.Contains (v1), Is.True, "non-null + null contains");
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
						Assert.That (result, Is.Not.Null, "empty + non-empty");
						Assert.That (result.Count, Is.EqualTo ((nuint) 1), "empty + non-empty Count");
						Assert.That (result.Contains (v2), Is.True, "empty + non-empty contains");
					}
				}
			}

			// First non-empty, second empty -> copy of first
			using (var first = new NSSet<NSString> (v1)) {
				using (var second = new NSSet<NSString> ()) {
					using (var result = first + second) {
						Assert.That (result, Is.Not.Null, "non-empty + empty");
						Assert.That (result.Count, Is.EqualTo ((nuint) 1), "non-empty + empty Count");
						Assert.That (result.Contains (v1), Is.True, "non-empty + empty contains");
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
			Assert.That (result1, Is.Null, "null - null");

			// null - non-null -> null
			using (var second = new NSSet<NSString> (v2)) {
				var result2 = nullSet - second;
				Assert.That (result2, Is.Null, "null - non-null");
			}

			// non-null - null -> copy of first
			using (var first = new NSSet<NSString> (v1, v2)) {
				using (var result3 = first - nullSet) {
					Assert.That (result3, Is.Not.Null, "non-null - null");
					Assert.That (result3.Count, Is.EqualTo ((nuint) 2), "non-null - null Count");
					Assert.That (result3.Contains (v1), Is.True, "non-null - null contains v1");
					Assert.That (result3.Contains (v2), Is.True, "non-null - null contains v2");
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
					Assert.That (result, Is.Null, "empty - non-empty");
				}
			}

			// non-empty - empty -> copy of first
			using (var first = new NSSet<NSString> (v1, v2)) {
				using (var second = new NSSet<NSString> ()) {
					using (var result = first - second) {
						Assert.That (result, Is.Not.Null, "non-empty - empty");
						Assert.That (result.Count, Is.EqualTo ((nuint) 2), "non-empty - empty Count");
						Assert.That (result.Contains (v1), Is.True, "non-empty - empty contains v1");
						Assert.That (result.Contains (v2), Is.True, "non-empty - empty contains v2");
					}
				}
			}

			// Result is empty after subtraction -> null
			using (var first = new NSSet<NSString> (v1)) {
				using (var second = new NSSet<NSString> (v1)) {
					var result = first - second;
					Assert.That (result, Is.Not.Null, "result is not null");
					Assert.That (result.Count, Is.EqualTo ((nuint) 0), "result is empty");
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
			var one = new NSSet<NSString> ((NSString) "1", (NSString) "2", (NSString) "3");
			var two = new NSSet<NSString> ((NSString) "4", (NSString) "5", (NSString) "6");
			NSSet<NSString> nil = null;
			using (var sum = one + nil)
			using (var sum2 = two + one)
			using (var sum3 = one + two) {

			}
			Assert.That (one.Handle, Is.Not.EqualTo (IntPtr.Zero), "Handle must be != IntPtr.Zero");
			Assert.That (two.Handle, Is.Not.EqualTo (IntPtr.Zero), "Handle must be != IntPtr.Zero");
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
