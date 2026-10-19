// Copyright (c) Microsoft Corporation.
// Licensed under the MIT License.

using System;
using System.Collections.Generic;
using System.Linq;

using Mono.Cecil;
using Mono.Cecil.Cil;
using Mono.Linker;
using Mono.Tuner;

using Registrar;
using Xamarin.Bundler;

#nullable enable

namespace Xamarin.Linker;

public class SpecializeGenericRegistrationsStep : ConfigurationAwareStep {
	protected override string Name => "Specialize Generic Registrations";
	protected override int ErrorCode => 2430;

	protected override void TryProcess ()
	{
		if (App.Optimizations.SpecializeGenericRegistrations != true || !App.IsAnyStaticRegistrar || App.IsPostProcessingAssemblies)
			return;

		// Use a separate registrar so the regular registrar first sees the completed forwarders.
		var registrar = new StaticRegistrar (App);
		registrar.Register (Configuration.GetNonDeletedAssemblies (this));
		var modified = new HashSet<AssemblyDefinition> ();
		var helpers = new Dictionary<MethodDefinition, MethodDefinition> ();

		foreach (var objcType in registrar.Types.Values) {
			var type = objcType.Type.Resolve ();
			if (objcType.IsWrapper || objcType.IsModel || objcType.IsProtocol || objcType.IsCategory || ManagedRegistrarStep.IsOpenType (type))
				continue;
			if (Configuration.Profile.IsSdkAssembly (type.Module.Assembly))
				continue;
			if (Configuration.HotReloadCompatibleBuild && Annotations.GetAction (type.Module.Assembly) != AssemblyAction.Link)
				continue;

			var selectors = new HashSet<string> (objcType.Methods?.Where (v => !v.IsStatic).Select (v => v.Selector) ?? []);
			var baseType = type.BaseType;
			while (baseType is not null) {
				var definition = baseType.Resolve ();
				if (registrar.TryFindType (definition, out var baseObjCType) && baseObjCType.Methods is not null) {
					foreach (var method in baseObjCType.Methods) {
						if (method.Method is null || method.IsStatic || method.IsConstructor || !selectors.Add (method.Selector))
							continue;
						if (baseType is not GenericInstanceType genericType || baseObjCType.IsWrapper || baseObjCType.IsModel)
							continue;

						var target = method.Method;
						if (target.DeclaringType != definition)
							continue;
						if (!target.IsPublic && !target.IsFamily && !target.IsFamilyOrAssembly
							&& !(target.IsAssembly && target.Module.Assembly == type.Module.Assembly)) {
							if (Configuration.HotReloadCompatibleBuild && Annotations.GetAction (target.Module.Assembly) != AssemblyAction.Link)
								continue;
							if (!helpers.TryGetValue (target, out var helper)) {
								helper = CreateForwarder (target.DeclaringType, target, target.DeclaringType.CreateGenericInstanceType (target.DeclaringType.GenericParameters.ToArray ()), false);
								helpers.Add (target, helper);
								modified.Add (target.Module.Assembly);
							}
							target = helper;
						}

						var forwarder = CreateForwarder (type, target, genericType, true);
						CopyMarshallingAttributes (registrar, method, forwarder);
						AddExportAttribute (forwarder, method);
						modified.Add (type.Module.Assembly);
					}
				}
				baseType = baseType is GenericInstanceType instance && definition.BaseType is not null
					? TypeReferenceExtensions.InflateGenericType (instance, definition.BaseType)
					: definition.BaseType;
			}
		}

		foreach (var assembly in modified) {
			var abr = Configuration.AppBundleRewriter;
			abr.SetCurrentAssembly (assembly);
			abr.SaveCurrentAssembly ();
			abr.ClearCurrentAssembly ();
		}
	}

	static MethodDefinition CreateForwarder (TypeDefinition type, MethodDefinition target, GenericInstanceType declaringType, bool specialize)
	{
		var module = type.Module;
		TypeReference Import (TypeReference value) => ManagedRegistrarStep.ImportTypeReference (module, value);
		TypeReference Inflate (TypeReference value) => Import (TypeReferenceExtensions.InflateGenericType (declaringType, value));
		var name = "__Registrar_Specialized_" + type.Methods.Count;
		while (type.Methods.Any (v => v.Name == name))
			name += "_";
		var forwarder = new MethodDefinition (name, (specialize ? MethodAttributes.Private : MethodAttributes.Public) | MethodAttributes.HideBySig,
			specialize ? Inflate (target.ReturnType) : Import (target.ReturnType));
		type.Methods.Add (forwarder);
		foreach (var parameter in target.Parameters)
			forwarder.Parameters.Add (new ParameterDefinition (parameter.Name, parameter.Attributes & ~(ParameterAttributes.HasDefault | ParameterAttributes.HasFieldMarshal), specialize ? Inflate (parameter.ParameterType) : Import (parameter.ParameterType)));

		// MemberRef signatures retain !n parameters; only the declaring type is inflated.
		var reference = new MethodReference (target.Name, Import (target.ReturnType), Import (declaringType)) { HasThis = true };
		foreach (var parameter in target.Parameters)
			reference.Parameters.Add (new ParameterDefinition (Import (parameter.ParameterType)));
		var il = forwarder.Body.GetILProcessor ();
		il.Emit (OpCodes.Ldarg_0);
		foreach (var parameter in forwarder.Parameters)
			il.Emit (OpCodes.Ldarg, parameter);
		il.Emit (target.IsVirtual ? OpCodes.Callvirt : OpCodes.Call, module.ImportReference (reference));
		il.Emit (OpCodes.Ret);
		return forwarder;
	}

