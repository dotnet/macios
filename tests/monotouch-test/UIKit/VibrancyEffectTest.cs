//
// Unit tests for UIVibrancyEffect
//
// Authors:
//	Sebastien Pouliot  <sebastien@xamarin.com>
//
// Copyright 2014 Xamarin Inc. All rights reserved.
//

#if HAS_NOTIFICATIONCENTER && HAS_UIKIT

using UIKit;
#if !__TVOS__
using NotificationCenter;
#endif
using Xamarin.Utils;

namespace MonoTouchFixtures.UIKit {

	[TestFixture]
	[Preserve (AllMembers = true)]
	public class VibrancyEffectTest {
		[Test]
		public void NotificationCenterVibrancyEffect_New ()
		{
			TestRuntime.AssertSystemVersion (ApplePlatform.iOS, 8, 0, throwIfOtherPlatform: false);

			UIVibrancyEffect.CreateForNotificationCenter ();
		}
	}
}

#endif // HAS_NOTIFICATIONCENTER
