using System;
using Foundation;
using ObjCRuntime;

namespace FactoryMethodInternalTest {
	[BaseType (typeof (NSObject))]
	interface InternalFactoryWidget {
		[Export ("initWithName:")]
		[FactoryMethod]
		[Internal]
		[return: NullAllowed]
		NativeHandle Constructor (string name);
	}
}
