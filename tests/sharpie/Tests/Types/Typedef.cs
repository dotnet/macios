using System;
using System.Runtime.InteropServices;

static class CFunctions {
	// extern int32_t _int32_t ();
	[DllImport ("__Internal")]
	[Verify (PlatformInvoke)]
	static extern int _int32_t ();

	// extern e _a_to_e ();
	[DllImport ("__Internal")]
	[Verify (PlatformInvoke)]
	static extern void _a_to_e ();

	// extern CGFloat _CGFloat ();
	[DllImport ("__Internal")]
	[Verify (PlatformInvoke)]
	static extern nfloat _CGFloat ();

	// extern BOOL _BOOL ();
	[DllImport ("__Internal")]
	[Verify (PlatformInvoke)]
	static extern bool _BOOL ();

	// extern GLboolean _GLboolean ();
	[DllImport ("__Internal")]
	[Verify (PlatformInvoke)]
	static extern bool _GLboolean ();

	// extern NSTimeInterval _NSTimeInterval ();
	[DllImport ("__Internal")]
	[Verify (PlatformInvoke)]
	static extern double _NSTimeInterval ();
}
