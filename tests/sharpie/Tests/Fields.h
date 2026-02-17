// Copyright (c) Microsoft Corporation.
// Licensed under the MIT License.

// RUN macosx: -sdk macosx -x objective-c
// RUN iphoneos: -sdk iphoneos -x objective-c

#import <Foundation/Foundation.h>

extern NSString *kFirstField;
extern int *kNextField;

@interface FirstInterface
@end

extern int64_t FirstFieldAfterFirstInterface;
extern NSString *SecondFieldAfterFirstInterface;

typedef enum : long {
	FirstEnumZero,
	FirstEnumOne = FirstEnumZero + 1
} FirstEnum;

extern bool FirstFieldAfterFirstEnum;

typedef long LooselyCoupledEnum;
enum {
	LooselyCoupledEnumZero,
	LooselyCoupledEnumOne
};

extern LooselyCoupledEnum EnumField;
