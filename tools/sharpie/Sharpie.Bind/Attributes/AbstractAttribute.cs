// Copyright (c) Microsoft Corporation.
// Licensed under the MIT License.

using ICSharpCode.NRefactory.CSharp;

namespace Sharpie.Bind.Attributes;

public sealed class AbstractAttribute : GroupedAttribute {
	public AbstractAttribute () : base (1)
	{
		// NOTE: this is in global namespace :-/
		Type = AstType.Create ("AbstractAttribute");
	}
}
