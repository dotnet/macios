//
// DispatchSource.cs: DispatchSource objects
//
// Authors:
//   Miguel de Icaza (miguel@gnome.org)
//   Marek Safar (marek.safar@gmail.com)
//
// Copyright 2015 Xamarin Inc
//

#nullable enable

using System;
using System.Runtime.InteropServices;
using System.Threading;
using System.IO;
using System.Runtime.Versioning;
using ObjCRuntime;
using Foundation;
using dispatch_source_type_t = System.IntPtr;
using dispatch_source_t = System.IntPtr;
using dispatch_queue_t = System.IntPtr;

namespace CoreFoundation {

	[Flags]
	public enum MemoryPressureFlags {
		/// <summary>The system memory pressure condition has returned to normal.</summary>
		Normal = 1,
		/// <summary>The system memory pressure condition has changed to warning.</summary>
		Warn = 2,
		/// <summary>The system memory pressure condition has changed to critical.</summary>
		Critical = 4,
	}

	[Flags]
	public enum ProcessMonitorFlags : uint {
		/// <summary>To be added.</summary>
		Exit = 0x80000000,
		/// <summary>To be added.</summary>
		Fork = 0x40000000,
		/// <summary>To be added.</summary>
		Exec = 0x20000000,
		/// <summary>To be added.</summary>
		Signal = 0x08000000,
	}

	[Flags]
	public enum VnodeMonitorKind : uint {
		/// <summary>The file was removed from the file system due to the unlink(2) system call.</summary>
		Delete = 1,
		/// <summary>A write to the referenced file occurred</summary>
		Write = 2,
		/// <summary>The file was extended.</summary>
		Extend = 4,
		/// <summary>The attributes on the file have changed</summary>
		Attrib = 8,
		/// <summary>The link count on the file has changed.</summary>
		Link = 0x10,
		/// <summary>The referenced node was renamed</summary>
		Rename = 0x20,
		/// <summary>Access to the referenced node was revoked via revoke(2) or the underlying fileystem was unmounted.</summary>
		Revoke = 0x40,
	}

	[SupportedOSPlatform ("ios")]
	[SupportedOSPlatform ("maccatalyst")]
	[SupportedOSPlatform ("macos")]
	[SupportedOSPlatform ("tvos")]
	public class DispatchSource : DispatchObject {
		DispatchQueue? queue;

		// constructors for use in bindings
		[Preserve (Conditional = true)]
		internal DispatchSource (NativeHandle handle, bool owns) : base (handle, owns)
		{
		}

		// Invoked by subclasses in this file that fully initialize both
		// queue and handle
		internal DispatchSource () { }


		[DllImport (Constants.libcLibrary)]
		extern static dispatch_source_t dispatch_source_create (dispatch_source_type_t type, IntPtr handle, IntPtr mask, dispatch_queue_t queue);

		[DllImport (Constants.libcLibrary)]
		extern static IntPtr dispatch_source_get_handle (dispatch_source_t source);

		[DllImport (Constants.libcLibrary)]
		extern static IntPtr dispatch_source_get_mask (dispatch_source_t source);

		[DllImport (Constants.libcLibrary)]
		extern static IntPtr dispatch_source_get_data (dispatch_source_t source);

		[DllImport (Constants.libcLibrary)]
		extern static IntPtr dispatch_source_merge_data (dispatch_source_t source, IntPtr data);

		[DllImport (Constants.libcLibrary)]
		unsafe extern static IntPtr dispatch_source_set_event_handler (dispatch_source_t source, BlockLiteral* handler);

		[DllImport (Constants.libcLibrary)]
		unsafe extern static IntPtr dispatch_source_set_registration_handler (dispatch_source_t source, BlockLiteral* handler);

		[DllImport (Constants.libcLibrary)]
		unsafe extern static IntPtr dispatch_source_set_cancel_handler (dispatch_source_t source, BlockLiteral* handler);

