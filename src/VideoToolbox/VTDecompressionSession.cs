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
	[SupportedOSPlatform ("ios")]
	[SupportedOSPlatform ("tvos")]
	[SupportedOSPlatform ("maccatalyst")]
	[SupportedOSPlatform ("macos")]
	public class VTDecompressionSession : VTSession {
		GCHandle? callbackHandle;

		[Preserve (Conditional = true)]
		internal VTDecompressionSession (NativeHandle handle, bool owns) : base (handle, owns)
		{
		}

		protected override void Dispose (bool disposing)
		{
			if (Handle != IntPtr.Zero)
				VTDecompressionSessionInvalidate (Handle);

			if (callbackHandle is not null && callbackHandle.Value.IsAllocated) {
				callbackHandle.Value.Free ();
				callbackHandle = null;
			}

			base.Dispose (disposing);
		}

		[StructLayout (LayoutKind.Sequential)]
		struct VTDecompressionOutputCallbackRecord {
			public unsafe delegate* unmanaged</* void* */ IntPtr, /* void* */ IntPtr, /* OSStatus */ VTStatus, VTDecodeInfoFlags, /* CVImageBuffer */ IntPtr, CMTime, CMTime, void> Proc;
			public IntPtr DecompressionOutputRefCon;
		}

		/// <summary>A delegate that will be called for each decompressed frame.</summary>
		/// <param name="sourceFrame">The token passed starting the decoding operation.</param>
		/// <param name="status">Status code indicating if the operation was successful or not.</param>
		/// <param name="flags">Contains information about the decoding operation.</param>
		/// <param name="buffer">Contains a pointer to the decoded buffer if successful. A <see langword="null" /> value indicates an error.</param>
		/// <param name="presentationTimeStamp">The presentation timestamp of the decoded frame.</param>
		/// <param name="presentationDuration">The duration of the decoded frame.</param>
		public delegate void VTDecompressionOutputCallback (/* void* */ IntPtr sourceFrame, /* OSStatus */ VTStatus status, VTDecodeInfoFlags flags, CVImageBuffer? buffer, CMTime presentationTimeStamp, CMTime presentationDuration);

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
			using var sampleBuffer = imageBufferPtr == IntPtr.Zero ? null : new CVPixelBuffer (imageBufferPtr, owns: false);
			func (sourceFrame, status, infoFlags, sampleBuffer, presentationTimeStamp, presentationDuration);
		}

		[DllImport (Constants.VideoToolboxLibrary)]
		unsafe extern static VTStatus VTDecompressionSessionCreate (
			/* CFAllocatorRef */ IntPtr allocator, // can be null
			/* CMVideoFormatDescriptionRef */ IntPtr videoFormatDescription,
			/* CFDictionaryRef */ IntPtr videoDecoderSpecification, // can be null
			/* CFDictionaryRef */ IntPtr destinationImageBufferAttributes, // can be null
			/* const VTDecompressionOutputCallbackRecord* CM_NULLABLE */ VTDecompressionOutputCallbackRecord* outputCallback,
			/* VTDecompressionSessionRef* */ IntPtr* decompressionSessionOut);


		/// <summary>Create a new <see cref="VTDecompressionSession" /> instance.</summary>
		/// <param name="outputCallback">To be added.</param>
		/// <param name="formatDescription">A format description for the source video frames.</param>
		/// <param name="decoderSpecification">Optionally specify which decoder to use</param>
		/// <param name="destinationImageBufferAttributes">Optionally specify any requirements for the decoded frames.</param>
		/// <returns>A new <see cref="VTDecompressionSession" /> instance if successful, <see langword="null" /> otherwise.</returns>
		public static VTDecompressionSession? Create (VTDecompressionOutputCallback? outputCallback,
								 CMVideoFormatDescription formatDescription,
								 VTVideoDecoderSpecification? decoderSpecification = null, // hardware acceleration is default behavior on iOS. no opt-in required.
								 CVPixelBufferAttributes? destinationImageBufferAttributes = null)
		{
			if (formatDescription is null)
				ObjCRuntime.ThrowHelper.ThrowArgumentNullException (nameof (formatDescription));

			GCHandle? callbackHandle = null;
			VTDecompressionOutputCallbackRecord callbackStruct = default;

			if (outputCallback is not null) {
				callbackHandle = GCHandle.Alloc (outputCallback);
				unsafe {
					callbackStruct.Proc = &DecompressionCallback;
				}
				callbackStruct.DecompressionOutputRefCon = GCHandle.ToIntPtr (callbackHandle.Value);
			}
			IntPtr ret;

			VTStatus result;
			unsafe {
				result = VTDecompressionSessionCreate (IntPtr.Zero, formatDescription.Handle,
					decoderSpecification.GetHandle (),
					destinationImageBufferAttributes.GetHandle (),
					outputCallback is null ? null : &callbackStruct,
					&ret);
				GC.KeepAlive (formatDescription);
				GC.KeepAlive (decoderSpecification);
				GC.KeepAlive (destinationImageBufferAttributes);
			}

			if (result == VTStatus.Ok && ret != IntPtr.Zero)
				return new VTDecompressionSession (ret, true) {
					callbackHandle = callbackHandle,
				};

			if (callbackHandle is not null)
				callbackHandle.Value.Free ();
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

		/// <summary>Decode a video frame.</summary>
		/// <param name="sampleBuffer">A sample buffer with one or more video frames.</param>
		/// <param name="decodeFlags">Any decoding flags to be passed to the decoder.</param>
		/// <param name="sourceFrame">A user-provided value that is passed to any decoding callbacks.</param>
		/// <param name="infoFlags">Upon return, any informational flags about the decode operation.</param>
		/// <returns><see cref="VTStatus.Ok" /> if successful, or an error code otherwise.</returns>
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

		[DllImport (Constants.VideoToolboxLibrary)]
		unsafe extern static VTStatus VTDecompressionSessionDecodeFrameWithOutputHandler (
			/* VTDecompressionSessionRef */ IntPtr session,
			/* CMSampleBufferRef */ IntPtr sampleBuffer,
			/* VTDecodeFrameFlags */ VTDecodeFrameFlags decodeFlags,
			/* VTDecodeInfoFlags */ VTDecodeInfoFlags* infoFlagsOut,
			/* VTDecompressionOutputHandler */ BlockLiteral* outputHandler);

		/// <summary>Decode a video frame.</summary>
		/// <param name="sampleBuffer">A sample buffer with one or more video frames.</param>
		/// <param name="decodeFlags">Any decoding flags to be passed to the decoder.</param>
		/// <param name="infoFlags">Upon return, any informational flags about the decode operation.</param>
		/// <param name="outputHandler">A callback that will be called when the decoding operation is complete.</param>
		/// <returns><see cref="VTStatus.Ok" /> if successful, or an error code otherwise.</returns>
		[BindingImpl (BindingImplOptions.Optimizable)]
		public unsafe VTStatus DecodeFrame (CMSampleBuffer sampleBuffer, VTDecodeFrameFlags decodeFlags, out VTDecodeInfoFlags infoFlags, VTDecompressionOutputHandler outputHandler)
		{
			delegate* unmanaged<BlockLiteral*, VTStatus, VTDecodeInfoFlags, IntPtr, CMTime, CMTime, void> trampoline = &VTDecompressionOutputHandlerTrampoline;
			using var outputHandlerBlock = new BlockLiteral (trampoline, outputHandler, typeof (VTDecompressionSession), nameof (VTDecompressionOutputHandlerTrampoline));

			infoFlags = default;

			var rv = VTDecompressionSessionDecodeFrameWithOutputHandler (
						GetCheckedHandle (),
						sampleBuffer.GetNonNullHandle (nameof (sampleBuffer)),
						decodeFlags,
						(VTDecodeInfoFlags*) Unsafe.AsPointer<VTDecodeInfoFlags> (ref infoFlags),
						&outputHandlerBlock);
			GC.KeepAlive (sampleBuffer);
			return rv;
		}

		[DllImport (Constants.VideoToolboxLibrary)]
		extern static VTStatus VTDecompressionSessionFinishDelayedFrames (IntPtr sesion);

		/// <summary>Request the decoder to decode all delayed frames.</summary>
		/// <returns><see cref="VTStatus.Ok" /> if successful, or an error code otherwise.</returns>
		public VTStatus FinishDelayedFrames ()
		{
			return VTDecompressionSessionFinishDelayedFrames (GetCheckedHandle ());
		}

		[DllImport (Constants.VideoToolboxLibrary)]
		extern static byte VTDecompressionSessionCanAcceptFormatDescription (IntPtr sesion, IntPtr newFormatDescriptor);

#if XAMCORE_5_0
		/// <summary>Checks whether the <see cref="VTDecompressionSession" /> can decode frames of the specified format.</summary>
		/// <param name="newDescriptor">The format to check.</param>
		/// <returns><see langword="true" /> if the format is supported, or <see langword="false" /> otherwise.</returns>
		public bool CanAcceptFormatDescriptor (CMFormatDescription newDescriptor)
#else
		/// <summary>Checks whether the <see cref="VTDecompressionSession" /> can decode frames of the specified format.</summary>
		/// <param name="newDescriptor">The format to check.</param>
		/// <returns><see cref="VTStatus.Ok" /> if the format is supported, or <see cref="VTStatus.Parameter" /> otherwise.</returns>
		public VTStatus CanAcceptFormatDescriptor (CMFormatDescription newDescriptor)
#endif
		{
			if (newDescriptor is null)
				ObjCRuntime.ThrowHelper.ThrowArgumentNullException (nameof (newDescriptor));

#if XAMCORE_5_0
			var status = VTDecompressionSessionCanAcceptFormatDescription (GetCheckedHandle (), newDescriptor.Handle) != 0;
#else
			var rv = VTDecompressionSessionCanAcceptFormatDescription (GetCheckedHandle (), newDescriptor.Handle) != 0;
			var status = rv ? VTStatus.Ok : VTStatus.Parameter;
#endif
			GC.KeepAlive (newDescriptor);
			return status;
		}

		[DllImport (Constants.VideoToolboxLibrary)]
		extern static VTStatus VTDecompressionSessionWaitForAsynchronousFrames (IntPtr sesion);

		/// <summary>Wait until all asynchronous frames have been decoded.</summary>
		/// <returns><see cref="VTStatus.Ok" /> if successful, or an error code otherwise.</returns>
		public VTStatus WaitForAsynchronousFrames ()
		{
			return VTDecompressionSessionWaitForAsynchronousFrames (GetCheckedHandle ());
		}

		[DllImport (Constants.VideoToolboxLibrary)]
		unsafe extern static VTStatus VTDecompressionSessionCopyBlackPixelBuffer (IntPtr sesion, IntPtr* pixelBufferOut);

		/// <summary>Get a black pixel buffer from the session.</summary>
		/// <param name="pixelBuffer">Upon return, the black pixel buffer if successful.</param>
		/// <returns><see cref="VTStatus.Ok" /> if successful, or an error code otherwise.</returns>
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

		/// <summary>Set any decompression properties.</summary>
		/// <param name="options">The decompression properties to set.</param>
		/// <returns><see cref="VTStatus.Ok" /> if successful, or an error code otherwise.</returns>
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

		/// <summary>Checks whether the specified <paramref name="codecType" /> can be decoded in hardware.</summary>
		/// <param name="codecType">The codec to check if the current system can decode in hardware.</param>
		/// <returns><see langword="true" /> if hardware decoding is supported for the specified codec, <see langword="false" /> otherwise.</returns>
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

#if !__TVOS__
		[SupportedOSPlatform ("macos14.0")]
		[SupportedOSPlatform ("ios17.0")]
		[SupportedOSPlatform ("maccatalyst17.0")]
		[UnsupportedOSPlatform ("tvos")]
		[DllImport (Constants.VideoToolboxLibrary)]
		unsafe extern static /* OSStatus */ VTStatus VTDecompressionSessionDecodeFrameWithMultiImageCapableOutputHandler (
				IntPtr /* CM_NONNULL VTDecompressionSessionRef */ session,
				IntPtr /* CM_NONNULL CMSampleBufferRef */ sampleBuffer,
				VTDecodeFrameFlags decodeFlags, // bit 0 is enableAsynchronousDecompression
				VTDecodeInfoFlags* /* VTDecodeInfoFlags * CM_NULLABLE */ infoFlagsOut,
				BlockLiteral* /* CM_NONNULL VTDecompressionMultiImageCapableOutputHandler */ multiImageCapableOutputHandler);

		/// <summary>Decode a video frame that may contain more than one image.</summary>
		/// <param name="sampleBuffer">A sample buffer with one or more video frames.</param>
		/// <param name="decodeFlags">Any decoding flags to be passed to the decoder.</param>
		/// <param name="infoFlags">Upon return, any informational flags about the decode operation.</param>
		/// <param name="multiImageCapableOutputHandler">A callback that will be called when the decoding operation is complete.</param>
		/// <returns><see cref="VTStatus.Ok" /> if successful, or an error code otherwise.</returns>
		[BindingImpl (BindingImplOptions.Optimizable)]
		[SupportedOSPlatform ("macos14.0")]
		[SupportedOSPlatform ("ios17.0")]
		[SupportedOSPlatform ("maccatalyst17.0")]
		[UnsupportedOSPlatform ("tvos")]
		public unsafe VTStatus DecodeFrame (CMSampleBuffer sampleBuffer, VTDecodeFrameFlags decodeFlags, out VTDecodeInfoFlags infoFlags, VTDecompressionMultiImageCapableOutputHandler multiImageCapableOutputHandler)
		{
			delegate* unmanaged<BlockLiteral*, VTStatus, VTDecodeInfoFlags, IntPtr, IntPtr, CMTime, CMTime, void> trampoline = &VTDecompressionMultiImageCapableOutputBlockCallback;
			using var multiImageCapableOutputHandlerBlock = new BlockLiteral (trampoline, multiImageCapableOutputHandler, typeof (VTDecompressionSession), nameof (VTDecompressionMultiImageCapableOutputBlockCallback));

			infoFlags = default;

			var rv = VTDecompressionSessionDecodeFrameWithMultiImageCapableOutputHandler (
				GetCheckedHandle (),
				sampleBuffer.GetNonNullHandle (nameof (sampleBuffer)),
				decodeFlags,
				(VTDecodeInfoFlags*) Unsafe.AsPointer<VTDecodeInfoFlags> (ref infoFlags),
				&multiImageCapableOutputHandlerBlock);
			GC.KeepAlive (sampleBuffer);
			return rv;
		}

		[UnmanagedCallersOnly]
		unsafe static void VTDecompressionMultiImageCapableOutputBlockCallback (BlockLiteral* block, VTStatus status, VTDecodeInfoFlags infoFlags, IntPtr imageBuffer, IntPtr taggedBufferGroup, CMTime presentationTimeStamp, CMTime presentationDuration)
		{
			var del = BlockLiteral.GetTarget<VTDecompressionMultiImageCapableOutputHandler> ((IntPtr) block);
			if (del is not null) {
				var imageBufferObj = imageBuffer == IntPtr.Zero ? null : new CVImageBuffer (imageBuffer, owns: false);
				var taggedBufferGroupObj = taggedBufferGroup == IntPtr.Zero ? null : new CMTaggedBufferGroup (taggedBufferGroup, owns: false);
				del (status, infoFlags, imageBufferObj, taggedBufferGroupObj, presentationTimeStamp, presentationDuration);
			}
		}

		/// <summary>A callback for <see cref="DecodeFrame(CMSampleBuffer,VTDecodeFrameFlags,out VTDecodeInfoFlags,VTDecompressionMultiImageCapableOutputHandler)" />.</summary>
		/// <param name="status"><see cref="VTStatus.Ok" /> if the decode operation was successful, an error code otherwise.</param>
		/// <param name="infoFlags">Any information flags about the decode operation.</param>
		/// <param name="imageBuffer">If the decoding operation was successful, and a single image was decoded, the decoded image.</param>
		/// <param name="taggedBufferGroup">If the decoding operation was successful, and multiple images were decoded, the decoded images.</param>
		/// <param name="presentationTimeStamp">The frame's presentation timestamp.</param>
		/// <param name="presentationDuration">The frame's presentation duration.</param>
		public delegate void VTDecompressionMultiImageCapableOutputHandler (
			VTStatus status,
			VTDecodeInfoFlags infoFlags,
			CVImageBuffer? /* CM_NULLABLE CVImageBufferRef */ imageBuffer,
			CMTaggedBufferGroup? /* CM_NULLABLE CMTaggedBufferGroupRef */ taggedBufferGroup,
			CMTime presentationTimeStamp,
			CMTime presentationDuration);
#endif // !__TVOS__

#if !__TVOS__
		[SupportedOSPlatform ("macos14.0")]
		[SupportedOSPlatform ("ios17.0")]
		[SupportedOSPlatform ("maccatalyst17.0")]
		[UnsupportedOSPlatform ("tvos")]
		[DllImport (Constants.VideoToolboxLibrary)]
		unsafe static extern VTStatus VTDecompressionSessionSetMultiImageCallback (
			IntPtr /* CM_NONNULL VTDecompressionSessionRef */ decompressionSession,
			BlockLiteral* /* CM_NONNULL VTDecompressionOutputMultiImageCallback */ outputMultiImageCallback,
			IntPtr /* void * CM_NULLABLE */ outputMultiImageRefcon);

		/// <summary>Set a callback that will be called when a single call to <see cref="DecodeFrame(CMSampleBuffer,VTDecodeFrameFlags,IntPtr,out VTDecodeInfoFlags)" /> produces multiple images.</summary>
		/// <param name="outputMultiImageCallback">The callback that will be called when a single call to <see cref="DecodeFrame(CMSampleBuffer,VTDecodeFrameFlags,IntPtr,out VTDecodeInfoFlags)" /> produces multiple images.</param>
		/// <param name="outputMultiImageReference">A user-provided value that is passed to the callback.</param>
		/// <returns><see cref="VTStatus.Ok" /> if successful, or an error code otherwise.</returns>
		[SupportedOSPlatform ("macos14.0")]
		[SupportedOSPlatform ("ios17.0")]
		[SupportedOSPlatform ("maccatalyst17.0")]
		[UnsupportedOSPlatform ("tvos")]
		[BindingImpl (BindingImplOptions.Optimizable)]
		public unsafe VTStatus SetMultiImageCallback (VTDecompressionOutputMultiImageCallback outputMultiImageCallback, IntPtr outputMultiImageReference)
		{
			delegate* unmanaged<BlockLiteral*, IntPtr, IntPtr, VTStatus, VTDecodeInfoFlags, IntPtr, CMTime, CMTime, void> trampoline = &VTDecompressionOutputMultiImageCallbackBlock;
			using var multiImageCapableOutputHandlerBlock = new BlockLiteral (trampoline, outputMultiImageCallback, typeof (VTDecompressionSession), nameof (VTDecompressionOutputMultiImageCallbackBlock));

			return VTDecompressionSessionSetMultiImageCallback (GetCheckedHandle (), &multiImageCapableOutputHandlerBlock, outputMultiImageReference);
		}

		[UnmanagedCallersOnly]
		unsafe static void VTDecompressionOutputMultiImageCallbackBlock (BlockLiteral* block, IntPtr decompressionOutputMultiImageRefCon, IntPtr sourceFrameRefCon, VTStatus status, VTDecodeInfoFlags infoFlags, IntPtr taggedBufferGroup, CMTime presentationTimeStamp, CMTime presentationDuration)
		{
			var del = BlockLiteral.GetTarget<VTDecompressionOutputMultiImageCallback> ((IntPtr) block);
			if (del is not null) {
				var taggedBufferGroupObj = taggedBufferGroup == IntPtr.Zero ? null : new CMTaggedBufferGroup (taggedBufferGroup, owns: false);
				del (decompressionOutputMultiImageRefCon, sourceFrameRefCon, status, infoFlags, taggedBufferGroupObj, presentationTimeStamp, presentationDuration);
			}
		}

		/// <summary>A callback for <see cref="SetMultiImageCallback" />.</summary>
		/// <param name="outputMultiImageReference">This is the <c>outputMultiImageReference</c> parameter passed to <see cref="SetMultiImageCallback" />.</param>
		/// <param name="sourceFrameReference">This is the <c>sourceFrame</c> parameter passed to <see cref="DecodeFrame(CMSampleBuffer,VTDecodeFrameFlags,IntPtr,out VTDecodeInfoFlags)" />.</param>
		/// <param name="status"><see cref="VTStatus.Ok" /> if the decode operation was successful, an error code otherwise.</param>
		/// <param name="infoFlags">Any information flags about the decode operation.</param>
		/// <param name="taggedBufferGroup">If the decoding operation was successful, the decoded images.</param>
		/// <param name="presentationTimeStamp">The frame's presentation timestamp.</param>
		/// <param name="presentationDuration">The frame's presentation duration.</param>
		public delegate void VTDecompressionOutputMultiImageCallback (IntPtr outputMultiImageReference, IntPtr sourceFrameReference, VTStatus status, VTDecodeInfoFlags infoFlags, CMTaggedBufferGroup? taggedBufferGroup, CMTime presentationTimeStamp, CMTime presentationDuration);
#endif // !__TVOS__


		[SupportedOSPlatform ("macos15.0")]
		[SupportedOSPlatform ("ios18.0")]
		[SupportedOSPlatform ("maccatalyst18.0")]
		[SupportedOSPlatform ("tvos18.0")]
		[DllImport (Constants.VideoToolboxLibrary)]
		unsafe static extern VTStatus VTDecompressionSessionDecodeFrameWithOptions (
			IntPtr /* CM_NONNULL VTDecompressionSessionRef */ session,
			IntPtr /* CM_NONNULL CMSampleBufferRef */ sampleBuffer,
			VTDecodeFrameFlags decodeFlags,
			IntPtr /* CM_NULLABLE CFDictionaryRef */ frameOptions,
			IntPtr /* void * CM_NULLABLE */ sourceFrameRefCon,
			VTDecodeInfoFlags* /* VTDecodeInfoFlags * CM_NULLABLE */ infoFlagsOut);

		/// <summary>Decode a video frame that may contain more than one image.</summary>
		/// <param name="sampleBuffer">A sample buffer with one or more video frames.</param>
		/// <param name="decodeFlags">Any decoding flags to be passed to the decoder.</param>
		/// <param name="frameOptions">Any additional decoding options to be passed to the decoder.</param>
		/// <param name="sourceFrameReference">A user-provided value that is passed to any decoding callbacks.</param>
		/// <param name="infoFlags">Upon return, any informational flags about the decode operation.</param>
		/// <returns><see cref="VTStatus.Ok" /> if successful, or an error code otherwise.</returns>
		[SupportedOSPlatform ("macos15.0")]
		[SupportedOSPlatform ("ios18.0")]
		[SupportedOSPlatform ("maccatalyst18.0")]
		[SupportedOSPlatform ("tvos18.0")]
		public unsafe VTStatus DecodeFrame (CMSampleBuffer sampleBuffer, VTDecodeFrameFlags decodeFlags, NSDictionary? frameOptions, IntPtr sourceFrameReference, out VTDecodeInfoFlags infoFlags)
		{
			infoFlags = default;

			var rv = VTDecompressionSessionDecodeFrameWithOptions (
				GetCheckedHandle (),
				sampleBuffer.GetNonNullHandle (nameof (sampleBuffer)),
				decodeFlags,
				frameOptions.GetHandle (),
				sourceFrameReference,
				(VTDecodeInfoFlags*) Unsafe.AsPointer<VTDecodeInfoFlags> (ref infoFlags));

			GC.KeepAlive (sampleBuffer);
			GC.KeepAlive (frameOptions);

			return rv;
		}

		/// <summary>Decode a video frame that may contain more than one image.</summary>
		/// <param name="sampleBuffer">A sample buffer with one or more video frames.</param>
		/// <param name="decodeFlags">Any decoding flags to be passed to the decoder.</param>
		/// <param name="frameOptions">Any additional decoding options to be passed to the decoder.</param>
		/// <param name="sourceFrameReference">A user-provided value that is passed to any decoding callbacks.</param>
		/// <param name="infoFlags">Upon return, any informational flags about the decode operation.</param>
		/// <returns><see cref="VTStatus.Ok" /> if successful, or an error code otherwise.</returns>
		[SupportedOSPlatform ("macos15.0")]
		[SupportedOSPlatform ("ios18.0")]
		[SupportedOSPlatform ("maccatalyst18.0")]
		[SupportedOSPlatform ("tvos18.0")]
		public VTStatus DecodeFrame (CMSampleBuffer sampleBuffer, VTDecodeFrameFlags decodeFlags, VTDecodeFrameOptions? frameOptions, IntPtr sourceFrameReference, out VTDecodeInfoFlags infoFlags)
		{
			return DecodeFrame (sampleBuffer, decodeFlags, frameOptions.GetDictionary (), sourceFrameReference, out infoFlags);
		}

		[SupportedOSPlatform ("macos15.0")]
		[SupportedOSPlatform ("ios18.0")]
		[SupportedOSPlatform ("maccatalyst18.0")]
		[SupportedOSPlatform ("tvos18.0")]
		[DllImport (Constants.VideoToolboxLibrary)]
		unsafe static extern VTStatus VTDecompressionSessionDecodeFrameWithOptionsAndOutputHandler (
			IntPtr /* CM_NONNULL VTDecompressionSessionRef */ session,
			IntPtr /* CM_NONNULL CMSampleBufferRef */ sampleBuffer,
			VTDecodeFrameFlags decodeFlags,
			IntPtr /* CM_NULLABLE CFDictionaryRef */ frameOptions,
			VTDecodeInfoFlags* /* VTDecodeInfoFlags * CM_NULLABLE */ infoFlagsOut,
			BlockLiteral* /* CM_NONNULL VTDecompressionOutputHandler */ outputHandler);

		/// <summary>Decode a video frame that may contain more than one image.</summary>
		/// <param name="sampleBuffer">A sample buffer with one or more video frames.</param>
		/// <param name="decodeFlags">Any decoding flags to be passed to the decoder.</param>
		/// <param name="frameOptions">Any additional decoding options to be passed to the decoder.</param>
		/// <param name="infoFlags">Upon return, any informational flags about the decode operation.</param>
		/// <param name="outputHandler">A callback that will be called when the decoding operation is complete.</param>
		/// <returns><see cref="VTStatus.Ok" /> if successful, or an error code otherwise.</returns>
		[SupportedOSPlatform ("macos15.0")]
		[SupportedOSPlatform ("ios18.0")]
		[SupportedOSPlatform ("maccatalyst18.0")]
		[SupportedOSPlatform ("tvos18.0")]
		[BindingImpl (BindingImplOptions.Optimizable)]
		public unsafe VTStatus DecodeFrame (CMSampleBuffer sampleBuffer, VTDecodeFrameFlags decodeFlags, NSDictionary? frameOptions, out VTDecodeInfoFlags infoFlags, VTDecompressionOutputHandler outputHandler)
		{
			delegate* unmanaged<BlockLiteral*, VTStatus, VTDecodeInfoFlags, IntPtr, CMTime, CMTime, void> trampoline = &VTDecompressionOutputHandlerTrampoline;
			using var outputHandlerBlock = new BlockLiteral (trampoline, outputHandler, typeof (VTDecompressionSession), nameof (VTDecompressionOutputHandlerTrampoline));

			infoFlags = default;

			var rv = VTDecompressionSessionDecodeFrameWithOptionsAndOutputHandler (
						GetCheckedHandle (),
						sampleBuffer.GetNonNullHandle (nameof (sampleBuffer)),
						decodeFlags,
						frameOptions.GetHandle (),
						(VTDecodeInfoFlags*) Unsafe.AsPointer<VTDecodeInfoFlags> (ref infoFlags),
						&outputHandlerBlock);
			GC.KeepAlive (sampleBuffer);
			GC.KeepAlive (frameOptions);
			return rv;
		}

		/// <summary>Decode a video frame that may contain more than one image.</summary>
		/// <param name="sampleBuffer">A sample buffer with one or more video frames.</param>
		/// <param name="decodeFlags">Any decoding flags to be passed to the decoder.</param>
		/// <param name="frameOptions">Any additional decoding options to be passed to the decoder.</param>
		/// <param name="infoFlags">Upon return, any informational flags about the decode operation.</param>
		/// <param name="outputHandler">A callback that will be called when the decoding operation is complete.</param>
		/// <returns><see cref="VTStatus.Ok" /> if successful, or an error code otherwise.</returns>
		[SupportedOSPlatform ("macos15.0")]
		[SupportedOSPlatform ("ios18.0")]
		[SupportedOSPlatform ("maccatalyst18.0")]
		[SupportedOSPlatform ("tvos18.0")]
		public unsafe VTStatus DecodeFrame (CMSampleBuffer sampleBuffer, VTDecodeFrameFlags decodeFlags, VTDecodeFrameOptions? frameOptions, out VTDecodeInfoFlags infoFlags, VTDecompressionOutputHandler outputHandler)
		{
			return DecodeFrame (sampleBuffer, decodeFlags, frameOptions.GetDictionary (), out infoFlags, outputHandler);
		}

		/// <summary>A callback for <see cref="DecodeFrame(CMSampleBuffer,VTDecodeFrameFlags,NSDictionary,out VTDecodeInfoFlags,VTDecompressionOutputHandler)" />.</summary>
		/// <param name="status"><see cref="VTStatus.Ok" /> if the decode operation was successful, an error code otherwise.</param>
		/// <param name="infoFlags">Any information flags about the decode operation.</param>
		/// <param name="imageBuffer">If the decoding operation was successful, the decoded image.</param>
		/// <param name="presentationTimeStamp">The frame's presentation timestamp.</param>
		/// <param name="presentationDuration">The frame's presentation duration.</param>
		public delegate void VTDecompressionOutputHandler (
			VTStatus status,
			VTDecodeInfoFlags infoFlags,
			CVImageBuffer? /* CM_NULLABLE CVImageBufferRef */ imageBuffer,
			CMTime presentationTimeStamp,
			CMTime presentationDuration);

		[UnmanagedCallersOnly]
		static unsafe void VTDecompressionOutputHandlerTrampoline (BlockLiteral* block, VTStatus status, VTDecodeInfoFlags infoFlags, IntPtr imageBuffer, CMTime presentationTimeStamp, CMTime presentationDuration)
		{
			var del = BlockLiteral.GetTarget<VTDecompressionOutputHandler> ((IntPtr) block);
			if (del is not null) {
				var imageBufferObj = imageBuffer == IntPtr.Zero ? null : new CVImageBuffer (imageBuffer, owns: false);
				del (status, infoFlags, imageBufferObj, presentationTimeStamp, presentationDuration);
			}
		}
	}
}
