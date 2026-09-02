// Copyright (c) Microsoft Corporation.
// Licensed under the MIT License.

@interface DefaultCtor
	-(instancetype)init;
@end

@interface DefaultCtorUnavailable
	-(instancetype)init __attribute__((availability(macosx,unavailable)));
@end
