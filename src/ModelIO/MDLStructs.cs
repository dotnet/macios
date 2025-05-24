//
// ModelIO/MIEnums.cs: definitions
//
// Authors:
//   Miguel de Icaza
//
// Copyright 2015 Xamarin, Inc.
//
//
using System;
using System.Numerics;
using System.Runtime.InteropServices;
using Foundation;
using CoreFoundation;
using CoreGraphics;
using Metal;
using ObjCRuntime;

using VectorInt4 = global::CoreGraphics.NVector4i;

#nullable enable

namespace ModelIO {

#if !COREBUILD
	/// <summary>Extension methods for <see cref="ModelIO.MDLVertexFormat" />.</summary>
	///     <remarks>To be added.</remarks>
	[SupportedOSPlatform ("ios")]
	[SupportedOSPlatform ("maccatalyst")]
	[SupportedOSPlatform ("macos")]
	[SupportedOSPlatform ("tvos")]
	public static class MDLVertexFormatExtensions {
		[SupportedOSPlatform ("ios")]
		[SupportedOSPlatform ("macos")]
		[SupportedOSPlatform ("maccatalyst")]
		[SupportedOSPlatform ("tvos")]
		[DllImport (Constants.MetalKitLibrary)]
		static extern /* MTLVertexFormat */ nuint MTKMetalVertexFormatFromModelIO (/* MTLVertexFormat */ nuint vertexFormat);

		/// <param name="vertexFormat">To be added.</param>
		///         <summary>Converts the current vertex format into the specified <paramref name="vertexFormat" />.</summary>
		///         <returns>To be added.</returns>
		///         <remarks>To be added.</remarks>
		[SupportedOSPlatform ("ios")]
		[SupportedOSPlatform ("macos")]
		[SupportedOSPlatform ("maccatalyst")]
		[SupportedOSPlatform ("tvos")]
		public static MTLVertexFormat ToMetalVertexFormat (this MDLVertexFormat vertexFormat)
		{
			nuint mtlVertexFormat = MTKMetalVertexFormatFromModelIO ((nuint) (ulong) vertexFormat);
			return (MTLVertexFormat) (ulong) mtlVertexFormat;
		}
	}
#endif

	/// <summary>A bounding box whose axes are aligned with its coordinate system.</summary>
	///     <remarks>To be added.</remarks>
	[SupportedOSPlatform ("ios")]
	[SupportedOSPlatform ("maccatalyst")]
	[SupportedOSPlatform ("macos")]
	[SupportedOSPlatform ("tvos")]
	[StructLayout (LayoutKind.Sequential)]
	public struct MDLAxisAlignedBoundingBox {
		/// <summary>Gets the maximum bounds.</summary>
		///         <remarks>To be added.</remarks>
		public Vector3 MaxBounds;
		/// <summary>Gets the minimum bounds.</summary>
		///         <remarks>To be added.</remarks>
		public Vector3 MinBounds;

		public MDLAxisAlignedBoundingBox (Vector3 maxBounds, Vector3 minBounds)
		{
			MaxBounds = maxBounds;
			MinBounds = minBounds;
		}

	}

	/// <summary>Provides the extent of voxel data.</summary>
	[SupportedOSPlatform ("ios")]
	[SupportedOSPlatform ("maccatalyst")]
	[SupportedOSPlatform ("macos")]
	[SupportedOSPlatform ("tvos")]
	[StructLayout (LayoutKind.Sequential)]
	public struct MDLVoxelIndexExtent {
		public VectorInt4 MinimumExtent { get; private set; }
		public VectorInt4 MaximumExtent { get; private set; }

		public MDLVoxelIndexExtent (VectorInt4 minimumExtent, VectorInt4 maximumExtent)
		{
			this.MinimumExtent = minimumExtent;
			this.MaximumExtent = maximumExtent;
		}
	}
}
