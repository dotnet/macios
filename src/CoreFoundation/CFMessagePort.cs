//
// CFMessagePort.cs: CFMessagePort is a wrapper around two native Mach ports with bidirectional communication support
//
// Authors:
//   Oleg Demchenko (oleg.demchenko@xamarin.com)
//
// Copyright 2015 Xamarin Inc
//

#nullable enable

using System.Collections.Generic;
using System.Threading;

using dispatch_queue_t = System.IntPtr;

namespace CoreFoundation {

	// untyped enum from CFMessagePort.h
	// used as a return value of type SInt32 (always 4 bytes)
	/// <summary>Specifies the result of sending a message with <see cref="CFMessagePort.SendRequest" />.</summary>
	public enum CFMessagePortSendRequestStatus {
		/// <summary>The message was sent, and any expected reply was received.</summary>
		Success = 0,

		/// <summary>The port timed out before the message could be sent.</summary>
		SendTimeout = -1,

		/// <summary>The port timed out before the response was received.</summary>
		ReceiveTimeout = -2,

		/// <summary>The port became invalid before the message was sent.</summary>
		IsInvalid = -3,

		/// <summary>An error occurred.</summary>
		TransportError = -4,

		/// <summary>The port became invalid after the message was sent, but before a response was received.</summary>
		BecameInvalidError = -5,
	}

	/// <summary>Provides local interprocess communication through named message ports.</summary>
	/// <remarks>
	///   <para>Create a local port with <see cref="CreateLocalPort" /> to receive messages, and create a remote port with <see cref="CreateRemotePort" /> to send messages to a named local port.</para>
	///   <para>A local port must be scheduled by calling <see cref="CreateRunLoopSource" /> and adding the returned source to a run loop, or by calling <see cref="SetDispatchQueue" />.</para>
	/// </remarks>
	[SupportedOSPlatform ("ios")]
	[SupportedOSPlatform ("maccatalyst")]
	[SupportedOSPlatform ("macos")]
	[SupportedOSPlatform ("tvos")]
	public class CFMessagePort : NativeObject {
		sealed class MessagePortContext {
			int retainCount = 1;

			public CFMessagePortCallBack Callback { get; }

			public MessagePortContext (CFMessagePortCallBack callback)
			{
				Callback = callback;
			}

			public void Retain ()
			{
				Interlocked.Increment (ref retainCount);
			}

			public void Release (GCHandle handle)
			{
				if (Interlocked.Decrement (ref retainCount) == 0)
					handle.Free ();
			}
		}

		[StructLayout (LayoutKind.Sequential)]
		unsafe struct ContextProxy {
			/* CFIndex */
			nint version; // must be 0
			public /* void * */ IntPtr info;
			public delegate* unmanaged<IntPtr, IntPtr> retain;
			public delegate* unmanaged<IntPtr, void> release;
			public delegate* unmanaged<IntPtr, IntPtr> copyDescription;
		}

		/// <summary>Handles a message received by a local message port.</summary>
		/// <param name="type">The application-defined message identifier.</param>
		/// <param name="data">The message data.</param>
		/// <returns>The data to return to the sender.</returns>
		/// <remarks>The <paramref name="data" /> object is only valid for the duration of the callback. Copy it if it must be retained after the callback returns.</remarks>
		public delegate NSData CFMessagePortCallBack (int type, NSData data);

		// Remote ports pass null as the native invalidation callback's info argument, and multiple
		// managed wrappers may share a native port, so invalidation callbacks must be keyed by handle.
		static Dictionary<IntPtr, Action> invalidationHandles = new Dictionary<IntPtr, Action> (Runtime.IntPtrEqualityComparer);

		/// <summary>Gets a value that indicates whether this instance represents a remote port.</summary>
		/// <value><see langword="true" /> for a remote port; <see langword="false" /> for a local port.</value>
		public bool IsRemote {
			get {
				return CFMessagePortIsRemote (GetCheckedHandle ()) != 0;
			}
		}

		/// <summary>Gets or sets the registered name of the message port.</summary>
		/// <value>The registered port name, or <see langword="null" /> if the port is unnamed.</value>
		/// <exception cref="ArgumentNullException">The value being assigned is <see langword="null" />.</exception>
		/// <remarks>The setter does not report whether the name was changed. Use <see cref="TrySetName" /> when the result is needed. Changing the name does not make an already scheduled unnamed local port able to receive messages.</remarks>
		public string? Name {
			get {
				return CFString.FromHandle (CFMessagePortGetName (GetCheckedHandle ()));
			}
			set {
				if (value is null)
					ObjCRuntime.ThrowHelper.ThrowArgumentNullException (nameof (value));
				TrySetName (value);
			}
		}

