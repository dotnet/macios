//
// Unit tests for MTAudioProcessingTap
//
// Authors:
//	Marek Safar (marek.safar@gmail.com)
//
// Copyright 2012 Xamarin Inc, All rights reserved.
//

using MediaToolbox;
using AudioToolbox;
using AVFoundation;
using Xamarin.Utils;

namespace MonoTouchFixtures.MediaToolbox {
	[TestFixture]
	[Preserve (AllMembers = true)]
	public class AudioProcessingTapTest {
		[DllImport (Constants.CoreFoundationLibrary)]
		extern static nint CFGetRetainCount (IntPtr handle);

		[Test]
		public unsafe void Initialization ()
		{
			TestRuntime.AssertSystemVersion (ApplePlatform.MacOSX, 10, 9, throwIfOtherPlatform: false);

			var cb = new MTAudioProcessingTapCallbacks (
				delegate (MTAudioProcessingTap tap, nint numberFrames, MTAudioProcessingTapFlags flags, AudioBuffers bufferList, out nint numberFramesOut, out MTAudioProcessingTapFlags flagsOut)
				{
					numberFramesOut = 2;
					flagsOut = MTAudioProcessingTapFlags.StartOfStream;
				});

			cb.Initialize = delegate (MTAudioProcessingTap tap, out void* tapStorage)
			{
				tapStorage = (void*) 44;
			};

			IntPtr handle;
			using (var res = new MTAudioProcessingTap (cb, MTAudioProcessingTapCreationFlags.PreEffects)) {
				handle = res.Handle;
				Assert.That ((int) res.GetStorage (), Is.EqualTo (44));
				Assert.That (CFGetRetainCount (handle), Is.EqualTo ((nint) 1), "RC");
			}
		}

		[Test]
		public unsafe void InitializationWithPreferredFormat ()
		{
			TestRuntime.AssertXcodeVersion (27, 0);

			var cb = new MTAudioProcessingTapCallbacks (
				delegate (MTAudioProcessingTap tap, nint numberFrames, MTAudioProcessingTapFlags flags, AudioBuffers bufferList, out nint numberFramesOut, out MTAudioProcessingTapFlags flagsOut)
				{
					numberFramesOut = 2;
					flagsOut = MTAudioProcessingTapFlags.StartOfStream;
				});

			cb.Initialize = delegate (MTAudioProcessingTap tap, out void* tapStorage)
			{
				tapStorage = (void*) 44;
			};

			using var format = new AVAudioFormat (AVAudioCommonFormat.PCMFloat32, 44100, 2, true);
			using (var tap = new MTAudioProcessingTap (cb, MTAudioProcessingTapCreationFlags.PreEffects, format.FormatDescription)) {
				Assert.That ((int) tap.GetStorage (), Is.EqualTo (44), "Storage");
				Assert.That (CFGetRetainCount (tap.Handle), Is.EqualTo ((nint) 1), "RC");
			}

			using (var tap = new MTAudioProcessingTap (cb, MTAudioProcessingTapCreationFlags.PreEffects, null)) {
				Assert.That ((int) tap.GetStorage (), Is.EqualTo (44), "Storage - null");
				Assert.That (CFGetRetainCount (tap.Handle), Is.EqualTo ((nint) 1), "RC - null");
			}
		}
	}
}
