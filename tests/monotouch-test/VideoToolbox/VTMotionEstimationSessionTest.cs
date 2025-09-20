// Copyright (c) Microsoft Corporation.
// Licensed under the MIT License.

using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using System.Threading;
using System.Threading.Tasks;

using CoreVideo;
using Foundation;
using VideoToolbox;

using NUnit.Framework;

using Xamarin.Utils;

namespace MonoTouchFixtures.VideoToolbox;

[TestFixture]
[Preserve (AllMembers = true)]
public class VTMotionEstimationSessionTest {
	[Test]
	public void CreateTest ()
	{
		TestRuntime.AssertXcodeVersion (26, 0);
		// VTMotionEstimationSessionCreate just returns in the simulator (a single 'ret' instruction),
		// which means it returns with status=VTStatus.Ok, but no session actually created. So ignore
		// this test in the simulator.
		TestRuntime.AssertNotSimulator ();

		var width = 120;
		var height = 120;
		using var session = VTMotionEstimationSession.Create ((NSDictionary?) null, (uint) width, (uint) height, out var status);

		/* Creation fail sometimes with -19350 on the bots. This error code is unfortunately not documented by Apple.
		 * Hoping it's "kVTMotionEstimationNotSupportedErr", which Apple mentions in their documentation, but whose
		 * value isn't defined in any headers (oversight?). */
		if ((int) status == -19350)
			Assert.Ignore ("Unknown error code -19350");

		Assert.That (status, Is.EqualTo (VTStatus.Ok), "Create status");
		Assert.That (session, Is.Not.Null, "Create");

		DoTest (session, width, height);
	}

	[Test]
	public void CreateStronglyTypedTest ()
	{
		TestRuntime.AssertXcodeVersion (26, 0);
		// VTMotionEstimationSessionCreate just returns in the simulator (a single 'ret' instruction),
		// which means it returns with status=VTStatus.Ok, but no session actually created. So ignore
		// this test in the simulator.
		TestRuntime.AssertNotSimulator ();

		var width = 120;
		var height = 120;
		var options = new VTMotionEstimationSessionCreationOption () {
			Label = "Test session",
		};
		using var session = VTMotionEstimationSession.Create (options, (uint) width, (uint) height, out var status);

		/* Creation fail sometimes with -19350 on the bots. This error code is unfortunately not documented by Apple.
		 * Hoping it's "kVTMotionEstimationNotSupportedErr", which Apple mentions in their documentation, but whose
		 * value isn't defined in any headers (oversight?). */
		if ((int) status == -19350)
			Assert.Ignore ("Unknown error code -19350");

		Assert.That (status, Is.EqualTo (VTStatus.Ok), "Create status");
		Assert.That (session, Is.Not.Null, "Create");

		DoTest (session, width, height);
	}

	void DoTest (VTMotionEstimationSession session, int width, int height)
	{
		Assert.That (session.TryGetSourcePixelBufferAttributes (out var pixelBufferAttributesDictionary, out var status), Is.EqualTo (true), "TryGetSourcePixelBufferAttributes");
		Assert.That (status, Is.EqualTo (VTStatus.Ok), "TryGetSourcePixelBufferAttributes: status");
		Assert.That (pixelBufferAttributesDictionary, Is.Not.Null, "TryGetSourcePixelBufferAttributes: pixel buffers");

		var pixelBufferAttributes = session.SourcePixelBufferAttributes;
		Assert.That (pixelBufferAttributes, Is.Not.Null, "SourcePixelBufferAttributes: pixel buffers");

		var tcs = new TaskCompletionSource<bool> ();

		var pixelFormatType = pixelBufferAttributes.PixelFormatTypes [0];

		using var referenceImage = new CVPixelBuffer (width, height, pixelFormatType);
		using var currentImage = new CVPixelBuffer (width, height, pixelFormatType);
		var estimatedAnything = false;
		status = session.EstimateMotionVectors (referenceImage, currentImage, VTMotionEstimationFrameFlags.None, null, (VTStatus status2, VTMotionEstimationInfoFlags infoFlags, NSDictionary? additionalInfo, CVPixelBuffer? motionVectors) => {
			try {
				estimatedAnything = true;
			} catch (Exception e) {
				tcs.TrySetException (e);
			} finally {
				tcs.TrySetResult (true);
			}
		});
		Assert.That (status, Is.EqualTo (VTStatus.Ok), "EstimateMotionVectors: status");

		status = session.CompleteFrames ();
		Assert.That (status, Is.EqualTo (VTStatus.Ok), "CompleteFrames: status");

		Assert.That (estimatedAnything, Is.EqualTo (true), "Estimated anything");
	}

	[Test]
	public void GetTypeId ()
	{
		TestRuntime.AssertXcodeVersion (26, 0);

		Assert.AreNotEqual (0, VTMotionEstimationSession.GetTypeId (), "GetTypeId");
	}
}
