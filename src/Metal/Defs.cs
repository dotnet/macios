//
// API for the Metal framework
//
// Authors:
//   Miguel de Icaza
//
// Copyrigh 2014, Xamarin Inc.
//
using System;
using System.ComponentModel;
using System.Runtime.InteropServices;

using Foundation;
using ModelIO;
using ObjCRuntime;

#nullable enable

namespace Metal {
	/// <summary>The location of a pixel in an image or texture.</summary>
	///     <remarks>To be added.</remarks>
	[SupportedOSPlatform ("ios")]
	[SupportedOSPlatform ("maccatalyst")]
	[SupportedOSPlatform ("macos")]
	[SupportedOSPlatform ("tvos")]
	public struct MTLOrigin {
		/// <summary>To be added.</summary>
		///         <remarks>To be added.</remarks>
		public nint X;
		/// <summary>To be added.</summary>
		///         <remarks>To be added.</remarks>
		public nint Y;
		/// <summary>To be added.</summary>
		///         <remarks>To be added.</remarks>
		public nint Z;

		public MTLOrigin (nint x, nint y, nint z)
		{
			X = x;
			Y = y;
			Z = z;
		}

		/// <summary>To be added.</summary>
		///         <returns>To be added.</returns>
		///         <remarks>To be added.</remarks>
		public override string ToString ()
		{
			return String.Format ("({0},{1},{2})", X, Y, Z);
		}
	}

	/// <summary>The dimensions of a grid, image, texture, or threadgroup.</summary>
	///     <remarks>To be added.</remarks>
	[SupportedOSPlatform ("ios")]
	[SupportedOSPlatform ("maccatalyst")]
	[SupportedOSPlatform ("macos")]
	[SupportedOSPlatform ("tvos")]
	public struct MTLSize {
		/// <summary>To be added.</summary>
		///         <remarks>To be added.</remarks>
		public nint Width;
		/// <summary>To be added.</summary>
		///         <remarks>To be added.</remarks>
		public nint Height;
		/// <summary>To be added.</summary>
		///         <remarks>To be added.</remarks>
		public nint Depth;

		public MTLSize (nint width, nint height, nint depth)
		{
			Width = width;
			Height = height;
			Depth = depth;
		}
	}

#if !COREBUILD
	/// <summary>Extension methods for <see cref="Metal.MTLVertexFormat" />.</summary>
	///     <remarks>To be added.</remarks>
	[SupportedOSPlatform ("ios")]
	[SupportedOSPlatform ("maccatalyst")]
	[SupportedOSPlatform ("macos")]
	[SupportedOSPlatform ("tvos")]
	public static class MTLVertexFormatExtensions {

		[SupportedOSPlatform ("ios")]
		[SupportedOSPlatform ("macos")]
		[SupportedOSPlatform ("maccatalyst")]
		[SupportedOSPlatform ("tvos")]
		[DllImport (Constants.MetalKitLibrary)]
		static extern /* MDLVertexFormat */ nuint MTKModelIOVertexFormatFromMetal (/* MTLVertexFormat */ nuint modelIODescriptor);

		/// <param name="vertexFormat">To be added.</param>
		///         <summary>Converts from the current to the desired <paramref name="vertexFormat" />.</summary>
		///         <returns>To be added.</returns>
		///         <remarks>To be added.</remarks>
		[SupportedOSPlatform ("ios")]
		[SupportedOSPlatform ("macos")]
		[SupportedOSPlatform ("maccatalyst")]
		[SupportedOSPlatform ("tvos")]
		public static MDLVertexFormat ToModelVertexFormat (this MTLVertexFormat vertexFormat)
		{
			nuint mdlVertexFormat = MTKModelIOVertexFormatFromMetal ((nuint) (ulong) vertexFormat);
			return (MDLVertexFormat) (ulong) mdlVertexFormat;
		}
	}
#endif

