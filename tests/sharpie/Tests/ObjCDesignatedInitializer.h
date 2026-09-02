// Copyright (c) Microsoft Corporation.
// Licensed under the MIT License.

// RUN macosx: -sdk macosx
// RUN iphoneos: -sdk iphoneos

@import Foundation;

@interface NSDesignatedInitializerTest
-(instancetype)init NS_UNAVAILABLE;
-(instancetype)initWithInt:(int)value;
-(instancetype)initWithString:(nonnull NSString *)value NS_DESIGNATED_INITIALIZER;
@end

@interface ObjDesignatedInitializerAttributeTest
-(instancetype)init NS_UNAVAILABLE;
-(instancetype)initWithInt:(int)value;
-(instancetype)initWithString:(nonnull NSString *)value __attribute__((objc_designated_initializer));
@end
