//
// Copyright 2015 Xamarin Inc (http://www.xamarin.com)
//
// This file contains a generic version of NSMutableDictionary.
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
using System.ComponentModel;
using System.Runtime.Versioning;

using ObjCRuntime;

#if !NET
using NativeHandle = System.IntPtr;
#endif

// Disable until we get around to enable + fix any issues.
#nullable disable

namespace Foundation {
#if NET
	[SupportedOSPlatform ("ios")]
	[SupportedOSPlatform ("maccatalyst")]
	[SupportedOSPlatform ("macos")]
	[SupportedOSPlatform ("tvos")]
#endif
	[Register ("NSMutableDictionary", SkipRegistration = true)]
	public sealed partial class NSMutableDictionary<TKey, TValue> : NSMutableDictionary, IDictionary<TKey, TValue>
		where TKey : class, INativeObject
		where TValue : class, INativeObject {

		/// <summary>To be added.</summary>
		///         <remarks>To be added.</remarks>
		public NSMutableDictionary ()
		{
		}

		/// <param name="coder">The unarchiver object.</param>
		///         <summary>A constructor that initializes the object from the data stored in the unarchiver object.</summary>
		///         <remarks>
		///           <para>This constructor is provided to allow the class to be initialized from an unarchiver (for example, during NIB deserialization).   This is part of the <see cref="T:Foundation.NSCoding" />  protocol.</para>
		///           <para>If developers want to create a subclass of this object and continue to support deserialization from an archive, they should implement a constructor with an identical signature: taking a single parameter of type <see cref="T:Foundation.NSCoder" /> and decorate it with the [Export("initWithCoder:"] attribute declaration.</para>
		///           <para>The state of this object can also be serialized by using the companion method, EncodeTo.</para>
		///         </remarks>
		public NSMutableDictionary (NSCoder coder)
			: base (coder)
		{
		}

		internal NSMutableDictionary (NativeHandle handle)
			: base (handle)
		{
		}

		/// <param name="other">To be added.</param>
		///         <summary>To be added.</summary>
		///         <remarks>To be added.</remarks>
		public NSMutableDictionary (NSMutableDictionary<TKey, TValue> other)
			: base (other)
		{
		}

		/// <param name="other">To be added.</param>
		///         <summary>To be added.</summary>
		///         <remarks>To be added.</remarks>
		public NSMutableDictionary (NSDictionary<TKey, TValue> other)
			: base (other)
		{
		}

		NSMutableDictionary (TKey [] keys, TValue [] values, bool validation)
			: base (NSArray.FromNSObjects (values), NSArray.FromNSObjects (keys))
		{
		}

		/// <param name="keys">To be added.</param>
		///         <param name="values">To be added.</param>
		///         <summary>To be added.</summary>
		///         <remarks>To be added.</remarks>
		public NSMutableDictionary (TKey [] keys, TValue [] values)
			: this (keys, values, NSDictionary<TKey, TValue>.ValidateKeysAndValues (keys, values))
		{
		}

		/// <param name="key">To be added.</param>
		///         <param name="value">To be added.</param>
		///         <summary>To be added.</summary>
		///         <remarks>To be added.</remarks>
		public NSMutableDictionary (TKey key, TValue value)
			: base (NSArray.FromNSObjects (value), NSArray.FromNSObjects (key))
		{

		}

#if hide_loosely_typed_members
		// Hide any loosely typed base members.
		[EditorBrowsable (EditorBrowsableState.Never)]
		public override NSObject this[NSObject key] {
			get {
				return (NSObject) (object) this [(TKey) (object) key];
			}
			set {
				this [(TKey) (object) key] = (TValue) (object) value;
			}
		}

		// This one can never be correct, so make it an error to use it.
		[Obsolete ("This indexer does not have the correct key type.", true)]
		[EditorBrowsable (EditorBrowsableState.Never)]
#pragma warning disable 809
		public override NSObject this[string key] {
#pragma warning restore 809
			get {
				throw new NotSupportedException ();
			}
			set {
				throw new NotSupportedException ();
			}
		}

		// This can be correct if TKey=NSString and TValue=NSObject (but still hide it).
		[EditorBrowsable (EditorBrowsableState.Never)]
		public override NSObject this[NSString key] {
			get {
				return (NSObject) (object) this [(TKey) (object) key];
			}
			set {
				this [(TKey) (object) key] = (TValue) (object) value;
			}
		}
#endif

		// Strongly typed methods from NSDictionary

		/// <param name="key">To be added.</param>
		///         <summary>To be added.</summary>
		///         <returns>To be added.</returns>
		///         <remarks>To be added.</remarks>
		public TValue ObjectForKey (TKey key)
		{
			if (key is null)
				throw new ArgumentNullException (nameof (key));

			var result = Runtime.GetINativeObject<TValue> (_ObjectForKey (key.Handle), false);
			GC.KeepAlive (key);
			return result;
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
				var result = NSArray.ArrayFromHandle<TKey> (_AllKeysForObject (obj.Handle));
				GC.KeepAlive (obj);
				return result;
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

			var keysArray = NSArray.From<TKey> (keys);
			var result = NSArray.ArrayFromHandle<TValue> (_ObjectsForKeys (keysArray.Handle, marker.Handle));
			GC.KeepAlive (keysArray);
			GC.KeepAlive (marker);
			return result;
		}

		// Strongly typed methods from NSMutableDictionary

		/// <param name="key">To be added.</param>
		///         <param name="value">To be added.</param>
		///         <summary>To be added.</summary>
		///         <remarks>To be added.</remarks>
		public void Add (TKey key, TValue value)
		{
			if (key is null)
				throw new ArgumentNullException (nameof (key));

			if (value is null)
				throw new ArgumentNullException (nameof (value));

			_SetObject (value.Handle, key.Handle);
			GC.KeepAlive (value);
			GC.KeepAlive (key);
		}

		/// <param name="key">To be added.</param>
		///         <summary>To be added.</summary>
		///         <returns>To be added.</returns>
		///         <remarks>To be added.</remarks>
		public bool Remove (TKey key)
		{
			if (key is null)
				throw new ArgumentNullException (nameof (key));

			var last = Count;
			_RemoveObjectForKey (key.Handle);
			GC.KeepAlive (key);
			return last != Count;
		}

		/// <param name="key">To be added.</param>
		///         <param name="value">To be added.</param>
		///         <summary>To be added.</summary>
		///         <returns>To be added.</returns>
		///         <remarks>To be added.</remarks>
		public bool TryGetValue (TKey key, out TValue value)
		{
			return (value = ObjectForKey (key)) is not null;
		}

		/// <param name="key">To be added.</param>
		///         <summary>To be added.</summary>
		///         <returns>To be added.</returns>
		///         <remarks>To be added.</remarks>
		public bool ContainsKey (TKey key)
		{
			return ObjectForKey (key) is not null;
		}

		public TValue this [TKey index] {
			get {
				return ObjectForKey (index);
			}
			set {
				Add (index, value);
			}
		}

		static NSMutableDictionary<TKey, TValue> GenericFromObjectsAndKeysInternal (NSArray objects, NSArray keys)
		{
			var result = Runtime.GetNSObject<NSMutableDictionary<TKey, TValue>> (_FromObjectsAndKeysInternal (objects.Handle, keys.Handle));
			GC.KeepAlive (objects);
			GC.KeepAlive (keys);
			return result;
		}

		public static NSMutableDictionary<TKey, TValue> FromObjectsAndKeys (TValue [] objects, TKey [] keys, nint count)
		{
			if (objects is null)
				throw new ArgumentNullException (nameof (objects));
			if (keys is null)
				throw new ArgumentNullException (nameof (keys));
			if (objects.Length != keys.Length)
				throw new ArgumentException (nameof (objects) + " and " + nameof (keys) + " arrays have different sizes");
			if (count < 1 || objects.Length < count)
				throw new ArgumentException (nameof (count));

			using (var no = NSArray.FromNSObjects (objects))
			using (var nk = NSArray.FromNSObjects (keys))
				return GenericFromObjectsAndKeysInternal (no, nk);
		}

#if NET
		public static NSMutableDictionary<TKey, TValue> FromObjectsAndKeys (TValue [] objects, TKey [] keys)
#else
		[Obsolete ("'TKey' and 'TValue' are inversed and won't work unless both types are identical. Use the generic overload that takes a count parameter instead.")]
		public static NSMutableDictionary<TKey, TValue> FromObjectsAndKeys (TKey [] objects, TValue [] keys)
#endif
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
		public static NSMutableDictionary<TKey, TValue> FromObjectsAndKeys (object [] objects, object [] keys)
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

		public static NSMutableDictionary<TKey, TValue> FromObjectsAndKeys (NSObject [] objects, NSObject [] keys, nint count)
		{
			if (objects is null)
				throw new ArgumentNullException (nameof (objects));
			if (keys is null)
				throw new ArgumentNullException (nameof (keys));
			if (objects.Length != keys.Length)
				throw new ArgumentException (nameof (objects) + " and " + nameof (keys) + " arrays have different sizes");
			if (count < 1 || objects.Length < count || keys.Length < count)
				throw new ArgumentException (nameof (count));

			using (var no = NSArray.FromNSObjects (objects))
			using (var nk = NSArray.FromNSObjects (keys))
				return GenericFromObjectsAndKeysInternal (no, nk);
		}

		public static NSMutableDictionary<TKey, TValue> FromObjectsAndKeys (object [] objects, object [] keys, nint count)
		{
			if (objects is null)
				throw new ArgumentNullException (nameof (objects));
			if (keys is null)
				throw new ArgumentNullException (nameof (keys));
			if (objects.Length != keys.Length)
				throw new ArgumentException (nameof (objects) + " and " + nameof (keys) + " arrays have different sizes");
			if (count < 1 || objects.Length < count || keys.Length < count)
				throw new ArgumentException (nameof (count));

			using (var no = NSArray.FromObjects (objects))
			using (var nk = NSArray.FromObjects (keys))
				return GenericFromObjectsAndKeysInternal (no, nk);
		}
		#region IDictionary<K,V>
		void IDictionary<TKey, TValue>.Add (TKey key, TValue value)
		{
			Add (key, value);
		}

		bool IDictionary<TKey, TValue>.Remove (TKey key)
		{
			return Remove (key);
		}

		bool IDictionary<TKey, TValue>.TryGetValue (TKey key, out TValue value)
		{
			return TryGetValue (key, out value);
		}

		TValue IDictionary<TKey, TValue>.this [TKey index] {
			get {
				return this [index];
			}
			set {
				this [index] = value;
			}
		}

		bool IDictionary<TKey, TValue>.ContainsKey (TKey key)
		{
			return ContainsKey (key);
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

		#region ICollection<KeyValuePair<TKey, TValue>>
		void ICollection<KeyValuePair<TKey, TValue>>.Add (KeyValuePair<TKey, TValue> item)
		{
			Add (item.Key, item.Value);
		}

		void ICollection<KeyValuePair<TKey, TValue>>.Clear ()
		{
			base.Clear ();
		}

		bool ICollection<KeyValuePair<TKey, TValue>>.Contains (KeyValuePair<TKey, TValue> item)
		{
			TValue value;

			if (!TryGetValue (item.Key, out value))
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
			TValue value;

			if (item.Key is null)
				throw new ArgumentNullException (nameof (item) + ".Key");

			if (item.Value is null)
				throw new ArgumentNullException (nameof (item) + ".Value");

			if (!TryGetValue (item.Key, out value))
				return false;

			if ((object) value == (object) item.Value) {
				Remove (item.Key);
				return true;
			}

			return false;
		}

		int ICollection<KeyValuePair<TKey, TValue>>.Count {
			get {
				return (int) base.Count;
			}
		}

		bool ICollection<KeyValuePair<TKey, TValue>>.IsReadOnly {
			get {
				return false;
			}
		}
		#endregion

		#region IEnumerable<KeyValuePair<TKey, TValue>>
		IEnumerator<KeyValuePair<TKey, TValue>> IEnumerable<KeyValuePair<TKey, TValue>>.GetEnumerator ()
		{
			foreach (var key in Keys)
				yield return new KeyValuePair<TKey, TValue> (key, ObjectForKey (key));
		}
		#endregion

		#region IEnumerable
		/// <summary>To be added.</summary>
		///         <returns>To be added.</returns>
		///         <remarks>To be added.</remarks>
		IEnumerator IEnumerable.GetEnumerator ()
		{
			return ((IEnumerable<KeyValuePair<TKey, TValue>>) this).GetEnumerator ();
		}
		#endregion

	}
}
