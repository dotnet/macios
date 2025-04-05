//
// Copyright 2010, Novell, Inc.
// Copyright 2011, 2012 Xamarin Inc
//
// Permission is hereby granted, free of charge, to any person obtaining
// a copy of this software and associated documentation files (the
// "Software"), to deal in the Software without restriction, including
// without limitation the rights to use, copy, modify, merge, publish,
// distribute, sublicense, and/or sell copies of the Software, and to
// permit persons to whom the Software is furnished to do so, subject to
// the following conditions:
// 
// The above copyright notice and this permission notice shall be
// included in all copies or substantial portions of the Software.
// 
// THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND,
// EXPRESS OR IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF
// MERCHANTABILITY, FITNESS FOR A PARTICULAR PURPOSE AND
// NONINFRINGEMENT. IN NO EVENT SHALL THE AUTHORS OR COPYRIGHT HOLDERS BE
// LIABLE FOR ANY CLAIM, DAMAGES OR OTHER LIABILITY, WHETHER IN AN ACTION
// OF CONTRACT, TORT OR OTHERWISE, ARISING FROM, OUT OF OR IN CONNECTION
// WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN THE SOFTWARE.
//
using System;
using System.Collections;
using System.Collections.Generic;
using ObjCRuntime;

#if !NET
using NativeHandle = System.IntPtr;
#endif

// Disable until we get around to enable + fix any issues.
#nullable disable

namespace Foundation {

	public partial class NSDictionary : NSObject, IDictionary, IDictionary<NSObject, NSObject> {
		/// <param name="first">First key.</param>
		///         <param name="second">First value.</param>
		///         <param name="args">Remaining pais of keys and values.</param>
		///         <summary>Creates an NSDictionary from a list of NSObject keys and NSObject values.</summary>
		///         <remarks>
		///           <para>
		/// 	    The list of keys and values are used to create the dictionary.   The number of parameters passed to this function must be even.
		/// 	  </para>
		///           <example>
		///             <code lang="csharp lang-csharp"><![CDATA[
		/// var key1 = new NSString ("key1");
		/// var value1 = new NSNumber ((byte) 1);
		/// var key2 = new NSString ("key2");
		/// var value2 = new NSNumber ((byte) 2);
		///
		/// var dict2 = new NSDictionary (key1, value1, key2, value2);
		/// ]]></code>
		///           </example>
		///         </remarks>
		public NSDictionary (NSObject first, NSObject second, params NSObject [] args) : this (PickOdd (second, args), PickEven (first, args))
		{
		}

		/// <param name="first">First key.</param>
		///         <param name="second">First value.</param>
		///         <param name="args">Remaining pais of keys and values.</param>
		///         <summary>Creates an NSDictionary from a list of keys and values.</summary>
		///         <remarks>
		///           <para>
		/// 	    Each C# object is boxed as an NSObject by calling <see cref="M:Foundation.NSObject.FromObject(System.Object)" />.
		/// 	  </para>
		///           <para>
		/// 	    The list of keys and values are used to create the dictionary.   The number of parameters passed to this function must be even.
		/// 	  </para>
		///           <example>
		///             <code lang="csharp lang-csharp"><![CDATA[
		/// //
		/// // Using C# objects, strings and ints, produces
		/// // a dictionary with 2 NSString keys, "key1" and "key2"
		/// // and two NSNumbers with the values 1 and 2
		/// //
		/// var dict = new NSDictionary ("key1", 1, "key2", 2);
		/// ]]></code>
		///           </example>
		///         </remarks>
		public NSDictionary (object first, object second, params object [] args) : this (PickOdd (second, args), PickEven (first, args))
		{
		}

		internal NSDictionary (NativeHandle handle, bool owns) : base (handle, owns)
		{
		}

