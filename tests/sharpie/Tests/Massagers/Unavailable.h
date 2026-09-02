// Copyright (c) Microsoft Corporation.
// Licensed under the MIT License.

// RUN macosx: -sdk macosx
// RUN iphoneos: -sdk iphoneos

@import Foundation;

@interface UnavailableCtors
-(instancetype)init NS_UNAVAILABLE;
-(instancetype)initWithFoo:(const char *)foo NS_UNAVAILABLE;
-(instancetype)initWithBar:(int)bar NS_DESIGNATED_INITIALIZER;
@end
