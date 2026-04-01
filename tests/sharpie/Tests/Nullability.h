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

// Functions with nullable return types
extern NSObject * _Nullable NullableReturnObject (void);
extern NSString * _Nullable NullableReturnString (int code);

// Functions with nullable parameters
extern void FuncWithNullableObject (NSObject * _Nullable obj);
extern void FuncWithNullableString (NSString * _Nullable str);
extern void FuncWithMixedNullability (NSObject * _Nonnull required, NSObject * _Nullable optional);

// Functions with both nullable params and return
extern NSObject * _Nullable FuncNullableInAndOut (NSString * _Nullable input);

// Functions with nullable block parameters
extern void FuncWithNullableBlock (void (^ _Nullable block)(void));
extern void FuncWithNullableBlockParam (void (^ _Nullable block)(NSObject * _Nullable obj));
extern void FuncWithNonnullBlock (void (^ _Nonnull block)(void));

// Functions returning nullable block
typedef void (^SimpleBlock)(void);
extern SimpleBlock _Nullable FuncReturningNullableBlock (void);

@interface Foo

@property (nullable) SEL selector;
@property (nullable, readonly) id someObject;
@property (nonatomic, readonly, nullable) __kindof NSObject *presentedObject;

-(__nullable id)nullableReturnPointer;
-(nullable id)nullableReturnPointer:(int)arg withNullable:(nullable id)obj;
-(__kindof NSObject * _Nullable) AnObject;
-(__kindof NSObject * _Nullable) createObjectWithObject:(__kindof NSObject * _Nullable)otherObject;

@end