		[DllImport (Constants.libcLibrary)]
		unsafe extern static IntPtr dispatch_source_set_event_handler_f (dispatch_source_t source, IntPtr handler);

		[DllImport (Constants.libcLibrary)]
		extern static IntPtr dispatch_source_cancel (dispatch_source_t source);

		[DllImport (Constants.libcLibrary)]
		extern static IntPtr dispatch_source_testcancel (dispatch_source_t source);

		public void SetEventHandler (Action handler)
		{
			if (handler is null) {
				dispatch_source_set_event_handler_f (GetCheckedHandle (), IntPtr.Zero);
				return;
			}

			Action callback =
				delegate
				{
					var sc = SynchronizationContext.Current;
					if (sc is null) {
						if (queue is null)
							ObjCRuntime.ThrowHelper.ThrowArgumentNullException (nameof (queue));
						SynchronizationContext.SetSynchronizationContext (new DispatchQueueSynchronizationContext (queue));
					}
					try {
						handler ();
					} finally {
						if (sc is null)
							SynchronizationContext.SetSynchronizationContext (null);
					}
				};
			unsafe {
				using var block = BlockStaticDispatchClass.CreateBlock (callback);
				dispatch_source_set_event_handler (GetCheckedHandle (), &block);
			}
		}

		public void Suspend ()
		{
			dispatch_suspend (GetCheckedHandle ());
		}

		public void Resume ()
		{
			dispatch_resume (GetCheckedHandle ());
		}

		public void SetRegistrationHandler (Action handler)
		{
			if (handler is null)
				ObjCRuntime.ThrowHelper.ThrowArgumentNullException (nameof (handler));

			Action callback =
				delegate
				{
					var sc = SynchronizationContext.Current;
					if (sc is null) {
						if (queue is null)
							ObjCRuntime.ThrowHelper.ThrowArgumentNullException (nameof (queue));
						SynchronizationContext.SetSynchronizationContext (new DispatchQueueSynchronizationContext (queue));
					}
					try {
						handler ();
					} finally {
						if (sc is null)
							SynchronizationContext.SetSynchronizationContext (null);
					}
				};
			unsafe {
				using var block = BlockStaticDispatchClass.CreateBlock (callback);
				dispatch_source_set_registration_handler (GetCheckedHandle (), &block);
			}
		}

		public void SetCancelHandler (Action handler)
		{
			if (handler is null)
				ObjCRuntime.ThrowHelper.ThrowArgumentNullException (nameof (handler));

			Action callback =
				delegate
				{
					var sc = SynchronizationContext.Current;
					if (sc is null) {
						if (queue is null)
							ObjCRuntime.ThrowHelper.ThrowArgumentNullException (nameof (queue));
						SynchronizationContext.SetSynchronizationContext (new DispatchQueueSynchronizationContext (queue));
					}
					try {
						handler ();
					} finally {
						if (sc is null)
							SynchronizationContext.SetSynchronizationContext (null);
					}
				};
			unsafe {
				using var block = BlockStaticDispatchClass.CreateBlock (callback);
				dispatch_source_set_cancel_handler (GetCheckedHandle (), &block);
			}
		}

		public void Cancel ()
		{
			dispatch_source_cancel (GetCheckedHandle ());
		}

		protected override void Dispose (bool disposing)
		{
			// Do not call the Cancel method here
			// This is important because the Dispathc Source might be electible for garbage collection
			// but might still have a side effect (for example, invoking a handler).
			base.Dispose (disposing);
		}

		/// <summary>Determine whether the specified source has been canceled.</summary>
		///         <value>True if the source has been canceled.</value>
		///         <remarks>
		///           <para>
		///           </para>
		///         </remarks>
		public bool IsCanceled {
			get {
				return dispatch_source_testcancel (GetCheckedHandle ()) != IntPtr.Zero;
			}
		}

