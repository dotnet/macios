//
// Unit tests for NSIndexPath
//
// Authors:
//	Rolf Bjarne Kvinge <rolf@xamarin.com>
//
// Copyright 2014 Xamarin Inc. All rights reserved.
//

namespace MonoTouchFixtures.Foundation {

	[TestFixture]
	[Preserve (AllMembers = true)]
	public class IndexPathTest {
		[Test]
		public void FromIndex ()
		{
			using (var ip = NSIndexPath.FromIndex (314159)) {
				ClassicAssert.AreEqual ((nint) 1, ip.Length, "Length");
				var rv = ip.GetIndexes ();
				ClassicAssert.AreEqual (1, rv.Length, "GetIndexes ().Length");
				ClassicAssert.AreEqual ((nuint) 314159, rv [0], "GetIndexes ()[0]");
			}
		}

		[Test]
		public void IndexPathByAddingIndexTest ()
		{
			using (var ip1 = new NSIndexPath ()) {
				using (var ip2 = ip1.IndexPathByAddingIndex (3141592)) {
					ClassicAssert.AreEqual ((nint) 1, ip2.Length, "Length");
					var rv = ip2.GetIndexes ();
					ClassicAssert.AreEqual (1, rv.Length, "GetIndexes ().Length");
					ClassicAssert.AreEqual ((nuint) 3141592, rv [0], "GetIndexes ()[0]");
				}
			}
		}

		[Test]
		public void IndexPathByRemovingLastIndexTest ()
		{
			using (var ip1 = NSIndexPath.FromIndex (3)) {
				using (var ip2 = ip1.IndexPathByRemovingLastIndex ()) {
					ClassicAssert.AreEqual ((nint) 0, ip2.Length, "Length");
					var rv = ip2.GetIndexes ();
					ClassicAssert.AreEqual (0, rv.Length, "GetIndexes ().Length");
				}
			}
		}

		[Test]
		public void IndexAtPositionTest ()
		{
			using (var ip = NSIndexPath.Create (3, 14, 15)) {
				ClassicAssert.AreEqual ((nint) 3, ip.Length, "Length");
				ClassicAssert.AreEqual ((nuint) 3, ip.IndexAtPosition (0), "[0]");
				ClassicAssert.AreEqual ((nuint) 14, ip.IndexAtPosition (1), "[0]");
				ClassicAssert.AreEqual ((nuint) 15, ip.IndexAtPosition (2), "[0]");
			}
		}

		[Test]
		public void CompareTest ()
		{
			using (var ip1 = NSIndexPath.Create (3, 14, 15)) {
				using (var ip2 = NSIndexPath.Create (3, 14, 15)) {
					using (var ip3 = NSIndexPath.Create (3, 14)) {
						ClassicAssert.AreEqual ((nint) 0, ip1.Compare (ip2), "ip1.Compare (ip2)");
						ClassicAssert.True (ip1.Equals (ip2), "ip1.Equals (ip2)");
						// "Two objects that are equal return hash codes that are equal."
						Assert.That (ip1.GetHashCode (), Is.EqualTo (ip2.GetHashCode ()), "GetHashCode");
						ClassicAssert.AreNotEqual ((nint) 0, ip1.Compare (ip3), "ip1.Compare (ip3)");
						ClassicAssert.False (ip1.Equals (ip3), "ip1.Equals (ip3)");
					}
				}
			}
		}

		[Test]
		public void CreateTest ()
		{
			Assert.Throws<ArgumentNullException> (() => NSIndexPath.Create ((int []) null), "ANE 1");
			Assert.Throws<ArgumentNullException> (() => NSIndexPath.Create ((uint []) null), "ANE 2");
			Assert.Throws<ArgumentNullException> (() => NSIndexPath.Create ((nint []) null), "ANE 3");
			Assert.Throws<ArgumentNullException> (() => NSIndexPath.Create ((nuint []) null), "ANE 4");

			using (var ip = NSIndexPath.Create (1, 2, 3, 4)) {
				ClassicAssert.AreEqual ((nint) 4, ip.Length, "Length");
				var rv = ip.GetIndexes ();
				ClassicAssert.AreEqual (4, rv.Length, "GetIndexes ().Length");
				ClassicAssert.AreEqual ((nuint) 1, rv [0], "GetIndexes ()[0]");
				ClassicAssert.AreEqual ((nuint) 2, rv [1], "GetIndexes ()[1]");
				ClassicAssert.AreEqual ((nuint) 3, rv [2], "GetIndexes ()[2]");
				ClassicAssert.AreEqual ((nuint) 4, rv [3], "GetIndexes ()[3]");
			}

			using (var ip = NSIndexPath.Create ((uint) 1, (uint) 2)) {
				ClassicAssert.AreEqual ((nint) 2, ip.Length, "Length");
				var rv = ip.GetIndexes ();
				ClassicAssert.AreEqual (2, rv.Length, "GetIndexes ().Length");
				ClassicAssert.AreEqual ((nuint) 1, rv [0], "GetIndexes ()[0]");
				ClassicAssert.AreEqual ((nuint) 2, rv [1], "GetIndexes ()[1]");
			}
		}
	}
}
