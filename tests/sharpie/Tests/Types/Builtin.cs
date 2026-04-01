using System;
using System.Runtime.InteropServices;

static class CFunctions {
	// extern void _void ();
	[DllImport ("__Internal")]
	[Verify (PlatformInvoke)]
	static extern void _void ();

	// extern _Bool _bool ();
	[DllImport ("__Internal")]
	[Verify (PlatformInvoke)]
	static extern bool _bool ();

	// extern char _char ();
	[DllImport ("__Internal")]
	[Verify (PlatformInvoke)]
	static extern sbyte _char ();

	// extern unsigned char _unsigned_char ();
	[DllImport ("__Internal")]
	[Verify (PlatformInvoke)]
	static extern byte _unsigned_char ();

	// extern short _short ();
	[DllImport ("__Internal")]
	[Verify (PlatformInvoke)]
	static extern short _short ();

	// extern unsigned short _unsigned_short ();
	[DllImport ("__Internal")]
	[Verify (PlatformInvoke)]
	static extern ushort _unsigned_short ();

	// extern int _int ();
	[DllImport ("__Internal")]
	[Verify (PlatformInvoke)]
	static extern int _int ();

	// extern unsigned int _unsigned_int ();
	[DllImport ("__Internal")]
	[Verify (PlatformInvoke)]
	static extern uint _unsigned_int ();

	// extern long _long ();
	[DllImport ("__Internal")]
	[Verify (PlatformInvoke)]
	static extern nint _long ();

	// extern unsigned long _unsigned_long ();
	[DllImport ("__Internal")]
	[Verify (PlatformInvoke)]
	static extern nuint _unsigned_long ();

	// extern long long _long_long ();
	[DllImport ("__Internal")]
	[Verify (PlatformInvoke)]
	static extern long _long_long ();

	// extern unsigned long long _unsigned_long_long ();
	[DllImport ("__Internal")]
	[Verify (PlatformInvoke)]
	static extern ulong _unsigned_long_long ();

	// extern float _float ();
	[DllImport ("__Internal")]
	[Verify (PlatformInvoke)]
	static extern float _float ();

	// extern double _double ();
	[DllImport ("__Internal")]
	[Verify (PlatformInvoke)]
	static extern double _double ();

	// extern long double _long_double ();
	[DllImport ("__Internal")]
	[Verify (PlatformInvoke)]
	static extern decimal _long_double ();
}
