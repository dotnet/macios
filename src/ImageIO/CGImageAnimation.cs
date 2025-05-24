//
// ImageIO - CGImageAnimation.cs
//
// Authors:
//	Whitney Schmidt  <whschm@microsoft.com>
//
// Copyright 2020, Microsoft Corp.
//

#nullable enable

using System;
using System.Runtime.InteropServices;
using CoreFoundation;
using CoreGraphics;
using Foundation;
using ObjCRuntime;

namespace ImageIO {
	[SupportedOSPlatform ("ios")]
	[SupportedOSPlatform ("maccatalyst")]
	[SupportedOSPlatform ("macos")]
	[SupportedOSPlatform ("tvos")]
	public static class CGImageAnimation {

		public delegate void CGImageSourceAnimationHandler (nint index, CGImage image, out bool stop);

		[SupportedOSPlatform ("macos")]
		[SupportedOSPlatform ("ios13.0")]
		[SupportedOSPlatform ("tvos13.0")]
		[SupportedOSPlatform ("maccatalyst")]
		[DllImport (Constants.ImageIOLibrary)]
		unsafe static extern /* OSStatus */ CGImageAnimationStatus CGAnimateImageAtURLWithBlock ( /* CFURLRef */ IntPtr url, /* CFDictionaryRef _iio_Nullable */ IntPtr options, /* CGImageSourceAnimationHandler */ BlockLiteral* block);

		[SupportedOSPlatform ("macos")]
		[SupportedOSPlatform ("ios13.0")]
		[SupportedOSPlatform ("tvos13.0")]
		[SupportedOSPlatform ("maccatalyst")]
		[DllImport (Constants.ImageIOLibrary)]
		unsafe static extern /* OSStatus */ CGImageAnimationStatus CGAnimateImageDataWithBlock ( /* CFDataRef _Nonnull */ IntPtr data, /* CFDictionaryRef _Nullable */ IntPtr options, /* CGImageSourceAnimationHandler _Nonnull */ BlockLiteral* block);

		[SupportedOSPlatform ("macos")]
		[SupportedOSPlatform ("ios13.0")]
		[SupportedOSPlatform ("tvos13.0")]
		[SupportedOSPlatform ("maccatalyst")]
		[BindingImpl (BindingImplOptions.Optimizable)]
		public static CGImageAnimationStatus AnimateImage (NSUrl url, CGImageAnimationOptions options, CGImageSourceAnimationHandler handler)
		{
			if (url is null)
				ObjCRuntime.ThrowHelper.ThrowArgumentNullException (nameof (url));
			if (handler is null)
				ObjCRuntime.ThrowHelper.ThrowArgumentNullException (nameof (handler));

			unsafe {
				delegate* unmanaged<IntPtr, nint, IntPtr, byte*, void> trampoline = &SDCGImageSourceAnimationBlock.Invoke;
				using var block = new BlockLiteral (trampoline, handler, typeof (SDCGImageSourceAnimationBlock), nameof (SDCGImageSourceAnimationBlock.Invoke));
				CGImageAnimationStatus status = CGAnimateImageAtURLWithBlock (url.Handle, options.GetHandle (), &block);
				GC.KeepAlive (url);
				GC.KeepAlive (options);
				return status;
			}
		}

		[SupportedOSPlatform ("macos")]
		[SupportedOSPlatform ("ios13.0")]
		[SupportedOSPlatform ("tvos13.0")]
		[SupportedOSPlatform ("maccatalyst")]
		[BindingImpl (BindingImplOptions.Optimizable)]
		public static CGImageAnimationStatus AnimateImage (NSData data, CGImageAnimationOptions options, CGImageSourceAnimationHandler handler)
		{
			if (data is null)
				ObjCRuntime.ThrowHelper.ThrowArgumentNullException (nameof (data));
			if (handler is null)
				ObjCRuntime.ThrowHelper.ThrowArgumentNullException (nameof (handler));

			unsafe {
				delegate* unmanaged<IntPtr, nint, IntPtr, byte*, void> trampoline = &SDCGImageSourceAnimationBlock.Invoke;
				using var block = new BlockLiteral (trampoline, handler, typeof (SDCGImageSourceAnimationBlock), nameof (SDCGImageSourceAnimationBlock.Invoke));
				CGImageAnimationStatus status = CGAnimateImageDataWithBlock (data.Handle, options.GetHandle (), &block);
				GC.KeepAlive (data);
				GC.KeepAlive (options);
				return status;
			}
		}

		//
		// This class bridges native block invocations that call into C#
		//
		static internal class SDCGImageSourceAnimationBlock {
			[UnmanagedCallersOnly]
			internal unsafe static void Invoke (IntPtr block, nint index, IntPtr image, byte* stop)
			{
				var del = BlockLiteral.GetTarget<CGImageSourceAnimationHandler> (block);
				if (del is not null) {
					del (index, new CoreGraphics.CGImage (image, false), out var stopValue);
					*stop = stopValue ? (byte) 1 : (byte) 0;
				} else
					*stop = 0;
			}
		} /* class SDCGImageSourceAnimationBlock */
	}

}
