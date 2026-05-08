//
// Unit tests for CGRect
//
// Authors:
//	Rolf Bjarne Kvinge <rolf@xamarin.com>
//
// Copyright 2016 Xamarin Inc. All rights reserved.
//

using CoreGraphics;

namespace MonoTouchFixtures.CoreGraphics {
	[TestFixture]
	[Preserve (AllMembers = true)]
	public class RectTest {
		[Test]
		public void Inflate ()
		{
			var rect = new CGRect (1, 2, 3, 4);
			rect.Inflate (5, 6);
			ClassicAssert.AreEqual (-4, (int) rect.X, "x 1");
			ClassicAssert.AreEqual (-4, (int) rect.Y, "y 1");
			ClassicAssert.AreEqual (13, (int) rect.Width, "w 1");
			ClassicAssert.AreEqual (16, (int) rect.Height, "h 1");

			rect.Inflate (new CGSize (10, 20));
			ClassicAssert.AreEqual (-14, (int) rect.X, "x 2");
			ClassicAssert.AreEqual (-24, (int) rect.Y, "y 2");
			ClassicAssert.AreEqual (33, (int) rect.Width, "w 2");
			ClassicAssert.AreEqual (56, (int) rect.Height, "h 2");

			rect = CGRect.Inflate (rect, 5, 4);
			ClassicAssert.AreEqual (-19, (int) rect.X, "x 3");
			ClassicAssert.AreEqual (-28, (int) rect.Y, "y 3");
			ClassicAssert.AreEqual (43, (int) rect.Width, "w 3");
			ClassicAssert.AreEqual (64, (int) rect.Height, "h 3");
		}

		[Test]
		public void Null ()
		{
			ClassicAssert.True (CGRect.Null.IsNull (), "Null.IsNull");
			ClassicAssert.True (CGRect.Null.IsEmpty, "Null.IsEmpty");
			ClassicAssert.False (CGRect.Null.IsInfinite (), "Null.IsInfinite");
		}

		[Test]
		public void Infinite ()
		{
			ClassicAssert.True (CGRect.Infinite.IsInfinite (), "Infinite.IsInfinite");
			ClassicAssert.False (CGRect.Infinite.IsEmpty, "Infinite.IsEmpty");
			ClassicAssert.False (CGRect.Infinite.IsNull (), "Infinite.IsNull");
		}

		[Test]
		public void Empty ()
		{
			ClassicAssert.True (CGRect.Empty.IsEmpty, "Empty.IsEmpty");
			ClassicAssert.False (CGRect.Empty.IsNull (), "Empty.IsNull");
			ClassicAssert.False (CGRect.Empty.IsInfinite (), "Empty.IsInfinite");

			// for System.Drawing compatibility this was named Empty - test confirms it's identical to CGRectZero
			var handle = Dlfcn.dlopen (Constants.CoreGraphicsLibrary, 0);
			try {
				var zero = Dlfcn.GetCGRect (handle, "CGRectZero");
				ClassicAssert.AreEqual (CGRect.Empty, zero, "CGRectZero");
			} finally {
				Dlfcn.dlclose (handle);
			}
		}

		[Test]
		public void ToStringTest ()
		{
			var rect = new CGRect ((nfloat) 1, (nfloat) 2, (nfloat) 3, (nfloat) 4);
			ClassicAssert.AreEqual ("{{1, 2}, {3, 4}}", rect.ToString (), "ToString");
		}
	}
}
