// Copyright (c) Microsoft Corporation.
// Licensed under the MIT License.

using System;
using System.Collections.Generic;
using System.Linq;

using Mono.Cecil;
using Mono.Cecil.Cil;
using Mono.Linker;

using Xamarin.Bundler;

#nullable enable

namespace Xamarin.Linker.Steps;

public class OptimizeObjCMsgSendStep : AssemblyModifierStep {
	protected override string Name => "Optimize Objective-C Message Sends";
	protected override int ErrorCode => 2570;

	public const string Library = "/usr/lib/libobjc.dylib";

	protected override bool IsActiveFor (AssemblyDefinition assembly)
	{
		return App.Optimizations.FixedFrameObjCMsgSend == true
			&& Configuration.PublishTrimmed
			&& (Configuration.Abi & Abi.ArchMask) == Abi.ARM64
			&& (App.XamarinRuntime == XamarinRuntime.CoreCLR || App.XamarinRuntime == XamarinRuntime.NativeAOT)
			&& !App.RequiresPInvokeWrappers
			&& Annotations.GetAction (assembly) == AssemblyAction.Link;
	}

	protected override bool ProcessType (TypeDefinition type)
	{
		if (type.HasGenericParameters)
			return false;

		var modified = false;
		foreach (var method in type.Methods.ToArray ())
			modified |= ProcessMethod (method);
		return modified;
	}

	protected override bool ProcessMethod (MethodDefinition method)
	{
		if (!CanRewrite (method, out var length))
			return false;

		var check = abr.GetMethodReference (abr.PlatformAssembly, abr.ObjCRuntime_Runtime, "ThrowPendingException", v => !v.HasParameters);
		Rewrite (method, length, check);
		return true;
	}

	public static bool CanRewrite (MethodDefinition method, out int length)
	{
		length = 0;
		return !method.DeclaringType.HasGenericParameters
			&& TryComputeFrameLength (method, out length);
	}

	public static void Rewrite (MethodDefinition method, int length, MethodReference check)
	{
		var module = method.Module;
		check = module.ImportReference (check);
		var library = module.ModuleReferences.FirstOrDefault (v => v.Name == "__Internal");
		if (library is null) {
			library = new ModuleReference ("__Internal");
			module.ModuleReferences.Add (library);
		}
		var name = method.Name + "_Native";
		while (method.DeclaringType.Methods.Any (v => v.Name == name))
			name += "_";
		var arguments = method.DeclaringType.NestedTypes.FirstOrDefault (v => v.Name == "<ObjCMessageSendArguments>");
		if (arguments is null) {
			arguments = new TypeDefinition ("", "<ObjCMessageSendArguments>",
				TypeAttributes.NestedPrivate | TypeAttributes.SequentialLayout | TypeAttributes.Sealed,
				new TypeReference ("System", "ValueType", module, module.TypeSystem.CoreLibrary));
			arguments.Fields.Add (new FieldDefinition ("FrameLength", FieldAttributes.Public, module.TypeSystem.IntPtr));
			arguments.Fields.Add (new FieldDefinition ("Receiver", FieldAttributes.Public, module.TypeSystem.IntPtr));
			arguments.Fields.Add (new FieldDefinition ("IsSuper", FieldAttributes.Public, module.TypeSystem.IntPtr));
			method.DeclaringType.NestedTypes.Add (arguments);
		}
		var native = new MethodDefinition (name, MethodAttributes.Private | MethodAttributes.Static | MethodAttributes.PInvokeImpl, method.ReturnType) {
			ImplAttributes = MethodImplAttributes.PreserveSig,
			PInvokeInfo = new PInvokeInfo (method.PInvokeInfo.Attributes, "xamarin_fixed_objc_msgSend", library),
		};
		native.Parameters.Add (new ParameterDefinition ("arguments", ParameterAttributes.None, new PointerType (arguments)));
		foreach (var parameter in method.Parameters.Skip (1))
			native.Parameters.Add (new ParameterDefinition (parameter.Name, parameter.Attributes, parameter.ParameterType));
		method.DeclaringType.Methods.Add (native);

		var super = method.PInvokeInfo.EntryPoint == "objc_msgSendSuper";
		method.PInvokeInfo = null;
		method.IsPInvokeImpl = false;
		method.ImplAttributes = MethodImplAttributes.IL | MethodImplAttributes.Managed | MethodImplAttributes.AggressiveInlining;
		method.Body = new MethodBody (method) { InitLocals = true };
		var local = new VariableDefinition (arguments);
		method.Body.Variables.Add (local);
		var il = method.Body.GetILProcessor ();
		il.Emit (OpCodes.Ldloca, local);
		il.Emit (OpCodes.Ldc_I4, length);
		il.Emit (OpCodes.Conv_I);
		il.Emit (OpCodes.Stfld, arguments.Fields [0]);
		il.Emit (OpCodes.Ldloca, local);
		il.Emit (OpCodes.Ldarg, method.Parameters [0]);
		var receiverType = method.Parameters [0].ParameterType;
		if (receiverType.FullName == "ObjCRuntime.NativeHandle") {
			var conversion = receiverType.Resolve ().Methods.Single (v => v.Name == "op_Implicit"
				&& v.ReturnType.MetadataType == MetadataType.IntPtr && v.Parameters.Count == 1
				&& v.Parameters [0].ParameterType.FullName == receiverType.FullName);
			il.Emit (OpCodes.Call, module.ImportReference (conversion));
		} else {
			il.Emit (OpCodes.Conv_I);
		}
		il.Emit (OpCodes.Stfld, arguments.Fields [1]);
		il.Emit (OpCodes.Ldloca, local);
		il.Emit (super ? OpCodes.Ldc_I4_1 : OpCodes.Ldc_I4_0);
		il.Emit (OpCodes.Conv_I);
		il.Emit (OpCodes.Stfld, arguments.Fields [2]);
		il.Emit (OpCodes.Ldloca, local);
		il.Emit (OpCodes.Conv_U);
		foreach (var parameter in method.Parameters.Skip (1))
			il.Emit (OpCodes.Ldarg, parameter);
		il.Emit (OpCodes.Call, native);
		// Leave the native return value on the evaluation stack, without consuming
		// it or running binding conversion/cleanup before checking the exception.
		il.Emit (OpCodes.Call, check);
		il.Emit (OpCodes.Ret);
	}

