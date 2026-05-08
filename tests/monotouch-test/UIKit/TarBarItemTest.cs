//
// Unit tests for UITabBarItem
//
// Authors:
//	Sebastien Pouliot <sebastien@xamarin.com>
//
// Copyright 2012-2013 Xamarin Inc. All rights reserved.
//

#if !MONOMAC

using System.Drawing;
using System.Reflection;
using UIKit;
using Xamarin.Utils;

namespace MonoTouchFixtures.UIKit {
	[TestFixture]
	[Preserve (AllMembers = true)]
	public class TabBarItemTest {
		[Test]
		public void Ctor_Defaults ()
		{
			using (UITabBarItem tbi = new UITabBarItem ()) {
				ClassicAssert.Null (tbi.BadgeValue, "BadgeValue");
				ClassicAssert.True (tbi.Enabled, "Enabled");
#if !__TVOS__
				ClassicAssert.Null (tbi.FinishedSelectedImage, "FinishedSelectedImage");
				ClassicAssert.Null (tbi.FinishedUnselectedImage, "FinishedUnselectedImage");
#endif
				ClassicAssert.Null (tbi.Image, "Image");
				Assert.That (tbi.ImageInsets, Is.EqualTo (UIEdgeInsets.Zero), "ImageInsets");
				Assert.That (tbi.Tag, Is.EqualTo ((nint) 0), "Tag");
				ClassicAssert.Null (tbi.Title, "Title");
				Assert.That (tbi.TitlePositionAdjustment.Horizontal, Is.EqualTo ((nfloat) 0f), "TitlePositionAdjustment.Horizontal");
				Assert.That (tbi.TitlePositionAdjustment.Vertical, Is.EqualTo ((nfloat) 0f), "TitlePositionAdjustment.Vertical");
			}
		}

		[Test]
		public void Ctor_2 ()
		{
			Assert.Multiple (() => {
				using (UITabBarItem tbi = new UITabBarItem (UITabBarSystemItem.Bookmarks, nint.MaxValue)) {
					ClassicAssert.Null (tbi.BadgeValue, "BadgeValue");
					ClassicAssert.True (tbi.Enabled, "Enabled");
#if !__TVOS__
					ClassicAssert.Null (tbi.FinishedSelectedImage, "FinishedSelectedImage");
					ClassicAssert.Null (tbi.FinishedUnselectedImage, "FinishedUnselectedImage");
#endif
					if (TestRuntime.CheckXcodeVersion (16, 0)) {
						ClassicAssert.NotNull (tbi.Image, "Image");
					} else {
						ClassicAssert.Null (tbi.Image, "Image");
					}
					Assert.That (tbi.ImageInsets, Is.EqualTo (UIEdgeInsets.Zero), "ImageInsets");
					Assert.That (tbi.Tag, Is.EqualTo (nint.MaxValue), "Tag");
					Assert.That (tbi.Title, Is.Null.Or.EqualTo ("Bookmarks"), "Title");
					Assert.That (tbi.TitlePositionAdjustment.Horizontal, Is.EqualTo ((nfloat) 0f), "TitlePositionAdjustment.Horizontal");
					Assert.That (tbi.TitlePositionAdjustment.Vertical, Is.EqualTo ((nfloat) 0f), "TitlePositionAdjustment.Vertical");
				}
			});
		}

		[Test]
		public void Ctor_3 ()
		{
			using (UIImage img = new UIImage ())
			using (UITabBarItem tbi = new UITabBarItem ("title", img, nint.MinValue)) {
				ClassicAssert.Null (tbi.BadgeValue, "BadgeValue");
				ClassicAssert.True (tbi.Enabled, "Enabled");
#if !__TVOS__
				ClassicAssert.Null (tbi.FinishedSelectedImage, "FinishedSelectedImage");
				ClassicAssert.Null (tbi.FinishedUnselectedImage, "FinishedUnselectedImage");
#endif
				ClassicAssert.AreSame (tbi.Image, img, "Image");
				Assert.That (tbi.ImageInsets, Is.EqualTo (UIEdgeInsets.Zero), "ImageInsets");
				Assert.That (tbi.Tag, Is.EqualTo (nint.MinValue), "Tag");
				Assert.That (tbi.Title, Is.EqualTo ("title"), "Title");
				Assert.That (tbi.TitlePositionAdjustment.Horizontal, Is.EqualTo ((nfloat) 0f), "TitlePositionAdjustment.Horizontal");
				Assert.That (tbi.TitlePositionAdjustment.Vertical, Is.EqualTo ((nfloat) 0f), "TitlePositionAdjustment.Vertical");
			}
		}

