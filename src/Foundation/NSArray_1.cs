//
// This file contains a generic version of NSArray
//
// Authors:
//		Alex Soto	(alex.soto@xamarin.com)
//
// Copyright 2015, Xamarin Inc.
//

#nullable enable

using System;
using System.Collections.Generic;
using System.Collections;
using System.Runtime.InteropServices;
using System.Runtime.Versioning;

using ObjCRuntime;

namespace Foundation {
#if false // https://github.com/xamarin/xamarin-macios/issues/15577
	public delegate bool NSOrderedCollectionDifferenceEquivalenceTest<TValue> (TValue? first, TValue? second);
	internal delegate bool NSOrderedCollectionDifferenceEquivalenceTestProxy (IntPtr blockLiteral, /* NSObject */ IntPtr first, /* NSObject */ IntPtr second);
#endif
#if NET
	/// <typeparam name="TKey">To be added.</typeparam>
	///     <summary>To be added.</summary>
	///     <remarks>To be added.</remarks>
	[SupportedOSPlatform ("ios")]
	[SupportedOSPlatform ("maccatalyst")]
	[SupportedOSPlatform ("macos")]
	[SupportedOSPlatform ("tvos")]
#endif
	[Register (SkipRegistration = true)]
	public sealed partial class NSArray<TKey> : NSArray, IEnumerable<TKey>
		where TKey : class, INativeObject {

		/// <summary>To be added.</summary>
		///         <remarks>To be added.</remarks>
		public NSArray ()
		{
		}

		/// <param name="coder">The unarchiver object.</param>
		///         <summary>A constructor that initializes the object from the data stored in the unarchiver object.</summary>
		///         <remarks>
		///           <para>This constructor is provided to allow the class to be initialized from an unarchiver (for example, during NIB deserialization).   This is part of the <see cref="T:Foundation.NSCoding" />  protocol.</para>
		///           <para>If developers want to create a subclass of this object and continue to support deserialization from an archive, they should implement a constructor with an identical signature: taking a single parameter of type <see cref="T:Foundation.NSCoder" /> and decorate it with the [Export("initWithCoder:"] attribute declaration.</para>
		///           <para>The state of this object can also be serialized by using the companion method, EncodeTo.</para>
		///         </remarks>
		public NSArray (NSCoder coder) : base (coder)
		{
		}

		internal NSArray (NativeHandle handle) : base (handle)
		{
		}

		/// <param name="items">To be added.</param>
		///         <summary>To be added.</summary>
		///         <returns>To be added.</returns>
		///         <remarks>To be added.</remarks>
		static public NSArray<TKey> FromNSObjects (params TKey [] items)
		{
			if (items is null)
				throw new ArgumentNullException (nameof (items));

			return FromNSObjects (items.Length, items);
		}

		/// <param name="count">To be added.</param>
		///         <param name="items">To be added.</param>
		///         <summary>To be added.</summary>
		///         <returns>To be added.</returns>
		///         <remarks>To be added.</remarks>
		static public NSArray<TKey> FromNSObjects (int count, params TKey [] items)
		{
			if (items is null)
				throw new ArgumentNullException (nameof (items));

			if (count > items.Length)
				throw new ArgumentException ("count is larger than the number of items", "count");

			IntPtr buf = Marshal.AllocHGlobal ((IntPtr) (count * IntPtr.Size));
			for (nint i = 0; i < count; i++) {
				var item = items [i];
				// The analyzer cannot deal with arrays, we manually keep alive the whole array below
#pragma warning disable RBI0014
				IntPtr h = item is null ? NSNull.Null.Handle : item.Handle;
				Marshal.WriteIntPtr (buf, (int) (i * IntPtr.Size), h);
#pragma warning restore RBI0014
			}
			IntPtr ret = NSArray.FromObjects (buf, count);
			var arr = Runtime.GetNSObject<NSArray<TKey>> (ret)!;
			Marshal.FreeHGlobal (buf);
			GC.KeepAlive (items);
			return arr;
		}

		#region IEnumerable<TKey>
		/// <summary>Returns an enumerator that iterates through the array.</summary>
		/// <returns>An enumerator that can be used to iterate through the array.</returns>
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

		public TKey this [nint idx] {
			get {
				return GetItem<TKey> ((nuint) idx);
			}
		}

		public new TKey [] ToArray ()
		{
			return base.ToArray<TKey> ();
		}

#if false // https://github.com/xamarin/xamarin-macios/issues/15577

		[SupportedOSPlatform ("ios13.0"), SupportedOSPlatform ("tvos13.0"), SupportedOSPlatform ("macos")]
		public NSOrderedCollectionDifference<TKey>? GetDifference (TKey[] other, NSOrderedCollectionDifferenceCalculationOptions options)
			=> Runtime.GetNSObject <NSOrderedCollectionDifference<TKey>> (_GetDifference (NSArray.FromNSObjects (other), options));

		[SupportedOSPlatform ("ios13.0"), SupportedOSPlatform ("tvos13.0"), SupportedOSPlatform ("macos")]
		public NSOrderedCollectionDifference<TKey>? GetDifference (TKey[] other)
			=> Runtime.GetNSObject <NSOrderedCollectionDifference<TKey>> (_GetDifference (NSArray.FromNSObjects (other)));

		[SupportedOSPlatform ("ios13.0"), SupportedOSPlatform ("tvos13.0"), SupportedOSPlatform ("macos")]
		public TKey[]? GetArrayByApplyingDifference (NSOrderedCollectionDifference difference)
			=> NSArray.ArrayFromHandle<TKey> (_GetArrayByApplyingDifference (difference));
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
		public NSOrderedCollectionDifference<TKey>? GetDifferenceFromArray (NSArray<TKey> other, NSOrderedCollectionDifferenceCalculationOptions options, NSOrderedCollectionDifferenceEquivalenceTest<TKey> equivalenceTest) 
		{
			if (equivalenceTest is null)
				throw new ArgumentNullException (nameof (equivalenceTest));

			var block = new BlockLiteral ();
			block.SetupBlock (static_DiffEqualityGeneric, equivalenceTest);
			try {
				return Runtime.GetNSObject<NSOrderedCollectionDifference<TKey>> (_GetDifferenceFromArray (other, options, ref block));
			} finally {
				block.CleanupBlock ();
			}
		}
#endif
	}
}
