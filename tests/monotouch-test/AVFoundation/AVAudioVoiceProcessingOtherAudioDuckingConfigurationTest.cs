//
// Unit tests for AVAudioVoiceProcessingOtherAudioDuckingConfiguration
//

#if !__TVOS__

using AVFoundation;

#nullable enable

namespace MonoTouchFixtures.AVFoundation {

	[TestFixture]
	[Preserve (AllMembers = true)]
	public class AVAudioVoiceProcessingOtherAudioDuckingConfigurationTest {

		[Test]
		public void Properties ()
		{
			var s = new AVAudioVoiceProcessingOtherAudioDuckingConfiguration ();
			Assert.That (s.EnableAdvancedDucking, Is.False, "EnableAdvancedDucking");
			Assert.That (s.DuckingLevel, Is.EqualTo ((AVAudioVoiceProcessingOtherAudioDuckingLevel) 0), "DuckingLevel");

			s.EnableAdvancedDucking = true;
			Assert.That (s.EnableAdvancedDucking, Is.True, "EnableAdvancedDucking 2");
			Assert.That (s.DuckingLevel, Is.EqualTo ((AVAudioVoiceProcessingOtherAudioDuckingLevel) 0), "DuckingLevel 2");

			s.EnableAdvancedDucking = false;
			Assert.That (s.EnableAdvancedDucking, Is.False, "EnableAdvancedDucking 3");
			Assert.That (s.DuckingLevel, Is.EqualTo ((AVAudioVoiceProcessingOtherAudioDuckingLevel) 0), "DuckingLevel 3");

			s.DuckingLevel = AVAudioVoiceProcessingOtherAudioDuckingLevel.Min;
			Assert.That (s.EnableAdvancedDucking, Is.False, "EnableAdvancedDucking 4");
			Assert.That (s.DuckingLevel, Is.EqualTo (AVAudioVoiceProcessingOtherAudioDuckingLevel.Min), "DuckingLevel 4");

			s.DuckingLevel = (AVAudioVoiceProcessingOtherAudioDuckingLevel) 314;
			Assert.That (s.EnableAdvancedDucking, Is.False, "EnableAdvancedDucking 5");
			Assert.That (s.DuckingLevel, Is.EqualTo ((AVAudioVoiceProcessingOtherAudioDuckingLevel) 314), "DuckingLevel 5");

			s.DuckingLevel = AVAudioVoiceProcessingOtherAudioDuckingLevel.Default;
			Assert.That (s.EnableAdvancedDucking, Is.False, "EnableAdvancedDucking 6");
			Assert.That (s.DuckingLevel, Is.EqualTo (AVAudioVoiceProcessingOtherAudioDuckingLevel.Default), "DuckingLevel 6");
		}
	}
}

#endif // !__TVOS__
