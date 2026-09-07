using System;
using Foundation;
using ObjCRuntime;

namespace FactoryMethodNamedError {
	// [FactoryMethod] on a non-constructor method must not specify a method name (the
	// binding method name is used instead). Doing so produces an error (BI1127).
	[BaseType (typeof (NSObject))]
	interface BadWidget {
		[Export ("initWithFoo:")]
		[FactoryMethod ("SomethingElse")]
		[return: NullAllowed]
		NativeHandle CreateWithFoo (nint foo);
	}
}
