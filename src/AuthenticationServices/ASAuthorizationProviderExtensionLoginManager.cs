#if MONOMAC

using System;
using System.Runtime.Versioning;
using Foundation;
using Security;

#nullable enable
namespace AuthenticationServices {

	public unsafe partial class ASAuthorizationProviderExtensionLoginManager {

		public void Save (SecCertificate certificate, ASAuthorizationProviderExtensionKeyType keyType)
		{
			_Save (certificate.Handle, keyType);
			GC.KeepAlive (certificate);
		}
	}
}
#endif // #if MONOMAC
