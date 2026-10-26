//
// NSIndexPath.cs
//
// Authors:
//   Miguel de Icaza
//
// Copyright 2009, Novell, Inc.
// Copyright 2014, Xamarin Inc.
//
using System.Reflection;
using System.Collections.Generic;

namespace Foundation {

	public partial class NSIndexPath {
		/// <summary>Creates an index path by copying an array of native signed integers.</summary>
		/// <param name="indexes">The indexes to copy, or an empty array to create an empty path.</param>
		/// <returns>An index path containing a copy of the indexes.</returns>
		/// <exception cref="ArgumentNullException"><paramref name="indexes" /> is <see langword="null" />.</exception>
		/// <remarks>Signed values are interpreted as unsigned native integers with the same bit pattern. Subsequent changes to the array do not affect the path.</remarks>
		public static NSIndexPath Create (params nint [] indexes)
		{
			if (indexes is null)
				throw new ArgumentNullException ("indexes");

			return Create ((ReadOnlySpan<nint>) indexes);
		}

		/// <summary>Creates an index path by copying an array of native unsigned integers.</summary>
		/// <param name="indexes">The indexes to copy, or an empty array to create an empty path.</param>
		/// <returns>An index path containing a copy of the indexes.</returns>
		/// <exception cref="ArgumentNullException"><paramref name="indexes" /> is <see langword="null" />.</exception>
		/// <remarks>Subsequent changes to the array do not affect the path.</remarks>
		public static NSIndexPath Create (params nuint [] indexes)
		{
			if (indexes is null)
				throw new ArgumentNullException ("indexes");

			return Create ((ReadOnlySpan<nuint>) indexes);
		}

		/// <summary>Creates an index path by copying an array of 32-bit signed integers.</summary>
		/// <param name="indexes">The indexes to copy, or an empty array to create an empty path.</param>
		/// <returns>An index path containing a copy of the indexes.</returns>
		/// <exception cref="ArgumentNullException"><paramref name="indexes" /> is <see langword="null" />.</exception>
		/// <remarks>Signed values are sign-extended to native width and interpreted as unsigned native integers with the same bit pattern. Subsequent changes to the array do not affect the path.</remarks>
		public static NSIndexPath Create (params int [] indexes)
		{
			if (indexes is null)
				throw new ArgumentNullException ("indexes");

			return Create ((ReadOnlySpan<int>) indexes);
		}

		/// <summary>Creates an index path by copying an array of 32-bit unsigned integers.</summary>
		/// <param name="indexes">The indexes to copy, or an empty array to create an empty path.</param>
		/// <returns>An index path containing a copy of the indexes.</returns>
		/// <exception cref="ArgumentNullException"><paramref name="indexes" /> is <see langword="null" />.</exception>
		/// <remarks>Values are zero-extended to native width. Subsequent changes to the array do not affect the path.</remarks>
		public static NSIndexPath Create (params uint [] indexes)
		{
			if (indexes is null)
				throw new ArgumentNullException ("indexes");

			return Create ((ReadOnlySpan<uint>) indexes);
		}

		/// <summary>Creates an index path by copying a span of native signed integers.</summary>
		/// <param name="indexes">The indexes to copy, or an empty span to create an empty path.</param>
		/// <returns>An index path containing a copy of the indexes.</returns>
		/// <remarks>The span is pinned only during the call. Signed values are interpreted as unsigned native integers with the same bit pattern. Subsequent changes to the source do not affect the path.</remarks>
		public unsafe static NSIndexPath Create (ReadOnlySpan<nint> indexes)
		{
			fixed (nint* ptr = indexes)
				return _FromIndex ((IntPtr) ptr, indexes.Length);
		}

		/// <summary>Creates an index path by copying a span of native unsigned integers.</summary>
		/// <param name="indexes">The indexes to copy, or an empty span to create an empty path.</param>
		/// <returns>An index path containing a copy of the indexes.</returns>
		/// <remarks>The span is pinned only during the call. Subsequent changes to the source do not affect the path.</remarks>
		public unsafe static NSIndexPath Create (ReadOnlySpan<nuint> indexes)
		{
			fixed (nuint* ptr = indexes)
				return _FromIndex ((IntPtr) ptr, indexes.Length);
		}

