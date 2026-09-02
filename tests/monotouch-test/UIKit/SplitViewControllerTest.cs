// Copyright 2012 Xamarin Inc. All rights reserved

#if !MONOMAC

using System.Drawing;
using System.Reflection;
using UIKit;

namespace MonoTouchFixtures.UIKit {
	[TestFixture]
	[Preserve (AllMembers = true)]
	public class SplitViewControllerTest {

		[Test]
		public void Defaults ()
		{
			// UISplitViewController feature is only available on iPads
			// and we (presently) crash on devices when an objective-c exception is thrown
			if (UIDevice.CurrentDevice.UserInterfaceIdiom == UIUserInterfaceIdiom.Phone)
				return;

			using (UIViewController v1 = new UIViewController ())
			using (UIViewController v2 = new UIViewController ())
			using (UISplitViewController svc = new UISplitViewController ()) {
				Assert.That (svc.ViewControllers, Is.Empty, "ViewControllers"); // not null, empty

				svc.ViewControllers = new UIViewController [] { v1, v2 };

				Assert.That (svc.ViewControllers [0], Is.SameAs (v1), "vc0");
				Assert.That (svc.ViewControllers [1], Is.SameAs (v2), "vc1");

				if (!TestRuntime.CheckXcodeVersion (26, 0)) {
					Assert.That (svc.ChildViewControllers.Length, Is.AtLeast (2), "cvc.Length");
					Assert.That (svc.ChildViewControllers [0], Is.SameAs (v1), "cvc0");
					Assert.That (svc.ChildViewControllers [1], Is.SameAs (v2), "cvc1");
				}
			}
		}

		[Test]
		public void PresentsWithGesture ()
		{
			// UISplitViewController feature is only available on iPads
			// and we (presently) crash on devices when an objective-c exception is thrown
			if (UIDevice.CurrentDevice.UserInterfaceIdiom == UIUserInterfaceIdiom.Phone)
				return;

			TestRuntime.IgnoreOnTVOS ();

			using (UISplitViewController svc = new UISplitViewController ()) {
				Assert.That (svc.PresentsWithGesture, Is.True, "PresentsWithGesture/default");
			}
		}
	}
}

#endif // !MONOMAC
