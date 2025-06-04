// 
// VTDecompressionSession.cs: VideoTools Decompression Session class 
//
// Authors:
//    Alex Soto (alex.soto@xamarin.com
// 
// Copyright 2015 Xamarin Inc.
//

#nullable enable

using System;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;

using CoreFoundation;
using ObjCRuntime;
using Foundation;
using CoreMedia;
using CoreVideo;

namespace VideoToolbox {
	/// <summary>Turns compressed frames into uncompressed video frames.</summary>
	///     <remarks>To be added.</remarks>
	[SupportedOSPlatform ("ios")]
	[SupportedOSPlatform ("tvos")]
	[SupportedOSPlatform ("maccatalyst")]
	[SupportedOSPlatform ("macos")]
	public class VTDecompressionSession : VTSession {
		GCHandle callbackHandle;

		[Preserve (Conditional = true)]
		internal VTDecompressionSession (NativeHandle handle, bool owns) : base (handle, owns)
		{
		}

		/// <include file="../../docs/api/VideoToolbox/VTDecompressionSession.xml" path="/Documentation/Docs[@DocId='M:VideoToolbox.VTDecompressionSession.Dispose(System.Boolean)']/*" />
		protected override void Dispose (bool disposing)
		{
			if (Handle != IntPtr.Zero)
				VTDecompressionSessionInvalidate (Handle);

			if (callbackHandle.IsAllocated)
				callbackHandle.Free ();

			base.Dispose (disposing);
		}

		[StructLayout (LayoutKind.Sequential)]
		struct VTDecompressionOutputCallbackRecord {
			public unsafe delegate* unmanaged</* void* */ IntPtr, /* void* */ IntPtr, /* OSStatus */ VTStatus, VTDecodeInfoFlags, /* CVImageBuffer */ IntPtr, CMTime, CMTime, void> Proc;
			public IntPtr DecompressionOutputRefCon;
		}

		// sourceFrame: It seems it's only used as a parameter to be passed into DecodeFrame so no need to strong type it
		/// <param name="sourceFrame">To be added.</param>
		///     <param name="status">To be added.</param>
		///     <param name="flags">To be added.</param>
		///     <param name="buffer">To be added.</param>
		///     <param name="presentationTimeStamp">To be added.</param>
		///     <param name="presentationDuration">To be added.</param>
		///     <summary>Handler prototype to be called for each decompressed frame.</summary>
		///     <remarks>To be added.</remarks>
		public delegate void VTDecompressionOutputCallback (/* void* */ IntPtr sourceFrame, /* OSStatus */ VTStatus status, VTDecodeInfoFlags flags, CVImageBuffer buffer, CMTime presentationTimeStamp, CMTime presentationDuration);

		[UnmanagedCallersOnly]
		static void DecompressionCallback (IntPtr outputCallbackClosure, IntPtr sourceFrame, VTStatus status,
			VTDecodeInfoFlags infoFlags, IntPtr imageBufferPtr, CMTime presentationTimeStamp, CMTime presentationDuration)
		{
			var gch = GCHandle.FromIntPtr (outputCallbackClosure);
			var func = gch.Target as VTDecompressionOutputCallback;

			if (func is null)
				return;

			// Apple headers states that the callback should get a CVImageBuffer but it turned out that not all of them are a
			// CVImageBuffer, some can be instances of CVImageBuffer and others can be instances of CVPixelBuffer. So we go one 
			// step further in the inheritance hierarchy and supply the callback a CVPixelBuffer and the callback supplies 
			// to the developer a CVImageBuffer, so the developer can choose when to use one or the other and we mimic
			// what Apple provides on its headers.
			using (var sampleBuffer = new CVPixelBuffer (imageBufferPtr, false)) {
				func (sourceFrame, status, infoFlags, sampleBuffer, presentationTimeStamp, presentationDuration);
			}
		}

		[UnmanagedCallersOnly]
		static void NewDecompressionCallback (IntPtr outputCallbackClosure, IntPtr sourceFrame, VTStatus status,
			VTDecodeInfoFlags infoFlags, IntPtr imageBufferPtr, CMTime presentationTimeStamp, CMTime presentationDuration)
		{
			var gch = GCHandle.FromIntPtr (outputCallbackClosure);
			var func = gch.Target as VTDecompressionOutputCallback;

			if (func is null)
				return;

			// Apple headers states that the callback should get a CVImageBuffer but it turned out that not all of them are a
			// CVImageBuffer, some can be instances of CVImageBuffer and others can be instances of CVPixelBuffer. So we go one 
			// step further in the inheritance hierarchy and supply the callback a CVPixelBuffer and the callback supplies 
			// to the developer a CVImageBuffer, so the developer can choose when to use one or the other and we mimic
			// what Apple provides on its headers.
			using (var sampleBuffer = new CVPixelBuffer (imageBufferPtr, owns: false)) {
				func (sourceFrame, status, infoFlags, sampleBuffer, presentationTimeStamp, presentationDuration);
			}
		}

