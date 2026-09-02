// Copyright (c) Microsoft Corporation.
// Licensed under the MIT License.

using ICSharpCode.NRefactory.CSharp;

namespace Sharpie.Bind.Attributes;

public sealed class InternalAttribute : GroupedAttribute {
	public InternalAttribute () : base (1)
	{
		// NOTE: this is in global namespace :-/
		Type = AstType.Create ("InternalAttribute");
	}
}
