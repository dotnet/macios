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
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
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
					Assert.That (vc.DefinesPresentationContext, Is.False, "DefinesPresentationContext");
					Assert.That (vc.DisablesAutomaticKeyboardDismissal, Is.EqualTo (true).Or.EqualTo (false), "DisablesAutomaticKeyboardDismissal");
					Assert.That (vc.Editing, Is.False, "Editing");
					Assert.That (vc.IsBeingDismissed, Is.False, "IsBeingDismissed");
					Assert.That (vc.IsBeingPresented, Is.False, "IsBeingPresented");
					Assert.That (vc.IsMovingFromParentViewController, Is.False, "IsMovingFromParentViewController");
					Assert.That (vc.IsMovingToParentViewController, Is.False, "IsMovingToParentViewController");
					Assert.That (vc.IsViewLoaded, Is.False, "IsViewLoaded");
					Assert.That (vc.ModalInPopover, Is.False, "ModalInPopover");
					Assert.That (vc.NavigationController, Is.Null, "NavigationController");
					Assert.That (vc.NibBundle, Is.Not.Null, "NibBundle");
					Assert.That (vc.NibName, Is.Null, "NibName");
					Assert.That (vc.ParentViewController, Is.Null, "ParentViewController");
					Assert.That (vc.PresentedViewController, Is.Null, "PresentedViewController");
					Assert.That (vc.PresentingViewController, Is.Null, "PresentingViewController");
					Assert.That (vc.ProvidesPresentationContextTransitionStyle, Is.False, "ProvidesPresentationContextTransitionStyle");
#if !__TVOS__
					Assert.That (vc.AutomaticallyForwardAppearanceAndRotationMethodsToChildViewControllers, Is.True, "AutomaticallyForwardAppearanceAndRotationMethodsToChildViewControllers");
					Assert.That (vc.HidesBottomBarWhenPushed, Is.False, "HidesBottomBarWhenPushed");
					Assert.That (vc.ModalViewController, Is.Null, "ModalViewController");
					Assert.That (vc.RotatingFooterView, Is.Null, "RotatingFooterView");
					Assert.That (vc.RotatingHeaderView, Is.Null, "RotatingHeaderView");
#if !__MACCATALYST__
					Assert.That (vc.SearchDisplayController, Is.Null, "SearchDisplayController");
#endif
					Assert.That (vc.WantsFullScreenLayout, Is.False, "WantsFullScreenLayout");
#endif
					Assert.That (vc.SplitViewController, Is.Null, "SplitViewController");
					Assert.That (vc.Storyboard, Is.Null, "Storyboard");
					Assert.That (vc.TabBarController, Is.Null, "TabBarController");
					Assert.That (vc.TabBarItem, Is.Not.Null, "TabBarItem");
					Assert.That (vc.Title, Is.Null, "Title");
					Assert.That (vc.ToolbarItems, Is.Null, "ToolbarItems");
					Assert.That (vc.View, Is.Not.Null, "View");
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
				Assert.That (vc.ToolbarItems, Is.Null, "2");
#if !__TVOS__
				vc.SetToolbarItems (buttons, true);
				Assert.That (vc.ToolbarItems.Length, Is.EqualTo (2), "3");
				vc.SetToolbarItems (null, false);
				Assert.That (vc.ToolbarItems, Is.Null, "4");
