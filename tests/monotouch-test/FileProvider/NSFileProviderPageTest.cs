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

			Assert.That (NSFileProviderPage.InitialPageSortedByDate, Is.Not.Null, "InitialPageSortedByDate should not be null");
			Assert.That (NSFileProviderPage.InitialPageSortedByName, Is.Not.Null, "InitialPageSortedByName should not be null");
		}
	}
}
#endif
