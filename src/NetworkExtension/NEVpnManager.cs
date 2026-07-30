//
// NEVpnManager.cs
//
// Authors:
//	Alex Soto  <alexsoto@microsoft.com>
//
// Copyright 2017 Xamarin Inc. All rights reserved.
//

#nullable enable

#if MONOMAC
using Security;

namespace NetworkExtension {
	/// <summary>Manages and controls VPN configurations and connections.</summary>
	/// <related type="externalDocumentation" href="https://developer.apple.com/library/ios/documentation/NetworkExtension/Reference/NEVPNManagerClassRef/index.html">Apple documentation for <c>NEVPNManager</c></related>
	public partial class NEVpnManager {
		/// <summary>Sets the authorization for this VPN manager.</summary>
		/// <param name="authorization">The authorization object to set.</param>
		[SupportedOSPlatform ("macos")]
		[UnsupportedOSPlatform ("ios")]
		[UnsupportedOSPlatform ("maccatalyst")]
		[UnsupportedOSPlatform ("tvos")]
		public void SetAuthorization (Authorization authorization)
		{
			if (authorization is null)
				ObjCRuntime.ThrowHelper.ThrowArgumentNullException (nameof (authorization));

			_SetAuthorization (authorization.Handle);
			GC.KeepAlive (authorization);
		}
	}
}
#endif
