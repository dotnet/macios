// Copyright (c) Microsoft Corporation.
// Licensed under the MIT License.

using System.Collections.Immutable;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Microsoft.Macios.Generator.DataModel;
using Microsoft.Macios.Generator.Extensions;
using static Microsoft.CodeAnalysis.CSharp.SyntaxFactory;

namespace Microsoft.Macios.Generator.Emitters;

static partial class BindingSyntaxFactory {

	/// <summary>
	/// Returns an argument syntax for the provided kind and literal expression.
	/// </summary>
	/// <param name="kind">The kind of the literal value argument.</param>
	/// <param name="literal">The value of the argument.</param>
	/// <returns>A literal argument with the provided value.</returns>
	static ArgumentSyntax GetLiteralExpressionArgument (SyntaxKind kind, string literal)
	{
		return Argument (LiteralExpression (kind, Literal (literal)));
	}

	static ExpressionSyntax StaticInvocationExpression (ExpressionSyntax staticClassName, string methodName,
		ImmutableArray<ArgumentSyntax> arguments, bool suppressNullableWarning = false)
	{
		var argumentList = ArgumentList (
			SeparatedList<ArgumentSyntax> (arguments.ToSyntaxNodeOrTokenArray ()));

		var invocation = InvocationExpression (
			MemberAccessExpression (
				SyntaxKind.SimpleMemberAccessExpression,
				staticClassName,
				IdentifierName (methodName).WithTrailingTrivia (Space)
			)
		).WithArgumentList (argumentList);

		return suppressNullableWarning
			? PostfixUnaryExpression (SyntaxKind.SuppressNullableWarningExpression, invocation)
			: invocation;
	}


	static ExpressionSyntax StaticInvocationGenericExpression (ExpressionSyntax staticClassName, string methodName,
		string genericName,
		ArgumentListSyntax argumentList, bool suppressNullableWarning = false)
	{
		var invocation = InvocationExpression (
			MemberAccessExpression (
				SyntaxKind.SimpleMemberAccessExpression,
				staticClassName,
				GenericName (
						Identifier (methodName))
					.WithTypeArgumentList (TypeArgumentList (
						SingletonSeparatedList<TypeSyntax> (IdentifierName (genericName))))
					.WithTrailingTrivia (Space)
			)
		).WithArgumentList (argumentList);

		return suppressNullableWarning
			? PostfixUnaryExpression (SyntaxKind.SuppressNullableWarningExpression, invocation)
			: invocation;
	}

	static ExpressionSyntax ThrowException (string type, string? message = null)
	{
		var throwExpression = ObjectCreationExpression (IdentifierName (type));

		if (message is not null) {
			var argumentList = ArgumentList (SingletonSeparatedList (
				GetLiteralExpressionArgument (SyntaxKind.StringLiteralExpression, message)
			)).WithLeadingTrivia (Space);

			throwExpression = throwExpression.WithArgumentList (argumentList).NormalizeWhitespace ();
		} else {
			throwExpression = throwExpression.WithArgumentList (ArgumentList ().WithLeadingTrivia (Space));
		}

		return ThrowExpression (throwExpression).NormalizeWhitespace ();
	}

	static ExpressionSyntax ThrowNotSupportedException (string message)
		=> ThrowException (type: "NotSupportedException", message: message);

	static ExpressionSyntax ThrowNotImplementedException ()
		=> ThrowException (type: "NotImplementedException");

	/// <summary>
	/// Generate a field declaration.
	/// </summary>
	/// <param name="variableName">The variable name to be used with the field.</param>
	/// <param name="variableType">The variable type.</param>
	/// <param name="nullable">If the variable type should be made nullable.. </param>
	/// <returns>The syntax for the field declaration.</returns>
	internal static MemberDeclarationSyntax StaticVariable (string variableName, string variableType, bool nullable)
	{
		return FieldDeclaration (
				VariableDeclaration (
						nullable
							? NullableType (IdentifierName (variableType))
							: IdentifierName (variableType)
					)
					.WithVariables (
						SingletonSeparatedList (
							VariableDeclarator (
								Identifier (variableName)))))
			.WithModifiers (TokenList (Token (SyntaxKind.StaticKeyword)))
			.NormalizeWhitespace ();
	}

