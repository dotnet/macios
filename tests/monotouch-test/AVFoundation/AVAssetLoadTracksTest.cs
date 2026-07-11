// Copyright (c) Microsoft Corporation.
// Licensed under the MIT License.

using System.IO;
using System.Threading.Tasks;

using AVFoundation;
using CoreMedia;

#nullable enable

namespace MonoTouchFixtures.AVFoundation {

	[TestFixture]
	[Preserve (AllMembers = true)]
	public class AVAssetLoadTracksTest {

		static string VideoPath => Path.Combine (NSBundle.MainBundle.ResourcePath, "xamvideotest.mp4");

		#region AVAsset

		[Test]
		public void AVAsset_LoadTrack2 ()
		{
			TestRuntime.AssertXcodeVersion (13, 0);

			using var url = NSUrl.FromFilename (VideoPath);
			using var asset = AVAsset.FromUrl (url);

			var tcs = new TaskCompletionSource<AVAssetTrack?> ();
			asset.LoadTrack2 (1, (track, error) => {
				tcs.TrySetResult (track);
			});

			Assert.That (tcs.Task.Wait (TimeSpan.FromSeconds (10)), Is.True, "Timed out");
			var result = tcs.Task.Result;
			if (result is not null)
				Assert.That (result, Is.InstanceOf<AVAssetTrack> (), "result type");
		}

		[Test]
		public void AVAsset_LoadTrackWithMediaCharacteristics ()
		{
			TestRuntime.AssertXcodeVersion (13, 0);

			using var url = NSUrl.FromFilename (VideoPath);
			using var asset = AVAsset.FromUrl (url);

			var tcs = new TaskCompletionSource<AVAssetTrack []?> ();
			asset.LoadTrackWithMediaCharacteristics (AVMediaCharacteristics.Visual.GetConstant ()!, (tracks, error) => {
				tcs.TrySetResult (tracks);
			});

			Assert.That (tcs.Task.Wait (TimeSpan.FromSeconds (10)), Is.True, "Timed out");
			var result = tcs.Task.Result;
			Assert.That (result, Is.Not.Null, "result");
			Assert.That (result!.Length, Is.GreaterThan (0), "result.Length");
			Assert.That (result [0], Is.InstanceOf<AVAssetTrack> (), "result[0] type");
		}

		[Test]
		public void AVAsset_LoadTracksWithMediaType ()
		{
			TestRuntime.AssertXcodeVersion (13, 0);

			using var url = NSUrl.FromFilename (VideoPath);
			using var asset = AVAsset.FromUrl (url);

			var tcs = new TaskCompletionSource<AVAssetTrack []?> ();
			asset.LoadTracksWithMediaType (AVMediaTypes.Video.GetConstant ()!, (AVAssetLoadTracksCallback) ((tracks, error) => {
				tcs.TrySetResult (tracks);
			}));

			Assert.That (tcs.Task.Wait (TimeSpan.FromSeconds (10)), Is.True, "Timed out");
			var result = tcs.Task.Result;
			Assert.That (result, Is.Not.Null, "result");
			Assert.That (result!.Length, Is.GreaterThan (0), "result.Length");
			Assert.That (result [0], Is.InstanceOf<AVAssetTrack> (), "result[0] type");
		}

		[Test]
		public void AVAsset_LoadTracksWithMediaType_Enum ()
		{
			TestRuntime.AssertXcodeVersion (13, 0);

			using var url = NSUrl.FromFilename (VideoPath);
			using var asset = AVAsset.FromUrl (url);

			var tcs = new TaskCompletionSource<AVAssetTrack []?> ();
			asset.LoadTracksWithMediaType (AVMediaTypes.Video, (AVAssetLoadTracksCallback) ((tracks, error) => {
				tcs.TrySetResult (tracks);
			}));

			Assert.That (tcs.Task.Wait (TimeSpan.FromSeconds (10)), Is.True, "Timed out");
			var result = tcs.Task.Result;
			Assert.That (result, Is.Not.Null, "result");
			Assert.That (result!.Length, Is.GreaterThan (0), "result.Length");
			Assert.That (result [0], Is.InstanceOf<AVAssetTrack> (), "result[0] type");
		}

		#endregion

#if !__TVOS__
		#region AVMovie

		[Test]
		public void AVMovie_LoadTrack ()
		{
			TestRuntime.AssertXcodeVersion (13, 0);

			using var url = NSUrl.FromFilename (VideoPath);
			using var movie = new AVMovie (url, (NSDictionary<NSString, NSObject>?) null);

			var tcs = new TaskCompletionSource<AVMovieTrack?> ();
			movie.LoadTrack (1, (AVMovieLoadTrackCallback) ((track, error) => {
				tcs.TrySetResult (track);
			}));

			Assert.That (tcs.Task.Wait (TimeSpan.FromSeconds (10)), Is.True, "Timed out");
			var result = tcs.Task.Result;
			if (result is not null)
				Assert.That (result, Is.InstanceOf<AVMovieTrack> (), "result type");
		}

