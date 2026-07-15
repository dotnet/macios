using System.Collections;

namespace MonoTouchFixtures.Foundation {

	[TestFixture]
	[Preserve (AllMembers = true)]
	public class NSMutableArray1Test {

		[Test]
		public void Ctor ()
		{
			using (var arr = new NSMutableArray<NSDate> ()) {
				Assert.That (arr.Count, Is.EqualTo ((nuint) 0), "Count");
			}
		}

		[Test]
		public void Ctor_Capacity ()
		{
			using (var arr = new NSMutableArray<NSString> (1)) {
				Assert.That (arr.Count, Is.EqualTo ((nuint) 0), "Count");
			}
		}

		[Test]
		public void ContainsTest ()
		{
			var v = (NSString) "value";
			var v2 = (NSString) "value 2";
			using (var arr = new NSMutableArray<NSString> (v, v)) {
				Assert.Throws<ArgumentNullException> (() => arr.Contains (null), "Contains ANE");
				Assert.That (arr.Contains (v), Is.True, "Contains 1");
				Assert.That (arr.Contains (v2), Is.False, "Contains 2");
			}
		}

		[Test]
		public void IndexOfTest ()
		{
			var v1 = (NSString) "1";
			var v2 = (NSString) "2";

			using (var arr = new NSMutableArray<NSString> (v1)) {
				Assert.Throws<ArgumentNullException> (() => arr.IndexOf (null), "IndexOf ANE");
				Assert.That (arr.IndexOf (v1), Is.EqualTo ((nuint) 0), "IndexOf 1");
				Assert.That (arr.IndexOf (v2), Is.EqualTo ((nuint) nint.MaxValue), "IndxOf 2"); // [NSArray indexOfObject:] returns NSNotFound = NSIntegerMax when object isn't found in the array
			}
		}

		[Test]
		public void AddTest ()
		{
			var v1 = (NSString) "1";
			var v2 = (NSString) "2";

			using (var arr = new NSMutableArray<NSString> (v1)) {
				Assert.That (arr.Count, Is.EqualTo ((nuint) 1), "Count 1");
				Assert.Throws<ArgumentNullException> (() => arr.Add (null), "Add ANE");
				arr.Add (v2);
				Assert.That (arr.Count, Is.EqualTo ((nuint) 2), "Count 2");
				Assert.That (arr [1], Is.SameAs (v2), "idx[1]");
			}
		}

		[Test]
		public void InsertTest ()
		{
			var v1 = (NSString) "1";
			var v2 = (NSString) "2";
			var v3 = (NSString) "3";

			using (var arr = new NSMutableArray<NSString> (v1, v3)) {
				Assert.That (arr.Count, Is.EqualTo ((nuint) 2), "Insert 1");
				Assert.Throws<ArgumentNullException> (() => arr.Insert (null, 0), "Insert ANE");
				Assert.Throws<IndexOutOfRangeException> (() => arr.Insert (v2, -1), "Insert AOORE 1");
				Assert.Throws<IndexOutOfRangeException> (() => arr.Insert (v2, 3), "Insert AOORE 2");
				arr.Insert (v2, 1);
				Assert.That (arr.Count, Is.EqualTo ((nuint) 3), "Insert 2");
				Assert.That (arr [0], Is.SameAs (v1), "[0]");
				Assert.That (arr [1], Is.SameAs (v2), "[1]");
				Assert.That (arr [2], Is.SameAs (v3), "[2]");
			}

			using (var arr = new NSMutableArray<NSString> ()) {
				Assert.DoesNotThrow (() => arr.Insert (v1, 0), "Insert into empty array");
			}
		}

		[Test]
		public void ReplaceObjectTest ()
		{
			var v1 = (NSString) "1";
			var v2 = (NSString) "2";
			var v3 = (NSString) "3";

			using (var arr = new NSMutableArray<NSString> (v1, v3)) {
				Assert.That (arr.Count, Is.EqualTo ((nuint) 2), "ReplaceObject 1");
				Assert.That (arr [0], Is.SameAs (v1), "a [0]");
				Assert.That (arr [1], Is.SameAs (v3), "a [1]");
				Assert.Throws<ArgumentNullException> (() => arr.ReplaceObject (0, null), "Insert ANE");
				Assert.Throws<IndexOutOfRangeException> (() => arr.ReplaceObject (-1, v2), "Insert AOORE 1");
				Assert.Throws<IndexOutOfRangeException> (() => arr.ReplaceObject (3, v2), "Insert AOORE 2");
				arr.ReplaceObject (1, v2);
				Assert.That (arr.Count, Is.EqualTo ((nuint) 2), "ReplaceObject 2");
				Assert.That (arr [0], Is.SameAs (v1), "b [0]");
				Assert.That (arr [1], Is.SameAs (v2), "b [1]");
			}
		}

		[Test]
		public void AddObjectsTest ()
		{
			var v1 = (NSString) "1";
			var v2 = (NSString) "2";
			var v3 = (NSString) "3";

			using (var arr = new NSMutableArray<NSString> ()) {
				Assert.Throws<ArgumentNullException> (() => arr.AddObjects ((NSString []) null), "AddObjects ANE 1");
				Assert.That (arr.Count, Is.EqualTo ((nuint) 0), "Count 1");

				Assert.Throws<ArgumentNullException> (() => arr.AddObjects (new NSString [] { null }), "AddObjects ANE 2");
				Assert.That (arr.Count, Is.EqualTo ((nuint) 0), "Count 2");

				Assert.Throws<ArgumentNullException> (() => arr.AddObjects (new NSString [] { v1, null, v3 }), "AddObjects ANE 3");
				Assert.That (arr.Count, Is.EqualTo ((nuint) 0), "Count 3");

				arr.AddObjects (v1, v2);
				Assert.That (arr.Count, Is.EqualTo ((nuint) 2), "AddObjects 1");
				Assert.That (arr [0], Is.SameAs (v1), "a [0]");
				Assert.That (arr [1], Is.SameAs (v2), "a [1]");
			}
		}

