// Copyright 2011 Xamarin Inc. All rights reserved

#if !__TVOS__ && !MONOMAC

using System.Drawing;
using CoreGraphics;
using UIKit;

namespace MonoTouchFixtures.UIKit {

	[TestFixture]
	[Preserve (AllMembers = true)]
	public class ToolbarTest {

		[Test]
		public void InitWithFrame ()
		{
			var frame = new CGRect (10, 10, 100, 100);
			using (UIToolbar tb = new UIToolbar (frame)) {
				Assert.That (tb.Frame, Is.EqualTo (frame), "Frame");
			}
		}

		[Test]
		public void BackgroundImage ()
		{
			using (UIToolbar tb = new UIToolbar ()) {
				Assert.Null (tb.GetBackgroundImage (UIToolbarPosition.Any, UIBarMetrics.Default), "Get");
				tb.SetBackgroundImage (null, UIToolbarPosition.Any, UIBarMetrics.Default);
			}
		}
	}
}

#endif // !__TVOS__ && !MONOMAC
