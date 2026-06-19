// Copyright (c) Microsoft Corporation.
// Licensed under the MIT License.

using Mono.Cecil;
using Mono.Tuner;

namespace MonoTouch.Tuner {

	// Populate FieldSymbols for InlineDlfcnMethodsStep's compatibility mode.
	// This is equivalent to what ProcessExportedFields does in the ILLink pipeline.
	public class CollectFieldsStep : ConfigurationAwareStep {
		protected override string Name { get; } = "CollectFields";
		protected override int ErrorCode { get; } = 2480;

		protected override bool IsActiveFor (AssemblyDefinition assembly)
		{
			if (!Configuration.InlineDlfcnMethodsEnabled)
				return false;

			if (!assembly.MainModule.HasTypeReference (Namespaces.Foundation + ".FieldAttribute"))
				return false;

			return true;
		}

		protected override void TryProcessAssembly (AssemblyDefinition assembly)
		{
			foreach (var type in assembly.MainModule.Types)
				CollectFieldSymbols (type);
		}

		void CollectFieldSymbols (TypeDefinition type)
		{
			if (type.HasNestedTypes) {
				foreach (var nested in type.NestedTypes)
					CollectFieldSymbols (nested);
			}

			if (!type.HasProperties)
				return;

			foreach (var property in type.Properties) {
				if (!property.HasCustomAttributes)
					continue;

				foreach (var attrib in property.CustomAttributes) {
					var declaringType = attrib.Constructor.DeclaringType.Resolve ();
					if (!declaringType.Is (Namespaces.Foundation, "FieldAttribute"))
						continue;
					if (attrib.ConstructorArguments.Count < 1)
						continue;
					Configuration.FieldSymbols.Add ((string) attrib.ConstructorArguments [0].Value);
					break;
				}
			}
		}
	}
}
