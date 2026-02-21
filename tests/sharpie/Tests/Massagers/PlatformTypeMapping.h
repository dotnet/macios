// Copyright (c) Microsoft Corporation.
// Licensed under the MIT License.

// RUN iphoneos: -sdk iphoneos
// RUN macosx: -sdk macosx

@import Foundation;

@interface WebFetcher : NSObject <NSURLConnectionDelegate>
@property (nonatomic, readonly, copy) NSURL *url;
-(NSURLResponse *)getResponseForUrl:(NSURL *)url withCredential:(NSURLCredential *)credential;
@end
