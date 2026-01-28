//
// Copyright 2009-2010, Novell, Inc.
// Copyright 2011-2013 Xamarin Inc
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
using System.Reflection;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics.CodeAnalysis;

using CoreFoundation;

// Disable until we get around to enable + fix any issues.
#nullable disable

namespace Foundation {

#if false // https://github.com/dotnet/macios/issues/15577
	public delegate bool NSOrderedCollectionDifferenceEquivalenceTest (NSObject first, NSObject second);
#endif

	public partial class NSArray : IEnumerable<NSObject> {

#nullable enable
		/// <summary>Creates an NSArray from a C# array of NSObjects.</summary>
		/// <param name="items">Strongly typed array of NSObjects. Null elements are stored as <see cref="NSNull.Null"/>. If the array itself is null, an empty <see cref="NSArray"/> is returned.</param>
		/// <returns>A new <see cref="NSArray"/> containing the specified objects.</returns>
		/// <remarks>Null items in the array are converted to <see cref="NSNull.Null"/> instances.</remarks>
		public static NSArray FromNSObjects (params NSObject? []? items)
		{
			return FromNativeObjects (items);
		}

#if !XAMCORE_5_0
		/// <summary>Creates an NSArray from the first <paramref name="count"/> elements of a C# array of NSObjects.</summary>
		/// <param name="count">Number of items to copy from the items array.</param>
		/// <param name="items">Strongly typed array of NSObjects. Null elements are stored as <see cref="NSNull.Null"/>. If the array itself is null, an empty <see cref="NSArray"/> is returned.</param>
		/// <returns>A new <see cref="NSArray"/> containing the first <paramref name="count"/> objects from the array.</returns>
		/// <remarks>Null items in the array are converted to <see cref="NSNull.Null"/> instances.</remarks>
		[EditorBrowsable (EditorBrowsableState.Never)]
		// Don't obsolete this method, because forcing the first parameter to a 'nint' to pick the good overload to avoid the obsolete warning makes the calling code ugly.
		public static NSArray FromNSObjects (int count, params NSObject? []? items)
		{
			return FromNativeObjects (items, count);
		}
#endif

		/// <summary>Creates an NSArray from the first <paramref name="count"/> elements of a C# array of NSObjects.</summary>
		/// <param name="count">Number of items to copy from the items array.</param>
		/// <param name="items">Strongly typed array of NSObjects. Null elements are stored as <see cref="NSNull.Null"/>. If the array itself is null, an empty <see cref="NSArray"/> is returned.</param>
		/// <returns>A new <see cref="NSArray"/> containing the first <paramref name="count"/> objects from the array.</returns>
		/// <remarks>Null items in the array are converted to <see cref="NSNull.Null"/> instances.</remarks>
		public static NSArray FromNSObjects (nint count, params NSObject? []? items)
		{
			return FromNativeObjects (items, count);
		}

		/// <summary>Creates an NSArray from a C# array of objects implementing <see cref="INativeObject"/>.</summary>
		/// <param name="items">Array of objects implementing <see cref="INativeObject"/>. Null elements are stored as <see cref="NSNull.Null"/>. If the array itself is null, an empty <see cref="NSArray"/> is returned.</param>
		/// <returns>A new <see cref="NSArray"/> containing the specified objects.</returns>
		/// <remarks>Null items in the array are converted to <see cref="NSNull.Null"/> instances.</remarks>
		public static NSArray FromNSObjects (params INativeObject? []? items)
		{
			return FromNativeObjects (items);
		}

#if !XAMCORE_5_0
		/// <summary>Creates an NSArray from a C# array of objects implementing <see cref="INativeObject"/>, using the first <paramref name="count"/> elements.</summary>
		/// <param name="count">Number of items to copy from the items array.</param>
		/// <param name="items">Array of objects implementing <see cref="INativeObject"/>. Null elements are stored as <see cref="NSNull.Null"/>. If the array itself is null, an empty <see cref="NSArray"/> is returned.</param>
		/// <returns>A new <see cref="NSArray"/> containing the first <paramref name="count"/> objects from the array.</returns>
		/// <remarks>Null items in the array are converted to <see cref="NSNull.Null"/> instances.</remarks>
		[EditorBrowsable (EditorBrowsableState.Never)]
		// Don't obsolete this method, because forcing the first parameter to a 'nint' to pick the good overload to avoid the obsolete warning makes the calling code ugly.
		public static NSArray FromNSObjects (int count, params INativeObject? []? items)
		{
			return FromNativeObjects (items, count);
		}
#endif

