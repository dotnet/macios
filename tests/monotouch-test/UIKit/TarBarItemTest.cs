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
				Assert.That (tbi.BadgeValue, Is.Null, "BadgeValue");
				Assert.That (tbi.Enabled, Is.True, "Enabled");
#if !__TVOS__
				Assert.That (tbi.FinishedSelectedImage, Is.Null, "FinishedSelectedImage");
				Assert.That (tbi.FinishedUnselectedImage, Is.Null, "FinishedUnselectedImage");
#endif
				Assert.That (tbi.Image, Is.Null, "Image");
				Assert.That (tbi.ImageInsets, Is.EqualTo (UIEdgeInsets.Zero), "ImageInsets");
				Assert.That (tbi.Tag, Is.EqualTo ((nint) 0), "Tag");
				Assert.That (tbi.Title, Is.Null, "Title");
				Assert.That (tbi.TitlePositionAdjustment.Horizontal, Is.EqualTo ((nfloat) 0f), "TitlePositionAdjustment.Horizontal");
				Assert.That (tbi.TitlePositionAdjustment.Vertical, Is.EqualTo ((nfloat) 0f), "TitlePositionAdjustment.Vertical");
			}
		}

		[Test]
		public void Ctor_2 ()
		{
			Assert.Multiple (() => {
				using (UITabBarItem tbi = new UITabBarItem (UITabBarSystemItem.Bookmarks, nint.MaxValue)) {
					Assert.That (tbi.BadgeValue, Is.Null, "BadgeValue");
					Assert.That (tbi.Enabled, Is.True, "Enabled");
#if !__TVOS__
					Assert.That (tbi.FinishedSelectedImage, Is.Null, "FinishedSelectedImage");
					Assert.That (tbi.FinishedUnselectedImage, Is.Null, "FinishedUnselectedImage");
#endif
					if (TestRuntime.CheckXcodeVersion (16, 0)) {
						Assert.That (tbi.Image, Is.Not.Null, "Image");
					} else {
						Assert.That (tbi.Image, Is.Null, "Image");
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
				Assert.That (tbi.BadgeValue, Is.Null, "BadgeValue");
				Assert.That (tbi.Enabled, Is.True, "Enabled");
#if !__TVOS__
				Assert.That (tbi.FinishedSelectedImage, Is.Null, "FinishedSelectedImage");
				Assert.That (tbi.FinishedUnselectedImage, Is.Null, "FinishedUnselectedImage");
#endif
				Assert.That (img, Is.SameAs (tbi.Image), "Image");
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
					Assert.That (tbi1.Title, Is.Null, "Title-1a");
					Assert.That (img, Is.SameAs (tbi1.Image), "Image-1a");
					tbi1.Title = "title";
					tbi1.Image = null;
					Assert.That (tbi1.Title, Is.EqualTo ("title"), "Title-1b");
					Assert.That (tbi1.Image, Is.Null, "Image-1b");
				}
				using (UITabBarItem tbi2 = new UITabBarItem ("title", null, nint.MaxValue)) {
					Assert.That (tbi2.Title, Is.EqualTo ("title"), "Title-2a");
					Assert.That (tbi2.Image, Is.Null, "Image-2a");
					tbi2.Title = null;
					tbi2.Image = img;
					Assert.That (tbi2.Title, Is.Null, "Title-2b");
					Assert.That (img, Is.SameAs (tbi2.Image), "Image-2b");
				}
				using (UITabBarItem tbi3 = new UITabBarItem (null, null, 0)) {
					Assert.That (tbi3.Title, Is.Null, "Title-3a");
					Assert.That (tbi3.Image, Is.Null, "Image-3a");
					tbi3.Title = "title";
					tbi3.Image = img;
					Assert.That (tbi3.Title, Is.EqualTo ("title"), "Title-3b");
					Assert.That (img, Is.SameAs (tbi3.Image), "Image-3b");
				}
			}
		}

		[Test]
		public void Ctor_3b_Null ()
		{
			TestRuntime.AssertSystemVersion (ApplePlatform.iOS, 7, 0, throwIfOtherPlatform: false);

			using (UIImage img = new UIImage ()) {
				using (UITabBarItem tbi1 = new UITabBarItem (null, null, null)) {
					Assert.That (tbi1.Title, Is.Null, "Title-1a");
					Assert.That (tbi1.Image, Is.Null, "Image-1a");
					Assert.That (tbi1.SelectedImage, Is.Null, "SelectedImage-1a");
				}
				using (UITabBarItem tbi2 = new UITabBarItem ("title", img, null)) {
					Assert.That (tbi2.Title, Is.EqualTo ("title"), "Title-2a");
					Assert.That (img, Is.SameAs (tbi2.Image), "Image-2a");
					// if not supplied Image is reused
					Assert.That (img, Is.SameAs (tbi2.SelectedImage), "SelectedImage-2a");
				}
				using (UITabBarItem tbi3 = new UITabBarItem (null, null, img)) {
					Assert.That (tbi3.Title, Is.Null, "Title-3a");
					Assert.That (tbi3.Image, Is.Null, "Image-3a");
					// looks like a select-only image is not something allowed on 7.1

					var hasSelectedImage = true;
#if __IOS__
					if (!TestRuntime.CheckSystemVersion (ApplePlatform.iOS, 7, 1))
						hasSelectedImage = false;
#endif
					if (hasSelectedImage)
						Assert.That (tbi3.SelectedImage, Is.Null, "SelectedImage-3a");
					else
						Assert.That (img, Is.SameAs (tbi3.SelectedImage), "SelectedImage-3a");
				}
			}
		}

		[Test]
		public void SelectedImage_7a ()
		{
			TestRuntime.AssertSystemVersion (ApplePlatform.iOS, 7, 0, throwIfOtherPlatform: false);

			using (UIImage i1 = new UIImage ())
			using (UITabBarItem tbi = new UITabBarItem ("title", i1, null)) {
				Assert.That (tbi.Image, Is.SameAs (i1), "Image");
				Assert.That (tbi.SelectedImage, Is.SameAs (i1), "SelectedImage");
#if !__TVOS__
				Assert.That (tbi.FinishedSelectedImage, Is.Null, "FinishedSelectedImage");
				Assert.That (tbi.FinishedUnselectedImage, Is.Null, "FinishedSelectedImage");
#endif
				// null does a reset, in this case i1 can be reused
				tbi.SelectedImage = null;
				Assert.That (tbi.SelectedImage, Is.SameAs (i1), "SelectedImage2");
			}
		}

		[Test]
		public void SelectedImage_7b ()
		{
			TestRuntime.AssertSystemVersion (ApplePlatform.iOS, 7, 0, throwIfOtherPlatform: false);

			using (UIImage i1 = new UIImage ())
			using (UIImage i2 = new UIImage ())
			using (UITabBarItem tbi = new UITabBarItem ("title", i1, i2)) {
				Assert.That (tbi.Image, Is.SameAs (i1), "Image");
				Assert.That (tbi.SelectedImage, Is.SameAs (i2), "SelectedImage");
#if !__TVOS__
				Assert.That (tbi.FinishedSelectedImage, Is.Null, "FinishedSelectedImage");
				Assert.That (tbi.FinishedUnselectedImage, Is.Null, "FinishedSelectedImage");
#endif
				tbi.SelectedImage = null;
				// null does a reset, in this case i2 is removed and i1 gets used
				Assert.That (tbi.SelectedImage, Is.SameAs (i1), "SelectedImage2");
			}
		}
	}
}

#endif // !MONOMAC