		[Test]
		public void Ctor_3a_Null ()
		{
			using (UIImage img = new UIImage ()) {
				using (UITabBarItem tbi1 = new UITabBarItem (null, img, nint.MinValue)) {
					ClassicAssert.Null (tbi1.Title, "Title-1a");
					ClassicAssert.AreSame (tbi1.Image, img, "Image-1a");
					tbi1.Title = "title";
					tbi1.Image = null;
					Assert.That (tbi1.Title, Is.EqualTo ("title"), "Title-1b");
					ClassicAssert.IsNull (tbi1.Image, "Image-1b");
				}
				using (UITabBarItem tbi2 = new UITabBarItem ("title", null, nint.MaxValue)) {
					Assert.That (tbi2.Title, Is.EqualTo ("title"), "Title-2a");
					ClassicAssert.Null (tbi2.Image, "Image-2a");
					tbi2.Title = null;
					tbi2.Image = img;
					ClassicAssert.Null (tbi2.Title, "Title-2b");
					ClassicAssert.AreSame (tbi2.Image, img, "Image-2b");
				}
				using (UITabBarItem tbi3 = new UITabBarItem (null, null, 0)) {
					ClassicAssert.Null (tbi3.Title, "Title-3a");
					ClassicAssert.Null (tbi3.Image, "Image-3a");
					tbi3.Title = "title";
					tbi3.Image = img;
					Assert.That (tbi3.Title, Is.EqualTo ("title"), "Title-3b");
					ClassicAssert.AreSame (tbi3.Image, img, "Image-3b");
				}
			}
		}

		[Test]
		public void Ctor_3b_Null ()
		{
			TestRuntime.AssertSystemVersion (ApplePlatform.iOS, 7, 0, throwIfOtherPlatform: false);

			using (UIImage img = new UIImage ()) {
				using (UITabBarItem tbi1 = new UITabBarItem (null, null, null)) {
					ClassicAssert.Null (tbi1.Title, "Title-1a");
					ClassicAssert.Null (tbi1.Image, "Image-1a");
					ClassicAssert.Null (tbi1.SelectedImage, "SelectedImage-1a");
				}
				using (UITabBarItem tbi2 = new UITabBarItem ("title", img, null)) {
					Assert.That (tbi2.Title, Is.EqualTo ("title"), "Title-2a");
					ClassicAssert.AreSame (tbi2.Image, img, "Image-2a");
					// if not supplied Image is reused
					ClassicAssert.AreSame (tbi2.SelectedImage, img, "SelectedImage-2a");
				}
				using (UITabBarItem tbi3 = new UITabBarItem (null, null, img)) {
					ClassicAssert.Null (tbi3.Title, "Title-3a");
					ClassicAssert.Null (tbi3.Image, "Image-3a");
					// looks like a select-only image is not something allowed on 7.1

					var hasSelectedImage = true;
#if __IOS__
					if (!TestRuntime.CheckSystemVersion (ApplePlatform.iOS, 7, 1))
						hasSelectedImage = false;
#endif
					if (hasSelectedImage)
						ClassicAssert.Null (tbi3.SelectedImage, "SelectedImage-3a");
					else
						ClassicAssert.AreSame (tbi3.SelectedImage, img, "SelectedImage-3a");
				}
			}
		}

		[Test]
		public void SelectedImage_7a ()
		{
			TestRuntime.AssertSystemVersion (ApplePlatform.iOS, 7, 0, throwIfOtherPlatform: false);

			using (UIImage i1 = new UIImage ())
			using (UITabBarItem tbi = new UITabBarItem ("title", i1, null)) {
				ClassicAssert.AreSame (i1, tbi.Image, "Image");
				ClassicAssert.AreSame (i1, tbi.SelectedImage, "SelectedImage");
#if !__TVOS__
				ClassicAssert.Null (tbi.FinishedSelectedImage, "FinishedSelectedImage");
				ClassicAssert.Null (tbi.FinishedUnselectedImage, "FinishedSelectedImage");
#endif
				// null does a reset, in this case i1 can be reused
				tbi.SelectedImage = null;
				ClassicAssert.AreSame (i1, tbi.SelectedImage, "SelectedImage2");
			}
		}

		[Test]
		public void SelectedImage_7b ()
		{
			TestRuntime.AssertSystemVersion (ApplePlatform.iOS, 7, 0, throwIfOtherPlatform: false);

			using (UIImage i1 = new UIImage ())
			using (UIImage i2 = new UIImage ())
			using (UITabBarItem tbi = new UITabBarItem ("title", i1, i2)) {
				ClassicAssert.AreSame (i1, tbi.Image, "Image");
				ClassicAssert.AreSame (i2, tbi.SelectedImage, "SelectedImage");
#if !__TVOS__
				ClassicAssert.Null (tbi.FinishedSelectedImage, "FinishedSelectedImage");
				ClassicAssert.Null (tbi.FinishedUnselectedImage, "FinishedSelectedImage");
#endif
				tbi.SelectedImage = null;
				// null does a reset, in this case i2 is removed and i1 gets used
				ClassicAssert.AreSame (i1, tbi.SelectedImage, "SelectedImage2");
			}
		}
	}
}

#endif // !MONOMAC
