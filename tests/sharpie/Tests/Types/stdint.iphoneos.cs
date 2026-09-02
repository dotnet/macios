using System;
using System.Runtime.InteropServices;

static class CFunctions {
	// extern int8_t _int8_t ();
	[DllImport ("__Internal")]
	[Verify (PlatformInvoke)]
	static extern sbyte _int8_t ();

	// extern uint8_t _uint8_t ();
	[DllImport ("__Internal")]
	[Verify (PlatformInvoke)]
	static extern byte _uint8_t ();

	// extern int16_t _int16_t ();
	[DllImport ("__Internal")]
	[Verify (PlatformInvoke)]
	static extern short _int16_t ();

	// extern uint16_t _uint16_t ();
	[DllImport ("__Internal")]
	[Verify (PlatformInvoke)]
	static extern ushort _uint16_t ();

	// extern int32_t _int32_t ();
	[DllImport ("__Internal")]
	[Verify (PlatformInvoke)]
	static extern int _int32_t ();

	// extern uint32_t _uint32_t ();
	[DllImport ("__Internal")]
	[Verify (PlatformInvoke)]
	static extern uint _uint32_t ();

	// extern int64_t _int64_t ();
	[DllImport ("__Internal")]
	[Verify (PlatformInvoke)]
	static extern long _int64_t ();

	// extern uint64_t _uint64_t ();
	[DllImport ("__Internal")]
	[Verify (PlatformInvoke)]
	static extern ulong _uint64_t ();

	// extern intptr_t _intptr_t ();
	[DllImport ("__Internal")]
	[Verify (PlatformInvoke)]
	static extern IntPtr _intptr_t ();

	// extern uintptr_t _uintptr_t ();
	[DllImport ("__Internal")]
	[Verify (PlatformInvoke)]
	static extern UIntPtr _uintptr_t ();
}
