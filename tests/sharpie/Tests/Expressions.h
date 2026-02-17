// Copyright (c) Microsoft Corporation.
// Licensed under the MIT License.

// RUN macosx_x86_64: -sdk macosx -arch x86_64
// RUN iphoneos_arm64: -sdk iphoneos -arch arm64
// RUN macosx_arm64: -sdk macosx -arch arm64

#define Celcius

struct SomeStruct {
	char buf [32];
};

enum Expressions
{
	// Literals
	One = 1,
	Char = 'a',
	FourCC = 'beer',

	// Binary
	Add = 1 + 2,
	Subtract = 5 - 3,
	Multiply = 10 * 4,
	Divide = 50 / 5,
	Modulus = 100 % 2,
	ShiftLeft = 200 << 4,
	ShiftRight = 16 >> 2,
	LessThan = 2 < 4,
	GreaterThan = 4 > 2,
	LessThanOrEqual = 50 <= 100,
	GreaterThanOrEqual = 100 >= 50,
	Equality = 200 == 200,
	InEquality = 200 != 400,
	BitwiseAnd = 8 & 4,
	BitwiseOr = 2 | 1,
	ExclusiveOr = 4 ^ 2,
	ConditionalAnd = 2 && 0,
	ConditionalOr = 2 || 0,

	// Unary
	Plus = +10,
	Minus = (int)-273.15 Celcius, // 0 Kelvin, of course
	Not = !100,
	BitNot = ~200,

	// C-C-C-C-Combo Breaker!
	BinaryTreeSimple = 1 + 2 * 3,
	BinaryTreeParen = ((2 + (3 * (4 / 1) + 8))),
	ParenSubExpr = (0),

	// TypeTraitExpr
	SizeofChar = sizeof (char),
	SizeofShort = sizeof (short),
	SizeofInt = sizeof (int),
	SizeofLong = sizeof (long),
	SizeofLongLong = sizeof (long long),
	SizeofFloat = sizeof (float),
	SizeofDouble = sizeof (double),
	SizeofLongDouble = sizeof (long double),
	SizeofPointer = sizeof (void *),
	SizeofSomeStruct = sizeof (struct SomeStruct),
	SizeofConstArray = sizeof (int [400])
};
