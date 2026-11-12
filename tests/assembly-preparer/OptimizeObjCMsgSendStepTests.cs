// Copyright (c) Microsoft Corporation.
// Licensed under the MIT License.

using Mono.Cecil.Cil;
using Xamarin.Linker.Steps;

namespace AssemblyPreparerTests;

public class OptimizeObjCMsgSendStepTests {
	static MethodDefinition CreateMethod (ModuleDefinition module, params TypeReference [] arguments)
	{
		var type = module.Types.FirstOrDefault (v => v.Name == "Messaging");
		if (type is null) {
			type = new TypeDefinition ("Test", "Messaging", TypeAttributes.Public | TypeAttributes.Abstract | TypeAttributes.Sealed);
			module.Types.Add (type);
		}
		var method = new MethodDefinition ("Send", MethodAttributes.Public | MethodAttributes.Static | MethodAttributes.PInvokeImpl, module.TypeSystem.Void) {
			ImplAttributes = MethodImplAttributes.PreserveSig,
			PInvokeInfo = new PInvokeInfo (PInvokeAttributes.CallConvCdecl, "objc_msgSend", new ModuleReference (OptimizeObjCMsgSendStep.Library)),
		};
		type.Methods.Add (method);
		method.Parameters.Add (new ParameterDefinition (module.TypeSystem.IntPtr));
		method.Parameters.Add (new ParameterDefinition (module.TypeSystem.IntPtr));
		foreach (var argument in arguments)
			method.Parameters.Add (new ParameterDefinition (argument));
		return method;
	}

	static TypeDefinition CreateStruct (ModuleDefinition module, string name, TypeReference fieldType, int count)
	{
		var type = new TypeDefinition ("Test", name, TypeAttributes.Public | TypeAttributes.SequentialLayout | TypeAttributes.Sealed, module.ImportReference (typeof (ValueType)));
		for (var i = 0; i < count; i++)
			type.Fields.Add (new FieldDefinition ("f" + i, FieldAttributes.Public, fieldType));
		module.Types.Add (type);
		return type;
	}

	static void AssertFrame (MethodDefinition method, int expected)
	{
		Assert.That (OptimizeObjCMsgSendStep.TryComputeFrameLength (method, out var length), Is.True, method.FullName);
		Assert.That (length, Is.EqualTo (expected), method.FullName);
	}

	static CustomAttribute CreateMarker (ModuleDefinition module, int options)
	{
		var type = new TypeReference ("ObjCRuntime", "BindingImplAttribute", module, module);
		var ctor = new MethodReference (".ctor", module.TypeSystem.Void, type) { HasThis = true };
		ctor.Parameters.Add (new ParameterDefinition (module.TypeSystem.Int32));
		var attribute = new CustomAttribute (ctor);
		attribute.ConstructorArguments.Add (new CustomAttributeArgument (module.TypeSystem.Int32, options));
		return attribute;
	}

	[Test]
	public void ImportsDoNotRequireGeneratedMarkersOrMessagingTypes ()
	{
		using var module = ModuleDefinition.CreateModule ("Test", ModuleKind.Dll);
		var generated = CreateMethod (module);
		generated.CustomAttributes.Add (CreateMarker (module, 3));
		var manual = CreateMethod (module);
		manual.DeclaringType.CustomAttributes.Add (CreateMarker (module, 3));
		Assert.That (OptimizeObjCMsgSendStep.CanRewrite (generated, out _), Is.True, "Generated method");
		Assert.That (OptimizeObjCMsgSendStep.CanRewrite (manual, out _), Is.True, "Manual member of generated partial class");
		manual.DeclaringType.Name = "NativeImports";
		Assert.That (OptimizeObjCMsgSendStep.CanRewrite (manual, out _), Is.True, "Other declaring type");
		generated.Name = "Send_variadic";
		Assert.That (OptimizeObjCMsgSendStep.CanRewrite (generated, out _), Is.True, "Fixed-signature variadic helper");
		manual.DeclaringType.GenericParameters.Add (new GenericParameter ("T", manual.DeclaringType));
		Assert.That (OptimizeObjCMsgSendStep.CanRewrite (manual, out _), Is.False, "Generic declaring type");
	}

