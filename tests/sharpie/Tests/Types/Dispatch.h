// Copyright (c) Microsoft Corporation.
// Licensed under the MIT License.

// RUN macosx: -sdk macosx -x objective-c
// RUN iphoneos: -sdk iphoneos -x objective-c

#include <dispatch/dispatch.h>

@interface DispatchTests
@property (assign) dispatch_object_t object;
@property (assign) dispatch_queue_t queue;
@property (assign) dispatch_group_t group;
@end
