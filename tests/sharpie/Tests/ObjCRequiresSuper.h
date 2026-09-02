// Copyright (c) Microsoft Corporation.
// Licensed under the MIT License.

#define NS_REQUIRES_SUPER __attribute__((objc_requires_super))

@interface RequiresSuperTest
-(void)foo NS_REQUIRES_SUPER;
@end