		[Test]
		public void InsertObjectsTest ()
		{
			var v1 = (NSString) "1";
			var v2 = (NSString) "2";
			var v3 = (NSString) "3";
			var v4 = (NSString) "4";

			using (var arr = new NSMutableArray<NSString> (v1, v2)) {
				var iset = new NSMutableIndexSet ();
				iset.Add (1);
				iset.Add (2);

				Assert.Throws<ArgumentNullException> (() => arr.InsertObjects ((NSString []) null, iset), "InsertObjects ANE 1");
				Assert.That (arr.Count, Is.EqualTo ((nuint) 2), "Count 1");

				Assert.Throws<ArgumentNullException> (() => arr.InsertObjects (new NSString [] { null, null }, iset), "InsertObjects ANE 2");
				Assert.That (arr.Count, Is.EqualTo ((nuint) 2), "Count 2");

				Assert.Throws<ArgumentNullException> (() => arr.InsertObjects (new NSString [] { v1, null }, iset), "InsertObjects ANE 3");
				Assert.That (arr.Count, Is.EqualTo ((nuint) 2), "Count 3");

				Assert.Throws<ArgumentNullException> (() => arr.InsertObjects (new NSString [] { v1 }, null), "InsertObjects ANE 4");
				Assert.That (arr.Count, Is.EqualTo ((nuint) 2), "Count 4");

				arr.InsertObjects (new NSString [] { v3, v4 }, iset);

				Assert.That (arr.Count, Is.EqualTo ((nuint) 4), "InsertObjects 1");
				Assert.That (arr [0], Is.SameAs (v1), "a [0]");
				Assert.That (arr [1], Is.SameAs (v3), "a [1]");
				Assert.That (arr [2], Is.SameAs (v4), "a [2]");
				Assert.That (arr [3], Is.SameAs (v2), "a [3]");

				iset.Clear ();
				iset.Add (9);
				Assert.Throws<IndexOutOfRangeException> (() => arr.InsertObjects (new NSString [] { v1 }, iset), "InsertObjects ANE 5");
			}
		}

		[Test]
		public void IndexerTest ()
		{
			var v1 = (NSString) "1";
			var v2 = (NSString) "2";
			var v3 = (NSString) "3";

			using (var arr = new NSMutableArray<NSString> (v1, v2)) {
				arr [1] = v3;
				Assert.That (arr.Count, Is.EqualTo ((nuint) 2), "a 1");
				Assert.That (arr [0], Is.SameAs (v1), "a [0]");
				Assert.That (arr [1], Is.SameAs (v3), "a [1]");

				Assert.Throws<ArgumentNullException> (() => arr [0] = null, "ANE 1");
				Assert.Throws<IndexOutOfRangeException> (() => arr [2] = v3, "IOORE 1");
			}
		}

		[Test]
		public void IEnumerableTest ()
		{
			const int C = 16 * 2 + 3; // NSFastEnumerator has a array of size 16, use more than that, and not an exact multiple.
			using (var arr = new NSMutableArray<NSString> ()) {
				for (int i = 0; i < C; i++)
					arr.Add ((NSString) i.ToString ());
				Assert.That (arr.Count, Is.EqualTo ((nuint) C), "Count 1");

				var lst = new List<NSString> ();
				foreach (NSString a in (IEnumerable) arr) {
					Assert.That (a, Is.Not.Null, "null item iterator");
					Assert.That (lst.Contains (a), Is.False, "duplicated item iterator");
					Assert.That ((string) a, Is.EqualTo (lst.Count.ToString ()), "#" + lst.Count.ToString ());
					lst.Add (a);
				}
				Assert.That (lst.Count, Is.EqualTo (C), "iterator count");
			}
		}

		[Test]
		public void IEnumerable1Test ()
		{
			const int C = 16 * 2 + 3; // NSFastEnumerator has a array of size 16, use more than that, and not an exact multiple.
			using (var arr = new NSMutableArray<NSString> ()) {
				for (int i = 0; i < C; i++)
					arr.Add ((NSString) i.ToString ());
				Assert.That (arr.Count, Is.EqualTo ((nuint) C), "Count 1");

				var lst = new List<NSString> ();
				foreach (var a in (IEnumerable<NSString>) arr) {
					Assert.That (a, Is.Not.Null, "null item iterator");
					Assert.That (lst.Contains (a), Is.False, "duplicated item iterator");
					Assert.That ((string) a, Is.EqualTo (lst.Count.ToString ()), "#" + lst.Count.ToString ());
					lst.Add (a);
				}
				Assert.That (lst.Count, Is.EqualTo (C), "iterator count");
			}
		}

		[Test]
		public void IEnumerable1Test_EnumeratorType ()
		{
			var myEnumerable = new NSMutableArray<NSNumber> ();
			foreach (var item in myEnumerable) {
				// The point of this test is to verify that the compiler finds the correct enumerator (the one returning NSNumbers, and not the one from the non-generic NSSet class returning NSObjects).
				// This means that we don't have to actually execute this code, it's enough to make it compile.
				Console.WriteLine (item.LongValue);
			}
		}
	}
}
