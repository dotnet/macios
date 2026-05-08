//
// Unit tests for AVSpeechUtterance

using AVFoundation;

#nullable enable

namespace MonoTouchFixtures.AVFoundation {

	[TestFixture]
	[Preserve (AllMembers = true)]
	public class AVSpeechUtteranceTest {
		[Test]
		public void StringCtor ()
		{
			using var utterance = new AVSpeechUtterance ("hello world");
			ClassicAssert.AreEqual (utterance.SpeechString, "hello world", "SpeechString");
		}

		[Test]
		public void StringOptionCtor_PlainText ()
		{
			using var utterance = new AVSpeechUtterance ("hello world", AVSpeechUtteranceInitializationOption.PlainText);
			ClassicAssert.AreEqual (utterance.SpeechString, "hello world", "SpeechString");
		}

		[Test]
		public void StringOptionCtor_Ssml ()
		{
			TestRuntime.AssertXcodeVersion (14, 0);

			var ssml = $"""<speak>Hello World</speak>""";
			using var utterance = new AVSpeechUtterance (ssml, AVSpeechUtteranceInitializationOption.SsmlRepresentation);
			ClassicAssert.AreEqual (utterance.SpeechString, "Hello World", "SpeechString");
		}
	}
}
