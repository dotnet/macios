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
			Assert.NotNull (CFNotificationCenter.Darwin, "Darwin");
			Assert.NotNull (CFNotificationCenter.Local, "Local");
		}
	}
}
