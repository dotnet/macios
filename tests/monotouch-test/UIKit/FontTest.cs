// Copyright 2011 Xamarin Inc. All rights reserved

#if !MONOMAC
using System.Drawing;
using UIKit;

namespace MonoTouchFixtures.UIKit {

	[TestFixture]
	[Preserve (AllMembers = true)]
	public class FontTest {

		void AssertNotBrokenFontWithSize ()
		{
#if __MACCATALYST__ || __MACOS__
			if (TestRuntime.CheckXcodeVersion (11, 0) && !TestRuntime.CheckXcodeVersion (12, 0)) {
				// Yep, GameKit breaks [UIKit fontWithSize] on macOS 10.15...
				Assert.Ignore ("GameKit breaks UIKIt.FontWithSize on this OS version. Ref: https://github.com/xamarin/maccore/issues/2382");
			}
#endif
		}

		[Test]
		public void WithSize ()
		{
			AssertNotBrokenFontWithSize ();
			var f1 = UIFont.SystemFontOfSize (10).WithSize (20);
			Assert.That ((nfloat) 20, Is.EqualTo (f1.PointSize), "#size");
		}

		[Test]
		public void GetWeight ()
		{
			var weight = UIFontWeight.Semibold;
			Assert.That (UIFontWeightConstants.Semibold, Is.EqualTo (weight.GetWeight ()));
		}

		[Test]
		public void TestDescriptors ()
		{
			TestRuntime.AssertXcodeVersion (5, 0);

			var font = UIFont.BoldSystemFontOfSize (80);
			var descriptor = font.FontDescriptor;

			// Ensure that Apple does not break things behind us, they documented
			// the size property as being a string, but it is a number (makes sense)
			// but make sure we dont regress if they fix it.

			var size = descriptor.FontAttributes.Size;
			Assert.That (size.HasValue, Is.EqualTo (true));
			Assert.That (size.Value, Is.EqualTo (80.0f));
		}

		// ref: https://trello.com/c/wKZyugio/437-many-managed-peers-on-a-single-native-instance

		void SemiFactory_25511 (UIFont f1, UIFont f2, string api)
		{
			using (f1) {
				// the same instance will be returned (from an iOS cache)
				Assert.That (f1.Handle, Is.EqualTo (f2.Handle), $"{api} Handle");
				// using means f1 will be disposed and it's handle will be zero'ed
				// but f2 is the same (managed) instance and _normally_ would become unusable
				// to fix this we now return a different instance - but we must still match the existing behavior
				Assert.That (f1 == f2, Is.True, $"{api} ==");
				Assert.That (f1.Equals ((object) f2), Is.True, $"{api} Equals(object)");
				// IEquatable<NSObject> is only in unified - otherwise it would be the same call as above
				Assert.That (f1.Equals (f2), Is.True, $"{api} Equals");
			}
			Assert.That (f1.Handle, Is.EqualTo (NativeHandle.Zero), $"{api} 1");
			// without our "fix" that would be the same managed instance (as f1) and the handle would be nil
			Assert.That (f2.Handle, Is.Not.EqualTo (NativeHandle.Zero), $"{api} 2");
		}

