// Copyright (c) Microsoft Corporation.
// Licensed under the MIT License.
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Diagnostics.CodeAnalysis;
using System.IO;
using System.Linq;
using Microsoft.CodeAnalysis;
using Microsoft.Macios.Generator.Context;
using Microsoft.Macios.Generator.Formatters;
using Microsoft.Macios.Generator.IO;
using TypeInfo = Microsoft.Macios.Generator.DataModel.TypeInfo;
using static Microsoft.Macios.Generator.Emitters.BindingSyntaxFactory;

namespace Microsoft.Macios.Generator.Emitters;

class TrampolineEmitter (
	RootContext context,
	TabbedStringBuilder builder) {
	/// <summary>
	/// The nomenclator is used to generate the name of the static class that will contain the trampoline, needs to
	/// be an instance class since we want to keep track of the already generated names.
	/// </summary>
	Nomenclator nomenclator = new ();

	public string SymbolNamespace => "ObjCRuntime";
	public string SymbolName => "Trampolines";

	/// <summary>
	/// Generate the static helper class for the trampoline uses to bridge the native block invocation.
	/// </summary>
	/// <param name="typeInfo">The type info of the trampoline to generate.</param>
	/// <param name="trampolineName">The trampoline name.</param>
	/// <param name="classBuilder">The tabbed string builder to use.</param>
	/// <returns>True if the code was generated, false otherwise.</returns>
	public bool TryEmitStaticClass (in TypeInfo typeInfo, string trampolineName, TabbedWriter<StringWriter> classBuilder)
	{
		// create a new static class using the name from the nomenclator
		var argumentSyntax = GetTrampolineInvokeArguments (trampolineName, typeInfo.Delegate!);
		var delegateIdentifier = typeInfo.GetIdentifierSyntax ();
		var className = Nomenclator.GetTrampolineClassName (trampolineName, Nomenclator.TrampolineClassType.StaticBridgeClass);
		var invokeMethodName = Nomenclator.GetTrampolineInvokeMethodName ();
		var trampolineVariableName = Nomenclator.GetTrampolineDelegatePointerVariableName ();
		var delegateVariableName = Nomenclator.GetTrampolineDelegateVariableName ();

		classBuilder.WriteDocumentation (Documentation.Class.TrampolineStaticClass (className));
		using (var classBlock = classBuilder.CreateBlock ($"static internal class {className}", true)) {
			// Invoke method
			classBlock.WriteLine ("[Preserve (Conditional = true)]");
			classBlock.WriteLine ("[UnmanagedCallersOnly]");
			classBlock.WriteLine ($"[UserDelegateType (typeof ({delegateIdentifier}))]");
			using (var invokeMethod = classBlock.CreateBlock (GetTrampolineInvokeSignature (typeInfo).ToString (), true)) {
				// initialized the parameters, this might be needed for the parameters that are out or ref
				foreach (var argument in argumentSyntax) {
					invokeMethod.Write (argument.Initializers);
				}

				// get the delegate from the block literal to execute with the trampoline
				invokeMethod.WriteLine ($"var {delegateVariableName} = {BlockLiteral}.GetTarget<{delegateIdentifier}> ({Nomenclator.GetTrampolineBlockParameterName (typeInfo.Delegate!.Parameters)});");

				// if the deletate is null, we return default, otherwise we call the delegate
				using (var ifBlock = invokeMethod.CreateBlock ($"if ({delegateVariableName} is not null)", true)) {

					// build any needed pre conversion operations before calling the delegate
					foreach (var argument in argumentSyntax) {
						ifBlock.Write (argument.PreDelegateCallConversion);
					}

					ifBlock.WriteLine ($"{CallTrampolineDelegate (typeInfo.Delegate!, argumentSyntax)}");

					// build any needed post conversion operations after calling the delegate
					foreach (var argument in argumentSyntax) {
						ifBlock.Write (argument.PostDelegateCallConversion);
					}

					// perform any needed
					if (typeInfo.Delegate.ReturnType.SpecialType != SpecialType.System_Void)
						ifBlock.WriteLine ($"return {GetTrampolineInvokeReturnType (typeInfo, Nomenclator.GetReturnVariableName ())};");
				}
				if (typeInfo.Delegate.ReturnType.SpecialType != SpecialType.System_Void)
					invokeMethod.WriteLine ("return default;");
			}

			// CreateNullableBlock
			classBlock.WriteLine (); // empty line for readability
			using (var createNullableBlock = classBlock.CreateBlock (
					   $"internal static unsafe {BlockLiteral} CreateNullableBlock ({delegateIdentifier}? callback)", true)) {
				createNullableBlock.WriteRaw (
$@"if (callback is null)
	return default ({BlockLiteral});
return CreateBlock (callback);
"
				);
			}

			// CreateBlock
			classBlock.WriteLine (); // empty line for readability
			classBlock.WriteLine ("[BindingImpl (BindingImplOptions.GeneratedCode | BindingImplOptions.Optimizable)]");
			using (var createBlock = classBlock.CreateBlock (
					   $"internal static unsafe {BlockLiteral} CreateBlock ({delegateIdentifier} callback)", true)) {
				createBlock.WriteLine (GetTrampolineDelegatePointer (typeInfo).ToFullString ());
				createBlock.WriteLine (
					$"return new {BlockLiteral} ({trampolineVariableName}, callback, typeof ({className}), nameof ({invokeMethodName}));");
			}
		}
		return true;
	}

	/// <summary>
	/// Emits the delegate declaration for the trampoline.
	/// </summary>
	/// <param name="typeInfo">The type of the trampoline to generate.</param>
	/// <param name="classBuilder">The current tabbed string builder to use.</param>
	/// <param name="addedDelegates">A hash set with the delegates already added.</param>
	/// <param name="delegateName">The delegate name.</param>
	/// <returns>True if the code was generated, false otherwise.</returns>
	public bool TryEmitInternalDelegate (in TypeInfo typeInfo, TabbedWriter<StringWriter> classBuilder, HashSet<string> addedDelegates,
		[NotNullWhen (true)] out string? delegateName)
	{
		delegateName = null;
		if (typeInfo.Delegate is null)
			return false;

		// generate the delegate and get its name, if we already emitted it, skip it
		var delegateDeclaration = GetTrampolineDelegateDeclaration (typeInfo, out delegateName);
		if (addedDelegates.Add (delegateName)) {
			// print the attributes needed for the delegate and the delegate itself
			classBuilder.WriteLine ("[UnmanagedFunctionPointerAttribute (CallingConvention.Cdecl)]");
			classBuilder.WriteLine ($"[UserDelegateType (typeof ({typeInfo.GetIdentifierSyntax ()}))]");
			classBuilder.WriteLine (delegateDeclaration.ToString ());
		}
		return true;
	}

	/// <summary>
	/// Write the using statements for the namespaces used by the trampolines.
	/// </summary>
	/// <param name="trampolines">The trampolines to generate.</param>
	public void WriteUsedNamespaces (IReadOnlySet<TypeInfo> trampolines)
	{
		// create set with the default namespaces
		var namespaces = new HashSet<string> () {
			"Foundation",
			"ObjCBindings",
			"ObjCRuntime",
			"System",
		};
		// loop through the trampolines and add the namespaces used by the delegates
		foreach (var info in trampolines)
			namespaces.Add (string.Join ('.', info.Namespace));

		// sort the namespaces so that we can write them in a deterministic way
		foreach (var ns in namespaces.OrderBy (x => x)) {
			builder.WriteLine ($"using {ns};");
		}
	}

	/// <summary>
	/// Generate the trampolines for the given set of types.
	/// </summary>
	/// <param name="trampolines">The trampolines type info to use for the code generation.</param>
	/// <param name="diagnostics">Possible diagnostic errors.</param>
	/// <returns>True if the code was generated, false otherwise.</returns>
	public bool TryEmit (IReadOnlySet<TypeInfo> trampolines,
		[NotNullWhen (false)] out ImmutableArray<Diagnostic>? diagnostics)
	{
		// TODO: actual trampoline generation, for the current time write the using statements, namespace, class
		// and some comments with the trampolines to emit
		diagnostics = null;

		// write the using statements
		WriteUsedNamespaces (trampolines);

		builder.WriteLine ();
		builder.WriteLine ("namespace ObjCRuntime;");
		builder.WriteLine ();

		// keep track of the already emitted delegates
		var addedDelegates = new HashSet<string> ();

		using (var classBlock = builder.CreateBlock ($"static partial class {SymbolName}", true)) {
			classBlock.WriteLine ($"// Generate trampolines for compilation");
			_ = context.CurrentPlatform;
			foreach (var info in trampolines) {
				var trampolineName = nomenclator.GetTrampolineName (info);
				// write the delegate declaration
				if (!TryEmitInternalDelegate (info, classBlock, addedDelegates, out var delegateDeclaration)) {
					diagnostics = [];
					return false;
				}

				classBlock.WriteLine (); // empty line for readability

				// generate the static class
				if (!TryEmitStaticClass (info, trampolineName, classBlock)) {
					diagnostics = [];
					return false;
				}

				classBlock.WriteLine ($"// TODO: generate trampoline for {info.FullyQualifiedName}");
				classBlock.WriteLine ();
			}
		}

		return true;
	}
}
