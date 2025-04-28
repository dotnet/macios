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
		/// <param name="identity">Identity to use for the credential.</param>
		///         <param name="certificates">Certificates.</param>
		///         <param name="persistence">Specifies how long the credential should be kept.</param>
		///         <summary>Creates an NSUrlCredential from an identity (digital certificate + private key) and a list of certificates. </summary>
		///         <remarks>
		///         </remarks>
		public NSUrlCredential (SecIdentity identity, SecCertificate [] certificates, NSUrlCredentialPersistence persistence)
			: base (NSObjectFlag.Empty)
		{
			if (identity is null)
				throw new ArgumentNullException ("identity");

			using (var certs = NSArray.FromNativeObjects (certificates)) {
				InitializeHandle (_InitWithIdentity (identity.Handle, certs.Handle, persistence));
				GC.KeepAlive (identity);
			}
		}

		/// <param name="identity">Identity to use for the credential.</param>
		///         <param name="certificates">Certificates for the credential.</param>
		///         <param name="persistence">Specifies how long the credential should be kept.</param>
		///         <summary>Creates an NSUrlCredential from an identity (digital certificate + private key) and a list of certificates. </summary>
		///         <returns>
		///         </returns>
		///         <remarks>
		///         </remarks>
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

		/// <summary>To be added.</summary>
		///         <value>To be added.</value>
		///         <remarks>To be added.</remarks>
		public SecIdentity SecIdentity {
			get {
				IntPtr handle = Identity;
				return (handle == IntPtr.Zero) ? null : new SecIdentity (handle, false);
			}
		}

		/// <param name="trust">To be added.</param>
		///         <summary>To be added.</summary>
		///         <returns>To be added.</returns>
		///         <remarks>To be added.</remarks>
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