	/// <summary>The retangle used for the scissor fragment test.</summary>
	///     <remarks>To be added.</remarks>
	[SupportedOSPlatform ("ios")]
	[SupportedOSPlatform ("maccatalyst")]
	[SupportedOSPlatform ("macos")]
	[SupportedOSPlatform ("tvos")]
	public struct MTLScissorRect {
		/// <summary>To be added.</summary>
		///         <remarks>To be added.</remarks>
		public nuint X;
		/// <summary>To be added.</summary>
		///         <remarks>To be added.</remarks>
		public nuint Y;
		/// <summary>To be added.</summary>
		///         <remarks>To be added.</remarks>
		public nuint Width;
		/// <summary>To be added.</summary>
		///         <remarks>To be added.</remarks>
		public nuint Height;

		public MTLScissorRect (nuint x, nuint y, nuint width, nuint height)
		{
			X = x;
			Y = y;
			Width = width;
			Height = height;
		}

		/// <summary>To be added.</summary>
		///         <returns>To be added.</returns>
		///         <remarks>To be added.</remarks>
		public override string ToString ()
		{
			return String.Format ("({0},{1},{2},{3}", X, Y, Width, Height);
		}
	}

	/// <summary>Defines the clipping viewport.</summary>
	///     <remarks>To be added.</remarks>
	[SupportedOSPlatform ("ios")]
	[SupportedOSPlatform ("maccatalyst")]
	[SupportedOSPlatform ("macos")]
	[SupportedOSPlatform ("tvos")]
	public struct MTLViewport {
		/// <summary>To be added.</summary>
		///         <remarks>To be added.</remarks>
		public double OriginX;
		/// <summary>To be added.</summary>
		///         <remarks>To be added.</remarks>
		public double OriginY;
		/// <summary>To be added.</summary>
		///         <remarks>To be added.</remarks>
		public double Width;
		/// <summary>To be added.</summary>
		///         <remarks>To be added.</remarks>
		public double Height;
		/// <summary>To be added.</summary>
		///         <remarks>To be added.</remarks>
		public double ZNear;
		/// <summary>To be added.</summary>
		///         <remarks>To be added.</remarks>
		public double ZFar;

		/// <param name="originX">To be added.</param>
		///         <param name="originY">To be added.</param>
		///         <param name="width">To be added.</param>
		///         <param name="height">To be added.</param>
		///         <param name="znear">To be added.</param>
		///         <param name="zfar">To be added.</param>
		///         <summary>To be added.</summary>
		///         <remarks>To be added.</remarks>
		public MTLViewport (double originX, double originY, double width, double height, double znear, double zfar)
		{
			OriginX = originX;
			OriginY = originY;
			Width = width;
			Height = height;
			ZNear = znear;
			ZFar = zfar;
		}

		/// <summary>To be added.</summary>
		///         <returns>To be added.</returns>
		///         <remarks>To be added.</remarks>
		public override string ToString ()
		{
			return String.Format ("({0},{1},{2},{3} Znear={4} Zfar={5})", OriginX, OriginY, Width, Height, ZNear, ZFar);
		}
	}

	/// <summary>A sample position.</summary>
	///     <remarks>To be added.</remarks>
	[SupportedOSPlatform ("ios")]
	[SupportedOSPlatform ("maccatalyst")]
	[SupportedOSPlatform ("macos")]
	[SupportedOSPlatform ("tvos")]
	[StructLayout (LayoutKind.Sequential)]
	public struct MTLSamplePosition {
		/// <summary>The X value, in the range [0.0, 1.0).</summary>
		///         <remarks>To be added.</remarks>
		public float X;

		/// <summary>The Y value, in the range [0.0, 1.0).</summary>
		///         <remarks>To be added.</remarks>
		public float Y;

		/// <param name="x">To be added.</param>
		///         <param name="y">To be added.</param>
		///         <summary>Creates a new normalized sample position.</summary>
		///         <remarks>To be added.</remarks>
		public MTLSamplePosition (float x, float y)
		{
			this.X = x;
			this.Y = y;
		}
	}

	/// <summary>An RGBA color representing a clear pixel.</summary>
	///     <remarks>To be added.</remarks>
	[SupportedOSPlatform ("ios")]
	[SupportedOSPlatform ("maccatalyst")]
	[SupportedOSPlatform ("macos")]
	[SupportedOSPlatform ("tvos")]
	public struct MTLClearColor {
		/// <summary>To be added.</summary>
		///         <remarks>To be added.</remarks>
		public double Red;
		/// <summary>To be added.</summary>
		///         <remarks>To be added.</remarks>
		public double Green;
		/// <summary>To be added.</summary>
		///         <remarks>To be added.</remarks>
		public double Blue;
		/// <summary>To be added.</summary>
		///         <remarks>To be added.</remarks>
		public double Alpha;

