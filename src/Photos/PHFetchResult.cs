#nullable enable

using ObjCRuntime;
using Foundation;
using System;
using System.Collections;
using System.Collections.Generic;

namespace Photos {
	public partial class PHFetchResult : IEnumerable<NSObject> {
		/// <param name="index">To be added.</param>
		/// <summary>Returns the asset at <paramref name="index" />.</summary>
		/// <value>To be added.</value>
		/// <remarks>To be added.</remarks>
		public NSObject this [nint index] {
			get { return _ObjectAtIndexedSubscript (index); }
		}

		/// <summary>A <see cref="System.Collections.Generic.IEnumerator{T}" /> that can iterate over the assets in the <see cref="Photos.PHFetchResult" />.</summary>
		///         <returns>To be added.</returns>
		///         <remarks>To be added.</remarks>
		public IEnumerator<NSObject> GetEnumerator ()
		{
			nint len = Count;

			for (nint i = 0; i < len; i++)
				yield return this [i];
		}

		/// <summary>Returns an <see cref="System.Collections.IEnumerator" /> of the assets in the <see cref="Photos.PHFetchResult" />.</summary>
		///         <returns>To be added.</returns>
		///         <remarks>To be added.</remarks>
		IEnumerator IEnumerable.GetEnumerator ()
		{
			nint len = Count;

			for (nint i = 0; i < len; i++)
				yield return this [i];
		}

		/// <typeparam name="T">To be added.</typeparam>
		///         <param name="indexes">To be added.</param>
		///         <summary>Returns the objects at <paramref name="indexes" />, all of which must be type T.</summary>
		///         <returns>To be added.</returns>
		///         <remarks>To be added.</remarks>
		public T [] ObjectsAt<T> (NSIndexSet indexes) where T : NSObject
		{
			var nsarr = _ObjectsAt (indexes);
			return NSArray.ArrayFromHandle<T> (nsarr);
		}
	}
}
