// Copyright (c) Microsoft Corporation.
// Licensed under the MIT License.

using ICSharpCode.NRefactory.CSharp;

namespace Sharpie.Bind.Attributes;

public sealed class FieldAttribute : GroupedAttribute {
	public string SymbolName { get; private set; }
	public string? LibraryName { get; private set; }

	public FieldAttribute (string symbolName, string? libraryName = null) : base (1)
	{
		SymbolName = symbolName;
		LibraryName = libraryName;

		Type = AstType.Create ("Foundation.FieldAttribute");

		Arguments.Add (new PrimitiveExpression (SymbolName));
		if (LibraryName is not null)
			Arguments.Add (new PrimitiveExpression (LibraryName));
	}
}
