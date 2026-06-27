// Copyright (c) Microsoft Corporation.
// Licensed under the MIT License.

namespace MonoTouch.Tuner {
	// Compute the map of which methods override which other methods, and store it in the Annotations.
	public class ComputeMethodOverridesStep : ConfigurationAwareStep {
		protected override string Name { get; } = "ComputeMethodOverrides";
		protected override int ErrorCode { get; } = 2490;

		protected override void TryProcess ()
		{
			Configuration.Context.Annotations.CollectOverrides (Context.Assemblies, Context);
		}
	}
}
