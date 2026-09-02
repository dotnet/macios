// Copyright (c) Microsoft Corporation.
// Licensed under the MIT License.

using ICSharpCode.NRefactory.CSharp;

namespace Sharpie.Bind.Attributes;

public sealed class FieldOffsetAttribute : GroupedAttribute {
	public int Offset { get; private set; }

	public FieldOffsetAttribute (int offset) : base (1)
	{
		Type = typeof (System.Runtime.InteropServices.FieldOffsetAttribute).ToAstType ();
		Offset = offset;
		Arguments.Add (new PrimitiveExpression (offset));
	}
}
