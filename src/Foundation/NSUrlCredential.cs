// Copyright 2013 Xamarin Inc.

using System.Reflection;
using System.Collections;

using Security;

#nullable enable

namespace Foundation {

	public partial class NSUrlCredential {
		/// <summary>Creates a new <see cref="NSUrlCredential" /> from an identity (digital certificate + private key) and a list of certificates.</summary>
		/// <param name="identity">The identity to use for the credential.</param>
		/// <param name="certificates">The certificates to use for the credential.</param>
		/// <param name="persistence">Specifies how long the credential should be kept.</param>
		public NSUrlCredential (SecIdentity identity, SecCertificate [] certificates, NSUrlCredentialPersistence persistence)
			: base (NSObjectFlag.Empty)
		{
			ArgumentNullException.ThrowIfNull (identity);

			using (var certs = NSArray.FromNativeObjects (certificates)) {
				InitializeHandle (_InitWithIdentity (identity.Handle, certs.Handle, persistence));
				GC.KeepAlive (identity);
			}
		}

		/// <summary>Creates a new <see cref="NSUrlCredential" /> from an identity (digital certificate + private key) and a list of certificates.</summary>
		/// <param name="identity">The identity to use for the credential.</param>
		/// <param name="certificates">The certificates to use for the credential.</param>
		/// <param name="persistence">Specifies how long the credential should be kept.</param>
		/// <returns>A new <see cref="NSUrlCredential" /> instance.</returns>
		public static NSUrlCredential FromIdentityCertificatesPersistance (SecIdentity identity, SecCertificate [] certificates, NSUrlCredentialPersistence persistence)
		{
			ArgumentNullException.ThrowIfNull (identity);
			ArgumentNullException.ThrowIfNull (certificates);

			using (var certs = NSArray.FromNativeObjects (certificates)) {
				NSUrlCredential result = FromIdentityCertificatesPersistanceInternal (identity.Handle, certs.Handle, persistence);
				GC.KeepAlive (identity);
				return result;
			}
		}

		/// <summary>Gets the identity (digital certificate + private key) associated with this credential.</summary>
		/// <value>A <see cref="SecIdentity" /> object representing the identity, or <see langword="null" /> if no identity is available.</value>
		public SecIdentity? SecIdentity {
			get {
				IntPtr handle = Identity;
				return (handle == IntPtr.Zero) ? null : new SecIdentity (handle, false);
			}
		}

		/// <summary>Creates a new <see cref="NSUrlCredential" /> from a server trust.</summary>
		/// <param name="trust">The server trust to use for the credential.</param>
		/// <returns>A new <see cref="NSUrlCredential" /> instance.</returns>
		public static NSUrlCredential FromTrust (SecTrust trust)
		{
			ArgumentNullException.ThrowIfNull (trust);

			NSUrlCredential result = FromTrust (trust.Handle);
			GC.KeepAlive (trust);
			return result;
		}
	}
}
