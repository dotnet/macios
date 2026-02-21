// Copyright (c) Microsoft Corporation.
// Licensed under the MIT License.

using ICSharpCode.NRefactory.CSharp;

namespace Sharpie.Bind;

public sealed class NativeCodeReferenceComment : Comment {
	public NativeCodeReferenceComment (string content) : base (content)
	{
	}
}
