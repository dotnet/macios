// Copyright (c) Microsoft Corporation.
// Licensed under the MIT License.

#nullable enable

using System.IO;
using System.Threading.Tasks;

using AVFoundation;

namespace MonoTouchFixtures.AVFoundation {

	[TestFixture]
	[Preserve (AllMembers = true)]
	public class FileTypeTest {

		static string VideoPath => Path.Combine (NSBundle.MainBundle.ResourcePath, "xamvideotest.mp4");

		[Test]
		public void Writer ()
		{
			var path = Path.Combine (Path.GetTempPath (), $"{Guid.NewGuid ()}.mov");
			try {
				using var url = NSUrl.FromFilename (path);
				using var writer = AVAssetWriter.Create (url, AVFileTypes.QuickTimeMovie, out var error);
				using var writerError = error;
				Assert.That (error, Is.Null, "Creation error");
				if (writer is null)
					throw new AssertionException ("Could not create an asset writer.");

#if XAMCORE_5_0
				Assert.That (writer.OutputFileType, Is.EqualTo (AVFileTypes.QuickTimeMovie), "File type enum");
				Assert.That (writer.WeakOutputFileType, Is.EqualTo ("com.apple.quicktime-movie"), "File type string");
				var mediaTypes = writer.AvailableMediaTypes;
				var rawMediaTypes = writer.WeakAvailableMediaTypes;
#else
				Assert.That (writer.OutputFileTypes, Is.EqualTo (AVFileTypes.QuickTimeMovie), "File type enum");
				Assert.That (writer.OutputFileType, Is.EqualTo ("com.apple.quicktime-movie"), "File type string");
				var mediaTypes = writer.AvailableMediaTypeValues;
				var rawMediaTypes = writer.AvailableMediaTypes;
#endif
				Assert.That (mediaTypes, Is.Not.Empty, "Available media types");
				Assert.That (Array.ConvertAll (mediaTypes, type => (string?) type.GetConstant ()),
					Is.EqualTo (Array.ConvertAll (rawMediaTypes, type => type.ToString ())), "Media type constants");
				Assert.That (writer.CanApplyOutputSettings ((NSDictionary?) null, AVMediaTypes.Video),
					Is.EqualTo (writer.CanApplyOutputSettings ((NSDictionary?) null, "vide")), "Passthrough settings");
			} finally {
				File.Delete (path);
			}
		}

		[Test]
		public void ExportSession ()
		{
			using var url = NSUrl.FromFilename (VideoPath);
			using var asset = AVAsset.FromUrl (url);
			using var session = new AVAssetExportSession (asset, AVAssetExportSessionPreset.Passthrough);

#if XAMCORE_5_0
			var supportedTypes = session.SupportedFileTypes;
			var rawSupportedTypes = session.WeakSupportedFileTypes;
			Assert.That (session.OutputFileType, Is.Null, "Initially unset");
			session.OutputFileType = AVFileTypes.Mpeg4;
			Assert.That (session.WeakOutputFileType, Is.EqualTo ("public.mpeg-4"), "Typed setter");
			session.WeakOutputFileType = "com.apple.quicktime-movie";
			Assert.That (session.OutputFileType, Is.EqualTo (AVFileTypes.QuickTimeMovie), "Typed getter");
			session.OutputFileType = null;
			Assert.That (session.WeakOutputFileType, Is.Null, "Cleared file type");
#else
			var supportedTypes = session.SupportedFileTypeValues;
			var rawSupportedTypes = session.SupportedFileTypes;
			Assert.That (session.OutputFileTypes, Is.Null, "Initially unset");
			session.OutputFileTypes = AVFileTypes.Mpeg4;
			Assert.That (session.OutputFileType, Is.EqualTo ("public.mpeg-4"), "Typed setter");
			session.OutputFileType = "com.apple.quicktime-movie";
			Assert.That (session.OutputFileTypes, Is.EqualTo (AVFileTypes.QuickTimeMovie), "Typed getter");
			session.OutputFileTypes = null;
			Assert.That (session.OutputFileType, Is.Null, "Cleared file type");
#endif
			Assert.That (supportedTypes, Is.Not.Empty, "Supported file types");
			Assert.That (Array.ConvertAll (supportedTypes, type => (string?) type.GetConstant ()),
				Is.EqualTo (rawSupportedTypes), "File type constants");
		}