		/// <summary>Creates an NSArray from a C# array of objects implementing <see cref="INativeObject"/>, using the first <paramref name="count"/> elements.</summary>
		/// <param name="count">Number of items to copy from the items array.</param>
		/// <param name="items">Array of objects implementing <see cref="INativeObject"/>. Null elements are stored as <see cref="NSNull.Null"/>. If the array itself is null, an empty <see cref="NSArray"/> is returned.</param>
		/// <returns>A new <see cref="NSArray"/> containing the first <paramref name="count"/> objects from the array.</returns>
		/// <remarks>Null items in the array are converted to <see cref="NSNull.Null"/> instances.</remarks>
		public static NSArray FromNSObjects (nint count, params INativeObject? []? items)
		{
			return FromNativeObjects (items, count);
		}

		/// <summary>Creates an NSArray from a C# array of objects implementing <see cref="INativeObject"/>.</summary>
		/// <typeparam name="T">The type of objects in the array, which must implement <see cref="INativeObject"/>.</typeparam>
		/// <param name="items">Array of objects. Null elements are stored as <see cref="NSNull.Null"/>. If the array itself is null, an empty <see cref="NSArray"/> is returned.</param>
		/// <returns>A new <see cref="NSArray"/> containing the specified objects.</returns>
		/// <remarks>Null items in the array are converted to <see cref="NSNull.Null"/> instances.</remarks>
		public static NSArray FromNSObjects<T> (params T? []? items) where T : class, INativeObject
		{
			return FromNativeObjects (items);
		}

		/// <summary>Creates a nested NSArray from a jagged array of objects implementing <see cref="INativeObject"/>.</summary>
		/// <typeparam name="T">The type of objects in the array, which must implement <see cref="INativeObject"/>.</typeparam>
		/// <param name="items">A jagged array of objects. If null, returns null.</param>
		/// <returns>A new <see cref="NSArray"/> containing nested NSArrays for each row. Returns null if <paramref name="items"/> is null.</returns>
		/// <exception cref="ArgumentNullException">Thrown if any row or element in the jagged array is null.</exception>
		/// <remarks>Each row of the jagged array is converted to an NSArray, and these NSArrays are then stored in the returned NSArray. Individual row elements and rows themselves cannot be null.</remarks>
		[return: NotNullIfNotNull (nameof (items))]
		public static NSArray? FromNSObjects<T> (params T [] []? items) where T : class, INativeObject
		{
			if (items is null)
				return null;

			var ret = new NSMutableArray ((nuint) items.Length);
			for (var i = 0; i < items.Length; i++) {
				var row = items [i];
				if (row is null)
					throw new ArgumentNullException (nameof (items), $"Element [{i}] is null");
				for (var j = 0; j < row.Length; j++) {
					var element = row [j];
					if (element is null)
						throw new ArgumentNullException (nameof (items), $"Element [{i}][{j}] is null");
				}
				ret.Add (NSArray.FromNSObjects (row));
			}

			return ret;
		}

		/// <summary>Creates a nested NSArray from a two-dimensional array of objects implementing <see cref="INativeObject"/>.</summary>
		/// <typeparam name="T">The type of objects in the array, which must implement <see cref="INativeObject"/>.</typeparam>
		/// <param name="items">A two-dimensional array of objects. If null, returns null.</param>
		/// <returns>A new <see cref="NSArray"/> containing nested NSArrays, one for each row of the 2D array. Returns null if <paramref name="items"/> is null.</returns>
		/// <remarks>The two-dimensional array is converted to a jagged array structure where each row becomes a nested NSArray.</remarks>
		[return: NotNullIfNotNull (nameof (items))]
		public static NSArray? FromNSObjects<T> (T [,]? items) where T : class, INativeObject
		{
			if (items is null)
				return null;

			var width = items.GetLength (0);
			var height = items.GetLength (1);
			var ret = new T [height] [];
			for (var y = 0; y < height; y++) {
				var row = new T [width];
				for (var x = 0; x < width; x++) {
					row [x] = items [x, y];
				}
				ret [y] = row;
			}
			return FromNSObjects (ret);
		}

#if !XAMCORE_5_0
		/// <summary>Creates an NSArray from a C# array of objects implementing <see cref="INativeObject"/>, using the first <paramref name="count"/> elements.</summary>
		/// <typeparam name="T">The type of objects in the array, which must implement <see cref="INativeObject"/>.</typeparam>
		/// <param name="count">Number of items to copy from the items array.</param>
		/// <param name="items">Array of objects. Null elements are stored as <see cref="NSNull.Null"/>. If the array itself is null, an empty <see cref="NSArray"/> is returned.</param>
		/// <returns>A new <see cref="NSArray"/> containing the first <paramref name="count"/> objects from the array.</returns>
		/// <remarks>Null items in the array are converted to <see cref="NSNull.Null"/> instances.</remarks>
		[EditorBrowsable (EditorBrowsableState.Never)]
		// Don't obsolete this method, because forcing the first parameter to a 'nint' to pick the good overload to avoid the obsolete warning makes the calling code ugly.
		public static NSArray FromNSObjects<T> (int count, params T? []? items) where T : class, INativeObject
		{
			return FromNativeObjects (items, count);
		}
#endif

