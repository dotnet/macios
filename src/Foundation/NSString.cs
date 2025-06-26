//
// Copyright 2010, Novell, Inc.
// Copyright 2011 - 2014 Xamarin Inc
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
using System;
using System.Reflection;
using System.Collections;
using System.ComponentModel;
using System.Diagnostics.CodeAnalysis;
using System.Runtime.InteropServices;

#if !COREBUILD
using CoreFoundation;
using CoreGraphics;
#endif
using ObjCRuntime;

#nullable enable

namespace Foundation {

#if COREBUILD
	[Protocol]
	public interface INSCopying { }
	[Protocol]
	public interface INSCoding { }
	[Protocol]
	public interface INSSecureCoding { }
#endif

	public partial class NSString : NSObject
#if COREBUILD
		, INSCopying, INSSecureCoding
#endif
	 {
#if !COREBUILD
		/// <summary>An <see cref="NSString" /> instance for an empty (zero-length) string.</summary>
		public static readonly NSString Empty = new NSString (String.Empty);

		internal NSString (NativeHandle handle, bool owns) : base (handle, owns)
		{
		}

		/// <summary>Creates an Objective-C NSString from the C# string and returns a pointer to it.</summary>
		/// <param name="str">C# String to wrap</param>
		/// <returns>Pointer to the <see cref="NSString" /> object, must be released with <see cref="ReleaseNative" />.</returns>
		/// <remarks>
		///   <para>
		///     This method creates an Objective-C NSString and returns an
		///     <see cref="NativeHandle" /> that points to it.  This does not create the managed
		///     NSString object that points to it, which is ideal for
		///     transient strings that must be passed to Objective-C as it is
		///     not necessary for the garbage collector to track this object.
		///   </para>
		///   <para>
		///     The memory associated with this object should be released
		///     by calling the <see cref="ReleaseNative" />
		///     method.
		///   </para>
		///   <example>
		///     <code lang="csharp lang-csharp"><![CDATA[
		/// IntPtr objcString = NSString.CreateNative ("Hello");
		/// // You can pass objcString to any methods that expect an Objective-C NSString pointer
		/// NSString.ReleaseNative (objcString);
		/// ]]></code></example>
		///   </remarks>
		public static NativeHandle CreateNative (string? str)
		{
			return CreateNative (str, false);
		}

		/// <inheritdoc cref="CreateNative(string)" />
		/// <param name="str">C# String to wrap</param>
		/// <param name="autorelease">Whether the return value is autoreleased (in which case <see cref="ReleaseNative" /> must not be called on the return value).</param>
		public static NativeHandle CreateNative (string? str, bool autorelease)
		{
			return CFString.CreateNative (str, autorelease);
		}

		/// <inheritdoc cref="CreateNative(string)" />
		/// <param name="value">C# String to wrap</param>
		/// <param name="start">The offset of the managed string to create the native string from.</param>
		/// <param name="length">The length of the managed string to create the native string from.</param>
		public static NativeHandle CreateNative (string? value, int start, int length)
		{
			return CFString.CreateNative (value, start, length, false);
		}

		/// <inheritdoc cref="CreateNative(string)" />
		/// <param name="value">C# String to wrap</param>
		/// <param name="start">The offset of the managed string to create the native string from.</param>
		/// <param name="length">The length of the managed string to create the native string from.</param>
		/// <param name="autorelease">Whether the return value is autoreleased (in which case <see cref="ReleaseNative" /> must not be called on the return value).</param>
		public static NativeHandle CreateNative (string? value, int start, int length, bool autorelease)
		{
			return CFString.CreateNative (value, start, length, autorelease);
		}

		/// <param name="handle">Handle to the Objective-C native NSString object.</param>
		/// <summary>Releases a native Objective-C string.</summary>
		/// <remarks>Use this method to release Objective-C NSString handles that were previously allocated with <see cref="CreateNative(System.String)" />.</remarks>
		public static void ReleaseNative (NativeHandle handle)
		{
			NSObject.DangerousRelease (handle);
		}

		/// <summary>Creates an <see cref="NSString" /> from a C# string.</summary>
		/// <param name="str">A C# string to create an <see cref="NSString" /> from.</param>
		public NSString (string str)
			: base (CFString.CreateNative (ThrowHelper.ThrowIfNull (str)))
		{
		}

		/// <summary>Creates an <see cref="NSString" /> from a C# string.</summary>
		/// <param name="value">A C# string to create an <see cref="NSString" /> from.</param>
		/// <param name="start">The starting index of the <paramref name="value" /> string to create the <see cref="NSString" /> from.</param>
		/// <param name="length">The length, starting at <paramref name="start" />, of the <paramref name="value" /> string to create the <see cref="NSString" /> from.</param>
		public NSString (string value, int start, int length)
			: base (CFString.CreateNative (ThrowHelper.ThrowIfNull (value), start, length, false))
		{
		}

		/// <summary>Returns a string representation of the value of the current instance.</summary>
		public override string ToString ()
		{
			return FromHandle (Handle)!;
		}

		/// <summary>Converts the <see cref="NSString" /> to a managed string.</summary>
		/// <param name="str">The NSString to convert.</param>
		/// <returns>A managed string.</returns>
		[return: NotNullIfNotNull (nameof (str))]
		public static implicit operator string? (NSString? str)
		{
			return str?.ToString ();
		}

		/// <summary>Converts the managed string to an <see cref="NSString" />.</summary>
		/// <param name="str">The managed string to convert.</param>
		/// <returns>A <see cref="NSString" /> instance.</returns>
		[return: NotNullIfNotNull (nameof (str))]
		public static explicit operator NSString? (string? str)
		{
			if (str is null)
				return null;
			return new NSString (str);
		}

		/// <summary>Utility method that returns a string from a pointer that points to an Objective-C NSString or CFString object.</summary>
		/// <param name="usrhandle">Pointer to an Objective-C NSString or CFString object (not the managed NSString object).</param>
		/// <returns>The Objective-C string in the NSString as a C# string.</returns>
		public static string? FromHandle (NativeHandle usrhandle)
		{
			return FromHandle (usrhandle, false);
		}

		/// <summary>Utility method that returns a string from a pointer that points to an Objective-C NSString or CFString object.</summary>
		/// <param name="handle">Pointer to an Objective-C NSString or CFString object (not the managed NSString object).</param>
		/// <param name="owns">Whether the <paramref name="handle" /> should be released or not.</param>
		/// <returns>The Objective-C string in the NSString as a C# string.</returns>
		public static string? FromHandle (NativeHandle handle, bool owns)
		{
			return CFString.FromHandle (handle, owns);
		}

		/// <summary>Compare two <see cref="NSString" /> instances.</summary>
		/// <param name="a">The first <see cref="NSString" /> to compare.</param>
		/// <param name="b">The second <see cref="NSString" /> to compare.</param>
		/// <returns><see langword="true" /> if equal, <see langword="false" /> otherwise.</returns>
		public static bool Equals (NSString? a, NSString? b)
		{
			if ((a as object) == (b as object))
				return true;

			if (a is null || b is null)
				return false;

			if (a.Handle == b.Handle)
				return true;
			bool result = a.IsEqualTo (b.Handle);
			GC.KeepAlive (b);
			return result;
		}

		/// <summary>Compare two <see cref="NSString" /> instances.</summary>
		/// <param name="a">The first <see cref="NSString" /> to compare.</param>
		/// <param name="b">The second <see cref="NSString" /> to compare.</param>
		/// <returns><see langword="true" /> if equal, <see langword="false" /> otherwise.</returns>
		public static bool operator == (NSString? a, NSString? b)
		{
			return Equals (a, b);
		}

		/// <summary>Compare two <see cref="NSString" /> instances.</summary>
		/// <param name="a">The first <see cref="NSString" /> to compare.</param>
		/// <param name="b">The second <see cref="NSString" /> to compare.</param>
		/// <returns><see langword="true" /> if not equal, <see langword="false" /> otherwise.</returns>
		public static bool operator != (NSString? a, NSString? b)
		{
			return !Equals (a, b);
		}

		/// <summary>Compare this <see cref="NSString" /> to another object.</summary>
		/// <param name="obj">The other object to compare against.</param>
		/// <returns><see langword="true" /> if equal, <see langword="false" /> otherwise.</returns>
		public override bool Equals (object? obj)
		{
			return Equals (this, obj as NSString);
		}

		[DllImport ("__Internal")]
		extern static IntPtr xamarin_localized_string_format (IntPtr fmt);
		[DllImport ("__Internal")]
		extern static IntPtr xamarin_localized_string_format_1 (IntPtr fmt, IntPtr arg1);
		[DllImport ("__Internal")]
		extern static IntPtr xamarin_localized_string_format_2 (IntPtr fmt, IntPtr arg1, IntPtr arg2);
		[DllImport ("__Internal")]
		extern static IntPtr xamarin_localized_string_format_3 (IntPtr fmt, IntPtr arg1, IntPtr arg2, IntPtr arg3);
		[DllImport ("__Internal")]
		extern static IntPtr xamarin_localized_string_format_4 (IntPtr fmt, IntPtr arg1, IntPtr arg2, IntPtr arg3, IntPtr arg4);
		[DllImport ("__Internal")]
		extern static IntPtr xamarin_localized_string_format_5 (IntPtr fmt, IntPtr arg1, IntPtr arg2, IntPtr arg3, IntPtr arg4, IntPtr arg5);
		[DllImport ("__Internal")]
		extern static IntPtr xamarin_localized_string_format_6 (IntPtr fmt, IntPtr arg1, IntPtr arg2, IntPtr arg3, IntPtr arg4, IntPtr arg5, IntPtr arg6);
		[DllImport ("__Internal")]
		extern static IntPtr xamarin_localized_string_format_7 (IntPtr fmt, IntPtr arg1, IntPtr arg2, IntPtr arg3, IntPtr arg4, IntPtr arg5, IntPtr arg6, IntPtr arg7);
		[DllImport ("__Internal")]
		extern static IntPtr xamarin_localized_string_format_8 (IntPtr fmt, IntPtr arg1, IntPtr arg2, IntPtr arg3, IntPtr arg4, IntPtr arg5, IntPtr arg6, IntPtr arg7, IntPtr arg8);
		[DllImport ("__Internal")]
		extern static IntPtr xamarin_localized_string_format_9 (IntPtr fmt, IntPtr arg1, IntPtr arg2, IntPtr arg3, IntPtr arg4, IntPtr arg5, IntPtr arg6, IntPtr arg7, IntPtr arg8, IntPtr arg9);

		/// <summary>Format a localizable string with the specified arguments.</summary>
		/// <param name="format">The localizable string to format.</param>
		/// <param name="args">The arguments to apply to the localizable string.</param>
		/// <returns>A localized string with the specified format arguments applied.</returns>
		public static NSString LocalizedFormat (string format, params object [] args)
		{
			using (var ns = new NSString (format))
				return LocalizedFormat (ns, args);
		}

		/// <summary>Format a localizable string with the specified arguments.</summary>
		/// <param name="format">The localizable string to format.</param>
		/// <param name="args">The arguments to apply to the localizable string.</param>
		/// <returns>A localized string with the specified format arguments applied.</returns>
		public static NSString LocalizedFormat (NSString format, params object [] args)
		{
			int argc = args.Length;
			var nso = new NSObject [argc];
			for (int i = 0; i < argc; i++)
				nso [i] = NSObject.FromObject (args [i]);

			return LocalizedFormat (format, nso);
		}

		/// <summary>Format a localizable string with the specified arguments.</summary>
		/// <param name="format">The localizable string to format.</param>
		/// <param name="args">The arguments to apply to the localizable string.</param>
		/// <returns>A localized string with the specified format arguments applied.</returns>
		public static NSString LocalizedFormat (NSString format, NSObject [] args)
		{
			NSString result;
			NativeHandle formatHandle = format.Handle;
			switch (args.Length) {
			case 0:
				result = new NSString (xamarin_localized_string_format (formatHandle));
				break;
			case 1:
				result = new NSString (xamarin_localized_string_format_1 (formatHandle, args [0].Handle));
				break;
			case 2:
				result = new NSString (xamarin_localized_string_format_2 (formatHandle, args [0].Handle, args [1].Handle));
				break;
			case 3:
				result = new NSString (xamarin_localized_string_format_3 (formatHandle, args [0].Handle, args [1].Handle, args [2].Handle));
				break;
			case 4:
				result = new NSString (xamarin_localized_string_format_4 (formatHandle, args [0].Handle, args [1].Handle, args [2].Handle, args [3].Handle));
				break;
			case 5:
				result = new NSString (xamarin_localized_string_format_5 (formatHandle, args [0].Handle, args [1].Handle, args [2].Handle, args [3].Handle, args [4].Handle));
				break;
			case 6:
				result = new NSString (xamarin_localized_string_format_6 (formatHandle, args [0].Handle, args [1].Handle, args [2].Handle, args [3].Handle, args [4].Handle, args [5].Handle));
				break;
			case 7:
				result = new NSString (xamarin_localized_string_format_7 (formatHandle, args [0].Handle, args [1].Handle, args [2].Handle, args [3].Handle, args [4].Handle, args [5].Handle, args [6].Handle));
				break;
			case 8:
				result = new NSString (xamarin_localized_string_format_8 (formatHandle, args [0].Handle, args [1].Handle, args [2].Handle, args [3].Handle, args [4].Handle, args [5].Handle, args [6].Handle, args [7].Handle));
				break;
			case 9:
				result = new NSString (xamarin_localized_string_format_9 (formatHandle, args [0].Handle, args [1].Handle, args [2].Handle, args [3].Handle, args [4].Handle, args [5].Handle, args [6].Handle, args [7].Handle, args [8].Handle));
				break;
			default:
				throw new Exception ("Unsupported number of arguments, maximum number is 9");
			}
			GC.KeepAlive (format);
			GC.KeepAlive (args);
			return result;
		}

		/// <summary>Transliterate this string using the specified string transformation.</summary>
		/// <param name="transform">The string transformation to use.</param>
		/// <param name="reverse">Whether to apply the transformation in reverse or not.</param>
		/// <returns>The transliterated string.</returns>
		public NSString? TransliterateString (NSStringTransform transform, bool reverse)
		{
			return TransliterateString (transform.GetConstant ()!, reverse);
		}

		/// <summary>Generates a hash code for the current instance.</summary>
		/// <returns>A int containing the hash code for this instance.</returns>
		/// <remarks>The algorithm used to generate the hash code is unspecified.</remarks>
		public override int GetHashCode ()
		{
			return base.GetHashCode ();
		}
#endif // !COREBUILD
	}
}
