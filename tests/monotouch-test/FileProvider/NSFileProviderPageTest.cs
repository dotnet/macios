//
// Unit tests for NSFileProviderPage
//
// Authors:
//	Alex Soto <alexsoto@microsoft.com>
//
//
// Copyright 2017 Xamarin Inc. All rights reserved.
//

#if __IOS__ && !__MACCATALYST__

using FileProvider;

namespace MonoTouchFixtures.FileProvider {

	[TestFixture]
	[Preserve (AllMembers = true)]
	public class NSFileProviderPageTests {

		[Test]
		public void CompressionSessionCreateTest ()
		{
			// The FileProvider's NSData constants are only available running on device.
			TestRuntime.AssertDevice ();
			TestRuntime.AssertXcodeVersion (9, 0);

			Assert.IsNotNull (NSFileProviderPage.InitialPageSortedByDate, "InitialPageSortedByDate should not be null");
			Assert.IsNotNull (NSFileProviderPage.InitialPageSortedByName, "InitialPageSortedByName should not be null");
		}
	}
}
#endif
