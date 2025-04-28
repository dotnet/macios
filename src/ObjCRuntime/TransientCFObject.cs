using System;
using System.Runtime.InteropServices;

using CoreFoundation;

#nullable enable

namespace ObjCRuntime {
	// a short-lived holder for a CFObject handle for native interop, the pointer will be released when holder is disposed.
	// typical usage:
	// using var cfobj = CFArray.Create ("a", "b");
	// SomePInvoke (cfobj);
	//
	internal ref struct TransientCFObject {
#if !COREBUILD
		IntPtr ptr;

		public TransientCFObject (IntPtr ptr)
		{
			this.ptr = ptr;
		}

		public void Dispose ()
		{
			CFObject.SafeRelease (ptr);
		}

		public static implicit operator IntPtr (TransientCFObject obj) => obj.ptr;
		public static implicit operator NativeHandle (TransientCFObject obj) => obj.ptr;
#endif
	}
}