		/// <param name="red">To be added.</param>
		///         <param name="green">To be added.</param>
		///         <param name="blue">To be added.</param>
		///         <param name="alpha">To be added.</param>
		///         <summary>To be added.</summary>
		///         <remarks>To be added.</remarks>
		public MTLClearColor (double red, double green, double blue, double alpha)
		{
			Red = red;
			Green = green;
			Blue = blue;
			Alpha = alpha;
		}
	}

	/// <summary>A rectangle of pixels in an image or texture.</summary>
	///     <remarks>To be added.</remarks>
	[SupportedOSPlatform ("ios")]
	[SupportedOSPlatform ("maccatalyst")]
	[SupportedOSPlatform ("macos")]
	[SupportedOSPlatform ("tvos")]
	public struct MTLRegion {
		/// <summary>To be added.</summary>
		///         <remarks>To be added.</remarks>
		public MTLOrigin Origin;
		/// <summary>To be added.</summary>
		///         <remarks>To be added.</remarks>
		public MTLSize Size;

		/// <param name="origin">To be added.</param>
		///         <param name="size">To be added.</param>
		///         <summary>To be added.</summary>
		///         <remarks>To be added.</remarks>
		public MTLRegion (MTLOrigin origin, MTLSize size)
		{
			Origin = origin;
			Size = size;
		}

		public static MTLRegion Create1D (nuint x, nuint width)
		{
			return Create1D ((nint) x, (nint) width);
		}

		public static MTLRegion Create1D (nint x, nint width)
		{
			var region = new MTLRegion ();
			region.Origin.X = x;
			region.Origin.Y = 0;
			region.Origin.Z = 0;
			region.Size.Width = width;
			region.Size.Height = 1;
			region.Size.Depth = 1;
			return region;
		}

		public static MTLRegion Create2D (nuint x, nuint y, nuint width, nuint height)
		{
			return Create2D ((nint) x, (nint) y, (nint) width, (nint) height);
		}

		public static MTLRegion Create2D (nint x, nint y, nint width, nint height)
		{
			var region = new MTLRegion ();
			region.Origin.X = x;
			region.Origin.Y = y;
			region.Origin.Z = 0;
			region.Size.Width = width;
			region.Size.Height = height;
			region.Size.Depth = 1;
			return region;
		}

		public static MTLRegion Create3D (nuint x, nuint y, nuint z, nuint width, nuint height, nuint depth)
		{
			return Create3D ((nint) x, (nint) y, (nint) z, (nint) width, (nint) height, (nint) depth);
		}

		public static MTLRegion Create3D (nint x, nint y, nint z, nint width, nint height, nint depth)
		{
			var region = new MTLRegion ();
			region.Origin.X = x;
			region.Origin.Y = y;
			region.Origin.Z = z;
			region.Size.Width = width;
			region.Size.Height = height;
			region.Size.Depth = depth;
			return region;
		}
	}

	/// <summary>Struct that contains values that are used to clear various buffers and stencils.</summary>
	///     <remarks>To be added.</remarks>
	[SupportedOSPlatform ("ios")]
	[SupportedOSPlatform ("maccatalyst")]
	[SupportedOSPlatform ("macos")]
	[SupportedOSPlatform ("tvos")]
	[StructLayout (LayoutKind.Explicit)]
	public struct MTLClearValue {
		/// <summary>To be added.</summary>
		///         <remarks>To be added.</remarks>
		[FieldOffset (0)]
		public MTLClearColor Color;
		/// <summary>To be added.</summary>
		///         <remarks>To be added.</remarks>
		[FieldOffset (0)]
		public double Depth;
		/// <summary>To be added.</summary>
		///         <remarks>To be added.</remarks>
		[FieldOffset (0)]
		public ulong Stencil;

