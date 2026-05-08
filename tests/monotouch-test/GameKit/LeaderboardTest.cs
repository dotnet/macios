//
// Unit tests for GKLeaderboard
//
// Authors:
//	Sebastien Pouliot  <sebastien@xamarin.com>
//
// Copyright 2013 Xamarin Inc. All rights reserved.
//

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
	public class LeaderboardTest {

		void Check (GKLeaderboard lb)
		{
#if !__TVOS__
			Assert.That (lb.Category, Is.Null, "Category");
#endif
#if __MACOS__
			var hasGroupIdentifier = true;
			var hasIdentifier = true;
			var hasRange = true;
#elif __IOS__
			var hasGroupIdentifier = TestRuntime.CheckSystemVersion (ApplePlatform.iOS, 6, 0);
			var hasIdentifier = TestRuntime.CheckSystemVersion (ApplePlatform.iOS, 7, 0);
			var hasRange = TestRuntime.CheckSystemVersion (ApplePlatform.iOS, 7, 0);
#elif __TVOS__
			var hasGroupIdentifier = true;
			var hasIdentifier = true;
			var hasRange = true;
#endif
			if (hasGroupIdentifier) {
				Assert.That (lb.GroupIdentifier, Is.Null, "GroupIdentifier");
				if (hasIdentifier)
					Assert.That (lb.Identifier, Is.Null, "Identifier");
			}
			Assert.That (lb.LocalPlayerScore, Is.Null, "LocalPlayerScore");
			Assert.That (lb.MaxRange, Is.EqualTo ((nint) 0), "MaxRange");
			Assert.That (lb.PlayerScope, Is.EqualTo (GKLeaderboardPlayerScope.Global), "PlayerScope");
			if (hasRange) {
				// depending on the ctor 1,10 (or 0,0) is returned before iOS7 - but 1,25 is documented (in iOS7)
				Assert.That (lb.Range.Location, Is.EqualTo ((nint) 1), "Range.Location");
				Assert.That (lb.Range.Length, Is.EqualTo ((nint) 25), "Range.Length");
			}
			Assert.That (lb.Scores, Is.Null, "Scores");
			Assert.That (lb.TimeScope, Is.EqualTo (GKLeaderboardTimeScope.AllTime), "TimeScope");
			Assert.That (lb.Title, Is.Null, "Title");
		}

		[Test]
		public void DefaultCtor ()
		{
			using (var lb = new GKLeaderboard ()) {
				Check (lb);
			}
		}

		[Test]
		public void PlayersCtor ()
		{
			// note: Mavericks does not like (respond to) this selector - but it did work with ML and is documented
			using (var lb = new GKLeaderboard (new string [0])) {
				Check (lb);
			}
		}
	}
}
