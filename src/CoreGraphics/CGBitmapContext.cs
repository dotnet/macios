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
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using System.Runtime.Versioning;

using ObjCRuntime;
using Foundation;

namespace CoreGraphics {
	/// <summary>CGContext backed by an in-memory bitmap.</summary>
	///     <remarks>To be added.</remarks>
	///     <related type="sample" href="https://github.com/xamarin/ios-samples/tree/master/Drawing/">Example_Drawing</related>
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

		/// <include file="../../docs/api/CoreGraphics/CGBitmapContext.xml" path="/Documentation/Docs[@DocId='M:CoreGraphics.CGBitmapContext.Dispose(System.Boolean)']/*" />
		protected override void Dispose (bool disposing)
		{
			if (buffer.IsAllocated)
				buffer.Free ();
			base.Dispose (disposing);
		}

		[DllImport (Constants.CoreGraphicsLibrary)]
		extern static /* void* */ IntPtr CGBitmapContextGetData (/* CGContextRef */ IntPtr context);

		/// <summary>Gets a pointer to the image data for <c>this</c> <see cref="CoreGraphics.CGBitmapContext" /> object, or <see langword="null" /> if <c>this</c> object is not a bitmap context.</summary>
		///         <value>A pointer to the data.</value>
		///         <remarks>To be added.</remarks>
		public IntPtr Data {
			get { return CGBitmapContextGetData (Handle); }
		}

		[DllImport (Constants.CoreGraphicsLibrary)]
		extern static /* size_t */ nint CGBitmapContextGetWidth (/* CGContextRef */ IntPtr context);

		/// <summary>Gets the width for <c>this</c> <see cref="CoreGraphics.CGBitmapContext" /> object, in pixels, or 0 if <c>this</c> object is not a bitmap context.</summary>
		///         <value>To be added.</value>
		///         <remarks>To be added.</remarks>
		public nint Width {
			get { return CGBitmapContextGetWidth (Handle); }
		}

		[DllImport (Constants.CoreGraphicsLibrary)]
		extern static /* size_t */ nint CGBitmapContextGetHeight (/* CGContextRef */ IntPtr context);

		/// <summary>Gets the height for <c>this</c> <see cref="CoreGraphics.CGBitmapContext" /> object, in pixels, or 0 if <c>this</c> object is not a bitmap context.</summary>
		///         <value>To be added.</value>
		///         <remarks>To be added.</remarks>
		public nint Height {
			get { return CGBitmapContextGetHeight (Handle); }
		}

		[DllImport (Constants.CoreGraphicsLibrary)]
		extern static /* size_t */ nint CGBitmapContextGetBitsPerComponent (/* CGContextRef */ IntPtr context);

		/// <summary>Number of bits per component for</summary>
		///         <value>Gets the number of bits per component for <c>this</c> <see cref="CoreGraphics.CGBitmapContext" /> object, or 0 if <c>this</c> object is not a bitmap context.</value>
		///         <remarks>The number of bits used by each component of a pixel in memory.  For example, when using 32-bit RGBA buffers the value for this would be an 8.</remarks>
		public nint BitsPerComponent {
			get { return CGBitmapContextGetBitsPerComponent (Handle); }
		}

		[DllImport (Constants.CoreGraphicsLibrary)]
		extern static /* size_t */ nint CGBitmapContextGetBitsPerPixel (/* CGContextRef */ IntPtr context);

		/// <summary>Number of bits per pixel.</summary>
		///         <value>Gets the number of bits per pixel for <c>this</c> <see cref="CoreGraphics.CGBitmapContext" /> object, or 0 if <c>this</c> object is not a bitmap context.</value>
		///         <remarks>To be added.</remarks>
		public nint BitsPerPixel {
			get { return (nint) CGBitmapContextGetBitsPerPixel (Handle); }
		}

		[DllImport (Constants.CoreGraphicsLibrary)]
		extern static /* size_t */ nint CGBitmapContextGetBytesPerRow (/* CGContextRef */ IntPtr context);

		/// <summary>Gets the number of bytes per row for <c>this</c> <see cref="CoreGraphics.CGBitmapContext" /> object, or 0 if <c>this</c> object is not a bitmap context.</summary>
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

