using System.Runtime.InteropServices;
using Foundation;

static class CFunctions {
	// extern int AnyAppleOSFunction () __attribute__((availability(anyAppleOS, introduced=27.0)));
	[TV (27, 0), MacCatalyst (27, 0), Mac (27, 0), iOS (27, 0)]
	[DllImport ("__Internal")]
	[Verify (PlatformInvoke)]
	static extern int AnyAppleOSFunction ();
}

// @interface AnyAppleOSIntroduced
[TV (27, 0), MacCatalyst (27, 0), Mac (27, 0), iOS (27, 0)]
interface AnyAppleOSIntroduced {
	// -(void)introducedMethod __attribute__((availability(anyAppleOS, introduced=27.0)));
	[TV (27, 0), MacCatalyst (27, 0), Mac (27, 0), iOS (27, 0)]
	[Export ("introducedMethod")]
	void IntroducedMethod ();
}

