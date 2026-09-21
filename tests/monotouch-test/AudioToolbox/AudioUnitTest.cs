//
// Unit tests for AudioUnit
//
// Authors:
//	Rolf Bjarne Kvinge (rolf@xamarin.com)
//
// Copyright 2022 Microsoft Corp. All rights reserved.
//

#if __MACOS__

using System.Threading;

using AudioToolbox;
using AudioUnit;

namespace MonoTouchFixtures.AudioToolbox {
	[TestFixture]
	[Preserve (AllMembers = true)]
	public class AudioUnitTest {
		ManualResetEvent inputCallbackEvent = new ManualResetEvent (false);

		// This test currently only works on macOS, probably due to missing microphone entitlements/permissions for mobile platforms.
		[Test]
		public void Callbacks ()
		{
			TestRuntime.IgnoreIfLockedScreen ();

			var audioComponent = AudioComponent.FindComponent (AudioTypeOutput.VoiceProcessingIO);
			using var audioUnit = new global::AudioUnit.AudioUnit (audioComponent);

			var rv = audioUnit.SetInputCallback (InputCallback, AudioUnitScopeType.Input, 1);
			if (rv == AudioUnitStatus.CannotDoInCurrentContext)
				Assert.Ignore ("Can't set input callback"); // No microphone? In a VM? This seems to happen often on bots.
			Assert.That (rv, Is.EqualTo (AudioUnitStatus.OK), "SetInputCallback");
			Assert.That (audioUnit.Initialize (), Is.EqualTo (AudioUnitStatus.OK), "Initialize");
			try {
				Assert.That (audioUnit.Start (), Is.EqualTo (AudioUnitStatus.OK), "Start");
				Assert.That (inputCallbackEvent.WaitOne (TimeSpan.FromSeconds (1)), Is.True, "No input callback for 1 second");
			} finally {
				Assert.That (audioUnit.Stop (), Is.EqualTo (AudioUnitStatus.OK), "Stop");
			}
		}

		AudioUnitStatus InputCallback (AudioUnitRenderActionFlags actionFlags, AudioTimeStamp timeStamp, uint busNumber, uint numberFrames, global::AudioUnit.AudioUnit audioUnit)
		{
			inputCallbackEvent.Set ();
			return AudioUnitStatus.NoError;
		}
	}
}

#endif // __MACOS__
