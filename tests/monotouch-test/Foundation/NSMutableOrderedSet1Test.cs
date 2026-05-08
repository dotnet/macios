//
// Unit tests for NSMutableOrderedSet Generic support
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
	public class NSMutableOrderedSet1Test {

		[Test]
		public void Ctor ()
		{
			var oset = new NSMutableOrderedSet<NSData> ();

			Assert.That (oset.Count, Is.EqualTo ((nint) 0), "NSMutableOrderedSet Count");
		}

		[Test]
		public void Ctor_Capacity ()
		{
			var oset = new NSMutableOrderedSet<NSData> (10);

			Assert.That (oset.Count, Is.EqualTo ((nint) 0), "NSMutableOrderedSet Count");
		}

		[Test]
		public void Ctor_Start ()
		{
			var oSet = new NSMutableOrderedSet<NSString> (start: (NSString) "foo");

			Assert.That (oSet.Count, Is.EqualTo ((nint) 1), "NSMutableOrderedSet Count");
		}

		[Test]
		public void Ctor_Params ()
		{
			var oSet = new NSMutableOrderedSet<NSString> ((NSString) "foo", (NSString) "bar", (NSString) "xyz");

			Assert.That (oSet.Count, Is.EqualTo ((nint) 3), "NSMutableOrderedSet Count");
		}

		[Test]
		public void Ctor_NSSet ()
		{
			var set = new NSSet<NSString> ((NSString) "foo", (NSString) "bar", (NSString) "xyz");
			var oSet = new NSMutableOrderedSet<NSString> (set);

			Assert.That (oSet.Count, Is.EqualTo ((nint) set.Count), "NSMutableOrderedSet Count");
		}

		[Test]
		public void Ctor_NSOrderedSet ()
		{
			var oSetSource = new NSOrderedSet<NSString> ((NSString) "foo", (NSString) "bar", (NSString) "xyz");
			var oSet = new NSMutableOrderedSet<NSString> (oSetSource);

			Assert.That (oSet.Count, Is.EqualTo (oSetSource.Count), "NSOrderedSet1Test Count");
		}

		[Test]
		public void Ctor_NSMutableOrderedSet ()
		{
			var oMutableSet = new NSMutableOrderedSet<NSString> ((NSString) "foo", (NSString) "bar", (NSString) "xyz");
			var oSet = new NSMutableOrderedSet<NSString> (oMutableSet);

			Assert.That (oSet.Count, Is.EqualTo (oMutableSet.Count), "NSOrderedSet1Test Count");
		}

		[Test]
		public void IndexerTest ()
		{
			var str1 = (NSString) "1";
			var str2 = (NSString) "2";
			var str3 = (NSString) "3";
			var oSet = new NSMutableOrderedSet<NSString> (str1, str2, str3);

			Assert.That (oSet.Count, Is.EqualTo ((nint) 3), "NSOrderedSet1Test Count");
			Assert.That (oSet [1], Is.SameAs (str2), "NSOrderedSet1Test IndexOf");
			Assert.Throws<ArgumentNullException> (() => oSet [1] = null);
		}

		[Test]
		public void AsSetTest ()
		{
			var str1 = (NSString) "1";
			var str2 = (NSString) "2";
			var str3 = (NSString) "3";
			var oSet = new NSMutableOrderedSet<NSString> (str1, str2, str3);
			NSSet<NSString> set = oSet.AsSet ();

			Assert.That (oSet.Count, Is.EqualTo ((nint) 3), "NSOrderedSet1Test Count");
			Assert.That (set.Count, Is.EqualTo ((nuint) 3), "NSOrderedSet1Test Count");
			Assert.That (set.LookupMember (str3), Is.SameAs (str3), "NSOrderedSet1Test IndexOf");
		}

		[Test]
		public void InsertTest ()
		{
			var str1 = (NSString) "1";
			var str2 = (NSString) "2";
			var str3 = (NSString) "3";
			var oSet = new NSMutableOrderedSet<NSString> ();
			Assert.That (oSet.Count, Is.EqualTo ((nint) 0), "InsertTest Count");

			oSet.Insert (str1, 0);
			oSet.Insert (str2, 1);
			oSet.Insert (str3, 2);
			Assert.That (oSet.Count, Is.EqualTo ((nint) 3), "InsertTest Count");
		}

		[Test]
		public void ReplaceTest ()
		{
			var str1 = (NSString) "1";
			var str2 = (NSString) "2";
			var str3 = (NSString) "3";
			var str4 = (NSString) "4";
			var oSet = new NSMutableOrderedSet<NSString> (str1, str2, str3);

			oSet.Replace (0, str4);

			Assert.That (oSet.Contains (str4), Is.True, "ReplaceTesr Contains 4");
			Assert.That (oSet.Contains (str1), Is.False, "ReplaceTesr Contains 4");
		}

		[Test]
		public void AddTest ()
		{
			var str1 = (NSString) "1";
			var str2 = (NSString) "2";
			var str3 = (NSString) "3";
			var oSet = new NSMutableOrderedSet<NSString> {
				str1, str2, str3
			};

			Assert.That (oSet.Count, Is.EqualTo ((nint) 3), "AddTest Count");
			Assert.That (oSet.Contains (str1), Is.True, "AddTest Contains 1");
			Assert.That (oSet.Contains (str2), Is.True, "AddTest Contains 2");
			Assert.That (oSet.Contains (str3), Is.True, "AddTest Contains 3");
		}

		[Test]
		public void AddObjectsTest ()
		{
			var str1 = (NSString) "1";
			var str2 = (NSString) "2";
			var str3 = (NSString) "3";
			var oSet = new NSMutableOrderedSet<NSString> ();
			oSet.AddObjects (str1, str2, str3);

			Assert.That (oSet.Count, Is.EqualTo ((nint) 3), "AddObjectsTest Count");
			Assert.That (oSet.Contains (str1), Is.True, "AddObjectsTest Contains 1");
			Assert.That (oSet.Contains (str2), Is.True, "AddObjectsTest Contains 2");
			Assert.That (oSet.Contains (str3), Is.True, "AddObjectsTest Contains 3");
		}

		[Test]
		public void InsertObjectsTest ()
		{
			var str1 = (NSString) "1";
			var str2 = (NSString) "2";
			var str3 = (NSString) "3";
			var str4 = (NSString) "4";
			var oSet = new NSMutableOrderedSet<NSString> (str4);
			oSet.InsertObjects (new [] { str1, str2, str3 }, NSIndexSet.FromNSRange (new NSRange (0, 3)));

			Assert.That (oSet.Count, Is.EqualTo ((nint) 4), "InsertObjectsTest Count");
			Assert.That (oSet.Contains (str1), Is.True, "InsertObjectsTest Contains 1");
			Assert.That (oSet.Contains (str2), Is.True, "InsertObjectsTest Contains 2");
			Assert.That (oSet.Contains (str3), Is.True, "InsertObjectsTest Contains 3");
			Assert.That (oSet.Contains (str4), Is.True, "InsertObjectsTest Contains 4");
			Assert.That (oSet [0], Is.SameAs (str1), "InsertObjectsTest 1 == 1");
			Assert.That (oSet [3], Is.SameAs (str4), "InsertObjectsTest 4 == 4");
		}

		[Test]
		public void ReplaceObjectsTest ()
		{
			var str1 = (NSString) "1";
			var str2 = (NSString) "2";
			var str3 = (NSString) "3";
			var str4 = (NSString) "4";

			var oSet = new NSMutableOrderedSet<NSString> (str1, str2);
			Assert.That (oSet.Count, Is.EqualTo ((nint) 2), "ReplaceObjectsTest Count");
			Assert.That (oSet [0], Is.SameAs (str1), "ReplaceObjectsTest 1 == 1");
			Assert.That (oSet [1], Is.SameAs (str2), "ReplaceObjectsTest 2 == 2");

			oSet.ReplaceObjects (NSIndexSet.FromNSRange (new NSRange (0, 2)), str3, str4);
			Assert.That (oSet [0], Is.SameAs (str3), "ReplaceObjectsTest 3 == 3");
			Assert.That (oSet [1], Is.SameAs (str4), "ReplaceObjectsTest 4 == 4");
		}

		[Test]
		public void RemoveObjectTest ()
		{
			var str1 = (NSString) "1";
			var str2 = (NSString) "2";
			var str3 = (NSString) "3";
			var oSet = new NSMutableOrderedSet<NSString> (str1, str2, str3);
			Assert.That (oSet.Count, Is.EqualTo ((nint) 3), "RemoveObjectTest Count");

			oSet.RemoveObject (str2);
			Assert.That (oSet.Count, Is.EqualTo ((nint) 2), "RemoveObjectTest Count");
			Assert.That (oSet.Contains (str2), Is.False, "RemoveObjectTest must not contain 2");
			Assert.That (oSet.Contains (str1), Is.True, "RemoveObjectTest Contains 1");
			Assert.That (oSet.Contains (str3), Is.True, "RemoveObjectTest Contains 3");
		}

		[Test]
		public void RemoveObjectsTest ()
		{
			var str1 = (NSString) "1";
			var str2 = (NSString) "2";
			var str3 = (NSString) "3";
			var oSet = new NSMutableOrderedSet<NSString> (str1, str2, str3);
			Assert.That (oSet.Count, Is.EqualTo ((nint) 3), "RemoveObjectsTest Count");

			oSet.RemoveObjects (str1, str2);
			Assert.That (oSet.Count, Is.EqualTo ((nint) 1), "RemoveObjectsTest Count");
			Assert.That (oSet.Contains (str1), Is.False, "RemoveObjectsTest must not contain 1");
			Assert.That (oSet.Contains (str2), Is.False, "RemoveObjectsTest must not contain 2");
			Assert.That (oSet.Contains (str3), Is.True, "RemoveObjectsTest Contains 3");
		}

		[Test]
		public void AddObjectsTest_NullValue ()
		{
			var str1 = (NSString) "1";
			NSString? str2 = null;
			var str3 = (NSString) "3";
			var oSet = new NSMutableOrderedSet<NSString> ();
			oSet.AddObjects (str1, str2, str3);

			Assert.That (oSet.Count, Is.EqualTo ((nint) 3), "AddObjectsTest_NullValue Count");
			Assert.That (oSet.Contains (str1), Is.True, "AddObjectsTest_NullValue Contains 1");
			Assert.That (oSet.Contains (NSNull.Null), Is.True, "AddObjectsTest_NullValue Contains NSNull");
			Assert.That (oSet.Contains (str3), Is.True, "AddObjectsTest_NullValue Contains 3");
		}

		[Test]
		public void InsertObjectsTest_NullValue ()
		{
			var str1 = (NSString) "1";
			NSString? str2 = null;
			var str3 = (NSString) "3";
			var str4 = (NSString) "4";
			var oSet = new NSMutableOrderedSet<NSString> (str4);
			oSet.InsertObjects (new NSString? [] { str1, str2, str3 }, NSIndexSet.FromNSRange (new NSRange (0, 3)));

			Assert.That (oSet.Count, Is.EqualTo ((nint) 4), "InsertObjectsTest_NullValue Count");
			Assert.That (oSet.Contains (str1), Is.True, "InsertObjectsTest_NullValue Contains 1");
			Assert.That (oSet.Contains (NSNull.Null), Is.True, "InsertObjectsTest_NullValue Contains NSNull");
			Assert.That (oSet.Contains (str3), Is.True, "InsertObjectsTest_NullValue Contains 3");
			Assert.That (oSet.Contains (str4), Is.True, "InsertObjectsTest_NullValue Contains 4");
			Assert.That (oSet [0], Is.SameAs (str1), "InsertObjectsTest_NullValue 1 == 1");
			Assert.That (oSet [3], Is.SameAs (str4), "InsertObjectsTest_NullValue 4 == 4");
		}

		[Test]
		public void ReplaceObjectsTest_NullValue ()
		{
			var str1 = (NSString) "1";
			var str2 = (NSString) "2";
			NSString? str3 = null;
			var str4 = (NSString) "4";

			var oSet = new NSMutableOrderedSet<NSString> (str1, str2);
			Assert.That (oSet.Count, Is.EqualTo ((nint) 2), "ReplaceObjectsTest_NullValue Count");
			Assert.That (oSet [0], Is.SameAs (str1), "ReplaceObjectsTest_NullValue 1 == 1");
			Assert.That (oSet [1], Is.SameAs (str2), "ReplaceObjectsTest_NullValue 2 == 2");

			oSet.ReplaceObjects (NSIndexSet.FromNSRange (new NSRange (0, 2)), str3, str4);
			var baseSet = (NSOrderedSet) oSet;
			var item0 = baseSet [0];
			Assert.That (item0, Is.InstanceOf<NSNull> (), "ReplaceObjectsTest_NullValue NSNull");
			Assert.That (oSet [1], Is.SameAs (str4), "ReplaceObjectsTest_NullValue 4 == 4");
		}

		[Test]
		public void RemoveObjectsTest_NullValue ()
		{
			var str1 = (NSString) "1";
			NSString? str2 = null;
			var str3 = (NSString) "3";
			var oSet = new NSMutableOrderedSet<NSString> ();
			oSet.AddObjects (str1, str2, str3);
			Assert.That (oSet.Count, Is.EqualTo ((nint) 3), "RemoveObjectsTest_NullValue Count");

			oSet.RemoveObjects (str1, str2);
			Assert.That (oSet.Count, Is.EqualTo ((nint) 1), "RemoveObjectsTest_NullValue Count After Remove");
			Assert.That (oSet.Contains (str1), Is.False, "RemoveObjectsTest_NullValue must not contain 1");
			Assert.That (oSet.Contains (NSNull.Null), Is.False, "RemoveObjectsTest_NullValue must not contain NSNull");
			Assert.That (oSet.Contains (str3), Is.True, "RemoveObjectsTest_NullValue Contains 3");
		}

		[Test]
		public void IEnumerable1Test ()
		{
			const int C = 16 * 2 + 3; // NSFastEnumerator has a array of size 16, use more than that, and not an exact multiple.
			var values = new NSString [C];
			for (int i = 0; i < C; i++)
				values [i] = (NSString) i.ToString ();

			var st = new NSMutableOrderedSet<NSString> (values);
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
			var myEnumerable = new NSMutableOrderedSet<NSNumber> ();
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

			var st = new NSMutableOrderedSet<NSString> (values);
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
		public void OperatorAddTest ()
		{
			var str1 = (NSString) "1";
			var str2 = (NSString) "2";
			var str3 = (NSString) "3";
			var str4 = (NSString) "4";

			var first = new NSMutableOrderedSet<NSString> (str1, str2);
			var second = new NSMutableOrderedSet<NSString> (str3, str4);
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

			var first = new NSMutableOrderedSet<NSString> (str1, str2);
			var second = new NSSet<NSString> (str3, str4);
			var third = first + second;
			Assert.That (third.Count, Is.EqualTo ((nint) 4), "OperatorAdd Count");
			Assert.That (third.Contains (str1), Is.True, "OperatorAdd 1");
			Assert.That (third.Contains (str2), Is.True, "OperatorAdd 2");
			Assert.That (third.Contains (str3), Is.True, "OperatorAdd 3");
			Assert.That (third.Contains (str4), Is.True, "OperatorAdd 4");
		}

		[Test]
		public void OperatorAddTest3 ()
		{
			var str1 = (NSString) "1";
			var str2 = (NSString) "2";
			var str3 = (NSString) "3";
			var str4 = (NSString) "4";

			var first = new NSMutableOrderedSet<NSString> (str1, str2);
			var second = new NSOrderedSet<NSString> (str3, str4);
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

			var first = new NSMutableOrderedSet<NSString> (str1, str2, str3, str4);
			var second = new NSMutableOrderedSet<NSString> (str3, str4);
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

			var first = new NSMutableOrderedSet<NSString> (str1, str2, str3, str4);
			var second = new NSSet<NSString> (str3, str4);
			var third = first - second;

			Assert.That (third.Count, Is.EqualTo ((nint) 2), "OperatorSubtract Count");
			Assert.That (third.Contains (str1), Is.True, "OperatorSubtract 1");
			Assert.That (third.Contains (str2), Is.True, "OperatorSubtract 2");
			Assert.That (third.Contains (str3), Is.False, "OperatorSubtract 3");
			Assert.That (third.Contains (str4), Is.False, "OperatorSubtract 4");
		}

		[Test]
		public void OperatorSubtractTest3 ()
		{
			var str1 = (NSString) "1";
			var str2 = (NSString) "2";
			var str3 = (NSString) "3";
			var str4 = (NSString) "4";

			var first = new NSMutableOrderedSet<NSString> (str1, str2, str3, str4);
			var second = new NSOrderedSet<NSString> (str3, str4);
			var third = first - second;

			Assert.That (third.Count, Is.EqualTo ((nint) 2), "OperatorSubtract Count");
			Assert.That (third.Contains (str1), Is.True, "OperatorSubtract 1");
			Assert.That (third.Contains (str2), Is.True, "OperatorSubtract 2");
			Assert.That (third.Contains (str3), Is.False, "OperatorSubtract 3");
			Assert.That (third.Contains (str4), Is.False, "OperatorSubtract 4");
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

			var one = new NSMutableOrderedSet<NSString> (str1, str2, str3);
			var two = new NSMutableOrderedSet<NSString> (str4, str5, str6);
			NSMutableOrderedSet<NSString> nil = null;
			using (var sum = one + nil)
			using (var sum2 = two + one)
			using (var sum3 = one + two) {

			}
			Assert.That (one.Handle, Is.Not.EqualTo (IntPtr.Zero), "Handle must be != IntPtr.Zero");
			Assert.That (two.Handle, Is.Not.EqualTo (IntPtr.Zero), "Handle must be != IntPtr.Zero");
		}

		[Test]
		public void OperatorAdd_NullNull ()
		{
			NSMutableOrderedSet<NSString> first = null;
			NSMutableOrderedSet<NSString> second = null;
			var result = first + second;
			Assert.That (result, Is.Null, "null + null should be null");
		}

		[Test]
		public void OperatorAdd_NullNonEmpty ()
		{
			NSMutableOrderedSet<NSString> first = null;
			var second = new NSMutableOrderedSet<NSString> ((NSString) "1", (NSString) "2");
			var result = first + second;
			Assert.That (result, Is.Not.Null, "null + non-empty should not be null");
			Assert.That (result.Count, Is.EqualTo ((nint) 2), "Count should be 2");
			Assert.That (result.Contains ((NSString) "1"), Is.True, "Should contain 1");
			Assert.That (result.Contains ((NSString) "2"), Is.True, "Should contain 2");
		}

		[Test]
		public void OperatorAdd_NonEmptyNull ()
		{
			var first = new NSMutableOrderedSet<NSString> ((NSString) "1", (NSString) "2");
			NSMutableOrderedSet<NSString> second = null;
			var result = first + second;
			Assert.That (result, Is.Not.Null, "non-empty + null should not be null");
			Assert.That (result.Count, Is.EqualTo ((nint) 2), "Count should be 2");
			Assert.That (result.Contains ((NSString) "1"), Is.True, "Should contain 1");
			Assert.That (result.Contains ((NSString) "2"), Is.True, "Should contain 2");
		}

		[Test]
		public void OperatorAdd_EmptyEmpty ()
		{
			var first = new NSMutableOrderedSet<NSString> ();
			var second = new NSMutableOrderedSet<NSString> ();
			var result = first + second;
			Assert.That (result, Is.Not.Null, "empty + empty should not be null");
			Assert.That (result.Count, Is.EqualTo ((nint) 0), "Count should be 0");
		}

		[Test]
		public void OperatorAdd_EmptyNonEmpty ()
		{
			var first = new NSMutableOrderedSet<NSString> ();
			var second = new NSMutableOrderedSet<NSString> ((NSString) "1", (NSString) "2");
			var result = first + second;
			Assert.That (result, Is.Not.Null, "empty + non-empty should not be null");
			Assert.That (result.Count, Is.EqualTo ((nint) 2), "Count should be 2");
			Assert.That (result.Contains ((NSString) "1"), Is.True, "Should contain 1");
			Assert.That (result.Contains ((NSString) "2"), Is.True, "Should contain 2");
		}

		[Test]
		public void OperatorAdd_NonEmptyEmpty ()
		{
			var first = new NSMutableOrderedSet<NSString> ((NSString) "1", (NSString) "2");
			var second = new NSMutableOrderedSet<NSString> ();
			var result = first + second;
			Assert.That (result, Is.Not.Null, "non-empty + empty should not be null");
			Assert.That (result.Count, Is.EqualTo ((nint) 2), "Count should be 2");
			Assert.That (result.Contains ((NSString) "1"), Is.True, "Should contain 1");
			Assert.That (result.Contains ((NSString) "2"), Is.True, "Should contain 2");
		}

		[Test]
		public void OperatorAdd_WithNSSet_NullNull ()
		{
			NSMutableOrderedSet<NSString> first = null;
			NSSet<NSString> second = null;
			var result = first + second;
			Assert.That (result, Is.Null, "null + null should be null");
		}

		[Test]
		public void OperatorAdd_WithNSSet_NullNonEmpty ()
		{
			NSMutableOrderedSet<NSString> first = null;
			var second = new NSSet<NSString> ((NSString) "1", (NSString) "2");
			var result = first + second;
			Assert.That (result, Is.Not.Null, "null + non-empty NSSet should not be null");
			Assert.That (result.Count, Is.EqualTo ((nint) 2), "Count should be 2");
			Assert.That (result.Contains ((NSString) "1"), Is.True, "Should contain 1");
			Assert.That (result.Contains ((NSString) "2"), Is.True, "Should contain 2");
		}

		[Test]
		public void OperatorAdd_WithNSSet_NonEmptyNull ()
		{
			var first = new NSMutableOrderedSet<NSString> ((NSString) "1", (NSString) "2");
			NSSet<NSString> second = null;
			var result = first + second;
			Assert.That (result, Is.Not.Null, "non-empty + null NSSet should not be null");
			Assert.That (result.Count, Is.EqualTo ((nint) 2), "Count should be 2");
			Assert.That (result.Contains ((NSString) "1"), Is.True, "Should contain 1");
			Assert.That (result.Contains ((NSString) "2"), Is.True, "Should contain 2");
		}

		[Test]
		public void OperatorAdd_WithNSSet_EmptyEmpty ()
		{
			var first = new NSMutableOrderedSet<NSString> ();
			var second = new NSSet<NSString> ();
			var result = first + second;
			Assert.That (result, Is.Not.Null, "empty + empty NSSet should not be null");
			Assert.That (result.Count, Is.EqualTo ((nint) 0), "Count should be 0");
		}

		[Test]
		public void OperatorAdd_WithNSOrderedSet_NullNull ()
		{
			NSMutableOrderedSet<NSString> first = null;
			NSOrderedSet<NSString> second = null;
			var result = first + second;
			Assert.That (result, Is.Null, "null + null should be null");
		}

		[Test]
		public void OperatorAdd_WithNSOrderedSet_NullNonEmpty ()
		{
			NSMutableOrderedSet<NSString> first = null;
			var second = new NSOrderedSet<NSString> ((NSString) "1", (NSString) "2");
			var result = first + second;
			Assert.That (result, Is.Not.Null, "null + non-empty NSOrderedSet should not be null");
			Assert.That (result.Count, Is.EqualTo ((nint) 2), "Count should be 2");
			Assert.That (result.Contains ((NSString) "1"), Is.True, "Should contain 1");
			Assert.That (result.Contains ((NSString) "2"), Is.True, "Should contain 2");
		}

		[Test]
		public void OperatorAdd_WithNSOrderedSet_NonEmptyNull ()
		{
			var first = new NSMutableOrderedSet<NSString> ((NSString) "1", (NSString) "2");
			NSOrderedSet<NSString> second = null;
			var result = first + second;
			Assert.That (result, Is.Not.Null, "non-empty + null NSOrderedSet should not be null");
			Assert.That (result.Count, Is.EqualTo ((nint) 2), "Count should be 2");
			Assert.That (result.Contains ((NSString) "1"), Is.True, "Should contain 1");
			Assert.That (result.Contains ((NSString) "2"), Is.True, "Should contain 2");
		}

		[Test]
		public void OperatorAdd_WithNSOrderedSet_EmptyEmpty ()
		{
			var first = new NSMutableOrderedSet<NSString> ();
			var second = new NSOrderedSet<NSString> ();
			var result = first + second;
			Assert.That (result, Is.Not.Null, "empty + empty NSOrderedSet should not be null");
			Assert.That (result.Count, Is.EqualTo ((nint) 0), "Count should be 0");
		}

		[Test]
		public void OperatorSubtract_NullNull ()
		{
			NSMutableOrderedSet<NSString> first = null;
			NSMutableOrderedSet<NSString> second = null;
			var result = first - second;
			Assert.That (result, Is.Null, "null - null should be null");
		}

		[Test]
		public void OperatorSubtract_NullNonEmpty ()
		{
			NSMutableOrderedSet<NSString> first = null;
			var second = new NSMutableOrderedSet<NSString> ((NSString) "1", (NSString) "2");
			var result = first - second;
			Assert.That (result, Is.Null, "null - non-empty should be null");
		}

		[Test]
		public void OperatorSubtract_NonEmptyNull ()
		{
			var first = new NSMutableOrderedSet<NSString> ((NSString) "1", (NSString) "2");
			NSMutableOrderedSet<NSString> second = null;
			var result = first - second;
			Assert.That (result, Is.Not.Null, "non-empty - null should not be null");
			Assert.That (result.Count, Is.EqualTo ((nint) 2), "Count should be 2");
			Assert.That (result.Contains ((NSString) "1"), Is.True, "Should contain 1");
			Assert.That (result.Contains ((NSString) "2"), Is.True, "Should contain 2");
		}

		[Test]
		public void OperatorSubtract_EmptyEmpty ()
		{
			var first = new NSMutableOrderedSet<NSString> ();
			var second = new NSMutableOrderedSet<NSString> ();
			var result = first - second;
			Assert.That (result, Is.Not.Null, "empty - empty should not be null");
			Assert.That (result.Count, Is.EqualTo ((nint) 0), "Count should be 0");
		}

		[Test]
		public void OperatorSubtract_EmptyNonEmpty ()
		{
			var first = new NSMutableOrderedSet<NSString> ();
			var second = new NSMutableOrderedSet<NSString> ((NSString) "1", (NSString) "2");
			var result = first - second;
			Assert.That (result, Is.Not.Null, "empty - non-empty should not be null");
			Assert.That (result.Count, Is.EqualTo ((nint) 0), "Count should be 0");
		}

		[Test]
		public void OperatorSubtract_NonEmptyEmpty ()
		{
			var first = new NSMutableOrderedSet<NSString> ((NSString) "1", (NSString) "2");
			var second = new NSMutableOrderedSet<NSString> ();
			var result = first - second;
			Assert.That (result, Is.Not.Null, "non-empty - empty should not be null");
			Assert.That (result.Count, Is.EqualTo ((nint) 2), "Count should be 2");
			Assert.That (result.Contains ((NSString) "1"), Is.True, "Should contain 1");
			Assert.That (result.Contains ((NSString) "2"), Is.True, "Should contain 2");
		}

		[Test]
		public void OperatorSubtract_WithNSSet_NullNull ()
		{
			NSMutableOrderedSet<NSString> first = null;
			NSSet<NSString> second = null;
			var result = first - second;
			Assert.That (result, Is.Null, "null - null should be null");
		}

		[Test]
		public void OperatorSubtract_WithNSSet_NullNonEmpty ()
		{
			NSMutableOrderedSet<NSString> first = null;
			var second = new NSSet<NSString> ((NSString) "1", (NSString) "2");
			var result = first - second;
			Assert.That (result, Is.Null, "null - non-empty NSSet should be null");
		}

		[Test]
		public void OperatorSubtract_WithNSSet_NonEmptyNull ()
		{
			var first = new NSMutableOrderedSet<NSString> ((NSString) "1", (NSString) "2");
			NSSet<NSString> second = null;
			var result = first - second;
			Assert.That (result, Is.Not.Null, "non-empty - null NSSet should not be null");
			Assert.That (result.Count, Is.EqualTo ((nint) 2), "Count should be 2");
			Assert.That (result.Contains ((NSString) "1"), Is.True, "Should contain 1");
			Assert.That (result.Contains ((NSString) "2"), Is.True, "Should contain 2");
		}

		[Test]
		public void OperatorSubtract_WithNSSet_EmptyEmpty ()
		{
			var first = new NSMutableOrderedSet<NSString> ();
			var second = new NSSet<NSString> ();
			var result = first - second;
			Assert.That (result, Is.Not.Null, "empty - empty NSSet should not be null");
			Assert.That (result.Count, Is.EqualTo ((nint) 0), "Count should be 0");
		}

		[Test]
		public void OperatorSubtract_WithNSOrderedSet_NullNull ()
		{
			NSMutableOrderedSet<NSString> first = null;
			NSOrderedSet<NSString> second = null;
			var result = first - second;
			Assert.That (result, Is.Null, "null - null should be null");
		}

		[Test]
		public void OperatorSubtract_WithNSOrderedSet_NullNonEmpty ()
		{
			NSMutableOrderedSet<NSString> first = null;
			var second = new NSOrderedSet<NSString> ((NSString) "1", (NSString) "2");
			var result = first - second;
			Assert.That (result, Is.Null, "null - non-empty NSOrderedSet should be null");
		}

		[Test]
		public void OperatorSubtract_WithNSOrderedSet_NonEmptyNull ()
		{
			var first = new NSMutableOrderedSet<NSString> ((NSString) "1", (NSString) "2");
			NSOrderedSet<NSString> second = null;
			var result = first - second;
			Assert.That (result, Is.Not.Null, "non-empty - null NSOrderedSet should not be null");
			Assert.That (result.Count, Is.EqualTo ((nint) 2), "Count should be 2");
			Assert.That (result.Contains ((NSString) "1"), Is.True, "Should contain 1");
			Assert.That (result.Contains ((NSString) "2"), Is.True, "Should contain 2");
		}

		[Test]
		public void OperatorSubtract_WithNSOrderedSet_EmptyEmpty ()
		{
			var first = new NSMutableOrderedSet<NSString> ();
			var second = new NSOrderedSet<NSString> ();
			var result = first - second;
			Assert.That (result, Is.Not.Null, "empty - empty NSOrderedSet should not be null");
			Assert.That (result.Count, Is.EqualTo ((nint) 0), "Count should be 0");
		}

		[Test]
		public void OperatorAdd_WithDuplicates ()
		{
			var str1 = (NSString) "1";
			var str2 = (NSString) "2";
			var str3 = (NSString) "3";

			var first = new NSMutableOrderedSet<NSString> (str1, str2);
			var second = new NSMutableOrderedSet<NSString> (str2, str3);
			var result = first + second;

			Assert.That (result.Count, Is.EqualTo ((nint) 3), "Count should be 3 (no duplicates)");
			Assert.That (result.Contains (str1), Is.True, "Should contain 1");
			Assert.That (result.Contains (str2), Is.True, "Should contain 2");
			Assert.That (result.Contains (str3), Is.True, "Should contain 3");
		}

		[Test]
		public void OperatorSubtract_PartialOverlap ()
		{
			var str1 = (NSString) "1";
			var str2 = (NSString) "2";
			var str3 = (NSString) "3";
			var str4 = (NSString) "4";

			var first = new NSMutableOrderedSet<NSString> (str1, str2, str3);
			var second = new NSMutableOrderedSet<NSString> (str2, str4);
			var result = first - second;

			Assert.That (result.Count, Is.EqualTo ((nint) 2), "Count should be 2");
			Assert.That (result.Contains (str1), Is.True, "Should contain 1");
			Assert.That (result.Contains (str2), Is.False, "Should not contain 2");
			Assert.That (result.Contains (str3), Is.True, "Should contain 3");
			Assert.That (result.Contains (str4), Is.False, "Should not contain 4");
		}

		[Test]
		public void OperatorSubtract_NoOverlap ()
		{
			var str1 = (NSString) "1";
			var str2 = (NSString) "2";
			var str3 = (NSString) "3";
			var str4 = (NSString) "4";

			var first = new NSMutableOrderedSet<NSString> (str1, str2);
			var second = new NSMutableOrderedSet<NSString> (str3, str4);
			var result = first - second;

			Assert.That (result.Count, Is.EqualTo ((nint) 2), "Count should be 2");
			Assert.That (result.Contains (str1), Is.True, "Should contain 1");
			Assert.That (result.Contains (str2), Is.True, "Should contain 2");
			Assert.That (result.Contains (str3), Is.False, "Should not contain 3");
			Assert.That (result.Contains (str4), Is.False, "Should not contain 4");
		}

		[Test]
		public void Insert_NullObject ()
		{
			var oSet = new NSMutableOrderedSet<NSString> ();
			Assert.Throws<ArgumentNullException> (() => oSet.Insert (null, 0), "Insert should throw for null");
		}

		[Test]
		public void Replace_NullObject ()
		{
			var str1 = (NSString) "1";
			var oSet = new NSMutableOrderedSet<NSString> (str1);
			Assert.Throws<ArgumentNullException> (() => oSet.Replace (0, null), "Replace should throw for null");
		}

		[Test]
		public void Add_NullObject ()
		{
			var oSet = new NSMutableOrderedSet<NSString> ();
			Assert.Throws<ArgumentNullException> (() => oSet.Add (null), "Add should throw for null");
		}

		[Test]
		public void AddObjects_NullArray ()
		{
			var oSet = new NSMutableOrderedSet<NSString> ();
			Assert.Throws<ArgumentNullException> (() => oSet.AddObjects (null), "AddObjects should throw for null array");
		}

		[Test]
		public void InsertObjects_NullArray ()
		{
			var oSet = new NSMutableOrderedSet<NSString> ();
			var indexSet = NSIndexSet.FromNSRange (new NSRange (0, 1));
			Assert.Throws<ArgumentNullException> (() => oSet.InsertObjects (null, indexSet), "InsertObjects should throw for null array");
		}

		[Test]
		public void InsertObjects_NullIndexSet ()
		{
			var str1 = (NSString) "1";
			var oSet = new NSMutableOrderedSet<NSString> ();
			Assert.Throws<ArgumentNullException> (() => oSet.InsertObjects (new [] { str1 }, null), "InsertObjects should throw for null index set");
		}

		[Test]
		public void ReplaceObjects_NullArray ()
		{
			var str1 = (NSString) "1";
			var oSet = new NSMutableOrderedSet<NSString> (str1);
			var indexSet = NSIndexSet.FromNSRange (new NSRange (0, 1));
			Assert.Throws<ArgumentNullException> (() => oSet.ReplaceObjects (indexSet, null), "ReplaceObjects should throw for null array");
		}

		[Test]
		public void ReplaceObjects_NullIndexSet ()
		{
			var str1 = (NSString) "1";
			var str2 = (NSString) "2";
			var oSet = new NSMutableOrderedSet<NSString> (str1);
			Assert.Throws<ArgumentNullException> (() => oSet.ReplaceObjects (null, str2), "ReplaceObjects should throw for null index set");
		}

		[Test]
		public void RemoveObject_NullObject ()
		{
			var oSet = new NSMutableOrderedSet<NSString> ();
			Assert.Throws<ArgumentNullException> (() => oSet.RemoveObject (null), "RemoveObject should throw for null");
		}

		[Test]
		public void RemoveObjects_NullArray ()
		{
			var oSet = new NSMutableOrderedSet<NSString> ();
			Assert.Throws<ArgumentNullException> (() => oSet.RemoveObjects (null), "RemoveObjects should throw for null array");
		}

		[Test]
		public void Indexer_OrderPreservation ()
		{
			var str1 = (NSString) "1";
			var str2 = (NSString) "2";
			var str3 = (NSString) "3";
			var oSet = new NSMutableOrderedSet<NSString> (str1, str2, str3);

			Assert.That (oSet [0], Is.SameAs (str1), "Index 0 should be str1");
			Assert.That (oSet [1], Is.SameAs (str2), "Index 1 should be str2");
			Assert.That (oSet [2], Is.SameAs (str3), "Index 2 should be str3");
		}

		[Test]
		public void Indexer_SetValue ()
		{
			var str1 = (NSString) "1";
			var str2 = (NSString) "2";
			var str3 = (NSString) "3";
			var oSet = new NSMutableOrderedSet<NSString> (str1, str2);

			oSet [1] = str3;
			Assert.That (oSet [1], Is.SameAs (str3), "Index 1 should now be str3");
			Assert.That (oSet.Count, Is.EqualTo ((nint) 2), "Count should remain 2");
		}

		[Test]
		public void Add_DuplicateElement ()
		{
			var str1 = (NSString) "1";
			var oSet = new NSMutableOrderedSet<NSString> (str1);
			Assert.That (oSet.Count, Is.EqualTo ((nint) 1), "Initial count should be 1");

			oSet.Add (str1);
			Assert.That (oSet.Count, Is.EqualTo ((nint) 1), "Count should still be 1 after adding duplicate");
		}

		[Test]
		public void Insert_AtBeginning ()
		{
			var str1 = (NSString) "1";
			var str2 = (NSString) "2";
			var str3 = (NSString) "3";
			var oSet = new NSMutableOrderedSet<NSString> (str2, str3);

			oSet.Insert (str1, 0);
			Assert.That (oSet.Count, Is.EqualTo ((nint) 3), "Count should be 3");
			Assert.That (oSet [0], Is.SameAs (str1), "Index 0 should be str1");
			Assert.That (oSet [1], Is.SameAs (str2), "Index 1 should be str2");
			Assert.That (oSet [2], Is.SameAs (str3), "Index 2 should be str3");
		}

		[Test]
		public void Insert_AtEnd ()
		{
			var str1 = (NSString) "1";
			var str2 = (NSString) "2";
			var str3 = (NSString) "3";
			var oSet = new NSMutableOrderedSet<NSString> (str1, str2);

			oSet.Insert (str3, 2);
			Assert.That (oSet.Count, Is.EqualTo ((nint) 3), "Count should be 3");
			Assert.That (oSet [0], Is.SameAs (str1), "Index 0 should be str1");
			Assert.That (oSet [1], Is.SameAs (str2), "Index 1 should be str2");
			Assert.That (oSet [2], Is.SameAs (str3), "Index 2 should be str3");
		}

		[Test]
		public void AsSet_EmptySet ()
		{
			var oSet = new NSMutableOrderedSet<NSString> ();
			var set = oSet.AsSet ();
			Assert.That (set, Is.Not.Null, "AsSet should not return null");
			Assert.That (set.Count, Is.EqualTo ((nuint) 0), "Set count should be 0");
		}

		[Test]
		public void RemoveObject_NotPresent ()
		{
			var str1 = (NSString) "1";
			var str2 = (NSString) "2";
			var oSet = new NSMutableOrderedSet<NSString> (str1);

			oSet.RemoveObject (str2);
			Assert.That (oSet.Count, Is.EqualTo ((nint) 1), "Count should remain 1");
			Assert.That (oSet.Contains (str1), Is.True, "Should still contain str1");
		}

		[Test]
		public void RemoveObjects_EmptyArray ()
		{
			var str1 = (NSString) "1";
			var oSet = new NSMutableOrderedSet<NSString> (str1);

			oSet.RemoveObjects (new NSString [0]);
			Assert.That (oSet.Count, Is.EqualTo ((nint) 1), "Count should remain 1");
			Assert.That (oSet.Contains (str1), Is.True, "Should still contain str1");
		}

		[Test]
		public void AddObjects_EmptyArray ()
		{
			var oSet = new NSMutableOrderedSet<NSString> ();
			oSet.AddObjects (new NSString [0]);
			Assert.That (oSet.Count, Is.EqualTo ((nint) 0), "Count should be 0");
		}
	}
}
