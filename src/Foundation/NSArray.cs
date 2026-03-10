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
				IntPtr h = item is null ? NSNull.NullHandle : item.Handle;
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

		/// <summary>Creates an NSArray from a C# array of strings.</summary>
		/// <param name="items">Array of C# strings.</param>
		/// <returns>A new <see cref="NSArray" /> containing the strings.</returns>
		/// <remarks>If any string in <paramref name="items" /> is null, a <see cref="NSNull" /> entry will be created for that element.</remarks>
		public static NSArray FromStrings (params string? [] items) => FromStrings ((IReadOnlyList<string?>) items);

		/// <summary>Creates an NSArray from a list of C# strings.</summary>
		/// <param name="items">List of C# strings.</param>
		/// <returns>A new <see cref="NSArray" /> containing the strings.</returns>
		/// <remarks>If any string in <paramref name="items" /> is null, a <see cref="NSNull" /> entry will be created for that element.</remarks>
		public static NSArray FromStrings (IReadOnlyList<string?> items)
		{
			return Runtime.GetNSObject<NSArray> (CFArray.Create (items), owns: true)!;
		}

		/// <summary>Creates an NSArray from a potentially null array of strings.</summary>
		/// <param name="items">Array of C# strings, or null.</param>
		/// <returns>A new <see cref="NSArray" /> containing the strings, or null if <paramref name="items" /> is null.</returns>
		/// <remarks>If any string in <paramref name="items" /> is null, a <see cref="NSNull" /> entry will be created for that element.</remarks>
		[return: NotNullIfNotNull (nameof (items))]
		public static NSArray? FromNullableStrings (params string? []? items)
		{
			if (items is null)
				return null;
			return FromStrings ((IReadOnlyList<string?>) items);
		}

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

		/// <summary>Create an <see cref="NSArray" /> from the specified pointers.</summary>
		/// <param name="array">Array of pointers (to <see cref="NSObject" /> instances).</param>
		/// <param name="getHandle">A delegate to convert each array element to a native handle.</param>
		/// <remarks>If the <paramref name="array" /> array is null, <see langword="null" /> is returned.</remarks>
		[return: NotNullIfNotNull (nameof (array))]
		internal static NSArray? FromIntPtrs<T> (T []? array, Func<T, NativeHandle> getHandle)
		{
			if (array is null)
				return null;

			var handles = new NativeHandle [array.Length];
			for (var i = 0; i < handles.Length; i++)
				handles [i] = getHandle (array [i]);

			var rv = FromIntPtrs (handles);
			GC.KeepAlive (array);
			return rv;
		}

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

#if !XAMCORE_5_0
		/// <summary>Creates a string array from a handle to a native NSArray object.</summary>
		/// <param name="handle">Pointer (handle) to the unmanaged object.</param>
		/// <returns>A string array, or <see langword="null" /> if handle is <see cref="NativeHandle.Zero" />.</returns>
		/// <remarks>Call <see cref="CFArray.StringArrayFromHandle(NativeHandle)" /> instead for better performance.</remarks>
		[Obsolete ("Use 'CFArray.StringArrayFromHandle' instead.")]
		[EditorBrowsable (EditorBrowsableState.Never)]
		public static string? []? StringArrayFromHandle (NativeHandle handle)
		{
			return CFArray.StringArrayFromHandle (handle);
		}
