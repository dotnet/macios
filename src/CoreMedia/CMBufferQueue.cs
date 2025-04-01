//
// CMBufferQueue.cs: Implements the CMBufferQueue and CMBuffer managed bindings
//
// Authors:
//   Miguel de Icaza
//
// Copyright 2012-2016 Xamarin Inc. All rights reserved.
//
//

#nullable enable

using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;

using Foundation;
using CoreFoundation;
using ObjCRuntime;

namespace CoreMedia {

	public delegate CMTime CMBufferGetTime (INativeObject buffer);
	public delegate bool CMBufferGetBool (INativeObject buffer);
	public delegate int CMBufferCompare (INativeObject first, INativeObject second);

	// [SupportedOSPlatform ("ios")] -  SupportedOSPlatform is not valid on this declaration type "delegate" 
	public delegate nint CMBufferGetSize (INativeObject buffer);

	[SupportedOSPlatform ("ios")]
	[SupportedOSPlatform ("maccatalyst")]
	[SupportedOSPlatform ("macos")]
	[SupportedOSPlatform ("tvos")]
	public class CMBufferQueue : NativeObject {
#if !COREBUILD
		GCHandle gch;
		Dictionary<IntPtr, INativeObject> queueObjects;
		CMBufferGetTime? getDecodeTimeStamp;
		CMBufferGetTime? getPresentationTimeStamp;
		CMBufferGetTime? getDuration;
		CMBufferGetBool? isDataReady;
		CMBufferCompare? compare;
		CMBufferGetSize? getTotalSize;

		[StructLayout (LayoutKind.Sequential)]
		struct CMBufferCallbacks {
			internal uint version;
			internal IntPtr refcon;
			internal unsafe delegate* unmanaged<IntPtr, IntPtr, CMTime> XgetDecodeTimeStamp;
			internal unsafe delegate* unmanaged<IntPtr, IntPtr, CMTime> XgetPresentationTimeStamp;
			internal unsafe delegate* unmanaged<IntPtr, IntPtr, CMTime> XgetDuration;
			internal unsafe delegate* unmanaged<IntPtr, IntPtr, byte> XisDataReady;
			internal unsafe delegate* unmanaged<IntPtr, IntPtr, IntPtr, int> Xcompare;
			internal IntPtr cfStringPtr_dataBecameReadyNotification;
			internal unsafe delegate* unmanaged<IntPtr, IntPtr, nint> XgetSize;
		}

		// A version with no delegates, just native pointers
		[StructLayout (LayoutKind.Sequential)]
		struct CMBufferCallbacks2 {
			internal uint version;
			internal IntPtr refcon;
			internal IntPtr XgetDecodeTimeStamp;
			internal IntPtr XgetPresentationTimeStamp;
			internal IntPtr XgetDuration;
			internal IntPtr XisDataReady;
			internal IntPtr Xcompare;
			internal IntPtr cfStringPtr_dataBecameReadyNotification;
			internal IntPtr XgetSize;
		}

		/// <include file="../../docs/api/CoreMedia/CMBufferQueue.xml" path="/Documentation/Docs[@DocId='M:CoreMedia.CMBufferQueue.Dispose(System.Boolean)']/*" />
		protected override void Dispose (bool disposing)
		{
			queueObjects.Clear ();
			if (gch.IsAllocated)
				gch.Free ();
			base.Dispose (disposing);
		}

		// CMItemCount -> CMBase.h (looks weird but it's 4 bytes in 32bits and 8 bytes in 64bits, x86_64 and ARM64)

		[DllImport (Constants.CoreMediaLibrary)]
		unsafe extern static OSStatus CMBufferQueueCreate (/* CFAllocatorRef */ IntPtr allocator, /* CMItemCount */ nint capacity, CMBufferCallbacks* cbacks, /* CMBufferQueueRef* */ IntPtr* result);

		CMBufferQueue (IntPtr handle, bool owns, int count)
			: base (handle, owns)
		{
			queueObjects = new Dictionary<IntPtr, INativeObject> (count, Runtime.IntPtrEqualityComparer);
			gch = GCHandle.Alloc (this);
		}

