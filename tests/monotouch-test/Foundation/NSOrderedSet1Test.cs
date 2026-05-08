//
// Unit tests for NSOrderedSet Generic support
//
// Authors:
//	Alex Soto <alex.soto@xamarin.com>
//	
//
// Copyright 2015 Xamarin Inc. All rights reserved.
//

using System.Collections;

namespace MonoTouchFixtures.Foundation {

	[TestFixture]
	[Preserve (AllMembers = true)]
	public class NSOrderedSet1Test {

		[Test]
		public void Ctor ()
		{
			var oset = new NSOrderedSet<NSData> ();

			Assert.That (oset.Count, Is.EqualTo ((nint) 0), "NSOrderedSet1Test Count");
		}

		[Test]
		public void Ctor_Start ()
		{
			var oSet = new NSOrderedSet<NSString> (start: (NSString) "foo");

			Assert.That (oSet.Count, Is.EqualTo ((nint) 1), "NSOrderedSet1Test Count");
		}

		[Test]
		public void Ctor_Params ()
		{
			var oSet = new NSOrderedSet<NSString> ((NSString) "foo", (NSString) "bar", (NSString) "xyz");

			Assert.That (oSet.Count, Is.EqualTo ((nint) 3), "NSOrderedSet1Test Count");
		}

		[Test]
		public void Ctor_NSSet ()
		{
			var set = new NSSet<NSString> ((NSString) "foo", (NSString) "bar", (NSString) "xyz");
			var oSet = new NSOrderedSet<NSString> (set);

			Assert.That (oSet.Count, Is.EqualTo ((nint) set.Count), "NSOrderedSet1Test Count");
		}

		[Test]
		public void Ctor_NSOrderedSet ()
		{
			var oSetSource = new NSOrderedSet<NSString> ((NSString) "foo", (NSString) "bar", (NSString) "xyz");
			var oSet = new NSOrderedSet<NSString> (oSetSource);

			Assert.That (oSet.Count, Is.EqualTo ((nint) oSetSource.Count), "NSOrderedSet1Test Count");
		}

		[Test]
		public void Ctor_NSMutableOrderedSet ()
		{
			var oMutableSet = new NSMutableOrderedSet<NSString> ((NSString) "foo", (NSString) "bar", (NSString) "xyz");
			var oSet = new NSOrderedSet<NSString> (oMutableSet);

			Assert.That (oSet.Count, Is.EqualTo (oMutableSet.Count), "NSOrderedSet1Test Count");
		}

		[Test]
		public void IndexerTest ()
		{
			var str1 = (NSString) "1";
			var str2 = (NSString) "2";
			var str3 = (NSString) "3";
			var oSet = new NSOrderedSet<NSString> (str1, str2, str3);

			Assert.That (oSet.Count, Is.EqualTo ((nint) 3), "NSOrderedSet1Test Count");
			Assert.That (oSet [1], Is.SameAs (str2), "NSOrderedSet1Test IndexOf");
		}

		[Test]
		public void ToArrayTest ()
		{
			var str = (NSString) "Test";
			var oSet = new NSOrderedSet<NSString> (str);
			var arr = oSet.ToArray ();

			Assert.That (arr.Length, Is.EqualTo (1), "NSOrderedSet1Test ToArray Length");
			Assert.That (arr [0], Is.SameAs (str), "NSOrderedSet1Test ToArray () [0]");
		}

		[Test]
		public void ContainsTest ()
		{
			var str1 = (NSString) "1";
			var str2 = (NSString) "2";
			var oSet = new NSOrderedSet<NSString> (str1);

			Assert.Throws<ArgumentNullException> (() => oSet.Contains ((NSString) null), "NSOrderedSet1Test Contains str1");
			Assert.That (oSet.Contains (str1), Is.True, "NSOrderedSet1Test Contains str1");
			Assert.That (oSet.Contains (str2), Is.False, "NSOrderedSet1Test Does not Contains str2");
		}

		[Test]
		public void IndexOfTest ()
		{
			var str1 = (NSString) "1";
			var str2 = (NSString) "2";
			var str3 = (NSString) "3";
			var oSet = new NSOrderedSet<NSString> (str1, str2, str3);

			Assert.That (oSet.Count, Is.EqualTo ((nint) 3), "NSOrderedSet1Test Count");
			Assert.That (oSet.IndexOf (str2), Is.EqualTo ((nint) 1), "NSOrderedSet1Test IndexOf");
		}

