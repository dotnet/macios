// Copyright (c) Microsoft Corporation.
// Licensed under the MIT License.

using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace Microsoft.Macios.Generator.Extensions;

static class BaseTypeDeclarationSyntaxExtensions {
	/// <summary>
	/// Return the fully qualified identifier for a given <see cref="BaseTypeDeclarationSyntax"/> by
	/// navigating the syntax tree and getting the namespace and class names.
	/// </summary>
	/// <param name="self">The declaration whose fully qualified name we want to retrieve.</param>
	/// <param name="semanticModel">The current compilation semantic model.</param>
	/// <returns>A fully qualified identifier with all namespaces and classes found in the syntax tree.</returns>
	public static string GetFullyQualifiedIdentifier (this BaseTypeDeclarationSyntax self, SemanticModel semanticModel)
	{
		var symbol = semanticModel.GetDeclaredSymbol (self);
		return symbol is null ?
			string.Empty :
			symbol.ToDisplayString (SymbolDisplayFormat.FullyQualifiedFormat.WithGlobalNamespaceStyle (SymbolDisplayGlobalNamespaceStyle.Omitted));
	}
}
