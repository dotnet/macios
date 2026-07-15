//
// Unit tests for UITableViewController
//
// Authors:
//	Sebastien Pouliot <sebastien@xamarin.com>
//
// Copyright 2014 Xamarin Inc. All rights reserved.
//

#if !MONOMAC

using System.Drawing;
using UIKit;

namespace MonoTouchFixtures.UIKit {

	[TestFixture]
	[Preserve (AllMembers = true)]
	public class TableViewControllerTest {

#if !__TVOS__
		[Test]
		public void RefreshControl_18744 ()
		{
			using (var rc = new UIRefreshControl ())
			using (var tvc = new UITableViewController ()) {
				Assert.That (tvc.RefreshControl, Is.Null, "default");
				tvc.RefreshControl = rc;
				Assert.That (rc, Is.SameAs (tvc.RefreshControl), "same");
				tvc.RefreshControl = null;
				Assert.That (tvc.RefreshControl, Is.Null, "nullable");
			}
		}
#endif // !__TVOS__
	}
}

#endif // !MONOMAC
