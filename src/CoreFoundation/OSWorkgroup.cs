// Copyright (c) Microsoft Corporation.
// Licensed under the MIT License.
//
// OSWorkgroup.cs: Bindings to the os_workgroup API (<os/workgroup.h>)

#nullable enable

using System.Diagnostics.CodeAnalysis;
using System.Runtime.InteropServices;

using ObjCRuntime;

namespace CoreFoundation {

	/// <summary>The clock used for the timestamps of an <see cref="OSWorkgroupInterval" />.</summary>
	[SupportedOSPlatform ("ios")]
	[SupportedOSPlatform ("maccatalyst")]
	[SupportedOSPlatform ("macos")]
	[SupportedOSPlatform ("tvos")]
	public enum OSClockId : uint {
		/// <summary>The Mach absolute time clock.</summary>
		MachAbsoluteTime = 32,
	}

	/// <summary>A token returned when joining an <see cref="OSWorkgroup" />. Dispose it to leave the workgroup.</summary>
	/// <remarks>
	/// The token must be disposed on the same thread that joined the workgroup.
	/// Leaving a workgroup can't be done from the finalizer thread, so if the token is collected by the GC without being disposed,
	/// a warning is logged and the thread stays in the workgroup.
	/// </remarks>
	[SupportedOSPlatform ("ios")]
	[SupportedOSPlatform ("maccatalyst")]
	[SupportedOSPlatform ("macos")]
	[SupportedOSPlatform ("tvos")]
	public sealed class OSWorkgroupJoinToken : IDisposable {
#if !COREBUILD
		// sizeof (os_workgroup_join_token_s) is 40 bytes; allocate extra room.
		const int TokenSize = 64;

		IntPtr token;
		// An independently retained reference to the workgroup, so that disposing the OSWorkgroup instance doesn't affect leaving.
		IntPtr workgroup;

		[DllImport (Constants.libSystemLibrary)]
		static extern void os_workgroup_leave (IntPtr wg, IntPtr token);

		internal OSWorkgroupJoinToken (IntPtr workgroup, IntPtr token)
		{
			this.workgroup = workgroup;
			this.token = token;
		}

		~OSWorkgroupJoinToken ()
		{
			if (token == IntPtr.Zero)
				return;
			Runtime.NSLog ("An OSWorkgroupJoinToken was collected by the GC without being disposed; the thread has not left the workgroup.");
			Marshal.FreeHGlobal (token);
			OSWorkgroup.os_release (workgroup);
		}

		internal static IntPtr Allocate () => Marshal.AllocHGlobal (TokenSize);

		/// <summary>Leave the workgroup.</summary>
		public void Dispose ()
		{
			if (token == IntPtr.Zero)
				return;
			os_workgroup_leave (workgroup, token);
			Marshal.FreeHGlobal (token);
			OSWorkgroup.os_release (workgroup);
			token = IntPtr.Zero;
			workgroup = IntPtr.Zero;
			GC.SuppressFinalize (this);
		}
#else
		/// <summary>Leave the workgroup.</summary>
		public void Dispose () { }
#endif // !COREBUILD
	}

	/// <summary>A group of threads that work towards a common goal.</summary>
	[SupportedOSPlatform ("ios")]
	[SupportedOSPlatform ("maccatalyst")]
	[SupportedOSPlatform ("macos")]
	[SupportedOSPlatform ("tvos")]
	public class OSWorkgroup : NativeObject {
#if !COREBUILD
		[DllImport (Constants.libSystemLibrary)]
		internal static extern IntPtr os_retain (IntPtr obj);

		[DllImport (Constants.libSystemLibrary)]
		internal static extern void os_release (IntPtr obj);

		[DllImport (Constants.libSystemLibrary)]
		static extern IntPtr os_workgroup_create_with_workgroup (IntPtr name, IntPtr wg);

		[DllImport (Constants.libSystemLibrary)]
		static extern int os_workgroup_join (IntPtr wg, IntPtr token_out);

		[DllImport (Constants.libSystemLibrary)]
		static extern void os_workgroup_cancel (IntPtr wg);

