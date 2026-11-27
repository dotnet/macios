// Copyright (c) Microsoft Corporation.
// Licensed under the MIT License.

#nullable enable

using AVFoundation;

namespace MonoTouchFixtures.AVFoundation {

	[TestFixture]
	[Preserve (AllMembers = true)]
	public class MediaTypeTest {

		[TestCase (AVMediaTypes.Video, "vide")]
		[TestCase (AVMediaTypes.Audio, "soun")]
		public void MediaTypes (AVMediaTypes mediaType, string constant)
		{
			using var composition = new AVMutableComposition ();
			using var track = composition.AddMutableTrack (mediaType, 0) ?? throw new AssertionException ("Could not create a composition track.");
			using var output = new AVAssetReaderTrackOutput (track, (NSDictionary?) null);
			using var input = new AVAssetWriterInput (constant, (AudioSettings?) null, null);
			using var audioInput = new AVAssetWriterInput (mediaType, (AudioSettings?) null);
			using var videoInput = new AVAssetWriterInput (mediaType, (AVVideoSettingsCompressed?) null);
			using var audioInputWithHint = new AVAssetWriterInput (mediaType, (AudioSettings?) null, null);
			using var videoInputWithHint = new AVAssetWriterInput (mediaType, (AVVideoSettingsCompressed?) null, null);
			using var audioFactoryInput = AVAssetWriterInput.Create (mediaType, (AudioSettings?) null);
			using var videoFactoryInput = AVAssetWriterInput.Create (mediaType, (AVVideoSettingsCompressed?) null);
			using var audioFactoryInputWithHint = AVAssetWriterInput.Create (mediaType, (AudioSettings?) null, null);
			using var videoFactoryInputWithHint = AVAssetWriterInput.Create (mediaType, (AVVideoSettingsCompressed?) null, null);

#if XAMCORE_5_0
			Assert.That (track.MediaType, Is.EqualTo (mediaType), "Track enum");
			Assert.That (output.MediaType, Is.EqualTo (mediaType), "Reader output enum");
			Assert.That (input.MediaType, Is.EqualTo (mediaType), "Writer input enum");
			Assert.That (track.WeakMediaType, Is.EqualTo (constant), "Track string");
			Assert.That (output.WeakMediaType, Is.EqualTo (constant), "Reader output string");
			Assert.That (input.WeakMediaType, Is.EqualTo (constant), "Writer input string");
#else
			Assert.That (track.MediaTypes, Is.EqualTo (mediaType), "Track enum");
			Assert.That (output.MediaTypes, Is.EqualTo (mediaType), "Reader output enum");
			Assert.That (input.MediaTypes, Is.EqualTo (mediaType), "Writer input enum");
			Assert.That (track.MediaType, Is.EqualTo (constant), "Track string");
			Assert.That (output.MediaType, Is.EqualTo (constant), "Reader output string");
			Assert.That (input.MediaType, Is.EqualTo (constant), "Writer input string");
#endif
			foreach (var writerInput in new [] { audioInput, videoInput, audioInputWithHint, videoInputWithHint, audioFactoryInput, videoFactoryInput, audioFactoryInputWithHint, videoFactoryInputWithHint }) {
#if XAMCORE_5_0
				Assert.That (writerInput.MediaType, Is.EqualTo (mediaType), "Typed writer input enum");
				Assert.That (writerInput.WeakMediaType, Is.EqualTo (constant), "Typed writer input string");
#else
				Assert.That (writerInput.MediaTypes, Is.EqualTo (mediaType), "Typed writer input enum");
				Assert.That (writerInput.MediaType, Is.EqualTo (constant), "Typed writer input string");
#endif
			}
		}

		[Test]
		public void CaptureConnection ()
		{
			TestRuntime.AssertXcodeVersion (15, 0);

			using var output = new AVCaptureVideoDataOutput ();
			Assert.That (output.ConnectionFromMediaType (AVMediaTypes.Video), Is.Null, "No connection before adding to a session");
		}
	}
}
