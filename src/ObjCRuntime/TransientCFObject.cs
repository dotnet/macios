using System.ComponentModel;

using CoreFoundation;

#nullable enable

namespace ObjCRuntime {
	/// <summary>Owns a temporary Core Foundation handle and releases it when disposed.</summary>
	/// <remarks>
	///   <para>This type is for internal use by generated bindings and native interop code.</para>
	///   <para>Always declare the holder with <c>using</c> to release its native resources. Omitting disposal leaks the owned handle.</para>
	/// </remarks>
	/// <example>
	///   <code>
	///     using var cfobj = new TransientCFObject (CFArray.Create ("a", "b"));
	///     // Pass cfobj to the native call.
	///   </code>
	/// </example>
	[EditorBrowsable (EditorBrowsableState.Never)]
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
