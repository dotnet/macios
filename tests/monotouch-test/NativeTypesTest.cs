//
// Unit tests for native types (nint, nuint, nfloat)
//
// Authors:
//	Rolf Bjarne Kvinge <rolf@xamarin.com>
//
// Copyright 2014 Xamarin Inc. All rights reserved.
//

using System.Globalization;

using Security;

using RectangleF = CoreGraphics.CGRect;
using SizeF = CoreGraphics.CGSize;
using PointF = CoreGraphics.CGPoint;

namespace MonoTouchFixtures.System {

	[TestFixture]
	[Preserve (AllMembers = true)]
	public class NativeTypes {

		[Test]
		public void CompareTo ()
		{
			Assert.That (((nint) 0).CompareTo ((nint) 0), Is.EqualTo (0), "compareto 1");
			Assert.That (((nint) 0).CompareTo ((nint) 1), Is.EqualTo (-1), "compareto 2");
			Assert.That (((nint) 1).CompareTo ((nint) 0), Is.EqualTo (1), "compareto 3");
			Assert.That (((nint) 0).CompareTo ((object) (nint) 0), Is.EqualTo (0), "compareto 4");
			Assert.That (((nint) 0).CompareTo ((object) (nint) 1), Is.EqualTo (-1), "compareto 5");
			Assert.That (((nint) 1).CompareTo ((object) (nint) 0), Is.EqualTo (1), "compareto 6");
			Assert.That (((nint) 1).CompareTo (null), Is.EqualTo (1), "compareto 7");


			Assert.That (((nuint) 0).CompareTo ((nuint) 0), Is.EqualTo (0), "compareto nuint 1");
			Assert.That (((nuint) 0).CompareTo ((nuint) 1), Is.EqualTo (-1), "compareto nuint 2");
			Assert.That (((nuint) 1).CompareTo ((nuint) 0), Is.EqualTo (1), "compareto nuint 3");
			Assert.That (((nuint) 0).CompareTo ((object) (nuint) 0), Is.EqualTo (0), "compareto nuint 4");
			Assert.That (((nuint) 0).CompareTo ((object) (nuint) 1), Is.EqualTo (-1), "compareto nuint 5");
			Assert.That (((nuint) 1).CompareTo ((object) (nuint) 0), Is.EqualTo (1), "compareto nuint 6");
			Assert.That (((nuint) 1).CompareTo (null), Is.EqualTo (1), "compareto nuint 7");

			Assert.That (((nfloat) 0).CompareTo ((nfloat) 0), Is.EqualTo (0), "compareto nfloat 1");
			Assert.That (((nfloat) 0).CompareTo ((nfloat) 1), Is.EqualTo (-1), "compareto nfloat 2");
			Assert.That (((nfloat) 1).CompareTo ((nfloat) 0), Is.EqualTo (1), "compareto nfloat 3");
			Assert.That (((nfloat) 0).CompareTo ((object) (nfloat) 0), Is.EqualTo (0), "compareto nfloat 4");
			Assert.That (((nfloat) 0).CompareTo ((object) (nfloat) 1), Is.EqualTo (-1), "compareto nfloat 5");
			Assert.That (((nfloat) 1).CompareTo ((object) (nfloat) 0), Is.EqualTo (1), "compareto nfloat 6");
			Assert.That (((nfloat) 1).CompareTo (null), Is.EqualTo (1), "compareto nfloat 7");
		}

		[Test]
		public void Equals ()
		{
			ClassicAssert.IsTrue (((nint) 0).Equals ((nint) 0), "eq nint 1");
			ClassicAssert.IsTrue (((nint) 0).Equals ((object) (nint) 0), "eq nint 2");
			ClassicAssert.IsFalse (((nint) 0).Equals (null), "eq nint 3");

			ClassicAssert.IsTrue (((nuint) 0).Equals ((nuint) 0), "eq nuint 1");
			ClassicAssert.IsTrue (((nuint) 0).Equals ((object) (nuint) 0), "eq nuint 2");
			ClassicAssert.IsFalse (((nuint) 0).Equals (null), "eq nuint 3");

			ClassicAssert.IsTrue (((nfloat) 0).Equals ((nfloat) 0), "eq nfloat 1");
			ClassicAssert.IsTrue (((nfloat) 0).Equals ((object) (nfloat) 0), "eq nfloat 2");
			ClassicAssert.IsFalse (((nfloat) 0).Equals (null), "eq nfloat 3");
		}

