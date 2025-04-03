//
// Dlfcn.cs: Support for looking up symbols in shared libraries
//
// Authors:
//   Jonathan Pryor:
//   Miguel de Icaza.
//
// Copyright 2009-2010, Novell, Inc.
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

#nullable enable

using System;
using System.ComponentModel;
using System.Runtime.InteropServices;
#if !COREBUILD
using Foundation;
using CoreFoundation;
using CoreGraphics;
using CoreMedia;
#endif
#if !NO_SYSTEM_DRAWING
using System.Drawing;
#endif

namespace ObjCRuntime {

	// most are being generated from bindings
	static partial class Libraries {
#if !COREBUILD
		static public class System {
			static public readonly IntPtr Handle = Dlfcn._dlopen (Constants.libSystemLibrary, 0);
		}
		static public class LibC {
			static public readonly IntPtr Handle = Dlfcn._dlopen (Constants.libcLibrary, 0);
		}
#if HAS_OPENGLES
		static public class OpenGLES {
			static public readonly IntPtr Handle = Dlfcn._dlopen (Constants.OpenGLESLibrary, 0);
		}
#endif
		static public class AudioToolbox {
			static public readonly IntPtr Handle = Dlfcn._dlopen (Constants.AudioToolboxLibrary, 0);
		}
#endif
	}

	public static class Dlfcn {
#if !COREBUILD
		public enum RTLD {
			/// <summary>The dynamic linker searches for the symbol in the dylibs the calling image linked against when built. It is usually used when you intentionally have multiply defined symbol across images and want to find the "next" definition. </summary>
			Next = -1,
			/// <summary>Searches all Mach-O images in the process (except those loaded with dlopen(xxx, RTLD_LOCAL)) in the order they were loaded.  This can be a costly search and should be avoided.</summary>
			Default = -2,
			/// <summary>Search for the symbol starts with the image that called dlsym.  If it is not found, the search continues as if Next was used.</summary>
			Self = -3,
			/// <summary>Only searches for symbol in the main executable.</summary>
			MainOnly = -5,
		}

		[Flags]
		public enum Mode : int {
			None = 0x0,
			Lazy = 0x1,
			Now = 0x2,
			Local = 0x4,
			Global = 0x8,
			NoLoad = 0x10,
			NoDelete = 0x80,
			First = 0x100,
		}

#if MONOMAC && !NET
		[DllImport (Constants.libcLibrary)]
		internal static extern int dladdr (IntPtr addr, out Dl_info info);

		internal struct Dl_info
		{
			internal IntPtr dli_fname; /* Pathname of shared object */
			internal IntPtr dli_fbase; /* Base address of shared object */
			internal IntPtr dli_sname; /* Name of nearest symbol */
			internal IntPtr dli_saddr; /* Address of nearest symbol */
		}
#endif

		/// <param name="handle">Handle previously returned by dlopen</param>
		///         <summary>Closes and unloads the native shared library referenced by the handle.</summary>
		///         <returns>A Unix error code, or zero on success.</returns>
		///         <remarks>
		///         </remarks>
		[DllImport (Constants.libSystemLibrary)]
		public static extern int dlclose (IntPtr handle);

		[DllImport (Constants.libSystemLibrary, EntryPoint = "dlopen")]
		static extern IntPtr _dlopen (IntPtr path, Mode mode /* this is int32, not nint */);

		internal static IntPtr _dlopen (string? path, Mode mode /* this is int32, not nint */)
		{
			using var pathPtr = new TransientString (path);
			return _dlopen (pathPtr, mode);
		}

		/// <param name="path">Path to the dynamic library.</param>
		///         <param name="mode">Bitmask, values defined in the Unix dlopen(2) man page.</param>
		///         <summary>Loads the specified dynamic library into memory. </summary>
		///         <returns>The handle to the library, or IntPtr.Zero on failure.</returns>
		///         <remarks>
		///         </remarks>
		public static IntPtr dlopen (string? path, int mode)
		{
			return dlopen (path, mode, showWarning: true);
		}

		public static IntPtr dlopen (string? path, Mode mode)
		{
			return _dlopen (path, mode);
		}

		static bool warningShown;
		// the linker can eliminate the body of this method (and the above static variable) on release builds
		static void WarnOnce ()
		{
			if (!warningShown)
				Runtime.NSLog ("You are using dlopen without a full path, retrying by prepending /usr/lib");
			warningShown = true;
		}

		internal static IntPtr dlopen (string? path, int mode, bool showWarning)
		{
			var x = _dlopen (path, (Mode) mode);
			if (x != IntPtr.Zero)
				return x;

			// In iOS < 9, you could dlopen ("libc") and that would work.
			// In iOS >= 9, this fails with:
			// "no cache image with name (<top>)"
			if (path?.IndexOf ('/') == -1) {
				if (showWarning)
					WarnOnce ();
				return dlopen ("/usr/lib/" + path, mode, false);
			}
			return IntPtr.Zero;
		}

