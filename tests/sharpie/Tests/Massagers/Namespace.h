// Copyright (c) Microsoft Corporation.
// Licensed under the MIT License.

// RUN: -namespace CatOverflow -x objective-c

@interface SuperCats
-(void)meow;
@end

typedef enum : long {
	CatBehaviorEvil
} CatBehavior;
