//
// Unit tests for CFNotificationCenter
//
// Authors:
//	Sebastien Pouliot  <sebastien@xamarin.com>
//
// Copyright 2015 Xamarin Inc. All rights reserved.
//

namespace MonoTouchFixtures.CoreFoundation {

	[TestFixture]
	[Preserve (AllMembers = true)]
	public class NotificationCenterTest {

		[Test]
		public void Static ()
		{
			ClassicAssert.NotNull (CFNotificationCenter.Darwin, "Darwin");
			ClassicAssert.NotNull (CFNotificationCenter.Local, "Local");
		}
	}
}
