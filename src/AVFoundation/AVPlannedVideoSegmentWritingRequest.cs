//
// AVPlannedVideoSegmentWritingRequest.cs: Manual bindings for AVPlannedVideoSegmentWritingRequest
//
// Copyright 2025 Microsoft Corp. All rights reserved.
//

using System;
using System.Runtime.InteropServices;

using CoreMedia;
using CoreVideo;
using Foundation;
using ObjCRuntime;
using VideoToolbox;

#nullable enable

namespace AVFoundation {
	public partial class AVPlannedVideoSegmentWritingRequest {
		/// <summary>Creates a <see cref="VideoToolbox.VTCompressionSession" /> that restores the video encoder state persisted at the end of the previous segment.</summary>
		/// <param name="width">The pixel width of video frames.</param>
		/// <param name="height">The pixel height of video frames.</param>
		/// <param name="codecType">The codec type.</param>
		/// <param name="compressionOutputCallback">The callback to invoke with compressed frames, or <see langword="null" /> if you'll be encoding frames with an output handler.</param>
		/// <param name="encoderSpecification">Parameters describing the characteristics of a video encoder to use, or <see langword="null" /> to let the system choose an encoder.</param>
		/// <param name="sourceImageBufferAttributes">Required attributes for source pixel buffers, or <see langword="null" /> if you don't want the system to create a pixel buffer pool.</param>
		/// <returns>A new <see cref="VideoToolbox.VTCompressionSession" /> if successful, <see langword="null" /> otherwise.</returns>
		[SupportedOSPlatform ("ios27.0")]
		[SupportedOSPlatform ("tvos27.0")]
		[SupportedOSPlatform ("macos27.0")]
		[SupportedOSPlatform ("maccatalyst27.0")]
		public VTCompressionSession? CreateResumableCompressionSession (int width, int height, CMVideoCodecType codecType, VTCompressionSession.VTCompressionOutputCallback? compressionOutputCallback, VTVideoEncoderSpecification? encoderSpecification = null, CVPixelBufferAttributes? sourceImageBufferAttributes = null)
			=> CreateResumableCompressionSession (width, height, codecType, compressionOutputCallback, encoderSpecification, sourceImageBufferAttributes, out _);

		/// <summary>Creates a <see cref="VideoToolbox.VTCompressionSession" /> that restores the video encoder state persisted at the end of the previous segment.</summary>
		/// <param name="width">The pixel width of video frames.</param>
		/// <param name="height">The pixel height of video frames.</param>
		/// <param name="codecType">The codec type.</param>
		/// <param name="compressionOutputCallback">The callback to invoke with compressed frames, or <see langword="null" /> if you'll be encoding frames with an output handler.</param>
		/// <param name="encoderSpecification">Parameters describing the characteristics of a video encoder to use, or <see langword="null" /> to let the system choose an encoder.</param>
		/// <param name="sourceImageBufferAttributes">Required attributes for source pixel buffers, or <see langword="null" /> if you don't want the system to create a pixel buffer pool.</param>
		/// <param name="error">On failure, the error that occurred.</param>
		/// <returns>A new <see cref="VideoToolbox.VTCompressionSession" /> if successful, <see langword="null" /> otherwise.</returns>
		[SupportedOSPlatform ("ios27.0")]
		[SupportedOSPlatform ("tvos27.0")]
		[SupportedOSPlatform ("macos27.0")]
		[SupportedOSPlatform ("maccatalyst27.0")]
		public VTCompressionSession? CreateResumableCompressionSession (int width, int height, CMVideoCodecType codecType, VTCompressionSession.VTCompressionOutputCallback? compressionOutputCallback, VTVideoEncoderSpecification? encoderSpecification, CVPixelBufferAttributes? sourceImageBufferAttributes, out NSError? error)
		{
			var (outputCallback, outputCallbackRefCon) = VTCompressionSession.PrepareOutputCallback (compressionOutputCallback, out var callbackHandle);

			IntPtr handle;
			try {
				handle = _CreateResumableCompressionSession (IntPtr.Zero, width, height, codecType, encoderSpecification?.Dictionary, sourceImageBufferAttributes?.Dictionary, IntPtr.Zero, outputCallback, outputCallbackRefCon, out error);
			} catch {
				if (callbackHandle.IsAllocated)
					callbackHandle.Free ();
				throw;
			}

			if (handle != IntPtr.Zero)
				return VTCompressionSession.CreateFromOwnedHandle (handle, callbackHandle);

			if (callbackHandle.IsAllocated)
				callbackHandle.Free ();
			return null;
		}
	}
}
