// Copyright (c) Microsoft Corporation.
// Licensed under the MIT License.

using Mono.Cecil;
using Mono.Cecil.Rocks;
using Mono.Linker;

using Xamarin.Bundler;
using Xamarin.Linker.Steps;

namespace Xamarin.Linker;

public class RemoveProtocolMemberPreservationStep : AssemblyModifierStep {
	protected override string Name { get; } = "RemoveProtocolMemberPreservation";
	protected override int ErrorCode { get; } = 2570;

	protected override bool IsActiveFor (AssemblyDefinition assembly)
	{
		// Match the configuration that trims ProtocolMemberAttribute in Xamarin.Shared.Sdk.targets.
		return App.Registrar == RegistrarMode.TrimmableStatic && !App.DynamicRegistrationSupported && Annotations.GetAction (assembly) == AssemblyAction.Link;
	}

	protected override bool ProcessType (TypeDefinition type)
	{
		if (!type.IsInterface || !type.HasCustomAttribute ("Foundation", "ProtocolAttribute"))
			return false;

		var cctor = type.GetStaticConstructor ();
		if (cctor is null || !cctor.IsBindingImplOptimizableCode (DerivedLinkContext))
			return false;

		// The registrar has already inspected these members. Post-processing reads missing
		// metadata from the pre-trim assemblies; runtime interface calls keep their own targets.
		return OptimizeGeneratedCode.RemoveProtocolMemberDependencies (cctor);
	}
}
