// Copyright (c) Microsoft Corporation.
// Licensed under the MIT License.

using System.Collections.Immutable;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Microsoft.Macios.Generator.DataModel;
using static Microsoft.Macios.Generator.Nomenclator;

namespace Microsoft.Macios.Generator.Emitters;

static partial class BindingSyntaxFactory {

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
