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
			Assert.That ((int) rect.X, Is.EqualTo (-4), "x 1");
			Assert.That ((int) rect.Y, Is.EqualTo (-4), "y 1");
			Assert.That ((int) rect.Width, Is.EqualTo (13), "w 1");
			Assert.That ((int) rect.Height, Is.EqualTo (16), "h 1");

			rect.Inflate (new CGSize (10, 20));
			Assert.That ((int) rect.X, Is.EqualTo (-14), "x 2");
			Assert.That ((int) rect.Y, Is.EqualTo (-24), "y 2");
			Assert.That ((int) rect.Width, Is.EqualTo (33), "w 2");
			Assert.That ((int) rect.Height, Is.EqualTo (56), "h 2");

			rect = CGRect.Inflate (rect, 5, 4);
			Assert.That ((int) rect.X, Is.EqualTo (-19), "x 3");
			Assert.That ((int) rect.Y, Is.EqualTo (-28), "y 3");
			Assert.That ((int) rect.Width, Is.EqualTo (43), "w 3");
			Assert.That ((int) rect.Height, Is.EqualTo (64), "h 3");
		}

		[Test]
		public void Null ()
		{
			Assert.That (CGRect.Null.IsNull (), Is.True, "Null.IsNull");
			Assert.That (CGRect.Null.IsEmpty, Is.True, "Null.IsEmpty");
			Assert.That (CGRect.Null.IsInfinite (), Is.False, "Null.IsInfinite");
		}

		[Test]
		public void Infinite ()
		{
			Assert.That (CGRect.Infinite.IsInfinite (), Is.True, "Infinite.IsInfinite");
			Assert.That (CGRect.Infinite.IsEmpty, Is.False, "Infinite.IsEmpty");
			Assert.That (CGRect.Infinite.IsNull (), Is.False, "Infinite.IsNull");
		}

		[Test]
		public void Empty ()
		{
			Assert.That (CGRect.Empty.IsEmpty, Is.True, "Empty.IsEmpty");
			Assert.That (CGRect.Empty.IsNull (), Is.False, "Empty.IsNull");
			Assert.That (CGRect.Empty.IsInfinite (), Is.False, "Empty.IsInfinite");

			// for System.Drawing compatibility this was named Empty - test confirms it's identical to CGRectZero
			var handle = Dlfcn.dlopen (Constants.CoreGraphicsLibrary, 0);
			try {
				var zero = Dlfcn.GetCGRect (handle, "CGRectZero");
				Assert.That (zero, Is.EqualTo (CGRect.Empty), "CGRectZero");
			} finally {
				Dlfcn.dlclose (handle);
			}
		}

		[Test]
		public void ToStringTest ()
		{
			var rect = new CGRect ((nfloat) 1, (nfloat) 2, (nfloat) 3, (nfloat) 4);
			Assert.That (rect.ToString (), Is.EqualTo ("{{1, 2}, {3, 4}}"), "ToString");
		}
	}
}
