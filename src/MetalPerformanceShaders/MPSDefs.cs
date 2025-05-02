#nullable enable

using System;
using System.Numerics;
using System.Runtime.InteropServices;

using Foundation;
using ObjCRuntime;
using Metal;

namespace MetalPerformanceShaders {
	/// <summary>A coordinate that represents an offset.</summary>
	///     <remarks>To be added.</remarks>
	[SupportedOSPlatform ("macos")]
	[SupportedOSPlatform ("ios")]
	[SupportedOSPlatform ("maccatalyst")]
	[SupportedOSPlatform ("tvos")]
	public struct MPSOffset {
		/// <summary>The X coordinate.</summary>
		///         <remarks>To be added.</remarks>
		public nint X;
		/// <summary>The Y coordinate.</summary>
		///         <remarks>To be added.</remarks>
		public nint Y;
		/// <summary>The Z coordinate.</summary>
		///         <remarks>To be added.</remarks>
		public nint Z;
	}

	/// <summary>A coordinate that represents the origin of a coordinate system.</summary>
	///     <remarks>To be added.</remarks>
	[SupportedOSPlatform ("macos")]
	[SupportedOSPlatform ("ios")]
	[SupportedOSPlatform ("maccatalyst")]
	[SupportedOSPlatform ("tvos")]
	public struct MPSOrigin {
		/// <summary>The X coordinate.</summary>
		///         <remarks>To be added.</remarks>
		public double X;
		/// <summary>The Y coordinate.</summary>
		///         <remarks>To be added.</remarks>
		public double Y;
		/// <summary>The Z coordinate.</summary>
		///         <remarks>To be added.</remarks>
		public double Z;
	}

	/// <summary>A structure that represents a width, height, and depth.</summary>
	///     <remarks>To be added.</remarks>
	[SupportedOSPlatform ("macos")]
	[SupportedOSPlatform ("ios")]
	[SupportedOSPlatform ("maccatalyst")]
	[SupportedOSPlatform ("tvos")]
	public struct MPSSize {
		/// <summary>The width of the region.</summary>
		///         <remarks>To be added.</remarks>
		public double Width;
		/// <summary>The height of the region.</summary>
		///         <remarks>To be added.</remarks>
		public double Height;
		/// <summary>The depth of the region.</summary>
		///         <remarks>To be added.</remarks>
		public double Depth;
	}

	[SupportedOSPlatform ("ios13.0")]
	[SupportedOSPlatform ("tvos13.0")]
	[SupportedOSPlatform ("macos")]
	[SupportedOSPlatform ("maccatalyst")]
	public struct MPSDimensionSlice {
		public nuint Start;
		public nuint Length;
	}

	/// <summary>Structure that represents a region as an origin and a size.</summary>
	///     <remarks>To be added.</remarks>
	[SupportedOSPlatform ("macos")]
	[SupportedOSPlatform ("ios")]
	[SupportedOSPlatform ("maccatalyst")]
	[SupportedOSPlatform ("tvos")]
	public struct MPSRegion {
		/// <summary>The origin of the region.</summary>
		///         <remarks>To be added.</remarks>
		public MPSOrigin Origin;
		/// <summary>The depth, width, and height of the region.</summary>
		///         <remarks>To be added.</remarks>
		public MPSSize Size;
	}

	/// <summary>A transformation for use with a Lanczos kernel.</summary>
	///     <remarks>To be added.</remarks>
	[SupportedOSPlatform ("macos")]
	[SupportedOSPlatform ("ios")]
	[SupportedOSPlatform ("maccatalyst")]
	[SupportedOSPlatform ("tvos")]
	public struct MPSScaleTransform {
		/// <summary>The X direction scale factor.</summary>
		///         <remarks>To be added.</remarks>
		public double ScaleX;
		/// <summary>The Y direction scale factor.</summary>
		///         <remarks>To be added.</remarks>
		public double ScaleY;
		/// <summary>The X direction translation amount.</summary>
		///         <remarks>To be added.</remarks>
		public double TranslateX;
		/// <summary>The Y direction translation amount.</summary>
		///         <remarks>To be added.</remarks>
		public double TranslateY;
	}

