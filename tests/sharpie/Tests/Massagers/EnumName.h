// Copyright (c) Microsoft Corporation.
// Licensed under the MIT License.

// RUN: -x objective-c

typedef enum : long {
	NINT_ENUM_ONE = 1,
	NINT_ENUM_TWO,
	NINT_ENUM_THREE
} NINT_ENUM;

extern NINT_ENUM AddOneToNintEnum (NINT_ENUM in);

typedef enum : unsigned long
{
	kPOPAnimationClampNone = 0,
	kPOPAnimationClampStart = 0 << 1,
	kPOPAnimationClampEnd = 1 << 1,
	kPOPAnimationClampBoth = kPOPAnimationClampEnd | kPOPAnimationClampStart
} POPAnimationClampFlags;
