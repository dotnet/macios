#nullable enable

using System.Runtime.CompilerServices;

namespace ObjCRuntime {

	/// <summary>A simple interface that is used to expose the unmanaged object pointer in various classes in Xamarin.iOS.</summary>
	///     <remarks>
	///       <para>All this interface requires is for a class to expose an IntPtr that points to the unmanaged pointer to the actual object.</para>
	///     </remarks>
	public interface INativeObject {
#if !COREBUILD
		/// <summary>Handle (pointer) to the unmanaged object representation.</summary>
		///         <value>A pointer</value>
		///         <remarks>
		///           <para>This IntPtr is a handle to the underlying unmanaged representation for this object.</para>
		///         </remarks>
		NativeHandle Handle {
			get;
		}
#endif

		// The method will be implemented via custom linker step if the managed static registrar is used
		// for classes which have an (NativeHandle, bool) or (IntPtr, bool) constructor.
		// This method will be made public when the managed static registrar is used.
		[MethodImpl (MethodImplOptions.NoInlining)]
		internal static virtual INativeObject? _Xamarin_ConstructINativeObject (NativeHandle handle, bool owns) => null;
	}

	/// <summary>Provides a factory for wrapping a native handle without constructor reflection.</summary>
	/// <typeparam name="T">The managed wrapper type created by the factory.</typeparam>
	/// <remarks>Implement this interface with the wrapper's own type as <typeparamref name="T"/>. Derived wrappers must implement their own closed interface to use self-typed factory APIs.</remarks>
	public interface ICreatableNativeObject<T> : INativeObject where T : INativeObject, ICreatableNativeObject<T> {
		/// <summary>Creates a managed wrapper for a native handle.</summary>
		/// <param name="handle">The nonzero native handle to wrap.</param>
		/// <param name="owns">Whether ownership of a native reference is transferred to the wrapper.</param>
		/// <returns>A new wrapper of type <typeparamref name="T"/>.</returns>
#if COREBUILD
		// Corebuild wrappers can omit their native constructors and factory bodies.
		static virtual T Create (NativeHandle handle, bool owns) => throw new NotImplementedException ();
#else
		static abstract T Create (NativeHandle handle, bool owns);
#endif
	}

#if !COREBUILD
	public static class NativeObjectExtensions {

#pragma warning disable RBI0014
		// help to avoid the (too common pattern)
		// 	var p = x is null ? IntPtr.Zero : x.Handle;
		static public NativeHandle GetHandle (this INativeObject? self)
		{
			return self is null ? NativeHandle.Zero : self.Handle;
		}

		static public NativeHandle GetNonNullHandle (this INativeObject self, string argumentName)
		{
			if (self is null)
				ThrowHelper.ThrowArgumentNullException (argumentName);
			if (self.Handle == NativeHandle.Zero)
				ThrowHelper.ThrowObjectDisposedException (self);
			return self.Handle;
		}

#pragma warning restore RBI0014

		internal static void CallWithPointerToFirstElementAndCount<T> (T [] array, string arrayVariableName, Action<IntPtr, nuint> callback)
			where T : INativeObject
		{
			if (array is null)
				ObjCRuntime.ThrowHelper.ThrowArgumentNullException (arrayVariableName);

			var handles = new IntPtr [array.Length];
			for (var i = 0; i < handles.Length; i++)
				handles [i] = array [i].GetNonNullHandle (arrayVariableName + $"[{i}]");

			unsafe {
				fixed (IntPtr* handlesPtr = handles) {
					callback ((IntPtr) handlesPtr, (nuint) handles.Length);
				}
			}

			GC.KeepAlive (array);
		}
	}
#endif
}