		[Test]
		public void FirstObjectTest ()
		{
			var str1 = (NSString) "1";
			var str2 = (NSString) "2";
			var str3 = (NSString) "3";
			var oSet = new NSOrderedSet<NSString> (str1, str2, str3);

			Assert.That (oSet.Count, Is.EqualTo ((nint) 3), "NSOrderedSet1Test Count");
			Assert.That (oSet.FirstObject (), Is.SameAs (str1), "NSOrderedSet1Test IndexOf");
		}

		[Test]
		public void LastObjectTest ()
		{
			var str1 = (NSString) "1";
			var str2 = (NSString) "2";
			var str3 = (NSString) "3";
			var oSet = new NSOrderedSet<NSString> (str1, str2, str3);

			Assert.That (oSet.Count, Is.EqualTo ((nint) 3), "NSOrderedSet1Test Count");
			Assert.That (oSet.LastObject (), Is.SameAs (str3), "NSOrderedSet1Test IndexOf");
		}

		[Test]
		public void AsSetTest ()
		{
			var str1 = (NSString) "1";
			var str2 = (NSString) "2";
			var str3 = (NSString) "3";
			var oSet = new NSOrderedSet<NSString> (str1, str2, str3);
			NSSet<NSString> set = oSet.AsSet ();

			Assert.That (oSet.Count, Is.EqualTo ((nint) 3), "NSOrderedSet1Test Count");
			Assert.That (set.Count, Is.EqualTo ((nuint) 3), "NSOrderedSet1Test Count");
			Assert.That (set.LookupMember (str3), Is.SameAs (str3), "NSOrderedSet1Test IndexOf");
		}

		[Test]
		public void IEnumerable1Test ()
		{
			const int C = 16 * 2 + 3; // NSFastEnumerator has a array of size 16, use more than that, and not an exact multiple.
			var values = new NSString [C];
			for (int i = 0; i < C; i++)
				values [i] = (NSString) i.ToString ();

			var st = new NSOrderedSet<NSString> (values);
			Assert.That (st.Count, Is.EqualTo ((nint) C), "Count 1");

			var lst = new List<NSString> ();
			foreach (var a in (IEnumerable<NSString>) st) {
				Assert.That (a, Is.Not.Null, "null item iterator");
				Assert.That (lst.Contains (a), Is.False, "duplicated item iterator");
				lst.Add (a);
				Assert.That (Array.IndexOf (values, a) >= 0, Is.True, "different object");
			}
			Assert.That (lst.Count, Is.EqualTo (C), "iterator count");
		}

		[Test]
		public void IEnumerable1Test_EnumeratorType ()
		{
			var myEnumerable = new NSOrderedSet<NSNumber> ();
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

			var st = new NSOrderedSet<NSString> (values);
			Assert.That (st.Count, Is.EqualTo ((nint) C), "Count 1");

			var lst = new List<NSString> ();
			foreach (NSString a in (IEnumerable) st) {
				Assert.That (a, Is.Not.Null, "null item iterator");
				Assert.That (lst.Contains (a), Is.False, "duplicated item iterator");
				lst.Add (a);
				Assert.That (Array.IndexOf (values, a) >= 0, Is.True, "different object");
			}
			Assert.That (lst.Count, Is.EqualTo (C), "iterator count");
		}

		[Test]
		public void OperatorEqualTest ()
		{
			var str1 = (NSString) "1";
			var str2 = (NSString) "2";
			var str3 = (NSString) "3";
			var oSet = new NSOrderedSet<NSString> (str1, str2, str3);
			var oSet2 = new NSOrderedSet<NSString> (str1, str2, str3);

			Assert.That (oSet == oSet2, Is.True, "NSOrderedSet1Test == must be true");
			Assert.That (oSet.Equals (oSet2), Is.True, "NSOrderedSet1Test Equals must be true");
		}

		[Test]
		public void OperatorDifferentTest ()
		{
			var str1 = (NSString) "1";
			var str2 = (NSString) "2";
			var str3 = (NSString) "3";
			var oSet = new NSOrderedSet<NSString> (str1, str2, str3);
			var oSet2 = new NSOrderedSet<NSString> (str3, str2, str1);

			Assert.That (oSet != oSet2, Is.True, "NSOrderedSet1Test != must be true");
			Assert.That (oSet.Equals (oSet2), Is.False, "NSOrderedSet1Test Equals must be false");
		}

