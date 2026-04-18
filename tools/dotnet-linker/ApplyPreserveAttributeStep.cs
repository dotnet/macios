using System.IO;
using System.Linq;
using System.Xml.Linq;

using Mono.Cecil;
using Mono.Linker;
using Mono.Linker.Steps;
using Xamarin.Bundler;
using Xamarin.Tuner;
using Xamarin.Utils;

#nullable enable

namespace Xamarin.Linker.Steps {

	public class ApplyPreserveAttributeStep : AssemblyModifierStep, IApplyPreserveAttribute {
		sealed class XmlTypeDescription {
			public XmlTypeDescription (TypeDefinition type)
			{
				Type = type;
			}

			public TypeDefinition Type { get; }
			public bool PreserveAllMembers { get; set; }
			public bool PreserveFields { get; set; }
			public bool PreserveType { get; set; }
			public Dictionary<string, bool> Fields { get; } = new (StringComparer.Ordinal);
			public Dictionary<string, bool> Methods { get; } = new (StringComparer.Ordinal);
		}

		ApplyPreserveAttributeImpl impl;
		readonly Dictionary<string, Dictionary<string, XmlTypeDescription>> xmlDescriptions = new (StringComparer.Ordinal);
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

		public bool UseXmlDescriptionFile { get; set; } = true;
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
				AddXmlDescription (type, allMembers);
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
				AddXmlDescription (onType, forMethod, conditional: true);
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

		static string GetXmlSignature (MethodDefinition method)
		{
			var marker = method.DeclaringType.FullName + "::";
			var index = method.FullName.IndexOf (marker, System.StringComparison.Ordinal);
			if (index < 0)
				return method.FullName;

			return method.FullName.Substring (0, index) + method.FullName.Substring (index + marker.Length);
		}

		XmlTypeDescription GetOrCreateXmlDescription (TypeDefinition type)
		{
			var assemblyName = type.Module.Assembly.Name.Name;
			if (!xmlDescriptions.TryGetValue (assemblyName, out var types)) {
				types = new Dictionary<string, XmlTypeDescription> (System.StringComparer.Ordinal);
				xmlDescriptions.Add (assemblyName, types);
			}

			if (!types.TryGetValue (type.FullName, out var description)) {
				description = new XmlTypeDescription (type);
				types.Add (type.FullName, description);
			}

			return description;
		}

		void AddXmlDescription (TypeDefinition type, bool allMembers)
		{
			var description = GetOrCreateXmlDescription (type);
			description.PreserveType = true;
			if (allMembers) {
				description.PreserveAllMembers = true;
				return;
			}

			if (type.IsEnum) {
				description.PreserveFields = true;
				return;
			}
		}

		void AddXmlDescription (TypeDefinition onType, MethodDefinition forMethod, bool conditional)
		{
			var description = GetOrCreateXmlDescription (onType);
			if (!conditional)
				description.PreserveType = true;
			description.Methods [GetXmlSignature (forMethod)] = conditional;
		}

		void AddUnconditionalXmlDescription (IMetadataTokenProvider provider)
		{
			switch (provider) {
			case MethodDefinition method:
				AddXmlDescription (method.DeclaringType, method, false);
				break;
			case FieldDefinition field:
				var description = GetOrCreateXmlDescription (field.DeclaringType);
				description.Fields [field.Name] = false;
				description.PreserveType = true;
				break;
			}
		}

		XElement CreateXmlTypeElement (XmlTypeDescription description)
		{
			var type = new XElement ("type", new XAttribute ("fullname", description.Type.FullName));

			if (description.PreserveAllMembers) {
				type.SetAttributeValue ("preserve", "all");
				return type;
			}

			if (description.PreserveFields && description.Fields.Count == 0 && description.Methods.Count == 0) {
				type.SetAttributeValue ("preserve", "fields");
				return type;
			}

			if (!description.PreserveType)
				type.SetAttributeValue ("required", "false");

			type.SetAttributeValue ("preserve", "nothing");

			foreach (var field in description.Fields.OrderBy (v => v.Key, System.StringComparer.Ordinal))
				type.Add (new XElement ("field", new XAttribute ("name", field.Key), new XAttribute ("required", field.Value ? "false" : "true")));

			foreach (var method in description.Methods.OrderBy (v => v.Key, System.StringComparer.Ordinal))
				type.Add (new XElement ("method", new XAttribute ("signature", method.Key), new XAttribute ("required", method.Value ? "false" : "true")));

			return type;
		}

		void WriteXmlDescription ()
		{
			var xmlPath = GetXmlDescriptionFilePath ();
			var directory = Path.GetDirectoryName (xmlPath);
			if (!string.IsNullOrEmpty (directory))
				Directory.CreateDirectory (directory);

			var document = new XDocument (
				new XElement ("linker",
					xmlDescriptions
						.OrderBy (v => v.Key, System.StringComparer.Ordinal)
						.Select (assembly => new XElement ("assembly",
							new XAttribute ("fullname", assembly.Key),
							assembly.Value
								.OrderBy (v => v.Key, System.StringComparer.Ordinal)
								.Select (v => CreateXmlTypeElement (v.Value))))));
			document.Save (xmlPath);

			if (CreateXmlDescriptionFile) {
				var items = new List<MSBuildItem> ();
				var item = new MSBuildItem (xmlPath);
				items.Add (item);
				Configuration.WriteOutputForMSBuild ("TrimmerRootDescriptor", items);
			}

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
		}
	}
}