		[DllImport (Constants.libSystemLibrary)]
		static extern IntPtr dlsym (IntPtr handle, IntPtr symbol);

		/// <include file="../../docs/api/ObjCRuntime/Dlfcn.xml" path="/Documentation/Docs[@DocId='M:ObjCRuntime.Dlfcn.dlsym(System.IntPtr,System.String)']/*" />
		public static IntPtr dlsym (IntPtr handle, string symbol)
		{
			using var symbolPtr = new TransientString (symbol);
			return dlsym (handle, symbolPtr);
		}

		/// <param name="lookupType">Determines how the symbol is looked up</param>
		///         <param name="symbol">Name of the public symbol in the dynamic library to look up.</param>
		///         <summary>Returns the address of the specified symbol in the
		/// 	current process.</summary>
		///         <returns>
		/// 	  Returns <see langword="null" /> if the symbol was not found.   The error condition can be probed using the <see cref="M:ObjCRuntime.Dlfcn.dlerror" />.
		///         </returns>
		///         <remarks>
		///           <para>
		/// 	    Returns the address of the specified symbol in the dynamic library.
		/// 	  </para>
		///           <para>
		/// 	    The <paramref name="lookupType" /> controls which libraries
		/// 	    the dynamic linker will search.   
		/// 	  </para>
		///         </remarks>
		public static IntPtr dlsym (RTLD lookupType, string symbol)
		{
			return dlsym ((IntPtr) lookupType, symbol);
		}

		[DllImport (Constants.libSystemLibrary, EntryPoint = "dlerror")]
		internal static extern IntPtr dlerror_ ();

		/// <summary>Returns a diagnostics message for the last failure when using any of the methods in this class.</summary>
		///         <returns>Human-readable message.</returns>
		///         <remarks>
		///         </remarks>
		public static string? dlerror ()
		{
			// we can't free the string returned from dlerror
			return Marshal.PtrToStringAnsi (dlerror_ ());
		}

		/// <param name="handle">Handle to the dynamic library previously opened with <see cref="M:ObjCRuntime.Dlfcn.dlopen(System.String,System.Int32)" />.</param>
		///         <param name="symbol">Name of the public symbol in the dynamic library to look up.</param>
		///         <summary>Gets the NSString value exposed with the given symbol from the dynamic library.</summary>
		///         <returns>The value from the library, or null on error.</returns>
		///         <remarks>
		///           <para>If this routine fails, it will return null.</para>
		///         </remarks>
		public static NSString? GetStringConstant (IntPtr handle, string symbol)
		{
			var indirect = dlsym (handle, symbol);
			if (indirect == IntPtr.Zero)
				return null;
			var actual = Marshal.ReadIntPtr (indirect);
			if (actual == IntPtr.Zero)
				return null;
			return Runtime.GetNSObject<NSString> (actual);
		}

		/// <param name="handle">Handle to the dynamic library previously opened with <see cref="M:ObjCRuntime.Dlfcn.dlopen(System.String,System.Int32)" />.</param>
		///         <param name="symbol">Name of the public symbol in the dynamic library to look up.</param>
		///         <summary>Gets the pointer in memory to the specified symbol.</summary>
		///         <returns>The value from the library, or IntPtr.Zero on failure.</returns>
		///         <remarks>
		///           <para>Use this to get a generic pointer to a public symbol in the library.</para>
		///         </remarks>
		public static IntPtr GetIndirect (IntPtr handle, string symbol)
		{
			return dlsym (handle, symbol);
		}

		/// <summary>Gets the struct value exposed with the given symbol from the dynamic library.</summary>
		/// <param name="handle">Handle to the dynamic library previously opened with <see cref="dlopen(string,int)" />.</param>
		/// <param name="symbol">Name of the public symbol in the dynamic library to look up.</param>
		/// <returns>The struct from the library, or an empty struct (<c>default(T)</c>) if the symbol couldn't be found.</returns>
		public static T GetStruct<T> (IntPtr handle, string symbol) where T : unmanaged
		{
			var ptr = GetIndirect (handle, symbol);
			if (ptr == IntPtr.Zero)
				return default (T);
			unsafe {
				return *(T*) ptr;
			}
		}

