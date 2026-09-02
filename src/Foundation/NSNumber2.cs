//
// Copyright 2010, Novell, Inc.
// Copyright 2014 Xamarin Inc. All rights reserved.
//

using System.Reflection;
using System.Collections;

#nullable enable

namespace Foundation {
	public partial class NSNumber : NSValue
#if COREBUILD
	{
#else
	, IComparable, IComparable<NSNumber>, IEquatable<NSNumber> {

		/// <summary>
		/// Implicitly converts a <see cref="float"/> value to an <see cref="NSNumber"/>.
		/// </summary>
		/// <param name="value">The float value to convert.</param>
		/// <returns>An <see cref="NSNumber"/> representing the specified value.</returns>
		public static implicit operator NSNumber (float value)
		{
			return FromFloat (value);
		}

		/// <summary>
		/// Implicitly converts a <see cref="double"/> value to an <see cref="NSNumber"/>.
		/// </summary>
		/// <param name="value">The double value to convert.</param>
		/// <returns>An <see cref="NSNumber"/> representing the specified value.</returns>
		public static implicit operator NSNumber (double value)
		{
			return FromDouble (value);
		}

		/// <summary>
		/// Implicitly converts a <see cref="bool"/> value to an <see cref="NSNumber"/>.
		/// </summary>
		/// <param name="value">The boolean value to convert.</param>
		/// <returns>An <see cref="NSNumber"/> representing the specified value.</returns>
		public static implicit operator NSNumber (bool value)
		{
			return FromBoolean (value);
		}

		/// <summary>
		/// Implicitly converts an <see cref="sbyte"/> value to an <see cref="NSNumber"/>.
		/// </summary>
		/// <param name="value">The sbyte value to convert.</param>
		/// <returns>An <see cref="NSNumber"/> representing the specified value.</returns>
		public static implicit operator NSNumber (sbyte value)
		{
			return FromSByte (value);
		}

		/// <summary>
		/// Implicitly converts a <see cref="byte"/> value to an <see cref="NSNumber"/>.
		/// </summary>
		/// <param name="value">The byte value to convert.</param>
		/// <returns>An <see cref="NSNumber"/> representing the specified value.</returns>
		public static implicit operator NSNumber (byte value)
		{
			return FromByte (value);
		}

		/// <summary>
		/// Implicitly converts a <see cref="short"/> value to an <see cref="NSNumber"/>.
		/// </summary>
		/// <param name="value">The short value to convert.</param>
		/// <returns>An <see cref="NSNumber"/> representing the specified value.</returns>
		public static implicit operator NSNumber (short value)
		{
			return FromInt16 (value);
		}

		/// <summary>
		/// Implicitly converts a <see cref="ushort"/> value to an <see cref="NSNumber"/>.
		/// </summary>
		/// <param name="value">The ushort value to convert.</param>
		/// <returns>An <see cref="NSNumber"/> representing the specified value.</returns>
		public static implicit operator NSNumber (ushort value)
		{
			return FromUInt16 (value);
		}

		/// <summary>
		/// Implicitly converts an <see cref="int"/> value to an <see cref="NSNumber"/>.
		/// </summary>
		/// <param name="value">The int value to convert.</param>
		/// <returns>An <see cref="NSNumber"/> representing the specified value.</returns>
		public static implicit operator NSNumber (int value)
		{
			return FromInt32 (value);
		}

		/// <summary>
		/// Implicitly converts a <see cref="uint"/> value to an <see cref="NSNumber"/>.
		/// </summary>
		/// <param name="value">The uint value to convert.</param>
		/// <returns>An <see cref="NSNumber"/> representing the specified value.</returns>
		public static implicit operator NSNumber (uint value)
		{
			return FromUInt32 (value);
		}

		/// <summary>
		/// Implicitly converts a <see cref="long"/> value to an <see cref="NSNumber"/>.
		/// </summary>
		/// <param name="value">The long value to convert.</param>
		/// <returns>An <see cref="NSNumber"/> representing the specified value.</returns>
		public static implicit operator NSNumber (long value)
		{
			return FromInt64 (value);
		}

		/// <summary>
		/// Implicitly converts a <see cref="ulong"/> value to an <see cref="NSNumber"/>.
		/// </summary>
		/// <param name="value">The ulong value to convert.</param>
		/// <returns>An <see cref="NSNumber"/> representing the specified value.</returns>
		public static implicit operator NSNumber (ulong value)
		{
			return FromUInt64 (value);
		}

		/// <summary>
		/// Explicitly converts an <see cref="NSNumber"/> to a <see cref="byte"/> value.
		/// </summary>
		/// <param name="source">The <see cref="NSNumber"/> to convert.</param>
		/// <returns>The byte value of the <see cref="NSNumber"/>.</returns>
		/// <exception cref="ArgumentNullException">Thrown when <paramref name="source"/> is <see langword="null"/>.</exception>
		public static explicit operator byte (NSNumber? source)
		{
			ArgumentNullException.ThrowIfNull (source);
			return source.ByteValue;
		}

		/// <summary>
		/// Explicitly converts an <see cref="NSNumber"/> to an <see cref="sbyte"/> value.
		/// </summary>
		/// <param name="source">The <see cref="NSNumber"/> to convert.</param>
		/// <returns>The sbyte value of the <see cref="NSNumber"/>.</returns>
		/// <exception cref="ArgumentNullException">Thrown when <paramref name="source"/> is <see langword="null"/>.</exception>
		public static explicit operator sbyte (NSNumber? source)
		{
			ArgumentNullException.ThrowIfNull (source);
			return source.SByteValue;
		}

		/// <summary>
		/// Explicitly converts an <see cref="NSNumber"/> to a <see cref="short"/> value.
		/// </summary>
		/// <param name="source">The <see cref="NSNumber"/> to convert.</param>
		/// <returns>The short value of the <see cref="NSNumber"/>.</returns>
		/// <exception cref="ArgumentNullException">Thrown when <paramref name="source"/> is <see langword="null"/>.</exception>
		public static explicit operator short (NSNumber? source)
		{
			ArgumentNullException.ThrowIfNull (source);
			return source.Int16Value;
		}

		/// <summary>
		/// Explicitly converts an <see cref="NSNumber"/> to a <see cref="ushort"/> value.
		/// </summary>
		/// <param name="source">The <see cref="NSNumber"/> to convert.</param>
		/// <returns>The ushort value of the <see cref="NSNumber"/>.</returns>
		/// <exception cref="ArgumentNullException">Thrown when <paramref name="source"/> is <see langword="null"/>.</exception>
		public static explicit operator ushort (NSNumber? source)
		{
			ArgumentNullException.ThrowIfNull (source);
			return source.UInt16Value;
		}

		/// <summary>
		/// Explicitly converts an <see cref="NSNumber"/> to an <see cref="int"/> value.
		/// </summary>
		/// <param name="source">The <see cref="NSNumber"/> to convert.</param>
		/// <returns>The int value of the <see cref="NSNumber"/>.</returns>
		/// <exception cref="ArgumentNullException">Thrown when <paramref name="source"/> is <see langword="null"/>.</exception>
		public static explicit operator int (NSNumber? source)
		{
			ArgumentNullException.ThrowIfNull (source);
			return source.Int32Value;
		}

		/// <summary>
		/// Explicitly converts an <see cref="NSNumber"/> to a <see cref="uint"/> value.
		/// </summary>
		/// <param name="source">The <see cref="NSNumber"/> to convert.</param>
		/// <returns>The uint value of the <see cref="NSNumber"/>.</returns>
		/// <exception cref="ArgumentNullException">Thrown when <paramref name="source"/> is <see langword="null"/>.</exception>
		public static explicit operator uint (NSNumber? source)
		{
			ArgumentNullException.ThrowIfNull (source);
			return source.UInt32Value;
		}

		/// <summary>
		/// Explicitly converts an <see cref="NSNumber"/> to a <see cref="long"/> value.
		/// </summary>
		/// <param name="source">The <see cref="NSNumber"/> to convert.</param>
		/// <returns>The long value of the <see cref="NSNumber"/>.</returns>
		/// <exception cref="ArgumentNullException">Thrown when <paramref name="source"/> is <see langword="null"/>.</exception>
		public static explicit operator long (NSNumber? source)
		{
			ArgumentNullException.ThrowIfNull (source);
			return source.Int64Value;
		}

		/// <summary>
		/// Explicitly converts an <see cref="NSNumber"/> to a <see cref="ulong"/> value.
		/// </summary>
		/// <param name="source">The <see cref="NSNumber"/> to convert.</param>
		/// <returns>The ulong value of the <see cref="NSNumber"/>.</returns>
		/// <exception cref="ArgumentNullException">Thrown when <paramref name="source"/> is <see langword="null"/>.</exception>
		public static explicit operator ulong (NSNumber? source)
		{
			ArgumentNullException.ThrowIfNull (source);
			return source.UInt64Value;
		}

		/// <summary>
		/// Explicitly converts an <see cref="NSNumber"/> to a <see cref="float"/> value.
		/// </summary>
		/// <param name="source">The <see cref="NSNumber"/> to convert.</param>
		/// <returns>The float value of the <see cref="NSNumber"/>.</returns>
		/// <exception cref="ArgumentNullException">Thrown when <paramref name="source"/> is <see langword="null"/>.</exception>
		public static explicit operator float (NSNumber? source)
		{
			ArgumentNullException.ThrowIfNull (source);
			return source.FloatValue;
		}

		/// <summary>
		/// Explicitly converts an <see cref="NSNumber"/> to a <see cref="double"/> value.
		/// </summary>
		/// <param name="source">The <see cref="NSNumber"/> to convert.</param>
		/// <returns>The double value of the <see cref="NSNumber"/>.</returns>
		/// <exception cref="ArgumentNullException">Thrown when <paramref name="source"/> is <see langword="null"/>.</exception>
		public static explicit operator double (NSNumber? source)
		{
			ArgumentNullException.ThrowIfNull (source);
			return source.DoubleValue;
		}

		/// <summary>
		/// Explicitly converts an <see cref="NSNumber"/> to a <see cref="bool"/> value.
		/// </summary>
		/// <param name="source">The <see cref="NSNumber"/> to convert.</param>
		/// <returns>The boolean value of the <see cref="NSNumber"/>.</returns>
		/// <exception cref="ArgumentNullException">Thrown when <paramref name="source"/> is <see langword="null"/>.</exception>
		public static explicit operator bool (NSNumber? source)
		{
			ArgumentNullException.ThrowIfNull (source);
			return source.BoolValue;
		}

		/// <summary>
		/// Converts a <see cref="NativeHandle"/> to an <see cref="NSNumber"/> and returns its value as a boolean.
		/// </summary>
		/// <param name="handle">The <see cref="NativeHandle"/> to convert.</param>
		/// <returns>The boolean value of the <see cref="NSNumber"/>.</returns>
		public static bool ToBool (NativeHandle handle)
		{
			using var num = Runtime.GetNSObject<NSNumber> (handle)!;
			return num.BoolValue;
		}

		/// <summary>
		/// Converts a <see cref="NativeHandle"/> to an <see cref="NSNumber"/> and returns its value as a byte.
		/// </summary>
		/// <param name="handle">The <see cref="NativeHandle"/> to convert.</param>
		/// <returns>The byte value of the <see cref="NSNumber"/>.</returns>
		public static byte ToByte (NativeHandle handle)
		{
			using var num = Runtime.GetNSObject<NSNumber> (handle)!;
			return num.ByteValue;
		}

		/// <summary>
		/// Converts a <see cref="NativeHandle"/> to an <see cref="NSNumber"/> and returns its value as an sbyte.
		/// </summary>
		/// <param name="handle">The <see cref="NativeHandle"/> to convert.</param>
		/// <returns>The sbyte value of the <see cref="NSNumber"/>.</returns>
		public static sbyte ToSByte (NativeHandle handle)
		{
			using var num = Runtime.GetNSObject<NSNumber> (handle)!;
			return num.SByteValue;
		}

		/// <summary>
		/// Converts a <see cref="NativeHandle"/> to an <see cref="NSNumber"/> and returns its value as a short.
		/// </summary>
		/// <param name="handle">The <see cref="NativeHandle"/> to convert.</param>
		/// <returns>The short value of the <see cref="NSNumber"/>.</returns>
		public static short ToInt16 (NativeHandle handle)
		{
			using var num = Runtime.GetNSObject<NSNumber> (handle)!;
			return num.Int16Value;
		}

		/// <summary>
		/// Converts a <see cref="NativeHandle"/> to an <see cref="NSNumber"/> and returns its value as a ushort.
		/// </summary>
		/// <param name="handle">The <see cref="NativeHandle"/> to convert.</param>
		/// <returns>The ushort value of the <see cref="NSNumber"/>.</returns>
		public static ushort ToUInt16 (NativeHandle handle)
		{
			using var num = Runtime.GetNSObject<NSNumber> (handle)!;
			return num.UInt16Value;
		}

		/// <summary>
		/// Converts a <see cref="NativeHandle"/> to an <see cref="NSNumber"/> and returns its value as an int.
		/// </summary>
		/// <param name="handle">The <see cref="NativeHandle"/> to convert.</param>
		/// <returns>The int value of the <see cref="NSNumber"/>.</returns>
		public static int ToInt32 (NativeHandle handle)
		{
			using var num = Runtime.GetNSObject<NSNumber> (handle)!;
			return num.Int32Value;
		}

		/// <summary>
		/// Converts a <see cref="NativeHandle"/> to an <see cref="NSNumber"/> and returns its value as a uint.
		/// </summary>
		/// <param name="handle">The <see cref="NativeHandle"/> to convert.</param>
		/// <returns>The uint value of the <see cref="NSNumber"/>.</returns>
		public static uint ToUInt32 (NativeHandle handle)
		{
			using var num = Runtime.GetNSObject<NSNumber> (handle)!;
			return num.UInt32Value;
		}

		/// <summary>
		/// Converts a <see cref="NativeHandle"/> to an <see cref="NSNumber"/> and returns its value as a long.
		/// </summary>
		/// <param name="handle">The <see cref="NativeHandle"/> to convert.</param>
		/// <returns>The long value of the <see cref="NSNumber"/>.</returns>
		public static long ToInt64 (NativeHandle handle)
		{
			using var num = Runtime.GetNSObject<NSNumber> (handle)!;
			return num.Int64Value;
		}

		/// <summary>
		/// Converts a <see cref="NativeHandle"/> to an <see cref="NSNumber"/> and returns its value as a ulong.
		/// </summary>
		/// <param name="handle">The <see cref="NativeHandle"/> to convert.</param>
		/// <returns>The ulong value of the <see cref="NSNumber"/>.</returns>
		public static ulong ToUInt64 (NativeHandle handle)
		{
			using var num = Runtime.GetNSObject<NSNumber> (handle)!;
			return num.UInt64Value;
		}

		/// <summary>
		/// Converts a <see cref="NativeHandle"/> to an <see cref="NSNumber"/> and returns its value as a float.
		/// </summary>
		/// <param name="handle">The <see cref="NativeHandle"/> to convert.</param>
		/// <returns>The float value of the <see cref="NSNumber"/>.</returns>
		public static float ToFloat (NativeHandle handle)
		{
			using var num = Runtime.GetNSObject<NSNumber> (handle)!;
			return num.FloatValue;
		}

		/// <summary>
		/// Converts a <see cref="NativeHandle"/> to an <see cref="NSNumber"/> and returns its value as an <see cref="nfloat"/>.
		/// </summary>
		/// <param name="handle">The <see cref="NativeHandle"/> to convert.</param>
		/// <returns>The nfloat value of the <see cref="NSNumber"/>.</returns>
		public static nfloat ToNFloat (NativeHandle handle)
		{
			using var num = Runtime.GetNSObject<NSNumber> (handle)!;
			return num.NFloatValue;
		}

		/// <summary>
		/// Converts a <see cref="NativeHandle"/> to an <see cref="NSNumber"/> and returns its value as a double.
		/// </summary>
		/// <param name="handle">The <see cref="NativeHandle"/> to convert.</param>
		/// <returns>The double value of the <see cref="NSNumber"/>.</returns>
		public static double ToDouble (NativeHandle handle)
		{
			using var num = Runtime.GetNSObject<NSNumber> (handle)!;
			return num.DoubleValue;
		}

		/// <summary>
		/// Converts a <see cref="NativeHandle"/> to an <see cref="NSNumber"/> and returns its value as an <see cref="nint"/>.
		/// </summary>
		/// <param name="handle">The <see cref="NativeHandle"/> to convert.</param>
		/// <returns>The nint value of the <see cref="NSNumber"/>.</returns>
		public static nint ToNInt (NativeHandle handle)
		{
			using var num = Runtime.GetNSObject<NSNumber> (handle)!;
			return num.NIntValue;
		}

		/// <summary>
		/// Converts a <see cref="NativeHandle"/> to an <see cref="NSNumber"/> and returns its value as a <see cref="nuint"/>.
		/// </summary>
		/// <param name="handle">The <see cref="NativeHandle"/> to convert.</param>
		/// <returns>The nuint value of the <see cref="NSNumber"/>.</returns>
		public static nuint ToNUInt (NativeHandle handle)
		{
			using var num = Runtime.GetNSObject<NSNumber> (handle)!;
			return num.NUIntValue;
		}

		/// <summary>
		/// Initializes a new instance of the <see cref="NSNumber"/> class with the specified <see cref="nfloat"/> value.
		/// </summary>
		/// <param name="value">The nfloat value to wrap.</param>
		public NSNumber (nfloat value) :
			this ((double) value)
		{
		}

		/// <summary>
		/// Gets the value of the <see cref="NSNumber"/> as an <see cref="nfloat"/>.
		/// </summary>
		/// <value>The nfloat value of the <see cref="NSNumber"/>.</value>
		public nfloat NFloatValue {
			get {
				return (nfloat) DoubleValue;
			}
		}

		/// <summary>
		/// Creates an <see cref="NSNumber"/> from an <see cref="nfloat"/> value.
		/// </summary>
		/// <param name="value">The nfloat value to convert.</param>
		/// <returns>An <see cref="NSNumber"/> representing the specified value.</returns>
		public static NSNumber FromNFloat (nfloat value)
		{
			return (FromDouble ((double) value));
		}

		/// <summary>
		/// Returns a string representation of the value of the current instance.
		/// </summary>
		/// <returns>A string representation of the <see cref="NSNumber"/> value.</returns>
		public override string ToString ()
		{
			return StringValue;
		}

		/// <summary>
		/// Compares the current instance with another object of the same type and returns an integer that indicates whether the current instance precedes, follows, or occurs in the same position in the sort order as the other object.
		/// </summary>
		/// <param name="obj">An object to compare with this instance.</param>
		/// <returns>A value that indicates the relative order of the objects being compared.</returns>
		public int CompareTo (object? obj)
		{
			return CompareTo (obj as NSNumber);
		}

		/// <summary>
		/// Compares the current instance with another <see cref="NSNumber"/> and returns an integer that indicates whether the current instance precedes, follows, or occurs in the same position in the sort order as the other <see cref="NSNumber"/>.
		/// </summary>
		/// <param name="other">An <see cref="NSNumber"/> to compare with this instance.</param>
		/// <returns>A value that indicates the relative order of the objects being compared.</returns>
		/// <exception cref="ArgumentException">Thrown when <paramref name="other"/> is <see langword="null"/>.</exception>
		public int CompareTo (NSNumber? other)
		{
			// value must not be `nil` to call the `compare:` selector
			// that match well with the not same type of .NET check
			if (other is null)
				ArgumentNullException.ThrowIfNull (other);
			return (int) Compare (other);
		}

		/// <summary>
		/// Determines whether the specified object is equal to the current <see cref="NSNumber"/>.
		/// </summary>
		/// <param name="other">The object to compare with the current <see cref="NSNumber"/>.</param>
		/// <returns><see langword="true"/> if the specified object is equal to the current <see cref="NSNumber"/>; otherwise, <see langword="false"/>.</returns>
		public override bool Equals (object? other)
		{
			return Equals (other as NSNumber);
		}

		/// <summary>
		/// Determines whether the specified <see cref="NSNumber"/> is equal to the current <see cref="NSNumber"/>.
		/// </summary>
		/// <param name="other">The <see cref="NSNumber"/> to compare with the current <see cref="NSNumber"/>.</param>
		/// <returns><see langword="true"/> if the specified <see cref="NSNumber"/> is equal to the current <see cref="NSNumber"/>; otherwise, <see langword="false"/>.</returns>
		public bool Equals (NSNumber? other)
		{
			if (other is null)
				return false;
			bool result = IsEqualTo (other.Handle);
			GC.KeepAlive (other);
			return result;
		}

		/// <summary>
		/// Generates a hash code for the current instance.
		/// </summary>
		/// <returns>An int containing the hash code for this instance.</returns>
		public override int GetHashCode ()
		{
			// this is heavy weight :( but it's the only way to follow .NET rule where:
			// "If two objects compare as equal, the GetHashCode method for each object must return the same value."
			// otherwise NSNumber (1) needs to be != from NSNumber (1d), a breaking change from classic and 
			// something that's really not obvious
			return StringValue.GetHashCode ();
		}

		/// <summary>
		/// Determines whether the current <see cref="NSNumber"/> is equal to the specified <see cref="NSNumber"/>.
		/// </summary>
		/// <param name="number">The <see cref="NSNumber"/> to compare with the current <see cref="NSNumber"/>.</param>
		/// <returns><see langword="true"/> if the specified <see cref="NSNumber"/> is equal to the current <see cref="NSNumber"/>; otherwise, <see langword="false"/>.</returns>
		public bool IsEqualTo (NSNumber? number)
		{
			var result = IsEqualTo (number.GetHandle ());
			GC.KeepAlive (number);
			return result;
		}
#endif
	}
}