		[SupportedOSPlatform ("ios")]
		[SupportedOSPlatform ("maccatalyst")]
		[SupportedOSPlatform ("macos")]
		[SupportedOSPlatform ("tvos")]
		public class Data : DispatchSource {
			internal Data () { }
			internal Data (IntPtr handle, bool owns) : base (handle, owns) { }

			public void MergeData (IntPtr value)
			{
				dispatch_source_merge_data (Handle, value);
			}

			/// <summary>Retrieves the data that has been posted for this event source.</summary>
			///         <value>The result of the data posted to the data dispatch source.</value>
			///         <remarks>
			///           <para>If multiple calls to MergeData are done, the result surfaced by PendingData will depend on whether you created a <see cref="T:CoreFoundation.DispatchSource.DataAdd" /> which will add the values together or a <see cref="T:CoreFoundation.DispatchSource.DataOr" /> which will or the values together.</para>
			///           <para>
			///           </para>
			///         </remarks>
			public IntPtr PendingData {
				get {
					return dispatch_source_get_data (Handle);
				}
			}
		}

		[SupportedOSPlatform ("ios")]
		[SupportedOSPlatform ("maccatalyst")]
		[SupportedOSPlatform ("macos")]
		[SupportedOSPlatform ("tvos")]
		public class DataAdd : Data {
			static IntPtr type_data_add;

			public DataAdd (IntPtr handle, bool owns) : base (handle, owns) { }
			public DataAdd (IntPtr handle) : base (handle, false) { }

			public DataAdd (DispatchQueue? queue = null)
			{
				if (type_data_add == IntPtr.Zero)
					type_data_add = Dlfcn.dlsym (Libraries.System.Handle, "_dispatch_source_type_data_add");

				this.queue = queue;
				var handle = dispatch_source_create (type_data_add,
								 handle: IntPtr.Zero,
								 mask: IntPtr.Zero,
								 queue: queue.GetHandle ());
				GC.KeepAlive (queue);
				if (handle != IntPtr.Zero)
					InitializeHandle (handle);
			}
		}

		[SupportedOSPlatform ("ios")]
		[SupportedOSPlatform ("maccatalyst")]
		[SupportedOSPlatform ("macos")]
		[SupportedOSPlatform ("tvos")]
		public class DataOr : Data {
			static IntPtr type_data_or;

			public DataOr (IntPtr handle, bool owns) : base (handle, owns) { }
			public DataOr (IntPtr handle) : base (handle, false) { }

			public DataOr (DispatchQueue? queue = null)
			{
				if (type_data_or == IntPtr.Zero)
					type_data_or = Dlfcn.dlsym (Libraries.System.Handle, "_dispatch_source_type_data_or");
				this.queue = queue;
				var handle = dispatch_source_create (type_data_or,
								 handle: IntPtr.Zero,
								 mask: IntPtr.Zero,
								 queue: queue.GetHandle ());
				GC.KeepAlive (queue);
				if (handle != IntPtr.Zero)
					InitializeHandle (handle);
			}
		}

		[SupportedOSPlatform ("ios")]
		[SupportedOSPlatform ("maccatalyst")]
		[SupportedOSPlatform ("macos")]
		[SupportedOSPlatform ("tvos")]
		public class Mach : DispatchSource {
			internal Mach (IntPtr handle, bool owns) : base (handle, owns) { }
			internal Mach (IntPtr handle) : base (handle, false) { }
			internal Mach ()
			{ }

			/// <summary>The MachPort that this DispatchSource is monitoring.</summary>
			///         <value>To be added.</value>
			///         <remarks>To be added.</remarks>
			public int MachPort {
				get {
					return (int) dispatch_source_get_handle (GetCheckedHandle ());
				}
			}
		}

		[SupportedOSPlatform ("ios")]
		[SupportedOSPlatform ("maccatalyst")]
		[SupportedOSPlatform ("macos")]
		[SupportedOSPlatform ("tvos")]
		public class MachSend : Mach {
			static IntPtr type_mach_send;

