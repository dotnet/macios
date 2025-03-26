using System;
using System.Runtime.Versioning;
using Foundation;
using Security;

#nullable enable
namespace AuthenticationServices {

	public unsafe partial class ASAuthorizationProviderExtensionLoginManager : NSObject {

#if MONOMAC
		public void Save (SecCertificate certificate, ASAuthorizationProviderExtensionKeyType keyType)
		{
			_Save (certificate.Handle, keyType);
			GC.KeepAlive (certificate);
		}
#endif
	}
}
