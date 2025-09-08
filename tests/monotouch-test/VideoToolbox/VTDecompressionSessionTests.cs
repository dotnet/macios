//
// Unit tests for VTDecompressionSession
//
// Authors:
//	Alex Soto <alex.soto@xamarin.com>
//	
//
// Copyright 2015 Xamarin Inc. All rights reserved.
//

using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

using Foundation;
using VideoToolbox;
using CoreMedia;
using CoreVideo;
using AVFoundation;
using CoreFoundation;
using ObjCRuntime;
using NUnit.Framework;
using Xamarin.Utils;

namespace MonoTouchFixtures.VideoToolbox {

	[TestFixture]
	[Preserve (AllMembers = true)]
	public class VTDecompressionSessionTests {
		[Test]
		public void DecompressionSessionPropertiesTest ()
		{
			TestRuntime.AssertXcodeVersion (15, 0);

			Assert.That (VTDecompressionSession.IsStereoMvHevcDecodeSupported (), Is.EqualTo (false).Or.EqualTo (true), "IsStereoMvHevcDecodeSupported");
		}

		[Test]
		public void DecompressionSessionCreateTest ()
		{
			TestRuntime.AssertSystemVersion (ApplePlatform.iOS, 8, 0, throwIfOtherPlatform: false);
			TestRuntime.AssertSystemVersion (ApplePlatform.MacOSX, 10, 8, throwIfOtherPlatform: false);
			TestRuntime.AssertSystemVersion (ApplePlatform.TVOS, 10, 2, throwIfOtherPlatform: false);

			using (var asset = AVAsset.FromUrl (NSBundle.MainBundle.GetUrlForResource ("xamvideotest", "mp4")))
			using (var session = CreateSession (asset)) {
				Assert.IsNotNull (session, "Session should not be null");
			}
		}

		[Test]
		public void DecompressionSessionSetDecompressionPropertiesTest ()
		{
			TestRuntime.AssertSystemVersion (ApplePlatform.iOS, 8, 0, throwIfOtherPlatform: false);
			TestRuntime.AssertSystemVersion (ApplePlatform.MacOSX, 10, 10, throwIfOtherPlatform: false);
			TestRuntime.AssertSystemVersion (ApplePlatform.TVOS, 10, 2, throwIfOtherPlatform: false);

			using (var asset = AVAsset.FromUrl (NSBundle.MainBundle.GetUrlForResource ("xamvideotest", "mp4")))
			using (var session = CreateSession (asset)) {

				var result = session.SetDecompressionProperties (new VTDecompressionProperties {
					RealTime = true,
					OnlyTheseFrames = VTOnlyTheseFrames.AllFrames
				});

				Assert.AreEqual (VTStatus.Ok, result, "SetDecompressionProperties");
			}
		}

		[Test]
		public void DecompressionSessionSetPropertiesTest ()
		{
			TestRuntime.AssertSystemVersion (ApplePlatform.iOS, 8, 0, throwIfOtherPlatform: false);
			TestRuntime.AssertSystemVersion (ApplePlatform.MacOSX, 10, 8, throwIfOtherPlatform: false);
			TestRuntime.AssertSystemVersion (ApplePlatform.TVOS, 10, 2, throwIfOtherPlatform: false);

			using (var asset = AVAsset.FromUrl (NSBundle.MainBundle.GetUrlForResource ("xamvideotest", "mp4")))
			using (var session = CreateSession (asset)) {

				var result = session.SetProperties (new VTPropertyOptions {
					ReadWriteStatus = VTReadWriteStatus.ReadWrite,
					ShouldBeSerialized = true
				});

				Assert.AreEqual (VTStatus.Ok, result, "SetProperties");
			}
		}

