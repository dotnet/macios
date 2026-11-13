//
// Unit tests for ExtAudioFile
//
// Authors:
//	Marek Safar (marek.safar@gmail.com)
//
// Copyright 2012 Xamarin Inc. All rights reserved.
//

using System.IO;

using AudioToolbox;
using AudioUnit;

namespace MonoTouchFixtures.AudioUnit {

	[TestFixture]
	[Preserve (AllMembers = true)]
	public class ExtAudioFileTest {
		[TestCase (0)]
		[TestCase (1)]
		[TestCase (2)]
		public void CreateFormats (int overload)
		{
			var path = Path.Combine (NSSearchPath.GetDirectories (NSSearchPathDirectory.CachesDirectory, NSSearchPathDomain.User, true) [0], $"ext-audio-file-{Guid.NewGuid ()}.caf");
			using var url = NSUrl.FromFilename (path);
			using var cfurl = CFUrl.FromFile (path);
			var formats = new [] { AudioStreamBasicDescription.CreateLinearPCM () };
			ref var format = ref formats [0];
			var expected = format;
			var error = ExtAudioFileError.OK;
			try {
				using (var file = overload switch {
					0 => ExtAudioFile.CreateWithUrl (url, AudioFileType.CAF, format, AudioFileFlags.EraseFile, out error),
					1 => ExtAudioFile.CreateWithUrl (cfurl, AudioFileType.CAF, format, AudioFileFlags.EraseFile, out error),
					2 => ExtAudioFile.CreateWithUrl (cfurl, AudioFileType.CAF, format, AudioFileFlags.EraseFile),
					_ => throw new ArgumentOutOfRangeException (nameof (overload)),
				}) {
					Assert.That (error, Is.EqualTo (ExtAudioFileError.OK), "Create status");
					Assert.That (file, Is.Not.Null, "File");
					Assert.That (file.FileDataFormat, Is.EqualTo (expected), "FileDataFormat");
					Assert.That (format, Is.EqualTo (expected), "Format unchanged");
				}
#if XAMCORE_5_0
				using (var file = overload switch {
					0 => ExtAudioFile.CreateWithUrl (url, AudioFileType.CAF, in format, AudioFileFlags.EraseFile, out error),
					1 => ExtAudioFile.CreateWithUrl (cfurl, AudioFileType.CAF, in format, AudioFileFlags.EraseFile, out error),
					2 => ExtAudioFile.CreateWithUrl (cfurl, AudioFileType.CAF, in format, AudioFileFlags.EraseFile),
					_ => throw new ArgumentOutOfRangeException (nameof (overload)),
				}) {
					Assert.That (error, Is.EqualTo (ExtAudioFileError.OK), "Create in status");
					Assert.That (file, Is.Not.Null, "File in");
					Assert.That (file.FileDataFormat, Is.EqualTo (expected), "FileDataFormat in");
					Assert.That (format, Is.EqualTo (expected), "Format in unchanged");
				}
#endif
				using var reopened = ExtAudioFile.OpenUrl (url, out error);
				Assert.That (error, Is.EqualTo (ExtAudioFileError.OK), "Reopen status");
				Assert.That (reopened, Is.Not.Null, "Reopened");
				Assert.That (reopened.FileDataFormat, Is.EqualTo (expected), "Reopened format");
			} finally {
				File.Delete (path);
			}
		}

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
