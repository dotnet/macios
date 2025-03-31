//
// Copyright 2012-2014 Xamarin
//

#nullable enable

using System;
using System.Runtime.InteropServices;
using System.Runtime.Versioning;

using CoreFoundation;
using Foundation;
using ObjCRuntime;

namespace CoreFoundation {
	[SupportedOSPlatform ("ios")]
	[SupportedOSPlatform ("maccatalyst")]
	[SupportedOSPlatform ("macos")]
	[SupportedOSPlatform ("tvos")]
	public class CFType : NativeObject, ICFType {
		/// <param name="typeRef">Handle to a CoreFoundation object.</param>
		///         <summary>Returns the CoreFoundation type for the specified object.</summary>
		///         <returns>
		///         </returns>
		///         <remarks>
		///         </remarks>
		[DllImport (Constants.CoreFoundationLibrary, EntryPoint = "CFGetTypeID")]
		public static extern nint GetTypeID (IntPtr typeRef);

		[DllImport (Constants.CoreFoundationLibrary)]
		extern static IntPtr CFCopyDescription (IntPtr ptr);

		/// <summary>To be added.</summary>
		///         <remarks>To be added.</remarks>
		internal CFType ()
		{
		}

		[Preserve (Conditional = true)]
		internal CFType (NativeHandle handle, bool owns)
			: base (handle, owns)
		{
		}

		/// <param name="handle">Handle to the native CoreFoundation object.</param>
		///         <summary>Returns a textual representation of the specified object.</summary>
		///         <returns>
		///         </returns>
		///         <remarks>
		///         </remarks>
		public string? GetDescription (IntPtr handle)
		{
			if (handle == IntPtr.Zero)
				ObjCRuntime.ThrowHelper.ThrowArgumentNullException (nameof (handle));

			return CFString.FromHandle (CFCopyDescription (handle));
		}

		[DllImport (Constants.CoreFoundationLibrary)]
		extern static byte CFEqual (/*CFTypeRef*/ IntPtr cf1, /*CFTypeRef*/ IntPtr cf2);

		/// <param name="cf1">To be added.</param>
		///         <param name="cf2">To be added.</param>
		///         <summary>Compares two handles of native objects for equality.</summary>
		///         <returns>true if the types are the same.</returns>
		///         <remarks>
		///         </remarks>
		public static bool Equal (IntPtr cf1, IntPtr cf2)
		{
			// CFEqual is not happy (but crashy) when it receive null
			if (cf1 == IntPtr.Zero)
				return cf2 == IntPtr.Zero;
			else if (cf2 == IntPtr.Zero)
				return false;
			return CFEqual (cf1, cf2) != 0;
		}
	}

	public interface ICFType : INativeObject {
	}
}
