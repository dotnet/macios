//
// Unit tests for ExtAudioFile
//
// Authors:
//	Marek Safar (marek.safar@gmail.com)
//
// Copyright 2012 Xamarin Inc. All rights reserved.
//

using AudioUnit;

namespace MonoTouchFixtures.AudioUnit {

	[TestFixture]
	[Preserve (AllMembers = true)]
	public class ExtAudioFileTest {
		[Test]
		public void WrapAudioFileID ()
		{
			var path = NSBundle.MainBundle.PathForResource ("1", "caf", "AudioToolbox");
			using (var file = ExtAudioFile.OpenUrl (CFUrl.FromFile (path))) {
				Assert.That (file.AudioFile, Is.Not.Null, "#1");

				ExtAudioFile f2;
				Assert.That (ExtAudioFile.WrapAudioFileID (file.AudioFile.Value, true, out f2), Is.EqualTo (ExtAudioFileError.OK));
			}
		}

		[Test]
		public void ClientDataFormat ()
		{
			var path = NSBundle.MainBundle.PathForResource ("1", "caf", "AudioToolbox");
			using (var file = ExtAudioFile.OpenUrl (CFUrl.FromFile (path))) {
				var fmt = file.ClientDataFormat;
			}
		}

		[Test]
		public void OpenNSUrlTest ()
		{
			var path = NSBundle.MainBundle.PathForResource ("1", "caf", "AudioToolbox");
			ExtAudioFileError err;
			using (var file = ExtAudioFile.OpenUrl (NSUrl.FromFilename (path), out err)) {
				Assert.That (err == ExtAudioFileError.OK, Is.True, "OpenNSUrlTest");
				Assert.That (file.AudioFile, Is.Not.Null, "OpenNSUrlTest");
			}
		}

		[Test]
		public void OpenCFUrlTest ()
		{
			var path = NSBundle.MainBundle.PathForResource ("1", "caf", "AudioToolbox");
			ExtAudioFileError err;
			using (var file = ExtAudioFile.OpenUrl (CFUrl.FromFile (path), out err)) {
				Assert.That (err == ExtAudioFileError.OK, Is.True, "OpenCFUrlTest");
				Assert.That (file.AudioFile, Is.Not.Null, "OpenCFUrlTest");
			}
		}
	}
}
