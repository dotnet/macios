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
using CoreFoundation;
using Foundation;
using ModelIO;
using ObjCRuntime;

#nullable enable

namespace Metal {

	/// <summary>Argument values for use with <see cref="T:Metal.MTLBlendOperation" />.</summary>
	[Native]
	public enum MTLBlendFactor : ulong {
		/// <summary>To be added.</summary>
		Zero = 0,
		/// <summary>To be added.</summary>
		One = 1,
		/// <summary>To be added.</summary>
		SourceColor = 2,
		/// <summary>To be added.</summary>
		OneMinusSourceColor = 3,
		/// <summary>To be added.</summary>
		SourceAlpha = 4,
		/// <summary>To be added.</summary>
		OneMinusSourceAlpha = 5,
		/// <summary>To be added.</summary>
		DestinationColor = 6,
		/// <summary>To be added.</summary>
		OneMinusDestinationColor = 7,
		/// <summary>To be added.</summary>
		DestinationAlpha = 8,
		/// <summary>To be added.</summary>
		OneMinusDestinationAlpha = 9,
		/// <summary>To be added.</summary>
		SourceAlphaSaturated = 10,
		/// <summary>To be added.</summary>
		BlendColor = 11,
		/// <summary>To be added.</summary>
		OneMinusBlendColor = 12,
		/// <summary>To be added.</summary>
		BlendAlpha = 13,
		/// <summary>To be added.</summary>
		OneMinusBlendAlpha = 14,
		[MacCatalyst (13, 1)]
		Source1Color = 15,
		[MacCatalyst (13, 1)]
		OneMinusSource1Color = 16,
		[MacCatalyst (13, 1)]
		Source1Alpha = 17,
		[MacCatalyst (13, 1)]
		OneMinusSource1Alpha = 18,
	}

	/// <summary>How the source and destination values are combined.</summary>
	[Native]
	public enum MTLBlendOperation : ulong {
		/// <summary>To be added.</summary>
		Add = 0,
		/// <summary>To be added.</summary>
		Subtract = 1,
		/// <summary>To be added.</summary>
		ReverseSubtract = 2,
		/// <summary>To be added.</summary>
		Min = 3,
		/// <summary>To be added.</summary>
		Max = 4,
	}

	/// <summary>Permits or restricts writing to specific color channels.</summary>
	[Native]
	[Flags]
	public enum MTLColorWriteMask : ulong {
		/// <summary>To be added.</summary>
		None = 0,
		/// <summary>To be added.</summary>
		Red = 0x1 << 3,
		/// <summary>To be added.</summary>
		Green = 0x1 << 2,
		/// <summary>To be added.</summary>
		Blue = 0x1 << 1,
		/// <summary>To be added.</summary>
		Alpha = 0x1 << 0,
		/// <summary>To be added.</summary>
		All = 0xf,
	}

	/// <summary>The state of the command-buffer state-machine. Enqueued-&gt;Committed-&gt;Scheduled-&gt;Completed.</summary>
	[Native]
	public enum MTLCommandBufferStatus : ulong {
		/// <summary>To be added.</summary>
		NotEnqueued,
		/// <summary>To be added.</summary>
		Enqueued,
		/// <summary>To be added.</summary>
		Committed,
		/// <summary>To be added.</summary>
		Scheduled,
		/// <summary>To be added.</summary>
		Completed,
		/// <summary>To be added.</summary>
		Error,
	}

	/// <summary>Enumeration of error codes describing why a command buffer has failed.</summary>
	[Native]
	[ErrorDomain ("MTLCommandBufferErrorDomain")]
	public enum MTLCommandBufferError : ulong {
		/// <summary>To be added.</summary>
		None = 0,
		/// <summary>To be added.</summary>
		Internal = 1,
		/// <summary>To be added.</summary>
		Timeout = 2,
		/// <summary>To be added.</summary>
		PageFault = 3,
		/// <summary>To be added.</summary>
		Blacklisted = 4,
		/// <summary>To be added.</summary>
		NotPermitted = 7,
		/// <summary>To be added.</summary>
		OutOfMemory = 8,
		/// <summary>To be added.</summary>
		InvalidResource = 9,
		/// <summary>To be added.</summary>
		Memoryless = 10,
		/// <summary>Indicates that the device was removed.</summary>
		DeviceRemoved = 11,
		StackOverflow = 12,
	}

	/// <summary>An action performed at the start of a rendering pass.</summary>
	[Native]
	public enum MTLLoadAction : ulong {
		/// <summary>To be added.</summary>
		DontCare,
		/// <summary>To be added.</summary>
		Load,
		/// <summary>To be added.</summary>
		Clear,
	}

	/// <summary>The action to be performed at the end of a rendering pass.</summary>
	[Native]
	public enum MTLStoreAction : ulong {
		/// <summary>To be added.</summary>
		DontCare,
		/// <summary>To be added.</summary>
		Store,
		/// <summary>To be added.</summary>
		MultisampleResolve,
		/// <summary>To be added.</summary>
		[MacCatalyst (13, 1)]
		StoreAndMultisampleResolve,
		/// <summary>To be added.</summary>
		[MacCatalyst (13, 1)]
		Unknown,
		/// <summary>Indicates that data will be stored in a way that does not consider the sample position.</summary>
		[MacCatalyst (13, 1)]
		CustomSampleDepthStore,
	}

	/// <summary>The dimensionality of each texture and how multiple images are arranged.</summary>
	[Native]
	public enum MTLTextureType : ulong {
		/// <summary>To be added.</summary>
		k1D = 0,
		/// <summary>To be added.</summary>
		k1DArray = 1,
		/// <summary>To be added.</summary>
		k2D = 2,
		/// <summary>To be added.</summary>
		k2DArray = 3,
		/// <summary>To be added.</summary>
		k2DMultisample = 4,
		/// <summary>To be added.</summary>
		kCube = 5,
		/// <summary>To be added.</summary>
		[MacCatalyst (13, 1)]
		kCubeArray = 6,
		/// <summary>To be added.</summary>
		k3D = 7,
		/// <summary>To be added.</summary>
		[MacCatalyst (13, 1)]
		k2DMultisampleArray = 8,
		/// <summary>To be added.</summary>
		[MacCatalyst (13, 1)]
		kTextureBuffer = 9,
	}

	/// <summary>Options for calculating the pixel value returned within a mipmap level.</summary>
	[Native]
	public enum MTLSamplerMinMagFilter : ulong {
		/// <summary>To be added.</summary>
		Nearest,
		/// <summary>To be added.</summary>
		Linear,
	}

	/// <summary>Options for calculating the pixel value returned with multiple mipmap levels.</summary>
	[Native]
	public enum MTLSamplerMipFilter : ulong {
		/// <summary>To be added.</summary>
		NotMipmapped,
		/// <summary>To be added.</summary>
		Nearest,
		/// <summary>To be added.</summary>
		Linear,
	}

	/// <summary>The wrapping mode. Used when a fetch falls outside the bounds of a texture.</summary>
	[Native]
	public enum MTLSamplerAddressMode : ulong {
		/// <summary>To be added.</summary>
		ClampToEdge = 0,
		/// <summary>To be added.</summary>
		[iOS (14, 0)]
		[MacCatalyst (14, 0)]
		MirrorClampToEdge = 1,
		/// <summary>To be added.</summary>
		Repeat = 2,
		/// <summary>To be added.</summary>
		MirrorRepeat = 3,
		/// <summary>To be added.</summary>
		ClampToZero = 4,

		/// <summary>To be added.</summary>
		[iOS (14, 0)]
		[MacCatalyst (14, 0)]
		ClampToBorderColor = 5,
	}

	/// <summary>Enumerates vertex data formats (type and number).</summary>
	[Native]
	public enum MTLVertexFormat : ulong {
		/// <summary>To be added.</summary>
		Invalid = 0,

		/// <summary>To be added.</summary>
		UChar2 = 1,
		/// <summary>To be added.</summary>
		UChar3 = 2,
		/// <summary>To be added.</summary>
		UChar4 = 3,

		/// <summary>To be added.</summary>
		Char2 = 4,
		/// <summary>To be added.</summary>
		Char3 = 5,
		/// <summary>To be added.</summary>
		Char4 = 6,

		/// <summary>To be added.</summary>
		UChar2Normalized = 7,
		/// <summary>To be added.</summary>
		UChar3Normalized = 8,
		/// <summary>To be added.</summary>
		UChar4Normalized = 9,

		/// <summary>To be added.</summary>
		Char2Normalized = 10,
		/// <summary>To be added.</summary>
		Char3Normalized = 11,
		/// <summary>To be added.</summary>
		Char4Normalized = 12,

		/// <summary>To be added.</summary>
		UShort2 = 13,
		/// <summary>To be added.</summary>
		UShort3 = 14,
		/// <summary>To be added.</summary>
		UShort4 = 15,

		/// <summary>To be added.</summary>
		Short2 = 16,
		/// <summary>To be added.</summary>
		Short3 = 17,
		/// <summary>To be added.</summary>
		Short4 = 18,

		/// <summary>To be added.</summary>
		UShort2Normalized = 19,
		/// <summary>To be added.</summary>
		UShort3Normalized = 20,
		/// <summary>To be added.</summary>
		UShort4Normalized = 21,

		/// <summary>To be added.</summary>
		Short2Normalized = 22,
		/// <summary>To be added.</summary>
		Short3Normalized = 23,
		/// <summary>To be added.</summary>
		Short4Normalized = 24,

		/// <summary>To be added.</summary>
		Half2 = 25,
		/// <summary>To be added.</summary>
		Half3 = 26,
		/// <summary>To be added.</summary>
		Half4 = 27,

		/// <summary>To be added.</summary>
		Float = 28,
		/// <summary>To be added.</summary>
		Float2 = 29,
		/// <summary>To be added.</summary>
		Float3 = 30,
		/// <summary>To be added.</summary>
		Float4 = 31,
		/// <summary>To be added.</summary>
		Int = 32,
		/// <summary>To be added.</summary>
		Int2 = 33,
		/// <summary>To be added.</summary>
		Int3 = 34,
		/// <summary>To be added.</summary>
		Int4 = 35,

		/// <summary>To be added.</summary>
		UInt = 36,
		/// <summary>To be added.</summary>
		UInt2 = 37,
		/// <summary>To be added.</summary>
		UInt3 = 38,
		/// <summary>To be added.</summary>
		UInt4 = 39,

		/// <summary>To be added.</summary>
		Int1010102Normalized = 40,
		/// <summary>To be added.</summary>
		UInt1010102Normalized = 41,

		/// <summary>Indicates four unsigned 8-bit characters that describe BGRA channels.</summary>
		[MacCatalyst (13, 1)]
		UChar4NormalizedBgra = 42,
		/// <summary>Indicates a single unsigned 8-bit character.</summary>
		[MacCatalyst (13, 1)]
		UChar = 45,
		/// <summary>Indicates a single signed 8-bit character.</summary>
		[MacCatalyst (13, 1)]
		Char = 46,
		/// <summary>Indicates a single unsigned 8-bit character.</summary>
		[MacCatalyst (13, 1)]
		UCharNormalized = 47,

		/// <summary>Indicates a single normalized signed 8-bit character.</summary>
		[MacCatalyst (13, 1)]
		CharNormalized = 48,
		/// <summary>Indicates a single unsigned 16-bit two's complement value.</summary>
		[MacCatalyst (13, 1)]
		UShort = 49,
		/// <summary>Indicates a single signed 16-bit two's complement value.</summary>
		[MacCatalyst (13, 1)]
		Short = 50,
		/// <summary>ndicates a single normalized unsigned 16-bit two's complement value.</summary>
		[MacCatalyst (13, 1)]
		UShortNormalized = 51,
		/// <summary>Indicates a single normalized signed 16-bit two's complement value</summary>
		[MacCatalyst (13, 1)]
		ShortNormalized = 52,

		/// <summary>Indicates a single half-precision floating point value.</summary>
		[MacCatalyst (13, 1)]
		Half = 53,

		[Mac (14, 0), iOS (17, 0), TV (17, 0), MacCatalyst (17, 0)]
		FloatRG11B10 = 54,
		[Mac (14, 0), iOS (17, 0), TV (17, 0), MacCatalyst (17, 0)]
		FloatRgb9E5 = 55,
	}

