using System;

using Mono.Cecil;
using Mono.Linker;
using Mono.Linker.Steps;

#nullable enable

namespace Xamarin.Linker {
	public class MarkIProtocolHandler : ConfigurationAwareMarkHandler {

		protected override string Name { get; } = "IProtocol Marker";
		protected override int ErrorCode { get; } = 2420;

		public override void Initialize (LinkContext context, MarkContext markContext)
		{
			base.Initialize (context);

			if (LinkContext.Registrar == Bundler.RegistrarMode.Dynamic) {
				markContext.RegisterMarkTypeAction (ProcessType);
			}
		}

		protected override void Process (TypeDefinition type)
		{
			if (!type.HasInterfaces)
				return;

			var anyAdded = false;
			foreach (var iface in type.Interfaces) {
				var resolvedInterfaceType = iface.InterfaceType.Resolve ();
				// If we're using the dynamic registrar, we need to mark interfaces that represent protocols
				// even if it doesn't look like the interfaces are used, since we need them at runtime.
				var isProtocol = type.IsNSObject (LinkContext) && resolvedInterfaceType.HasCustomAttribute (LinkContext, Namespaces.Foundation, "ProtocolAttribute");
				if (isProtocol) {
					// Preserve the method and field on the static constructor of the type.
					abr.AddDynamicDependencyAttributeToStaticConstructor (type, resolvedInterfaceType);
					anyAdded = true;
				}
			}

#if ASSEMBLY_PREPARER
			if (anyAdded) {
				abr.SetCurrentAssembly (type.Module.Assembly);
				abr.SaveCurrentAssembly ();
			}
#else
			_ = anyAdded;
#endif
		}
	}
}
