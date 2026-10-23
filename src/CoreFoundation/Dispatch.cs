//
// Dispatch.cs: Support for Grand Central Dispatch framework
//
// Authors:
//   Miguel de Icaza (miguel@gnome.org)
//   Marek Safar (marek.safar@gmail.com)
//
// Copyright 2010 Novell, Inc.
// Copyright 2011-2014 Xamarin Inc
//
// this contains DispathcObject, Group and Queue
//
//
// Permission is hereby granted, free of charge, to any person obtaining
// a copy of this software and associated documentation files (the
// "Software"), to deal in the Software without restriction, including
// without limitation the rights to use, copy, modify, merge, publish,
// distribute, sublicense, and/or sell copies of the Software, and to
// permit persons to whom the Software is furnished to do so, subject to
// the following conditions:
// 
// The above copyright notice and this permission notice shall be
// included in all copies or substantial portions of the Software.
// 
// THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND,
// EXPRESS OR IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF
// MERCHANTABILITY, FITNESS FOR A PARTICULAR PURPOSE AND
// NONINFRINGEMENT. IN NO EVENT SHALL THE AUTHORS OR COPYRIGHT HOLDERS BE
// LIABLE FOR ANY CLAIM, DAMAGES OR OTHER LIABILITY, WHETHER IN AN ACTION
// OF CONTRACT, TORT OR OTHERWISE, ARISING FROM, OUT OF OR IN CONNECTION
// WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN THE SOFTWARE.
//

#nullable enable

using System.Collections.Concurrent;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Runtime.ExceptionServices;
using System.Threading;

namespace CoreFoundation {

	// The native constants are defined in usr/include/dispatch/queue.h, but since they're
	// not in any enum, they're untyped.
	/// <summary>Specifies the scheduling priority of a global dispatch queue.</summary>
	public enum DispatchQueuePriority : int {
		/// <summary>A high-priority queue.</summary>
		High = 2,
		/// <summary>The default-priority queue.</summary>
		Default = 0,
		/// <summary>A low-priority queue.</summary>
		Low = -2,
		/// <summary>A queue for work that does not require timely execution.</summary>
		Background = Int16.MinValue,
	}

	// dispatch_qos_class_t is defined in usr/include/dispatch/queue.h, but redirects to qos_class_t
	// the qos_class_t enum is defined in usr/include/sys/qos.h (typed as 'unsigned int')
	/// <summary>Specifies the quality-of-service class used to schedule dispatch work.</summary>
	public enum DispatchQualityOfService : uint {
		/// <summary>Work needed immediately to update the user interface.</summary>
		UserInteractive = 0x21,
		/// <summary>Work initiated by the user whose result is needed promptly.</summary>
		UserInitiated = 0x19,
		/// <summary>Work scheduled with the system's default quality of service.</summary>
		Default = 0x15,
		/// <summary>Long-running work whose progress may be visible to the user.</summary>
		Utility = 0x11,
		/// <summary>Work that is not time-sensitive or directly visible to the user.</summary>
		Background = 0x09,
		/// <summary>No explicit quality-of-service class is specified.</summary>
		Unspecified = 0x00,
	}

	/// <summary>Base class for reference-counted Grand Central Dispatch objects.</summary>
	/// <remarks>
	///   Dispatch queues and sources support suspension and target queues. Balance suspension and resumption
	///   before releasing the final native reference; releasing a suspended dispatch object is a programming error.
	/// </remarks>
	[SupportedOSPlatform ("ios")]
	[SupportedOSPlatform ("maccatalyst")]
	[SupportedOSPlatform ("macos")]
	[SupportedOSPlatform ("tvos")]
	public abstract class DispatchObject : NativeObject {
#if !COREBUILD
		//
		// Constructors and lifecycle
		//
		[Preserve (Conditional = true)]
		internal DispatchObject (NativeHandle handle, bool owns)
			: base (handle, owns, verify: true)
		{
		}

		internal DispatchObject ()
		{
		}

		[DllImport (Constants.libcLibrary)]
		extern static IntPtr dispatch_release (IntPtr o);

		[DllImport (Constants.libcLibrary)]
		extern static IntPtr dispatch_retain (IntPtr o);

		/// <summary>Increments the native dispatch object's reference count.</summary>
		/// <exception cref="ObjectDisposedException">This object has been disposed.</exception>
		protected internal override void Retain ()
		{
			dispatch_retain (GetCheckedHandle ());
		}

		/// <summary>Decrements the native dispatch object's reference count.</summary>
		/// <exception cref="ObjectDisposedException">This object has been disposed.</exception>
		/// <remarks>Normally called by disposal. Balance any manual retain and release operations.</remarks>
		protected internal override void Release ()
		{
			dispatch_release (GetCheckedHandle ());
		}

		[DllImport (Constants.libcLibrary)]
		extern static void dispatch_set_target_queue (/* dispatch_object_t */ IntPtr queue, /* dispatch_queue_t */ IntPtr target);

		/// <summary>Sets the queue used to execute this dispatch object's work.</summary>
		/// <param name="queue">The target queue, or <see langword="null" /> to use the default target queue.</param>
		/// <exception cref="ObjectDisposedException">This object or <paramref name="queue" /> has been disposed.</exception>
		/// <remarks>
		///   Only use this method with dispatch objects that support target queues. Set a queue's target before activation;
		///   changing the target of an active or global queue is not supported by modern dispatch APIs.
		///   A serial target queue also serializes work from queues that target it.
		/// </remarks>
		public void SetTargetQueue (DispatchQueue? queue)
		{
			// note: null is allowed because DISPATCH_TARGET_QUEUE_DEFAULT is defined as NULL (dispatch/queue.h)
			IntPtr q = queue is null ? IntPtr.Zero : queue.GetCheckedHandle ();
			dispatch_set_target_queue (GetCheckedHandle (), q);
			GC.KeepAlive (queue);
		}

		[DllImport (Constants.libcLibrary)]
		internal extern static void dispatch_resume (IntPtr o);

		[DllImport (Constants.libcLibrary)]
		internal extern static void dispatch_suspend (IntPtr o);

		/// <summary>Activates a dispatch object created in an initially inactive state.</summary>
		/// <exception cref="ObjectDisposedException">This object has been disposed.</exception>
		/// <remarks>
		///   Configure an inactive object before activation. Activate it before disposing it.
		///   Calling this method on an already active object has no effect.
		/// </remarks>
		[SupportedOSPlatform ("macos")]
		[SupportedOSPlatform ("ios")]
		[SupportedOSPlatform ("tvos")]
		[SupportedOSPlatform ("maccatalyst")]
		public void Activate ()
		{
			dispatch_activate (GetCheckedHandle ());
		}

		[SupportedOSPlatform ("macos")]
		[SupportedOSPlatform ("ios")]
		[SupportedOSPlatform ("tvos")]
		[SupportedOSPlatform ("maccatalyst")]
		[DllImport (Constants.libcLibrary)]
		extern static void dispatch_activate (/* dispatch_object_t */ IntPtr @object);
#endif // !COREBUILD
	}

