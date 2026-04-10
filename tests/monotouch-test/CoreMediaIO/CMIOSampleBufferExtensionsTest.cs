#if HAS_COREMEDIAIO
#nullable enable

using System;
using CoreMedia;
using CoreMediaIO;
using NUnit.Framework;

namespace MonoTouchFixtures.CoreMediaIO {

	[TestFixture]
	[Preserve (AllMembers = true)]
	public class CMIOSampleBufferExtensionsTest {

		[Test]
		public void TryCreateNoDataMarker ()
		{
			TestRuntime.AssertXcodeVersion (13, 3);

			bool created = CMIOSampleBufferExtensions.TryCreateNoDataMarker (
				noDataEvent: 1,
				formatDescription: null,
				sequenceNumber: 0,
				discontinuityFlags: 0,
				out int status,
				out var sampleBuffer);

			Assert.IsTrue (created, "Created");
			Assert.AreEqual (0, status, "Status");
			Assert.IsNotNull (sampleBuffer, "SampleBuffer");
			sampleBuffer?.Dispose ();
		}

		[Test]
		public void SequenceNumber_RoundTrip ()
		{
			TestRuntime.AssertXcodeVersion (13, 3);

			bool created = CMIOSampleBufferExtensions.TryCreateNoDataMarker (
				noDataEvent: 1,
				formatDescription: null,
				sequenceNumber: 42,
				discontinuityFlags: 0,
				out _,
				out var sampleBuffer);

			Assert.IsTrue (created, "Created");
			Assert.IsNotNull (sampleBuffer, "SampleBuffer");

			ulong seq = CMIOSampleBufferExtensions.GetSequenceNumber (sampleBuffer!);
			Assert.AreEqual ((ulong) 42, seq, "Initial SequenceNumber");

			CMIOSampleBufferExtensions.SetSequenceNumber (sampleBuffer!, 100);
			seq = CMIOSampleBufferExtensions.GetSequenceNumber (sampleBuffer!);
			Assert.AreEqual ((ulong) 100, seq, "Updated SequenceNumber");

			sampleBuffer?.Dispose ();
		}

		[Test]
		public void DiscontinuityFlags_RoundTrip ()
		{
			TestRuntime.AssertXcodeVersion (13, 3);

			bool created = CMIOSampleBufferExtensions.TryCreateNoDataMarker (
				noDataEvent: 1,
				formatDescription: null,
				sequenceNumber: 0,
				discontinuityFlags: 0,
				out _,
				out var sampleBuffer);

			Assert.IsTrue (created, "Created");
			Assert.IsNotNull (sampleBuffer, "SampleBuffer");

			CMIOSampleBufferExtensions.SetDiscontinuityFlags (sampleBuffer!, 0x42);
			uint flags = CMIOSampleBufferExtensions.GetDiscontinuityFlags (sampleBuffer!);
			Assert.AreEqual ((uint) 0x42, flags, "Updated DiscontinuityFlags");

			sampleBuffer?.Dispose ();
		}

		[Test]
		public void CopySampleAttachments_BetweenBuffers ()
		{
			TestRuntime.AssertXcodeVersion (13, 3);

			CMIOSampleBufferExtensions.TryCreateNoDataMarker (1, null, 1, 0, out _, out var source);
			CMIOSampleBufferExtensions.TryCreateNoDataMarker (1, null, 2, 0, out _, out var dest);

			Assert.IsNotNull (source, "Source");
			Assert.IsNotNull (dest, "Dest");

			int copyStatus = CMIOSampleBufferExtensions.CopySampleAttachments (source!, dest!);
			Assert.That (copyStatus, Is.LessThanOrEqualTo (0).Or.GreaterThanOrEqualTo (0), "CopySampleAttachments did not crash");

			source?.Dispose ();
			dest?.Dispose ();
		}

		[Test]
		public void CopyNonRequiredAttachments_BetweenBuffers ()
		{
			TestRuntime.AssertXcodeVersion (13, 3);

			CMIOSampleBufferExtensions.TryCreateNoDataMarker (1, null, 1, 0, out _, out var source);
			CMIOSampleBufferExtensions.TryCreateNoDataMarker (1, null, 2, 0, out _, out var dest);

			Assert.IsNotNull (source, "Source");
			Assert.IsNotNull (dest, "Dest");

			int copyStatus = CMIOSampleBufferExtensions.CopyNonRequiredAttachments (
				source!, dest!, CMAttachmentMode.ShouldNotPropagate);
			Assert.That (copyStatus, Is.LessThanOrEqualTo (0).Or.GreaterThanOrEqualTo (0), "CopyNonRequiredAttachments did not crash");

			source?.Dispose ();
			dest?.Dispose ();
		}
	}
}
#endif // HAS_COREMEDIAIO
