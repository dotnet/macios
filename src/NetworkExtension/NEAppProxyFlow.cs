#if __MACCATALYST__ || MONOMAC

using System;
using System.Runtime.Versioning;
using Foundation;
using Network;
using ObjCRuntime;

namespace NetworkExtension {

	public unsafe partial class NEAppProxyFlow {

		[UnsupportedOSPlatform ("tvos")]
		[UnsupportedOSPlatform ("ios")]
		[SupportedOSPlatform ("maccatalyst")]
		[SupportedOSPlatform ("macos")]
		public void SetMetadata (NWParameters parameters)
		{
			SetMetadata ((IntPtr) parameters.GetHandle ());
			GC.KeepAlive (parameters);
		}
	}
}
#endif // __MACCATALYST__ || MONOMAC
