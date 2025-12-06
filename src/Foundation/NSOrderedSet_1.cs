//
// This file contains a generic version of NSOrderedSet
//
// Authors:
//		Alex Soto	(alex.soto@xamarin.com)
//
// Copyright 2015, Xamarin Inc.
//
#nullable enable

using System.Collections.Generic;
using System.Collections;

namespace Foundation {
	[Register ("NSOrderedSet", SkipRegistration = true)]
	public sealed partial class NSOrderedSet<TKey> : NSOrderedSet, IEnumerable<TKey>
		where TKey : class, INativeObject {

		/// <summary>Initializes a new empty instance of the <see cref="NSOrderedSet{TKey}"/> class.</summary>
		public NSOrderedSet ()
		{
		}

		/// <summary>A constructor that initializes the object from the data stored in the unarchiver object.</summary>
		/// <param name="coder">The unarchiver object.</param>
		/// <remarks>
		/// <para>This constructor is provided to allow the class to be initialized from an unarchiver (for example, during NIB deserialization). This is part of the <see cref="Foundation.NSCoding" /> protocol.</para>
		/// <para>If developers want to create a subclass of this object and continue to support deserialization from an archive, they should implement a constructor with an identical signature: taking a single parameter of type <see cref="Foundation.NSCoder" /> and decorate it with the [Export("initWithCoder:"] attribute declaration.</para>
		/// <para>The state of this object can also be serialized by using the companion method, EncodeTo.</para>
		/// </remarks>
		public NSOrderedSet (NSCoder coder) : base (coder)
		{
		}

		internal NSOrderedSet (NativeHandle handle) : base (handle)
		{
		}

		/// <summary>Initializes a new instance of the <see cref="NSOrderedSet{TKey}"/> class with a single object.</summary>
		/// <param name="start">The object to add to the set.</param>
		public NSOrderedSet (TKey start) : base (start)
		{
		}

		/// <summary>Initializes a new instance of the <see cref="NSOrderedSet{TKey}"/> class with the specified objects.</summary>
		/// <param name="objs">The objects to add to the set.</param>
		public NSOrderedSet (params TKey [] objs) : base (objs)
		{
		}

		/// <summary>Initializes a new instance of the <see cref="NSOrderedSet{TKey}"/> class from an <see cref="NSSet{TKey}"/>.</summary>
		/// <param name="source">The source set.</param>
		public NSOrderedSet (NSSet<TKey> source) : base (source)
		{
		}

		/// <summary>Initializes a new instance of the <see cref="NSOrderedSet{TKey}"/> class by copying another ordered set.</summary>
		/// <param name="other">The ordered set to copy.</param>
		public NSOrderedSet (NSOrderedSet<TKey> other) : base (other)
		{
		}

		/// <summary>Initializes a new instance of the <see cref="NSOrderedSet{TKey}"/> class by copying a mutable ordered set.</summary>
		/// <param name="other">The mutable ordered set to copy.</param>
		public NSOrderedSet (NSMutableOrderedSet<TKey> other) : base (other)
		{
		}

		/// <summary>Gets the object at the specified index.</summary>
		/// <param name="idx">The index of the object to retrieve.</param>
		/// <returns>The object at the specified index, or <see langword="null"/> if the index is out of bounds.</returns>
		public new TKey? this [nint idx] {
			get {
				var ret = _GetObject (idx);
				return Runtime.GetINativeObject<TKey> (ret, false);
			}
		}

		/// <summary>Converts the ordered set to an array.</summary>
		/// <returns>An array containing all the objects in the ordered set.</returns>
		public TKey [] ToArray ()
		{
			return base.ToArray<TKey> ();
		}

		/// <summary>Determines whether the ordered set contains the specified object.</summary>
		/// <param name="obj">The object to locate in the ordered set.</param>
		/// <returns><see langword="true"/> if the object is found in the ordered set; otherwise, <see langword="false"/>.</returns>
		public bool Contains (TKey obj)
		{
			if (obj is null)
				throw new ArgumentNullException (nameof (obj));

			bool result = _Contains (obj.Handle);
			GC.KeepAlive (obj);
			return result;
		}

		/// <summary>Returns the index of the specified object.</summary>
		/// <param name="obj">The object to locate in the ordered set.</param>
		/// <returns>The zero-based index of the object, or <see cref="nint.MaxValue" /> (<see href="https://developer.apple.com/documentation/foundation/nsnotfound">NSNotFound</see>) if the object is not found.</returns>
		public nint IndexOf (TKey obj)
		{
			if (obj is null)
				throw new ArgumentNullException (nameof (obj));

			nint result = _IndexOf (obj.Handle);
			GC.KeepAlive (obj);
			return result;
		}

		/// <summary>Gets the first object in the ordered set.</summary>
		/// <returns>The first object, or <see langword="null"/> if the set is empty.</returns>
		public TKey? FirstObject ()
		{
			var ret = _FirstObject ();
			return Runtime.GetINativeObject<TKey> (ret, false);
		}

		/// <summary>Gets the last object in the ordered set.</summary>
		/// <returns>The last object, or <see langword="null"/> if the set is empty.</returns>
		public TKey? LastObject ()
		{
			var ret = _LastObject ();
			return Runtime.GetINativeObject<TKey> (ret, false);
		}

		/// <summary>Converts the ordered set to an unordered <see cref="NSSet{TKey}"/>.</summary>
		/// <returns>An unordered set containing all the objects from the ordered set.</returns>
		public NSSet<TKey> AsSet ()
		{
			var ret = _AsSet ();
			return Runtime.GetINativeObject<NSSet<TKey>> (ret, false)!;
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

		/// <summary>Combines two ordered sets into a new ordered set.</summary>
		/// <param name="first">The first ordered set.</param>
		/// <param name="second">The second ordered set.</param>
		/// <returns>A new ordered set containing the union of both sets, or <see langword="null"/> if both sets are <see langword="null"/>.</returns>
		public static NSOrderedSet<TKey>? operator + (NSOrderedSet<TKey>? first, NSOrderedSet<TKey>? second)
		{
			if (first is null)
				return second is not null ? new NSOrderedSet<TKey> (second) : null;
			if (second is null)
				return new NSOrderedSet<TKey> (first);
			var copy = new NSMutableOrderedSet<TKey> (first);
			copy.UnionSet (second);
			var copyset = new NSOrderedSet<TKey> (copy);
			return copyset;
		}

		/// <summary>Combines an ordered set with an unordered set into a new ordered set.</summary>
		/// <param name="first">The ordered set.</param>
		/// <param name="second">The unordered set.</param>
		/// <returns>A new ordered set containing the union of both sets, or <see langword="null"/> if both sets are <see langword="null"/>.</returns>
		public static NSOrderedSet<TKey>? operator + (NSOrderedSet<TKey>? first, NSSet<TKey>? second)
		{
			if (first is null)
				return second is not null ? new NSOrderedSet<TKey> (second) : null;
			if (second is null)
				return new NSOrderedSet<TKey> (first);
			var copy = new NSMutableOrderedSet<TKey> (first);
			copy.UnionSet (second);
			var copyset = new NSOrderedSet<TKey> (copy);
			return copyset;
		}

		/// <summary>Subtracts one ordered set from another.</summary>
		/// <param name="first">The ordered set to subtract from.</param>
		/// <param name="second">The ordered set to subtract.</param>
		/// <returns>A new ordered set containing the elements in the first set that are not in the second set, or <see langword="null"/> if the first set is <see langword="null"/>.</returns>
		public static NSOrderedSet<TKey>? operator - (NSOrderedSet<TKey>? first, NSOrderedSet<TKey>? second)
		{
			if (first is null)
				return null;
			if (second is null)
				return new NSOrderedSet<TKey> (first);
			var copy = new NSMutableOrderedSet<TKey> (first);
			copy.MinusSet (second);
			var copyset = new NSOrderedSet<TKey> (copy);
			return copyset;
		}

		/// <summary>Subtracts an unordered set from an ordered set.</summary>
		/// <param name="first">The ordered set to subtract from.</param>
		/// <param name="second">The unordered set to subtract.</param>
		/// <returns>A new ordered set containing the elements in the first set that are not in the second set, or <see langword="null"/> if the first set is <see langword="null"/>.</returns>
		public static NSOrderedSet<TKey>? operator - (NSOrderedSet<TKey>? first, NSSet<TKey>? second)
		{
			if (first is null)
				return null;
			if (second is null)
				return new NSOrderedSet<TKey> (first);
			var copy = new NSMutableOrderedSet<TKey> (first);
			copy.MinusSet (second);
			var copyset = new NSOrderedSet<TKey> (copy);
			return copyset;
		}

		/// <summary>Determines whether two ordered sets are equal.</summary>
		/// <param name="first">The first ordered set to compare.</param>
		/// <param name="second">The second ordered set to compare.</param>
		/// <returns><see langword="true"/> if the sets are equal; otherwise, <see langword="false"/>.</returns>
		public static bool operator == (NSOrderedSet<TKey> first, NSOrderedSet<TKey> second)
		{
			// IsEqualToOrderedSet does not allow null
			if (object.ReferenceEquals (null, first))
				return object.ReferenceEquals (null, second);
			if (object.ReferenceEquals (null, second))
				return false;

			return first.IsEqualToOrderedSet (second);
		}

		/// <summary>Determines whether two ordered sets are not equal.</summary>
		/// <param name="first">The first ordered set to compare.</param>
		/// <param name="second">The second ordered set to compare.</param>
		/// <returns><see langword="true"/> if the sets are not equal; otherwise, <see langword="false"/>.</returns>
		public static bool operator != (NSOrderedSet<TKey> first, NSOrderedSet<TKey> second)
		{
			// IsEqualToOrderedSet does not allow null
			if (object.ReferenceEquals (null, first))
				return !object.ReferenceEquals (null, second);
			if (object.ReferenceEquals (null, second))
				return true;

			return !first.IsEqualToOrderedSet (second);
		}

		/// <summary>Determines whether the specified object is equal to the current ordered set.</summary>
		/// <param name="other">The object to compare with the current ordered set.</param>
		/// <returns><see langword="true"/> if the specified object is equal to the current ordered set; otherwise, <see langword="false"/>.</returns>
		public override bool Equals (object other)
		{
			var o = other as NSOrderedSet<TKey>;
			if (o is null)
				return false;
			return IsEqualToOrderedSet (o);
		}

		/// <summary>Generates a hash code for the current instance.</summary>
		/// <returns>An int containing the hash code for this instance.</returns>
		/// <remarks>The algorithm used to generate the hash code is unspecified.</remarks>
		public override int GetHashCode ()
		{
			return (int) GetNativeHash ();
		}

#if false // https://github.com/dotnet/macios/issues/15577

		[SupportedOSPlatform ("ios13.0"), SupportedOSPlatform ("tvos13.0"), SupportedOSPlatform ("macos")]
		public NSOrderedCollectionDifference<TKey> GetDifference (NSOrderedSet<TKey> other, NSOrderedCollectionDifferenceCalculationOptions options)
			=> new NSOrderedCollectionDifference<TKey> (_GetDifference (other, options));

		[SupportedOSPlatform ("ios13.0"), SupportedOSPlatform ("tvos13.0"), SupportedOSPlatform ("macos")]
		public NSOrderedCollectionDifference<TKey> GetDifference (NSOrderedSet other)
			=> new NSOrderedCollectionDifference<TKey> (_GetDifference (other));

		[SupportedOSPlatform ("ios13.0"), SupportedOSPlatform ("tvos13.0"), SupportedOSPlatform ("macos")]
		public NSOrderedSet<TKey>? GetOrderedSet (NSOrderedCollectionDifference difference)
		{
			var ptr = _GetOrderedSet (difference); 
			return Runtime.GetNSObject<NSOrderedSet<TKey>> (ptr);
		}

		static readonly NSOrderedCollectionDifferenceEquivalenceTestProxy static_DiffEqualityGeneric = DiffEqualityHandlerGeneric;

		[MonoPInvokeCallback (typeof (NSOrderedCollectionDifferenceEquivalenceTestProxy))]
		static bool DiffEqualityHandlerGeneric (IntPtr block, IntPtr first, IntPtr second)
		{
			var callback = BlockLiteral.GetTarget<NSOrderedCollectionDifferenceEquivalenceTest<TKey>> (block);
			if (callback is not null) {
				var nsFirst = Runtime.GetINativeObject<TKey> (first, false);
				var nsSecond = Runtime.GetINativeObject<TKey> (second, false);
				return callback (nsFirst, nsSecond);
			}
			return false;
		}

		[SupportedOSPlatform ("ios13.0"), SupportedOSPlatform ("tvos13.0"), SupportedOSPlatform ("macos")]
		public NSOrderedCollectionDifference<TKey>? GetDifference (NSOrderedSet<TKey> other, NSOrderedCollectionDifferenceCalculationOptions options, NSOrderedCollectionDifferenceEquivalenceTest<TKey> equivalenceTest) 
		{
			if (equivalenceTest is null)
				throw new ArgumentNullException (nameof (equivalenceTest));

			var block = new BlockLiteral ();
			block.SetupBlock (static_DiffEqualityGeneric, equivalenceTest);
			try {
				return Runtime.GetNSObject<NSOrderedCollectionDifference<TKey>> (_GetDifference (other, options, ref block));
			} finally {
				block.CleanupBlock ();
			}
		}
#endif
	}
}