		[DllImport (Constants.libSystemLibrary)]
		[return: MarshalAs (UnmanagedType.I1)]
		static extern bool os_workgroup_testcancel (IntPtr wg);

		[DllImport (Constants.libSystemLibrary)]
		static extern int os_workgroup_max_parallel_threads (IntPtr wg, IntPtr attr);

		[DllImport (Constants.libSystemLibrary)]
		static extern int os_workgroup_copy_port (IntPtr wg, out uint mach_port);

		[DllImport (Constants.libSystemLibrary)]
		static extern IntPtr os_workgroup_create_with_port (IntPtr name, uint mach_port);

		[Preserve (Conditional = true)]
		internal OSWorkgroup (NativeHandle handle, bool owns)
			: base (handle, owns, verify: true)
		{
		}

		/// <inheritdoc />
		protected internal override void Retain ()
		{
			os_retain (GetCheckedHandle ());
		}

		/// <inheritdoc />
		protected internal override void Release ()
		{
			os_release (GetCheckedHandle ());
		}

		/// <summary>Create a new workgroup from an existing workgroup.</summary>
		/// <param name="name">An optional name for the new workgroup.</param>
		/// <param name="workgroup">The workgroup to create the new workgroup from.</param>
		/// <returns>The new workgroup, or <see langword="null" /> if it couldn't be created.</returns>
		public static OSWorkgroup? Create (string? name, OSWorkgroup workgroup)
		{
			ArgumentNullException.ThrowIfNull (workgroup);
			using var namePtr = new TransientString (name, TransientString.Encoding.Ansi);
			var handle = os_workgroup_create_with_workgroup (namePtr, workgroup.GetCheckedHandle ());
			GC.KeepAlive (workgroup);
			return handle == IntPtr.Zero ? null : new OSWorkgroup (handle, true);
		}

		/// <summary>Join the current thread to this workgroup.</summary>
		/// <returns>A token that must be disposed (on the same thread) to leave the workgroup.</returns>
		/// <exception cref="InvalidOperationException">If the thread is already in a workgroup, or if the workgroup has been cancelled.</exception>
		public OSWorkgroupJoinToken Join ()
		{
			if (!TryJoin (out var token, out var error))
				throw new InvalidOperationException ($"Unable to join the workgroup (error {error}).");
			return token;
		}

		/// <summary>Try to join the current thread to this workgroup.</summary>
		/// <param name="token">A token that must be disposed (on the same thread) to leave the workgroup. Only valid if this method returns true.</param>
		/// <returns>True if the thread joined the workgroup, false otherwise (for instance if the thread is already in a workgroup, or if the workgroup has been cancelled).</returns>
		public bool TryJoin ([NotNullWhen (true)] out OSWorkgroupJoinToken? token)
		{
			return TryJoin (out token, out _);
		}

		/// <summary>Try to join the current thread to this workgroup.</summary>
		/// <param name="token">A token that must be disposed (on the same thread) to leave the workgroup. Only valid if this method returns true.</param>
		/// <param name="error">The error code returned by the native function (0 on success).</param>
		/// <returns>True if the thread joined the workgroup, false otherwise (for instance if the thread is already in a workgroup, or if the workgroup has been cancelled).</returns>
		public bool TryJoin ([NotNullWhen (true)] out OSWorkgroupJoinToken? token, out int error)
		{
			var handle = GetCheckedHandle ();
			var tokenPtr = OSWorkgroupJoinToken.Allocate ();
			error = os_workgroup_join (handle, tokenPtr);
			if (error != 0) {
				Marshal.FreeHGlobal (tokenPtr);
				GC.KeepAlive (this);
				token = null;
				return false;
			}
			os_retain (handle);
			GC.KeepAlive (this);
			token = new OSWorkgroupJoinToken (handle, tokenPtr);
			return true;
		}

		/// <summary>Cancel the workgroup. Threads can no longer join a cancelled workgroup.</summary>
		public void Cancel ()
		{
			os_workgroup_cancel (GetCheckedHandle ());
			GC.KeepAlive (this);
		}

