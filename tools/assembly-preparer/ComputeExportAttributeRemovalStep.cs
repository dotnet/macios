// Copyright (c) Microsoft Corporation.
// Licensed under the MIT License.

using System.Linq;

using Xamarin.Bundler;
using Xamarin.Linker;

#nullable enable

namespace Xamarin.Linker;

public class ComputeExportAttributeRemovalStep : ConfigurationAwareStep {
	protected override string Name { get; } = "ComputeExportAttributeRemoval";
	protected override int ErrorCode { get; } = 2530;

	protected override void TryProcess ()
	{
		if (App.TrimExportAttributes == false) {
			Configuration.SetOutputForMSBuild ("TrimExportAttributes", "false");
			return;
		}

		var explicitlyEnabled = App.TrimExportAttributes == true;
		if (App.DynamicRegistrationSupported)
			App.TrimExportAttributesBlockers.Add (ExportAttributeRemovalBlocker.DynamicRegistrationSupported);
		if (App.Optimizations.OptimizeBlockLiteralSetupBlock != true)
			App.TrimExportAttributesBlockers.Add (ExportAttributeRemovalBlocker.BlockLiteralSetupBlockOptimizationDisabled);
		if (App.Optimizations.StaticBlockToDelegateLookup != true)
			App.TrimExportAttributesBlockers.Add (ExportAttributeRemovalBlocker.StaticBlockToDelegateLookupOptimizationDisabled);

		var trimExportAttributes = App.TrimExportAttributesBlockers.Count == 0;
		App.TrimExportAttributes = trimExportAttributes;
		Configuration.SetOutputForMSBuild ("TrimExportAttributes", trimExportAttributes ? "true" : "false");

		if (trimExportAttributes)
			return;

		foreach (var blocker in App.TrimExportAttributesBlockers.OrderBy (v => v)) {
			var (code, message) = GetDiagnostic (blocker);
			var exception = explicitlyEnabled
				? ErrorHelper.CreateError (code, message)
				: ErrorHelper.CreateWarning (code, message);
			Report (exception);
		}
	}

	static (int Code, string Message) GetDiagnostic (ExportAttributeRemovalBlocker blocker)
	{
		return blocker switch {
			ExportAttributeRemovalBlocker.DynamicRegistrationSupported => (4192, Errors.MX4192),
			ExportAttributeRemovalBlocker.BlockLiteralSetupBlockOptimizationDisabled => (4193, Errors.MX4193),
			ExportAttributeRemovalBlocker.StaticBlockToDelegateLookupOptimizationDisabled => (4194, Errors.MX4194),
			ExportAttributeRemovalBlocker.RuntimeGetBlockWrapperCreatorRequired => (4195, Errors.MX4195),
			ExportAttributeRemovalBlocker.RegistrarHelperGetBlockForDelegateRequired => (4196, Errors.MX4196),
			ExportAttributeRemovalBlocker.NSXpcInterfaceMethodInfoOverloadUsed => (4197, Errors.MX4197),
			_ => throw new InvalidOperationException ($"Unknown Export attribute removal blocker: {blocker}."),
		};
	}
}
