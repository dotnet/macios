//
// Authors:
//     Rolf Bjarne Kvinge <rolf@xamarin.com>
//
// Copyright (c) 2017 Microsoft Inc
//

// 
// This represents the native matrix_float2x2 type, which has a column-major layout
// (as opposed to OpenTK.Matrix2, which has a row-major layout).
// 

#nullable enable

using System;
using System.ComponentModel;
using System.Runtime.InteropServices;
using System.Runtime.Versioning;

// This type does not come from the CoreGraphics framework; it's defined in /usr/include/simd/matrix_types.h
namespace CoreGraphics {
	[SupportedOSPlatform ("ios")]
	[SupportedOSPlatform ("maccatalyst")]
	[SupportedOSPlatform ("macos")]
	[SupportedOSPlatform ("tvos")]
	[StructLayout (LayoutKind.Sequential)]
	public struct NMatrix2 : IEquatable<NMatrix2> {
		public float M11;
		public float M21;
		public float M12;
		public float M22;

#if !XAMCORE_5_0
		[Obsolete ("Use 'M11' instead.")]
		[EditorBrowsable (EditorBrowsableState.Never)]
		public float R0C0 { get => M11; set => M11 = value; }
		[Obsolete ("Use 'M21' instead.")]
		[EditorBrowsable (EditorBrowsableState.Never)]
		public float R1C0 { get => M21; set => M21 = value; }
		[Obsolete ("Use 'M12' instead.")]
		[EditorBrowsable (EditorBrowsableState.Never)]
		public float R0C1 { get => M12; set => M12 = value; }
		[Obsolete ("Use 'M22' instead.")]
		[EditorBrowsable (EditorBrowsableState.Never)]
		public float R1C1 { get => M22; set => M22 = value; }
#endif // !XAMCORE_5_0

		static readonly NMatrix2 _identity = new NMatrix2
		(
			1, 0,
			0, 1);

		public static NMatrix2 Identity { get => _identity; }

		public NMatrix2 (
			float m11, float m12,
			float m21, float m22)
		{
			M11 = m11;
			M21 = m21;
			M12 = m12;
			M22 = m22;
		}

		public readonly float GetDeterminant ()
		{
			return M11 * M22 - M21 * M12;
		}

		public float Determinant {
			get {
				return M11 * M22 - M21 * M12;
			}
		}

		public void Transpose ()
		{
			this = Transpose (this);
		}

		public static NMatrix2 Transpose (NMatrix2 mat)
		{
			return new NMatrix2 (mat.M11, mat.M21, mat.M12, mat.M22);
		}

		public static void Transpose (ref NMatrix2 mat, out NMatrix2 result)
		{
			result.M11 = mat.M11;
			result.M12 = mat.M21;
			result.M21 = mat.M12;
			result.M22 = mat.M22;
		}

		public static NMatrix2 Multiply (NMatrix2 left, NMatrix2 right)
		{
			NMatrix2 result;
			Multiply (ref left, ref right, out result);
			return result;
		}

		public static void Multiply (ref NMatrix2 left, ref NMatrix2 right, out NMatrix2 result)
		{
			result.M11 = left.M11 * right.M11 + left.M12 * right.M21;
			result.M12 = left.M11 * right.M12 + left.M12 * right.M22;

			result.M21 = left.M21 * right.M11 + left.M22 * right.M21;
			result.M22 = left.M21 * right.M12 + left.M22 * right.M22;
		}

		public static NMatrix2 operator * (NMatrix2 left, NMatrix2 right)
		{
			return Multiply (left, right);
		}

		public static bool operator == (NMatrix2 left, NMatrix2 right)
		{
			return left.Equals (right);
		}

		public static bool operator != (NMatrix2 left, NMatrix2 right)
		{
			return !left.Equals (right);
		}

		public override string ToString ()
		{
			return $"({M11}, {M12})\n({M21}, {M22})";
		}

		public override int GetHashCode ()
		{
			return HashCode.Combine (M11, M12, M21, M22);
		}

		public override bool Equals (object? obj)
		{
			if (!(obj is NMatrix2 matrix))
				return false;

			return Equals (matrix);
		}

		public bool Equals (NMatrix2 other)
		{
			return
				M11 == other.M11 &&
				M12 == other.M12 &&
				M21 == other.M21 &&
				M22 == other.M22;
		}
	}
}
