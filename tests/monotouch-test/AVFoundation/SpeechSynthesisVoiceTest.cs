//
// Unit tests for AVSpeechSynthesisVoice
//
// Authors:
//	Sebastien Pouliot <sebastien@xamarin.com>
//
// Copyright 2014 Xamarin Inc. All rights reserved.
//

#if !MONOMAC
using AVFoundation;
#if MONOMAC
using AppKit;
#else
using UIKit;
#endif
using Xamarin.Utils;

namespace MonoTouchFixtures.AVFoundation {

	[TestFixture]
	[Preserve (AllMembers = true)]
	[TestFixture]
	public class SpeechSynthesisVoiceTest {

		[SetUp]
		public void SetUp ()
		{
			TestRuntime.AssertSystemVersion (ApplePlatform.iOS, 7, 0, throwIfOtherPlatform: false);
		}

		[Test]
		public void Default ()
		{
			// it's not clear that `init` should be called... it works (as it does not crash) but you can't set anything
			using (var ssv = new AVSpeechSynthesisVoice ()) {
				Assert.That (ssv.Language, Is.Null, "Language");
			}
		}

		[Test]
		public void Static ()
		{
			Assert.That (AVSpeechSynthesisVoice.CurrentLanguageCode, Is.Not.Null, "CurrentLanguageCode");
			foreach (var ssv in AVSpeechSynthesisVoice.GetSpeechVoices ()) {
				Assert.That (ssv.Language, Is.Not.Null, ssv.Language);
			}
		}
	}
}
#endif
