// Copyright (c) Microsoft Corporation.
// Licensed under the MIT License.

using ICSharpCode.NRefactory.CSharp;

namespace Sharpie.Bind.Attributes;

public sealed class RequiresSuperAttribute : GroupedAttribute {
	public RequiresSuperAttribute () : base (3)
	{
		Type = AstType.Create ("RequiresSuperAttribute");
	}
}
