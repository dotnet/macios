// Copyright (c) Microsoft Corporation.
// Licensed under the MIT License.

@interface PrimaryDelegate
@end

@interface SecondaryDelegate
@end

@interface DelegateConsumer
	@property (assign) PrimaryDelegate *delegate;
	@property (assign) SecondaryDelegate *secondaryDelegate;
@end
