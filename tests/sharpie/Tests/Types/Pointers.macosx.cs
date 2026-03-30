using System.Runtime.InteropServices;
using Foundation;

static class CFunctions {
	// extern void * _void_ptr ();
	[DllImport ("__Internal")]
	[Verify (PlatformInvoke)]
	static extern unsafe void* _void_ptr ();

	// extern int * _int_ptr ();
	[DllImport ("__Internal")]
	[Verify (PlatformInvoke)]
	static extern unsafe int* _int_ptr ();

	// extern int ** _int_ptr_ptr ();
	[DllImport ("__Internal")]
	[Verify (PlatformInvoke)]
	static extern unsafe int** _int_ptr_ptr ();

	// extern int *** _int_ptr_ptr_ptr ();
	[DllImport ("__Internal")]
	[Verify (PlatformInvoke)]
	static extern unsafe int*** _int_ptr_ptr_ptr ();
}

// @interface ObjCObjectOutPointers
interface ObjCObjectOutPointers {
	// -(void)getError:(NSError **)error;
	[Export ("getError:")]
	void GetError (out NSError error);
}