		internal static NSArray PickEven (NSObject f, NSObject [] args)
		{
			int al = args.Length;
			if ((al % 2) != 0)
				throw new ArgumentException ("The arguments to NSDictionary should be a multiple of two", "args");
			var ret = new NSObject [1 + al / 2];
			ret [0] = f;
			for (int i = 0, target = 1; i < al; i += 2)
				ret [target++] = args [i];
			return NSArray.FromNSObjects (ret);
		}

		internal static NSArray PickOdd (NSObject f, NSObject [] args)
		{
			var ret = new NSObject [1 + args.Length / 2];
			ret [0] = f;
			for (int i = 1, target = 1; i < args.Length; i += 2)
				ret [target++] = args [i];
			return NSArray.FromNSObjects (ret);
		}

		internal static NSArray PickEven (object f, object [] args)
		{
			int al = args.Length;
			if ((al % 2) != 0)
				throw new ArgumentException ("The arguments to NSDictionary should be a multiple of two", "args");
			var ret = new object [1 + al / 2];
			ret [0] = f;
			for (int i = 0, target = 1; i < al; i += 2)
				ret [target++] = args [i];
			return NSArray.FromObjects (ret);
		}

		internal static NSArray PickOdd (object f, object [] args)
		{
			var ret = new object [1 + args.Length / 2];
			ret [0] = f;
			for (int i = 1, target = 1; i < args.Length; i += 2)
				ret [target++] = args [i];
			return NSArray.FromObjects (ret);
		}

		/// <param name="objects">Array of values for the dictionary.</param>
		///         <param name="keys">Array of keys for the dictionary.</param>
		///         <summary>Creates a dictionary from a set of values and keys.</summary>
		///         <returns>
		///         </returns>
		///         <remarks>
		///         </remarks>
		public static NSDictionary FromObjectsAndKeys (NSObject [] objects, NSObject [] keys)
		{
			if (objects is null)
				throw new ArgumentNullException (nameof (objects));
			if (keys is null)
				throw new ArgumentNullException (nameof (keys));
			if (objects.Length != keys.Length)
				throw new ArgumentException (nameof (objects) + " and " + nameof (keys) + " arrays have different sizes");

			using (var no = NSArray.FromNSObjects (objects))
			using (var nk = NSArray.FromNSObjects (keys))
				return FromObjectsAndKeysInternal (no, nk);
		}

		/// <param name="objects">Array of values for the dictionary.</param>
		///         <param name="keys">Array of keys for the dictionary.</param>
		///         <summary>Creates a dictionary from a set of values and keys.</summary>
		///         <returns>
		///         </returns>
		///         <remarks>
		///           <para>
		/// 	    The keys and values will first be boxed into
		/// 	    NSObjects using <see cref="M:Foundation.NSObject.FromObject(System.Object)" />.
		/// 	  </para>
		///         </remarks>
		public static NSDictionary FromObjectsAndKeys (object [] objects, object [] keys)
		{
			if (objects is null)
				throw new ArgumentNullException (nameof (objects));
			if (keys is null)
				throw new ArgumentNullException (nameof (keys));
			if (objects.Length != keys.Length)
				throw new ArgumentException (nameof (objects) + " and " + nameof (keys) + " arrays have different sizes");

			using (var no = NSArray.FromObjects (objects))
			using (var nk = NSArray.FromObjects (keys))
				return FromObjectsAndKeysInternal (no, nk);
		}

		public static NSDictionary FromObjectsAndKeys (NSObject [] objects, NSObject [] keys, nint count)
		{
			if (objects is null)
				throw new ArgumentNullException (nameof (objects));
			if (keys is null)
				throw new ArgumentNullException (nameof (keys));
			if (objects.Length != keys.Length)
				throw new ArgumentException (nameof (objects) + " and " + nameof (keys) + " arrays have different sizes");
			if (count < 1 || objects.Length < count || keys.Length < count)
				throw new ArgumentException ("count");

			using (var no = NSArray.FromNativeObjects (objects, count))
			using (var nk = NSArray.FromNativeObjects (keys, count))
				return FromObjectsAndKeysInternal (no, nk);
		}

