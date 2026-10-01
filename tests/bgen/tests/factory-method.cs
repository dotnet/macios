using System;
using Foundation;
using ObjCRuntime;

namespace FactoryMethodTest {
	[BaseType (typeof (NSObject))]
	interface FactoryWidget {
		// Default factory method name ("Create"), nullable (failable initializer).
		[Export ("initWithName:")]
		[FactoryMethod]
		[return: NullAllowed]
		NativeHandle Constructor (string name);

		// Explicit factory method name, nullable (failable initializer).
		[Export ("initWithName:count:")]
		[FactoryMethod ("CreateWithCount")]
		[return: NullAllowed]
		NativeHandle Constructor (string name, nint count);

		// Non-nullable factory method: the initializer never fails, so the factory
		// method returns a non-nullable value and doesn't check for a nil result.
		[Export ("initWithColor:")]
		[FactoryMethod ("CreateWithColor")]
		NativeHandle Constructor (nint color);
	}
}