		/// <param name="handle">Handle to the dynamic library previously opened with <see cref="M:ObjCRuntime.Dlfcn.dlopen(System.String,System.Int32)" />.</param>
		///         <param name="symbol">Name of the public symbol in the dynamic library to look up.</param>
		///         <summary>Gets an NSNumber value exposed with the given symbol from the dynamic library.</summary>
		///         <returns>The value from the library, or null on error.</returns>
		///         <remarks>
		///           <para>If this routine fails, it will return null.</para>
		///         </remarks>
		public static NSNumber? GetNSNumber (IntPtr handle, string symbol)
		{
			var indirect = dlsym (handle, symbol);
			if (indirect == IntPtr.Zero)
				return null;
			var actual = Marshal.ReadIntPtr (indirect);
			if (actual == IntPtr.Zero)
				return null;
			return Runtime.GetNSObject<NSNumber> (actual);
		}

		/// <summary>Gets the signed byte value exposed with the given symbol from the dynamic library.</summary>
		/// <param name="handle">Handle to the dynamic library previously opened with <see cref="dlopen(System.String,System.Int32)" /> or <see cref="dlopen(System.String,Mode)" />.</param>
		/// <param name="symbol">Name of the public symbol in the dynamic library to look up.</param>
		/// <returns>The value from the library, or zero on failure.</returns>
		/// <remarks>If this routine fails, it will return zero.</remarks>
		public static sbyte GetSByte (IntPtr handle, string symbol)
		{
			var indirect = dlsym (handle, symbol);
			if (indirect == IntPtr.Zero)
				return 0;
			unchecked {
				return (sbyte) Marshal.ReadByte (indirect);
			}
		}

		/// <summary>Sets the specified symbol in the library handle to the specified signed byte value.</summary>
		/// <param name="handle">Handle to the dynamic library previously opened with <see cref="dlopen(System.String,System.Int32)" /> or <see cref="dlopen(System.String,Mode)" />.</param>
		/// <param name="symbol">Name of the public symbol in the dynamic library to look up.</param>
		/// <param name="value">The value to set.</param>
		public static void SetSByte (IntPtr handle, string symbol, sbyte value)
		{
			var indirect = dlsym (handle, symbol);
			if (indirect == IntPtr.Zero)
				return;
			unsafe {
				Marshal.WriteByte (indirect, (byte) value);
			}
		}

		/// <summary>Gets the byte value exposed with the given symbol from the dynamic library.</summary>
		/// <param name="handle">Handle to the dynamic library previously opened with <see cref="dlopen(System.String,System.Int32)" /> or <see cref="dlopen(System.String,Mode)" />.</param>
		/// <param name="symbol">Name of the public symbol in the dynamic library to look up.</param>
		/// <returns>The value from the library, or zero on failure.</returns>
		/// <remarks>If this routine fails, it will return zero.</remarks>
		public static byte GetByte (IntPtr handle, string symbol)
		{
			var indirect = dlsym (handle, symbol);
			if (indirect == IntPtr.Zero)
				return 0;
			return Marshal.ReadByte (indirect);
		}

		/// <summary>Sets the specified symbol in the library handle to the specified byte value.</summary>
		/// <param name="handle">Handle to the dynamic library previously opened with <see cref="dlopen(System.String,System.Int32)" /> or <see cref="dlopen(System.String,Mode)" />.</param>
		/// <param name="symbol">Name of the public symbol in the dynamic library to look up.</param>
		/// <param name="value">The value to set.</param>
		public static void SetByte (IntPtr handle, string symbol, byte value)
		{
			var indirect = dlsym (handle, symbol);
			if (indirect == IntPtr.Zero)
				return;
			Marshal.WriteByte (indirect, value);
		}

		/// <summary>Gets the short value exposed with the given symbol from the dynamic library.</summary>
		/// <param name="handle">Handle to the dynamic library previously opened with <see cref="dlopen(System.String,System.Int32)" /> or <see cref="dlopen(System.String,Mode)" />.</param>
		/// <param name="symbol">Name of the public symbol in the dynamic library to look up.</param>
		/// <returns>The value from the library, or zero on failure.</returns>
		/// <remarks>If this routine fails, it will return zero.</remarks>
		public static short GetInt16 (IntPtr handle, string symbol)
		{
			var indirect = dlsym (handle, symbol);
			if (indirect == IntPtr.Zero)
				return 0;
			return Marshal.ReadInt16 (indirect);
		}

		/// <summary>Sets the specified symbol in the library handle to the specified short value.</summary>
		/// <param name="handle">Handle to the dynamic library previously opened with <see cref="dlopen(System.String,System.Int32)" /> or <see cref="dlopen(System.String,Mode)" />.</param>
		/// <param name="symbol">Name of the public symbol in the dynamic library to look up.</param>
		/// <param name="value">The value to set.</param>
		public static void SetInt16 (IntPtr handle, string symbol, short value)
		{
			var indirect = dlsym (handle, symbol);
			if (indirect == IntPtr.Zero)
				return;
			Marshal.WriteInt16 (indirect, value);
		}

