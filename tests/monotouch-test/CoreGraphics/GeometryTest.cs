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
			Assert.That (r.IsEmpty, Is.False, "IsEmpty");
			Assert.That (r.IsNull (), Is.False, "IsNull");
			Assert.That (r.IsInfinite (), Is.True, "IsInfinite");
		}

		[Test]
		public void Null ()
		{
			var r = GetRect (Dlfcn.dlsym (Handle, "CGRectNull"));
			Assert.That (r.IsEmpty, Is.True, "IsEmpty");
			Assert.That (r.IsNull (), Is.True, "IsNull");
			Assert.That (r.IsInfinite (), Is.False, "IsInfinite");
		}

		[Test]
		public void Zero ()
		{
			var r = GetRect (Dlfcn.dlsym (Handle, "CGRectZero"));
			Assert.That (r.IsEmpty, Is.True, "IsEmpty");
			Assert.That (r.IsNull (), Is.False, "IsNull");
			Assert.That (r.IsInfinite (), Is.False, "IsInfinite");
		}
	}
}
