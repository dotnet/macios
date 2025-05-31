// 
// VTPixelRotationSession.cs: VideoTools Pixel Rotation Session class 
//
// Authors: 
//    Israel Soto (issoto@microsoft.com)
// 
// Copyright 2022 Microsoft Corporation.
//

#nullable enable

using System;
using System.Runtime.InteropServices;

using CoreFoundation;
using ObjCRuntime;
using Foundation;
using CoreMedia;
using CoreVideo;

namespace VideoToolbox {
	[SupportedOSPlatform ("macos13.0")]
	[SupportedOSPlatform ("ios16.0")]
	[SupportedOSPlatform ("maccatalyst16.0")]
	[SupportedOSPlatform ("tvos16.0")]
	public class VTPixelRotationSession : VTSession {

		[DllImport (Constants.VideoToolboxLibrary)]
		extern static /* CFTypeID */ nint VTPixelRotationSessionGetTypeID ();
		public static nint GetTypeID () => VTPixelRotationSessionGetTypeID ();

		[Preserve (Conditional = true)]
		internal VTPixelRotationSession (NativeHandle handle, bool owns) : base (handle, owns)
		{
		}

		[DllImport (Constants.VideoToolboxLibrary)]
		extern static void VTPixelRotationSessionInvalidate (/* VTPixelRotationSessionRef */ IntPtr session);

		protected override void Dispose (bool disposing)
		{
			if (Handle != IntPtr.Zero)
				VTPixelRotationSessionInvalidate (Handle);

			base.Dispose (disposing);
		}

		[DllImport (Constants.VideoToolboxLibrary)]
		unsafe extern static VTStatus VTPixelRotationSessionCreate (
			/* CFAllocatorRef */ IntPtr allocator, /* can be null */
			/* VTPixelRotationSessionRef* */ IntPtr* pixelRotationSessionOut);

		public static VTPixelRotationSession? Create () => Create (null);

		public static VTPixelRotationSession? Create (CFAllocator? allocator)
		{
			VTStatus result;
			IntPtr ret;
			unsafe {
				result = VTPixelRotationSessionCreate (allocator.GetHandle (), &ret);
				GC.KeepAlive (allocator);
			}

			if (result == VTStatus.Ok && ret != IntPtr.Zero)
				return new VTPixelRotationSession (ret, true);

			return null;
		}

		[DllImport (Constants.VideoToolboxLibrary)]
		extern static VTStatus VTPixelRotationSessionRotateImage (
			/* VTPixelRotationSessionRef */ IntPtr session,
			/* CVPixelBuffer */ IntPtr sourceBuffer,
			/* CVPixelBuffer */ IntPtr destinationBuffer);

		public VTStatus RotateImage (CVPixelBuffer sourceBuffer, CVPixelBuffer destinationBuffer)
		{
			if (sourceBuffer is null)
				ObjCRuntime.ThrowHelper.ThrowArgumentNullException (nameof (sourceBuffer));

			if (destinationBuffer is null)
				ObjCRuntime.ThrowHelper.ThrowArgumentNullException (nameof (destinationBuffer));

			VTStatus status = VTPixelRotationSessionRotateImage (GetCheckedHandle (), sourceBuffer.Handle, destinationBuffer.Handle);
			GC.KeepAlive (sourceBuffer);
			GC.KeepAlive (destinationBuffer);
			return status;
		}

		public VTStatus SetRotationProperties (VTPixelRotationProperties options)
		{
			if (options is null)
				ObjCRuntime.ThrowHelper.ThrowArgumentNullException (nameof (options));

			return VTSessionSetProperties (Handle, options.Dictionary.Handle);
		}
	}
}
