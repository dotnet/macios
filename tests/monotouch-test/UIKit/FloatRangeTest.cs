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
				Assert.That (UIFloatRange.Zero.Equals (Zero), Is.True, "Zero");

				var infinite = Dlfcn.dlsym (uikit, "UIFloatRangeInfinite");
				var Infinite = Marshal.PtrToStructure<UIFloatRange> (infinite);
				Assert.That (Infinite.IsInfinite, Is.True, "IsInfinite");
				Assert.That (UIFloatRange.Infinite.Equals (Infinite), Is.False, "Infinite");
			} finally {
				Dlfcn.dlclose (uikit);
			}
		}

		[Test]
		public void IsInfinite ()
		{
			TestRuntime.AssertXcodeVersion (7, 0);
			Assert.That (UIFloatRange.Infinite.IsInfinite, Is.True, "Infinite");
			Assert.That (UIFloatRange.Zero.IsInfinite, Is.False, "Zero");
		}

		[Ignore ("https://github.com/xamarin/maccore/issues/1885")]
		[Test]
		public void Equals ()
		{
			TestRuntime.AssertXcodeVersion (7, 0);
			Assert.That (UIFloatRange.Zero.Equals (UIFloatRange.Zero), Is.True, "Zero-Zero");
			var one = new UIFloatRange (1f, 1f);
			Assert.That (one.Equals (UIFloatRange.Zero), Is.False, "one-Zero");
			Assert.That (UIFloatRange.Zero.Equals ((object) one), Is.False, "Zero-one");
			Assert.That (one.Equals (one), Is.True, "one-one");

			Assert.That (UIFloatRange.Infinite.Equals (UIFloatRange.Infinite), Is.False, "Infinite-Infinite");
			Assert.That (UIFloatRange.Infinite.Equals (UIFloatRange.Zero), Is.False, "Infinite-Zero");
			Assert.That (UIFloatRange.Zero.Equals (UIFloatRange.Infinite), Is.False, "Zero-Infinite");
		}
	}
}

#endif // !MONOMAC
