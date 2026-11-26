// Copyright 2014, Xamarin Inc. All rights reserved.

#nullable enable

#if !COREBUILD

using System.Threading;

using CoreFoundation;
using Foundation;

namespace ObjCRuntime {

	/// <summary>Base class used by the bindinge generator to generate Protocol Wrapper Types.</summary>
	///     <remarks>
	///       <para>This class is intended to support the binding generator, and contains some of the common idioms and patterns used for implementing a managed object that wraps an unmanaged Objective-C object.</para>
	///       <para />
	///       <para>The class provides a constructor that take a native handle, and a flag indicating whether the underlying object has already been retained by managed code or not as well as implementing the IDisposable interface which will invoke the Objective-C release method on the target when the object is no longer referenced by managed code.</para>
	///     </remarks>
	public abstract class BaseWrapper : NativeObject, IReleaseOnMainThread {
		const int Active = 0;
		const int ReleaseQueued = 1;
		const int Disposed = 2;
		int disposalState;

		protected BaseWrapper (NativeHandle handle, bool owns)
			: base (handle, owns)
		{
		}

		/// <summary>Releases the wrapper's native reference.</summary>
		/// <param name="disposing">If <see langword="true" />, releases the native reference on the calling thread. If <see langword="false" />, queues the release on the main thread.</param>
		/// <remarks>The queued release runs when the main run loop processes the disposal queue.</remarks>
		protected override void Dispose (bool disposing)
		{
			if (disposing) {
				if (Interlocked.CompareExchange (ref disposalState, Disposed, Active) == Active)
					base.Dispose (true);
				return;
			}

			if (Interlocked.CompareExchange (ref disposalState, ReleaseQueued, Active) != Active)
				return;

			if (Handle != NativeHandle.Zero) {
				NSObject.NSObject_Disposer.Add (this);
				return;
			}

			Interlocked.Exchange (ref disposalState, Disposed);
			base.Dispose (false);
		}

		void IReleaseOnMainThread.ReleaseOnMainThread ()
		{
			if (Interlocked.CompareExchange (ref disposalState, Disposed, ReleaseQueued) == ReleaseQueued)
				base.Dispose (false);
		}

		protected internal override void Retain ()
		{
			if (Handle != IntPtr.Zero)
				Messaging.void_objc_msgSend (Handle, Selector.GetHandle ("retain"));
		}

		protected internal override void Release ()
		{
			if (Handle != IntPtr.Zero)
				Messaging.void_objc_msgSend (Handle, Selector.GetHandle ("release"));
		}
	}
}

#endif
