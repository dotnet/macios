//
// Unit tests for GKLeaderboardViewController
//
// Authors:
//	Sebastien Pouliot  <sebastien@xamarin.com>
//
// Copyright 2013 Xamarin Inc. All rights reserved.
//

#if !__TVOS__ && !__MACCATALYST__

using System.IO;
using System.Threading;
#if MONOMAC
using AppKit;
#else
using UIKit;
#endif
using GameKit;
using Xamarin.Utils;

namespace MonoTouchFixtures.GameKit {

	[TestFixture]
	[Preserve (AllMembers = true)]
	public class LeaderboardViewControllerTest {

		[Test]
		public void DefaultCtor ()
		{
#if MONOMAC
			Assert.Inconclusive ("'LeaderboardViewControllerTest' the native 'init' method returned nil.");
#endif
			using (var vc = new GKLeaderboardViewController ()) {
				Assert.That (vc.Category, Is.Null, "Category");
				Assert.That (vc.Delegate, Is.Null, "Delegate");
				// default Scope vary by iOS version and can't be changed on iOS7 - not worth testing
			}
		}
	}
}

#endif // !__TVOS__ && !__MACCATALYST__
