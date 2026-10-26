// Unit test for AVAudioRecorder
// Authors: 
// 		Paola Villarreal (paola.villarreal@xamarin.com)
// Copyright 2014 Xamarin Inc. All rights reserved.

#if !__TVOS__

using AudioToolbox;
using AVFoundation;
namespace MonoTouchFixtures.AVFoundation {

	[TestFixture]
	[Preserve (AllMembers = true)]
	public class AudioRecorderTest {
		[Test]
		public void ChannelLayoutRoundTrip ()
		{
			var settings = new AudioSettings ();
			Assert.That (settings.ChannelLayout, Is.Null, "Default");
			var layout = new AudioChannelLayout {
				AudioTag = AudioChannelLayoutTag.Stereo,
				Channels = [],
			};
			settings.ChannelLayout = layout;
			var actual = settings.ChannelLayout;
			Assert.That (actual, Is.Not.Null, "Layout");
			Assert.That (actual.AudioTag, Is.EqualTo (layout.AudioTag), "Tag");
			Assert.That (actual.ChannelUsage, Is.EqualTo (layout.ChannelUsage), "Channel usage");
			Assert.That (actual.Channels, Is.Empty, "Channels");

			settings.ChannelLayout = null;
			Assert.That (settings.ChannelLayout, Is.Null, "Cleared");
			Assert.That (settings.Dictionary.Count, Is.EqualTo ((nuint) 0), "Removed key");
		}

		NSObject [] Values = new NSObject []
		{
			NSNumber.FromFloat ((float)44100), //Sample Rate
			NSNumber.FromInt32 ((int)AudioFormatType.AppleLossless), //AVFormat
			NSNumber.FromInt32 (2),
			NSNumber.FromInt32 ((int)AVAudioQuality.Min)
		};

		NSObject [] Keys = new NSObject []
		{
			AVAudioSettings.AVSampleRateKey,
			AVAudioSettings.AVFormatIDKey,
			AVAudioSettings.AVNumberOfChannelsKey,
			AVAudioSettings.AVEncoderAudioQualityKey
		};
		[Test]
		public void Create ()
		{
			TestRuntime.RequestMicrophonePermission ();

			var url = NSUrl.FromFilename ("/dev/null");
			NSError error;
			var audioSettings = new AudioSettings (NSDictionary.FromObjectsAndKeys (Values, Keys));

			using (var recorder = AVAudioRecorder.Create (url, audioSettings, out error)) {
				Assert.That (recorder, Is.Not.Null);
				Assert.That (error, Is.Null);
			}
		}
		[Test]
		public void CreateWithError ()
		{
			TestRuntime.RequestMicrophonePermission ();

			var url = NSUrl.FromFilename ("/dev/fake.wav");
			NSError error;
			var audioSettings = new AudioSettings (NSDictionary.FromObjectsAndKeys (Values, Keys));
			using (var recorder = AVAudioRecorder.Create (url, audioSettings, out error)) {
				Assert.That (recorder, Is.Null);
				Assert.That (error, Is.Not.Null);
			}
		}

	}
}

#endif // !__TVOS__
