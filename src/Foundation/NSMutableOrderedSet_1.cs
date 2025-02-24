//
// This file contains a generic version of NSMutableOrderedSet
//
// Authors:
//		Alex Soto	(alex.soto@xamarin.com)
//
// Copyright 2015, Xamarin Inc.
//

using System;
using System.Collections.Generic;
using System.Collections;
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
	[Register ("NSMutableOrderedSet", SkipRegistration = true)]
	public sealed partial class NSMutableOrderedSet<TKey> : NSMutableOrderedSet, IEnumerable<TKey>
		where TKey : class, INativeObject {

		/// <summary>To be added.</summary>
		///         <remarks>To be added.</remarks>
		public NSMutableOrderedSet ()
		{
		}

		/// <param name="coder">The unarchiver object.</param>
		///         <summary>A constructor that initializes the object from the data stored in the unarchiver object.</summary>
		///         <remarks>
		///           <para>This constructor is provided to allow the class to be initialized from an unarchiver (for example, during NIB deserialization).   This is part of the <see cref="T:Foundation.NSCoding" />  protocol.</para>
		///           <para>If developers want to create a subclass of this object and continue to support deserialization from an archive, they should implement a constructor with an identical signature: taking a single parameter of type <see cref="T:Foundation.NSCoder" /> and decorate it with the [Export("initWithCoder:"] attribute declaration.</para>
		///           <para>The state of this object can also be serialized by using the companion method, EncodeTo.</para>
		///         </remarks>
		public NSMutableOrderedSet (NSCoder coder) : base (coder)
		{
		}

		internal NSMutableOrderedSet (NativeHandle handle) : base (handle)
		{
		}

		public NSMutableOrderedSet (nint capacity) : base (capacity)
		{
		}

		/// <param name="start">To be added.</param>
		///         <summary>To be added.</summary>
		///         <remarks>To be added.</remarks>
		public NSMutableOrderedSet (TKey start) : base (start)
		{
		}

		/// <param name="objs">To be added.</param>
		///         <summary>To be added.</summary>
		///         <remarks>To be added.</remarks>
		public NSMutableOrderedSet (params TKey [] objs) : base (objs)
		{
		}

		/// <param name="source">To be added.</param>
		///         <summary>To be added.</summary>
		///         <remarks>To be added.</remarks>
		public NSMutableOrderedSet (NSSet<TKey> source) : base (source)
		{
		}

		/// <param name="other">To be added.</param>
		///         <summary>To be added.</summary>
		///         <remarks>To be added.</remarks>
		public NSMutableOrderedSet (NSOrderedSet<TKey> other) : base (other)
		{
		}

		/// <param name="other">To be added.</param>
		///         <summary>To be added.</summary>
		///         <remarks>To be added.</remarks>
		public NSMutableOrderedSet (NSMutableOrderedSet<TKey> other) : base (other)
		{
		}

		public new TKey this [nint idx] {
			get {
				var ret = _GetObject (idx);
				return Runtime.GetINativeObject<TKey> (ret, false);
			}

			set {
				if (value is null) // You can't pass nil here
					throw new ArgumentNullException (nameof (value));

				_SetObject (value.Handle, idx);
				GC.KeepAlive (value);
			}
		}

		/// <summary>To be added.</summary>
		///         <returns>To be added.</returns>
		///         <remarks>To be added.</remarks>
		public NSSet<TKey> AsSet ()
		{
			var ret = _AsSet ();
			return Runtime.GetINativeObject<NSSet<TKey>> (ret, false);
		}

		public void Insert (TKey obj, nint atIndex)
		{
			if (obj is null)
				throw new ArgumentNullException (nameof (obj));

			_Insert (obj.Handle, atIndex);
			GC.KeepAlive (obj);
		}

		public void Replace (nint objectAtIndex, TKey newObject)
		{
			if (newObject is null)
				throw new ArgumentNullException (nameof (newObject));

			_Replace (objectAtIndex, newObject.Handle);
			GC.KeepAlive (newObject);
		}

		/// <param name="obj">To be added.</param>
		///         <summary>To be added.</summary>
		///         <remarks>To be added.</remarks>
		public void Add (TKey obj)
		{
			if (obj is null)
				throw new ArgumentNullException (nameof (obj));

			_Add (obj.Handle);
			GC.KeepAlive (obj);
		}

		/// <param name="source">To be added.</param>
		///         <summary>To be added.</summary>
		///         <remarks>To be added.</remarks>
		public void AddObjects (params TKey [] source)
		{
			if (source is null)
				throw new ArgumentNullException (nameof (source));

			_AddObjects (NSArray.FromNativeObjects (source));
		}

		/// <param name="objects">To be added.</param>
		///         <param name="atIndexes">To be added.</param>
		///         <summary>To be added.</summary>
		///         <remarks>To be added.</remarks>
		public void InsertObjects (TKey [] objects, NSIndexSet atIndexes)
		{
			if (objects is null)
				throw new ArgumentNullException (nameof (objects));
			if (atIndexes is null)
				throw new ArgumentNullException (nameof (atIndexes));

			_InsertObjects (NSArray.FromNativeObjects (objects), atIndexes);
		}

		/// <param name="indexSet">To be added.</param>
		///         <param name="replacementObjects">To be added.</param>
		///         <summary>To be added.</summary>
		///         <remarks>To be added.</remarks>
		public void ReplaceObjects (NSIndexSet indexSet, params TKey [] replacementObjects)
		{
			if (replacementObjects is null)
				throw new ArgumentNullException (nameof (replacementObjects));
			if (indexSet is null)
				throw new ArgumentNullException (nameof (indexSet));

			_ReplaceObjects (indexSet, NSArray.FromNativeObjects (replacementObjects));
		}

		/// <param name="obj">To be added.</param>
		///         <summary>To be added.</summary>
		///         <remarks>To be added.</remarks>
		public void RemoveObject (TKey obj)
		{
			if (obj is null)
				throw new ArgumentNullException (nameof (obj));

			_RemoveObject (obj.Handle);
			GC.KeepAlive (obj);
		}

		/// <param name="objects">To be added.</param>
		///         <summary>To be added.</summary>
		///         <remarks>To be added.</remarks>
		public void RemoveObjects (params TKey [] objects)
		{
			if (objects is null)
				throw new ArgumentNullException (nameof (objects));

			_RemoveObjects (NSArray.FromNativeObjects (objects));
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

		public static NSMutableOrderedSet<TKey> operator + (NSMutableOrderedSet<TKey> first, NSMutableOrderedSet<TKey> second)
		{
			if (first is null)
				return second is not null ? new NSMutableOrderedSet<TKey> (second) : null;
			if (second is null)
				return new NSMutableOrderedSet<TKey> (first);
			var copy = new NSMutableOrderedSet<TKey> (first);
			copy.UnionSet (second);
			return copy;
		}

		public static NSMutableOrderedSet<TKey> operator + (NSMutableOrderedSet<TKey> first, NSSet<TKey> second)
		{
			if (first is null)
				return second is not null ? new NSMutableOrderedSet<TKey> (second) : null;
			if (second is null)
				return new NSMutableOrderedSet<TKey> (first);
			var copy = new NSMutableOrderedSet<TKey> (first);
			copy.UnionSet (second);
			return copy;
		}

		public static NSMutableOrderedSet<TKey> operator + (NSMutableOrderedSet<TKey> first, NSOrderedSet<TKey> second)
		{
			if (first is null)
				return second is not null ? new NSMutableOrderedSet<TKey> (second) : null;
			if (second is null)
				return new NSMutableOrderedSet<TKey> (first);
			var copy = new NSMutableOrderedSet<TKey> (first);
			copy.UnionSet (second);
			return copy;
		}

		public static NSMutableOrderedSet<TKey> operator - (NSMutableOrderedSet<TKey> first, NSMutableOrderedSet<TKey> second)
		{
			if (first is null)
				return null;
			if (second is null)
				return new NSMutableOrderedSet<TKey> (first);
			var copy = new NSMutableOrderedSet<TKey> (first);
			copy.MinusSet (second);
			return copy;
		}

		public static NSMutableOrderedSet<TKey> operator - (NSMutableOrderedSet<TKey> first, NSSet<TKey> second)
		{
			if (first is null)
				return null;
			if (second is null)
				return new NSMutableOrderedSet<TKey> (first);
			var copy = new NSMutableOrderedSet<TKey> (first);
			copy.MinusSet (second);
			return copy;
		}

		public static NSMutableOrderedSet<TKey> operator - (NSMutableOrderedSet<TKey> first, NSOrderedSet<TKey> second)
		{
			if (first is null)
				return null;
			if (second is null)
				return new NSMutableOrderedSet<TKey> (first);
			var copy = new NSMutableOrderedSet<TKey> (first);
			copy.MinusSet (second);
			return copy;
		}

#if false // https://github.com/xamarin/xamarin-macios/issues/15577
#if !NET
		[TV (13,0), iOS (13,0)]
#else
		[SupportedOSPlatform ("ios13.0"), SupportedOSPlatform ("tvos13.0"), SupportedOSPlatform ("macos")]
#endif
		public void ApplyDifference (NSOrderedCollectionDifference<TKey> difference)
		{
			if (difference is null)
				throw new ArgumentNullException (nameof (difference));
			_ApplyDifference (difference.Handle);
		}
#endif
	}
}
