//
// Unit tests for AudioFileGlobalInfo
//
// Authors:
//	Marek Safar (marek.safar@gmail.com)
//
// Copyright 2012 Xamarin Inc. All rights reserved.
//

#nullable enable

using System.Linq;

using AudioToolbox;

namespace MonoTouchFixtures.AudioToolbox {

	[TestFixture]
	[Preserve (AllMembers = true)]
	public class AudioFileGlobalInfoTest {
		[Test]
		public void Properties ()
		{
			Assert.Multiple (() => {
				Assert.That (AudioFileGlobalInfo.ReadableTypes, Is.Not.Null, "ReadableTypes");
				Assert.That (AudioFileGlobalInfo.ReadableTypes?.Length, Is.GreaterThan (0), "ReadableTypes #");

				Assert.That (AudioFileGlobalInfo.WritableTypes, Is.Not.Null, "WritableTypes");
				Assert.That (AudioFileGlobalInfo.WritableTypes?.Length, Is.GreaterThan (0), "WritableTypes #");

				var validFileTypeAndAudioFormatTypeCombinations = 0;
				var validAudioFileTypes = Enum.GetValues<AudioFileType> ().ToList ();
				validAudioFileTypes.Remove (AudioFileType.SoundDesigner2); // returns null in most APIs below
				validAudioFileTypes.Remove (AudioFileType.AAC_ADTS); // doesn't work on macOS 11
				validAudioFileTypes.Remove (AudioFileType.AMR); // doesn't work on macOS 11
				validAudioFileTypes.Remove (AudioFileType.MP1); // doesn't work on macOS 11
				validAudioFileTypes.Remove (AudioFileType.MP2); // doesn't work on macOS 11

				foreach (var fileType in validAudioFileTypes) {
					Assert.That (AudioFileGlobalInfo.GetFileTypeName (fileType), Is.Not.Null, $"GetFileTypeName: {fileType}");

					Assert.That (AudioFileGlobalInfo.GetAvailableFormats (fileType), Is.Not.Null, $"GetAvailableFormats: {fileType}");
					Assert.That (AudioFileGlobalInfo.GetAvailableFormats (fileType)?.Length ?? -1, Is.GreaterThan (0), $"GetAvailableFormats #: {fileType}");

					Assert.That (AudioFileGlobalInfo.GetExtensions (fileType), Is.Not.Null, $"GetExtensions: {fileType}");
					Assert.That (AudioFileGlobalInfo.GetExtensions (fileType)?.Length ?? -1, Is.GreaterThan (0), $"GetExtensions #: {fileType}");

					Assert.That (AudioFileGlobalInfo.GetMIMETypes (fileType), Is.Not.Null, $"GetMIMETypes: {fileType}");
					Assert.That (AudioFileGlobalInfo.GetMIMETypes (fileType)?.Length ?? -1, Is.GreaterThan (0), $"GetMIMETypes #: {fileType}");

					Assert.That (AudioFileGlobalInfo.GetUTIs (fileType), Is.Not.Null, $"GetUTIs: {fileType}");
					Assert.That (AudioFileGlobalInfo.GetUTIs (fileType)?.Length ?? -1, Is.GreaterThan (0), $"GetUTIs #: {fileType}");

					foreach (var audioFormatType in Enum.GetValues<AudioFormatType> ()) {
						var descs = AudioFileGlobalInfo.GetAvailableStreamDescriptions (fileType, audioFormatType);
						if (descs is not null) {
							validFileTypeAndAudioFormatTypeCombinations++;
							Assert.That (descs?.Length ?? -1, Is.GreaterThan (0), $"GetAvailableStreamDescriptions ({fileType}, {audioFormatType}) #");
						}
					}
				}
				Assert.That (validFileTypeAndAudioFormatTypeCombinations, Is.GreaterThan (50), "Valid FileType And AudioFormatType Combinations");

				Assert.That (AudioFileGlobalInfo.AllExtensions, Is.Not.Null, "AllExtensions");
				Assert.That (AudioFileGlobalInfo.AllExtensions.Length, Is.GreaterThan (0), $"AllExtensions #");
			});
		}
	}
}