		[Test]
		public void Methods ()
		{
			var f1 = UIFont.FromName ("Helvetica", 20.0f);
			// the same instance will be returned (from an iOS cache)
			var f2 = UIFont.FromName ("Helvetica", 20.0f);
			// first instance will be disposed and the 2nd one needs to stay valid
			SemiFactory_25511 (f1, f2, "FromName");

			f1 = UIFont.SystemFontOfSize (12);
			f2 = UIFont.SystemFontOfSize (12);
			SemiFactory_25511 (f1, f2, "SystemFontOfSize");

			f1 = UIFont.BoldSystemFontOfSize (12);
			f2 = UIFont.BoldSystemFontOfSize (12);
			SemiFactory_25511 (f1, f2, "BoldSystemFontOfSize");

			f1 = UIFont.ItalicSystemFontOfSize (12);
			f2 = UIFont.ItalicSystemFontOfSize (12);
			SemiFactory_25511 (f1, f2, "ItalicSystemFontOfSize");

			f1 = UIFont.SystemFontOfSize (12);
			f2 = UIFont.SystemFontOfSize (12);
			SemiFactory_25511 (f1, f2, "SystemFontOfSize");

#if !__MACCATALYST__ // https://github.com/xamarin/maccore/issues/2382
			// instance
			f1 = f2.WithSize (12);
			f2 = f2.WithSize (12);
			SemiFactory_25511 (f1, f2, "WithSize");
#endif

			if (!TestRuntime.CheckXcodeVersion (5, 0))
				return;

			using (var name = new NSString ("UICTFontTextStyleBody")) {
				f1 = UIFont.GetPreferredFontForTextStyle (name);
				f2 = UIFont.GetPreferredFontForTextStyle (name);
				SemiFactory_25511 (f1, f2, "GetPreferredFontForTextStyle");
			}

			var d = f2.FontDescriptor;
			f1 = UIFont.FromDescriptor (d, 12);
			f2 = UIFont.FromDescriptor (d, 12);
			SemiFactory_25511 (f1, f2, "FromDescriptor");

			if (!TestRuntime.CheckXcodeVersion (14, 0))
				return;

			f1 = UIFont.SystemFontOfSize (12, UIFontWeight.UltraLight, UIFontWidth.Condensed);
			f2 = UIFont.SystemFontOfSize (12, UIFontWeight.UltraLight, UIFontWidth.Condensed);
			SemiFactory_25511 (f1, f2, "SystemFontOfSizeWeightWidth");
		}

		[Test]
		public void Properties ()
		{
			TestRuntime.AssertXcodeVersion (5, 0);

			var f1 = UIFont.PreferredBody;
			// the same instance will be returned (from an iOS cache)
			var f2 = UIFont.PreferredBody;
			// first instance will be disposed and the 2nd one needs to stay valid
			SemiFactory_25511 (f1, f2, "PreferredBody");

			f1 = UIFont.PreferredCaption1;
			f2 = UIFont.PreferredCaption1;
			SemiFactory_25511 (f1, f2, "PreferredCaption1");

			f1 = UIFont.PreferredCaption2;
			f2 = UIFont.PreferredCaption2;
			SemiFactory_25511 (f1, f2, "PreferredCaption2");

			f1 = UIFont.PreferredFootnote;
			f2 = UIFont.PreferredFootnote;
			SemiFactory_25511 (f1, f2, "PreferredFootnote");

			f1 = UIFont.PreferredHeadline;
			f2 = UIFont.PreferredHeadline;
			SemiFactory_25511 (f1, f2, "PreferredHeadline");

			f1 = UIFont.PreferredSubheadline;
			f2 = UIFont.PreferredSubheadline;
			SemiFactory_25511 (f1, f2, "PreferredSubheadline");
		}

		[Test]
		public void NullFonts ()
		{
			var invalidFontName = new NSString ("Invalid Font Name");
			if (TestRuntime.CheckXcodeVersion (5, 0)) {
				Assert.That (UIFont.GetPreferredFontForTextStyle (invalidFontName), Is.Not.Null, "GetPreferredFontForTextStyle");
				Assert.That (UIFont.FromDescriptor (new UIFontDescriptor (), -2), Is.Not.Null, "FromDescriptor (,)");
			}

			Assert.That (UIFont.FromName (invalidFontName, 1), Is.Null, "FromName");

			Assert.That (UIFont.SystemFontOfSize (-3), Is.Not.Null, "SystemFontOfSize()");

			if (TestRuntime.CheckXcodeVersion (6, 2)) {
				Assert.That (UIFont.SystemFontOfSize (0, UIFontWeight.Regular), Is.Not.Null, "SystemFontOfSize (nfloat, UIFontWeight)");
				Assert.That (UIFont.SystemFontOfSize (0, (nfloat) 0), Is.Not.Null, "SystemFontOfSize (nfloat, nfloat)");
			}

			Assert.That (UIFont.BoldSystemFontOfSize (-4), Is.Not.Null, "BoldSystemFontOfSize");
			Assert.That (UIFont.ItalicSystemFontOfSize (-5), Is.Not.Null, "ItalicSystemFontOfSize");

			AssertNotBrokenFontWithSize ();
			using (var font = UIFont.SystemFontOfSize (12)) {
				Assert.That (font.WithSize (-6), Is.Not.Null, "WithSize");
			}
		}
	}
}
#endif
