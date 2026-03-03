// Copyright (c) Microsoft Corporation.
// Licensed under the MIT License.

using ICSharpCode.NRefactory.CSharp;

namespace Sharpie.Bind.Attributes;

public sealed class NullAllowedAttribute : GroupedAttribute {
	public NullAllowedAttribute () : base (2)
	{
		// NOTE: this is in global namespace :-/
		Type = AstType.Create ("NullAllowedAttribute");
	}
}
