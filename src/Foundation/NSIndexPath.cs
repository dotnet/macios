//
// NSIndexPath.cs
//
// Authors:
//   Miguel de Icaza
//
// Copyright 2009, Novell, Inc.
// Copyright 2014, Xamarin Inc.
//
using System;
using System.Reflection;
using System.Collections.Generic;
using System.Runtime.InteropServices;

using ObjCRuntime;

namespace Foundation {

	public partial class NSIndexPath {
		/// <param name="indexes">
		/// Array of indexes to make the index-path.
		///   	   </param>
		/// <summary>Creates an <see cref="Foundation.NSIndexPath" /> with the indexes specified in the provided array of native integers.</summary>
		/// <returns>To be added.</returns>
		/// <remarks>
		///           <para />
		///         </remarks>
		public unsafe static NSIndexPath Create (params nint [] indexes)
		{
			if (indexes is null)
				throw new ArgumentNullException ("indexes");

			fixed (nint* ptr = indexes)
				return _FromIndex ((IntPtr) ptr, indexes.Length);
		}

		/// <param name="indexes">
		/// Array of indexes to make the index-path.
		///   	   </param>
		/// <summary>Creates an <see cref="Foundation.NSIndexPath" /> with the indexes specified in the provided array of native unsigned integers.</summary>
		/// <returns>To be added.</returns>
		/// <remarks>
		///           <para>
		///           </para>
		///         </remarks>
		public unsafe static NSIndexPath Create (params nuint [] indexes)
		{
			if (indexes is null)
				throw new ArgumentNullException ("indexes");

			fixed (nuint* ptr = indexes)
				return _FromIndex ((IntPtr) ptr, indexes.Length);
		}

		/// <param name="indexes">
		///
		///
		///
		/// Array of indexes to make the index-path.
		///
		///
		///
		///   	   </param>
		///         <summary>Creates an <see cref="Foundation.NSIndexPath" /> with the indexes specified in the provided array of integers.</summary>
		///         <returns>
		///         </returns>
		///         <remarks>
		///         </remarks>
		public unsafe static NSIndexPath Create (params int [] indexes)
		{
			if (indexes is null)
				throw new ArgumentNullException ("indexes");

			fixed (nint* ptr = Array.ConvertAll<int, nint> (indexes, (v) => v))
				return _FromIndex ((IntPtr) ptr, indexes.Length);
		}

		/// <param name="indexes">
		///
		///
		///
		/// Array of indexes to make the index-path.
		///
		///
		///
		///   	   </param>
		///         <summary>Creates an <see cref="Foundation.NSIndexPath" /> with the indexes specified in the provided array of unsigned integers.</summary>
		///         <returns>
		///         </returns>
		///         <remarks>
		///         </remarks>
		public unsafe static NSIndexPath Create (params uint [] indexes)
		{
			if (indexes is null)
				throw new ArgumentNullException ("indexes");

			fixed (nuint* ptr = Array.ConvertAll<uint, nuint> (indexes, (v) => v))
				return _FromIndex ((IntPtr) ptr, indexes.Length);
		}

		/// <summary>Copies the objects contained in the index-path to an array.</summary>
		public unsafe nuint [] GetIndexes ()
		{
			var ret = new nuint [Length];
			fixed (nuint* ptr = ret)
				_GetIndexes ((IntPtr) ptr);
			return ret;
		}

		/// <param name="range">To be added.</param>
		///         <summary>To be added.</summary>
		///         <returns>To be added.</returns>
		///         <remarks>To be added.</remarks>
		[SupportedOSPlatform ("ios")]
		[SupportedOSPlatform ("macos")]
		[SupportedOSPlatform ("maccatalyst")]
		[SupportedOSPlatform ("tvos")]
		public unsafe nuint [] GetIndexes (NSRange range)
		{
			var ret = new nuint [range.Length];
			fixed (nuint* ptr = ret)
				_GetIndexes ((IntPtr) ptr, range);
			return ret;
		}
	}
}