		[Test]
		public void AVMovie_LoadTracksWithMediaType ()
		{
			TestRuntime.AssertXcodeVersion (13, 0);

			using var url = NSUrl.FromFilename (VideoPath);
			using var movie = new AVMovie (url, (NSDictionary<NSString, NSObject>?) null);

			var tcs = new TaskCompletionSource<AVMovieTrack []?> ();
			movie.LoadTracksWithMediaType (AVMediaTypes.Video.GetConstant ()!, (AVMovieLoadTracksCallback) ((tracks, error) => {
				tcs.TrySetResult (tracks);
			}));

			Assert.That (tcs.Task.Wait (TimeSpan.FromSeconds (10)), Is.True, "Timed out");
			var result = tcs.Task.Result;
			// mp4 files may not be recognized as movies, so tracks could be null
			if (result is not null) {
				Assert.That (result.Length, Is.GreaterThan (0), "result.Length");
				Assert.That (result [0], Is.InstanceOf<AVMovieTrack> (), "result[0] type");
			}
		}

		[Test]
		public void AVMovie_LoadTracksWithMediaType_Enum ()
		{
			TestRuntime.AssertXcodeVersion (13, 0);

			using var url = NSUrl.FromFilename (VideoPath);
			using var movie = new AVMovie (url, (NSDictionary<NSString, NSObject>?) null);

			var tcs = new TaskCompletionSource<AVMovieTrack []?> ();
			movie.LoadTracksWithMediaType (AVMediaTypes.Video, (AVMovieLoadTracksCallback) ((tracks, error) => {
				tcs.TrySetResult (tracks);
			}));

			Assert.That (tcs.Task.Wait (TimeSpan.FromSeconds (10)), Is.True, "Timed out");
			var result = tcs.Task.Result;
			if (result is not null) {
				Assert.That (result.Length, Is.GreaterThan (0), "result.Length");
				Assert.That (result [0], Is.InstanceOf<AVMovieTrack> (), "result[0] type");
			}
		}

		[Test]
		public void AVMovie_LoadTracksWithMediaCharacteristic ()
		{
			TestRuntime.AssertXcodeVersion (13, 0);

			using var url = NSUrl.FromFilename (VideoPath);
			using var movie = new AVMovie (url, (NSDictionary<NSString, NSObject>?) null);

			var tcs = new TaskCompletionSource<AVMovieTrack []?> ();
			movie.LoadTracksWithMediaCharacteristic (AVMediaCharacteristics.Visual.GetConstant ()!, (AVMovieLoadTracksCallback) ((tracks, error) => {
				tcs.TrySetResult (tracks);
			}));

			Assert.That (tcs.Task.Wait (TimeSpan.FromSeconds (10)), Is.True, "Timed out");
			var result = tcs.Task.Result;
			if (result is not null) {
				Assert.That (result.Length, Is.GreaterThan (0), "result.Length");
				Assert.That (result [0], Is.InstanceOf<AVMovieTrack> (), "result[0] type");
			}
		}

		#endregion

		#region AVMutableMovie

		[Test]
		public void AVMutableMovie_LoadTrack2 ()
		{
			TestRuntime.AssertXcodeVersion (13, 0);

			using var url = NSUrl.FromFilename (VideoPath);
			using var movie = new AVMutableMovie (url, (NSDictionary<NSString, NSObject>?) null, out var initError);
			if (initError is not null)
				Assert.Ignore ($"Could not create AVMutableMovie from test file: {initError.LocalizedDescription}");

			var tcs = new TaskCompletionSource<AVMutableMovieTrack?> ();
			movie.LoadTrack2 (1, (track, error) => {
				tcs.TrySetResult (track);
			});

			Assert.That (tcs.Task.Wait (TimeSpan.FromSeconds (10)), Is.True, "Timed out");
			var result = tcs.Task.Result;
			if (result is not null)
				Assert.That (result, Is.InstanceOf<AVMutableMovieTrack> (), "result type");
		}