		/// <summary>Gets the ushort value exposed with the given symbol from the dynamic library.</summary>
		/// <param name="handle">Handle to the dynamic library previously opened with <see cref="dlopen(System.String,System.Int32)" /> or <see cref="dlopen(System.String,Mode)" />.</param>
		/// <param name="symbol">Name of the public symbol in the dynamic library to look up.</param>
		/// <returns>The value from the library, or zero on failure.</returns>
		/// <remarks>If this routine fails, it will return zero.</remarks>
		public static ushort GetUInt16 (IntPtr handle, string symbol)
		{
			var indirect = dlsym (handle, symbol);
			if (indirect == IntPtr.Zero)
				return 0;
			unchecked {
				return (ushort) Marshal.ReadInt16 (indirect);
			}
		}

		/// <summary>Sets the specified symbol in the library handle to the specified ushort value.</summary>
		/// <param name="handle">Handle to the dynamic library previously opened with <see cref="dlopen(System.String,System.Int32)" /> or <see cref="dlopen(System.String,Mode)" />.</param>
		/// <param name="symbol">Name of the public symbol in the dynamic library to look up.</param>
		/// <param name="value">The value to set.</param>
		public static void SetUInt16 (IntPtr handle, string symbol, ushort value)
		{
			var indirect = dlsym (handle, symbol);
			if (indirect == IntPtr.Zero)
				return;
			unchecked {
				Marshal.WriteInt16 (indirect, (short) value);
			}
		}

		/// <param name="handle">Handle to the dynamic library previously opened with <see cref="M:ObjCRuntime.Dlfcn.dlopen(System.String,System.Int32)" />.</param>
		///         <param name="symbol">Name of the public symbol in the dynamic library to look up.</param>
		///         <summary>Gets the int value exposed with the given symbol from the dynamic library.</summary>
		///         <returns>The value from the library, or zero on failure.</returns>
		///         <remarks>
		///           <para>If this routine fails, it will return zero.</para>
		///         </remarks>
		public static int GetInt32 (IntPtr handle, string symbol)
		{
			var indirect = dlsym (handle, symbol);
			if (indirect == IntPtr.Zero)
				return 0;
			return Marshal.ReadInt32 (indirect);
		}

		/// <param name="handle">Handle to the dynamic library previously opened with <see cref="M:ObjCRuntime.Dlfcn.dlopen(System.String,System.Int32)" />.</param>
		///         <param name="symbol">Name of the public symbol in the dynamic library to look up.</param>
		///         <param name="value">The value to set.</param>
		///         <summary>Sets the specified symbol in the library handle to the specified int value.</summary>
		///         <remarks>
		///         </remarks>
		public static void SetInt32 (IntPtr handle, string symbol, int value)
		{
			var indirect = dlsym (handle, symbol);
			if (indirect == IntPtr.Zero)
				return;
			Marshal.WriteInt32 (indirect, value);
		}

		/// <param name="handle">Handle to the dynamic library previously opened with <see cref="M:ObjCRuntime.Dlfcn.dlopen(System.String,System.Int32)" />.</param>
		///         <param name="symbol">Name of the public symbol in the dynamic library to look up.</param>
		///         <summary>Gets the uint value exposed with the given symbol from the dynamic library.</summary>
		///         <returns>The value from the library, or zero on failure.</returns>
		///         <remarks>
		///           <para>If this routine fails, it will return zero.</para>
		///         </remarks>
		public static uint GetUInt32 (IntPtr handle, string symbol)
		{
			var indirect = dlsym (handle, symbol);
			if (indirect == IntPtr.Zero)
				return 0;
			return (uint) Marshal.ReadInt32 (indirect);
		}

		/// <param name="handle">Handle to the dynamic library previously opened with <see cref="M:ObjCRuntime.Dlfcn.dlopen(System.String,System.Int32)" />.</param>
		///         <param name="symbol">Name of the public symbol in the dynamic library to look up.</param>
		///         <param name="value">The value to set.</param>
		///         <summary>Sets the specified symbol in the library handle to the specified uint value.</summary>
		///         <remarks>
		///         </remarks>
		public static void SetUInt32 (IntPtr handle, string symbol, uint value)
		{
			var indirect = dlsym (handle, symbol);
			if (indirect == IntPtr.Zero)
				return;
			Marshal.WriteInt32 (indirect, (int) value);
		}

