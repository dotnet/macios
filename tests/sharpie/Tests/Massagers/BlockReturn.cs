using System;
using Foundation;

// @interface Blocks
interface Blocks {
	// -(Blocks *(^)(id))get_Func_NSObject_Blocks;
	[Export ("get_Func_NSObject_Blocks")]
	[Verify (MethodToProperty)]
	Func<NSObject, Blocks> Get_Func_NSObject_Blocks { get; }
}
