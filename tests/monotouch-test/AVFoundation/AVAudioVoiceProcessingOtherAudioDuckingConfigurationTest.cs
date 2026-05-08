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
			ClassicAssert.IsFalse (s.EnableAdvancedDucking, "EnableAdvancedDucking");
			ClassicAssert.AreEqual ((AVAudioVoiceProcessingOtherAudioDuckingLevel) 0, s.DuckingLevel, "DuckingLevel");

			s.EnableAdvancedDucking = true;
			ClassicAssert.IsTrue (s.EnableAdvancedDucking, "EnableAdvancedDucking 2");
			ClassicAssert.AreEqual ((AVAudioVoiceProcessingOtherAudioDuckingLevel) 0, s.DuckingLevel, "DuckingLevel 2");

			s.EnableAdvancedDucking = false;
			ClassicAssert.IsFalse (s.EnableAdvancedDucking, "EnableAdvancedDucking 3");
			ClassicAssert.AreEqual ((AVAudioVoiceProcessingOtherAudioDuckingLevel) 0, s.DuckingLevel, "DuckingLevel 3");

			s.DuckingLevel = AVAudioVoiceProcessingOtherAudioDuckingLevel.Min;
			ClassicAssert.IsFalse (s.EnableAdvancedDucking, "EnableAdvancedDucking 4");
			ClassicAssert.AreEqual (AVAudioVoiceProcessingOtherAudioDuckingLevel.Min, s.DuckingLevel, "DuckingLevel 4");

			s.DuckingLevel = (AVAudioVoiceProcessingOtherAudioDuckingLevel) 314;
			ClassicAssert.IsFalse (s.EnableAdvancedDucking, "EnableAdvancedDucking 5");
			ClassicAssert.AreEqual ((AVAudioVoiceProcessingOtherAudioDuckingLevel) 314, s.DuckingLevel, "DuckingLevel 5");

			s.DuckingLevel = AVAudioVoiceProcessingOtherAudioDuckingLevel.Default;
			ClassicAssert.IsFalse (s.EnableAdvancedDucking, "EnableAdvancedDucking 6");
			ClassicAssert.AreEqual (AVAudioVoiceProcessingOtherAudioDuckingLevel.Default, s.DuckingLevel, "DuckingLevel 6");
		}
	}
}

#endif // !__TVOS__