	/// <summary>The format used by pixels, including color, depth, compression technigue, and data type for components.</summary>
	[MacCatalyst (13, 1)]
	[Native]
	public enum MTLPixelFormat : ulong {
		/// <summary>Indicates an invalid pixel format.</summary>
		Invalid = 0,
		/// <summary>Indicates the unsigned, normalized integer format.</summary>
		A8Unorm = 1,
		/// <summary>Indicates a single 8-bit normalized integer format.</summary>
		R8Unorm = 10,
		/// <summary>Indicates a single 8-bit unnormalized signed integer format with sRGB-Linear conversion.</summary>
		[NoiOS]
		[NoTV]
		[NoMacCatalyst]
		R8Unorm_sRGB = 11,
		/// <summary>Indicates a single 8-bit normalized integer format.</summary>
		R8Snorm = 12,
		/// <summary>Indicates a single 8-bit unsigned integer format.</summary>
		R8Uint = 13,
		/// <summary>Indicates a single 8-bit signed integer format.</summary>
		R8Sint = 14,
		/// <summary>Indicates a single 16-bit normalized integer format.</summary>
		R16Unorm = 20,
		/// <summary>Indicates a single 16-bit normalized integer format.</summary>
		R16Snorm = 22,
		/// <summary>Indicates a single 16-bit unsigned integer format.</summary>
		R16Uint = 23,
		/// <summary>Indicates a single 16-bit signed integer format.</summary>
		R16Sint = 24,
		/// <summary>Indicates a single 16-bit floating point format.</summary>
		R16Float = 25,
		/// <summary>Indicates a 2-channel unnormalized signed integer format with 8 bits per channel.</summary>
		RG8Unorm = 30,
		/// <summary>Indicates a 2-channel unnormalized signed integer format with 8 bits per channel and sRGB-Linear conversion.</summary>
		[MacCatalyst (13, 1)]
		RG8Unorm_sRGB = 31,
		/// <summary>Indicates a 2-channel normalized signed integer format with 8 bits per channel.</summary>
		RG8Snorm = 32,
		/// <summary>Indicates a 2-channel unsigned integer format with 8 bits per channel.</summary>
		RG8Uint = 33,
		/// <summary>Indicates a 2-channel signed integer format with 8 bits per channel.</summary>
		RG8Sint = 34,
		/// <summary>Indicates a 5-bit B, 6-bit G, and 5-bit R unsigned, normalized integer format.</summary>
		[MacCatalyst (13, 1)]
		B5G6R5Unorm = 40,
		/// <summary>Indicates the unsigned, normalized integer format.</summary>
		[MacCatalyst (13, 1)]
		A1BGR5Unorm = 41,
		/// <summary>Indicates the unsigned, normalized integer format.</summary>
		[MacCatalyst (13, 1)]
		ABGR4Unorm = 42,
		/// <summary>Indicates a 5-bit B, 6-bit G, 5-bit R, and 1-bit Alpha unnormalized signed integer format.</summary>
		[MacCatalyst (13, 1)]
		BGR5A1Unorm = 43,
		/// <summary>Indicates a single 32-bit unsigned integer format.</summary>
		R32Uint = 53,
		/// <summary>Indicates a single 32-bit signed integer format.</summary>
		R32Sint = 54,
		/// <summary>Indicates a single 32-bit floating point format.</summary>
		R32Float = 55,
		/// <summary>Indicates a 2-channel unnormalized signed integer format with 16 bits per channel.</summary>
		RG16Unorm = 60,
		/// <summary>Indicates a 2-channel normalized integer format with 16 bits per channel.</summary>
		RG16Snorm = 62,
		/// <summary>Indicates a 2-channel unsigned integer format with 16 bits per channel.</summary>
		RG16Uint = 63,
		/// <summary>Indicates a 2-channel signed integer format with 16 bits per channel.</summary>
		RG16Sint = 64,
		/// <summary>Indicates a 2-channel floating point format with 16 bits per channel.</summary>
		RG16Float = 65,
		/// <summary>Indicates a format with 4 8-bit normalized unsigned integer channels.</summary>
		RGBA8Unorm = 70,
		/// <summary>Indicates a format with 4 8-bit normalized unsigned integer channels, with sRGB-Linear conversion.</summary>
		RGBA8Unorm_sRGB = 71,
		/// <summary>Indicates a format with 4 8-bit normalized signed integer channels.</summary>
		RGBA8Snorm = 72,
		/// <summary>Indicates a format with 4 8-bit unsigned integer channels.</summary>
		RGBA8Uint = 73,
		/// <summary>Indicates a format with 4 8-bit signed integer channels.</summary>
		RGBA8Sint = 74,
		/// <summary>Indicates an 8-bit BGRA unnormalized signed integer format.</summary>
		BGRA8Unorm = 80,
		/// <summary>Indicates an 8-bit BGRA unnormalized signed integer format with sRGB-Linear conversion</summary>
		BGRA8Unorm_sRGB = 81,
		/// <summary>Indicates a format with 10-bit normalized unsigned integer RGB values plus 2 alpha bits.</summary>
		RGB10A2Unorm = 90,
		/// <summary>Indicates a format with 10-bit unsigned integer RGB values plus 2 alpha bits.</summary>
		RGB10A2Uint = 91,
		/// <summary>Indicates an 11-bit RG plus 10-bit B value floating-point format.</summary>
		RG11B10Float = 92,
		/// <summary>Indicates a format with 9 bits per RGB channel plus a 5-bit shared exponent.</summary>
		RGB9E5Float = 93,
		/// <summary>Indicates a 10-bit B, 10-bit G, 10-bit R, and 2-bit Alpha normalized unsigned integer format.</summary>
		[MacCatalyst (13, 1)]
		BGR10A2Unorm = 94,
		/// <summary>Indicates a 2-channel unsigned integer format with 32 bits per channel.</summary>
		RG32Uint = 103,
		/// <summary>Indicates a 2-channel signed integer format with 32 bits per channel.</summary>
		RG32Sint = 104,
		/// <summary>Indicates a 2-channel floating point format with 32 bits per channel.</summary>
		RG32Float = 105,
		/// <summary>Indicates a format with 4 16-bit normalized unsigned integer channels.</summary>
		RGBA16Unorm = 110,
		/// <summary>Indicates a format with 4 16-bit normalized signed integer channels.</summary>
		RGBA16Snorm = 112,
		/// <summary>Indicates a format with 4 16-bit unsigned integer channels.</summary>
		RGBA16Uint = 113,
		/// <summary>Indicates a format with 4 16-bit signed integer channels.</summary>
		RGBA16Sint = 114,
		/// <summary>Indicates a format with 4 16-bit floating point channels.</summary>
		RGBA16Float = 115,
		/// <summary>Indicates a format with 4 32-bit unsigned integer channels.</summary>
		RGBA32Uint = 123,
		/// <summary>Indicates a format with 4 32-bit signed integer channels.</summary>
		RGBA32Sint = 124,
		/// <summary>Indicates a format with 4 32-bit floating point channels.</summary>
		RGBA32Float = 125,
		/// <summary>To be added.</summary>
		[NoTV]
		[NoiOS]
		[NoMacCatalyst]
		BC1RGBA = 130,
		/// <summary>To be added.</summary>
		[NoTV]
		[NoiOS]
		[NoMacCatalyst]
		BC1_RGBA_sRGB = 131,
		/// <summary>To be added.</summary>
		[NoTV]
		[NoiOS]
		[NoMacCatalyst]
		BC2RGBA = 132,
		/// <summary>To be added.</summary>
		[NoTV]
		[NoiOS]
		[NoMacCatalyst]
		BC2_RGBA_sRGB = 133,
		/// <summary>To be added.</summary>
		[NoTV]
		[NoiOS]
		[NoMacCatalyst]
		BC3RGBA = 134,
		/// <summary>To be added.</summary>
		[NoTV]
		[NoiOS]
		[NoMacCatalyst]
		BC3_RGBA_sRGB = 135,
		/// <summary>To be added.</summary>
		[NoTV]
		[NoiOS]
		[NoMacCatalyst]
		BC4_RUnorm = 140,
		/// <summary>To be added.</summary>
		[NoTV]
		[NoiOS]
		[NoMacCatalyst]
		BC4_RSnorm = 141,
		/// <summary>To be added.</summary>
		[NoTV]
		[NoiOS]
		[NoMacCatalyst]
		BC5_RGUnorm = 142,
		/// <summary>To be added.</summary>
		[NoTV]
		[NoiOS]
		[NoMacCatalyst]
		BC5_RGSnorm = 143,
		/// <summary>To be added.</summary>
		[NoTV]
		[NoiOS]
		[NoMacCatalyst]
		BC6H_RGBFloat = 150,
		/// <summary>To be added.</summary>
		[NoTV]
		[NoiOS]
		[NoMacCatalyst]
		BC6H_RGBUFloat = 151,
		/// <summary>To be added.</summary>
		[NoTV]
		[NoiOS]
		[NoMacCatalyst]
		BC7_RGBAUnorm = 152,
		/// <summary>To be added.</summary>
		[NoTV]
		[NoiOS]
		[NoMacCatalyst]
		BC7_RGBAUnorm_sRGB = 153,

		/// <summary>Indicatea a 2bpp PVRTC RGB format.</summary>
		[Deprecated (PlatformName.iOS, 18, 0, message: "Use ASTC/ETC2/BC formats instead.")]
		[Deprecated (PlatformName.MacCatalyst, 18, 0, message: "Use ASTC/ETC2/BC formats instead.")]
		[Deprecated (PlatformName.MacOSX, 15, 0, message: "Use ASTC/ETC2/BC formats instead.")]
		[Deprecated (PlatformName.TvOS, 18, 0, message: "Use ASTC/ETC2/BC formats instead.")]
		PVRTC_RGB_2BPP = 160,

		/// <summary>Indicatea a 2bpp PVRTC RGB format with sRGB-Linear conversion.</summary>
		[Deprecated (PlatformName.iOS, 18, 0, message: "Use ASTC/ETC2/BC formats instead.")]
		[Deprecated (PlatformName.MacCatalyst, 18, 0, message: "Use ASTC/ETC2/BC formats instead.")]
		[Deprecated (PlatformName.MacOSX, 15, 0, message: "Use ASTC/ETC2/BC formats instead.")]
		[Deprecated (PlatformName.TvOS, 18, 0, message: "Use ASTC/ETC2/BC formats instead.")]
		PVRTC_RGB_2BPP_sRGB = 161,

		/// <summary>Indicatea a 4bpp PVRTC RGB format.</summary>
		[Deprecated (PlatformName.iOS, 18, 0, message: "Use ASTC/ETC2/BC formats instead.")]
		[Deprecated (PlatformName.MacCatalyst, 18, 0, message: "Use ASTC/ETC2/BC formats instead.")]
		[Deprecated (PlatformName.MacOSX, 15, 0, message: "Use ASTC/ETC2/BC formats instead.")]
		[Deprecated (PlatformName.TvOS, 18, 0, message: "Use ASTC/ETC2/BC formats instead.")]
		PVRTC_RGB_4BPP = 162,

		/// <summary>Indicatea a 4bpp PVRTC RGB format with sRGB-Linear conversion.</summary>
		[Deprecated (PlatformName.iOS, 18, 0, message: "Use ASTC/ETC2/BC formats instead.")]
		[Deprecated (PlatformName.MacCatalyst, 18, 0, message: "Use ASTC/ETC2/BC formats instead.")]
		[Deprecated (PlatformName.MacOSX, 15, 0, message: "Use ASTC/ETC2/BC formats instead.")]
		[Deprecated (PlatformName.TvOS, 18, 0, message: "Use ASTC/ETC2/BC formats instead.")]
		PVRTC_RGB_4BPP_sRGB = 163,

		/// <summary>Indicatea a 2bpp PVRTC RGBA format.</summary>
		[Deprecated (PlatformName.iOS, 18, 0, message: "Use ASTC/ETC2/BC formats instead.")]
		[Deprecated (PlatformName.MacCatalyst, 18, 0, message: "Use ASTC/ETC2/BC formats instead.")]
		[Deprecated (PlatformName.MacOSX, 15, 0, message: "Use ASTC/ETC2/BC formats instead.")]
		[Deprecated (PlatformName.TvOS, 18, 0, message: "Use ASTC/ETC2/BC formats instead.")]
		PVRTC_RGBA_2BPP = 164,

