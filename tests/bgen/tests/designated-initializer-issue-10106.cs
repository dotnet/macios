using System;
using Foundation;

namespace NativeLibrary {
	using ObjCRuntime;

	[BaseType (typeof (NSObject))]
	[DisableDefaultCtor]
	interface SomeClass {
		[Export ("initWithSomething:")]
		[DesignatedInitializer]
		IntPtr Constructor (string something);
	}
}
