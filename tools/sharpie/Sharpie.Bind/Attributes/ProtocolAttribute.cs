// Copyright (c) Microsoft Corporation.
// Licensed under the MIT License.

using ICSharpCode.NRefactory.CSharp;

namespace Sharpie.Bind.Attributes;

public sealed class ProtocolAttribute : GroupedAttribute {
	public ProtocolAttribute () : base (1)
	{
		Type = AstType.Create ("Foundation.ProtocolAttribute");
	}
}