		[Test]
		public void DecompressionSessionGetSupportedPropertiesTest ()
		{
			TestRuntime.AssertSystemVersion (ApplePlatform.iOS, 8, 0, throwIfOtherPlatform: false);
			TestRuntime.AssertSystemVersion (ApplePlatform.MacOSX, 10, 8, throwIfOtherPlatform: false);
			TestRuntime.AssertSystemVersion (ApplePlatform.TVOS, 10, 2, throwIfOtherPlatform: false);

			using (var asset = AVAsset.FromUrl (NSBundle.MainBundle.GetUrlForResource ("xamvideotest", "mp4")))
			using (var session = CreateSession (asset)) {
				var supportedProps = session.GetSupportedProperties ();
				Assert.NotNull (supportedProps, "GetSupportedProperties");
				Assert.That (supportedProps.Count, Is.GreaterThan ((nuint) 0), "GetSupportedProperties should be more than zero");
			}
		}
		VTDecompressionSession CreateSession (AVAsset asset)
		{
			var videoTracks = asset.TracksWithMediaType (AVMediaTypes.Video.GetConstant ());
			var track = videoTracks [0];
			var formatDescriptor = track.FormatDescriptions [0] as CMVideoFormatDescription;
			return CreateSession (formatDescriptor);
		}

		VTDecompressionSession CreateSession (CMVideoFormatDescription formatDescriptor)
		{
			return CreateSession (formatDescriptor, (sourceFrame, status, flags, buffer, presentationTimeStamp, presentationDuration) => { });
		}

		VTDecompressionSession CreateSession (CMVideoFormatDescription formatDescriptor, VTDecompressionSession.VTDecompressionOutputCallback? callback)
		{
			return VTDecompressionSession.Create (callback, formatDescriptor);
		}

		class SampleBufferEnumerator {
			public CMVideoFormatDescription FormatDescription;
			AVAssetTrack? videoTrack;
			AVAsset? asset;

			public SampleBufferEnumerator (NSUrl url, AVMediaCharacteristics characteristic = AVMediaCharacteristics.Visual)
			{
				asset = AVAsset.FromUrl (url);
				Assert.That (asset, Is.Not.Null, "Asset");

				var loaded = new TaskCompletionSource<CMVideoFormatDescription> ();

				asset.LoadTrackWithMediaCharacteristics (characteristic.GetConstant (), (tracks, error) => {
					try {
						Assert.Null (error, "Failed to load track");

						videoTrack = (AVAssetTrack) tracks.ToArray ().First ();

						var format = (CMVideoFormatDescription) videoTrack.FormatDescriptions [0];
						loaded.SetResult (format);
					} catch (Exception e) {
						loaded.SetException (e);
					}
				});

				Assert.IsTrue (loaded.Task.Wait (TimeSpan.FromSeconds (15)), "Timed out waiting for track to load");
				FormatDescription = loaded.Task.Result;
			}

			public void Enumerate (Action<CMSampleBuffer> iterator)
			{
				using var cursor = videoTrack.MakeSampleCursorAtFirstSampleInDecodeOrder ();
				using var sampleBufferGenerator = new AVSampleBufferGenerator (asset, null);
				using var request = new AVSampleBufferRequest (cursor);
				var sampleCount = 0L;

				do {
					using var buffer = sampleBufferGenerator.CreateSampleBuffer (request, out var sampleBufferError);
					Assert.NotNull (buffer, "Sample Buffer");
					Assert.Null (sampleBufferError, "Sample Buffer Error");

					iterator (buffer);

					sampleCount = cursor.StepInDecodeOrder (1);
				} while (sampleCount == 1);
			}
		}

