// Copyright (c) Microsoft Corporation.
// Licensed under the MIT License.
//
// VectorDouble2.cs:
//     This represents the native vector_byte16 type, which is 16 bytes.
//

#nullable enable

using System;
using System.Runtime.InteropServices;
using System.Text;

// This type does not come from the CoreGraphics framework; it's defined in /usr/include/simd/vector_types.h
namespace CoreGraphics {
	/// <summary>Represents a native vector_byte16 type, which contains 16 byte values.</summary>
	/// <remarks>This type provides a managed representation of the native vector_byte16 type defined in /usr/include/simd/vector_types.h.</remarks>
	[StructLayout (LayoutKind.Sequential)]
	public struct NVector16b : IEquatable<NVector16b> {
		byte v01;
		byte v02;
		byte v03;
		byte v04;
		byte v05;
		byte v06;
		byte v07;
		byte v08;
		byte v09;
		byte v10;
		byte v11;
		byte v12;
		byte v13;
		byte v14;
		byte v15;
		byte v16;

		/// <summary>Initializes a new instance of the <see cref="NVector16b"/> struct from a byte array.</summary>
		/// <param name="array">The byte array to copy values from. If null, the vector will be initialized to zero. Only the first 16 elements will be used if the array is larger.</param>
		/// <remarks>If the array has fewer than 16 elements, the remaining elements of the vector will be initialized to zero.</remarks>
		public NVector16b (params byte []? array)
		{
			if (array is not null) {
				for (var i = 0; i < Math.Min (array.Length, 16); i++)
					this [i] = array [i];
			}
		}

		/// <summary>Gets or sets the byte value at the specified index.</summary>
		/// <param name="index">The zero-based index of the element to get or set. Must be between 0 and 15 (inclusive).</param>
		/// <value>The byte value at the specified index.</value>
		/// <returns>The byte value at the specified index.</returns>
		/// <exception cref="ArgumentOutOfRangeException">Thrown when <paramref name="index"/> is less than 0 or greater than 15.</exception>
		public byte this [int index] {
			get {
				if (index < 0 || index > 15)
					ObjCRuntime.ThrowHelper.ThrowArgumentOutOfRangeException (nameof (index), "The index must be positive and less than 16.");
				unsafe {
					fixed (byte* ptr = &this.v01)
						return ptr [index];
				}
			}
			set {
				if (index < 0 || index > 15)
					ObjCRuntime.ThrowHelper.ThrowArgumentOutOfRangeException (nameof (index), "The index must be positive and less than 16.");
				unsafe {
					fixed (byte* ptr = &this.v01)
						ptr [index] = value;
				}
			}
		}

		/// <summary>Determines whether two <see cref="NVector16b"/> instances are equal.</summary>
		/// <param name="left">The first vector to compare.</param>
		/// <param name="right">The second vector to compare.</param>
		/// <returns><c>true</c> if the vectors are equal; otherwise, <c>false</c>.</returns>
		public static bool operator == (NVector16b left, NVector16b right)
		{
			return left.Equals (right);
		}

		/// <summary>Determines whether two <see cref="NVector16b"/> instances are not equal.</summary>
		/// <param name="left">The first vector to compare.</param>
		/// <param name="right">The second vector to compare.</param>
		/// <returns><c>true</c> if the vectors are not equal; otherwise, <c>false</c>.</returns>
		public static bool operator != (NVector16b left, NVector16b right)
		{
			return !left.Equals (right);
		}

		/// <summary>Returns a string representation of the vector.</summary>
		/// <returns>A string representation of the vector in the format "(v1, v2, v3, ..., v16)".</returns>
		public override string ToString ()
		{
			var sb = new StringBuilder ();
			sb.Append ('(');
			for (var i = 0; i < 16; i++) {
				if (i > 0)
					sb.Append (", ");
				sb.Append (this [i]);
			}
			sb.Append (')');
			return sb.ToString ();
		}

		/// <summary>Returns a read-only span over the 16 bytes of this vector.</summary>
		/// <returns>A <see cref="ReadOnlySpan{T}"/> of <see cref="byte"/> representing the vector's data.</returns>
		/// <remarks>This method provides direct access to the underlying bytes of the vector without copying.</remarks>
		public unsafe ReadOnlySpan<byte> AsSpan ()
		{
			fixed (byte* ptr = &v01)
				return new ReadOnlySpan<byte> (ptr, 16);
		}

		/// <summary>Returns the hash code for this vector.</summary>
		/// <returns>A 32-bit signed integer hash code.</returns>
		/// <remarks>The hash code is computed from all 16 byte values in the vector.</remarks>
		public override int GetHashCode ()
		{
			var hc = new HashCode ();
			hc.AddBytes (AsSpan ());
			return hc.ToHashCode ();
		}

		/// <summary>Determines whether the specified object is equal to this vector.</summary>
		/// <param name="obj">The object to compare with this vector.</param>
		/// <returns><c>true</c> if the specified object is a <see cref="NVector16b"/> and is equal to this vector; otherwise, <c>false</c>.</returns>
		public override bool Equals (object? obj)
		{
			if (!(obj is NVector16b vector))
				return false;

			return Equals (vector);
		}

		/// <summary>Determines whether this vector is equal to the specified vector.</summary>
		/// <param name="other">The vector to compare with this vector.</param>
		/// <returns><c>true</c> if the vectors are equal; otherwise, <c>false</c>.</returns>
		/// <remarks>Two vectors are considered equal if all corresponding byte values are equal.</remarks>
		public bool Equals (NVector16b other)
		{
			return AsSpan ().SequenceEqual (other.AsSpan ());
		}

		/// <summary>Gets a vector with all elements set to zero.</summary>
		/// <value>A <see cref="NVector16b"/> with all 16 byte values set to 0.</value>
		/// <remarks>This property provides a convenient way to get a zero-initialized vector.</remarks>
		public static NVector16b Zero {
			get => default;
		}
	}
}