	public static bool TryComputeFrameLength (MethodDefinition method, out int length)
	{
		length = 0;
		if (!method.IsPInvokeImpl || !method.HasPInvokeInfo || method.PInvokeInfo.Module.Name != Library)
			return false;
		if (method.PInvokeInfo.EntryPoint != "objc_msgSend" && method.PInvokeInfo.EntryPoint != "objc_msgSendSuper")
			return false;
		if (!method.IsStatic || method.HasGenericParameters || method.CallingConvention == MethodCallingConvention.VarArg
			|| !method.IsPreserveSig || method.Parameters.Count < 2
			|| method.CustomAttributes.Any (v => v.AttributeType.FullName != "ObjCRuntime.BindingImplAttribute"))
			return false;
		if (!IsPointer (method.Parameters [0].ParameterType) || !IsPointer (method.Parameters [1].ParameterType))
			return false;
		// Moving a managed byref into the descriptor would hide its pinning requirement.
		if (method.Parameters [0].ParameterType.IsByReference)
			return false;
		if (method.MethodReturnType.HasMarshalInfo || method.ReturnType.IsByReference)
			return false;
		var types = new Dictionary<string, ArgumentLayout?> ();
		if (method.ReturnType.MetadataType != MetadataType.Void && !TryGetLayout (method.ReturnType, types, out _))
			return false;

		var generalRegisters = 0;
		var vectorRegisters = 0;
		foreach (var parameter in method.Parameters) {
			if (parameter.HasMarshalInfo || !TryGetLayout (parameter.ParameterType, types, out var layout))
				return false;

			if (layout.FloatCount > 0) {
				if (vectorRegisters + layout.FloatCount <= 8) {
					vectorRegisters += layout.FloatCount;
					continue;
				}
				vectorRegisters = 8;
			} else {
				// Non-homogeneous aggregates larger than 16 bytes are passed indirectly.
				if (layout.Size > 16)
					layout = new ArgumentLayout (8, 8);
				var registers = (layout.Size + 7) / 8;
				if (generalRegisters + registers <= 8) {
					generalRegisters += registers;
					continue;
				}
				generalRegisters = 8;
			}

			// Darwin packs scalar and HFA stack arguments, but lowers other small
			// aggregates to eight-byte chunks, including their stack alignment.
			var aggregate = layout.IsAggregate && layout.FloatCount == 0;
			var stackAlignment = aggregate ? Math.Max (8, layout.Alignment) : layout.Alignment;
			var stackSize = aggregate ? Align (layout.Size, 8) : layout.Size;
			if (length > int.MaxValue - stackSize - stackAlignment - 15)
				return false;
			length = Align (length, stackAlignment) + stackSize;
		}
		length = Align (length, 16);
		return true;
	}

