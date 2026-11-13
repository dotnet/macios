// Copyright (c) Microsoft Corporation.
// Licensed under the MIT License.

#nullable enable

using CoreMedia;
using CoreVideo;

namespace CoreMediaIO {
	/// <summary>Creates sample buffers and manages their Core Media I/O attachments.</summary>
	[SupportedOSPlatform ("macos")]
	[SupportedOSPlatform ("maccatalyst")]
	[UnsupportedOSPlatform ("ios")]
	[UnsupportedOSPlatform ("tvos")]
	public static class CMIOSampleBuffer {
		/// <summary>The sequence number used when no valid sequence number is available.</summary>
		public const ulong InvalidSequenceNumber = ulong.MaxValue;

		/// <summary>Returns the next sequence number, wrapping the invalid sequence number to zero.</summary>
		public static ulong GetNextSequenceNumber (ulong sequenceNumber) => sequenceNumber == InvalidSequenceNumber ? 0 : sequenceNumber + 1;

		/// <summary>Gets whether flags contain any discontinuity other than an extended duration.</summary>
		public static bool HasHardDiscontinuities (CMIOSampleBufferDiscontinuityFlags flags) => (flags & ~CMIOSampleBufferDiscontinuityFlags.DurationWasExtended) != 0;

		[DllImport (Constants.CoreMediaIOLibrary)]
		static extern unsafe int CMIOSampleBufferCreate (IntPtr allocator, IntPtr dataBuffer, IntPtr formatDescription, uint numSamples, uint numSampleTimingEntries, CMSampleTimingInfo* sampleTimingArray, uint numSampleSizeEntries, nuint* sampleSizeArray, ulong sequenceNumber, CMIOSampleBufferDiscontinuityFlags discontinuityFlags, out IntPtr sampleBuffer);

		/// <summary>Creates a sample buffer with Core Media I/O attachments, returning an OSStatus error code or zero on success.</summary>
		/// <remarks>The timing and size spans must contain zero, one, or <paramref name="numSamples" /> entries, as required by the media format. Dispose the returned sample buffer when finished.</remarks>
		public static unsafe int Create (CMBlockBuffer? dataBuffer, CMFormatDescription formatDescription, uint numSamples, ReadOnlySpan<CMSampleTimingInfo> sampleTiming, ReadOnlySpan<nuint> sampleSizes, ulong sequenceNumber, CMIOSampleBufferDiscontinuityFlags discontinuityFlags, out CMSampleBuffer? sampleBuffer)
		{
			if (sampleTiming.Length > 1 && (uint) sampleTiming.Length != numSamples)
				throw new ArgumentException ("The timing span must contain zero, one, or one entry per sample.", nameof (sampleTiming));
			if (sampleSizes.Length > 1 && (uint) sampleSizes.Length != numSamples)
				throw new ArgumentException ("The size span must contain zero, one, or one entry per sample.", nameof (sampleSizes));
			fixed (CMSampleTimingInfo* timing = sampleTiming)
			fixed (nuint* sizes = sampleSizes) {
				var status = CMIOSampleBufferCreate (IntPtr.Zero, dataBuffer.GetHandle (), formatDescription.GetNonNullHandle (nameof (formatDescription)), numSamples, (uint) sampleTiming.Length, timing, (uint) sampleSizes.Length, sizes, sequenceNumber, discontinuityFlags, out var handle);
				GC.KeepAlive (dataBuffer);
				GC.KeepAlive (formatDescription);
				sampleBuffer = status == 0 ? new CMSampleBuffer (handle, true) : null;
				return status;
			}
		}

		[DllImport (Constants.CoreMediaIOLibrary)]
		static extern int CMIOSampleBufferCreateForImageBuffer (IntPtr allocator, IntPtr imageBuffer, IntPtr formatDescription, in CMSampleTimingInfo sampleTiming, ulong sequenceNumber, CMIOSampleBufferDiscontinuityFlags discontinuityFlags, out IntPtr sampleBuffer);

		/// <summary>Creates a sample buffer for an image with Core Media I/O attachments, returning an OSStatus error code or zero on success.</summary>
		public static int CreateForImageBuffer (CVImageBuffer imageBuffer, CMVideoFormatDescription formatDescription, CMSampleTimingInfo sampleTiming, ulong sequenceNumber, CMIOSampleBufferDiscontinuityFlags discontinuityFlags, out CMSampleBuffer? sampleBuffer)
		{
			var status = CMIOSampleBufferCreateForImageBuffer (IntPtr.Zero, imageBuffer.GetNonNullHandle (nameof (imageBuffer)), formatDescription.GetNonNullHandle (nameof (formatDescription)), in sampleTiming, sequenceNumber, discontinuityFlags, out var handle);
			GC.KeepAlive (imageBuffer);
			GC.KeepAlive (formatDescription);
			sampleBuffer = status == 0 ? new CMSampleBuffer (handle, true) : null;
			return status;
		}

