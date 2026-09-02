using System;
using System.Runtime.InteropServices;

static class CFunctions {
	// extern ptrdiff_t _ptrdiff_t ();
	[DllImport ("__Internal")]
	[Verify (PlatformInvoke)]
	static extern IntPtr _ptrdiff_t ();

	// extern size_t _size_t ();
	[DllImport ("__Internal")]
	[Verify (PlatformInvoke)]
	static extern nuint _size_t ();

	// extern wchar_t _wchar_t ();
	[DllImport ("__Internal")]
	[Verify (PlatformInvoke)]
	static extern int _wchar_t ();
}
