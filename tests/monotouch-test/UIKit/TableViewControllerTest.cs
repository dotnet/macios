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
				ClassicAssert.Null (tvc.RefreshControl, "default");
				tvc.RefreshControl = rc;
				ClassicAssert.AreSame (tvc.RefreshControl, rc, "same");
				tvc.RefreshControl = null;
				ClassicAssert.Null (tvc.RefreshControl, "nullable");
			}
		}
#endif // !__TVOS__
	}
}

#endif // !MONOMAC