		/// <summary>Gets the color space for <c>this</c> <see cref="CoreGraphics.CGBitmapContext" /> object, as a <see cref="CoreGraphics.CGColorSpace" />, or <see langword="null" /> if <c>this</c> object is not a bitmap context.</summary>
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

		/// <summary>Gets the alpha information for <c>this</c> <see cref="CoreGraphics.CGBitmapContext" /> object, as a <see cref="CoreGraphics.CGImageAlphaInfo" /> object, or <see cref="CoreGraphics.CGImageAlphaInfo.None" /> if <c>this</c> object is not a bitmap context.</summary>
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

		/// <summary>To be added.</summary>
		///         <returns>To be added.</returns>
		///         <remarks>To be added.</remarks>
		public CGImage? ToImage ()
		{
			var h = CGBitmapContextCreateImage (Handle);
			// do not return an invalid instance (null handle) if something went wrong
			return h == IntPtr.Zero ? null : new CGImage (h, true);
		}

		[SupportedOSPlatform ("ios26.0")]
		[SupportedOSPlatform ("tvos26.0")]
		[SupportedOSPlatform ("maccatalyst26.0")]
		[SupportedOSPlatform ("macos26.0")]
		[DllImport (Constants.CoreGraphicsLibrary)]
		static extern unsafe IntPtr /* CGContextRef */ CGBitmapContextCreateAdaptive (
				nuint /* size_t */ width,
				nuint /* size_t */ height,
				IntPtr /* CFDictionaryRef __nullable */ auxiliaryInfo,
				BlockLiteral* /* (^__nullable onResolve)(const CGContentInfo* __nonnull, CGBitmapParameters* __nonnull) */ onResolve,
				BlockLiteral* /* (^__nullable onAllocate)(const CGContentInfo* __nonnull, const CGBitmapParameters* __nonnull) */ onAllocate,
				BlockLiteral* /* (^__nullable onFree)(CGRenderingBufferProviderRef __nonnull, const CGContentInfo* __nonnull, const CGBitmapParameters* __nonnull) */ onFree,
				BlockLiteral* /* (^__nullable onError)(CFErrorRef __nonnull, const CGContentInfo* __nonnull, const CGBitmapParameters* __nonnull) */ onError
		);

		/// <summary>Create a bitmap context designed to choose the optimal bit depth, colorspace and EDR target headroom.</summary>
		/// <param name="width">The width of the new <see cref="CGBitmapContext" />.</param>
		/// <param name="height">The height of the new <see cref="CGBitmapContext" />.</param>
		/// <param name="auxiliaryInfo">Any additional information for the creation of the new <see cref="CGBitmapContext" />.</param>
		/// <param name="onResolve">A callback to modify the <see cref="CGBitmapParameters" /> used to create the new <see cref="CGBitmapContext" />.</param>
		/// <param name="onAllocate">A callback to allocate memory for the new <see cref="CGBitmapContext" />.</param>
		/// <param name="onFree">A callback to free any allocated memory for the new <see cref="CGBitmapContext" />.</param>
		/// <param name="onError">A callback that is called in case of any errors.</param>
		/// <returns> A new <see cref="CGBitmapContext" /> if successful, <see langword="null" /> otherwise.</returns>
		[SupportedOSPlatform ("ios26.0")]
		[SupportedOSPlatform ("tvos26.0")]
		[SupportedOSPlatform ("maccatalyst26.0")]
		[SupportedOSPlatform ("macos26.0")]
		[BindingImpl (BindingImplOptions.Optimizable)]
		public unsafe static CGBitmapContext? Create (nuint width, nuint height, NSDictionary? auxiliaryInfo, OnResolveCallback? onResolve, OnAllocateCallback? onAllocate, OnFreeCallback? onFree, OnErrorCallback? onError)
		{
			delegate* unmanaged<IntPtr, CGContentInfo*, CGBitmapParameters*, byte> onResolveTrampoline = &CGBitmapContextCreateAdaptive_OnResolve;
			using var onResolveBlock = onResolve is null ? default (BlockLiteral) : new BlockLiteral (onResolveTrampoline, onResolve, typeof (CGBitmapContext), nameof (CGBitmapContextCreateAdaptive_OnResolve));

			delegate* unmanaged<IntPtr, CGContentInfo*, CGBitmapParameters*, IntPtr> onAllocateTrampoline = &CGBitmapContextCreateAdaptive_OnAllocate;
			using var onAllocateBlock = onAllocate is null ? default (BlockLiteral) : new BlockLiteral (onAllocateTrampoline, onAllocate, typeof (CGBitmapContext), nameof (CGBitmapContextCreateAdaptive_OnAllocate));

			delegate* unmanaged<IntPtr, IntPtr, CGContentInfo*, CGBitmapParameters*, void> onFreeTrampoline = &CGBitmapContextCreateAdaptive_OnFree;
			using var onFreeBlock = onFree is null ? default (BlockLiteral) : new BlockLiteral (onFreeTrampoline, onFree, typeof (CGBitmapContext), nameof (CGBitmapContextCreateAdaptive_OnFree));

			delegate* unmanaged<IntPtr, IntPtr, CGContentInfo*, CGBitmapParameters*, void> onErrorTrampoline = &CGBitmapContextCreateAdaptive_OnError;
			using var onErrorBlock = onError is null ? default (BlockLiteral) : new BlockLiteral (onErrorTrampoline, onError, typeof (CGBitmapContext), nameof (CGBitmapContextCreateAdaptive_OnError));

			var rv = CGBitmapContextCreateAdaptive (
					width, height,
					auxiliaryInfo.GetHandle (),
					onResolve is null ? null : &onResolveBlock,
					onAllocate is null ? null : &onAllocateBlock,
					onFree is null ? null : &onFreeBlock,
					onError is null ? null : &onErrorBlock
				);

			GC.KeepAlive (auxiliaryInfo);

			if (rv == IntPtr.Zero)
				return null;

			return new CGBitmapContext (rv, true);
		}

