using System;
using Foundation;
using ObjCRuntime;

[Static]
[Verify (ConstantsInterfaceAssociation)]
partial interface Constants {
	// extern NSString * kFirstField;
	[Field ("kFirstField", "__Internal")]
	NSString kFirstField { get; }

	// extern int * kNextField;
	[Field ("kNextField", "__Internal")]
	unsafe int* kNextField { get; }
}

// @interface FirstInterface
interface FirstInterface {
}

[Static]
[Verify (ConstantsInterfaceAssociation)]
partial interface Constants {
	// extern int64_t FirstFieldAfterFirstInterface;
	[Field ("FirstFieldAfterFirstInterface", "__Internal")]
	long FirstFieldAfterFirstInterface { get; }

	// extern NSString * SecondFieldAfterFirstInterface;
	[Field ("SecondFieldAfterFirstInterface", "__Internal")]
	NSString SecondFieldAfterFirstInterface { get; }
}

[Native]
public enum FirstEnum : long {
	Zero,
	One = Zero + 1
}

[Static]
[Verify (ConstantsInterfaceAssociation)]
partial interface Constants {
	// extern _Bool FirstFieldAfterFirstEnum;
	[Field ("FirstFieldAfterFirstEnum", "__Internal")]
	bool FirstFieldAfterFirstEnum { get; }
}

[Verify (InferredFromMemberPrefix)]
public enum LooselyCoupledEnum : uint {
	Zero,
	One
}

[Static]
[Verify (ConstantsInterfaceAssociation)]
partial interface Constants {
	// extern LooselyCoupledEnum EnumField;
	[Field ("EnumField", "__Internal")]
	nint EnumField { get; }
}
