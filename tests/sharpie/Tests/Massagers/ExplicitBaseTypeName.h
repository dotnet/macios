// Copyright (c) Microsoft Corporation.
// Licensed under the MIT License.

// RUN macosx: -sdk macosx -massage +ExplicitBaseTypeName -x objective-c
// RUN iphoneos: -sdk iphoneos -massage +ExplicitBaseTypeName -x objective-c

#import <Foundation/Foundation.h>

@interface ExplicitBaseType : NSObject

@end
