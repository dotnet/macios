// Copyright (c) Microsoft Corporation.
// Licensed under the MIT License.

// RUN macosx: -sdk macosx
// RUN iphoneos: -sdk iphoneos

@import Foundation;

@interface DisableDefaultCtorTest
-(instancetype)init NS_UNAVAILABLE;
-(instancetype)initWithString:(nonnull NSString *)str;
@end
