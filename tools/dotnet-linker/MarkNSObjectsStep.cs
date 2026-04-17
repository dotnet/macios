// Copyright (c) Microsoft Corporation.
// Licensed under the MIT License.

using Mono.Cecil;
using Mono.Linker;

using Xamarin.Tuner;

#nullable enable

namespace Xamarin.Linker.Steps {
	public class MarkNSObjectsStep : AssemblyModifierStep, IMarkNSObjects {
		protected override string Name { get; } = "MarkNSObjects";
		protected override int ErrorCode { get; } = 2080;

		protected override bool IsActiveFor (AssemblyDefinition assembly)
		{
			// We only care about assemblies that are being linked.
			if (Annotations.GetAction (assembly) != AssemblyAction.Link)
				return false;

			// We only care about assemblies that is or references our product assembly, because those are the only
			// assemblies that can have NSObject subclasses.
			if (!Configuration.Profile.IsOrReferencesProductAssembly (assembly))
				return false;

			return true;
		}

		protected override void TryProcess ()
		{
			DerivedLinkContext.DidRunMarkNSObjectsStep = true;
			base.TryProcess ();
		}

		AnnotationStore IMarkNSObjects.Annotations => Annotations;
		DerivedLinkContext IMarkNSObjects.Context => Configuration.DerivedLinkContext;

		protected override bool ProcessType (TypeDefinition type)
		{
			return MarkNSObjectsImpl.ProcessType (this, type);
		}

		public bool PreserveType (TypeDefinition type, bool allMembers)
		{
			var moduleConstructor = abr.GetOrCreateStaticConstructor (type.Module.GetModuleType (), out var modified);
			var attrib = CreateDynamicDependencyAttribute (type, allMembers);
			modified |= abr.AddAttributeOnlyOnce (moduleConstructor, attrib);
			return modified;
		}

		public bool PreserveType (TypeDefinition onType, TypeDefinition type)
		{
			return abr.AddDynamicDependencyAttributeToStaticConstructor (onType, type);
		}

		// We want to avoid `DynamicallyAccessedMemberTypes.All` because it preserves nested types.
		// `All` would also keep unused private members of base types.
		const DynamicallyAccessedMemberTypes allMemberTypes =
			DynamicallyAccessedMemberTypes.PublicFields | DynamicallyAccessedMemberTypes.NonPublicFields
			| DynamicallyAccessedMemberTypes.PublicProperties | DynamicallyAccessedMemberTypes.NonPublicProperties
			| DynamicallyAccessedMemberTypes.PublicMethods | DynamicallyAccessedMemberTypes.NonPublicMethods
			| DynamicallyAccessedMemberTypes.PublicConstructors | DynamicallyAccessedMemberTypes.NonPublicConstructors
			| DynamicallyAccessedMemberTypes.PublicEvents | DynamicallyAccessedMemberTypes.NonPublicEvents
			| DynamicallyAccessedMemberTypes.Interfaces;

		CustomAttribute CreateDynamicDependencyAttribute (TypeDefinition type, bool allMembers)
		{
			var members = allMembers
				? allMemberTypes
				: DynamicallyAccessedMemberTypes.PublicConstructors | DynamicallyAccessedMemberTypes.NonPublicConstructors;

			return abr.CreateDynamicDependencyAttribute (members, type);
		}

		public bool PreserveMethod (TypeDefinition onType, MethodDefinition method)
		{
			return abr.AddDynamicDependencyAttributeToStaticConstructor (onType, method);
		}
	}
}
