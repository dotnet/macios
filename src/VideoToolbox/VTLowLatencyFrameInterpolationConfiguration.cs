// Copyright (c) Microsoft Corporation.
// Licensed under the MIT License.

using Foundation;

namespace VideoToolbox;

public partial class VTLowLatencyFrameInterpolationConfiguration {
	/// <summary>Create a new <see cref="VTLowLatencyFrameInterpolationConfiguration" /> instance, specifying the number of interpolated frames.</summary>
	/// <param name="frameWidth">The frame width for the new <see cref="VTLowLatencyFrameInterpolationConfiguration" /> instance.</param>
	/// <param name="frameHeight">The frame height for the new <see cref="VTLowLatencyFrameInterpolationConfiguration" /> instance.</param>
	/// <param name="numberOfInterpolatedFrames">The number of interpolated frames for the new <see cref="VTLowLatencyFrameInterpolationConfiguration" /> instance.</param>
	/// <returns>A new <see cref="VTLowLatencyFrameInterpolationConfiguration" /> instance if successful, <see langword="null" /> otherwise.</returns>
	public static VTLowLatencyFrameInterpolationConfiguration CreateWithNumberOfInterpolatedFrames (nint frameWidth, nint frameHeight, nint numberOfInterpolatedFrames)
	{
		var rv = new VTLowLatencyFrameInterpolationConfiguration (NSObjectFlag.Empty);
		rv.InitializeHandle (rv._InitWithFrameWidthAndNumberOfInterpolatedFrames (frameWidth, frameHeight, numberOfInterpolatedFrames), "initWithFrameWidth:frameHeight:numberOfInterpolatedFrames:");
		return rv;
	}

	/// <summary>Create a new <see cref="VTLowLatencyFrameInterpolationConfiguration" /> instance, specifying the spatial scale factor.</summary>
	/// <param name="frameWidth">The frame width for the new <see cref="VTLowLatencyFrameInterpolationConfiguration" /> instance.</param>
	/// <param name="frameHeight">The frame height for the new <see cref="VTLowLatencyFrameInterpolationConfiguration" /> instance.</param>
	/// <param name="spatialScaleFactor">The spatial scale factor for the new <see cref="VTLowLatencyFrameInterpolationConfiguration" /> instance.</param>
	/// <returns>A new <see cref="VTLowLatencyFrameInterpolationConfiguration" /> instance if successful, <see langword="null" /> otherwise.</returns>
	public static VTLowLatencyFrameInterpolationConfiguration CreateWithSpatialScaleFactor (nint frameWidth, nint frameHeight, nint spatialScaleFactor)
	{
		var rv = new VTLowLatencyFrameInterpolationConfiguration (NSObjectFlag.Empty);
		rv.InitializeHandle (rv._InitWithFrameWidthAndSpatialScaleFactor (frameWidth, frameHeight, spatialScaleFactor), "initWithFrameWidth:frameHeight:spatialScaleFactor:");
		return rv;
	}
}
