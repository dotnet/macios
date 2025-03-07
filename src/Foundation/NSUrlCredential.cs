// Copyright 2013 Xamarin Inc.

using System;
using System.Reflection;
using System.Collections;
using System.Runtime.InteropServices;

using ObjCRuntime;
using Security;

// Disable until we get around to enable + fix any issues.
#nullable disable

namespace Foundation {

	public partial class NSUrlCredential {
#pragma warning disable RBI0014
		public NSUrlCredential (SecIdentity identity, SecCertificate [] certificates, NSUrlCredentialPersistence persistence)
			: this (identity.Handle, NSArray.FromNativeObjects (certificates).Handle, persistence)
		{
			GC.KeepAlive (identity);
			// FIXME: what about NSArray.FromNativeObjects (certificates)?
		}
#pragma warning restore RBI0014

		public static NSUrlCredential FromIdentityCertificatesPersistance (SecIdentity identity, SecCertificate [] certificates, NSUrlCredentialPersistence persistence)
		{
			if (identity is null)
				throw new ArgumentNullException ("identity");

			if (certificates is null)
				throw new ArgumentNullException ("certificates");

			using (var certs = NSArray.FromNativeObjects (certificates)) {
				NSUrlCredential result = FromIdentityCertificatesPersistanceInternal (identity.Handle, certs.Handle, persistence);
				GC.KeepAlive (identity);
				return result;
			}
		}

		public SecIdentity SecIdentity {
			get {
				IntPtr handle = Identity;
				return (handle == IntPtr.Zero) ? null : new SecIdentity (handle, false);
			}
		}

		public static NSUrlCredential FromTrust (SecTrust trust)
		{
			if (trust is null)
				throw new ArgumentNullException ("trust");

			NSUrlCredential result = FromTrust (trust.Handle);
			GC.KeepAlive (trust);
			return result;
		}
	}
}
