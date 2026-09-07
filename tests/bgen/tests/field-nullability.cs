// Copyright (c) Microsoft Corporation.
// Licensed under the MIT License.

using Foundation;

namespace FieldNullability {

	[Static]
	interface FieldConstants {
		[Field ("NullableString", "__Internal")]
		[NullAllowed]
		NSString NullableString { get; }

		[Field ("NonNullableString", "__Internal")]
		NSString NonNullableString { get; }
	}
}
