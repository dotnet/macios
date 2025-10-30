//
// Unit tests for EKAlarm
//
// Authors:
//	Rolf Bjarne Kvinge <rolf@xamarin.com>
//
// Copyright 2013 Xamarin Inc. All rights reserved.
//

#if !__TVOS__

using CoreGraphics;
using EventKit;
using Xamarin.Utils;

namespace MonoTouchFixtures.EventKit {

	[TestFixture]
	[Preserve (AllMembers = true)]
	public class AlarmTest {

		[Test]
		public void NullAllowedTest ()
		{
			TestRuntime.AssertSystemVersion (ApplePlatform.MacOSX, 10, 8, throwIfOtherPlatform: false);

			using (var alarm = EKAlarm.FromTimeInterval (1234)) {
				alarm.AbsoluteDate = null;
				alarm.StructuredLocation = null;
			}
		}
	}
}

#endif // !__TVOS__
