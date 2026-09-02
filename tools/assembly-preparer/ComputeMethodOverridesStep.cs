// Copyright (c) Microsoft Corporation.
// Licensed under the MIT License.

namespace MonoTouch.Tuner {
	// Register the assemblies for the method-override map. The map itself is computed lazily, the first time
	// it's queried through Annotations.GetOverrides, so we don't do the work when nothing ends up needing it.
	public class ComputeMethodOverridesStep : ConfigurationAwareStep {
		protected override string Name { get; } = "ComputeMethodOverrides";
		protected override int ErrorCode { get; } = 2490;

		protected override void TryProcess ()
		{
			Configuration.Context.Annotations.CollectOverrides (Context.Assemblies, Context);
		}
	}
}
