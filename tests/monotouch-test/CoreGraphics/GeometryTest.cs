//
// Unit tests for CGGeometry (and related)
//
// Authors:
//	Sebastien Pouliot <sebastien@xamarin.com>
//
// Copyright 2014 Xamarin Inc. All rights reserved.
//

using CoreGraphics;

namespace MonoTouchFixtures.CoreGraphics {

	[TestFixture]
	[Preserve (AllMembers = true)]
	public class GeometryTest {

		static public readonly IntPtr Handle = Dlfcn.dlopen (Constants.CoreGraphicsLibrary, 0);

		public static CGRect GetRect (IntPtr indirect)
		{
			if (indirect == IntPtr.Zero)
				return CGRect.Empty;
			unsafe {
				nfloat* ptr = (nfloat*) indirect;
				return new CGRect (ptr [0], ptr [1], ptr [2], ptr [3]);
			}
		}

		[Test]
		public void Infinite ()
		{
			var r = GetRect (Dlfcn.dlsym (Handle, "CGRectInfinite"));
			ClassicAssert.False (r.IsEmpty, "IsEmpty");
			ClassicAssert.False (r.IsNull (), "IsNull");
			ClassicAssert.True (r.IsInfinite (), "IsInfinite");
		}

		[Test]
		public void Null ()
		{
			var r = GetRect (Dlfcn.dlsym (Handle, "CGRectNull"));
			ClassicAssert.True (r.IsEmpty, "IsEmpty");
			ClassicAssert.True (r.IsNull (), "IsNull");
			ClassicAssert.False (r.IsInfinite (), "IsInfinite");
		}

		[Test]
		public void Zero ()
		{
			var r = GetRect (Dlfcn.dlsym (Handle, "CGRectZero"));
			ClassicAssert.True (r.IsEmpty, "IsEmpty");
			ClassicAssert.False (r.IsNull (), "IsNull");
			ClassicAssert.False (r.IsInfinite (), "IsInfinite");
		}
	}
}
