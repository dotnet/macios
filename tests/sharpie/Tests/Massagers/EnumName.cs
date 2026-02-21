using System.Runtime.InteropServices;
using ObjCRuntime;

[Native]
public enum NintEnum : long {
	One = 1,
	Two,
	Three
}

static class CFunctions {
	// extern NINT_ENUM AddOneToNintEnum (NINT_ENUM in);
	[DllImport ("__Internal")]
	[Verify (PlatformInvoke)]
	static extern NintEnum AddOneToNintEnum (NintEnum @in);
}

[Native]
public enum POPAnimationClampFlags : ulong {
	None = 0,
	Start = 0uL << 1,
	End = 1uL << 1,
	Both = End | Start
}
