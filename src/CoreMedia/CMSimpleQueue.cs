// Copyright (c) Microsoft Corporation.
// Licensed under the MIT License.

#nullable enable

using CoreFoundation;

namespace CoreMedia {
	/// <summary>A fixed-capacity, single-reader, single-writer queue of unmanaged pointers.</summary>
	/// <remarks>The queue does not retain or release its elements. The caller manages their lifetimes. Enqueue and dequeue may run concurrently on one writer and one reader; other access must be synchronized by the caller.</remarks>
	[SupportedOSPlatform ("ios")]
	[SupportedOSPlatform ("tvos")]
	[SupportedOSPlatform ("macos")]
	[SupportedOSPlatform ("maccatalyst")]
	public class CMSimpleQueue : NativeObject {
		[Preserve (Conditional = true)]
		internal CMSimpleQueue (NativeHandle handle, bool owns) : base (handle, owns) { }

		[DllImport (Constants.CoreMediaLibrary)]
		static extern int CMSimpleQueueCreate (IntPtr allocator, int capacity, out IntPtr queue);

		/// <summary>Creates a queue with a positive capacity, returning an OSStatus error code or zero on success.</summary>
		public static int Create (int capacity, out CMSimpleQueue? queue)
		{
			var status = CMSimpleQueueCreate (IntPtr.Zero, capacity, out var handle);
			queue = status == 0 ? new CMSimpleQueue (handle, true) : null;
			return status;
		}

		[DllImport (Constants.CoreMediaLibrary)]
		static extern int CMSimpleQueueEnqueue (IntPtr queue, IntPtr element);

		/// <summary>Enqueues a non-null pointer, returning an OSStatus error code or zero on success.</summary>
		public int Enqueue (IntPtr element)
		{
			if (element == IntPtr.Zero)
				throw new ArgumentException ("The queue element must not be null.", nameof (element));
			return CMSimpleQueueEnqueue (GetCheckedHandle (), element);
		}

		[DllImport (Constants.CoreMediaLibrary)]
		static extern IntPtr CMSimpleQueueDequeue (IntPtr queue);

		/// <summary>Removes and returns the head pointer, or zero if the queue is empty.</summary>
		public IntPtr Dequeue () => CMSimpleQueueDequeue (GetCheckedHandle ());

		[DllImport (Constants.CoreMediaLibrary)]
		static extern IntPtr CMSimpleQueueGetHead (IntPtr queue);

		/// <summary>Gets the head pointer without removing it, or zero if the queue is empty.</summary>
		public IntPtr Head => CMSimpleQueueGetHead (GetCheckedHandle ());

		[DllImport (Constants.CoreMediaLibrary)]
		static extern int CMSimpleQueueReset (IntPtr queue);

		/// <summary>Empties the queue without releasing its elements, returning an OSStatus error code or zero on success. Reader and writer access must be suspended during this call.</summary>
		public int Reset () => CMSimpleQueueReset (GetCheckedHandle ());

		[DllImport (Constants.CoreMediaLibrary)]
		static extern int CMSimpleQueueGetCapacity (IntPtr queue);

		/// <summary>Gets the maximum number of elements the queue can hold.</summary>
		public int Capacity => CMSimpleQueueGetCapacity (GetCheckedHandle ());

		[DllImport (Constants.CoreMediaLibrary)]
		static extern int CMSimpleQueueGetCount (IntPtr queue);

		/// <summary>Gets the number of elements currently in the queue.</summary>
		public int Count => CMSimpleQueueGetCount (GetCheckedHandle ());

		[DllImport (Constants.CoreMediaLibrary)]
		static extern nint CMSimpleQueueGetTypeID ();

		/// <summary>Gets the Core Foundation type identifier for simple queues.</summary>
		public static nint GetTypeID () => CMSimpleQueueGetTypeID ();
	}
}
