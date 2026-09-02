//
// PublicPrivateKeyAuthentication.cs
//
// Authors:
//	TJ Lambert <antlambe@microsoft.com>
//
// Copyright 2021 Microsoft Corporation
//

#if !TVOS

using CoreGraphics;

#nullable enable

namespace AuthenticationServices {
	[SupportedOSPlatform ("macos")]
	[SupportedOSPlatform ("ios15.0")]
	[SupportedOSPlatform ("maccatalyst")]
	[UnsupportedOSPlatform ("tvos")]
	public static class PublicPrivateKeyAuthentication {
		[DllImport (Constants.AuthenticationServicesLibrary)]
		static extern /* NSString[] */ IntPtr ASAuthorizationAllSupportedPublicKeyCredentialDescriptorTransports ();

		public static ASAuthorizationSecurityKeyPublicKeyCredentialDescriptorTransport []? GetAllSupportedPublicKeyCredentialDescriptorTransports ()
		{
			var nsStringArray = NSArray.StringArrayFromHandle (ASAuthorizationAllSupportedPublicKeyCredentialDescriptorTransports ());

			if (nsStringArray is null)
				return null;

			var asArray = new ASAuthorizationSecurityKeyPublicKeyCredentialDescriptorTransport [nsStringArray.Length];
			for (var i = 0; i < nsStringArray.Length; i++) {
				switch (nsStringArray [i]) {
				case "usb":
					asArray [i] = ASAuthorizationSecurityKeyPublicKeyCredentialDescriptorTransport.Usb;
					break;
				case "nfc":
					asArray [i] = ASAuthorizationSecurityKeyPublicKeyCredentialDescriptorTransport.Nfc;
					break;
				case "ble":
					asArray [i] = ASAuthorizationSecurityKeyPublicKeyCredentialDescriptorTransport.Bluetooth;
					break;
				default:
					break;
				}
			}
			return asArray;
		}
	}
}

#endif