			public MachSend (IntPtr handle, bool owns) : base (handle, owns) { }
			public MachSend (IntPtr handle) : base (handle, false) { }

			public MachSend (int machPort, bool sendDead = false, DispatchQueue? queue = null)
			{
				if (type_mach_send == IntPtr.Zero)
					type_mach_send = Dlfcn.dlsym (Libraries.System.Handle, "_dispatch_source_type_mach_send");
				this.queue = queue;
				var handle = dispatch_source_create (type_mach_send,
								 handle: (IntPtr) machPort,
								 mask: (IntPtr) (sendDead ? 1 : 0),
								 queue: queue.GetHandle ());
				GC.KeepAlive (queue);
				if (handle != IntPtr.Zero)
					InitializeHandle (handle);
			}

			/// <summary>Determines if the handler was invoked due to a send right being destroyed.</summary>
			///         <value>True if the send right was destroyed.</value>
			///         <remarks>
			///           <para />
			///         </remarks>
			public bool SendRightsDestroyed {
				get {
					return dispatch_source_get_data (GetCheckedHandle ()) != IntPtr.Zero;
				}
			}
		}

		[SupportedOSPlatform ("ios")]
		[SupportedOSPlatform ("maccatalyst")]
		[SupportedOSPlatform ("macos")]
		[SupportedOSPlatform ("tvos")]
		public class MachReceive : DispatchSource {
			static IntPtr type_mach_recv;

			public MachReceive (IntPtr handle, bool owns) : base (handle, owns) { }
			public MachReceive (IntPtr handle) : base (handle, false) { }

			public MachReceive (int machPort, DispatchQueue? queue = null)
			{
				if (type_mach_recv == IntPtr.Zero)
					type_mach_recv = Dlfcn.dlsym (Libraries.System.Handle, "_dispatch_source_type_mach_recv");
				this.queue = queue;
				var handle = dispatch_source_create (type_mach_recv,
								 handle: (IntPtr) machPort,
								 mask: IntPtr.Zero,
								 queue: queue.GetHandle ());
				GC.KeepAlive (queue);
				if (handle != IntPtr.Zero)
					InitializeHandle (handle);
			}
		}


		[SupportedOSPlatform ("ios")]
		[SupportedOSPlatform ("maccatalyst")]
		[SupportedOSPlatform ("macos")]
		[SupportedOSPlatform ("tvos")]
		public class MemoryPressure : DispatchSource {
			static IntPtr type_memorypressure;
			public MemoryPressure (IntPtr handle, bool owns) : base (handle, owns) { }
			public MemoryPressure (IntPtr handle) : base (handle, false) { }

			public MemoryPressure (MemoryPressureFlags monitorFlags = MemoryPressureFlags.Normal | MemoryPressureFlags.Warn, DispatchQueue? queue = null)
			{
				if (type_memorypressure == IntPtr.Zero)
					type_memorypressure = Dlfcn.dlsym (Libraries.System.Handle, "_dispatch_source_type_memorypressure");
				this.queue = queue;
				var handle = dispatch_source_create (type_memorypressure,
								 handle: IntPtr.Zero,
								 mask: (IntPtr) monitorFlags,
								 queue: queue.GetHandle ());
				GC.KeepAlive (queue);
				if (handle != IntPtr.Zero)
					InitializeHandle (handle);
			}

			/// <summary>Reports the condition that was observed.</summary>
			///         <value>The condition observed.</value>
			///         <remarks>The event handler can probe this property to determine why it was invoked.</remarks>
			public MemoryPressureFlags PressureFlags {
				get {
					return (MemoryPressureFlags) dispatch_source_get_data (GetCheckedHandle ());
				}
			}
		}

