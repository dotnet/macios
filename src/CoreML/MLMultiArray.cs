//
// MLMultiArray.cs
//
// Authors:
//	Alex Soto  <alexsoto@microsoft.com>
//
// Copyright 2017 Xamarin Inc. All rights reserved.
//

#nullable enable

using System;
using Foundation;
using ObjCRuntime;

namespace CoreML {
	public partial class MLMultiArray {
		static NSNumber [] ConvertArray (nint [] value)
		{
			if (value is null)
				return Array.Empty<NSNumber> ();

			return Array.ConvertAll<nint, NSNumber> (value, NSNumber.FromNInt);
		}

		// NSArray<NSNumber> => nint[]
		internal static nint [] ConvertArray (IntPtr handle)
		{
			return NSArray.ArrayFromHandle<nint> (handle, (v) => (nint) Messaging.IntPtr_objc_msgSend (v, Selector.GetHandle ("integerValue")));
		}

		/// <param name="shape">To be added.</param>
		/// <param name="dataType">To be added.</param>
		/// <param name="error">To be added.</param>
		/// <summary>Creates a new MLMultiArray with the specified shape and data type.</summary>
		/// <remarks>To be added.</remarks>
		public MLMultiArray (nint [] shape, MLMultiArrayDataType dataType, out NSError error)
			: this (ConvertArray (shape), dataType, out error)
		{
		}

		public MLMultiArray (IntPtr dataPointer, nint [] shape, MLMultiArrayDataType dataType, nint [] strides, Action<IntPtr> deallocator, out NSError error)
			: this (dataPointer, ConvertArray (shape), dataType, ConvertArray (strides), deallocator, out error)
		{
		}

		public NSNumber this [nint idx] {
			get { return GetObject (idx); }
			set { SetObject (value, idx); }
		}

		public NSNumber this [params nint [] indices] {
			get { return GetObject (indices); }
			set { SetObject (value, indices); }
		}

		public NSNumber this [NSNumber [] key] {
			get { return GetObject (key); }
			set { SetObject (value, key); }
		}

		/// <param name="indices">A multidimensional coordinate for the object to get.</param>
		/// <summary>Retrieves the element at <paramref name="indices" />.</summary>
		/// <returns>To be added.</returns>
		/// <remarks>To be added.</remarks>
		public NSNumber GetObject (params nint [] indices)
		{
			using (var arr = NSArray.FromNSObjects<nint> (NSNumber.FromNInt, indices))
				return GetObjectInternal (arr.GetHandle ());
		}

		/// <param name="obj">The new value</param>
		/// <param name="indices">The multidimensional coordinate of the item to set</param>
		/// <summary>Sets the element at <paramref name="indices" />.</summary>
		/// <remarks>To be added.</remarks>
		public void SetObject (NSNumber obj, params nint [] indices)
		{
			using (var arr = NSArray.FromNSObjects<nint> (NSNumber.FromNInt, indices))
				SetObjectInternal (obj, arr.GetHandle ());
		}

		/// <summary>The dimensions of the array.</summary>
		///         <value>To be added.</value>
		///         <remarks>To be added.</remarks>
		public nint [] Shape {
			get {
				return ConvertArray (_Shape);
			}
		}

		/// <summary>The number of elements to skip to advance an index value by one in the chosen direction.</summary>
		///         <value>To be added.</value>
		///         <remarks>
		///           <para>
		///             <see cref="T:CoreML.MLMultiArray" /> objects can be treated as one-dimensional arrays. The <see cref="P:CoreML.MLMultiArray.Strides" /> property retrieves the number of elements in a one-dimensional array that are necessary to "skip over" in order to advance by 1 in the desired dimension.</para>
		///           <example>
		///             <code lang="csharp lang-csharp"><![CDATA[
		/// NSError err;
		/// var ma = new CoreML.MLMultiArray(new NSNumber[] { 3, 5, 7, 9 },CoreML.MLMultiArrayDataType.Int32, out err);
		/// if (err is not null) 
		/// {
		///    HandleError(err);
		///    return;
		/// }
		/// var strides = ma.Strides;
		/// /*
		/// strides[0] = 315
		/// strides[1] =  63
		/// strides[2] =   9
		/// strides[3] =   1
		/// */        
		///
		///     ]]></code>
		///           </example>
		///         </remarks>
		public nint [] Strides {
			get {
				return ConvertArray (_Strides);
			}
		}
	}
}
