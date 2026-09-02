// Copyright (c) Microsoft Corporation.
// Licensed under the MIT License.

// RUN macosx: -sdk macosx -x objective-c
// RUN iphoneos: -sdk iphoneos -x objective-c

#import <Foundation/Foundation.h>

@interface PropertyTests
@property (readonly) int readonly;
@property int readWrite;
@property (getter=customGetter) int readWriteCustomGetterInt32;
@property (setter=customSetter:) int readWriteCustomSetterInt32;
@property (getter=customGetter, setter=customSetterInt32:) int readWriteCustomGetterAndSetter;
@property (readonly, copy) PropertyTests *clone;
@property (readonly, copy) NSString *stringValue;
@property (class) int staticInt;
@property (readonly, class) int staticReadonlyInt;
@property (nonatomic, copy, null_resettable) NSDate *nullResettableDate;
@end