		[DllImport (Constants.CoreMediaIOLibrary)]
		static extern int CMIOSampleBufferCreateNoDataMarker (IntPtr allocator, CMIOSampleBufferNoDataEvent noDataEvent, IntPtr formatDescription, ulong sequenceNumber, CMIOSampleBufferDiscontinuityFlags discontinuityFlags, out IntPtr sampleBuffer);

		/// <summary>Creates a marker for missing media data, returning an OSStatus error code or zero on success.</summary>
		public static int CreateNoDataMarker (CMIOSampleBufferNoDataEvent noDataEvent, CMFormatDescription? formatDescription, ulong sequenceNumber, CMIOSampleBufferDiscontinuityFlags discontinuityFlags, out CMSampleBuffer? sampleBuffer)
		{
			var status = CMIOSampleBufferCreateNoDataMarker (IntPtr.Zero, noDataEvent, formatDescription.GetHandle (), sequenceNumber, discontinuityFlags, out var handle);
			GC.KeepAlive (formatDescription);
			sampleBuffer = status == 0 ? new CMSampleBuffer (handle, true) : null;
			return status;
		}

		[DllImport (Constants.CoreMediaIOLibrary)]
		static extern ulong CMIOSampleBufferGetSequenceNumber (IntPtr sampleBuffer);

		/// <summary>Gets the buffer's Core Media I/O sequence number.</summary>
		public static ulong GetSequenceNumber (CMSampleBuffer sampleBuffer)
		{
			var value = CMIOSampleBufferGetSequenceNumber (sampleBuffer.GetNonNullHandle (nameof (sampleBuffer)));
			GC.KeepAlive (sampleBuffer);
			return value;
		}

		[DllImport (Constants.CoreMediaIOLibrary)]
		static extern void CMIOSampleBufferSetSequenceNumber (IntPtr allocator, IntPtr sampleBuffer, ulong sequenceNumber);

		/// <summary>Sets the buffer's Core Media I/O sequence number.</summary>
		public static void SetSequenceNumber (CMSampleBuffer sampleBuffer, ulong sequenceNumber)
		{
			CMIOSampleBufferSetSequenceNumber (IntPtr.Zero, sampleBuffer.GetNonNullHandle (nameof (sampleBuffer)), sequenceNumber);
			GC.KeepAlive (sampleBuffer);
		}

		[DllImport (Constants.CoreMediaIOLibrary)]
		static extern CMIOSampleBufferDiscontinuityFlags CMIOSampleBufferGetDiscontinuityFlags (IntPtr sampleBuffer);

		/// <summary>Gets the buffer's Core Media I/O discontinuity flags.</summary>
		public static CMIOSampleBufferDiscontinuityFlags GetDiscontinuityFlags (CMSampleBuffer sampleBuffer)
		{
			var value = CMIOSampleBufferGetDiscontinuityFlags (sampleBuffer.GetNonNullHandle (nameof (sampleBuffer)));
			GC.KeepAlive (sampleBuffer);
			return value;
		}

		[DllImport (Constants.CoreMediaIOLibrary)]
		static extern void CMIOSampleBufferSetDiscontinuityFlags (IntPtr allocator, IntPtr sampleBuffer, CMIOSampleBufferDiscontinuityFlags discontinuityFlags);

		/// <summary>Sets the buffer's Core Media I/O discontinuity flags.</summary>
		public static void SetDiscontinuityFlags (CMSampleBuffer sampleBuffer, CMIOSampleBufferDiscontinuityFlags discontinuityFlags)
		{
			CMIOSampleBufferSetDiscontinuityFlags (IntPtr.Zero, sampleBuffer.GetNonNullHandle (nameof (sampleBuffer)), discontinuityFlags);
			GC.KeepAlive (sampleBuffer);
		}

		[DllImport (Constants.CoreMediaIOLibrary)]
		static extern int CMIOSampleBufferCopyNonRequiredAttachments (IntPtr source, IntPtr destination, CMAttachmentMode attachmentMode);

		/// <summary>Copies optional buffer attachments using the specified propagation mode, returning an OSStatus error code or zero on success.</summary>
		public static int CopyNonRequiredAttachments (CMSampleBuffer source, CMSampleBuffer destination, CMAttachmentMode attachmentMode)
		{
			var status = CMIOSampleBufferCopyNonRequiredAttachments (source.GetNonNullHandle (nameof (source)), destination.GetNonNullHandle (nameof (destination)), attachmentMode);
			GC.KeepAlive (source);
			GC.KeepAlive (destination);
			return status;
		}

		[DllImport (Constants.CoreMediaIOLibrary)]
		static extern int CMIOSampleBufferCopySampleAttachments (IntPtr source, IntPtr destination);

		/// <summary>Copies per-sample attachments, returning an OSStatus error code or zero on success.</summary>
		public static int CopySampleAttachments (CMSampleBuffer source, CMSampleBuffer destination)
		{
			var status = CMIOSampleBufferCopySampleAttachments (source.GetNonNullHandle (nameof (source)), destination.GetNonNullHandle (nameof (destination)));
			GC.KeepAlive (source);
			GC.KeepAlive (destination);
			return status;
		}
	}
}
