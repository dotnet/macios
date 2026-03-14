// Copyright (c) Microsoft Corporation.
// Licensed under the MIT License.

namespace Foundation;

enum NSNullBehavior {
	/// <summary>NSNull values are dropped.</summary>
	Drop,
	/// <summary>NSNull values are dropped, unless they're compatible with the target type (NSObject, NSNull, INativeObject, etc.).</summary>
	DropIfIncompatible,
	/// <summary>NSNull values are converted to <see langword="null" />.</summary>
	ConvertToNull,
	/// <summary>An exception is thrown.</summary>
	Throw,
}
