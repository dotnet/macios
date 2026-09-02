// Copyright (c) Microsoft Corporation.
// Licensed under the MIT License.

// RUN macosx: -sdk macosx -x objective-c
// RUN iphoneos: -sdk iphoneos -x objective-c
//
// These types are "specialized" by clang on ASTContext

#include <stdint.h>
#include <stddef.h>
#include <objc/objc.h>

#import <Foundation/Foundation.h>

@interface CommonClang
+(instancetype)_instancetype;
-(BOOL)_BOOL;
-(intptr_t)_intptr_t;
-(uintptr_t)_uintptr_t;
-(NSString *)_NSString_ptr;
@end
