// Copyright (c) Microsoft Corporation.
// Licensed under the MIT License.

using ICSharpCode.NRefactory.CSharp;

namespace Sharpie.Bind.Attributes;

public class AdviceAttribute : GroupedAttribute {
	public AdviceAttribute () : base (1) => Type = AstType.Create ("Foundation.AdviceAttribute");
}