		/// <summary>Indicatea a 2bpp PVRTC RGBA format with sRGB-Linear conversion.</summary>
		[Deprecated (PlatformName.iOS, 18, 0, message: "Use ASTC/ETC2/BC formats instead.")]
		[Deprecated (PlatformName.MacCatalyst, 18, 0, message: "Use ASTC/ETC2/BC formats instead.")]
		[Deprecated (PlatformName.MacOSX, 15, 0, message: "Use ASTC/ETC2/BC formats instead.")]
		[Deprecated (PlatformName.TvOS, 18, 0, message: "Use ASTC/ETC2/BC formats instead.")]
		PVRTC_RGBA_2BPP_sRGB = 165,

		/// <summary>Indicatea a 4bpp PVRTC RGBA format.</summary>
		[Deprecated (PlatformName.iOS, 18, 0, message: "Use ASTC/ETC2/BC formats instead.")]
		[Deprecated (PlatformName.MacCatalyst, 18, 0, message: "Use ASTC/ETC2/BC formats instead.")]
		[Deprecated (PlatformName.MacOSX, 15, 0, message: "Use ASTC/ETC2/BC formats instead.")]
		[Deprecated (PlatformName.TvOS, 18, 0, message: "Use ASTC/ETC2/BC formats instead.")]
		PVRTC_RGBA_4BPP = 166,

		/// <summary>Indicatea a 4bpp PVRTC RGBA format with sRGB-Linear conversion.</summary>
		[Deprecated (PlatformName.iOS, 18, 0, message: "Use ASTC/ETC2/BC formats instead.")]
		[Deprecated (PlatformName.MacCatalyst, 18, 0, message: "Use ASTC/ETC2/BC formats instead.")]
		[Deprecated (PlatformName.MacOSX, 15, 0, message: "Use ASTC/ETC2/BC formats instead.")]
		[Deprecated (PlatformName.TvOS, 18, 0, message: "Use ASTC/ETC2/BC formats instead.")]
		PVRTC_RGBA_4BPP_sRGB = 167,

		/// <summary>Indicates a single-channel EAC, unnormalized signed integer format.</summary>
		EAC_R11Unorm = 170,
		/// <summary>Indicates a single-channel EAC, normalized signed integer format.</summary>
		EAC_R11Snorm = 172,
		/// <summary>Indicates a two-channel EAC, unnormalized signed integer format.</summary>
		EAC_RG11Unorm = 174,
		/// <summary>Indicates a two-channel EAC, normalized signed integer format.</summary>
		EAC_RG11Snorm = 176,
		/// <summary>Indicates an 8-bit RGB EAC format.</summary>
		EAC_RGBA8 = 178,
		/// <summary>Indicates an 8-bit RGBa EAC format with sRGB-Linear conversion.</summary>
		EAC_RGBA8_sRGB = 179,
		/// <summary>Indicates an ETC2 format.</summary>
		ETC2_RGB8 = 180,
		/// <summary>Indicates an ETC2 format with sRGB-Linear conversion.</summary>
		ETC2_RGB8_sRGB = 181,
		/// <summary>Indicates an ETC2 format with 8-bit RGB channels and a single-bit Alpha channel.</summary>
		ETC2_RGB8A1 = 182,
		/// <summary>Indicates an ETC2 format with 8-bit RGB channels and a single-bit Alpha channel, with sRGB-Linear conversion.</summary>
		ETC2_RGB8A1_sRGB = 183,


		/// <summary>Indicates the 4x4 ASTC format, with sRGB-Linear conversion.</summary>
		ASTC_4x4_sRGB = 186,
		/// <summary>Indicates the 5x4 ASTC format, with sRGB-Linear conversion.</summary>
		ASTC_5x4_sRGB = 187,
		/// <summary>Indicates the 5x5 ASTC format, with sRGB-Linear conversion.</summary>
		ASTC_5x5_sRGB = 188,
		/// <summary>Indicates the 6x5 ASTC format, with sRGB-Linear conversion.</summary>
		ASTC_6x5_sRGB = 189,
		/// <summary>Indicates the 6x6 ASTC format, with sRGB-Linear conversion.</summary>
		ASTC_6x6_sRGB = 190,
		/// <summary>Indicates the 8x5 ASTC format, with sRGB-Linear conversion.</summary>
		ASTC_8x5_sRGB = 192,
		/// <summary>Indicates the 8x6 ASTC format, with sRGB-Linear conversion.</summary>
		ASTC_8x6_sRGB = 193,
		/// <summary>Indicates the 8x8 ASTC format, with sRGB-Linear conversion.</summary>
		ASTC_8x8_sRGB = 194,
		/// <summary>Indicates the 10x5 ASTC format, with sRGB-Linear conversion.</summary>
		ASTC_10x5_sRGB = 195,
		/// <summary>Indicates the 10x6 ASTC format, with sRGB-Linear conversion.</summary>
		ASTC_10x6_sRGB = 196,
		/// <summary>Indicates the 10x8 ASTC format, with sRGB-Linear conversion.</summary>
		ASTC_10x8_sRGB = 197,
		/// <summary>Indicates the 10x10 ASTC format, with sRGB-Linear conversion.</summary>
		ASTC_10x10_sRGB = 198,
		/// <summary>Indicates the 12x10 ASTC format, with sRGB-Linear conversion.</summary>
		ASTC_12x10_sRGB = 199,
		/// <summary>Indicates the 12x12 ASTC format, with sRGB-Linear conversion.</summary>
		ASTC_12x12_sRGB = 200,

		/// <summary>Indicates the 4x4 ASTC, low dynamic range format.</summary>
		ASTC_4x4_LDR = 204,
		/// <summary>Indicates the 5x4 ASTC, low dynamic range format.</summary>
		ASTC_5x4_LDR = 205,
		/// <summary>Indicates the 5x5 ASTC, low dynamic range format.</summary>
		ASTC_5x5_LDR = 206,
		/// <summary>Indicates the 6x5 ASTC, low dynamic range format.</summary>
		ASTC_6x5_LDR = 207,
		/// <summary>Indicates the 6x6 ASTC, low dynamic range format.</summary>
		ASTC_6x6_LDR = 208,
		/// <summary>Indicates the 8x5 ASTC, low dynamic range format.</summary>
		ASTC_8x5_LDR = 210,
		/// <summary>Indicates the 8x6 ASTC, low dynamic range format.</summary>
		ASTC_8x6_LDR = 211,
		/// <summary>Indicates the 8x8 ASTC, low dynamic range format.</summary>
		ASTC_8x8_LDR = 212,
		/// <summary>Indicates the 10x5 ASTC, low dynamic range format.</summary>
		ASTC_10x5_LDR = 213,
		/// <summary>Indicates the 10x6 ASTC, low dynamic range format.</summary>
		ASTC_10x6_LDR = 214,
		/// <summary>Indicates the 10x8 ASTC, low dynamic range format.</summary>
		ASTC_10x8_LDR = 215,
		/// <summary>Indicates the 10x10 ASTC, low dynamic range format.</summary>
		ASTC_10x10_LDR = 216,
		/// <summary>Indicates the 12x10 ASTC, low dynamic range format.</summary>
		ASTC_12x10_LDR = 217,
		/// <summary>Indicates the 12x12 ASTC, low dynamic range format.</summary>
		ASTC_12x12_LDR = 218,

		[iOS (13, 0)]
		[NoTV]
		[MacCatalyst (13, 1)]
		ASTC_4x4_HDR = 222,
		[iOS (13, 0)]
		[NoTV]
		[MacCatalyst (13, 1)]
		ASTC_5x4_HDR = 223,
		[iOS (13, 0)]
		[NoTV]
		[MacCatalyst (13, 1)]
		ASTC_5x5_HDR = 224,
		[iOS (13, 0)]
		[NoTV]
		[MacCatalyst (13, 1)]
		ASTC_6x5_HDR = 225,
		[iOS (13, 0)]
		[NoTV]
		[MacCatalyst (13, 1)]
		ASTC_6x6_HDR = 226,
		[iOS (13, 0)]
		[NoTV]
		[MacCatalyst (13, 1)]
		ASTC_8x5_HDR = 228,
		[iOS (13, 0)]
		[NoTV]
		[MacCatalyst (13, 1)]
		ASTC_8x6_HDR = 229,
		[iOS (13, 0)]
		[NoTV]
		[MacCatalyst (13, 1)]
		ASTC_8x8_HDR = 230,
		[iOS (13, 0)]
		[NoTV]
		[MacCatalyst (13, 1)]
		ASTC_10x5_HDR = 231,
		[iOS (13, 0)]
		[NoTV]
		[MacCatalyst (13, 1)]
		ASTC_10x6_HDR = 232,
		[iOS (13, 0)]
		[NoTV]
		[MacCatalyst (13, 1)]
		ASTC_10x8_HDR = 233,
		[iOS (13, 0)]
		[NoTV]
		[MacCatalyst (13, 1)]
		ASTC_10x10_HDR = 234,
		[iOS (13, 0)]
		[NoTV]
		[MacCatalyst (13, 1)]
		ASTC_12x10_HDR = 235,
		[iOS (13, 0)]
		[NoTV]
		[MacCatalyst (13, 1)]
		ASTC_12x12_HDR = 236,

		/// <summary>Indicates a red-green horizontally subsampled format.</summary>
		GBGR422 = 240,
		/// <summary>Indicates a red-green horizontally subsampled format.</summary>
		BGRG422 = 241,

		Depth16Unorm = 250,

		/// <summary>Indicates a 32-bit format that is suitable for depth stencils.</summary>
		Depth32Float = 252,
		/// <summary>Indicates a single-channel 8-bit stencil format.</summary>
		Stencil8 = 253,

		/// <summary>To be added.</summary>
		[NoiOS]
		[NoTV]
		[NoMacCatalyst]
		Depth24Unorm_Stencil8 = 255,

		/// <summary>Indicates a 24-bit depth and 8-bit stencil floating-point format.</summary>
		[MacCatalyst (13, 1)]
		Depth32Float_Stencil8 = 260,

		/// <summary>Indicates an 8-bit stencil format.</summary>
		[MacCatalyst (13, 1)]
		X32_Stencil8 = 261,

		/// <summary>To be added.</summary>
		[NoiOS]
		[NoTV]
		[NoMacCatalyst]
		X24_Stencil8 = 262,

		[MacCatalyst (13, 1)]
		BGRA10_XR = 552,
		[MacCatalyst (13, 1)]
		BGRA10_XR_sRGB = 553,
		[MacCatalyst (13, 1)]
		BGR10_XR = 554,
		[MacCatalyst (13, 1)]
		BGR10_XR_sRGB = 555,
	}

	/// <summary>The type of a Metal function.</summary>
	[Native]
	public enum MTLFunctionType : ulong {
		/// <summary>To be added.</summary>
		Vertex = 1,
		/// <summary>To be added.</summary>
		Fragment = 2,
		/// <summary>To be added.</summary>
		Kernel = 3,
		[iOS (14, 0)]
		[TV (14, 0)]
		[MacCatalyst (14, 0)]
		Visible = 5,
		[iOS (14, 0)]
		[NoTV]
		[MacCatalyst (14, 0)]
		Intersection = 6,
		[Mac (13, 0), iOS (16, 0), TV (16, 0), MacCatalyst (16, 0)]
		Mesh = 7,
		[Mac (13, 0), iOS (16, 0), TV (16, 0), MacCatalyst (16, 0)]
		Object = 8,
	}

	/// <summary>Enumerates errors possible in the creation of an MTLLibrary or MTLFunction object.</summary>
	[Native]
	[ErrorDomain ("MTLLibraryErrorDomain")]
	public enum MTLLibraryError : ulong {
		/// <summary>To be added.</summary>
		Unsupported = 1,
		/// <summary>To be added.</summary>
		Internal,
		/// <summary>To be added.</summary>
		CompileFailure,
		/// <summary>To be added.</summary>
		CompileWarning,
		/// <summary>To be added.</summary>
		FunctionNotFound,
		/// <summary>To be added.</summary>
		FileNotFound,
	}