		[DllImport (Constants.VideoToolboxLibrary)]
		unsafe extern static VTStatus VTDecompressionSessionCreate (
			/* CFAllocatorRef */ IntPtr allocator, // can be null
			/* CMVideoFormatDescriptionRef */ IntPtr videoFormatDescription,
			/* CFDictionaryRef */ IntPtr videoDecoderSpecification, // can be null
			/* CFDictionaryRef */ IntPtr destinationImageBufferAttributes, // can be null
			/* const VTDecompressionOutputCallbackRecord* */ VTDecompressionOutputCallbackRecord* outputCallback,
			/* VTDecompressionSessionRef* */ IntPtr* decompressionSessionOut);

#if false // Disabling for now until we have some tests on this
		public static VTDecompressionSession Create (CMVideoFormatDescription formatDescription,
			VTVideoDecoderSpecification decoderSpecification = null, // hardware acceleration is default behavior on iOS. no opt-in required.
			NSDictionary destinationImageBufferAttributes = null) // Undocumented options, probably always null
		{
			if (formatDescription is null)
				ObjCRuntime.ThrowHelper.ThrowArgumentNullException (nameof (formatDescription));

			var callbackStruct = default (VTDecompressionOutputCallbackRecord);

			IntPtr ret;

			VTStatus result;
			unsafe {
				result = VTDecompressionSessionCreate (IntPtr.Zero, formatDescription.Handle,
					decoderSpecification is not null ? decoderSpecification.Dictionary.Handle : IntPtr.Zero,
					destinationImageBufferAttributes.GetHandle (),
					&callbackStruct,
					&ret);
			}

			return result == VTStatus.Ok && ret != IntPtr.Zero
				? new VTDecompressionSession (ret, true)
				: null;
		}
#endif

		/// <param name="outputCallback">To be added.</param>
		///         <param name="formatDescription">To be added.</param>
		///         <param name="decoderSpecification">To be added.</param>
		///         <param name="destinationImageBufferAttributes">To be added.</param>
		///         <summary>To be added.</summary>
		///         <returns>To be added.</returns>
		///         <remarks>To be added.</remarks>
		public static VTDecompressionSession? Create (VTDecompressionOutputCallback outputCallback,
								 CMVideoFormatDescription formatDescription,
								 VTVideoDecoderSpecification? decoderSpecification = null, // hardware acceleration is default behavior on iOS. no opt-in required.
								 CVPixelBufferAttributes? destinationImageBufferAttributes = null)
		{
			unsafe {
				return Create (outputCallback, formatDescription, decoderSpecification, destinationImageBufferAttributes?.Dictionary, &NewDecompressionCallback);
			}
		}

		unsafe static VTDecompressionSession? Create (VTDecompressionOutputCallback outputCallback,
							  CMVideoFormatDescription formatDescription,
							  VTVideoDecoderSpecification? decoderSpecification, // hardware acceleration is default behavior on iOS. no opt-in required.
							  NSDictionary? destinationImageBufferAttributes,
							  delegate* unmanaged</* void* */ IntPtr, /* void* */ IntPtr, /* OSStatus */ VTStatus, VTDecodeInfoFlags, /* CVImageBuffer */ IntPtr, CMTime, CMTime, void> cback)
		{
			if (outputCallback is null)
				ObjCRuntime.ThrowHelper.ThrowArgumentNullException (nameof (outputCallback));

			if (formatDescription is null)
				ObjCRuntime.ThrowHelper.ThrowArgumentNullException (nameof (formatDescription));

			var callbackHandle = GCHandle.Alloc (outputCallback);
			var callbackStruct = new VTDecompressionOutputCallbackRecord () {
				Proc = cback,
				DecompressionOutputRefCon = GCHandle.ToIntPtr (callbackHandle),
			};
			IntPtr ret;

			VTStatus result;
			unsafe {
				result = VTDecompressionSessionCreate (IntPtr.Zero, formatDescription.Handle,
					decoderSpecification.GetHandle (),
					destinationImageBufferAttributes.GetHandle (),
					&callbackStruct,
					&ret);
				GC.KeepAlive (formatDescription);
				GC.KeepAlive (decoderSpecification);
				GC.KeepAlive (destinationImageBufferAttributes);
			}

			if (result == VTStatus.Ok && ret != IntPtr.Zero)
				return new VTDecompressionSession (ret, true) {
					callbackHandle = callbackHandle,
				};

			callbackHandle.Free ();
			return null;
		}

