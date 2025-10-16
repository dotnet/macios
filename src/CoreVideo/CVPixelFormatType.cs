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

using CoreFoundation;

#nullable enable

namespace CoreVideo {
	/// <summary>An enumeration of known pixel formats.</summary>
	public enum CVPixelFormatType : uint {
		CV1Monochrome = 0x00000001,
		CV2Indexed = 0x00000002,
		CV4Indexed = 0x00000004,
		CV8Indexed = 0x00000008,
		CV1IndexedGray_WhiteIsZero = 0x00000021,
		CV2IndexedGray_WhiteIsZero = 0x00000022,
		CV4IndexedGray_WhiteIsZero = 0x00000024,
		CV8IndexedGray_WhiteIsZero = 0x00000028,
		CV16BE555 = 0x00000010,
		CV16LE555 = ('L' << 24) + ('5' << 16) + ('5' << 8) + '5', // 'L555'
		CV16LE5551 = ('5' << 24) + ('5' << 16) + ('5' << 8) + '1', // '5551'
		CV16BE565 = ('B' << 24) + ('5' << 16) + ('6' << 8) + '5', // 'B565'
		CV16LE565 = ('L' << 24) + ('5' << 16) + ('6' << 8) + '5', // 'L565'
		CV24RGB = 0x00000018,
		CV24BGR = ('2' << 24) + ('4' << 16) + ('B' << 8) + 'G', // '24BG'
		CV32ARGB = 0x00000020,
		CV32BGRA = ('B' << 24) + ('G' << 16) + ('R' << 8) + 'A', // 'BGRA'
		CV32ABGR = ('A' << 24) + ('B' << 16) + ('G' << 8) + 'R', // 'ABGR'
		CV32RGBA = ('R' << 24) + ('G' << 16) + ('B' << 8) + 'A', // 'RGBA'
		CV64ARGB = ('b' << 24) + ('6' << 16) + ('4' << 8) + 'a', // 'b64a'
		CV64RgbaLE = ('l' << 24) + ('6' << 16) + ('4' << 8) + 'r', // 'l64r'
		CV48RGB = ('b' << 24) + ('4' << 16) + ('8' << 8) + 'r', // 'b48r'
		CV32AlphaGray = ('b' << 24) + ('3' << 16) + ('2' << 8) + 'a', // 'b32a'
		CV16Gray = ('b' << 24) + ('1' << 16) + ('6' << 8) + 'g', // 'b16g'
		CV30RGB = ('R' << 24) + ('1' << 16) + ('0' << 8) + 'k', // 'R10k'
		/// <summary>30-bit RGB, 10-bit big endian samples, 2 unused padding bits (at most significant end).</summary>
		CV30RGB_r210 = ('r' << 24) + ('2' << 16) + ('1' << 8) + '0', // 'r210'
		CV422YpCbCr8 = ('2' << 24) + ('v' << 16) + ('u' << 8) + 'y', // '2vuy'
		CV4444YpCbCrA8 = ('v' << 24) + ('4' << 16) + ('0' << 8) + '8', // 'v408'
		CV4444YpCbCrA8R = ('r' << 24) + ('4' << 16) + ('0' << 8) + '8', // 'r408'
		CV4444AYpCbCr8 = ('y' << 24) + ('4' << 16) + ('0' << 8) + '8', // 'y408'
		CV4444AYpCbCr16 = ('y' << 24) + ('4' << 16) + ('1' << 8) + '6', // 'y416'
		CV4444AYpCbCrFloat = ('r' << 24) + ('4' << 16) + ('f' << 8) + 'l', // 'r4fl'
		CV444YpCbCr8 = ('v' << 24) + ('3' << 16) + ('0' << 8) + '8', // 'v308'
		CV422YpCbCr16 = ('v' << 24) + ('2' << 16) + ('1' << 8) + '6', // 'v216'
		CV422YpCbCr10 = ('v' << 24) + ('2' << 16) + ('1' << 8) + '0', // 'v210'
		CV444YpCbCr10 = ('v' << 24) + ('4' << 16) + ('1' << 8) + '0', // 'v410'
		CV420YpCbCr8Planar = ('y' << 24) + ('4' << 16) + ('2' << 8) + '0', // 'y420'
		CV420YpCbCr8PlanarFullRange = ('f' << 24) + ('4' << 16) + ('2' << 8) + '0', // 'f420'
		CV422YpCbCr_4A_8BiPlanar = ('a' << 24) + ('2' << 16) + ('v' << 8) + 'y', // 'a2vy'
		CV420YpCbCr8BiPlanarVideoRange = ('4' << 24) + ('2' << 16) + ('0' << 8) + 'v', // '420v'
		CV420YpCbCr8BiPlanarFullRange = ('4' << 24) + ('2' << 16) + ('0' << 8) + 'f', // '420f'
		CV422YpCbCr8BiPlanarVideoRange = ('4' << 24) + ('2' << 16) + ('2' << 8) + 'v', // '422v'
		CV422YpCbCr8BiPlanarFullRange = ('4' << 24) + ('2' << 16) + ('2' << 8) + 'f', // '422f'
		CV444YpCbCr8BiPlanarVideoRange = ('4' << 24) + ('4' << 16) + ('4' << 8) + 'v', // '444v'
		CV444YpCbCr8BiPlanarFullRange = ('4' << 24) + ('4' << 16) + ('4' << 8) + 'f', // '444f'
		CV422YpCbCr8_yuvs = ('y' << 24) + ('u' << 16) + ('v' << 8) + 's', // 'yuvs'
		CV422YpCbCr8FullRange = ('y' << 24) + ('u' << 16) + ('v' << 8) + 'f', // 'yuvf'
		OneComponent8 = ('L' << 24) + ('0' << 16) + ('0' << 8) + '8', // 'L008'
		TwoComponent8 = ('2' << 24) + ('C' << 16) + ('0' << 8) + '8', // '2C08'
		CV30RgbLePackedWideGamut = ('w' << 24) + ('3' << 16) + ('0' << 8) + 'r', // 'w30r'
		Argb2101010LEPacked = ('l' << 24) + ('1' << 16) + ('0' << 8) + 'r', // 'l10r'
		CV40ARGBLEWideGamut = ('w' << 24) + ('4' << 16) + ('0' << 8) + 'a', // 'w40a'
		CV40ARGBLEWideGamutPremultiplied = ('w' << 24) + ('4' << 16) + ('0' << 8) + 'm', // 'w40m'
		OneComponent10 = ('L' << 24) + ('0' << 16) + ('1' << 8) + '0', // 'L010'
		OneComponent12 = ('L' << 24) + ('0' << 16) + ('1' << 8) + '2', // 'L012'
		OneComponent16 = ('L' << 24) + ('0' << 16) + ('1' << 8) + '6', // 'L016'
		TwoComponent16 = ('2' << 24) + ('C' << 16) + ('1' << 8) + '6', // '2C16'
		OneComponent16Half = ('L' << 24) + ('0' << 16) + ('0' << 8) + 'h', // 'L00h'
		OneComponent32Float = ('L' << 24) + ('0' << 16) + ('0' << 8) + 'f', // 'L00f'
		TwoComponent16Half = ('2' << 24) + ('C' << 16) + ('0' << 8) + 'h', // '2C0h'
		TwoComponent32Float = ('2' << 24) + ('C' << 16) + ('0' << 8) + 'f', // '2C0f'
		CV64RGBAHalf = ('R' << 24) + ('G' << 16) + ('h' << 8) + 'A', // 'RGhA'
		CV128RGBAFloat = ('R' << 24) + ('G' << 16) + ('f' << 8) + 'A', // 'RGfA'
		CV14BayerGrbg = ('g' << 24) + ('r' << 16) + ('b' << 8) + '4', // 'grb4'
		CV14BayerRggb = ('r' << 24) + ('g' << 16) + ('g' << 8) + '4', // 'rgg4'
		CV14BayerBggr = ('b' << 24) + ('g' << 16) + ('g' << 8) + '4', // 'bgg4'
		CV14BayerGbrg = ('g' << 24) + ('b' << 16) + ('r' << 8) + '4', // 'gbr4'
		DisparityFloat16 = ('h' << 24) + ('d' << 16) + ('i' << 8) + 's', // 'hdis'
		DisparityFloat32 = ('f' << 24) + ('d' << 16) + ('i' << 8) + 's', // 'fdis'
		DepthFloat16 = ('h' << 24) + ('d' << 16) + ('e' << 8) + 'p', // 'hdep'
		DepthFloat32 = ('f' << 24) + ('d' << 16) + ('e' << 8) + 'p', // 'fdep'
		CV420YpCbCr10BiPlanarVideoRange = ('x' << 24) + ('4' << 16) + ('2' << 8) + '0', // 'x420'
		CV422YpCbCr10BiPlanarVideoRange = ('x' << 24) + ('4' << 16) + ('2' << 8) + '2', // 'x422'
		CV444YpCbCr10BiPlanarVideoRange = ('x' << 24) + ('4' << 16) + ('4' << 8) + '4', // 'x444'
		CV420YpCbCr10BiPlanarFullRange = ('x' << 24) + ('f' << 16) + ('2' << 8) + '0', // 'xf20'
		CV422YpCbCr10BiPlanarFullRange = ('x' << 24) + ('f' << 16) + ('2' << 8) + '2', // 'xf22'
		CV444YpCbCr10BiPlanarFullRange = ('x' << 24) + ('f' << 16) + ('4' << 8) + '4', // 'xf44'
		CV420YpCbCr8VideoRange_8A_TriPlanar = ('v' << 24) + ('0' << 16) + ('a' << 8) + '8', // 'v0a8'
		CV16VersatileBayer = ('b' << 24) + ('p' << 16) + ('1' << 8) + '6', // 'bp16'
		CV96VersatileBayerPacked12 = ('b' << 24) + ('t' << 16) + ('p' << 8) + '2', // 'btp2'
		CV64Rgba_DownscaledProResRaw = ('b' << 24) + ('p' << 16) + ('6' << 8) + '4', // 'bp64'
		CV422YpCbCr16BiPlanarVideoRange = ('s' << 24) + ('v' << 16) + ('2' << 8) + '2', // 'sv22'
		CV444YpCbCr16BiPlanarVideoRange = ('s' << 24) + ('v' << 16) + ('4' << 8) + '4', // 'sv44'
		CV444YpCbCr16VideoRange_16A_TriPlanar = ('s' << 24) + ('4' << 16) + ('a' << 8) + 's', // 's4as'
		CV30RGBLE_8A_BiPlanar = ('b' << 24) + ('3' << 16) + ('a' << 8) + '8', // 'b3a8'

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

