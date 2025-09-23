// Copyright (c) Microsoft Corporation.
// Licensed under the MIT License.

#nullable enable

using System.Diagnostics.CodeAnalysis;
using System.Runtime.InteropServices;

using CoreVideo;
using Foundation;
using ObjCRuntime;

namespace VideoToolbox;

/// <summary>A class that is used to create estimated motion vectors between two pixel buffers.</summary>
[SupportedOSPlatform ("ios26.0")]
[SupportedOSPlatform ("tvos26.0")]
[SupportedOSPlatform ("macos26.0")]
[SupportedOSPlatform ("maccatalyst26.0")]
public class VTMotionEstimationSession : VTSession {

	[Preserve (Conditional = true)]
	internal VTMotionEstimationSession (NativeHandle handle, bool owns)
		: base (handle, owns)
	{
	}

	protected override void Dispose (bool disposing)
	{
		if (Handle != IntPtr.Zero)
			VTMotionEstimationSessionInvalidate (Handle);

		base.Dispose (disposing);
	}

	[DllImport (Constants.VideoToolboxLibrary)]
	static extern unsafe VTStatus VTMotionEstimationSessionCreate (
		IntPtr /* CM_NULLABLE CFAllocatorRef */  allocator,
		IntPtr /* CM_NULLABLE CFDictionaryRef */  motionVectorProcessorSelectionOptions,
		uint width,
		uint height,
		IntPtr* /* CM_RETURNS_RETAINED_PARAMETER CM_NULLABLE VTMotionEstimationSessionRef * CM_NONNULL */ motionEstimationSessionOut);

	/// <summary>Create a new <see cref="VTMotionEstimationSession" /> instance.</summary>
	/// <param name="options">Any options for the new <see cref="VTMotionEstimationSession" /> instance.</param>
	/// <param name="width">The frame width of the source and destination frames.</param>
	/// <param name="height">The frame height of the source and destination frames.</param>
	/// <param name="status">Upon return, <see cref="VTStatus.Ok" /> if successful, or an error code otherwise.</param>
	/// <returns>A new <see cref="VTMotionEstimationSession" /> instance if successful, <see langword="null" /> otherwise.</returns>
	public static VTMotionEstimationSession? Create (NSDictionary? options, uint width, uint height, out VTStatus status)
	{
		IntPtr handle;
		unsafe {
			status = VTMotionEstimationSessionCreate (IntPtr.Zero, options.GetHandle (), width, height, &handle);
			GC.KeepAlive (options);
		}
		if (handle == IntPtr.Zero)
			return null;

		return new VTMotionEstimationSession (handle, true);
	}

	/// <summary>Create a new <see cref="VTMotionEstimationSession" /> instance.</summary>
	/// <param name="options">Any options for the new <see cref="VTMotionEstimationSession" /> instance.</param>
	/// <param name="width">The frame width of the source and destination frames.</param>
	/// <param name="height">The frame height of the source and destination frames.</param>
	/// <param name="status">Upon return, <see cref="VTStatus.Ok" /> if successful, or an error code otherwise.</param>
	/// <returns>A new <see cref="VTMotionEstimationSession" /> instance if successful, <see langword="null" /> otherwise.</returns>
	public static VTMotionEstimationSession? Create (VTMotionEstimationSessionCreationOption? options, uint width, uint height, out VTStatus status)
	{
		return Create (options?.Dictionary, width, height, out status);
	}

	[DllImport (Constants.VideoToolboxLibrary)]
	static extern unsafe VTStatus /* OSStatus */ VTMotionEstimationSessionCopySourcePixelBufferAttributes (
		IntPtr /* CM_NONNULL VTMotionEstimationSessionRef */  motionEstimationSession,
		IntPtr* /* CM_RETURNS_RETAINED_PARAMETER CM_NULLABLE CFDictionaryRef * CM_NONNULL */ attributesOut);

	/// <summary>Get the pixel buffer attributes this session expects for any source pixel buffers.</summary>
	/// <param name="pixelBufferAttributes">A <see cref="CVPixelBufferAttributes" /> instance if successful, <see langword="null" /> otherwise.</param>
	/// <param name="status">Upon return, <see cref="VTStatus.Ok" /> if successful, or an error code otherwise.</param>
	/// <returns><see langword="true" /> if successful, <see langword="false" /> otherwise.</returns>
	public unsafe bool TryGetSourcePixelBufferAttributes ([NotNullWhen (true)] out NSDictionary? pixelBufferAttributes, out VTStatus status)
	{
		IntPtr handle;
		status = VTMotionEstimationSessionCopySourcePixelBufferAttributes (GetCheckedHandle (), &handle);
		pixelBufferAttributes = Runtime.GetNSObject<NSDictionary> (handle, owns: true);
		return status == 0 && pixelBufferAttributes is not null;
	}

	/// <summary>Get the pixel buffer attributes this session expects for any source pixel buffers.</summary>
	/// <returns>A <see cref="CVPixelBufferAttributes" /> instance if successful, <see langword="null" /> otherwise.</returns>
	public CVPixelBufferAttributes? SourcePixelBufferAttributes {
		get {
			if (TryGetSourcePixelBufferAttributes (out var dict, out var _))
				return new CVPixelBufferAttributes (dict);
			return null;
		}
	}

