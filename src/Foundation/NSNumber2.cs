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
		/// Reads the boolean value of a native <see cref="NSNumber"/> without creating a managed wrapper.
		/// </summary>
		/// <param name="handle">The <see cref="NativeHandle"/> to convert.</param>
		/// <returns>The boolean value of the <see cref="NSNumber"/>, or <see langword="false"/> if <paramref name="handle"/> is zero.</returns>
		public static bool ToBool (NativeHandle handle)
		{
			if (handle == NativeHandle.Zero)
				return default (bool);
			return Messaging.bool_objc_msgSend (handle, Selector.GetHandle ("boolValue")) != 0;
		}

		/// <summary>
		/// Reads the byte value of a native <see cref="NSNumber"/> without creating a managed wrapper.
		/// </summary>
		/// <param name="handle">The <see cref="NativeHandle"/> to convert.</param>
		/// <returns>The byte value of the <see cref="NSNumber"/>, or zero if <paramref name="handle"/> is zero.</returns>
		public static byte ToByte (NativeHandle handle)
		{
			if (handle == NativeHandle.Zero)
				return default (byte);
			return Messaging.byte_objc_msgSend (handle, Selector.GetHandle ("unsignedCharValue"));
		}

		/// <summary>
		/// Reads the sbyte value of a native <see cref="NSNumber"/> without creating a managed wrapper.
		/// </summary>
		/// <param name="handle">The <see cref="NativeHandle"/> to convert.</param>
		/// <returns>The sbyte value of the <see cref="NSNumber"/>, or zero if <paramref name="handle"/> is zero.</returns>
		public static sbyte ToSByte (NativeHandle handle)
		{
			if (handle == NativeHandle.Zero)
				return default (sbyte);
			return Messaging.SByte_objc_msgSend (handle, Selector.GetHandle ("charValue"));
		}

		/// <summary>
		/// Reads the short value of a native <see cref="NSNumber"/> without creating a managed wrapper.
		/// </summary>
		/// <param name="handle">The <see cref="NativeHandle"/> to convert.</param>
		/// <returns>The short value of the <see cref="NSNumber"/>, or zero if <paramref name="handle"/> is zero.</returns>
		public static short ToInt16 (NativeHandle handle)
		{
			if (handle == NativeHandle.Zero)
				return default (short);
			return Messaging.short_objc_msgSend (handle, Selector.GetHandle ("shortValue"));
		}

		/// <summary>
		/// Reads the ushort value of a native <see cref="NSNumber"/> without creating a managed wrapper.
		/// </summary>
		/// <param name="handle">The <see cref="NativeHandle"/> to convert.</param>
		/// <returns>The ushort value of the <see cref="NSNumber"/>, or zero if <paramref name="handle"/> is zero.</returns>
		public static ushort ToUInt16 (NativeHandle handle)
		{
			if (handle == NativeHandle.Zero)
				return default (ushort);
			return Messaging.UInt16_objc_msgSend (handle, Selector.GetHandle ("unsignedShortValue"));
		}

		/// <summary>
		/// Reads the int value of a native <see cref="NSNumber"/> without creating a managed wrapper.
		/// </summary>
		/// <param name="handle">The <see cref="NativeHandle"/> to convert.</param>
		/// <returns>The int value of the <see cref="NSNumber"/>, or zero if <paramref name="handle"/> is zero.</returns>
		public static int ToInt32 (NativeHandle handle)
		{
			if (handle == NativeHandle.Zero)
				return default (int);
			return Messaging.int_objc_msgSend (handle, Selector.GetHandle ("intValue"));
		}

		/// <summary>
		/// Reads the uint value of a native <see cref="NSNumber"/> without creating a managed wrapper.
		/// </summary>
		/// <param name="handle">The <see cref="NativeHandle"/> to convert.</param>
		/// <returns>The uint value of the <see cref="NSNumber"/>, or zero if <paramref name="handle"/> is zero.</returns>
		public static uint ToUInt32 (NativeHandle handle)
		{
			if (handle == NativeHandle.Zero)
				return default (uint);
			return Messaging.UInt32_objc_msgSend (handle, Selector.GetHandle ("unsignedIntValue"));
		}

		/// <summary>
		/// Reads the long value of a native <see cref="NSNumber"/> without creating a managed wrapper.
		/// </summary>
		/// <param name="handle">The <see cref="NativeHandle"/> to convert.</param>
		/// <returns>The long value of the <see cref="NSNumber"/>, or zero if <paramref name="handle"/> is zero.</returns>
		public static long ToInt64 (NativeHandle handle)
		{
			if (handle == NativeHandle.Zero)
				return default (long);
			return Messaging.Int64_objc_msgSend (handle, Selector.GetHandle ("longLongValue"));
		}

		/// <summary>
		/// Reads the ulong value of a native <see cref="NSNumber"/> without creating a managed wrapper.
		/// </summary>
		/// <param name="handle">The <see cref="NativeHandle"/> to convert.</param>
		/// <returns>The ulong value of the <see cref="NSNumber"/>, or zero if <paramref name="handle"/> is zero.</returns>
		public static ulong ToUInt64 (NativeHandle handle)
		{
			if (handle == NativeHandle.Zero)
				return default (ulong);
			return Messaging.UInt64_objc_msgSend (handle, Selector.GetHandle ("unsignedLongLongValue"));
		}

		/// <summary>
		/// Reads the float value of a native <see cref="NSNumber"/> without creating a managed wrapper.
		/// </summary>
		/// <param name="handle">The <see cref="NativeHandle"/> to convert.</param>
		/// <returns>The float value of the <see cref="NSNumber"/>, or zero if <paramref name="handle"/> is zero.</returns>
		public static float ToFloat (NativeHandle handle)
		{
			if (handle == NativeHandle.Zero)
				return default (float);
			return Messaging.float_objc_msgSend (handle, Selector.GetHandle ("floatValue"));
		}

		/// <summary>
		/// Reads the <see cref="nfloat"/> value of a native <see cref="NSNumber"/> without creating a managed wrapper.
		/// </summary>
		/// <param name="handle">The <see cref="NativeHandle"/> to convert.</param>
		/// <returns>The nfloat value of the <see cref="NSNumber"/>, or zero if <paramref name="handle"/> is zero.</returns>
		public static nfloat ToNFloat (NativeHandle handle)
		{
			return (nfloat) ToDouble (handle);
		}

		/// <summary>
		/// Reads the double value of a native <see cref="NSNumber"/> without creating a managed wrapper.
		/// </summary>
		/// <param name="handle">The <see cref="NativeHandle"/> to convert.</param>
		/// <returns>The double value of the <see cref="NSNumber"/>, or zero if <paramref name="handle"/> is zero.</returns>
		public static double ToDouble (NativeHandle handle)
		{
			if (handle == NativeHandle.Zero)
				return default (double);
			return Messaging.Double_objc_msgSend (handle, Selector.GetHandle ("doubleValue"));
		}

		/// <summary>
		/// Reads the <see cref="nint"/> value of a native <see cref="NSNumber"/> without creating a managed wrapper.
		/// </summary>
		/// <param name="handle">The <see cref="NativeHandle"/> to convert.</param>
		/// <returns>The nint value of the <see cref="NSNumber"/>, or zero if <paramref name="handle"/> is zero.</returns>
		public static nint ToNInt (NativeHandle handle)
		{
			if (handle == NativeHandle.Zero)
				return default (nint);
			return Messaging.IntPtr_objc_msgSend (handle, Selector.GetHandle ("integerValue"));
		}

		/// <summary>
		/// Reads the <see cref="nuint"/> value of a native <see cref="NSNumber"/> without creating a managed wrapper.
		/// </summary>
		/// <param name="handle">The <see cref="NativeHandle"/> to convert.</param>
		/// <returns>The nuint value of the <see cref="NSNumber"/>, or zero if <paramref name="handle"/> is zero.</returns>
		public static nuint ToNUInt (NativeHandle handle)
		{
			if (handle == NativeHandle.Zero)
				return default (nuint);
			return (nuint) Messaging.UIntPtr_objc_msgSend (handle, Selector.GetHandle ("unsignedIntegerValue"));
		}

		/// <summary>Reads the nullable boolean value of a native <see cref="NSNumber"/> without creating a managed wrapper.</summary>
		/// <param name="handle">The native number handle.</param>
		/// <returns>The boolean value, or <see langword="null"/> if <paramref name="handle"/> is zero.</returns>
		public static bool? ToNullableBool (NativeHandle handle)
		{
			return handle == NativeHandle.Zero ? null : ToBool (handle);
		}

		/// <summary>Reads the nullable byte value of a native <see cref="NSNumber"/> without creating a managed wrapper.</summary>
		/// <param name="handle">The native number handle.</param>
		/// <returns>The byte value, or <see langword="null"/> if <paramref name="handle"/> is zero.</returns>
		public static byte? ToNullableByte (NativeHandle handle)
		{
			return handle == NativeHandle.Zero ? null : ToByte (handle);
		}

		/// <summary>Reads the nullable sbyte value of a native <see cref="NSNumber"/> without creating a managed wrapper.</summary>
		/// <param name="handle">The native number handle.</param>
		/// <returns>The sbyte value, or <see langword="null"/> if <paramref name="handle"/> is zero.</returns>
		public static sbyte? ToNullableSByte (NativeHandle handle)
		{
			return handle == NativeHandle.Zero ? null : ToSByte (handle);
		}

		/// <summary>Reads the nullable short value of a native <see cref="NSNumber"/> without creating a managed wrapper.</summary>
		/// <param name="handle">The native number handle.</param>
		/// <returns>The short value, or <see langword="null"/> if <paramref name="handle"/> is zero.</returns>
		public static short? ToNullableInt16 (NativeHandle handle)
		{
			return handle == NativeHandle.Zero ? null : ToInt16 (handle);
		}

		/// <summary>Reads the nullable ushort value of a native <see cref="NSNumber"/> without creating a managed wrapper.</summary>
		/// <param name="handle">The native number handle.</param>
		/// <returns>The ushort value, or <see langword="null"/> if <paramref name="handle"/> is zero.</returns>
		public static ushort? ToNullableUInt16 (NativeHandle handle)
		{
			return handle == NativeHandle.Zero ? null : ToUInt16 (handle);
		}

		/// <summary>Reads the nullable int value of a native <see cref="NSNumber"/> without creating a managed wrapper.</summary>
		/// <param name="handle">The native number handle.</param>
		/// <returns>The int value, or <see langword="null"/> if <paramref name="handle"/> is zero.</returns>
		public static int? ToNullableInt32 (NativeHandle handle)
		{
			return handle == NativeHandle.Zero ? null : ToInt32 (handle);
		}

		/// <summary>Reads the nullable uint value of a native <see cref="NSNumber"/> without creating a managed wrapper.</summary>
		/// <param name="handle">The native number handle.</param>
		/// <returns>The uint value, or <see langword="null"/> if <paramref name="handle"/> is zero.</returns>
		public static uint? ToNullableUInt32 (NativeHandle handle)
		{
			return handle == NativeHandle.Zero ? null : ToUInt32 (handle);
		}

		/// <summary>Reads the nullable long value of a native <see cref="NSNumber"/> without creating a managed wrapper.</summary>
		/// <param name="handle">The native number handle.</param>
		/// <returns>The long value, or <see langword="null"/> if <paramref name="handle"/> is zero.</returns>
		public static long? ToNullableInt64 (NativeHandle handle)
		{
			return handle == NativeHandle.Zero ? null : ToInt64 (handle);
		}

		/// <summary>Reads the nullable ulong value of a native <see cref="NSNumber"/> without creating a managed wrapper.</summary>
		/// <param name="handle">The native number handle.</param>
		/// <returns>The ulong value, or <see langword="null"/> if <paramref name="handle"/> is zero.</returns>
		public static ulong? ToNullableUInt64 (NativeHandle handle)
		{
			return handle == NativeHandle.Zero ? null : ToUInt64 (handle);
		}

		/// <summary>Reads the nullable float value of a native <see cref="NSNumber"/> without creating a managed wrapper.</summary>
		/// <param name="handle">The native number handle.</param>
		/// <returns>The float value, or <see langword="null"/> if <paramref name="handle"/> is zero.</returns>
		public static float? ToNullableFloat (NativeHandle handle)
		{
			return handle == NativeHandle.Zero ? null : ToFloat (handle);
		}

		/// <summary>Reads the nullable <see cref="nfloat"/> value of a native <see cref="NSNumber"/> without creating a managed wrapper.</summary>
		/// <param name="handle">The native number handle.</param>
		/// <returns>The nfloat value, or <see langword="null"/> if <paramref name="handle"/> is zero.</returns>
		public static nfloat? ToNullableNFloat (NativeHandle handle)
		{
			return handle == NativeHandle.Zero ? null : ToNFloat (handle);
		}

		/// <summary>Reads the nullable double value of a native <see cref="NSNumber"/> without creating a managed wrapper.</summary>
		/// <param name="handle">The native number handle.</param>
		/// <returns>The double value, or <see langword="null"/> if <paramref name="handle"/> is zero.</returns>
		public static double? ToNullableDouble (NativeHandle handle)
		{
			return handle == NativeHandle.Zero ? null : ToDouble (handle);
		}

		/// <summary>Reads the nullable <see cref="nint"/> value of a native <see cref="NSNumber"/> without creating a managed wrapper.</summary>
		/// <param name="handle">The native number handle.</param>
		/// <returns>The nint value, or <see langword="null"/> if <paramref name="handle"/> is zero.</returns>
		public static nint? ToNullableNInt (NativeHandle handle)
		{
			return handle == NativeHandle.Zero ? null : ToNInt (handle);
		}

		/// <summary>Reads the nullable <see cref="nuint"/> value of a native <see cref="NSNumber"/> without creating a managed wrapper.</summary>
		/// <param name="handle">The native number handle.</param>
		/// <returns>The nuint value, or <see langword="null"/> if <paramref name="handle"/> is zero.</returns>
		public static nuint? ToNullableNUInt (NativeHandle handle)
		{
			return handle == NativeHandle.Zero ? null : ToNUInt (handle);
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
