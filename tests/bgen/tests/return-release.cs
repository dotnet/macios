using System;
using Foundation;

namespace NS {
	[Protocol]
	interface ProtocolWithReturnRelease {
		[Abstract, Export ("newRequiredObject")]
		[return: Release]
		NSObject CreateRequiredObject ();

		[Export ("newOptionalObject")]
		[return: Release]
		NSObject CreateOptionalObject ();

		[Abstract, Export ("newRequiredProperty")]
		NSObject RequiredProperty { [return: Release] get; set; }

		[Export ("newOptionalProperty")]
		NSObject OptionalProperty { [return: Release] get; }
	}
}