		/// <param name="handle">Handle to the dynamic library previously opened with <see cref="M:ObjCRuntime.Dlfcn.dlopen(System.String,System.Int32)" />.</param>
		///         <param name="symbol">Name of the public symbol in the dynamic library to look up.</param>
		///         <summary>Gets the long value exposed with the given symbol from the dynamic library.</summary>
		///         <returns>The value from the library, or zero on failure.</returns>
		///         <remarks>
		///           <para>If this routine fails, it will return zero.</para>
		///         </remarks>
		public static long GetInt64 (IntPtr handle, string symbol)
		{
			var indirect = dlsym (handle, symbol);
			if (indirect == IntPtr.Zero)
				return 0;
			return Marshal.ReadInt64 (indirect);
		}

		/// <param name="handle">Handle to the dynamic library previously opened with <see cref="M:ObjCRuntime.Dlfcn.dlopen(System.String,System.Int32)" />.</param>
		///         <param name="symbol">Name of the public symbol in the dynamic library to look up.</param>
		///         <param name="value">The value to set.</param>
		///         <summary>Sets the specified symbol in the library handle to the specified long value.</summary>
		///         <remarks>
		///         </remarks>
		public static void SetInt64 (IntPtr handle, string symbol, long value)
		{
			var indirect = dlsym (handle, symbol);
			if (indirect == IntPtr.Zero)
				return;
			Marshal.WriteInt64 (indirect, value);
		}

		/// <param name="handle">Handle to the dynamic library previously opened with <see cref="M:ObjCRuntime.Dlfcn.dlopen(System.String,System.Int32)" />.</param>
		///         <param name="symbol">Name of the public symbol in the dynamic library to look up.</param>
		///         <summary>Gets the ulong value exposed with the given symbol from the dynamic library.</summary>
		///         <returns>The value from the library, or zero on failure.</returns>
		///         <remarks>
		///           <para>If this routine fails, it will return zero.</para>
		///         </remarks>
		public static ulong GetUInt64 (IntPtr handle, string symbol)
		{
			var indirect = dlsym (handle, symbol);
			if (indirect == IntPtr.Zero)
				return 0;

			return (ulong) Marshal.ReadInt64 (indirect);
		}

#if !NET
		[Obsolete ("Use 'SetInt64' for long values instead.")]
		public static void SetUInt64 (IntPtr handle, string symbol, long value)
		{
			var indirect = dlsym (handle, symbol);
			if (indirect == IntPtr.Zero)
				return;

			Marshal.WriteInt64 (indirect, (long) value);
		}
#endif

		/// <param name="handle">Handle to the dynamic library previously opened with <see cref="M:ObjCRuntime.Dlfcn.dlopen(System.String,System.Int32)" />.</param>
		///         <param name="symbol">Name of the public symbol in the dynamic library to look up.</param>
		///         <param name="value">The value to set.</param>
		///         <summary>Sets the specified symbol in the library handle to the specified ulong value.</summary>
		///         <remarks>
		///         </remarks>
		public static void SetUInt64 (IntPtr handle, string symbol, ulong value)
		{
			var indirect = dlsym (handle, symbol);
			if (indirect == IntPtr.Zero)
				return;

			Marshal.WriteInt64 (indirect, (long) value);
		}

		/// <param name="handle">Handle to the dynamic library previously opened with <see cref="M:ObjCRuntime.Dlfcn.dlopen(System.String,System.Int32)" />.</param>
		///         <param name="symbol">Name of the public symbol in the dynamic library to look up.</param>
		///         <param name="value">The string to set, can be null.</param>
		///         <summary>Sets the specified symbol in the library handle to the specified string value.</summary>
		///         <remarks>
		///           <para>The previous string value is not released, it is up to the developer to release the handle to that string if needed.</para>
		///         </remarks>
		public static void SetString (IntPtr handle, string symbol, string? value)
		{
			var indirect = dlsym (handle, symbol);
			if (indirect == IntPtr.Zero)
				return;
			Marshal.WriteIntPtr (indirect, CFString.CreateNative (value));
		}

		/// <param name="handle">Handle to the dynamic library previously opened with <see cref="M:ObjCRuntime.Dlfcn.dlopen(System.String,System.Int32)" />.</param>
		///         <param name="symbol">Name of the public symbol in the dynamic library to look up.</param>
		///         <param name="value">The string to set, can be null.</param>
		///         <summary>Sets the specified symbol in the library handle to the specified string value.</summary>
		///         <remarks>
		///           <para>The previous string value is not released, it is up to the developer to release the handle to that string if needed.</para>
		///         </remarks>
		public static void SetString (IntPtr handle, string symbol, NSString? value)
		{
			SetObject (handle, symbol, value);
		}