		/// <summary>Create a bitmap context designed to choose the optimal bit depth, colorspace and EDR target headroom.</summary>
		/// <param name="width">The width of the new <see cref="CGBitmapContext" />.</param>
		/// <param name="height">The height of the new <see cref="CGBitmapContext" />.</param>
		/// <param name="auxiliaryInfo">Any additional information for the creation of the new <see cref="CGBitmapContext" />.</param>
		/// <param name="onResolve">A callback to modify the <see cref="CGBitmapParameters" /> used to create the new <see cref="CGBitmapContext" />.</param>
		/// <param name="onAllocate">A callback to allocate memory for the new <see cref="CGBitmapContext" />.</param>
		/// <param name="onFree">A callback to free any allocated memory for the new <see cref="CGBitmapContext" />.</param>
		/// <param name="onError">A callback that is called in case of any errors.</param>
		/// <returns> A new <see cref="CGBitmapContext" /> if successful, <see langword="null" /> otherwise.</returns>
		[SupportedOSPlatform ("ios26.0")]
		[SupportedOSPlatform ("tvos26.0")]
		[SupportedOSPlatform ("maccatalyst26.0")]
		[SupportedOSPlatform ("macos26.0")]
		public unsafe static CGBitmapContext? Create (nuint width, nuint height, CGAdaptiveOptions? auxiliaryInfo, OnResolveCallback? onResolve, OnAllocateCallback? onAllocate, OnFreeCallback? onFree, OnErrorCallback? onError)
		{
			return Create (width, height, auxiliaryInfo?.Dictionary, onResolve, onAllocate, onFree, onError);
		}

		public delegate bool OnResolveCallback (ref CGContentInfo contentInfo, ref CGBitmapParameters bitmapParameters);

		[UnmanagedCallersOnly]
		unsafe static byte CGBitmapContextCreateAdaptive_OnResolve (IntPtr block, CGContentInfo* contentInfo, CGBitmapParameters* bitmapParameters)
		{
			var del = BlockLiteral.GetTarget<OnResolveCallback> (block);
			if (del is not null) {
				var rv = del (ref Unsafe.AsRef<CGContentInfo> (contentInfo), ref Unsafe.AsRef<CGBitmapParameters> (bitmapParameters));
				return rv.AsByte ();
			}
			return 0;
		}

		public delegate CGRenderingBufferProvider /* CGRenderingBufferProviderRef */ OnAllocateCallback (ref CGContentInfo contentInfo, ref CGBitmapParameters bitmapParameters);