	static void CopyAttributes (ModuleDefinition module, ICustomAttributeProvider source, ICustomAttributeProvider destination)
	{
		foreach (var attribute in source.CustomAttributes) {
			if (attribute.AttributeType.Is ("Foundation", "ExportAttribute") || attribute.AttributeType.Is ("ObjCRuntime", "BindingImplAttribute")
				|| attribute.AttributeType.Is ("ObjCRuntime", "BindAsAttribute")
				|| attribute.AttributeType.Is ("System.Diagnostics.CodeAnalysis", "DynamicDependencyAttribute"))
				continue;
			destination.CustomAttributes.Add (CopyAttribute (module, attribute));
		}
	}

	static CustomAttribute CopyAttribute (ModuleDefinition module, CustomAttribute source)
	{
		CustomAttributeArgument ImportArgument (CustomAttributeArgument argument)
		{
			var value = argument.Value;
			if (value is TypeReference type)
				value = ManagedRegistrarStep.ImportTypeReference (module, type);
			else if (value is CustomAttributeArgument nested)
				value = ImportArgument (nested);
			else if (value is CustomAttributeArgument [] array)
				value = array.Select (ImportArgument).ToArray ();
			return new CustomAttributeArgument (module.ImportReference (argument.Type), value);
		}

		var copy = new CustomAttribute (module.ImportReference (source.Constructor));
		foreach (var argument in source.ConstructorArguments)
			copy.ConstructorArguments.Add (ImportArgument (argument));
		foreach (var property in source.Properties)
			copy.Properties.Add (new CustomAttributeNamedArgument (property.Name, ImportArgument (property.Argument)));
		foreach (var field in source.Fields)
			copy.Fields.Add (new CustomAttributeNamedArgument (field.Name, ImportArgument (field.Argument)));
		return copy;
	}

	void CopyMarshallingAttributes (StaticRegistrar registrar, Registrar.Registrar.ObjCMethod source, MethodDefinition destination)
	{
		var method = registrar.GetBaseMethodInTypeHierarchy (source.Method ?? throw new InvalidOperationException ());
		var module = destination.Module;
		CopyAttributes (module, method, destination);
		CopyAttributes (module, method.MethodReturnType, destination.MethodReturnType);
		if (!destination.ReturnType.Is ("System", "Void"))
			CopyBindAsAttribute (registrar, source, -1, destination.MethodReturnType, module);
		for (var i = 0; i < method.Parameters.Count; i++) {
			CopyAttributes (module, method.Parameters [i], destination.Parameters [i]);
			CopyBindAsAttribute (registrar, source, i, destination.Parameters [i], module);
		}
	}

	void CopyBindAsAttribute (StaticRegistrar registrar, Registrar.Registrar.ObjCMethod source, int parameterIndex, ICustomAttributeProvider destination, ModuleDefinition module)
	{
		var bindAs = registrar.GetBindAsAttribute (source, parameterIndex);
		if (bindAs is null)
			return;
		var attributeType = Configuration.AppBundleRewriter.PlatformAssembly.MainModule.GetType ("ObjCRuntime.BindAsAttribute");
		var constructor = attributeType.Methods.Single (v => v.IsConstructor && v.Parameters.Count == 1);
		var typeType = module.ImportReference (constructor.Parameters [0].ParameterType);
		var attribute = new CustomAttribute (module.ImportReference (constructor));
		attribute.ConstructorArguments.Add (new CustomAttributeArgument (typeType, ManagedRegistrarStep.ImportTypeReference (module, bindAs.Type)));
		if (bindAs.OriginalType is not null)
			attribute.Fields.Add (new CustomAttributeNamedArgument ("OriginalType", new CustomAttributeArgument (typeType, ManagedRegistrarStep.ImportTypeReference (module, bindAs.OriginalType))));
		destination.CustomAttributes.Add (attribute);
	}

	void AddExportAttribute (MethodDefinition method, Registrar.Registrar.ObjCMethod source)
	{
		var module = method.Module;
		var exportType = Configuration.AppBundleRewriter.PlatformAssembly.MainModule.GetType ("Foundation.ExportAttribute");
		var constructor = exportType.Methods.Single (v => v.IsConstructor && v.Parameters.Count == 2);
		var attribute = new CustomAttribute (module.ImportReference (constructor));
		attribute.ConstructorArguments.Add (new CustomAttributeArgument (module.TypeSystem.String, source.Selector));
		attribute.ConstructorArguments.Add (new CustomAttributeArgument (module.ImportReference (constructor.Parameters [1].ParameterType), (int) source.ArgumentSemantic));
		if (source.IsVariadic)
			attribute.Properties.Add (new CustomAttributeNamedArgument ("IsVariadic", new CustomAttributeArgument (module.TypeSystem.Boolean, true)));
		method.CustomAttributes.Add (attribute);
	}
}
