// 
// SecIdentity.cs: Implements the managed SecIdentity wrapper.
//
// Authors: 
//  Sebastien Pouliot  <sebastien@xamarin.com>
//
// Copyright 2013 Xamarin Inc.
//

#nullable enable

using CoreFoundation;

namespace Security {

	/// <summary>Encapsulate a security identity. A security identity comprises a certificate and its private key.</summary>
	///     <remarks>To be added.</remarks>
	public partial class SecIdentity {

		[DllImport (Constants.SecurityLibrary)]
		unsafe extern static SecStatusCode /* OSStatus */ SecIdentityCopyPrivateKey (IntPtr /* SecIdentityRef */ identity, IntPtr* /* SecKeyRef* */ privatekey);

		/// <summary>To be added.</summary>
		///         <value>To be added.</value>
		///         <remarks>To be added.</remarks>
		public SecKey PrivateKey {
			get {
				IntPtr p;
				SecStatusCode result;
				unsafe {
					result = SecIdentityCopyPrivateKey (Handle, &p);
				}
				if (result != SecStatusCode.Success)
					throw new InvalidOperationException (result.ToString ());
				return new SecKey (p, true);
			}
		}

		[SupportedOSPlatform ("ios26.0")]
		[SupportedOSPlatform ("maccatalyst26.0")]
		[SupportedOSPlatform ("macos26.0")]
		[SupportedOSPlatform ("tvos26.0")]
		[DllImport (Constants.SecurityLibrary)]
		extern static /* __nullable CF_RETURNS_RETAINED SecIdentityRef */ IntPtr SecIdentityCreate (
			IntPtr /* CFAllocatorRef __nullable */ allocator,
			IntPtr /* SecCertificateRef */ certificate,
			IntPtr /* SecKeyRef */ privateKey);

		/// <summary>Create a <see cref="SecIdentity" /> instance from a certificate and a private key.</summary>
		/// <param name="certificate">The certificate to use for the new <see cref="SecIdentity" /> instance.</param>
		/// <param name="privateKey">The private to use for the new <see cref="SecIdentity" /> instance.</param>
		/// <returns>A new <see cref="SecIdentity" /> instance if successful, otherwise <see langword="null" />.</returns>
		[SupportedOSPlatform ("ios26.0")]
		[SupportedOSPlatform ("maccatalyst26.0")]
		[SupportedOSPlatform ("macos26.0")]
		[SupportedOSPlatform ("tvos26.0")]
		public static SecIdentity? Create (SecCertificate certificate, SecKey privateKey)
		{
			var rv = SecIdentityCreate (IntPtr.Zero, certificate.GetNonNullHandle (nameof (certificate)), privateKey.GetNonNullHandle (nameof (privateKey)));
			GC.KeepAlive (certificate);
			GC.KeepAlive (privateKey);
			if (rv == IntPtr.Zero)
				return null;
			return new SecIdentity (rv, true /* CF_RETURNS_RETAINED */);
		}
	}
}
