#nullable enable

using System;
using System.Diagnostics.CodeAnalysis;
using System.Runtime.InteropServices;
using System.Runtime.Versioning;
using CoreFoundation;
using CoreMedia;
using CoreVideo;
using ObjCRuntime;

namespace CoreMediaIO {

#if !COREBUILD
	/// <summary>Provides managed wrappers for CoreMediaIO sample buffer C functions.</summary>
	[SupportedOSPlatform ("macos")]
	[SupportedOSPlatform ("maccatalyst15.4")]
	[UnsupportedOSPlatform ("ios")]
	[UnsupportedOSPlatform ("tvos")]
	public static class CMIOSampleBufferExtensions {

		/// <summary>Attempts to create a <see cref="CMSampleBuffer" /> that can be used in a CoreMediaIO graph.</summary>
		/// <param name="dataBuffer">The block buffer containing the media data, or <see langword="null" /> for special-use cases.</param>
		/// <param name="formatDescription">A description of the media data's format.</param>
		/// <param name="numSamples">The number of samples represented by the buffer.</param>
		/// <param name="sampleTimingArray">An array of timing information for the samples, or <see langword="null" />.</param>
		/// <param name="sampleSizeArray">An array of sample sizes, or <see langword="null" />.</param>
		/// <param name="sequenceNumber">The position of this buffer in the stream.</param>
		/// <param name="discontinuityFlags">Flags indicating any discontinuity in the stream.</param>
		/// <param name="status">On return, the status code from the native call; 0 indicates success.</param>
		/// <param name="sampleBuffer">On success, receives the created sample buffer.</param>
		/// <returns><see langword="true" /> if the sample buffer was created successfully; otherwise, <see langword="false" />.</returns>
		public static unsafe bool TryCreateSampleBuffer (
			CMBlockBuffer? dataBuffer,
			CMFormatDescription formatDescription,
			uint numSamples,
			CMSampleTimingInfo []? sampleTimingArray,
			nuint []? sampleSizeArray,
			ulong sequenceNumber,
			uint discontinuityFlags,
			out int status,
			[NotNullWhen (true)] out CMSampleBuffer? sampleBuffer)
		{
			sampleBuffer = null;
			IntPtr bufferOut;

			uint timingCount = sampleTimingArray is null ? 0 : (uint) sampleTimingArray.Length;
			uint sizeCount = sampleSizeArray is null ? 0 : (uint) sampleSizeArray.Length;
			var dataBufferHandle = dataBuffer.GetHandle ();
			var formatDescriptionHandle = formatDescription.GetHandle ();

			fixed (CMSampleTimingInfo* timingPtr = sampleTimingArray)
			fixed (nuint* sizePtr = sampleSizeArray) {
				status = CMIOInterop.CMIOSampleBufferCreate (
					IntPtr.Zero,
					dataBufferHandle,
					formatDescriptionHandle,
					numSamples,
					timingCount,
					timingPtr,
					sizeCount,
					sizePtr,
					sequenceNumber,
					discontinuityFlags,
					&bufferOut);
			}

			GC.KeepAlive (dataBuffer);
			GC.KeepAlive (formatDescription);

			if (status != 0 || bufferOut == IntPtr.Zero)
				return false;

			sampleBuffer = Runtime.GetINativeObject<CMSampleBuffer> (bufferOut, owns: true)!;
			return true;
		}

