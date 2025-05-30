// 
// VTCompressionSession.cs: VideoTools Compression Session class 
//
// Authors: 
//    Miguel de Icaza (miguel@xamarin.com)
//    Alex Soto (alex.soto@xamarin.com
// 
// Copyright 2014 Xamarin Inc.
//
using System;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;

using CoreFoundation;
using ObjCRuntime;
using Foundation;
using CoreMedia;
using CoreVideo;

#nullable enable

namespace VideoToolbox {
	/// <summary>Turns uncompressed frames into compressed video frames</summary>
	///     <remarks>To be added.</remarks>
	[SupportedOSPlatform ("ios")]
	[SupportedOSPlatform ("tvos")]
	[SupportedOSPlatform ("maccatalyst")]
	[SupportedOSPlatform ("macos")]
	public class VTCompressionSession : VTSession {
		GCHandle callbackHandle;

		[Preserve (Conditional = true)]
		internal VTCompressionSession (NativeHandle handle, bool owns) : base (handle, owns)
		{
		}

		/// <include file="../../docs/api/VideoToolbox/VTCompressionSession.xml" path="/Documentation/Docs[@DocId='M:VideoToolbox.VTCompressionSession.Dispose(System.Boolean)']/*" />
		protected override void Dispose (bool disposing)
		{
			if (Handle != IntPtr.Zero)
				VTCompressionSessionInvalidate (Handle);

			if (callbackHandle.IsAllocated)
				callbackHandle.Free ();

			base.Dispose (disposing);
		}

		// sourceFrame: It seems it's only used as a parameter to be passed into EncodeFrame so no need to strong type it
		/// <include file="../../docs/api/VideoToolbox.VTCompressionSession/VTCompressionOutputCallback.xml" path="/Documentation/Docs[@DocId='T:VideoToolbox.VTCompressionSession.VTCompressionOutputCallback']/*" />
		public delegate void VTCompressionOutputCallback (/* void* */ IntPtr sourceFrame, /* OSStatus */ VTStatus status, VTEncodeInfoFlags flags, CMSampleBuffer? buffer);

		static void CompressionCallback (IntPtr outputCallbackClosure, IntPtr sourceFrame, VTStatus status, VTEncodeInfoFlags infoFlags, IntPtr cmSampleBufferPtr, bool owns)
		{
			var gch = GCHandle.FromIntPtr (outputCallbackClosure);
			var func = (VTCompressionOutputCallback) gch.Target!;
			if (cmSampleBufferPtr == IntPtr.Zero) {
				func (sourceFrame, status, infoFlags, null);
			} else {
				using (var sampleBuffer = new CMSampleBuffer (cmSampleBufferPtr, owns: owns))
					func (sourceFrame, status, infoFlags, sampleBuffer);
			}
		}

		/// <param name="width">Frame width in pixels.</param>
		///         <param name="height">Frame height in pixels.</param>
		///         <param name="codecType">Encoder to use to compress the frames.</param>
		///         <param name="compressionOutputCallback">Method that will be invoked to process a compressed frame.  See the delegate type for more information on the received parameters.</param>
		///         <param name="encoderSpecification">Parameters to choose the encoder, or null to let VideoToolbox choose it.</param>
		///         <param name="sourceImageBufferAttributes">The Dictionary property extracted from a <see cref="CoreVideo.CVPixelBufferAttributes" /> type, or an NSDictionary with the desired CoreVideo Pixel Buffer Attributes values.</param>
		///         <summary>Creates a compression session</summary>
		///         <returns>To be added.</returns>
		///         <remarks>The <paramref name="compressionOutputCallback" /> will be invoked for each frame in decode order, not necessarily the display order.</remarks>
		public static VTCompressionSession? Create (int width, int height, CMVideoCodecType codecType,
			VTCompressionOutputCallback compressionOutputCallback,
			VTVideoEncoderSpecification? encoderSpecification = null, // hardware acceleration is default behavior on iOS. no opt-in required.
			NSDictionary? sourceImageBufferAttributes = null)
		{
			unsafe {
				return Create (width, height, codecType, compressionOutputCallback, encoderSpecification, sourceImageBufferAttributes, &NewCompressionCallback);
			}
		}

		[UnmanagedCallersOnly]
		static void NewCompressionCallback (IntPtr outputCallbackClosure, IntPtr sourceFrame, VTStatus status, VTEncodeInfoFlags infoFlags, IntPtr cmSampleBufferPtr)
		{
			CompressionCallback (outputCallbackClosure, sourceFrame, status, infoFlags, cmSampleBufferPtr, false);
		}