		CMBufferQueue (int count)
			: this (IntPtr.Zero, true, count)
		{
		}

		// for compatibility with 7.0 and earlier
		/// <param name="count">Number of items in the queue.</param>
		///         <param name="getDecodeTimeStamp">To be added.</param>
		///         <param name="getPresentationTimeStamp">To be added.</param>
		///         <param name="getDuration">To be added.</param>
		///         <param name="isDataReady">To be added.</param>
		///         <param name="compare">To be added.</param>
		///         <param name="dataBecameReadyNotification">To be added.</param>
		///         <summary>Creates a custom CMBufferQueue that sorts and returns the objects in the queue based on the various callbacks you provide.</summary>
		///         <returns>To be added.</returns>
		///         <remarks>To be added.</remarks>
		public static CMBufferQueue? FromCallbacks (int count, CMBufferGetTime? getDecodeTimeStamp, CMBufferGetTime? getPresentationTimeStamp, CMBufferGetTime? getDuration,
			CMBufferGetBool? isDataReady, CMBufferCompare? compare, NSString dataBecameReadyNotification)
		{
			return FromCallbacks (count, getDecodeTimeStamp, getPresentationTimeStamp, getDuration, isDataReady,
				compare, dataBecameReadyNotification, null);
		}

		/// <param name="count">To be added.</param>
		///         <param name="getDecodeTimeStamp">To be added.</param>
		///         <param name="getPresentationTimeStamp">To be added.</param>
		///         <param name="getDuration">To be added.</param>
		///         <param name="isDataReady">To be added.</param>
		///         <param name="compare">To be added.</param>
		///         <param name="dataBecameReadyNotification">To be added.</param>
		///         <param name="getTotalSize">To be added.</param>
		///         <summary>To be added.</summary>
		///         <returns>To be added.</returns>
		///         <remarks>To be added.</remarks>
		public static CMBufferQueue? FromCallbacks (int count, CMBufferGetTime? getDecodeTimeStamp, CMBufferGetTime? getPresentationTimeStamp, CMBufferGetTime? getDuration,
			CMBufferGetBool? isDataReady, CMBufferCompare? compare, NSString dataBecameReadyNotification, CMBufferGetSize? getTotalSize)
		{
			var bq = new CMBufferQueue (count);
			CMBufferCallbacks cbacks;
			unsafe {
				cbacks = new CMBufferCallbacks () {
					version = (uint) (getTotalSize is null ? 0 : 1),
					refcon = GCHandle.ToIntPtr (bq.gch),
					XgetDecodeTimeStamp = getDecodeTimeStamp is not null ? &GetDecodeTimeStamp : null,
					XgetPresentationTimeStamp = getPresentationTimeStamp is not null ? &GetPresentationTimeStamp : null,
					XgetDuration = getDuration is not null ? &GetDuration : null,
					XisDataReady = isDataReady is not null ? &GetDataReady : null,
					Xcompare = compare is not null ? &Compare : null,
					cfStringPtr_dataBecameReadyNotification = dataBecameReadyNotification is null ? IntPtr.Zero : dataBecameReadyNotification.Handle,
					XgetSize = getTotalSize is not null ? &GetTotalSize : null,
				};
				GC.KeepAlive (dataBecameReadyNotification);
			}

			bq.getDecodeTimeStamp = getDecodeTimeStamp;
			bq.getPresentationTimeStamp = getPresentationTimeStamp;
			bq.getDuration = getDuration;
			bq.isDataReady = isDataReady;
			bq.compare = compare;
			bq.getTotalSize = getTotalSize;

			OSStatus rv;
			IntPtr handle;
			unsafe {
				rv = CMBufferQueueCreate (IntPtr.Zero, count, &cbacks, &handle);
			}
			if (rv == 0) {
				bq.InitializeHandle (handle);
				return bq;
			}

			bq.Dispose ();
			return null;
		}

