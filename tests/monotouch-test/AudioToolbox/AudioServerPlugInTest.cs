// Copyright (c) Microsoft Corporation.
// Licensed under the MIT License.

#nullable enable

#if __MACCATALYST__

using AudioToolbox;
using Xamarin.Utils;

namespace MonoTouchFixtures.AudioToolbox {

	[TestFixture]
	[Preserve (AllMembers = true)]
	public class AudioServerPlugInTest {

		[Test]
		public void RegisterMediaDeviceExtension ()
		{
			TestRuntime.AssertSystemVersion (ApplePlatform.MacCatalyst, 27, 0);

			_ = AudioServerPlugIn.RegisterMediaDeviceExtension (IntPtr.Zero, () => { });
			_ = AudioServerPlugIn.RegisterMediaDeviceExtension (IntPtr.Zero, null);
		}
	}
}

#endif // __MACCATALYST__