	[Test]
	public void GeneralRegistersAndStack ()
	{
		using var module = ModuleDefinition.CreateModule ("Test", ModuleKind.Dll);
		for (var count = 0; count <= 17; count++) {
			var method = CreateMethod (module, Enumerable.Repeat<TypeReference> (module.TypeSystem.IntPtr, count).ToArray ());
			var expected = ((Math.Max (count - 6, 0) * 8 + 15) / 16) * 16;
			AssertFrame (method, expected);
			method.PInvokeInfo.EntryPoint = "objc_msgSendSuper";
			AssertFrame (method, expected);
		}
	}

	[Test]
	public void FloatingPointRegistersAreIndependent ()
	{
		using var module = ModuleDefinition.CreateModule ("Test", ModuleKind.Dll);
		AssertFrame (CreateMethod (module, Enumerable.Repeat<TypeReference> (module.TypeSystem.Double, 8).ToArray ()), 0);
		AssertFrame (CreateMethod (module, Enumerable.Repeat<TypeReference> (module.TypeSystem.Double, 9).ToArray ()), 16);
		var args = Enumerable.Repeat<TypeReference> (module.TypeSystem.Int64, 6)
			.Concat (Enumerable.Repeat<TypeReference> (module.TypeSystem.Double, 8)).ToArray ();
		AssertFrame (CreateMethod (module, args), 0);
		AssertFrame (CreateMethod (module, args.Concat (new [] { module.TypeSystem.Int64, module.TypeSystem.Double }).ToArray ()), 16);
	}

	[Test]
	public void DarwinStackPacking ()
	{
		using var module = ModuleDefinition.CreateModule ("Test", ModuleKind.Dll);
		var args = Enumerable.Repeat<TypeReference> (module.TypeSystem.Int64, 6)
			.Concat (Enumerable.Repeat<TypeReference> (module.TypeSystem.Byte, 17)).ToArray ();
		AssertFrame (CreateMethod (module, args), 32);
	}

	[Test]
	public void FramesAreNotLimitedToEightyBytes ()
	{
		using var module = ModuleDefinition.CreateModule ("Test", ModuleKind.Dll);
		var method = CreateMethod (module, Enumerable.Repeat<TypeReference> (module.TypeSystem.IntPtr, 64).ToArray ());
		Assert.That (OptimizeObjCMsgSendStep.CanRewrite (method, out var length), Is.True);
		Assert.That (length, Is.EqualTo (464));
	}

	[Test]
	public void SpilledAggregatesUseAbiLoweredStackSlots ()
	{
		using var module = ModuleDefinition.CreateModule ("Test", ModuleKind.Dll);
		var tiny = CreateStruct (module, "Tiny", module.TypeSystem.Byte, 3);
		var hfa = CreateStruct (module, "Hfa", module.TypeSystem.Single, 3);
		var general = Enumerable.Repeat<TypeReference> (module.TypeSystem.Int64, 6);
		var vector = Enumerable.Repeat<TypeReference> (module.TypeSystem.Double, 8);
		AssertFrame (CreateMethod (module, general.Concat (new [] { tiny })
			.Concat (Enumerable.Repeat<TypeReference> (module.TypeSystem.Byte, 9)).ToArray ()), 32);
		AssertFrame (CreateMethod (module, general.Concat (new [] { module.TypeSystem.Byte, tiny, module.TypeSystem.Byte }).ToArray ()), 32);
		AssertFrame (CreateMethod (module, general.Concat (vector).Concat (new [] { hfa })
			.Concat (Enumerable.Repeat<TypeReference> (module.TypeSystem.Byte, 4)).ToArray ()), 16);
		AssertFrame (CreateMethod (module, general.Concat (vector).Concat (new [] { module.TypeSystem.Byte, hfa, module.TypeSystem.Byte }).ToArray ()), 32);
	}