		[SupportedOSPlatform ("ios")]
		[SupportedOSPlatform ("maccatalyst")]
		[SupportedOSPlatform ("macos")]
		[SupportedOSPlatform ("tvos")]
		public class ProcessMonitor : DispatchSource {
			static IntPtr type_proc;

			public ProcessMonitor (IntPtr handle, bool owns) : base (handle, owns) { }
			public ProcessMonitor (IntPtr handle) : base (handle, false) { }
			public ProcessMonitor (int processId, ProcessMonitorFlags monitorKind = ProcessMonitorFlags.Exit, DispatchQueue? queue = null)
			{

				if (type_proc == IntPtr.Zero)
					type_proc = Dlfcn.dlsym (Libraries.System.Handle, "_dispatch_source_type_proc");
				this.queue = queue;
				var handle = dispatch_source_create (type_proc,
								 handle: (IntPtr) processId,
								 mask: (IntPtr) monitorKind,
								 queue: queue.GetHandle ());
				GC.KeepAlive (queue);
				if (handle != IntPtr.Zero)
					InitializeHandle (handle);
			}

			/// <summary>The process ID that is being monitored</summary>
			///         <value>To be added.</value>
			///         <remarks>To be added.</remarks>
			public int ProcessId {
				get {
					return (int) dispatch_source_get_handle (GetCheckedHandle ());
				}
			}

			/// <summary>Determines which events were observed.</summary>
			///         <value>Returns the observed events.</value>
			///         <remarks>Method that can be invoked by the dispath source event handler.</remarks>
			public ProcessMonitorFlags MonitorFlags {
				get {
					return (ProcessMonitorFlags) dispatch_source_get_data (GetCheckedHandle ());
				}
			}
		}

		[SupportedOSPlatform ("ios")]
		[SupportedOSPlatform ("maccatalyst")]
		[SupportedOSPlatform ("macos")]
		[SupportedOSPlatform ("tvos")]
		public class ReadMonitor : DispatchSource {
			static IntPtr type_read;
			public ReadMonitor (IntPtr handle, bool owns) : base (handle, owns) { }
			public ReadMonitor (IntPtr handle) : base (handle, false) { }
			public ReadMonitor (int fileDescriptor, DispatchQueue? queue = null)
			{

				if (type_read == IntPtr.Zero)
					type_read = Dlfcn.dlsym (Libraries.System.Handle, "_dispatch_source_type_read");
				this.queue = queue;
				var handle = dispatch_source_create (type_read,
								 handle: (IntPtr) fileDescriptor,
								 mask: IntPtr.Zero,
								 queue: queue.GetHandle ());
				GC.KeepAlive (queue);
				if (handle != IntPtr.Zero)
					InitializeHandle (handle);
			}

			/// <summary>The file descriptor being monitored.</summary>
			///         <value>
			///           <para>
			///           </para>
			///         </value>
			///         <remarks>
			///           <para />
			///         </remarks>
			public int FileDescriptor {
				get {
					return (int) dispatch_source_get_handle (GetCheckedHandle ());
				}
			}

			/// <summary>Estimated number of bytes available to read from the file descriptor.</summary>
			///         <value>
			///           <para />
			///         </value>
			///         <remarks>
			///           <para />
			///         </remarks>
			public int BytesAvailable {
				get {
					return (int) dispatch_source_get_data (GetCheckedHandle ());
				}
			}
		}

		[SupportedOSPlatform ("ios")]
		[SupportedOSPlatform ("maccatalyst")]
		[SupportedOSPlatform ("macos")]
		[SupportedOSPlatform ("tvos")]
		public class SignalMonitor : DispatchSource {
			static IntPtr type_signal;
			public SignalMonitor (IntPtr handle, bool owns) : base (handle, owns) { }
			public SignalMonitor (IntPtr handle) : base (handle, false) { }
			public SignalMonitor (int signalNumber, DispatchQueue? queue = null)
			{
				if (type_signal == IntPtr.Zero)
					type_signal = Dlfcn.dlsym (Libraries.System.Handle, "_dispatch_source_type_signal");
				this.queue = queue;
				var handle = dispatch_source_create (type_signal,
								 handle: (IntPtr) signalNumber,
								 mask: IntPtr.Zero,
								 queue: queue.GetHandle ());
				GC.KeepAlive (queue);
				if (handle != IntPtr.Zero)
					InitializeHandle (handle);
			}

