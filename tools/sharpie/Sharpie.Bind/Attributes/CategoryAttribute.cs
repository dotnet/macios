// Copyright (c) Microsoft Corporation.
// Licensed under the MIT License.

using ICSharpCode.NRefactory.CSharp;

namespace Sharpie.Bind.Attributes;

public sealed class CategoryAttribute : GroupedAttribute {
	public CategoryAttribute () : base (1)
	{
		Type = AstType.Create ("Category");
	}
}