		[DllImport (Constants.VideoToolboxLibrary)]
		unsafe extern static VTStatus VTCompressionSessionCreate (
			/* CFAllocatorRef */ IntPtr allocator, /* can be null */
			/* int32_t */ int width,
			/* int32_t */ int height,
			/* CMVideoCodecType */ CMVideoCodecType codecType,
			/* CFDictionaryRef */ IntPtr dictionaryEncoderSpecification, /* can be null */
			/* CFDictionaryRef */ IntPtr dictionarySourceImageBufferAttributes, /* can be null */
			/* CFDictionaryRef */ IntPtr compressedDataAllocator, /* can be null */
			/* VTCompressionOutputCallback */ delegate* unmanaged</* void* CM_NULLABLE */ IntPtr, /* void* CM_NULLABLE */ IntPtr, /* OSStatus */ VTStatus, VTEncodeInfoFlags, /* CMSampleBufferRef CM_NULLABLE */ IntPtr, void> outputCallback,
			/* void* */ IntPtr outputCallbackClosure,
			/* VTCompressionSessionRef* */ IntPtr* compressionSessionOut);

#if false // Disabling for now until we have some tests on this
		public static VTCompressionSession? Create (int width, int height, CMVideoCodecType codecType,
			VTVideoEncoderSpecification? encoderSpecification = null,
			NSDictionary? sourceImageBufferAttributes = null)
		{
			return Create (width, height, codecType, null,
				encoderSpecification, sourceImageBufferAttributes);
		}
#endif
		/// <param name="width">To be added.</param>
		///         <param name="height">To be added.</param>
		///         <param name="codecType">To be added.</param>
		///         <param name="compressionOutputCallback">To be added.</param>
		///         <param name="encoderSpecification">To be added.</param>
		///         <param name="sourceImageBufferAttributes">To be added.</param>
		///         <summary>To be added.</summary>
		///         <returns>To be added.</returns>
		///         <remarks>To be added.</remarks>
		public static VTCompressionSession? Create (int width, int height, CMVideoCodecType codecType,
			VTCompressionOutputCallback compressionOutputCallback,
			VTVideoEncoderSpecification? encoderSpecification, // hardware acceleration is default behavior on iOS. no opt-in required.
			CVPixelBufferAttributes? sourceImageBufferAttributes)
		{
			return Create (width, height, codecType, compressionOutputCallback, encoderSpecification, sourceImageBufferAttributes?.Dictionary);
		}

		unsafe static VTCompressionSession? Create (int width, int height, CMVideoCodecType codecType,
			VTCompressionOutputCallback compressionOutputCallback,
			VTVideoEncoderSpecification? encoderSpecification, // hardware acceleration is default behavior on iOS. no opt-in required.
				NSDictionary? sourceImageBufferAttributes, // Undocumented options, probably always null
				delegate* unmanaged</* void* CM_NULLABLE */ IntPtr, /* void* CM_NULLABLE */ IntPtr, /* OSStatus */ VTStatus, VTEncodeInfoFlags, /* CMSampleBufferRef CM_NULLABLE */ IntPtr, void> staticCback)
		{
			var callbackHandle = default (GCHandle);
			if (compressionOutputCallback is not null)
				callbackHandle = GCHandle.Alloc (compressionOutputCallback);

			IntPtr ret;
			var result = VTCompressionSessionCreate (IntPtr.Zero, width, height, codecType,
				encoderSpecification.GetHandle (),
				sourceImageBufferAttributes.GetHandle (),
				IntPtr.Zero,
				callbackHandle.IsAllocated ? (staticCback) : null,
				GCHandle.ToIntPtr (callbackHandle),
				&ret);
			GC.KeepAlive (encoderSpecification);
			GC.KeepAlive (sourceImageBufferAttributes);

			if (result == VTStatus.Ok && ret != IntPtr.Zero)
				return new VTCompressionSession (ret, true) {
					callbackHandle = callbackHandle,
				};

			if (callbackHandle.IsAllocated)
				callbackHandle.Free ();

			return null;
		}

		[DllImport (Constants.VideoToolboxLibrary)]
		extern static void VTCompressionSessionInvalidate (IntPtr handle);

		[DllImport (Constants.VideoToolboxLibrary)]
		extern static IntPtr /* cvpixelbufferpoolref */ VTCompressionSessionGetPixelBufferPool (IntPtr handle);

