//
// Unit tests for AVAudioPlayer
//
// Authors:
//	Sebastien Pouliot <sebastien@xamarin.com>
//
// Copyright 2013 Xamarin Inc. All rights reserved.
//

using System.IO;
using AVFoundation;

#nullable enable

namespace MonoTouchFixtures.AVFoundation {

	[TestFixture]
	[Preserve (AllMembers = true)]
	public class AudioPlayerTest {

		[Test]
		public void FromUrl ()
		{
			string file = Path.Combine (NSBundle.MainBundle.ResourcePath, "Hand.wav");
			Assert.That (File.Exists (file), Is.True, file);
			using (NSUrl url = new (file, false))
			using (AVAudioPlayer ap = AVAudioPlayer.FromUrl (url, out NSError error)) {
				Assert.That (ap, Is.Not.Null, "AVAudioPlayer");
				Assert.That (error, Is.Null, "NSError");
			}
		}

		[Test]
		public void FromUrlWithInvalidUrl ()
		{
			Assert.DoesNotThrow (() => {
				using (AVAudioPlayer player = AVAudioPlayer.FromUrl (NSUrl.FromString ("sdf"), out NSError error)) {
					Assert.That (player, Is.Null, "AVAudioPlayer");
					Assert.That (error, Is.Not.Null, "NSError");
				}
			});
		}

		[Test]
		public void FromUrlWithHint ()
		{
			var file = Path.Combine (NSBundle.MainBundle.ResourcePath, "Hand.wav");
			Assert.That (File.Exists (file), Is.True, file);
			using var url = new NSUrl (file, false);
			{
				using var ap = AVAudioPlayer.FromUrl (url, AVFileTypes.Wave, out var error);
				Assert.That (ap, Is.Not.Null, "AVAudioPlayer");
				Assert.That (error, Is.Null, "NSError");
			}
			{
				using var ap = AVAudioPlayer.FromUrl (url, AVFileTypes.Wave.GetConstant (), out var error);
				Assert.That (ap, Is.Not.Null, "AVAudioPlayer 2");
				Assert.That (error, Is.Null, "NSError 2");
			}
		}

		[Test]
		public void FromInvalidUrlWithHint ()
		{
			using var url = new NSUrl ("sdf", false);
			{
				using var ap = AVAudioPlayer.FromUrl (url, AVFileTypes.Wave, out var error);
				Assert.That (ap, Is.Null, "AVAudioPlayer");
				Assert.That (error, Is.Not.Null, "NSError");
			}
			{
				using var ap = AVAudioPlayer.FromUrl (url, AVFileTypes.Wave.GetConstant (), out var error);
				Assert.That (ap, Is.Null, "AVAudioPlayer 2");
				Assert.That (error, Is.Not.Null, "NSError 2");
			}
		}

		[Test]
		public void FromData ()
		{
			using (NSData data = NSData.FromFile (NSBundle.MainBundle.PathForResource ("Hand", "wav")))
			using (AVAudioPlayer player = AVAudioPlayer.FromData (data, out NSError error)) {
				Assert.That (player, Is.Not.Null, "AVAudioPlayer");
				Assert.That (error, Is.Null, "NSError");
			}
		}

		[Test]
		public void FromDataWithHint ()
		{
			using var data = NSData.FromFile (NSBundle.MainBundle.PathForResource ("Hand", "wav"));
			{
				using var player = AVAudioPlayer.FromData (data, AVFileTypes.Wave, out var error);
				Assert.That (player, Is.Not.Null, "AVAudioPlayer");
				Assert.That (error, Is.Null, "NSError");
			}
			{
				using var player = AVAudioPlayer.FromData (data, AVFileTypes.Wave.GetConstant (), out var error);
				Assert.That (player, Is.Not.Null, "AVAudioPlayer 2");
				Assert.That (error, Is.Null, "NSError 2");
			}
		}

		[Test]
		public void FromDataWithNullData ()
		{
			Assert.Throws<ArgumentNullException> (() => {
				using (var player = AVAudioPlayer.FromData (null, out NSError error)) {
					Assert.That (player, Is.Null, "AVAudioPlayer");
					Assert.That (error, Is.Not.Null, "NSError");
				}
			});
		}
	}
}
