// Copyright (c) Microsoft Corporation.
// Licensed under the MIT License.

using ICSharpCode.NRefactory.CSharp;

namespace Sharpie.Bind.Attributes;

public sealed class StaticAttribute : GroupedAttribute {
	public StaticAttribute () : base (1)
	{
		// NOTE: this is in global namespace :-/
		Type = AstType.Create ("StaticAttribute");
	}
}
