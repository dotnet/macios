//
// NSNumber.cs: extensions to NSNumber
//
// Authors:
//   Aaron Bockover (abock@xamarin.com)
//
// Copyright 2013 Xamarin, Inc.

#if MONOMAC

#nullable enable

namespace Foundation {
	public partial class NSNumber {
#if !COREBUILD
		/// <summary>Creates an <see cref="NSNumber" /> from a boxed numeric value or boolean.</summary>
		/// <param name="value">A boxed numeric value (float, double, sbyte, byte, short, ushort, int, uint, long, ulong, nint, nuint, nfloat) or boolean.</param>
		/// <returns>An <see cref="NSNumber" /> representing the value, or <see langword="null" /> if the value type is not supported.</returns>
		public static NSNumber? FromObject (object? value)
		{
			if (value is float) {
				return FromFloat ((float) value);
			} else if (value is double) {
				return FromDouble ((double) value);
			} else if (value is sbyte) {
				return FromSByte ((sbyte) value);
			} else if (value is byte) {
				return FromByte ((byte) value);
			} else if (value is short) {
				return FromInt16 ((short) value);
			} else if (value is ushort) {
				return FromUInt16 ((ushort) value);
			} else if (value is int) {
				return FromInt32 ((int) value);
			} else if (value is uint) {
				return FromUInt32 ((uint) value);
			} else if (value is long) {
				return FromInt64 ((long) value);
			} else if (value is ulong) {
				return FromUInt64 ((ulong) value);
			} else if (value is bool) {
				return FromBoolean ((bool) value);
			} else if (value is nint) {
				return FromNInt ((nint) value);
			} else if (value is nuint) {
				return FromNUInt ((nuint) value);
			} else if (value is nfloat) {
				return FromNFloat ((nfloat) value);
			}

			return null;
		}
#endif
	}
}

#endif // MONOMAC
