//
// UIAppearance Unit Tests
//
// Authors:
//	Sebastien Pouliot  <sebastien@xamarin.com>
//
// Copyright 2014 Xamarin Inc. All rights reserved.
//

#if !MONOMAC

using UIKit;

using Xamarin.Utils;

namespace MonoTouchFixtures.UIKit {

	[TestFixture]
	[Preserve (AllMembers = true)]
	public class AppearanceTest {

		[Test]
		public void Equality ()
		{
			using (var a = UITableView.Appearance)
			using (var b = UITableView.Appearance)
			using (var c = UILabel.Appearance) {
				ClassicAssert.True (a == b, "1");
				ClassicAssert.False (a == c, "2");
			}
		}

		[Test]
		public void Inequality ()
		{
			using (var a = UITableView.Appearance)
			using (var b = UITableView.Appearance)
			using (var c = UILabel.Appearance) {
				ClassicAssert.False (a != b, "1");
				ClassicAssert.True (a != c, "2");
			}
		}

		[Test]
		public void Appearance ()
		{
			TestRuntime.AssertSystemVersion (ApplePlatform.iOS, 8, 0, throwIfOtherPlatform: false);

			using (var traits = new UITraitCollection ()) {
				nfloat r, g, b, a;

				// Appearance
				// it can be set
				ClassicAssert.IsNull (UILabel.Appearance.TextColor, "null 1");
				UILabel.Appearance.TextColor = UIColor.Red;
				UILabel.Appearance.TextColor.GetRGBA (out r, out g, out b, out a);
				ClassicAssert.AreEqual ((nfloat) 1, a, "a1");
				ClassicAssert.AreEqual ((nfloat) 1, r, "r1");
				ClassicAssert.AreEqual ((nfloat) 0, g, "g1");
				ClassicAssert.AreEqual ((nfloat) 0, b, "b1");

				// check that other appearance instances didn't change
				ClassicAssert.IsNull (UILabel.GetAppearance (traits).TextColor, "other null 2");
				ClassicAssert.IsNull (UILabel.GetAppearance (traits, typeof (UITextField)).TextColor, "other null 3");
				ClassicAssert.IsNull (UILabel.AppearanceWhenContainedIn (typeof (UITextField)).TextColor, "other null 4");

				// it can be cleared
				UILabel.Appearance.TextColor = null;
				ClassicAssert.IsNull (UILabel.Appearance.TextColor, "null 2");
			}
		}

		[Test]
		public void AppearanceWhenContainedIn ()
		{
			TestRuntime.AssertSystemVersion (ApplePlatform.iOS, 8, 0, throwIfOtherPlatform: false);

			using (var traits = new UITraitCollection ()) {
				nfloat r, g, b, a;

				// it can be set
				ClassicAssert.IsNull (UILabel.AppearanceWhenContainedIn (typeof (UITextField)).TextColor, "null 1");
				UILabel.AppearanceWhenContainedIn (typeof (UITextField)).TextColor = UIColor.Blue;
				UILabel.AppearanceWhenContainedIn (typeof (UITextField)).TextColor.GetRGBA (out r, out g, out b, out a);
				ClassicAssert.AreEqual ((nfloat) 1, a, "a1");
				ClassicAssert.AreEqual ((nfloat) 0, r, "r1");
				ClassicAssert.AreEqual ((nfloat) 0, g, "g1");
				ClassicAssert.AreEqual ((nfloat) 1, b, "b1");

				// check that other appearance instances didn't change (bug 26353)
				ClassicAssert.IsNull (UILabel.Appearance.TextColor, "other null 1");
				ClassicAssert.IsNull (UILabel.GetAppearance (traits).TextColor, "other null 2");
				ClassicAssert.IsNull (UILabel.GetAppearance (traits, typeof (UITextField)).TextColor, "other null 3");

				// it can be cleared
				UILabel.AppearanceWhenContainedIn (typeof (UITextField)).TextColor = null;
				ClassicAssert.IsNull (UILabel.AppearanceWhenContainedIn (typeof (UITextField)).TextColor, "null 2");
			}
		}