#endif // !XAMCORE_5_0

		/// <summary>Returns a strongly-typed C# array of the parametrized type from a handle to an NSArray.</summary>
		/// <typeparam name="T">Parameter type, determines the kind of array returned.</typeparam>
		/// <param name="handle">Pointer (handle) to the unmanaged object.</param>
		/// <returns>An C# array with the values.</returns>
		/// <remarks>
		///   <example>
		///     <code lang="c#"><![CDATA[
		/// NativeHandle someHandle = ...;
		/// var values = NSArray.ArrayFromHandle<NSString> (someHandle);
		/// ]]></code>
		///   </example>
		/// </remarks>
		public static T? []? ArrayFromHandle<T> (NativeHandle handle) where T : class, INativeObject
		{
			return ArrayFromHandle<T> (handle, false);
		}

		/// <summary>Returns a strongly-typed C# array of the parametrized type from a handle to an NSArray.</summary>
		/// <typeparam name="T">Parameter type, determines the kind of array returned.</typeparam>
		/// <param name="handle">Pointer (handle) to the unmanaged object.</param>
		/// <param name="releaseHandle">Whether the native NSArray instance should be released before returning or not.</param>
		/// <returns>A C# array with the values.</returns>
		/// <remarks>
		///   <example>
		///     <code lang="c#"><![CDATA[
		/// var someHandle = GetCopyOfNativeArray (...);
		/// var values = NSArray.ArrayFromHandle<NSString> (someHandle, releaseHandle: true);
		/// ]]></code>
		///   </example>
		/// </remarks>
		public static T? []? ArrayFromHandle<T> (NativeHandle handle, bool releaseHandle) where T : class, INativeObject
		{
			return ArrayFromHandle<T> (handle, h => Runtime.GetINativeObject<T> (h, false), NSNullBehavior.ConvertToNull, releaseHandle);
		}

		/// <typeparam name="T">Parameter type, determines the kind of array returned.</typeparam>
		/// <param name="handle">Pointer (handle) to the unmanaged object.</param>
		/// <param name="creator">Method that can create objects of type T from a given IntPtr.</param>
		/// <summary>Returns a strongly-typed C# array of the parametrized type from a handle to an NSArray.</summary>
		/// <returns>An C# array with the values.</returns>
		/// <remarks>
		///   <example>
		///     <code lang="c#"><![CDATA[
		/// int myCreator (IntPtr v)
		/// {
		/// 	return (int) v;
		/// }
		///
		/// var args = NSArray.ArrayFromHandle<int> (someHandle, myCreator);
		/// ]]></code>
		///   </example>
		/// </remarks>
		public static T? []? ArrayFromHandle<T> (NativeHandle handle, Converter<NativeHandle, T> creator)
		{
			return ArrayFromHandle<T> (handle, creator, NSNullBehavior.ConvertToNull, false);
		}

		/// <summary>Returns a strongly-typed C# array of the parametrized type from a handle to an NSArray.</summary>
		/// <typeparam name="T">Parameter type, determines the kind of array returned.</typeparam>
		/// <param name="handle">Pointer (handle) to the unmanaged object.</param>
		/// <param name="creator">Method that can create objects of type T from a given handle.</param>
		/// <param name="releaseHandle">Whether the native NSArray instance should be released before returning or not.</param>
		/// <returns>A C# array with the values, or <see langword="null" /> if the handle is <see cref="NativeHandle.Zero" />.</returns>
		public static T? []? ArrayFromHandle<T> (NativeHandle handle, Converter<NativeHandle, T> creator, bool releaseHandle)
		{
			return ArrayFromHandle<T> (handle, creator, NSNullBehavior.ConvertToNull, releaseHandle);
		}

		/// <summary>Returns a strongly-typed C# array of the parametrized type from a handle to an NSArray.</summary>
		/// <typeparam name="T">Parameter type, determines the kind of array returned.</typeparam>
		/// <param name="handle">Pointer (handle) to the unmanaged object.</param>
		/// <param name="createObject">A delegate to convert a native handle to an object of type T.</param>
		/// <param name="nsNullElementBehavior">How to handle null and NSNull elements in the native array.</param>
		/// <param name="releaseHandle">Whether the native NSArray instance should be released before returning or not.</param>
		/// <returns>A C# array with the values, or <see langword="null" /> if the handle is <see cref="NativeHandle.Zero" />.</returns>
		internal static T? []? ArrayFromHandle<T> (NativeHandle handle, Converter<NativeHandle, T?> createObject, NSNullBehavior nsNullElementBehavior, bool releaseHandle = false)
		{
			if (handle == NativeHandle.Zero)
				return null;

			try {
				var count = GetCount (handle);
				var ret = new T? [count];
				nuint nextIndex = 0;

				for (nuint i = 0; i < count; i++) {
					var val = GetAtIndex (handle, i);
					if (!TryGetItem<T> (val, createObject, nsNullElementBehavior, i, out var value))
						continue;
					ret [nextIndex++] = value;
				}

				if (nextIndex != count)
					Array.Resize<T?> (ref ret, (int) nextIndex);

				return ret;
			} finally {
				if (releaseHandle)
					NSObject.DangerousRelease (handle);
			}
		}

		/// <summary>Returns a strongly-typed C# array of the parametrized type from a handle to an NSArray.</summary>
		/// <typeparam name="T">Parameter type, determines the kind of array returned.</typeparam>
		/// <param name="handle">Pointer (handle) to the unmanaged object.</param>
		/// <param name="nsNullElementBehavior">How to handle null and NSNull elements in the native array.</param>
		/// <param name="releaseHandle">Whether the native NSArray instance should be released before returning or not.</param>
		/// <returns>A C# array with the values, or <see langword="null" /> if the handle is <see cref="NativeHandle.Zero" />.</returns>
		internal static T? []? ArrayFromHandle<T> (NativeHandle handle, NSNullBehavior nsNullElementBehavior, bool releaseHandle = false) where T : class, INativeObject
		{
			return ArrayFromHandle<T> (handle, (h) => Runtime.GetINativeObject<T> (h, false), nsNullElementBehavior, releaseHandle);
		}

		/// <summary>Attempts to get an item from the native array, handling null and NSNull elements according to the specified behavior.</summary>
		/// <typeparam name="T">The type of the item to create.</typeparam>
		/// <param name="elementHandle">The native handle of the element.</param>
		/// <param name="createObject">A delegate to convert a native handle to an object of type T.</param>
		/// <param name="nsNullElementBehavior">How to handle null and NSNull elements.</param>
		/// <param name="index">The index of the element in the source array (used for error messages).</param>
		/// <param name="value">When this method returns, contains the created object, or the default value if the element was skipped.</param>
		/// <returns><see langword="true" /> if the element should be included in the result array; <see langword="false" /> if it should be skipped.</returns>
		static bool TryGetItem<T> (NativeHandle elementHandle, Converter<NativeHandle, T?> createObject, NSNullBehavior nsNullElementBehavior, nuint index, out T? value)
		{
			value = default (T);

			switch (nsNullElementBehavior) {
			case NSNullBehavior.Drop:
				if (elementHandle == NativeHandle.Zero)
					return false;
				if (elementHandle == NSNull.NullHandle)
					return false;
				value = createObject (elementHandle);
				return value is not null;
			case NSNullBehavior.DropIfIncompatible:
				if (elementHandle == NativeHandle.Zero)
					return false;
				if (elementHandle == NSNull.NullHandle) {
					if (NSNull.Null is T nullT)
						value = nullT;
				} else {
					value = createObject (elementHandle);
				}
				return value is not null;
			case NSNullBehavior.ConvertToNull:
				if (elementHandle == NativeHandle.Zero)
					return true;
				if (elementHandle == NSNull.NullHandle)
					return true;
				value = createObject (elementHandle);
				return true;
			case NSNullBehavior.Throw:
				if (elementHandle != NSNull.NullHandle && elementHandle != NativeHandle.Zero)
					value = createObject (elementHandle);
				if (value is null)
					throw new InvalidOperationException ($"Invalid null element at index {index}");
				return true;
			default:
				throw new InvalidOperationException ($"Unknown null behavior: {nsNullElementBehavior}");
			}
		}

		/// <summary>Returns a strongly-typed C# array from a handle to an NSArray, guaranteeing a non-null return value.</summary>
		/// <typeparam name="T">Parameter type, determines the kind of array returned.</typeparam>
		/// <param name="handle">Pointer (handle) to the unmanaged object.</param>
		/// <param name="releaseHandle">Whether the native NSArray instance should be released before returning or not.</param>
		/// <returns>A C# array with the values. Returns an empty array if the handle is <see cref="NativeHandle.Zero" />.</returns>
		internal static T? [] NonNullArrayFromHandle<T> (NativeHandle handle, bool releaseHandle = false) where T : class, INativeObject
		{
			return NonNullArrayFromHandle<T> (handle, NSNullBehavior.ConvertToNull, releaseHandle);
		}

		/// <summary>Returns a strongly-typed C# array from a handle to an NSArray, guaranteeing a non-null return value.</summary>
		/// <typeparam name="T">Parameter type, determines the kind of array returned.</typeparam>
		/// <param name="handle">Pointer (handle) to the unmanaged object.</param>
		/// <param name="nsNullElementBehavior">How to handle null and NSNull elements in the native array.</param>
		/// <param name="releaseHandle">Whether the native NSArray instance should be released before returning or not.</param>
		/// <returns>A C# array with the values. Returns an empty array if the handle is <see cref="NativeHandle.Zero" />.</returns>
		internal static T? [] NonNullArrayFromHandle<T> (NativeHandle handle, NSNullBehavior nsNullElementBehavior, bool releaseHandle = false) where T : class, INativeObject
		{
			var rv = ArrayFromHandle<T> (handle, nsNullElementBehavior, releaseHandle);
			return rv ?? Array.Empty<T> ();
		}

		/// <summary>Returns a strongly-typed C# array from a handle to an NSArray, guaranteeing a non-null return value.</summary>
		/// <typeparam name="T">Parameter type, determines the kind of array returned.</typeparam>
		/// <param name="handle">Pointer (handle) to the unmanaged object.</param>
		/// <param name="creator">A delegate to convert a native handle to an object of type T.</param>
		/// <param name="releaseHandle">Whether the native NSArray instance should be released before returning or not.</param>
		/// <returns>A C# array with the values. Returns an empty array if the handle is <see cref="NativeHandle.Zero" />.</returns>
		internal static T? [] NonNullArrayFromHandle<T> (NativeHandle handle, Converter<NativeHandle, T> creator, bool releaseHandle = false)
		{
			var rv = ArrayFromHandle<T> (handle, creator, NSNullBehavior.ConvertToNull, releaseHandle);
			return rv ?? Array.Empty<T> ();
		}

		/// <summary>Returns a strongly-typed C# array from a handle to an NSArray, dropping any null or NSNull elements.</summary>
		/// <typeparam name="T">Parameter type, determines the kind of array returned.</typeparam>
		/// <param name="handle">Pointer (handle) to the unmanaged object.</param>
		/// <param name="releaseHandle">Whether the native NSArray instance should be released before returning or not.</param>
		/// <returns>A C# array with the values (excluding null elements), or <see langword="null" /> if the handle is <see cref="NativeHandle.Zero" />.</returns>
		internal static T []? ArrayFromHandleDropNullElements<T> (NativeHandle handle, bool releaseHandle = false) where T : class, INativeObject
		{
			return ArrayFromHandleDropNullElements<T> (handle, (h) => Runtime.GetINativeObject<T> (h, false)!, releaseHandle);
		}

		/// <summary>Returns a strongly-typed C# array from a handle to an NSArray, dropping any null or NSNull elements.</summary>
		/// <typeparam name="T">Parameter type, determines the kind of array returned.</typeparam>
		/// <param name="handle">Pointer (handle) to the unmanaged object.</param>
		/// <param name="createObject">A delegate to convert a native handle to an object of type T.</param>
		/// <param name="releaseHandle">Whether the native NSArray instance should be released before returning or not.</param>
		/// <returns>A C# array with the values (excluding null elements), or <see langword="null" /> if the handle is <see cref="NativeHandle.Zero" />.</returns>
		internal static T []? ArrayFromHandleDropNullElements<T> (NativeHandle handle, Converter<NativeHandle, T> createObject, bool releaseHandle = false)
		{
			return ArrayFromHandle<T> (handle, createObject, NSNullBehavior.Drop, releaseHandle)!;
		}

		/// <summary>Returns a strongly-typed C# array from a handle to an NSArray, dropping null elements and guaranteeing a non-null return value.</summary>
		/// <typeparam name="T">Parameter type, determines the kind of array returned.</typeparam>
		/// <param name="handle">Pointer (handle) to the unmanaged object.</param>
		/// <param name="releaseHandle">Whether the native NSArray instance should be released before returning or not.</param>
		/// <returns>A C# array with the values (excluding null elements). Returns an empty array if the handle is <see cref="NativeHandle.Zero" />.</returns>
		internal static T [] NonNullArrayFromHandleDropNullElements<T> (NativeHandle handle, bool releaseHandle = false) where T : class, INativeObject
		{
			return NonNullArrayFromHandleDropNullElements<T> (handle, (h) => Runtime.GetINativeObject<T> (h, false)!, releaseHandle);
		}

		/// <summary>Returns a strongly-typed C# array from a handle to an NSArray, dropping null elements and guaranteeing a non-null return value.</summary>
		/// <typeparam name="T">Parameter type, determines the kind of array returned.</typeparam>
		/// <param name="handle">Pointer (handle) to the unmanaged object.</param>
		/// <param name="createObject">A delegate to convert a native handle to an object of type T.</param>
		/// <param name="releaseHandle">Whether the native NSArray instance should be released before returning or not.</param>
		/// <returns>A C# array with the values (excluding null elements). Returns an empty array if the handle is <see cref="NativeHandle.Zero" />.</returns>
		internal static T [] NonNullArrayFromHandleDropNullElements<T> (NativeHandle handle, Converter<NativeHandle, T> createObject, bool releaseHandle = false)
		{
			return NonNullArrayFromHandleDropNullElements<T> (handle, createObject, NSNullBehavior.Drop, releaseHandle);
		}

		/// <summary>Returns a strongly-typed C# array from a handle to an NSArray, dropping null elements and guaranteeing a non-null return value.</summary>
		/// <typeparam name="T">Parameter type, determines the kind of array returned.</typeparam>
		/// <param name="handle">Pointer (handle) to the unmanaged object.</param>
		/// <param name="nsNullElementBehavior">How to handle null and NSNull elements in the native array.</param>
		/// <param name="releaseHandle">Whether the native NSArray instance should be released before returning or not.</param>
		/// <returns>A C# array with the values (excluding null elements). Returns an empty array if the handle is <see cref="NativeHandle.Zero" />.</returns>
		internal static T [] NonNullArrayFromHandleDropNullElements<T> (NativeHandle handle, NSNullBehavior nsNullElementBehavior, bool releaseHandle = false) where T : class, INativeObject
		{
			return NonNullArrayFromHandleDropNullElements<T> (handle, (h) => Runtime.GetINativeObject<T> (h, false)!, nsNullElementBehavior, releaseHandle);
		}

		/// <summary>Returns a strongly-typed C# array from a handle to an NSArray, dropping null elements and guaranteeing a non-null return value.</summary>
		/// <typeparam name="T">Parameter type, determines the kind of array returned.</typeparam>
		/// <param name="handle">Pointer (handle) to the unmanaged object.</param>
		/// <param name="createObject">A delegate to convert a native handle to an object of type T.</param>
		/// <param name="nsNullElementBehavior">How to handle null and NSNull elements in the native array.</param>
		/// <param name="releaseHandle">Whether the native NSArray instance should be released before returning or not.</param>
		/// <returns>A C# array with the values (excluding null elements). Returns an empty array if the handle is <see cref="NativeHandle.Zero" />.</returns>
		internal static T [] NonNullArrayFromHandleDropNullElements<T> (NativeHandle handle, Converter<NativeHandle, T> createObject, NSNullBehavior nsNullElementBehavior, bool releaseHandle = false)
		{
			var rv = ArrayFromHandle<T> (handle, createObject, nsNullElementBehavior, releaseHandle);
			if (rv is null)
				return Array.Empty<T> ();
			return rv!;
		}

