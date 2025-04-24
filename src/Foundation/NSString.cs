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
using System.Runtime.InteropServices;

#if !COREBUILD
using CoreFoundation;
using CoreGraphics;
#endif
using ObjCRuntime;

#if !NET
using NativeHandle = System.IntPtr;
#endif

// Disable until we get around to enable + fix any issues.
#nullable disable

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
		const string selUTF8String = "UTF8String";
		const string selInitWithCharactersLength = "initWithCharacters:length:";

#if MONOMAC
		static IntPtr selUTF8StringHandle = Selector.GetHandle (selUTF8String);
		static IntPtr selInitWithCharactersLengthHandle = Selector.GetHandle (selInitWithCharactersLength);
#endif

		/// <summary>To be added.</summary>
		///         <remarks>To be added.</remarks>
		public static readonly NSString Empty = new NSString (String.Empty);

		internal NSString (NativeHandle handle, bool owns) : base (handle, owns)
		{
		}

		static NativeHandle CreateWithCharacters (NativeHandle handle, string str, int offset, int length, bool autorelease = false)
		{
			unsafe {
				fixed (char* ptrFirstChar = str) {
					var ptrStart = (IntPtr) (ptrFirstChar + offset);
#if MONOMAC
					handle = Messaging.IntPtr_objc_msgSend_IntPtr_IntPtr (handle, selInitWithCharactersLengthHandle, ptrStart, (IntPtr) length);
#else
					handle = Messaging.IntPtr_objc_msgSend_IntPtr_IntPtr (handle, Selector.GetHandle (selInitWithCharactersLength), ptrStart, (IntPtr) length);
#endif

					if (autorelease)
						NSObject.DangerousAutorelease (handle);

					return handle;
				}
			}
		}

		/// <include file="../../docs/api/Foundation/NSString.xml" path="/Documentation/Docs[@DocId='M:Foundation.NSString.CreateNative(System.String)']/*" />
		[Obsolete ("Use of 'CFString.CreateNative' offers better performance.")]
		[EditorBrowsable (EditorBrowsableState.Never)]
		public static NativeHandle CreateNative (string str)
		{
			return CreateNative (str, false);
		}

		/// <param name="str">To be added.</param>
		///         <param name="autorelease">To be added.</param>
		///         <summary>To be added.</summary>
		///         <returns>To be added.</returns>
		///         <remarks>To be added.</remarks>
		public static NativeHandle CreateNative (string str, bool autorelease)
		{
			if (str is null)
				return NativeHandle.Zero;

			return CreateNative (str, 0, str.Length, autorelease);
		}

		/// <param name="value">To be added.</param>
		///         <param name="start">To be added.</param>
		///         <param name="length">To be added.</param>
		///         <summary>To be added.</summary>
		///         <returns>To be added.</returns>
		///         <remarks>To be added.</remarks>
		public static NativeHandle CreateNative (string value, int start, int length)
		{
			return CreateNative (value, start, length, false);
		}

		/// <param name="value">To be added.</param>
		///         <param name="start">To be added.</param>
		///         <param name="length">To be added.</param>
		///         <param name="autorelease">To be added.</param>
		///         <summary>To be added.</summary>
		///         <returns>To be added.</returns>
		///         <remarks>To be added.</remarks>
		public static NativeHandle CreateNative (string value, int start, int length, bool autorelease)
		{
			if (value is null)
				return NativeHandle.Zero;

			if (start < 0 || start > value.Length)
				throw new ArgumentOutOfRangeException (nameof (start));

			if (length < 0 || start > value.Length - length)
				throw new ArgumentOutOfRangeException (nameof (length));

#if MONOMAC
			var handle = Messaging.IntPtr_objc_msgSend (class_ptr, Selector.AllocHandle);
#else
			var handle = Messaging.IntPtr_objc_msgSend (class_ptr, Selector.GetHandle (Selector.Alloc));
#endif

			return CreateWithCharacters (handle, value, start, length, autorelease);
		}

		public static void ReleaseNative (NativeHandle handle)
		{
			NSObject.DangerousRelease (handle);
		}

		/// <summary>Creates an <see cref="NSString" /> from a C# string.</summary>
		/// <param name="str">A C# string to create an <see cref="NSString" /> from.</param>
		public NSString (string str)
			: base (NSObjectFlag.Empty)
		{
			if (str is null)
				throw new ArgumentNullException (nameof (str));

			InitializeHandle (CreateWithCharacters (Handle, str, 0, str.Length));
		}

		/// <summary>Creates an <see cref="NSString" /> from a C# string.</summary>
		/// <param name="value">A C# string to create an <see cref="NSString" /> from.</param>
		/// <param name="start">The starting index of the <paramref name="value" /> string to create the <see cref="NSString" /> from.</param>
		/// <param name="length">The length, starting at <paramref name="start" />, of the <paramref name="value" /> string to create the <see cref="NSString" /> from.</param>
		public NSString (string value, int start, int length)
			: base (NSObjectFlag.Empty)
		{
			if (value is null)
				throw new ArgumentNullException (nameof (value));

			if (start < 0 || start > value.Length)
				throw new ArgumentOutOfRangeException (nameof (start));

			if (length < 0 || start > value.Length - length)
				throw new ArgumentOutOfRangeException (nameof (length));

			InitializeHandle (CreateWithCharacters (Handle, value, start, length));
		}

		/// <summary>Returns a string representation of the value of the current instance.</summary>
		///         <returns>
		///         </returns>
		///         <remarks>
		///         </remarks>
		public override string ToString ()
		{
			return FromHandle (Handle);
		}

		public static implicit operator string (NSString str)
		{
			if (((object) str) is null)
				return null;
			return str.ToString ();
		}

		public static explicit operator NSString (string str)
		{
			if (str is null)
				return null;
			return new NSString (str);
		}

		[EditorBrowsable (EditorBrowsableState.Never)]
		[Obsolete ("Use of 'CFString.FromHandle' offers better performance.")]
		public static string FromHandle (NativeHandle usrhandle)
		{
			return FromHandle (usrhandle, false);
		}

		[EditorBrowsable (EditorBrowsableState.Never)]
		[Obsolete ("Use of 'CFString.FromHandle' offers better performance.")]
		public static string FromHandle (NativeHandle handle, bool owns)
		{
			if (handle == NativeHandle.Zero)
				return null;

			try {
#if MONOMAC
				return Marshal.PtrToStringAuto (Messaging.IntPtr_objc_msgSend (handle, selUTF8StringHandle));
#else
				return Marshal.PtrToStringAuto (Messaging.IntPtr_objc_msgSend (handle, Selector.GetHandle (selUTF8String)));
#endif
			} finally {
				if (owns)
					DangerousRelease (handle);
			}
		}

		/// <param name="a">To be added.</param>
		///         <param name="b">To be added.</param>
		///         <summary>To be added.</summary>
		///         <returns>To be added.</returns>
		///         <remarks>To be added.</remarks>
		public static bool Equals (NSString a, NSString b)
		{
			if ((a as object) == (b as object))
				return true;

			if (((object) a) is null || ((object) b) is null)
				return false;

			if (a.Handle == b.Handle)
				return true;
			bool result = a.IsEqualTo (b.Handle);
			GC.KeepAlive (b);
			return result;
		}

		public static bool operator == (NSString a, NSString b)
		{
			return Equals (a, b);
		}

		public static bool operator != (NSString a, NSString b)
		{
			return !Equals (a, b);
		}

		/// <param name="obj">To be added.</param>
		///         <summary>To be added.</summary>
		///         <returns>To be added.</returns>
		///         <remarks>To be added.</remarks>
		public override bool Equals (Object obj)
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

		/// <param name="format">To be added.</param>
		///         <param name="args">To be added.</param>
		///         <summary>To be added.</summary>
		///         <returns>To be added.</returns>
		///         <remarks>To be added.</remarks>
		public static NSString LocalizedFormat (string format, params object [] args)
		{
			using (var ns = new NSString (format))
				return LocalizedFormat (ns, args);
		}

		/// <param name="format">To be added.</param>
		///         <param name="args">To be added.</param>
		///         <summary>To be added.</summary>
		///         <returns>To be added.</returns>
		///         <remarks>To be added.</remarks>
		public static NSString LocalizedFormat (NSString format, params object [] args)
		{
			int argc = args.Length;
			var nso = new NSObject [argc];
			for (int i = 0; i < argc; i++)
				nso [i] = NSObject.FromObject (args [i]);

			return LocalizedFormat (format, nso);
		}

		/// <param name="format">To be added.</param>
		///         <param name="args">To be added.</param>
		///         <summary>To be added.</summary>
		///         <returns>To be added.</returns>
		///         <remarks>To be added.</remarks>
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

		/// <param name="transform">To be added.</param>
		///         <param name="reverse">To be added.</param>
		///         <summary>To be added.</summary>
		///         <returns>To be added.</returns>
		///         <remarks>To be added.</remarks>
		public NSString TransliterateString (NSStringTransform transform, bool reverse)
		{
			return TransliterateString (transform.GetConstant (), reverse);
		}

		/// <summary>Generates a hash code for the current instance.</summary>
		///         <returns>A int containing the hash code for this instance.</returns>
		///         <remarks>The algorithm used to generate the hash code is unspecified.</remarks>
		public override int GetHashCode ()
		{
			return base.GetHashCode ();
		}
#endif // !COREBUILD
	}
}
