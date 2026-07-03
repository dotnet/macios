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
// (Apply/Dispose/Finalize). The information is stored in a static managed
// dictionary keyed by the native Objective-C handle, so it survives even after
// the managed instance has been garbage collected. When the marshalling
// exception from the issue is raised, this information is looked up (by native
// handle) and appended to the error message.

using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;

using ObjCRuntime;

#nullable enable

namespace Foundation {

#if !COREBUILD
	static class NSAsyncDispatcherInstrumentation {
		// Maps a native handle -> diagnostic info. Keyed by IntPtr (not by the
		// managed object), so it does NOT keep the managed instance alive and
		// therefore does not perturb the GC race we're trying to reproduce.
		static readonly Dictionary<IntPtr, string> infos = new Dictionary<IntPtr, string> ();
		static readonly object lockObj = new object ();
		static int counter;

		// Safety cap so a long test run doesn't accumulate unbounded memory.
		const int MaxEntries = 20000;

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

				lock (lockObj) {
					if (infos.Count >= MaxEntries)
						infos.Clear ();
					infos [(IntPtr) handle] = sb.ToString ();
				}
			} catch {
				// Instrumentation must never throw.
			}
			return id;
		}

		// Appends a lifecycle event (e.g. "Apply", "Dispose(true)", "Finalize") to
		// the info stored for the native handle.
		public static void RecordEvent (NativeHandle handle, int id, string @event)
		{
			if (handle == NativeHandle.Zero)
				return;

			try {
				var line = $"    [#{id}] {@event} on managed thread {Environment.CurrentManagedThreadId} at {DateTime.UtcNow:HH:mm:ss.fffffff} UTC\n";
				lock (lockObj) {
					infos.TryGetValue ((IntPtr) handle, out var existing);
					infos [(IntPtr) handle] = (existing ?? string.Empty) + line;
				}
			} catch {
				// Instrumentation must never throw.
			}
		}

		// Reads the instrumentation info stored for a native handle, if any.
		public static string? GetInfo (IntPtr handle)
		{
			if (handle == IntPtr.Zero)
				return null;
			try {
				lock (lockObj) {
					infos.TryGetValue (handle, out var info);
					return info;
				}
			} catch {
				return null;
			}
		}
	}
#endif // !COREBUILD
}
