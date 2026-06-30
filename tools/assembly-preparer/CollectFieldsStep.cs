// Copyright (c) Microsoft Corporation.
// Licensed under the MIT License.

using Mono.Cecil;
using Mono.Linker;
using Mono.Tuner;

using Xamarin.Linker;

namespace MonoTouch.Tuner {

	// Populate FieldSymbols and ExportedFields annotation for InlineDlfcnMethodsStep
	// and ListExportedSymbols. This is equivalent to what ProcessExportedFields does
	// in the ILLink pipeline.
	public class CollectFieldsStep : ConfigurationAwareStep {
		protected override string Name { get; } = "CollectFields";
		protected override int ErrorCode { get; } = 2480;

		protected override bool IsActiveFor (AssemblyDefinition assembly)
		{
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

					var symbolName = (string) attrib.ConstructorArguments [0].Value;

					// Populate FieldSymbols for InlineDlfcnMethodsStep's compatibility mode.
					if (Configuration.InlineDlfcnMethodsEnabled)
						Configuration.FieldSymbols.Add (symbolName);

					// Populate ExportedFields annotation for ListExportedSymbols
					// (so native linker keeps __Internal field symbols for dlsym).
					if (attrib.ConstructorArguments.Count == 2) {
						var libraryName = (string) attrib.ConstructorArguments [1].Value;
						if (libraryName == "__Internal")
							Annotations.GetCustomAnnotations ("ExportedFields") [property] = symbolName;
					}

					break;
				}
			}
		}
	}
}