		[Test]
		public void DecodeFrameTest ()
		{
			TestRuntime.AssertXcodeVersion (26, 0);

			using var url = NSBundle.MainBundle.GetUrlForResource ("xamvideotest", "mp4");
			Assert.That (url, Is.Not.Null, "Url");

			var failures = new List<string> ();

			var bufferEnumerator = new SampleBufferEnumerator (url);

			var frameCallbackCounter = 0;
			const nint sourceFrameValue = 0x0ee1f00d;
			using var session = CreateSession (bufferEnumerator.FormatDescription,
				(sourceFrame, status, flags, buffer, presentationTimeStamp, presentationDuration) => {
					frameCallbackCounter++;
					if (status != VTStatus.Ok)
						failures.Add ($"Output callback #{frameCallbackCounter} failed. Expected status = Ok, got status = {status}");
					if (sourceFrame != sourceFrameValue)
						failures.Add ($"Output callback #{frameCallbackCounter} failed: Expected sourceFrame = 0x{sourceFrameValue:x}, got sourceFrame = 0x{sourceFrame:x}");
				});

			bufferEnumerator.Enumerate ((buffer) => {
				var status = session.DecodeFrame (buffer, VTDecodeFrameFlags.EnableAsynchronousDecompression, sourceFrameValue, out var infoFlags);
				Assert.That (status, Is.EqualTo (VTStatus.Ok), "DecodeFrame");
			});

			Assert.That (session.FinishDelayedFrames (), Is.EqualTo (VTStatus.Ok), "FinishDelayedFrames");
			Assert.That (frameCallbackCounter, Is.GreaterThan (0), "Frame callback counter");
			Assert.That (failures, Is.Empty, "Failures");
		}

#if !__TVOS__
		[Test]
		public void DecodeFrameMultiImageCallbackTest ()
		{
			TestRuntime.AssertXcodeVersion (26, 0);

			if (!VTDecompressionSession.IsStereoMvHevcDecodeSupported ())
				Assert.Ignore ("Stereo MV-HEVC decoding is not supported on the current system.");

			TestRuntime.AssertXcodeVersion (16, 0);

			using var url = NSBundle.MainBundle.GetUrlForResource ("hummingbird", "mov");
			Assert.That (url, Is.Not.Null, "Url");

			var failures = new List<string> ();

			var bufferEnumerator = new SampleBufferEnumerator (url, characteristic: AVMediaCharacteristics.ContainsStereoMultiviewVideo);

			var frameCallbackCounter = 0;

			using var session = CreateSession (bufferEnumerator.FormatDescription, null);

			bufferEnumerator.Enumerate ((buffer) => {
				var status = session.DecodeFrame (buffer, VTDecodeFrameFlags.EnableAsynchronousDecompression, out var infoFlags,
					(status, infoFlags, imageBuffer, taggedBufferGroup, presentationTimeStamp, presentationDuration) => {
						frameCallbackCounter++;
						if (status != VTStatus.Ok)
							failures.Add ($"DecodeFrameMultiImageCallbackTest #{frameCallbackCounter} failed. Expected status = Ok, got status = {status}");
					});
				Assert.That (status, Is.EqualTo (VTStatus.Ok), "DecodeFrame");
			});

			Assert.That (session.FinishDelayedFrames (), Is.EqualTo (VTStatus.Ok), "FinishDelayedFrames");
			Assert.That (frameCallbackCounter, Is.GreaterThan (0), "Frame callback counter 2");
			Assert.That (failures, Is.Empty, "Failures");
		}