	/// <summary>Schedules work for synchronous or asynchronous execution using Grand Central Dispatch.</summary>
	/// <remarks>
	///   <para>
	///     Serial queues execute one work item at a time in submission order. Concurrent queues can execute
	///     multiple work items at once, and their completion order is not guaranteed. Except for the main queue,
	///     a queue is not bound to a specific thread.
	///   </para>
	///   <para>
	///     Barriers provide exclusive execution on queues created with concurrent attributes.
	///     On global or serial queues, barrier submissions behave like ordinary submissions.
	///   </para>
	///   <para>
	///     Managed delegate callbacks install a queue synchronization context when the executing thread has none.
	///     Synchronous managed delegate APIs propagate callback exceptions to their caller.
	///     They wait only for the delegate to return, not for continuations of an <c>async void</c> delegate.
	///     Asynchronous callbacks and native dispatch blocks retain the runtime's unhandled-exception behavior;
	///     catch exceptions inside those callbacks when recovery is required.
	///   </para>
	/// </remarks>
	[SupportedOSPlatform ("ios")]
	[SupportedOSPlatform ("maccatalyst")]
	[SupportedOSPlatform ("macos")]
	[SupportedOSPlatform ("tvos")]
	public sealed class DispatchQueue : DispatchObject {
#if !COREBUILD
		[Preserve (Conditional = true)]
		internal DispatchQueue (NativeHandle handle, bool owns) : base (handle, owns)
		{
		}

		/// <summary>Creates a named serial dispatch queue.</summary>
		/// <param name="label">A diagnostic label for the queue, conventionally written in reverse-DNS notation.</param>
		/// <remarks>Work submitted to this queue executes serially in submission order.</remarks>
		public DispatchQueue (string label)
			: base (dispatch_queue_create (label, IntPtr.Zero), true)
		{
			if (Handle == IntPtr.Zero)
				throw new Exception ("Error creating dispatch queue");
		}

		static IntPtr concurrent_queue;
		static IntPtr ConcurrentQueue {
			get {
				if (concurrent_queue == IntPtr.Zero)
					concurrent_queue = Dlfcn.dlsym (Dlfcn.RTLD.Self, "_dispatch_queue_attr_concurrent");
				return concurrent_queue;
			}
		}

		/// <summary>Creates a named serial or concurrent dispatch queue.</summary>
		/// <param name="label">A diagnostic label for the queue, conventionally written in reverse-DNS notation.</param>
		/// <param name="concurrent"><see langword="true" /> to allow concurrent execution; otherwise, <see langword="false" />.</param>
		/// <remarks>Concurrent queues support barriers that execute exclusively relative to other work on the queue.</remarks>
		public DispatchQueue (string label, bool concurrent)
				: base (dispatch_queue_create (label, concurrent ? ConcurrentQueue : IntPtr.Zero), true)
		{
			if (Handle == IntPtr.Zero)
				throw new Exception ("Error creating dispatch queue");
		}

		/// <summary>Creates a named dispatch queue with the specified attributes and target queue.</summary>
		/// <param name="label">A diagnostic label for the queue.</param>
		/// <param name="attributes">The attributes used when creating the queue. Subsequent changes do not affect the queue.</param>
		/// <param name="target">The target queue, or <see langword="null" /> to use the default target queue.</param>
		/// <exception cref="ObjectDisposedException"><paramref name="target" /> has been disposed.</exception>
		/// <remarks>An initially inactive queue must be activated before it can execute work or be disposed.</remarks>
		[SupportedOSPlatform ("macos")]
		[SupportedOSPlatform ("ios")]
		[SupportedOSPlatform ("tvos")]
		[SupportedOSPlatform ("maccatalyst")]
		public DispatchQueue (string label, Attributes attributes, DispatchQueue? target = null)
			: base (dispatch_queue_create_with_target (label, attributes?.Create () ?? IntPtr.Zero, target is null ? IntPtr.Zero : target.GetCheckedHandle ()), true)
		{
			GC.KeepAlive (target);
		}

		//
		// Properties and methods
		//

		/// <summary>Gets the diagnostic label of this queue.</summary>
		/// <value>The label supplied at construction, or the system-assigned label for a system queue.</value>
		/// <exception cref="ObjectDisposedException">This queue has been disposed.</exception>
		public string? Label {
			get {
				return Marshal.PtrToStringAnsi (dispatch_queue_get_label (GetCheckedHandle ()));
			}
		}

		/// <summary>Gets the diagnostic label of the queue executing the current work item.</summary>
		/// <value>The current queue's label, or an empty string when there is no current queue.</value>
		[SupportedOSPlatform ("ios")]
		[SupportedOSPlatform ("maccatalyst")]
		[SupportedOSPlatform ("macos")]
		[SupportedOSPlatform ("tvos")]
		public static string? CurrentQueueLabel {
			get {
				return Marshal.PtrToStringAnsi (dispatch_queue_get_label (IntPtr.Zero));
			}
		}

		/// <summary>Increments this queue's suspension count, preventing new work items from starting.</summary>
		/// <exception cref="ObjectDisposedException">This queue has been disposed.</exception>
		/// <remarks>
		///   Work already executing is not interrupted. Balance every call with <see cref="Resume" /> before disposal.
		///   Suspension has no effect on global queues.
		/// </remarks>
		public void Suspend ()
		{
			dispatch_suspend (GetCheckedHandle ());
		}

		/// <summary>Decrements this queue's suspension count, allowing work to resume when it reaches zero.</summary>
		/// <exception cref="ObjectDisposedException">This queue has been disposed.</exception>
		/// <remarks>Balance calls with <see cref="Suspend" />. Resuming an unsuspended queue is a programming error.</remarks>
		public void Resume ()
		{
			dispatch_resume (GetCheckedHandle ());
		}

		[DllImport (Constants.libcLibrary)]
		extern static IntPtr dispatch_get_context (IntPtr o);

		[DllImport (Constants.libcLibrary)]
		extern static void dispatch_set_context (IntPtr o, IntPtr ctx);

		[DllImport (Constants.libcLibrary)]
		extern unsafe static void dispatch_apply_f (nint iterations, IntPtr queue, IntPtr ctx, delegate* unmanaged<IntPtr, nint, void> dispatch);

		/// <summary>Gets or sets an unmanaged context pointer associated with this queue.</summary>
		/// <value>An application-defined pointer, initially <see cref="IntPtr.Zero" />.</value>
		/// <exception cref="ObjectDisposedException">This queue has been disposed.</exception>
		/// <remarks>The caller manages the pointed-to storage. Context is not supported on global queues.</remarks>
		public IntPtr Context {
			get {
				return dispatch_get_context (GetCheckedHandle ());
			}
			set {
				dispatch_set_context (GetCheckedHandle (), value);
			}
		}

		/// <summary>Gets the current dispatch queue using the deprecated native queue lookup API.</summary>
		/// <value>A wrapper for the current dispatch queue.</value>
		/// <remarks>
		///   Do not use this property to decide whether synchronous dispatch is safe; target queues can also cause deadlocks.
		///   Prefer <see cref="CurrentQueueLabel" /> for diagnostics and queue-specific data for queue identification.
		/// </remarks>
		[SupportedOSPlatform ("ios")]
		[SupportedOSPlatform ("maccatalyst")]
		[SupportedOSPlatform ("macos")]
		[SupportedOSPlatform ("tvos")]
		[ObsoletedOSPlatform ("macos10.9")]
		[ObsoletedOSPlatform ("ios6.0")]
		public static DispatchQueue CurrentQueue {
			get {
				return new DispatchQueue (dispatch_get_current_queue (), false);
			}
		}