		[Test]
		public void OperatorAddTest ()
		{
			var str1 = (NSString) "1";
			var str2 = (NSString) "2";
			var str3 = (NSString) "3";
			var str4 = (NSString) "4";

			var first = new NSOrderedSet<NSString> (str1, str2);
			var second = new NSOrderedSet<NSString> (str3, str4);
			var third = first + second;
			Assert.That (third.Count, Is.EqualTo ((nint) 4), "OperatorAdd Count");
			Assert.That (third.Contains (str1), Is.True, "OperatorAdd 1");
			Assert.That (third.Contains (str2), Is.True, "OperatorAdd 2");
			Assert.That (third.Contains (str3), Is.True, "OperatorAdd 3");
			Assert.That (third.Contains (str4), Is.True, "OperatorAdd 4");
		}

		[Test]
		public void OperatorAddTest2 ()
		{
			var str1 = (NSString) "1";
			var str2 = (NSString) "2";
			var str3 = (NSString) "3";
			var str4 = (NSString) "4";

			var first = new NSOrderedSet<NSString> (str1, str2);
			var second = new NSSet<NSString> (str3, str4);
			var third = first + second;
			Assert.That (third.Count, Is.EqualTo ((nint) 4), "OperatorAdd Count");
			Assert.That (third.Contains (str1), Is.True, "OperatorAdd 1");
			Assert.That (third.Contains (str2), Is.True, "OperatorAdd 2");
			Assert.That (third.Contains (str3), Is.True, "OperatorAdd 3");
			Assert.That (third.Contains (str4), Is.True, "OperatorAdd 4");
		}

		[Test]
		public void OperatorSubtractTest ()
		{
			var str1 = (NSString) "1";
			var str2 = (NSString) "2";
			var str3 = (NSString) "3";
			var str4 = (NSString) "4";

			var first = new NSOrderedSet<NSString> (str1, str2, str3, str4);
			var second = new NSOrderedSet<NSString> (str3, str4);
			var third = first - second;

			Assert.That (third.Count, Is.EqualTo ((nint) 2), "OperatorSubtract Count");
			Assert.That (third.Contains (str1), Is.True, "OperatorSubtract 1");
			Assert.That (third.Contains (str2), Is.True, "OperatorSubtract 2");
			Assert.That (third.Contains (str3), Is.False, "OperatorSubtract 3");
			Assert.That (third.Contains (str4), Is.False, "OperatorSubtract 4");
		}

		[Test]
		public void OperatorSubtractTest2 ()
		{
			var str1 = (NSString) "1";
			var str2 = (NSString) "2";
			var str3 = (NSString) "3";
			var str4 = (NSString) "4";

			var first = new NSOrderedSet<NSString> (str1, str2, str3, str4);
			var second = new NSSet<NSString> (str3, str4);
			var third = first - second;

			Assert.That (third.Count, Is.EqualTo ((nint) 2), "OperatorSubtract2 Count");
			Assert.That (third.Contains (str1), Is.True, "OperatorSubtract2 1");
			Assert.That (third.Contains (str2), Is.True, "OperatorSubtract2 2");
			Assert.That (third.Contains (str3), Is.False, "OperatorSubtract2 3");
			Assert.That (third.Contains (str4), Is.False, "OperatorSubtract2 4");
		}

		[Test]
		public void OperatorPlusReferenceTest ()
		{
			var str1 = (NSString) "1";
			var str2 = (NSString) "2";
			var str3 = (NSString) "3";
			var str4 = (NSString) "4";
			var str5 = (NSString) "5";
			var str6 = (NSString) "6";

			var one = new NSOrderedSet<NSString> (str1, str2, str3);
			var two = new NSOrderedSet<NSString> (str4, str5, str6);
			NSOrderedSet<NSString> nil = null;
			using (var sum = one + nil)
			using (var sum2 = two + one)
			using (var sum3 = one + two) {

			}
			Assert.That (one.Handle, Is.Not.EqualTo (IntPtr.Zero), "Handle must be != IntPtr.Zero");
			Assert.That (two.Handle, Is.Not.EqualTo (IntPtr.Zero), "Handle must be != IntPtr.Zero");
		}
	}
}
