using System;
using Foundation;
using ObjCRuntime;

namespace NS {
	[iOS (14, 0), Mac (15, 0), MacCatalyst (16, 0), TV (17, 0)]
	interface EverywhereShort {
	}

	[NoiOS, NoMac, NoMacCatalyst, NoTV]
	interface NowhereShort {
	}

	[Introduced (PlatformName.iOS, 15, 0)]
	[Introduced (PlatformName.MacOSX, 16, 0)]
	[Introduced (PlatformName.MacCatalyst, 17, 0)]
	[Introduced (PlatformName.TvOS, 18, 0)]
	interface EverywhereLong {
	}

	[Obsoleted (PlatformName.iOS, 15, 0, message: "So long")]
	[Obsoleted (PlatformName.MacOSX, 16, 0, message: "So long")]
	[Obsoleted (PlatformName.MacCatalyst, 17, 0, message: "So long")]
	[Obsoleted (PlatformName.TvOS, 18, 0, message: "So long")]
	interface GoodbyeLong {
	}

	[Deprecated (PlatformName.iOS, 15, 0, message: "And thanks for all the fish!")]
	[Deprecated (PlatformName.MacOSX, 16, 0, message: "And thanks for all the fish!")]
	[Deprecated (PlatformName.MacCatalyst, 17, 0, message: "And thanks for all the fish!")]
	[Deprecated (PlatformName.TvOS, 18, 0, message: "And thanks for all the fish!")]
	interface BegoneLong {
	}

	[Unavailable (PlatformName.iOS)]
	[Unavailable (PlatformName.MacOSX)]
	[Unavailable (PlatformName.MacCatalyst)]
	[Unavailable (PlatformName.TvOS)]
	interface NowhereLong {
	}
}
