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
			Assert.That (((nint) 0).Equals ((nint) 0), Is.True, "eq nint 1");
			Assert.That (((nint) 0).Equals ((object) (nint) 0), Is.True, "eq nint 2");
			Assert.That (((nint) 0).Equals (null), Is.False, "eq nint 3");

			Assert.That (((nuint) 0).Equals ((nuint) 0), Is.True, "eq nuint 1");
			Assert.That (((nuint) 0).Equals ((object) (nuint) 0), Is.True, "eq nuint 2");
			Assert.That (((nuint) 0).Equals (null), Is.False, "eq nuint 3");

			Assert.That (((nfloat) 0).Equals ((nfloat) 0), Is.True, "eq nfloat 1");
			Assert.That (((nfloat) 0).Equals ((object) (nfloat) 0), Is.True, "eq nfloat 2");
			Assert.That (((nfloat) 0).Equals (null), Is.False, "eq nfloat 3");
		}

		[Test]
		public void IsInfinity ()
		{
			Assert.That (nfloat.IsInfinity (nfloat.PositiveInfinity), Is.True, "PositiveInfinity");
			Assert.That (nfloat.IsInfinity (nfloat.NegativeInfinity), Is.True, "NegativeInfinity");
			Assert.That (!nfloat.IsInfinity (12), Is.True, "12");
			Assert.That (!nfloat.IsInfinity (nfloat.NaN), Is.True, "NaN");
		}

		[Test]
		public void IsNan ()
		{
			Assert.That (nfloat.IsNaN (nfloat.NaN), Is.True, "Nan");
			Assert.That (!nfloat.IsNaN (12), Is.True, "12");
			Assert.That (!nfloat.IsNaN (nfloat.PositiveInfinity), Is.True, "PositiveInfinity");
			Assert.That (!nfloat.IsNaN (nfloat.PositiveInfinity), Is.True, "NegativeInfinity");
		}

		[Test]
		public void IsNegativeInfinity ()
		{
			Assert.That (nfloat.IsNegativeInfinity (nfloat.NegativeInfinity), Is.True, "IsNegativeInfinity");
			Assert.That (!nfloat.IsNegativeInfinity (12), Is.True, "12");
			Assert.That (!nfloat.IsNegativeInfinity (nfloat.NaN), Is.True, "NaN");
		}

		[Test]
		public void IsPositiveInfinity ()
		{
			Assert.That (nfloat.IsPositiveInfinity (nfloat.PositiveInfinity), Is.True, "PositiveInfinity");
			Assert.That (!nfloat.IsPositiveInfinity (12), Is.True, "12");
			Assert.That (!nfloat.IsPositiveInfinity (nfloat.NaN), Is.True, "NaN");
		}

		[Test]
		public void PositiveInfinity_Cast ()
		{
			float f = float.PositiveInfinity;
			Assert.That (float.IsPositiveInfinity (f), Is.True, "float PositiveInfinity");
			nfloat n = (nfloat) f; // no-op on 32 bits arch
			Assert.That (nfloat.IsPositiveInfinity (n), Is.True, "nfloat PositiveInfinity 1");

			double d = double.PositiveInfinity;
			Assert.That (double.IsPositiveInfinity (d), Is.True, "double PositiveInfinity");
			n = (nfloat) d; // no-op on 64 bits arch
			Assert.That (nfloat.IsPositiveInfinity (n), Is.True, "nfloat PositiveInfinity 2");
		}

		[Test]
		public void NegativeInfinity_Cast ()
		{
			float f = float.NegativeInfinity;
			Assert.That (float.IsNegativeInfinity (f), Is.True, "float NegativeInfinity");
			nfloat n = (nfloat) f; // no-op on 32 bits arch
			Assert.That (nfloat.IsNegativeInfinity (n), Is.True, "nfloat NegativeInfinity 1");

			double d = double.NegativeInfinity;
			Assert.That (double.IsNegativeInfinity (d), Is.True, "double NegativeInfinity");
			n = (nfloat) d; // no-op on 64 bits arch
			Assert.That (nfloat.IsNegativeInfinity (n), Is.True, "nfloat NegativeInfinity 2");
		}

		[Test]
		public void NaN_Cast ()
		{
			float f = float.NaN;
			Assert.That (float.IsNaN (f), Is.True, "float NaN");
			nfloat n = (nfloat) f; // no-op on 32 bits arch
			Assert.That (nfloat.IsNaN (n), Is.True, "nfloat NaN 1");

			double d = double.NaN;
			Assert.That (double.IsNaN (d), Is.True, "double NaN");
			n = (nfloat) d; // no-op on 64 bits arch
			Assert.That (nfloat.IsNaN (n), Is.True, "nfloat NaN 2");
		}
	}
}
