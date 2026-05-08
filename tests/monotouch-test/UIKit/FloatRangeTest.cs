// Authors:
//	Sebastien Pouliot <sebastien@xamarin.com>
//
// Copyright 2013, 2016 Xamarin Inc. All rights reserved.
//

#if !MONOMAC

using System.Drawing;
using UIKit;

namespace MonoTouchFixtures.UIKit {

	[TestFixture]
	[Preserve (AllMembers = true)]
	public class FloatRangeTest {

		[Ignore ("https://github.com/xamarin/maccore/issues/1885")]
		[Test]
		public void ManagedVersusNative ()
		{
			TestRuntime.AssertXcodeVersion (7, 0);
			var uikit = Dlfcn.dlopen (Constants.UIKitLibrary, 0);
			try {
				var zero = Dlfcn.dlsym (uikit, "UIFloatRangeZero");
				var Zero = Marshal.PtrToStructure<UIFloatRange> (zero);
				ClassicAssert.True (UIFloatRange.Zero.Equals (Zero), "Zero");

				var infinite = Dlfcn.dlsym (uikit, "UIFloatRangeInfinite");
				var Infinite = Marshal.PtrToStructure<UIFloatRange> (infinite);
				ClassicAssert.True (Infinite.IsInfinite, "IsInfinite");
				ClassicAssert.False (UIFloatRange.Infinite.Equals (Infinite), "Infinite");
			} finally {
				Dlfcn.dlclose (uikit);
			}
		}

		[Test]
		public void IsInfinite ()
		{
			TestRuntime.AssertXcodeVersion (7, 0);
			ClassicAssert.True (UIFloatRange.Infinite.IsInfinite, "Infinite");
			ClassicAssert.False (UIFloatRange.Zero.IsInfinite, "Zero");
		}

		[Ignore ("https://github.com/xamarin/maccore/issues/1885")]
		[Test]
		public void Equals ()
		{
			TestRuntime.AssertXcodeVersion (7, 0);
			ClassicAssert.True (UIFloatRange.Zero.Equals (UIFloatRange.Zero), "Zero-Zero");
			var one = new UIFloatRange (1f, 1f);
			ClassicAssert.False (one.Equals (UIFloatRange.Zero), "one-Zero");
			ClassicAssert.False (UIFloatRange.Zero.Equals ((object) one), "Zero-one");
			ClassicAssert.True (one.Equals (one), "one-one");

			ClassicAssert.False (UIFloatRange.Infinite.Equals (UIFloatRange.Infinite), "Infinite-Infinite");
			ClassicAssert.False (UIFloatRange.Infinite.Equals (UIFloatRange.Zero), "Infinite-Zero");
			ClassicAssert.False (UIFloatRange.Zero.Equals (UIFloatRange.Infinite), "Zero-Infinite");
		}
	}
}

#endif // !MONOMAC
