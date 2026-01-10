//
// This file describes the API that the generator will produce
//
// Authors:
//   Miguel de Icaza
//
// Copyright 2009, Novell, Inc.
// Copyright 2010, Novell, Inc.
// Copyright 2012, 2015, Xamarin Inc
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

	public partial class NSSet : IEnumerable<NSObject> {
		/// <summary>Creates a new <see cref="NSSet" /> from an array of <see cref="NSObject" /> instances.</summary>
		/// <param name="objs">An array of <see cref="NSObject" /> instances to include in the set.</param>
		public NSSet (params NSObject [] objs) : this (NSArray.FromNSObjects (objs))
		{
		}

		/// <summary>Creates a new <see cref="NSSet" /> from an array of objects.</summary>
		/// <param name="objs">An array of objects to include in the set.</param>
		public NSSet (params object [] objs) : this (NSArray.FromObjects (objs))
		{
		}

		/// <summary>Creates a new <see cref="NSSet" /> from an array of strings.</summary>
		/// <param name="strings">An array of strings.</param>
		/// <remarks>The C# strings are stored as <see cref="NSString" /> objects in the set.</remarks>
		public NSSet (params string [] strings) : this (NSArray.FromStrings (strings))
		{
		}

		/// <summary>Returns the contents of the set as a strongly typed array.</summary>
		/// <typeparam name="T">Strongly typed version of the array that you want to get, must be a class that derives from <see cref="NSObject" />.</typeparam>
		/// <returns>An array of type <typeparamref name="T" /> with the contents of the set.</returns>
		/// <remarks>
		/// <para>The following example shows how to get an array of UIFonts:</para>
		///   <example>
		///   <code lang="c#"><![CDATA[
		/// var myColors = new NSSet (UIColor.Red, UIColor.Blue, UIColor.Yellow);
		/// UIColor [] asArray = myColors.ToArray<UIColor> ();
		/// ]]></code>
		///   </example>
		/// </remarks>
		public T [] ToArray<T> () where T : class, INativeObject
		{
			IntPtr nsarr = _AllObjects ();
			return NSArray.ArrayFromHandle<T> (nsarr);
		}

		/// <summary>Creates a new <see cref="NSSet" /> from an array of strongly typed values.</summary>
		/// <typeparam name="T">The type of values in the array, must be a class that derives from <see cref="NSObject" />.</typeparam>
		/// <param name="values">An array of strongly typed values to include in the set.</param>
		/// <returns>A new <see cref="NSSet" /> containing the specified values.</returns>
		public static NSSet MakeNSObjectSet<T> (T [] values) where T : class, INativeObject
		{
			using (var a = NSArray.FromNSObjects (values))
				return Runtime.GetNSObject<NSSet> (_SetWithArray (a.Handle))!;
		}

		#region IEnumerable<T>
		/// <summary>Returns an enumerator that iterates through the set.</summary>
		/// <returns>An enumerator that can be used to iterate through the set.</returns>
		public IEnumerator<NSObject> GetEnumerator ()
		{
			var enumerator = _GetEnumerator ();
			NSObject obj;

			while ((obj = enumerator.NextObject ()) is not null)
				yield return obj as NSObject;
		}
		#endregion

		#region IEnumerable
		/// <summary>Enumerates over the <see cref="NSObject" /> instances in the set.</summary>
		/// <returns>An enumerator that can be used to iterate through the set.</returns>
		/// <remarks>This returns an enumerator that returns the <see cref="NSObject" /> objects contained in the set. They are returned as <see cref="System.Object" /> objects.</remarks>
		IEnumerator IEnumerable.GetEnumerator ()
		{
			return ((IEnumerable<NSObject>) this).GetEnumerator ();
		}
		#endregion

		/// <summary>Adds two sets together, creating a new set that contains all elements from both sets.</summary>
		/// <param name="first">The first set.</param>
		/// <param name="second">The second set.</param>
		/// <returns>A new <see cref="NSSet" /> containing all elements from both sets.</returns>
		public static NSSet? operator + (NSSet? first, NSSet? second)
		{
			if (first is null)
				return second is not null ? new NSSet (second) : null;
			if (second is null)
				return new NSSet (first);
			return first.SetByAddingObjectsFromSet (second);
		}

		/// <summary>Adds a set and an ordered set together, creating a new set that contains all elements from both.</summary>
		/// <param name="first">The set.</param>
		/// <param name="second">The ordered set.</param>
		/// <returns>A new <see cref="NSSet" /> containing all elements from both the set and the ordered set.</returns>
		public static NSSet? operator + (NSSet? first, NSOrderedSet? second)
		{
			if (first is null)
				return second is not null ? new NSSet (second.AsSet ()) : null;
			if (second is null)
				return new NSSet (first);
			var copy = new NSMutableSet (first);
			copy.UnionSet (second.AsSet ());
			return copy;
		}

		/// <summary>Subtracts the elements of the second set from the first set.</summary>
		/// <param name="first">The set to subtract from.</param>
		/// <param name="second">The set whose elements should be removed.</param>
		/// <returns>A new <see cref="NSSet" /> containing the elements in <paramref name="first" /> that are not in <paramref name="second" />, or <see langword="null" /> if <paramref name="first" /> is <see langword="null" />.</returns>
		public static NSSet? operator - (NSSet? first, NSSet? second)
		{
			if (first is null)
				return null;
			if (second is null)
				return new NSSet (first);
			var copy = new NSMutableSet (first);
			copy.MinusSet (second);
			return copy;
		}

		/// <summary>Subtracts the elements of an ordered set from a set.</summary>
		/// <param name="first">The set to subtract from.</param>
		/// <param name="second">The ordered set whose elements should be removed.</param>
		/// <returns>A new <see cref="NSSet" /> containing the elements in <paramref name="first" /> that are not in <paramref name="second" />, or <see langword="null" /> if <paramref name="first" /> is <see langword="null" />.</returns>
		public static NSSet? operator - (NSSet? first, NSOrderedSet? second)
		{
			if (first is null)
				return null;
			if (second is null)
				return new NSSet (first);
			var copy = new NSMutableSet (first);
			copy.MinusSet (second.AsSet ());
			return copy;
		}

		/// <summary>Determines whether the set contains the specified object.</summary>
		/// <param name="obj">The object to locate in the set.</param>
		/// <returns><see langword="true" /> if the set contains the specified object; otherwise, <see langword="false" />.</returns>
		public bool Contains (object? obj)
		{
			var nsobj = NSObject.FromObject (obj);
			if (nsobj is null)
				return false;
			return Contains (nsobj);
		}
	}
}
