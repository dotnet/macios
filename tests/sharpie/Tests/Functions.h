// Copyright (c) Microsoft Corporation.
// Licensed under the MIT License.

// RUN macosx: -x objective-c -sdk macosx
// RUN iphoneos: -x objective-c -sdk iphoneos

#include <stddef.h>

extern void Action ();
extern void ActionTakingInt (int i);
extern void ActionTakingIntAndCString (int i, const char *str);
extern char *FuncTakingInt (int i);

// interesting - if you redeclare this as 'snprintf' even if you do not
// include stdio.h, Clang provides a builtin Decl to preceed it.
extern int __snprintf (char * restrict str, size_t size, const char * restrict format, ...);

@interface ObjCMethods
-(void)action;
-(void)actionTakingInt:(int)i;
-(void)actionTakingInt:(int)i andCString:(const char *)str;
-(char *)funcTakingInt:(int)i;

+(int)snprintf:(char * restrict)str ofSize:(size_t)size withFormat:(const char * restrict) format, ...;
@end