		/// <summary>Creates an index path by copying a span of 32-bit signed integers.</summary>
		/// <param name="indexes">The indexes to copy, or an empty span to create an empty path.</param>
		/// <returns>An index path containing a copy of the indexes.</returns>
		/// <remarks>Signed values are sign-extended to native width and interpreted as unsigned native integers with the same bit pattern. Subsequent changes to the source do not affect the path.</remarks>
		public static NSIndexPath Create (ReadOnlySpan<int> indexes)
		{
			var converted = new nint [indexes.Length];
			for (var i = 0; i < indexes.Length; i++)
				converted [i] = indexes [i];
			return Create ((ReadOnlySpan<nint>) converted);
		}

		/// <summary>Creates an index path by copying a span of 32-bit unsigned integers.</summary>
		/// <param name="indexes">The indexes to copy, or an empty span to create an empty path.</param>
		/// <returns>An index path containing a copy of the indexes.</returns>
		/// <remarks>Values are zero-extended to native width. Subsequent changes to the source do not affect the path.</remarks>
		public static NSIndexPath Create (ReadOnlySpan<uint> indexes)
		{
			var converted = new nuint [indexes.Length];
			for (var i = 0; i < indexes.Length; i++)
				converted [i] = indexes [i];
			return Create ((ReadOnlySpan<nuint>) converted);
		}

		/// <summary>Copies all indexes in this path to a new array.</summary>
		/// <returns>An independent copy of the indexes, or an empty array for an empty path.</returns>
		public nuint [] GetIndexes ()
		{
			var ret = new nuint [Length];
			GetIndexes (ret);
			return ret;
		}

		/// <summary>Copies the indexes at the specified positions to a new array.</summary>
		/// <param name="range">The positions to copy. An empty range may start at the end of the path.</param>
		/// <returns>An independent copy of the selected indexes, or an empty array for an empty range.</returns>
		/// <exception cref="ArgumentOutOfRangeException"><paramref name="range" /> has a negative location or length, or extends beyond the path.</exception>
		[SupportedOSPlatform ("ios")]
		[SupportedOSPlatform ("macos")]
		[SupportedOSPlatform ("maccatalyst")]
		[SupportedOSPlatform ("tvos")]
		public nuint [] GetIndexes (NSRange range)
		{
			ValidateRange (range);
			var ret = new nuint [range.Length];
			GetIndexes (ret, range);
			return ret;
		}

		/// <summary>Copies all indexes in this path to a span.</summary>
		/// <param name="indexes">The destination, which must have room for every index in the path.</param>
		/// <exception cref="ArgumentException"><paramref name="indexes" /> is shorter than the path.</exception>
		/// <remarks>The destination is pinned only during the call. Elements beyond the path length remain unchanged. An empty path does not modify the destination.</remarks>
		public unsafe void GetIndexes (Span<nuint> indexes)
		{
			if (indexes.Length < Length)
				throw new ArgumentException ("The destination is shorter than the index path.", nameof (indexes));
			if (Length == 0)
				return;

			fixed (nuint* ptr = indexes)
				_GetIndexes ((IntPtr) ptr);
			GC.KeepAlive (this);
		}

		/// <summary>Copies the indexes at the specified positions to a span.</summary>
		/// <param name="indexes">The destination, which must have room for every index in <paramref name="range" />.</param>
		/// <param name="range">The positions to copy. An empty range may start at the end of the path.</param>
		/// <exception cref="ArgumentOutOfRangeException"><paramref name="range" /> has a negative location or length, or extends beyond the path.</exception>
		/// <exception cref="ArgumentException"><paramref name="indexes" /> is shorter than <paramref name="range" />.</exception>
		/// <remarks>Selected indexes are copied to the beginning of the destination, which is pinned only during the call. Elements beyond the range length remain unchanged. An empty range does not modify the destination.</remarks>
		[SupportedOSPlatform ("ios")]
		[SupportedOSPlatform ("macos")]
		[SupportedOSPlatform ("maccatalyst")]
		[SupportedOSPlatform ("tvos")]
		public unsafe void GetIndexes (Span<nuint> indexes, NSRange range)
		{
			ValidateRange (range);
			if (indexes.Length < range.Length)
				throw new ArgumentException ("The destination is shorter than the requested range.", nameof (indexes));
			if (range.Length == 0)
				return;

			fixed (nuint* ptr = indexes)
				_GetIndexes ((IntPtr) ptr, range);
			GC.KeepAlive (this);
		}

		void ValidateRange (NSRange range)
		{
			var length = Length;
			if (range.Location < 0 || range.Length < 0 || range.Location > length || range.Length > length - range.Location)
				throw new ArgumentOutOfRangeException (nameof (range));
		}
	}
}
