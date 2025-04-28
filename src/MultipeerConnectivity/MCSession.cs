//
// MCSession Helpers
//
// Authors:
//	Sebastien Pouliot  <sebastien@xamarin.com>
//
// Copyright 2013-2014 Xamarin Inc.
//

#nullable enable

using System;
using System.Collections.Generic;

using Foundation;
using ObjCRuntime;
using Security;

namespace MultipeerConnectivity {

	public partial class MCSession {

		/// <summary>Create a new instance of an MCSession using a specific identity.</summary>
		/// <param name="myPeerID">An id for the local device.</param>
		/// <param name="identity">An identity that can be used to identify the local peer.</param>
		/// <param name="encryptionPreference">The encryption preference of the new <see cref="MCSession" /> instance.</param>
		public MCSession (MCPeerID myPeerID, SecIdentity identity, MCEncryptionPreference encryptionPreference)
			: base (NSObjectFlag.Empty)
		{
			if (identity is null) {
				InitializeHandle (_Init (myPeerID, null, encryptionPreference));
			} else {
				using (var a = NSArray.FromNSObjects (identity))
					InitializeHandle (_Init (myPeerID, a, encryptionPreference));
			}
		}

		/// <summary>Create a new instance of an MCSession using a specific identity and intermediate certificates.</summary>
		/// <param name="myPeerID">An id for the local device.</param>
		/// <param name="identity">An identity that can be used to identify the local peer.</param>
		/// <param name="certificates">Any additional intermediate certificates that might be required to verify the <paramref name="identity" />.</param>
		/// <param name="encryptionPreference">The encryption preference of the new <see cref="MCSession" /> instance.</param>
		public MCSession (MCPeerID myPeerID, SecIdentity identity, SecCertificate [] certificates, MCEncryptionPreference encryptionPreference)
			: base (NSObjectFlag.Empty)
		{
			if (identity is null) {
				if (certificates is null)
					InitializeHandle (_Init (myPeerID, null, encryptionPreference));
				else
					ObjCRuntime.ThrowHelper.ThrowArgumentNullException (nameof (identity));
			} else {
				var array = new INativeObject [certificates.Length + 1];
				array [0] = identity;
				Array.Copy (certificates, 0, array, 1, certificates.Length);
				using var certs = NSArray.FromNativeObjects (array);
				InitializeHandle (_Init (myPeerID, certs, encryptionPreference));
			}
		}
	}
}
