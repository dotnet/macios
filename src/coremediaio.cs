// Copyright (c) Microsoft Corporation.
// Licensed under the MIT License.

#nullable enable

namespace CoreMediaIO {
	/// <summary>Attachment keys used by Core Media I/O sample buffers.</summary>
	[NoiOS, NoTV, MacCatalyst (13, 1)]
	[Static]
	interface CMIOSampleBufferAttachmentKey {
		/// <summary>The sample buffer's sequence number.</summary>
		[Field ("kCMIOSampleBufferAttachmentKey_SequenceNumber")]
		NSString SequenceNumber { get; }

		/// <summary>The host time associated with the sample buffer.</summary>
		[Field ("kCMIOSampleBufferAttachmentKey_HostTime")]
		NSString HostTime { get; }

		/// <summary>Flags describing discontinuities in the stream.</summary>
		[Field ("kCMIOSampleBufferAttachmentKey_DiscontinuityFlags")]
		NSString DiscontinuityFlags { get; }
	}
}