		/// <summary>Creates an NSArray from a C# array of objects implementing <see cref="INativeObject"/>, using the first <paramref name="count"/> elements.</summary>
		/// <typeparam name="T">The type of objects in the array, which must implement <see cref="INativeObject"/>.</typeparam>
		/// <param name="count">Number of items to copy from the items array.</param>
		/// <param name="items">Array of objects. Null elements are stored as <see cref="NSNull.Null"/>. If the array itself is null, an empty <see cref="NSArray"/> is returned.</param>
		/// <returns>A new <see cref="NSArray"/> containing the first <paramref name="count"/> objects from the array.</returns>
		/// <remarks>Null items in the array are converted to <see cref="NSNull.Null"/> instances.</remarks>
		public static NSArray FromNSObjects<T> (nint count, params T? []? items) where T : class, INativeObject
		{
			return FromNativeObjects (items, count);
		}

		/// <summary>Creates an NSArray from a C# array using a custom conversion function.</summary>
		/// <typeparam name="T">The type of objects in the input array.</typeparam>
		/// <param name="nsobjectificator">A function that converts each item in the array to an <see cref="NSObject"/>. The function may return null.</param>
		/// <param name="items">Array of objects to convert. If null, returns null.</param>
		/// <returns>A new <see cref="NSArray"/> containing the converted objects. Returns null if <paramref name="items"/> is null.</returns>
		/// <exception cref="ArgumentNullException">Thrown if <paramref name="nsobjectificator"/> is null.</exception>
		/// <remarks>Each item is converted using the provided function. Null results from the conversion function are stored as <see cref="NSNull.Null"/> in the resulting array.</remarks>
		[return: NotNullIfNotNull (nameof (items))]
		public static NSArray? FromNSObjects<T> (Func<T?, NSObject?> nsobjectificator, params T? []? items)
		{
			return FromNSObjects<T> ((nint) (items?.Length ?? 0), nsobjectificator, items);
		}

		/// <summary>Creates an NSArray from a C# array using a custom conversion function.</summary>
		/// <typeparam name="T">The type of objects in the input array.</typeparam>
		/// <param name="count">Number of items to copy from the items array.</param>
		/// <param name="nsobjectificator">A function that converts each item in the array to an <see cref="NSObject"/>. The function may return null.</param>
		/// <param name="items">Array of objects to convert. If null, returns null.</param>
		/// <returns>A new <see cref="NSArray"/> containing the converted objects. Returns null if <paramref name="items"/> is null.</returns>
		/// <exception cref="ArgumentNullException">Thrown if <paramref name="nsobjectificator"/> is null.</exception>
		/// <remarks>Each item is converted using the provided function. Null results from the conversion function are stored as <see cref="NSNull.Null"/> in the resulting array.</remarks>
		[return: NotNullIfNotNull (nameof (items))]
		static NSArray? FromNSObjects<T> (nint count, Func<T?, NSObject?> nsobjectificator, params T? []? items)
		{
			if (nsobjectificator is null)
				throw new ArgumentNullException (nameof (nsobjectificator));

			if (items is null)
				return null;

			if (count > items.Length)
				throw new ArgumentException ("count is larger than the number of items", nameof (count));

			if (count < 0)
				throw new ArgumentOutOfRangeException (nameof (count), "count is negative");

			if (count == 0)
				return new NSArray ();

			var arr = new NSObject? [count];
			for (int i = 0; i < count; i++) {
				arr [i] = nsobjectificator (items [i]);
			}

			return FromNativeObjects (arr);
		}

