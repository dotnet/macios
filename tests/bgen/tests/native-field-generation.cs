// Copyright (c) Microsoft Corporation.
// Licensed under the MIT License.

#nullable enable

using System;

using Foundation;

namespace NativeFieldGeneration {

	public struct NativeStruct {
		public int Value;
	}

	[Partial (IsStruct = true, Name = nameof (NativeStruct))]
	interface NativeStructFields {
		[Field ("RequiredStruct", "__Internal")]
		NativeStruct Required { get; }

		[DefaultValueOnMissingSymbol]
		[Field ("OptionalStruct", "__Internal")]
		NativeStruct Optional { get; }

		[Field ("NullableString", "__Internal")]
		[NullAllowed]
		NSString NullableString { get; }
	}

	[Partial]
	interface SymbolAddresses {
		[Field ("CallbackTable", "__Internal")]
		[SymbolAddress]
		IntPtr CallbackTable { get; }
	}
}
