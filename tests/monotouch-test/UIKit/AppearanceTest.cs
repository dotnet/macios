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
				Assert.That (a == b, Is.True, "1");
				Assert.That (a == c, Is.False, "2");
			}
		}

		[Test]
		public void Inequality ()
		{
			using (var a = UITableView.Appearance)
			using (var b = UITableView.Appearance)
			using (var c = UILabel.Appearance) {
				Assert.That (a != b, Is.False, "1");
				Assert.That (a != c, Is.True, "2");
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
				Assert.That (UILabel.Appearance.TextColor, Is.Null, "null 1");
				UILabel.Appearance.TextColor = UIColor.Red;
				UILabel.Appearance.TextColor.GetRGBA (out r, out g, out b, out a);
				Assert.That (a, Is.EqualTo ((nfloat) 1), "a1");
				Assert.That (r, Is.EqualTo ((nfloat) 1), "r1");
				Assert.That (g, Is.EqualTo ((nfloat) 0), "g1");
				Assert.That (b, Is.EqualTo ((nfloat) 0), "b1");

				// check that other appearance instances didn't change
				Assert.That (UILabel.GetAppearance (traits).TextColor, Is.Null, "other null 2");
				Assert.That (UILabel.GetAppearance (traits, typeof (UITextField)).TextColor, Is.Null, "other null 3");
				Assert.That (UILabel.AppearanceWhenContainedIn (typeof (UITextField)).TextColor, Is.Null, "other null 4");

				// it can be cleared
				UILabel.Appearance.TextColor = null;
				Assert.That (UILabel.Appearance.TextColor, Is.Null, "null 2");
			}
		}

		[Test]
		public void AppearanceWhenContainedIn ()
		{
			TestRuntime.AssertSystemVersion (ApplePlatform.iOS, 8, 0, throwIfOtherPlatform: false);

			using (var traits = new UITraitCollection ()) {
				nfloat r, g, b, a;

				// it can be set
				Assert.That (UILabel.AppearanceWhenContainedIn (typeof (UITextField)).TextColor, Is.Null, "null 1");
				UILabel.AppearanceWhenContainedIn (typeof (UITextField)).TextColor = UIColor.Blue;
				UILabel.AppearanceWhenContainedIn (typeof (UITextField)).TextColor.GetRGBA (out r, out g, out b, out a);
				Assert.That (a, Is.EqualTo ((nfloat) 1), "a1");
				Assert.That (r, Is.EqualTo ((nfloat) 0), "r1");
				Assert.That (g, Is.EqualTo ((nfloat) 0), "g1");
				Assert.That (b, Is.EqualTo ((nfloat) 1), "b1");

				// check that other appearance instances didn't change (bug 26353)
				Assert.That (UILabel.Appearance.TextColor, Is.Null, "other null 1");
				Assert.That (UILabel.GetAppearance (traits).TextColor, Is.Null, "other null 2");
				Assert.That (UILabel.GetAppearance (traits, typeof (UITextField)).TextColor, Is.Null, "other null 3");

				// it can be cleared
				UILabel.AppearanceWhenContainedIn (typeof (UITextField)).TextColor = null;
				Assert.That (UILabel.AppearanceWhenContainedIn (typeof (UITextField)).TextColor, Is.Null, "null 2");
			}
		}

		[TestCase (0, false)]
		[TestCase (0, true)]
		[TestCase (1, false)]
		[TestCase (1, true)]
		[TestCase (2, false)]
		[TestCase (2, true)]
		public void AppearanceWhenContainedInInstancesOfClasses (int containerCount, bool withTraits)
		{
			Type [] containers = { typeof (UITextField), typeof (UIView) };
			Array.Resize (ref containers, containerCount);
			var handles = new NativeHandle [containerCount];
			for (int i = 0; i < containerCount; i++)
				handles [i] = Class.GetHandle (containers [i]);

			using (var array = NSArray.FromIntPtrs (handles))
			using (var traits = UITraitCollection.FromHorizontalSizeClass (UIUserInterfaceSizeClass.Compact))
			using (var appearance = withTraits ? UILabel.GetAppearance (traits, containers) : UILabel.AppearanceWhenContainedIn (containers)) {
				var expected = withTraits
					? Messaging.IntPtr_objc_msgSend_IntPtr_IntPtr (Class.GetHandle (typeof (UILabel)), Selector.GetHandle ("appearanceForTraitCollection:whenContainedInInstancesOfClasses:"), traits.Handle, array.Handle)
					: Messaging.IntPtr_objc_msgSend_IntPtr (Class.GetHandle (typeof (UILabel)), Selector.GetHandle ("appearanceWhenContainedInInstancesOfClasses:"), array.Handle);

				Assert.That ((IntPtr) appearance.Handle, Is.EqualTo (expected), "Appearance proxy");
			}
		}


		[Test]
		public void AppearanceWhenContainedIn_UITraitCollection ()
		{
			TestRuntime.AssertSystemVersion (ApplePlatform.iOS, 8, 0, throwIfOtherPlatform: false);

			using (var traits = new UITraitCollection ()) {
				nfloat r, g, b, a;

				// it can be set
				Assert.That (UILabel.GetAppearance (traits, typeof (UITextField)).TextColor, Is.Null, "null 1");
				UILabel.GetAppearance (traits, typeof (UITextField)).TextColor = UIColor.Blue;
				UILabel.GetAppearance (traits, typeof (UITextField)).TextColor.GetRGBA (out r, out g, out b, out a);
				Assert.That (a, Is.EqualTo ((nfloat) 1), "a1");
				Assert.That (r, Is.EqualTo ((nfloat) 0), "r1");
				Assert.That (g, Is.EqualTo ((nfloat) 0), "g1");
				Assert.That (b, Is.EqualTo ((nfloat) 1), "b1");

				// check that other appearance instances didn't change
				Assert.That (UILabel.Appearance.TextColor, Is.Null, "other null 1");
				Assert.That (UILabel.GetAppearance (traits).TextColor, Is.Null, "other null 2");
				Assert.That (UILabel.AppearanceWhenContainedIn (typeof (UITextField)).TextColor, Is.Null, "other null 4");

				// it can be cleared
				UILabel.GetAppearance (traits, typeof (UITextField)).TextColor = null;
				Assert.That (UILabel.GetAppearance (traits, typeof (UITextField)).TextColor, Is.Null, "null 2");
			}

			using (var traits = new UITraitCollection ()) {
				nfloat r, g, b, a;

				// it can be set
				Assert.That (UILabel.GetAppearance<CustomLabel> (traits, typeof (UITextField)).TextColor, Is.Null, "g null 1");
				UILabel.GetAppearance<CustomLabel> (traits, typeof (UITextField)).TextColor = UIColor.Blue;
				UILabel.GetAppearance<CustomLabel> (traits, typeof (UITextField)).TextColor.GetRGBA (out r, out g, out b, out a);
				Assert.That (a, Is.EqualTo ((nfloat) 1), "g a1");
				Assert.That (r, Is.EqualTo ((nfloat) 0), "g r1");
				Assert.That (g, Is.EqualTo ((nfloat) 0), "g g1");
				Assert.That (b, Is.EqualTo ((nfloat) 1), "g b1");

				// check that other appearance instances didn't change
				Assert.That (UILabel.Appearance.TextColor, Is.Null, "g other null 1");
				Assert.That (UILabel.GetAppearance<CustomLabel> (traits).TextColor, Is.Null, "g other null 2");
				Assert.That (UILabel.AppearanceWhenContainedIn (typeof (UITextField)).TextColor, Is.Null, "g other null 4");

				// it can be cleared
				UILabel.GetAppearance<CustomLabel> (traits, typeof (UITextField)).TextColor = null;
				Assert.That (UILabel.GetAppearance<CustomLabel> (traits, typeof (UITextField)).TextColor, Is.Null, "g null 2");
			}
		}

		[Test]
		public void Appearance_UITraitCollection ()
		{
			TestRuntime.AssertSystemVersion (ApplePlatform.iOS, 8, 0, throwIfOtherPlatform: false);

			using (var traits = new UITraitCollection ()) {
				nfloat r, g, b, a;

				// it can be set
				Assert.That (UILabel.GetAppearance (traits).TextColor, Is.Null, "null 1");
				UILabel.GetAppearance (traits).TextColor = UIColor.Blue;
				UILabel.GetAppearance (traits).TextColor.GetRGBA (out r, out g, out b, out a);
				Assert.That (a, Is.EqualTo ((nfloat) 1), "a1");
				Assert.That (r, Is.EqualTo ((nfloat) 0), "r1");
				Assert.That (g, Is.EqualTo ((nfloat) 0), "g1");
				Assert.That (b, Is.EqualTo ((nfloat) 1), "b1");

				// check that other appearance instances didn't change
				Assert.That (UILabel.Appearance.TextColor, Is.Null, "other null 1");
				Assert.That (UILabel.GetAppearance (traits, typeof (UITextField)).TextColor, Is.Null, "other null 3");
				Assert.That (UILabel.AppearanceWhenContainedIn (typeof (UITextField)).TextColor, Is.Null, "other null 4");

				// it can be cleared
				UILabel.GetAppearance (traits).TextColor = null;
				Assert.That (UILabel.GetAppearance (traits).TextColor, Is.Null, "null 2");
			}

			// generic version
			using (var traits = new UITraitCollection ()) {
				nfloat r, g, b, a;

				// it can be set
				Assert.That (UILabel.GetAppearance<CustomLabel> (traits).TextColor, Is.Null, "g null 1");
				UILabel.GetAppearance<CustomLabel> (traits).TextColor = UIColor.Blue;
				UILabel.GetAppearance<CustomLabel> (traits).TextColor.GetRGBA (out r, out g, out b, out a);
				Assert.That (a, Is.EqualTo ((nfloat) 1), "g a1");
				Assert.That (r, Is.EqualTo ((nfloat) 0), "g r1");
				Assert.That (g, Is.EqualTo ((nfloat) 0), "g g1");
				Assert.That (b, Is.EqualTo ((nfloat) 1), "g b1");

				// check that other appearance instances didn't change
				Assert.That (UILabel.Appearance.TextColor, Is.Null, "g other null 1");
				Assert.That (UILabel.GetAppearance<CustomLabel> (traits, typeof (UITextField)).TextColor, Is.Null, "g other null 3");
				Assert.That (UILabel.AppearanceWhenContainedIn (typeof (UITextField)).TextColor, Is.Null, "g other null 4");

				// it can be cleared
				UILabel.GetAppearance<CustomLabel> (traits).TextColor = null;
				Assert.That (UILabel.GetAppearance<CustomLabel> (traits).TextColor, Is.Null, "g null 2");
			}
		}

		class CustomLabel : UILabel { }
	}
}

#endif // !MONOMAC