			/// <summary>To be added.</summary>
			///         <value>To be added.</value>
			///         <remarks>To be added.</remarks>
			public int SignalNumber {
				get {
					return (int) dispatch_source_get_handle (GetCheckedHandle ());
				}
			}

			/// <summary>The number of signals received since the last invocation of the event handler.</summary>
			///         <value>count</value>
			///         <remarks>The number of signals received since the last invocation of the event handler.</remarks>
			public int SignalsDelivered {
				get {
					return (int) dispatch_source_get_data (GetCheckedHandle ());
				}
			}
		}

		[SupportedOSPlatform ("ios")]
		[SupportedOSPlatform ("maccatalyst")]
		[SupportedOSPlatform ("macos")]
		[SupportedOSPlatform ("tvos")]
		public class Timer : DispatchSource {
			static IntPtr type_timer;
			public Timer (IntPtr handle, bool owns) : base (handle, owns) { }
			public Timer (IntPtr handle) : base (handle, false) { }
			public Timer (DispatchQueue? queue = null) : this (false, queue) { }

			public Timer (bool strict = false, DispatchQueue? queue = null)
			{
				if (type_timer == IntPtr.Zero)
					type_timer = Dlfcn.dlsym (Libraries.System.Handle, "_dispatch_source_type_timer");
				this.queue = queue;
				var handle = dispatch_source_create (type_timer,
								 handle: IntPtr.Zero,
								 mask: strict ? (IntPtr) 1 : IntPtr.Zero,
								 queue: queue.GetHandle ());
				GC.KeepAlive (queue);
				if (handle != IntPtr.Zero)
					InitializeHandle (handle);
			}

			/// <summary>Number of times the timer has fired since the last invocation of the event handler</summary>
			///         <value>Number of times the timer has fired since the last invocation of the event handler</value>
			///         <remarks>
			///           <para />
			///         </remarks>
			public int TimerFiredCount {
				get {
					return (int) dispatch_source_get_data (GetCheckedHandle ());
				}
			}
			[DllImport (Constants.libcLibrary)]
			extern static void dispatch_source_set_timer (dispatch_source_t source, /* dispathc_time_t */ulong start, long interval, long leeway);

			public void SetTimer (DispatchTime time, long nanosecondInterval, long nanosecondLeeway)
			{
				dispatch_source_set_timer (GetCheckedHandle (), time.Nanoseconds, nanosecondInterval, nanosecondLeeway);
			}
		}

		[SupportedOSPlatform ("ios")]
		[SupportedOSPlatform ("maccatalyst")]
		[SupportedOSPlatform ("macos")]
		[SupportedOSPlatform ("tvos")]
		public class VnodeMonitor : DispatchSource {
			static IntPtr type_vnode;

			// If different than -1, we opened the descriptor and must close it.
			int fd;

			public VnodeMonitor (IntPtr handle, bool owns) : base (handle, owns) { }
			public VnodeMonitor (IntPtr handle) : base (handle, false) { }

			public VnodeMonitor (int fileDescriptor, VnodeMonitorKind vnodeKind, DispatchQueue? queue = null)
			{
				if (type_vnode == IntPtr.Zero)
					type_vnode = Dlfcn.dlsym (Libraries.System.Handle, "_dispatch_source_type_vnode");
				this.queue = queue;
				fd = -1;
				var handle = dispatch_source_create (type_vnode,
								 handle: (IntPtr) fileDescriptor,
								 mask: (IntPtr) vnodeKind,
								 queue: queue.GetHandle ());
				GC.KeepAlive (queue);
				if (handle != IntPtr.Zero)
					InitializeHandle (handle);
			}

