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
				Assert.That (ip.Length, Is.EqualTo ((nint) 1), "Length");
				var rv = ip.GetIndexes ();
				Assert.That (rv.Length, Is.EqualTo (1), "GetIndexes ().Length");
				Assert.That (rv [0], Is.EqualTo ((nuint) 314159), "GetIndexes ()[0]");
			}
		}

		[Test]
		public void IndexPathByAddingIndexTest ()
		{
			using (var ip1 = new NSIndexPath ()) {
				using (var ip2 = ip1.IndexPathByAddingIndex (3141592)) {
					Assert.That (ip2.Length, Is.EqualTo ((nint) 1), "Length");
					var rv = ip2.GetIndexes ();
					Assert.That (rv.Length, Is.EqualTo (1), "GetIndexes ().Length");
					Assert.That (rv [0], Is.EqualTo ((nuint) 3141592), "GetIndexes ()[0]");
				}
			}
		}

		[Test]
		public void IndexPathByRemovingLastIndexTest ()
		{
			using (var ip1 = NSIndexPath.FromIndex (3)) {
				using (var ip2 = ip1.IndexPathByRemovingLastIndex ()) {
					Assert.That (ip2.Length, Is.EqualTo ((nint) 0), "Length");
					var rv = ip2.GetIndexes ();
					Assert.That (rv.Length, Is.EqualTo (0), "GetIndexes ().Length");
				}
			}
		}

		[Test]
		public void IndexAtPositionTest ()
		{
			using (var ip = NSIndexPath.Create (3, 14, 15)) {
				Assert.That (ip.Length, Is.EqualTo ((nint) 3), "Length");
				Assert.That (ip.IndexAtPosition (0), Is.EqualTo ((nuint) 3), "[0]");
				Assert.That (ip.IndexAtPosition (1), Is.EqualTo ((nuint) 14), "[0]");
				Assert.That (ip.IndexAtPosition (2), Is.EqualTo ((nuint) 15), "[0]");
			}
		}

		[Test]
		public void CompareTest ()
		{
			using (var ip1 = NSIndexPath.Create (3, 14, 15)) {
				using (var ip2 = NSIndexPath.Create (3, 14, 15)) {
					using (var ip3 = NSIndexPath.Create (3, 14)) {
						Assert.That (ip1.Compare (ip2), Is.EqualTo ((nint) 0), "ip1.Compare (ip2)");
						Assert.That (ip1.Equals (ip2), Is.True, "ip1.Equals (ip2)");
						// "Two objects that are equal return hash codes that are equal."
						Assert.That (ip1.GetHashCode (), Is.EqualTo (ip2.GetHashCode ()), "GetHashCode");
						Assert.That (ip1.Compare (ip3), Is.Not.EqualTo ((nint) 0), "ip1.Compare (ip3)");
						Assert.That (ip1.Equals (ip3), Is.False, "ip1.Equals (ip3)");
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
				Assert.That (ip.Length, Is.EqualTo ((nint) 4), "Length");
				var rv = ip.GetIndexes ();
				Assert.That (rv.Length, Is.EqualTo (4), "GetIndexes ().Length");
				Assert.That (rv [0], Is.EqualTo ((nuint) 1), "GetIndexes ()[0]");
				Assert.That (rv [1], Is.EqualTo ((nuint) 2), "GetIndexes ()[1]");
				Assert.That (rv [2], Is.EqualTo ((nuint) 3), "GetIndexes ()[2]");
				Assert.That (rv [3], Is.EqualTo ((nuint) 4), "GetIndexes ()[3]");
			}

			using (var ip = NSIndexPath.Create ((uint) 1, (uint) 2)) {
				Assert.That (ip.Length, Is.EqualTo ((nint) 2), "Length");
				var rv = ip.GetIndexes ();
				Assert.That (rv.Length, Is.EqualTo (2), "GetIndexes ().Length");
				Assert.That (rv [0], Is.EqualTo ((nuint) 1), "GetIndexes ()[0]");
				Assert.That (rv [1], Is.EqualTo ((nuint) 2), "GetIndexes ()[1]");
			}
		}
	}
}
