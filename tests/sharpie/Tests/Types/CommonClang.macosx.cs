using System;
using Foundation;

// @interface CommonClang
interface CommonClang {
	// +(instancetype)_instancetype;
	[Static]
	[Export ("_instancetype")]
	CommonClang _instancetype ();

	// -(BOOL)_BOOL;
	[Export ("_BOOL")]
	[Verify (MethodToProperty)]
	bool _BOOL { get; }

	// -(intptr_t)_intptr_t;
	[Export ("_intptr_t")]
	[Verify (MethodToProperty)]
	IntPtr _intptr_t { get; }

	// -(uintptr_t)_uintptr_t;
	[Export ("_uintptr_t")]
	[Verify (MethodToProperty)]
	UIntPtr _uintptr_t { get; }

	// -(NSString *)_NSString_ptr;
	[Export ("_NSString_ptr")]
	[Verify (MethodToProperty)]
	string _NSString_ptr { get; }
}
