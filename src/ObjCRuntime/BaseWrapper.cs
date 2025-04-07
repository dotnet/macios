// Copyright 2014, Xamarin Inc. All rights reserved.

#nullable enable

#if !COREBUILD

using System;
using System.Runtime.InteropServices;
using Foundation;
using CoreFoundation;

#if !NET
using NativeHandle = System.IntPtr;
#endif

namespace ObjCRuntime {

	/// <summary>Base class used by the bindinge generator to generate Protocol Wrapper Types.</summary>
	///     <remarks>
	///       <para>This class is intended to support the binding generator, and contains some of the common idioms and patterns used for implementing a managed object that wraps an unmanaged Objective-C object.</para>
	///       <para />
	///       <para>The class provides a constructor that take a native handle, and a flag indicating whether the underlying object has already been retained by managed code or not as well as implementing the IDisposable interface which will invoke the Objective-C release method on the target when the object is no longer referenced by managed code.</para>
	///     </remarks>
	public abstract class BaseWrapper : NativeObject {

#if NET
		protected BaseWrapper (NativeHandle handle, bool owns)
#else
		public BaseWrapper (NativeHandle handle, bool owns)
#endif
			: base (handle, owns)
		{
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
