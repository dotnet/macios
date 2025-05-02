//
// Copyright 2011-2014, Xamarin Inc.
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
using ObjCRuntime;

#nullable enable

namespace CoreImage {

	// convenience enum for values used with kCGImagePropertyOrientation (key) as NSNumber
	// values are part of the header file (CGImageProperties.h) as comments (not constants or fields)
	/// <summary>An enumeration whose values specify the origin of the <see cref="CoreImage.CIImage" />.</summary>
	///     <remarks>
	///       <para>The enumeration values are equal to those of the EXIF and TIFF specifications.</para>
	///     </remarks>
	public enum CIImageOrientation {
		/// <summary>Standard position.</summary>
		TopLeft = 1,
		/// <summary>Mirror image of standard position.</summary>
		TopRight = 2,
		/// <summary>Rotated 180 degrees from standard position.</summary>
		BottomRight = 3,
		/// <summary>Mirror image taken after the standard position image has been rotated 180 degrees.</summary>
		BottomLeft = 4,
		/// <summary>Mirror image taken after the standard position image has been rotated 90 degrees clockwise.</summary>
		LeftTop = 5,
		/// <summary>Rotated 90 degrees counterclockwise from standard position.</summary>
		RightTop = 6,
		/// <summary>Mirror image taken after the standard position image has been rotated 90 degrees counterclockwise.</summary>
		RightBottom = 7,
		/// <summary>Rotated 90 degrees clockwise from standard position.</summary>
		LeftBottom = 8,
	}

	// convenience enum (fields are used) but also a `typedef int` -> CIImage.h
	/// <summary>An enumeration whose values specify pixel formats.</summary>
	public enum CIFormat {
		/// <summary>Value that represents the kCIFormatARGB8 pixel format.</summary>
		ARGB8 = 0,
		/// <summary>Value that represents the kCIFormatRGBAh pixel format.</summary>
		RGBAh = 1,
#if MONOMAC
		/// <summary>To be added.</summary>
		RGBA16 = 2,
		/// <summary>To be added.</summary>
		[Obsolete ("This value can not be shared across Mac/iOS binaries, future proof with kRGBAf instead.")]
		RGBAf = 3,

		// Please, do not add values into MonoMac/iOS without adding an explicit value
#endif
		/// <summary>Value that represents the kCIFormatkRGBAf pixel format.</summary>
		kRGBAf = 4,
		/// <summary>Value that represents the kCIFormatkBGRA8 pixel format.</summary>
		kBGRA8 = 5,
		/// <summary>Value that represents the kCIFormatkRGBA8 pixel format.</summary>
		kRGBA8 = 6,
		/// <summary>Value that represents the kCIFormatABGR8 pixel format.</summary>
		ABGR8 = 7,
		/// <summary>Value that represents the kCIFormatA8 pixel format.</summary>
		A8 = 11,
		/// <summary>Value that represents the kCIFormatA16 pixel format.</summary>
		A16 = 12,
		/// <summary>Value that represents the kCIFormatAh pixel format.</summary>
		Ah = 13,
		/// <summary>Value that represents the kCIFormatAf pixel format.</summary>
		Af = 14,
		/// <summary>Value that represents the kCIFormatR8 pixel format.</summary>
		R8 = 15,
		/// <summary>Value that represents the kCIFormatR16 pixel format.</summary>
		R16 = 16,
		/// <summary>Value that represents the kCIFormatRh pixel format.</summary>
		Rh = 17,
		/// <summary>Value that represents the kCIFormatRf pixel format.</summary>
		Rf = 18,
		/// <summary>Value that represents the kCIFormatRG8 pixel format.</summary>
		RG8 = 19,
		/// <summary>Value that represents the kCIFormatRG16 pixel format.</summary>
		RG16 = 20,
		/// <summary>Value that represents the kCIFormatRGh pixel format.</summary>
		RGh = 21,
		/// <summary>Value that represents the kCIFormatRGf pixel format.</summary>
		RGf = 22,
	}

	/// <summary>Enumerates the error correction levels for QR codes.</summary>
	[MacCatalyst (13, 1)]
	[Native]
	public enum CIQRCodeErrorCorrectionLevel : long {
		/// <summary>To be added.</summary>
		L = 76,
		/// <summary>To be added.</summary>
		M = 77,
		/// <summary>To be added.</summary>
		Q = 81,
		/// <summary>To be added.</summary>
		H = 72,
	}

	/// <summary>Enumerates the ECC versions for the Data Matrix barcode scheme.</summary>
	[MacCatalyst (13, 1)]
	[Native]
	public enum CIDataMatrixCodeEccVersion : long {
		/// <summary>To be added.</summary>
		V000 = 0,
		/// <summary>To be added.</summary>
		V050 = 50,
		/// <summary>To be added.</summary>
		V080 = 80,
		/// <summary>To be added.</summary>
		V100 = 100,
		/// <summary>To be added.</summary>
		V140 = 140,
		/// <summary>To be added.</summary>
		V200 = 200,
	}

	[MacCatalyst (13, 1)]
	[Native]
	public enum CIRenderDestinationAlphaMode : ulong {
		/// <summary>To be added.</summary>
		None = 0,
		/// <summary>To be added.</summary>
		Premultiplied = 1,
		/// <summary>To be added.</summary>
		Unpremultiplied = 2,
	}
}
