//
// This file contains a generic version of NSMutableOrderedSet
//
// Authors:
//		Alex Soto	(alex.soto@xamarin.com)
//
// Copyright 2015, Xamarin Inc.
//

using System.Collections.Generic;
using System.Collections;
using System.Diagnostics.CodeAnalysis;

#nullable enable

namespace Foundation {
	[Register ("NSMutableOrderedSet", SkipRegistration = true)]
	public sealed partial class NSMutableOrderedSet<TKey> : NSMutableOrderedSet, IEnumerable<TKey>
		where TKey : class, INativeObject {

		/// <summary>Initializes a new empty mutable ordered set.</summary>
		public NSMutableOrderedSet ()
		{
		}

		/// <summary>A constructor that initializes the object from the data stored in the unarchiver object.</summary>
		/// <param name="coder">The unarchiver object.</param>
		/// <remarks>
		/// <para>This constructor is provided to allow the class to be initialized from an unarchiver (for example, during NIB deserialization). This is part of the <see cref="Foundation.NSCoding" /> protocol.</para>
		/// <para>If developers want to create a subclass of this object and continue to support deserialization from an archive, they should implement a constructor with an identical signature: taking a single parameter of type <see cref="Foundation.NSCoder" /> and decorate it with the [Export("initWithCoder:")] attribute declaration.</para>
		/// <para>The state of this object can also be serialized by using the companion method, EncodeTo.</para>
		/// </remarks>
		public NSMutableOrderedSet (NSCoder coder) : base (coder)
		{
		}

		internal NSMutableOrderedSet (NativeHandle handle) : base (handle)
		{
		}

		/// <summary>Initializes a new mutable ordered set with the specified initial capacity.</summary>
		/// <param name="capacity">The initial capacity of the set.</param>
		public NSMutableOrderedSet (nint capacity) : base (capacity)
		{
		}

		/// <summary>Initializes a new mutable ordered set containing the specified object.</summary>
		/// <param name="start">The object to add to the set.</param>
		public NSMutableOrderedSet (TKey start) : base (start)
		{
		}

		/// <summary>Initializes a new mutable ordered set with the specified objects.</summary>
		/// <param name="objs">An array of objects to add to the set.</param>
		public NSMutableOrderedSet (params TKey [] objs) : base (objs)
		{
		}

		/// <summary>Initializes a new mutable ordered set with the contents of the specified set.</summary>
		/// <param name="source">The source set to copy from.</param>
		public NSMutableOrderedSet (NSSet<TKey> source) : base (source)
		{
		}

		/// <summary>Initializes a new mutable ordered set with the contents of the specified ordered set.</summary>
		/// <param name="other">The ordered set to copy from.</param>
		public NSMutableOrderedSet (NSOrderedSet<TKey> other) : base (other)
		{
		}

		/// <summary>Initializes a new mutable ordered set with the contents of the specified mutable ordered set.</summary>
		/// <param name="other">The mutable ordered set to copy from.</param>
		public NSMutableOrderedSet (NSMutableOrderedSet<TKey> other) : base (other)
		{
		}

		/// <summary>Gets or sets the object at the specified index.</summary>
		/// <param name="idx">The index of the object.</param>
		/// <returns>The object at the specified index.</returns>
		public new TKey this [nint idx] {
			get {
				var ret = _GetObject (idx);
				return Runtime.GetINativeObject<TKey> (ret, false)!;
			}

			set {
				ArgumentNullException.ThrowIfNull (value);
				_SetObject (value.Handle, idx);
				GC.KeepAlive (value);
			}
		}

		/// <summary>Returns a set containing the objects from this ordered set.</summary>
		/// <returns>A new <see cref="NSSet{TKey}"/> containing the same objects as this ordered set.</returns>
		public NSSet<TKey> AsSet ()
		{
			var ret = _AsSet ();
			return Runtime.GetINativeObject<NSSet<TKey>> (ret, false)!;
		}

		/// <summary>Inserts the specified object at the specified index in the ordered set.</summary>
		/// <param name="obj">The object to insert.</param>
		/// <param name="atIndex">The index at which to insert the object.</param>
		public void Insert (TKey obj, nint atIndex)
		{
			ArgumentNullException.ThrowIfNull (obj);
			_Insert (obj.Handle, atIndex);
			GC.KeepAlive (obj);
		}

		/// <summary>Replaces the object at the specified index with a new object.</summary>
		/// <param name="objectAtIndex">The index of the object to replace.</param>
		/// <param name="newObject">The new object to place at the specified index.</param>
		public void Replace (nint objectAtIndex, TKey newObject)
		{
			ArgumentNullException.ThrowIfNull (newObject);
			_Replace (objectAtIndex, newObject.Handle);
			GC.KeepAlive (newObject);
		}

		/// <summary>Adds the specified object to the ordered set.</summary>
		/// <param name="obj">The object to add to the set.</param>
		public void Add (TKey obj)
		{
			ArgumentNullException.ThrowIfNull (obj);
			_Add (obj.Handle);
			GC.KeepAlive (obj);
		}

		/// <summary>Adds the objects in the specified array to the ordered set.</summary>
		/// <param name="source">An array of objects to add to the set.</param>
		public void AddObjects (params TKey [] source)
		{
			ArgumentNullException.ThrowIfNull (source);
			_AddObjects (NSArray.FromNativeObjects (source));
		}

		/// <summary>Inserts the specified objects at the specified indexes in the ordered set.</summary>
		/// <param name="objects">An array of objects to insert.</param>
		/// <param name="atIndexes">The indexes at which to insert the objects.</param>
		public void InsertObjects (TKey [] objects, NSIndexSet atIndexes)
		{
			ArgumentNullException.ThrowIfNull (objects);
			ArgumentNullException.ThrowIfNull (atIndexes);
			_InsertObjects (NSArray.FromNativeObjects (objects), atIndexes);
		}

		/// <summary>Replaces the objects at the specified indexes with the specified replacement objects.</summary>
		/// <param name="indexSet">The indexes of the objects to replace.</param>
		/// <param name="replacementObjects">An array of objects to use as replacements.</param>
		public void ReplaceObjects (NSIndexSet indexSet, params TKey [] replacementObjects)
		{
			ArgumentNullException.ThrowIfNull (replacementObjects);
			ArgumentNullException.ThrowIfNull (indexSet);
			_ReplaceObjects (indexSet, NSArray.FromNativeObjects (replacementObjects));
		}

		/// <summary>Removes the specified object from the ordered set.</summary>
		/// <param name="obj">The object to remove from the set.</param>
		public void RemoveObject (TKey obj)
		{
			ArgumentNullException.ThrowIfNull (obj);
			_RemoveObject (obj.Handle);
			GC.KeepAlive (obj);
		}

		/// <summary>Removes the specified objects from the ordered set.</summary>
		/// <param name="objects">An array of objects to remove from the set.</param>
		public void RemoveObjects (params TKey [] objects)
		{
			ArgumentNullException.ThrowIfNull (objects);
			_RemoveObjects (NSArray.FromNativeObjects (objects));
		}

		#region IEnumerable<TKey>
		/// <summary>Returns an enumerator that iterates through the ordered set.</summary>
		/// <returns>An enumerator that can be used to iterate through the ordered set.</returns>
		public new IEnumerator<TKey> GetEnumerator ()
		{
			return new NSFastEnumerator<TKey> (this);
		}
		#endregion

		#region IEnumerable implementation
		/// <summary>Returns an enumerator that iterates through the ordered set.</summary>
		/// <returns>An enumerator that can be used to iterate through the ordered set.</returns>
		IEnumerator IEnumerable.GetEnumerator ()
		{
			return new NSFastEnumerator<TKey> (this);
		}
		#endregion

		/// <summary>Computes the union of two ordered sets.</summary>
		/// <param name="first">The first ordered set.</param>
		/// <param name="second">The second ordered set.</param>
		/// <returns>A new <see cref="NSMutableOrderedSet{TKey}"/> containing all objects from both ordered sets, or <see langword="null"/> if both are <see langword="null"/>.</returns>
		[return: NotNullIfNotNull (nameof (first))]
		[return: NotNullIfNotNull (nameof (second))]
		public static NSMutableOrderedSet<TKey>? operator + (NSMutableOrderedSet<TKey>? first, NSMutableOrderedSet<TKey>? second)
		{
			if (first is null)
				return second is not null ? new NSMutableOrderedSet<TKey> (second) : null;
			if (second is null)
				return new NSMutableOrderedSet<TKey> (first);
			var copy = new NSMutableOrderedSet<TKey> (first);
			copy.UnionSet (second);
			return copy;
		}

		/// <summary>Computes the union of an ordered set and a set.</summary>
		/// <param name="first">The ordered set.</param>
		/// <param name="second">The set to add to the ordered set.</param>
		/// <returns>A new <see cref="NSMutableOrderedSet{TKey}"/> containing all objects from both collections, or <see langword="null"/> if both are <see langword="null"/>.</returns>
		[return: NotNullIfNotNull (nameof (first))]
		[return: NotNullIfNotNull (nameof (second))]
		public static NSMutableOrderedSet<TKey>? operator + (NSMutableOrderedSet<TKey>? first, NSSet<TKey>? second)
		{
			if (first is null)
				return second is not null ? new NSMutableOrderedSet<TKey> (second) : null;
			if (second is null)
				return new NSMutableOrderedSet<TKey> (first);
			var copy = new NSMutableOrderedSet<TKey> (first);
			copy.UnionSet (second);
			return copy;
		}

		/// <summary>Computes the union of two ordered sets.</summary>
		/// <param name="first">The first ordered set.</param>
		/// <param name="second">The second ordered set to add.</param>
		/// <returns>A new <see cref="NSMutableOrderedSet{TKey}"/> containing all objects from both ordered sets, or <see langword="null"/> if both are <see langword="null"/>.</returns>
		[return: NotNullIfNotNull (nameof (first))]
		[return: NotNullIfNotNull (nameof (second))]
		public static NSMutableOrderedSet<TKey>? operator + (NSMutableOrderedSet<TKey>? first, NSOrderedSet<TKey>? second)
		{
			if (first is null)
				return second is not null ? new NSMutableOrderedSet<TKey> (second) : null;
			if (second is null)
				return new NSMutableOrderedSet<TKey> (first);
			var copy = new NSMutableOrderedSet<TKey> (first);
			copy.UnionSet (second);
			return copy;
		}

		/// <summary>Computes the difference between two ordered sets.</summary>
		/// <param name="first">The ordered set to subtract from.</param>
		/// <param name="second">The ordered set to subtract.</param>
		/// <returns>A new <see cref="NSMutableOrderedSet{TKey}"/> containing objects in <paramref name="first"/> but not in <paramref name="second"/>, or <see langword="null"/> if <paramref name="first"/> is <see langword="null"/>.</returns>
		[return: NotNullIfNotNull (nameof (first))]
		public static NSMutableOrderedSet<TKey>? operator - (NSMutableOrderedSet<TKey>? first, NSMutableOrderedSet<TKey>? second)
		{
			if (first is null)
				return null;
			if (second is null)
				return new NSMutableOrderedSet<TKey> (first);
			var copy = new NSMutableOrderedSet<TKey> (first);
			copy.MinusSet (second);
			return copy;
		}

		/// <summary>Computes the difference between an ordered set and a set.</summary>
		/// <param name="first">The ordered set to subtract from.</param>
		/// <param name="second">The set to subtract.</param>
		/// <returns>A new <see cref="NSMutableOrderedSet{TKey}"/> containing objects in <paramref name="first"/> but not in <paramref name="second"/>, or <see langword="null"/> if <paramref name="first"/> is <see langword="null"/>.</returns>
		[return: NotNullIfNotNull (nameof (first))]
		public static NSMutableOrderedSet<TKey>? operator - (NSMutableOrderedSet<TKey>? first, NSSet<TKey>? second)
		{
			if (first is null)
				return null;
			if (second is null)
				return new NSMutableOrderedSet<TKey> (first);
			var copy = new NSMutableOrderedSet<TKey> (first);
			copy.MinusSet (second);
			return copy;
		}

		/// <summary>Computes the difference between two ordered sets.</summary>
		/// <param name="first">The ordered set to subtract from.</param>
		/// <param name="second">The ordered set to subtract.</param>
		/// <returns>A new <see cref="NSMutableOrderedSet{TKey}"/> containing objects in <paramref name="first"/> but not in <paramref name="second"/>, or <see langword="null"/> if <paramref name="first"/> is <see langword="null"/>.</returns>
		[return: NotNullIfNotNull (nameof (first))]
		public static NSMutableOrderedSet<TKey>? operator - (NSMutableOrderedSet<TKey>? first, NSOrderedSet<TKey>? second)
		{
			if (first is null)
				return null;
			if (second is null)
				return new NSMutableOrderedSet<TKey> (first);
			var copy = new NSMutableOrderedSet<TKey> (first);
			copy.MinusSet (second);
			return copy;
		}

#if false // https://github.com/dotnet/macios/issues/15577
		[SupportedOSPlatform ("ios13.0"), SupportedOSPlatform ("tvos13.0"), SupportedOSPlatform ("macos")]
		public void ApplyDifference (NSOrderedCollectionDifference<TKey> difference)
		{
			if (difference is null)
				throw new ArgumentNullException (nameof (difference));
			_ApplyDifference (difference.Handle);
		}
#endif
	}
}
