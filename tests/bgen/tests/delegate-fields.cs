// Copyright (c) Microsoft Corporation.
// Licensed under the MIT License.

#nullable enable

using System;
using Foundation;

namespace DelegateFields {

	delegate NSObject Transformer (NSObject value);

	[Static]
	interface FieldConstants {
		[Field ("RequiredTransformer", "__Internal")]
		Transformer RequiredTransformer { get; }

		[Field ("RequiredTransformer", "__Internal")]
		IntPtr _RequiredTransformer { get; }

		[NullAllowed]
		[Field ("NullableTransformer", "__Internal")]
		Transformer NullableTransformer { get; }

		[Field ("Action", "__Internal")]
		Action Action { get; }

		[Field ("GenericAction", "__Internal")]
		Action<NSObject> GenericAction { get; }

		[Field ("GenericFunc", "__Internal")]
		Func<NSObject, NSObject> GenericFunc { get; }
	}

	[BaseType (typeof (NSObject))]
	interface FieldObject {
		[Field ("RequiredTransformer", "__Internal")]
		Transformer RequiredTransformer { get; }
	}
}
