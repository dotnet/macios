// Copyright (c) Microsoft Corporation.
// Licensed under the MIT License.

@interface UnsafeMassagerTest
-(int *)returnPointerMethod;
-(void)acceptPointerMethod:(int *)ptr;
@property (readonly) int * readonlyPtrProperty;
@property int *readWritePtrProperty;
@end