		/// <param name="color">To be added.</param>
		///         <summary>To be added.</summary>
		///         <remarks>To be added.</remarks>
		public MTLClearValue (MTLClearColor color)
		{
			Depth = 0;
			Stencil = 0;
			Color = color;
		}

		/// <param name="depth">To be added.</param>
		///         <summary>To be added.</summary>
		///         <remarks>To be added.</remarks>
		public MTLClearValue (double depth)
		{
			Color.Red = 0;
			Stencil = 0;

			Depth = depth;
			Color.Green = 0;
			Color.Blue = 0;
			Color.Alpha = 0;
		}

		/// <param name="stencil">To be added.</param>
		///         <summary>To be added.</summary>
		///         <remarks>To be added.</remarks>
		public MTLClearValue (ulong stencil)
		{
			Color.Red = 0;
			Depth = 0;

			Stencil = stencil;
			Color.Green = 0;
			Color.Blue = 0;
			Color.Alpha = 0;
		}
	}

	/// <summary>Represents the number of threadgroups in each grid dimension for indirectly dispatched threadgroups.</summary>
	///     <remarks>To be added.</remarks>
	[SupportedOSPlatform ("ios")]
	[SupportedOSPlatform ("maccatalyst")]
	[SupportedOSPlatform ("macos")]
	[SupportedOSPlatform ("tvos")]
	public struct MTLDispatchThreadgroupsIndirectArguments {
		/// <summary>Gets the threadgrops for the first dimension of the grid.</summary>
		///         <remarks>To be added.</remarks>
		public uint ThreadGroupsPerGrid1;
		/// <summary>Gets the threadgrops for the second dimension of the grid.</summary>
		///         <remarks>To be added.</remarks>
		public uint ThreadGroupsPerGrid2;
		/// <summary>Gets the threadgrops for the third dimension of the grid.</summary>
		///         <remarks>To be added.</remarks>
		public uint ThreadGroupsPerGrid3;
	}

	/// <summary>To be added.</summary>
	///     <remarks>To be added.</remarks>
	[SupportedOSPlatform ("macos")]
	[SupportedOSPlatform ("ios")]
	[SupportedOSPlatform ("tvos")]
	[SupportedOSPlatform ("maccatalyst")]
	[StructLayout (LayoutKind.Sequential)]
	public struct MTLStageInRegionIndirectArguments {
		/// <summary>To be added.</summary>
		///         <remarks>To be added.</remarks>
		public uint StageInOrigin1;
		/// <summary>To be added.</summary>
		///         <remarks>To be added.</remarks>
		public uint StageInOrigin2;
		/// <summary>To be added.</summary>
		///         <remarks>To be added.</remarks>
		public uint StageInOrigin3;

		/// <summary>To be added.</summary>
		///         <remarks>To be added.</remarks>
		public uint StageInSize1;
		/// <summary>To be added.</summary>
		///         <remarks>To be added.</remarks>
		public uint StageInSize2;
		/// <summary>To be added.</summary>
		///         <remarks>To be added.</remarks>
		public uint StageInSize3;
	}

	/// <summary>Represents the data layout needed to draw primitives.</summary>
	///     <remarks>To be added.</remarks>
	[SupportedOSPlatform ("ios")]
	[SupportedOSPlatform ("maccatalyst")]
	[SupportedOSPlatform ("macos")]
	[SupportedOSPlatform ("tvos")]
	public struct MTLDrawPrimitivesIndirectArguments {
		/// <summary>The number of vertices.</summary>
		///         <remarks>To be added.</remarks>
		public uint VertexCount;
		/// <summary>The number instances.</summary>
		///         <remarks>To be added.</remarks>
		public uint InstanceCount;
		/// <summary>The first vertex.</summary>
		///         <remarks>To be added.</remarks>
		public uint VertexStart;
		/// <summary>The first primitive.</summary>
		///         <remarks>To be added.</remarks>
		public uint BaseInstance;
	}