		/// <summary>Gets a global concurrent queue with the specified scheduling priority.</summary>
		/// <param name="priority">The requested queue priority.</param>
		/// <returns>A wrapper for the system-managed global queue.</returns>
		/// <remarks>Priority influences scheduling but does not guarantee execution order between queues.</remarks>
		public static DispatchQueue GetGlobalQueue (DispatchQueuePriority priority)
		{
			return new DispatchQueue (dispatch_get_global_queue ((nint) (int) priority, 0), false);
		}

		/// <summary>Gets a global concurrent queue with the specified quality-of-service class.</summary>
		/// <param name="service">The requested quality-of-service class.</param>
		/// <returns>A wrapper for the system-managed global queue.</returns>
		public static DispatchQueue GetGlobalQueue (DispatchQualityOfService service)
		{
			return new DispatchQueue (dispatch_get_global_queue ((nint) (int) service, 0), false);
		}

		/// <summary>Gets the global concurrent queue with default priority.</summary>
		/// <value>A wrapper for the system-managed default global queue.</value>
		public static DispatchQueue DefaultGlobalQueue {
			get {
				return new DispatchQueue (dispatch_get_global_queue ((nint) (int) DispatchQueuePriority.Default, 0), false);
			}
		}

		static IntPtr main_q;

		/// <summary>Gets the serial dispatch queue associated with the application's main thread.</summary>
		/// <value>A wrapper for the system-managed main queue.</value>
		/// <remarks>
		///   The application's main run loop normally services this queue. A command-line macOS application without
		///   a main run loop can service it using <c>DispatchQueue.MainIteration</c>, which does not return.
		/// </remarks>
		public static DispatchQueue MainQueue {
			get {
				if (main_q == IntPtr.Zero) {
					// Can't use a Field attribute because we don't support generating a call to Dlfcn.GetIndirect.
					main_q = Dlfcn.GetIndirect (Libraries.libdispatch.Handle, "_dispatch_main_q");
				}

				return new DispatchQueue (main_q, false);
			}
		}


		//
		// Dispatching
		//
		[UnmanagedCallersOnly]
		internal static void static_dispatcher_to_managed (IntPtr context)
		{
			GCHandle gch = GCHandle.FromIntPtr (context);
			var obj = gch.Target as Tuple<Action, DispatchQueue>;
			gch.Free ();
			if (obj is not null) {
				var sc = SynchronizationContext.Current;

				// Set GCD synchronization context. Mainly used when await executes inside GCD to continue
				// execution on same dispatch queue. Set the context only when there is no user context
				// set, including UIKitSynchronizationContext
				//
				// This assumes that only 1 queue can run on thread at the same time
				//
				if (sc is null)
					SynchronizationContext.SetSynchronizationContext (new DispatchQueueSynchronizationContext (obj.Item2));

				try {
					obj.Item1 ();
				} finally {
					if (sc is null)
						SynchronizationContext.SetSynchronizationContext (null);
				}
			}

		}

		[UnmanagedCallersOnly]
		static void static_dispatcher_iterations_to_managed (IntPtr context, nint count)
		{
			GCHandle gch = GCHandle.FromIntPtr (context);
			var obj = gch.Target as Tuple<Action<nint>, DispatchQueue>;
			if (obj is not null) {
				var sc = SynchronizationContext.Current;

				// Set GCD synchronization context. Mainly used when await executes inside GCD to continue
				// execution on same dispatch queue. Set the context only when there is no user context
				// set, including UIKitSynchronizationContext
				//
				// This assumes that only 1 queue can run on thread at the same time
				//
				if (sc is null)
					SynchronizationContext.SetSynchronizationContext (new DispatchQueueSynchronizationContext (obj.Item2));

				try {
					obj.Item1 (count);
				} finally {
					if (sc is null)
						SynchronizationContext.SetSynchronizationContext (null);
				}
			}

		}

		// Synchronize native destruction with readers using any wrapper for the same queue.
		static readonly object specific_lock = new object ();

		[UnmanagedCallersOnly]
		static void static_free_gchandle (IntPtr context)
		{
			lock (specific_lock) {
				GCHandle.FromIntPtr (context).Free ();
			}
		}

		/// <summary>Schedules an action for asynchronous execution on this queue.</summary>
		/// <param name="action">The action to execute.</param>
		/// <exception cref="ArgumentNullException"><paramref name="action" /> is <see langword="null" />.</exception>
		/// <exception cref="ObjectDisposedException">This queue has been disposed.</exception>
		/// <remarks>
		///   Returns without waiting for execution. Callback exceptions are unhandled on the executing thread;
		///   they are not propagated to the submitting caller. Catch exceptions inside the callback to handle them.
		/// </remarks>
		public void DispatchAsync (Action action)
		{
			if (action is null)
				ObjCRuntime.ThrowHelper.ThrowArgumentNullException (nameof (action));
			var handle = GetCheckedHandle ();
			unsafe {
				dispatch_async_f (handle, (IntPtr) GCHandle.Alloc (Tuple.Create (action, this)), &static_dispatcher_to_managed);
			}
		}

		/// <summary>Schedules a native dispatch block for asynchronous execution on this queue.</summary>
		/// <param name="block">The dispatch block to execute.</param>
		/// <exception cref="ArgumentNullException"><paramref name="block" /> is <see langword="null" />.</exception>
		/// <exception cref="ObjectDisposedException">This queue or <paramref name="block" /> has been disposed.</exception>
		/// <remarks>Exceptions must be handled inside the block; they are not propagated to the submitting caller.</remarks>
		public void DispatchAsync (DispatchBlock block)
		{
			if (block is null)
				ObjCRuntime.ThrowHelper.ThrowArgumentNullException (nameof (block));

			dispatch_async (GetCheckedHandle (), block.GetCheckedHandle ());
			GC.KeepAlive (block);
		}

		/// <summary>Executes an action synchronously on this queue.</summary>
		/// <param name="action">The action to execute.</param>
		/// <exception cref="ArgumentNullException"><paramref name="action" /> is <see langword="null" />.</exception>
		/// <exception cref="ObjectDisposedException">This queue has been disposed.</exception>
		/// <remarks>
		///   Waits for completion and rethrows callback exceptions on the caller with their original stack trace.
		///   Calling from the same serial queue, or a serial queue needed by this queue, causes a deadlock.
		/// </remarks>
		public void DispatchSync (Action action)
		{
			if (action is null)
				ObjCRuntime.ThrowHelper.ThrowArgumentNullException (nameof (action));

			DispatchSynchronous (action, barrier: false);
		}

		void DispatchSynchronous (Action action, bool barrier)
		{
			var handle = GetCheckedHandle ();
			ExceptionDispatchInfo? exception = null;
			Action callback = () => {
				try {
					action ();
				} catch (Exception ex) {
					exception = ExceptionDispatchInfo.Capture (ex);
				}
			};
			unsafe {
				var context = (IntPtr) GCHandle.Alloc (Tuple.Create (callback, this));
				if (barrier)
					dispatch_barrier_sync_f (handle, context, &static_dispatcher_to_managed);
				else
					dispatch_sync_f (handle, context, &static_dispatcher_to_managed);
			}
			exception?.Throw ();
		}

