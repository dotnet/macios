// Copyright (c) Microsoft Corporation.
// Licensed under the MIT License.

// RUN iphoneos: -sdk iphoneos -x objective-c
// RUN macosx: -sdk macosx -x objective-c

#import <Foundation/Foundation.h>

#if __has_feature(nullability)
#else
#  error nullability feature should be defined
#endif

void Func (const char * __nullable str);

@interface Foo

@property (nullable) SEL selector;
@property (nullable, readonly) id someObject;
@property (nonatomic, readonly, nullable) __kindof NSObject *presentedObject;

-(__nullable id)nullableReturnPointer;
-(nullable id)nullableReturnPointer:(int)arg withNullable:(nullable id)obj;
-(__kindof NSObject * _Nullable) AnObject;
-(__kindof NSObject * _Nullable) createObjectWithObject:(__kindof NSObject * _Nullable)otherObject;

@end
