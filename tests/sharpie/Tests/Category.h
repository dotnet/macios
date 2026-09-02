// Copyright (c) Microsoft Corporation.
// Licensed under the MIT License.

@interface Interface
@end

@protocol SomeProtocol
@end

@interface Interface (Category)
@end

@interface Interface (CategoryWithProtocol) <SomeProtocol>
@end

@interface Interface (CategoryWithInstanceType)
+(instancetype)hello;
@end

@interface NSString
@end

@interface NSData
@end

@interface NSString (Extensions)
@end

@interface NSData (Extensions)
@end