		public static NSDictionary FromObjectsAndKeys (object [] objects, object [] keys, nint count)
		{
			if (objects is null)
				throw new ArgumentNullException (nameof (objects));
			if (keys is null)
				throw new ArgumentNullException (nameof (keys));
			if (objects.Length != keys.Length)
				throw new ArgumentException (nameof (objects) + " and " + nameof (keys) + " arrays have different sizes");
			if (count < 1 || objects.Length < count || keys.Length < count)
				throw new ArgumentException ("count");

			using (var no = NSArray.FromObjects (count, objects))
			using (var nk = NSArray.FromObjects (count, keys))
				return FromObjectsAndKeysInternal (no, nk);
		}

		internal bool ContainsKeyValuePair (KeyValuePair<NSObject, NSObject> pair)
		{
			NSObject value;
			if (!TryGetValue (pair.Key, out value))
				return false;

			return EqualityComparer<NSObject>.Default.Equals (pair.Value, value);
		}

		#region ICollection
		/// <param name="array">To be added.</param>
		///         <param name="arrayIndex">To be added.</param>
		///         <summary>To be added.</summary>
		///         <remarks>To be added.</remarks>
		void ICollection.CopyTo (Array array, int arrayIndex)
		{
			if (array is null)
				throw new ArgumentNullException (nameof (array));
			if (arrayIndex < 0)
				throw new ArgumentOutOfRangeException (nameof (arrayIndex));
			if (array.Rank > 1)
				throw new ArgumentException (nameof (array) + " is multidimensional");
			if ((array.Length > 0) && (arrayIndex >= array.Length))
				throw new ArgumentException (nameof (arrayIndex) + " is equal to or greater than " + nameof (array) + ".Length");
			if (arrayIndex + (int) Count > array.Length)
				throw new ArgumentException ("Not enough room from " + nameof (arrayIndex) + " to end of " + nameof (array) + " for this Hashtable");
			IDictionaryEnumerator e = ((IDictionary) this).GetEnumerator ();
			int i = arrayIndex;
			while (e.MoveNext ())
				array.SetValue (e.Entry, i++);
		}

		/// <summary>To be added.</summary>
		///         <value>To be added.</value>
		///         <remarks>To be added.</remarks>
		int ICollection.Count {
			get { return (int) Count; }
		}

		/// <summary>To be added.</summary>
		///         <value>To be added.</value>
		///         <remarks>To be added.</remarks>
		bool ICollection.IsSynchronized {
			get { return false; }
		}

		/// <summary>To be added.</summary>
		///         <value>To be added.</value>
		///         <remarks>To be added.</remarks>
		object ICollection.SyncRoot {
			get { return this; }
		}
		#endregion

		#region ICollection<KeyValuePair<NSObject, NSObject>>
		void ICollection<KeyValuePair<NSObject, NSObject>>.Add (KeyValuePair<NSObject, NSObject> item)
		{
			throw new NotSupportedException ();
		}

		void ICollection<KeyValuePair<NSObject, NSObject>>.Clear ()
		{
			throw new NotSupportedException ();
		}

		bool ICollection<KeyValuePair<NSObject, NSObject>>.Contains (KeyValuePair<NSObject, NSObject> keyValuePair)
		{
			return ContainsKeyValuePair (keyValuePair);
		}

		void ICollection<KeyValuePair<NSObject, NSObject>>.CopyTo (KeyValuePair<NSObject, NSObject> [] array, int index)
		{
			if (array is null)
				throw new ArgumentNullException (nameof (array));
			if (index < 0)
				throw new ArgumentOutOfRangeException (nameof (index));
			// we want no exception for index==array.Length && Count == 0
			if (index > array.Length)
				throw new ArgumentException (nameof (index) + " larger than largest valid index of " + nameof (array));
			if (array.Length - index < (int) Count)
				throw new ArgumentException ("Destination array cannot hold the requested elements!");

			var e = GetEnumerator ();
			while (e.MoveNext ())
				array [index++] = e.Current;
		}

