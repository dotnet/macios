// 
// CVPixelFormatType.cs
//
// Authors: Mono Team
//     
// Copyright 2011 Novell, Inc
// Copyright 2011-2014, 2016 Xamarin Inc
// Copyright 2020 Microsoft Corporation
//
// Permission is hereby granted, free of charge, to any person obtaining
// a copy of this software and associated documentation files (the
// "Software"), to deal in the Software without restriction, including
// without limitation the rights to use, copy, modify, merge, publish,
// distribute, sublicense, and/or sell copies of the Software, and to
// permit persons to whom the Software is furnished to do so, subject to
// the following conditions:
// 
// The above copyright notice and this permission notice shall be
// included in all copies or substantial portions of the Software.
// 
// THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND,
// EXPRESS OR IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF
// MERCHANTABILITY, FITNESS FOR A PARTICULAR PURPOSE AND
// NONINFRINGEMENT. IN NO EVENT SHALL THE AUTHORS OR COPYRIGHT HOLDERS BE
// LIABLE FOR ANY CLAIM, DAMAGES OR OTHER LIABILITY, WHETHER IN AN ACTION
// OF CONTRACT, TORT OR OTHERWISE, ARISING FROM, OUT OF OR IN CONNECTION
// WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN THE SOFTWARE.
//
using System;
using System.Runtime.InteropServices;
using ObjCRuntime;

#nullable enable

namespace CoreVideo {

	// Note: CoreVideo is not supported in watchOS except for this enum
	// for which ObjC API uses `int` instead of the enum

