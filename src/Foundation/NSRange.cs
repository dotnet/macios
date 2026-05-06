//
// Copyright 2010, Novell, Inc.
// Copyright 2011, 2012 Xamarin Inc
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

#nullable enable

namespace Foundation {
	/// <summary>Represents a range given by a location and length.</summary>
	/// <remarks>
	///   <para>The <see cref="NSRange" /> structure is used throughout Foundation to represent ranges of items, such as characters in a string or elements in an array.</para>
	/// </remarks>
	/// <related type="sample" href="https://github.com/xamarin/ios-samples/tree/master/SimpleTextInput/">SimpleTextInput</related>
	public struct NSRange : IEquatable<NSRange> {
		/// <summary>The starting location of the range.</summary>
		public nint Location;
		/// <summary>The length of the range.</summary>
		public nint Length;

		/// <summary>A constant indicating that a requested item was not found.</summary>
		/// <remarks>This value is equal to <see cref="nint.MaxValue" />.</remarks>
		public static readonly nint NotFound = nint.MaxValue;

		/// <summary>Initializes a new instance of the <see cref="NSRange" /> structure with the specified location and length.</summary>
		/// <param name="start">The starting location of the range.</param>
		/// <param name="len">The length of the range.</param>
		public NSRange (nint start, nint len)
		{
			Location = start;
			Length = len;
		}

		/// <summary>Returns the hash code for this <see cref="NSRange" />.</summary>
		/// <returns>A 32-bit signed integer hash code.</returns>
		public override int GetHashCode ()
		{
			return HashCode.Combine (Location, Length);
		}

		/// <summary>Determines whether this <see cref="NSRange" /> and the specified object have the same value.</summary>
		/// <param name="obj">The object to compare with this instance.</param>
		/// <returns><see langword="true" /> if <paramref name="obj" /> is an <see cref="NSRange" /> and has the same location and length as this instance; otherwise, <see langword="false" />.</returns>
		public override bool Equals (object? obj)
		{
			return obj is NSRange other && Equals (other);
		}

		/// <summary>Determines whether this <see cref="NSRange" /> and another <see cref="NSRange" /> have the same value.</summary>
		/// <param name="other">The <see cref="NSRange" /> to compare with this instance.</param>
		/// <returns><see langword="true" /> if <paramref name="other" /> has the same location and length as this instance; otherwise, <see langword="false" />.</returns>
		public bool Equals (NSRange other)
		{
			return Location == other.Location && Length == other.Length;
		}

		/// <summary>Returns a string representation of this <see cref="NSRange" />.</summary>
		/// <returns>A string in the format "[Location={Location},Length={Length}]".</returns>
		public override string ToString ()
		{
			return string.Format ("[Location={0},Length={1}]", Location, Length);
		}
	}
}
