//
// Unit tests for AudioFormat
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
	public class AudioFormatTest {
		[Test]
		public void AvailableEncodeFormats ()
		{
			var formats = new [] { new AudioStreamBasicDescription (AudioFormatType.MPEG4AAC) { SampleRate = 44100, ChannelsPerFrame = 2 } };
			ref var format = ref formats [0];
			Assert.That (AudioStreamBasicDescription.GetFormatInfo (ref format), Is.EqualTo (AudioFormatError.None), "GetFormatInfo");
			var expected = format;
			var tags = AudioStreamBasicDescription.GetAvailableEncodeChannelLayoutTags (format);
			Assert.That (tags, Is.Not.Null.And.Not.Empty, "Layout tags");
			Assert.That (tags, Does.Contain (AudioChannelLayoutTag.Stereo), "Stereo layout");
			Assert.That (format, Is.EqualTo (expected), "Layout format unchanged");
			var channels = AudioStreamBasicDescription.GetAvailableEncodeNumberChannels (format);
			Assert.That (channels, Is.Not.Null.And.Not.Empty, "Channel counts");
			Assert.That (channels, Does.Contain (2), "Stereo channels");
			Assert.That (format, Is.EqualTo (expected), "Channels format unchanged");
#if XAMCORE_5_0
			Assert.That (AudioStreamBasicDescription.GetAvailableEncodeChannelLayoutTags (in format), Is.EqualTo (tags), "Layout tags in");
			Assert.That (format, Is.EqualTo (expected), "Layout format in unchanged");
			Assert.That (AudioStreamBasicDescription.GetAvailableEncodeNumberChannels (in format), Is.EqualTo (channels), "Channel counts in");
			Assert.That (format, Is.EqualTo (expected), "Channels format in unchanged");
#endif
		}

		[Test]
		public void GetFirstPlayableFormat ()
		{
			var asbd = new AudioStreamBasicDescription (AudioFormatType.MPEG4AAC);
			AudioStreamBasicDescription.GetFormatInfo (ref asbd);

			var ofl = asbd.GetOutputFormatList ();

			Assert.That (AudioFormat.GetFirstPlayableFormat (ofl), Is.Not.Null);
		}
	}
}
