// Copyright (c) Microsoft Corporation.
// Licensed under the MIT License.

using ICSharpCode.NRefactory.CSharp;

namespace Sharpie.Bind.Attributes;

public sealed class BindAttribute : GroupedAttribute {
	public BindAttribute (string name) : base (1)
	{
		// NOTE: this is in global namespace :-/
		Type = AstType.Create ("BindAttribute");
		Arguments.Add (new PrimitiveExpression (name));
	}
}