		[Test]
		public void AVMutableMovie_LoadTracksWithMediaType ()
		{
			TestRuntime.AssertXcodeVersion (13, 0);

			using var url = NSUrl.FromFilename (VideoPath);
			using var movie = new AVMutableMovie (url, (NSDictionary<NSString, NSObject>?) null, out var initError);
			if (initError is not null)
				Assert.Ignore ($"Could not create AVMutableMovie from test file: {initError.LocalizedDescription}");

			var tcs = new TaskCompletionSource<AVMutableMovieTrack []?> ();
			movie.LoadTracksWithMediaType (AVMediaTypes.Video.GetConstant ()!, (AVMutableMovieLoadTracksCallback) ((tracks, error) => {
				tcs.TrySetResult (tracks);
			}));

			Assert.That (tcs.Task.Wait (TimeSpan.FromSeconds (10)), Is.True, "Timed out");
			var result = tcs.Task.Result;
			if (result is not null) {
				Assert.That (result.Length, Is.GreaterThan (0), "result.Length");
				Assert.That (result [0], Is.InstanceOf<AVMutableMovieTrack> (), "result[0] type");
			}
		}

		[Test]
		public void AVMutableMovie_LoadTracksWithMediaType_Enum ()
		{
			TestRuntime.AssertXcodeVersion (13, 0);

			using var url = NSUrl.FromFilename (VideoPath);
			using var movie = new AVMutableMovie (url, (NSDictionary<NSString, NSObject>?) null, out var initError);
			if (initError is not null)
				Assert.Ignore ($"Could not create AVMutableMovie from test file: {initError.LocalizedDescription}");

			var tcs = new TaskCompletionSource<AVMutableMovieTrack []?> ();
			movie.LoadTracksWithMediaType (AVMediaTypes.Video, (AVMutableMovieLoadTracksCallback) ((tracks, error) => {
				tcs.TrySetResult (tracks);
			}));

			Assert.That (tcs.Task.Wait (TimeSpan.FromSeconds (10)), Is.True, "Timed out");
			var result = tcs.Task.Result;
			if (result is not null) {
				Assert.That (result.Length, Is.GreaterThan (0), "result.Length");
				Assert.That (result [0], Is.InstanceOf<AVMutableMovieTrack> (), "result[0] type");
			}
		}

		[Test]
		public void AVMutableMovie_LoadTracksWithMediaCharacteristic ()
		{
			TestRuntime.AssertXcodeVersion (13, 0);

			using var url = NSUrl.FromFilename (VideoPath);
			using var movie = new AVMutableMovie (url, (NSDictionary<NSString, NSObject>?) null, out var initError);
			if (initError is not null)
				Assert.Ignore ($"Could not create AVMutableMovie from test file: {initError.LocalizedDescription}");

			var tcs = new TaskCompletionSource<AVMutableMovieTrack []?> ();
			movie.LoadTracksWithMediaCharacteristic (AVMediaCharacteristics.Visual.GetConstant ()!, (AVMutableMovieLoadTracksCallback) ((tracks, error) => {
				tcs.TrySetResult (tracks);
			}));

			Assert.That (tcs.Task.Wait (TimeSpan.FromSeconds (10)), Is.True, "Timed out");
			var result = tcs.Task.Result;
			if (result is not null) {
				Assert.That (result.Length, Is.GreaterThan (0), "result.Length");
				Assert.That (result [0], Is.InstanceOf<AVMutableMovieTrack> (), "result[0] type");
			}
		}

		#endregion
#endif // !__TVOS__

		#region AVComposition

		[Test]
		public void AVComposition_LoadTrack ()
		{
			TestRuntime.AssertXcodeVersion (13, 0);

			using var url = NSUrl.FromFilename (VideoPath);
			using var asset = AVAsset.FromUrl (url);
			using var composition = AVMutableComposition.Create ();

			var compositionTrack = composition.AddMutableTrack (AVMediaTypes.Video.GetConstant ()!, 0);
			if (compositionTrack is null)
				Assert.Ignore ("Could not add track to composition");

			var sourceTracks = asset.GetTracks (AVMediaTypes.Video);
			if (sourceTracks.Length == 0)
				Assert.Ignore ("No video tracks in source asset");

			compositionTrack.InsertTimeRange (new CMTimeRange { Start = CMTime.Zero, Duration = asset.Duration }, sourceTracks [0], CMTime.Zero, out _);

			var trackId = compositionTrack.TrackID;
			var tcs = new TaskCompletionSource<AVCompositionTrack?> ();
			composition.LoadTrack (trackId, (AVCompositionLoadTrackCallback) ((track, error) => {
				tcs.TrySetResult (track);
			}));

			Assert.That (tcs.Task.Wait (TimeSpan.FromSeconds (10)), Is.True, "Timed out");
			var result = tcs.Task.Result;
			Assert.That (result, Is.Not.Null, "result");
			Assert.That (result, Is.InstanceOf<AVCompositionTrack> (), "result type");
		}

