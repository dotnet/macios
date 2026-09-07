using System;
using Foundation;
using ObjCRuntime;

namespace FactoryMethodNonInitTest {
	// [FactoryMethod] requires an Objective-C 'init' selector. Using it with a non-init
	// selector must produce an error (BI1126).
	[BaseType (typeof (NSObject))]
	interface BadWidget {
		[Export ("createWithFoo:")]
		[FactoryMethod]
		[return: NullAllowed]
		NativeHandle CreateWithFoo (nint foo);
	}
}
