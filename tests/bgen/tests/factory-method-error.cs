using System;
using Foundation;
using ObjCRuntime;

namespace FactoryMethodErrorTest {
	[BaseType (typeof (NSObject))]
	interface FailableWidget {
		// A failable initializer (it has an 'out NSError' parameter), but the return value
		// isn't marked nullable, so bgen should warn (BI1125).
		[Export ("initWithName:error:")]
		[FactoryMethod]
		NativeHandle Constructor (string name, out NSError error);
	}
}