	/// <summary>To be added.</summary>
	///     <remarks>To be added.</remarks>
	[SupportedOSPlatform ("ios")]
	[SupportedOSPlatform ("tvos")]
	[SupportedOSPlatform ("macos")]
	[SupportedOSPlatform ("maccatalyst")]
	public struct MPSImageCoordinate {
		/// <summary>To be added.</summary>
		///         <remarks>To be added.</remarks>
		public nuint X;
		/// <summary>To be added.</summary>
		///         <remarks>To be added.</remarks>
		public nuint Y;
		/// <summary>To be added.</summary>
		///         <remarks>To be added.</remarks>
		public nuint Channel;
	}

	/// <summary>To be added.</summary>
	///     <remarks>To be added.</remarks>
	[SupportedOSPlatform ("ios")]
	[SupportedOSPlatform ("tvos")]
	[SupportedOSPlatform ("macos")]
	[SupportedOSPlatform ("maccatalyst")]
	public struct MPSImageRegion {
		/// <summary>To be added.</summary>
		///         <remarks>To be added.</remarks>
		public MPSImageCoordinate Offset;
		/// <summary>To be added.</summary>
		///         <remarks>To be added.</remarks>
		public MPSImageCoordinate Size;
	}

	/// <summary>Specifies the range of histogram data in a histogram, the number of entries, and whether to encode the alpha channel.</summary>
	///     <remarks>To be added.</remarks>
	[SupportedOSPlatform ("macos")]
	[SupportedOSPlatform ("ios")]
	[SupportedOSPlatform ("maccatalyst")]
	[SupportedOSPlatform ("tvos")]
	[StructLayout (LayoutKind.Explicit)]
	public struct MPSImageHistogramInfo {
		/// <summary>Specifies  the number of entries in a histogram.</summary>
		///         <remarks>To be added.</remarks>
		[FieldOffset (0)]
		public nuint NumberOfHistogramEntries;
		/// <summary>Specifies whether to encode the alpha channel in the histogram..</summary>
		///         <remarks>To be added.</remarks>
		[FieldOffset (8)]
		public bool HistogramForAlpha;
		/// <summary>Specifies the minimum value of histogram data in a histogram.</summary>
		///         <remarks>To be added.</remarks>
		[FieldOffset (16)]
		public Vector4 MinPixelValue;
		/// <summary>Specifies the maximum value of histogram data in a histogram.</summary>
		///         <remarks>To be added.</remarks>
		[FieldOffset (32)]
		public Vector4 MaxPixelValue;
	}

	// MPSTypes.h
	// FIXME: public delegate IMTLTexture MPSCopyAllocator (MPSKernel filter, IMTLCommandBuffer commandBuffer, IMTLTexture sourceTexture);
	/// <param name="filter">The <see cref="MetalPerformanceShaders.MPSKernel" /> that is requesting the memory.</param>
	///     <param name="commandBuffer">A command buffer that gets the device on which to allocate space for the texture data, along with optional commands to initialize the texture with an encoder.</param>
	///     <param name="sourceTexture">The source image.</param>
	///     <summary>Commands to copy a source texture to a new location. Used for out-of-place filters.</summary>
	///     <returns>Returns a <see cref="Metal.IMTLTexture" /> into which texture data can be written.</returns>
	///     <remarks>Application developers must not enque the <paramref name="commandBuffer" /> parameter, enqueue it, nor wait for scheduling events on it.</remarks>
	public delegate NSObject MPSCopyAllocator (MPSKernel filter, NSObject commandBuffer, NSObject sourceTexture);
	// https://trello.com/c/GqtNId1C/517-generator-our-block-delegates-needs-to-use-wrapper-for-protocols

	/// <summary>Describes a copy operation that supports offsets.</summary>
	///     <remarks>To be added.</remarks>
	[SupportedOSPlatform ("tvos")]
	[SupportedOSPlatform ("macos")]
	[SupportedOSPlatform ("ios")]
	[SupportedOSPlatform ("maccatalyst")]
	public struct MPSMatrixCopyOffsets {
		/// <summary>To be added.</summary>
		///         <remarks>To be added.</remarks>
		public uint SourceRowOffset;
		/// <summary>To be added.</summary>
		///         <remarks>To be added.</remarks>
		public uint SourceColumnOffset;
		/// <summary>To be added.</summary>
		///         <remarks>To be added.</remarks>
		public uint DestinationRowOffset;
		/// <summary>To be added.</summary>
		///         <remarks>To be added.</remarks>
		public uint DestinationColumnOffset;
	}