		[Test]
		public void IsInfinity ()
		{
			ClassicAssert.IsTrue (nfloat.IsInfinity (nfloat.PositiveInfinity), "PositiveInfinity");
			ClassicAssert.IsTrue (nfloat.IsInfinity (nfloat.NegativeInfinity), "NegativeInfinity");
			ClassicAssert.IsTrue (!nfloat.IsInfinity (12), "12");
			ClassicAssert.IsTrue (!nfloat.IsInfinity (nfloat.NaN), "NaN");
		}

		[Test]
		public void IsNan ()
		{
			ClassicAssert.IsTrue (nfloat.IsNaN (nfloat.NaN), "Nan");
			ClassicAssert.IsTrue (!nfloat.IsNaN (12), "12");
			ClassicAssert.IsTrue (!nfloat.IsNaN (nfloat.PositiveInfinity), "PositiveInfinity");
			ClassicAssert.IsTrue (!nfloat.IsNaN (nfloat.PositiveInfinity), "NegativeInfinity");
		}

		[Test]
		public void IsNegativeInfinity ()
		{
			ClassicAssert.IsTrue (nfloat.IsNegativeInfinity (nfloat.NegativeInfinity), "IsNegativeInfinity");
			ClassicAssert.IsTrue (!nfloat.IsNegativeInfinity (12), "12");
			ClassicAssert.IsTrue (!nfloat.IsNegativeInfinity (nfloat.NaN), "NaN");
		}

		[Test]
		public void IsPositiveInfinity ()
		{
			ClassicAssert.IsTrue (nfloat.IsPositiveInfinity (nfloat.PositiveInfinity), "PositiveInfinity");
			ClassicAssert.IsTrue (!nfloat.IsPositiveInfinity (12), "12");
			ClassicAssert.IsTrue (!nfloat.IsPositiveInfinity (nfloat.NaN), "NaN");
		}

		[Test]
		public void PositiveInfinity_Cast ()
		{
			float f = float.PositiveInfinity;
			ClassicAssert.IsTrue (float.IsPositiveInfinity (f), "float PositiveInfinity");
			nfloat n = (nfloat) f; // no-op on 32 bits arch
			ClassicAssert.IsTrue (nfloat.IsPositiveInfinity (n), "nfloat PositiveInfinity 1");

			double d = double.PositiveInfinity;
			ClassicAssert.IsTrue (double.IsPositiveInfinity (d), "double PositiveInfinity");
			n = (nfloat) d; // no-op on 64 bits arch
			ClassicAssert.IsTrue (nfloat.IsPositiveInfinity (n), "nfloat PositiveInfinity 2");
		}

		[Test]
		public void NegativeInfinity_Cast ()
		{
			float f = float.NegativeInfinity;
			ClassicAssert.IsTrue (float.IsNegativeInfinity (f), "float NegativeInfinity");
			nfloat n = (nfloat) f; // no-op on 32 bits arch
			ClassicAssert.IsTrue (nfloat.IsNegativeInfinity (n), "nfloat NegativeInfinity 1");

			double d = double.NegativeInfinity;
			ClassicAssert.IsTrue (double.IsNegativeInfinity (d), "double NegativeInfinity");
			n = (nfloat) d; // no-op on 64 bits arch
			ClassicAssert.IsTrue (nfloat.IsNegativeInfinity (n), "nfloat NegativeInfinity 2");
		}

		[Test]
		public void NaN_Cast ()
		{
			float f = float.NaN;
			ClassicAssert.IsTrue (float.IsNaN (f), "float NaN");
			nfloat n = (nfloat) f; // no-op on 32 bits arch
			ClassicAssert.IsTrue (nfloat.IsNaN (n), "nfloat NaN 1");

			double d = double.NaN;
			ClassicAssert.IsTrue (double.IsNaN (d), "double NaN");
			n = (nfloat) d; // no-op on 64 bits arch
			ClassicAssert.IsTrue (nfloat.IsNaN (n), "nfloat NaN 2");
		}
	}
}