		/// <summary>Executes a function synchronously on this dispatch queue and returns its result.</summary>
		/// <typeparam name="T">The type of the result.</typeparam>
		/// <param name="func">The function to execute.</param>
		/// <returns>The value returned by <paramref name="func" />.</returns>
		/// <exception cref="ArgumentNullException"><paramref name="func" /> is <see langword="null" />.</exception>
		/// <exception cref="ObjectDisposedException">This queue has been disposed.</exception>
		/// <remarks>
		///   Waits for completion and rethrows callback exceptions on the caller with their original stack trace.
		///   Calling from the same serial queue, or a serial queue needed by this queue, causes a deadlock.
		/// </remarks>
		public T DispatchSync<T> (Func<T> func)
		{
			if (func is null)
				ObjCRuntime.ThrowHelper.ThrowArgumentNullException (nameof (func));

			var result = new T [1];
			DispatchSync (() => {
				result [0] = func ();
			});
			return result [0];
		}

		/// <summary>Executes a native dispatch block synchronously on this queue.</summary>
		/// <param name="block">The dispatch block to execute.</param>
		/// <exception cref="ArgumentNullException"><paramref name="block" /> is <see langword="null" />.</exception>
		/// <exception cref="ObjectDisposedException">This queue or <paramref name="block" /> has been disposed.</exception>
		/// <remarks>
		///   Unlike the managed delegate overloads, this overload does not capture exceptions inside native blocks.
		///   Handle exceptions inside the block to avoid the runtime's unhandled-exception behavior.
		///   Calling from the same serial queue, or a serial queue needed by this queue, causes a deadlock.
		/// </remarks>
		public void DispatchSync (DispatchBlock block)
		{
			if (block is null)
				ObjCRuntime.ThrowHelper.ThrowArgumentNullException (nameof (block));

			dispatch_sync (GetCheckedHandle (), block.GetCheckedHandle ());
			GC.KeepAlive (block);
		}

		/// <summary>Schedules an action for asynchronous barrier execution on this queue.</summary>
		/// <param name="action">The action to execute exclusively on a custom concurrent queue.</param>
		/// <exception cref="ArgumentNullException"><paramref name="action" /> is <see langword="null" />.</exception>
		/// <exception cref="ObjectDisposedException">This queue has been disposed.</exception>
		/// <remarks>
		///   On a custom concurrent queue, waits for earlier work to finish and prevents later work from starting until completion.
		///   Returns without waiting. Callback exceptions are unhandled on the executing thread, not rethrown to the caller.
		///   On global and serial queues, this behaves like an ordinary asynchronous submission.
		/// </remarks>
		public void DispatchBarrierAsync (Action action)
		{
			if (action is null)
				ObjCRuntime.ThrowHelper.ThrowArgumentNullException (nameof (action));

			var handle = GetCheckedHandle ();
			unsafe {
				dispatch_barrier_async_f (handle, (IntPtr) GCHandle.Alloc (Tuple.Create (action, this)), &static_dispatcher_to_managed);
			}
		}

		/// <summary>Schedules a native dispatch block for asynchronous barrier execution on this queue.</summary>
		/// <param name="block">The dispatch block to execute.</param>
		/// <exception cref="ArgumentNullException"><paramref name="block" /> is <see langword="null" />.</exception>
		/// <exception cref="ObjectDisposedException">This queue or <paramref name="block" /> has been disposed.</exception>
		/// <remarks>
		///   Barrier exclusivity applies only to custom concurrent queues. Handle exceptions inside the block;
		///   they are not propagated to the submitting caller.
		/// </remarks>
		public void DispatchBarrierAsync (DispatchBlock block)
		{
			if (block is null)
				ObjCRuntime.ThrowHelper.ThrowArgumentNullException (nameof (block));

			dispatch_barrier_async (GetCheckedHandle (), block.GetCheckedHandle ());
			GC.KeepAlive (block);
		}

		/// <summary>Executes an action synchronously as a barrier on this queue.</summary>
		/// <param name="action">The action to execute.</param>
		/// <exception cref="ArgumentNullException"><paramref name="action" /> is <see langword="null" />.</exception>
		/// <exception cref="ObjectDisposedException">This queue has been disposed.</exception>
		/// <remarks>
		///   On a custom concurrent queue, waits for earlier work and executes the action exclusively before later work.
		///   Callback exceptions are rethrown on the caller with their original stack trace.
		///   Calling from this queue, or a serial queue needed by this queue, can deadlock.
		/// </remarks>
		public void DispatchBarrierSync (Action action)
		{
			if (action is null)
				ObjCRuntime.ThrowHelper.ThrowArgumentNullException (nameof (action));

			DispatchSynchronous (action, barrier: true);
		}

		/// <summary>Executes a native dispatch block synchronously as a barrier on this queue.</summary>
		/// <param name="block">The dispatch block to execute.</param>
		/// <exception cref="ArgumentNullException"><paramref name="block" /> is <see langword="null" />.</exception>
		/// <exception cref="ObjectDisposedException">This queue or <paramref name="block" /> has been disposed.</exception>
		/// <remarks>
		///   Barrier exclusivity applies only to custom concurrent queues. Unlike the managed delegate overload,
		///   this overload does not capture exceptions inside native blocks; handle them inside the block.
		///   Calling from this queue, or a serial queue needed by this queue, can deadlock.
		/// </remarks>
		public void DispatchBarrierSync (DispatchBlock block)
		{
			if (block is null)
				ObjCRuntime.ThrowHelper.ThrowArgumentNullException (nameof (block));

			dispatch_barrier_sync (GetCheckedHandle (), block.GetCheckedHandle ());
			GC.KeepAlive (block);
		}

		/// <summary>Schedules an action for asynchronous execution no earlier than the specified deadline.</summary>
		/// <param name="when">The earliest time at which the action may be submitted to this queue.</param>
		/// <param name="action">The action to execute.</param>
		/// <exception cref="ArgumentNullException"><paramref name="action" /> is <see langword="null" />.</exception>
		/// <exception cref="ObjectDisposedException">This queue has been disposed.</exception>
		/// <remarks>
		///   This method does not block, and execution may occur later than the deadline.
		///   Callback exceptions are unhandled on the executing thread, not rethrown to the submitting caller.
		/// </remarks>
		public void DispatchAfter (DispatchTime when, Action action)
		{
			if (action is null)
				ObjCRuntime.ThrowHelper.ThrowArgumentNullException (nameof (action));
			var handle = GetCheckedHandle ();
			unsafe {
				dispatch_after_f (when.Nanoseconds, handle, (IntPtr) GCHandle.Alloc (Tuple.Create (action, this)), &static_dispatcher_to_managed);
			}
		}