	/// <summary>Options for the reading and writing of feature channels in an image.</summary>
	///     <remarks>To be added.</remarks>
	[SupportedOSPlatform ("tvos")]
	[SupportedOSPlatform ("macos")]
	[SupportedOSPlatform ("ios")]
	[SupportedOSPlatform ("maccatalyst")]
	public struct MPSImageReadWriteParams {
		/// <summary>To be added.</summary>
		///         <remarks>To be added.</remarks>
		public nuint FeatureChannelOffset;
		/// <summary>To be added.</summary>
		///         <remarks>To be added.</remarks>
		public nuint NumberOfFeatureChannelsToReadWrite;
	}

	/// <summary>Options for the discovery of keypoints in an image.</summary>
	///     <remarks>To be added.</remarks>
	[SupportedOSPlatform ("tvos")]
	[SupportedOSPlatform ("macos")]
	[SupportedOSPlatform ("ios")]
	[SupportedOSPlatform ("maccatalyst")]
	public struct MPSImageKeypointRangeInfo {
		/// <summary>To be added.</summary>
		///         <remarks>To be added.</remarks>
		public nuint MaximumKeypoints;
		/// <summary>To be added.</summary>
		///         <remarks>To be added.</remarks>
		public float MinimumThresholdValue;
	}

	/// <summary>To be added.</summary>
	///     <remarks>To be added.</remarks>
	[SupportedOSPlatform ("tvos")]
	[SupportedOSPlatform ("ios")]
	[SupportedOSPlatform ("macos")]
	[SupportedOSPlatform ("maccatalyst")]
	public struct MPSStateTextureInfo {
		/// <summary>To be added.</summary>
		///         <remarks>To be added.</remarks>
		public nuint Width;
		/// <summary>To be added.</summary>
		///         <remarks>To be added.</remarks>
		public nuint Height;
		/// <summary>To be added.</summary>
		///         <remarks>To be added.</remarks>
		public nuint Depth;
		/// <summary>To be added.</summary>
		///         <remarks>To be added.</remarks>
		public nuint ArrayLength;

#pragma warning disable 0169 // Avoid warning when building core.dll and the unused reserved fields
		nuint _PixelFormat;
		nuint _TextureType;
		nuint _TextureUsage;

		//NSUInteger _reserved [4];
		nuint Reserved0;
		nuint Reserved1;
		nuint Reserved2;
		nuint Reserved3;
#pragma warning restore 0169
#if !COREBUILD
		/// <summary>To be added.</summary>
		///         <value>To be added.</value>
		///         <remarks>To be added.</remarks>
		public MTLPixelFormat PixelFormat {
			get => (MTLPixelFormat) (ulong) _PixelFormat;
			set => _PixelFormat = (nuint) (ulong) value;
		}

		/// <summary>To be added.</summary>
		///         <value>To be added.</value>
		///         <remarks>To be added.</remarks>
		public MTLTextureType TextureType {
			get => (MTLTextureType) (ulong) _TextureType;
			set => _TextureType = (nuint) (ulong) value;
		}

		/// <summary>To be added.</summary>
		///         <value>To be added.</value>
		///         <remarks>To be added.</remarks>
		public MTLTextureUsage TextureUsage {
			get => (MTLTextureUsage) (ulong) _TextureUsage;
			set => _TextureUsage = (nuint) (ulong) value;
		}
#endif
	}

	/// <summary>To be added.</summary>
	///     <remarks>To be added.</remarks>
	[SupportedOSPlatform ("tvos")]
	[SupportedOSPlatform ("macos")]
	[SupportedOSPlatform ("ios")]
	[SupportedOSPlatform ("maccatalyst")]
	[StructLayout (LayoutKind.Sequential)]
	public struct MPSAxisAlignedBoundingBox {
		/// <summary>To be added.</summary>
		///         <remarks>To be added.</remarks>
		public Vector3 Min;
		/// <summary>To be added.</summary>
		///         <remarks>To be added.</remarks>
		public Vector3 Max;
	}