		/// <summary>Attempts to change the registered name of this local message port.</summary>
		/// <param name="name">The new port name.</param>
		/// <returns><see langword="true" /> if the name was changed; otherwise, <see langword="false" />.</returns>
		/// <exception cref="ArgumentNullException"><paramref name="name" /> is <see langword="null" />.</exception>
		/// <remarks>This method returns <see langword="false" /> for remote ports, duplicate names, invalid names, and native registration failures.</remarks>
		public bool TrySetName (string name)
		{
			if (name is null)
				ObjCRuntime.ThrowHelper.ThrowArgumentNullException (nameof (name));

			var n = CFString.CreateNative (name);
			try {
				return CFMessagePortSetName (GetCheckedHandle (), n) != 0;
			} finally {
				CFString.ReleaseNative (n);
			}
		}

		/// <summary>Gets a value that indicates whether the port can send or receive messages.</summary>
		/// <value><see langword="true" /> if the port is valid; otherwise, <see langword="false" />.</value>
		public bool IsValid {
			get {
				return CFMessagePortIsValid (GetCheckedHandle ()) != 0;
			}
		}

		/// <summary>Gets or sets the callback invoked when the message port becomes invalid.</summary>
		/// <value>The invalidation callback, or <see langword="null" /> if no callback is installed.</value>
		/// <remarks>Assigning a new callback replaces the previous callback. Assign <see langword="null" /> to remove it. If the port is already invalid when a callback is assigned, the callback is invoked synchronously.</remarks>
		public Action? InvalidationCallback {
			get {
				lock (invalidationHandles) {
					invalidationHandles.TryGetValue (GetCheckedHandle (), out var result);
					return result;
				}
			}
			set {
				var handle = GetCheckedHandle ();
				lock (invalidationHandles) {
					if (value is null)
						invalidationHandles.Remove (handle);
					else
						invalidationHandles [handle] = value;
				}

				unsafe {
					delegate* unmanaged<IntPtr, IntPtr, void> callback = value is null ? null : &MessagePortInvalidationCallback;
					CFMessagePortSetInvalidationCallBack (handle, callback);
				}
			}
		}

		[Preserve (Conditional = true)]
		internal CFMessagePort (NativeHandle handle, bool owns)
			: base (handle, owns)
		{
		}

		[DllImport (Constants.CoreFoundationLibrary)]
		static unsafe extern /* CFMessagePortRef */ IntPtr CFMessagePortCreateLocal (/* CFAllocatorRef */ IntPtr allocator, /* CFStringRef */ IntPtr name, delegate* unmanaged<IntPtr, int, IntPtr, IntPtr, IntPtr> callout, /*  CFMessagePortContext */ ContextProxy* context, byte* shouldFreeInfo);

		[DllImport (Constants.CoreFoundationLibrary)]
		static extern /* CFMessagePortRef */ IntPtr CFMessagePortCreateRemote (/* CFAllocatorRef */ IntPtr allocator, /* CFStringRef */ IntPtr name);

		[DllImport (Constants.CoreFoundationLibrary)]
		static extern void CFMessagePortInvalidate (/* CFMessagePortRef */ IntPtr ms);

		[DllImport (Constants.CoreFoundationLibrary)]
		static extern IntPtr CFMessagePortCreateRunLoopSource (/* CFAllocatorRef */ IntPtr allocator, /* CFMessagePortRef */ IntPtr local, /* CFIndex */ nint order);

		[DllImport (Constants.CoreFoundationLibrary)]
		unsafe static extern /* SInt32 */ CFMessagePortSendRequestStatus CFMessagePortSendRequest (/* CFMessagePortRef */ IntPtr remote, /* SInt32 */ int msgid, /* CFDataRef */ IntPtr data, /* CFTiemInterval */ double sendTimeout, /* CFTiemInterval */ double rcvTimeout, /* CFStringRef */ IntPtr replyMode, /* CFDataRef* */ IntPtr* returnData);

