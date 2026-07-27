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
			App.TrimExportAttributesBlockers.Add ("dynamic registration is supported");
		if (App.Optimizations.OptimizeBlockLiteralSetupBlock != true)
			App.TrimExportAttributesBlockers.Add ("the blockliteral-setupblock optimization is disabled");
		if (App.Optimizations.StaticBlockToDelegateLookup != true)
			App.TrimExportAttributesBlockers.Add ("the static-block-to-delegate-lookup optimization is disabled");

		var trimExportAttributes = App.TrimExportAttributesBlockers.Count == 0;
		App.TrimExportAttributes = trimExportAttributes;
		Configuration.SetOutputForMSBuild ("TrimExportAttributes", trimExportAttributes ? "true" : "false");

		if (trimExportAttributes)
			return;

		var blockers = string.Join (", ", App.TrimExportAttributesBlockers.OrderBy (v => v));
		var exception = explicitlyEnabled
			? ErrorHelper.CreateError (4192, Errors.MX4192, blockers)
			: ErrorHelper.CreateWarning (4192, Errors.MX4192, blockers);
		Report (exception);
	}
}
