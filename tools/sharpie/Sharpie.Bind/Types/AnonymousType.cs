// Copyright (c) Microsoft Corporation.
// Licensed under the MIT License.

using ICSharpCode.NRefactory.CSharp;

namespace Sharpie.Bind.Types;

public sealed class AnonymousType : SimpleType {
	public AnonymousType (ClangSharp.Type type)
		: base ("[anonymous " + type.Kind + ": " + type + "]")
	{
	}
}