		[DllImport (Constants.CoreMediaLibrary)]
		unsafe extern static /* CMBufferCallbacks */ CMBufferCallbacks* CMBufferQueueGetCallbacksForUnsortedSampleBuffers ();

		/// <param name="count">Number of items in the queue.</param>
		///         <summary>To be added.</summary>
		///         <returns>To be added.</returns>
		///         <remarks>To be added.</remarks>
		public static CMBufferQueue? CreateUnsorted (int count)
		{
			// note: different version of iOS can return a different (size) structure, e.g. iOS 7.1,
			// that structure might not map to our `CMBufferCallbacks2` managed representation
			// and can cause a crash, e.g. bug #17330
			// since we don't need the managed bcallbacks (it's the native callback that will be used for this queue)
			// then we can simply use an IntPtr to represent them (no GCHandle)

			IntPtr handle;
			unsafe {
				var callbacks = CMBufferQueueGetCallbacksForUnsortedSampleBuffers ();
				if (CMBufferQueueCreate (IntPtr.Zero, count, callbacks, &handle) == 0)
					return new CMBufferQueue (handle, true, count);
			}

			return null;
		}

		[DllImport (Constants.CoreMediaLibrary)]
		extern static OSStatus CMBufferQueueEnqueue (/* CMBufferQueueRef */ IntPtr queue, /* CMBufferRef */ IntPtr buf);

		//
		// It really should be ICFType, and we should pepper various classes with ICFType
		//
		/// <param name="cftypeBuffer">To be added.</param>
		///         <summary>To be added.</summary>
		///         <remarks>To be added.</remarks>
		public void Enqueue (INativeObject cftypeBuffer)
		{
			if (cftypeBuffer is null)
				ObjCRuntime.ThrowHelper.ThrowArgumentNullException (nameof (cftypeBuffer));
			lock (queueObjects) {
				var cfh = cftypeBuffer.Handle;
				CMBufferQueueEnqueue (Handle, cfh);
				if (!queueObjects.ContainsKey (cfh))
					queueObjects [cfh] = cftypeBuffer;
			}
		}

		[DllImport (Constants.CoreMediaLibrary)]
		extern static /* CMBufferRef */ IntPtr CMBufferQueueDequeueAndRetain (/* CMBufferQueueRef */ IntPtr queue);

		/// <summary>To be added.</summary>
		///         <returns>To be added.</returns>
		///         <remarks>To be added.</remarks>
		public INativeObject? Dequeue ()
		{
			//
			// Our managed objects already take a reference on the object,
			// and by keeping the objects alive in the `queueObjects'
			// dictionary, we kept the reference alive.   So we need to
			// release the newly acquired reference
			//
			var oHandle = CMBufferQueueDequeueAndRetain (Handle);
			if (oHandle == IntPtr.Zero)
				return null;

			CFObject.CFRelease (oHandle);
			lock (queueObjects) {
				var managed = queueObjects [oHandle];
				queueObjects.Remove (oHandle);
				return managed;
			}
		}

		[DllImport (Constants.CoreMediaLibrary)]
		extern static /* CMBufferRef */ IntPtr CMBufferQueueDequeueIfDataReadyAndRetain (/* CMBufferQueueRef */ IntPtr queue);

		/// <summary>To be added.</summary>
		///         <returns>To be added.</returns>
		///         <remarks>To be added.</remarks>
		public INativeObject? DequeueIfDataReady ()
		{
			//
			// Our managed objects already take a reference on the object,
			// and by keeping the objects alive in the `queueObjects'
			// dictionary, we kept the reference alive.   So we need to
			// release the newly acquired reference
			//
			var oHandle = CMBufferQueueDequeueIfDataReadyAndRetain (Handle);
			if (oHandle == IntPtr.Zero)
				return null;

			CFObject.CFRelease (oHandle);
			lock (queueObjects) {
				var managed = queueObjects [oHandle];
				queueObjects.Remove (oHandle);
				return managed;
			}
		}