		[Test]
		public void AVComposition_LoadTracksWithMediaType ()
		{
			TestRuntime.AssertXcodeVersion (13, 0);

			using var url = NSUrl.FromFilename (VideoPath);
			using var asset = AVAsset.FromUrl (url);
			using var composition = AVMutableComposition.Create ();

			var compositionTrack = composition.AddMutableTrack (AVMediaTypes.Video.GetConstant ()!, 0);
			if (compositionTrack is null)
				Assert.Ignore ("Could not add track to composition");

			var sourceTracks = asset.GetTracks (AVMediaTypes.Video);
			if (sourceTracks.Length == 0)
				Assert.Ignore ("No video tracks in source asset");

			compositionTrack.InsertTimeRange (new CMTimeRange { Start = CMTime.Zero, Duration = asset.Duration }, sourceTracks [0], CMTime.Zero, out _);

			var tcs = new TaskCompletionSource<AVCompositionTrack []?> ();
			composition.LoadTracksWithMediaType (AVMediaTypes.Video.GetConstant ()!, (AVCompositionLoadTracksCallback) ((tracks, error) => {
				tcs.TrySetResult (tracks);
			}));

			Assert.That (tcs.Task.Wait (TimeSpan.FromSeconds (10)), Is.True, "Timed out");
			var result = tcs.Task.Result;
			Assert.That (result, Is.Not.Null, "result");
			Assert.That (result!.Length, Is.GreaterThan (0), "result.Length");
			Assert.That (result [0], Is.InstanceOf<AVCompositionTrack> (), "result[0] type");
		}

		[Test]
		public void AVComposition_LoadTracksWithMediaType_Enum ()
		{
			TestRuntime.AssertXcodeVersion (13, 0);

			using var url = NSUrl.FromFilename (VideoPath);
			using var asset = AVAsset.FromUrl (url);
			using var composition = AVMutableComposition.Create ();

			var compositionTrack = composition.AddMutableTrack (AVMediaTypes.Video.GetConstant ()!, 0);
			if (compositionTrack is null)
				Assert.Ignore ("Could not add track to composition");

			var sourceTracks = asset.GetTracks (AVMediaTypes.Video);
			if (sourceTracks.Length == 0)
				Assert.Ignore ("No video tracks in source asset");

			compositionTrack.InsertTimeRange (new CMTimeRange { Start = CMTime.Zero, Duration = asset.Duration }, sourceTracks [0], CMTime.Zero, out _);

			var tcs = new TaskCompletionSource<AVCompositionTrack []?> ();
			composition.LoadTracksWithMediaType (AVMediaTypes.Video, (AVCompositionLoadTracksCallback) ((tracks, error) => {
				tcs.TrySetResult (tracks);
			}));

			Assert.That (tcs.Task.Wait (TimeSpan.FromSeconds (10)), Is.True, "Timed out");
			var result = tcs.Task.Result;
			Assert.That (result, Is.Not.Null, "result");
			Assert.That (result!.Length, Is.GreaterThan (0), "result.Length");
			Assert.That (result [0], Is.InstanceOf<AVCompositionTrack> (), "result[0] type");
		}

		[Test]
		public void AVComposition_LoadTracksWithMediaCharacteristic ()
		{
			TestRuntime.AssertXcodeVersion (13, 0);

			using var url = NSUrl.FromFilename (VideoPath);
			using var asset = AVAsset.FromUrl (url);
			using var composition = AVMutableComposition.Create ();

			var compositionTrack = composition.AddMutableTrack (AVMediaTypes.Video.GetConstant ()!, 0);
			if (compositionTrack is null)
				Assert.Ignore ("Could not add track to composition");

			var sourceTracks = asset.GetTracks (AVMediaTypes.Video);
			if (sourceTracks.Length == 0)
				Assert.Ignore ("No video tracks in source asset");

			compositionTrack.InsertTimeRange (new CMTimeRange { Start = CMTime.Zero, Duration = asset.Duration }, sourceTracks [0], CMTime.Zero, out _);

			var tcs = new TaskCompletionSource<AVCompositionTrack []?> ();
			composition.LoadTracksWithMediaCharacteristic (AVMediaCharacteristics.Visual.GetConstant ()!, (AVCompositionLoadTracksCallback) ((tracks, error) => {
				tcs.TrySetResult (tracks);
			}));

			Assert.That (tcs.Task.Wait (TimeSpan.FromSeconds (10)), Is.True, "Timed out");
			var result = tcs.Task.Result;
			Assert.That (result, Is.Not.Null, "result");
			Assert.That (result!.Length, Is.GreaterThan (0), "result.Length");
			Assert.That (result [0], Is.InstanceOf<AVCompositionTrack> (), "result[0] type");
		}

		#endregion
	}
}
