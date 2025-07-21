// Copyright (c) Microsoft Corporation.
// Licensed under the MIT License.

using System.Collections.Immutable;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Microsoft.Macios.Generator.Attributes;
using Microsoft.Macios.Generator.Availability;
using Microsoft.Macios.Generator.Context;
using Microsoft.Macios.Generator.Extensions;
using Microsoft.Macios.Generator.Formatters;
using ObjCRuntime;

namespace Microsoft.Macios.Generator.DataModel;

readonly partial struct Method {

	/// <summary>
	/// The data of the export attribute used to mark the value as a property binding. 
	/// </summary>
	public ExportData<ObjCBindings.Method> ExportMethodData { get; }


	/// <summary>
	/// Return the native selector that references the enum value.
	/// </summary>
	public string? Selector => ExportMethodData.Selector;

	/// <summary>
	/// Returns the bind from data if present in the binding.
	/// </summary>
	public BindFromData? BindAs { get; init; }

	/// <summary>
	/// Returns the forced type data if present in the binding.
	/// </summary>
	public ForcedTypeData? ForcedType { get; init; }

	/// <summary>
	/// Returns if the method was marked as thread safe.
	/// </summary>
	public bool IsThreadSafe => ExportMethodData.Flags.HasFlag (ObjCBindings.Method.IsThreadSafe);

	/// <summary>
	/// Return if the method invocation should be wrapped by a NSAutoReleasePool.
	/// </summary>
	public bool AutoRelease => ExportMethodData.Flags.HasFlag (ObjCBindings.Method.AutoRelease);

	/// <summary>
	/// True if the method was exported with the MarshalNativeExceptions flag allowing it to support native exceptions.
	/// </summary>
	public bool MarshalNativeExceptions => ExportMethodData.Flags.HasFlag (ObjCBindings.Method.MarshalNativeExceptions);

	/// <summary>
	/// True if the generator should not use a NSString for marshalling.
	/// </summary>
	public bool UsePlainString
		=> ExportMethodData.Flags.HasFlag (ObjCBindings.Method.PlainString);

	/// <summary>
	/// True if the generated code should retain the return value.
	/// </summary>
	public bool RetainReturnValue => ExportMethodData.Flags.HasFlag (ObjCBindings.Method.RetainReturnValue);

	/// <summary>
	/// True if the generated code should release the return value.
	/// </summary>
	public bool ReleaseReturnValue => ExportMethodData.Flags.HasFlag (ObjCBindings.Method.ReleaseReturnValue);

	/// <summary>
	/// True if the method was marked as a factory method.
	/// </summary>
	public bool IsFactory => ExportMethodData.Flags.HasFlag (ObjCBindings.Method.Factory);

	/// <summary>
	/// True if the return type of the method was returned as a proxy object.
	/// </summary>
	public bool IsProxy => ExportMethodData.Flags.HasFlag (ObjCBindings.Method.Proxy);

	/// <summary>
	/// True if the method was marked to be generated as an async method.
	/// </summary>
	public bool IsAsync => ExportMethodData.Flags.HasFlag (ObjCBindings.Method.Async);

	public Method (string type, string name, TypeInfo returnType,
		SymbolAvailability symbolAvailability,
		ExportData<ObjCBindings.Method> exportMethodData,
		ImmutableArray<AttributeCodeChange> attributes,
		ImmutableArray<SyntaxToken> modifiers,
		ImmutableArray<Parameter> parameters)
	{
		Type = type;
		Name = name;
		ReturnType = returnType;
		SymbolAvailability = symbolAvailability;
		ExportMethodData = exportMethodData;
		Attributes = attributes;
		Modifiers = modifiers;
		Parameters = parameters;
	}

	public static bool TryCreate (MethodDeclarationSyntax declaration, RootContext context,
		[NotNullWhen (true)] out Method? change)
	{
		if (ModelExtensions.GetDeclaredSymbol (context.SemanticModel, declaration) is not IMethodSymbol method) {
			change = null;
			return false;
		}

		var attributes = declaration.GetAttributeCodeChanges (context.SemanticModel);
		var parametersBucket = ImmutableArray.CreateBuilder<Parameter> ();
		// loop over the parameters of the construct since changes on those implies a change in the generated code
		foreach (var parameter in method.Parameters) {
			var parameterDeclaration = declaration.ParameterList.Parameters [parameter.Ordinal];
			if (!Parameter.TryCreate (parameter, parameterDeclaration, context, out var parameterChange))
				continue;
			parametersBucket.Add (parameterChange.Value);
		}

		// DO NOT USE default if null, the reason is that it will set the ArgumentSemantics to be value 0, when
		// none is value 1. The reason for that is that the default of an enum is 0, that was a mistake 
		// in the old binding code.
		var exportData = method.GetExportData<ObjCBindings.Method> ()
						 ?? new (null, ArgumentSemantic.None, ObjCBindings.Method.Default);

		change = new (
			type: method.ContainingSymbol.ToDisplayString ().Trim (), // we want the full name
			name: method.Name,
			returnType: new TypeInfo (method.ReturnType, context.Compilation),
			symbolAvailability: method.GetSupportedPlatforms (),
			exportMethodData: exportData,
			attributes: attributes,
			modifiers: [.. declaration.Modifiers],
			parameters: parametersBucket.ToImmutableArray ()) {
			BindAs = method.GetBindFromData (),
			ForcedType = method.GetForceTypeData (),
		};

		return true;
	}

	/// <summary>
	/// Converts the current method to its asynchronous version if it's marked with the `Async` flag.
	/// </summary>
	/// <returns>
	/// A new <see cref="Method"/> instance representing the asynchronous version of the method,
	/// or the current instance if the method is not marked as async.
	/// </returns>
	public Method ToAsync ()
	{
		if (!IsAsync)
			return this;

		// calculating the return type depends on the data present in the export data
		var resultType = Parameters [^1].Type.ToTask ();

		// if the user provided a result type, we need to update the calculated result type to a task
		if (ExportMethodData.ResultType is not null) {
			resultType = resultType.ToTask (ExportMethodData.ResultType.Value.GetIdentifierSyntax ().ToString ());
		}

		if (ExportMethodData.ResultTypeName is not null) {
			resultType = resultType.ToTask (ExportMethodData.ResultTypeName);
		}

		return this with {
			// update name, if user did not specify a name, use the default one
			Name = ExportMethodData.MethodName ?? $"{Name}Async",
			// remove last parameter which is the completion handler
			Parameters = [.. Parameters.SkipLast (1)],
			// update the return type to be a task
			ReturnType = resultType,
			// remove the unsafe modifier if present since our async methods are not unsafe
			Modifiers = [
				.. Modifiers.Where (m => !m.IsKind (SyntaxKind.UnsafeKeyword) && !m.IsKind (SyntaxKind.PartialKeyword)),
			]
		};
	}
}
