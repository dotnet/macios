// Copyright (c) Microsoft Corporation.
// Licensed under the MIT License.

using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;

namespace Microsoft.Macios.Generator.Extensions;

public static class ParameterSymbolExtensions {

	/// <summary>
	/// Gets the safe name of the parameter symbol, prefixing with "@" if it's a C# keyword.
	/// </summary>
	/// <param name="symbol">The parameter symbol.</param>
	/// <returns>The safe name of the parameter, suitable for use in generated C# code. 
	/// For example, if the parameter name is "event", this method returns "@event". 
	/// Otherwise, it returns the original parameter name.</returns>
	public static string GetSafeName (this IParameterSymbol symbol)
	{
		// In C#, the @ symbol is used as an escape character to allow reserved keywords (like event) to be used as
		// identifiers. However, when working with Roslyn's IParameterSymbol, the Name property returns the identifier
		// without the @ prefix because the @ is only required in the source code to escape the keyword.
		// Internally, the compiler treats the name as the unescaped version.
		string parameterName = SyntaxFacts.GetKeywordKind (symbol.Name) != SyntaxKind.None
			? "@" + symbol.Name
			: symbol.Name;
		return parameterName;
	}
}
