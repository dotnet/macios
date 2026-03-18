using System;
using System.Linq;

using Mono.Cecil;
using Mono.Linker;
using Mono.Linker.Steps;

using Xamarin.Tuner;

#nullable enable

namespace Xamarin.Linker {
	public class MarkIProtocolHandler : BaseStep {
		public LinkerConfiguration Configuration {
			get {
				return LinkerConfiguration.GetInstance (Context);
			}
		}

		public DerivedLinkContext DerivedLinkContext {
			get {
				return Configuration.DerivedLinkContext;
			}
		}
		
		AppBundleRewriter abr => Configuration.AppBundleRewriter;

		protected override void ProcessAssembly (AssemblyDefinition assembly)
		{
			base.ProcessAssembly (assembly);

			if (DerivedLinkContext.App.Registrar != Bundler.RegistrarMode.Dynamic)
				return;

			if (Annotations.GetAction (assembly) != AssemblyAction.Link)
				return;

			if (!assembly.MainModule.HasTypes)
				return;

			if (!assembly.MainModule.HasAssemblyReferences)
				return;

			// In fact, unless an assembly is or references our platform assembly, then it won't have anything we need to register
			if (!Configuration.Profile.IsOrReferencesProductAssembly (assembly))
				return;

			abr.SetCurrentAssembly (assembly);
			var modified = false;
			foreach (var type in assembly.MainModule.Types)
				modified |= ProcessType (type);
			if (modified)
				abr.SaveCurrentAssembly ();
			abr.ClearCurrentAssembly ();
		}

		bool ProcessType (TypeDefinition type)
		{
			var modified = false;

			if (type.HasNestedTypes) {
				foreach (var nested in type.NestedTypes)
					modified |= ProcessType (nested);
			}

			if (!type.HasInterfaces)
				return modified;

			if (!type.IsNSObject (DerivedLinkContext))
				return modified;

			// If we're using the dynamic registrar, we need to mark interfaces that represent protocols
			// even if it doesn't look like the interfaces are used, since we need them at runtime.
			
			var hasProtocols = false;
			foreach (var iface in type.Interfaces) {
				var resolvedInterfaceType = iface.InterfaceType.Resolve ();
				hasProtocols = resolvedInterfaceType.HasCustomAttribute (DerivedLinkContext, Namespaces.Foundation, "ProtocolAttribute");
				if (hasProtocols)
					break;
			}

			if (!hasProtocols)
				return modified;

			var attrib = abr.CreateDynamicDependencyAttribute (DynamicallyAccessedMemberTypes.Interfaces, type);
			modified |= abr.AddAttributeToStaticConstructor (type, attrib);
			return modified;
		}
	}
}
