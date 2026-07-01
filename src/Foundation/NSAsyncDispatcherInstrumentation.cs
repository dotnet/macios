// Copyright (c) Microsoft Corporation.
// Licensed under the MIT License.
//
// ⚠️ THROWAWAY INSTRUMENTATION — DO NOT MERGE ⚠️
//
// This file adds instrumentation to help diagnose the GC/marshalling race in
// https://github.com/dotnet/macios/issues/25861.
//
// It records, for every __MonoMac_NSAsyncActionDispatcher (and other async
// dispatchers), the stack trace at creation time and a log of lifecycle events
// (Apply/Dispose/Finalize). This information is stored directly on the native
// Objective-C object using an associated reference, so it survives even after
// the managed instance has been garbage collected. When the marshalling
// exception from the issue is raised, this information is appended to the error
// message.

using System;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading;

using CoreFoundation;
using ObjCRuntime;

#nullable enable

namespace Foundation {

#if !COREBUILD
	static class NSAsyncDispatcherInstrumentation {
		// A unique, stable pointer used as the associated-object key.
		static readonly IntPtr AssociationKey = Marshal.AllocHGlobal (1);

		// OBJC_ASSOCIATION_RETAIN (01401) - retain the value (atomically) so the
		// stored string stays alive as long as the native object does.
		const nint OBJC_ASSOCIATION_RETAIN = 01401;

		// Serializes read-modify-write access to the associated object.
		static readonly object lockObj = new object ();

		static int counter;

		[DllImport (Messaging.LIBOBJC_DYLIB)]
		static extern void objc_setAssociatedObject (IntPtr obj, IntPtr key, IntPtr value, nint policy);

		[DllImport (Messaging.LIBOBJC_DYLIB)]
		static extern IntPtr objc_getAssociatedObject (IntPtr obj, IntPtr key);

		// Records the creation of an async dispatcher and returns a unique id that
		// can be used to correlate later lifecycle events.
		public static int RecordCreation (NativeHandle handle)
		{
			var id = Interlocked.Increment (ref counter);
			if (handle == NativeHandle.Zero)
				return id;

			try {
				var sb = new StringBuilder ();
				sb.Append ("=== __MonoMac_NSAsyncActionDispatcher instrumentation (issue #25861) ===\n");
				sb.Append ($"Instance #{id} (native 0x{((IntPtr) handle).ToString ("x")}) created on managed thread {Environment.CurrentManagedThreadId} at {DateTime.UtcNow:HH:mm:ss.fffffff} UTC\n");
				sb.Append ("Creation stack trace:\n");
				sb.Append (Environment.StackTrace);
				sb.Append ("\nLifecycle events:\n");
				SetInfo ((IntPtr) handle, sb.ToString ());
			} catch {
				// Instrumentation must never throw.
			}
			return id;
		}

		// Appends a lifecycle event (e.g. "Apply", "Dispose(true)", "Finalize") to
		// the info stored on the native object.
		public static void RecordEvent (NativeHandle handle, int id, string @event)
		{
			if (handle == NativeHandle.Zero)
				return;

			try {
				lock (lockObj) {
					var existing = GetInfo ((IntPtr) handle) ?? string.Empty;
					var updated = existing + $"    [#{id}] {@event} on managed thread {Environment.CurrentManagedThreadId} at {DateTime.UtcNow:HH:mm:ss.fffffff} UTC\n";
					SetInfo ((IntPtr) handle, updated);
				}
			} catch {
				// Instrumentation must never throw.
			}
		}

		// Reads the instrumentation info stored on a native object, if any.
		public static string? GetInfo (IntPtr handle)
		{
			if (handle == IntPtr.Zero)
				return null;
			var str = objc_getAssociatedObject (handle, AssociationKey);
			if (str == IntPtr.Zero)
				return null;
			return CFString.FromHandle (str);
		}

		static void SetInfo (IntPtr handle, string value)
		{
			var str = CFString.CreateNative (value); // +1
			objc_setAssociatedObject (handle, AssociationKey, (IntPtr) str, OBJC_ASSOCIATION_RETAIN); // +1 (=2)
			if (str != NativeHandle.Zero)
				CFObject.CFRelease ((IntPtr) str); // -1 (=1)
		}
	}
#endif // !COREBUILD
}
