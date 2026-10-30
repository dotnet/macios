// Copyright (c) Microsoft Corporation.
// Licensed under the MIT License.

#nullable enable

using CoreMedia;

namespace CoreMediaIO {
	/// <summary>Provides access to Core Media I/O stream buffer queues and deck controls.</summary>
	[SupportedOSPlatform ("macos")]
	[SupportedOSPlatform ("maccatalyst")]
	[UnsupportedOSPlatform ("ios")]
	[UnsupportedOSPlatform ("tvos")]
	public static class CMIOStream {
		[DllImport (Constants.CoreMediaIOLibrary)]
		static extern unsafe int CMIOStreamCopyBufferQueue (uint streamId, delegate* unmanaged<uint, IntPtr, IntPtr, void> queueAlteredCallback, IntPtr context, out IntPtr queue);

		/// <summary>Copies a stream's buffer queue, returning an OSStatus error code or zero on success.</summary>
		/// <remarks>
		///   <para>Dispose the returned queue when finished. Queue elements are unmanaged pointers; for input streams they are sample buffers whose ownership transfers to the caller when dequeued.</para>
		///   <para>The optional unmanaged callback receives the stream identifier, a token, and the context. Only the most recently registered callback is invoked. Pass a null callback to unregister it; this still returns a queue that must be disposed. Disposing a queue does not unregister the callback. Keep the callback and context valid while the stream can invoke them. Exceptions must not escape the callback.</para>
		/// </remarks>
		public static unsafe int CopyBufferQueue (uint streamId, delegate* unmanaged<uint, IntPtr, IntPtr, void> queueAlteredCallback, IntPtr context, out CMSimpleQueue? queue)
		{
			var status = CMIOStreamCopyBufferQueue (streamId, queueAlteredCallback, context, out var handle);
			queue = status == 0 ? new CMSimpleQueue (handle, true) : null;
			return status;
		}

		[DllImport (Constants.CoreMediaIOLibrary)]
		static extern int CMIOStreamDeckPlay (uint streamId);

		/// <summary>Starts the stream's deck, returning an OSStatus error code or zero on success.</summary>
		public static int DeckPlay (uint streamId) => CMIOStreamDeckPlay (streamId);

		[DllImport (Constants.CoreMediaIOLibrary)]
		static extern int CMIOStreamDeckStop (uint streamId);

		/// <summary>Stops the stream's deck, returning an OSStatus error code or zero on success.</summary>
		public static int DeckStop (uint streamId) => CMIOStreamDeckStop (streamId);

		[DllImport (Constants.CoreMediaIOLibrary)]
		static extern int CMIOStreamDeckJog (uint streamId, int speed);

		/// <summary>Winds the stream's deck at the specified shuttle speed, returning an OSStatus error code or zero on success.</summary>
		public static int DeckJog (uint streamId, int speed) => CMIOStreamDeckJog (streamId, speed);

		[DllImport (Constants.CoreMediaIOLibrary)]
		static extern int CMIOStreamDeckCueTo (uint streamId, ulong frameNumber, byte playOnCue);

		/// <summary>Cues the stream's deck to a frame, optionally starting playback, and returns an OSStatus error code or zero on success.</summary>
		public static int DeckCueTo (uint streamId, ulong frameNumber, bool playOnCue) => CMIOStreamDeckCueTo (streamId, frameNumber, playOnCue.AsByte ());
	}
}
