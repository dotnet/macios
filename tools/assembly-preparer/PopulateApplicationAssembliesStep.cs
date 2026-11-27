// Copyright (c) Microsoft Corporation.
// Licensed under the MIT License.

using Mono.Cecil;
using Mono.Linker;

using Xamarin.Build;
using Xamarin.Bundler;
using Xamarin.Linker;

namespace MonoTouch.Tuner {
	// Populate Application.Assemblies with the loaded assemblies.
	// This is needed so that steps like ExtractBindingLibrariesStep and GatherFrameworksStep
	// can find the assemblies they need to process.
	// This is equivalent to LoadNonSkippedAssembliesStep in the ILLink path (and runs at the
	// same point in the sequence of steps).
	public class PopulateApplicationAssembliesStep : ConfigurationAwareStep {
		protected override string Name { get; } = "PopulateApplicationAssemblies";
		protected override int ErrorCode { get; } = 2520;

		protected override void TryProcess ()
		{
			foreach (var assembly in Configuration.AssemblyInfos) {
				if (!assembly.IsCILAssembly)
					continue;
				var assemblyDefinition = assembly.Assembly;
				if (assemblyDefinition is null)
					continue;
				var action = Context.Annotations.GetAction (assemblyDefinition);
				switch (action) {
				case AssemblyAction.Delete:
				case AssemblyAction.Skip:
					break;
				default:
					var ad = Configuration.Application.AddAssembly (assemblyDefinition);
					ad.FullPath = assembly.OutputPath;
					break;
				}
			}
		}
	}
}
