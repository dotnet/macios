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
using Security;

namespace MultipeerConnectivity {

	public partial class MCSession {

		/// <param name="myPeerID">To be added.</param>
		///         <param name="identity">To be added.</param>
		///         <param name="encryptionPreference">To be added.</param>
		///         <summary>Constructor that allows the application developer to specify security constraints.</summary>
		///         <remarks>To be added.</remarks>
		public MCSession (MCPeerID myPeerID, SecIdentity identity, MCEncryptionPreference encryptionPreference)
			: base (NSObjectFlag.Empty)
		{
			if (identity is null) {
				Handle = Init (myPeerID, null, encryptionPreference);
			} else {
				using (var a = NSArray.FromNSObjects (identity))
					Handle = Init (myPeerID, a, encryptionPreference);
			}
		}

		/// <param name="myPeerID">To be added.</param>
		///         <param name="identity">To be added.</param>
		///         <param name="certificates">To be added.</param>
		///         <param name="encryptionPreference">To be added.</param>
		///         <summary>Constructor that allows the application developer to specify security constraints and certificates.</summary>
		///         <remarks>To be added.</remarks>
		public MCSession (MCPeerID myPeerID, SecIdentity identity, SecCertificate [] certificates, MCEncryptionPreference encryptionPreference)
			: base (NSObjectFlag.Empty)
		{
			if (identity is null) {
				if (certificates is null)
					Handle = Init (myPeerID, null, encryptionPreference);
				else
					ObjCRuntime.ThrowHelper.ThrowArgumentNullException (nameof (identity));
			} else {
				using (var certs = NSArray.FromNativeObjects (certificates))
					Handle = Init (myPeerID, certs, encryptionPreference);
			}
		}
	}
}