	[SupportedOSPlatform ("ios")]
	[SupportedOSPlatform ("maccatalyst")]
	[SupportedOSPlatform ("macos")]
	[SupportedOSPlatform ("tvos")]
	public static class MPSConstants {
		public const uint FunctionConstantIndex = 127;
		public const uint BatchSizeIndex = 126;
		public const uint UserConstantIndex = 125;
		public const uint NDArrayConstantIndex = 124;
		// Maximum number of textures depends on the platform
		// MaxTextures = 128 or 32,
	}

	[SupportedOSPlatform ("ios")]
	[SupportedOSPlatform ("tvos")]
	[SupportedOSPlatform ("macos")]
	[SupportedOSPlatform ("maccatalyst")]
	[StructLayout (LayoutKind.Sequential)]
	public struct MPSMatrixOffset {
		public uint RowOffset;
		public uint ColumnOffset;
	}

	[StructLayout (LayoutKind.Sequential)]
	public struct MPSNDArrayOffsets {
		// NSInteger dimensions[16];
		nint dimension0;
		nint dimension1;
		nint dimension2;
		nint dimension3;
		nint dimension4;
		nint dimension5;
		nint dimension6;
		nint dimension7;
		nint dimension8;
		nint dimension9;
		nint dimension10;
		nint dimension11;
		nint dimension12;
		nint dimension13;
		nint dimension14;
		nint dimension15;

		public nint [] Dimensions {
			get => new nint [] {
						dimension0,
						dimension1,
						dimension2,
						dimension3,
						dimension4,
						dimension5,
						dimension6,
						dimension7,
						dimension8,
						dimension9,
						dimension10,
						dimension11,
						dimension12,
						dimension13,
						dimension14,
						dimension15,
					};
			set {
				if (value is null)
					ObjCRuntime.ThrowHelper.ThrowArgumentNullException (nameof (value));
				if (value.Length != 16)
					ObjCRuntime.ThrowHelper.ThrowArgumentOutOfRangeException (nameof (value), "Length must be 16.");

				dimension0 = value [0];
				dimension1 = value [1];
				dimension2 = value [2];
				dimension3 = value [3];
				dimension4 = value [4];
				dimension5 = value [5];
				dimension6 = value [6];
				dimension7 = value [7];
				dimension8 = value [8];
				dimension9 = value [9];
				dimension10 = value [10];
				dimension11 = value [11];
				dimension12 = value [12];
				dimension13 = value [13];
				dimension14 = value [14];
				dimension15 = value [15];
			}
		}
	}

	[StructLayout (LayoutKind.Sequential)]
	public struct MPSNDArraySizes {
		// NSUInteger dimensions[16];
		nuint dimension0;
		nuint dimension1;
		nuint dimension2;
		nuint dimension3;
		nuint dimension4;
		nuint dimension5;
		nuint dimension6;
		nuint dimension7;
		nuint dimension8;
		nuint dimension9;
		nuint dimension10;
		nuint dimension11;
		nuint dimension12;
		nuint dimension13;
		nuint dimension14;
		nuint dimension15;

		public nuint [] Dimensions {
			get => new nuint [] {
						dimension0,
						dimension1,
						dimension2,
						dimension3,
						dimension4,
						dimension5,
						dimension6,
						dimension7,
						dimension8,
						dimension9,
						dimension10,
						dimension11,
						dimension12,
						dimension13,
						dimension14,
						dimension15,
					};
			set {
				if (value is null)
					ObjCRuntime.ThrowHelper.ThrowArgumentNullException (nameof (value));
				if (value.Length != 16)
					ObjCRuntime.ThrowHelper.ThrowArgumentOutOfRangeException (nameof (value), "Length must be 16.");

				dimension0 = value [0];
				dimension1 = value [1];
				dimension2 = value [2];
				dimension3 = value [3];
				dimension4 = value [4];
				dimension5 = value [5];
				dimension6 = value [6];
				dimension7 = value [7];
				dimension8 = value [8];
				dimension9 = value [9];
				dimension10 = value [10];
				dimension11 = value [11];
				dimension12 = value [12];
				dimension13 = value [13];
				dimension14 = value [14];
				dimension15 = value [15];
			}
		}
	}
}