	// untyped enum, some are 4CC -> CVPixelBuffer.h
	/// <summary>An enumeration of known pixel formats.</summary>
	///     <remarks>To be added.</remarks>
	public enum CVPixelFormatType : uint {
		// FIXME: These all start with integers; what should we do here?
		/// <summary>To be added.</summary>
		CV1Monochrome = 0x00000001,
		/// <summary>To be added.</summary>
		CV2Indexed = 0x00000002,
		/// <summary>To be added.</summary>
		CV4Indexed = 0x00000004,
		/// <summary>To be added.</summary>
		CV8Indexed = 0x00000008,
		/// <summary>To be added.</summary>
		CV1IndexedGray_WhiteIsZero = 0x00000021,
		/// <summary>To be added.</summary>
		CV2IndexedGray_WhiteIsZero = 0x00000022,
		/// <summary>To be added.</summary>
		CV4IndexedGray_WhiteIsZero = 0x00000024,
		/// <summary>To be added.</summary>
		CV8IndexedGray_WhiteIsZero = 0x00000028,
		/// <summary>To be added.</summary>
		CV16BE555 = 0x00000010,
		/// <summary>To be added.</summary>
		CV24RGB = 0x00000018,
		/// <summary>To be added.</summary>
		CV32ARGB = 0x00000020,
		/// <summary>To be added.</summary>
		CV16LE555 = 0x4c353535,
		/// <summary>To be added.</summary>
		CV16LE5551 = 0x35353531,
		/// <summary>To be added.</summary>
		CV16BE565 = 0x42353635,
		/// <summary>To be added.</summary>
		CV16LE565 = 0x4c353635,
		/// <summary>To be added.</summary>
		CV24BGR = 0x32344247,
		/// <summary>To be added.</summary>
		CV32BGRA = 0x42475241,
		/// <summary>To be added.</summary>
		CV32ABGR = 0x41424752,
		/// <summary>To be added.</summary>
		CV32RGBA = 0x52474241,
		/// <summary>To be added.</summary>
		CV64ARGB = 0x62363461,
		/// <summary>To be added.</summary>
		CV48RGB = 0x62343872,
		/// <summary>To be added.</summary>
		CV32AlphaGray = 0x62333261,
		/// <summary>To be added.</summary>
		CV16Gray = 0x62313667,
		/// <summary>30-bit RGB, 10-bit big endian samples, 2 unused padding bits (at most significant end).</summary>
		CV30RGB_r210 = 0x72323130, // 'r210'
		/// <summary>To be added.</summary>
		CV422YpCbCr8 = 0x32767579,
		/// <summary>To be added.</summary>
		CV4444YpCbCrA8 = 0x76343038,
		/// <summary>To be added.</summary>
		CV4444YpCbCrA8R = 0x72343038,
		/// <summary>To be added.</summary>
		CV444YpCbCr8 = 0x76333038,
		/// <summary>To be added.</summary>
		CV422YpCbCr16 = 0x76323136,
		/// <summary>To be added.</summary>
		CV422YpCbCr10 = 0x76323130,
		/// <summary>To be added.</summary>
		CV444YpCbCr10 = 0x76343130,
		/// <summary>To be added.</summary>
		CV420YpCbCr8Planar = 0x79343230,
		/// <summary>To be added.</summary>
		CV420YpCbCr8PlanarFullRange = 0x66343230,
		/// <summary>To be added.</summary>
		CV422YpCbCr_4A_8BiPlanar = 0x61327679,
		/// <summary>To be added.</summary>
		CV420YpCbCr8BiPlanarVideoRange = 0x34323076,
		/// <summary>To be added.</summary>
		CV420YpCbCr8BiPlanarFullRange = 0x34323066,
		/// <summary>To be added.</summary>
		CV422YpCbCr8_yuvs = 0x79757673,
		/// <summary>To be added.</summary>
		CV422YpCbCr8FullRange = 0x79757666,
		/// <summary>To be added.</summary>
		CV30RGB = 0x5231306b,
		/// <summary>To be added.</summary>
		CV4444AYpCbCr8 = 0x79343038,
		/// <summary>To be added.</summary>
		CV4444AYpCbCr16 = 0x79343136,
		// Since 5.1
		/// <summary>To be added.</summary>
		OneComponent8 = 0x4C303038,
		/// <summary>To be added.</summary>
		TwoComponent8 = 0x32433038,
		// Since 6.0
		/// <summary>To be added.</summary>
		OneComponent16Half = 0x4C303068, // 'L00h'
		/// <summary>To be added.</summary>
		OneComponent32Float = 0x4C303066, // 'L00f'
		/// <summary>To be added.</summary>
		TwoComponent16Half = 0x32433068, // '2C0h'
		/// <summary>To be added.</summary>
		TwoComponent32Float = 0x32433066, // '2C0f'
		/// <summary>To be added.</summary>
		CV64RGBAHalf = 0x52476841, // 'RGhA'
		/// <summary>To be added.</summary>
		CV128RGBAFloat = 0x52476641, // 'RGfA'
									 // iOS 10
		/// <summary>To be added.</summary>
		CV30RgbLePackedWideGamut = 0x77333072, // 'w30r'
		/// <summary>To be added.</summary>
		CV14BayerGrbg = 0x67726234, // 'grb4',
		/// <summary>To be added.</summary>
		CV14BayerRggb = 0x72676734, // 'rgg4',
		/// <summary>To be added.</summary>
		CV14BayerBggr = 0x62676734, // 'bgg4',
		/// <summary>To be added.</summary>
		CV14BayerGbrg = 0x67627234, // 'gbr4',
									// iOS 10.3
		/// <summary>To be added.</summary>
		Argb2101010LEPacked = 0x6C313072, // 'l10r'
										  // iOS 11.0
		/// <summary>To be added.</summary>
		DisparityFloat16 = 0x68646973, // hdis
		/// <summary>To be added.</summary>
		DisparityFloat32 = 0x66646973, // fdis
		/// <summary>To be added.</summary>
		DepthFloat16 = 0x68646570, // hdep
		/// <summary>To be added.</summary>
		DepthFloat32 = 0x66646570, // fdep
		/// <summary>To be added.</summary>
		CV420YpCbCr10BiPlanarVideoRange = 0x78343230, // x420
		/// <summary>To be added.</summary>
		CV422YpCbCr10BiPlanarVideoRange = 0x78343232, // x422
		/// <summary>To be added.</summary>
		CV444YpCbCr10BiPlanarVideoRange = 0x78343434, // x444
		/// <summary>To be added.</summary>
		CV420YpCbCr10BiPlanarFullRange = 0x78663230, // xf20
		/// <summary>To be added.</summary>
		CV422YpCbCr10BiPlanarFullRange = 0x78663232, // xf22
		/// <summary>To be added.</summary>
		CV444YpCbCr10BiPlanarFullRange = 0x78663434, // xf44
													 // iOS 14.0
		CV422YpCbCr8BiPlanarVideoRange = 0x34323276, // '422v'
		CV422YpCbCr8BiPlanarFullRange = 0x34323266, // '422f'
		CV444YpCbCr8BiPlanarVideoRange = 0x34343476, // '444v'
		CV444YpCbCr8BiPlanarFullRange = 0x34343466, // '444f'
		OneComponent10 = 0x4c303130, // 'L010'
		OneComponent12 = 0x4c303132, // 'L012'
		OneComponent16 = 0x4c303136, // 'L016'
		TwoComponent16 = 0x32433136, // '2C16'
		CV16VersatileBayer = 0x62703136, // 'bp16'
		CV64Rgba_DownscaledProResRaw = 0x62703634, // 'bp64'
												   // iOS 14.2
		CV64RgbaLE = 0x6C363472,

		/* Lossless pixel formats */

		/// <summary>Lossless-compressed form of <see cref="CV32BGRA" />.</summary>
		Lossless_32BGRA = ('&' << 24) + ('B' << 16) + ('G' << 8) + 'A', // '&BGA'

