// Copyright (c) Microsoft Corporation.
// Licensed under the MIT License.

// RUN macosx: -sdk macosx -x objective-c
// RUN iphoneos: -sdk iphoneos -x objective-c

#import <Foundation/Foundation.h>

@interface NSArrayToArrayOfNSObjectTest
-(NSArray *)getNSArray;
@end

@interface NSArrayOfNSStringToArrayOfStringTest
-(NSArray<NSString *> *)getStrings;
@end

@interface NSStringPropertyTest
@property (copy, readonly) NSString *someString;
@end

extern NSString * __nullable CFunctionWithNSStringTest (NSString * __nonnull, int c, NSString * __nullable);
