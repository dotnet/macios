// Copyright (c) Microsoft Corporation.
// Licensed under the MIT License.

// RUN iphoneos: -x objective-c -sdk iphoneos
// RUN macosx: -x objective-c -sdk macosx

extern int AnyAppleOSFunction(void) __attribute__((availability(anyAppleOS,introduced=27.0)));

__attribute__((availability(anyAppleOS,introduced=27.0)))
@interface AnyAppleOSIntroduced
-(void)introducedMethod __attribute__((availability(anyAppleOS,introduced=27.0)));
@end