		/// <summary>Creates an NSArray from a C# array of objects.</summary>
		/// <param name="items">Array of C# objects. Null elements will be boxed as <see cref="NSNull.Null"/>. If the array itself is null, an empty <see cref="NSArray"/> is returned.</param>
		/// <returns>A new <see cref="NSArray"/> containing the boxed objects.</returns>
		/// <remarks>The values will be boxed into NSObjects using <see cref="Foundation.NSObject.FromObject(System.Object)" />. Null items in the array are converted to <see cref="NSNull.Null"/> instances.</remarks>
		public static NSArray FromObjects (params object? []? items)
		{
			return FromObjects (items?.Length ?? 0, items);
		}

		/// <summary>Creates an NSArray from the first <paramref name="count"/> elements of a C# array.</summary>
		/// <param name="count">Number of items to copy from the items array.</param>
		/// <param name="items">Array of C# objects. Null elements will be boxed as <see cref="NSNull.Null"/>. If the array itself is null, an empty <see cref="NSArray"/> is returned.</param>
		/// <returns>A new <see cref="NSArray"/> containing the first <paramref name="count"/> boxed objects from the array.</returns>
		/// <remarks>The values will be boxed into NSObjects using <see cref="Foundation.NSObject.FromObject(System.Object)" />. Null items in the array are converted to <see cref="NSNull.Null"/> instances.</remarks>
		public static NSArray FromObjects (nint count, params object? []? items)
		{
			return From<object> (items, count);
		}

		internal static NSArray From<T> (T? []? items)
		{
			return From<T> (items, items?.Length ?? 0);
		}

		internal static NSArray From<T> (T? []? items, nint count)
		{
			if (items is null || count == 0)
				return new NSArray ();

			return FromNSObjects (count, (item) => {
				var k = NSObject.FromObject (item);
				if (k is null)
					throw new ArgumentException (String.Format ("Do not know how to marshal object of type '{0}' to an NSObject", item?.GetType ()));
				return k;
			}, items);
		}

		/// <summary>Creates an <see cref="NSArray" /> from an array of native objects.</summary>
		/// <typeparam name="T">The type of native objects in the array.</typeparam>
		/// <param name="items">An array of objects implementing <see cref="INativeObject" />. If null, returns an empty <see cref="NSArray" />. Any <see langword="null" /> elements will throw an exception.</param>
		/// <returns>A new <see cref="NSArray" /> containing the specified objects. </returns>
		/// <remarks>This method creates a native NSArray from managed objects. An exception will be thrown if there are any <see langword="null" /> elements.</remarks>
		internal static NSArray FromNonNullNativeObjects<T> (T []? items) where T : class, INativeObject
		{
			return FromNativeObjectsImpl<T> (items, items?.Length ?? 0, allowNullElements: false);
		}

		/// <summary>Creates an <see cref="NSArray" /> from an array of native objects.</summary>
		/// <typeparam name="T">The type of native objects in the array.</typeparam>
		/// <param name="items">An array of objects implementing <see cref="INativeObject" />. If null, returns an empty <see cref="NSArray" />.</param>
		/// <returns>A new <see cref="NSArray" /> containing the specified objects. Null items are represented as <see cref="NSNull.Null" />.</returns>
		/// <remarks>
		///   <para>
		///     This method creates a native NSArray from managed objects. Null items are represented as <see cref="NSNull.Null" />.
		///   </para>
		/// </remarks>
		internal static NSArray FromNativeObjects<T> (T? []? items) where T : class, INativeObject
		{
			return FromNativeObjectsImpl<T> (items, items?.Length ?? 0, allowNullElements: true);
		}

		/// <summary>Creates an <see cref="NSArray" /> from an array of native objects with a specified count.</summary>
		/// <typeparam name="T">The type of native objects in the array.</typeparam>
		/// <param name="items">An array of objects implementing <see cref="INativeObject" />. If null, returns an empty <see cref="NSArray" />.</param>
		/// <param name="count">The number of items from the array to include in the <see cref="NSArray" />.</param>
		/// <returns>A new <see cref="NSArray" /> containing the specified number of objects from the array. Null items are represented as <see cref="NSNull.Null" />.</returns>
		/// <exception cref="ArgumentException">Thrown when <paramref name="count" /> is greater than the length of <paramref name="items" />, or when <paramref name="count" /> is negative.</exception>
		/// <remarks>This method creates a native NSArray from the first <paramref name="count" /> elements of the managed array. Null items are represented as <see cref="NSNull.Null" />.</remarks>
		internal static NSArray FromNativeObjects<T> (T? []? items, nint count) where T : class, INativeObject
		{
			return FromNativeObjectsImpl<T> (items, count, allowNullElements: true);
		}

