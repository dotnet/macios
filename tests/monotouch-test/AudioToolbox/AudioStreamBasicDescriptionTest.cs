//
// Unit tests for AudioStreamBasicDescription
//
// Authors:
//	Marek Safar (marek.safar@gmail.com)
//
// Copyright 2012 Xamarin Inc. All rights reserved.
//

using AudioToolbox;

namespace MonoTouchFixtures.AudioToolbox {

	[TestFixture]
	[Preserve (AllMembers = true)]
	public class AudioStreamBasicDescriptionTest {
		[Test]
		public void CreateLinearPCM ()
		{
			var pcm = AudioStreamBasicDescription.CreateLinearPCM ();
			ClassicAssert.IsNotNull (pcm.FormatName);
			ClassicAssert.IsFalse (pcm.IsVariableBitrate);
		}

		[Test]
		public void VBR ()
		{
			var mp3 = new AudioStreamBasicDescription (AudioFormatType.MPEGLayer3);
			ClassicAssert.IsTrue (mp3.IsVariableBitrate);
		}

		[Test]
		public void GetFormatInfo ()
		{
			var asbd = new AudioStreamBasicDescription (AudioFormatType.MPEG4AAC);
			ClassicAssert.AreEqual (AudioFormatError.None, AudioStreamBasicDescription.GetFormatInfo (ref asbd));

			ClassicAssert.IsNotNull (AudioStreamBasicDescription.GetAvailableEncodeChannelLayoutTags (asbd));
			ClassicAssert.IsNotNull (AudioStreamBasicDescription.GetAvailableEncodeNumberChannels (asbd));
			ClassicAssert.IsNotNull (asbd.GetOutputFormatList ());
		}
	}
}
