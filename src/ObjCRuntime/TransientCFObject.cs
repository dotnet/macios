
using CoreFoundation;

#nullable enable

namespace ObjCRuntime {
	// a short-lived holder for a CFObject handle for native interop, the pointer will be released when holder is disposed.
	// typical usage:
	// using var cfobj = CFArray.Create ("a", "b");
	// SomePInvoke (cfobj);
	//
	/// <summary>Owns a temporary Core Foundation handle and releases it when disposed.</summary>
	public ref struct TransientCFObject {
#if !COREBUILD
		IntPtr ptr;

		/// <summary>Takes ownership of a native Core Foundation handle.</summary>
		/// <param name="ptr">The owned handle, or <see cref="IntPtr.Zero" />.</param>
		public TransientCFObject (IntPtr ptr)
		{
			this.ptr = ptr;
		}

		/// <summary>Releases the owned handle. Repeated calls have no effect.</summary>
		public void Dispose ()
		{
			CFObject.SafeRelease (ptr);
			ptr = IntPtr.Zero;
		}

		/// <summary>Gets the underlying native pointer without transferring ownership.</summary>
		/// <param name="obj">The temporary handle holder.</param>
		/// <returns>The native pointer.</returns>
		public static implicit operator IntPtr (TransientCFObject obj) => obj.ptr;

		/// <summary>Gets the underlying native handle without transferring ownership.</summary>
		/// <param name="obj">The temporary handle holder.</param>
		/// <returns>The native handle.</returns>
		public static implicit operator NativeHandle (TransientCFObject obj) => obj.ptr;
#endif
	}
}
