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
			using var track = composition.AddMutableTrack (constant, 0);
			using var output = new AVAssetReaderTrackOutput (track, (NSDictionary?) null);
			using var input = new AVAssetWriterInput (constant, (AudioSettings?) null, null);

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
		}
	}
}
