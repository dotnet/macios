//
// Copyright 2015 Xamarin Inc (http://www.xamarin.com)
//
// This file contains a generic version of NSSet.
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
//

using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
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
	[Register ("NSSet", SkipRegistration = true)]
	public sealed class NSSet<TKey> : NSSet, IEnumerable<TKey>
		where TKey : class, INativeObject {
		/// <summary>To be added.</summary>
		///         <remarks>To be added.</remarks>
		public NSSet ()
		{
		}

		/// <param name="coder">The unarchiver object.</param>
		///         <summary>A constructor that initializes the object from the data stored in the unarchiver object.</summary>
		///         <remarks>
		///           <para>This constructor is provided to allow the class to be initialized from an unarchiver (for example, during NIB deserialization).   This is part of the <see cref="T:Foundation.NSCoding" />  protocol.</para>
		///           <para>If developers want to create a subclass of this object and continue to support deserialization from an archive, they should implement a constructor with an identical signature: taking a single parameter of type <see cref="T:Foundation.NSCoder" /> and decorate it with the [Export("initWithCoder:"] attribute declaration.</para>
		///           <para>The state of this object can also be serialized by using the companion method, EncodeTo.</para>
		///         </remarks>
		public NSSet (NSCoder coder)
			: base (coder)
		{
		}

		internal NSSet (NativeHandle handle)
			: base (handle)
		{
		}

		/// <param name="objs">To be added.</param>
		///         <summary>To be added.</summary>
		///         <remarks>To be added.</remarks>
		public NSSet (params TKey [] objs)
			: base (objs)
		{
		}

		/// <param name="other">To be added.</param>
		///         <summary>To be added.</summary>
		///         <remarks>To be added.</remarks>
		public NSSet (NSSet<TKey> other)
			: base (other)
		{
		}

		/// <param name="other">To be added.</param>
		///         <summary>To be added.</summary>
		///         <remarks>To be added.</remarks>
		public NSSet (NSMutableSet<TKey> other)
			: base (other)
		{
		}

#nullable enable
		/// <summary>Create an <cref name="NSSet`1" /> from a collection of items.</summary>
		/// <param name="items">The items to add to the created <cref name="NSSet`1" />.</param>
		/// <param name="convertCallback">A callback function to convert from the type of the element into the type to add to the returned set.</param>
		/// <returns>Null if the collection of items is null, otherwise a new <cref name="NSSet`1" /> with the collection of items.</returns>
		[return: NotNullIfNotNull (nameof (items))]
		public static NSSet<TKey>? Create<V> (IEnumerable<V>? items, Func<V, TKey> convertCallback)
		{
			if (items is null)
				return null;

			using var mutableSet = new NSMutableSet<TKey> ();
			foreach (var item in items)
				mutableSet.Add (convertCallback (item));
			return Runtime.GetNSObject<NSSet<TKey>> (mutableSet.Handle, false)!;
		}

		/// <summary>Create an <cref name="HashSet`2" /> from this set of items.</summary>
		/// <param name="convertCallback">A callback function to convert from the type of each element into the type to add to the returned set.</param>
		/// <returns>A new <cref name="HashSet`2" /> with this set of items.</returns>
		public HashSet<T> ToHashSet<T> (Func<TKey, T> convertCallback)
		{
			var rv = new HashSet<T> ();
			foreach (var item in this)
				rv.Add (convertCallback (item));
			return rv;
		}
#nullable disable

		// Strongly typed versions of API from NSSet

		/// <param name="probe">To be added.</param>
		///         <summary>To be added.</summary>
		///         <returns>To be added.</returns>
		///         <remarks>To be added.</remarks>
		public TKey LookupMember (TKey probe)
		{
			if (probe is null)
				throw new ArgumentNullException (nameof (probe));

			TKey result = Runtime.GetINativeObject<TKey> (_LookupMember (probe.Handle), false);
			GC.KeepAlive (probe);
			return result;
		}

		/// <summary>To be added.</summary>
		///         <value>To be added.</value>
		///         <remarks>To be added.</remarks>
		public TKey AnyObject {
			get {
				return Runtime.GetINativeObject<TKey> (_AnyObject, false);
			}
		}

		/// <param name="obj">To be added.</param>
		///         <summary>To be added.</summary>
		///         <returns>To be added.</returns>
		///         <remarks>To be added.</remarks>
		public bool Contains (TKey obj)
		{
			if (obj is null)
				throw new ArgumentNullException (nameof (obj));

			bool result = _Contains (obj.Handle);
			GC.KeepAlive (obj);
			return result;
		}

		/// <summary>To be added.</summary>
		///         <returns>To be added.</returns>
		///         <remarks>To be added.</remarks>
		public TKey [] ToArray ()
		{
			return base.ToArray<TKey> ();
		}

		public static NSSet<TKey> operator + (NSSet<TKey> first, NSSet<TKey> second)
		{
			if (first is null || first.Count == 0)
				return new NSSet<TKey> (second);
			if (second is null || second.Count == 0)
				return new NSSet<TKey> (first);
			var result = new NSSet<TKey> (first._SetByAddingObjectsFromSet (second.Handle));
			GC.KeepAlive (second);
			return result;
		}

		public static NSSet<TKey> operator - (NSSet<TKey> first, NSSet<TKey> second)
		{
			if (first is null || first.Count == 0)
				return null;
			if (second is null || second.Count == 0)
				return new NSSet<TKey> (first);
			var copy = new NSMutableSet<TKey> (first);
			copy.MinusSet (second);
			return new NSSet<TKey> (copy);
		}

		#region IEnumerable<TKey>
		/// <summary>Returns an enumerator that iterates through the set.</summary>
		/// <returns>An enumerator that can be used to iterate through the set.</returns>
		public new IEnumerator<TKey> GetEnumerator ()
		{
			return new NSFastEnumerator<TKey> (this);
		}
		#endregion

		#region IEnumerable implementation
		/// <summary>To be added.</summary>
		///         <returns>To be added.</returns>
		///         <remarks>To be added.</remarks>
		IEnumerator IEnumerable.GetEnumerator ()
		{
			return new NSFastEnumerator<TKey> (this);
		}
		#endregion
	}
}