	/// <summary>Holds a comparison test. When the comparison test passes, the incoming fragment is compared to the stored data at the specified location.</summary>
	[Native]
	public enum MTLCompareFunction : ulong {
		/// <summary>To be added.</summary>
		Never = 0,
		/// <summary>To be added.</summary>
		Less = 1,
		/// <summary>To be added.</summary>
		Equal = 2,
		/// <summary>To be added.</summary>
		LessEqual = 3,
		/// <summary>To be added.</summary>
		Greater = 4,
		/// <summary>To be added.</summary>
		NotEqual = 5,
		/// <summary>To be added.</summary>
		GreaterEqual = 6,
		/// <summary>To be added.</summary>
		Always = 7,
	}


	/// <summary>What to do if a stencil value passes or fails a comparison test.</summary>
	[Native]
	public enum MTLStencilOperation : ulong {
		/// <summary>To be added.</summary>
		Keep = 0,
		/// <summary>To be added.</summary>
		Zero = 1,
		/// <summary>To be added.</summary>
		Replace = 2,
		/// <summary>To be added.</summary>
		IncrementClamp = 3,
		/// <summary>To be added.</summary>
		DecrementClamp = 4,
		/// <summary>To be added.</summary>
		Invert = 5,
		/// <summary>To be added.</summary>
		IncrementWrap = 6,
		/// <summary>To be added.</summary>
		DecrementWrap = 7,
	}

	/// <summary>Enumerates the primitive geometry used for drawing commands.</summary>
	[Native]
	public enum MTLPrimitiveType : ulong {
		/// <summary>To be added.</summary>
		Point = 0,
		/// <summary>To be added.</summary>
		Line = 1,
		/// <summary>To be added.</summary>
		LineStrip = 2,
		/// <summary>To be added.</summary>
		Triangle = 3,
		/// <summary>To be added.</summary>
		TriangleStrip = 4,
	}

	/// <summary>The index type for an index buffer.</summary>
	[Native]
	public enum MTLIndexType : ulong {
		/// <summary>To be added.</summary>
		UInt16,
		/// <summary>To be added.</summary>
		UInt32,
	}

	/// <summary>Enumerates values that control how and whether to monitor samples that pass depth and stencil tests.</summary>
	[Native]
	public enum MTLVisibilityResultMode : ulong {
		/// <summary>Indicates that monitoring is turned off.</summary>
		Disabled = 0,
		/// <summary>Indicates that only whether the samples pass the depth and stencil tests should be tracked.</summary>
		Boolean = 1,
		/// <summary>Indicates that the samples that pass should be monitored.</summary>
		Counting = 2,
	}

	/// <summary>Enumerates modes for culling and which types of primitives are culled.</summary>
	[Native]
	public enum MTLCullMode : ulong {
		/// <summary>To be added.</summary>
		None = 0,
		/// <summary>To be added.</summary>
		Front = 1,
		/// <summary>To be added.</summary>
		Back = 2,
	}

	/// <summary>Vertex winding rule for front-facing primitives.</summary>
	[Native]
	public enum MTLWinding : ulong {
		/// <summary>To be added.</summary>
		Clockwise = 0,
		/// <summary>To be added.</summary>
		CounterClockwise = 1,
	}

	/// <summary>How to rasterize triangle and triangle-strip primitives.</summary>
	[Native]
	public enum MTLTriangleFillMode : ulong {
		/// <summary>To be added.</summary>
		Fill,
		/// <summary>To be added.</summary>
		Lines,
	}

	/// <summary>The purgeable state of the resource.</summary>
	[Native]
	public enum MTLPurgeableState : ulong {
		/// <summary>To be added.</summary>
		KeepCurrent = 1,
		/// <summary>To be added.</summary>
		NonVolatile = 2,
		/// <summary>To be added.</summary>
		Volatile = 3,
		/// <summary>To be added.</summary>
		Empty = 4,
	}

	/// <summary>Enumerates what cache mode is used for CPU mapping of a resource.</summary>
	[Native]
	public enum MTLCpuCacheMode : ulong {
		/// <summary>To be added.</summary>
		DefaultCache,
		/// <summary>To be added.</summary>
		WriteCombined,
	}

	/// <summary>Enumerates the ways in which a <see cref="T:Metal.IMTLTexture" /> may be used.</summary>
	[MacCatalyst (13, 1)]
	[Native]
	[Flags]
	public enum MTLTextureUsage : ulong {
		/// <summary>A value that indicates that it is not known what the texture usage option is.</summary>
		Unknown = 0x0000,
		/// <summary>A value that indicates that the texture will be read by shaders at any stage in rendering.</summary>
		ShaderRead = 0x0001,
		/// <summary>A value that indicates that the texture will be written to by compute shaders.</summary>
		ShaderWrite = 0x0002,
		/// <summary>A value that indicates that the texture will be used as a color, depth, or stencil render target in a rendering pass.</summary>
		RenderTarget = 0x0004,
		/// <summary>A value that indicates that the texture will be used for creating new textures.</summary>
		PixelFormatView = 0x0010,

		[Mac (14, 0), iOS (17, 0), TV (17, 0), MacCatalyst (17, 0)]
		ShaderAtomic = 0x20,
	}

	/// <summary>Holds options used by buffer and texture objects.</summary>
	[MacCatalyst (13, 1)]
	[Native]
	[Flags]
	public enum MTLResourceOptions : ulong {
		/// <summary>Indicates the default CPU cache mode.</summary>
		CpuCacheModeDefault = MTLCpuCacheMode.DefaultCache << 0,
		/// <summary>Indicates a write-combined CPU cache mode, which is best for resources that the CPU will write but never read.</summary>
		CpuCacheModeWriteCombined = MTLCpuCacheMode.WriteCombined << 0,

		/// <summary>Indicates that the resource is stored in a location where both the GPU and CPU can access it.</summary>
		[MacCatalyst (13, 1)]
		StorageModeShared = MTLStorageMode.Shared << 4,
		/// <summary>To be added.</summary>
		[NoiOS]
		[NoTV]
		[NoMacCatalyst]
		StorageModeManaged = MTLStorageMode.Managed << 4,
		/// <summary>Indicates that the resource is stored in a location where only the GPU can acces it.</summary>
		[MacCatalyst (13, 1)]
		StorageModePrivate = MTLStorageMode.Private << 4,

		/// <summary>Indicates that the resource is a temporary render target and is stored only on the tile.</summary>
		[NoMac]
		[MacCatalyst (13, 1)]
		StorageModeMemoryless = MTLStorageMode.Memoryless << 4,

		/// <summary>Indicates that the developer must track command encoder dependencies with fences.</summary>
		[MacCatalyst (13, 1)]
		HazardTrackingModeUntracked = 1 << 8,

		[iOS (13, 0), TV (13, 0)]
		[MacCatalyst (13, 1)]
		HazardTrackingModeTracked = 1 << 9,
	}

	// MTLVertexDescriptor.h
	/// <summary>The frequency at which the vertex shader function should fetch attribute data.</summary>
	[Native]
	public enum MTLVertexStepFunction : ulong {
		/// <summary>To be added.</summary>
		Constant,
		/// <summary>To be added.</summary>
		PerVertex,
		/// <summary>To be added.</summary>
		PerInstance,
		/// <summary>To be added.</summary>
		[MacCatalyst (13, 1)]
		PerPatch = 3,
		/// <summary>To be added.</summary>
		[MacCatalyst (13, 1)]
		PerPatchControlPoint = 4,
	}

	/// <summary>The data type of a function argument, as used in the shader language code.</summary>
	[Native]
	public enum MTLDataType : ulong {

		/// <summary>Indicates an unrecognized or invalid type.</summary>
		None = 0,

		/// <summary>Indicates a struct.</summary>
		Struct = 1,
		/// <summary>Indicates an array.</summary>
		Array = 2,

		/// <summary>Indicates a 32-bit floating point value.</summary>
		Float = 3,
		/// <summary>Indicates a vector of two 32-bit floating point values.</summary>
		Float2 = 4,
		/// <summary>Indicates a vector of three 32-bit floating point values.</summary>
		Float3 = 5,
		/// <summary>Indicates a vector of four 32-bit floating point values.</summary>
		Float4 = 6,

		/// <summary>Indicates a 2x2 matrix of 32-bit floating point values.</summary>
		Float2x2 = 7,
		/// <summary>Indicates a 2x3 matrix of 32-bit floating point values.</summary>
		Float2x3 = 8,
		/// <summary>Indicates a 2x4 matrix of 32-bit floating point values.</summary>
		Float2x4 = 9,

		/// <summary>Indicates a 3x2 matrix of 32-bit floating point values.</summary>
		Float3x2 = 10,
		/// <summary>Indicates a 3x3 matrix of 32-bit floating point values.</summary>
		Float3x3 = 11,
		/// <summary>Indicates a 3x4 matrix of 32-bit floating point values.</summary>
		Float3x4 = 12,

		/// <summary>Indicates a 4x2 matrix of 32-bit floating point values.</summary>
		Float4x2 = 13,
		/// <summary>Indicates a 4x3 matrix of 32-bit floating point values.</summary>
		Float4x3 = 14,
		/// <summary>Indicates a 4x4 matrix of 32-bit floating point values.</summary>
		Float4x4 = 15,

		/// <summary>Indicates a 16-bit floating point value.</summary>
		Half = 16,
		/// <summary>Indicates a vector of two 16-bit floating point values.</summary>
		Half2 = 17,
		/// <summary>Indicates a vector of three 16-bit floating point values.</summary>
		Half3 = 18,
		/// <summary>Indicates a vector of four 16-bit floating point values.</summary>
		Half4 = 19,

		/// <summary>Indicates a 2x2 matrix of 16-bit floating point values.</summary>
		Half2x2 = 20,
		/// <summary>Indicates a 2x3 matrix of 16-bit floating point values.</summary>
		Half2x3 = 21,
		/// <summary>Indicates a 2x4 matrix of 16-bit floating point values.</summary>
		Half2x4 = 22,

		/// <summary>Indicates a 3x2 matrix of 16-bit floating point values.</summary>
		Half3x2 = 23,
		/// <summary>Indicates a 3x3 matrix of 16-bit floating point values.</summary>
		Half3x3 = 24,
		/// <summary>Indicates a 3x4 matrix of 16-bit floating point values.</summary>
		Half3x4 = 25,

		/// <summary>Indicates a 4x2 matrix of 16-bit floating point values.</summary>
		Half4x2 = 26,
		/// <summary>Indicates a 4x3 matrix of 16-bit floating point values.</summary>
		Half4x3 = 27,
		/// <summary>Indicates a 4x4 matrix of 16-bit floating point values.</summary>
		Half4x4 = 28,

		/// <summary>Indicates a signed 32-bit integer.</summary>
		Int = 29,
		/// <summary>Indicates a vector of two 32-bit signed integers.</summary>
		Int2 = 30,
		/// <summary>Indicates a vector of three signed 32-bit integers.</summary>
		Int3 = 31,
		/// <summary>Indicates a vector of four signed 32-bit integers.</summary>
		Int4 = 32,

		/// <summary>Indicates an unsigned 32-bit integer.</summary>
		UInt = 33,
		/// <summary>Indicates a value that contains two unsigned 32-bit integer components.</summary>
		UInt2 = 34,
		/// <summary>Indicates a value that contains three unsigned 32-bit integer components.</summary>
		UInt3 = 35,
		/// <summary>Indicates a value that contains four unsigned 32-bit integer components.</summary>
		UInt4 = 36,

		/// <summary>Indicates a signed 16-bit integer.</summary>
		Short = 37,
		/// <summary>Indicates a value that contains two signed 16-bit integer components.</summary>
		Short2 = 38,
		/// <summary>Indicates a value that contains three signed 16-bit integer components.</summary>
		Short3 = 39,
		/// <summary>Indicates a value that contains four signed 16-bit integer components.</summary>
		Short4 = 40,

		/// <summary>Indicates an unsigned 16-bit integer.</summary>
		UShort = 41,
		/// <summary>Indicates a value that contains two unsigned 16-bit integer components.</summary>
		UShort2 = 42,
		/// <summary>Indicates a value that contains three unsigned 16-bit integer components.</summary>
		UShort3 = 43,
		/// <summary>Indicates a value that contains four unsigned 16-bit integer components.</summary>
		UShort4 = 44,