		/// <summary>Lossless-compressed form of <see cref="CV64RGBAHalf" />. No CVPlanarPixelBufferInfo struct.</summary>
		Lossless_64RGBAHalf = ('&' << 24) + ('R' << 16) + ('h' << 8) + 'A', // '&RhA'

		/// <summary>Lossless-compressed form of <see cref="CV420YpCbCr8BiPlanarVideoRange" />. >No CVPlanarPixelBufferInfo struct.</summary>
		Lossless_420YpCbCr8BiPlanarVideoRange = ('&' << 24) + ('8' << 16) + ('v' << 8) + '0', // '&8v0'

		/// <summary>Lossless-compressed form of <see cref="CV420YpCbCr8BiPlanarFullRange" />. No CVPlanarPixelBufferInfo struct.</summary>
		Lossless_420YpCbCr8BiPlanarFullRange = ('&' << 24) + ('8' << 16) + ('f' << 8) + '0', // '&8f0'

		/// <summary>Lossless-compressed form of <see cref="CV420YpCbCr10BiPlanarVideoRange" />. No CVPlanarPixelBufferInfo struct. Compressed-packed with bo padding bits between pixels.</summary>
		Lossless_420YpCbCr10PackedBiPlanarVideoRange = ('&' << 24) + ('x' << 16) + ('v' << 8) + '0', // '&xv0'

		/// <summary>Lossless-compressed form of <see cref="CV422YpCbCr10BiPlanarVideoRange" />. No CVPlanarPixelBufferInfo struct. Compressed-packed with bo padding bits between pixels.</summary>
		Lossless_422YpCbCr10PackedBiPlanarVideoRange = ('&' << 24) + ('x' << 16) + ('v' << 8) + '2', // '&xv2'

		/// <summary>Lossless-compressed form of <see cref="CV420YpCbCr10BiPlanarFullRange" />. No CVPlanarPixelBufferInfo struct. Compressed-packed with bo padding bits between pixels.</summary>
		Lossless_420YpCbCr10PackedBiPlanarFullRange = ('&' << 24) + ('x' << 16) + ('f' << 8) + '0', // '&xf0'

		/* Lossy pixel formats */

		/// <summary>Lossy-compressed form of <see cref="CV32BGRA" />. No CVPlanarPixelBufferInfo struct.</summary>
		Lossy_32BGRA = ('-' << 24) + ('B' << 16) + ('G' << 8) + 'A', // '-BGA'

		/// <summary>Lossy-compressed form of <see cref="CV420YpCbCr8BiPlanarVideoRange" />. No CVPlanarPixelBufferInfo struct.</summary>
		Lossy_420YpCbCr8BiPlanarVideoRange = ('-' << 24) + ('8' << 16) + ('v' << 8) + '0', // '-8v0'

		/// <summary>Lossy-compressed form of <see cref="CV420YpCbCr8BiPlanarFullRange" />. >No CVPlanarPixelBufferInfo struct.</summary>
		Lossy_420YpCbCr8BiPlanarFullRange = ('-' << 24) + ('8' << 16) + ('f' << 8) + '0', // '-8f0'

		/// <summary>Lossy-compressed form of <see cref="CV420YpCbCr10BiPlanarVideoRange" />. No CVPlanarPixelBufferInfo struct. Compressed-packed with bo padding bits between pixels.</summary>
		Lossy_420YpCbCr10PackedBiPlanarVideoRange = ('-' << 24) + ('x' << 16) + ('v' << 8) + '0', // '-xv0'

		/// <summary>Lossy-compressed form of <see cref="CV422YpCbCr10BiPlanarVideoRange" />. No CVPlanarPixelBufferInfo struct. Compressed-packed with bo padding bits between pixels.</summary>
		Lossy_422YpCbCr10PackedBiPlanarVideoRange = ('-' << 24) + ('x' << 16) + ('v' << 8) + '2', // '-xv2'
	}

#if !COREBUILD
	[SupportedOSPlatform ("ios")]
	[SupportedOSPlatform ("maccatalyst")]
	[SupportedOSPlatform ("macos")]
	[SupportedOSPlatform ("tvos")]
	public static class CVPixelFormatTypeExtensions {

		[SupportedOSPlatform ("tvos15.0")]
		[SupportedOSPlatform ("macos")]
		[SupportedOSPlatform ("ios15.0")]
		[SupportedOSPlatform ("maccatalyst")]
		[DllImport (Constants.CoreVideoLibrary)]
		static extern byte CVIsCompressedPixelFormatAvailable (uint pixelFormatType);

		[SupportedOSPlatform ("tvos15.0")]
		[SupportedOSPlatform ("macos")]
		[SupportedOSPlatform ("ios15.0")]
		[SupportedOSPlatform ("maccatalyst")]
		public static bool IsCompressedPixelFormatAvailable (this CVPixelFormatType type)
			=> CVIsCompressedPixelFormatAvailable ((uint) type) != 0;
	}
#endif
}