		static NSArray FromNativeObjectsImpl<T> (T? []? items, nint count, bool allowNullElements) where T : class, INativeObject
		{
			if (items is null)
				return new NSArray ();

			if (count > items.Length)
				throw new ArgumentException ("count is larger than the number of items", "count");

			if (count < 0)
				throw new ArgumentOutOfRangeException (nameof (count), "count is negative");

			var handles = new IntPtr [count];
			for (nint i = 0; i < count; i++) {
				var item = items [i];
				if (item is null && !allowNullElements)
					throw new ArgumentNullException ($"{nameof (items)}[{i}]");
				// The analyzer cannot deal with arrays, we manually keep alive the whole array below
#pragma warning disable RBI0014
				IntPtr h = item is null ? NSNull.Null.Handle : item.Handle;
				handles [i] = h;
#pragma warning restore RBI0014
			}
			var rv = FromIntPtrs (handles);
			GC.KeepAlive (items);
			return rv;
		}

		internal static NSArray FromNSObjects (IList<NSObject>? items)
		{
			if (items is null)
				return new NSArray ();

			var count = items.Count;
			var handles = new IntPtr [count];
			for (int i = 0; i < count; i++)
				handles [i] = items [i].Handle;

			var rv = FromIntPtrs (handles);
			GC.KeepAlive (items);
			return rv;
		}
#nullable disable

		/// <param name="items">Array of C# strings.</param>
		///         <summary>Creates an NSArray from a C# array of strings.</summary>
		///         <returns>
		///         </returns>
		///         <remarks>To be added.</remarks>
		static public NSArray FromStrings (params string [] items) => FromStrings ((IReadOnlyList<string>) items);

		static public NSArray FromStrings (IReadOnlyList<string> items)
		{
			if (items is null)
				throw new ArgumentNullException (nameof (items));

			IntPtr buf = Marshal.AllocHGlobal (items.Count * IntPtr.Size);
			try {
				for (int i = 0; i < items.Count; i++) {
					IntPtr val;

					if (items [i] is null)
						val = NSNull.Null.Handle;
					else {
						val = NSString.CreateNative (items [i], true);
					}

					Marshal.WriteIntPtr (buf, i * IntPtr.Size, val);
				}
				NSArray arr = Runtime.GetNSObject<NSArray> (NSArray.FromObjects (buf, items.Count));
				return arr;
			} finally {
				Marshal.FreeHGlobal (buf);
			}
		}

#nullable enable
		/// <summary>Create an <see cref="NSArray" /> from the specified pointers.</summary>
		/// <param name="items">Array of pointers (to <see cref="NSObject" /> instances).</param>
		/// <remarks>If the <paramref name="items" /> array is null, an <see cref="ArgumentNullException" /> is thrown.</remarks>
		static internal NSArray FromIntPtrs (IntPtr [] items)
		{
			if (items is null)
				throw new ArgumentNullException (nameof (items));

			unsafe {
				fixed (IntPtr* valuesPtr = items)
					return Runtime.GetNSObject<NSArray> (NSArray.FromObjects ((IntPtr) valuesPtr, items.Length)) ?? new NSArray ();
			}
		}

		/// <summary>Create an <see cref="NSArray" /> from the specified pointers.</summary>
		/// <param name="vals">Array of pointers (to <see cref="NSObject" /> instances).</param>
		/// <remarks>If the <paramref name="vals" /> array is null, an <see cref="ArgumentNullException" /> is thrown.</remarks>
		public static NSArray FromIntPtrs (NativeHandle [] vals)
		{
			if (vals is null)
				throw new ArgumentNullException (nameof (vals));

			unsafe {
				fixed (NativeHandle* valuesPtr = vals) {
					return Runtime.GetNSObject<NSArray> (NSArray.FromObjects ((IntPtr) valuesPtr, vals.Length)) ?? new NSArray ();
				}
			}
		}
#nullable disable

		internal static nuint GetCount (IntPtr handle)
		{
#if MONOMAC
			return (nuint) Messaging.UIntPtr_objc_msgSend (handle, selCountXHandle);
#else
			return (nuint) Messaging.UIntPtr_objc_msgSend (handle, Selector.GetHandle ("count"));
#endif
		}

