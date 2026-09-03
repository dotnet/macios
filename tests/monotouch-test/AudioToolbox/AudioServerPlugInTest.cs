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

			if (NSProcessInfo.ProcessInfo.OperatingSystemVersionString.Contains ("26A5388", StringComparison.Ordinal))
				Assert.Ignore ("AudioServerPlugInRegisterMediaDeviceExtension crashes on macOS 27 beta 4.");

			_ = AudioServerPlugIn.RegisterMediaDeviceExtension (IntPtr.Zero, () => { });
			_ = AudioServerPlugIn.RegisterMediaDeviceExtension (IntPtr.Zero, null);
		}
	}
}

#endif // __MACCATALYST__