		/// <summary>Lossless-compressed form of <see cref="CV30RGBLE_8A_BiPlanar" />. No CVPlanarPixelBufferInfo struct.</summary>
		Lossless_30RGBLE_8A_BiPlanar = ('&' << 24) + ('b' << 16) + ('3' << 8) + '8', // '&b38'
		/// <summary>Lossless-compressed form of <see cref="CV30RgbLePackedWideGamut" />. No CVPlanarPixelBufferInfo struct.</summary>
		Lossless_30RGBLEPackedWideGamut = ('&' << 24) + ('w' << 16) + ('3' << 8) + 'r', // '&w3r'

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

		[SupportedOSPlatform ("tvos26.0")]
		[SupportedOSPlatform ("macos26.0")]
		[SupportedOSPlatform ("ios26.0")]
		[SupportedOSPlatform ("maccatalyst26.0")]
		[DllImport (Constants.CoreVideoLibrary)]
		static extern IntPtr /* CFStringRef CV_NONNULL CF_RETURNS_RETAINED */ CVPixelFormatTypeCopyFourCharCodeString (CVPixelFormatType /* uint */ pixelFormatType);

		/// <summary>Get the pixel format type's FourCC code as a string.</summary>
		/// <param name="type">The pixel format type to convert to a string.</param>
		/// <summary>The pixel format type's FourCC code as a string.</summary>
		[SupportedOSPlatform ("tvos26.0")]
		[SupportedOSPlatform ("macos26.0")]
		[SupportedOSPlatform ("ios26.0")]
		[SupportedOSPlatform ("maccatalyst26.0")]
		public static string? ToFourCharCodeString (this CVPixelFormatType type)
		{
			var rv = CVPixelFormatTypeCopyFourCharCodeString (type);
			return CFString.FromHandle (rv, releaseHandle: true);
		}
	}
#endif
}
