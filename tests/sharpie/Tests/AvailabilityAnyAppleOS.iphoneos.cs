using System.Runtime.InteropServices;
using Foundation;
using ObjCRuntime;

static class CFunctions {
	// extern int AnyAppleOSFunction () __attribute__((availability(anyAppleOS, introduced=27.0)));
	[Introduced (PlatformName.VisionOS, 27, 0)]
	[Watch (27, 0), TV (27, 0), MacCatalyst (27, 0), Mac (27, 0), iOS (27, 0)]
	[DllImport ("__Internal")]
	[Verify (PlatformInvoke)]
	static extern int AnyAppleOSFunction ();
}

// @interface AnyAppleOSIntroduced
[Introduced (PlatformName.VisionOS, 27, 0)]
[Watch (27, 0), TV (27, 0), MacCatalyst (27, 0), Mac (27, 0), iOS (27, 0)]
interface AnyAppleOSIntroduced {
	// -(void)introducedMethod __attribute__((availability(anyAppleOS, introduced=27.0)));
	[Introduced (PlatformName.VisionOS, 27, 0)]
	[Watch (27, 0), TV (27, 0), MacCatalyst (27, 0), Mac (27, 0), iOS (27, 0)]
	[Export ("introducedMethod")]
	void IntroducedMethod ();
}

// @interface AnyAppleOSUnavailable
[Unavailable (PlatformName.VisionOS)]
[NoWatch, NoTV, NoMacCatalyst, NoMac, NoiOS]
interface AnyAppleOSUnavailable {
}

