// 
// CVImageBuffer.cs: Implements the managed CVImageBuffer
//
// Authors: Mono Team
//     
// Copyright 2010 Novell, Inc
// Copyright 2011-2014 Xamarin Inc
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
using CoreFoundation;
using ObjCRuntime;
using Foundation;
using CoreGraphics;

#nullable enable

namespace CoreVideo {

	// CVImageBuffer.h
	[SupportedOSPlatform ("ios")]
	[SupportedOSPlatform ("maccatalyst")]
	[SupportedOSPlatform ("macos")]
	[SupportedOSPlatform ("tvos")]
	public partial class CVImageBuffer : CVBuffer {
#if !COREBUILD
		[Preserve (Conditional = true)]
		internal CVImageBuffer (NativeHandle handle, bool owns)
			: base (handle, owns)
		{
		}

		[DllImport (Constants.CoreVideoLibrary)]
		extern static CGRect CVImageBufferGetCleanRect (/* CVImageBufferRef __nonnull */ IntPtr imageBuffer);

		/// <summary>To be added.</summary>
		///         <value>To be added.</value>
		///         <remarks>To be added.</remarks>
		public CGRect CleanRect {
			get {
				return CVImageBufferGetCleanRect (Handle);
			}
		}

		[DllImport (Constants.CoreVideoLibrary)]
		extern static CGSize CVImageBufferGetDisplaySize (/* CVImageBufferRef __nonnull */ IntPtr imageBuffer);

		/// <summary>To be added.</summary>
		///         <value>To be added.</value>
		///         <remarks>To be added.</remarks>
		public CGSize DisplaySize {
			get {
				return CVImageBufferGetDisplaySize (Handle);
			}
		}

		[DllImport (Constants.CoreVideoLibrary)]
		extern static CGSize CVImageBufferGetEncodedSize (/* CVImageBufferRef __nonnull */ IntPtr imageBuffer);

		/// <summary>To be added.</summary>
		///         <value>To be added.</value>
		///         <remarks>To be added.</remarks>
		public CGSize EncodedSize {
			get {
				return CVImageBufferGetDisplaySize (Handle);
			}
		}

		[DllImport (Constants.CoreVideoLibrary)]
		extern static /* Boolean */ byte CVImageBufferIsFlipped (/* CVImageBufferRef __nonnull */ IntPtr imageBuffer);

		/// <summary>To be added.</summary>
		///         <value>To be added.</value>
		///         <remarks>To be added.</remarks>
		public bool IsFlipped {
			get {
				return CVImageBufferIsFlipped (Handle) != 0;
			}
		}

		[DllImport (Constants.CoreVideoLibrary)]
		extern static /* CGColorSpaceRef */ IntPtr CVImageBufferGetColorSpace (/* CVImageBufferRef */ IntPtr imageBuffer);

		/// <summary>Get the color space for this image buffer</summary>
		/// <value>The color space for this image buffer.</value>
		public CGColorSpace? ColorSpace {
			get {
				var h = CVImageBufferGetColorSpace (Handle);
				return h == IntPtr.Zero ? null : new CGColorSpace (h, false);
			}
		}

		[DllImport (Constants.CoreVideoLibrary)]
		extern static /* CGColorSpaceRef */ IntPtr CVImageBufferCreateColorSpaceFromAttachments (/* CFDictionaryRef */ IntPtr attachments);

		/// <summary>Try to create a <see cref="CGColorSpace" /> from an image buffer's attachments.</summary>
		/// <param name="attachments">An image buffer's attachments to use when trying to create a new color space.</param>
		/// <returns>If successful, a new <see cref="CGColorSpace" /> instance, otherwise <see langword="null" />.</returns>
		public static CGColorSpace? CreateFrom (NSDictionary attachments)
		{
			var h = CVImageBufferCreateColorSpaceFromAttachments (attachments.GetNonNullHandle (nameof (attachments)));
			GC.KeepAlive (attachments);
			return h == IntPtr.Zero ? null : new CGColorSpace (h, true);
		}

		[SupportedOSPlatform ("ios")]
		[SupportedOSPlatform ("macos")]
		[SupportedOSPlatform ("tvos")]
		[SupportedOSPlatform ("maccatalyst")]
		[DllImport (Constants.CoreVideoLibrary)]
		extern static int CVYCbCrMatrixGetIntegerCodePointForString (IntPtr yCbCrMatrixString);

		/// <param name="yCbCrMatrix">To be added.</param>
		///         <summary>To be added.</summary>
		///         <returns>To be added.</returns>
		///         <remarks>To be added.</remarks>
		[SupportedOSPlatform ("ios")]
		[SupportedOSPlatform ("macos")]
		[SupportedOSPlatform ("tvos")]
		[SupportedOSPlatform ("maccatalyst")]
		public static int GetCodePoint (CVImageBufferYCbCrMatrix yCbCrMatrix)
		{
			return CVYCbCrMatrixGetIntegerCodePointForString (yCbCrMatrix.GetConstant ()!.Handle);
		}

