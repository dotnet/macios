// Copyright 2014, Xamarin Inc. All rights reserved.

#nullable enable

#if !COREBUILD

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
		protected BaseWrapper (NativeHandle handle, bool owns)
			: base (handle, owns)
		{
		}

		protected override void Dispose (bool disposing)
		{
			if (!disposing && Handle != NativeHandle.Zero) {
				NSObject.NSObject_Disposer.Add (this);
				return;
			}
			base.Dispose (disposing);
		}

		void IReleaseOnMainThread.ReleaseOnMainThread ()
		{
			if (Handle != NativeHandle.Zero) {
				Release ();
				ClearHandle ();
			}
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
