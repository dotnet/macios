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
			ClassicAssert.True (File.Exists (file), file);
			using (NSUrl url = new (file, false))
			using (AVAudioPlayer ap = AVAudioPlayer.FromUrl (url, out NSError error)) {
				ClassicAssert.NotNull (ap, "AVAudioPlayer");
				ClassicAssert.Null (error, "NSError");
			}
		}

		[Test]
		public void FromUrlWithInvalidUrl ()
		{
			Assert.DoesNotThrow (() => {
				using (AVAudioPlayer player = AVAudioPlayer.FromUrl (NSUrl.FromString ("sdf"), out NSError error)) {
					ClassicAssert.Null (player, "AVAudioPlayer");
					ClassicAssert.NotNull (error, "NSError");
				}
			});
		}

		[Test]
		public void FromUrlWithHint ()
		{
			var file = Path.Combine (NSBundle.MainBundle.ResourcePath, "Hand.wav");
			ClassicAssert.True (File.Exists (file), file);
			using var url = new NSUrl (file, false);
			{
				using var ap = AVAudioPlayer.FromUrl (url, AVFileTypes.Wave, out var error);
				ClassicAssert.NotNull (ap, "AVAudioPlayer");
				ClassicAssert.Null (error, "NSError");
			}
			{
				using var ap = AVAudioPlayer.FromUrl (url, AVFileTypes.Wave.GetConstant (), out var error);
				ClassicAssert.NotNull (ap, "AVAudioPlayer 2");
				ClassicAssert.Null (error, "NSError 2");
			}
		}

		[Test]
		public void FromInvalidUrlWithHint ()
		{
			using var url = new NSUrl ("sdf", false);
			{
				using var ap = AVAudioPlayer.FromUrl (url, AVFileTypes.Wave, out var error);
				ClassicAssert.Null (ap, "AVAudioPlayer");
				ClassicAssert.NotNull (error, "NSError");
			}
			{
				using var ap = AVAudioPlayer.FromUrl (url, AVFileTypes.Wave.GetConstant (), out var error);
				ClassicAssert.Null (ap, "AVAudioPlayer 2");
				ClassicAssert.NotNull (error, "NSError 2");
			}
		}

		[Test]
		public void FromData ()
		{
			using (NSData data = NSData.FromFile (NSBundle.MainBundle.PathForResource ("Hand", "wav")))
			using (AVAudioPlayer player = AVAudioPlayer.FromData (data, out NSError error)) {
				ClassicAssert.NotNull (player, "AVAudioPlayer");
				ClassicAssert.Null (error, "NSError");
			}
		}

		[Test]
		public void FromDataWithHint ()
		{
			using var data = NSData.FromFile (NSBundle.MainBundle.PathForResource ("Hand", "wav"));
			{
				using var player = AVAudioPlayer.FromData (data, AVFileTypes.Wave, out var error);
				ClassicAssert.NotNull (player, "AVAudioPlayer");
				ClassicAssert.Null (error, "NSError");
			}
			{
				using var player = AVAudioPlayer.FromData (data, AVFileTypes.Wave.GetConstant (), out var error);
				ClassicAssert.NotNull (player, "AVAudioPlayer 2");
				ClassicAssert.Null (error, "NSError 2");
			}
		}

		[Test]
		public void FromDataWithNullData ()
		{
			Assert.Throws<ArgumentNullException> (() => {
				using (var player = AVAudioPlayer.FromData (null, out NSError error)) {
					ClassicAssert.Null (player, "AVAudioPlayer");
					ClassicAssert.NotNull (error, "NSError");
				}
			});
		}
	}
}
