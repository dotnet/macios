// Copyright (c) Microsoft Corporation.
// Licensed under the MIT License.

using ICSharpCode.NRefactory.CSharp;

namespace Sharpie.Bind.Types;

public sealed class UnsupportedType : SimpleType {
	public UnsupportedType (ClangSharp.Type type)
		: base ($"PlaceholderType /* Unsupported: {type.Kind}: {type} */")
	{
	}
}