		[Test]
		public void AppearanceWhenContainedIn_UITraitCollection ()
		{
			TestRuntime.AssertSystemVersion (ApplePlatform.iOS, 8, 0, throwIfOtherPlatform: false);

			using (var traits = new UITraitCollection ()) {
				nfloat r, g, b, a;

				// it can be set
				ClassicAssert.IsNull (UILabel.GetAppearance (traits, typeof (UITextField)).TextColor, "null 1");
				UILabel.GetAppearance (traits, typeof (UITextField)).TextColor = UIColor.Blue;
				UILabel.GetAppearance (traits, typeof (UITextField)).TextColor.GetRGBA (out r, out g, out b, out a);
				ClassicAssert.AreEqual ((nfloat) 1, a, "a1");
				ClassicAssert.AreEqual ((nfloat) 0, r, "r1");
				ClassicAssert.AreEqual ((nfloat) 0, g, "g1");
				ClassicAssert.AreEqual ((nfloat) 1, b, "b1");

				// check that other appearance instances didn't change
				ClassicAssert.IsNull (UILabel.Appearance.TextColor, "other null 1");
				ClassicAssert.IsNull (UILabel.GetAppearance (traits).TextColor, "other null 2");
				ClassicAssert.IsNull (UILabel.AppearanceWhenContainedIn (typeof (UITextField)).TextColor, "other null 4");

				// it can be cleared
				UILabel.GetAppearance (traits, typeof (UITextField)).TextColor = null;
				ClassicAssert.IsNull (UILabel.GetAppearance (traits, typeof (UITextField)).TextColor, "null 2");
			}

			using (var traits = new UITraitCollection ()) {
				nfloat r, g, b, a;

				// it can be set
				ClassicAssert.IsNull (UILabel.GetAppearance<CustomLabel> (traits, typeof (UITextField)).TextColor, "g null 1");
				UILabel.GetAppearance<CustomLabel> (traits, typeof (UITextField)).TextColor = UIColor.Blue;
				UILabel.GetAppearance<CustomLabel> (traits, typeof (UITextField)).TextColor.GetRGBA (out r, out g, out b, out a);
				ClassicAssert.AreEqual ((nfloat) 1, a, "g a1");
				ClassicAssert.AreEqual ((nfloat) 0, r, "g r1");
				ClassicAssert.AreEqual ((nfloat) 0, g, "g g1");
				ClassicAssert.AreEqual ((nfloat) 1, b, "g b1");

				// check that other appearance instances didn't change
				ClassicAssert.IsNull (UILabel.Appearance.TextColor, "g other null 1");
				ClassicAssert.IsNull (UILabel.GetAppearance<CustomLabel> (traits).TextColor, "g other null 2");
				ClassicAssert.IsNull (UILabel.AppearanceWhenContainedIn (typeof (UITextField)).TextColor, "g other null 4");

				// it can be cleared
				UILabel.GetAppearance<CustomLabel> (traits, typeof (UITextField)).TextColor = null;
				ClassicAssert.IsNull (UILabel.GetAppearance<CustomLabel> (traits, typeof (UITextField)).TextColor, "g null 2");
			}
		}

		[Test]
		public void Appearance_UITraitCollection ()
		{
			TestRuntime.AssertSystemVersion (ApplePlatform.iOS, 8, 0, throwIfOtherPlatform: false);

			using (var traits = new UITraitCollection ()) {
				nfloat r, g, b, a;

				// it can be set
				ClassicAssert.IsNull (UILabel.GetAppearance (traits).TextColor, "null 1");
				UILabel.GetAppearance (traits).TextColor = UIColor.Blue;
				UILabel.GetAppearance (traits).TextColor.GetRGBA (out r, out g, out b, out a);
				ClassicAssert.AreEqual ((nfloat) 1, a, "a1");
				ClassicAssert.AreEqual ((nfloat) 0, r, "r1");
				ClassicAssert.AreEqual ((nfloat) 0, g, "g1");
				ClassicAssert.AreEqual ((nfloat) 1, b, "b1");

				// check that other appearance instances didn't change
				ClassicAssert.IsNull (UILabel.Appearance.TextColor, "other null 1");
				ClassicAssert.IsNull (UILabel.GetAppearance (traits, typeof (UITextField)).TextColor, "other null 3");
				ClassicAssert.IsNull (UILabel.AppearanceWhenContainedIn (typeof (UITextField)).TextColor, "other null 4");

				// it can be cleared
				UILabel.GetAppearance (traits).TextColor = null;
				ClassicAssert.IsNull (UILabel.GetAppearance (traits).TextColor, "null 2");
			}

			// generic version
			using (var traits = new UITraitCollection ()) {
				nfloat r, g, b, a;

				// it can be set
				ClassicAssert.IsNull (UILabel.GetAppearance<CustomLabel> (traits).TextColor, "g null 1");
				UILabel.GetAppearance<CustomLabel> (traits).TextColor = UIColor.Blue;
				UILabel.GetAppearance<CustomLabel> (traits).TextColor.GetRGBA (out r, out g, out b, out a);
				ClassicAssert.AreEqual ((nfloat) 1, a, "g a1");
				ClassicAssert.AreEqual ((nfloat) 0, r, "g r1");
				ClassicAssert.AreEqual ((nfloat) 0, g, "g g1");
				ClassicAssert.AreEqual ((nfloat) 1, b, "g b1");

				// check that other appearance instances didn't change
				ClassicAssert.IsNull (UILabel.Appearance.TextColor, "g other null 1");
				ClassicAssert.IsNull (UILabel.GetAppearance<CustomLabel> (traits, typeof (UITextField)).TextColor, "g other null 3");
				ClassicAssert.IsNull (UILabel.AppearanceWhenContainedIn (typeof (UITextField)).TextColor, "g other null 4");

				// it can be cleared
				UILabel.GetAppearance<CustomLabel> (traits).TextColor = null;
				ClassicAssert.IsNull (UILabel.GetAppearance<CustomLabel> (traits).TextColor, "g null 2");
			}
		}

		class CustomLabel : UILabel { }
	}
}

#endif // !MONOMAC
