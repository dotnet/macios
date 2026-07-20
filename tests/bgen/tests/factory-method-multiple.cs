using System;
using Foundation;
using ObjCRuntime;

namespace FactoryMethodMultipleTest {
	// A native class with two initializers that share the same managed signature. They
	// can't both be bound as constructors (C# doesn't allow two constructors with identical
	// parameter types), so they're bound as named factory methods instead.
	[BaseType (typeof (NSObject))]
	interface MultiWidget {
		[Export ("initWithFoo:")]
		[FactoryMethod]
		[return: NullAllowed]
		NativeHandle CreateWithFoo (nint foo);

		[Export ("initWithBar:")]
		[FactoryMethod]
		[return: NullAllowed]
		NativeHandle CreateWithBar (nint bar);
	}
}
