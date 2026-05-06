// Copyright (c) Microsoft Corporation.
// Licensed under the MIT License.

using Mono.Cecil;

#nullable enable

namespace Xamarin.Linker.Steps {
	public class OptimizeGeneratedCodeStep : AssemblyModifierStep {
		protected override string Name { get; } = "Binding Optimizer";
		protected override int ErrorCode { get; } = 2020;

		OptimizeGeneratedCodeData? data;

		protected override bool IsActiveFor (AssemblyDefinition assembly)
		{
			return OptimizeGeneratedCodeHandler.IsActiveFor (assembly, Configuration.Profile, DerivedLinkContext.Annotations);
		}

		protected override bool ProcessType (TypeDefinition type)
		{
			return ProcessMethods (type);
		}

		protected override bool ProcessMethod (MethodDefinition method)
		{
			if (data is null) {
				data = new OptimizeGeneratedCodeData {
					LinkContext = DerivedLinkContext,
					InlineIsArm64CallingConvention = App.InlineIsArm64CallingConventionForCurrentAbi,
					Optimizations = App.Optimizations,
					Device = App.IsDeviceBuild,
				};
			}
			return OptimizeGeneratedCodeHandler.OptimizeMethod (data, method);
		}
	}
}
