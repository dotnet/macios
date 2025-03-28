// 
// CGBitmapContext.cs:
//
// Authors: Mono Team
//     
// Copyright 2009 Novell, Inc
// Copyright 2014 Xamarin Inc.
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

#nullable enable

using System;
using System.Runtime.InteropServices;
using System.Runtime.Versioning;

using ObjCRuntime;
using Foundation;

namespace CoreGraphics {
	[SupportedOSPlatform ("ios")]
	[SupportedOSPlatform ("maccatalyst")]
	[SupportedOSPlatform ("macos")]
	[SupportedOSPlatform ("tvos")]
	public class CGBitmapContext : CGContext {
#if !COREBUILD
		// If allocated, this points to the byte array buffer that is passed.
		GCHandle buffer;

		[Preserve (Conditional = true)]
		internal CGBitmapContext (NativeHandle handle, bool owns) : base (handle, owns)
		{
		}

		// CGBitmapInfo -> uint32_t -> CGImage.h
		// CGImageAlphaInfo -> uint32_t -> CGImage.h

		[DllImport (Constants.CoreGraphicsLibrary)]
		extern static IntPtr CGBitmapContextCreate (/* void* */ IntPtr data, /* size_t */ nint width, /* size_t */ nint height, /* size_t */ nint bitsPerComponent,
			/* size_t */ nint bytesPerRow, /* CGColorSpaceRef */ IntPtr colorSpace, /* CGBitmapInfo = uint32_t */ uint bitmapInfo);

		public CGBitmapContext (IntPtr data, nint width, nint height, nint bitsPerComponent, nint bytesPerRow, CGColorSpace? colorSpace, CGImageAlphaInfo bitmapInfo)
			: base (CGBitmapContextCreate (data, width, height, bitsPerComponent, bytesPerRow, colorSpace.GetHandle (), (uint) bitmapInfo), true)
		{
			GC.KeepAlive (colorSpace);
		}

		public CGBitmapContext (IntPtr data, nint width, nint height, nint bitsPerComponent, nint bytesPerRow, CGColorSpace? colorSpace, CGBitmapFlags bitmapInfo)
			: base (CGBitmapContextCreate (data, width, height, bitsPerComponent, bytesPerRow, colorSpace.GetHandle (), (uint) bitmapInfo), true)
		{
			GC.KeepAlive (colorSpace);
		}

		[DllImport (Constants.CoreGraphicsLibrary)]
		extern static IntPtr CGBitmapContextCreate (/* void* */ byte []? data, /* size_t */ nint width, /* size_t */ nint height, /* size_t */ nint bitsPerComponent,
			/* size_t */ nint bytesPerRow, /* CGColorSpaceRef */ IntPtr colorSpace, /* CGBitmapInfo = uint32_t */ uint bitmapInfo);

		static IntPtr Create (byte []? data, nint width, nint height, nint bitsPerComponent, nint bytesPerRow, CGColorSpace? colorSpace, CGImageAlphaInfo bitmapInfo, out GCHandle buffer)
		{
			buffer = default (GCHandle);
			if (data is not null)
				buffer = GCHandle.Alloc (data, GCHandleType.Pinned); // This requires a pinned GCHandle, because unsafe code is scoped to the current block, and the address of the byte array will be used after this function returns.
			IntPtr result = CGBitmapContextCreate (data, width, height, bitsPerComponent, bytesPerRow, colorSpace.GetHandle (), (uint) bitmapInfo);
			GC.KeepAlive (colorSpace);
			return result;
		}

		public CGBitmapContext (byte []? data, nint width, nint height, nint bitsPerComponent, nint bytesPerRow, CGColorSpace? colorSpace, CGImageAlphaInfo bitmapInfo)
			: base (Create (data, width, height, bitsPerComponent, bytesPerRow, colorSpace, bitmapInfo, out var buffer), true)
		{
			this.buffer = buffer;
		}

		static IntPtr Create (byte []? data, nint width, nint height, nint bitsPerComponent, nint bytesPerRow, CGColorSpace? colorSpace, CGBitmapFlags bitmapInfo, out GCHandle buffer)
		{
			buffer = default (GCHandle);
			if (data is not null)
				buffer = GCHandle.Alloc (data, GCHandleType.Pinned); // This requires a pinned GCHandle, because unsafe code is scoped to the current block, and the address of the byte array will be used after this function returns.
			IntPtr result = CGBitmapContextCreate (data, width, height, bitsPerComponent, bytesPerRow, colorSpace.GetHandle (), (uint) bitmapInfo);
			GC.KeepAlive (colorSpace);
			return result;
		}

		public CGBitmapContext (byte []? data, nint width, nint height, nint bitsPerComponent, nint bytesPerRow, CGColorSpace? colorSpace, CGBitmapFlags bitmapInfo)
			: base (Create (data, width, height, bitsPerComponent, bytesPerRow, colorSpace, bitmapInfo, out var buffer), true)
		{
			this.buffer = buffer;
		}

		protected override void Dispose (bool disposing)
		{
			if (buffer.IsAllocated)
				buffer.Free ();
			base.Dispose (disposing);
		}

		[DllImport (Constants.CoreGraphicsLibrary)]
		extern static /* void* */ IntPtr CGBitmapContextGetData (/* CGContextRef */ IntPtr context);

		/// <summary>Gets a pointer to the image data for <c>this</c> <see cref="T:CoreGraphics.CGBitmapContext" /> object, or <see langword="null" /> if <c>this</c> object is not a bitmap context.</summary>
		///         <value>A pointer to the data.</value>
		///         <remarks>To be added.</remarks>
		public IntPtr Data {
			get { return CGBitmapContextGetData (Handle); }
		}

