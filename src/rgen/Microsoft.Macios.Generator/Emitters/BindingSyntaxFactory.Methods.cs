// Copyright (c) Microsoft Corporation.
// Licensed under the MIT License.

using System.Collections.Immutable;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Microsoft.Macios.Generator.DataModel;
using Microsoft.Macios.Generator.Extensions;
using static Microsoft.Macios.Generator.Nomenclator;
using static Microsoft.CodeAnalysis.CSharp.SyntaxFactory;
using TypeInfo = Microsoft.Macios.Generator.DataModel.TypeInfo;

namespace Microsoft.Macios.Generator.Emitters;

static partial class BindingSyntaxFactory {

	/// <summary>
	/// Generates the body for a callback lambda used in async methods.
	/// </summary>
	/// <remarks>
	/// This method creates the logic to complete a `TaskCompletionSource` based on the callback's parameters.
	/// If the last parameter is an `NSError`, it will set an exception if the error is not null, otherwise it will set the result.
	/// If there is no `NSError` parameter, it will always set the result.
	/// </remarks>
	/// <param name="delegateType">The type information of the delegate for which to generate the callback body.</param>
	/// <returns>An immutable array of statements that form the body of the callback.</returns>
	internal static StatementSyntax? GetCallbackBody (in TypeInfo delegateType)
	{
		if (delegateType.Delegate is null)
			return null;

		// tcs name
		var completionSourceName = GetTaskCompletionSourceName ();

		// get the argument list of the delegate type, if the last on is an NSError, we need a if statement to check for null
		// else, the body is just the set result for the tcs
		if (delegateType.Delegate.Parameters [^1].Type.Name.Contains ("NSError")) {
			// we are dealing with a callback that has an NSError parameter, we need to check if it is null and set the exception
			// else set the result
			var nsErrorParamName = GetTaskCallbackParameterName (delegateType.Delegate.Parameters [^1].Name);
			var newException = New (
				type: NSErrorException,
				arguments: [Argument (IdentifierName (nsErrorParamName))]);
			// create the if statement to check for null in the error parameter, this can be shared between the code paths
			// since we can add the Else clause with the WithElse method.
			// if + throw using the mono style, other methods will remove the spaces with added before the ()
			var ifErrorNotNull = IfStatement (
				attributeLists: default,
				ifKeyword: Token (SyntaxKind.IfKeyword).WithLeadingTrivia (Tab).WithTrailingTrivia (Space),
				openParenToken: Token (SyntaxKind.OpenParenToken),
				condition: IsNotNull (nsErrorParamName),
				closeParenToken: Token (SyntaxKind.CloseParenToken),
				statement: ExpressionStatement (
					TcsSetException (
						tcsVariableName: completionSourceName,
						arguments: [Argument (newException)]
					)
				).WithLeadingTrivia (LineFeed, Tab, Tab),
				@else: default);

			if (delegateType.Delegate.Parameters.Length == 1) {
				// we only have a single parameter, which is the NSError, check for null and set the exception
				return ifErrorNotNull;
			}

			// build the argument list for the result method, this params are the delegate parameters minus the last one
			var noNSErrorArgs = ImmutableArray.CreateBuilder<ArgumentSyntax> (delegateType.Delegate.Parameters.Length - 1);
			// append a new argument for each parameter except the last one
			foreach (var parameter in delegateType.Delegate.Parameters [..^1]) {
				noNSErrorArgs.Add (Argument (IdentifierName (GetTaskCallbackParameterName (parameter.Name))));
			}
			var resultsArgs = noNSErrorArgs.ToImmutable ();
			var resultArgsSyntax = resultsArgs.Length == 1
				? resultsArgs [0]
				: Argument (New (resultsArgs));

			// update the if to include the result setting, we are doing by hand to fix the indentation to match monos
			ifErrorNotNull = IfStatement (
				attributeLists: default,
				ifKeyword: Token (SyntaxKind.IfKeyword).WithLeadingTrivia (Tab).WithTrailingTrivia (Space),
				openParenToken: Token (SyntaxKind.OpenParenToken),
				condition: IsNotNull (nsErrorParamName),
				closeParenToken: Token (SyntaxKind.CloseParenToken),
				statement: ExpressionStatement (
					TcsSetException (
						tcsVariableName: completionSourceName,
						arguments: [Argument (newException)]
					)
				).WithLeadingTrivia (LineFeed, Tab, Tab),
				@else: ElseClause (
					ExpressionStatement (
						TcsSetResult (
							tcsVariableName: completionSourceName,
							arguments: [resultArgsSyntax]
						)
					).WithLeadingTrivia (LineFeed, Tab, Tab)
					).WithLeadingTrivia (LineFeed, Tab)
				);

			return ifErrorNotNull;
		}
		// no nserror as the last parameter, create the parameters list for the result and set the result directly to the tcs, not if
		// needed
		var arguments = ImmutableArray.CreateBuilder<ArgumentSyntax> (delegateType.Delegate.Parameters.Length);
		foreach (var parameter in delegateType.Delegate.Parameters) {
			arguments.Add (Argument (IdentifierName (GetTaskCallbackParameterName (parameter.Name))));
		}

		var argList = arguments.ToImmutable ();
		// return a single expression that sets the result of the tcs
		var argSyntax = argList.Length == 1
			? argList [0]
			: Argument (New (argList));

		return ExpressionStatement (
			TcsSetResult (
				tcsVariableName: completionSourceName,
				arguments: [argSyntax]
			)
		).WithLeadingTrivia (Tab);
	}

