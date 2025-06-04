// Copyright 2013 Xamarin Inc.

using System;

using ObjCRuntime;
using Security;

#nullable enable

namespace Foundation {

	public partial class NSUrlProtectionSpace {
		/// <summary>Create a new <see cref="NSUrlProtectionSpace" /> instance.</summary>
		/// <param name="host">The host for the new <see cref="NSUrlProtectionSpace" /> instance.</param>
		/// <param name="port">The port for the new <see cref="NSUrlProtectionSpace" /> instance.</param>
		/// <param name="protocol">The protocol for the new <see cref="NSUrlProtectionSpace" /> instance.</param>
		/// <param name="realm">The realm for the new <see cref="NSUrlProtectionSpace" /> instance.</param>
		/// <param name="authenticationMethod">The authentication method for the new <see cref="NSUrlProtectionSpace" /> instance.</param>
		/// <remarks>This overload does not support proxies.</remarks>
		public NSUrlProtectionSpace (string host, int port, string? protocol, string? realm, string? authenticationMethod)
			: this (host, port, protocol, realm, authenticationMethod, false)
		{
		}

		/// <summary>Create a new <see cref="NSUrlProtectionSpace" /> instance.</summary>
		/// <param name="host">The host for the new <see cref="NSUrlProtectionSpace" /> instance.</param>
		/// <param name="port">The port for the new <see cref="NSUrlProtectionSpace" /> instance.</param>
		/// <param name="protocol">The protocol for the new <see cref="NSUrlProtectionSpace" /> instance.</param>
		/// <param name="realm">The realm for the new <see cref="NSUrlProtectionSpace" /> instance.</param>
		/// <param name="authenticationMethod">The authentication method for the new <see cref="NSUrlProtectionSpace" /> instance.</param>
		/// <param name="useProxy">Whether a proxy is used or not.</param>
		public NSUrlProtectionSpace (string host, int port, string? protocol, string? realm, string? authenticationMethod, bool useProxy)
			: base (NSObjectFlag.Empty)
		{
			if (useProxy)
				InitializeHandle (_InitWithProxy (host, port, protocol, realm, authenticationMethod), "initWithProxyHost:port:type:realm:authenticationMethod:");
			else
				InitializeHandle (_Init (host, port, protocol, realm, authenticationMethod), "initWithHost:port:protocol:realm:authenticationMethod:");
		}

		// Disable until we get around to enable + fix any issues.
#nullable disable
		/// <summary>To be added.</summary>
		///         <value>To be added.</value>
		///         <remarks>To be added.</remarks>
		public SecTrust ServerSecTrust {
			get {
				IntPtr handle = ServerTrust;
				return (handle == IntPtr.Zero) ? null : new SecTrust (handle, false);
			}
		}
#nullable restore
	}
}
