// Copyright (c) Microsoft Corporation.
// Licensed under the MIT License.

using System.Collections.Immutable;
using System.Linq;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Microsoft.Macios.Generator.DataModel;
using Microsoft.Macios.Generator.Extensions;
using Microsoft.Macios.Generator.Formatters;
using static Microsoft.CodeAnalysis.CSharp.SyntaxFactory;
using TypeInfo = Microsoft.Macios.Generator.DataModel.TypeInfo;

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
		TypeSyntax genericName,
		ArgumentListSyntax argumentList, bool suppressNullableWarning = false)
	{
		var invocation = InvocationExpression (
			MemberAccessExpression (
				SyntaxKind.SimpleMemberAccessExpression,
				staticClassName,
				GenericName (
						Identifier (methodName))
					.WithTypeArgumentList (TypeArgumentList (
						SingletonSeparatedList (genericName)))
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
	internal static MemberDeclarationSyntax StaticVariable (string variableName, TypeSyntax variableType, bool nullable)
	{
		return FieldDeclaration (
				VariableDeclaration (
						nullable
							? NullableType (variableType)
							: variableType
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
		var variableType = property.ReturnType.Name.GetIdentifierName (property.ReturnType.Namespace);
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
	/// Helper method that will return the Identifier name for a class. 
	/// </summary>
	/// <param name="class">The class whose identifier we want to retrieve.</param>
	/// <returns>The identifier name expression for a given class.</returns>
	internal static TypeSyntax GetIdentifierName (string @class)
		=> IdentifierName (@class);

	/// <summary>
	/// Create the necessary expression to call the AsRef method from the Unsafe class.
	/// </summary>
	/// <param name="objectType">The target type for get the ref from.</param>
	/// <param name="arguments">The arguments to pass to the AsRef method.</param>
	/// <returns>The needed expression to call the AsRef method.</returns>
	internal static ExpressionSyntax AsRef (TypeSyntax objectType, ImmutableArray<ArgumentSyntax> arguments)
	{
		var argsList = ArgumentList (SeparatedList<ArgumentSyntax> (arguments.ToSyntaxNodeOrTokenArray ()));
		return StaticInvocationGenericExpression (Unsafe, "AsRef",
			objectType, argsList);
	}

	/// <summary>
	/// Create the necessary expression to call the AsPointer method from the Unsafe class and cast the result to a pointer of the objectType.
	/// </summary>
	/// <param name="objectType">The target type for the pointer.</param>
	/// <param name="arguments">The arguments to pass to the AsPointer method.</param>
	/// <param name="castType">The explicit type to cast the pointer to. If null, <paramref name="objectType"/> is used.</param>
	/// <returns>The necessary expression to call the AsPointer method and cast to a pointer.</returns>
	internal static ExpressionSyntax AsPointer (TypeSyntax objectType, ImmutableArray<ArgumentSyntax> arguments, TypeSyntax? castType = null)
	{
		var argsList = ArgumentList (SeparatedList<ArgumentSyntax> (arguments.ToSyntaxNodeOrTokenArray ()));
		var invocation = StaticInvocationGenericExpression (Unsafe, "AsPointer",
			objectType, argsList);
		// we have the invocation, but we need to convert it to a pointer
		return CastExpression (PointerType (castType ?? objectType),
			invocation.WithLeadingTrivia (Space));

	}

	/// <summary>
	/// Create the necessary expression to call the AsPointer method from the Unsafe class and cast the result to a pointer of the objectType.
	/// This overload handles specific type conversions, such as System.Boolean to byte*.
	/// </summary>
	/// <param name="objectType">The <see cref="TypeInfo"/> for the target type for the pointer.</param>
	/// <param name="arguments">The arguments to pass to the AsPointer method.</param>
	/// <returns>The needed expression to call the AsPointer method and cast to a pointer.</returns>
	internal static ExpressionSyntax AsPointer (in TypeInfo objectType, ImmutableArray<ArgumentSyntax> arguments)
	{
		// some types need to be cast to a pointer type that can be handled by the native code
#pragma warning disable format
		var castType = objectType switch {
			{ SpecialType: SpecialType.System_Boolean } => PredefinedType (Token (SyntaxKind.ByteKeyword)),
			_ => null,
		};
#pragma warning restore format
		return AsPointer (
			objectType: objectType.GetIdentifierSyntax (),
			arguments: arguments,
			castType: castType);
	}

	/// <summary>
	/// Create the necessary expression to call the GetDelegateForFunctionPointer method from the Marshal class.
	/// </summary>
	/// <param name="delegateType">The type of the delegate function pointer to cast to.</param>
	/// <param name="arguments">Arguments for the GetDelegateForFunctionPointer call.</param>
	/// <returns>The needed expression to call the GetDelegateForFunctionPointer method.</returns>
	internal static ExpressionSyntax GetDelegateForFunctionPointer (TypeSyntax delegateType,
		ImmutableArray<ArgumentSyntax> arguments)
	{
		var marshalType = StringExtensions.GetIdentifierName (
			@namespace: ["System", "Runtime", "InteropServices"],
			@class: "Marshal");
		// Marshal.GetDelegateForFunctionPointer<T>(IntPtr)
		var argsList = ArgumentList (SeparatedList<ArgumentSyntax> (arguments.ToSyntaxNodeOrTokenArray ()));
		return StaticInvocationGenericExpression (marshalType, "GetDelegateForFunctionPointer",
			delegateType, argsList);
	}

	/// <summary>
	/// Creates an <see cref="ArgumentSyntax"/> for a given parameter name and reference kind.
	/// </summary>
	/// <param name="argumentName">The name of the argument.</param>
	/// <param name="referenceKind">The <see cref="ReferenceKind"/> of the argument.</param>
	/// <returns>An <see cref="ArgumentSyntax"/> representing the parameter.</returns>
	internal static ArgumentSyntax ArgumentForParameter (string argumentName, ReferenceKind referenceKind = ReferenceKind.None)
	{
		var arg = Argument (IdentifierName (argumentName));
#pragma warning disable format
		arg = referenceKind switch {
			ReferenceKind.In => arg.WithRefOrOutKeyword (Token (SyntaxKind.InKeyword)),
			ReferenceKind.Out => arg.WithRefOrOutKeyword (Token (SyntaxKind.OutKeyword)),
			ReferenceKind.Ref => arg.WithRefOrOutKeyword (Token (SyntaxKind.RefKeyword)),
			_ => arg
		};
#pragma warning restore format
		return arg.NormalizeWhitespace ();
	}

	/// <summary>
	/// Creates an <see cref="ArgumentSyntax"/> for a given <see cref="Parameter"/>.
	/// </summary>
	/// <param name="parameter">The <see cref="Parameter"/> to create the argument for.</param>
	/// <returns>An <see cref="ArgumentSyntax"/> representing the parameter.</returns>
	internal static ArgumentSyntax ArgumentForParameter (in Parameter parameter)
		=> ArgumentForParameter (parameter.Name, parameter.ReferenceKind);

	/// <summary>
	/// Creates an <see cref="ArgumentSyntax"/> for a given <see cref="DelegateParameter"/>.
	/// </summary>
	/// <param name="parameter">The <see cref="DelegateParameter"/> to create the argument for.</param>
	/// <returns>An <see cref="ArgumentSyntax"/> representing the parameter.</returns>
	internal static ArgumentSyntax ArgumentForParameter (in DelegateParameter parameter)
		=> ArgumentForParameter (parameter.Name, parameter.ReferenceKind);
}
