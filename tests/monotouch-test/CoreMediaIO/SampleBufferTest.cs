// Copyright (c) Microsoft Corporation.
// Licensed under the MIT License.

#nullable enable

#if __MACOS__ || __MACCATALYST__

using CoreMedia;
using CoreMediaIO;
using CoreVideo;

namespace MonoTouchFixtures.CoreMediaIO {
	[TestFixture]
	[Preserve (AllMembers = true)]
	public class SampleBufferTest {
		[Test]
		public void SequenceNumbers ()
		{
			Assert.That (CMIOSampleBuffer.GetNextSequenceNumber (0), Is.EqualTo (1), "Increment");
			Assert.That (CMIOSampleBuffer.GetNextSequenceNumber (ulong.MaxValue - 1), Is.EqualTo (CMIOSampleBuffer.InvalidSequenceNumber), "Last valid");
			Assert.That (CMIOSampleBuffer.GetNextSequenceNumber (CMIOSampleBuffer.InvalidSequenceNumber), Is.Zero, "Wrap");
			Assert.That (CMIOSampleBuffer.HasHardDiscontinuities (CMIOSampleBufferDiscontinuityFlags.None), Is.False, "None");
			Assert.That (CMIOSampleBuffer.HasHardDiscontinuities (CMIOSampleBufferDiscontinuityFlags.DurationWasExtended), Is.False, "Soft");
			Assert.That (CMIOSampleBuffer.HasHardDiscontinuities (CMIOSampleBufferDiscontinuityFlags.DurationWasExtended | CMIOSampleBufferDiscontinuityFlags.DataWasDropped), Is.True, "Hard");
		}

		[Test]
		public void NoDataMarker ()
		{
			Assert.That (CMIOSampleBuffer.CreateNoDataMarker (CMIOSampleBufferNoDataEvent.NoMedia, null, 42, CMIOSampleBufferDiscontinuityFlags.NoDataMarker, out var buffer), Is.Zero, "Create");
			if (buffer is null)
				throw new AssertionException ("No marker buffer returned.");
			using (buffer) {
				Assert.That (CMIOSampleBuffer.GetSequenceNumber (buffer), Is.EqualTo (42), "Sequence");
				CMIOSampleBuffer.SetSequenceNumber (buffer, 43);
				Assert.That (CMIOSampleBuffer.GetSequenceNumber (buffer), Is.EqualTo (43), "Updated sequence");
				Assert.That (CMIOSampleBuffer.GetDiscontinuityFlags (buffer), Is.EqualTo (CMIOSampleBufferDiscontinuityFlags.NoDataMarker), "Flags");
				CMIOSampleBuffer.SetDiscontinuityFlags (buffer, CMIOSampleBufferDiscontinuityFlags.DataWasDropped);
				Assert.That (CMIOSampleBuffer.GetDiscontinuityFlags (buffer), Is.EqualTo (CMIOSampleBufferDiscontinuityFlags.DataWasDropped), "Updated flags");
			}
			Assert.Throws<ObjectDisposedException> (() => CMIOSampleBuffer.GetSequenceNumber (buffer), "Disposed");
		}

		[Test]
		public void Create ()
		{
			using var format = CMFormatDescription.Create (CMMediaType.ClosedCaption, (uint) CMClosedCaptionFormatType.CEA608, out var formatError);
			Assert.That (formatError, Is.EqualTo (CMFormatDescriptionError.None), "Format");
			if (format is null)
				throw new AssertionException ("No format description returned.");
			var timing = new CMSampleTimingInfo {
				Duration = new CMTime (1, 30),
				PresentationTimeStamp = CMTime.Zero,
				DecodeTimeStamp = CMTime.Invalid,
			};
			Assert.That (CMIOSampleBuffer.Create (null, format, 1, [timing], [], 7, CMIOSampleBufferDiscontinuityFlags.None, out var buffer), Is.Zero, "Create");
			if (buffer is null)
				throw new AssertionException ("No sample buffer returned.");
			using (buffer) {
				Assert.That (CMIOSampleBuffer.GetSequenceNumber (buffer), Is.EqualTo (7), "Sequence");
				Assert.That (CMIOSampleBuffer.GetDiscontinuityFlags (buffer), Is.EqualTo (CMIOSampleBufferDiscontinuityFlags.None), "Flags");
				Assert.That (buffer.NumSamples, Is.EqualTo (1), "Samples");
			}
		}

		[Test]
		public void SpanValidation ()
		{
			using var format = CMFormatDescription.Create (CMMediaType.ClosedCaption, (uint) CMClosedCaptionFormatType.CEA608, out var error);
			Assert.That (error, Is.EqualTo (CMFormatDescriptionError.None), "Format");
			if (format is null)
				throw new AssertionException ("No format description returned.");
			Assert.Throws<ArgumentException> (() => CMIOSampleBuffer.Create (null, format, 3, new CMSampleTimingInfo [2], [], 0, 0, out _), "Timing count");
			Assert.Throws<ArgumentException> (() => CMIOSampleBuffer.Create (null, format, 3, [], new nuint [2], 0, 0, out _), "Size count");
		}

		[Test]
		public void ImageBuffer ()
		{
			using var image = new CVPixelBuffer (16, 16, CVPixelFormatType.CV32BGRA);
			using var format = CMVideoFormatDescription.CreateForImageBuffer (image, out var formatError);
			Assert.That (formatError, Is.EqualTo (CMFormatDescriptionError.None), "Format");
			if (format is null)
				throw new AssertionException ("No format description returned.");
			var timing = new CMSampleTimingInfo {
				Duration = new CMTime (1, 30),
				PresentationTimeStamp = CMTime.Zero,
				DecodeTimeStamp = CMTime.Invalid,
			};
			Assert.That (CMIOSampleBuffer.CreateForImageBuffer (image, format, timing, 12, CMIOSampleBufferDiscontinuityFlags.Unknown, out var buffer), Is.Zero, "Create");
			if (buffer is null)
				throw new AssertionException ("No image sample buffer returned.");
			using (buffer) {
				Assert.That (CMIOSampleBuffer.GetSequenceNumber (buffer), Is.EqualTo (12), "Sequence");
				Assert.That (CMIOSampleBuffer.GetDiscontinuityFlags (buffer), Is.EqualTo (CMIOSampleBufferDiscontinuityFlags.Unknown), "Flags");
				Assert.That (buffer.NumSamples, Is.EqualTo (1), "Samples");
			}
		}

		[Test]
		public void CopyAttachments ()
		{
			Assert.That (CMIOSampleBuffer.CreateNoDataMarker (CMIOSampleBufferNoDataEvent.NoMedia, null, 1, 0, out var source), Is.Zero, "Source");
			if (source is null)
				throw new AssertionException ("No source buffer returned.");
			using (source) {
				Assert.That (CMIOSampleBuffer.CreateNoDataMarker (CMIOSampleBufferNoDataEvent.NoMedia, null, 2, 0, out var destination), Is.Zero, "Destination");
				if (destination is null)
					throw new AssertionException ("No destination buffer returned.");
				using (destination) {
					Assert.That (CMIOSampleBuffer.CopyNonRequiredAttachments (source, destination, CMAttachmentMode.ShouldPropagate), Is.Zero, "Copy buffer attachments");
					Assert.That (CMIOSampleBuffer.CopySampleAttachments (source, destination), Is.Zero, "Copy sample attachments");
					Assert.That (CMIOSampleBuffer.GetSequenceNumber (destination), Is.EqualTo (2), "Required sequence preserved");
				}
			}
		}
	}
}

#endif
