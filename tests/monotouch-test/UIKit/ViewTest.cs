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
				Assert.That (result, Is.Null, "outside");
				result = v.HitTest (new CGPoint (50, 50), null);
				Assert.That (result.Handle, Is.EqualTo (v.Handle), "inside");
			}
		}

		[Test]
		public void PointInside_Null ()
		{
			var frame = new CGRect (10, 10, 100, 100);
			using (UIView v = new UIView (frame)) {
				Assert.That (v.PointInside (new CGPoint (-10, -10), null), Is.False, "outside");
				Assert.That (v.PointInside (new CGPoint (50, 50), null), Is.True, "inside");
			}
		}

		[Test]
		public void SizeThatFits ()
		{
			// same as LinkerTest in 'linksdk' project - but won't be linked here (for simulator)
			var empty = CGSize.Empty;
			using (UIView v = new UIView ()) {
				Assert.That (v.SizeThatFits (empty).IsEmpty, Is.True, "Empty");
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
				Assert.That (view.AccessibilityTraits, Is.EqualTo (UIAccessibilityTrait.None), "a");
				view.AccessibilityTraits = UIAccessibilityTrait.None;
				Assert.That (view.AccessibilityTraits, Is.EqualTo (UIAccessibilityTrait.None), "b");
				view.AccessibilityTraits = UIAccessibilityTrait.Adjustable;
				Assert.That (view.AccessibilityTraits, Is.EqualTo (UIAccessibilityTrait.Adjustable), "c");
				view.AccessibilityTraits = UIAccessibilityTrait.Adjustable | UIAccessibilityTrait.Button;
				Assert.That (view.AccessibilityTraits, Is.EqualTo (UIAccessibilityTrait.Adjustable | UIAccessibilityTrait.Button), "e");
			}
		}

		[Test]
		public void TraitMatch ()
		{
			Assert.That (UIView.TraitAdjustable, Is.EqualTo ((int) UIAccessibilityTrait.Adjustable), "Adjustable");
			Assert.That (UIView.TraitAllowsDirectInteraction, Is.EqualTo ((int) UIAccessibilityTrait.AllowsDirectInteraction), "AllowsDirectInteraction");
			Assert.That (UIView.TraitButton, Is.EqualTo ((int) UIAccessibilityTrait.Button), "Button");
			Assert.That (UIView.TraitCausesPageTurn, Is.EqualTo ((int) UIAccessibilityTrait.CausesPageTurn), "CausesPageTurn");
			Assert.That (UIView.TraitImage, Is.EqualTo ((int) UIAccessibilityTrait.Image), "Image");
			Assert.That (UIView.TraitKeyboardKey, Is.EqualTo ((int) UIAccessibilityTrait.KeyboardKey), "KeyboardKey");
			Assert.That (UIView.TraitLink, Is.EqualTo ((int) UIAccessibilityTrait.Link), "Link");
			Assert.That (UIView.TraitNone, Is.EqualTo ((int) UIAccessibilityTrait.None), "None");
			Assert.That (UIView.TraitNotEnabled, Is.EqualTo ((int) UIAccessibilityTrait.NotEnabled), "NotEnabled");
			Assert.That (UIView.TraitPlaysSound, Is.EqualTo ((int) UIAccessibilityTrait.PlaysSound), "PlaysSound");
			Assert.That (UIView.TraitSearchField, Is.EqualTo ((int) UIAccessibilityTrait.SearchField), "SearchField");
			Assert.That (UIView.TraitSelected, Is.EqualTo ((int) UIAccessibilityTrait.Selected), "Selected");
			Assert.That (UIView.TraitStartsMediaSession, Is.EqualTo ((int) UIAccessibilityTrait.StartsMediaSession), "StartsMediaSession");
			Assert.That (UIView.TraitStaticText, Is.EqualTo ((int) UIAccessibilityTrait.StaticText), "StaticText");
			Assert.That (UIView.TraitSummaryElement, Is.EqualTo ((int) UIAccessibilityTrait.SummaryElement), "SummaryElement");
			Assert.That (UIView.TraitUpdatesFrequently, Is.EqualTo ((int) UIAccessibilityTrait.UpdatesFrequently), "UpdatesFrequently");

			Assert.That (UIView.TraitHeader, Is.EqualTo ((int) UIAccessibilityTrait.Header), "Header");
		}

		[Test]
		public void Subviews ()
		{
			using (var v = new UIView ()) {
				Assert.That (v.Subviews, Is.Not.Null);

				// even if null we want to ensure we can use UIView.GetEnumarator to iterate subviews
				int n = 0;
				foreach (var sv in v)
					Assert.That (sv, Is.Not.Null, n++.ToString ());
			}
		}

		[Test]
		public void TintColor ()
		{
			TestRuntime.AssertSystemVersion (ApplePlatform.iOS, 7, 0, throwIfOtherPlatform: false);

			using (var v = new UIView ()) {
				var tc = v.TintColor;
				Assert.That (tc, Is.Not.Null, "TintColor-1");
				v.TintColor = UIColor.Red;
				v.TintColor = null;
				// setting to null returns to default (i.e. not the last non-null value)
				Assert.That (v.TintColor, Is.Not.Null, "TintColor-2");
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
				Assert.That (v1.Equals (v2.Handle), Is.False, "Equals");
			}
		}
	}
}

#endif // !MONOMAC
