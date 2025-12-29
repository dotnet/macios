// Copyright (c) Microsoft Corporation.
// Licensed under the MIT License.

using ICSharpCode.NRefactory.CSharp;

namespace Sharpie.Bind.Attributes;

public sealed class DesignatedInitializerAttribute : GroupedAttribute {
	public DesignatedInitializerAttribute () : base (3)
	{
		Type = AstType.Create ("Foundation.DesignatedInitializerAttribute");
	}
}
