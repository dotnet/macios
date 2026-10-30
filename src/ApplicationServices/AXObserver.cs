// Copyright (c) Microsoft Corporation.
// Licensed under the MIT License.

#nullable enable

#if __MACOS__
using System.Collections.Generic;
using CoreFoundation;

namespace ApplicationServices {
	/// <summary>Handles a notification from an application's accessibility element.</summary>
	/// <param name="observer">The observer receiving the notification.</param>
	/// <param name="element">The element that posted the notification, borrowed for the duration of the callback.</param>
	/// <param name="notification">The notification name.</param>
	/// <param name="refcon">The application-defined value passed when registering the notification.</param>
	public delegate void AXObserverCallback (AXObserver observer, AXUIElement element, string notification, IntPtr refcon);

	/// <summary>Handles an accessibility notification with an information dictionary.</summary>
	/// <param name="observer">The observer receiving the notification.</param>
	/// <param name="element">The element that posted the notification, borrowed for the duration of the callback.</param>
	/// <param name="notification">The notification name.</param>
	/// <param name="info">Additional notification information, borrowed for the duration of the callback.</param>
	/// <param name="refcon">The application-defined value passed when registering the notification.</param>
	public delegate void AXObserverCallbackWithInfo (AXObserver observer, AXUIElement element, string notification, NSDictionary info, IntPtr refcon);

	/// <summary>Receives accessibility notifications from an application.</summary>
	/// <remarks>
	///   <para>Add the source returned by <see cref="GetRunLoopSource" /> to a run loop before receiving notifications. Keep the observer alive while notifications are needed.</para>
	///   <para>Disposing the observer removes its native run loop source. Dispose on the notification run loop's thread, and do not let exceptions escape from callbacks.</para>
	/// </remarks>
	[SupportedOSPlatform ("macos")]
	[UnsupportedOSPlatform ("ios")]
	[UnsupportedOSPlatform ("tvos")]
	[UnsupportedOSPlatform ("maccatalyst")]
	public class AXObserver : NativeObject {
		static readonly Dictionary<IntPtr, WeakReference<AXObserver>> observers = new Dictionary<IntPtr, WeakReference<AXObserver>> ();
		AXObserverCallback? callback;
		AXObserverCallbackWithInfo? infoCallback;

		[Preserve (Conditional = true)]
		internal AXObserver (NativeHandle handle, bool owns) : base (handle, owns)
		{
		}

		[DllImport (Constants.ApplicationServicesLibrary)]
		extern static nuint AXObserverGetTypeID ();

		/// <summary>Gets the Core Foundation type identifier of accessibility observers.</summary>
		/// <returns>The native type identifier.</returns>
		public static nuint GetTypeID () => AXObserverGetTypeID ();

		[DllImport (Constants.ApplicationServicesLibrary)]
		unsafe extern static AXError AXObserverCreate (int application, delegate* unmanaged<IntPtr, IntPtr, IntPtr, IntPtr, void> callback, IntPtr* observer);

		/// <summary>Creates an observer for an application.</summary>
		/// <param name="pid">The application's process identifier.</param>
		/// <param name="callback">The notification handler.</param>
		/// <param name="observer">The owned observer, or <see langword="null" /> on failure.</param>
		/// <returns>The native result code.</returns>
		public static unsafe AXError Create (int pid, AXObserverCallback callback, out AXObserver? observer)
		{
			if (callback is null)
				ObjCRuntime.ThrowHelper.ThrowArgumentNullException (nameof (callback));

			var handle = IntPtr.Zero;
			var error = AXObserverCreate (pid, &NotificationCallback, &handle);
			observer = null;
			if (error == AXError.Success) {
				observer = new AXObserver (handle, true) { callback = callback };
				lock (observers)
					observers.Add (handle, new WeakReference<AXObserver> (observer));
			}
			return error;
		}

		[DllImport (Constants.ApplicationServicesLibrary)]
		unsafe extern static AXError AXObserverCreateWithInfoCallback (int application, delegate* unmanaged<IntPtr, IntPtr, IntPtr, IntPtr, IntPtr, void> callback, IntPtr* observer);

