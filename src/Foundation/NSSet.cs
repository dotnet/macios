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

using System;
using System.Collections;
using System.Collections.Generic;

using ObjCRuntime;

// Disable until we get around to enable + fix any issues.
#nullable disable

namespace Foundation {

	public partial class NSSet : IEnumerable<NSObject> {
		/// <param name="objs">To be added.</param>
		///         <summary>To be added.</summary>
		///         <remarks>To be added.</remarks>
		public NSSet (params NSObject [] objs) : this (NSArray.FromNSObjects (objs))
		{
		}

		/// <param name="objs">To be added.</param>
		///         <summary>To be added.</summary>
		///         <remarks>To be added.</remarks>
		public NSSet (params object [] objs) : this (NSArray.FromObjects (objs))
		{
		}

		/// <param name="strings">An array of strings.</param>
		///         <summary>Creates a set from an array of strings.</summary>
		///         <remarks>The C# strings are stored as NSString objects in the set.</remarks>
		public NSSet (params string [] strings) : this (NSArray.FromStrings (strings))
		{
		}

		/// <typeparam name="T">Strongly typed version of the array that you want to get, must be a class that derives from <see cref="Foundation.NSObject" />.</typeparam>
		///         <summary>Returns the contents of the set as a strongly typed array.</summary>
		///         <returns>An array of type T with the contents of the set.</returns>
		///         <remarks>
		///           <para>
		/// 	    The following example shows how to get an array of UIFonts
		/// 	  </para>
		///           <example>
		///             <code lang="c#"><![CDATA[
		/// var myColors = new NSSet (UIColor.Red, UIColor.Blue, UIColor.Yellow);
		/// UIColor [] asArray = myColors.ToArray<UIColor> ();
		/// ]]></code>
		///           </example>
		///         </remarks>
		public T [] ToArray<T> () where T : class, INativeObject
		{
			IntPtr nsarr = _AllObjects ();
			return NSArray.ArrayFromHandle<T> (nsarr);
		}

		/// <typeparam name="T">To be added.</typeparam>
		///         <param name="values">To be added.</param>
		///         <summary>To be added.</summary>
		///         <returns>To be added.</returns>
		///         <remarks>To be added.</remarks>
		public static NSSet MakeNSObjectSet<T> (T [] values) where T : class, INativeObject
		{
			using (var a = NSArray.FromNSObjects (values))
				return Runtime.GetNSObject<NSSet> (_SetWithArray (a.Handle));
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
		/// <summary>Enumerate over the NSObjects in the set.</summary>
		///         <returns>
		///         </returns>
		///         <remarks>This returns an enumerator that returns the NSObject objects contained in the set.  They are returned as System.Object objects.</remarks>
		IEnumerator IEnumerable.GetEnumerator ()
		{
			return ((IEnumerable<NSObject>) this).GetEnumerator ();
		}
		#endregion

		public static NSSet operator + (NSSet first, NSSet second)
		{
			if (first is null)
				return new NSSet (second);
			if (second is null)
				return new NSSet (first);
			return first.SetByAddingObjectsFromSet (second);
		}

		public static NSSet operator + (NSSet first, NSOrderedSet second)
		{
			if (first is null)
				return new NSSet (second.AsSet ());
			if (second is null)
				return new NSSet (first);
			var copy = new NSMutableSet (first);
			copy.UnionSet (second.AsSet ());
			return copy;
		}

		public static NSSet operator - (NSSet first, NSSet second)
		{
			if (first is null)
				return null;
			if (second is null)
				return new NSSet (first);
			var copy = new NSMutableSet (first);
			copy.MinusSet (second);
			return copy;
		}

		public static NSSet operator - (NSSet first, NSOrderedSet second)
		{
			if (first is null)
				return null;
			if (second is null)
				return new NSSet (first);
			var copy = new NSMutableSet (first);
			copy.MinusSet (second.AsSet ());
			return copy;
		}

		/// <param name="obj">To be added.</param>
		///         <summary>To be added.</summary>
		///         <returns>To be added.</returns>
		///         <remarks>To be added.</remarks>
		public bool Contains (object obj)
		{
			return Contains (NSObject.FromObject (obj));
		}
	}
}
