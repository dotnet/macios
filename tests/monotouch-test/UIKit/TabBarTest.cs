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
				Assert.That (tb.SelectedItem, Is.Null, "1a");

				tb.SelectedItem = item;
				// setter did not work because 'item' is not in Items
				Assert.That (tb.SelectedItem, Is.Null, "2a");
				Assert.That (tb.Items, Is.Null, "2b");

				tb.SelectedItem = null;
				Assert.That (tb.SelectedItem, Is.Null, "3a");
			}
		}

		[Test]
		public void Items ()
		{
			using (UITabBarItem item = new UITabBarItem ())
			using (UITabBar tb = new UITabBar ()) {
				Assert.That (tb.Items, Is.Null, "1a");
				Assert.That (tb.SelectedItem, Is.Null, "1b");

				tb.Items = new UITabBarItem [] { item };
				Assert.That (tb.Items, Is.Not.Null, "2a");
				tb.SelectedItem = item;
				Assert.That (tb.SelectedItem, Is.Not.Null, "2b");

				tb.Items = null;
				Assert.That (tb.Items, Is.Null, "3a");
				// Interaction between Items and SelectedItems -> backing fields!
				Assert.That (tb.SelectedItem, Is.Null, "3b");
			}
		}

#if !__TVOS__
		[Test]
		public void Customizing ()
		{
			using (UITabBarItem item = new UITabBarItem ())
			using (UITabBar tb = new UITabBar ()) {
				Assert.That (tb.IsCustomizing, Is.False, "IsCustomizing-1");

				tb.BeginCustomizingItems (new UITabBarItem [] { item });
				Assert.That (tb.IsCustomizing, Is.True, "IsCustomizing-2");
				Assert.That (tb.EndCustomizing (false), Is.False, "End-1");

				tb.BeginCustomizingItems (null);
				Assert.That (tb.EndCustomizing (false), Is.False, "End-2");

				Assert.That (tb.IsCustomizing, Is.False, "IsCustomizing-3");
			}
		}
#endif

		[Test]
		public void BackgroundImage ()
		{
			using (UIImage i = new UIImage ())
			using (UITabBar tb = new UITabBar ()) {
				Assert.That (tb.BackgroundImage, Is.Null, "1");

				tb.BackgroundImage = i;
				Assert.That (tb.BackgroundImage, Is.Not.Null, "2");

				tb.BackgroundImage = null;
				Assert.That (tb.BackgroundImage, Is.Null, "3");
			}
		}

		[Test]
		public void SelectionIndicatorImage ()
		{
			using (UIImage i = new UIImage ())
			using (UITabBar tb = new UITabBar ()) {
				Assert.That (tb.SelectionIndicatorImage, Is.Null, "1");

				tb.SelectionIndicatorImage = i;
				Assert.That (tb.SelectionIndicatorImage, Is.Not.Null, "2");

				tb.SelectionIndicatorImage = null;
				Assert.That (tb.SelectionIndicatorImage, Is.Null, "3");
			}
		}

		[Test]
		public void TintColor ()
		{
			using (UITabBar tb = new UITabBar ()) {
				// TintColor is inherited in iOS7 so it won't be null by default
				if (TestRuntime.CheckSystemVersion (ApplePlatform.iOS, 7, 0, throwIfOtherPlatform: false))
					Assert.That (tb.TintColor, Is.Not.Null, "1");
				else
					Assert.That (tb.TintColor, Is.Null, "1");

				tb.TintColor = UIColor.White;
				Assert.That (tb.TintColor, Is.EqualTo (UIColor.White), "2");

				tb.TintColor = null;
				if (TestRuntime.IsTVOS) {
					// we only care that setting `null` gives us back some default OS value
					Assert.That (tb.TintColor, Is.Not.Null, "3");
				} else if (TestRuntime.CheckSystemVersion (ApplePlatform.iOS, 7, 0, throwIfOtherPlatform: false)) {
					Assert.That (tb.TintColor, Is.Not.EqualTo (UIColor.White), "3");
				} else
					Assert.That (tb.TintColor, Is.Null, "3");
			}
		}

#if !__TVOS__
		[Test]
		public void SelectedImageTintColor ()
		{
			using (UITabBar tb = new UITabBar ()) {
				Assert.That (tb.SelectedImageTintColor, Is.Null, "1");

				tb.SelectedImageTintColor = UIColor.Black;
				if (!TestRuntime.CheckSystemVersion (ApplePlatform.iOS, 7, 1)) {
					// before 7.1 the tintColor would have been accepted
					Assert.That (tb.SelectedImageTintColor, Is.Not.Null, "2");

					tb.SelectedImageTintColor = null;
				}
				Assert.That (tb.SelectedImageTintColor, Is.Null, "3");
			}
		}
#endif
	}
}

#endif // !MONOMAC
