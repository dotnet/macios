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

	public string SymbolNamespace => "ObjCRuntime";
	public string SymbolName => "Trampolines";

	bool emittedNativeObjectActionInvoker;

	static bool CanShareNativeObjectActionInvoker (in TypeInfo typeInfo)
	{
		if (typeInfo.Delegate is not { ReturnType.IsVoid: true, Parameters.Length: 1 } info)
			return false;

		// Non-nullable parameters need checks that retain the original parameter name.
		var parameter = info.Parameters [0];
		return !parameter.IsByRef && parameter.BindAs is null &&
			parameter.Type is { IsReferenceType: true, IsINativeObject: true, IsNullable: true };
	}

	void EmitNativeObjectActionInvoker (TabbedWriter<StringWriter> classBuilder)
	{
		// bgen and rgen can emit trampolines into the same assembly.
		classBuilder.WriteRaw (
@"[UnmanagedFunctionPointerAttribute (CallingConvention.Cdecl)]
unsafe delegate void RgenNativeObjectAction (IntPtr block, NativeHandle obj);

internal class RgenNativeObjectActionInvoker : TrampolineBlockBase
{
	readonly RgenNativeObjectAction invoker;

	[BindingImpl (BindingImplOptions.GeneratedCode | BindingImplOptions.Optimizable)]
	public unsafe RgenNativeObjectActionInvoker (BlockLiteral *block) : base (block)
	{
		invoker = block->GetDelegateForBlock<RgenNativeObjectAction> ();
	}

	[BindingImpl (BindingImplOptions.GeneratedCode | BindingImplOptions.Optimizable)]
	protected void Invoke (INativeObject? obj)
	{
		var handle = obj.GetHandle ();
		invoker (BlockPointer, handle);
		GC.KeepAlive (obj);
	}
}
"
		);
	}

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
		var (argumentSyntax, conversions) = GetTrampolineInvokeArguments (trampolineName, typeInfo.Delegate!);
		var delegateIdentifier = typeInfo.GetIdentifierSyntax ();
		var className = Nomenclator.GetTrampolineClassName (trampolineName, Nomenclator.TrampolineClassType.StaticBridgeClass);
		var invokeMethodName = Nomenclator.GetTrampolineInvokeMethodName ();
		var trampolineVariableName = Nomenclator.GetTrampolineDelegatePointerVariableName ();
		var delegateVariableName = Nomenclator.GetTrampolineDelegateVariableName ();
		var trampolineParameterName = Nomenclator.GetTrampolineBlockParameterName (typeInfo.Delegate!.Parameters);

		classBuilder.WriteDocumentation (Documentation.Class.TrampolineStaticClass (className));
		using (var classBlock = classBuilder.CreateBlock ($"static internal class {className}", true)) {
			// Invoke method
			classBlock.WriteLine ("[Preserve (Conditional = true)]");
			classBlock.WriteLine ("[UnmanagedCallersOnly]");
			classBlock.WriteLine ($"[UserDelegateType (typeof ({delegateIdentifier}))]");
			using (var invokeMethod = classBlock.CreateBlock (GetTrampolineInvokeSignature (typeInfo).ToString (), true)) {
				// initialized the parameters, this might be needed for the parameters that are out or ref
				foreach (var c in conversions) {
					invokeMethod.Write (c.Initializers);
				}

				// get the delegate from the block literal to execute with the trampoline
				invokeMethod.WriteLine ($"var {delegateVariableName} = {BlockLiteral}.GetTarget<{delegateIdentifier}> ({trampolineParameterName});");
				// check if the delegate is null, if it is we will throw an exception
				invokeMethod.WriteRaw (
$@"if ({delegateVariableName} is null)
	throw ErrorHelper.CreateError (8059, Errors.MX8059, {trampolineParameterName}, typeof ({delegateIdentifier}));
"
);

				// build any needed pre conversion operations before calling the delegate
				foreach (var c in conversions) {
					invokeMethod.Write (c.PreCallConversion);
				}

				invokeMethod.WriteLine ($"{CallTrampolineDelegate (typeInfo.Delegate!, argumentSyntax)}");

				// build any needed post conversion operations after calling the delegate
				foreach (var c in conversions) {
					invokeMethod.Write (c.PostCallConversion);
				}

				// perform any return conversions needed
				if (typeInfo.Delegate.ReturnType.SpecialType != SpecialType.System_Void)
					invokeMethod.WriteLine ($"return {GetTrampolineInvokeReturnType (typeInfo, Nomenclator.GetReturnVariableName ())};");
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

	public bool TryEmitNativeInvocationClass (in TypeInfo typeInfo, string trampolineName,
		TabbedWriter<StringWriter> classBuilder)
	{
		var className = Nomenclator.GetTrampolineClassName (trampolineName, Nomenclator.TrampolineClassType.NativeInvocationClass);
		var delegateName = Nomenclator.GetTrampolineClassName (trampolineName, Nomenclator.TrampolineClassType.DelegateType);
		var delegateIdentifier = Nomenclator.GetTrampolineClassName (trampolineName, Nomenclator.TrampolineClassType.DelegateType);

		var shareNativeObjectActionInvoker = CanShareNativeObjectActionInvoker (typeInfo);
		if (shareNativeObjectActionInvoker && !emittedNativeObjectActionInvoker) {
			EmitNativeObjectActionInvoker (classBuilder);
			emittedNativeObjectActionInvoker = true;
		}
		if (shareNativeObjectActionInvoker)
			delegateIdentifier = typeInfo.GetIdentifierSyntax ().ToString ();

		using (var classBlock = classBuilder.CreateBlock ($"internal sealed class {className} : {(shareNativeObjectActionInvoker ? "RgenNativeObjectActionInvoker" : "TrampolineBlockBase")}", true)) {
			if (!shareNativeObjectActionInvoker)
				classBlock.WriteLine ($"{delegateName} {Nomenclator.GetNativeInvokerVariableName ()};");
			classBlock.WriteLine (); // empty line for readability
									 // constructor
			classBlock.WriteRaw (
$@"[BindingImpl (BindingImplOptions.GeneratedCode | BindingImplOptions.Optimizable)]
public unsafe {className} ({BlockLiteral} *block) : base (block)
{{
	{(shareNativeObjectActionInvoker ? "" : $"invoker = block->GetDelegateForBlock<{delegateName}> ();")}
}}
"
			);
			classBlock.WriteLine (); // empty line for readability
									 // create method
			classBlock.WriteRaw (
$@"[Preserve (Conditional=true)]
[BindingImpl (BindingImplOptions.GeneratedCode | BindingImplOptions.Optimizable)]
public unsafe static {delegateIdentifier}? Create (IntPtr block)
{{
	if (block == IntPtr.Zero)
		return null;
	var del = ({delegateIdentifier}{(shareNativeObjectActionInvoker ? "?" : "")}) GetExistingManagedDelegate (block);
	return del ?? new {className} (({BlockLiteral} *) block).Invoke;
}}
"
);
			if (shareNativeObjectActionInvoker)
				return true;
			classBlock.WriteLine (); // empty line for readability
									 // invoke method
			using (var invokeBlock = classBlock.CreateBlock (GetTrampolineNativeInvokeSignature (typeInfo).ToString (), true)) {
				// retrieve the arguments for the invoker execution. 
				var (argumentSyntax, conversions) = GetTrampolineNativeInvokeArguments (typeInfo.Delegate!);
				foreach (var c in conversions) {
					invokeBlock.Write (c.Validations, verifyTrivia: false);
					invokeBlock.Write (c.Initializers, verifyTrivia: false);
				}
				// write the conversion code for the arguments
				foreach (var c in conversions) {
					invokeBlock.Write (c.PreCallConversion, verifyTrivia: false);
				}

				// execute the native invoker delegate
				invokeBlock.WriteLine ($"{CallNativeInvokerDelegate (typeInfo.Delegate!, argumentSyntax)}");

				// build any needed post conversion operations after calling the delegate
				foreach (var c in conversions) {
					invokeBlock.Write (c.PostCallConversion, verifyTrivia: false);
				}

				// perform any return conversions needed
				if (typeInfo.Delegate!.ReturnType.SpecialType != SpecialType.System_Void)
					invokeBlock.WriteLine ($"return {GetTrampolineNativeInvokeReturnType (typeInfo, Nomenclator.GetReturnVariableName ())};");
			}
		}

		return true;
	}

	/// <summary>
	/// Emits the delegate declaration for the trampoline.
	/// </summary>
	/// <param name="typeInfo">The type of the trampoline to generate.</param>
	/// <param name="classBuilder">The current tabbed string builder to use.</param>
	/// <param name="trampolineName">The name of the trampoline.</param>
	/// <returns>True if the code was generated, false otherwise.</returns>
	public bool TryEmitInternalDelegate (in TypeInfo typeInfo, TabbedWriter<StringWriter> classBuilder, string trampolineName)
	{
		if (typeInfo.Delegate is null)
			return false;

		var delegateName = Nomenclator.GetTrampolineClassName (trampolineName, Nomenclator.TrampolineClassType.DelegateType);
		// generate the delegate and get its name, if we already emitted it, skip it
		var delegateDeclaration = GetTrampolineDelegateDeclaration (typeInfo, delegateName);

		// print the attributes needed for the delegate and the delegate itself
		classBuilder.WriteLine ("[UnmanagedFunctionPointerAttribute (CallingConvention.Cdecl)]");
		classBuilder.WriteLine ($"[UserDelegateType (typeof ({typeInfo.GetIdentifierSyntax ()}))]");
		classBuilder.WriteLine (delegateDeclaration.ToString ());
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
		diagnostics = null;

		// write the using statements
		WriteUsedNamespaces (trampolines);

		builder.WriteLine ();
		builder.WriteLine ("namespace ObjCRuntime;");
		builder.WriteLine ();

		using (var classBlock = builder.CreateBlock ($"static partial class {SymbolName}", true)) {
			classBlock.WriteLine ($"// Generate trampolines for compilation");
			_ = context.CurrentPlatform;
			foreach (var info in trampolines) {
				var trampolineName = Nomenclator.GetTrampolineName (info);
				// write the delegate declaration
				if (!TryEmitInternalDelegate (info, classBlock, trampolineName)) {
					diagnostics = [];
					return false;
				}

				classBlock.WriteLine (); // empty line for readability

				// generate the static class
				if (!TryEmitStaticClass (info, trampolineName, classBlock)) {
					diagnostics = [];
					return false;
				}

				if (!TryEmitNativeInvocationClass (info, trampolineName, classBlock)) {
					diagnostics = [];
					return false;
				}
				// generate the native invoker class
				classBlock.WriteLine ();
			}
		}

		return true;
	}
}
