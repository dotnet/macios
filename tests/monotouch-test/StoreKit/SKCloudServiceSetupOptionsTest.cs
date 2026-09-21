//
// Unit tests for SKCloudServiceSetupOptionsTest
//
// Authors:
//	Vincent Dondain <vidondai@microsoft.com>
//
// Copyright 2017 Microsoft. All rights reserved.
//

#if __IOS__

using StoreKit;

namespace MonoTouchFixtures.StoreKit {

	[TestFixture]
	[Preserve (AllMembers = true)]
	public class SKCloudServiceSetupOptionsTest {

		[Test]
		public void ActionTest ()
		{
			TestRuntime.AssertXcodeVersion (8, 1);

			var optionsObject = new SKCloudServiceSetupOptions {
				Action = SKCloudServiceSetupAction.Subscribe
			};
			Assert.That (optionsObject.Dictionary ["SKCloudServiceSetupOptionsActionKey"].ToString (), Is.EqualTo ("sdkSubscribe"), "SKCloudServiceSetupOptionsActionKey");
			Assert.That (optionsObject.Action, Is.EqualTo (SKCloudServiceSetupAction.Subscribe), "SKCloudServiceSetupOptions.Action");
		}
	}
}

#endif // __IOS__
