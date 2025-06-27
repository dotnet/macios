// Copyright (c) Microsoft Corporation.
// Licensed under the MIT License.

using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Microsoft.Macios.Generator.DataModel;
using static Microsoft.CodeAnalysis.CSharp.SyntaxFactory;

namespace Microsoft.Macios.Generator.Formatters;

static class ConstructorFormatter {

	/// <summary>
	/// Converts a <see cref="Constructor"/> to its C# declaration as a <see cref="CompilationUnitSyntax"/>.
	/// </summary>
	/// <param name="constructor">The constructor to convert.</param>
	/// <returns>A <see cref="CompilationUnitSyntax"/> representing the constructor declaration.</returns>
	public static CompilationUnitSyntax ToDeclaration (this in Constructor constructor)
	{
		var compilationUnit = CompilationUnit ().WithMembers (
			SingletonList<MemberDeclarationSyntax> (
				ConstructorDeclaration (Identifier (constructor.Type)
						.WithTrailingTrivia (Space) // add spaces manually to use the mono style
					)
					.WithModifiers (TokenList (constructor.Modifiers)).NormalizeWhitespace ()
					.WithParameterList (constructor.Parameters.GetParameterList ().WithLeadingTrivia (Space))
			));
		return compilationUnit;
	}

	/// <summary>
	/// Converts a nullable <see cref="Constructor"/> to its C# declaration as a <see cref="CompilationUnitSyntax"/>.
	/// </summary>
	/// <param name="constructor">The constructor to convert.</param>
	/// <returns>A <see cref="CompilationUnitSyntax"/> representing the constructor declaration, or <c>null</c> if the input constructor is <c>null</c>.</returns>
	public static CompilationUnitSyntax? ToDeclaration (this in Constructor? constructor)
		=> constructor?.ToDeclaration ();
}
