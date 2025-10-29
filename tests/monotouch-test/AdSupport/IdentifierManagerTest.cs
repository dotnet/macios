//
// Unit tests for ASIdentifierManager
//
// Authors:
//	Sebastien Pouliot <sebastien@xamarin.com>
//
// Copyright 2012,2015 Xamarin Inc. All rights reserved.
//

#if !MONOMAC

using UIKit;
using AdSupport;

namespace MonoTouchFixtures.AdSupport {

	[TestFixture]
	[Preserve (AllMembers = true)]
	public class IdentifierManagerTest {

		[Test]
		public void SharedManager ()
		{
			// IsAdvertisingTrackingEnabled - device specific config
			Assert.NotNull (ASIdentifierManager.SharedManager.AdvertisingIdentifier, "AdvertisingIdentifier");
		}
	}
}

#endif // !MONOMAC
