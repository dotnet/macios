using System;
using Foundation;
using ObjCRuntime;

namespace NS {
	[Unavailable (PlatformName.iOS)]
	[Protocol]
	interface MyProtocol {
	}

	[BaseType (typeof (NSObject))]
	interface MyClass : MyProtocol {
	}
}
