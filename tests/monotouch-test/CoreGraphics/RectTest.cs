//
// Unit tests for CGRect
//
// Authors:
//	Rolf Bjarne Kvinge <rolf@xamarin.com>
//
// Copyright 2016 Xamarin Inc. All rights reserved.
//

using System;
#if XAMCORE_2_0
using Foundation;
using CoreGraphics;
using ObjCRuntime;
#else
using MonoTouch.CoreGraphics;
using MonoTouch.Foundation;
using MonoTouch.ObjCRuntime;
using System.Drawing;
#endif
using NUnit.Framework;

#if XAMCORE_2_0
using RectangleF=CoreGraphics.CGRect;
using SizeF=CoreGraphics.CGSize;
using PointF=CoreGraphics.CGPoint;
#else
using nfloat = global::System.Single;
using nint = global::System.Int32;
using nuint = global::System.UInt32;
#endif

namespace MonoTouchFixtures.CoreGraphics
{
	[TestFixture]
	[Preserve (AllMembers = true)]
	public class RectTest
	{
		[Test]
		public void Inflate ()
		{
			var rect = new RectangleF (1, 2, 3, 4);
			rect.Inflate (5, 6);
			Assert.AreEqual (-4, (int)rect.X, "x 1");
			Assert.AreEqual (-4, (int)rect.Y, "y 1");
			Assert.AreEqual (13, (int)rect.Width, "w 1");
			Assert.AreEqual (16, (int)rect.Height, "h 1");

			rect.Inflate (new SizeF (10, 20));
			Assert.AreEqual (-14, (int)rect.X, "x 2");
			Assert.AreEqual (-24, (int)rect.Y, "y 2");
			Assert.AreEqual (33, (int)rect.Width, "w 2");
			Assert.AreEqual (56, (int)rect.Height, "h 2");

			rect = RectangleF.Inflate (rect, 5, 4);
			Assert.AreEqual (-19, (int)rect.X, "x 3");
			Assert.AreEqual (-28, (int)rect.Y, "y 3");
			Assert.AreEqual (43, (int)rect.Width, "w 3");
			Assert.AreEqual (64, (int)rect.Height, "h 3");
		}

		[Test]
		public void Null ()
		{
			Assert.True (CGRect.Null.IsNull (), "Null.IsNull");
			Assert.True (CGRect.Null.IsEmpty, "Null.IsEmpty");
			Assert.False (CGRect.Null.IsInfinite (), "Null.IsInfinite");
		}

		[Test]
		public void Infinite ()
		{
			Assert.True (CGRect.Infinite.IsInfinite (), "Infinite.IsInfinite");
			Assert.False (CGRect.Infinite.IsEmpty, "Infinite.IsEmpty");
			Assert.False (CGRect.Infinite.IsNull (), "Infinite.IsNull");
		}

		[Test]
		public void Empty ()
		{
			Assert.True (CGRect.Empty.IsEmpty, "Empty.IsEmpty");
			Assert.False (CGRect.Empty.IsNull (), "Empty.IsNull");
			Assert.False (CGRect.Empty.IsInfinite (), "Empty.IsInfinite");

			// for System.Drawing compatibility this was named Empty - test confirms it's identical to CGRectZero
			var handle = Dlfcn.dlopen (Constants.CoreGraphicsLibrary, 0);
			try {
				var zero = Dlfcn.GetCGRect (handle, "CGRectZero");
				Assert.AreEqual (CGRect.Empty, zero, "CGRectZero");
			} finally {
				Dlfcn.dlclose (handle);
			}
		}
	}
}