		/// <param name="handle">Handle to the dynamic library previously opened with <see cref="M:ObjCRuntime.Dlfcn.dlopen(System.String,System.Int32)" />.</param>
		///         <param name="symbol">Name of the public symbol in the dynamic library to look up.</param>
		///         <param name="array">The array to set, can be null.</param>
		///         <summary>Sets the specified symbol in the library handle to the specified array value.</summary>
		///         <remarks>
		/// 	  The previous array is not released, it is up to the developer to release the handle to that array if needed.</remarks>
		public static void SetArray (IntPtr handle, string symbol, NSArray? array)
		{
			SetObject (handle, symbol, array);
		}

		/// <summary>Sets the specified symbol in the library handle to the specified NSObject value.</summary>
		/// <param name="handle">Handle to the dynamic library previously opened with <see cref="dlopen(System.String,Mode)" />.</param>
		/// <param name="symbol">Name of the public symbol in the dynamic library to look up.</param>
		/// <param name="value">The object to set, can be null.</param>
		/// <remarks>The previous object value is not released, it is up to the developer to release the handle to that object if needed.</remarks>
		public static void SetObject (IntPtr handle, string symbol, NSObject? value)
		{
			var indirect = dlsym (handle, symbol);
			if (indirect == IntPtr.Zero)
				return;
			var objectHandle = value.GetHandle ();
			if (objectHandle != IntPtr.Zero)
				CFObject.CFRetain (objectHandle);
			GC.KeepAlive (value);
			Marshal.WriteIntPtr (indirect, objectHandle);
		}

		/// <param name="handle">Handle to the dynamic library previously opened with <see cref="M:ObjCRuntime.Dlfcn.dlopen(System.String,System.Int32)" />.</param>
		///         <param name="symbol">Name of the public symbol in the dynamic library to look up.</param>
		///         <summary>Gets the nint value exposed with the given symbol from the dynamic library.</summary>
		///         <returns>The value from the library, or zero on failure.</returns>
		///         <remarks>
		///           <para>If this routine fails, it will return zero.</para>
		///         </remarks>
		public static nint GetNInt (IntPtr handle, string symbol)
		{
			return (nint) GetIntPtr (handle, symbol);
		}

		public static void SetNInt (IntPtr handle, string symbol, nint value)
		{
			SetIntPtr (handle, symbol, (IntPtr) value);
		}

		/// <param name="handle">Handle to the dynamic library previously opened with <see cref="M:ObjCRuntime.Dlfcn.dlopen(System.String,System.Int32)" />.</param>
		///         <param name="symbol">Name of the public symbol in the dynamic library to look up.</param>
		///         <summary>Gets the nuint value exposed with the given symbol from the dynamic library.</summary>
		///         <returns>The value from the library, or zero on failure.</returns>
		///         <remarks>
		///           <para>If this routine fails, it will return zero.</para>
		///         </remarks>
		public static nuint GetNUInt (IntPtr handle, string symbol)
		{
			return (nuint) (ulong) GetUIntPtr (handle, symbol);
		}

		public static void SetNUInt (IntPtr handle, string symbol, nuint value)
		{
			SetUIntPtr (handle, symbol, (UIntPtr) (ulong) value);
		}

		/// <param name="handle">Handle to the dynamic library previously opened with <see cref="M:ObjCRuntime.Dlfcn.dlopen(System.String,System.Int32)" />.</param>
		///         <param name="symbol">Name of the public symbol in the dynamic library to look up.</param>
		///         <summary>Gets the nfloat value exposed with the given symbol from the dynamic library.</summary>
		///         <returns>The value from the library, or zero on failure.</returns>
		///         <remarks>
		///           <para>If this routine fails, it will return zero.</para>
		///         </remarks>
		public static nfloat GetNFloat (IntPtr handle, string symbol)
		{
			var indirect = dlsym (handle, symbol);
			if (indirect == IntPtr.Zero)
				return 0;

			unsafe {
				if (sizeof (IntPtr) == 4)
					return (nfloat) (*(float*) indirect);
				else
					return (nfloat) (*(double*) indirect);
			}
		}

		public static void SetNFloat (IntPtr handle, string symbol, nfloat value)
		{
			var indirect = dlsym (handle, symbol);
			if (indirect == IntPtr.Zero)
				return;

			unsafe {
				nfloat* ptr = (nfloat*) indirect;
				*ptr = value;
			}
		}

		/// <param name="handle">Handle to the dynamic library previously opened with <see cref="M:ObjCRuntime.Dlfcn.dlopen(System.String,System.Int32)" />.</param>
		///         <param name="symbol">Name of the public symbol in the dynamic library to look up.</param>
		///         <summary>Gets the IntPtr value exposed with the given symbol from the dynamic library.</summary>
		///         <returns>The value from the library, or IntPtr.Zero on failure.</returns>
		///         <remarks>
		///           <para>If this routine fails, it will return IntPtr.Zero.</para>
		///         </remarks>
		public static IntPtr GetIntPtr (IntPtr handle, string symbol)
		{
			var indirect = dlsym (handle, symbol);
			if (indirect == IntPtr.Zero)
				return IntPtr.Zero;
			return Marshal.ReadIntPtr (indirect);
		}