		[Test]
		public void DecodeFrameSetMultiImageCallbackTest ()
		{
			TestRuntime.AssertXcodeVersion (26, 0);

			if (!VTDecompressionSession.IsStereoMvHevcDecodeSupported ())
				Assert.Ignore ("Stereo MV-HEVC decoding is not supported on the current system.");

			TestRuntime.AssertXcodeVersion (16, 0);

			using var url = NSBundle.MainBundle.GetUrlForResource ("hummingbird", "mov");
			Assert.That (url, Is.Not.Null, "Url");

			var failures = new List<string> ();

			var bufferEnumerator = new SampleBufferEnumerator (url, characteristic: AVMediaCharacteristics.ContainsStereoMultiviewVideo);

			var frameCallbackCounter = 0;
			var multiFrameCallbackCounter = 0;

			const nint sourceFrameValue = 0x0a1efeab;
			const nint multiSourceFrameValue = 0x0ea1f00d;

			using var session = CreateSession (bufferEnumerator.FormatDescription,
				(sourceFrame, status, flags, buffer, presentationTimeStamp, presentationDuration) => {
					frameCallbackCounter++;
					if (status != VTStatus.Ok)
						failures.Add ($"Output callback #{frameCallbackCounter} failed. Expected status = Ok, got status = {status}");
					if (sourceFrame != sourceFrameValue)
						failures.Add ($"Output callback #{frameCallbackCounter} failed: Expected sourceFrame = 0x{sourceFrameValue:x}, got sourceFrame = 0x{sourceFrame:x}");
				});

			var st = session.SetMultiImageCallback ((outputMultiImageReference, sourceFrameReference, status, infoFlags, taggedBufferGroup, presentationTimeStamp, presentationDuration) => {
				if (sourceFrameReference != sourceFrameValue)
					failures.Add ($"Multi output callback #{multiFrameCallbackCounter} failed: Expected sourceFrame = 0x{sourceFrameValue:x}, got sourceFrame = 0x{sourceFrameReference:x}");
				if (outputMultiImageReference != multiSourceFrameValue)
					failures.Add ($"Multi output callback #{multiFrameCallbackCounter} failed: Expected outputMultiImageReference = 0x{multiSourceFrameValue:x}, got outputMultiImageReference = 0x{outputMultiImageReference:x}");
				multiFrameCallbackCounter++;
			}, multiSourceFrameValue);

			bufferEnumerator.Enumerate ((buffer) => {
				var status = session.DecodeFrame (buffer, VTDecodeFrameFlags.EnableAsynchronousDecompression, sourceFrameValue, out var infoFlags);
				Assert.That (status, Is.EqualTo (VTStatus.Ok), "DecodeFrame");
			});

			Assert.That (session.FinishDelayedFrames (), Is.EqualTo (VTStatus.Ok), "FinishDelayedFrames");
			Assert.That (frameCallbackCounter, Is.GreaterThan (0), "Frame callback counter 2");
			Assert.That (failures, Is.Empty, "Failures");
		}

		[Test]
		public void DecodeFrameCallbackTest ()
		{
			TestRuntime.AssertXcodeVersion (16, 0);

			using var url = NSBundle.MainBundle.GetUrlForResource ("hummingbird", "mov");
			Assert.That (url, Is.Not.Null, "Url");

			var failures = new List<string> ();

			var bufferEnumerator = new SampleBufferEnumerator (url);

			var frameCallbackCounter = 0;
			var inlineCallback = 0;
			using var session = CreateSession (bufferEnumerator.FormatDescription, null);

			const nint sourceFrameValue = 0x0a1efeab;
			var st = session.SetMultiImageCallback ((outputMultiImageReference, sourceFrameReference, status, infoFlags, taggedBufferGroup, presentationTimeStamp, presentationDuration) => {
				if (sourceFrameReference != sourceFrameValue)
					failures.Add ($"Output callback #{frameCallbackCounter} failed: Expected sourceFrame = 0x{sourceFrameValue:x}, got sourceFrame = 0x{sourceFrameReference:x}");
				frameCallbackCounter++;
			}, sourceFrameValue);

			bufferEnumerator.Enumerate ((buffer) => {
				var status = session.DecodeFrame (buffer, VTDecodeFrameFlags.EnableAsynchronousDecompression, (NSDictionary?) null, out var infoFlags,
					(status, flags, buffer, presentationTimeStamp, presentationDuration) => {
						inlineCallback++;
					});
				Assert.That (status, Is.EqualTo (VTStatus.Ok), "DecodeFrame");
			});
			Assert.That (session.FinishDelayedFrames (), Is.EqualTo (VTStatus.Ok), "FinishDelayedFrames");

			Assert.That (frameCallbackCounter, Is.EqualTo (0), "Frame callback counter 3");
			Assert.That (inlineCallback, Is.GreaterThan (0), "Frame callback counter 4");
			Assert.That (failures, Is.Empty, "Failures");
		}
#endif // !__TVOS__
	}
}