		internal static NativeHandle GetAtIndex (NativeHandle handle, nuint i)
		{
			return Messaging.NativeHandle_objc_msgSend_UIntPtr (handle, Selector.GetHandle ("objectAtIndex:"), (UIntPtr) i);
		}

		/// <param name="handle">Pointer (handle) to the unmanaged object.</param>
		/// <summary>Creates a string array from an NSArray handle.</summary>
		/// <returns>
		///         </returns>
		/// <remarks>
		///         </remarks>
		[Obsolete ("Use of 'CFArray.StringArrayFromHandle' offers better performance.")]
		[EditorBrowsable (EditorBrowsableState.Never)]
		static public string [] StringArrayFromHandle (NativeHandle handle)
		{
			if (handle == NativeHandle.Zero)
				return null;

			var c = GetCount (handle);
			string [] ret = new string [c];

			for (nuint i = 0; i < c; i++)
				ret [i] = CFString.FromHandle (GetAtIndex (handle, i));
			return ret;
		}

		/// <typeparam name="T">Parameter type, determines the kind of array returned.</typeparam>
		/// <param name="handle">Pointer (handle) to the unmanaged object.</param>
		/// <summary>Returns a strongly-typed C# array of the parametrized type from a handle to an NSArray.</summary>
		/// <returns>An C# array with the values.</returns>
		/// <remarks>
		///           <para>Use this method to get a set of NSObject arrays from a handle to an NSArray</para>
		///           <example>
		///             <code lang="c#"><![CDATA[
		/// IntPtr someHandle = ...;
		/// 
		/// NSString [] values = NSArray.ArrayFromHandle<NSString> (someHandle);
		/// ]]></code>
		///           </example>
		///         </remarks>
		static public T [] ArrayFromHandle<T> (NativeHandle handle) where T : class, INativeObject
		{
			if (handle == NativeHandle.Zero)
				return null;

			var c = GetCount (handle);
			T [] ret = new T [c];

			for (uint i = 0; i < c; i++) {
				ret [i] = UnsafeGetItem<T> (handle, i);
			}
			return ret;
		}

		static Array ArrayFromHandle (NativeHandle handle, Type elementType)
		{
			if (handle == NativeHandle.Zero)
				return null;

			var c = (int) GetCount (handle);
			var rv = Array.CreateInstance (elementType, c);
			for (int i = 0; i < c; i++) {
				rv.SetValue (UnsafeGetItem (handle, (nuint) i, elementType), i);
			}
			return rv;
		}

		static public T [] EnumsFromHandle<T> (NativeHandle handle) where T : struct, IConvertible
		{
			if (handle == NativeHandle.Zero)
				return null;
			if (!typeof (T).IsEnum)
				throw new ArgumentException ("T must be an enum");

			var c = GetCount (handle);
			T [] ret = new T [c];

			for (uint i = 0; i < c; i++) {
				ret [i] = (T) Convert.ChangeType (UnsafeGetItem<NSNumber> (handle, i).LongValue, typeof (T));
			}
			return ret;
		}

		/// <typeparam name="T">Parameter type, determines the kind of
		/// 	array returned, limited to NSObject and subclasses of it.</typeparam>
		///         <param name="weakArray">Handle to an weakly typed NSArray.</param>
		///         <summary>Returns a strongly-typed C# array of the parametrized type from a weakly typed NSArray.</summary>
		///         <returns>An C# array with the values.</returns>
		///         <remarks>
		///           <para>Use this method to get a set of NSObject arrays from an NSArray.</para>
		///           <example>
		///             <code lang="c#"><![CDATA[
		/// NSArray someArray = ...;
		///
		/// NSString [] values = NSArray.FromArray<CGImage> (someArray);
		/// ]]></code>
		///           </example>
		///         </remarks>
		static public T [] FromArray<T> (NSArray weakArray) where T : NSObject
		{
			if (weakArray is null || weakArray.Handle == NativeHandle.Zero)
				return null;
			try {
				nuint n = weakArray.Count;
				T [] ret = new T [n];
				for (nuint i = 0; i < n; i++) {
					ret [i] = Runtime.GetNSObject<T> (weakArray.ValueAt (i));
				}
				return ret;
			} catch {
				return null;
			}
		}

