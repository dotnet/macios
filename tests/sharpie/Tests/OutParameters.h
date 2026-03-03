// Copyright (c) Microsoft Corporation.
// Licensed under the MIT License.

// RUN macosx: -sdk macosx -x objective-c
// RUN iphoneos: -sdk iphoneos -x objective-c

#import <Foundation/Foundation.h>

@interface OutParams
-(void)foo:(id)foo withError:(NSError **)error;
-(void)bar:(NSNumber *)num withOutObject:(id *)obj;
@end