		/// <summary>To be added.</summary>
		///         <returns>To be added.</returns>
		///         <remarks>To be added.</remarks>
		public CVPixelBufferPool? GetPixelBufferPool ()
		{
			var ret = VTCompressionSessionGetPixelBufferPool (GetCheckedHandle ());

			if (ret != IntPtr.Zero)
				return new CVPixelBufferPool (ret, false);

			return null;
		}

		[SupportedOSPlatform ("macos")]
		[SupportedOSPlatform ("ios")]
		[SupportedOSPlatform ("tvos")]
		[SupportedOSPlatform ("maccatalyst")]
		[DllImport (Constants.VideoToolboxLibrary)]
		extern static VTStatus VTCompressionSessionPrepareToEncodeFrames (IntPtr handle);

		/// <summary>To be added.</summary>
		///         <returns>To be added.</returns>
		///         <remarks>To be added.</remarks>
		[SupportedOSPlatform ("macos")]
		[SupportedOSPlatform ("ios")]
		[SupportedOSPlatform ("tvos")]
		[SupportedOSPlatform ("maccatalyst")]
		public VTStatus PrepareToEncodeFrames ()
		{
			return VTCompressionSessionPrepareToEncodeFrames (GetCheckedHandle ());
		}

		[DllImport (Constants.VideoToolboxLibrary)]
		unsafe extern static VTStatus VTCompressionSessionEncodeFrame (
			/* VTCompressionSessionRef */ IntPtr session,
			/* CVImageBufferRef */ IntPtr imageBuffer,
			/* CMTime */ CMTime presentation,
			/* CMTime */ CMTime duration, // can ve CMTime.Invalid
			/* CFDictionaryRef */ IntPtr dict, // can be null, undocumented options
			/* void* */ IntPtr sourceFrame,
			/* VTEncodeInfoFlags */ VTEncodeInfoFlags* flags);

		public VTStatus EncodeFrame (CVImageBuffer imageBuffer, CMTime presentationTimestamp, CMTime duration,
			NSDictionary frameProperties, CVImageBuffer sourceFrame, out VTEncodeInfoFlags infoFlags)
		{
			if (sourceFrame is null)
				ObjCRuntime.ThrowHelper.ThrowArgumentNullException (nameof (sourceFrame));

			VTStatus status = EncodeFrame (imageBuffer, presentationTimestamp, duration, frameProperties, sourceFrame.GetCheckedHandle (), out infoFlags);
			GC.KeepAlive (sourceFrame);
			return status;
		}

		/// <param name="imageBuffer">To be added.</param>
		///         <param name="presentationTimestamp">To be added.</param>
		///         <param name="duration">To be added.</param>
		///         <param name="frameProperties">To be added.</param>
		///         <param name="sourceFrame">To be added.</param>
		///         <param name="infoFlags">To be added.</param>
		///         <summary>To be added.</summary>
		///         <returns>To be added.</returns>
		///         <remarks>To be added.</remarks>
		public VTStatus EncodeFrame (CVImageBuffer imageBuffer, CMTime presentationTimestamp, CMTime duration,
			NSDictionary frameProperties, IntPtr sourceFrame, out VTEncodeInfoFlags infoFlags)
		{
			if (imageBuffer is null)
				ObjCRuntime.ThrowHelper.ThrowArgumentNullException (nameof (imageBuffer));

			infoFlags = default;
			unsafe {
				VTStatus status = VTCompressionSessionEncodeFrame (GetCheckedHandle (), imageBuffer.Handle, presentationTimestamp, duration,
					frameProperties.GetHandle (),
					sourceFrame, (VTEncodeInfoFlags*) Unsafe.AsPointer<VTEncodeInfoFlags> (ref infoFlags));
				GC.KeepAlive (imageBuffer);
				GC.KeepAlive (frameProperties);
				return status;
			}
		}

#if false // Disabling for now until we have some tests on this
		[DllImport (Constants.VideoToolboxLibrary)]
		extern static VTStatus VTCompressionSessionEncodeFrameWithOutputHandler (
			/* VTCompressionSessionRef */ IntPtr session,
			/* CVImageBufferRef */ IntPtr imageBuffer,
			/* CMTime */ CMTime presentation,
			/* CMTime */ CMTime duration, // can ve CMTime.Invalid
			/* CFDictionaryRef */ IntPtr dict, // can be null, undocumented options
			/* VTEncodeInfoFlags */ out VTEncodeInfoFlags flags,
			/* VTCompressionOutputHandler */ ref BlockLiteral outputHandler);

