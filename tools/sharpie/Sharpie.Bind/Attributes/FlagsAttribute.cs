// Copyright (c) Microsoft Corporation.
// Licensed under the MIT License.

using ICSharpCode.NRefactory.CSharp;

namespace Sharpie.Bind.Attributes;

public sealed class FlagsAttribute : GroupedAttribute {
	public FlagsAttribute () : base (0)
		=> Type = AstType.Create ("System.FlagsAttribute");
}