		/// <summary>Indicates a signed 8-bit character.</summary>
		Char = 45,
		/// <summary>Indicates a vector of two signed 8-bit characters.</summary>
		Char2 = 46,
		/// <summary>Indicates a vector of three signed 8-bit characters.</summary>
		Char3 = 47,
		/// <summary>Indicates a vector of four signed 8-bit characters.</summary>
		Char4 = 48,

		/// <summary>Indicates an unsigned 8-bit character.</summary>
		UChar = 49,
		/// <summary>Indicates a value that contains two unsigned 8-bit integer components.</summary>
		UChar2 = 50,
		/// <summary>Indicates a value that contains three unsigned 8-bit integer components.</summary>
		UChar3 = 51,
		/// <summary>Indicates a value that contains four unsigned 8-bit integer components.</summary>
		UChar4 = 52,

		/// <summary>Indicates a Boolean value.</summary>
		Bool = 53,
		/// <summary>Indicates vector of two Boolean values.</summary>
		Bool2 = 54,
		/// <summary>Indicates vector of three Boolean values.</summary>
		Bool3 = 55,
		/// <summary>Indicates vector of four Boolean values</summary>
		Bool4 = 56,
		/// <summary>Indicates a texture.</summary>
		[MacCatalyst (13, 1)]

		Texture = 58,
		/// <summary>Indicates a sampler.</summary>
		[MacCatalyst (13, 1)]
		Sampler = 59,
		/// <summary>Indicates a pointer.</summary>
		[MacCatalyst (13, 1)]
		Pointer = 60,
		/// <summary>Indicates an unsigned 8-bit normalized integer.</summary>
		[MacCatalyst (13, 1)]

		[NoMac, TV (14, 5)] R8Unorm = 62,
		/// <summary>Indicates a signed 8-bit normalized integer.</summary>
		[MacCatalyst (13, 1)]
		[NoMac, TV (14, 5)] R8Snorm = 63,
		/// <summary>Indicates an unsigned 16-bit normalized integer.</summary>
		[MacCatalyst (13, 1)]
		[NoMac, TV (14, 5)] R16Unorm = 64,
		/// <summary>Indicates a signed 16-bit normalized integer.</summary>
		[MacCatalyst (13, 1)]
		[NoMac, TV (14, 5)] R16Snorm = 65,
		/// <summary>Indicates a value that contains two unsigned 8-bit normalized integer components.</summary>
		[MacCatalyst (13, 1)]
		[NoMac, TV (14, 5)] Rg8Unorm = 66,
		/// <summary>Indicates a value that contains two signed 8-bit normalized integer components.</summary>
		[MacCatalyst (13, 1)]
		[NoMac, TV (14, 5)] Rg8Snorm = 67,
		/// <summary>Indicates a value that contains two unsigned 16-bit normalized integer components.</summary>
		[MacCatalyst (13, 1)]
		[NoMac, TV (14, 5)] Rg16Unorm = 68,
		/// <summary>Indicates a value that contains two signed 16-bit normalized integer components.</summary>
		[MacCatalyst (13, 1)]
		[NoMac, TV (14, 5)] Rg16Snorm = 69,
		/// <summary>Indicates a value that contains four unsigned 8-bit normalized integer components.</summary>
		[MacCatalyst (13, 1)]
		[NoMac, TV (14, 5)] Rgba8Unorm = 70,
		/// <summary>Indicates normalized unsigned 8-bit RGBA values, convertible to sRGB.</summary>
		[MacCatalyst (13, 1)]
		[NoMac, TV (14, 5)] Rgba8Unorm_sRgb = 71,
		/// <summary>Indicates a value that contains four signed 8-bit normalized integer components.</summary>
		[MacCatalyst (13, 1)]
		[NoMac, TV (14, 5)] Rgba8Snorm = 72,
		/// <summary>Indicates a value that contains four unsigned 16-bit normalized integer components.</summary>
		[MacCatalyst (13, 1)]
		[NoMac, TV (14, 5)] Rgba16Unorm = 73,
		/// <summary>Indicates a value that contains four signed 16-bit normalized integer components.</summary>
		[MacCatalyst (13, 1)]
		[NoMac, TV (14, 5)] Rgba16Snorm = 74,
		/// <summary>Indicates a packed RGBA normalized integer value with 10 bits each for RGB and 2 bits for A.</summary>
		[MacCatalyst (13, 1)]
		[NoMac, TV (14, 5)] Rgb10A2Unorm = 75,
		/// <summary>Indicates a 32-bit packed floating point RGB value with 11 bits for R and G and 10 bits for B.</summary>
		[MacCatalyst (13, 1)]
		[NoMac, TV (14, 5)] Rg11B10Float = 76,
		/// <summary>Indicates a packed RGBE normalized floating point value with 9 bits each for R, G, and B, with a 5-bit exponent.</summary>
		[MacCatalyst (13, 1)]
		[NoMac, TV (14, 5)] Rgb9E5Float = 77,
		/// <summary>To be added.</summary>
		[MacCatalyst (13, 1)]

		RenderPipeline = 78,
		[MacCatalyst (13, 1)]
		[iOS (13, 0), TV (13, 0)] ComputePipeline = 79,
		/// <summary>To be added.</summary>
		[MacCatalyst (13, 1)]
		IndirectCommandBuffer = 80,

		[iOS (15, 0), TV (15, 0), MacCatalyst (15, 0)] Long = 81,
		[iOS (15, 0), TV (15, 0), MacCatalyst (15, 0)] Long2 = 82,
		[iOS (15, 0), TV (15, 0), MacCatalyst (15, 0)] Long3 = 83,
		[iOS (15, 0), TV (15, 0), MacCatalyst (15, 0)] Long4 = 84,
		[iOS (15, 0), TV (15, 0), MacCatalyst (15, 0)] ULong = 85,
		[iOS (15, 0), TV (15, 0), MacCatalyst (15, 0)] ULong2 = 86,
		[iOS (15, 0), TV (15, 0), MacCatalyst (15, 0)] ULong3 = 87,
		[iOS (15, 0), TV (15, 0), MacCatalyst (15, 0)] ULong4 = 88,
		[MacCatalyst (14, 0)]

		[iOS (14, 0), NoTV] VisibleFunctionTable = 115,
		[MacCatalyst (14, 0)]
		[iOS (14, 0), NoTV] IntersectionFunctionTable = 116,
		[MacCatalyst (14, 0)]
		[iOS (14, 0), NoTV] PrimitiveAccelerationStructure = 117,
		[MacCatalyst (14, 0)]
		[iOS (14, 0), NoTV] InstanceAccelerationStructure = 118,

		[iOS (17, 0), TV (17, 0), Mac (14, 0), MacCatalyst (17, 0)]
		BFloat = 121,
		[iOS (17, 0), TV (17, 0), Mac (14, 0), MacCatalyst (17, 0)]
		BFloat2 = 122,
		[iOS (17, 0), TV (17, 0), Mac (14, 0), MacCatalyst (17, 0)]
		BFloat3 = 123,
		[iOS (17, 0), TV (17, 0), Mac (14, 0), MacCatalyst (17, 0)]
		BFloat4 = 124,
	}

	/// <summary>The resource type of an argument of a function.</summary>
	[Native]
	public enum MTLArgumentType : ulong {
		/// <summary>To be added.</summary>
		Buffer = 0,
		/// <summary>To be added.</summary>
		ThreadgroupMemory = 1,
		/// <summary>To be added.</summary>
		Texture = 2,
		/// <summary>To be added.</summary>
		Sampler = 3,
		/// <summary>To be added.</summary>
		[MacCatalyst (13, 1)]

		[NoMac, TV (14, 5)] ImageblockData = 16,
		/// <summary>To be added.</summary>
		[MacCatalyst (13, 1)]
		[NoMac, TV (14, 5)] Imageblock = 17,

		[iOS (14, 0)]
		[NoTV]
		[MacCatalyst (14, 0)]
		VisibleFunctionTable = 24,
		[iOS (14, 0)]
		[NoTV]
		[MacCatalyst (14, 0)]
		PrimitiveAccelerationStructure = 25,
		[iOS (14, 0)]
		[NoTV]
		[MacCatalyst (14, 0)]
		InstanceAccelerationStructure = 26,
		[iOS (14, 0)]
		[NoTV]
		[MacCatalyst (14, 0)]
		IntersectionFunctionTable = 27,
	}

#if !XAMCORE_5_0
	/// <summary>Function access restrictions of argument data in the shader code.</summary>
	[Deprecated (PlatformName.MacOSX, 14, 0)]
	[Deprecated (PlatformName.iOS, 17, 0)]
	[Deprecated (PlatformName.TvOS, 17, 0)]
	[Deprecated (PlatformName.MacCatalyst, 17, 0)]
	[Native]
	public enum MTLArgumentAccess : ulong {
		/// <summary>To be added.</summary>
		ReadOnly,
		/// <summary>To be added.</summary>
		ReadWrite,
		/// <summary>To be added.</summary>
		WriteOnly,
	}
#endif

	/// <summary>Options relating to argument information in the creation of a pipeline.</summary>
	[Native]
	[Flags]
	public enum MTLPipelineOption : ulong {
		/// <summary>To be added.</summary>
		None,
#if !XAMCORE_5_0
		/// <summary>To be added.</summary>
		[Deprecated (PlatformName.iOS, 18, 0, message: "Use 'BindingInfo' instead.")]
		[Deprecated (PlatformName.TvOS, 18, 0, message: "Use 'BindingInfo' instead.")]
		[Deprecated (PlatformName.MacCatalyst, 18, 0, message: "Use 'BindingInfo' instead.")]
		[Deprecated (PlatformName.MacOSX, 15, 0, message: "Use 'BindingInfo' instead.")]
		ArgumentInfo = 1 << 0,
#endif
		BindingInfo = 1 << 0,
		/// <summary>To be added.</summary>
		BufferTypeInfo,
		[iOS (14, 0)]
		[TV (14, 0)]
		[MacCatalyst (14, 0)]
		FailOnBinaryArchiveMiss = 4,
	}

	/// <summary>Enumerates the hardware feature sets that are available on a device.</summary>
	[Native]
	public enum MTLFeatureSet : ulong {
		/// <summary>The iOS GPU Family 1 v1 feature set.</summary>
#if XAMCORE_5_0
		[NoMacCatalyst]
#elif __MACCATALYST__
		[Obsolete ("Not available on the current platform.")]
#endif
		[NoTV, NoMac]
		iOS_GPUFamily1_v1 = 0,
		/// <summary>The iOS GPU Family 1 v2 feature set.</summary>
#if XAMCORE_5_0
		[NoMacCatalyst]
#elif __MACCATALYST__
		[Obsolete ("Not available on the current platform.")]
#endif
		[NoTV, NoMac]
		iOS_GPUFamily1_v2 = 2,
		/// <summary>The iOS GPU Family 2 v1 feature set.</summary>
#if XAMCORE_5_0
		[NoMacCatalyst]
#elif __MACCATALYST__
		[Obsolete ("Not available on the current platform.")]
#endif
		[NoTV, NoMac]
		iOS_GPUFamily2_v1 = 1,
		/// <summary>The iOS GPU Family 2 v2 feature set.</summary>
#if XAMCORE_5_0
		[NoMacCatalyst]
#elif __MACCATALYST__
		[Obsolete ("Not available on the current platform.")]
#endif
		[NoTV, NoMac]
		iOS_GPUFamily2_v2 = 3,
		/// <summary>The iOS GPU Family 3 v1 feature set.</summary>
#if XAMCORE_5_0
		[NoMacCatalyst]
#elif __MACCATALYST__
		[Obsolete ("Not available on the current platform.")]
#endif
		[NoTV, NoMac]
		iOS_GPUFamily3_v1 = 4,
		/// <summary>The iOS GPU Family 1 v3 feature set.</summary>
		[NoTV, NoMac, NoMacCatalyst]
		iOS_GPUFamily1_v3 = 5,
		/// <summary>The iOS GPU Family 2 v3 feature set.</summary>
		[NoTV, NoMac, NoMacCatalyst]
		iOS_GPUFamily2_v3 = 6,
		/// <summary>The iOS GPU Family 3 v2 feature set.</summary>
		[NoTV, NoMac, NoMacCatalyst]
		iOS_GPUFamily3_v2 = 7,
		/// <summary>The iOS GPU Family 1 v4 feature set.</summary>
		[NoTV, NoMac, NoMacCatalyst]
		iOS_GPUFamily1_v4 = 8,
		/// <summary>The iOS GPU Family 2 v4 feature set.</summary>
		[NoTV, NoMac, NoMacCatalyst]
		iOS_GPUFamily2_v4 = 9,
		/// <summary>The iOS GPU Family 3 v3 feature set.</summary>
		[NoTV, NoMac, NoMacCatalyst]
		iOS_GPUFamily3_v3 = 10,
		/// <summary>The iOS GPU Family 4 v1 feature set.</summary>
		[NoTV, NoMac, NoMacCatalyst]
		iOS_GPUFamily4_v1 = 11,