		[Test]
		public async Task CompatibleFileTypes ()
		{
			using var url = NSUrl.FromFilename (VideoPath);
			using var asset = AVAsset.FromUrl (url);
			using var session = new AVAssetExportSession (asset, AVAssetExportSessionPreset.Passthrough);

#if XAMCORE_5_0
			var types = await session.DetermineCompatibleFileTypesAsync ().WaitAsync (TimeSpan.FromSeconds (10));
			var rawTypes = await session.WeakDetermineCompatibleFileTypesAsync ().WaitAsync (TimeSpan.FromSeconds (10));
#else
			var types = await session.DetermineCompatibleFileTypeValuesAsync ().WaitAsync (TimeSpan.FromSeconds (10));
			var rawTypes = await session.DetermineCompatibleFileTypesAsync ().WaitAsync (TimeSpan.FromSeconds (10));
#endif
			Assert.That (types, Is.Not.Empty, "Compatible file types");
			Assert.That (Array.ConvertAll (types, type => (string?) type.GetConstant ()),
				Is.EquivalentTo (rawTypes), "Compatible file type constants");
		}

#if !__TVOS__
		[Test]
		public void Movie ()
		{
			TestRuntime.AssertXcodeVersion (11, 0);

			using var url = NSUrl.FromFilename (VideoPath);
			using var movie = new AVMovie (url, (NSDictionary<NSString, NSObject>?) null);
			Assert.That (movie.IsCompatibleWithFileType (AVFileTypes.QuickTimeMovie),
				Is.EqualTo (movie.IsCompatibleWithFileType ("com.apple.quicktime-movie")), "Compatibility");
			using var header = movie.GetMovieHeader (AVFileTypes.QuickTimeMovie, out var error);
			using var headerError = error;
			Assert.That (error, Is.Null, "Header error");
			Assert.That (header, Is.Not.Null, "Header");

#if XAMCORE_5_0
			var types = AVMovie.MovieTypes;
			var rawTypes = AVMovie.WeakMovieTypes;
#else
			var types = AVMovie.MovieTypeValues;
			var rawTypes = AVMovie.MovieTypes;
#endif
			Assert.That (types, Is.Not.Empty, "Movie file types");
			Assert.That (Array.ConvertAll (types, type => (string?) type.GetConstant ()),
				Is.EqualTo (rawTypes), "Movie file type constants");
		}
#endif

#if !__MACOS__
		[Test]
		public void PhotoSettings ()
		{
			TestRuntime.AssertXcodeVersion (15, 0);

			using var format = new NSMutableDictionary<NSString, NSObject> ();
			format [AVVideo.CodecKey] = AVVideoCodecType.Jpeg.GetConstant () ?? throw new InvalidOperationException ("JPEG codec constant is unavailable.");
			using var settings = AVCapturePhotoSettings.Create (0, null, format, AVFileTypes.Jpeg);

#if XAMCORE_5_0
			Assert.That (settings.ProcessedFileType, Is.EqualTo (AVFileTypes.Jpeg), "Processed file type");
			Assert.That (settings.WeakProcessedFileType, Is.EqualTo ("public.jpeg"), "Processed file type string");
			Assert.That (settings.RawFileType, Is.Null, "No raw file type");
#else
			Assert.That (settings.ProcessedFileTypes, Is.EqualTo (AVFileTypes.Jpeg), "Processed file type");
			Assert.That (settings.ProcessedFileType, Is.EqualTo ("public.jpeg"), "Processed file type string");
			Assert.That (settings.RawFileTypes, Is.Null, "No raw file type");
#endif
		}
#endif
	}
}
