//
// This file describes the API that the generator will produce
//
// Authors:
//   Miguel de Icaza
//
// Copyright 2012, Xamarin Inc.
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

	public partial class NSMutableSet : IEnumerable<NSObject> {
		/// <summary>Initializes a new mutable set with the specified objects.</summary>
		/// <param name="objs">The objects to add to the set.</param>
		public NSMutableSet (params NSObject? []? objs)
			: this (NSArray.FromNSObjects (objs))
		{
		}

		/// <summary>Initializes a new mutable set with the specified strings.</summary>
		/// <param name="strings">The strings to add to the set.</param>
		public NSMutableSet (params string [] strings)
			: this (NSArray.FromStrings (strings))
		{
		}

		internal NSMutableSet (params INativeObject? []? objs)
			: this (NSArray.FromNSObjects (objs))
		{
		}

		/// <summary>Creates a new mutable set containing all objects from both sets.</summary>
		/// <param name="first">The first set.</param>
		/// <param name="second">The second set.</param>
		/// <returns>A new <see cref="NSMutableSet" /> containing the union of both sets, or <see langword="null" /> if both sets are <see langword="null" />.</returns>
		[return: NotNullIfNotNull (nameof (first))]
		[return: NotNullIfNotNull (nameof (second))]
		public static NSMutableSet? operator + (NSMutableSet? first, NSMutableSet? second)
		{
			if (first is null && second is null)
				return null;
			if (first is null || first.Count == 0)
				return second is null ? new NSMutableSet () : new NSMutableSet (second);
			if (second is null || second.Count == 0)
				return new NSMutableSet (first);

			var copy = new NSMutableSet (first);
			copy.UnionSet (second);
			return copy;
		}

		/// <summary>Creates a new mutable set with objects from the first set that are not in the second set.</summary>
		/// <param name="first">The first set.</param>
		/// <param name="second">The second set.</param>
		/// <returns>A new <see cref="NSMutableSet" /> containing the difference, or <see langword="null" /> if the first set is <see langword="null" />.</returns>
		[return: NotNullIfNotNull (nameof (first))]
		public static NSMutableSet? operator - (NSMutableSet? first, NSMutableSet? second)
		{
			if (first is null)
				return null;
			if (first.Count == 0)
				return new NSMutableSet ();
			if (second is null || second.Count == 0)
				return new NSMutableSet (first);

			var copy = new NSMutableSet (first);
			copy.MinusSet (second);
			return copy;
		}
	}
}