		/// <summary>To be added.</summary>
		[NoTV, NoMac, NoMacCatalyst]
		iOS_GPUFamily1_v5 = 12,
		/// <summary>To be added.</summary>
		[NoTV, NoMac, NoMacCatalyst]
		iOS_GPUFamily2_v5 = 13,
		/// <summary>To be added.</summary>
		[NoTV, NoMac, NoMacCatalyst]
		iOS_GPUFamily3_v4 = 14,
		/// <summary>To be added.</summary>
		[NoTV, NoMac, NoMacCatalyst]
		iOS_GPUFamily4_v2 = 15,
		/// <summary>To be added.</summary>
		[NoTV, NoMac, NoMacCatalyst]
		iOS_GPUFamily5_v1 = 16,

		/// <summary>The macOS GPU Family 1 v1 feature set.</summary>
		[NoiOS, NoTV, NoMacCatalyst]
		macOS_GPUFamily1_v1 = 10000,

		/// <summary>The macOS GPU Family 1 v2 feature set.</summary>
		[NoiOS, NoTV]
		[NoMacCatalyst]
		macOS_GPUFamily1_v2 = 10001,

		/// <summary>The macOS read-write texture, tier 2 feature set.</summary>
		[NoiOS, NoTV]
		[NoMacCatalyst]
		macOS_ReadWriteTextureTier2 = 10002,

		/// <summary>The macOS GPU Family 1 v3 feature set.</summary>
		[NoiOS, NoTV]
		[NoMacCatalyst]
		macOS_GPUFamily1_v3 = 10003,

		[NoiOS, NoTV]
		[NoMacCatalyst]
		macOS_GPUFamily1_v4 = 10004,

		[NoiOS, NoTV]
		[NoMacCatalyst]
		macOS_GPUFamily2_v1 = 10005,

		/// <summary>The tvOS GPU Family 1 v1 feature set.</summary>
#if XAMCORE_5_0
		[NoMacCatalyst]
#elif __MACCATALYST__
		[Obsolete ("Not available on the current platform.")]
#endif
		[NoiOS, NoMac]
		tvOS_GPUFamily1_v1 = 30000,

		/// <summary>The TVOS GPU Family 1 v2 feature set.</summary>
		[NoiOS, NoMac]
		[NoMacCatalyst]
		tvOS_GPUFamily1_v2 = 30001,

		[NoiOS, NoMac]
		[NoMacCatalyst]
		tvOS_GPUFamily1_v3 = 30002,

		/// <summary>The TVOS GPU Family 2 v1 feature set.</summary>
		[NoiOS, NoMac]
		[NoMacCatalyst]
		tvOS_GPUFamily2_v1 = 30003,

		/// <summary>To be added.</summary>
		[NoiOS, NoMac]
		[NoMacCatalyst]
		tvOS_GPUFamily1_v4 = 30004,
		[NoiOS, NoMac]
		[NoMacCatalyst]
		tvOS_GPUFamily2_v2 = 30005,
	}

	/// <summary>Enumerates Metal shader programming language versions.</summary>
	[MacCatalyst (13, 1)]
	[Native]
	public enum MTLLanguageVersion : ulong {
		/// <summary>Indicates version 1.0.</summary>
		[NoMac]
		[NoMacCatalyst]
		v1_0 = (1 << 16),
		/// <summary>Indicates version 1.1.</summary>
		v1_1 = (1 << 16) + 1,
		/// <summary>Indicates version 1.2.</summary>
		[MacCatalyst (13, 1)]
		v1_2 = (1 << 16) + 2,
		/// <summary>Indicates version 2.0.</summary>
		[MacCatalyst (13, 1)]
		v2_0 = (2 << 16),
		/// <summary>To be added.</summary>
		[MacCatalyst (13, 1)]
		v2_1 = (2 << 16) + 1,
		[iOS (13, 0), TV (13, 0)]
		[MacCatalyst (13, 1)]
		v2_2 = (2 << 16) + 2,
		[iOS (14, 0), TV (14, 0)]
		[MacCatalyst (14, 0)]
		v2_3 = (2 << 16) + 3,
		[iOS (15, 0), TV (15, 0), MacCatalyst (15, 0)]
		v2_4 = (2uL << 16) + 4,
		[iOS (16, 0), TV (16, 0), MacCatalyst (16, 0), Mac (13, 0)]
		v3_0 = (3uL << 16) + 0,
		[iOS (17, 0), TV (17, 0), MacCatalyst (17, 0), Mac (14, 0)]
		v3_1 = (3uL << 16) + 1,
		[TV (18, 0), Mac (15, 0), iOS (18, 0), MacCatalyst (18, 0)]
		v3_2 = (3ul << 16) + 2,
	}

	/// <summary>Enumerates values that indicate whether to clip or clamp depth values.</summary>
	[MacCatalyst (13, 1)]
	[Native]
	public enum MTLDepthClipMode : ulong {
		/// <summary>To be added.</summary>
		Clip = 0,
		/// <summary>To be added.</summary>
		Clamp = 1,
	}

	/// <summary>Enumerates blitting options.</summary>
	[MacCatalyst (13, 1)]
	[Native]
	[Flags]
	public enum MTLBlitOption : ulong {
		/// <summary>Indicates that no blit option was specified.</summary>
		None = 0,
		/// <summary>Indicates that the depth attachment part of a depth/stencil resource will be blitted.</summary>
		DepthFromDepthStencil = 1 << 0,
		/// <summary>Indicates that the stencil attachment part of a depth/stencil resource will be blitted.</summary>
		StencilFromDepthStencil = 1 << 1,
		/// <summary>Indicates that compressed row-major, linearly arranged PVRTC texture data will be blitted.</summary>
		[NoMac]
		[MacCatalyst (13, 1)]
		RowLinearPvrtc = 1 << 2,
	}

	/// <summary>Enumerates CPU cache modes for resource mapping.</summary>
	[MacCatalyst (13, 1)]
	[Native]
	public enum MTLStorageMode : ulong {
		/// <summary>Indicates that both the CPU and GPU can access the location of the resource.</summary>
		Shared = 0,
		/// <summary>To be added.</summary>
		[NoiOS]
		[NoTV]
		[NoMacCatalyst]
		Managed = 1,
		/// <summary>Indicates that only the GPU can access the location of the resource.</summary>
		Private = 2,
		/// <summary>Indicates the memory mode for an unbacked temporary render target.</summary>
		[NoMac]
		[MacCatalyst (13, 1)]
		Memoryless = 3,
	}

	/// <summary>Enumerates MSAA dept resolve filter operations.</summary>
	[Native]
	public enum MTLMultisampleDepthResolveFilter : ulong {
		/// <summary>Indicates that no filter is applied. Default.</summary>
		Sample0,
		/// <summary>Indicates that a minimum filter is applied, selecting the minimum depth sample for each pixel.</summary>
		Min,
		/// <summary>Indicates that a maximum filter is applied, selecting the largest depth sample for each pixel.</summary>
		Max,
	}

	[TV (16, 0)]
	[iOS (14, 0)]
	[MacCatalyst (14, 0)]
	[Native]
	public enum MTLSamplerBorderColor : ulong {
		/// <summary>To be added.</summary>
		TransparentBlack = 0,
		/// <summary>To be added.</summary>
		OpaqueBlack = 1,
		/// <summary>To be added.</summary>
		OpaqueWhite = 2,
	}

	[TV (13, 0)]
	[MacCatalyst (13, 1)]
	[Native]
	public enum MTLPrimitiveTopologyClass : ulong {
		/// <summary>To be added.</summary>
		Unspecified = 0,
		/// <summary>To be added.</summary>
		Point = 1,
		/// <summary>To be added.</summary>
		Line = 2,
		/// <summary>To be added.</summary>
		Triangle = 3,
	}

	[MacCatalyst (13, 1)]
	[Native]
	public enum MTLTessellationPartitionMode : ulong {
		/// <summary>To be added.</summary>
		Pow2 = 0,
		/// <summary>To be added.</summary>
		Integer = 1,
		/// <summary>To be added.</summary>
		FractionalOdd = 2,
		/// <summary>To be added.</summary>
		FractionalEven = 3,
	}

	[MacCatalyst (13, 1)]
	[Native]
	public enum MTLTessellationFactorFormat : ulong {
		/// <summary>To be added.</summary>
		Half = 0,
	}

	[MacCatalyst (13, 1)]
	[Native]
	public enum MTLTessellationControlPointIndexType : ulong {
		/// <summary>To be added.</summary>
		None = 0,
		/// <summary>To be added.</summary>
		UInt16 = 1,
		/// <summary>To be added.</summary>
		UInt32 = 2,
	}

	[MacCatalyst (13, 1)]
	[Native]
	public enum MTLTessellationFactorStepFunction : ulong {
		/// <summary>To be added.</summary>
		Constant = 0,
		/// <summary>To be added.</summary>
		PerPatch = 1,
		/// <summary>To be added.</summary>
		PerInstance = 2,
		/// <summary>To be added.</summary>
		PerPatchAndPerInstance = 3,
	}

	[MacCatalyst (13, 1)]
	[Native]
	public enum MTLPatchType : ulong {
		/// <summary>To be added.</summary>
		None = 0,
		/// <summary>To be added.</summary>
		Triangle = 1,
		/// <summary>To be added.</summary>
		Quad = 2,
	}