		[DllImport (Constants.VideoToolboxLibrary)]
		extern static void VTDecompressionSessionInvalidate (IntPtr sesion);

		[DllImport (Constants.VideoToolboxLibrary)]
		unsafe extern static VTStatus VTDecompressionSessionDecodeFrame (
			/* VTDecompressionSessionRef */ IntPtr session,
			/* CMSampleBufferRef */ IntPtr sampleBuffer,
			/* VTDecodeFrameFlags */ VTDecodeFrameFlags decodeFlags,
			/* void* */ IntPtr sourceFrame,
			/* VTDecodeInfoFlags */ VTDecodeInfoFlags* infoFlagsOut);

		/// <param name="sampleBuffer">To be added.</param>
		///         <param name="decodeFlags">To be added.</param>
		///         <param name="sourceFrame">To be added.</param>
		///         <param name="infoFlags">To be added.</param>
		///         <summary>To be added.</summary>
		///         <returns>To be added.</returns>
		///         <remarks>To be added.</remarks>
		public VTStatus DecodeFrame (CMSampleBuffer sampleBuffer, VTDecodeFrameFlags decodeFlags, IntPtr sourceFrame, out VTDecodeInfoFlags infoFlags)
		{
			if (sampleBuffer is null)
				ObjCRuntime.ThrowHelper.ThrowArgumentNullException (nameof (sampleBuffer));

			infoFlags = default;
			unsafe {
				VTStatus status = VTDecompressionSessionDecodeFrame (GetCheckedHandle (), sampleBuffer.Handle, decodeFlags, sourceFrame, (VTDecodeInfoFlags*) Unsafe.AsPointer<VTDecodeInfoFlags> (ref infoFlags));
				GC.KeepAlive (sampleBuffer);
				return status;
			}
		}
#if false // Disabling for now until we have some tests on this
		[DllImport (Constants.VideoToolboxLibrary)]
		extern static VTStatus VTDecompressionSessionDecodeFrameWithOutputHandler (
			/* VTDecompressionSessionRef */ IntPtr session,
			/* CMSampleBufferRef */ IntPtr sampleBuffer,
			/* VTDecodeFrameFlags */ VTDecodeFrameFlags decodeFlags,
			/* VTDecodeInfoFlags */ out VTDecodeInfoFlags infoFlagsOut,
			/* VTDecompressionOutputHandler */ ref BlockLiteral outputHandler);

		public delegate void VTDecompressionOutputHandler (VTStatus status, VTDecodeInfoFlags infoFlags,
			CVImageBuffer imageBuffer, CMTime presentationTimeStamp, CMTime presentationDuration);

		unsafe delegate void VTDecompressionOutputHandlerProxy (BlockLiteral *block, VTStatus status, VTDecodeInfoFlags infoFlags,
			IntPtr imageBuffer, CMTime presentationTimeStamp, CMTime presentationDuration);

		static unsafe readonly VTDecompressionOutputHandlerProxy decompressionOutputHandlerTrampoline = VTDecompressionOutputHandlerTrampoline;

		[MonoPInvokeCallback (typeof (VTDecompressionOutputHandlerProxy))]
		static unsafe void VTDecompressionOutputHandlerTrampoline (BlockLiteral *block,
			VTStatus status, VTDecodeInfoFlags infoFlags, IntPtr imageBuffer,
			CMTime presentationTimeStamp, CMTime presentationDuration)
		{
			var del = (VTDecompressionOutputHandler)(block->Target);
			if (del is not null)
				del (status, infoFlags, new CVImageBuffer (imageBuffer, false), presentationTimeStamp, presentationDuration);
		}

		public VTStatus DecodeFrame (CMSampleBuffer sampleBuffer, VTDecodeFrameFlags decodeFlags,
			out VTDecodeInfoFlags infoFlags, VTDecompressionOutputHandler outputHandler)
		{
			if (sampleBuffer is null)
				ObjCRuntime.ThrowHelper.ThrowArgumentNullException (nameof (sampleBuffer));
			if (outputHandler is null)
				ObjCRuntime.ThrowHelper.ThrowArgumentNullException (nameof (outputHandler));

			var block = new BlockLiteral ();
			block.SetupBlockUnsafe (decompressionOutputHandlerTrampoline, outputHandler);
			try {
				VTStatus status = VTDecompressionSessionDecodeFrameWithOutputHandler (GetCheckedHandle (),
					sampleBuffer.Handle, decodeFlags, out infoFlags, ref block);
				GC.KeepAlive (sampleBuffer);
				return status;
			} finally {
				block.CleanupBlock ();
			}
		}
#endif
		[DllImport (Constants.VideoToolboxLibrary)]
		extern static VTStatus VTDecompressionSessionFinishDelayedFrames (IntPtr sesion);

