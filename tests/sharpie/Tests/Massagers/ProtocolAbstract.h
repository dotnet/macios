// Copyright (c) Microsoft Corporation.
// Licensed under the MIT License.

@protocol ProtocolAbstract
	-(void)implicitRequired;

	@optional
	-(void)explicitOptional;

	@required
	-(void)explicitRequired;
@end