		[SupportedOSPlatform ("ios")]
		[SupportedOSPlatform ("macos")]
		[SupportedOSPlatform ("tvos")]
		[SupportedOSPlatform ("maccatalyst")]
		[DllImport (Constants.CoreVideoLibrary)]
		extern static int CVColorPrimariesGetIntegerCodePointForString (IntPtr colorPrimariesString);

		/// <param name="color">To be added.</param>
		///         <summary>To be added.</summary>
		///         <returns>To be added.</returns>
		///         <remarks>To be added.</remarks>
		[SupportedOSPlatform ("ios")]
		[SupportedOSPlatform ("macos")]
		[SupportedOSPlatform ("tvos")]
		[SupportedOSPlatform ("maccatalyst")]
		public static int GetCodePoint (CVImageBufferColorPrimaries color)
		{
			return CVColorPrimariesGetIntegerCodePointForString (color.GetConstant ()!.Handle);
		}

		[SupportedOSPlatform ("ios")]
		[SupportedOSPlatform ("macos")]
		[SupportedOSPlatform ("tvos")]
		[SupportedOSPlatform ("maccatalyst")]
		[DllImport (Constants.CoreVideoLibrary)]
		extern static int CVTransferFunctionGetIntegerCodePointForString (IntPtr colorPrimariesString);

		/// <param name="function">To be added.</param>
		///         <summary>To be added.</summary>
		///         <returns>To be added.</returns>
		///         <remarks>To be added.</remarks>
		[SupportedOSPlatform ("ios")]
		[SupportedOSPlatform ("macos")]
		[SupportedOSPlatform ("tvos")]
		[SupportedOSPlatform ("maccatalyst")]
		public static int GetCodePoint (CVImageBufferTransferFunction function)
		{
			return CVTransferFunctionGetIntegerCodePointForString (function.GetConstant ()!.Handle);
		}

		[SupportedOSPlatform ("ios")]
		[SupportedOSPlatform ("macos")]
		[SupportedOSPlatform ("tvos")]
		[SupportedOSPlatform ("maccatalyst")]
		[DllImport (Constants.CoreVideoLibrary)]
		extern static IntPtr CVYCbCrMatrixGetStringForIntegerCodePoint (int yCbCrMatrixCodePoint);

		/// <param name="yCbCrMatrixCodePoint">To be added.</param>
		///         <summary>To be added.</summary>
		///         <returns>To be added.</returns>
		///         <remarks>To be added.</remarks>
		[SupportedOSPlatform ("ios")]
		[SupportedOSPlatform ("macos")]
		[SupportedOSPlatform ("tvos")]
		[SupportedOSPlatform ("maccatalyst")]
		public static CVImageBufferYCbCrMatrix GetYCbCrMatrixOption (int yCbCrMatrixCodePoint)
		{
			var ret = Runtime.GetNSObject<NSString> (CVYCbCrMatrixGetStringForIntegerCodePoint (yCbCrMatrixCodePoint));
			return CVImageBufferYCbCrMatrixExtensions.GetValue (ret);
		}

		[SupportedOSPlatform ("ios")]
		[SupportedOSPlatform ("macos")]
		[SupportedOSPlatform ("tvos")]
		[SupportedOSPlatform ("maccatalyst")]
		[DllImport (Constants.CoreVideoLibrary)]
		extern static IntPtr CVColorPrimariesGetStringForIntegerCodePoint (int colorPrimariesCodePoint);

		/// <param name="colorPrimariesCodePoint">To be added.</param>
		///         <summary>To be added.</summary>
		///         <returns>To be added.</returns>
		///         <remarks>To be added.</remarks>
		[SupportedOSPlatform ("ios")]
		[SupportedOSPlatform ("macos")]
		[SupportedOSPlatform ("tvos")]
		[SupportedOSPlatform ("maccatalyst")]
		public static CVImageBufferColorPrimaries GetColorPrimariesOption (int colorPrimariesCodePoint)
		{
			var ret = Runtime.GetNSObject<NSString> (CVColorPrimariesGetStringForIntegerCodePoint (colorPrimariesCodePoint));
			return CVImageBufferColorPrimariesExtensions.GetValue (ret);
		}

		[SupportedOSPlatform ("ios")]
		[SupportedOSPlatform ("macos")]
		[SupportedOSPlatform ("tvos")]
		[SupportedOSPlatform ("maccatalyst")]
		[DllImport (Constants.CoreVideoLibrary)]
		extern static IntPtr CVTransferFunctionGetStringForIntegerCodePoint (int transferFunctionCodePoint);

		/// <param name="transferFunctionCodePoint">To be added.</param>
		///         <summary>To be added.</summary>
		///         <returns>To be added.</returns>
		///         <remarks>To be added.</remarks>
		[SupportedOSPlatform ("ios")]
		[SupportedOSPlatform ("macos")]
		[SupportedOSPlatform ("tvos")]
		[SupportedOSPlatform ("maccatalyst")]
		public static CVImageBufferTransferFunction GetTransferFunctionOption (int transferFunctionCodePoint)
		{
			var ret = Runtime.GetNSObject<NSString> (CVTransferFunctionGetStringForIntegerCodePoint (transferFunctionCodePoint));
			return CVImageBufferTransferFunctionExtensions.GetValue (ret);
		}

#endif // !COREBUILD
	}
}
