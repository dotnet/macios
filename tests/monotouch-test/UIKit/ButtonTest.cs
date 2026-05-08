// Copyright 2011, 2013 Xamarin Inc. All rights reserved

#if !MONOMAC

using System.Drawing;
using CoreGraphics;
using UIKit;
using Xamarin.Utils;

namespace MonoTouchFixtures.UIKit {

	[TestFixture]
	[Preserve (AllMembers = true)]
	public class ButtonTest {

		[Test]
		public void InitWithFrame ()
		{
			var frame = new CGRect (10, 10, 100, 100);
			using (UIButton b = new UIButton (frame)) {
				Assert.That (b.Frame, Is.EqualTo (frame), "Frame");
			}
		}

		[Test]
		public void NullAllowed ()
		{
			using (var b = new UIButton ()) {
				b.SetTitle (null, UIControlState.Normal);
				Assert.That (b.Title (UIControlState.Normal), Is.Null, "title");

				b.SetTitleColor (null, UIControlState.Normal);
				var hasTitleColor = true;
#if __IOS__
				if (!TestRuntime.CheckSystemVersion (ApplePlatform.iOS, 7, 0))
					hasTitleColor = false;
#endif
				if (hasTitleColor)
					Assert.That (b.TitleColor (UIControlState.Normal), Is.EqualTo (UIColor.White), "titlecolor");
				else
					Assert.That (b.TitleColor (UIControlState.Normal), Is.Null, "titlecolor");

				b.SetTitleShadowColor (null, UIControlState.Normal);
				Assert.That (b.TitleShadowColor (UIControlState.Normal), Is.Null, "titleshadowcolor");
			}
		}

		[Test]
		public void Tag_12557 ()
		{
			using (UIButton b = new UIButton ()) {
				Assert.That (b.Tag, Is.EqualTo ((nint) 0), "default");
				b.Tag = nint.MaxValue;
				Assert.That (b.Tag, Is.EqualTo (nint.MaxValue), "nint.MaxValue");
				b.Tag = nint.MinValue;
				Assert.That (b.Tag, Is.EqualTo (nint.MinValue), "nint.MinValue");
				b.Tag = 0;
				Assert.That (b.Tag, Is.EqualTo ((nint) 0), "0");
			}
		}
	}
}

#endif // !MONOMAC
