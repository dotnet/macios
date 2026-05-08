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
			ClassicAssert.Null (c.CustomizableViewControllers, "CustomizableViewControllers");
			ClassicAssert.NotNull (c.MoreNavigationController, "MoreNavigationController");
#endif
			Assert.That (c.SelectedIndex, Is.EqualTo (nint.MaxValue), "SelectedIndex");
			ClassicAssert.Null (c.SelectedViewController, "SelectedViewController");
			ClassicAssert.Null (c.ShouldSelectViewController, "ShouldSelectViewController");
			ClassicAssert.NotNull (c.TabBar, "TabBar");
			ClassicAssert.Null (c.ViewControllers, "ViewControllers");
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