		public static UIntPtr GetUIntPtr (IntPtr handle, string symbol)
		{
			var indirect = dlsym (handle, symbol);
			if (indirect == IntPtr.Zero)
				return UIntPtr.Zero;
			return (UIntPtr) (long) Marshal.ReadIntPtr (indirect);
		}

		public static void SetUIntPtr (IntPtr handle, string symbol, UIntPtr value)
		{
			var indirect = dlsym (handle, symbol);
			if (indirect == IntPtr.Zero)
				return;
			Marshal.WriteIntPtr (indirect, (IntPtr) (ulong) value);
		}

		/// <param name="handle">Handle to the dynamic library previously opened with <see cref="M:ObjCRuntime.Dlfcn.dlopen(System.String,System.Int32)" />.</param>
		///         <param name="symbol">Name of the public symbol in the dynamic library to look up.</param>
		///         <param name="value">The value to set.</param>
		///         <summary>Sets the specified symbol in the library handle to the specified IntPtr value.</summary>
		///         <remarks>
		///         </remarks>
		public static void SetIntPtr (IntPtr handle, string symbol, IntPtr value)
		{
			var indirect = dlsym (handle, symbol);
			if (indirect == IntPtr.Zero)
				return;
			Marshal.WriteIntPtr (indirect, value);
		}

		/// <param name="handle">Handle to the dynamic library previously opened with <see cref="M:ObjCRuntime.Dlfcn.dlopen(System.String,System.Int32)" />.</param>
		///         <param name="symbol">Name of the public symbol in the dynamic library to look up.</param>
		///         <summary>Gets the <see cref="T:CoreGraphics.CGRect" /> value exposed with the given symbol from the dynamic library.</summary>
		///         <returns>The value from the library, or an empty CGRect on failure.</returns>
		///         <remarks>
		///           <para>If this routine fails to find the symbol, this will return an empty CGRect.</para>
		///         </remarks>
		public static CGRect GetCGRect (IntPtr handle, string symbol)
		{
			var indirect = dlsym (handle, symbol);
			if (indirect == IntPtr.Zero)
				return CGRect.Empty;
			unsafe {
				nfloat* ptr = (nfloat*) indirect;
				return new CGRect (ptr [0], ptr [1], ptr [2], ptr [3]);
			}
		}

		/// <param name="handle">Handle to the dynamic library previously opened with <see cref="M:ObjCRuntime.Dlfcn.dlopen(System.String,System.Int32)" />.</param>
		///         <param name="symbol">Name of the public symbol in the dynamic library to look up.</param>
		///         <summary>Gets the <see cref="T:CoreGraphics.CGSize" /> value exposed with the given symbol from the dynamic library.</summary>
		///         <returns>The value from the library, or an empty CGSize on failure.</returns>
		///         <remarks>
		///           <para>If this routine fails to find the symbol, this will return an empty CGSize.</para>
		///         </remarks>
		public static CGSize GetCGSize (IntPtr handle, string symbol)
		{
			var indirect = dlsym (handle, symbol);
			if (indirect == IntPtr.Zero)
				return CGSize.Empty;
			unsafe {
				nfloat* ptr = (nfloat*) indirect;
				return new CGSize (ptr [0], ptr [1]);
			}
		}

		/// <param name="handle">Handle to the dynamic library previously opened with <see cref="M:ObjCRuntime.Dlfcn.dlopen(System.String,System.Int32)" />.</param>
		///         <param name="symbol">Name of the public symbol in the dynamic library to look up.</param>
		///         <param name="value">The value to set.</param>
		///         <summary>Gets the <see cref="T:CoreGraphics.CGSize" /> value exposed with the given symbol from the dynamic library.</summary>
		///         <remarks>
		///         </remarks>
		public static void SetCGSize (IntPtr handle, string symbol, CGSize value)
		{
			var indirect = dlsym (handle, symbol);
			if (indirect == IntPtr.Zero)
				return;
			unsafe {
				nfloat* ptr = (nfloat*) indirect;
				ptr [0] = value.Width;
				ptr [1] = value.Height;
			}
		}

		/// <param name="handle">Handle to the dynamic library previously opened with <see cref="M:ObjCRuntime.Dlfcn.dlopen(System.String,System.Int32)" />.</param>
		///         <param name="symbol">Name of the public symbol in the dynamic library to look up.</param>
		///         <summary>Gets the double value exposed with the given symbol from the dynamic library.</summary>
		///         <returns>The value from the library, or zero on failure.</returns>
		///         <remarks>
		///           <para>If this routine fails, it will return zero.</para>
		///         </remarks>
		public static double GetDouble (IntPtr handle, string symbol)
		{
			var indirect = dlsym (handle, symbol);
			if (indirect == IntPtr.Zero)
				return 0;
			unsafe {
				double* d = (double*) indirect;

				return *d;
			}
		}