		/// <typeparam name="T">Parameter type, determines the kind of
		/// 	array returned, can be either an NSObject, or other
		/// 	CoreGraphics data types.</typeparam>
		///         <param name="weakArray">Handle to an weakly typed NSArray.</param>
		///         <summary>Returns a strongly-typed C# array of the parametrized type from a weakly typed NSArray.</summary>
		///         <returns>An C# array with the values.</returns>
		///         <remarks>
		///           <para>Use this method to get a set of NSObject arrays from an NSArray.</para>
		///           <example>
		///             <code lang="c#"><![CDATA[
		/// NSArray someArray = ...;
		///
		/// CGImage [] myImages = NSArray.FromArray<CGImage> (someArray);
		/// ]]></code>
		///           </example>
		///         </remarks>
		static public T [] FromArrayNative<T> (NSArray weakArray) where T : class, INativeObject
		{
			if (weakArray is null || weakArray.Handle == NativeHandle.Zero)
				return null;
			try {
				nuint n = weakArray.Count;
				T [] ret = new T [n];
				for (nuint i = 0; i < n; i++) {
					ret [i] = Runtime.GetINativeObject<T> (weakArray.ValueAt (i), false);
				}
				return ret;
			} catch {
				return null;
			}
		}

		// Used when we need to provide our constructor
		/// <typeparam name="T">Parameter type, determines the kind of array returned.</typeparam>
		/// <param name="handle">Pointer (handle) to the unmanaged object.</param>
		/// <param name="createObject">To be added.</param>
		/// <summary>Returns a strongly-typed C# array of the parametrized type from a handle to an NSArray.</summary>
		/// <returns>An C# array with the values.</returns>
		/// <remarks>
		///           <para>Use this method to get a set of NSObject arrays from a handle to an NSArray.   Instead of wrapping the results in NSObjects, the code invokes your method to create the return value.</para>
		///           <example>
		///             <code lang="c#"><![CDATA[
		/// int [] args = NSArray.ArrayFromHandle<int> (someHandle, (x) => (int) x);
		/// ]]></code>
		///           </example>
		///         </remarks>
		static public T [] ArrayFromHandleFunc<T> (NativeHandle handle, Func<NativeHandle, T> createObject)
		{
			if (handle == NativeHandle.Zero)
				return null;

			var c = GetCount (handle);
			T [] ret = new T [c];

			for (uint i = 0; i < c; i++)
				ret [i] = createObject (GetAtIndex (handle, i));

			return ret;
		}

		/// <summary>Create a managed array from a pointer to a native NSArray instance.</summary>
		/// <param name="handle">The pointer to the native NSArray instance.</param>
		/// <param name="createObject">A callback that returns an instance of the type T for a given pointer (for an element in the NSArray).</param>
		/// <param name="releaseHandle">Whether the native NSArray instance should be released before returning or not.</param>
		public static T [] ArrayFromHandleFunc<T> (NativeHandle handle, Func<NativeHandle, T> createObject, bool releaseHandle)
		{
			var rv = ArrayFromHandleFunc<T> (handle, createObject);
			if (releaseHandle && handle != NativeHandle.Zero)
				NSObject.DangerousRelease (handle);
			return rv;
		}

		/// <typeparam name="T">Parameter type, determines the kind of array returned.</typeparam>
		/// <param name="handle">Pointer (handle) to the unmanaged object.</param>
		/// <param name="creator">Method that can create objects of type T from a given IntPtr.</param>
		/// <summary>Returns a strongly-typed C# array of the parametrized type from a handle to an NSArray.</summary>
		/// <returns>An C# array with the values.</returns>
		/// <remarks>
		///           <para>Use this method to get a set of NSObject arrays from a handle to an NSArray.   Instead of wrapping the results in NSObjects, the code invokes your method to create the return value.</para>
		///           <example>
		///             <code lang="c#"><![CDATA[
		/// int myCreator (IntPtr v)
		/// {
		/// 	return (int) v;
		/// }
		/// 
		/// int [] args = NSArray.ArrayFromHandle<int> (someHandle, myCreator);
		/// ]]></code>
		///           </example>
		///         </remarks>
		static public T [] ArrayFromHandle<T> (NativeHandle handle, Converter<NativeHandle, T> creator)
		{
			if (handle == NativeHandle.Zero)
				return null;

			var c = GetCount (handle);
			T [] ret = new T [c];

			for (uint i = 0; i < c; i++)
				ret [i] = creator (GetAtIndex (handle, i));

			return ret;
		}

