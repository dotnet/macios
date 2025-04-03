// Copyright 2013 Xamarin Inc.

using System;

using ObjCRuntime;
using Security;

// Disable until we get around to enable + fix any issues.
#nullable disable

namespace Foundation {

	public partial class NSUrlProtectionSpace {

		/// <param name="host">To be added.</param>
		///         <param name="port">To be added.</param>
		///         <param name="protocol">To be added.</param>
		///         <param name="realm">To be added.</param>
		///         <param name="authenticationMethod">To be added.</param>
		///         <summary>To be added.</summary>
		///         <remarks>To be added.</remarks>
		public NSUrlProtectionSpace (string host, int port, string protocol, string realm, string authenticationMethod)
			: base (NSObjectFlag.Empty)
		{
			Handle = Init (host, port, protocol, realm, authenticationMethod);
		}

		/// <param name="host">To be added.</param>
		///         <param name="port">To be added.</param>
		///         <param name="protocol">To be added.</param>
		///         <param name="realm">To be added.</param>
		///         <param name="authenticationMethod">To be added.</param>
		///         <param name="useProxy">To be added.</param>
		///         <summary>To be added.</summary>
		///         <remarks>To be added.</remarks>
		public NSUrlProtectionSpace (string host, int port, string protocol, string realm, string authenticationMethod, bool useProxy)
			: base (NSObjectFlag.Empty)
		{
			if (useProxy)
				Handle = InitWithProxy (host, port, protocol, realm, authenticationMethod);
			else
				Handle = Init (host, port, protocol, realm, authenticationMethod);
		}

		/// <summary>To be added.</summary>
		///         <value>To be added.</value>
		///         <remarks>To be added.</remarks>
		public SecTrust ServerSecTrust {
			get {
				IntPtr handle = ServerTrust;
				return (handle == IntPtr.Zero) ? null : new SecTrust (handle, false);
			}
		}
	}
}
