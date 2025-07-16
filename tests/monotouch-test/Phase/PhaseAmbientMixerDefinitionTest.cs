#if __IOS__ || MONOMAC

using System;

using CoreGraphics;
using Foundation;
using ObjCRuntime;
using Phase;

using NUnit.Framework;
using AVFoundation;
using AudioToolbox;

using System.Numerics;

#nullable enable

namespace MonoTouchFixtures.Phase {

	[TestFixture]
	// we want the test to be available if we use the linker
	[Preserve (AllMembers = true)]
	public class PhaseAmbientMixerDefinitionTest {

		[SetUp]
		public void Setup ()
		{
			TestRuntime.AssertXcodeVersion (13, 0);
			// not supported in simulator
			TestRuntime.AssertDevice ();
		}

		[Test]
		public void TestConstructor ()
		{
			var orientation = new Quaternion (1, 0, 0, 0);
			using var layout = new AVAudioChannelLayout (AudioChannelLayoutTag.MPEG_5_1_A);
			using (var mixer = new PhaseAmbientMixerDefinition (layout, orientation)) {
				Assert.AreEqual (orientation, mixer.Orientation);
			}
		}
	}
}

#endif // __IOS__ || MONOMAC
