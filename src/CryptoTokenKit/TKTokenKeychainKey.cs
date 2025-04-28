using System;
using Foundation;
using ObjCRuntime;
using Security;

#nullable enable

namespace CryptoTokenKit {
	public partial class TKTokenKeychainKey {
		public TKTokenKeychainKey (SecCertificate? certificate, NSObject objectId)
			: this (certificate.GetHandle (), objectId)
		{
			GC.KeepAlive (certificate);
		}
	}
}
