//
// Unit tests for PKAddPassesViewController
//
// Authors:
//	Sebastien Pouliot <sebastien@xamarin.com>
//
// Copyright 2012 Xamarin Inc. All rights reserved.
//

#if !__TVOS__ && !MONOMAC

using UIKit;
using PassKit;
using Xamarin.Utils;

namespace MonoTouchFixtures.PassKit {

	[TestFixture]
	[Preserve (AllMembers = true)]
	public class AddPassesViewControllerTest {

		[Test]
		public void BoardingPass ()
		{
			if (UIDevice.CurrentDevice.UserInterfaceIdiom != UIUserInterfaceIdiom.Phone)
				Assert.Inconclusive ("PassKit does not work on iPads");

			using (var pass = PassTest.GetBoardingPass ())
			using (var ctrl = new PKAddPassesViewController (pass)) {
				ctrl.Finished += delegate { };
				// not available on iPad...
				ClassicAssert.True ((ctrl.Delegate is not null) == PKPassLibrary.IsAvailable, "Delegate");
				ClassicAssert.True ((ctrl.WeakDelegate is not null) == PKPassLibrary.IsAvailable, "WeakDelegate");
			}
		}

		[Test]
		public void InitWithNibNameTest ()
		{
			// initWithNibName:bundle: returns nil in iOS 6
			TestRuntime.AssertSystemVersion (ApplePlatform.iOS, 7, 0, throwIfOtherPlatform: false);

			if (UIDevice.CurrentDevice.UserInterfaceIdiom != UIUserInterfaceIdiom.Phone)
				Assert.Inconclusive ("PassKit does not work on iPads");

			PKAddPassesViewController ctrl = new PKAddPassesViewController (null, null);
			ClassicAssert.NotNull (ctrl, "PKAddPassesViewController ctor(String, NSBundle)");

			ctrl.Finished += delegate { };
			ClassicAssert.True ((ctrl.Delegate is not null) == PKPassLibrary.IsAvailable, "Delegate");
			ClassicAssert.True ((ctrl.WeakDelegate is not null) == PKPassLibrary.IsAvailable, "WeakDelegate");
		}
	}
}

#endif // !__TVOS__ && !MONOMAC