	[Test]
	public void AggregatesAndIndirectArguments ()
	{
		using var module = ModuleDefinition.CreateModule ("Test", ModuleKind.Dll);
		var pair = CreateStruct (module, "Pair", module.TypeSystem.Int64, 2);
		var large = CreateStruct (module, "Large", module.TypeSystem.Int64, 3);
		var fourDoubles = CreateStruct (module, "FourDoubles", module.TypeSystem.Double, 4);
		AssertFrame (CreateMethod (module, pair, pair, pair), 0);
		AssertFrame (CreateMethod (module, pair, pair, pair, pair), 16);
		AssertFrame (CreateMethod (module, Enumerable.Repeat<TypeReference> (large, 6).ToArray ()), 0);
		AssertFrame (CreateMethod (module, Enumerable.Repeat<TypeReference> (large, 7).ToArray ()), 16);
		AssertFrame (CreateMethod (module, fourDoubles, fourDoubles), 0);
		AssertFrame (CreateMethod (module, fourDoubles, fourDoubles, fourDoubles), 32);
		var args = Enumerable.Repeat<TypeReference> (module.TypeSystem.Int64, 5).Concat (new [] { pair, module.TypeSystem.Int64 }).ToArray ();
		AssertFrame (CreateMethod (module, args), 32);
	}

	[Test]
	public void ByReferencesRequireBlittableElements ()
	{
		using var module = ModuleDefinition.CreateModule ("Test", ModuleKind.Dll);
		var managed = CreateStruct (module, "Managed", module.TypeSystem.String, 1);
		var boolean = CreateStruct (module, "Boolean", module.TypeSystem.Boolean, 1);
		var custom = CreateStruct (module, "Custom", module.TypeSystem.Int32, 1);
		var attribute = new TypeReference ("System.Runtime.InteropServices.Marshalling", "NativeMarshallingAttribute", module, module);
		custom.CustomAttributes.Add (new CustomAttribute (new MethodReference (".ctor", module.TypeSystem.Void, attribute) { HasThis = true }));
		foreach (var type in new [] { module.TypeSystem.Boolean, module.TypeSystem.Char, module.TypeSystem.String, module.TypeSystem.Object,
			new ArrayType (module.TypeSystem.IntPtr), managed, boolean, custom }) {
			var method = CreateMethod (module, new ByReferenceType (type));
			method.CustomAttributes.Add (CreateMarker (module, 3));
			Assert.That (OptimizeObjCMsgSendStep.CanRewrite (method, out _), Is.False, type.FullName);
			AssertFrame (CreateMethod (module, new PointerType (type)), 0);
		}
		var blittable = CreateStruct (module, "Blittable", module.TypeSystem.IntPtr, 2);
		AssertFrame (CreateMethod (module, new ByReferenceType (blittable)), 0);
		AssertFrame (CreateMethod (module, new ByReferenceType (module.TypeSystem.Byte)), 0);
		AssertFrame (CreateMethod (module, new ByReferenceType (module.TypeSystem.Int64)), 0);
		var super = CreateMethod (module);
		super.Parameters [0].ParameterType = new ByReferenceType (blittable);
		super.PInvokeInfo.EntryPoint = "objc_msgSendSuper";
		Assert.That (OptimizeObjCMsgSendStep.CanRewrite (super, out _), Is.False, "Preserve managed receiver pinning");
		super.Parameters [0].ParameterType = new PointerType (blittable);
		AssertFrame (super, 0);
	}

	[Test]
	public void UnsupportedSignatures ()
	{
		using var module = ModuleDefinition.CreateModule ("Test", ModuleKind.Dll);
		foreach (var type in new [] { module.TypeSystem.Boolean, module.TypeSystem.Char, module.TypeSystem.String, new ArrayType (module.TypeSystem.IntPtr) })
			Assert.That (OptimizeObjCMsgSendStep.TryComputeFrameLength (CreateMethod (module, type), out _), Is.False, type.FullName);
		var packed = CreateStruct (module, "Packed", module.TypeSystem.Int32, 2);
		packed.PackingSize = 1;
		Assert.That (OptimizeObjCMsgSendStep.TryComputeFrameLength (CreateMethod (module, packed), out _), Is.False, "Packed struct");
		var method = CreateMethod (module);
		method.Name = "objc_msgSend_variadic";
		AssertFrame (method, 0);
		method.Name = "Send";
		method.CallingConvention = MethodCallingConvention.VarArg;
		Assert.That (OptimizeObjCMsgSendStep.TryComputeFrameLength (method, out _), Is.False, "Vararg calling convention");
		method.CallingConvention = MethodCallingConvention.Default;
		method.PInvokeInfo.Module.Name = "__Internal";
		Assert.That (OptimizeObjCMsgSendStep.TryComputeFrameLength (method, out _), Is.False, "Other library");
	}

