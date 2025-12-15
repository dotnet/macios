//
// Authors:
//   Miguel de Icaza
//
// Copyright 2009, Novell, Inc.
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
//
using System.Globalization;
using System.Runtime.CompilerServices;

#nullable enable

namespace Foundation {
	/// <summary>Represents an immutable value that can range from mantissa*10^exponent where mantissa is a decimal integer of up to 38 digits length, and the exponent is an integer that can range from -128 through 127.</summary>
	/// <remarks>This structure provides a high-precision decimal type that is compatible with Foundation's NSDecimal type.</remarks>
	[SupportedOSPlatform ("ios")]
	[SupportedOSPlatform ("maccatalyst")]
	[SupportedOSPlatform ("macos")]
	[SupportedOSPlatform ("tvos")]
	[StructLayout (LayoutKind.Sequential)]
	public struct NSDecimal
#if !COREBUILD
		: IEquatable<NSDecimal>
#endif
	{
		// unsigned int
		/// <summary>Bit field containing the sign, exponent, length, and validity information.</summary>
		public int fields;
		// unsigned short [8]
		/// <summary>First component of the mantissa.</summary>
		public short m1;
		/// <summary>Second component of the mantissa.</summary>
		public short m2;
		/// <summary>Third component of the mantissa.</summary>
		public short m3;
		/// <summary>Fourth component of the mantissa.</summary>
		public short m4;
		/// <summary>Fifth component of the mantissa.</summary>
		public short m5;
		/// <summary>Sixth component of the mantissa.</summary>
		public short m6;
		/// <summary>Seventh component of the mantissa.</summary>
		public short m7;
		/// <summary>Eighth component of the mantissa.</summary>
		public short m8;

#if !COREBUILD
		[DllImport (Constants.FoundationLibrary)]
		unsafe static extern nint NSDecimalCompare (NSDecimal* left, NSDecimal* right);
		/// <summary>Compares two <see cref="NSDecimal"/> values.</summary>
		/// <param name="left">The first value to compare.</param>
		/// <param name="right">The second value to compare.</param>
		/// <returns>An <see cref="NSComparisonResult"/> indicating the ordering of the two values.</returns>
		public unsafe static NSComparisonResult Compare (ref NSDecimal left, ref NSDecimal right)
		{
			fixed (NSDecimal* leftPtr = &left, rightPtr = &right) {
				return (NSComparisonResult) (long) NSDecimalCompare (leftPtr, rightPtr);
			}
		}

		[DllImport (Constants.FoundationLibrary)]
		unsafe static extern void NSDecimalRound (NSDecimal* result, NSDecimal* number, nint scale, nuint mode);
		/// <summary>Rounds an <see cref="NSDecimal"/> value to the specified number of decimal places using the specified rounding mode.</summary>
		/// <param name="result">The rounded result.</param>
		/// <param name="number">The number to round.</param>
		/// <param name="scale">The number of decimal places to round to.</param>
		/// <param name="mode">The <see cref="NSRoundingMode"/> to use.</param>
		public unsafe static void Round (out NSDecimal result, ref NSDecimal number, nint scale, NSRoundingMode mode)
		{
			result = default (NSDecimal);
			fixed (NSDecimal* resultPtr = &result, numberPtr = &number) {
				NSDecimalRound (
					resultPtr,
					numberPtr,
					scale,
					(nuint) (ulong) mode);
			}
		}

		[DllImport (Constants.FoundationLibrary)]
		unsafe static extern nuint NSDecimalNormalize (NSDecimal* number1, NSDecimal* number2);
		/// <summary>Normalizes two <see cref="NSDecimal"/> values to have the same exponent.</summary>
		/// <param name="number1">The first number to normalize.</param>
		/// <param name="number2">The second number to normalize.</param>
		/// <returns>An <see cref="NSCalculationError"/> indicating whether the operation succeeded or failed.</returns>
		public unsafe static NSCalculationError Normalize (ref NSDecimal number1, ref NSDecimal number2)
		{
			fixed (NSDecimal* number1Ptr = &number1, number2Ptr = &number2) {
				return (NSCalculationError) (ulong) NSDecimalNormalize (number1Ptr, number2Ptr);
			}
		}

		[DllImport (Constants.FoundationLibrary)]
		static unsafe extern nuint NSDecimalAdd (NSDecimal* result, NSDecimal* left, NSDecimal* right, nuint mode);
		/// <summary>Adds two <see cref="NSDecimal"/> values.</summary>
		/// <param name="result">The result of the addition.</param>
		/// <param name="left">The first value to add.</param>
		/// <param name="right">The second value to add.</param>
		/// <param name="mode">The <see cref="NSRoundingMode"/> to use.</param>
		/// <returns>An <see cref="NSCalculationError"/> indicating whether the operation succeeded or failed.</returns>
		public unsafe static NSCalculationError Add (out NSDecimal result, ref NSDecimal left, ref NSDecimal right, NSRoundingMode mode)
		{
			result = default (NSDecimal);
			fixed (NSDecimal* resultPtr = &result, leftPtr = &left, rightPtr = &right) {
				return (NSCalculationError) (ulong) NSDecimalAdd (
					resultPtr,
					leftPtr,
					rightPtr,
					(nuint) (ulong) mode);
			}
		}

		[DllImport (Constants.FoundationLibrary)]
		unsafe static extern nuint NSDecimalSubtract (NSDecimal* result, NSDecimal* left, NSDecimal* right, nuint mode);
		/// <summary>Subtracts one <see cref="NSDecimal"/> value from another.</summary>
		/// <param name="result">The result of the subtraction.</param>
		/// <param name="left">The value to subtract from.</param>
		/// <param name="right">The value to subtract.</param>
		/// <param name="mode">The <see cref="NSRoundingMode"/> to use.</param>
		/// <returns>An <see cref="NSCalculationError"/> indicating whether the operation succeeded or failed.</returns>
		public unsafe static NSCalculationError Subtract (out NSDecimal result, ref NSDecimal left, ref NSDecimal right, NSRoundingMode mode)
		{
			result = default (NSDecimal);
			fixed (NSDecimal* resultPtr = &result, leftPtr = &left, rightPtr = &right) {
				return (NSCalculationError) (ulong) NSDecimalSubtract (
					resultPtr,
					leftPtr,
					rightPtr,
					(nuint) (ulong) mode);
			}
		}

		[DllImport (Constants.FoundationLibrary)]
		static unsafe extern nuint NSDecimalMultiply (NSDecimal* result, NSDecimal* left, NSDecimal* right, nuint mode);
		/// <summary>Multiplies two <see cref="NSDecimal"/> values.</summary>
		/// <param name="result">The result of the multiplication.</param>
		/// <param name="left">The first value to multiply.</param>
		/// <param name="right">The second value to multiply.</param>
		/// <param name="mode">The <see cref="NSRoundingMode"/> to use.</param>
		/// <returns>An <see cref="NSCalculationError"/> indicating whether the operation succeeded or failed.</returns>
		public unsafe static NSCalculationError Multiply (out NSDecimal result, ref NSDecimal left, ref NSDecimal right, NSRoundingMode mode)
		{
			result = default (NSDecimal);
			fixed (NSDecimal* resultPtr = &result, leftPtr = &left, rightPtr = &right) {
				return (NSCalculationError) (ulong) NSDecimalMultiply (
					resultPtr,
					leftPtr,
					rightPtr,
					(nuint) (ulong) mode);
			}
		}

		[DllImport (Constants.FoundationLibrary)]
		unsafe static extern nuint NSDecimalDivide (NSDecimal* result, NSDecimal* left, NSDecimal* right, nuint mode);
		/// <summary>Divides one <see cref="NSDecimal"/> value by another.</summary>
		/// <param name="result">The result of the division.</param>
		/// <param name="left">The dividend.</param>
		/// <param name="right">The divisor.</param>
		/// <param name="mode">The <see cref="NSRoundingMode"/> to use.</param>
		/// <returns>An <see cref="NSCalculationError"/> indicating whether the operation succeeded or failed.</returns>
		public unsafe static NSCalculationError Divide (out NSDecimal result, ref NSDecimal left, ref NSDecimal right, NSRoundingMode mode)
		{
			result = default (NSDecimal);
			fixed (NSDecimal* resultPtr = &result, leftPtr = &left, rightPtr = &right) {
				return (NSCalculationError) (ulong) NSDecimalDivide (
					resultPtr,
					leftPtr,
					rightPtr,
					(nuint) (ulong) mode);
			}
		}

		[DllImport (Constants.FoundationLibrary)]
		unsafe static extern nuint NSDecimalPower (NSDecimal* result, NSDecimal* number, nint power, nuint mode);
		/// <summary>Raises an <see cref="NSDecimal"/> value to the specified power.</summary>
		/// <param name="result">The result of the exponentiation.</param>
		/// <param name="number">The base value.</param>
		/// <param name="power">The exponent.</param>
		/// <param name="mode">The <see cref="NSRoundingMode"/> to use.</param>
		/// <returns>An <see cref="NSCalculationError"/> indicating whether the operation succeeded or failed.</returns>
		public unsafe static NSCalculationError Power (out NSDecimal result, ref NSDecimal number, nint power, NSRoundingMode mode)
		{
			result = default (NSDecimal);
			fixed (NSDecimal* resultPtr = &result, numberPtr = &number) {
				return (NSCalculationError) (ulong) NSDecimalPower (
					resultPtr,
					numberPtr,
					power,
					(nuint) (ulong) mode);
			}
		}

		[DllImport (Constants.FoundationLibrary)]
		unsafe static extern nuint NSDecimalMultiplyByPowerOf10 (NSDecimal* result, NSDecimal* number, short power10, nuint mode);
		/// <summary>Multiplies an <see cref="NSDecimal"/> value by a power of 10.</summary>
		/// <param name="result">The result of the multiplication.</param>
		/// <param name="number">The value to multiply.</param>
		/// <param name="power10">The power of 10 to multiply by.</param>
		/// <param name="mode">The <see cref="NSRoundingMode"/> to use.</param>
		/// <returns>An <see cref="NSCalculationError"/> indicating whether the operation succeeded or failed.</returns>
		public unsafe static NSCalculationError MultiplyByPowerOf10 (out NSDecimal result, ref NSDecimal number, short power10, NSRoundingMode mode)
		{
			result = default (NSDecimal);
			fixed (NSDecimal* resultPtr = &result, numberPtr = &number) {
				return (NSCalculationError) (ulong) NSDecimalMultiplyByPowerOf10 (
					resultPtr,
					numberPtr,
					power10,
					(nuint) (ulong) mode);
			}
		}

		[DllImport (Constants.FoundationLibrary)]
		unsafe static extern IntPtr NSDecimalString (NSDecimal* value, /* _Nullable */ IntPtr locale);

		/// <summary>Returns a string representation of the <see cref="NSDecimal"/> value.</summary>
		/// <returns>A string representation of this value.</returns>
		public override string ToString ()
		{
			unsafe {
				fixed (NSDecimal* self = &this) {
					return new NSString (NSDecimalString (self, NSLocale.CurrentLocale.Handle));
				}
			}
		}

		/// <summary>Adds two <see cref="NSDecimal"/> values.</summary>
		/// <param name="left">The first value to add.</param>
		/// <param name="right">The second value to add.</param>
		/// <returns>The sum of the two values.</returns>
		public static NSDecimal operator + (NSDecimal left, NSDecimal right)
		{
			NSDecimal result;

			Add (out result, ref left, ref right, NSRoundingMode.Plain);
			return result;
		}

		/// <summary>Subtracts one <see cref="NSDecimal"/> value from another.</summary>
		/// <param name="left">The value to subtract from.</param>
		/// <param name="right">The value to subtract.</param>
		/// <returns>The difference between the two values.</returns>
		public static NSDecimal operator - (NSDecimal left, NSDecimal right)
		{
			NSDecimal result;

			Subtract (out result, ref left, ref right, NSRoundingMode.Plain);
			return result;
		}

		/// <summary>Multiplies two <see cref="NSDecimal"/> values.</summary>
		/// <param name="left">The first value to multiply.</param>
		/// <param name="right">The second value to multiply.</param>
		/// <returns>The product of the two values.</returns>
		public static NSDecimal operator * (NSDecimal left, NSDecimal right)
		{
			NSDecimal result;

			Multiply (out result, ref left, ref right, NSRoundingMode.Plain);
			return result;
		}

		/// <summary>Divides one <see cref="NSDecimal"/> value by another.</summary>
		/// <param name="left">The dividend.</param>
		/// <param name="right">The divisor.</param>
		/// <returns>The quotient of the two values.</returns>
		public static NSDecimal operator / (NSDecimal left, NSDecimal right)
		{
			NSDecimal result;

			Divide (out result, ref left, ref right, NSRoundingMode.Plain);
			return result;
		}

		/// <summary>Determines whether two <see cref="NSDecimal"/> values are equal.</summary>
		/// <param name="left">The first value to compare.</param>
		/// <param name="right">The second value to compare.</param>
		/// <returns><see langword="true"/> if the values are equal; otherwise, <see langword="false"/>.</returns>
		public static bool operator == (NSDecimal left, NSDecimal right)
		{
			return Compare (ref left, ref right) == NSComparisonResult.Same;
		}

		/// <summary>Determines whether two <see cref="NSDecimal"/> values are not equal.</summary>
		/// <param name="left">The first value to compare.</param>
		/// <param name="right">The second value to compare.</param>
		/// <returns><see langword="true"/> if the values are not equal; otherwise, <see langword="false"/>.</returns>
		public static bool operator != (NSDecimal left, NSDecimal right)
		{
			return Compare (ref left, ref right) != NSComparisonResult.Same;
		}

		/// <summary>Implicitly converts an <see cref="int"/> to an <see cref="NSDecimal"/>.</summary>
		/// <param name="value">The value to convert.</param>
		/// <returns>An <see cref="NSDecimal"/> representing the specified value.</returns>
		public static implicit operator NSDecimal (int value)
		{
			using var number = new NSNumber (value);
			return number.NSDecimalValue;
		}

		/// <summary>Explicitly converts an <see cref="NSDecimal"/> to an <see cref="int"/>.</summary>
		/// <param name="value">The value to convert.</param>
		/// <returns>An <see cref="int"/> representing the specified value.</returns>
		public static explicit operator int (NSDecimal value)
		{
			using var number = new NSDecimalNumber (value);
			return number.Int32Value;
		}

		/// <summary>Implicitly converts a <see cref="float"/> to an <see cref="NSDecimal"/>.</summary>
		/// <param name="value">The value to convert.</param>
		/// <returns>An <see cref="NSDecimal"/> representing the specified value.</returns>
		public static implicit operator NSDecimal (float value)
		{
			using var number = new NSNumber (value);
			return number.NSDecimalValue;
		}

		/// <summary>Explicitly converts an <see cref="NSDecimal"/> to a <see cref="float"/>.</summary>
		/// <param name="value">The value to convert.</param>
		/// <returns>A <see cref="float"/> representing the specified value.</returns>
		public static explicit operator float (NSDecimal value)
		{
			using var number = new NSDecimalNumber (value);
			return number.FloatValue;
		}

		/// <summary>Implicitly converts a <see cref="double"/> to an <see cref="NSDecimal"/>.</summary>
		/// <param name="value">The value to convert.</param>
		/// <returns>An <see cref="NSDecimal"/> representing the specified value.</returns>
		public static implicit operator NSDecimal (double value)
		{
			using var number = new NSNumber (value);
			return number.NSDecimalValue;
		}

		/// <summary>Explicitly converts an <see cref="NSDecimal"/> to a <see cref="double"/>.</summary>
		/// <param name="value">The value to convert.</param>
		/// <returns>A <see cref="double"/> representing the specified value.</returns>
		public static explicit operator double (NSDecimal value)
		{
			using var number = new NSDecimalNumber (value);
			return number.DoubleValue;
		}

		/// <summary>Implicitly converts a <see cref="decimal"/> to an <see cref="NSDecimal"/>.</summary>
		/// <param name="value">The value to convert.</param>
		/// <returns>An <see cref="NSDecimal"/> representing the specified value.</returns>
		public static implicit operator NSDecimal (decimal value)
		{
			using var number = new NSDecimalNumber (value.ToString (CultureInfo.InvariantCulture));
			return number.NSDecimalValue;
		}

		/// <summary>Explicitly converts an <see cref="NSDecimal"/> to a <see cref="decimal"/>.</summary>
		/// <param name="value">The value to convert.</param>
		/// <returns>A <see cref="decimal"/> representing the specified value.</returns>
		public static explicit operator decimal (NSDecimal value)
		{
			using var number = new NSDecimalNumber (value);
			return decimal.Parse (number.ToString (), CultureInfo.InvariantCulture);
		}

		/// <summary>Determines whether the current <see cref="NSDecimal"/> is equal to another <see cref="NSDecimal"/>.</summary>
		/// <param name="other">The value to compare with the current value.</param>
		/// <returns><see langword="true"/> if the values are equal; otherwise, <see langword="false"/>.</returns>
		public bool Equals (NSDecimal other)
		{
			return this == other;
		}

		/// <summary>Determines whether the current <see cref="NSDecimal"/> is equal to the specified object.</summary>
		/// <param name="obj">The object to compare with the current value.</param>
		/// <returns><see langword="true"/> if the specified object is an <see cref="NSDecimal"/> and is equal to the current value; otherwise, <see langword="false"/>.</returns>
		public override bool Equals (object? obj)
		{
			return obj is NSDecimal other && this == other;
		}

		/// <summary>Returns the hash code for this instance.</summary>
		/// <returns>A 32-bit signed integer hash code.</returns>
		public override int GetHashCode ()
		{
			// this is heavy weight :( but it's the only way to follow .NET rule where:
			// "If two objects compare as equal, the GetHashCode method for each object must return the same value."
			// otherwise the same value (e.g. 100) can be represented with different values (e.g.
			// by varying mantissa an exponent)
			return ToString ().GetHashCode ();
		}
#endif // !COREBUILD
	}
}
