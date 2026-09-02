// 
// BackingField.cs:
//
//  Helper to convert between an IntPtr and a non-'generator'd managed 
//  representation with intelligent caching of the managed rep.  
//  This allows repeated calls to a property getter to avoid recreating new
//  instances of the managed representation:
//
//    ABNewPersonViewController a = ...;
//    var p = a.DisplayedPerson;  // OK; may create a new instance.
//    p = a.DisplayedPerson;      // OK; returns previous version IFF the
//                                //     handle hasn't changed in the meantime.
//    p = GetPerson();
//    a.DisplayedPerson = p;      // OK; flushes handle
//
// This can be supported by following a simple pattern:
//
//   class ABNewPersonViewController {
//      ABPerson displayedPerson;
//      public ABPerson DisplayedPerson {
//        get {
//			MarkDirty ();
//			return BackingField.Get (ref displayedPerson, _DisplayedPerson, h => new ABPerson (h));
//		}
//        set {
//			_DisplayedPerson = BackingField.Save (ref displayedPerson, value);
//			MarkDirty ();
//		  }
//      }
//   }
//
// Authors: Mono Team
//     
// Copyright (C) 2009 Novell, Inc
// Copyright 2011 Xamarin Inc. All rights reserved
//

#if IOS

#nullable enable

namespace ObjCRuntime {

	/// <summary>Helpers to convert between native handles and cached managed wrappers.</summary>
	static class BackingField {
		/// <summary>Gets a cached managed wrapper or creates a new one from a native handle.</summary>
		/// <typeparam name="T">The managed wrapper type.</typeparam>
		/// <param name="value">The cached managed value.</param>
		/// <param name="handle">The native handle.</param>
		/// <param name="c">A converter that creates a managed wrapper from a native handle.</param>
		/// <returns>The cached or newly created managed value, or <see langword="null"/> if <paramref name="handle"/> is <see cref="IntPtr.Zero"/>.</returns>
		public static T? Get<T> (ref T? value, IntPtr handle, Converter<IntPtr, T?> c)
			where T : class, INativeObject, IDisposable
		{
			c = ThrowHelper.ThrowIfNull (c);

			if (handle == IntPtr.Zero) {
				if (value is not null)
					value.Dispose ();
				return value = null;
			}
			if (value is not null) {
				if (handle == value.Handle)
					return value;
				value.Dispose ();
			}
			return value = c (handle);
		}

		/// <summary>Saves a managed wrapper in a backing field and returns the corresponding native handle.</summary>
		/// <typeparam name="T">The managed wrapper type.</typeparam>
		/// <param name="value">The current cached managed value.</param>
		/// <param name="newValue">The new managed value to cache.</param>
		/// <returns>The native handle for <paramref name="newValue"/>, or <see cref="IntPtr.Zero"/> if <paramref name="newValue"/> is <see langword="null"/>.</returns>
		public static IntPtr Save<T> (ref T? value, T? newValue)
			where T : class, INativeObject, IDisposable
		{
#pragma warning disable RBI0014
			if (object.ReferenceEquals (value, newValue))
				return value is null ? IntPtr.Zero : value.Handle;
			if (value is not null)
				value.Dispose ();
			value = newValue;
			return value is null ? IntPtr.Zero : value.Handle;
#pragma warning restore RBI0014
		}
	}
}

#endif // IOS