		/// <summary>Schedules a native dispatch block for execution no earlier than the specified deadline.</summary>
		/// <param name="when">The earliest time at which the block may be submitted to this queue.</param>
		/// <param name="block">The dispatch block to execute.</param>
		/// <exception cref="ArgumentNullException"><paramref name="block" /> is <see langword="null" />.</exception>
		/// <exception cref="ObjectDisposedException">This queue or <paramref name="block" /> has been disposed.</exception>
		/// <remarks>This method does not block. Handle exceptions inside the block; they are not propagated to the caller.</remarks>
		public void DispatchAfter (DispatchTime when, DispatchBlock block)
		{
			if (block is null)
				ObjCRuntime.ThrowHelper.ThrowArgumentNullException (nameof (block));

			dispatch_after (when.Nanoseconds, GetCheckedHandle (), block.GetCheckedHandle ());
			GC.KeepAlive (block);
		}

#if !XAMCORE_5_0
		/// <summary>Synchronously executes an action the specified number of times on this queue.</summary>
		/// <param name="action">The action to execute, receiving the zero-based iteration index as a 32-bit integer.</param>
		/// <param name="times">The nonnegative number of iterations to execute.</param>
		/// <exception cref="ArgumentNullException"><paramref name="action" /> is <see langword="null" />.</exception>
		/// <exception cref="ArgumentOutOfRangeException"><paramref name="times" /> is negative.</exception>
		/// <exception cref="OverflowException"><paramref name="times" /> cannot be represented as a native-sized integer.</exception>
		/// <exception cref="ObjectDisposedException">This queue has been disposed.</exception>
		/// <exception cref="AggregateException">One or more callbacks failed, including an iteration index that exceeded <see cref="int.MaxValue" />.</exception>
		/// <remarks>
		///   This method returns after all iterations complete. Iterations may execute concurrently when this is a concurrent queue.
		///   Callback exceptions are collected and thrown together after all iterations finish.
		///   Use <see cref="Submit(Action{nint}, nint)" /> for native-sized iteration indices.
		/// </remarks>
		[OverloadResolutionPriority (-1)]
		public void Submit (Action<int> action, long times)
		{
			if (action is null)
				ObjCRuntime.ThrowHelper.ThrowArgumentNullException (nameof (action));
			if (times < 0)
				throw new ArgumentOutOfRangeException (nameof (times));
			Submit (index => action (checked((int) index)), checked((nint) times));
		}
#endif // !XAMCORE_5_0

		/// <summary>Synchronously executes an action the specified number of times on this queue.</summary>
		/// <param name="action">The action to execute, receiving the zero-based native-sized iteration index.</param>
		/// <param name="times">The nonnegative number of iterations to execute.</param>
		/// <exception cref="ArgumentNullException"><paramref name="action" /> is <see langword="null" />.</exception>
		/// <exception cref="ArgumentOutOfRangeException"><paramref name="times" /> is negative.</exception>
		/// <exception cref="ObjectDisposedException">This queue has been disposed.</exception>
		/// <exception cref="AggregateException">One or more callbacks failed.</exception>
		/// <remarks>
		///   This method returns after all iterations complete. Iterations may execute concurrently when this is a concurrent queue.
		///   If <paramref name="times" /> is zero, the action is not invoked.
		///   Callback exceptions are collected and thrown together after all iterations finish, without stopping other iterations.
		///   Calling from the same serial queue, or a serial queue needed by this queue, causes a deadlock.
		/// </remarks>
		public void Submit (Action<nint> action, nint times)
		{
			if (action is null)
				ObjCRuntime.ThrowHelper.ThrowArgumentNullException (nameof (action));
			if (times < 0)
				throw new ArgumentOutOfRangeException (nameof (times));
			var handle = GetCheckedHandle ();
			var exceptions = new ConcurrentQueue<Exception> ();
			Action<nint> callback = index => {
				try {
					action (index);
				} catch (Exception ex) {
					exceptions.Enqueue (ex);
				}
			};
			var gch = GCHandle.Alloc (Tuple.Create (callback, this));
			try {
				unsafe {
					dispatch_apply_f (times, handle, (IntPtr) gch, &static_dispatcher_iterations_to_managed);
				}
			} finally {
				gch.Free ();
			}
			if (!exceptions.IsEmpty)
				throw new AggregateException (exceptions);
		}

		/// <summary>Associates a managed object with a queue-specific key.</summary>
		/// <param name="key">A unique, nonzero pointer used as an identity; its pointed-to contents are not read.</param>
		/// <param name="context">The object to associate with the key.</param>
		/// <exception cref="ArgumentException"><paramref name="key" /> is <see cref="IntPtr.Zero" />.</exception>
		/// <exception cref="ObjectDisposedException">This queue has been disposed.</exception>
		/// <remarks>
		///   Replacing a value releases the previous managed reference. The queue retains the object until replacement
		///   or native queue destruction. Concurrent reads and replacements are supported, including through different
		///   wrappers for the same native queue.
		///   This API is not supported on global queues.
		/// </remarks>
		public void SetSpecific (IntPtr key, object context)
		{
			if (key == IntPtr.Zero)
				throw new ArgumentException ("The key must not be zero.", nameof (key));
			var handle = GetCheckedHandle ();
			unsafe {
				dispatch_queue_set_specific (handle, key, (IntPtr) GCHandle.Alloc (context), &static_free_gchandle);
			}
		}

		/// <summary>Gets the managed object associated with a key on this queue.</summary>
		/// <param name="key">The identity pointer previously passed to <see cref="SetSpecific" />.</param>
		/// <returns>The associated object, or <see langword="null" /> if the key is unset.</returns>
		/// <exception cref="ObjectDisposedException">This queue has been disposed.</exception>
		/// <remarks>
		///   Looks only on this queue, not its target queues. Only read values registered by <see cref="SetSpecific" />.
		///   Concurrent reads and replacements are supported, including through different wrappers for the same native queue.
		/// </remarks>
		public object? GetSpecific (IntPtr key)
		{
			lock (specific_lock) {
				var context = dispatch_queue_get_specific (GetCheckedHandle (), key);
				if (context == IntPtr.Zero)
					return null;
				GCHandle gchandle = (GCHandle) context;
				return gchandle.Target;
			}
		}

		/// <summary>Gets this queue's assigned quality-of-service class and relative priority.</summary>
		/// <param name="relative_priority">Receives the priority offset within the quality-of-service class.</param>
		/// <returns>The queue's assigned quality-of-service class, or <see cref="DispatchQualityOfService.Unspecified" /> if none is assigned.</returns>
		/// <exception cref="ObjectDisposedException">This queue has been disposed.</exception>
		/// <remarks>The default global queue has no explicitly assigned quality-of-service class.</remarks>
		[SupportedOSPlatform ("macos")]
		[SupportedOSPlatform ("ios")]
		[SupportedOSPlatform ("maccatalyst")]
		[SupportedOSPlatform ("tvos")]
		public DispatchQualityOfService GetQualityOfService (out int relative_priority)
		{
			unsafe {
				fixed (int* rel_pri = &relative_priority)
					return dispatch_queue_get_qos_class (GetCheckedHandle (), rel_pri);
			}
		}

		/// <summary>Gets this queue's assigned quality-of-service class.</summary>
		/// <value>The assigned class, or <see cref="DispatchQualityOfService.Unspecified" /> if none is assigned.</value>
		/// <exception cref="ObjectDisposedException">This queue has been disposed.</exception>
		/// <remarks>The default global queue has no explicitly assigned quality-of-service class.</remarks>
		[SupportedOSPlatform ("macos")]
		[SupportedOSPlatform ("ios")]
		[SupportedOSPlatform ("maccatalyst")]
		[SupportedOSPlatform ("tvos")]
		public DispatchQualityOfService QualityOfService {
			get {
				unsafe {
					return dispatch_queue_get_qos_class (GetCheckedHandle (), null);
				}
			}
		}

		static IntPtr dispatch_queue_create (string label, IntPtr attr)
		{
			using var labelPtr = new TransientString (label);
			return dispatch_queue_create (labelPtr, attr);
		}
		//
		// Native methods
		//
		[DllImport (Constants.libcLibrary)]
		extern static IntPtr dispatch_queue_create (IntPtr label, IntPtr attr);

