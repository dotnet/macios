using System;

using Foundation;

namespace MaxSupportedOSPlatformVersion {
	public class Program {
		static int Main (string [] args)
		{
			GC.KeepAlive (typeof (NSObject)); // prevent linking away the platform assembly

			return 0;
		}
	}
}
