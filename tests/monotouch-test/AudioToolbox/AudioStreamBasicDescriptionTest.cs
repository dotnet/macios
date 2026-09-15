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
			Assert.That (pcm.FormatName, Is.Not.Null);
			Assert.That (pcm.IsVariableBitrate, Is.False);
		}

		[Test]
		public void VBR ()
		{
			var mp3 = new AudioStreamBasicDescription (AudioFormatType.MPEGLayer3);
			Assert.That (mp3.IsVariableBitrate, Is.True);
		}

		[Test]
		public void GetFormatInfo ()
		{
			var asbd = new AudioStreamBasicDescription (AudioFormatType.MPEG4AAC);
			Assert.That (AudioStreamBasicDescription.GetFormatInfo (ref asbd), Is.EqualTo (AudioFormatError.None));

			Assert.That (AudioStreamBasicDescription.GetAvailableEncodeChannelLayoutTags (asbd), Is.Not.Null);
			Assert.That (AudioStreamBasicDescription.GetAvailableEncodeNumberChannels (asbd), Is.Not.Null);
			Assert.That (asbd.GetOutputFormatList (), Is.Not.Null);
		}
	}
}