	[DllImport (Constants.VideoToolboxLibrary)]
	static extern void VTMotionEstimationSessionInvalidate (
		IntPtr /* CM_NONNULL VTMotionEstimationSessionRef */  session);

	[DllImport (Constants.VideoToolboxLibrary)]
	static extern unsafe VTStatus /* OSStatus */ VTMotionEstimationSessionEstimateMotionVectors (
		IntPtr /* CM_NONNULL VTMotionEstimationSessionRef */  session,
		IntPtr /* CM_NONNULL CVPixelBufferRef */ referenceImage,
		IntPtr /* CM_NONNULL CVPixelBufferRef */ currentImage,
		VTMotionEstimationFrameFlags motionEstimationFrameFlags,
		IntPtr /* CM_NULLABLE CFDictionaryRef */ additionalFrameOptions,
		BlockLiteral* /* CM_NONNULL VTMotionEstimationOutputHandler */ outputHandler);

	/// <summary>Compute estimated motion vectors between to pixel buffers.</summary>
	/// <param name="referenceImage">The reference image to use for the computation.</param>
	/// <param name="currentImage">The current image to use for the computation.</param>
	/// <param name="motionEstimationFrameFlags">Any flags for the operation.</param>
	/// <param name="additionalFrameOptions">Any additional frame options for the operation.</param>
	/// <param name="outputHandler">The callback that will be called with the result of the operation.</param>
	/// <returns><see cref="VTStatus.Ok" /> if successful, or an error code otherwise.</returns>
	[BindingImpl (BindingImplOptions.Optimizable)]
	public unsafe VTStatus EstimateMotionVectors (CVPixelBuffer referenceImage, CVPixelBuffer currentImage, VTMotionEstimationFrameFlags motionEstimationFrameFlags, NSDictionary? additionalFrameOptions, VTMotionEstimationOutputHandler outputHandler)
	{
		delegate* unmanaged<BlockLiteral*, VTStatus, VTMotionEstimationInfoFlags, IntPtr, IntPtr, void> trampoline = &OutputHandlerTrampoline;
		using var outputBlock = new BlockLiteral (trampoline, outputHandler, typeof (VTMotionEstimationSession), nameof (OutputHandlerTrampoline));

		var rv = VTMotionEstimationSessionEstimateMotionVectors (
					GetCheckedHandle (),
					referenceImage.GetNonNullHandle (nameof (referenceImage)),
					currentImage.GetNonNullHandle (nameof (currentImage)),
					motionEstimationFrameFlags,
					additionalFrameOptions.GetHandle (),
					&outputBlock);

		GC.KeepAlive (referenceImage);
		GC.KeepAlive (currentImage);
		GC.KeepAlive (additionalFrameOptions);

		return rv;
	}

	[UnmanagedCallersOnly]
	unsafe static void OutputHandlerTrampoline (BlockLiteral* block, VTStatus status, VTMotionEstimationInfoFlags infoFlags, IntPtr additionalInfo, IntPtr motionVectors)
	{
		var del = BlockLiteral.GetTarget<VTMotionEstimationOutputHandler> ((IntPtr) block);
		if (del is not null) {
			var motionVectorObj = motionVectors == IntPtr.Zero ? null : new CVPixelBuffer (motionVectors, false);
			del (status, infoFlags, Runtime.GetNSObject<NSDictionary> (additionalInfo, false), motionVectorObj);
		}
	}

	[DllImport (Constants.VideoToolboxLibrary)]
	static extern unsafe VTStatus /* OSStatus */ VTMotionEstimationSessionCompleteFrames (
		IntPtr /* CM_NONNULL VTMotionEstimationSessionRef */ session);

	/// <summary>Tells the motion estimation session to finish processing all frames.</summary>
	/// <returns><see cref="VTStatus.Ok" /> if successful, or an error code otherwise.</returns>
	public VTStatus CompleteFrames ()
	{
		return VTMotionEstimationSessionCompleteFrames (GetCheckedHandle ());
	}

	[DllImport (Constants.VideoToolboxLibrary)]
	static extern nuint VTMotionEstimationSessionGetTypeID ();

	/// <summary>Get this type's CFTypeID.</summary>
	/// <returns>This type's CFTypeID.</returns>
	public static nuint GetTypeId ()
	{
		return VTMotionEstimationSessionGetTypeID ();
	}
}

/// <summary>A callback for <see cref="VTMotionEstimationSession.EstimateMotionVectors" />.</summary>
/// <param name="status"><see cref="VTStatus.Ok" /> if the operation was successful, an error code otherwise.</param>
/// <param name="infoFlags">Any information flags about the operation.</param>
/// <param name="additionalInfo">A dictionary with any additional info for the operation.</param>
/// <param name="motionVectors">If successful, a <see cref="CVPixelBuffer" /> with the result of the operation, or <see langword="null" /> otherwise.</param>
public delegate void VTMotionEstimationOutputHandler (
	VTStatus /* OSStatus */ status,
	VTMotionEstimationInfoFlags infoFlags,
	NSDictionary? additionalInfo,
	CVPixelBuffer? motionVectors);