	/// <summary>
	/// Generates a lambda expression for a callback based on a delegate type.
	/// </summary>
	/// <param name="delegateType">The type information of the delegate to use for the callback.</param>
	/// <returns>An expression syntax representing the lambda expression for the callback.</returns>
	internal static ExpressionSyntax GetCallbackDeclaration (in TypeInfo delegateType)
	{
		if (delegateType.Delegate is null)
			return null!;

		// build the arguments using the delegate parameters
		var parameters = ImmutableArray.CreateBuilder<ParameterSyntax> (delegateType.Delegate.Parameters.Length);
		foreach (var parameter in delegateType.Delegate.Parameters) {
			parameters.Add (Parameter (Identifier (GetTaskCallbackParameterName (parameter.Name))));
		}

		// create the block by hand so that we keep the mono style indentation
		var block = Block (
			attributeLists: default,
			openBraceToken: (Token (SyntaxKind.OpenBraceToken).WithTrailingTrivia (LineFeed)).WithLeadingTrivia (Space),
			statements: List ([GetCallbackBody (delegateType)!]),
			closeBraceToken: Token (SyntaxKind.CloseBraceToken).WithLeadingTrivia (LineFeed));

		return ParenthesizedLambdaExpression ()
			.WithParameterList (ParameterList (
				SeparatedList<ParameterSyntax> (
					parameters.ToSyntaxNodeOrTokenArray ()))).NormalizeWhitespace ()
			.WithBlock (block);
	}

	/// <summary>
	/// Generates an invocation expression for the synchronous part of an async method wrapper.
	/// </summary>
	/// <param name="method">The method to be called.</param>
	/// <returns>An invocation expression syntax.</returns>
	internal static ExpressionSyntax ExecuteSyncCall (in Method method)
	{
		// retrieve the last parameter from the method, since that should be the completion handler
		var completionHandler = GetCallbackDeclaration (method.Parameters [^1].Type);
		var arguments = ImmutableArray.CreateBuilder<ArgumentSyntax> (method.Parameters.Length);
		// build the arguments for the method call, those are the same arguments as the method parameters but the last one
		foreach (var parameter in method.Parameters [..^1]) {
			arguments.Add (Argument (IdentifierName (parameter.Name)));
		}
		// add the completion handler as the last argument
		arguments.Add (Argument (completionHandler));

		var argumentList = ArgumentList (
			SeparatedList<ArgumentSyntax> (arguments.ToSyntaxNodeOrTokenArray ()));
		return InvocationExpression (IdentifierName (method.Name).WithTrailingTrivia (Space)).WithArgumentList (argumentList);
	}

	/// <summary>
	/// Generates an invocation expression for sending an Objective-C message.
	/// </summary>
	/// <param name="method">The method to generate the invocation for.</param>
	/// <param name="arguments">The arguments to pass to the method.</param>
	/// <param name="isSuper">A boolean value indicating whether to call the superclass implementation.</param>
	/// <returns>An expression syntax representing the message send invocation.</returns>
	internal static ExpressionSyntax GetSendInvocation (in Method method,
		in ImmutableArray<ArgumentSyntax> arguments, bool isSuper)
	{
		// calculate the send expressions, we use the export data information to determine the selector
		var returnType = method.BindAs?.Type ?? method.ReturnType;
		var send = GetObjCMessageSendMethodName (
			exportData: method.ExportMethodData,
			returnType: returnType,
			parameters: method.Parameters,
			isSuper: isSuper,
			isStret: returnType.NeedsStret
		);
		if (send is null || method.ExportMethodData.Selector is null) {
			return ThrowNotImplementedException ();
		}
		var invocation = MessagingInvocation (send, method.ExportMethodData.Selector, arguments);
		if (method.ReturnType.IsVoid)
			return invocation;
		// we need to convert the return type to the managed type and assign it to the return variable
		return AssignVariable (GetReturnVariableName (), ConvertToManaged (method, invocation)!);
	}

	/// <summary>
	/// Generates the necessary invocations and argument transformations for a given method.
	/// </summary>
	/// <param name="method">The method for which to generate invocations.</param>
	/// <returns>A <see cref="MethodInvocations"/> struct containing the argument transformations and the send invocations.</returns>
	internal static MethodInvocations GetInvocations (in Method method)
	{
		// calculate all the needed transformations for the method parameters
		var argumentsTransformations = ImmutableArray.CreateBuilder<TrampolineArgumentSyntax> (method.Parameters.Length);
		var argumentSyntax = ImmutableArray.CreateBuilder<ArgumentSyntax> (method.Parameters.Length);
		foreach (var param in method.Parameters) {
			var trampolineSyntax = new TrampolineArgumentSyntax (GetNativeInvokeArgument (param)) {
				Initializers = GetNativeInvokeArgumentInitializations (param),
				PreCallConversion = GetPreNativeInvokeArgumentConversions (param),
				PostCallConversion = GetPostNativeInvokeArgumentConversions (param),
			};
			argumentsTransformations.Add (trampolineSyntax);
			argumentSyntax.Add (trampolineSyntax.ArgumentSyntax);
		}

		// calculate the send expressions, we use the export data information to determine the selector
		var args = argumentSyntax.ToImmutable ();
		return new MethodInvocations () {
			Arguments = argumentsTransformations.ToImmutable (),
			Send = GetSendInvocation (method, args, false),
			SendSuper = GetSendInvocation (method, args, true)
		};

	}
}