	/// <summary>Represents the data layout needed to draw indexed primitives.</summary>
	///     <remarks>To be added.</remarks>
	[SupportedOSPlatform ("ios")]
	[SupportedOSPlatform ("maccatalyst")]
	[SupportedOSPlatform ("macos")]
	[SupportedOSPlatform ("tvos")]
	public struct MTLDrawIndexedPrimitivesIndirectArguments {
		/// <summary>The number of indices to read from the index buffer for each instance.</summary>
		///         <remarks>To be added.</remarks>
		public uint IndexCount;
		/// <summary>The number of instances.</summary>
		///         <remarks>To be added.</remarks>
		public uint InstanceCount;
		/// <summary>The first index.</summary>
		///         <remarks>To be added.</remarks>
		public uint IndexStart;
		/// <summary>The first vertex.</summary>
		///         <remarks>To be added.</remarks>
		public uint BaseVertex;
		/// <summary>The first primitive.</summary>
		///         <remarks>To be added.</remarks>
		public uint BaseInstance;
	}

	/// <summary>To be added.</summary>
	///     <remarks>To be added.</remarks>
	[SupportedOSPlatform ("ios")]
	[SupportedOSPlatform ("tvos")]
	[SupportedOSPlatform ("macos")]
	[SupportedOSPlatform ("maccatalyst")]
	[StructLayout (LayoutKind.Sequential)]
	public struct MTLSizeAndAlign {
		/// <summary>To be added.</summary>
		///         <remarks>To be added.</remarks>
		public nuint Size;
		/// <summary>To be added.</summary>
		///         <remarks>To be added.</remarks>
		public nuint Align;

		public MTLSizeAndAlign (nuint size, nuint align)
		{
			Size = size;
			Align = align;
		}

	}

	/// <summary>To be added.</summary>
	///     <remarks>To be added.</remarks>
	[SupportedOSPlatform ("ios")]
	[SupportedOSPlatform ("tvos")]
	[SupportedOSPlatform ("macos")]
	[SupportedOSPlatform ("maccatalyst")]
	[StructLayout (LayoutKind.Sequential)]
	public struct MTLDrawPatchIndirectArguments {
		/// <summary>To be added.</summary>
		///         <remarks>To be added.</remarks>
		public uint PatchCount;
		/// <summary>To be added.</summary>
		///         <remarks>To be added.</remarks>
		public uint InstanceCount;
		/// <summary>To be added.</summary>
		///         <remarks>To be added.</remarks>
		public uint PatchStart;
		/// <summary>To be added.</summary>
		///         <remarks>To be added.</remarks>
		public uint BaseInstance;

		/// <param name="pathCount">To be added.</param>
		///         <param name="instanceCount">To be added.</param>
		///         <param name="patchStart">To be added.</param>
		///         <param name="baseInstance">To be added.</param>
		///         <summary>To be added.</summary>
		///         <remarks>To be added.</remarks>
		public MTLDrawPatchIndirectArguments (uint pathCount, uint instanceCount, uint patchStart, uint baseInstance)
		{
			PatchCount = pathCount;
			InstanceCount = instanceCount;
			PatchStart = patchStart;
			BaseInstance = baseInstance;
		}

	}

	/// <summary>To be added.</summary>
	///     <remarks>To be added.</remarks>
	[SupportedOSPlatform ("ios")]
	[SupportedOSPlatform ("tvos")]
	[SupportedOSPlatform ("macos")]
	[SupportedOSPlatform ("maccatalyst")]
	[StructLayout (LayoutKind.Sequential)]
	public struct MTLQuadTessellationFactorsHalf {
#if XAMCORE_5_0
		ushort edgeTessellationFactor0;
		ushort edgeTessellationFactor1;
		ushort edgeTessellationFactor2;
		ushort edgeTessellationFactor3;
		ushort insideTessellationFactor0;
		ushort insideTessellationFactor1;

		public ushort [] EdgeTessellationFactor {
			get => new ushort [] { edgeTessellationFactor0, edgeTessellationFactor1, edgeTessellationFactor2, edgeTessellationFactor3 };
			set {
				if (value.Length > 4)
					throw new ArgumentOutOfRangeException ($"The '{nameof (value)}' array length can't be greater than 4.");

				edgeTessellationFactor0 = value.Length >= 1 ? value [0] : 0;
				edgeTessellationFactor1 = value.Length >= 2 ? value [1] : 0;
				edgeTessellationFactor2 = value.Length >= 3 ? value [2] : 0;
				edgeTessellationFactor3 = value.Length >= 4 ? value [3] : 0;
			}
		}