		static public T [] ArrayFromHandle<T> (NativeHandle handle, Converter<NativeHandle, T> creator, bool releaseHandle)
		{
			var rv = ArrayFromHandle<T> (handle, creator);
			if (releaseHandle && handle != NativeHandle.Zero)
				NSObject.DangerousRelease (handle);
			return rv;
		}

		// FIXME: before proving a real `this` indexer we need to clean the issues between
		// NSObject and INativeObject coexistance across all the API (it can not return T)

		static T UnsafeGetItem<T> (NativeHandle handle, nuint index) where T : class, INativeObject
		{
			var val = GetAtIndex (handle, index);
			// A native code could return NSArray with NSNull.Null elements
			// and they should be valid for things like T : NSDate so we handle
			// them as just null values inside the array
			if (val == NSNull.Null.Handle)
				return null;

			return Runtime.GetINativeObject<T> (val, false);
		}

		static object UnsafeGetItem (NativeHandle handle, nuint index, Type type)
		{
			var val = GetAtIndex (handle, index);
			// A native code could return NSArray with NSNull.Null elements
			// and they should be valid for things like T : NSDate so we handle
			// them as just null values inside the array
			if (val == NSNull.Null.Handle)
				return null;

			return Runtime.GetINativeObject (val, false, type);
		}

		// can return an INativeObject or an NSObject
		/// <typeparam name="T">To be added.</typeparam>
		/// <param name="index">To be added.</param>
		/// <summary>To be added.</summary>
		/// <returns>To be added.</returns>
		/// <remarks>To be added.</remarks>
		public T GetItem<T> (nuint index) where T : class, INativeObject
		{
			if (index >= GetCount (Handle))
				throw new ArgumentOutOfRangeException ("index");

			return UnsafeGetItem<T> (Handle, index);
		}

		/// <param name="weakArray">To be added.</param>
		///         <summary>To be added.</summary>
		///         <returns>To be added.</returns>
		///         <remarks>To be added.</remarks>
		public static NSObject [] [] FromArrayOfArray (NSArray weakArray)
		{
			if (weakArray is null || weakArray.Handle == IntPtr.Zero)
				return null;

			try {
				nuint n = weakArray.Count;
				var ret = new NSObject [n] [];
				for (nuint i = 0; i < n; i++)
					ret [i] = NSArray.FromArray<NSObject> (weakArray.GetItem<NSArray> (i));
				return ret;
			} catch {
				return null;
			}
		}

		/// <param name="items">To be added.</param>
		///         <summary>To be added.</summary>
		///         <returns>To be added.</returns>
		///         <remarks>To be added.</remarks>
		public static NSArray From (NSObject [] [] items)
		{
			if (items is null)
				return null;

			try {
				var ret = new NSMutableArray ((nuint) items.Length);
				for (int i = 0; i < items.Length; i++)
					ret.Add (NSArray.FromNSObjects (items [i]));
				return ret;
			} catch {
				return null;
			}
		}

		public TKey [] ToArray<TKey> () where TKey : class, INativeObject
		{
			var rv = new TKey [GetCount (Handle)];
			for (var i = 0; i < rv.Length; i++)
				rv [i] = GetItem<TKey> ((nuint) i);
			return rv;
		}

		public NSObject [] ToArray ()
		{
			return ToArray<NSObject> ();
		}

		/// <summary>Returns an enumerator that iterates through the array.</summary>
		/// <returns>An enumerator that can be used to iterate through the array.</returns>
		public IEnumerator<NSObject> GetEnumerator ()
		{
			return new NSFastEnumerator<NSObject> (this);
		}

		IEnumerator IEnumerable.GetEnumerator ()
		{
			return new NSFastEnumerator<NSObject> (this);
		}

#if false // https://github.com/dotnet/macios/issues/15577

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
		public NSOrderedCollectionDifference GetDifferenceFromArray (NSArray other, NSOrderedCollectionDifferenceCalculationOptions options, NSOrderedCollectionDifferenceEquivalenceTest equivalenceTest) 
		{
			if (equivalenceTest is null)
				throw new ArgumentNullException (nameof (equivalenceTest));

			var block = new BlockLiteral ();
			block.SetupBlock (static_DiffEquality, equivalenceTest);
			try {
				return Runtime.GetNSObject<NSOrderedCollectionDifference> (_GetDifferenceFromArray (other, options, ref block));
			} finally {
				block.CleanupBlock ();
			}
		}
#endif
	}
}
