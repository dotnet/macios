// Copyright (c) Microsoft Corporation.
// Licensed under the MIT License.

__attribute__((availability(macosx,introduced=10.9)))
@protocol Foo
@required
+(id)availability_StaticAbstractInternal_Export:(int)n, ...
	__attribute__((availability(macosx,introduced=10.10)))
	__attribute__((availability(ios,introduced=8.0)))
	;
@end