		/// <param name="handle">Handle to the dynamic library previously opened with <see cref="M:ObjCRuntime.Dlfcn.dlopen(System.String,System.Int32)" />.</param>
		///         <param name="symbol">Name of the public symbol in the dynamic library to look up.</param>
		///         <param name="value">The value to set.</param>
		///         <summary>Sets the specified symbol in the library handle to the specified double value.</summary>
		///         <remarks>
		///         </remarks>
		public static void SetDouble (IntPtr handle, string symbol, double value)
		{
			var indirect = dlsym (handle, symbol);
			if (indirect == IntPtr.Zero)
				return;
			unsafe {
				*(double*) indirect = value;
			}
		}

		/// <param name="handle">Handle to the dynamic library previously opened with <see cref="M:ObjCRuntime.Dlfcn.dlopen(System.String,System.Int32)" />.</param>
		///         <param name="symbol">Name of the public symbol in the dynamic library to look up.</param>
		///         <summary>Gets the float value exposed with the given symbol from the dynamic library.</summary>
		///         <returns>The value from the library, or zero on failure.</returns>
		///         <remarks>
		///           <para>If this routine fails, it will return zero.</para>
		///         </remarks>
		public static float GetFloat (IntPtr handle, string symbol)
		{
			var indirect = dlsym (handle, symbol);
			if (indirect == IntPtr.Zero)
				return 0;
			unsafe {
				float* d = (float*) indirect;

				return *d;
			}
		}

		/// <param name="handle">Handle to the dynamic library previously opened with <see cref="M:ObjCRuntime.Dlfcn.dlopen(System.String,System.Int32)" />.</param>
		///         <param name="symbol">Name of the public symbol in the dynamic library to look up.</param>
		///         <param name="value">The value to set.</param>
		///         <summary>Sets the specified symbol in the library handle to the specified float value.</summary>
		///         <remarks>
		///         </remarks>
		public static void SetFloat (IntPtr handle, string symbol, float value)
		{
			var indirect = dlsym (handle, symbol);
			if (indirect == IntPtr.Zero)
				return;
			unsafe {
				*(float*) indirect = value;
			}
		}

		internal static int SlowGetInt32 (string? lib, string symbol)
		{
			var handle = dlopen (lib, 0);
			if (handle == IntPtr.Zero)
				return 0;
			try {
				return GetInt32 (handle, symbol);
			} finally {
				dlclose (handle);
			}
		}

		internal static long SlowGetInt64 (string? lib, string symbol)
		{
			var handle = dlopen (lib, 0);
			if (handle == IntPtr.Zero)
				return 0;
			try {
				return GetInt64 (handle, symbol);
			} finally {
				dlclose (handle);
			}
		}

		internal static IntPtr SlowGetIntPtr (string? lib, string symbol)
		{
			var handle = dlopen (lib, 0);
			if (handle == IntPtr.Zero)
				return IntPtr.Zero;
			try {
				return GetIntPtr (handle, symbol);
			} finally {
				dlclose (handle);
			}
		}

		internal static double SlowGetDouble (string? lib, string symbol)
		{
			var handle = dlopen (lib, 0);
			if (handle == IntPtr.Zero)
				return 0;
			try {
				return GetDouble (handle, symbol);
			} finally {
				dlclose (handle);
			}
		}

		internal static NSString? SlowGetStringConstant (string? lib, string symbol)
		{
			var handle = dlopen (lib, 0);
			if (handle == IntPtr.Zero)
				return null;
			try {
				return GetStringConstant (handle, symbol);
			} finally {
				dlclose (handle);
			}
		}
#endif // !COREBUILD
		/// <param name="handle">The handle for the library to search.</param>
		///         <param name="constant">The symbol to find.</param>
		///         <param name="storage">A pointer to a storage location for the resulting pointer.</param>
		///         <summary>Looks up the specified constant symbol in the specified library, and stores it in the specified storage (unless the storage already contains a value, in which case that value is returned)</summary>
		///         <returns>A pointer to the constant symbol in the specified library.</returns>
		///         <remarks>
		///         </remarks>
		[EditorBrowsable (EditorBrowsableState.Advanced)]
		public static unsafe IntPtr CachePointer (IntPtr handle, string constant, IntPtr* storage)
		{
			if (*storage == IntPtr.Zero)
				*storage = Dlfcn.GetIntPtr (handle, constant);
			return *storage;
		}
	}
}
