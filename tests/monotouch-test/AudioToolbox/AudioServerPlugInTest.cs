// Copyright (c) Microsoft Corporation.
// Licensed under the MIT License.

#nullable enable

#if __MACCATALYST__

using AudioToolbox;

namespace MonoTouchFixtures.AudioToolbox {

	[TestFixture]
	[Preserve (AllMembers = true)]
	public class AudioServerPlugInTest {

		[Test]
		public void RegisterMediaDeviceExtensionSignature ()
		{
			// Native registration with IntPtr.Zero can asynchronously crash CoreAudio.
			// Verify the managed signature without invoking the native API.
			Func<IntPtr, Action?, int> method = AudioServerPlugIn.RegisterMediaDeviceExtension;

			Assert.That (method, Is.Not.Null);
		}
	}
}

#endif // __MACCATALYST__