		[DllImport (Constants.CoreMediaLibrary)]
		extern static byte CMBufferQueueIsEmpty (/* CMBufferQueueRef */ IntPtr queue);
		/// <summary>To be added.</summary>
		///         <value>To be added.</value>
		///         <remarks>To be added.</remarks>
		public bool IsEmpty {
			get {
				return CMBufferQueueIsEmpty (Handle) != 0;
			}
		}


		[DllImport (Constants.CoreMediaLibrary)]
		extern static OSStatus CMBufferQueueMarkEndOfData (/* CMBufferQueueRef */ IntPtr queue);
		/// <summary>To be added.</summary>
		///         <returns>To be added.</returns>
		///         <remarks>To be added.</remarks>
		public int MarkEndOfData ()
		{
			return CMBufferQueueMarkEndOfData (Handle);
		}

		[DllImport (Constants.CoreMediaLibrary)]
		extern static byte CMBufferQueueContainsEndOfData (/* CMBufferQueueRef */ IntPtr queue);
		/// <summary>To be added.</summary>
		///         <value>To be added.</value>
		///         <remarks>To be added.</remarks>
		public bool ContainsEndOfData {
			get {
				return CMBufferQueueContainsEndOfData (Handle) != 0;
			}
		}

		[DllImport (Constants.CoreMediaLibrary)]
		extern static byte CMBufferQueueIsAtEndOfData (/* CMBufferQueueRef */ IntPtr queue);
		/// <summary>To be added.</summary>
		///         <value>To be added.</value>
		///         <remarks>To be added.</remarks>
		public bool IsAtEndOfData {
			get {
				return CMBufferQueueIsAtEndOfData (Handle) != 0;
			}
		}

		[DllImport (Constants.CoreMediaLibrary)]
		extern static OSStatus CMBufferQueueReset (/* CMBufferQueueRef */ IntPtr queue);
		/// <summary>To be added.</summary>
		///         <returns>To be added.</returns>
		///         <remarks>To be added.</remarks>
		public OSStatus Reset ()
		{
			return CMBufferQueueReset (Handle);
		}

		[DllImport (Constants.CoreMediaLibrary)]
		extern static nint CMBufferQueueGetBufferCount (/* CMBufferQueueRef */ IntPtr queue);
		/// <summary>To be added.</summary>
		///         <value>To be added.</value>
		///         <remarks>To be added.</remarks>
		public nint BufferCount {
			get {
				return CMBufferQueueGetBufferCount (Handle);
			}
		}

		[DllImport (Constants.CoreMediaLibrary)]
		extern static CMTime CMBufferQueueGetDuration (/* CMBufferQueueRef */ IntPtr queue);
		/// <summary>To be added.</summary>
		///         <value>To be added.</value>
		///         <remarks>To be added.</remarks>
		public CMTime Duration {
			get {
				return CMBufferQueueGetDuration (Handle);
			}
		}

		[SupportedOSPlatform ("ios")]
		[SupportedOSPlatform ("macos")]
		[SupportedOSPlatform ("maccatalyst")]
		[SupportedOSPlatform ("tvos")]
		[DllImport (Constants.CoreMediaLibrary)]
		extern static /* size_t */ nint CMBufferQueueGetTotalSize (/* CMBufferQueueRef */ IntPtr queue);

		/// <summary>To be added.</summary>
		///         <returns>To be added.</returns>
		///         <remarks>To be added.</remarks>
		[SupportedOSPlatform ("ios")]
		[SupportedOSPlatform ("macos")]
		[SupportedOSPlatform ("maccatalyst")]
		[SupportedOSPlatform ("tvos")]
		public nint GetTotalSize ()
		{
			return CMBufferQueueGetTotalSize (Handle);
		}


		// Surfaces the given buffer pointer to a managed object
		INativeObject Surface (IntPtr v)
		{
			return queueObjects [v];
		}