		/// <summary>Creates an observer that receives notification information dictionaries.</summary>
		/// <param name="pid">The application's process identifier.</param>
		/// <param name="callback">The notification handler.</param>
		/// <param name="observer">The owned observer, or <see langword="null" /> on failure.</param>
		/// <returns>The native result code.</returns>
		public static unsafe AXError CreateWithInfoCallback (int pid, AXObserverCallbackWithInfo callback, out AXObserver? observer)
		{
			if (callback is null)
				ObjCRuntime.ThrowHelper.ThrowArgumentNullException (nameof (callback));

			var handle = IntPtr.Zero;
			var error = AXObserverCreateWithInfoCallback (pid, &NotificationWithInfoCallback, &handle);
			observer = null;
			if (error == AXError.Success) {
				observer = new AXObserver (handle, true) { infoCallback = callback };
				lock (observers)
					observers.Add (handle, new WeakReference<AXObserver> (observer));
			}
			return error;
		}

		static AXObserver? FindObserver (IntPtr handle)
		{
			lock (observers) {
				if (observers.TryGetValue (handle, out var reference) && reference.TryGetTarget (out var observer))
					return observer;
			}
			return null;
		}

		[UnmanagedCallersOnly]
		static void NotificationCallback (IntPtr observer, IntPtr element, IntPtr notification, IntPtr refcon)
		{
			var target = FindObserver (observer);
			if (target?.callback is null)
				return;
			using var source = new AXUIElement (element, false);
			target.callback (target, source, CFString.FromHandle (notification) ?? throw new InvalidOperationException ("The accessibility notification has no name."), refcon);
		}

		[UnmanagedCallersOnly]
		static void NotificationWithInfoCallback (IntPtr observer, IntPtr element, IntPtr notification, IntPtr info, IntPtr refcon)
		{
			var target = FindObserver (observer);
			if (target?.infoCallback is null)
				return;
			using var dictionary = new NSDictionary (info, false);
			using var source = new AXUIElement (element, false);
			target.infoCallback (target, source, CFString.FromHandle (notification) ?? throw new InvalidOperationException ("The accessibility notification has no name."), dictionary, refcon);
		}

		[DllImport (Constants.ApplicationServicesLibrary)]
		extern static AXError AXObserverAddNotification (IntPtr observer, IntPtr element, IntPtr notification, IntPtr refcon);

		/// <summary>Registers for a notification from an element.</summary>
		/// <param name="element">The element to observe.</param>
		/// <param name="notification">The notification name.</param>
		/// <param name="refcon">An application-defined value passed to the callback.</param>
		/// <returns>The native result code.</returns>
		public AXError AddNotification (AXUIElement element, string notification, IntPtr refcon = default)
		{
			using var name = new CFString (notification);
			var error = AXObserverAddNotification (GetCheckedHandle (), element.GetNonNullHandle (nameof (element)), name.Handle, refcon);
			GC.KeepAlive (element);
			GC.KeepAlive (this);
			return error;
		}

		[DllImport (Constants.ApplicationServicesLibrary)]
		extern static AXError AXObserverRemoveNotification (IntPtr observer, IntPtr element, IntPtr notification);

		/// <summary>Unregisters a notification from an element.</summary>
		/// <param name="element">The observed element.</param>
		/// <param name="notification">The notification name.</param>
		/// <returns>The native result code.</returns>
		public AXError RemoveNotification (AXUIElement element, string notification)
		{
			using var name = new CFString (notification);
			var error = AXObserverRemoveNotification (GetCheckedHandle (), element.GetNonNullHandle (nameof (element)), name.Handle);
			GC.KeepAlive (element);
			GC.KeepAlive (this);
			return error;
		}

		[DllImport (Constants.ApplicationServicesLibrary)]
		extern static IntPtr AXObserverGetRunLoopSource (IntPtr observer);

		/// <summary>Gets the run loop source used to deliver notifications.</summary>
		/// <returns>A retained wrapper for the source. The caller must dispose this wrapper.</returns>
		public CFRunLoopSource GetRunLoopSource ()
		{
			var source = new CFRunLoopSource (AXObserverGetRunLoopSource (GetCheckedHandle ()), false);
			GC.KeepAlive (this);
			return source;
		}

		/// <inheritdoc />
		protected override void Dispose (bool disposing)
		{
			lock (observers) {
				if (observers.TryGetValue (Handle, out var reference) && (!reference.TryGetTarget (out var observer) || ReferenceEquals (observer, this)))
					observers.Remove (Handle);
			}
			base.Dispose (disposing);
		}
	}
}
#endif
