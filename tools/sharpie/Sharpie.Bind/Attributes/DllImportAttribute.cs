// Copyright (c) Microsoft Corporation.
// Licensed under the MIT License.

using ICSharpCode.NRefactory.CSharp;

namespace Sharpie.Bind.Attributes;

public sealed class DllImportAttribute : GroupedAttribute {
	public DllImportAttribute (string libraryName) : base (1)
	{
		Type = AstType.Create ("System.Runtime.InteropServices.DllImport");
		Arguments.Add (new PrimitiveExpression (libraryName));
	}
}
