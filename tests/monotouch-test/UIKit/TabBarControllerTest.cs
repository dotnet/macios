//
// Unit tests for UITabBarController
//
// Authors:
//	Sebastien Pouliot <sebastien@xamarin.com>
//
// Copyright 2012 Xamarin Inc. All rights reserved.
//

#if !MONOMAC

using System.Drawing;
using System.Reflection;
using UIKit;

namespace MonoTouchFixtures.UIKit {
	[TestFixture]
	[Preserve (AllMembers = true)]
	public class TabBarControllerTest {

		void CheckDefault (UITabBarController c)
		{
#if !__TVOS__
			Assert.That (c.CustomizableViewControllers, Is.Null, "CustomizableViewControllers");
			Assert.That (c.MoreNavigationController, Is.Not.Null, "MoreNavigationController");
#endif
			Assert.That (c.SelectedIndex, Is.EqualTo (nint.MaxValue), "SelectedIndex");
			Assert.That (c.SelectedViewController, Is.Null, "SelectedViewController");
			Assert.That (c.ShouldSelectViewController, Is.Null, "ShouldSelectViewController");
			Assert.That (c.TabBar, Is.Not.Null, "TabBar");
			Assert.That (c.ViewControllers, Is.Null, "ViewControllers");
		}

		[Test]
		public void Ctor_Defaults ()
		{
			using (UITabBarController c = new UITabBarController ()) {
				CheckDefault (c);
			}
		}

#if false // Disabling for now due to Xcode 9 does not support nibs if deployment target == 6.0
		[Test]
		public void Ctor_Nib ()
		{
			using (UITabBarController c = new UITabBarController ("EmptyNib", null)) {
				// `initWithNibName:bundle:` is defined on a base class so it does not
				// affect the fields from UITabBarController
				CheckDefault (c);
			}
		}
#endif
	}
}

#endif // !MONOMAC
