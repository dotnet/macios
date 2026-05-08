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

			ClassicAssert.AreEqual ((nint) 0, oset.Count, "NSOrderedSet1Test Count");
		}

		[Test]
		public void Ctor_Start ()
		{
			var oSet = new NSOrderedSet<NSString> (start: (NSString) "foo");

			ClassicAssert.AreEqual ((nint) 1, oSet.Count, "NSOrderedSet1Test Count");
		}

		[Test]
		public void Ctor_Params ()
		{
			var oSet = new NSOrderedSet<NSString> ((NSString) "foo", (NSString) "bar", (NSString) "xyz");

			ClassicAssert.AreEqual ((nint) 3, oSet.Count, "NSOrderedSet1Test Count");
		}

		[Test]
		public void Ctor_NSSet ()
		{
			var set = new NSSet<NSString> ((NSString) "foo", (NSString) "bar", (NSString) "xyz");
			var oSet = new NSOrderedSet<NSString> (set);

			ClassicAssert.AreEqual ((nint) set.Count, oSet.Count, "NSOrderedSet1Test Count");
		}

		[Test]
		public void Ctor_NSOrderedSet ()
		{
			var oSetSource = new NSOrderedSet<NSString> ((NSString) "foo", (NSString) "bar", (NSString) "xyz");
			var oSet = new NSOrderedSet<NSString> (oSetSource);

			ClassicAssert.AreEqual ((nint) oSetSource.Count, oSet.Count, "NSOrderedSet1Test Count");
		}

		[Test]
		public void Ctor_NSMutableOrderedSet ()
		{
			var oMutableSet = new NSMutableOrderedSet<NSString> ((NSString) "foo", (NSString) "bar", (NSString) "xyz");
			var oSet = new NSOrderedSet<NSString> (oMutableSet);

			ClassicAssert.AreEqual (oMutableSet.Count, oSet.Count, "NSOrderedSet1Test Count");
		}

		[Test]
		public void IndexerTest ()
		{
			var str1 = (NSString) "1";
			var str2 = (NSString) "2";
			var str3 = (NSString) "3";
			var oSet = new NSOrderedSet<NSString> (str1, str2, str3);

			ClassicAssert.AreEqual ((nint) 3, oSet.Count, "NSOrderedSet1Test Count");
			ClassicAssert.AreSame (str2, oSet [1], "NSOrderedSet1Test IndexOf");
		}

		[Test]
		public void ToArrayTest ()
		{
			var str = (NSString) "Test";
			var oSet = new NSOrderedSet<NSString> (str);
			var arr = oSet.ToArray ();

			ClassicAssert.AreEqual (1, arr.Length, "NSOrderedSet1Test ToArray Length");
			ClassicAssert.AreSame (str, arr [0], "NSOrderedSet1Test ToArray () [0]");
		}

		[Test]
		public void ContainsTest ()
		{
			var str1 = (NSString) "1";
			var str2 = (NSString) "2";
			var oSet = new NSOrderedSet<NSString> (str1);

			Assert.Throws<ArgumentNullException> (() => oSet.Contains ((NSString) null), "NSOrderedSet1Test Contains str1");
			ClassicAssert.IsTrue (oSet.Contains (str1), "NSOrderedSet1Test Contains str1");
			ClassicAssert.IsFalse (oSet.Contains (str2), "NSOrderedSet1Test Does not Contains str2");
		}

		[Test]
		public void IndexOfTest ()
		{
			var str1 = (NSString) "1";
			var str2 = (NSString) "2";
			var str3 = (NSString) "3";
			var oSet = new NSOrderedSet<NSString> (str1, str2, str3);

			ClassicAssert.AreEqual ((nint) 3, oSet.Count, "NSOrderedSet1Test Count");
			ClassicAssert.AreEqual ((nint) 1, oSet.IndexOf (str2), "NSOrderedSet1Test IndexOf");
		}

		[Test]
		public void FirstObjectTest ()
		{
			var str1 = (NSString) "1";
			var str2 = (NSString) "2";
			var str3 = (NSString) "3";
			var oSet = new NSOrderedSet<NSString> (str1, str2, str3);

			ClassicAssert.AreEqual ((nint) 3, oSet.Count, "NSOrderedSet1Test Count");
			ClassicAssert.AreSame (str1, oSet.FirstObject (), "NSOrderedSet1Test IndexOf");
		}

		[Test]
		public void LastObjectTest ()
		{
			var str1 = (NSString) "1";
			var str2 = (NSString) "2";
			var str3 = (NSString) "3";
			var oSet = new NSOrderedSet<NSString> (str1, str2, str3);

			ClassicAssert.AreEqual ((nint) 3, oSet.Count, "NSOrderedSet1Test Count");
			ClassicAssert.AreSame (str3, oSet.LastObject (), "NSOrderedSet1Test IndexOf");
		}

		[Test]
		public void AsSetTest ()
		{
			var str1 = (NSString) "1";
			var str2 = (NSString) "2";
			var str3 = (NSString) "3";
			var oSet = new NSOrderedSet<NSString> (str1, str2, str3);
			NSSet<NSString> set = oSet.AsSet ();

			ClassicAssert.AreEqual ((nint) 3, oSet.Count, "NSOrderedSet1Test Count");
			ClassicAssert.AreEqual ((nuint) 3, set.Count, "NSOrderedSet1Test Count");
			ClassicAssert.AreSame (str3, set.LookupMember (str3), "NSOrderedSet1Test IndexOf");
		}

		[Test]
		public void IEnumerable1Test ()
		{
			const int C = 16 * 2 + 3; // NSFastEnumerator has a array of size 16, use more than that, and not an exact multiple.
			var values = new NSString [C];
			for (int i = 0; i < C; i++)
				values [i] = (NSString) i.ToString ();

			var st = new NSOrderedSet<NSString> (values);
			ClassicAssert.AreEqual ((nint) C, st.Count, "Count 1");

			var lst = new List<NSString> ();
			foreach (var a in (IEnumerable<NSString>) st) {
				ClassicAssert.IsNotNull (a, "null item iterator");
				ClassicAssert.IsFalse (lst.Contains (a), "duplicated item iterator");
				lst.Add (a);
				ClassicAssert.IsTrue (Array.IndexOf (values, a) >= 0, "different object");
			}
			ClassicAssert.AreEqual (C, lst.Count, "iterator count");
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
			ClassicAssert.AreEqual ((nint) C, st.Count, "Count 1");

			var lst = new List<NSString> ();
			foreach (NSString a in (IEnumerable) st) {
				ClassicAssert.IsNotNull (a, "null item iterator");
				ClassicAssert.IsFalse (lst.Contains (a), "duplicated item iterator");
				lst.Add (a);
				ClassicAssert.IsTrue (Array.IndexOf (values, a) >= 0, "different object");
			}
			ClassicAssert.AreEqual (C, lst.Count, "iterator count");
		}

		[Test]
		public void OperatorEqualTest ()
		{
			var str1 = (NSString) "1";
			var str2 = (NSString) "2";
			var str3 = (NSString) "3";
			var oSet = new NSOrderedSet<NSString> (str1, str2, str3);
			var oSet2 = new NSOrderedSet<NSString> (str1, str2, str3);

			ClassicAssert.IsTrue (oSet == oSet2, "NSOrderedSet1Test == must be true");
			ClassicAssert.IsTrue (oSet.Equals (oSet2), "NSOrderedSet1Test Equals must be true");
		}

		[Test]
		public void OperatorDifferentTest ()
		{
			var str1 = (NSString) "1";
			var str2 = (NSString) "2";
			var str3 = (NSString) "3";
			var oSet = new NSOrderedSet<NSString> (str1, str2, str3);
			var oSet2 = new NSOrderedSet<NSString> (str3, str2, str1);

			ClassicAssert.IsTrue (oSet != oSet2, "NSOrderedSet1Test != must be true");
			ClassicAssert.IsFalse (oSet.Equals (oSet2), "NSOrderedSet1Test Equals must be false");
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
			ClassicAssert.AreEqual ((nint) 4, third.Count, "OperatorAdd Count");
			ClassicAssert.IsTrue (third.Contains (str1), "OperatorAdd 1");
			ClassicAssert.IsTrue (third.Contains (str2), "OperatorAdd 2");
			ClassicAssert.IsTrue (third.Contains (str3), "OperatorAdd 3");
			ClassicAssert.IsTrue (third.Contains (str4), "OperatorAdd 4");
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
			ClassicAssert.AreEqual ((nint) 4, third.Count, "OperatorAdd Count");
			ClassicAssert.IsTrue (third.Contains (str1), "OperatorAdd 1");
			ClassicAssert.IsTrue (third.Contains (str2), "OperatorAdd 2");
			ClassicAssert.IsTrue (third.Contains (str3), "OperatorAdd 3");
			ClassicAssert.IsTrue (third.Contains (str4), "OperatorAdd 4");
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

			ClassicAssert.AreEqual ((nint) 2, third.Count, "OperatorSubtract Count");
			ClassicAssert.IsTrue (third.Contains (str1), "OperatorSubtract 1");
			ClassicAssert.IsTrue (third.Contains (str2), "OperatorSubtract 2");
			ClassicAssert.IsFalse (third.Contains (str3), "OperatorSubtract 3");
			ClassicAssert.IsFalse (third.Contains (str4), "OperatorSubtract 4");
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

			ClassicAssert.AreEqual ((nint) 2, third.Count, "OperatorSubtract2 Count");
			ClassicAssert.IsTrue (third.Contains (str1), "OperatorSubtract2 1");
			ClassicAssert.IsTrue (third.Contains (str2), "OperatorSubtract2 2");
			ClassicAssert.IsFalse (third.Contains (str3), "OperatorSubtract2 3");
			ClassicAssert.IsFalse (third.Contains (str4), "OperatorSubtract2 4");
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
			ClassicAssert.AreNotEqual (IntPtr.Zero, one.Handle, "Handle must be != IntPtr.Zero");
			ClassicAssert.AreNotEqual (IntPtr.Zero, two.Handle, "Handle must be != IntPtr.Zero");
		}
	}
}
