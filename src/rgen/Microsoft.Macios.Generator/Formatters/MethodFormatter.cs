// Copyright (c) Microsoft Corporation.
// Licensed under the MIT License.
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Microsoft.Macios.Generator.DataModel;
using static Microsoft.CodeAnalysis.CSharp.SyntaxFactory;

namespace Microsoft.Macios.Generator.Formatters;

static class MethodFormatter {

	/// <summary>
	/// Converts a <see cref="Method"/> to its C# declaration as a <see cref="CompilationUnitSyntax"/>.
	/// </summary>
	/// <param name="method">The method to convert.</param>
	/// <returns>A <see cref="CompilationUnitSyntax"/> representing the method declaration.</returns>
	public static CompilationUnitSyntax ToDeclaration (this in Method method)
	{
		var compilationUnit = CompilationUnit ()
			.WithMembers (
				SingletonList<MemberDeclarationSyntax> (
					MethodDeclaration (
							returnType: method.ReturnType.GetIdentifierSyntax (),
							identifier: Identifier (method.Name)
								.WithLeadingTrivia (Space)
								.WithTrailingTrivia (Space)) // adding the spaces manually to follow the mono style
						.WithModifiers (TokenList (method.Modifiers))
						.WithParameterList (method.Parameters.GetParameterList ())));
		return compilationUnit;
	}

	/// <summary>
	/// Converts a nullable <see cref="Method"/> to its C# declaration as a <see cref="CompilationUnitSyntax"/>.
	/// </summary>
	/// <param name="method">The method to convert.</param>
	/// <returns>A <see cref="CompilationUnitSyntax"/> representing the method declaration, or <c>null</c> if the input method is <c>null</c>.</returns>
	public static CompilationUnitSyntax? ToDeclaration (this in Method? method)
		=> method?.ToDeclaration ();
}
