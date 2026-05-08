// Copyright 2011 Xamarin Inc. All rights reserved

#if !MONOMAC

using System.Drawing;
using CoreGraphics;
using UIKit;
using Xamarin.Utils;

namespace MonoTouchFixtures.UIKit {

	[TestFixture]
	[Preserve (AllMembers = true)]
	public class ViewTest {

		[Test]
		public void InitWithFrame ()
		{
			var frame = new CGRect (10, 10, 100, 100);
			using (UIView v = new UIView (frame)) {
				Assert.That (v.Frame, Is.EqualTo (frame), "Frame");
			}
		}

		[Test]
		public void HitTest_Null ()
		{
			var frame = new CGRect (10, 10, 100, 100);
			using (UIView v = new UIView (frame)) {
				UIView result = v.HitTest (new CGPoint (-10, -10), null);
				ClassicAssert.Null (result, "outside");
				result = v.HitTest (new CGPoint (50, 50), null);
				Assert.That (result.Handle, Is.EqualTo (v.Handle), "inside");
			}
		}

		[Test]
		public void PointInside_Null ()
		{
			var frame = new CGRect (10, 10, 100, 100);
			using (UIView v = new UIView (frame)) {
				ClassicAssert.False (v.PointInside (new CGPoint (-10, -10), null), "outside");
				ClassicAssert.True (v.PointInside (new CGPoint (50, 50), null), "inside");
			}
		}

		[Test]
		public void SizeThatFits ()
		{
			// same as LinkerTest in 'linksdk' project - but won't be linked here (for simulator)
			var empty = CGSize.Empty;
			using (UIView v = new UIView ()) {
				ClassicAssert.True (v.SizeThatFits (empty).IsEmpty, "Empty");
			}
		}

		[Test]
		public void Convert_Null ()
		{
			using (UIView v = new UIView ()) {
				Assert.That (v.ConvertPointFromView (CGPoint.Empty, null), Is.EqualTo (CGPoint.Empty), "ConvertPointFromView");
				Assert.That (v.ConvertPointToView (CGPoint.Empty, null), Is.EqualTo (CGPoint.Empty), "ConvertPointToView");
				Assert.That (v.ConvertRectFromView (CGRect.Empty, null), Is.EqualTo (CGRect.Empty), "ConvertRectFromView");
				Assert.That (v.ConvertRectToView (CGRect.Empty, null), Is.EqualTo (CGRect.Empty), "ConvertRectToView");
			}
		}

		// Apple does not allow NULL on 'animations' parameters used in animate* and transition* selectors

		void Completion ()
		{
		}

		void CompletionHandler (bool finished)
		{
		}

		[Test]
		public void Animate_Null_a1 ()
		{
			Assert.Throws<ArgumentNullException> (() => UIView.Animate (1.0, null));
		}

		[Test]
		public void Animate_Null_a2 ()
		{
			Assert.Throws<ArgumentNullException> (() => UIView.Animate (1.0, null, Completion));
		}

		[Test]
		public void Animate_Null_a3 ()
		{
			Assert.Throws<ArgumentNullException> (() => UIView.Animate (1.0, 2.0, UIViewAnimationOptions.Autoreverse, null, Completion));
		}

		[Test]
		public void AnimateNotify_Null_a1 ()
		{
			Assert.Throws<ArgumentNullException> (() => UIView.AnimateNotify (1.0, null, CompletionHandler));
		}

		[Test]
		public void AnimateNotify_Null_a2 ()
		{
			Assert.Throws<ArgumentNullException> (() => UIView.AnimateNotify (1.0, 2.0, UIViewAnimationOptions.Autoreverse, null, CompletionHandler));
		}

		[Test]
		public void Transition_Null_a1 ()
		{
			using (UIView v = new UIView ()) {
				UIView.Transition (v, 1.0, UIViewAnimationOptions.AllowAnimatedContent, null, Completion);
			}
		}

		[Test]
		public void TransitionNotify_Null_a1 ()
		{
			using (UIView v = new UIView ()) {
				UIView.TransitionNotify (v, 1.0, UIViewAnimationOptions.AllowAnimatedContent, null, CompletionHandler);
			}
		}

		// Apple allows NULL on 'completion' parameters used in animate* and transition* selectors

		void Animations ()
		{
		}

		[Test]
		public void Animate_Null_c1 ()
		{
			UIView.Animate (1.0, Animations, null);
		}

		[Test]
		public void Animate_Null_c2 ()
		{
			UIView.Animate (1.0, 2.0, UIViewAnimationOptions.Autoreverse, Animations, null);
		}

		[Test]
		public void AnimateNotify_Null_c1 ()
		{
			UIView.AnimateNotify (1.0, Animations, null);
		}

		[Test]
		public void AnimateNotify_Null_c2 ()
		{
			UIView.AnimateNotify (1.0, 2.0, UIViewAnimationOptions.Autoreverse, Animations, null);
		}

		[Test]
		public void Transition_Null_c1 ()
		{
			using (UIView v = new UIView ()) {
				UIView.Transition (v, 1.0, UIViewAnimationOptions.AllowAnimatedContent, Animations, null);
			}
		}

		[Test]
		public void Transition_Null_c2 ()
		{
			using (UIView v = new UIView ()) {
				UIView.Transition (v, v, 1.0, UIViewAnimationOptions.AllowAnimatedContent, null);
			}
		}

		[Test]
		public void TransitionNotify_Null_c1 ()
		{
			using (UIView v = new UIView ()) {
				UIView.TransitionNotify (v, 1.0, UIViewAnimationOptions.AllowAnimatedContent, Animations, null);
			}
		}