		/// <summary>Whether the workgroup has been cancelled.</summary>
		public bool IsCancelled {
			get {
				var rv = os_workgroup_testcancel (GetCheckedHandle ());
				GC.KeepAlive (this);
				return rv;
			}
		}

		/// <summary>The maximum number of threads that can work in parallel on this workgroup.</summary>
		public int MaxParallelThreads {
			get {
				var rv = os_workgroup_max_parallel_threads (GetCheckedHandle (), IntPtr.Zero);
				GC.KeepAlive (this);
				return rv;
			}
		}

		/// <summary>Create a workgroup from a Mach port.</summary>
		/// <param name="name">An optional name for the workgroup.</param>
		/// <param name="machPort">The Mach port to create the workgroup from.</param>
		/// <returns>The new workgroup, or <see langword="null" /> if it couldn't be created.</returns>
		[SupportedOSPlatform ("macos")]
		[UnsupportedOSPlatform ("ios")]
		[UnsupportedOSPlatform ("maccatalyst")]
		[UnsupportedOSPlatform ("tvos")]
		public static OSWorkgroup? Create (string? name, uint machPort)
		{
			using var namePtr = new TransientString (name, TransientString.Encoding.Ansi);
			var handle = os_workgroup_create_with_port (namePtr, machPort);
			return handle == IntPtr.Zero ? null : new OSWorkgroup (handle, true);
		}

		/// <summary>Get a Mach port that represents this workgroup.</summary>
		/// <param name="machPort">The Mach port. The caller owns a send right to it.</param>
		/// <returns>True if successful, false otherwise.</returns>
		[SupportedOSPlatform ("macos")]
		[UnsupportedOSPlatform ("ios")]
		[UnsupportedOSPlatform ("maccatalyst")]
		[UnsupportedOSPlatform ("tvos")]
		public bool TryCopyPort (out uint machPort)
		{
			return TryCopyPort (out machPort, out _);
		}

		/// <summary>Get a Mach port that represents this workgroup.</summary>
		/// <param name="machPort">The Mach port. The caller owns a send right to it.</param>
		/// <param name="error">The error code returned by the native function (0 on success).</param>
		/// <returns>True if successful, false otherwise.</returns>
		[SupportedOSPlatform ("macos")]
		[UnsupportedOSPlatform ("ios")]
		[UnsupportedOSPlatform ("maccatalyst")]
		[UnsupportedOSPlatform ("tvos")]
		public bool TryCopyPort (out uint machPort, out int error)
		{
			error = os_workgroup_copy_port (GetCheckedHandle (), out machPort);
			GC.KeepAlive (this);
			return error == 0;
		}
#endif // !COREBUILD
	}

	/// <summary>A workgroup whose threads work towards a repeating deadline.</summary>
	[SupportedOSPlatform ("ios")]
	[SupportedOSPlatform ("maccatalyst")]
	[SupportedOSPlatform ("macos")]
	[SupportedOSPlatform ("tvos")]
	public class OSWorkgroupInterval : OSWorkgroup {
#if !COREBUILD
		[DllImport (Constants.libSystemLibrary)]
		static extern int os_workgroup_interval_start (IntPtr wg, ulong start, ulong deadline, IntPtr data);

		[DllImport (Constants.libSystemLibrary)]
		static extern int os_workgroup_interval_update (IntPtr wg, ulong deadline, IntPtr data);

		[DllImport (Constants.libSystemLibrary)]
		static extern int os_workgroup_interval_finish (IntPtr wg, IntPtr data);

		[Preserve (Conditional = true)]
		internal OSWorkgroupInterval (NativeHandle handle, bool owns)
			: base (handle, owns)
		{
		}

		static void ThrowIfError (int rv, string operation)
		{
			if (rv != 0)
				throw new InvalidOperationException ($"Unable to {operation} the workgroup interval (error {rv}).");
		}

		/// <summary>Start the interval.</summary>
		/// <param name="start">The start timestamp, in the clock the interval was created with.</param>
		/// <param name="deadline">The deadline timestamp, in the clock the interval was created with.</param>
		public void Start (ulong start, ulong deadline)
		{
			TryStart (start, deadline, out var error);
			ThrowIfError (error, "start");
		}

