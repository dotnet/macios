// Copyright 2011-2012 Xamarin Inc. All rights reserved

#if !MONOMAC

using System.Drawing;
using System.Reflection;
using CoreGraphics;
using UIKit;
using Xamarin.Utils;

namespace MonoTouchFixtures.UIKit {
	[TestFixture]
	[Preserve (AllMembers = true)]
	public class TabBarTest {

		[Test]
		public void InitWithFrame ()
		{
			var frame = new CGRect (10, 10, 100, 100);
			using (UITabBar tb = new UITabBar (frame)) {
				Assert.That (tb.Frame, Is.EqualTo (frame), "Frame");
			}
		}

		[Test]
		public void SelectedItem ()
		{
			using (UITabBarItem item = new UITabBarItem ())
			using (UITabBar tb = new UITabBar ()) {
				ClassicAssert.Null (tb.SelectedItem, "1a");

				tb.SelectedItem = item;
				// setter did not work because 'item' is not in Items
				ClassicAssert.Null (tb.SelectedItem, "2a");
				ClassicAssert.Null (tb.Items, "2b");

				tb.SelectedItem = null;
				ClassicAssert.Null (tb.SelectedItem, "3a");
			}
		}

		[Test]
		public void Items ()
		{
			using (UITabBarItem item = new UITabBarItem ())
			using (UITabBar tb = new UITabBar ()) {
				ClassicAssert.Null (tb.Items, "1a");
				ClassicAssert.Null (tb.SelectedItem, "1b");

				tb.Items = new UITabBarItem [] { item };
				ClassicAssert.NotNull (tb.Items, "2a");
				tb.SelectedItem = item;
				ClassicAssert.NotNull (tb.SelectedItem, "2b");

				tb.Items = null;
				ClassicAssert.Null (tb.Items, "3a");
				// Interaction between Items and SelectedItems -> backing fields!
				ClassicAssert.Null (tb.SelectedItem, "3b");
			}
		}

#if !__TVOS__
		[Test]
		public void Customizing ()
		{
			using (UITabBarItem item = new UITabBarItem ())
			using (UITabBar tb = new UITabBar ()) {
				ClassicAssert.False (tb.IsCustomizing, "IsCustomizing-1");

				tb.BeginCustomizingItems (new UITabBarItem [] { item });
				ClassicAssert.True (tb.IsCustomizing, "IsCustomizing-2");
				ClassicAssert.False (tb.EndCustomizing (false), "End-1");

				tb.BeginCustomizingItems (null);
				ClassicAssert.False (tb.EndCustomizing (false), "End-2");

				ClassicAssert.False (tb.IsCustomizing, "IsCustomizing-3");
			}
		}
#endif

		[Test]
		public void BackgroundImage ()
		{
			using (UIImage i = new UIImage ())
			using (UITabBar tb = new UITabBar ()) {
				ClassicAssert.Null (tb.BackgroundImage, "1");

				tb.BackgroundImage = i;
				ClassicAssert.NotNull (tb.BackgroundImage, "2");

				tb.BackgroundImage = null;
				ClassicAssert.Null (tb.BackgroundImage, "3");
			}
		}

		[Test]
		public void SelectionIndicatorImage ()
		{
			using (UIImage i = new UIImage ())
			using (UITabBar tb = new UITabBar ()) {
				ClassicAssert.Null (tb.SelectionIndicatorImage, "1");

				tb.SelectionIndicatorImage = i;
				ClassicAssert.NotNull (tb.SelectionIndicatorImage, "2");

				tb.SelectionIndicatorImage = null;
				ClassicAssert.Null (tb.SelectionIndicatorImage, "3");
			}
		}

		[Test]
		public void TintColor ()
		{
			using (UITabBar tb = new UITabBar ()) {
				// TintColor is inherited in iOS7 so it won't be null by default
				if (TestRuntime.CheckSystemVersion (ApplePlatform.iOS, 7, 0, throwIfOtherPlatform: false))
					ClassicAssert.NotNull (tb.TintColor, "1");
				else
					ClassicAssert.Null (tb.TintColor, "1");

				tb.TintColor = UIColor.White;
				Assert.That (tb.TintColor, Is.EqualTo (UIColor.White), "2");

				tb.TintColor = null;
				if (TestRuntime.IsTVOS) {
					// we only care that setting `null` gives us back some default OS value
					ClassicAssert.NotNull (tb.TintColor, "3");
				} else if (TestRuntime.CheckSystemVersion (ApplePlatform.iOS, 7, 0, throwIfOtherPlatform: false)) {
					Assert.That (tb.TintColor, Is.Not.EqualTo (UIColor.White), "3");
				} else
					ClassicAssert.Null (tb.TintColor, "3");
			}
		}

#if !__TVOS__
		[Test]
		public void SelectedImageTintColor ()
		{
			using (UITabBar tb = new UITabBar ()) {
				ClassicAssert.Null (tb.SelectedImageTintColor, "1");

				tb.SelectedImageTintColor = UIColor.Black;
				if (!TestRuntime.CheckSystemVersion (ApplePlatform.iOS, 7, 1)) {
					// before 7.1 the tintColor would have been accepted
					ClassicAssert.NotNull (tb.SelectedImageTintColor, "2");

					tb.SelectedImageTintColor = null;
				}
				ClassicAssert.Null (tb.SelectedImageTintColor, "3");
			}
		}
#endif
	}
}

#endif // !MONOMAC
