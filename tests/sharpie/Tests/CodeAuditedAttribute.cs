using System.Runtime.InteropServices;

static class CFunctions {
	// extern void DoSomething () ;
	[DllImport ("__Internal")]
	[Verify (PlatformInvoke)]
	static extern void DoSomething ();
}
