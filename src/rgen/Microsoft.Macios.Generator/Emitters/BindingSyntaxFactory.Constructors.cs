// Copyright (c) Microsoft Corporation.
// Licensed under the MIT License.

using System.Collections.Generic;
using System.Collections.Immutable;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Microsoft.Macios.Generator.DataModel;
using Microsoft.Macios.Generator.Extensions;
using static Microsoft.CodeAnalysis.CSharp.SyntaxFactory;
using TypeInfo = Microsoft.Macios.Generator.DataModel.TypeInfo;

namespace Microsoft.Macios.Generator.Emitters;

static partial class BindingSyntaxFactory {

	/// <summary>
	/// Generates an invocation to the `InitializeHandle` method.
	/// </summary>
	/// <param name="arguments">The arguments to pass to the `InitializeHandle` method.</param>
	/// <returns>An invocation expression syntax.</returns>
	internal static ExpressionSyntax InitializeHandle (in ImmutableArray<ArgumentSyntax> arguments)
	{
		var argumentList = ArgumentList (
			SeparatedList<ArgumentSyntax> (arguments.ToSyntaxNodeOrTokenArray ()));
		return InvocationExpression (
			IdentifierName ("InitializeHandle").WithTrailingTrivia (Space)).WithArgumentList (argumentList);
	}

	/// <summary>
	/// Generates the `objc_msgSend` invocation for a constructor.
	/// </summary>
	/// <param name="constructor">The constructor information.</param>
	/// <param name="arguments">The arguments to pass to the constructor.</param>
	/// <param name="isSuper">A value indicating whether to call the base implementation (`super`).</param>
	/// <returns>An expression syntax representing the `objc_msgSend` invocation.</returns>
	internal static ExpressionSyntax GetSendInvocation (in Constructor constructor,
		in ImmutableArray<ArgumentSyntax> arguments, bool isSuper)
	{
		var send = GetObjCMessageSendMethodName (
			exportData: constructor.ExportMethodData,
			returnType: TypeInfo.NativeHandle, // constructors always return an native handle
			parameters: constructor.Parameters,
			isSuper: isSuper,
			isStret: false // we do not need to stret for constructors since we are always getting a handle
		);
		if (send is null || constructor.ExportMethodData.Selector is null) {
			return ThrowNotImplementedException ();
		}
		// return the call to the InitializeHandle method, which will take the execution of the send method and the 
		// selector
		return InitializeHandle (
			[
				Argument (MessagingInvocation (send, constructor.ExportMethodData.Selector, arguments, isSuper)),
				GetLiteralExpressionArgument (SyntaxKind.StringLiteralExpression, constructor.ExportMethodData.Selector)
			]
		);
	}

	/// <summary>
	/// Generates the method invocations for a given constructor.
	/// </summary>
	/// <param name="constructor">The constructor for which to generate invocations.</param>
	/// <returns>A <see cref="MethodInvocations"/> object containing the generated syntax for the constructor.</returns>
	internal static MethodInvocations GetInvocations (in Constructor constructor)
	{
		var conversions = new PriorityQueue<ArgumentConversions, ArgumentInfo> (new ArgumentInfoConversionComparer ());
		var argumentSyntax = ImmutableArray.CreateBuilder<ArgumentSyntax> (constructor.Parameters.Length);
		foreach (var param in constructor.Parameters) {
			var trampolineSyntax = new ArgumentConversions {
				Initializers = GetNativeInvokeArgumentInitializations (param),
				Validations = GetNativeInvokeArgumentValidations (param),
				PreCallConversion = GetPreNativeInvokeArgumentConversions (param),
				PostCallConversion = GetPostNativeInvokeArgumentConversions (param),
			};
			conversions.Enqueue (trampolineSyntax, param);
			argumentSyntax.Add (GetNativeInvokeArgument (param));
		}

		// calculate the send expressions, we use the export data information to determine the selector
		var args = argumentSyntax.ToImmutable ();
		return new MethodInvocations () {
			Arguments = conversions.ToImmutable (),
			Send = GetSendInvocation (constructor, args, false),
			SendSuper = GetSendInvocation (constructor, args, true)
		};
	}
}
