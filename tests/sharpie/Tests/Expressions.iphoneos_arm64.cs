using System.Runtime.InteropServices;

[StructLayout (LayoutKind.Sequential)]
public struct SomeStruct {
	public sbyte [] buf;
}

public enum Expressions {
	One = 1,
	Char = 97,
	FourCC = 1650812274,
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
	Plus = +10,
	Minus = (int) -273.15,
	Not = !100,
	BitNot = ~200,
	BinaryTreeSimple = 1 + 2 * 3,
	BinaryTreeParen = ((2 + (3 * (4 / 1) + 8))),
	ParenSubExpr = (0),
	SizeofChar = sizeof (sbyte),
	SizeofShort = sizeof (short),
	SizeofInt = sizeof (int),
	SizeofLong = 8,
	SizeofLongLong = sizeof (long),
	SizeofFloat = sizeof (float),
	SizeofDouble = sizeof (double),
	SizeofLongDouble = 8,
	SizeofPointer = 8,
	SizeofSomeStruct = 32,
	SizeofConstArray = 1600
}