		/// <summary>To be added.</summary>
		///         <returns>To be added.</returns>
		///         <remarks>To be added.</remarks>
		public VTStatus FinishDelayedFrames ()
		{
			return VTDecompressionSessionFinishDelayedFrames (GetCheckedHandle ());
		}

		[DllImport (Constants.VideoToolboxLibrary)]
		extern static VTStatus VTDecompressionSessionCanAcceptFormatDescription (IntPtr sesion, IntPtr newFormatDescriptor);

		/// <param name="newDescriptor">To be added.</param>
		///         <summary>To be added.</summary>
		///         <returns>To be added.</returns>
		///         <remarks>To be added.</remarks>
		public VTStatus CanAcceptFormatDescriptor (CMFormatDescription newDescriptor)
		{
			if (newDescriptor is null)
				ObjCRuntime.ThrowHelper.ThrowArgumentNullException (nameof (newDescriptor));

			VTStatus status = VTDecompressionSessionCanAcceptFormatDescription (GetCheckedHandle (), newDescriptor.Handle);
			GC.KeepAlive (newDescriptor);
			return status;
		}

		[DllImport (Constants.VideoToolboxLibrary)]
		extern static VTStatus VTDecompressionSessionWaitForAsynchronousFrames (IntPtr sesion);

		/// <summary>To be added.</summary>
		///         <returns>To be added.</returns>
		///         <remarks>To be added.</remarks>
		public VTStatus WaitForAsynchronousFrames ()
		{
			return VTDecompressionSessionWaitForAsynchronousFrames (GetCheckedHandle ());
		}

		[DllImport (Constants.VideoToolboxLibrary)]
		unsafe extern static VTStatus VTDecompressionSessionCopyBlackPixelBuffer (IntPtr sesion, IntPtr* pixelBufferOut);

		/// <param name="pixelBuffer">To be added.</param>
		///         <summary>To be added.</summary>
		///         <returns>To be added.</returns>
		///         <remarks>To be added.</remarks>
		public VTStatus CopyBlackPixelBuffer (out CVPixelBuffer? pixelBuffer)
		{
			VTStatus result;
			IntPtr ret;
			unsafe {
				result = VTDecompressionSessionCopyBlackPixelBuffer (GetCheckedHandle (), &ret);
			}
			pixelBuffer = Runtime.GetINativeObject<CVPixelBuffer> (ret, true);
			return result;
		}

		/// <param name="options">To be added.</param>
		///         <summary>To be added.</summary>
		///         <returns>To be added.</returns>
		///         <remarks>To be added.</remarks>
		public VTStatus SetDecompressionProperties (VTDecompressionProperties options)
		{
			if (options is null)
				ObjCRuntime.ThrowHelper.ThrowArgumentNullException (nameof (options));

			return VTSessionSetProperties (GetCheckedHandle (), options.Dictionary.Handle);
		}

		[SupportedOSPlatform ("macos")]
		[SupportedOSPlatform ("ios")]
		[SupportedOSPlatform ("tvos")]
		[SupportedOSPlatform ("maccatalyst")]
		[DllImport (Constants.VideoToolboxLibrary)]
		extern static byte VTIsHardwareDecodeSupported (CMVideoCodecType codecType);

		/// <param name="codecType">To be added.</param>
		///         <summary>To be added.</summary>
		///         <returns>To be added.</returns>
		///         <remarks>To be added.</remarks>
		[SupportedOSPlatform ("macos")]
		[SupportedOSPlatform ("ios")]
		[SupportedOSPlatform ("tvos")]
		[SupportedOSPlatform ("maccatalyst")]
		public static bool IsHardwareDecodeSupported (CMVideoCodecType codecType)
		{
			return VTIsHardwareDecodeSupported (codecType) != 0;
		}

		[SupportedOSPlatform ("macos14.0")]
		[SupportedOSPlatform ("ios17.0")]
		[SupportedOSPlatform ("tvos17.0")]
		[SupportedOSPlatform ("maccatalyst17.0")]
		[DllImport (Constants.VideoToolboxLibrary)]
		extern static /* Boolean */ byte VTIsStereoMVHEVCDecodeSupported ();

		/// <summary>Returns whether the current system supports stereo MV-HEVC decode.</summary>
		/// <returns>True if the current system supports stereo MV-HEVC decode, false otherwise.</returns>
		[SupportedOSPlatform ("macos14.0")]
		[SupportedOSPlatform ("ios17.0")]
		[SupportedOSPlatform ("tvos17.0")]
		[SupportedOSPlatform ("maccatalyst17.0")]
		public static bool IsStereoMvHevcDecodeSupported ()
		{
			return VTIsStereoMVHEVCDecodeSupported () != 0;
		}
	}
}