		public delegate void VTCompressionOutputHandler (VTStatus status, VTEncodeInfoFlags infoFlags, CMSampleBuffer sampleBuffer);

		unsafe delegate void VTCompressionOutputHandlerProxy (BlockLiteral *block,
			VTStatus status, VTEncodeInfoFlags infoFlags, IntPtr sampleBuffer);

		static unsafe readonly VTCompressionOutputHandlerProxy compressionOutputHandlerTrampoline = VTCompressionOutputHandlerTrampoline;

		[MonoPInvokeCallback (typeof (VTCompressionOutputHandlerProxy))]
		static unsafe void VTCompressionOutputHandlerTrampoline (BlockLiteral *block,
			VTStatus status, VTEncodeInfoFlags infoFlags, IntPtr sampleBuffer)
		{
			var del = (VTCompressionOutputHandler)(block->Target);
			if (del is not null)
				del (status, infoFlags, new CMSampleBuffer (sampleBuffer));
		}

		public VTStatus EncodeFrame (CVImageBuffer imageBuffer, CMTime presentationTimestamp, CMTime duration,
			NSDictionary frameProperties, CVImageBuffer sourceFrame, out VTEncodeInfoFlags infoFlags,
			VTCompressionOutputHandler outputHandler)
		{
			if (sourceFrame is null)
				ObjCRuntime.ThrowHelper.ThrowArgumentNullException (nameof (sourceFrame));

			VTStatus status = EncodeFrame (imageBuffer, presentationTimestamp, duration, frameProperties, sourceFrame.GetCheckedHandle (), out infoFlags, outputHandler);
			GC.KeepAlive (sourceFrame);
			return status;
		}

		public VTStatus EncodeFrame (CVImageBuffer imageBuffer, CMTime presentationTimestamp, CMTime duration,
			NSDictionary frameProperties, IntPtr sourceFrame, out VTEncodeInfoFlags infoFlags,
			VTCompressionOutputHandler outputHandler)
		{
			if (imageBuffer is null)
				ObjCRuntime.ThrowHelper.ThrowArgumentNullException (nameof (imageBuffer));
			if (outputHandler is null)
				ObjCRuntime.ThrowHelper.ThrowArgumentNullException (nameof (outputHandler));

			var block = new BlockLiteral ();
			block.SetupBlockUnsafe (compressionOutputHandlerTrampoline, outputHandler);

			try {
				VTStatus status = VTCompressionSessionEncodeFrameWithOutputHandler (GetCheckedHandle (),
					imageBuffer.Handle, presentationTimestamp, duration,
					frameProperties.GetHandle (),
					out infoFlags, ref block);
				GC.KeepAlive (imageBuffer);
				GC.KeepAlive (frameProperties);
				return status;
			} finally {
				block.CleanupBlock ();
			}
		}
#endif
		[DllImport (Constants.VideoToolboxLibrary)]
		extern static VTStatus VTCompressionSessionCompleteFrames (IntPtr session, CMTime completeUntilPresentationTimeStamp);

		/// <param name="completeUntilPresentationTimeStamp">To be added.</param>
		///         <summary>To be added.</summary>
		///         <returns>To be added.</returns>
		///         <remarks>To be added.</remarks>
		public VTStatus CompleteFrames (CMTime completeUntilPresentationTimeStamp)
		{
			return VTCompressionSessionCompleteFrames (GetCheckedHandle (), completeUntilPresentationTimeStamp);
		}

		[SupportedOSPlatform ("macos")]
		[SupportedOSPlatform ("ios")]
		[SupportedOSPlatform ("tvos")]
		[SupportedOSPlatform ("maccatalyst")]
		[DllImport (Constants.VideoToolboxLibrary)]
		extern static VTStatus VTCompressionSessionBeginPass (IntPtr session, VTCompressionSessionOptionFlags flags, IntPtr reserved);

		/// <param name="flags">To be added.</param>
		///         <summary>To be added.</summary>
		///         <returns>To be added.</returns>
		///         <remarks>To be added.</remarks>
		[SupportedOSPlatform ("macos")]
		[SupportedOSPlatform ("ios")]
		[SupportedOSPlatform ("tvos")]
		[SupportedOSPlatform ("maccatalyst")]
		public VTStatus BeginPass (VTCompressionSessionOptionFlags flags)
		{
			return VTCompressionSessionBeginPass (GetCheckedHandle (), flags, IntPtr.Zero);
		}

