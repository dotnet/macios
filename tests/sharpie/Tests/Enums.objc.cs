using System;
using System.Runtime.InteropServices;
using ObjCRuntime;

[Native]
public enum StronglyTypedObjCOrCXXEnum : long {
	Zero = 0,
	One = 1
}

public enum TypedefEnum : uint {
	One,
	Two,
	Three
}

public enum TestNameFixOne : uint {
	A,
	B,
	C
}

public enum ThisWasRenamedByATypedef : uint {
	A,
	B
}

static class CFunctions {
	// extern enum TestNameFixOne GetTestNameFixOne ();
	[DllImport ("__Internal")]
	[Verify (PlatformInvoke)]
	static extern TestNameFixOne GetTestNameFixOne ();

	// extern void ConsumeForwardDeclaredCEnum (enum ForwardDeclaredCEnum e);
	[DllImport ("__Internal")]
	[Verify (PlatformInvoke)]
	static extern void ConsumeForwardDeclaredCEnum (ForwardDeclaredCEnum e);
}

public enum ConstantExpressionEnum : uint {
	Implicit,
	One = 1,
	OneChar = 97,
	FourChars = 1633837924,
	Maths = (1 + 2) / 3 * 10,
	MathsWithChars = (122 - 119) * 542070098,
	EscapeChars = 571017762
}

[Native]
public enum DeclRefExprEnum : long {
	One = 1,
	Two = 2,
	Three = One + Two
}

public enum UnaryExprOrTypeTraitExprEnum : uint {
	SizeOf = 8
}

public enum TypedefFirstAnonEnum : uint {
	All = 0,
	One,
	Two
}

public enum TypedefSecondAnonEnum : uint {
	All = 0,
	One,
	Two
}

public enum TypedefFirstNamedEnum : uint {
	TdFirstNamedAll = 0
}

public enum AnonymousEnum1 : uint {
	AnonEnumFollowingTypedefedEnum
}

public enum LongAndUnsignedConstants : ulong {
	Int32Max = 2147483647,
	Int32MaxPlusOne = 2147483648L,
	Int64Max = 9223372036854775807L,
	Int64MaxPlusOne = 9223372036854775808uL
}

public enum Shifts : uint {
	Sh1 = 1u << 1,
	Sh2 = 1u << 2,
	Sh3 = 1u << 3,
	Sh4 = 1u << 4
}

[Verify (InferredFromMemberPrefix)]
public enum FullyAnonEnum : uint {
	Zero,
	One
}

public enum TypedefAnonCEnum : uint {
	Zero,
	One,
	Two = 1 + 1
}

public enum NamedCEnum : uint {
	NamedCEnumZero
}

public enum NamedCEnumRenamedByTypedef : uint {
	NamedCEnumToBeRenamedByTypedefZero
}

public enum NamedCEnumRenamedByInlineTypedef : uint {
	NamedCEnumToBeRenamedByInlineTypedefZero
}

public enum SugarLevelFourEnum : uint {
	FullyDesugaredEnumZero
}

[Flags]
public enum NSActivityOptions : ulong {
	IdleDisplaySleepDisabled = (1uL << 40),
	IdleSystemSleepDisabled = (1uL << 20),
	SuddenTerminationDisabled = (1uL << 14),
	AutomaticTerminationDisabled = (1uL << 15),
	UserInitiated = (0xffffff | IdleSystemSleepDisabled),
	UserInitiatedAllowingIdleSystemSleep = (UserInitiated & ~IdleSystemSleepDisabled),
	Background = 0xff,
	LatencyCritical = 0xff00000000L
}
