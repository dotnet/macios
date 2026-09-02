// Copyright (c) Microsoft Corporation.
// Licensed under the MIT License.

// RUN cxx11: -xc++ -std=c++11 -Wno-elaborated-enum-base
// RUN objc: -xobjective-c

typedef long NSInteger;

enum StronglyTypedObjCOrCXXEnum : NSInteger {
	Zero = 0,
	One = 1
};

typedef enum {
	TypedefEnumOne,
	TypedefEnumTwo,
	TypedefEnumThree
} TypedefEnum;

enum TestNameFixOne {
	kTestNameFixOneA,
	kTestNameFixOneB,
	kTestNameFixOneC
};

enum ToBeRenamedByTypedef {
	ToBeRenamedByTypedefA,
	ToBeRenamedByTypedefB
};

extern enum TestNameFixOne GetTestNameFixOne ();

enum ConstantExpressionEnum {
	ConstantExpressionEnumImplicit,
	ConstantExpressionEnumOne = 1,
	ConstantExpressionEnum_OneChar = 'a',
	ConstantExpressionEnum_FourChars = 'abcd',
	ConstantExpressionEnum_Maths = (1 + 2) / 3 * 10,
	ConstantExpressionEnum_MathsWithChars = ('z' - 'w') * ' OUR',
	ConstantExpressionEnum_EscapeChars = '"\t\n"'
};

typedef enum ToBeRenamedByTypedef ThisWasRenamedByATypedef;

enum DeclRefExprEnum : NSInteger {
	DREOne = 1,
	DRETwo = 2,
	DREThree = DREOne + DRETwo
};

enum UnaryExprOrTypeTraitExprEnum {
	SizeOf = sizeof (long)
};

typedef enum {
	TD_FIRST_ANON_ALL = 0,
	TD_FIRST_ANON_ONE,
	TD_FIRST_ANON_TWO
} TYPEDEF_FIRST_ANON_ENUM;

typedef enum {
	TD_SECOND_ANON_ALL = 0,
	TD_SECOND_ANON_ONE,
	TD_SECOND_ANON_TWO
} TYPEDEF_SECOND_ANON_ENUM;

typedef enum TD_FIRST_NAMED_ENUM {
	TD_FIRST_NAMED_ALL = 0
} TYPEDEF_FIRST_NAMED_ENUM;

enum {
	ANON_ENUM_FOLLOWING_TYPEDEFED_ENUM
};

enum LongAndUnsignedConstants : unsigned long long {
	Int32Max = 2147483647U, // should be written as Int32
	Int32MaxPlusOne = 2147483648U, // should be written as UInt32
	Int64Max = 9223372036854775807ULL, // should be written as Int64
	Int64MaxPlusOne = 9223372036854775808ULL, // should be written as Int64
};

enum Shifts : unsigned {
	Sh1 = 1 << 1,
	Sh2 = 1 << 2,
	Sh3 = 1 << 3,
	Sh4 = 1 << 4
};

enum {
	FullyAnonEnumZero,
	FullyAnonEnumOne
};

typedef enum {
	TypedefAnonCEnumZero,
	TypedefAnonCEnumOne,
	TypedefAnonCEnumTwo = 1 + 1
} TypedefAnonCEnum;

enum NamedCEnum {
	NamedCEnumZero
};

enum NamedCEnumToBeRenamedByTypedef {
	NamedCEnumToBeRenamedByTypedefZero
};

typedef enum NamedCEnumToBeRenamedByTypedef NamedCEnumRenamedByTypedef;

typedef enum NamedCEnumToBeRenamedByInlineTypedef {
	NamedCEnumToBeRenamedByInlineTypedefZero
} NamedCEnumRenamedByInlineTypedef;

// Sugaring Test - should be bound as 'SugarLevelFourEnum'
enum FullyDesugaredEnum {
	FullyDesugaredEnumZero
};

typedef enum FullyDesugaredEnum SugarLevelOneEnum;
typedef SugarLevelOneEnum SugarLevelTwoEnum;
typedef SugarLevelTwoEnum SugarLevelThreeEnum;
typedef SugarLevelThreeEnum SugarLevelFourEnum;

#if defined(__cplusplus)

enum class Cxx11EnumClass {
	Zero,
	One,
	Two
};

enum class Cxx11TypedEnumClass : unsigned long {
	Zero,
	One,
	Two
};

enum struct Cxx11EnumStruct {
	Zero,
	One,
	Two
};

enum struct Cxx11TypedEnumStruct : long {
	Zero
};

enum class ForwardDeclaredCxx11ScopedEnum : long;

extern "C" void ConsumeForwardDeclaredCxx11ScopedEnum (ForwardDeclaredCxx11ScopedEnum e);

#else

enum ForwardDeclaredCEnum;

extern void ConsumeForwardDeclaredCEnum (enum ForwardDeclaredCEnum e);

#endif

typedef unsigned long long uint64_t;

// https://github.com/xamarin/ObjectiveSharpie/issues/106

typedef enum __attribute__((flag_enum,enum_extensibility(open))) NSActivityOptions : uint64_t NSActivityOptions;

enum NSActivityOptions : uint64_t {
    NSActivityIdleDisplaySleepDisabled = (1ULL << 40),
    NSActivityIdleSystemSleepDisabled = (1ULL << 20),
    NSActivitySuddenTerminationDisabled = (1ULL << 14),
    NSActivityAutomaticTerminationDisabled = (1ULL << 15),
    NSActivityUserInitiated = (0x00FFFFFFULL | NSActivityIdleSystemSleepDisabled),
    NSActivityUserInitiatedAllowingIdleSystemSleep = (NSActivityUserInitiated & ~NSActivityIdleSystemSleepDisabled),
    NSActivityBackground = 0x000000FFULL,
    NSActivityLatencyCritical = 0xFF00000000ULL
};
