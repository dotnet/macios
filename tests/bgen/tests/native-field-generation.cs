// Copyright (c) Microsoft Corporation.
// Licensed under the MIT License.

#nullable enable

using System;

using CoreGraphics;
using Foundation;

namespace NativeFieldGeneration {

	[Static]
	[Internal]
	interface NativeStructFields {
		[Field ("RequiredStruct", "__Internal")]
		CGRect Required { get; }

		[Field ("NullableString", "__Internal")]
		[NullAllowed]
		NSString NullableString { get; }
	}

	[Partial]
	interface SymbolAddresses {
		[Field ("CallbackTable", "__Internal", SymbolAddress = true)]
		IntPtr CallbackTable { get; }
	}
}
