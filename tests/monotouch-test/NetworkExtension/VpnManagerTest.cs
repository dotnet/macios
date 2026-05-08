//
// Unit tests for NEVpnManager
//
// Authors:
//	Sebastien Pouliot <sebastien@xamarin.com>
//
// Copyright 2014 Xamarin Inc. All rights reserved.
//

#if !__TVOS__

using NetworkExtension;
using Xamarin.Utils;

namespace MonoTouchFixtures.NetworkExtension {

	[TestFixture]
	[Preserve (AllMembers = true)]
	public class VpnManagerTest {

		[Test]
		public void SharedManager ()
		{
			TestRuntime.AssertSystemVersion (ApplePlatform.iOS, 8, 0, throwIfOtherPlatform: false);

			var shared = NEVpnManager.SharedManager;
			// https://developer.apple.com/library/ios/documentation/IDEs/Conceptual/AppDistributionGuide/AddingCapabilities/AddingCapabilities.html#//apple_ref/doc/uid/TP40012582-CH26-SW59
			// Enabling Personal VPN (iOS Only)
			if (shared is null)
				Assert.Inconclusive ("Requires enabling Personal PVN (entitlements)");

			Assert.That (shared.Connection.Status, Is.EqualTo (NEVpnStatus.Invalid), "Connection");
#if MONOMAC || __MACCATALYST__
			ClassicAssert.True (shared.Enabled, "Enabled");
#else
			ClassicAssert.False (shared.Enabled, "Enabled");
#endif
#if __IOS__
			var HasLocalizedDescription = TestRuntime.CheckSystemVersion (ApplePlatform.iOS, 9, 0);
#elif __MACOS__
			var HasLocalizedDescription = true;
#endif
			if (HasLocalizedDescription) {
				ClassicAssert.AreEqual ("MonoTouchTest", shared.LocalizedDescription, "LocalizedDescription");
			} else {
				ClassicAssert.IsNull (shared.LocalizedDescription, "LocalizedDescription");
			}
			ClassicAssert.False (shared.OnDemandEnabled, "OnDemandEnabled");
			ClassicAssert.Null (shared.OnDemandRules, "OnDemandRules");
			ClassicAssert.Null (shared.Protocol, "Protocol");
		}

		[Test]
		public void Fields ()
		{
			TestRuntime.AssertSystemVersion (ApplePlatform.iOS, 8, 0, throwIfOtherPlatform: false);

			Assert.That (NEVpnError.ConnectionFailed.GetDomain ().ToString (), Is.EqualTo ("NEVPNErrorDomain"), "ErrorDomain");
		}
	}
}

#endif // !__TVOS__
