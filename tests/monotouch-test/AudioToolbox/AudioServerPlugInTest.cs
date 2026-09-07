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

			// The native API crashes instead of returning an error status on these macOS 27 builds.
			// Add affected EXPECTED_MACOS_BUILD_VERSION prefixes here until the behavior changes.
			if (NSProcessInfo.ProcessInfo.OperatingSystemVersionString.Contains ("26A5388", StringComparison.Ordinal) ||
				NSProcessInfo.ProcessInfo.OperatingSystemVersionString.Contains ("26A5425", StringComparison.Ordinal))
				Assert.Ignore ("AudioServerPlugInRegisterMediaDeviceExtension crashes on macOS 27 builds 26A5388 and 26A5425.");

			_ = AudioServerPlugIn.RegisterMediaDeviceExtension (IntPtr.Zero, () => { });
			_ = AudioServerPlugIn.RegisterMediaDeviceExtension (IntPtr.Zero, null);
		}
	}
}

#endif // __MACCATALYST__
