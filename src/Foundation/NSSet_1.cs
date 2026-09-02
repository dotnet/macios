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

using System.Collections;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;

#nullable enable

namespace Foundation {
	[Register ("NSSet", SkipRegistration = true)]
	public sealed class NSSet<TKey> : NSSet, IEnumerable<TKey>
		where TKey : class, INativeObject {
		/// <summary>Initializes a new instance of the <see cref="NSSet{TKey}" /> class.</summary>
		public NSSet ()
		{
		}

		/// <summary>Initializes a new instance of the <see cref="NSSet{TKey}" /> class from the data stored in the unarchiver object.</summary>
		/// <param name="coder">The unarchiver object.</param>
		/// <remarks>
		/// <para>This constructor is provided to allow the class to be initialized from an unarchiver (for example, during NIB deserialization). This is part of the <see cref="NSCoding" /> protocol.</para>
		/// <para>If developers want to create a subclass of this object and continue to support deserialization from an archive, they should implement a constructor with an identical signature: taking a single parameter of type <see cref="NSCoder" /> and decorate it with the [Export("initWithCoder:"] attribute declaration.</para>
		/// <para>The state of this object can also be serialized by using the companion method, <see cref="INSCoding.EncodeTo" />.</para>
		/// </remarks>
		public NSSet (NSCoder coder)
			: base (coder)
		{
		}

		internal NSSet (NativeHandle handle)
			: base (handle)
		{
		}

		/// <summary>Initializes a new instance of the <see cref="NSSet{TKey}" /> class with the specified objects.</summary>
		/// <param name="objs">An array of objects to include in the set.</param>
		public NSSet (params TKey [] objs)
			: base (objs)
		{
		}

		/// <summary>Initializes a new instance of the <see cref="NSSet{TKey}" /> class by copying another set.</summary>
		/// <param name="other">The set to copy.</param>
		public NSSet (NSSet<TKey> other)
			: base (other)
		{
		}

		/// <summary>Initializes a new instance of the <see cref="NSSet{TKey}" /> class by copying a mutable set.</summary>
		/// <param name="other">The mutable set to copy.</param>
		public NSSet (NSMutableSet<TKey> other)
			: base (other)
		{
		}
		/// <summary>Create an <see cref="NSSet{TKey}" /> from a collection of items.</summary>
		/// <param name="items">The items to add to the created <see cref="NSSet{TKey}" />.</param>
		/// <param name="convertCallback">A callback function to convert from the type of the element into the type to add to the returned set.</param>
		/// <returns>Null if the collection of items is null, otherwise a new <see cref="NSSet{TKey}" /> with the collection of items.</returns>
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

		/// <summary>Create an <see cref="HashSet{T}" /> from this set of items.</summary>
		/// <param name="convertCallback">A callback function to convert from the type of each element into the type to add to the returned set.</param>
		/// <returns>A new <see cref="HashSet{T}" /> with this set of items.</returns>
		public HashSet<T> ToHashSet<T> (Func<TKey, T> convertCallback)
		{
			var rv = new HashSet<T> ();
			foreach (var item in this)
				rv.Add (convertCallback (item));
			return rv;
		}

		// Strongly typed versions of API from NSSet

		/// <summary>Returns the object in the set that is equal to the specified object.</summary>
		/// <param name="probe">The object to look up.</param>
		/// <returns>The object in the set that is equal to <paramref name="probe" />, or <see langword="null" /> if no such object exists.</returns>
		public TKey? LookupMember (TKey probe)
		{
			ArgumentNullException.ThrowIfNull (probe);

			TKey? result = Runtime.GetINativeObject<TKey> (_LookupMember (probe.Handle), false);
			GC.KeepAlive (probe);
			return result;
		}

		/// <summary>Gets one of the objects in the set.</summary>
		/// <value>One of the objects in the set, or <see langword="null" /> if the set is empty.</value>
		public TKey? AnyObject {
			get {
				return Runtime.GetINativeObject<TKey> (_AnyObject, false);
			}
		}

		/// <summary>Determines whether the set contains the specified object.</summary>
		/// <param name="obj">The object to locate in the set.</param>
		/// <returns><see langword="true" /> if the set contains the specified object; otherwise, <see langword="false" />.</returns>
		public bool Contains (TKey obj)
		{
			ArgumentNullException.ThrowIfNull (obj);

			bool result = _Contains (obj.Handle);
			GC.KeepAlive (obj);
			return result;
		}

		/// <summary>Returns the contents of the set as an array.</summary>
		/// <returns>An array containing all objects in the set.</returns>
		public TKey [] ToArray ()
		{
			return base.ToArray<TKey> ();
		}

		/// <summary>Adds two sets together, creating a new set that contains all elements from both sets.</summary>
		/// <param name="first">The first set.</param>
		/// <param name="second">The second set.</param>
		/// <returns>A new <see cref="NSSet{TKey}" /> containing all elements from both sets, or <see langword="null" /> if both <paramref name="first" /> and <paramref name="second" /> are <see langword="null" />.</returns>
		[return: NotNullIfNotNull (nameof (first))]
		[return: NotNullIfNotNull (nameof (second))]
		public static NSSet<TKey>? operator + (NSSet<TKey>? first, NSSet<TKey>? second)
		{
			if (first is null && second is null)
				return null;

			if (first is null)
				return new NSSet<TKey> (second!);

			if (second is null)
				return new NSSet<TKey> (first);

			if (first.Count == 0)
				return new NSSet<TKey> (second);

			if (second.Count == 0)
				return new NSSet<TKey> (first);

			var result = new NSSet<TKey> (first._SetByAddingObjectsFromSet (second.Handle));
			GC.KeepAlive (second);
			return result;
		}

		/// <summary>Subtracts the elements of the second set from the first set.</summary>
		/// <param name="first">The set to subtract from.</param>
		/// <param name="second">The set whose elements should be removed.</param>
		/// <returns>A new <see cref="NSSet{TKey}" /> containing the elements in <paramref name="first" /> that are not in <paramref name="second" />, or <see langword="null" /> if <paramref name="first" /> is <see langword="null" /> or empty.</returns>
		public static NSSet<TKey>? operator - (NSSet<TKey>? first, NSSet<TKey>? second)
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
		/// <summary>Returns an enumerator that iterates through the set.</summary>
		/// <returns>An enumerator that can be used to iterate through the set.</returns>
		IEnumerator IEnumerable.GetEnumerator ()
		{
			return new NSFastEnumerator<TKey> (this);
		}
		#endregion
	}
}
