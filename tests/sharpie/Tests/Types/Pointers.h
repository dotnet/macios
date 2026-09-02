// Copyright (c) Microsoft Corporation.
// Licensed under the MIT License.

// RUN macosx: -sdk macosx -x objective-c
// RUN iphoneos: -sdk iphoneos -x objective-c

#import <Foundation/Foundation.h>

void *_void_ptr ();
int *_int_ptr ();
int **_int_ptr_ptr ();
int ***_int_ptr_ptr_ptr ();

@interface ObjCObjectOutPointers
-(void)getError:(NSError **)error;
@end