	/// <summary>Enumerates formats for function attributes.</summary>
	[MacCatalyst (13, 1)]
	[Native]
	public enum MTLAttributeFormat : ulong {
		/// <summary>Indicates an invalid format.</summary>
		Invalid = 0,
		/// <summary>Indicates two unsigned 8-bit characters.</summary>
		UChar2 = 1,
		/// <summary>Indicates three unsigned 8-bit characters.</summary>
		UChar3 = 2,
		/// <summary>Indicates four unsigned 8-bit characters..</summary>
		UChar4 = 3,
		/// <summary>Indicates two signed 8-bit characters.</summary>
		Char2 = 4,
		/// <summary>Indicates three signed 8-bit characters.</summary>
		Char3 = 5,
		/// <summary>Indicates four signed 8-bit characters.</summary>
		Char4 = 6,
		/// <summary>Indicates two normalized unsigned 8-bit characters.</summary>
		UChar2Normalized = 7,
		/// <summary>Indicates three normalized unsigned 8-bit characters.</summary>
		UChar3Normalized = 8,
		/// <summary>Indicates four normalized unsigned 8-bit characters.</summary>
		UChar4Normalized = 9,
		/// <summary>Indicates two normalized signed 8-bit characters.</summary>
		Char2Normalized = 10,
		/// <summary>Indicates three normalized signed 8-bit characters.</summary>
		Char3Normalized = 11,
		/// <summary>Indicates four normalized signed 8-bit characters.</summary>
		Char4Normalized = 12,
		/// <summary>Indicates two unsigned 16-bit two's complement values.</summary>
		UShort2 = 13,
		/// <summary>Indicates three unsigned 16-bit two's complement values.</summary>
		UShort3 = 14,
		/// <summary>Indicates four unsigned 16-bit two's complement values.</summary>
		UShort4 = 15,
		/// <summary>Indicates two signed 16-bit two's complement value.</summary>
		Short2 = 16,
		/// <summary>Indicates three signed 16-bit two's complement values.</summary>
		Short3 = 17,
		/// <summary>Indicates four signed 16-bit two's complement values.</summary>
		Short4 = 18,
		/// <summary>Indicates two normalized unsigned 16-bit two's complement values.</summary>
		UShort2Normalized = 19,
		/// <summary>Indicates three normalized unsigned 16-bit two's complement values.</summary>
		UShort3Normalized = 20,
		/// <summary>Indicates four normalized unsigned 16-bit two's complement values.</summary>
		UShort4Normalized = 21,
		/// <summary>Indicates two normalized signed 16-bit two's complement values.</summary>
		Short2Normalized = 22,
		/// <summary>Indicates three normalized signed 16-bit two's complement values.</summary>
		Short3Normalized = 23,
		/// <summary>Indicates four normalized signed 16-bit two's complement values.</summary>
		Short4Normalized = 24,
		/// <summary>Indicates two half-precision floating point values.</summary>
		Half2 = 25,
		/// <summary>Indicates three half-precision floating point values.</summary>
		Half3 = 26,
		/// <summary>Indicates four half-precision floating point values.</summary>
		Half4 = 27,
		/// <summary>Indicates one single-precision floating point value.</summary>
		Float = 28,
		/// <summary>Indicates two single-precision floating point values.</summary>
		Float2 = 29,
		/// <summary>Indicates three single-precision floating point values.</summary>
		Float3 = 30,
		/// <summary>Indicates four single-precision floating point values.</summary>
		Float4 = 31,
		/// <summary>Indicates a single signed 32-bit two's complement value.</summary>
		Int = 32,
		/// <summary>Indicates two signed 32-bit two's complement values.</summary>
		Int2 = 33,
		/// <summary>Indicates three signed 32-bit two's complement values.</summary>
		Int3 = 34,
		/// <summary>Indicates four signed 32-bit two's complement values.</summary>
		Int4 = 35,
		/// <summary>Indicates a single unsigned 32-bit two's complement value.</summary>
		UInt = 36,
		/// <summary>Indicates two unsigned 32-bit two's complement values.</summary>
		UInt2 = 37,
		/// <summary>Indicates three unsigned 32-bit two's complement values.</summary>
		UInt3 = 38,
		/// <summary>Indicates four unsigned 32-bit two's complement values.</summary>
		UInt4 = 39,
		/// <summary>Indicates a 10-10-10-2 scheme of packed normalized signed integers.</summary>
		Int1010102Normalized = 40,
		/// <summary>To be added.</summary>
		UInt1010102Normalized = 41,
		/// <summary>Indicates four unsigned 8-bit characters that describe BGRA channels.</summary>
		[MacCatalyst (13, 1)]
		UChar4Normalized_Bgra = 42,
		/// <summary>Indicates a single unsigned 8-bit character.</summary>
		[MacCatalyst (13, 1)]
		UChar = 45,
		/// <summary>Indicates a single signed 8-bit character.</summary>
		[MacCatalyst (13, 1)]
		Char = 46,
		/// <summary>Indicates a single unsigned 8-bit character.</summary>
		[MacCatalyst (13, 1)]
		UCharNormalized = 47,
		/// <summary>Indicates a single normalized signed 8-bit character.</summary>
		[MacCatalyst (13, 1)]
		CharNormalized = 48,
		/// <summary>Indicates a single unsigned 16-bit two's complement value.</summary>
		[MacCatalyst (13, 1)]
		UShort = 49,
		/// <summary>Indicates a single signed 16-bit two's complement value.</summary>
		[MacCatalyst (13, 1)]
		Short = 50,
		/// <summary>Indicates a single normalized unsigned 16-bit two's complement value.</summary>
		[MacCatalyst (13, 1)]
		UShortNormalized = 51,
		/// <summary>Indicates a single normalized signed 16-bit two's complement value.</summary>
		[MacCatalyst (13, 1)]
		ShortNormalized = 52,
		/// <summary>Indicates a single half-precision floating point value.</summary>
		[MacCatalyst (13, 1)]
		Half = 53,
		[Mac (14, 0), iOS (17, 0), TV (17, 0), MacCatalyst (17, 0)]
		FloatRG11B10 = 54,
		[Mac (14, 0), iOS (17, 0), TV (17, 0), MacCatalyst (17, 0)]
		FloatRGB9E5 = 55,
	}

	[MacCatalyst (13, 1)]
	[Native]
	public enum MTLStepFunction : ulong {
		/// <summary>To be added.</summary>
		Constant = 0,
		/// <summary>To be added.</summary>
		PerVertex = 1,
		/// <summary>To be added.</summary>
		PerInstance = 2,
		/// <summary>To be added.</summary>
		PerPatch = 3,
		/// <summary>To be added.</summary>
		PerPatchControlPoint = 4,
		/// <summary>To be added.</summary>
		ThreadPositionInGridX = 5,
		/// <summary>To be added.</summary>
		ThreadPositionInGridY = 6,
		/// <summary>To be added.</summary>
		ThreadPositionInGridXIndexed = 7,
		/// <summary>To be added.</summary>
		ThreadPositionInGridYIndexed = 8,
	}

	[MacCatalyst (13, 1)]
	[Native]
	public enum MTLRenderStages : ulong {
		/// <summary>To be added.</summary>
		Vertex = (1 << 0),
		/// <summary>To be added.</summary>
		Fragment = (1 << 1),
		[iOS (15, 0), TV (15, 0), MacCatalyst (15, 0)]
		Tile = (1uL << 2),

		[iOS (16, 0), TV (16, 0), Mac (13, 0), MacCatalyst (16, 0)]
		Object = (1uL << 3),
		[iOS (16, 0), TV (16, 0), Mac (13, 0), MacCatalyst (16, 0)]
		Mesh = (1uL << 4),
	}

	/// <summary>Enumerates resource usage patterns.</summary>
	[MacCatalyst (13, 1)]
	[Native, Flags]
	public enum MTLResourceUsage : ulong {
		/// <summary>Indicates reading operations.</summary>
		Read = 1 << 0,
		/// <summary>Indicate writing operations.</summary>
		Write = 1 << 1,
		/// <summary>Indicates sampling operations.</summary>
		Sample = 1 << 2,
	}

	/// <summary>Enumerates buffer mutability descriptions.</summary>
	[MacCatalyst (13, 1)]
	[Native]
	public enum MTLMutability : ulong {
		/// <summary>Indicates the default mutability of the buffer, by tupe.</summary>
		Default = 0,
		/// <summary>Indicates a mutable buffer.</summary>
		Mutable = 1,
		/// <summary>Indicates an immutable buffer.</summary>
		Immutable = 2,
	}

	[MacCatalyst (13, 1)]
	[Native]
	public enum MTLReadWriteTextureTier : ulong {
		/// <summary>To be added.</summary>
		None = 0,
		/// <summary>To be added.</summary>
		One = 1,
		/// <summary>To be added.</summary>
		Two = 2,
	}

	/// <summary>Enumerates argument buffer tiers.</summary>
	[MacCatalyst (13, 1)]
	[Native]
	public enum MTLArgumentBuffersTier : ulong {
		/// <summary>Indicates tier one.</summary>
		One = 0,
		/// <summary>Indicates tier two.</summary>
		Two = 1,
	}

	/// <summary>Enumerates store action options.</summary>
	[MacCatalyst (13, 1)]
	[Native, Flags]
	public enum MTLStoreActionOptions : ulong {
		/// <summary>Indicates no store action modification.</summary>
		None = 0,
		/// <summary>Indicates that data will be stored in a way that does not consider the sample position.</summary>
		CustomSamplePositions = 1 << 0,
	}

	[MacCatalyst (13, 1)]
	[Native]
	public enum MTLDispatchType : ulong {
		/// <summary>To be added.</summary>
		Serial,
		/// <summary>To be added.</summary>
		Concurrent,
	}

	[Flags]
	[MacCatalyst (13, 1)]
	[Native]
	public enum MTLBarrierScope : ulong {
		/// <summary>To be added.</summary>
		Buffers = 1 << 0,
		/// <summary>To be added.</summary>
		Textures = 1 << 1,
		/// <summary>To be added.</summary>
		[NoiOS, NoTV]
		[NoMacCatalyst]
		RenderTargets = 1 << 2,
	}

	[Flags]
	[MacCatalyst (13, 1)]
	[Native]
	public enum MTLIndirectCommandType : ulong {
		/// <summary>To be added.</summary>
		Draw = 1 << 0,
		/// <summary>To be added.</summary>
		DrawIndexed = 1 << 1,
		/// <summary>To be added.</summary>
		[TV (14, 5)]
		[MacCatalyst (13, 1)]
		DrawPatches = 1 << 2,
		/// <summary>To be added.</summary>
		[TV (14, 5)]
		[MacCatalyst (13, 1)]
		DrawIndexedPatches = 1 << 3,
		[iOS (13, 0), TV (13, 0)]
		[MacCatalyst (13, 1)]
		ConcurrentDispatch = 1 << 5,
		[iOS (13, 0), TV (13, 0)]
		[MacCatalyst (13, 1)]
		ConcurrentDispatchThreads = 1 << 6,
		[NoMac, iOS (17, 0), TV (18, 1), MacCatalyst (17, 0)]
		DrawMeshThreadgroups = (1uL << 7),
		[NoMac, iOS (17, 0), TV (18, 1), MacCatalyst (17, 0)]
		DrawMeshThreads = (1uL << 8),
	}

	[TV (14, 5)]
	[MacCatalyst (13, 1)]
	[Native]
	public enum MTLMultisampleStencilResolveFilter : ulong {
		/// <summary>To be added.</summary>
		Sample0 = 0,
		/// <summary>To be added.</summary>
		DepthResolvedSample = 1,
	}


	[Flags, TV (17, 0), iOS (13, 0)]
	[MacCatalyst (13, 1)]
	[Native]
	public enum MTLSparseTextureRegionAlignmentMode : ulong {
		Outward = 0x0,
		Inward = 0x1,
	}

	[Flags, TV (17, 0), iOS (13, 0)]
	[MacCatalyst (14, 0)]
	[Native]
	public enum MTLSparseTextureMappingMode : ulong {
		Map = 0x0,
		Unmap = 0x1,
	}

	[iOS (13, 0)]
	[MacCatalyst (13, 1)]
	[Native]
	public enum MTLHazardTrackingMode : ulong {
		Default = 0,
		Untracked = 1,
		Tracked = 2,
	}

	[TV (13, 0), iOS (13, 0)]
	[MacCatalyst (13, 1)]
	[Native]
	[ErrorDomain ("MTLCaptureErrorDomain")]
	public enum MTLCaptureError : long {
		NotSupported = 1,
		AlreadyCapturing,
		InvalidDescriptor,
	}

	[TV (13, 0), iOS (13, 0)]
	[MacCatalyst (13, 1)]
	[Native]
	public enum MTLGpuFamily : long {
		Apple1 = 1001,
		Apple2 = 1002,
		Apple3 = 1003,
		Apple4 = 1004,
		Apple5 = 1005,
		[NoTV, iOS (14, 0)]
		[MacCatalyst (14, 0)]
		Apple6 = 1006,
		[NoTV, iOS (14, 0)]
		[MacCatalyst (14, 0)]
		Apple7 = 1007,
		Apple8 = 1008,
		[NoTV]
		Apple9 = 1009,
		Mac1 = 2001,
		Mac2 = 2002,
		Common1 = 3001,
		Common2 = 3002,
		Common3 = 3003,
		iOSMac1 = 4001,
		iOSMac2 = 4002,

		[iOS (16, 0), TV (16, 0), MacCatalyst (16, 0), Mac (13, 0)]
		Metal3 = 5001,
	}

	[TV (13, 0), iOS (13, 0)]
	[MacCatalyst (13, 1)]
	[Native]
	public enum MTLHeapType : long {
		Automatic = 0,
		Placement = 1,
		[NoTV]
		[MacCatalyst (13, 1)]
		Sparse = 2,
	}

	[iOS (13, 0), TV (13, 0)]
	[MacCatalyst (13, 1)]
	[Native]
	public enum MTLCaptureDestination : long {
		DeveloperTools = 1,
		GpuTraceDocument,
	}

	[NoiOS, NoTV]
	[NoMacCatalyst]
	[Native]
	public enum MTLDeviceLocation : ulong {
		BuiltIn = 0,
		Slot = 1,
		External = 2,
		Unspecified = ulong.MaxValue,
	}

	[iOS (14, 0), TV (14, 0)]
	[MacCatalyst (14, 0)]
	[Native]
	[ErrorDomain ("MTLCounterErrorDomain")]
	public enum MTLCounterSampleBufferError : long {
		OutOfMemory,
		Invalid = 1,
		Internal = 2,
	}