		[DllImport (Constants.CoreGraphicsLibrary)]
		extern static /* size_t */ nint CGBitmapContextGetWidth (/* CGContextRef */ IntPtr context);

		/// <summary>Gets the width for <c>this</c> <see cref="T:CoreGraphics.CGBitmapContext" /> object, in pixels, or 0 if <c>this</c> object is not a bitmap context.</summary>
		///         <value>To be added.</value>
		///         <remarks>To be added.</remarks>
		public nint Width {
			get { return CGBitmapContextGetWidth (Handle); }
		}

		[DllImport (Constants.CoreGraphicsLibrary)]
		extern static /* size_t */ nint CGBitmapContextGetHeight (/* CGContextRef */ IntPtr context);

		/// <summary>Gets the height for <c>this</c> <see cref="T:CoreGraphics.CGBitmapContext" /> object, in pixels, or 0 if <c>this</c> object is not a bitmap context.</summary>
		///         <value>To be added.</value>
		///         <remarks>To be added.</remarks>
		public nint Height {
			get { return CGBitmapContextGetHeight (Handle); }
		}

		[DllImport (Constants.CoreGraphicsLibrary)]
		extern static /* size_t */ nint CGBitmapContextGetBitsPerComponent (/* CGContextRef */ IntPtr context);

		/// <summary>Number of bits per component for</summary>
		///         <value>Gets the number of bits per component for <c>this</c> <see cref="T:CoreGraphics.CGBitmapContext" /> object, or 0 if <c>this</c> object is not a bitmap context.</value>
		///         <remarks>The number of bits used by each component of a pixel in memory.  For example, when using 32-bit RGBA buffers the value for this would be an 8.</remarks>
		public nint BitsPerComponent {
			get { return CGBitmapContextGetBitsPerComponent (Handle); }
		}

		[DllImport (Constants.CoreGraphicsLibrary)]
		extern static /* size_t */ nint CGBitmapContextGetBitsPerPixel (/* CGContextRef */ IntPtr context);

		/// <summary>Number of bits per pixel.</summary>
		///         <value>Gets the number of bits per pixel for <c>this</c> <see cref="T:CoreGraphics.CGBitmapContext" /> object, or 0 if <c>this</c> object is not a bitmap context.</value>
		///         <remarks>To be added.</remarks>
		public nint BitsPerPixel {
			get { return (nint) CGBitmapContextGetBitsPerPixel (Handle); }
		}

		[DllImport (Constants.CoreGraphicsLibrary)]
		extern static /* size_t */ nint CGBitmapContextGetBytesPerRow (/* CGContextRef */ IntPtr context);

		/// <summary>Gets the number of bytes per row for <c>this</c> <see cref="T:CoreGraphics.CGBitmapContext" /> object, or 0 if <c>this</c> object is not a bitmap context.</summary>
		///         <value>To be added.</value>
		///         <remarks>
		///           <para>Number of bytes per row, a number greather or equal that the number of bytes used by a row of pixels.   </para>
		///           <para>Typically is the width multiplied by the number of bytes per pixel, with some extra padding.   This is called the image stride.   </para>
		///           <para>While this does not affect the rendering of the image, it can improve the performance of image rendering by aligning the first pixel to the natural processor alignment.</para>
		///         </remarks>
		public nint BytesPerRow {
			get { return CGBitmapContextGetBytesPerRow (Handle); }
		}

		[DllImport (Constants.CoreGraphicsLibrary)]
		extern static /* CGColorSpaceRef */ IntPtr CGBitmapContextGetColorSpace (/* CGContextRef */ IntPtr context);

		/// <summary>Gets the color space for <c>this</c> <see cref="T:CoreGraphics.CGBitmapContext" /> object, as a <see cref="T:CoreGraphics.CGColorSpace" />, or <see langword="null" /> if <c>this</c> object is not a bitmap context.</summary>
		///         <value>To be added.</value>
		///         <remarks>To be added.</remarks>
		public CGColorSpace? ColorSpace {
			get {
				var ptr = CGBitmapContextGetColorSpace (Handle);
				return ptr == IntPtr.Zero ? null : new CGColorSpace (ptr, false);
			}
		}

		[DllImport (Constants.CoreGraphicsLibrary)]
		extern static CGImageAlphaInfo CGBitmapContextGetAlphaInfo (/* CGContextRef */ IntPtr context);

		/// <summary>Gets the alpha information for <c>this</c> <see cref="T:CoreGraphics.CGBitmapContext" /> object, as a <see cref="T:CoreGraphics.CGImageAlphaInfo" /> object, or <see cref="F:CoreGraphics.CGImageAlphaInfo.None" /> if <c>this</c> object is not a bitmap context.</summary>
		///         <value>To be added.</value>
		///         <remarks>To be added.</remarks>
		public CGImageAlphaInfo AlphaInfo {
			get { return CGBitmapContextGetAlphaInfo (Handle); }
		}

		[DllImport (Constants.CoreGraphicsLibrary)]
		extern static /* CGBitmapInfo */ uint CGBitmapContextGetBitmapInfo (/* CGContextRef */ IntPtr context);

		/// <summary>To be added.</summary>
		///         <value>To be added.</value>
		///         <remarks>To be added.</remarks>
		public CGBitmapFlags BitmapInfo {
			get { return (CGBitmapFlags) CGBitmapContextGetBitmapInfo (Handle); }
		}

		[DllImport (Constants.CoreGraphicsLibrary)]
		extern static /* CGImageRef */ IntPtr CGBitmapContextCreateImage (/* CGContextRef */ IntPtr context);

		public CGImage? ToImage ()
		{
			var h = CGBitmapContextCreateImage (Handle);
			// do not return an invalid instance (null handle) if something went wrong
			return h == IntPtr.Zero ? null : new CGImage (h, true);
		}
#endif // !COREBUILD
	}
}