		[Test]
		public void TransitionNotify_Null_c2 ()
		{
			using (UIView v = new UIView ()) {
				UIView.TransitionNotify (v, v, 1.0, UIViewAnimationOptions.AllowAnimatedContent, null);
			}
		}

		[Test]
		public void BackgroundColorTest ()
		{
			using (var color = UIColor.FromRGB (5, 6, 7)) {
				using (var view = new ViewWithCustomBackgroundColor ()) {
					Messaging.void_objc_msgSend_IntPtr (view.Handle, new Selector ("setBackgroundColor:").Handle, color.Handle);
				}
			}
		}

		[Register ("ViewWithCustomBackgroundColor")]
		public class ViewWithCustomBackgroundColor : UIView {
			public override UIColor BackgroundColor {
				get { return base.BackgroundColor; }
				set { base.BackgroundColor = value; }
			}
		}

		[Test]
		public void TraitTest ()
		{
			using (var view = new UIView ()) {
				ClassicAssert.AreEqual (UIAccessibilityTrait.None, view.AccessibilityTraits, "a");
				view.AccessibilityTraits = UIAccessibilityTrait.None;
				ClassicAssert.AreEqual (UIAccessibilityTrait.None, view.AccessibilityTraits, "b");
				view.AccessibilityTraits = UIAccessibilityTrait.Adjustable;
				ClassicAssert.AreEqual (UIAccessibilityTrait.Adjustable, view.AccessibilityTraits, "c");
				view.AccessibilityTraits = UIAccessibilityTrait.Adjustable | UIAccessibilityTrait.Button;
				ClassicAssert.AreEqual (UIAccessibilityTrait.Adjustable | UIAccessibilityTrait.Button, view.AccessibilityTraits, "e");
			}
		}

		[Test]
		public void TraitMatch ()
		{
			ClassicAssert.AreEqual ((int) UIAccessibilityTrait.Adjustable, UIView.TraitAdjustable, "Adjustable");
			ClassicAssert.AreEqual ((int) UIAccessibilityTrait.AllowsDirectInteraction, UIView.TraitAllowsDirectInteraction, "AllowsDirectInteraction");
			ClassicAssert.AreEqual ((int) UIAccessibilityTrait.Button, UIView.TraitButton, "Button");
			ClassicAssert.AreEqual ((int) UIAccessibilityTrait.CausesPageTurn, UIView.TraitCausesPageTurn, "CausesPageTurn");
			ClassicAssert.AreEqual ((int) UIAccessibilityTrait.Image, UIView.TraitImage, "Image");
			ClassicAssert.AreEqual ((int) UIAccessibilityTrait.KeyboardKey, UIView.TraitKeyboardKey, "KeyboardKey");
			ClassicAssert.AreEqual ((int) UIAccessibilityTrait.Link, UIView.TraitLink, "Link");
			ClassicAssert.AreEqual ((int) UIAccessibilityTrait.None, UIView.TraitNone, "None");
			ClassicAssert.AreEqual ((int) UIAccessibilityTrait.NotEnabled, UIView.TraitNotEnabled, "NotEnabled");
			ClassicAssert.AreEqual ((int) UIAccessibilityTrait.PlaysSound, UIView.TraitPlaysSound, "PlaysSound");
			ClassicAssert.AreEqual ((int) UIAccessibilityTrait.SearchField, UIView.TraitSearchField, "SearchField");
			ClassicAssert.AreEqual ((int) UIAccessibilityTrait.Selected, UIView.TraitSelected, "Selected");
			ClassicAssert.AreEqual ((int) UIAccessibilityTrait.StartsMediaSession, UIView.TraitStartsMediaSession, "StartsMediaSession");
			ClassicAssert.AreEqual ((int) UIAccessibilityTrait.StaticText, UIView.TraitStaticText, "StaticText");
			ClassicAssert.AreEqual ((int) UIAccessibilityTrait.SummaryElement, UIView.TraitSummaryElement, "SummaryElement");
			ClassicAssert.AreEqual ((int) UIAccessibilityTrait.UpdatesFrequently, UIView.TraitUpdatesFrequently, "UpdatesFrequently");

			ClassicAssert.AreEqual ((int) UIAccessibilityTrait.Header, UIView.TraitHeader, "Header");
		}

		[Test]
		public void Subviews ()
		{
			using (var v = new UIView ()) {
				Assert.That (v.Subviews, Is.Not.Null);

				// even if null we want to ensure we can use UIView.GetEnumarator to iterate subviews
				int n = 0;
				foreach (var sv in v)
					ClassicAssert.NotNull (sv, n++.ToString ());
			}
		}

		[Test]
		public void TintColor ()
		{
			TestRuntime.AssertSystemVersion (ApplePlatform.iOS, 7, 0, throwIfOtherPlatform: false);

			using (var v = new UIView ()) {
				var tc = v.TintColor;
				ClassicAssert.NotNull (tc, "TintColor-1");
				v.TintColor = UIColor.Red;
				v.TintColor = null;
				// setting to null returns to default (i.e. not the last non-null value)
				ClassicAssert.NotNull (v.TintColor, "TintColor-2");
			}
		}

		[Test]
		public void Equality ()
		{
			using (var v1 = new UIView ())
			using (var v2 = new UIView ()) {
				// two basic/init'ed instances differ only by their handles
				Assert.That (v1.Handle, Is.Not.EqualTo (v2.Handle), "Handle");
				// and that's enough to make them totally different (natively in objc for both `hash` and `isEqual:`)
				Assert.That (v1.GetHashCode (), Is.Not.EqualTo (v2.GetHashCode ()), "GetHashCode");
				ClassicAssert.False (v1.Equals (v2.Handle), "Equals");
			}
		}
	}
}

#endif // !MONOMAC
