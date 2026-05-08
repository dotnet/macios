//
// Unit tests for UIViewController
//
// Authors:
//	Sebastien Pouliot  <sebastien@xamarin.com>
//
// Copyright 2012-2013 Xamarin Inc. All rights reserved.
//

#if !MONOMAC

using System.Reflection;
using UIKit;
using Xamarin.Utils;

namespace MonoTouchFixtures.UIKit {
	[TestFixture]
	[Preserve (AllMembers = true)]
	public class ViewControllerTest {

#if !__TVOS__
		[Test]
		public void Bug3489 ()
		{
			using (UIViewController a = new UIViewController ())
			using (UIViewController b = new UIViewController ())
			using (UIViewController c = new UIViewController ()) {
				a.PresentModalViewController (b, true);
				b.PresentModalViewController (c, true);

				b.DismissModalViewController (true);
				a.DismissModalViewController (true); //error
			}
		}
#endif

#if !__TVOS__
		[Test]
		public void Bug3189 ()
		{
			using (UIViewController a = new UIViewController ())
			using (UIViewController b = new UIViewController ())
			using (UIViewController c = new UIViewController ())
			using (UIViewController wb = new UINavigationController (b))
			using (UIViewController wc = new UINavigationController (c)) {
				a.PresentModalViewController (wb, true);
				b.PresentModalViewController (wc, true);

				c.DismissModalViewController (true); //error
			}
		}
#endif

		[Test]
		public void NonModal ()
		{
			using (UIViewController a = new UIViewController ())
			using (UIViewController b = new UIViewController ())
			using (UIViewController c = new UIViewController ())
			using (UIViewController wb = new UINavigationController (b))
			using (UIViewController wc = new UINavigationController (c)) {
				// interesting [PreSnippet] for the linker (wrt backing field elimitation)
				a.PresentViewController (wb, true, null);
				b.PresentViewController (wc, true, null);

				// interesting [PostSnippet] for the linker (wrt backing field elimitation)
				c.DismissViewController (true, null);
			}
		}

		[Test]
		public void NSAction_Null ()
		{
			using (var vc = new UIViewController ())
			using (var child = new UIViewController ()) {
				vc.PresentViewController (child, false, null);
				child.DismissViewController (false, null);
			}
		}

		[Test]
		public void Defaults ()
		{
			using (var vc = new UIViewController ()) {
				Assert.Multiple (() => {
					Assert.That (vc.ChildViewControllers, Is.Empty, "ChildViewControllers");
					ClassicAssert.False (vc.DefinesPresentationContext, "DefinesPresentationContext");
					Assert.That (vc.DisablesAutomaticKeyboardDismissal, Is.EqualTo (true).Or.EqualTo (false), "DisablesAutomaticKeyboardDismissal");
					ClassicAssert.False (vc.Editing, "Editing");
					ClassicAssert.False (vc.IsBeingDismissed, "IsBeingDismissed");
					ClassicAssert.False (vc.IsBeingPresented, "IsBeingPresented");
					ClassicAssert.False (vc.IsMovingFromParentViewController, "IsMovingFromParentViewController");
					ClassicAssert.False (vc.IsMovingToParentViewController, "IsMovingToParentViewController");
					ClassicAssert.False (vc.IsViewLoaded, "IsViewLoaded");
					ClassicAssert.False (vc.ModalInPopover, "ModalInPopover");
					ClassicAssert.Null (vc.NavigationController, "NavigationController");
					ClassicAssert.NotNull (vc.NibBundle, "NibBundle");
					ClassicAssert.Null (vc.NibName, "NibName");
					ClassicAssert.Null (vc.ParentViewController, "ParentViewController");
					ClassicAssert.Null (vc.PresentedViewController, "PresentedViewController");
					ClassicAssert.Null (vc.PresentingViewController, "PresentingViewController");
					ClassicAssert.False (vc.ProvidesPresentationContextTransitionStyle, "ProvidesPresentationContextTransitionStyle");
#if !__TVOS__
					ClassicAssert.True (vc.AutomaticallyForwardAppearanceAndRotationMethodsToChildViewControllers, "AutomaticallyForwardAppearanceAndRotationMethodsToChildViewControllers");
					ClassicAssert.False (vc.HidesBottomBarWhenPushed, "HidesBottomBarWhenPushed");
					ClassicAssert.Null (vc.ModalViewController, "ModalViewController");
					ClassicAssert.Null (vc.RotatingFooterView, "RotatingFooterView");
					ClassicAssert.Null (vc.RotatingHeaderView, "RotatingHeaderView");
#if !__MACCATALYST__
					ClassicAssert.Null (vc.SearchDisplayController, "SearchDisplayController");
#endif
					ClassicAssert.False (vc.WantsFullScreenLayout, "WantsFullScreenLayout");
#endif
					ClassicAssert.Null (vc.SplitViewController, "SplitViewController");
					ClassicAssert.Null (vc.Storyboard, "Storyboard");
					ClassicAssert.Null (vc.TabBarController, "TabBarController");
					ClassicAssert.NotNull (vc.TabBarItem, "TabBarItem");
					ClassicAssert.Null (vc.Title, "Title");
					ClassicAssert.Null (vc.ToolbarItems, "ToolbarItems");
					ClassicAssert.NotNull (vc.View, "View");
				});
			}
		}

		[Test]
		public void Toolbars_Null ()
		{
			using (var undo = new UIBarButtonItem (UIBarButtonSystemItem.Undo))
			using (var redo = new UIBarButtonItem (UIBarButtonSystemItem.Redo))
			using (var vc = new UIViewController ()) {
				var buttons = new UIBarButtonItem [] { undo, redo };
				vc.ToolbarItems = buttons;
				Assert.That (vc.ToolbarItems.Length, Is.EqualTo (2), "1");
				vc.ToolbarItems = null;
				ClassicAssert.Null (vc.ToolbarItems, "2");
#if !__TVOS__
				vc.SetToolbarItems (buttons, true);
				Assert.That (vc.ToolbarItems.Length, Is.EqualTo (2), "3");
				vc.SetToolbarItems (null, false);
				ClassicAssert.Null (vc.ToolbarItems, "4");
#endif
			}
		}

		[Test]
		public void View_Null ()
		{
			using (var vc = new UIViewController ()) {
				// even if the default is null <quote>The default value of this property is nil.</quote>
				// we'll never see it as such as it will be loaded (loadView)
				ClassicAssert.NotNull (vc.View, "View-a");
				// OTOH we can set it to null ourself
				// or the controller can do it if iOS runs out of memory
				vc.View = null;
				// but again, accessing it will load the view
				ClassicAssert.NotNull (vc.View, "View-b");
			}
		}

		[Test]
		public void AppearanceTransition ()
		{
			// this was retroactively documented as available in 5.0 (officially added in 6.0)
			// but respondToSelector return false
			using (var vc = new UIViewController ()) {
				vc.BeginAppearanceTransition (true, true);
				vc.EndAppearanceTransition ();
			}
		}
	}
}

#endif // !MONOMAC
