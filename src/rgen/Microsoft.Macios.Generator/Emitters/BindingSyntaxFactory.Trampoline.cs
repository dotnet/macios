// Copyright (c) Microsoft Corporation.
// Licensed under the MIT License.

using Microsoft.CodeAnalysis.CSharp.Syntax;
using Microsoft.Macios.Generator.DataModel;

namespace Microsoft.Macios.Generator.Emitters;

static partial class BindingSyntaxFactory {
	/// <summary>
	/// Returns the statement with the return type of the invoke method for the given type info delegate.
	/// </summary>
	/// <param name="typeInfo"></param>
	/// <param name="auxVariableName"></param>
	/// <returns></returns>
	internal static StatementSyntax? GetInvokeReturnType (TypeInfo typeInfo, string auxVariableName)
	{
		if (!typeInfo.IsDelegate)
			return null;

		// based on the return type of the delegate we build a statement that will return the expected value
		return null;
	}
}
