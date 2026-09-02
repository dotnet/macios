// Copyright (c) Microsoft Corporation.
// Licensed under the MIT License.

// RUN macosx x86_64 c:    -sdk macosx -arch x86_64 -x c
// RUN macosx x86_64 objc: -sdk macosx -arch x86_64 -x objective-c
// RUN macosx arm64 c:    -sdk macosx -arch arm64 -x c
// RUN macosx arm64 objc: -sdk macosx -arch arm64 -x objective-c
// RUN iphoneos arm64 c:     -sdk iphoneos -arch arm64 -x c
// RUN iphoneos arm64 objc:  -sdk iphoneos -arch arm64 -x objective-c

#include <os/object.h>

#if OS_OBJECT_HAVE_OBJC_SUPPORT
extern _Bool OS_OBJECT_HAVE_OBJC_SUPPORT__True;
#else
extern _Bool OS_OBJECT_HAVE_OBJC_SUPPORT__False;
#endif

#if OS_OBJECT_USE_OBJC
extern _Bool OS_OBJECT_USE_OBJC__True;
#else
extern _Bool OS_OBJECT_USE_OBJC__False;
#endif
