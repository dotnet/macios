// Copyright (c) Microsoft Corporation.
// Licensed under the MIT License.

namespace Sharpie.Bind;

public enum ArgumentSemantic {
	None,
	Assign,
	Retain,
	Copy,
	Weak,
	Strong,
	UnsafeUnretained,
}