		[SupportedOSPlatform ("macos")]
		[SupportedOSPlatform ("ios")]
		[SupportedOSPlatform ("tvos")]
		[SupportedOSPlatform ("maccatalyst")]
		static IntPtr dispatch_queue_create_with_target (string label, IntPtr attr, IntPtr target)
		{
			using var labelPtr = new TransientString (label);
			return dispatch_queue_create_with_target (labelPtr, attr, target);
		}

		[DllImport (Constants.libcLibrary, EntryPoint = "dispatch_queue_create_with_target$V2")]
		extern static IntPtr dispatch_queue_create_with_target (IntPtr label, IntPtr attr, IntPtr target);

		[DllImport (Constants.libcLibrary)]
		extern unsafe static void dispatch_async_f (IntPtr queue, IntPtr context, delegate* unmanaged<IntPtr, void> dispatch);

		[DllImport (Constants.libcLibrary)]
		extern static void dispatch_async (IntPtr queue, IntPtr block);

		[DllImport (Constants.libcLibrary)]
		extern unsafe static void dispatch_sync_f (IntPtr queue, IntPtr context, delegate* unmanaged<IntPtr, void> dispatch);

		[DllImport (Constants.libcLibrary)]
		extern static void dispatch_sync (IntPtr queue, IntPtr block);

		[DllImport (Constants.libcLibrary)]
		extern unsafe static void dispatch_barrier_async_f (IntPtr queue, IntPtr context, delegate* unmanaged<IntPtr, void> dispatch);

		[DllImport (Constants.libcLibrary)]
		extern static void dispatch_barrier_async (IntPtr queue, IntPtr block);

		[DllImport (Constants.libcLibrary)]
		extern unsafe static void dispatch_barrier_sync_f (IntPtr queue, IntPtr context, delegate* unmanaged<IntPtr, void> dispatch);

		[DllImport (Constants.libcLibrary)]
		extern static void dispatch_barrier_sync (IntPtr queue, IntPtr block);

		[DllImport (Constants.libcLibrary)]
		extern unsafe static void dispatch_after_f (/* dispath_time_t */ ulong time, IntPtr queue, IntPtr context, delegate* unmanaged<IntPtr, void> dispatch);

		[DllImport (Constants.libcLibrary)]
		extern static void dispatch_after (/* dispath_time_t */ ulong time, IntPtr queue, IntPtr block);

		[SupportedOSPlatform ("ios")]
		[SupportedOSPlatform ("maccatalyst")]
		[SupportedOSPlatform ("macos")]
		[SupportedOSPlatform ("tvos")]
		[ObsoletedOSPlatform ("macos10.9")]
		[ObsoletedOSPlatform ("ios6.0")]
		[DllImport (Constants.libcLibrary)]
		extern static IntPtr dispatch_get_current_queue ();

		[DllImport (Constants.libcLibrary)]
		// dispatch_queue_t dispatch_get_global_queue (long identifier, unsigned long flags);
		extern static IntPtr dispatch_get_global_queue (nint identifier, nuint flags);

		[DllImport (Constants.libcLibrary)]
		// this returns a "const char*" so we cannot make a string out of it since it will be freed (and crash)
		extern static IntPtr dispatch_queue_get_label (IntPtr queue);

		[DllImport (Constants.libcLibrary)]
		extern unsafe static void dispatch_queue_set_specific (IntPtr queue, /* const void* */ IntPtr key, /* void *_Nullable */ IntPtr context, delegate* unmanaged<IntPtr, void> /* _Nullable */ destructor);

		[DllImport (Constants.libcLibrary)]
		extern static IntPtr dispatch_queue_get_specific (IntPtr queue, /* const void* */ IntPtr key);

		[SupportedOSPlatform ("macos")]
		[SupportedOSPlatform ("ios")]
		[SupportedOSPlatform ("maccatalyst")]
		[SupportedOSPlatform ("tvos")]
		[DllImport (Constants.libcLibrary)]
		unsafe extern static /* dispatch_qos_class_t */ DispatchQualityOfService dispatch_queue_get_qos_class (/* dispatch_queue_t */ IntPtr queue, /* int *_Nullable */ int* relative_priority);

#if MONOMAC
		//
		// Not to be used by apps that use UIApplicationMain, NSApplicationMain or CFRunLoopRun,
		// so not available on Monotouch
		//
		[DllImport (Constants.libcLibrary)]
		static extern IntPtr dispatch_main ();

		/// <summary>Services the main dispatch queue indefinitely in a command-line macOS application.</summary>
		/// <remarks>
		///   Call only on the main thread, instead of running an AppKit application or a main run loop.
		///   Despite its name, this method does not perform a single iteration and never returns.
		/// </remarks>
		public static void MainIteration ()
		{
			dispatch_main ();
		}
#endif

		/// <summary>Specifies the attributes used to create a dispatch queue.</summary>
		/// <remarks>Changing this object after queue creation does not change the queue.</remarks>
		public class Attributes {
			/// <summary>Gets or sets whether the queue may execute work items concurrently.</summary>
			/// <value><see langword="true" /> for a concurrent queue; otherwise, <see langword="false" />. The default is <see langword="false" />.</value>
			public bool Concurrent { get; set; }

			/// <summary>Gets or sets whether the queue is created inactive.</summary>
			/// <value><see langword="true" /> to require explicit activation; otherwise, <see langword="false" />. The default is <see langword="false" />.</value>
			/// <remarks>An inactive queue must be activated before it can execute work or be disposed.</remarks>
			[SupportedOSPlatform ("macos")]
			[SupportedOSPlatform ("maccatalyst")]
			[SupportedOSPlatform ("ios")]
			[SupportedOSPlatform ("tvos")]
			public bool IsInitiallyInactive { get; set; }

			/// <summary>Gets or sets the autorelease-pool behavior for the queue's work items.</summary>
			/// <value>The autorelease frequency, or <see langword="null" /> to use the native default.</value>
			[SupportedOSPlatform ("macos")]
			[SupportedOSPlatform ("maccatalyst")]
			[SupportedOSPlatform ("ios")]
			[SupportedOSPlatform ("tvos")]
			public AutoreleaseFrequency? AutoreleaseFrequency { get; set; }

			/// <summary>Gets or sets the priority offset within the assigned quality-of-service class.</summary>
			/// <value>An offset from -15 through 0. The default is 0.</value>
			/// <remarks>Used only when <see cref="QualityOfService" /> is set.</remarks>
			[SupportedOSPlatform ("macos")]
			[SupportedOSPlatform ("ios")]
			[SupportedOSPlatform ("tvos")]
			[SupportedOSPlatform ("maccatalyst")]
			public int RelativePriority { get; set; }

			/// <summary>Gets or sets the quality-of-service class assigned to the queue.</summary>
			/// <value>The requested class, or <see langword="null" /> to leave it unspecified.</value>
			[SupportedOSPlatform ("macos")]
			[SupportedOSPlatform ("ios")]
			[SupportedOSPlatform ("tvos")]
			[SupportedOSPlatform ("maccatalyst")]
			public DispatchQualityOfService? QualityOfService { get; set; }