		/// <summary>Try to start the interval.</summary>
		/// <param name="start">The start timestamp, in the clock the interval was created with.</param>
		/// <param name="deadline">The deadline timestamp, in the clock the interval was created with.</param>
		/// <returns>True if successful, false otherwise.</returns>
		public bool TryStart (ulong start, ulong deadline)
		{
			return TryStart (start, deadline, out _);
		}

		/// <summary>Try to start the interval.</summary>
		/// <param name="start">The start timestamp, in the clock the interval was created with.</param>
		/// <param name="deadline">The deadline timestamp, in the clock the interval was created with.</param>
		/// <param name="error">The error code returned by the native function (0 on success).</param>
		/// <returns>True if successful, false otherwise.</returns>
		public bool TryStart (ulong start, ulong deadline, out int error)
		{
			error = os_workgroup_interval_start (GetCheckedHandle (), start, deadline, IntPtr.Zero);
			GC.KeepAlive (this);
			return error == 0;
		}

		/// <summary>Update the deadline of a started interval.</summary>
		/// <param name="deadline">The new deadline timestamp, in the clock the interval was created with.</param>
		public void Update (ulong deadline)
		{
			TryUpdate (deadline, out var error);
			ThrowIfError (error, "update");
		}

		/// <summary>Try to update the deadline of a started interval.</summary>
		/// <param name="deadline">The new deadline timestamp, in the clock the interval was created with.</param>
		/// <returns>True if successful, false otherwise.</returns>
		public bool TryUpdate (ulong deadline)
		{
			return TryUpdate (deadline, out _);
		}

		/// <summary>Try to update the deadline of a started interval.</summary>
		/// <param name="deadline">The new deadline timestamp, in the clock the interval was created with.</param>
		/// <param name="error">The error code returned by the native function (0 on success).</param>
		/// <returns>True if successful, false otherwise.</returns>
		public bool TryUpdate (ulong deadline, out int error)
		{
			error = os_workgroup_interval_update (GetCheckedHandle (), deadline, IntPtr.Zero);
			GC.KeepAlive (this);
			return error == 0;
		}

		/// <summary>Finish the current interval.</summary>
		public void Finish ()
		{
			TryFinish (out var error);
			ThrowIfError (error, "finish");
		}

		/// <summary>Try to finish the current interval.</summary>
		/// <returns>True if successful, false otherwise.</returns>
		public bool TryFinish ()
		{
			return TryFinish (out _);
		}

		/// <summary>Try to finish the current interval.</summary>
		/// <param name="error">The error code returned by the native function (0 on success).</param>
		/// <returns>True if successful, false otherwise.</returns>
		public bool TryFinish (out int error)
		{
			error = os_workgroup_interval_finish (GetCheckedHandle (), IntPtr.Zero);
			GC.KeepAlive (this);
			return error == 0;
		}
#endif // !COREBUILD
	}

	/// <summary>A workgroup whose threads are allowed to work in parallel.</summary>
	[SupportedOSPlatform ("ios")]
	[SupportedOSPlatform ("maccatalyst")]
	[SupportedOSPlatform ("macos")]
	[SupportedOSPlatform ("tvos")]
	public class OSWorkgroupParallel : OSWorkgroup {
#if !COREBUILD
		[DllImport (Constants.libSystemLibrary)]
		static extern IntPtr os_workgroup_parallel_create (IntPtr name, IntPtr attr);

		[Preserve (Conditional = true)]
		internal OSWorkgroupParallel (NativeHandle handle, bool owns)
			: base (handle, owns)
		{
		}

		/// <summary>Create a new parallel workgroup.</summary>
		/// <param name="name">An optional name for the workgroup.</param>
		/// <returns>The new workgroup, or <see langword="null" /> if it couldn't be created.</returns>
		public static OSWorkgroupParallel? Create (string? name)
		{
			using var namePtr = new TransientString (name, TransientString.Encoding.Ansi);
			var handle = os_workgroup_parallel_create (namePtr, IntPtr.Zero);
			return handle == IntPtr.Zero ? null : new OSWorkgroupParallel (handle, true);
		}
#endif // !COREBUILD
	}
}
