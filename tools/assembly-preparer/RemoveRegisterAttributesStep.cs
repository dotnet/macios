// Copyright (c) Microsoft Corporation.
// Licensed under the MIT License.

using Mono.Cecil;
using Mono.Linker;

using Xamarin.Bundler;
using Xamarin.Linker.Steps;

namespace Xamarin.Linker;

public class RemoveRegisterAttributesStep : AssemblyModifierStep {
	protected override string Name { get; } = "RemoveRegisterAttributes";
	protected override int ErrorCode { get; } = 2540;

	protected override bool IsActiveFor (AssemblyDefinition assembly)
	{
		return App.CanTrimRegistrarAttributes
			&& App.Registrar == RegistrarMode.TrimmableStatic
			&& !App.IsPostProcessingAssemblies
			&& Annotations.GetAction (assembly) == AssemblyAction.Link;
	}

	protected override bool ProcessType (TypeDefinition type)
	{
		if (DerivedLinkContext.IsDirectBindingValue?.TryGetValue (type, out var isDirectBinding) != true || isDirectBinding != true)
			return false;

		var modified = false;
		for (var i = type.CustomAttributes.Count - 1; i >= 0; i--) {
			var attribute = type.CustomAttributes [i];
			if (attribute.AttributeType.Namespace != "Foundation" || attribute.AttributeType.Name != "RegisterAttribute")
				continue;

			DerivedLinkContext.StoreCustomAttribute (type, attribute);
			type.CustomAttributes.RemoveAt (i);
			modified = true;
		}
		return modified;
	}
}