		/// <summary>Attempts to create a <see cref="CMSampleBuffer" /> containing a <see cref="CVImageBuffer" /> for use in a CoreMediaIO graph.</summary>
		/// <param name="imageBuffer">The image buffer containing the media data.</param>
		/// <param name="formatDescription">A description of the media data's format.</param>
		/// <param name="sampleTiming">Timing information for the media sample.</param>
		/// <param name="sequenceNumber">The position of this buffer in the stream.</param>
		/// <param name="discontinuityFlags">Flags indicating any discontinuity in the stream.</param>
		/// <param name="status">On return, the status code from the native call; 0 indicates success.</param>
		/// <param name="sampleBuffer">On success, receives the created sample buffer.</param>
		/// <returns><see langword="true" /> if the sample buffer was created successfully; otherwise, <see langword="false" />.</returns>
		public static unsafe bool TryCreateSampleBufferForImageBuffer (
			CVImageBuffer imageBuffer,
			CMFormatDescription formatDescription,
			CMSampleTimingInfo sampleTiming,
			ulong sequenceNumber,
			uint discontinuityFlags,
			out int status,
			[NotNullWhen (true)] out CMSampleBuffer? sampleBuffer)
		{
			if (imageBuffer is null)
				ObjCRuntime.ThrowHelper.ThrowArgumentNullException (nameof (imageBuffer));
			if (formatDescription is null)
				ObjCRuntime.ThrowHelper.ThrowArgumentNullException (nameof (formatDescription));

			sampleBuffer = null;
			IntPtr bufferOut;

			status = CMIOInterop.CMIOSampleBufferCreateForImageBuffer (
				IntPtr.Zero,
				imageBuffer.Handle,
				formatDescription.Handle,
				&sampleTiming,
				sequenceNumber,
				discontinuityFlags,
				&bufferOut);

			GC.KeepAlive (imageBuffer);
			GC.KeepAlive (formatDescription);

			if (status != 0 || bufferOut == IntPtr.Zero)
				return false;

			sampleBuffer = Runtime.GetINativeObject<CMSampleBuffer> (bufferOut, owns: true)!;
			return true;
		}

		/// <summary>Attempts to create a <see cref="CMSampleBuffer" /> with no data that serves as a marker indicating the device has stopped sending data.</summary>
		/// <param name="noDataEvent">The type of no-data event.</param>
		/// <param name="formatDescription">A description of the media data's format, or <see langword="null" />.</param>
		/// <param name="sequenceNumber">The position of this buffer in the stream.</param>
		/// <param name="discontinuityFlags">Flags indicating any discontinuity in the stream.</param>
		/// <param name="status">On return, the status code from the native call; 0 indicates success.</param>
		/// <param name="sampleBuffer">On success, receives the created sample buffer.</param>
		/// <returns><see langword="true" /> if the marker was created successfully; otherwise, <see langword="false" />.</returns>
		public static unsafe bool TryCreateNoDataMarker (
			uint noDataEvent,
			CMFormatDescription? formatDescription,
			ulong sequenceNumber,
			uint discontinuityFlags,
			out int status,
			[NotNullWhen (true)] out CMSampleBuffer? sampleBuffer)
		{
			sampleBuffer = null;
			IntPtr bufferOut;

			status = CMIOInterop.CMIOSampleBufferCreateNoDataMarker (
				IntPtr.Zero,
				noDataEvent,
				formatDescription.GetHandle (),
				sequenceNumber,
				discontinuityFlags,
				&bufferOut);

			GC.KeepAlive (formatDescription);

			if (status != 0 || bufferOut == IntPtr.Zero)
				return false;

			sampleBuffer = Runtime.GetINativeObject<CMSampleBuffer> (bufferOut, owns: true)!;
			return true;
		}

		/// <summary>Sets the sequence number on a <see cref="CMSampleBuffer" />.</summary>
		/// <param name="sampleBuffer">The sample buffer to modify.</param>
		/// <param name="sequenceNumber">The new sequence number.</param>
		public static void SetSequenceNumber (CMSampleBuffer sampleBuffer, ulong sequenceNumber)
		{
			if (sampleBuffer is null)
				ObjCRuntime.ThrowHelper.ThrowArgumentNullException (nameof (sampleBuffer));
			CMIOInterop.CMIOSampleBufferSetSequenceNumber (IntPtr.Zero, sampleBuffer.Handle, sequenceNumber);
			GC.KeepAlive (sampleBuffer);
		}

