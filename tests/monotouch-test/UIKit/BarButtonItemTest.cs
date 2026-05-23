// Copyright 2011-2013 Xamarin Inc. All rights reserved

#if !MONOMAC

using System.Drawing;
using System.Reflection;
using UIKit;

namespace MonoTouchFixtures.UIKit {
	class MyView : UIView {

		public MyView (string note)
		{
			Annotation = note;
		}

		public string Annotation { get; private set; }
	}

	[TestFixture]
	[Preserve (AllMembers = true)]
	public class ButtonBarItemTest {

		[Test]
		public void InitWithImage ()
		{
			using (var img = new UIImage ())
			using (var btn = new UIBarButtonItem (img, UIBarButtonItemStyle.Bordered, null, null)) {
				Assert.That (btn.Target, Is.Null, "Target");
				Assert.That (btn.Image, Is.SameAs (img), "Image");
				btn.Image = null; // nullable
			}

			using (var btn = new UIBarButtonItem ((UIImage) null, UIBarButtonItemStyle.Bordered, null, null)) {
				Assert.That (btn.Image, Is.Null, "Image-null");
			}
		}

		[Test]
		public void InitWithImage2 ()
		{
			using (var img = new UIImage ())
			using (var btn = new UIBarButtonItem (img, img, UIBarButtonItemStyle.Bordered, null, null)) {
				Assert.That (btn.Target, Is.Null, "Target");
				Assert.That (btn.Image, Is.SameAs (img), "Image");
				btn.Image = null; // nullable
			}

			using (var btn = new UIBarButtonItem (null, null, UIBarButtonItemStyle.Bordered, null, null)) {
				Assert.That (btn.Image, Is.Null, "Image-null");
			}
		}

		[Test]
		public void InitWithText ()
		{
			using (var btn = new UIBarButtonItem ("title", UIBarButtonItemStyle.Bordered, null, null)) {
				Assert.That (btn.Target, Is.Null, "Target");
				Assert.That (btn.Title, Is.EqualTo ("title"), "Title");
				btn.Title = null; // nullable
			}

			using (var btn = new UIBarButtonItem ((string) null, UIBarButtonItemStyle.Bordered, null, null)) {
				Assert.That (btn.Title, Is.Null, "Title-null");
			}
		}

		[Test]
		public void CustomView_Null ()
		{
			using (var btn = new UIBarButtonItem ("title", UIBarButtonItemStyle.Bordered, null, null)) {
				Assert.That (btn.CustomView, Is.Null, "default");
				btn.CustomView = null; // nullable
			}
		}

		[Test]
		public void TintColor_Null ()
		{
			using (var btn = new UIBarButtonItem ("title", UIBarButtonItemStyle.Bordered, null, null)) {
				Assert.That (btn.TintColor, Is.Null, "default");
				btn.TintColor = UIColor.Blue;
				Assert.That (btn.TintColor == UIColor.Blue, "blue");
				btn.TintColor = null;
				Assert.That (btn.TintColor, Is.Null, "null");
			}
		}

		[Test]
		public void Action_Set ()
		{
			using (UIBarButtonItem btn = new UIBarButtonItem ()) {
				btn.Action = null;
				// that caller the getter, which is not a valid selector if null and was throwing, ref: #10876
				btn.Action = btn.Action;
			}
		}

		[Test]
		public void BackgroundImage ()
		{
			using (UIBarButtonItem btn = new UIBarButtonItem ()) {
				Assert.That (btn.GetBackgroundImage (UIControlState.Highlighted, UIBarMetrics.Default), Is.Null, "Get");
				btn.SetBackgroundImage (null, UIControlState.Highlighted, UIBarMetrics.Default);

				Assert.That (btn.GetBackgroundImage (UIControlState.Highlighted, UIBarButtonItemStyle.Plain, UIBarMetrics.Default), Is.Null, "Get2");
				btn.SetBackgroundImage (null, UIControlState.Highlighted, UIBarButtonItemStyle.Plain, UIBarMetrics.Default);
			}
		}

		[Test]
		public void BackButtonBackgroundImage ()
		{
			using (UIBarButtonItem btn = new UIBarButtonItem ()) {
#if !__TVOS__
				Assert.That (btn.GetBackButtonBackgroundImage (UIControlState.Highlighted, UIBarMetrics.Default), Is.Null, "Get");
				btn.SetBackButtonBackgroundImage (null, UIControlState.Highlighted, UIBarMetrics.Default);
#endif
			}
		}

#if !__TVOS__
		[Test]
		public void SetTitleTextAttributes_Null ()
		{
			using (MyView v = new MyView ("note"))
			using (var b = new UIBarButtonItem (v)) {
				b.SetTitleTextAttributes ((UIStringAttributes) null, UIControlState.Disabled);
			}
		}
#endif
	}
}

#endif // !MONOMAC