	[iOS (14, 0), TV (14, 0)]
	[MacCatalyst (14, 0)]
	public enum MTLCommonCounter {
		[Field ("MTLCommonCounterTimestamp")]
		Timestamp,

		[Field ("MTLCommonCounterTessellationInputPatches")]
		TessellationInputPatches,

		[Field ("MTLCommonCounterVertexInvocations")]
		VertexInvocations,

		[Field ("MTLCommonCounterPostTessellationVertexInvocations")]
		PostTessellationVertexInvocations,

		[Field ("MTLCommonCounterClipperInvocations")]
		ClipperInvocations,

		[Field ("MTLCommonCounterClipperPrimitivesOut")]
		ClipperPrimitivesOut,

		[Field ("MTLCommonCounterFragmentInvocations")]
		FragmentInvocations,

		[Field ("MTLCommonCounterFragmentsPassed")]
		FragmentsPassed,

		[Field ("MTLCommonCounterComputeKernelInvocations")]
		ComputeKernelInvocations,

		[Field ("MTLCommonCounterTotalCycles")]
		TotalCycles,

		[Field ("MTLCommonCounterVertexCycles")]
		VertexCycles,

		[Field ("MTLCommonCounterTessellationCycles")]
		TessellationCycles,

		[Field ("MTLCommonCounterPostTessellationVertexCycles")]
		PostTessellationVertexCycles,

		[Field ("MTLCommonCounterFragmentCycles")]
		FragmentCycles,

		[Field ("MTLCommonCounterRenderTargetWriteCycles")]
		RenderTargetWriteCycles,

		[Field ("MTLCommonCounterSetTimestamp")]
		SetTimestamp,

		[Field ("MTLCommonCounterSetStageUtilization")]
		SetStageUtilization,

		[Field ("MTLCommonCounterSetStatistic")]
		SetStatistic,
	}

	[Flags, iOS (14, 0), TV (14, 0)]
	[MacCatalyst (14, 0)]
	public enum MTLAccelerationStructureInstanceOptions : uint {
		None = 0x0,
		DisableTriangleCulling = (1u << 0),
		TriangleFrontFacingWindingCounterClockwise = (1u << 1),
		Opaque = (1u << 2),
		NonOpaque = (1u << 3),
	}

	[iOS (14, 0), TV (16, 0)]
	[MacCatalyst (14, 0)]
	[Flags]
	[Native]
	public enum MTLAccelerationStructureUsage : ulong {
		None = 0x0,
		Refit = (1uL << 0),
		PreferFastBuild = (1uL << 1),
		[iOS (15, 0), MacCatalyst (15, 0)]
		ExtendedLimits = (1uL << 2),
	}

	[iOS (14, 0), TV (14, 0)]
	[MacCatalyst (14, 0)]
	[ErrorDomain ("MTLBinaryArchiveDomain")]
	[Native]
	public enum MTLBinaryArchiveError : ulong {
		None = 0,
		InvalidFile = 1,
		UnexpectedElement = 2,
		CompilationFailure = 3,
		InternalError = 4,
	}

	[iOS (14, 0), TV (14, 0)]
	[MacCatalyst (14, 0)]
	[Flags]
	[Native]
	public enum MTLCommandBufferErrorOption : ulong {
		None = 0x0,
		EncoderExecutionStatus = 1uL << 0,
	}

	[iOS (14, 0), TV (14, 0)]
	[MacCatalyst (14, 0)]
	[Native]
	public enum MTLCommandEncoderErrorState : long {
		Unknown = 0,
		Completed = 1,
		Affected = 2,
		Pending = 3,
		Faulted = 4,
	}

	[iOS (14, 0), TV (14, 0)]
	[MacCatalyst (14, 0)]
	[Native]
	public enum MTLCounterSamplingPoint : ulong {
		StageBoundary,
		DrawBoundary,
		DispatchBoundary,
		TileDispatchBoundary,
		BlitBoundary,
	}

	[iOS (14, 0), TV (14, 0)]
	[MacCatalyst (14, 0)]
	[ErrorDomain ("MTLDynamicLibraryDomain")]
	[Native]
	public enum MTLDynamicLibraryError : ulong {
		None = 0,
		InvalidFile = 1,
		CompilationFailure = 2,
		UnresolvedInstallName = 3,
		DependencyLoadFailure = 4,
		Unsupported = 5,
	}

	[iOS (14, 0), TV (14, 0)]
	[MacCatalyst (14, 0)]
	[Native]
	public enum MTLFunctionLogType : ulong {
		Validation = 0,
	}

	[Flags, iOS (14, 0), TV (14, 0)]
	[MacCatalyst (14, 0)]
	[Native]
	public enum MTLFunctionOptions : ulong {
		None = 0x0,
		[NoTV]
		[MacCatalyst (14, 0)]
		CompileToBinary = 1uL << 0,
		[iOS (17, 0), TV (17, 0), MacCatalyst (17, 0), Mac (14, 0)]
		[Deprecated (PlatformName.MacCatalyst, 18, 0, message: "Use 'StoreFunctionInMetalPipelinesScript' instead.")]
		[Deprecated (PlatformName.iOS, 18, 0, message: "Use 'StoreFunctionInMetalPipelinesScript' instead.")]
		[Deprecated (PlatformName.TvOS, 18, 0, message: "Use 'StoreFunctionInMetalPipelinesScript' instead.")]
		[Deprecated (PlatformName.MacOSX, 15, 0, message: "Use 'StoreFunctionInMetalPipelinesScript' instead.")]
		StoreFunctionInMetalScript = 1uL << 1,
		[TV (18, 0), Mac (15, 0), iOS (18, 0), MacCatalyst (18, 0)]
		StoreFunctionInMetalPipelinesScript = 1 << 1,
		[TV (18, 0), Mac (15, 0), iOS (18, 0), MacCatalyst (18, 0)]
		FailOnBinaryArchiveMiss = 1 << 2,
	}

	[Flags, iOS (14, 0), TV (16, 0)]
	[MacCatalyst (14, 0)]
	[Native]
	public enum MTLIntersectionFunctionSignature : ulong {
		None = 0x0,
		Instancing = (1uL << 0),
		TriangleData = (1uL << 1),
		WorldSpaceData = (1uL << 2),
		[iOS (15, 0), MacCatalyst (15, 0)]
		InstanceMotion = (1uL << 3),
		[iOS (15, 0), MacCatalyst (15, 0)]
		PrimitiveMotion = (1uL << 4),
		[iOS (15, 0), MacCatalyst (15, 0)]
		ExtendedLimits = (1uL << 5),
		[Mac (14, 0), iOS (17, 0), TV (17, 0), MacCatalyst (17, 0)]
		MaxLevels = (1uL << 6),
		[Mac (14, 0), iOS (17, 0), TV (17, 0), MacCatalyst (17, 0)]
		CurveData = (1uL << 7),
	}

	[iOS (14, 0), TV (14, 0)]
	[MacCatalyst (14, 0)]
	[Native]
	public enum MTLLibraryType : long {
		Executable = 0,
		Dynamic = 1,
	}

	[iOS (13, 0), TV (13, 0)]
	[MacCatalyst (13, 1)]
	public enum MTLTextureSwizzle : byte {
		Zero = 0,
		One = 1,
		Red = 2,
		Green = 3,
		Blue = 4,
		Alpha = 5,
	}

	[iOS (15, 0), MacCatalyst (15, 0), TV (17, 0)]
	public enum MTLMotionBorderMode : uint {
		Clamp = 0,
		Vanish = 1,
	}

	[iOS (15, 0), MacCatalyst (15, 0), TV (16, 0)]
	[Native]
	public enum MTLAccelerationStructureInstanceDescriptorType : ulong {
		Default = 0,
		UserID = 1,
		Motion = 2,
		[Mac (14, 0), iOS (17, 0), MacCatalyst (17, 0), TV (17, 0)]
		Indirect = 3,
		[Mac (14, 0), iOS (17, 0), MacCatalyst (17, 0), TV (17, 0)]
		IndirectMotion = 4,
	}

	[Mac (12, 5), iOS (15, 0), MacCatalyst (15, 0), TV (17, 0)]
	[Native]
	public enum MTLTextureCompressionType : long {
		Lossless = 0,
		Lossy = 1,
	}

	[Flags, Mac (13, 0), iOS (16, 0), MacCatalyst (16, 0), TV (16, 0)]
	[Native]
	public enum MTLAccelerationStructureRefitOptions : ulong {
		VertexData = (1uL << 0),
		PerPrimitiveData = (1uL << 1),
	}

	[iOS (14, 0), MacCatalyst (14, 0), TV (14, 0)]
	[Native]
	public enum MTLBindingType : long {
		Buffer = 0,
		ThreadgroupMemory = 1,
		Texture = 2,
		Sampler = 3,
		ImageblockData = 16,
		Imageblock = 17,
		VisibleFunctionTable = 24,
		PrimitiveAccelerationStructure = 25,
		InstanceAccelerationStructure = 26,
		IntersectionFunctionTable = 27,
		ObjectPayload = 34,
	}

	[Mac (13, 0), iOS (16, 0), MacCatalyst (16, 0), TV (16, 0)]
	[Native]
	public enum MTLIOCommandQueueType : long {
		Concurrent = 0,
		Serial = 1,
	}

	[Mac (13, 0), iOS (16, 0), MacCatalyst (16, 0), TV (16, 0)]
	[Native]
	public enum MTLIOCompressionMethod : long {
		Zlib = 0,
		Lzfse = 1,
		Lz4 = 2,
		Lzma = 3,
		LzBitmap = 4,
	}

	[Mac (13, 0), iOS (16, 0), MacCatalyst (16, 0), TV (16, 0)]
	[Native]
	public enum MTLIOCompressionStatus : long {
		Complete = 0,
		Error = 1,
	}

	[Mac (13, 0), iOS (16, 0), MacCatalyst (16, 0), TV (16, 0)]
	[Native]
	[ErrorDomain ("MTLIOErrorDomain")]
	public enum MTLIOError : long {
		UrlInvalid = 1,
		Internal = 2,
	}

	[Mac (13, 0), iOS (16, 0), MacCatalyst (16, 0), TV (16, 0)]
	[Native]
	public enum MTLIOPriority : long {
		High = 0,
		Normal = 1,
		Low = 2,
	}

	[Mac (13, 0), iOS (16, 0), MacCatalyst (16, 0), TV (16, 0)]
	[Native]
	public enum MTLIOStatus : long {
		Pending = 0,
		Cancelled = 1,
		Error = 2,
		Complete = 3,
	}

	[Mac (13, 0), iOS (16, 0), MacCatalyst (16, 0), TV (16, 0)]
	[Native]
	public enum MTLLibraryOptimizationLevel : long {
		Default = 0,
		Size = 1,
	}

	[Mac (13, 0), iOS (16, 0), MacCatalyst (16, 0), TV (16, 0)]
	[Native]
	public enum MTLSparsePageSize : long {
		Size16 = 101,
		Size64 = 102,
		Size256 = 103,
	}

	[Mac (13, 0), iOS (16, 0), MacCatalyst (16, 0), TV (16, 0)]
	[Native]
	public enum MTLBindingAccess : long {
		ReadOnly = 0,
		ReadWrite = 1,
		WriteOnly = 2,
	}

	[Mac (14, 0), iOS (17, 0), MacCatalyst (17, 0), TV (17, 0)]
	[Native]
	public enum MTLCurveType : long {
		Round = 0,
		Flat = 1,
	}

	[Mac (14, 0), iOS (17, 0), MacCatalyst (17, 0), TV (17, 0)]
	[Native]
	public enum MTLCurveBasis : long {
		BSpline = 0,
		CatmullRom = 1,
		Linear = 2,
		Bezier = 3,
	}

	[Mac (14, 0), iOS (17, 0), MacCatalyst (17, 0), TV (17, 0)]
	[Native]
	public enum MTLCurveEndCaps : long {
		None = 0,
		Disk = 1,
		Sphere = 2,
	}

	[Mac (13, 3), iOS (16, 4), MacCatalyst (16, 4), TV (16, 4)]
	[Native]
	public enum MTLCompileSymbolVisibility : long {
		Default = 0,
		Hidden = 1,
	}

}
