using System;

using Mono.Cecil;
using Mono.Linker;
using Mono.Linker.Steps;

using Xamarin.Tuner;

#nullable enable

namespace Xamarin.Linker {

	// If we're using the dynamic registrar, we need to mark interfaces that represent protocols
	// even if it doesn't look like the interfaces are used, since we need them at runtime.
	//
	// Sadly, there doesn't seem to be a way to preserve only some interfaces, we have to preserve them all.
	// We add this to the current type's static cctor:
	//
	//     [DynamicDependency (DynamicallyAccessedMemberTypes.Interfaces, typeof (TheClass))]
	//     static TheClass ()
	//     {
	//     }
	//
	// This step is a replacement for the MarkIProtocolHandler step (which can't be moved out of the linker).
	//
	// One difference with the MarkIProtocolHandler step is that this step will preserve all interfaces for
	// a type, while MarkIProtocolHandler will only preserve protocol interfaces. Taking into account that
	// preserving protocol interfaces is only needed when using the dynamic registrar, and that's not our
	// most optimized build configuration (nor the default release configuration on any platform), this should
	// hopefully not be a big issue).
	//
	public class PreserveProtocolsStep : BaseStep {
		AppBundleRewriter abr => Configuration.AppBundleRewriter;

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