			internal IntPtr Create ()
			{
				IntPtr rv = IntPtr.Zero;

				if (Concurrent)
					rv = DispatchQueue.ConcurrentQueue;

				if (IsInitiallyInactive)
					rv = dispatch_queue_attr_make_initially_inactive (rv);

				if (AutoreleaseFrequency.HasValue)
					rv = dispatch_queue_attr_make_with_autorelease_frequency (rv, (nuint) (ulong) AutoreleaseFrequency.Value);

				if (QualityOfService.HasValue)
					rv = dispatch_queue_attr_make_with_qos_class (rv, QualityOfService.Value, RelativePriority);

				return rv;
			}

			[SupportedOSPlatform ("macos")]
			[SupportedOSPlatform ("ios")]
			[SupportedOSPlatform ("tvos")]
			[DllImport (Constants.libcLibrary)]
			static extern /* dispatch_queue_attr_t */ IntPtr dispatch_queue_attr_make_initially_inactive (/* dispatch_queue_attr_t _Nullable */ IntPtr attr);

			[SupportedOSPlatform ("macos")]
			[SupportedOSPlatform ("ios")]
			[SupportedOSPlatform ("tvos")]
			[DllImport (Constants.libcLibrary)]
			static extern /* dispatch_queue_attr_t */ IntPtr dispatch_queue_attr_make_with_autorelease_frequency (/* dispatch_queue_attr_t _Nullable */ IntPtr attr, /* dispatch_autorelease_frequency_t */ nuint frequency);

			[SupportedOSPlatform ("macos")]
			[SupportedOSPlatform ("ios")]
			[DllImport (Constants.libcLibrary)]
			static extern /* dispatch_queue_attr_t */ IntPtr dispatch_queue_attr_make_with_qos_class (/* dispatch_queue_attr_t _Nullable */ IntPtr attr, /* dispatch_qos_class_t */ DispatchQualityOfService qos_class, int relative_priority);
		}

		/// <summary>Specifies how a dispatch queue manages autorelease pools for its work items.</summary>
		[SupportedOSPlatform ("macos")]
		[SupportedOSPlatform ("ios")]
		[SupportedOSPlatform ("tvos")]
		[SupportedOSPlatform ("maccatalyst")]
		[Native]
		public enum AutoreleaseFrequency : ulong /* unsigned long */
		{
			/// <summary>Inherit autorelease-pool behavior from the target queue.</summary>
			Inherit = 0,
			/// <summary>Create and drain an autorelease pool around each work item.</summary>
			WorkItem = 1,
			/// <summary>Do not create autorelease pools for work items; the application must manage them as needed.</summary>
			Never = 2,
		}
#endif // !COREBUILD
	}

	// Some insights from: https://opensource.apple.com/source/libdispatch/libdispatch-442.1.4/src/time.c

	/// <summary>Represents an opaque dispatch deadline or a special immediate or infinite deadline.</summary>
	/// <remarks>
	///   Create deadlines relative to <see cref="Now" /> for monotonic time, or relative to <see cref="WallTime" />
	///   for wall-clock time. The stored native representation is not generally a nanosecond count.
	/// </remarks>
	[SupportedOSPlatform ("ios")]
	[SupportedOSPlatform ("maccatalyst")]
	[SupportedOSPlatform ("macos")]
	[SupportedOSPlatform ("tvos")]
	public struct DispatchTime {
#if !COREBUILD
		/// <summary>Represents the current time, or an immediate timeout when used for waiting.</summary>
		public static readonly DispatchTime Now = new DispatchTime ();
		/// <summary>Represents a deadline that never expires.</summary>
		public static readonly DispatchTime Forever = new DispatchTime (ulong.MaxValue);

		/// <summary>Creates a dispatch time from its raw native representation.</summary>
		/// <param name="nanoseconds">An encoded native dispatch time, or a special immediate or infinite value.</param>
		/// <remarks>
		///   This constructor does not convert a nanosecond duration to a deadline.
		///   Use a constructor with a reference time and delta for relative deadlines.
		/// </remarks>
		public DispatchTime (ulong nanoseconds)
			: this ()
		{
			Nanoseconds = nanoseconds;
		}

		/// <summary>Creates a deadline by adding a nanosecond delta to a reference dispatch time.</summary>
		/// <param name="when">The reference time. <see cref="Now" /> uses the current monotonic time.</param>
		/// <param name="deltaNanoseconds">The signed number of nanoseconds to add.</param>
		/// <remarks>Native dispatch arithmetic saturates deadlines that exceed its supported range.</remarks>
		public DispatchTime (DispatchTime when, long deltaNanoseconds)
			: this ()
		{
			Nanoseconds = dispatch_time (when.Nanoseconds, deltaNanoseconds);
		}

		/// <summary>Creates a deadline by adding a time interval to a reference dispatch time.</summary>
		/// <param name="when">The reference time. <see cref="Now" /> uses the current monotonic time.</param>
		/// <param name="delta">The signed interval to add.</param>
		/// <exception cref="OverflowException"><paramref name="delta" /> cannot be represented as a signed 64-bit nanosecond count.</exception>
		public DispatchTime (DispatchTime when, TimeSpan delta) : this ()
		{
			Nanoseconds = dispatch_time (when.Nanoseconds, checked(delta.Ticks * 100));
		}

		/// <summary>Gets the raw native dispatch-time representation.</summary>
		/// <value>An opaque encoded deadline, zero for <see cref="Now" />, or <see cref="ulong.MaxValue" /> for <see cref="Forever" />.</value>
		/// <remarks>Despite the property name, this value is not generally a duration measured in nanoseconds.</remarks>
		public ulong Nanoseconds { get; private set; }

		/// <summary>Gets a dispatch deadline representing the current wall-clock time.</summary>
		/// <value>A newly sampled wall-clock dispatch time, independent of this instance's value.</value>
		/// <remarks>Use this as a reference time when a deadline should track wall-clock adjustments.</remarks>
		public DispatchTime WallTime {
			get {
				// This gives us access to setting the time to _dispatch_get_nanoseconds.
				// We can create the alternative dispatch_walltime with a time by setting a
				// negative value for nanoseconds.
				return new DispatchTime (dispatch_walltime (IntPtr.Zero, 0));
			}
		}

		[DllImport (Constants.libcLibrary)]
		extern static ulong dispatch_time (/* dispatch_time_t */ ulong when, long delta);

		[DllImport (Constants.libcLibrary)]
		extern static ulong dispatch_walltime (IntPtr /* struct timespec */ when, long delta);
#endif // !COREBUILD
	}

	/// <summary>Tracks a group of work items for completion notifications and synchronous waiting.</summary>
	/// <remarks>
	///   Work items may execute on different queues. Groups track completion, not callback exceptions;
	///   handle exceptions within asynchronous callbacks. A completed group can be reused.
	/// </remarks>
	[SupportedOSPlatform ("ios")]
	[SupportedOSPlatform ("maccatalyst")]
	[SupportedOSPlatform ("macos")]
	[SupportedOSPlatform ("tvos")]
	public class DispatchGroup : DispatchObject {
#if !COREBUILD
		[Preserve (Conditional = true)]
		private DispatchGroup (NativeHandle handle, bool owns)
			: base (handle, owns)
		{
		}

		/// <summary>Creates an empty dispatch group.</summary>
		public DispatchGroup ()
			: base (dispatch_group_create (), true)
		{
		}

		/// <summary>Creates an empty dispatch group, returning <see langword="null" /> if native allocation fails.</summary>
		/// <returns>The new group, or <see langword="null" /> if the native group could not be created.</returns>
		public static DispatchGroup? Create ()
		{
			var ptr = dispatch_group_create ();
			if (ptr == IntPtr.Zero)
				return null;

			return new DispatchGroup (ptr, true);
		}

