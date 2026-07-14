using System.IO;

using Mono.Cecil;
using Mono.Linker;
using Mono.Linker.Steps;
using Xamarin.Bundler;
using Xamarin.Tuner;
using Xamarin.Utils;

#nullable enable

namespace Xamarin.Linker.Steps {

	public class ApplyPreserveAttributeStep : AssemblyModifierStep, IApplyPreserveAttribute {
		ApplyPreserveAttributeImpl impl;
		readonly XmlDescriptor xmlDescriptor = new ();
		protected override string Name { get => "Apply Preserve Attribute"; }
		protected override int ErrorCode { get => 2450; }

		bool? create_xml_description_file;
		public bool CreateXmlDescriptionFile {
			get {
				if (create_xml_description_file.HasValue)
					return create_xml_description_file.Value;
				return Configuration.Application.XamarinRuntime == XamarinRuntime.NativeAOT;
			}
			set {
				create_xml_description_file = value;
			}
		}

#if ASSEMBLY_PREPARER
		public bool UseXmlDescriptionFile { get; set; }
#else
		public bool UseXmlDescriptionFile { get; set; } = true;
#endif
		public string XmlDescriptionPath { get; set; } = string.Empty;

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

		protected override void TryEndProcess ()
		{
			if (!UseXmlDescriptionFile)
				return;

			WriteXmlDescription ();
		}

		bool IApplyPreserveAttribute.PreserveUnconditional (IMetadataTokenProvider provider)
		{
			if (UseXmlDescriptionFile) {
				AddUnconditionalXmlDescription (provider);
				return false;
			}

			// We want to add a dynamic dependency attribute to preserve methods and fields
			// but not to preserve types while we're marking the chain of declaring types.
			if (provider is not TypeDefinition) {
				return AddDynamicDependencyAttribute (provider);
			}
			return false;
		}

		bool IApplyPreserveAttribute.PreserveType (TypeDefinition type, bool allMembers)
		{
			if (UseXmlDescriptionFile) {
				if (allMembers)
					xmlDescriptor.PreserveTypeWithAllMembers (type);
				else if (type.IsEnum)
					xmlDescriptor.PreserveTypeFields (type);
				else
					xmlDescriptor.PreserveType (type);
				return false;
			}

			return AddDynamicDependencyAttribute (type, allMembers);
		}

		MethodDefinition GetOrCreateModuleConstructor (ModuleDefinition @module, out bool modified)
		{
			var moduleType = @module.GetModuleType ();
			return abr.GetOrCreateStaticConstructor (moduleType, out modified);
		}

		bool IApplyPreserveAttribute.PreserveConditional (TypeDefinition onType, MethodDefinition forMethod)
		{
			if (UseXmlDescriptionFile) {
				xmlDescriptor.PreserveMethod (forMethod, required: false);
				return false;
			}

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

		string GetXmlDescriptionFilePath ()
		{
			if (!string.IsNullOrEmpty (XmlDescriptionPath))
				return XmlDescriptionPath;

			return Path.Combine (Configuration.CacheDirectory, "apply-preserve-attribute.xml");
		}

		void AddUnconditionalXmlDescription (IMetadataTokenProvider provider)
		{
			switch (provider) {
			case MethodDefinition method:
				xmlDescriptor.PreserveMethod (method);
				break;
			case FieldDefinition field:
				xmlDescriptor.PreserveField (field);
				break;
			}
		}

		void WriteXmlDescription ()
		{
			var xmlPath = GetXmlDescriptionFilePath ();
			xmlDescriptor.Save (xmlPath);

			if (CreateXmlDescriptionFile) {
				var items = new List<MSBuildItem> ();
				var item = new MSBuildItem (xmlPath);
				items.Add (item);
				Configuration.WriteOutputForMSBuild ("TrimmerRootDescriptor", items);
			}

#if !ASSEMBLY_PREPARER
			// The current linker run still needs these roots immediately. Writing the TrimmerRootDescriptor item only
			// makes the descriptor available to MSBuild after this step has already finished running.
			var applyXmlStepType = Context.GetType ().Assembly.GetType ("Mono.Linker.Steps.ResolveFromXmlStep");
			if (applyXmlStepType is not null) {
				var documentStream = File.OpenRead (xmlPath); // ResolveFromXmlStep will dispose the stream.
				var applyXmlStep = (BaseStep) Activator.CreateInstance (applyXmlStepType, new object [] { documentStream, xmlPath })!;
				applyXmlStep.Process (Context);
			} else {
				throw ErrorHelper.CreateError (99, $"Unable to find Mono.Linker.Steps.ResolveFromXmlStep to apply the generated XML description file {xmlPath}");
			}
#endif
		}
	}
}
