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

using System.Collections;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;

#nullable enable

namespace Foundation {
	[Register ("NSDictionary", SkipRegistration = true)]
	public sealed partial class NSDictionary<TKey, TValue> : NSDictionary, IDictionary<TKey, TValue>
		where TKey : class, INativeObject
		where TValue : class, INativeObject {
		/// <summary>
		/// Initializes a new instance of the <see cref="NSDictionary{TKey, TValue}"/> class.
		/// </summary>
		public NSDictionary ()
		{
		}

		/// <summary>
		/// A constructor that initializes the object from the data stored in the unarchiver object.
		/// </summary>
		/// <param name="coder">The unarchiver object.</param>
		/// <remarks>
		/// <para>This constructor is provided to allow the class to be initialized from an unarchiver (for example, during NIB deserialization). This is part of the <see cref="Foundation.NSCoding"/> protocol.</para>
		/// <para>If developers want to create a subclass of this object and continue to support deserialization from an archive, they should implement a constructor with an identical signature: taking a single parameter of type <see cref="Foundation.NSCoder"/> and decorate it with the [Export("initWithCoder:"] attribute declaration.</para>
		/// <para>The state of this object can also be serialized by using the companion method, EncodeTo.</para>
		/// </remarks>
		public NSDictionary (NSCoder coder)
			: base (coder)
		{
		}

		/// <summary>
		/// Initializes a new instance of the <see cref="NSDictionary{TKey, TValue}"/> class from a property list file.
		/// </summary>
		/// <param name="filename">The path to a property list file.</param>
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

		/// <summary>
		/// Initializes a new instance of the <see cref="NSDictionary{TKey, TValue}"/> class from a property list at the specified URL.
		/// </summary>
		/// <param name="url">The URL to a property list file.</param>
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

		/// <summary>
		/// Initializes a new instance of the <see cref="NSDictionary{TKey, TValue}"/> class from another dictionary.
		/// </summary>
		/// <param name="other">The dictionary to copy from.</param>
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

		NSDictionary (TKey? [] keys, TValue? [] values, bool validation)
			: base (NSArray.FromNSObjects (values), NSArray.FromNSObjects (keys))
		{
		}

		/// <summary>
		/// Initializes a new instance of the <see cref="NSDictionary{TKey, TValue}"/> class from parallel arrays of keys and values.
		/// </summary>
		/// <param name="keys">An array of keys.</param>
		/// <param name="values">An array of values.</param>
		public NSDictionary (TKey [] keys, TValue [] values)
			: this (keys, values, ValidateKeysAndValues (keys, values))
		{
		}

		/// <summary>
		/// Initializes a new instance of the <see cref="NSDictionary{TKey, TValue}"/> class with a single key-value pair.
		/// </summary>
		/// <param name="key">The key.</param>
		/// <param name="value">The value.</param>
		public NSDictionary (TKey? key, TValue? value)
			: base (NSArray.FromNSObjects (value), NSArray.FromNSObjects (key))
		{
		}

		/// <summary>
		/// Creates a <see cref="Dictionary{TKey,TValue}"/> from this dictionary.
		/// </summary>
		/// <typeparam name="K">The key type for the returned dictionary.</typeparam>
		/// <typeparam name="V">The value type for the returned dictionary.</typeparam>
		/// <param name="convertCallback">A callback function to convert from the type of each key and value into the type to add to the returned dictionary.</param>
		/// <returns>A new <see cref="Dictionary{TKey,TValue}"/> from this dictionary.</returns>
		public Dictionary<K, V> ToDictionary<K, V> (Func<TKey, TValue, (K Key, V Value)> convertCallback) where K : notnull
		{
			ArgumentNullException.ThrowIfNull (convertCallback);

			var rv = new Dictionary<K, V> ();
			foreach (var kvp in (IDictionary<TKey, TValue>) this) {
				var converted = convertCallback (kvp.Key, kvp.Value);
				rv.Add (converted.Key, converted.Value);
			}
			return rv;
		}

		// Strongly typed methods from NSDictionary

		/// <summary>
		/// Returns the value associated with the specified key, or <see langword="null"/> if the key is not present.
		/// </summary>
		/// <param name="key">The key to look up.</param>
		/// <returns>The value associated with the specified key, or <see langword="null"/> if the key is not present.</returns>
		public TValue? ObjectForKey (TKey key)
		{
			ArgumentNullException.ThrowIfNull (key);

			var ret = Runtime.GetINativeObject<TValue> (_ObjectForKey (key.Handle), false);
			GC.KeepAlive (key);

			return ret;
		}

		/// <summary>
		/// Gets an array containing all keys in the dictionary.
		/// </summary>
		/// <value>An array of keys.</value>
		public TKey [] Keys {
			get {
				using (var pool = new NSAutoreleasePool ())
					return NSArray.ArrayFromHandle<TKey> (_AllKeys ());
			}
		}

		/// <summary>
		/// Returns an array of keys corresponding to all occurrences of the specified value in the dictionary.
		/// </summary>
		/// <param name="obj">The value to search for.</param>
		/// <returns>An array of keys whose corresponding values are equal to the specified value.</returns>
		public TKey [] KeysForObject (TValue obj)
		{
			ArgumentNullException.ThrowIfNull (obj);

			using (var pool = new NSAutoreleasePool ()) {
				var ret = NSArray.ArrayFromHandle<TKey> (_AllKeysForObject (obj.Handle));
				GC.KeepAlive (obj);
				return ret;
			}
		}

		/// <summary>
		/// Gets an array containing all values in the dictionary.
		/// </summary>
		/// <value>An array of values.</value>
		public TValue [] Values {
			get {
				using (var pool = new NSAutoreleasePool ())
					return NSArray.ArrayFromHandle<TValue> (_AllValues ());
			}
		}

		/// <summary>
		/// Returns an array of values corresponding to a specified set of keys.
		/// </summary>
		/// <param name="keys">An array of keys.</param>
		/// <param name="marker">The marker object to substitute for values that are not present in the dictionary.</param>
		/// <returns>An array of values corresponding to the keys in the <paramref name="keys"/> array, with the marker object substituted for missing values.</returns>
		public TValue [] ObjectsForKeys (TKey [] keys, TValue marker)
		{
			ArgumentNullException.ThrowIfNull (keys);
			ArgumentNullException.ThrowIfNull (marker);

			if (keys.Length == 0)
				return [];

			using (var pool = new NSAutoreleasePool ()) {
				var keysArray = NSArray.FromNativeObjects<TKey> (keys);
				var result = NSArray.ArrayFromHandle<TValue> (_ObjectsForKeys (keysArray.Handle, marker.Handle));
				GC.KeepAlive (keysArray);
				GC.KeepAlive (marker);
				return result;
			}
		}

		static NSDictionary<TKey, TValue> GenericFromObjectsAndKeysInternal (NSArray objects, NSArray keys)
		{
			var result = Runtime.GetNSObject<NSDictionary<TKey, TValue>> (_FromObjectsAndKeysInternal (objects.Handle, keys.Handle))!;
			GC.KeepAlive (objects);
			GC.KeepAlive (keys);
			return result;
		}

		/// <summary>
		/// Creates a dictionary from parallel arrays of values and keys, using only the first <paramref name="count"/> elements.
		/// </summary>
		/// <param name="objects">An array of values. Null elements are stored as <see cref="NSNull.Null"/>.</param>
		/// <param name="keys">An array of keys.</param>
		/// <param name="count">The number of elements to use from each array.</param>
		/// <returns>A new dictionary containing the specified key-value pairs.</returns>
		public static NSDictionary<TKey, TValue> FromObjectsAndKeys (TValue? [] objects, TKey [] keys, nint count)
		{
			if (!ValidateFromObjectsAndKeys (objects, keys, count))
				return new NSDictionary<TKey, TValue> ();

			using (var no = NSArray.FromNativeObjects (objects, count))
			using (var nk = NSArray.FromNativeObjects (keys, count))
				return GenericFromObjectsAndKeysInternal (no, nk);
		}

		/// <summary>
		/// Creates a dictionary from parallel arrays of values and keys.
		/// </summary>
		/// <param name="objects">An array of values. Null elements are stored as <see cref="NSNull.Null"/>.</param>
		/// <param name="keys">An array of keys.</param>
		/// <returns>A new dictionary containing the specified key-value pairs.</returns>
		public static NSDictionary<TKey, TValue> FromObjectsAndKeys (TValue? [] objects, TKey [] keys)
		{
			if (!ValidateFromObjectsAndKeys (objects, keys))
				return new NSDictionary<TKey, TValue> ();

			return FromObjectsAndKeys (objects, keys, objects.Length);
		}

		/// <summary>
		/// Creates a dictionary from parallel arrays of object values and keys.
		/// </summary>
		/// <param name="objects">An array of object values.</param>
		/// <param name="keys">An array of object keys.</param>
		/// <returns>A new dictionary containing the specified key-value pairs.</returns>
		public static NSDictionary<TKey, TValue> FromObjectsAndKeys (object [] objects, object [] keys)
		{
			if (!ValidateFromObjectsAndKeys (objects, keys))
				return new NSDictionary<TKey, TValue> ();

			return FromObjectsAndKeys (objects, keys, objects.Length);
		}

		/// <summary>
		/// Creates a dictionary from parallel arrays of <see cref="NSObject"/> values and keys, using only the first <paramref name="count"/> elements.
		/// </summary>
		/// <param name="objects">An array of <see cref="NSObject"/> values. Null elements are stored as <see cref="NSNull.Null"/>.</param>
		/// <param name="keys">An array of <see cref="NSObject"/> keys.</param>
		/// <param name="count">The number of elements to use from each array.</param>
		/// <returns>A new dictionary containing the specified key-value pairs.</returns>
		public static NSDictionary<TKey, TValue> FromObjectsAndKeys (NSObject? [] objects, NSObject [] keys, nint count)
		{
			if (!ValidateFromObjectsAndKeys (objects, keys, count))
				return new NSDictionary<TKey, TValue> ();

			using (var no = NSArray.FromNativeObjects (objects, count))
			using (var nk = NSArray.FromNativeObjects (keys, count))
				return GenericFromObjectsAndKeysInternal (no, nk);
		}

		/// <summary>
		/// Creates a dictionary from parallel arrays of object values and keys, using only the first <paramref name="count"/> elements.
		/// </summary>
		/// <param name="objects">An array of object values.</param>
		/// <param name="keys">An array of object keys.</param>
		/// <param name="count">The number of elements to use from each array.</param>
		/// <returns>A new dictionary containing the specified key-value pairs.</returns>
		public static NSDictionary<TKey, TValue> FromObjectsAndKeys (object [] objects, object [] keys, nint count)
		{
			if (!ValidateFromObjectsAndKeys (objects, keys, count))
				return new NSDictionary<TKey, TValue> ();

			using (var no = NSArray.FromObjects (count, objects))
			using (var nk = NSArray.FromObjects (count, keys))
				return GenericFromObjectsAndKeysInternal (no, nk);
		}

		// Other implementations

		/// <summary>
		/// Determines whether the dictionary contains the specified key.
		/// </summary>
		/// <param name="key">The key to locate in the dictionary.</param>
		/// <returns><see langword="true"/> if the dictionary contains an element with the specified key; otherwise, <see langword="false"/>.</returns>
		public bool ContainsKey (TKey key)
		{
			ArgumentNullException.ThrowIfNull (key);

			var ret = _ObjectForKey (key.Handle) != IntPtr.Zero;
			GC.KeepAlive (key);
			return ret;
		}

		/// <summary>
		/// Gets the value associated with the specified key.
		/// </summary>
		/// <param name="key">The key to look up.</param>
		/// <param name="value">When this method returns, contains the value associated with the specified key, if the key is found; otherwise, <see langword="null"/>.</param>
		/// <returns><see langword="true"/> if the dictionary contains an element with the specified key; otherwise, <see langword="false"/>.</returns>
		public bool TryGetValue (TKey key, [NotNullWhen (true)] out TValue? value)
		{
			// NSDictionary can not contain NULLs, if you want a NULL, it exists as an NSNull
			return (value = ObjectForKey (key)) is not null;
		}

		/// <summary>
		/// Gets the value associated with the specified key.
		/// </summary>
		/// <param name="key">The key of the value to get.</param>
		/// <returns>The value associated with the specified key, or <see langword="null"/> if the key is not present.</returns>
		public TValue? this [TKey key] {
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

		bool IDictionary<TKey, TValue>.TryGetValue (TKey key, [MaybeNullWhen (false)] out TValue value)
		{
			var result = TryGetValue (key, out var nullableValue);
			value = nullableValue!;
			return result;
		}

		TValue? IDictionary<TKey, TValue>.this [TKey key] {
#pragma warning disable CS8768 // Nullability of reference types in return type doesn't match implemented member 'TValue IDictionary<TKey, TValue>.this[TKey key].get'
			get {
#pragma warning restore CS8768
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
			if (!TryGetValue (item.Key, out var value))
				return false;

			return (object?) value == (object?) item.Value;
		}

		void ICollection<KeyValuePair<TKey, TValue>>.CopyTo (KeyValuePair<TKey, TValue> [] array, int arrayIndex)
		{
			ArgumentNullException.ThrowIfNull (array);

			if (arrayIndex < 0)
				throw new ArgumentOutOfRangeException (nameof (arrayIndex));
			var c = array.Length;
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
				yield return new KeyValuePair<TKey, TValue> (key, ObjectForKey (key)!);
			}
		}
		#endregion

		#region IEnumerable implementation
		/// <summary>
		/// Returns an enumerator that iterates through the dictionary.
		/// </summary>
		/// <returns>An enumerator that can be used to iterate through the dictionary.</returns>
		IEnumerator IEnumerable.GetEnumerator ()
		{
			return ((IEnumerable<KeyValuePair<TKey, TValue>>) this).GetEnumerator ();
		}
		#endregion
	}
}
