// Copyright (c) Microsoft Corporation.
// Licensed under the MIT License.

using System;
using System.Linq;

using Mono.Cecil;
using Mono.Cecil.Cil;
using Mono.Linker;
using Mono.Tuner;

using Xamarin.Bundler;
using Xamarin.Tuner;

#nullable enable

namespace Xamarin.Linker.Steps;

static class DisposeFieldClearsStep {
	public const string MarkerPrefix = "__macios_dispose_field__:";

	public static bool IsActiveFor (AssemblyDefinition assembly, AnnotationStore annotations, Xamarin.Bundler.Application app)
	{
		// NativeAOT compiles assemblies before post-processing, so restoring the field stores afterwards
		// would not affect the compiled application.
		return app.XamarinRuntime != XamarinRuntime.NativeAOT && annotations.GetAction (assembly) == AssemblyAction.Link;
	}

	public static bool IsDisposeMethod (MethodDefinition method, DerivedLinkContext context)
	{
		return method.HasParameters
			&& method.IsVirtual
			&& method.HasBody
			&& method.Name == "Dispose"
			&& method.IsBindingImplOptimizableCode (context)
			&& method.DeclaringType.IsNSObject (context);
	}
}

public class DelayDisposeFieldClearsStep : AssemblyModifierStep {
	protected override string Name { get; } = "Delay Dispose Field Clears";
	protected override int ErrorCode { get; } = 2510;

	protected override bool IsActiveFor (AssemblyDefinition assembly)
	{
		return DisposeFieldClearsStep.IsActiveFor (assembly, Annotations, App);
	}

	protected override bool ProcessType (TypeDefinition type)
	{
		return ProcessMethods (type);
	}

	protected override bool ProcessMethod (MethodDefinition method)
	{
		if (!DisposeFieldClearsStep.IsDisposeMethod (method, DerivedLinkContext))
			return false;

		var modified = false;
		var instructions = method.Body.Instructions;
		for (var i = 0; i < instructions.Count; i++) {
			var storeField = instructions [i];
			if (storeField.OpCode.Code != Code.Stfld || storeField.Operand is not FieldReference field)
				continue;

			var loadNull = storeField.Previous;
			var loadThis = loadNull?.Previous;
			if (loadNull?.OpCode.Code != Code.Ldnull || loadThis?.OpCode.Code != Code.Ldarg_0)
				continue;

			// Keep a marker in place of the store so the trimmer doesn't treat the field as live.
			loadThis.OpCode = OpCodes.Ldstr;
			loadThis.Operand = DisposeFieldClearsStep.MarkerPrefix + field.Name;
			loadNull.OpCode = OpCodes.Call;
			loadNull.Operand = abr.System_GC__KeepAlive;
			storeField.OpCode = OpCodes.Nop;
			storeField.Operand = null;
			modified = true;
		}

		return modified;
	}
}

public class RestoreDisposeFieldClearsStep : AssemblyModifierStep {
	protected override string Name { get; } = "Restore Dispose Field Clears";
	protected override int ErrorCode { get; } = 2520;

	protected override bool IsActiveFor (AssemblyDefinition assembly)
	{
		return DisposeFieldClearsStep.IsActiveFor (assembly, Annotations, App);
	}

	protected override bool ProcessType (TypeDefinition type)
	{
		return ProcessMethods (type);
	}

	protected override bool ProcessMethod (MethodDefinition method)
	{
		if (!method.HasBody)
			return false;

		var modified = false;
		var instructions = method.Body.Instructions;
		for (var i = 0; i < instructions.Count - 2; i++) {
			var loadMarker = instructions [i];
			if (loadMarker.OpCode.Code != Code.Ldstr || loadMarker.Operand is not string marker || !marker.StartsWith (DisposeFieldClearsStep.MarkerPrefix, StringComparison.Ordinal))
				continue;

			var keepAlive = instructions [i + 1];
			if (keepAlive.OpCode.Code != Code.Call || keepAlive.Operand is not MethodReference keepAliveMethod ||
				keepAliveMethod.Name != "KeepAlive" || !keepAliveMethod.DeclaringType.Is ("System", "GC") ||
				instructions [i + 2].OpCode.Code != Code.Nop)
				continue;

			var fieldName = marker.Substring (DisposeFieldClearsStep.MarkerPrefix.Length);
			var field = method.DeclaringType.Fields.FirstOrDefault (v => v.Name == fieldName);
			if (field is null) {
				loadMarker.OpCode = OpCodes.Nop;
				loadMarker.Operand = null;
				keepAlive.OpCode = OpCodes.Nop;
				keepAlive.Operand = null;
			} else {
				loadMarker.OpCode = OpCodes.Ldarg_0;
				loadMarker.Operand = null;
				keepAlive.OpCode = OpCodes.Ldnull;
				keepAlive.Operand = null;
				instructions [i + 2].OpCode = OpCodes.Stfld;
				instructions [i + 2].Operand = field;
			}
			modified = true;
		}

		return modified;
	}
}
