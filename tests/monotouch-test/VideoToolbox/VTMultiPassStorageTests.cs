//
// Unit tests for VTMultiPassStorage
//
// Authors:
//	Alex Soto <alex.soto@xamarin.com>
//	
//
// Copyright 2015 Xamarin Inc. All rights reserved.
//

using VideoToolbox;
using CoreMedia;
using AVFoundation;
using Xamarin.Utils;

namespace MonoTouchFixtures.VideoToolbox {

	[TestFixture]
	[Preserve (AllMembers = true)]
	public class VTMultiPassStorageTests {
		[Test]
		public void MultiPassStorageCreateTest ()
		{
			TestRuntime.AssertSystemVersion (ApplePlatform.iOS, 8, 0, throwIfOtherPlatform: false);
			TestRuntime.AssertSystemVersion (ApplePlatform.MacOSX, 10, 10, throwIfOtherPlatform: false);
			TestRuntime.AssertSystemVersion (ApplePlatform.TVOS, 10, 2, throwIfOtherPlatform: false);

			using (var storage = VTMultiPassStorage.Create ()) {
				Assert.That (storage, Is.Not.Null, "Storage should not be null");
			}
		}

		[Test]
		public void MultiPassStorageCloseTest ()
		{
			TestRuntime.AssertSystemVersion (ApplePlatform.iOS, 8, 0, throwIfOtherPlatform: false);
			TestRuntime.AssertSystemVersion (ApplePlatform.MacOSX, 10, 10, throwIfOtherPlatform: false);
			TestRuntime.AssertSystemVersion (ApplePlatform.TVOS, 10, 2, throwIfOtherPlatform: false);

			using (var storage = VTMultiPassStorage.Create ()) {
				var result = storage.Close ();
				Assert.That (result == VTStatus.Ok, Is.True, "VTMultiPassStorage Close");
			}
		}
	}
}
