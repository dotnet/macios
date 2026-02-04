using System.Runtime.InteropServices;

static class CFunctions {
	// extern void Version_10 () __attribute__((availability(macos, introduced=10)));
	[Mac (10, 0)]
	[DllImport ("__Internal")]
	[Verify (PlatformInvoke)]
	static extern void Version_10 ();

	// extern void Version_10_5 () __attribute__((availability(macos, introduced=10.5)));
	[Mac (10, 5)]
	[DllImport ("__Internal")]
	[Verify (PlatformInvoke)]
	static extern void Version_10_5 ();

	// extern void Version_10_5_9 () __attribute__((availability(macos, introduced=10.5.9)));
	[Mac (10, 5, 9)]
	[DllImport ("__Internal")]
	[Verify (PlatformInvoke)]
	static extern void Version_10_5_9 ();
}