		bool ICollection<KeyValuePair<NSObject, NSObject>>.Remove (KeyValuePair<NSObject, NSObject> keyValuePair)
		{
			throw new NotSupportedException ();
		}

		int ICollection<KeyValuePair<NSObject, NSObject>>.Count {
			get { return (int) Count; }
		}

		bool ICollection<KeyValuePair<NSObject, NSObject>>.IsReadOnly {
			get { return true; }
		}
		#endregion

		#region IDictionary

		/// <param name="key">To be added.</param>
		///         <param name="value">To be added.</param>
		///         <summary>To be added.</summary>
		///         <remarks>To be added.</remarks>
		void IDictionary.Add (object key, object value)
		{
			throw new NotSupportedException ();
		}

		/// <summary>To be added.</summary>
		///         <remarks>To be added.</remarks>
		void IDictionary.Clear ()
		{
			throw new NotSupportedException ();
		}

		/// <param name="key">To be added.</param>
		///         <summary>To be added.</summary>
		///         <returns>To be added.</returns>
		///         <remarks>To be added.</remarks>
		bool IDictionary.Contains (object key)
		{
			if (key is null)
				throw new ArgumentNullException (nameof (key));
			NSObject _key = key as NSObject;
			if (_key is null)
				return false;
			return ContainsKey (_key);
		}

		/// <summary>To be added.</summary>
		///         <returns>To be added.</returns>
		///         <remarks>To be added.</remarks>
		IDictionaryEnumerator IDictionary.GetEnumerator ()
		{
			return (IDictionaryEnumerator) ((IEnumerable<KeyValuePair<NSObject, NSObject>>) this).GetEnumerator ();
		}

		/// <param name="key">To be added.</param>
		///         <summary>To be added.</summary>
		///         <remarks>To be added.</remarks>
		void IDictionary.Remove (object key)
		{
			throw new NotSupportedException ();
		}

		/// <summary>To be added.</summary>
		///         <value>To be added.</value>
		///         <remarks>To be added.</remarks>
		bool IDictionary.IsFixedSize {
			get { return true; }
		}

		/// <summary>To be added.</summary>
		///         <value>To be added.</value>
		///         <remarks>To be added.</remarks>
		bool IDictionary.IsReadOnly {
			get { return true; }
		}

		object IDictionary.this [object key] {
			get {
				NSObject _key = key as NSObject;
				if (_key is null)
					return null;
				return ObjectForKey (_key);
			}
			set {
				throw new NotSupportedException ();
			}
		}

		/// <summary>To be added.</summary>
		///         <value>To be added.</value>
		///         <remarks>To be added.</remarks>
		ICollection IDictionary.Keys {
			get { return Keys; }
		}

		/// <summary>To be added.</summary>
		///         <value>To be added.</value>
		///         <remarks>To be added.</remarks>
		ICollection IDictionary.Values {
			get { return Values; }
		}

		#endregion

		#region IDictionary<NSObject, NSObject>

		void IDictionary<NSObject, NSObject>.Add (NSObject key, NSObject value)
		{
			throw new NotSupportedException ();
		}

		/// <param name="key">Key to lookup in the dictionary.</param>
		///         <summary>Determines whether the specified key exists in the dictionary.</summary>
		///         <returns>
		///         </returns>
		///         <remarks>
		///         </remarks>
		public bool ContainsKey (NSObject key)
		{
			return ObjectForKey (key) is not null;
		}

		internal bool ContainsKey (IntPtr key)
		{
			return LowlevelObjectForKey (key) != IntPtr.Zero;
		}

		bool IDictionary<NSObject, NSObject>.Remove (NSObject key)
		{
			throw new NotSupportedException ();
		}