		[DllImport (Constants.CoreFoundationLibrary)]
		static extern /* Boolean */ byte CFMessagePortIsRemote (/* CFMessagePortRef */ IntPtr ms);

		[DllImport (Constants.CoreFoundationLibrary)]
		static extern /* Boolean */ byte CFMessagePortSetName (/* CFMessagePortRef */ IntPtr ms, /* CFStringRef */ IntPtr newName);

		[DllImport (Constants.CoreFoundationLibrary)]
		static extern /* CFStringRef */ IntPtr CFMessagePortGetName (/* CFMessagePortRef */ IntPtr ms);

		[DllImport (Constants.CoreFoundationLibrary)]
		static extern /* Boolean */ byte CFMessagePortIsValid (/* CFMessagePortRef */ IntPtr ms);

		[DllImport (Constants.CoreFoundationLibrary)]
		static extern void CFMessagePortSetDispatchQueue (/* CFMessagePortRef */ IntPtr ms, dispatch_queue_t queue);

		[DllImport (Constants.CoreFoundationLibrary)]
		static unsafe extern void CFMessagePortSetInvalidationCallBack (/* CFMessagePortRef */ IntPtr ms, delegate* unmanaged<IntPtr, IntPtr, void> callout);

		/// <summary>Creates a local message port that receives messages.</summary>
		/// <param name="name">The name to register, or <see langword="null" /> to create an unnamed port.</param>
		/// <param name="callback">The callback that handles received messages.</param>
		/// <param name="allocator">The allocator to use, or <see langword="null" /> to use the default allocator.</param>
		/// <returns>A local message port, or <see langword="null" /> if the port could not be created.</returns>
		/// <exception cref="ArgumentNullException"><paramref name="callback" /> is <see langword="null" />.</exception>
		/// <remarks>If another local port with the same name already exists in the process, Core Foundation returns that port and continues to use its original callback.</remarks>
		public static CFMessagePort? CreateLocalPort (string? name, CFMessagePortCallBack callback, CFAllocator? allocator = null)
		{
			if (callback is null)
				ObjCRuntime.ThrowHelper.ThrowArgumentNullException (nameof (callback));

			var n = CFString.CreateNative (name);
			byte shouldFreeInfo = 0;
			var context = new MessagePortContext (callback);
			var contextHandle = GCHandle.Alloc (context);
			var contextProxy = new ContextProxy {
				info = GCHandle.ToIntPtr (contextHandle),
			};
			unsafe {
				contextProxy.retain = &RetainProxy;
				contextProxy.release = &ReleaseProxy;
			}

			try {
				IntPtr portHandle;
				unsafe {
					portHandle = CFMessagePortCreateLocal (allocator.GetHandle (), n, &MessagePortCallback, &contextProxy, &shouldFreeInfo);
					GC.KeepAlive (allocator);
				}

				if (portHandle == IntPtr.Zero)
					return null;

				return new CFMessagePort (portHandle, true);
			} finally {
				CFString.ReleaseNative (n);
				context.Release (contextHandle);
			}
		}

		//
		// Proxy callbacks
		//
		[UnmanagedCallersOnly]
		static IntPtr RetainProxy (IntPtr info)
		{
			var context = GCHandle.FromIntPtr (info).Target as MessagePortContext;
			context?.Retain ();
			return info;
		}

		[UnmanagedCallersOnly]
		static void ReleaseProxy (IntPtr info)
		{
			var handle = GCHandle.FromIntPtr (info);
			var context = handle.Target as MessagePortContext;
			context?.Release (handle);
		}

		[UnmanagedCallersOnly]
		static IntPtr MessagePortCallback (IntPtr local, int msgid, IntPtr data, IntPtr info)
		{
			var context = GCHandle.FromIntPtr (info).Target as MessagePortContext;
			if (context is null)
				return IntPtr.Zero;

			using (var managedData = Runtime.GetNSObject<NSData> (data)) {
				if (managedData is null)
					return IntPtr.Zero;

				var result = context.Callback.Invoke (msgid, managedData);
				// System will release returned CFData
				result?.DangerousRetain ();
#pragma warning disable RBI0014
				return result.GetHandle ();
#pragma warning restore RBI0014
			}
		}

		[UnmanagedCallersOnly]
		static void MessagePortInvalidationCallback (IntPtr messagePort, IntPtr info)
		{
			Action? callback;

			lock (invalidationHandles) {
				invalidationHandles.TryGetValue (messagePort, out callback);
				invalidationHandles.Remove (messagePort);
			}

			callback?.Invoke ();
		}

