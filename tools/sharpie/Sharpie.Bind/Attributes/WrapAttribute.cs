// Copyright (c) Microsoft Corporation.
// Licensed under the MIT License.

using ICSharpCode.NRefactory.CSharp;

namespace Sharpie.Bind.Attributes;

public sealed class WrapAttribute : GroupedAttribute {
	public WrapAttribute (string name) : base (1)
	{
		// NOTE: this is in global namespace :-/
		Type = AstType.Create ("WrapAttribute");
		Arguments.Add (new PrimitiveExpression (name));
	}
}
