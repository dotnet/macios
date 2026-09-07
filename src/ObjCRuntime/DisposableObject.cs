//
// DisposableObject.cs: A base class for many native data types without assuming any particular lifecycle.

// Authors:
//   Rolf Bjarne Kvinge
//
// Copyright 2021 Microsoft Corp
//

#nullable enable

namespace ObjCRuntime {
	//
	// The DisposableObject class is intended to be a base class for many native data
	// data types, without assuming any particular lifecycle (might be reference counted,
	// might not be).
	//
	// It provides the common boilerplate for this kind of objects and the Dispose
	// pattern.
	//
	public abstract class DisposableObject : INativeObject, IDisposable {
		NativeHandle handle;
		readonly bool owns;

		public NativeHandle Handle {
			get => handle;
			protected set => InitializeHandle (value);
		}

		protected bool Owns { get => owns; }

		protected DisposableObject ()
		{
		}

		protected DisposableObject (NativeHandle handle, bool owns)
			: this (handle, owns, true)
		{
		}

		protected DisposableObject (NativeHandle handle, bool owns, bool verify)
		{
			InitializeHandle (handle, verify);
			this.owns = owns;
		}

		~DisposableObject ()
		{
			Dispose (false);
		}

		/// <summary>Releases all resources used by this object.</summary>
		/// <remarks>
		///   <para>This method releases the resources used by this object, and marks the object as disposed. Once developers have invoked the <see cref="Dispose()" /> method, the object is no longer useful and developers should no longer make any calls to it.</para>
		///   <para>Calling this method when the application is finished using this object ensures that all external resources used by this object are released as soon as possible.</para>
		/// </remarks>
		public void Dispose ()
		{
			Dispose (true);
			GC.SuppressFinalize (this);
		}

		/// <summary>Releases the resources used by this object.</summary>
		/// <param name="disposing">If set to <see langword="true" />, the method is invoked directly and disposes managed and unmanaged resources. If set to <see langword="false" />, the method is called by the garbage collector finalizer and should only release unmanaged resources.</param>
		/// <remarks>
		///   <para>This method is called by both the <see cref="Dispose()" /> method and the object finalizer. When invoked by the <see cref="Dispose()" /> method, the <paramref name="disposing" /> parameter is set to <see langword="true" />. When invoked by the object finalizer, on the finalizer thread, the <paramref name="disposing" /> parameter is set to <see langword="false" />.</para>
		///   <para>Subclasses should override this method to release any resources they hold, and then call the base implementation.</para>
		/// </remarks>
		protected virtual void Dispose (bool disposing)
		{
			handle = NativeHandle.Zero;
		}

		protected void ClearHandle ()
		{
			handle = NativeHandle.Zero;
		}

		void InitializeHandle (NativeHandle handle, bool verify)
		{
#if !COREBUILD
			if (verify && handle == NativeHandle.Zero && Class.ThrowOnInitFailure) {
				throw new Exception ($"Could not initialize an instance of the type '{GetType ().FullName}': handle is null.\n{Constants.SetThrowOnInitFailureToFalse}.");
			}
#endif
			this.handle = handle;
		}

		protected virtual void InitializeHandle (NativeHandle handle)
		{
			InitializeHandle (handle, true);
		}

		public NativeHandle GetCheckedHandle ()
		{
			var h = handle;
			if (h == NativeHandle.Zero)
				ObjCRuntime.ThrowHelper.ThrowObjectDisposedException (this);
			return h;
		}

		public override int GetHashCode ()
		{
			return handle.GetHashCode ();
		}

		public override bool Equals (object? obj)
		{
			if (obj is DisposableObject d)
				return handle.Equals (d.handle);

			return false;
		}

		public static bool operator == (DisposableObject? a, DisposableObject? b)
		{
			if (a is null)
				return b is null;
			else if (b is null)
				return false;
			return a.Handle == b.Handle;
		}

		public static bool operator != (DisposableObject? a, DisposableObject? b)
		{
			if (a is null)
				return b is not null;
			else if (b is null)
				return true;
			return a.Handle != b.Handle;
		}
	}
}