		/// <summary>Creates a remote message port for sending messages to a named local port.</summary>
		/// <param name="allocator">The allocator to use, or <see langword="null" /> to use the default allocator.</param>
		/// <param name="name">The name of the local port.</param>
		/// <returns>A remote message port, or <see langword="null" /> if no valid local port with the specified name is available.</returns>
		/// <exception cref="ArgumentNullException"><paramref name="name" /> is <see langword="null" />.</exception>
		public static CFMessagePort? CreateRemotePort (CFAllocator? allocator, string name)
		{
			if (name is null)
				ObjCRuntime.ThrowHelper.ThrowArgumentNullException (nameof (name));

			var n = CFString.CreateNative (name);
			try {
				var portHandle = CFMessagePortCreateRemote (allocator.GetHandle (), n);
				GC.KeepAlive (allocator);
				return portHandle == IntPtr.Zero ? null : new CFMessagePort (portHandle, true);
			} finally {
				CFString.ReleaseNative (n);
			}
		}

		/// <summary>Invalidates the message port so that it can no longer send or receive messages.</summary>
		/// <remarks>Invalidation is permanent and does not dispose the managed object. After this method returns, <see cref="IsValid" /> is <see langword="false" />.</remarks>
		public void Invalidate ()
		{
			CFMessagePortInvalidate (GetCheckedHandle ());
		}

		/// <summary>Sends a message through a remote message port.</summary>
		/// <param name="msgid">The application-defined message identifier.</param>
		/// <param name="data">The message data, or <see langword="null" /> to send an empty message.</param>
		/// <param name="sendTimeout">The maximum number of seconds to wait while sending the message.</param>
		/// <param name="rcvTimeout">The maximum number of seconds to wait for a reply.</param>
		/// <param name="replyMode">The run loop mode in which to wait for a reply, or <see langword="null" /> if no reply is expected.</param>
		/// <param name="returnData">On return, the reply data, or <see langword="null" /> if no data was returned.</param>
		/// <returns>A value that describes whether the message was sent and, if requested, whether a reply was received.</returns>
		/// <remarks>This method is intended for remote ports. When <paramref name="replyMode" /> is <see langword="null" />, the method returns after sending and does not wait for a reply.</remarks>
		public CFMessagePortSendRequestStatus SendRequest (int msgid, NSData? data, double sendTimeout, double rcvTimeout, NSString? replyMode, out NSData? returnData)
		{
			CFMessagePortSendRequestStatus result;
			IntPtr returnDataHandle = IntPtr.Zero;
			unsafe {
				result = CFMessagePortSendRequest (GetCheckedHandle (), msgid, data.GetHandle (), sendTimeout, rcvTimeout, replyMode.GetHandle (), &returnDataHandle);
				GC.KeepAlive (data);
				GC.KeepAlive (replyMode);
			}

			// Apple's documentation says ownership of returnData follows the Create Rule.
			returnData = Runtime.GetINativeObject<NSData> (returnDataHandle, true);

			return result;
		}

		/// <summary>Creates a run loop source that delivers messages to this local port.</summary>
		/// <returns>A new run loop source that has not yet been added to a run loop.</returns>
		/// <remarks>Add the returned source to a run loop with <see cref="CFRunLoop.AddSource" />. A port cannot use both a run loop source and a dispatch queue.</remarks>
		public CFRunLoopSource CreateRunLoopSource ()
		{
			// note: order is currently ignored by CFMessagePort object run loop sources. Pass 0 for this value.
			var runLoopHandle = CFMessagePortCreateRunLoopSource (IntPtr.Zero, GetCheckedHandle (), 0);
			return new CFRunLoopSource (runLoopHandle, true);
		}

		/// <summary>Schedules this local port's callbacks on a dispatch queue.</summary>
		/// <param name="queue">The dispatch queue to use, or <see langword="null" /> to stop using the current queue.</param>
		/// <remarks>A port cannot use both a dispatch queue and a run loop source. Calling this method on a remote or invalid port has no effect.</remarks>
		public void SetDispatchQueue (DispatchQueue? queue)
		{
			CFMessagePortSetDispatchQueue (GetCheckedHandle (), queue.GetHandle ());
			GC.KeepAlive (queue);
		}
	}
}