#endif
			}
		}

		[Test]
		public void View_Null ()
		{
			using (var vc = new UIViewController ()) {
				// even if the default is null <quote>The default value of this property is nil.</quote>
				// we'll never see it as such as it will be loaded (loadView)
				Assert.That (vc.View, Is.Not.Null, "View-a");
				// OTOH we can set it to null ourself
				// or the controller can do it if iOS runs out of memory
				vc.View = null;
				// but again, accessing it will load the view
				Assert.That (vc.View, Is.Not.Null, "View-b");
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

		[TestCase (false)]
		[TestCase (true)]
		public void RunAsyncKeepsVisibleRoot (bool useNavigationController)
		{
			if (useNavigationController && UIDevice.CurrentDevice.UserInterfaceIdiom != UIUserInterfaceIdiom.Phone)
				Assert.Ignore ("Navigation-controller containment is only used on iPhone.");

			var originalWindow = UIApplication.SharedApplication.ConnectedScenes
				.OfType<UIWindowScene> ()
				.SelectMany (scene => scene.Windows)
				.LastOrDefault (window => window.IsKeyWindow);
			Assert.That (originalWindow, Is.Not.Null, "Existing key window");

			using var content = new AppearanceTrackingController ();
			using var navigation = useNavigationController ? new UINavigationController (content) : null;
			UIViewController root = content;
			if (navigation is not null)
				root = navigation;
			using var window = new UIWindow (originalWindow.WindowScene);
			var timeout = TimeSpan.FromSeconds (5);
			try {
				window.RootViewController = root;
				window.MakeKeyAndVisible ();
				Assert.That (NSRunLoop.Main.RunUntil (content.Appeared.Task, timeout), Is.True, "Root appeared");
				Assert.That (window.IsKeyWindow, Is.True, "Temporary key window");
				var originalChildren = root.ChildViewControllers;
				var originalAppearance = content.Appearance.ToArray ();
				Assert.That (originalAppearance, Is.EqualTo (new [] { "WillAppear", "DidAppear" }), "Initial appearance");

				for (var iteration = 0; iteration < 2; iteration++) {
					UIViewController overlay = null;
					var inspected = false;
					try {
						Assert.That (TestRuntime.RunAsync (timeout, () => {
							Assert.That (window.RootViewController, Is.SameAs (root), "Root during RunAsync");
							overlay = root.ChildViewControllers.Except (originalChildren).Single ();
							Assert.That (overlay.ParentViewController, Is.SameAs (root), "Overlay parent");
							Assert.That (overlay.View.Superview, Is.SameAs (root.View), "Overlay superview");
							Assert.That (overlay.View.Window, Is.SameAs (window), "Overlay window");
							Assert.That (overlay.View.Frame, Is.EqualTo (root.View.Bounds), "Overlay frame");
							if (navigation is not null)
								Assert.That (navigation.ViewControllers, Is.EqualTo (new [] { content }), "Navigation stack");
							inspected = true;
						}, () => inspected), Is.True, "RunAsync completed");
						Assert.That (inspected, Is.True, "Inspected overlay");
						Assert.That (overlay.ParentViewController, Is.Null, "Detached overlay parent");
						Assert.That (overlay.View.Superview, Is.Null, "Detached overlay view");
						Assert.That (window.RootViewController, Is.SameAs (root), "Root after RunAsync");
						Assert.That (root.ChildViewControllers, Is.EqualTo (originalChildren), "Restored children");
						Assert.That (content.Appearance, Is.EqualTo (originalAppearance), "Root appearance unchanged");
					} finally {
						overlay?.Dispose ();
					}
				}
			} finally {
				window.Hidden = true;
				window.RootViewController = null;
				originalWindow.MakeKeyWindow ();
			}
		}

		[Preserve (AllMembers = true)]
		class AppearanceTrackingController : UIViewController {
			public List<string> Appearance { get; } = new List<string> ();
			public TaskCompletionSource<bool> Appeared { get; } = new TaskCompletionSource<bool> ();

			public override void ViewWillAppear (bool animated)
			{
				base.ViewWillAppear (animated);
				Appearance.Add ("WillAppear");
			}

			public override void ViewDidAppear (bool animated)
			{
				base.ViewDidAppear (animated);
				Appearance.Add ("DidAppear");
				Appeared.TrySetResult (true);
			}

			public override void ViewWillDisappear (bool animated)
			{
				base.ViewWillDisappear (animated);
				Appearance.Add ("WillDisappear");
			}

			public override void ViewDidDisappear (bool animated)
			{
				base.ViewDidDisappear (animated);
				Appearance.Add ("DidDisappear");
			}
		}
	}
}

#endif // !MONOMAC