		[UnmanagedCallersOnly]
		unsafe static IntPtr CGBitmapContextCreateAdaptive_OnAllocate (IntPtr block, CGContentInfo* contentInfo, CGBitmapParameters* bitmapParameters)
		{
			var del = BlockLiteral.GetTarget<OnAllocateCallback> (block);
			if (del is not null) {
				var rv = del (ref Unsafe.AsRef<CGContentInfo> (contentInfo), ref Unsafe.AsRef<CGBitmapParameters> (bitmapParameters));
				return Runtime.RetainAndAutoreleaseHandle (rv);
			}
			return IntPtr.Zero;
		}

		public delegate void OnFreeCallback (CGRenderingBufferProvider /* CGRenderingBufferProviderRef */ renderingBufferProvider, ref CGContentInfo contentInfo, ref CGBitmapParameters bitmapParameters);

		[UnmanagedCallersOnly]
		unsafe static void CGBitmapContextCreateAdaptive_OnFree (IntPtr block, IntPtr /* CGRenderingBufferProviderRef */ bufferingProviderRef, CGContentInfo* contentInfo, CGBitmapParameters* bitmapParameters)
		{
			var del = BlockLiteral.GetTarget<OnFreeCallback> (block);
			if (del is not null) {
				using var renderingBufferProvider = new CGRenderingBufferProvider (bufferingProviderRef, false);
				del (renderingBufferProvider, ref Unsafe.AsRef<CGContentInfo> (contentInfo), ref Unsafe.AsRef<CGBitmapParameters> (bitmapParameters));
			}
		}

		public delegate void OnErrorCallback (NSError error, ref CGContentInfo contentInfo, ref CGBitmapParameters bitmapParameters);

		[UnmanagedCallersOnly]
		unsafe static void CGBitmapContextCreateAdaptive_OnError (IntPtr block, IntPtr errorHandle, CGContentInfo* contentInfo, CGBitmapParameters* bitmapParameters)
		{
			var del = BlockLiteral.GetTarget<OnErrorCallback> (block);
			if (del is not null) {
				var error = Runtime.GetNSObject<NSError> (errorHandle, false)!;
				del (error, ref Unsafe.AsRef<CGContentInfo> (contentInfo), ref Unsafe.AsRef<CGBitmapParameters> (bitmapParameters));
			}
		}

		[SupportedOSPlatform ("ios26.0")]
		[SupportedOSPlatform ("tvos26.0")]
		[SupportedOSPlatform ("maccatalyst26.0")]
		[SupportedOSPlatform ("macos26.0")]
		[DllImport (Constants.CoreGraphicsLibrary)]
		static extern CGContentToneMappingInfo CGContextGetContentToneMappingInfo (IntPtr /* CGContextRef __nonnull */ context);

		[SupportedOSPlatform ("ios26.0")]
		[SupportedOSPlatform ("tvos26.0")]
		[SupportedOSPlatform ("maccatalyst26.0")]
		[SupportedOSPlatform ("macos26.0")]
		[DllImport (Constants.CoreGraphicsLibrary)]
		static extern void CGContextSetContentToneMappingInfo (IntPtr /* CGContextRef __nonnull */ context, CGContentToneMappingInfo info);

		/// <summary>Get or set the content tone mapping info.</summary>
		[SupportedOSPlatform ("ios26.0")]
		[SupportedOSPlatform ("tvos26.0")]
		[SupportedOSPlatform ("maccatalyst26.0")]
		[SupportedOSPlatform ("macos26.0")]
		public CGContentToneMappingInfo ContentToneMappingInfo {
			get => CGContextGetContentToneMappingInfo (GetCheckedHandle ());
			set => CGContextSetContentToneMappingInfo (GetCheckedHandle (), value);
		}

		[SupportedOSPlatform ("ios26.0")]
		[SupportedOSPlatform ("tvos26.0")]
		[SupportedOSPlatform ("maccatalyst26.0")]
		[SupportedOSPlatform ("macos26.0")]
		[DllImport (Constants.CoreGraphicsLibrary)]
		static extern void CGContextSynchronizeAttributes (IntPtr /* CGContextRef */ context);

		/// <summary>Synchronize destination attributes.</summary>
		[SupportedOSPlatform ("ios26.0")]
		[SupportedOSPlatform ("tvos26.0")]
		[SupportedOSPlatform ("maccatalyst26.0")]
		[SupportedOSPlatform ("macos26.0")]
		public void SynchronizeAttributes ()
		{
			CGContextSynchronizeAttributes (GetCheckedHandle ());
		}
#endif // !COREBUILD
	}
}
