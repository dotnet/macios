using Mono.Cecil;
using Mono.Linker;

using Xamarin.Bundler;
using Xamarin.Tuner;
using Xamarin.Utils;

#nullable enable

namespace Xamarin.Linker.Steps {

	public class ApplyPreserveAttributeStep : AssemblyModifierStep, IApplyPreserveAttribute {
		ApplyPreserveAttributeImpl impl;
		protected override string Name { get => "Apply Preserve Attribute"; }
		protected override int ErrorCode { get => 2450; }

		public ApplyPreserveAttributeStep ()
		{
			impl = new ApplyPreserveAttributeImpl (this);
		}

		protected override void TryProcess ()
		{
			DerivedLinkContext.DidRunApplyPreserveAttributeStep = true;
			base.TryProcess ();
		}

		protected override bool IsActiveFor (AssemblyDefinition assembly)
		{
			// We only care about assemblies that are being linked.
			if (Annotations.GetAction (assembly) != AssemblyAction.Link)
				return false;

			return true;
		}

		protected override bool ModifyAssembly (AssemblyDefinition assembly)
		{
			return impl.Process (assembly);
		}

		bool IApplyPreserveAttribute.PreserveUnconditional (IMetadataTokenProvider provider)
		{
			// We want to add a dynamic dependency attribute to preserve methods and fields
			// but not to preserve types while we're marking the chain of declaring types.
			if (provider is not TypeDefinition) {
				return AddDynamicDependencyAttribute (provider);
			}
			return false;
		}

		bool IApplyPreserveAttribute.PreserveType (TypeDefinition type, bool allMembers)
		{
			return AddDynamicDependencyAttribute (type, allMembers);
		}

		MethodDefinition GetOrCreateModuleConstructor (ModuleDefinition @module, out bool modified)
		{
			var moduleType = @module.GetModuleType ();
			return abr.GetOrCreateStaticConstructor (moduleType, out modified);
		}

		bool IApplyPreserveAttribute.PreserveConditional (TypeDefinition onType, MethodDefinition forMethod)
		{
			return AddConditionalDynamicDependencyAttribute (onType, forMethod);
		}

		// We want to avoid `DynamicallyAccessedMemberTypes.All` because the semantics are different
		// from `[Preserve (AllMembers = true)]`. Specifically, we don't want to preserve nested types.
		// `All` would also keep unused private members of base types which `Preserve` also doesn't cover.
		const DynamicallyAccessedMemberTypes allMemberTypes =
			DynamicallyAccessedMemberTypes.PublicFields | DynamicallyAccessedMemberTypes.NonPublicFields
			| DynamicallyAccessedMemberTypes.PublicProperties | DynamicallyAccessedMemberTypes.NonPublicProperties
			| DynamicallyAccessedMemberTypes.PublicMethods | DynamicallyAccessedMemberTypes.NonPublicMethods
			| DynamicallyAccessedMemberTypes.PublicConstructors | DynamicallyAccessedMemberTypes.NonPublicConstructors
			| DynamicallyAccessedMemberTypes.PublicEvents | DynamicallyAccessedMemberTypes.NonPublicEvents
			| DynamicallyAccessedMemberTypes.Interfaces;

		bool AddDynamicDependencyAttribute (TypeDefinition type, bool allMembers)
		{
			var moduleConstructor = GetOrCreateModuleConstructor (abr.CurrentAssembly.MainModule, out var modified);
			var members = allMembers
				? allMemberTypes
				: DynamicallyAccessedMemberTypes.PublicConstructors | DynamicallyAccessedMemberTypes.NonPublicConstructors;

			// only preserve fields for enums
			if (type.IsEnum) {
				members = DynamicallyAccessedMemberTypes.PublicFields;
			}

			var attrib = abr.CreateDynamicDependencyAttribute (members, type);
			modified |= abr.AddAttributeOnlyOnce (moduleConstructor, attrib);
			return modified;
		}

		bool AddConditionalDynamicDependencyAttribute (TypeDefinition onType, MethodDefinition forMethod)
		{
			return abr.AddDynamicDependencyAttributeToStaticConstructor (onType, forMethod);
		}

		bool AddDynamicDependencyAttribute (IMetadataTokenProvider provider)
		{
			var member = provider as IMemberDefinition;
			if (member is null)
				throw ErrorHelper.CreateError (99, $"Unable to add dynamic dependency attribute to {provider.GetType ().FullName}");

			var moduleConstructor = GetOrCreateModuleConstructor (member.GetModule (), out var modified);
			var signature = DocumentationComments.GetSignature (member);
			var attrib = abr.CreateDynamicDependencyAttribute (signature, member.DeclaringType);
			modified |= abr.AddAttributeOnlyOnce (moduleConstructor, attrib);
			return modified;
		}
	}
}