		[UnmanagedCallersOnly]
		static CMTime GetDecodeTimeStamp (IntPtr buffer, IntPtr refcon)
		{
			var queue = (CMBufferQueue?) GCHandle.FromIntPtr (refcon).Target;
			if (queue?.getDecodeTimeStamp is null)
				return default (CMTime);
			return queue.getDecodeTimeStamp (queue.Surface (buffer));
		}

		[UnmanagedCallersOnly]
		static CMTime GetPresentationTimeStamp (IntPtr buffer, IntPtr refcon)
		{
			var queue = (CMBufferQueue?) GCHandle.FromIntPtr (refcon).Target;
			if (queue?.getPresentationTimeStamp is null)
				return default (CMTime);
			return queue.getPresentationTimeStamp (queue.Surface (buffer));
		}

		[UnmanagedCallersOnly]
		static CMTime GetDuration (IntPtr buffer, IntPtr refcon)
		{
			var queue = (CMBufferQueue?) GCHandle.FromIntPtr (refcon).Target;
			if (queue?.getDuration is null)
				return default (CMTime);
			return queue.getDuration (queue.Surface (buffer));
		}

		[UnmanagedCallersOnly]
		static byte GetDataReady (IntPtr buffer, IntPtr refcon)
		{
			var queue = (CMBufferQueue?) GCHandle.FromIntPtr (refcon).Target;
			if (queue?.isDataReady is null)
				return 0;
			return (byte) (queue.isDataReady (queue.Surface (buffer)) ? 1 : 0);
		}

		[UnmanagedCallersOnly]
		static int Compare (IntPtr buffer1, IntPtr buffer2, IntPtr refcon)
		{
			var queue = (CMBufferQueue?) GCHandle.FromIntPtr (refcon).Target;
			if (queue?.compare is null)
				return 0;
			return queue.compare (queue.Surface (buffer1), queue.Surface (buffer2));
		}

		[UnmanagedCallersOnly]
		static nint GetTotalSize (IntPtr buffer, IntPtr refcon)
		{
			var queue = (CMBufferQueue?) GCHandle.FromIntPtr (refcon).Target;
			if (queue?.getTotalSize is null)
				return 0;
			return queue.getTotalSize (queue.Surface (buffer));
		}
#endif // !COREBUILD

		public enum TriggerCondition {
			/// <summary>The trigger is raised when the elapsed time becomes less than the specified value.</summary>
			WhenDurationBecomesLessThan = 1,
			/// <summary>The trigger is raised when the elapsed time becomes less than or equal to the specified value.</summary>
			WhenDurationBecomesLessThanOrEqualTo = 2,
			/// <summary>The trigger is raised when the elapsed time is greater than the specified value.</summary>
			WhenDurationBecomesGreaterThan = 3,
			/// <summary>The trigger is raised when the elapsed time is greater than or equal to the specified value.</summary>
			WhenDurationBecomesGreaterThanOrEqualTo = 4,
			/// <summary>The trigger is raised when the minimum presentation time stamp changes.</summary>
			WhenMinPresentationTimeStampChanges = 5,
			/// <summary>The trigger is raised when the maximum presentation time stamp changes.</summary>
			WhenMaxPresentationTimeStampChanges = 6,
			/// <summary>The trigger is raised when the data is ready.</summary>
			WhenDataBecomesReady = 7,
			/// <summary>The trigger is raised when the end of the data is reached.</summary>
			WhenEndOfDataReached = 8,
			/// <summary>The trigger is raised when the buffer queue is reset.</summary>
			WhenReset = 9,
			/// <summary>The trigger is raised when the buffer count is less than the specified value.</summary>
			WhenBufferCountBecomesLessThan = 10,
			/// <summary>The trigger is raised when the buffer count is greater than the specified value.</summary>
			WhenBufferCountBecomesGreaterThan = 11,
			/// <summary>The trigger is raised when the elapsed time is greater than or equal to its specified value and the elapsed time is greater than its specified value.</summary>
			WhenDurationBecomesGreaterThanOrEqualToAndBufferCountBecomesGreaterThan = 12,
		}
	}
}