		public ushort [] InsideTessellationFactor {
			get => new ushort [] { insideTessellationFactor0, insideTessellationFactor1 };
			set {
				if (value.Length > 2)
					throw new ArgumentOutOfRangeException ($"The '{nameof (value)}' array length can't be greater than 2.");

				insideTessellationFactor0 = value.Length >= 1 ? value [0] : 0;
				insideTessellationFactor1 = value.Length >= 2 ? value [1] : 0;
			}
		}
#else
		/// <summary>To be added.</summary>
		///         <remarks>To be added.</remarks>
		[MarshalAs (UnmanagedType.ByValArray, SizeConst = 4)]
		public ushort [] EdgeTessellationFactor;
		/// <summary>To be added.</summary>
		///         <remarks>To be added.</remarks>
		[MarshalAs (UnmanagedType.ByValArray, SizeConst = 2)]
		public ushort [] InsideTessellationFactor;
#endif

		/// <param name="edgeTessellationFactor">To be added.</param>
		///         <param name="insideTessellationFactor">To be added.</param>
		///         <summary>To be added.</summary>
		///         <remarks>To be added.</remarks>
		public MTLQuadTessellationFactorsHalf (ushort [] edgeTessellationFactor, ushort [] insideTessellationFactor)
		{
			if (edgeTessellationFactor.Length > 4)
				throw new ArgumentOutOfRangeException ($"The '{nameof (edgeTessellationFactor)}' array length can't be greater than 4.");

			if (insideTessellationFactor.Length > 2)
				throw new ArgumentOutOfRangeException ($"The '{nameof (insideTessellationFactor)}' array length can't be greater than 2.");
#if XAMCORE_5_0
			edgeTessellationFactor0 = edgeTessellationFactor.Length >= 1 ? edgeTessellationFactor [0] : 0;
			edgeTessellationFactor1 = edgeTessellationFactor.Length >= 2 ? edgeTessellationFactor [1] : 0;
			edgeTessellationFactor2 = edgeTessellationFactor.Length >= 3 ? edgeTessellationFactor [2] : 0;
			edgeTessellationFactor3 = edgeTessellationFactor.Length >= 4 ? edgeTessellationFactor [3] : 0;
			insideTessellationFactor0 = insideTessellationFactor.Length >= 1 ? insideTessellationFactor [0] : 0;
			insideTessellationFactor1 = insideTessellationFactor.Length >= 2 ? insideTessellationFactor [1] : 0;
#else
			EdgeTessellationFactor = edgeTessellationFactor;
			InsideTessellationFactor = insideTessellationFactor;
#endif
		}

	}

	/// <summary>To be added.</summary>
	///     <remarks>To be added.</remarks>
	[SupportedOSPlatform ("ios")]
	[SupportedOSPlatform ("tvos")]
	[SupportedOSPlatform ("macos")]
	[SupportedOSPlatform ("maccatalyst")]
	[StructLayout (LayoutKind.Sequential)]
	public struct MTLTriangleTessellationFactorsHalf {
#if XAMCORE_5_0
		ushort edgeTessellationFactor0;
		ushort edgeTessellationFactor1;
		ushort edgeTessellationFactor2;

		public ushort [] EdgeTessellationFactor {
			get => new ushort [] { edgeTessellationFactor0, edgeTessellationFactor1, edgeTessellationFactor2 };
			set {
				if (value.Length > 3)
					throw new ArgumentOutOfRangeException ($"The '{nameof (value)}' array length can't be greater than 3.");
				edgeTessellationFactor0 = value.Length >= 1 ? value [0] : 0;
				edgeTessellationFactor1 = value.Length >= 2 ? value [1] : 0;
				edgeTessellationFactor2 = value.Length >= 3 ? value [2] : 0;
			}
		}
#else
		/// <summary>To be added.</summary>
		///         <remarks>To be added.</remarks>
		[MarshalAs (UnmanagedType.ByValArray, SizeConst = 3)]
		public ushort [] EdgeTessellationFactor;
#endif
		/// <summary>To be added.</summary>
		///         <remarks>To be added.</remarks>
		public ushort InsideTessellationFactor;