		[SupportedOSPlatform ("macos")]
		[SupportedOSPlatform ("ios")]
		[SupportedOSPlatform ("tvos")]
		[SupportedOSPlatform ("maccatalyst")]
		[DllImport (Constants.VideoToolboxLibrary)]
		unsafe extern static VTStatus VTCompressionSessionEndPass (IntPtr session, byte* furtherPassesRequestedOut, IntPtr reserved);

		/// <param name="furtherPassesRequested">To be added.</param>
		///         <summary>To be added.</summary>
		///         <returns>To be added.</returns>
		///         <remarks>To be added.</remarks>
		[SupportedOSPlatform ("macos")]
		[SupportedOSPlatform ("ios")]
		[SupportedOSPlatform ("tvos")]
		[SupportedOSPlatform ("maccatalyst")]
		public VTStatus EndPass (out bool furtherPassesRequested)
		{
			byte b;
			VTStatus result;
			unsafe {
				result = VTCompressionSessionEndPass (GetCheckedHandle (), &b, IntPtr.Zero);
			}
			furtherPassesRequested = b != 0;
			return result;
		}

		// Like EndPass, but this will be the final pass, so the encoder will skip the evaluation.
		/// <summary>To be added.</summary>
		///         <returns>To be added.</returns>
		///         <remarks>To be added.</remarks>
		public VTStatus EndPassAsFinal ()
		{
			unsafe {
				return VTCompressionSessionEndPass (GetCheckedHandle (), null, IntPtr.Zero);
			}
		}

		[SupportedOSPlatform ("macos")]
		[SupportedOSPlatform ("ios")]
		[SupportedOSPlatform ("tvos")]
		[SupportedOSPlatform ("maccatalyst")]
		[DllImport (Constants.VideoToolboxLibrary)]
		unsafe extern static VTStatus VTCompressionSessionGetTimeRangesForNextPass (
			/* VTCompressionSessionRef */ IntPtr session,
			/* CMItemCount* */ int* itemCount,
			/* const CMTimeRange** */ IntPtr* target);

		/// <param name="timeRanges">To be added.</param>
		///         <summary>To be added.</summary>
		///         <returns>To be added.</returns>
		///         <remarks>To be added.</remarks>
		[SupportedOSPlatform ("macos")]
		[SupportedOSPlatform ("ios")]
		[SupportedOSPlatform ("tvos")]
		[SupportedOSPlatform ("maccatalyst")]
		public VTStatus GetTimeRangesForNextPass (out CMTimeRange []? timeRanges)
		{
			VTStatus v;
			int count;
			IntPtr target;
			unsafe {
				v = VTCompressionSessionGetTimeRangesForNextPass (GetCheckedHandle (), &count, &target);
			}
			if (v != VTStatus.Ok) {
				timeRanges = null;
				return v;
			}
			timeRanges = new CMTimeRange [count];
			unsafe {
				CMTimeRange* ptr = (CMTimeRange*) target;
				for (int i = 0; i < count; i++)
					timeRanges [i] = ptr [i];
			}
			return VTStatus.Ok;
		}

		/// <param name="options">To be added.</param>
		///         <summary>To be added.</summary>
		///         <returns>To be added.</returns>
		///         <remarks>To be added.</remarks>
		public VTStatus SetCompressionProperties (VTCompressionProperties options)
		{
			if (options is null)
				ObjCRuntime.ThrowHelper.ThrowArgumentNullException (nameof (options));

			return VTSessionSetProperties (GetCheckedHandle (), options.Dictionary.Handle);
		}

		[SupportedOSPlatform ("macos14.0")]
		[SupportedOSPlatform ("ios17.0")]
		[SupportedOSPlatform ("tvos17.0")]
		[SupportedOSPlatform ("maccatalyst17.0")]
		[DllImport (Constants.VideoToolboxLibrary)]
		extern static /* Boolean */ byte VTIsStereoMVHEVCEncodeSupported ();

		/// <summary>Returns whether the current system supports stereo MV-HEVC encode.</summary>
		/// <returns>True if the current system supports stereo MV-HEVC encode, false otherwise.</returns>
		[SupportedOSPlatform ("macos14.0")]
		[SupportedOSPlatform ("ios17.0")]
		[SupportedOSPlatform ("tvos17.0")]
		[SupportedOSPlatform ("maccatalyst17.0")]
		public static bool IsStereoMvHevcEncodeSupported ()
		{
			return VTIsStereoMVHEVCEncodeSupported () != 0;
		}
	}
}
