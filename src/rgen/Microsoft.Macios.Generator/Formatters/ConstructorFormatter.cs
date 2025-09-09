// Copyright (c) Microsoft Corporation.
// Licensed under the MIT License.

using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Microsoft.Macios.Generator.DataModel;
using Microsoft.Macios.Generator.Emitters;
using static Microsoft.CodeAnalysis.CSharp.SyntaxFactory;

namespace Microsoft.Macios.Generator.Formatters;

static class ConstructorFormatter {
	/// <summary>
	/// Converts a <see cref="Constructor"/> to its C# declaration as a <see cref="CompilationUnitSyntax"/>.
	/// </summary>
	/// <param name="constructor">The constructor to convert.</param>
	/// <param name="withBaseNSFlag">States if the call to the base (NSFlag) call should be added.</param>
	/// <returns>A <see cref="CompilationUnitSyntax"/> representing the constructor declaration.</returns>
	public static CompilationUnitSyntax ToDeclaration (this in Constructor constructor, bool withBaseNSFlag = false)
	{
		var declaration = ConstructorDeclaration (Identifier (constructor.Type).WithTrailingTrivia (Space))
				.WithModifiers (TokenList (constructor.Modifiers)).NormalizeWhitespace ()
				.WithParameterList (constructor.Parameters.GetParameterList ().WithLeadingTrivia (Space));

		if (withBaseNSFlag) {
			// this could be done easier using the WithInitializer method, but we need to add a colon token to follow
			// the mono style which forces us not to use the NormalizeWhitespace method
			var baseInitializer = ConstructorInitializer (
				SyntaxKind.BaseConstructorInitializer,
				ArgumentList (
					SingletonSeparatedList (
						Argument (
							MemberAccessExpression (
								SyntaxKind.SimpleMemberAccessExpression,
								BindingSyntaxFactory.NSObjectFlag,
								IdentifierName ("Empty"))))
				).WithLeadingTrivia (Space)
			);
			declaration = declaration.WithTrailingTrivia (Space).WithInitializer (
				baseInitializer.WithColonToken (
					Token (SyntaxKind.ColonToken).WithTrailingTrivia (Space)
				)
			);
		}
		var compilationUnit = CompilationUnit ().WithMembers (
			SingletonList<MemberDeclarationSyntax> (declaration));
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