		/// <summary>Gets the sequence number from a <see cref="CMSampleBuffer" />.</summary>
		/// <param name="sampleBuffer">The sample buffer to query.</param>
		/// <returns>The sequence number of the buffer.</returns>
		public static ulong GetSequenceNumber (CMSampleBuffer sampleBuffer)
		{
			if (sampleBuffer is null)
				ObjCRuntime.ThrowHelper.ThrowArgumentNullException (nameof (sampleBuffer));
			var result = CMIOInterop.CMIOSampleBufferGetSequenceNumber (sampleBuffer.Handle);
			GC.KeepAlive (sampleBuffer);
			return result;
		}

		/// <summary>Sets the discontinuity flags on a <see cref="CMSampleBuffer" />.</summary>
		/// <param name="sampleBuffer">The sample buffer to modify.</param>
		/// <param name="discontinuityFlags">The new discontinuity flags.</param>
		public static void SetDiscontinuityFlags (CMSampleBuffer sampleBuffer, uint discontinuityFlags)
		{
			if (sampleBuffer is null)
				ObjCRuntime.ThrowHelper.ThrowArgumentNullException (nameof (sampleBuffer));
			CMIOInterop.CMIOSampleBufferSetDiscontinuityFlags (IntPtr.Zero, sampleBuffer.Handle, discontinuityFlags);
			GC.KeepAlive (sampleBuffer);
		}

		/// <summary>Gets the discontinuity flags from a <see cref="CMSampleBuffer" />.</summary>
		/// <param name="sampleBuffer">The sample buffer to query.</param>
		/// <returns>The discontinuity flags of the buffer.</returns>
		public static uint GetDiscontinuityFlags (CMSampleBuffer sampleBuffer)
		{
			if (sampleBuffer is null)
				ObjCRuntime.ThrowHelper.ThrowArgumentNullException (nameof (sampleBuffer));
			var result = CMIOInterop.CMIOSampleBufferGetDiscontinuityFlags (sampleBuffer.Handle);
			GC.KeepAlive (sampleBuffer);
			return result;
		}

		/// <summary>Copies the non-required attachments from one <see cref="CMSampleBuffer" /> to another.</summary>
		/// <param name="source">The source sample buffer.</param>
		/// <param name="destination">The destination sample buffer.</param>
		/// <param name="attachmentMode">The attachment mode.</param>
		/// <returns>An <see cref="int" /> status code; 0 indicates success.</returns>
		public static int CopyNonRequiredAttachments (CMSampleBuffer source, CMSampleBuffer destination, CMAttachmentMode attachmentMode)
		{
			if (source is null)
				ObjCRuntime.ThrowHelper.ThrowArgumentNullException (nameof (source));
			if (destination is null)
				ObjCRuntime.ThrowHelper.ThrowArgumentNullException (nameof (destination));
			var result = CMIOInterop.CMIOSampleBufferCopyNonRequiredAttachments (source.Handle, destination.Handle, (uint) attachmentMode);
			GC.KeepAlive (source);
			GC.KeepAlive (destination);
			return result;
		}

		/// <summary>Copies the per-sample attachments from one <see cref="CMSampleBuffer" /> to another.</summary>
		/// <param name="source">The source sample buffer.</param>
		/// <param name="destination">The destination sample buffer.</param>
		/// <returns>An <see cref="int" /> status code; 0 indicates success.</returns>
		public static int CopySampleAttachments (CMSampleBuffer source, CMSampleBuffer destination)
		{
			if (source is null)
				ObjCRuntime.ThrowHelper.ThrowArgumentNullException (nameof (source));
			if (destination is null)
				ObjCRuntime.ThrowHelper.ThrowArgumentNullException (nameof (destination));
			var result = CMIOInterop.CMIOSampleBufferCopySampleAttachments (source.Handle, destination.Handle);
			GC.KeepAlive (source);
			GC.KeepAlive (destination);
			return result;
		}
	}
#endif // !COREBUILD
}
