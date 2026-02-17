// Copyright (c) Microsoft Corporation.
// Licensed under the MIT License.

using ICSharpCode.NRefactory.CSharp;

namespace Sharpie.Bind.Attributes;

public sealed class DisableDefaultCtorAttribute : GroupedAttribute {
	public DisableDefaultCtorAttribute () : base (3)
	{
		// NOTE: this is in global namespace :-/
		Type = AstType.Create ("DisableDefaultCtorAttribute");
	}
}
