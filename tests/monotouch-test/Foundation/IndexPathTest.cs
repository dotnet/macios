﻿//
// Unit tests for NSIndexPath
//
// Authors:
//	Rolf Bjarne Kvinge <rolf@xamarin.com>
//
// Copyright 2014 Xamarin Inc. All rights reserved.
//

using System;
#if XAMCORE_2_0
using Foundation;
#else
using MonoTouch.Foundation;
#endif
using NUnit.Framework;

#if XAMCORE_2_0
using RectangleF=CoreGraphics.CGRect;
using SizeF=CoreGraphics.CGSize;
using PointF=CoreGraphics.CGPoint;
#else
using nfloat=global::System.Single;
using nint=global::System.Int32;
using nuint=global::System.UInt32;
#endif

namespace MonoTouchFixtures.Foundation {

	[TestFixture]
	public class IndexPathTest {
		[Test]
		public void FromIndex ()
		{
			using (var ip = NSIndexPath.FromIndex (314159)) {
				Assert.AreEqual ((nuint)1, ip.Length, "Length");
				var rv = ip.GetIndexes ();
				Assert.AreEqual (1, rv.Length, "GetIndexes ().Length");
				Assert.AreEqual (314159, rv [0], "GetIndexes ()[0]");
			}
		}

		[Test]
		public void IndexPathByAddingIndexTest ()
		{
			using (var ip1 = new NSIndexPath ()) {
				using (var ip2 = ip1.IndexPathByAddingIndex (3141592)) {
					Assert.AreEqual ((nuint)1, ip2.Length, "Length");
					var rv = ip2.GetIndexes ();
					Assert.AreEqual (1, rv.Length, "GetIndexes ().Length");
					Assert.AreEqual (3141592, rv [0], "GetIndexes ()[0]");
				}
			}
		}

		[Test]
		public void IndexPathByRemovingLastIndexTest ()
		{
			using (var ip1 = NSIndexPath.FromIndex (3)) {
				using (var ip2 = ip1.IndexPathByRemovingLastIndex ()) {
					Assert.AreEqual ((nuint)0, ip2.Length, "Length");
					var rv = ip2.GetIndexes ();
					Assert.AreEqual (0, rv.Length, "GetIndexes ().Length");
				}
			}
		}

		[Test]
		public void IndexAtPositionTest ()
		{
			using (var ip = NSIndexPath.Create (3, 14, 15)) {
				Assert.AreEqual ((nuint) 3, ip.Length, "Length");
				Assert.AreEqual ((nuint)3, ip.IndexAtPosition (0), "[0]");
				Assert.AreEqual ((nuint)14, ip.IndexAtPosition (1), "[0]");
				Assert.AreEqual ((nuint)15, ip.IndexAtPosition (2), "[0]");
			}
		}

		[Test]
		public void CompareTest ()
		{
			using (var ip1 = NSIndexPath.Create (3, 14, 15)) {
				using (var ip2 = NSIndexPath.Create (3, 14, 15)) {
					using (var ip3 = NSIndexPath.Create (3, 14)) {
						Assert.AreEqual (0, ip1.Compare (ip2), "ip1.Compare (ip2)");
						Assert.True (ip1.Equals (ip2), "ip1.Equals (ip2)");
						// "Two objects that are equal return hash codes that are equal."
						Assert.That (ip1.GetHashCode (), Is.EqualTo (ip2.GetHashCode ()), "GetHashCode");
						Assert.AreNotEqual (0, ip1.Compare (ip3), "ip1.Compare (ip3)");
						Assert.False (ip1.Equals (ip3), "ip1.Equals (ip3)");
					}
				}
			}
		}

		[Test]
		public void CreateTest ()
		{
			Assert.Throws<ArgumentNullException> (() => NSIndexPath.Create ((int[])null), "ANE 1");
			Assert.Throws<ArgumentNullException> (() => NSIndexPath.Create ((uint[])null), "ANE 2");
			Assert.Throws<ArgumentNullException> (() => NSIndexPath.Create ((nint[])null), "ANE 3");
			Assert.Throws<ArgumentNullException> (() => NSIndexPath.Create ((nuint[])null), "ANE 4");

			using (var ip = NSIndexPath.Create (1, 2, 3, 4)) {
				Assert.AreEqual ((nuint)4, ip.Length, "Length");
				var rv = ip.GetIndexes ();
				Assert.AreEqual ((nuint) 4, rv.Length, "GetIndexes ().Length");
				Assert.AreEqual ((nuint) 1, rv [0], "GetIndexes ()[0]");
				Assert.AreEqual ((nuint) 2, rv [1], "GetIndexes ()[1]");
				Assert.AreEqual ((nuint) 3, rv [2], "GetIndexes ()[2]");
				Assert.AreEqual ((nuint) 4, rv [3], "GetIndexes ()[3]");
			}

			using (var ip = NSIndexPath.Create ((uint) 1, (uint) 2)) {
				Assert.AreEqual ((nuint) 2, ip.Length, "Length");
				var rv = ip.GetIndexes ();
				Assert.AreEqual ((nuint) 2, rv.Length, "GetIndexes ().Length");
				Assert.AreEqual ((nuint) 1, rv [0], "GetIndexes ()[0]");
				Assert.AreEqual ((nuint) 2, rv [1], "GetIndexes ()[1]");
			}
		}
	}
}