		/// <summary>Schedules an asynchronous action on a queue and tracks it as part of this group.</summary>
		/// <param name="queue">The queue on which to execute the action.</param>
		/// <param name="action">The action to execute.</param>
		/// <exception cref="ArgumentNullException"><paramref name="queue" /> or <paramref name="action" /> is <see langword="null" />.</exception>
		/// <exception cref="ObjectDisposedException">This group or <paramref name="queue" /> has been disposed.</exception>
		/// <remarks>
		///   Returns without waiting. Callback exceptions are unhandled on the executing thread;
		///   neither this method nor <see cref="Wait(DispatchTime)" /> propagates them to the caller.
		/// </remarks>
		public void DispatchAsync (DispatchQueue queue, Action action)
		{
			if (queue is null)
				ObjCRuntime.ThrowHelper.ThrowArgumentNullException (nameof (queue));
			if (action is null)
				ObjCRuntime.ThrowHelper.ThrowArgumentNullException (nameof (action));

			var handle = GetCheckedHandle ();
			var queueHandle = queue.GetCheckedHandle ();
			unsafe {
				dispatch_group_async_f (handle, queueHandle, (IntPtr) GCHandle.Alloc (Tuple.Create (action, queue)), &DispatchQueue.static_dispatcher_to_managed);
				GC.KeepAlive (queue);
			}
		}

		/// <summary>Schedules a native dispatch block when all currently outstanding work in this group completes.</summary>
		/// <param name="queue">The queue on which to execute the notification.</param>
		/// <param name="block">The notification block.</param>
		/// <exception cref="ArgumentNullException"><paramref name="queue" /> or <paramref name="block" /> is <see langword="null" />.</exception>
		/// <exception cref="ObjectDisposedException">This group, <paramref name="queue" />, or <paramref name="block" /> has been disposed.</exception>
		/// <remarks>
		///   An empty group schedules the notification immediately. The notification is not itself tracked by the group.
		///   Handle exceptions inside the native block; they are not propagated to the submitting caller.
		/// </remarks>
		public void Notify (DispatchQueue queue, DispatchBlock block)
		{
			if (queue is null)
				ObjCRuntime.ThrowHelper.ThrowArgumentNullException (nameof (queue));
			if (block is null)
				ObjCRuntime.ThrowHelper.ThrowArgumentNullException (nameof (block));
			dispatch_group_notify (GetCheckedHandle (), queue.GetCheckedHandle (), block.GetCheckedHandle ());
			GC.KeepAlive (queue);
			GC.KeepAlive (block);
		}

		/// <summary>Schedules an action when all currently outstanding work in this group completes.</summary>
		/// <param name="queue">The queue on which to execute the notification.</param>
		/// <param name="action">The notification action.</param>
		/// <exception cref="ArgumentNullException"><paramref name="queue" /> or <paramref name="action" /> is <see langword="null" />.</exception>
		/// <exception cref="ObjectDisposedException">This group or <paramref name="queue" /> has been disposed.</exception>
		/// <remarks>
		///   An empty group schedules the notification immediately. The notification is not itself tracked by the group.
		///   Callback exceptions are unhandled on the executing thread, not propagated to the submitting caller.
		/// </remarks>
		public void Notify (DispatchQueue queue, Action action)
		{
			if (queue is null)
				ObjCRuntime.ThrowHelper.ThrowArgumentNullException (nameof (queue));
			if (action is null)
				ObjCRuntime.ThrowHelper.ThrowArgumentNullException (nameof (action));
			var handle = GetCheckedHandle ();
			var queueHandle = queue.GetCheckedHandle ();
			unsafe {
				dispatch_group_notify_f (handle, queueHandle, (IntPtr) GCHandle.Alloc (Tuple.Create (action, queue)), &DispatchQueue.static_dispatcher_to_managed);
				GC.KeepAlive (queue);
			}
		}

		/// <summary>Increments the number of outstanding work items tracked by this group.</summary>
		/// <exception cref="ObjectDisposedException">This group has been disposed.</exception>
		/// <remarks>Balance each call with <see cref="Leave" /> after the associated work completes.</remarks>
		public void Enter ()
		{
			dispatch_group_enter (GetCheckedHandle ());
		}

		/// <summary>Decrements the number of outstanding work items tracked by this group.</summary>
		/// <exception cref="ObjectDisposedException">This group has been disposed.</exception>
		/// <remarks>Balance calls with <see cref="Enter" />. Leaving an empty group is a programming error.</remarks>
		public void Leave ()
		{
			dispatch_group_leave (GetCheckedHandle ());
		}

		/// <summary>Waits synchronously for all outstanding work in this group to complete or the deadline to expire.</summary>
		/// <param name="timeout">An absolute dispatch deadline, <see cref="DispatchTime.Now" /> to poll, or <see cref="DispatchTime.Forever" /> to wait indefinitely.</param>
		/// <returns><see langword="true" /> if the group completed; otherwise, <see langword="false" /> if the deadline expired.</returns>
		/// <exception cref="ObjectDisposedException">This group has been disposed.</exception>
		/// <remarks>
		///   Returns immediately for an empty group. Notifications may still be pending after this method returns.
		///   Waiting does not propagate callback exceptions and can deadlock if this thread is needed by the group's work.
		/// </remarks>
		public bool Wait (DispatchTime timeout)
		{
			return dispatch_group_wait (GetCheckedHandle (), timeout.Nanoseconds) == 0;
		}

		/// <summary>Waits synchronously for all outstanding work in this group to complete or a relative timeout to expire.</summary>
		/// <param name="timeout">The interval to wait, measured from the current monotonic time.</param>
		/// <returns><see langword="true" /> if the group completed; otherwise, <see langword="false" /> if the timeout expired.</returns>
		/// <exception cref="ObjectDisposedException">This group has been disposed.</exception>
		/// <exception cref="OverflowException"><paramref name="timeout" /> cannot be represented as a signed 64-bit nanosecond count.</exception>
		/// <remarks>Waiting does not propagate callback exceptions or wait for group notifications to finish executing.</remarks>
		public bool Wait (TimeSpan timeout)
		{
			return Wait (new DispatchTime (DispatchTime.Now, timeout));
		}

		[DllImport (Constants.libcLibrary)]
		extern static IntPtr dispatch_group_create ();

		[DllImport (Constants.libcLibrary)]
		extern unsafe static void dispatch_group_async_f (IntPtr group, IntPtr queue, IntPtr context, delegate* unmanaged<IntPtr, void> block);

		[DllImport (Constants.libcLibrary)]
		extern unsafe static void dispatch_group_notify_f (IntPtr group, IntPtr queue, IntPtr context, delegate* unmanaged<IntPtr, void> block);

		[DllImport (Constants.libcLibrary)]
		extern static void dispatch_group_notify (IntPtr group, IntPtr queue, IntPtr block);

		[DllImport (Constants.libcLibrary)]
		extern static void dispatch_group_enter (IntPtr group);

		[DllImport (Constants.libcLibrary)]
		extern static void dispatch_group_leave (IntPtr group);

		[DllImport (Constants.libcLibrary)]
		extern static nint dispatch_group_wait (IntPtr group, ulong timeout);
#endif // !COREBUILD
	}
}
