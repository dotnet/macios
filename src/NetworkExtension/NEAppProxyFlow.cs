using System;
using System.Runtime.Versioning;
using Foundation;
using ObjCRuntime;

namespace NetworkExtension {

	public unsafe partial class NEAppProxyFlow : NSObject {


#if MACCATALYST || MACCORE
		[UnsupportedOSPlatform ("tvos")]
		[UnsupportedOSPlatform ("ios")]
		[SupportedOSPlatform ("maccatalyst")]
		[SupportedOSPlatform ("macos")]
		public void SetMetadata (NWParameters parameters)
		{
			SetMetadata ((IntPtr) parameters.GetHandle ());
            GC.KeepAlive (parameters);
		}
#endif

	}
}
