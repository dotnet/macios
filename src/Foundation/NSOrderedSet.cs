//
// NSOrderedSet.cs:
//
// Authors:
//   Miguel de Icaza
//
// Copyright 2013, Xamarin Inc
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

	public partial class NSOrderedSet : IEnumerable<NSObject> {
		const string selSetWithArray = "orderedSetWithArray:";

		/// <summary>Initializes a new instance of the <see cref="NSOrderedSet" /> class from an array of <see cref="NSObject" /> instances.</summary>
		/// <param name="objs">An array of <see cref="NSObject" /> instances to include in the set.</param>
		public NSOrderedSet (params NSObject? []? objs) : this (NSArray.FromNSObjects (objs))
		{
		}

		/// <summary>Initializes a new instance of the <see cref="NSOrderedSet" /> class from an array of objects.</summary>
		/// <param name="objs">An array of objects to include in the set.</param>
		public NSOrderedSet (params object [] objs) : this (NSArray.FromObjects (objs))
		{
		}

		/// <summary>Initializes a new instance of the <see cref="NSOrderedSet" /> class from an array of strings.</summary>
		/// <param name="strings">An array of strings to include in the set.</param>
		public NSOrderedSet (params string [] strings) : this (NSArray.FromStrings (strings))
		{
		}

		/// <summary>Gets the object at the specified index.</summary>
		/// <param name="idx">The zero-based index of the object to get.</param>
		/// <returns>The object at the specified index.</returns>
		public NSObject this [nint idx] {
			get {
				return GetObject (idx);
			}
		}

		/// <summary>Returns the contents of the ordered set as a strongly typed array.</summary>
		/// <typeparam name="T">The type of values in the array, must be a class that implements <see cref="INativeObject" />.</typeparam>
		/// <returns>An array of type <typeparamref name="T" /> with the contents of the ordered set.</returns>
		public T [] ToArray<T> () where T : class, INativeObject
		{
			IntPtr nsarr = _ToArray ();
			return NSArray.ArrayFromHandle<T> (nsarr);
		}

		/// <summary>Creates a new <see cref="NSOrderedSet" /> from an array of strongly typed values.</summary>
		/// <typeparam name="T">The type of values in the array, must be a class that derives from <see cref="NSObject" />.</typeparam>
		/// <param name="values">An array of strongly typed values to include in the ordered set.</param>
		/// <returns>A new <see cref="NSOrderedSet" /> containing the specified values.</returns>
		public static NSOrderedSet MakeNSOrderedSet<T> (T? []? values) where T : NSObject
		{
			NSArray a = NSArray.FromNSObjects (values);
			var result = (NSOrderedSet) Runtime.GetNSObject (ObjCRuntime.Messaging.IntPtr_objc_msgSend_IntPtr (class_ptr, Selector.GetHandle (selSetWithArray), a.Handle))!;
			GC.KeepAlive (a);
			return result;
		}

		/// <summary>Returns an enumerator that iterates through the set.</summary>
		/// <returns>An enumerator that can be used to iterate through the set.</returns>
		public IEnumerator<NSObject> GetEnumerator ()
		{
			var enumerator = _GetEnumerator ();
			NSObject obj;

			while ((obj = enumerator.NextObject ()) is not null)
				yield return obj as NSObject;
		}

		/// <summary>Returns an enumerator that iterates through the ordered set.</summary>
		/// <returns>An enumerator that can be used to iterate through the ordered set.</returns>
		IEnumerator IEnumerable.GetEnumerator ()
		{
			var enumerator = _GetEnumerator ();
			NSObject obj;

			while ((obj = enumerator.NextObject ()) is not null)
				yield return obj;
		}

		/// <summary>Adds two ordered sets together, creating a new ordered set that contains all elements from both sets.</summary>
		/// <param name="first">The first ordered set.</param>
		/// <param name="second">The second ordered set.</param>
		/// <returns>A new <see cref="NSOrderedSet" /> containing all elements from both ordered sets.</returns>
		public static NSOrderedSet? operator + (NSOrderedSet? first, NSOrderedSet? second)
		{
			if (first is null)
				return second is not null ? new NSOrderedSet (second) : null;
			if (second is null)
				return new NSOrderedSet (first);
			var copy = new NSMutableOrderedSet (first);
			copy.UnionSet (second);
			return copy;
		}

		/// <summary>Adds an ordered set and a set together, creating a new ordered set that contains all elements from both.</summary>
		/// <param name="first">The ordered set.</param>
		/// <param name="second">The set.</param>
		/// <returns>A new <see cref="NSOrderedSet" /> containing all elements from both the ordered set and the set.</returns>
		public static NSOrderedSet? operator + (NSOrderedSet? first, NSSet? second)
		{
			if (first is null)
				return second is not null ? new NSOrderedSet (second) : null;
			if (second is null)
				return new NSOrderedSet (first);
			var copy = new NSMutableOrderedSet (first);
			copy.UnionSet (second);
			return copy;
		}

		/// <summary>Subtracts the elements of the second ordered set from the first ordered set.</summary>
		/// <param name="first">The ordered set to subtract from.</param>
		/// <param name="second">The ordered set whose elements should be removed.</param>
		/// <returns>A new <see cref="NSOrderedSet" /> containing the elements in <paramref name="first" /> that are not in <paramref name="second" />, or <see langword="null" /> if <paramref name="first" /> is <see langword="null" />.</returns>
		public static NSOrderedSet? operator - (NSOrderedSet? first, NSOrderedSet? second)
		{
			if (first is null)
				return null;
			if (second is null)
				return new NSOrderedSet (first);
			var copy = new NSMutableOrderedSet (first);
			copy.MinusSet (second);
			return copy;
		}

		/// <summary>Subtracts the elements of a set from an ordered set.</summary>
		/// <param name="first">The ordered set to subtract from.</param>
		/// <param name="second">The set whose elements should be removed.</param>
		/// <returns>A new <see cref="NSOrderedSet" /> containing the elements in <paramref name="first" /> that are not in <paramref name="second" />, or <see langword="null" /> if <paramref name="first" /> is <see langword="null" />.</returns>
		public static NSOrderedSet? operator - (NSOrderedSet? first, NSSet? second)
		{
			if (first is null)
				return null;
			if (second is null)
				return new NSOrderedSet (first);
			var copy = new NSMutableOrderedSet (first);
			copy.MinusSet (second);
			return copy;
		}

		/// <summary>Determines whether two <see cref="NSOrderedSet" /> instances are equal.</summary>
		/// <param name="first">The first ordered set to compare.</param>
		/// <param name="second">The second ordered set to compare.</param>
		/// <returns><see langword="true" /> if both ordered sets are equal; otherwise, <see langword="false" />.</returns>
		public static bool operator == (NSOrderedSet? first, NSOrderedSet? second)
		{
			// IsEqualToOrderedSet does not allow null
			if (object.ReferenceEquals (null, first))
				return object.ReferenceEquals (null, second);
			if (object.ReferenceEquals (null, second))
				return false;

			return first.IsEqualToOrderedSet (second);
		}

		/// <summary>Determines whether two <see cref="NSOrderedSet" /> instances are not equal.</summary>
		/// <param name="first">The first ordered set to compare.</param>
		/// <param name="second">The second ordered set to compare.</param>
		/// <returns><see langword="true" /> if the ordered sets are not equal; otherwise, <see langword="false" />.</returns>
		public static bool operator != (NSOrderedSet? first, NSOrderedSet? second)
		{
			// IsEqualToOrderedSet does not allow null
			if (object.ReferenceEquals (null, first))
				return !object.ReferenceEquals (null, second);
			if (object.ReferenceEquals (null, second))
				return true;

			return !first.IsEqualToOrderedSet (second);
		}

		/// <summary>Determines whether this <see cref="NSOrderedSet" /> and the specified object are equal.</summary>
		/// <param name="other">The object to compare with this instance.</param>
		/// <returns><see langword="true" /> if <paramref name="other" /> is an <see cref="NSOrderedSet" /> and has the same elements in the same order as this instance; otherwise, <see langword="false" />.</returns>
		public override bool Equals (object? other)
		{
			var o = other as NSOrderedSet;
			if (o is null)
				return false;
			return IsEqualToOrderedSet (o);
		}

		/// <summary>Generates a hash code for the current instance.</summary>
		/// <returns>A 32-bit signed integer hash code for this instance.</returns>
		/// <remarks>The algorithm used to generate the hash code is unspecified.</remarks>
		public override int GetHashCode ()
		{
			return (int) GetNativeHash ();
		}

		/// <summary>Determines whether the ordered set contains the specified object.</summary>
		/// <param name="obj">The object to locate in the ordered set.</param>
		/// <returns><see langword="true" /> if the ordered set contains the specified object; otherwise, <see langword="false" />.</returns>
		public bool Contains (object? obj)
		{
			var nsobj = NSObject.FromObject (obj);
			if (nsobj is null)
				return false;
			return Contains (nsobj);
		}
	}

	public partial class NSMutableOrderedSet {
		/// <summary>Initializes a new instance of the <see cref="NSMutableOrderedSet" /> class from an array of <see cref="NSObject" /> instances.</summary>
		/// <param name="objs">An array of <see cref="NSObject" /> instances to include in the set.</param>
		public NSMutableOrderedSet (params NSObject? []? objs) : this (NSArray.FromNSObjects (objs))
		{
		}

		/// <summary>Initializes a new instance of the <see cref="NSMutableOrderedSet" /> class from an array of objects.</summary>
		/// <param name="objs">An array of objects to include in the set.</param>
		public NSMutableOrderedSet (params object [] objs) : this (NSArray.FromObjects (objs))
		{
		}

		/// <summary>Initializes a new instance of the <see cref="NSMutableOrderedSet" /> class from an array of strings.</summary>
		/// <param name="strings">An array of strings to include in the set.</param>
		public NSMutableOrderedSet (params string [] strings) : this (NSArray.FromStrings (strings))
		{
		}

		/// <summary>Gets or sets the object at the specified index.</summary>
		/// <param name="idx">The zero-based index of the object to get or set.</param>
		/// <returns>The object at the specified index.</returns>
		public new NSObject this [nint idx] {
			get {
				return GetObject (idx);
			}

			set {
				SetObject (value, idx);
			}
		}

#if false // https://github.com/dotnet/macios/issues/15577
		delegate bool NSOrderedCollectionDifferenceEquivalenceTestProxy (IntPtr blockLiteral, /* NSObject */ IntPtr first, /* NSObject */ IntPtr second);
		static readonly NSOrderedCollectionDifferenceEquivalenceTestProxy static_DiffEquality = DiffEqualityHandler;

		[MonoPInvokeCallback (typeof (NSOrderedCollectionDifferenceEquivalenceTestProxy))]
		static bool DiffEqualityHandler (IntPtr block, IntPtr first, IntPtr second)
		{
			var callback = BlockLiteral.GetTarget<NSOrderedCollectionDifferenceEquivalenceTest> (block);
			if (callback is not null) {
				var nsFirst = Runtime.GetNSObject<NSObject> (first, false);
				var nsSecond = Runtime.GetNSObject<NSObject> (second, false);
				return callback (nsFirst, nsSecond);
			}
			return false;
		}

		[SupportedOSPlatform ("ios13.0"), SupportedOSPlatform ("tvos13.0"), SupportedOSPlatform ("macos")]
		public NSOrderedCollectionDifference GetDifference (NSOrderedSet other, NSOrderedCollectionDifferenceCalculationOptions options, NSOrderedCollectionDifferenceEquivalenceTest equivalenceTest)
		{
			if (equivalenceTest is null)
				throw new ArgumentNullException (nameof (equivalenceTest));

			var block = new BlockLiteral ();
			block.SetupBlock (static_DiffEquality, equivalenceTest);
			try {
				return Runtime.GetNSObject<NSOrderedCollectionDifference> (_GetDifference (other, options, ref block));
			} finally {
				block.CleanupBlock ();
			}
		}
#endif
	}
}