		/// <param name="edgeTessellationFactor">To be added.</param>
		///         <param name="insideTessellationFactor">To be added.</param>
		///         <summary>To be added.</summary>
		///         <remarks>To be added.</remarks>
		public MTLTriangleTessellationFactorsHalf (ushort [] edgeTessellationFactor, ushort insideTessellationFactor)
		{
			if (edgeTessellationFactor.Length > 3)
				throw new ArgumentOutOfRangeException ($"The '{nameof (edgeTessellationFactor)}' array length can't be greater than 3.");
#if XAMCORE_5_0
			edgeTessellationFactor0 = edgeTessellationFactor.Length >= 1 ? edgeTessellationFactor [0] : 0;
			edgeTessellationFactor1 = edgeTessellationFactor.Length >= 2 ? edgeTessellationFactor [1] : 0;
			edgeTessellationFactor2 = edgeTessellationFactor.Length >= 3 ? edgeTessellationFactor [2] : 0;
#else
			EdgeTessellationFactor = edgeTessellationFactor;
#endif
			InsideTessellationFactor = insideTessellationFactor;
		}
	}

#if COREBUILD
	// IMTLCommandBuffer and IMTLTexture visibility are needed for MPSCopyAllocator - but they are generated later

	public partial interface IMTLCommandBuffer {
	}

	public partial interface IMTLTexture {
	}
#endif // COREBUILD
#if MONOMAC
	[SupportedOSPlatform ("macos")]
	[UnsupportedOSPlatform ("ios")]
	[UnsupportedOSPlatform ("tvos")]
	public struct MTLIndirectCommandBufferExecutionRange {
		public uint Location;
		public uint Length;

		public MTLIndirectCommandBufferExecutionRange (uint location, uint length)
		{
			Location = location;
			Length = length;
		}
	}
#endif // MONOMAC

	[SupportedOSPlatform ("macos")]
	[SupportedOSPlatform ("ios13.0")]
	[SupportedOSPlatform ("tvos13.0")]
	[SupportedOSPlatform ("maccatalyst")]
	[StructLayout (LayoutKind.Sequential)]
	public struct MTLTextureSwizzleChannels {
#if COREBUILD
		// keep size identical
		byte Red;
		byte Green;
		byte Blue;
		byte Alpha;
#else
		public MTLTextureSwizzle Red;

		public MTLTextureSwizzle Green;

		public MTLTextureSwizzle Blue;

		public MTLTextureSwizzle Alpha;
#endif
	}

#if IOS || MONOMAC || COREBUILD || TVOS
	[SupportedOSPlatform ("ios13.0")]
	[SupportedOSPlatform ("maccatalyst")]
	[SupportedOSPlatform ("macos")]
	[SupportedOSPlatform ("tvos16.0")]
	[StructLayout (LayoutKind.Sequential)]
	public struct MTLVertexAmplificationViewMapping {
		public uint ViewportArrayIndexOffset;

		public uint RenderTargetArrayIndexOffset;
	}

	[SupportedOSPlatform ("ios13.0")]
	[SupportedOSPlatform ("maccatalyst")]
	[SupportedOSPlatform ("macos")]
	[SupportedOSPlatform ("tvos17.0")]
	[StructLayout (LayoutKind.Sequential)]
	public struct MTLCoordinate2D {
		public float X;

		public float Y;
	}
#endif

	[SupportedOSPlatform ("maccatalyst")]
	[SupportedOSPlatform ("macos")]
	[SupportedOSPlatform ("ios14.0")]
	[SupportedOSPlatform ("tvos16.1")]
	[StructLayout (LayoutKind.Sequential)]
	public struct MTLAccelerationStructureSizes {
		public nuint AccelerationStructureSize;

		public nuint BuildScratchBufferSize;

		public nuint RefitScratchBufferSize;
	}

	[SupportedOSPlatform ("ios16.0")]
	[SupportedOSPlatform ("maccatalyst16.0")]
	[SupportedOSPlatform ("macos13.0")]
	[SupportedOSPlatform ("tvos16.0")]
	[NativeName ("MTLResourceID")]
	[StructLayout (LayoutKind.Sequential)]
	public struct MTLResourceId {
		public ulong Impl;
	}

}