	/// <summary>
	/// Generates the syntax to declare the variable used by a field property.  
	/// </summary>
	/// <param name="property">The field property whose backing variable we want to generate.</param>
	/// <returns>The variable declaration syntax.</returns>
	internal static MemberDeclarationSyntax FieldPropertyBackingVariable (in Property property)
	{
		var variableType = property.ReturnType.FullyQualifiedName;
		if (property.ReturnType.SpecialType is SpecialType.System_IntPtr or SpecialType.System_UIntPtr
			&& property.ReturnType.MetadataName is not null) {
			variableType = property.ReturnType.MetadataName;
		}

		return StaticVariable (property.BackingField, variableType, property.IsReferenceType);
	}

	/// <summary>
	/// Returns a using statement or block for a local declaration.
	///
	/// This allows to write the following for a binding:
	///
	/// <code>
	/// var conde = @"
	/// if ({variable} is not null) {
	///		{Using (GetAutoreleasePoolVariable ())}
	/// }
	/// ";
	/// </code>
	/// </summary>
	/// <param name="declaration"></param>
	/// <param name="isBlock"></param>
	/// <returns></returns>
	internal static LocalDeclarationStatementSyntax Using (LocalDeclarationStatementSyntax declaration)
	{
		return declaration.WithUsingKeyword (Token (SyntaxKind.UsingKeyword).WithTrailingTrivia (Space));
	}

	/// <summary>
	/// Suppresses the nullable warning for the provided expression.
	/// </summary>
	/// <param name="expression">The expression whose nullable warning we want to supress.</param>
	/// <returns>An expression with a suppressed warning operator.</returns>
	internal static ExpressionSyntax SuppressNullableWarning (ExpressionSyntax expression)
	{
		return PostfixUnaryExpression (SyntaxKind.SuppressNullableWarningExpression, expression);
	}

	/// <summary>
	/// Creates the attribute syntaxt for a boolean value.
	/// </summary>
	/// <param name="value"></param>
	/// <returns></returns>
	internal static ArgumentSyntax BoolArgument (bool value)
	{
		return Argument (
			LiteralExpression (
				value ? SyntaxKind.TrueLiteralExpression : SyntaxKind.FalseLiteralExpression));
	}

	/// <summary>
	/// Create an expression of a variable assignment for a previously declared variable.
	/// </summary>
	/// <param name="variableName">The name of the previously declared variable.</param>
	/// <param name="value">The expression syntax of the value.</param>
	/// <returns>An assigment expression.</returns>
	internal static AssignmentExpressionSyntax AssignVariable (string variableName, ExpressionSyntax value)
	{
		return AssignmentExpression (
			SyntaxKind.SimpleAssignmentExpression,
			IdentifierName (variableName).WithTrailingTrivia (Space),
			value.WithLeadingTrivia (Space));
	}

	/// <summary>
	/// Returns the expression required for an identifier name. The method will add the namespace and global qualifier
	/// if needed based on the parameters.
	/// </summary>
	/// <param name="namespace">The namespace of the class. This can be null.</param>
	/// <param name="class">The class name.</param>
	/// <param name="isGlobal">If the global alias qualifier will be used. This will only be used if the namespace
	/// was provided.</param>
	/// <returns>The identifier expression for a given class.</returns>
	internal static TypeSyntax GetIdentifierName (string []? @namespace, string @class, bool isGlobal = false)
	{
		// retrieve the name syntax for the namespace
		if (@namespace is null) {
			// if we have no namespace, we do not care about it being global
			return IdentifierName (@class);
		}

		var fullNamespace = string.Join (".", @namespace);
		if (isGlobal) {
			return QualifiedName (
				AliasQualifiedName (
					IdentifierName (
						Token (SyntaxKind.GlobalKeyword)),
					IdentifierName (fullNamespace)),
				IdentifierName (@class));
		}

		return QualifiedName (
			IdentifierName (fullNamespace),
			IdentifierName (@class));
	}

	/// <summary>
	/// Helper method that will return the Identifier name for a class. 
	/// </summary>
	/// <param name="class">The class whose identifier we want to retrieve.</param>
	/// <returns>The identifier name expression for a given class.</returns>
	internal static TypeSyntax GetIdentifierName (string @class)
		=> IdentifierName (@class);
}
