//
// Unit tests for CLBeaconRegion
//
// Authors:
//	Sebastien Pouliot <sebastien@xamarin.com>
//
// Copyright 2013 Xamarin Inc. All rights reserved.
//

#if !__TVOS__ && !MONOMAC

using UIKit;
using CoreLocation;
using Xamarin.Utils;

namespace MonoTouchFixtures.CoreLocation {

	[TestFixture]
	[Preserve (AllMembers = true)]
	public class BeaconRegionTest {

		[Test]
		public void Ctor2 ()
		{
			TestRuntime.AssertSystemVersion (ApplePlatform.iOS, 7, 0, throwIfOtherPlatform: false);

			using (var uuid = new NSUuid ("E2C56DB5-DFFB-48D2-B060-D0F5A71096E0"))
			using (var br = new CLBeaconRegion (uuid, "identifier")) {
				Assert.That (br.Major, Is.Null, "Major");
				Assert.That (br.Minor, Is.Null, "Minor");
				Assert.That (br.NotifyEntryStateOnDisplay, Is.False, "NotifyEntryStateOnDisplay");
				Assert.That (uuid.AsString (), Is.EqualTo (br.ProximityUuid.AsString ()), "ProximityUuid");
				// CLRegion
				Assert.That (br.Identifier, Is.EqualTo ("identifier"), "identifier");
				Assert.That (br.NotifyOnEntry, Is.True, "NotifyOnEntry");
				Assert.That (br.NotifyOnExit, Is.True, "NotifyOnExit");
				Assert.That (br.NotifyEntryStateOnDisplay, Is.False, "NotifyEntryStateOnDisplay");
			}
		}

		[Test]
		public void Ctor3 ()
		{
			TestRuntime.AssertSystemVersion (ApplePlatform.iOS, 7, 0, throwIfOtherPlatform: false);

			using (var uuid = new NSUuid ("E2C56DB5-DFFB-48D2-B060-D0F5A71096E0"))
			using (var br = new CLBeaconRegion (uuid, 0, "identifier")) {
				Assert.That (br.Major.Int32Value, Is.EqualTo (0), "Major");
				Assert.That (br.Minor, Is.Null, "Minor");
				Assert.That (br.NotifyEntryStateOnDisplay, Is.False, "NotifyEntryStateOnDisplay");
				Assert.That (uuid.AsString (), Is.EqualTo (br.ProximityUuid.AsString ()), "ProximityUuid");
				// CLRegion
				Assert.That (br.Identifier, Is.EqualTo ("identifier"), "identifier");
				Assert.That (br.NotifyOnEntry, Is.True, "NotifyOnEntry");
				Assert.That (br.NotifyOnExit, Is.True, "NotifyOnExit");
			}
		}

		[Test]
		public void Ctor4 ()
		{
			TestRuntime.AssertSystemVersion (ApplePlatform.iOS, 7, 0, throwIfOtherPlatform: false);

			using (var uuid = new NSUuid ("E2C56DB5-DFFB-48D2-B060-D0F5A71096E0"))
			using (var br = new CLBeaconRegion (uuid, 2, 3, "identifier")) {
				Assert.That (br.Major.Int32Value, Is.EqualTo (2), "Major");
				Assert.That (br.Minor.Int32Value, Is.EqualTo (3), "Minor");
				Assert.That (br.NotifyEntryStateOnDisplay, Is.False, "NotifyEntryStateOnDisplay");
				Assert.That (uuid.AsString (), Is.EqualTo (br.ProximityUuid.AsString ()), "ProximityUuid");
				// CLRegion
				Assert.That (br.Identifier, Is.EqualTo ("identifier"), "identifier");
				Assert.That (br.NotifyOnEntry, Is.True, "NotifyOnEntry");
				Assert.That (br.NotifyOnExit, Is.True, "NotifyOnExit");
			}
		}
	}
}

#endif // !__TVOS__ && !MONOMAC
