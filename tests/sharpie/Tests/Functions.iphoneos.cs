using System;
using System.Runtime.InteropServices;
using Foundation;

static class CFunctions {
	// extern void Action ();
	[DllImport ("__Internal")]
	[Verify (PlatformInvoke)]
	static extern void Action ();

	// extern void ActionTakingInt (int i);
	[DllImport ("__Internal")]
	[Verify (PlatformInvoke)]
	static extern void ActionTakingInt (int i);

	// extern void ActionTakingIntAndCString (int i, const char *str);
	[DllImport ("__Internal")]
	[Verify (PlatformInvoke)]
	static extern unsafe void ActionTakingIntAndCString (int i, sbyte* str);

	// extern char * FuncTakingInt (int i);
	[DllImport ("__Internal")]
	[Verify (PlatformInvoke)]
	static extern unsafe sbyte* FuncTakingInt (int i);

	// extern int __snprintf (char *restrict str, size_t size, const char *restrict format, ...);
	[DllImport ("__Internal")]
	[Verify (PlatformInvoke)]
	static extern unsafe int __snprintf (sbyte* str, nuint size, sbyte* format, IntPtr varArgs);
}

// @interface ObjCMethods
interface ObjCMethods {
	// -(void)action;
	[Export ("action")]
	void Action ();

	// -(void)actionTakingInt:(int)i;
	[Export ("actionTakingInt:")]
	void ActionTakingInt (int i);

	// -(void)actionTakingInt:(int)i andCString:(const char *)str;
	[Export ("actionTakingInt:andCString:")]
	unsafe void ActionTakingInt (int i, sbyte* str);

	// -(char *)funcTakingInt:(int)i;
	[Export ("funcTakingInt:")]
	unsafe sbyte* FuncTakingInt (int i);

	// +(int)snprintf:(char *restrict)str ofSize:(size_t)size withFormat:(const char *restrict)format, ...;
	[Static, Internal]
	[Export ("snprintf:ofSize:withFormat:", IsVariadic = true)]
	unsafe int Snprintf (sbyte* str, nuint size, sbyte* format, IntPtr varArgs);
}
