//
// Unit tests for AudioChannelLayout
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
	public class AudioChannelLayoutTest {
		[Test]
		public void Validate ()
		{
			var acl = AudioChannelLayout.FromAudioChannelLayoutTag (AudioChannelLayoutTag.AudioUnit_6_1);
			Assert.That (AudioChannelLayout.Validate (acl), Is.EqualTo (AudioFormatError.None));
		}

		[Test]
		public void FromAudioChannelBitmap ()
		{
			var bitmap = AudioChannelLayoutTag.AudioUnit_7_1_Front.ToAudioChannel ();
			Assert.That (AudioChannelLayout.FromAudioChannelBitmap (bitmap.Value), Is.Not.Null);
		}

		[Test]
		public void FromAudioChannelLayoutTag ()
		{
			Assert.That (AudioChannelLayout.FromAudioChannelLayoutTag (AudioChannelLayoutTag.AudioUnit_6_1), Is.Not.Null);
		}

		[Test]
		public void GetTagForChannelLayout ()
		{
			var acl = AudioChannelLayout.FromAudioChannelLayoutTag (AudioChannelLayoutTag.AudioUnit_6_1);
			Assert.That (AudioChannelLayout.GetTagForChannelLayout (acl), Is.EqualTo (AudioChannelLayoutTag.AudioUnit_6_1));
		}

		[Test]
		public void GetNumberOfChannels ()
		{
			var acl = AudioChannelLayout.FromAudioChannelLayoutTag (AudioChannelLayoutTag.MPEG_7_1_B);
			Assert.That (AudioChannelLayout.GetNumberOfChannels (acl), Is.EqualTo (8));
		}

		[Test]
		public void GetChannelMap ()
		{
			var acl1 = AudioChannelLayout.FromAudioChannelLayoutTag (AudioChannelLayoutTag.AudioUnit_6_1);
			var acl2 = AudioChannelLayout.FromAudioChannelLayoutTag (AudioChannelLayoutTag.MPEG_7_1_B);

			Assert.That (AudioChannelLayout.GetChannelMap (acl1, acl2), Is.Not.Null);
		}

		[Test]
		public void GetTagsForNumberOfChannels ()
		{
			Assert.That (AudioChannelLayout.GetTagsForNumberOfChannels (4), Is.Not.Null);
		}

		[Test]
		public void GetMatrixMixMap ()
		{
			var acl1 = AudioChannelLayout.FromAudioChannelLayoutTag (AudioChannelLayoutTag.AudioUnit_6_1);
			var acl2 = AudioChannelLayout.FromAudioChannelLayoutTag (AudioChannelLayoutTag.MPEG_7_1_B);

			Assert.That (AudioChannelLayout.GetMatrixMixMap (acl1, acl2), Is.Not.Null);
		}
	}
}