#nullable disable

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
			return ArrayFromHandle<T> (handle, (v) => createObject (v));
		}

		/// <summary>Create a managed array from a pointer to a native NSArray instance.</summary>
		/// <param name="handle">The pointer to the native NSArray instance.</param>
		/// <param name="createObject">A callback that returns an instance of the type T for a given pointer (for an element in the NSArray).</param>
		/// <param name="releaseHandle">Whether the native NSArray instance should be released before returning or not.</param>
		public static T [] ArrayFromHandleFunc<T> (NativeHandle handle, Func<NativeHandle, T> createObject, bool releaseHandle)
		{
			return ArrayFromHandle<T> (handle, (v) => createObject (v), releaseHandle);
		}

#nullable enable
		/// <summary>Creates a managed array from a pointer to a native NSArray of NSDictionary objects, dropping null and NSNull elements.</summary>
		/// <typeparam name="T">The type of objects to create from the dictionaries.</typeparam>
		/// <param name="handle">The pointer to the native NSArray instance containing NSDictionary objects.</param>
		/// <param name="createObjectFromDictionary">A factory function that creates an instance of type T from an NSDictionary.</param>
		/// <param name="releaseHandle">Whether the native NSArray instance should be released before returning or not. Defaults to false.</param>
		/// <returns>A C# array with the values, or null if the handle is zero. Null and NSNull elements are excluded from the result.</returns>
		/// <remarks>
		///   <para>This method converts a native NSArray of NSDictionary objects into a managed array. Any null or NSNull elements in the source array are skipped, and the resulting array is resized accordingly.</para>
		/// </remarks>
		internal static T []? DictionaryArrayFromHandleDropNullElements<T> (NativeHandle handle, Func<NSDictionary, T> createObjectFromDictionary, bool releaseHandle = false)
		{
			if (handle == NativeHandle.Zero)
				return null;

			return ArrayFromHandleDropNullElements<T> (handle,
				(dictionaryHandle) => {
					return createObjectFromDictionary (Runtime.GetNSObject<NSDictionary> (dictionaryHandle)!);
				},
				releaseHandle);
		}

		/// <summary>Creates a managed array from a pointer to a native NSArray of NSDictionary objects, dropping null and NSNull elements. Always returns a non-null array.</summary>
		/// <typeparam name="T">The type of objects to create from the dictionaries.</typeparam>
		/// <param name="handle">The pointer to the native NSArray instance containing NSDictionary objects.</param>
		/// <param name="createObjectFromDictionary">A factory function that creates an instance of type T from an NSDictionary.</param>
		/// <param name="releaseHandle">Whether the native NSArray instance should be released before returning or not. Defaults to false.</param>
		/// <returns>A C# array with the values. Returns an empty array if the handle is zero. Null and NSNull elements are excluded from the result.</returns>
		/// <remarks>
		///   <para>This method is a wrapper around <see cref="DictionaryArrayFromHandleDropNullElements{T}"/> that guarantees a non-null return value. If the handle is zero or null, an empty array is returned instead of null.</para>
		/// </remarks>
		internal static T [] NonNullDictionaryArrayFromHandleDropNullElements<T> (NativeHandle handle, Func<NSDictionary, T> createObjectFromDictionary, bool releaseHandle = false)
		{
			var rv = DictionaryArrayFromHandleDropNullElements<T> (handle, createObjectFromDictionary, releaseHandle);
			if (rv is null)
				return Array.Empty<T> ();
			return rv;
		}

		// FIXME: before proving a real `this` indexer we need to clean the issues between
		// NSObject and INativeObject coexistance across all the API (it can not return T)

		/// <summary>Gets a single item from a native NSArray at the specified index, without bounds checking.</summary>
		/// <typeparam name="T">The type of the item to retrieve.</typeparam>
		/// <param name="handle">Pointer (handle) to the native NSArray.</param>
		/// <param name="index">The zero-based index of the element to retrieve.</param>
		/// <returns>The item at the specified index, or <see langword="null" /> if the element is an NSNull instance.</returns>
		static T? UnsafeGetItem<T> (NativeHandle handle, nuint index) where T : class, INativeObject
		{
			var val = GetAtIndex (handle, index);
			// A native code could return NSArray with NSNull.Null elements
			// and they should be valid for things like T : NSDate so we handle
			// them as just null values inside the array
			if (val == NSNull.NullHandle)
				return null;

			return Runtime.GetINativeObject<T> (val, false);
		}

#nullable disable
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