			const int O_EVTONLY = 0x8000;
			[DllImport (Constants.libcLibrary, SetLastError = true)]
			extern static int open (IntPtr path, int flags);

			[DllImport (Constants.libcLibrary)]
			internal extern static int close (int fd);

			public VnodeMonitor (string path, VnodeMonitorKind vnodeKind, DispatchQueue? queue = null)
			{
				if (path is null)
					ObjCRuntime.ThrowHelper.ThrowArgumentNullException (nameof (path));

				using var pathPtr = new TransientString (path);
				fd = open (pathPtr, O_EVTONLY);
				if (fd == -1)
					throw new IOException ("Failure to open the file", Marshal.GetLastWin32Error ());
				if (type_vnode == IntPtr.Zero)
					type_vnode = Dlfcn.dlsym (Libraries.System.Handle, "_dispatch_source_type_vnode");
				this.queue = queue;

				var handle = dispatch_source_create (type_vnode,
								 handle: (IntPtr) fd,
								 mask: (IntPtr) vnodeKind,
								 queue: queue.GetHandle ());
				GC.KeepAlive (queue);
				if (handle != IntPtr.Zero)
					InitializeHandle (handle);
			}

			protected override void Dispose (bool disposing)
			{
				if (fd != -1) {
					close (fd);
					fd = -1;
				}
				base.Dispose (disposing);
			}

			/// <summary>File descriptor that is being monitored</summary>
			///         <value>To be added.</value>
			///         <remarks>To be added.</remarks>
			public int FileDescriptor {
				get {
					return (int) dispatch_source_get_handle (GetCheckedHandle ());
				}
			}

			/// <summary>Events that were observed on the file.</summary>
			///         <value>The events that were observed on the file.</value>
			///         <remarks>This property can be invoked from the event handler to check on which changes took place on the file being monitored.</remarks>
			public VnodeMonitorKind ObservedEvents {
				get {
					return (VnodeMonitorKind) (int) dispatch_source_get_data (GetCheckedHandle ());
				}
			}

		}

		[SupportedOSPlatform ("ios")]
		[SupportedOSPlatform ("maccatalyst")]
		[SupportedOSPlatform ("macos")]
		[SupportedOSPlatform ("tvos")]
		public class WriteMonitor : DispatchSource {
			static IntPtr type_write;
			public WriteMonitor (IntPtr handle, bool owns) : base (handle, owns) { }
			public WriteMonitor (IntPtr handle) : base (handle, false) { }

			public WriteMonitor (int fileDescriptor, DispatchQueue? queue = null)
			{
				if (type_write == IntPtr.Zero)
					type_write = Dlfcn.dlsym (Libraries.System.Handle, "_dispatch_source_type_write");
				this.queue = queue;
				var handle = dispatch_source_create (type_write,
								 handle: (IntPtr) fileDescriptor,
								 mask: IntPtr.Zero,
								 queue: queue.GetHandle ());
				GC.KeepAlive (queue);
				if (handle != IntPtr.Zero)
					InitializeHandle (handle);
			}
			/// <summary>The file descriptor being monitored.</summary>
			///         <value>
			///           <para />
			///         </value>
			///         <remarks>
			///           <para />
			///         </remarks>
			public int FileDescriptor {
				get {
					return (int) dispatch_source_get_handle (GetCheckedHandle ());
				}
			}

			/// <summary>Buffer space available to write on the file descriptor being monitored.</summary>
			///         <value>
			///           <para />
			///         </value>
			///         <remarks>
			///           <para />
			///         </remarks>
			public int BufferSpaceAvailable {
				get {
					return (int) dispatch_source_get_data (GetCheckedHandle ());
				}
			}
		}
	}
}