	[TestCase (false, false)]
	[TestCase (false, true)]
	[TestCase (true, false)]
	[TestCase (true, true)]
	public void WrapperChecksImmediatelyAfterNativeCall (bool super, bool structReturn)
	{
		using var module = ModuleDefinition.CreateModule ("Test", ModuleKind.Dll);
		var method = CreateMethod (module);
		if (super)
			method.PInvokeInfo.EntryPoint = "objc_msgSendSuper";
		method.ReturnType = structReturn ? CreateStruct (module, "Result", module.TypeSystem.Int64, 3) : module.TypeSystem.IntPtr;
		var check = new MethodDefinition ("ThrowPendingException", MethodAttributes.Public | MethodAttributes.Static, module.TypeSystem.Void);
		method.DeclaringType.Methods.Add (check);
		OptimizeObjCMsgSendStep.Rewrite (method, 0, check);
		Assert.That (method.IsPInvokeImpl, Is.False, "Managed wrapper");
		var instructions = method.Body.Instructions;
		var native = (MethodDefinition) instructions [instructions.Count - 3].Operand;
		Assert.That (native.PInvokeInfo.Module.Name, Is.EqualTo ("__Internal"));
		Assert.That (native.PInvokeInfo.EntryPoint, Is.EqualTo ("xamarin_fixed_objc_msgSend"));
		Assert.That (native.IsPrivate, Is.True);
		Assert.That (native.IsPreserveSig, Is.True);
		Assert.That (native.ReturnType, Is.SameAs (method.ReturnType));
		Assert.That (native.Parameters [0].ParameterType.IsPointer, Is.True, "Descriptor pointer");
		var arguments = method.Body.Variables.Single ().VariableType.Resolve ();
		Assert.That (arguments.IsSequentialLayout, Is.True);
		Assert.That (arguments.Fields.Select (v => v.Name), Is.EqualTo (new [] { "FrameLength", "Receiver", "IsSuper" }));
		Assert.That (arguments.Fields.All (v => v.FieldType.MetadataType == MetadataType.IntPtr), Is.True, "Three native-sized fields");
		Assert.That (instructions [9].OpCode, Is.EqualTo (super ? OpCodes.Ldc_I4_1 : OpCodes.Ldc_I4_0), "Dispatch flag");
		Assert.That (instructions.TakeLast (3).Select (v => v.OpCode), Is.EqualTo (new [] { OpCodes.Call, OpCodes.Call, OpCodes.Ret }));
		Assert.That (instructions [instructions.Count - 2].Operand, Is.SameAs (check));
	}

	[Test]
	public void NativeHandleReceiverUsesItsPointerConversion ()
	{
		using var module = ModuleDefinition.CreateModule ("Test", ModuleKind.Dll);
		var handle = CreateStruct (module, "NativeHandle", module.TypeSystem.IntPtr, 1);
		handle.Namespace = "ObjCRuntime";
		var conversion = new MethodDefinition ("op_Implicit", MethodAttributes.Public | MethodAttributes.Static, module.TypeSystem.IntPtr);
		conversion.Parameters.Add (new ParameterDefinition (handle));
		handle.Methods.Add (conversion);
		var method = CreateMethod (module);
		method.Parameters [0].ParameterType = handle;
		var check = new MethodDefinition ("ThrowPendingException", MethodAttributes.Public | MethodAttributes.Static, module.TypeSystem.Void);
		method.DeclaringType.Methods.Add (check);
		OptimizeObjCMsgSendStep.Rewrite (method, 0, check);
		Assert.That (method.Body.Instructions [6].OpCode, Is.EqualTo (OpCodes.Call));
		Assert.That (((MethodReference) method.Body.Instructions [6].Operand).Resolve (), Is.SameAs (conversion));
	}
}
