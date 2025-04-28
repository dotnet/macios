//
// MonoMac.CoreServices.CFHTTPAuthentication
//
// Authors:
//      Martin Baulig (martin.baulig@gmail.com)
//
// Copyright 2012-2014 Xamarin Inc. (http://www.xamarin.com)
//

#nullable enable

using System;
using System.Net;
using System.Runtime.InteropServices;
using System.Runtime.Versioning;
using Foundation;
using CoreFoundation;
using ObjCRuntime;

namespace CFNetwork {
	/// <summary>Represents HTTP authentication information for use with <see cref="T:CoreServices.CFHTTPMessage" />.</summary>
	///     <remarks>To be added.</remarks>
	[SupportedOSPlatform ("ios")]
	[SupportedOSPlatform ("maccatalyst")]
	[SupportedOSPlatform ("macos")]
	[SupportedOSPlatform ("tvos")]
	public class CFHTTPAuthentication : CFType {
		[Preserve (Conditional = true)]
		internal CFHTTPAuthentication (NativeHandle handle, bool owns)
			: base (handle, owns)
		{
		}

		[DllImport (Constants.CFNetworkLibrary, EntryPoint = "CFHTTPAuthenticationGetTypeID")]
		public extern static /* CFTypeID */ nint GetTypeID ();

		[DllImport (Constants.CFNetworkLibrary)]
		extern static /* CFHTTPAuthenticationRef */ IntPtr CFHTTPAuthenticationCreateFromResponse (/* CFAllocatorRef */ IntPtr alloc, /* CFHTTPMessageRef */ IntPtr response);

		public static CFHTTPAuthentication? CreateFromResponse (CFHTTPMessage response)
		{
			if (response is null)
				ObjCRuntime.ThrowHelper.ThrowArgumentNullException (nameof (response));

			if (response.IsRequest)
				throw new InvalidOperationException ();

			var handle = CFHTTPAuthenticationCreateFromResponse (IntPtr.Zero, response.Handle);
			GC.KeepAlive (response);
			if (handle == IntPtr.Zero)
				return null;

			return new CFHTTPAuthentication (handle, true);
		}

		[DllImport (Constants.CFNetworkLibrary)]
		extern static /* Boolean */ byte CFHTTPAuthenticationIsValid (/* CFHTTPAuthenticationRef */ IntPtr auth, /* CFStreamError* */ IntPtr error);

		public bool IsValid {
			get { return CFHTTPAuthenticationIsValid (Handle, IntPtr.Zero) != 0; }
		}

		[DllImport (Constants.CFNetworkLibrary)]
		extern static /* Boolean */ byte CFHTTPAuthenticationAppliesToRequest (/* CFHTTPAuthenticationRef */ IntPtr auth, /* CFHTTPMessageRef */ IntPtr request);

		public bool AppliesToRequest (CFHTTPMessage request)
		{
			if (request is null)
				ObjCRuntime.ThrowHelper.ThrowArgumentNullException (nameof (request));

			if (!request.IsRequest)
				throw new InvalidOperationException ();

			bool result = CFHTTPAuthenticationAppliesToRequest (Handle, request.Handle) != 0;
			GC.KeepAlive (request);
			return result;
		}

		[DllImport (Constants.CFNetworkLibrary)]
		extern static /* Boolean */ byte CFHTTPAuthenticationRequiresAccountDomain (/* CFHTTPAuthenticationRef */ IntPtr auth);

		public bool RequiresAccountDomain {
			get { return CFHTTPAuthenticationRequiresAccountDomain (Handle) != 0; }
		}

		[DllImport (Constants.CFNetworkLibrary)]
		extern static /* Boolean */ byte CFHTTPAuthenticationRequiresOrderedRequests (/* CFHTTPAuthenticationRef */ IntPtr auth);

		public bool RequiresOrderedRequests {
			get { return CFHTTPAuthenticationRequiresOrderedRequests (Handle) != 0; }
		}

		[DllImport (Constants.CFNetworkLibrary)]
		extern static /* Boolean */ byte CFHTTPAuthenticationRequiresUserNameAndPassword (/* CFHTTPAuthenticationRef */ IntPtr auth);

		public bool RequiresUserNameAndPassword {
			get { return CFHTTPAuthenticationRequiresUserNameAndPassword (Handle) != 0; }
		}

		[DllImport (Constants.CFNetworkLibrary)]
		extern static /* CFString */ IntPtr CFHTTPAuthenticationCopyMethod (/* CFHTTPAuthenticationRef */ IntPtr auth);

		public string? GetMethod ()
		{
			var ptr = CFHTTPAuthenticationCopyMethod (Handle);
			return CFString.FromHandle (ptr, true);
		}
	}
}
