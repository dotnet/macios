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
// (Apply/Dispose/Finalize/object-map operations).
//
// Two things are recorded:
//  1. A per-native-handle string (kept in a static managed dictionary keyed by
//     the native Objective-C handle) that survives after the managed instance
//     has been garbage collected. When the marshalling exception from the issue
//     is raised, this info is looked up (by native handle) and appended to the
//     error message.
//  2. An immediate chronological line printed to stdout with a distinctive
//     "#25861#" prefix + a monotonic sequence number + thread + native handle.
//     This lets us reconstruct the exact ordering of events across ALL handles
//     (including native-handle reuse across dispatcher instances), which the
//     per-handle dictionary alone cannot show.

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
		// The set of native handles we consider "interesting" (i.e. that belong
		// to an async dispatcher). Used by the Runtime object-map instrumentation
		// to decide whether to log an object-map operation.
		static readonly HashSet<IntPtr> tracked = new HashSet<IntPtr> ();
		static readonly object lockObj = new object ();
		static int counter;
		static long seq;

		// Safety cap so a long test run doesn't accumulate unbounded memory.
		const int MaxEntries = 100000;

		// Per-event stdout logging is very high volume: the stress test alone
		// (AsyncDispatcherStressTest) creates ~37k dispatchers in 45s, i.e. ~160k
		// stdout lines, which risks exceeding CI log limits and scrolling the
		// crucial failure (8027) dump off the top. So by default we only print
		// rare/important signals (handle reuse, zeroed handles, the failure dump).
		// The full per-handle history still lives in `infos` and is surfaced in the
		// error 8027 message regardless of this flag.
		static readonly bool VerboseStdout = false;

		static string Now () => DateTime.UtcNow.ToString ("HH:mm:ss.fffffff");

		// Prints a single chronological line to stdout. Cheap and greppable.
		static void Emit (string message)
		{
			try {
				var s = Interlocked.Increment (ref seq);
				Console.WriteLine ($"#25861# [{s}] [t{Environment.CurrentManagedThreadId}] [{Now ()}] {message}");
			} catch {
				// Instrumentation must never throw.
			}
		}

		// Returns true if the native handle belongs to a dispatcher we're tracking.
		public static bool IsTracked (IntPtr handle)
		{
			if (handle == IntPtr.Zero)
				return false;
			lock (lockObj) {
				return tracked.Contains (handle);
			}
		}

		// Records the creation of an async dispatcher and returns a unique id that
		// can be used to correlate later lifecycle events.
		public static int RecordCreation (NativeHandle handle)
		{
			var id = Interlocked.Increment (ref counter);
			var ptr = (IntPtr) handle;
			if (ptr == IntPtr.Zero)
				return id;

			try {
				var sb = new StringBuilder ();
				sb.Append ("=== __MonoMac_NSAsyncActionDispatcher instrumentation (issue #25861) ===\n");
				sb.Append ($"Instance #{id} (native 0x{ptr.ToString ("x")}) created on managed thread {Environment.CurrentManagedThreadId} at {Now ()} UTC\n");
				sb.Append ("Creation stack trace:\n");
				sb.Append (Environment.StackTrace);
				sb.Append ("\nLifecycle events:\n");

				lock (lockObj) {
					if (infos.Count >= MaxEntries) {
						infos.Clear ();
						tracked.Clear ();
					}
					// If the native handle is being reused, dump the previous
					// instance's history before overwriting it — handle reuse is a
					// prime suspect for issue #25861.
					if (infos.TryGetValue (ptr, out var previous)) {
						Emit ($"REUSE    native handle 0x{ptr.ToString ("x")} is being reused by instance #{id}. Previous history:\n{previous}");
					}
					infos [ptr] = sb.ToString ();
					tracked.Add (ptr);
				}
			} catch {
				// Instrumentation must never throw.
			}

			if (VerboseStdout)
				Emit ($"CREATE   instance #{id} handle 0x{ptr.ToString ("x")}");
			return id;
		}

		// Appends a lifecycle event (e.g. "Apply", "Dispose(true)", "Finalize") to
		// the info stored for the native handle. The caller must pass the native
		// handle captured at *creation* time, because the live Handle property may
		// already have been zeroed by the time these events run.
		public static void RecordEvent (IntPtr handle, int id, string @event)
		{
			if (handle == IntPtr.Zero) {
				Emit ($"EVENT    instance #{id} handle 0x0 (zeroed!) : {@event}");
				return;
			}

			try {
				var line = $"    [#{id}] {@event} on managed thread {Environment.CurrentManagedThreadId} at {Now ()} UTC\n";
				lock (lockObj) {
					infos.TryGetValue (handle, out var existing);
					infos [handle] = (existing ?? string.Empty) + line;
				}
			} catch {
				// Instrumentation must never throw.
			}

			if (VerboseStdout)
				Emit ($"EVENT    instance #{id} handle 0x{handle.ToString ("x")} : {@event}");
		}

		// Logs an object-map operation performed by the runtime for a tracked
		// dispatcher handle (Register/Unregister/NativeObjectHasDied/etc).
		public static void LogObjectMapOp (string op, IntPtr handle, string? extra = null)
		{
			if (handle == IntPtr.Zero)
				return;

			bool isTracked;
			lock (lockObj) {
				isTracked = tracked.Contains (handle);
			}
			if (!isTracked)
				return;

			try {
				var line = $"    [objmap] {op}{(extra is null ? "" : " " + extra)} on managed thread {Environment.CurrentManagedThreadId} at {Now ()} UTC\n";
				lock (lockObj) {
					infos.TryGetValue (handle, out var existing);
					infos [handle] = (existing ?? string.Empty) + line;
				}
			} catch {
				// Instrumentation must never throw.
			}

			if (VerboseStdout)
				Emit ($"OBJMAP   {op} handle 0x{handle.ToString ("x")}{(extra is null ? "" : " " + extra)}");
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
