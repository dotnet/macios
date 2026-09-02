using Foundation;
using ObjCRuntime;

// @interface UnsafeMassagerTest
interface UnsafeMassagerTest {
	// -(int *)returnPointerMethod;
	[Export ("returnPointerMethod")]
	[Verify (MethodToProperty)]
	unsafe int* ReturnPointerMethod { get; }

	// -(void)acceptPointerMethod:(int *)ptr;
	[Export ("acceptPointerMethod:")]
	unsafe void AcceptPointerMethod (int* ptr);

	// @property (readonly) int * readonlyPtrProperty;
	[Export ("readonlyPtrProperty")]
	unsafe int* ReadonlyPtrProperty { get; }

	// @property int * readWritePtrProperty;
	[Export ("readWritePtrProperty", ArgumentSemantic.Assign)]
	unsafe int* ReadWritePtrProperty { get; set; }
}
