using System;
using Foundation;

[Static]
[Verify (ConstantsInterfaceAssociation)]
partial interface Constants {
	// extern NSInteger NSIntegerType;
	[Field ("NSIntegerType", "__Internal")]
	nint NSIntegerType { get; }

	// extern NSUInteger NSUIntegerType;
	[Field ("NSUIntegerType", "__Internal")]
	nuint NSUIntegerType { get; }
}