	static bool IsPointer (TypeReference type)
	{
		return type.IsPointer || type.IsByReference || type.MetadataType == MetadataType.IntPtr
			|| type.MetadataType == MetadataType.UIntPtr || type.FullName == "ObjCRuntime.NativeHandle";
	}

	readonly struct ArgumentLayout {
		public readonly int Size;
		public readonly int Alignment;
		public readonly int FloatSize;
		public readonly int FloatCount;
		public readonly bool IsAggregate;

		public ArgumentLayout (int size, int alignment, int floatSize = 0, int floatCount = 0, bool isAggregate = false)
		{
			Size = size;
			Alignment = alignment;
			FloatSize = floatSize;
			FloatCount = floatCount;
			IsAggregate = isAggregate;
		}
	}

	static int Align (int value, int alignment)
	{
		return (value + alignment - 1) & -alignment;
	}

	static bool TryGetLayout (TypeReference type, Dictionary<string, ArgumentLayout?> types, out ArgumentLayout layout)
	{
		layout = default;
		// Managed byrefs may marshal and copy back their elements; unsafe pointers do not.
		if (type is ByReferenceType byReference
			&& (byReference.ElementType.IsByReference || !TryGetLayout (byReference.ElementType, types, out _)))
			return false;
		if (IsPointer (type)) {
			layout = new ArgumentLayout (8, 8);
			return true;
		}
		switch (type.MetadataType) {
		case MetadataType.SByte:
		case MetadataType.Byte:
			layout = new ArgumentLayout (1, 1);
			return true;
		case MetadataType.Int16:
		case MetadataType.UInt16:
			layout = new ArgumentLayout (2, 2);
			return true;
		case MetadataType.Int32:
		case MetadataType.UInt32:
			layout = new ArgumentLayout (4, 4);
			return true;
		case MetadataType.Int64:
		case MetadataType.UInt64:
			layout = new ArgumentLayout (8, 8);
			return true;
		case MetadataType.Single:
			layout = new ArgumentLayout (4, 4, 4, 1);
			return true;
		case MetadataType.Double:
			layout = new ArgumentLayout (8, 8, 8, 1);
			return true;
		}
		if (type.FullName == "System.Runtime.InteropServices.NFloat") {
			layout = new ArgumentLayout (8, 8, 8, 1);
			return true;
		}
		if (!type.IsValueType || type is TypeSpecification || type.Namespace.StartsWith ("System.Runtime.Intrinsics", StringComparison.Ordinal)
			|| type.Namespace == "System.Numerics")
			return false;
		var key = type.FullName + ", " + type.Scope;
		if (types.TryGetValue (key, out var cached)) {
			layout = cached.GetValueOrDefault ();
			return cached.HasValue;
		}
		types [key] = null;
		var definition = type.Resolve ();
		if (definition is null)
			return false;
		if (definition.IsEnum) {
			var underlying = definition.Fields.Single (v => !v.IsStatic);
			if (!TryGetLayout (underlying.FieldType, types, out layout))
				return false;
			types [key] = layout;
			return true;
		}
		if (!definition.IsSequentialLayout || definition.HasGenericParameters
			|| (definition.PackingSize > 0 && definition.PackingSize != 8)
			|| definition.CustomAttributes.Any (v => v.AttributeType.FullName == "System.Runtime.CompilerServices.UnsafeValueTypeAttribute"
				|| v.AttributeType.FullName == "System.Runtime.CompilerServices.InlineArrayAttribute"
				|| v.AttributeType.FullName == "System.Runtime.InteropServices.Marshalling.NativeMarshallingAttribute"))
			return false;

		var size = 0;
		var alignment = 1;
		var floatSize = 0;
		var floatCount = 0;
		var homogeneous = true;
		foreach (var field in definition.Fields) {
			if (field.IsStatic)
				continue;
			if (field.HasMarshalInfo || !TryGetLayout (field.FieldType, types, out var fieldLayout))
				return false;
			size = Align (size, fieldLayout.Alignment) + fieldLayout.Size;
			alignment = Math.Max (alignment, fieldLayout.Alignment);
			if (fieldLayout.FloatCount == 0 || (floatSize != 0 && floatSize != fieldLayout.FloatSize))
				homogeneous = false;
			floatSize = fieldLayout.FloatSize;
			floatCount += fieldLayout.FloatCount;
		}
		size = Math.Max (Align (size, alignment), definition.ClassSize);
		if (size <= 0 || size > 65536)
			return false;
		homogeneous &= floatCount > 0 && floatCount <= 4 && size == floatCount * floatSize;
		layout = new ArgumentLayout (size, alignment, homogeneous ? floatSize : 0, homogeneous ? floatCount : 0, isAggregate: true);
		types [key] = layout;
		return true;
	}
}
