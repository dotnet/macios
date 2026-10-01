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
			if (allMembers) {
				modified |= abr.AddPreserveAllMembersDynamicDependencyAttributes (moduleConstructor, type);
				return modified;
			}
			var attrib = abr.CreateDynamicDependencyAttribute (DynamicallyAccessedMemberTypes.PublicConstructors | DynamicallyAccessedMemberTypes.NonPublicConstructors, type);
			modified |= abr.AddAttributeOnlyOnce (moduleConstructor, attrib);
			return modified;
		}

		public bool PreserveType (TypeDefinition onType, TypeDefinition type)
		{
			return abr.AddDynamicDependencyAttributeToStaticConstructor (onType, type);
		}

		public bool PreserveMethod (TypeDefinition onType, MethodDefinition method)
		{
			return abr.AddDynamicDependencyAttributeToStaticConstructor (onType, method);
		}
	}
}