		internal bool TryGetValue<T> (INativeObject key, out T value) where T : class, INativeObject
		{
			value = null;
			if (key is null)
				ObjCRuntime.ThrowHelper.ThrowArgumentNullException (nameof (key));

			var ptr = _ObjectForKey (key.Handle);
			GC.KeepAlive (key);
			if (ptr == IntPtr.Zero)
				return false;

			value = Runtime.GetINativeObject<T> (ptr, false);
			return true;
		}

		/// <param name="key">To be added.</param>
		///         <param name="value">To be added.</param>
		///         <summary>To be added.</summary>
		///         <returns>To be added.</returns>
		///         <remarks>To be added.</remarks>
		public bool TryGetValue (NSObject key, out NSObject value)
		{
			if (key is null)
				ObjCRuntime.ThrowHelper.ThrowArgumentNullException (nameof (key));

			value = ObjectForKey (key);
			// NSDictionary can not contain NULLs, if you want a NULL, it exists as an NSNull
			return value is not null;
		}

		public virtual NSObject this [NSObject key] {
			get {
				return ObjectForKey (key);
			}
			set {
				throw new NotSupportedException ();
			}
		}

		public virtual NSObject this [NSString key] {
			get {
				return ObjectForKey (key);
			}
			set {
				throw new NotSupportedException ();
			}
		}

		public virtual NSObject this [string key] {
			get {
				if (key is null)
					throw new ArgumentNullException ("key");
				var nss = NSString.CreateNative (key, false);
				try {
					return Runtime.GetNSObject (LowlevelObjectForKey (nss));
				} finally {
					NSString.ReleaseNative (nss);
				}
			}
			set {
				throw new NotSupportedException ();
			}
		}

		ICollection<NSObject> IDictionary<NSObject, NSObject>.Keys {
			get { return Keys; }
		}

		ICollection<NSObject> IDictionary<NSObject, NSObject>.Values {
			get { return Values; }
		}

		#endregion

		/// <summary>To be added.</summary>
		///         <returns>To be added.</returns>
		///         <remarks>To be added.</remarks>
		IEnumerator IEnumerable.GetEnumerator ()
		{
			return GetEnumerator ();
		}

		/// <summary>Returns an enumerator that iterates through the dictionary.</summary>
		/// <returns>An enumerator that can be used to iterate through the dictionary.</returns>
		public IEnumerator<KeyValuePair<NSObject, NSObject>> GetEnumerator ()
		{
			foreach (var key in Keys) {
				yield return new KeyValuePair<NSObject, NSObject> (key, ObjectForKey (key));
			}
		}

		/// <param name="key">A handle to an NSObject that might be on the dictionary.</param>
		///         <summary>Low-level key lookup.</summary>
		///         <returns>Handle to an object, or IntPtr.Zero if the key does not exist in the dictionary.</returns>
		///         <remarks>
		/// 	  In some cases, where you might be iterating over a loop, or
		/// 	  you have not surfaced a bound type, but you have the handle to
		/// 	  the key, you can use the <see cref="M:Foundation.NSDictionary.LowlevelObjectForKey(System.IntPtr)" />
		/// 	  which takes a handle for the key and returns a handle for the returned object. 
		/// 	</remarks>
		public IntPtr LowlevelObjectForKey (IntPtr key)
		{
#if MONOMAC
			return ObjCRuntime.Messaging.IntPtr_objc_msgSend_IntPtr (this.Handle, selObjectForKey_XHandle, key);
#else
			return ObjCRuntime.Messaging.IntPtr_objc_msgSend_IntPtr (this.Handle, Selector.GetHandle ("objectForKey:"), key);
#endif
		}

		/// <summary>To be added.</summary>
		///         <returns>To be added.</returns>
		///         <remarks>To be added.</remarks>
		public NSFileAttributes ToFileAttributes ()
		{
			return NSFileAttributes.FromDictionary (this);
		}
	}
}
