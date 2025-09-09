// Copyright (c) Microsoft Corporation.
// Licensed under the MIT License.

using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;

using Foundation;
using VideoToolbox;

using NUnit.Framework;

using Xamarin.Utils;

namespace MonoTouchFixtures.VideoToolbox;

[TestFixture]
[Preserve (AllMembers = true)]
public class VTLowLatencyFrameInterpolationConfigurationTest {
	[Test]
	public void NumberOfInterpolatedFramesCtor ()
	{
		TestRuntime.AssertXcodeVersion (26, 0);
		TestRuntime.AssertNotSimulator (); // VTLowLatencyFrameInterpolationConfiguration is not available in the simulator

		if (!VTLowLatencyFrameInterpolationConfiguration.Supported)
			Assert.Ignore ($"VTLowLatencyFrameInterpolationConfiguration is not supported on this processor");

		Assert.Multiple (() => {
			using var obj = VTLowLatencyFrameInterpolationConfiguration.CreateWithNumberOfInterpolatedFrames (120, 240, 2);
			Assert.That (obj, Is.Not.Null, "obj");
			Assert.That (obj.FrameWidth, Is.EqualTo ((nint) 120), "FrameWidth");
			Assert.That (obj.FrameHeight, Is.EqualTo ((nint) 240), "FrameHeight");
			Assert.That (obj.SpatialScaleFactor, Is.EqualTo ((nint) 1), "SpatialScaleFactor");
			Assert.That (obj.NumberOfInterpolatedFrames, Is.EqualTo ((nint) 0 /* this doesn't make sense, should be 2, but I confirmed the same behavior in Xcode */), "NumberOfInterpolatedFrames");
		});
	}

	[Test]
	public void WithSpatialScaleFactorCtor ()
	{
		TestRuntime.AssertXcodeVersion (26, 0);
		TestRuntime.AssertNotSimulator (); // VTLowLatencyFrameInterpolationConfiguration is not available in the simulator

		if (!VTLowLatencyFrameInterpolationConfiguration.Supported)
			Assert.Ignore ($"VTLowLatencyFrameInterpolationConfiguration is not supported on this processor");

		Assert.Multiple (() => {
			using var obj = VTLowLatencyFrameInterpolationConfiguration.CreateWithSpatialScaleFactor (120, 240, 2);
			Assert.That (obj, Is.Not.Null, "obj");
			Assert.That (obj.FrameWidth, Is.EqualTo ((nint) 120), "FrameWidth");
			Assert.That (obj.FrameHeight, Is.EqualTo ((nint) 240), "FrameHeight");
			Assert.That (obj.SpatialScaleFactor, Is.EqualTo ((nint) 2), "SpatialScaleFactor");
			Assert.That (obj.NumberOfInterpolatedFrames, Is.EqualTo ((nint) 0), "NumberOfInterpolatedFrames");
		});
	}
}
