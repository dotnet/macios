//
// Copyright 2015 Xamarin Inc
//
// This file contains a generic version of NSDictionary.
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
using System.Runtime.Versioning;
using ObjCRuntime;

// Disable until we get around to enable + fix any issues.
#nullable disable

namespace Foundation {
	[SupportedOSPlatform ("ios")]
	[SupportedOSPlatform ("maccatalyst")]
	[SupportedOSPlatform ("macos")]
	[SupportedOSPlatform ("tvos")]
	[Register ("NSDictionary", SkipRegistration = true)]
	public sealed partial class NSDictionary<TKey, TValue> : NSDictionary, IDictionary<TKey, TValue>
		where TKey : class, INativeObject
		where TValue : class, INativeObject {
		/// <summary>To be added.</summary>
		///         <remarks>To be added.</remarks>
		public NSDictionary ()
		{
		}

		/// <param name="coder">The unarchiver object.</param>
		///         <summary>A constructor that initializes the object from the data stored in the unarchiver object.</summary>
		///         <remarks>
		///           <para>This constructor is provided to allow the class to be initialized from an unarchiver (for example, during NIB deserialization).   This is part of the <see cref="Foundation.NSCoding" />  protocol.</para>
		///           <para>If developers want to create a subclass of this object and continue to support deserialization from an archive, they should implement a constructor with an identical signature: taking a single parameter of type <see cref="Foundation.NSCoder" /> and decorate it with the [Export("initWithCoder:"] attribute declaration.</para>
		///           <para>The state of this object can also be serialized by using the companion method, EncodeTo.</para>
		///         </remarks>
		public NSDictionary (NSCoder coder)
			: base (coder)
		{
		}

		/// <param name="filename">To be added.</param>
		///         <summary>To be added.</summary>
		///         <remarks>To be added.</remarks>
		[SupportedOSPlatform ("ios")]
		[SupportedOSPlatform ("macos")]
		[SupportedOSPlatform ("tvos")]
		[SupportedOSPlatform ("maccatalyst")]
		[ObsoletedOSPlatform ("macos10.15", "Use 'NSMutableDictionary(string)' constructor instead.")]
		[ObsoletedOSPlatform ("ios13.0", "Use 'NSMutableDictionary(string)' constructor instead.")]
		[ObsoletedOSPlatform ("tvos13.0", "Use 'NSMutableDictionary(string)' constructor instead.")]
		[ObsoletedOSPlatform ("maccatalyst13.1", "Use 'NSMutableDictionary(string)' constructor instead.")]
		public NSDictionary (string filename)
			: base (filename)
		{
		}

		/// <param name="url">To be added.</param>
		///         <summary>To be added.</summary>
		///         <remarks>To be added.</remarks>
		[SupportedOSPlatform ("ios")]
		[SupportedOSPlatform ("macos")]
		[SupportedOSPlatform ("tvos")]
		[SupportedOSPlatform ("maccatalyst")]
		[ObsoletedOSPlatform ("macos10.15", "Use 'NSMutableDictionary(NSUrl)' constructor instead.")]
		[ObsoletedOSPlatform ("ios13.0", "Use 'NSMutableDictionary(NSUrl)' constructor instead.")]
		[ObsoletedOSPlatform ("tvos13.0", "Use 'NSMutableDictionary(NSUrl)' constructor instead.")]
		[ObsoletedOSPlatform ("maccatalyst13.1", "Use 'NSMutableDictionary(NSUrl)' constructor instead.")]
		public NSDictionary (NSUrl url)
			: base (url)
		{
		}

		internal NSDictionary (NativeHandle handle)
			: base (handle)
		{
		}

		/// <param name="other">To be added.</param>
		///         <summary>To be added.</summary>
		///         <remarks>To be added.</remarks>
		public NSDictionary (NSDictionary<TKey, TValue> other)
			: base (other)
		{
		}

		internal static bool ValidateKeysAndValues (TKey [] keys, TValue [] values)
		{
			if (keys is null)
				throw new ArgumentNullException (nameof (keys));

			if (values is null)
				throw new ArgumentNullException (nameof (values));

			if (values.Length != keys.Length)
				throw new ArgumentException (nameof (values) + " and " + nameof (keys) + " arrays have different sizes");

			return true;
		}

		NSDictionary (TKey [] keys, TValue [] values, bool validation)
			: base (NSArray.FromNSObjects (values), NSArray.FromNSObjects (keys))
		{
		}

		/// <param name="keys">To be added.</param>
		///         <param name="values">To be added.</param>
		///         <summary>To be added.</summary>
		///         <remarks>To be added.</remarks>
		public NSDictionary (TKey [] keys, TValue [] values)
			: this (keys, values, ValidateKeysAndValues (keys, values))
		{
		}

		/// <param name="key">To be added.</param>
		///         <param name="value">To be added.</param>
		///         <summary>To be added.</summary>
		///         <remarks>To be added.</remarks>
		public NSDictionary (TKey key, TValue value)
			: base (NSArray.FromNSObjects (value), NSArray.FromNSObjects (key))
		{
		}

#nullable enable
		/// <summary>Create an <cref name="Dictionary`2" /> from this dictionary.</summary>
		/// <param name="convertCallback">A callback function to convert from the type of each key and value into the type to add to the returned dictionary.</param>
		/// <returns>Null if the collection of items is null, otherwise a new <cref name="Dictionary`2" /> from this dictionary.</returns>
		public Dictionary<K, V> ToDictionary<K, V> (Func<TKey, TValue, (K Key, V Value)> convertCallback) where K : notnull
		{
			var rv = new Dictionary<K, V> ();
			foreach (var kvp in (IDictionary<TKey, TValue>) this) {
				var converted = convertCallback (kvp.Key, kvp.Value);
				rv.Add (converted.Key, converted.Value);
			}
			return rv;
		}
#nullable disable
		// Strongly typed methods from NSDictionary

		/// <param name="key">To be added.</param>
		///         <summary>To be added.</summary>
		///         <returns>To be added.</returns>
		///         <remarks>To be added.</remarks>
		public TValue ObjectForKey (TKey key)
		{
			if (key is null)
				throw new ArgumentNullException (nameof (key));

			TValue ret = Runtime.GetINativeObject<TValue> (_ObjectForKey (key.Handle), false);
			GC.KeepAlive (key);

			return ret;
		}

		/// <summary>To be added.</summary>
		///         <value>To be added.</value>
		///         <remarks>To be added.</remarks>
		public TKey [] Keys {
			get {
				using (var pool = new NSAutoreleasePool ())
					return NSArray.ArrayFromHandle<TKey> (_AllKeys ());
			}
		}

		/// <param name="obj">To be added.</param>
		///         <summary>To be added.</summary>
		///         <returns>To be added.</returns>
		///         <remarks>To be added.</remarks>
		public TKey [] KeysForObject (TValue obj)
		{
			if (obj is null)
				throw new ArgumentNullException (nameof (obj));

			using (var pool = new NSAutoreleasePool ()) {
				TKey [] ret = NSArray.ArrayFromHandle<TKey> (_AllKeysForObject (obj.Handle));
				GC.KeepAlive (obj);
				return ret;
			}
		}

		/// <summary>To be added.</summary>
		///         <value>To be added.</value>
		///         <remarks>To be added.</remarks>
		public TValue [] Values {
			get {
				using (var pool = new NSAutoreleasePool ())
					return NSArray.ArrayFromHandle<TValue> (_AllValues ());
			}
		}

		/// <param name="keys">To be added.</param>
		///         <param name="marker">To be added.</param>
		///         <summary>To be added.</summary>
		///         <returns>To be added.</returns>
		///         <remarks>To be added.</remarks>
		public TValue [] ObjectsForKeys (TKey [] keys, TValue marker)
		{
			if (keys is null)
				throw new ArgumentNullException (nameof (keys));

			if (marker is null)
				throw new ArgumentNullException (nameof (marker));

			if (keys.Length == 0)
				return new TValue [] { };

			using (var pool = new NSAutoreleasePool ()) {
				var keysArray = NSArray.From<TKey> (keys);
				var result = NSArray.ArrayFromHandle<TValue> (_ObjectsForKeys (keysArray.Handle, marker.Handle));
				GC.KeepAlive (keysArray);
				GC.KeepAlive (marker);
				return result;
			}
		}

		static NSDictionary<TKey, TValue> GenericFromObjectsAndKeysInternal (NSArray objects, NSArray keys)
		{
			var result = Runtime.GetNSObject<NSDictionary<TKey, TValue>> (_FromObjectsAndKeysInternal (objects.Handle, keys.Handle));
			GC.KeepAlive (objects);
			GC.KeepAlive (keys);
			return result;
		}

		/// <param name="objects">To be added.</param>
		/// <param name="keys">To be added.</param>
		/// <param name="count">To be added.</param>
		/// <summary>To be added.</summary>
		/// <returns>To be added.</returns>
		/// <remarks>To be added.</remarks>
		public static NSDictionary<TKey, TValue> FromObjectsAndKeys (TValue [] objects, TKey [] keys, nint count)
		{
			if (objects is null)
				throw new ArgumentNullException (nameof (objects));
			if (keys is null)
				throw new ArgumentNullException (nameof (keys));
			if (objects.Length != keys.Length)
				throw new ArgumentException (nameof (objects) + " and " + nameof (keys) + " arrays have different sizes");
			if (count < 1 || objects.Length < count)
				throw new ArgumentException (nameof (count));

			using (var no = NSArray.FromNativeObjects (objects, count))
			using (var nk = NSArray.FromNativeObjects (keys, count))
				return GenericFromObjectsAndKeysInternal (no, nk);
		}

		public static NSDictionary<TKey, TValue> FromObjectsAndKeys (TValue [] objects, TKey [] keys)
		{
			if (objects is null)
				throw new ArgumentNullException (nameof (objects));
			if (keys is null)
				throw new ArgumentNullException (nameof (keys));
			if (objects.Length != keys.Length)
				throw new ArgumentException (nameof (objects) + " and " + nameof (keys) + " arrays have different sizes");

			using (var no = NSArray.FromNSObjects (objects))
			using (var nk = NSArray.FromNSObjects (keys))
				return GenericFromObjectsAndKeysInternal (no, nk);
		}

		/// <param name="objects">To be added.</param>
		///         <param name="keys">To be added.</param>
		///         <summary>To be added.</summary>
		///         <returns>To be added.</returns>
		///         <remarks>To be added.</remarks>
		public static NSDictionary<TKey, TValue> FromObjectsAndKeys (object [] objects, object [] keys)
		{
			if (objects is null)
				throw new ArgumentNullException (nameof (objects));
			if (keys is null)
				throw new ArgumentNullException (nameof (keys));
			if (objects.Length != keys.Length)
				throw new ArgumentException (nameof (objects) + " and " + nameof (keys) + " arrays have different sizes");

			using (var no = NSArray.FromObjects (objects))
			using (var nk = NSArray.FromObjects (keys))
				return GenericFromObjectsAndKeysInternal (no, nk);
		}

		/// <param name="objects">To be added.</param>
		/// <param name="keys">To be added.</param>
		/// <param name="count">To be added.</param>
		/// <summary>To be added.</summary>
		/// <returns>To be added.</returns>
		/// <remarks>To be added.</remarks>
		public static NSDictionary<TKey, TValue> FromObjectsAndKeys (NSObject [] objects, NSObject [] keys, nint count)
		{
			if (objects is null)
				throw new ArgumentNullException (nameof (objects));
			if (keys is null)
				throw new ArgumentNullException (nameof (keys));
			if (objects.Length != keys.Length)
				throw new ArgumentException (nameof (objects) + " and " + nameof (keys) + " arrays have different sizes");
			if (count < 1 || objects.Length < count || keys.Length < count)
				throw new ArgumentException (nameof (count));

			using (var no = NSArray.FromNativeObjects (objects, count))
			using (var nk = NSArray.FromNativeObjects (keys, count))
				return GenericFromObjectsAndKeysInternal (no, nk);
		}

		/// <param name="objects">To be added.</param>
		/// <param name="keys">To be added.</param>
		/// <param name="count">To be added.</param>
		/// <summary>To be added.</summary>
		/// <returns>To be added.</returns>
		/// <remarks>To be added.</remarks>
		public static NSDictionary<TKey, TValue> FromObjectsAndKeys (object [] objects, object [] keys, nint count)
		{
			if (objects is null)
				throw new ArgumentNullException (nameof (objects));
			if (keys is null)
				throw new ArgumentNullException (nameof (keys));
			if (objects.Length != keys.Length)
				throw new ArgumentException (nameof (objects) + " and " + nameof (keys) + " arrays have different sizes");
			if (count < 1 || objects.Length < count || keys.Length < count)
				throw new ArgumentException (nameof (count));

			using (var no = NSArray.FromObjects (count, objects))
			using (var nk = NSArray.FromObjects (count, keys))
				return GenericFromObjectsAndKeysInternal (no, nk);
		}

		// Other implementations

		/// <param name="key">To be added.</param>
		///         <summary>To be added.</summary>
		///         <returns>To be added.</returns>
		///         <remarks>To be added.</remarks>
		public bool ContainsKey (TKey key)
		{
			if (key is null)
				throw new ArgumentNullException (nameof (key));

			bool ret = _ObjectForKey (key.Handle) != IntPtr.Zero;
			GC.KeepAlive (key);
			return ret;
		}

		/// <param name="key">To be added.</param>
		///         <param name="value">To be added.</param>
		///         <summary>To be added.</summary>
		///         <returns>To be added.</returns>
		///         <remarks>To be added.</remarks>
		public bool TryGetValue (TKey key, out TValue value)
		{
			// NSDictionary can not contain NULLs, if you want a NULL, it exists as an NSNull
			return (value = ObjectForKey (key)) is not null;
		}

		public TValue this [TKey key] {
			get {
				return ObjectForKey (key);
			}
		}

		#region IDictionary<K,V> implementation
		bool IDictionary<TKey, TValue>.ContainsKey (TKey key)
		{
			return ContainsKey (key);
		}

		void IDictionary<TKey, TValue>.Add (TKey key, TValue value)
		{
			throw new NotSupportedException ();
		}

		bool IDictionary<TKey, TValue>.Remove (TKey key)
		{
			throw new NotSupportedException ();
		}

		bool IDictionary<TKey, TValue>.TryGetValue (TKey key, out TValue value)
		{
			return TryGetValue (key, out value);
		}

		TValue IDictionary<TKey, TValue>.this [TKey key] {
			get {
				return this [key];
			}
			set {
				throw new NotSupportedException ();
			}
		}

		ICollection<TKey> IDictionary<TKey, TValue>.Keys {
			get {
				return Keys;
			}
		}

		ICollection<TValue> IDictionary<TKey, TValue>.Values {
			get {
				return Values;
			}
		}
		#endregion

		#region ICollection<K,V> implementation
		void ICollection<KeyValuePair<TKey, TValue>>.Add (KeyValuePair<TKey, TValue> item)
		{
			throw new NotSupportedException ();
		}

		void ICollection<KeyValuePair<TKey, TValue>>.Clear ()
		{
			throw new NotSupportedException ();
		}

		bool ICollection<KeyValuePair<TKey, TValue>>.Contains (KeyValuePair<TKey, TValue> item)
		{
			TValue value;
			if (!TryGetValue<TValue> (item.Key, out value))
				return false;

			return (object) value == (object) item.Value;
		}

		void ICollection<KeyValuePair<TKey, TValue>>.CopyTo (KeyValuePair<TKey, TValue> [] array, int arrayIndex)
		{
			if (array is null)
				throw new ArgumentNullException (nameof (array));
			if (arrayIndex < 0)
				throw new ArgumentOutOfRangeException (nameof (arrayIndex));
			int c = array.Length;
			if ((c > 0) && (arrayIndex >= c))
				throw new ArgumentException (nameof (arrayIndex) + " is equal to or greater than " + nameof (array) + ".Length");
			if (arrayIndex + (int) Count > c)
				throw new ArgumentException ("Not enough room from " + nameof (arrayIndex) + " to end of " + nameof (array) + " for this dictionary");

			var idx = arrayIndex;
			foreach (var kvp in (IEnumerable<KeyValuePair<TKey, TValue>>) this)
				array [idx++] = kvp;
		}

		bool ICollection<KeyValuePair<TKey, TValue>>.Remove (KeyValuePair<TKey, TValue> item)
		{
			throw new NotSupportedException ();
		}

		int ICollection<KeyValuePair<TKey, TValue>>.Count {
			get {
				return (int) base.Count;
			}
		}

		bool ICollection<KeyValuePair<TKey, TValue>>.IsReadOnly {
			get {
				return true;
			}
		}
		#endregion

		#region IEnumerable<KVP> implementation
		IEnumerator<KeyValuePair<TKey, TValue>> IEnumerable<KeyValuePair<TKey, TValue>>.GetEnumerator ()
		{
			foreach (var key in Keys) {
				yield return new KeyValuePair<TKey, TValue> (key, ObjectForKey (key));
			}
		}
		#endregion

		#region IEnumerable implementation
		/// <summary>To be added.</summary>
		///         <returns>To be added.</returns>
		///         <remarks>To be added.</remarks>
		IEnumerator IEnumerable.GetEnumerator ()
		{
			return GetEnumerator ();
		}
		#endregion
	}
}
