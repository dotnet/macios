//
// Unit tests for VTCompressionSession
//
// Authors:
//	Alex Soto <alex.soto@xamarin.com>
//	
//
// Copyright 2015 Xamarin Inc. All rights reserved.
//

using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using System.Threading;

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
	public class VTCompressionSessionTests {
		[Test]
		public void CompressionSessionPropertiesTest ()
		{
			TestRuntime.AssertXcodeVersion (15, 0);

			Assert.That (VTCompressionSession.IsStereoMvHevcEncodeSupported (), Is.EqualTo (true).Or.EqualTo (false), "IsStereoMvHevcEncodeSupported");
		}

		[Test]
		public void CompressionSessionCreateTest ()
		{
			TestRuntime.AssertSystemVersion (ApplePlatform.iOS, 8, 0, throwIfOtherPlatform: false);
			TestRuntime.AssertSystemVersion (ApplePlatform.MacOSX, 10, 8, throwIfOtherPlatform: false);
			TestRuntime.AssertSystemVersion (ApplePlatform.TVOS, 10, 2, throwIfOtherPlatform: false);

			using (var session = CreateSession ()) {
				Assert.IsNotNull (session, "Session should not be null");
			}
		}

		[Test]
		public void CompressionSessionSetCompressionPropertiesTest ()
		{
			TestRuntime.AssertSystemVersion (ApplePlatform.iOS, 8, 0, throwIfOtherPlatform: false);
			TestRuntime.AssertSystemVersion (ApplePlatform.MacOSX, 10, 10, throwIfOtherPlatform: false);
			TestRuntime.AssertSystemVersion (ApplePlatform.TVOS, 10, 2, throwIfOtherPlatform: false);

			using (var session = CreateSession ()) {

				var result = session.SetCompressionProperties (new VTCompressionProperties {
					RealTime = true,
					AllowFrameReordering = false
				});

				Assert.That (result, Is.EqualTo (VTStatus.Ok), "SetCompressionProperties");
			}
		}

		[Test]
		public void CompressionSessionSetPropertiesTest ()
		{
			TestRuntime.AssertSystemVersion (ApplePlatform.iOS, 8, 0, throwIfOtherPlatform: false);
			TestRuntime.AssertSystemVersion (ApplePlatform.MacOSX, 10, 8, throwIfOtherPlatform: false);
			TestRuntime.AssertSystemVersion (ApplePlatform.TVOS, 10, 2, throwIfOtherPlatform: false);

			using (var session = CreateSession ()) {

				var result = session.SetProperties (new VTPropertyOptions {
					ReadWriteStatus = VTReadWriteStatus.ReadWrite,
					ShouldBeSerialized = true
				});

				Assert.That (result == VTStatus.Ok, "SetProperties");
			}
		}

		[Test]
		public void CompressionSessionSetCompressionPropertiesMultiPassStorageTest ()
		{
			TestRuntime.AssertSystemVersion (ApplePlatform.iOS, 8, 0, throwIfOtherPlatform: false);
			TestRuntime.AssertSystemVersion (ApplePlatform.MacOSX, 10, 10, throwIfOtherPlatform: false);
			TestRuntime.AssertSystemVersion (ApplePlatform.TVOS, 10, 2, throwIfOtherPlatform: false);

			using (var session = CreateSession ())
			using (var storage = VTMultiPassStorage.Create ()) {
				var result = session.SetCompressionProperties (new VTCompressionProperties {
					RealTime = false,
					AllowFrameReordering = true,
					MultiPassStorage = storage
				});

				Assert.That (result == VTStatus.Ok, "SetCompressionPropertiesMultiPassStorage");
			}
		}

		// On iOS 8 all properties in GetSupportedProperties for Compression session return false on ShouldBeSerialized
		// with this test we will be able to catch if apple changes its mind about this in the future.
		[Test]
		public void CompressionSessionGetSupportedPropertiesTest ()
		{
			TestRuntime.AssertSystemVersion (ApplePlatform.iOS, 8, 0, throwIfOtherPlatform: false);
			TestRuntime.AssertSystemVersion (ApplePlatform.MacOSX, 10, 8, throwIfOtherPlatform: false);
			TestRuntime.AssertSystemVersion (ApplePlatform.TVOS, 10, 2, throwIfOtherPlatform: false);

			using (var session = CreateSession ()) {
				var supportedProps = session.GetSupportedProperties ();
				Assert.NotNull (supportedProps, "GetSupportedProperties IsNull");

				var key = new NSString ("ShouldBeSerialized");
				foreach (var item in supportedProps) {
					var dict = (NSDictionary) item.Value;
					if (dict is null) continue;

					NSObject value;
					if (dict.TryGetValue (key, out value) && value is not null) {
						var number = (NSNumber) value;
						Assert.IsFalse (number.BoolValue, "CompressionSession GetSupportedPropertiesTest ShouldBeSerialized is True");
					}
				}
			}
		}

		// This test is (kind of) expected to be null due to as of iOS 8 all supported properties are not meant to be serialized
		// see CompressionSessionGetSupportedPropertiesTest.
		[Test]
#if MONOMAC || __MACCATALYST__ // https://bugzilla.xamarin.com/show_bug.cgi?id=51258
		[Ignore ("Crashes with SIGSEGV when trying to dispose session after calling session.GetSerializableProperties ()")]
#endif
		public void CompressionSessionGetSerializablePropertiesTest ()
		{
			TestRuntime.AssertSystemVersion (ApplePlatform.iOS, 8, 0, throwIfOtherPlatform: false);
			TestRuntime.AssertSystemVersion (ApplePlatform.MacOSX, 10, 8, throwIfOtherPlatform: false);
			TestRuntime.AssertSystemVersion (ApplePlatform.TVOS, 10, 2, throwIfOtherPlatform: false);

			using (var session = CreateSession ()) {
				var supportedProps = session.GetSerializableProperties ();
				Assert.IsNull (supportedProps, "CompressionSession GetSerializableProperties is not null");
			}
		}

		VTCompressionSession CreateSession (int width = 1024, int height = 768)
		{
			var session = VTCompressionSession.Create (width, height, CMVideoCodecType.H264,
				(sourceFrame, status, flags, buffer) => { });
			return session;
		}


		VTCompressionSession CreateSession2 (bool stronglyTyped, int width = 640, int height = 480, CMVideoCodecType codecType = CMVideoCodecType.H264, VTCompressionSession.VTCompressionOutputCallback? callback = null, CVPixelBufferAttributes? source_attributes = null)
		{
			VTCompressionSession rv;
			if (stronglyTyped) {
				rv = VTCompressionSession.Create (
						width, height,
						codecType,
						callback,
						null,
						source_attributes
						);
			} else {
				rv = VTCompressionSession.Create (
						width, height,
						codecType,
						callback,
						null,
						source_attributes?.Dictionary
						);
			}

			return rv;
		}
		[TestCase (true)]
		[TestCase (false)]
		public void TestCallback (bool stronglyTyped)
		{
			Exception ex = null;
			var thread = new Thread (() => {
				try {
					TestCallbackBackground (stronglyTyped);
				} catch (Exception e) {
					ex = e;
				}
			});
			thread.IsBackground = true;
			thread.Start ();
			var completed = thread.Join (TimeSpan.FromSeconds (30));
			Assert.IsNull (ex); // We check for this before the completion assert, to show any other assertion failures that may occur in CI.
			if (!completed)
				TestRuntime.IgnoreInCI ("This test fails occasionally in CI");
			Assert.IsTrue (completed, "timed out");
		}

		public void TestCallbackBackground (bool stronglyTyped)
		{
			var duration = new CMTime (40, 1);
			VTStatus status;
			var width = 120;
			var height = 120;

			const nint sourceFrameValue = 0x0ea1f00d;
			int callbackCounter = 0;
			var failures = new List<string> ();
			var callback = new VTCompressionSession.VTCompressionOutputCallback ((IntPtr sourceFrame, VTStatus status, VTEncodeInfoFlags flags, CMSampleBuffer buffer) => {
				Interlocked.Increment (ref callbackCounter);
				if (status != VTStatus.Ok)
					failures.Add ($"Callback #{callbackCounter} failed: Expected status = Ok, got status = {status}");
				if (sourceFrame != sourceFrameValue)
					failures.Add ($"Callback #{callbackCounter} failed: Expected sourceFrame = 0x{sourceFrameValue:x}, got sourceFrame = 0x{sourceFrame}");
			});
			using var session = CreateSession2 (stronglyTyped, callback: callback);

			var frameCount = 3;
			for (var i = 0; i < frameCount; i++) {
				using var imageBuffer = new CVPixelBuffer (width, height, CVPixelFormatType.CV420YpCbCr8BiPlanarFullRange);
				var pts = new CMTime (40 * i, 1);
				status = session.EncodeFrame (imageBuffer, pts, duration, null, sourceFrameValue, out var infoFlags);
				Assert.AreEqual (VTStatus.Ok, status, $"status #{i}");
				// This looks weird, but it seems the video encoder can become overwhelmed otherwise, and it
				// will start failing (and taking a long time to do so, eventually timing out the test).
				Thread.Sleep (10);
			}
			status = session.CompleteFrames (new CMTime (40 * frameCount, 1));
			Assert.AreEqual (VTStatus.Ok, status, "status finished");
			Assert.AreEqual (frameCount, callbackCounter, "frame count");
			Assert.That (failures, Is.Empty, "no callback failures");
		}

#if !__TVOS__
		[TestCase (true, true)]
		[TestCase (false, true)]
		[TestCase (true, false)]
		[TestCase (false, false)]
		public void TestMultiImage (bool stronglyTyped, bool customCallback)
		{
			TestRuntime.AssertXcodeVersion (26, 0);

			if (!VTCompressionSession.IsStereoMvHevcEncodeSupported ())
				Assert.Ignore ("Stereo MV-HEVC encoding is not supported on the current system.");

			Exception ex = null;
			var thread = new Thread (() => {
				try {
					TestMultiImageCallbackBackground (stronglyTyped, customCallback);
				} catch (Exception e) {
					ex = e;
				}
			});
			thread.IsBackground = true;
			thread.Start ();
			var completed = thread.Join (TimeSpan.FromSeconds (30));

			if (ex is NUnit.Framework.Internal.NUnitException)
				throw ex;
			Assert.IsNull (ex); // We check for this before the completion assert, to show any other assertion failures that may occur in CI.

			if (!completed)
				TestRuntime.IgnoreInCI ("This test fails occasionally in CI");
			Assert.IsTrue (completed, "timed out");
		}

		void TestMultiImageCallbackBackground (bool stronglyTyped, bool customCallback)
		{
			var duration = new CMTime (40, 1);
			VTStatus status;

			const nint sourceFrameValue = 0x0ee1f00d;
			int callbackCounter = 0;
			int callbackCounter2 = 0;
			var failures = new List<string> ();
			var callback = new VTCompressionSession.VTCompressionOutputCallback ((IntPtr sourceFrame, VTStatus status, VTEncodeInfoFlags flags, CMSampleBuffer buffer) => {
				Interlocked.Increment (ref callbackCounter);
				if (status != VTStatus.Ok)
					failures.Add ($"Output callback #{callbackCounter} failed: Expected status = Ok, got status = {status} = 0x{(int) status:x}");
				if (sourceFrame != sourceFrameValue)
					failures.Add ($"Output callback #{callbackCounter} failed: Expected sourceFrame = 0x{sourceFrameValue:x}, got sourceFrame = 0x{sourceFrame:x}");
			});
			var callback2 = new VTCompressionSession.VTCompressionOutputHandler ((VTStatus status, VTEncodeInfoFlags flags, CMSampleBuffer buffer) => {
				Interlocked.Increment (ref callbackCounter2);
				if (status != VTStatus.Ok)
					failures.Add ($"Output handler #{callbackCounter2} failed B. Expected status = Ok, got status = {status} = 0x{(int) status:x}");
			});

			var width = 120;
			var height = 120;
			var codecType = CMVideoCodecType.Hevc;
			var pixelFormat = CVPixelFormatType.CV420YpCbCr8BiPlanarVideoRange;
			using var session = CreateSession2 (stronglyTyped, width: width, height: height, codecType: codecType, callback: customCallback ? null : callback);

			var IDs = new [] { new NSNumber (0), new NSNumber (1) };
			var compressionProperties = new VTCompressionProperties {
				MvHevcVideoLayerIds = IDs,
				MvHevcViewIds = IDs,
				MvHevcLeftAndRightViewIds = IDs,
				HasLeftStereoEyeView = true,
				HasRightStereoEyeView = true,
			};
			session.SetCompressionProperties (compressionProperties);

			var frameCount = 3;
			var chunks = 2;
			for (var i = 0; i < frameCount; i++) {
				var buffers = new List<CVPixelBuffer> ();
				var tagCollections = new List<CMTagCollection> ();

				buffers.Add (new CVPixelBuffer (width, height, pixelFormat));
				tagCollections.Add (CMTagCollection.Create (CMTag.CreateWithFlagsValue (CMTagCategory.StereoView, 1), CMTag.CreateWithSInt64Value (CMTagCategory.VideoLayerId, 0)));
				buffers.Add (new CVPixelBuffer (width, height, pixelFormat));
				tagCollections.Add (CMTagCollection.Create (CMTag.CreateWithFlagsValue (CMTagCategory.StereoView, 2), CMTag.CreateWithSInt64Value (CMTagCategory.VideoLayerId, 1)));

				using var taggedBufferGroup = CMTaggedBufferGroup.Create (tagCollections.ToArray (), buffers.ToArray (), out var taggedBufferGroupStatus);
				Assert.That (taggedBufferGroup, Is.Not.Null, $"TaggedBuff1erGroup #{i}");
				Assert.That (taggedBufferGroupStatus, Is.EqualTo (CMTaggedBufferGroupError.Success), $"TaggedBufferGroup #{i} Ok");

				var pts = new CMTime (40 * i, 1);
				var infoFlags = default (VTEncodeInfoFlags);
				if (customCallback) {
					status = session.EncodeMultiImageFrame (taggedBufferGroup, pts, duration, null, out infoFlags, callback2);
				} else {
					status = session.EncodeMultiImageFrame (taggedBufferGroup, pts, duration, null, sourceFrameValue, out infoFlags);
				}
				Assert.AreEqual (VTStatus.Ok, status, $"status #{i}");
				Assert.That (infoFlags, Is.EqualTo (VTEncodeInfoFlags.Asynchronous), $"infoFlags #{i}");

				foreach (var img in buffers)
					img.Dispose ();
			}
			status = session.CompleteFrames (new CMTime (40 * frameCount * chunks, 1));
			GC.KeepAlive (session);
			Assert.AreEqual (VTStatus.Ok, status, "status finished");
			if (customCallback) {
				Assert.AreEqual (0, callbackCounter, "frame count A");
				Assert.AreEqual (frameCount, callbackCounter2, "frame count A2");
			} else {
				Assert.AreEqual (frameCount, callbackCounter, "frame count B");
				Assert.AreEqual (0, callbackCounter2, "frame count B2");
			}
			Assert.That (failures, Is.Empty, "no callback failures");
		}
#endif // __TVOS__
	}
}
