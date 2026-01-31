//
// Copyright 2015 Xamarin Inc (http://www.xamarin.com)
//
// This file contains a generic version of NSMutableSet.
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

#nullable enable

namespace Foundation {
	/// <summary>A strongly-typed mutable set that contains objects of type <typeparamref name="TKey" />.</summary>
	/// <typeparam name="TKey">The type of objects in the set.</typeparam>
	[Register ("NSMutableSet", SkipRegistration = true)]
	public sealed partial class NSMutableSet<TKey> : NSMutableSet, IEnumerable<TKey>
		where TKey : class, INativeObject {
		/// <summary>Initializes a new empty mutable set.</summary>
		public NSMutableSet ()
		{
		}

		/// <summary>Initializes the object from the data stored in the unarchiver object.</summary>
		/// <param name="coder">The unarchiver object.</param>
		/// <remarks>
		/// <para>This constructor is provided to allow the class to be initialized from an unarchiver (for example, during NIB deserialization). This is part of the <see cref="Foundation.NSCoding" /> protocol.</para>
		/// <para>If developers want to create a subclass of this object and continue to support deserialization from an archive, they should implement a constructor with an identical signature: taking a single parameter of type <see cref="Foundation.NSCoder" /> and decorate it with the [Export("initWithCoder:"] attribute declaration.</para>
		/// <para>The state of this object can also be serialized by using the companion method, EncodeTo.</para>
		/// </remarks>
		public NSMutableSet (NSCoder coder)
			: base (coder)
		{
		}

		internal NSMutableSet (NativeHandle handle)
			: base (handle)
		{
		}

		/// <summary>Initializes a new mutable set with the specified objects.</summary>
		/// <param name="objs">The objects to add to the set.</param>
		public NSMutableSet (params TKey [] objs)
			: base (objs)
		{
		}

		/// <summary>Initializes a new mutable set with the contents of the specified set.</summary>
		/// <param name="other">The set whose contents will be copied to the new set.</param>
		public NSMutableSet (NSSet<TKey> other)
			: base (other)
		{
		}

		/// <summary>Initializes a new mutable set with the contents of the specified mutable set.</summary>
		/// <param name="other">The mutable set whose contents will be copied to the new set.</param>
		public NSMutableSet (NSMutableSet<TKey> other)
			: base (other)
		{
		}

		/// <summary>Initializes a new mutable set with the specified initial capacity.</summary>
		/// <param name="capacity">The initial capacity of the set.</param>
		public NSMutableSet (nint capacity)
			: base (capacity)
		{
		}

		// Strongly typed versions of API from NSSet

		/// <summary>Returns the object in the set that is equal to the specified object.</summary>
		/// <param name="probe">The object to search for in the set.</param>
		/// <returns>The object in the set that is equal to <paramref name="probe" />, or <see langword="null" /> if no such object exists.</returns>
		public TKey? LookupMember (TKey probe)
		{
			if (probe is null)
				throw new ArgumentNullException (nameof (probe));

			TKey? result = Runtime.GetINativeObject<TKey> (_LookupMember (probe.Handle), false);
			GC.KeepAlive (probe);
			return result;
		}

		/// <summary>Gets one of the objects in the set, or <see langword="null" /> if the set is empty.</summary>
		/// <value>An arbitrary object from the set, or <see langword="null" /> if the set contains no objects.</value>
		public TKey? AnyObject {
			get {
				return Runtime.GetINativeObject<TKey> (_AnyObject, false);
			}
		}

		/// <summary>Determines whether the set contains the specified object.</summary>
		/// <param name="obj">The object to locate in the set.</param>
		/// <returns><see langword="true" /> if the set contains <paramref name="obj" />; otherwise, <see langword="false" />.</returns>
		public bool Contains (TKey obj)
		{
			if (obj is null)
				throw new ArgumentNullException (nameof (obj));

			bool result = _Contains (obj.Handle);
			GC.KeepAlive (obj);
			return result;
		}

		/// <summary>Converts the set to an array.</summary>
		/// <returns>An array containing all the objects in the set.</returns>
		public TKey [] ToArray ()
		{
			return base.ToArray<TKey> ();
		}

		/// <summary>Creates a new mutable set containing all objects from both sets.</summary>
		/// <param name="first">The first set.</param>
		/// <param name="second">The second set.</param>
		/// <returns>A new <see cref="NSMutableSet{TKey}" /> containing the union of both sets, or <see langword="null" /> if both sets are <see langword="null" />.</returns>
		public static NSMutableSet<TKey>? operator + (NSMutableSet<TKey>? first, NSMutableSet<TKey>? second)
		{
			if (first is null && second is null)
				return null;
			if (first is null || first.Count == 0)
				return second is null ? new NSMutableSet<TKey> () : new NSMutableSet<TKey> (second);
			if (second is null || second.Count == 0)
				return new NSMutableSet<TKey> (first);
			var result = new NSMutableSet<TKey> (first._SetByAddingObjectsFromSet (second.Handle));
			GC.KeepAlive (second);
			return result;
		}

		/// <summary>Creates a new mutable set with objects from the first set that are not in the second set.</summary>
		/// <param name="first">The first set.</param>
		/// <param name="second">The second set.</param>
		/// <returns>A new <see cref="NSMutableSet{TKey}" /> containing the difference, or <see langword="null" /> if the first set is <see langword="null" /> or empty.</returns>
		public static NSMutableSet<TKey>? operator - (NSMutableSet<TKey>? first, NSMutableSet<TKey>? second)
		{
			if (first is null || first.Count == 0)
				return null;
			if (second is null || second.Count == 0)
				return new NSMutableSet<TKey> (first);
			var copy = new NSMutableSet<TKey> (first);
			copy.MinusSet (second);
			return copy;
		}

		// Strongly typed versions of API from NSMutableSet
		/// <summary>Adds the specified object to the set.</summary>
		/// <param name="obj">The object to add to the set.</param>
		public void Add (TKey obj)
		{
			if (obj is null)
				throw new ArgumentNullException (nameof (obj));

			_Add (obj.Handle);
			GC.KeepAlive (obj);
		}

		/// <summary>Removes the specified object from the set.</summary>
		/// <param name="obj">The object to remove from the set.</param>
		public void Remove (TKey obj)
		{
			if (obj is null)
				throw new ArgumentNullException (nameof (obj));

			_Remove (obj.Handle);
			GC.KeepAlive (obj);
		}

		/// <summary>Adds multiple objects to the set.</summary>
		/// <param name="objects">The objects to add to the set.</param>
		public void AddObjects (params TKey [] objects)
		{
			if (objects is null)
				throw new ArgumentNullException (nameof (objects));

			using (var array = NSArray.FromNonNullNativeObjects<TKey> (objects))
				_AddObjects (array.Handle);
		}

		#region IEnumerable<T> implementation
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
