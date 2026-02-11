// Copyright (c) Microsoft Corporation.
// Licensed under the MIT License.

namespace Sharpie.Bind;

public class SdkDbTool : ObjectiveCBinder {
	public List<string> ExcludedFrameworks { get; } = new List<string> ();
	public List<string> ExtraHashImports { get; } = new List<string> ();

	public override string Tool { get => "sdk-db"; }

	public SdkDbTool ()
	{
		SplitDocuments = false;
	}

	protected override bool AddArguments (List<string> args)
	{
		foreach (var framework in ExcludedFrameworks)
			args.Add ($"-x={framework}");

		foreach (var import in ExtraHashImports)
			args.Add ($"-i={import}");

		return base.AddArguments (args);
	}

	public void PrepareArguments ()
	{
		SourceFile = GenerateImportFile (GetSdk (), ExcludedFrameworks, ExtraHashImports);
		ClangArguments.Add ("-x");
		ClangArguments.Add ("objective-c");
	}

	public override BindingResult Bind ()
	{
		PrepareArguments ();
		return base.Bind ();
	}

	string GenerateImportFile (
		Sdk sdk,
		IReadOnlyList<string> excludedFrameworks,
		IReadOnlyList<string> extraHashImports)
	{
		var importDir = Path.Combine (Path.GetTempPath (), "sharpie");
		var importFile = Path.Combine (importDir, $"sdk-db-{sdk.Identifier}.h");

		var allExcludedFrameworks = new HashSet<string> ();

		Directory.CreateDirectory (importDir);
		using (var writer = new StreamWriter (importFile)) {
			var frameworks = sdk.Frameworks.ToList ();

			// CoreHaptics need to go first, because otherwise there's a conflict with the Speech framework causing a build failure.
			// See https://github.com/xamarin/maccore/issues/2871.
			// This started happening in Xcode 16 beta 1.
			// It's still an issue in Xcode 26.2.
			var idxCoreHaptics = frameworks.FindIndex (fx => fx.Name == "CoreHaptics");
			if (idxCoreHaptics > -1) {
				var fx = frameworks [idxCoreHaptics];
				frameworks.RemoveAt (idxCoreHaptics);
				frameworks.Insert (0, fx);
			}

			foreach (var fx in frameworks) {
				if (excludedFrameworks.Contains (fx.Name)) {
					allExcludedFrameworks.Add (fx.Name);
					continue;
				}

				if (EnableModules != false) {
					if (File.Exists (fx.ModuleMap)) {
						writer.WriteLine ("@import {0}; // {1}", fx.Name, fx.Location);
					} else {
						writer.WriteLine ($"#include <{fx.Name}/{fx.Name}.h> // {fx.Location}");
					}
				} else {
					writer.WriteLine ($"#include <{fx.Name}/{fx.Name}.h> // {fx.Location}");
					if (File.Exists (fx.ModuleMap)) {
						var map = Map.Parse (this, fx.ModuleMap);
						if (map is not null) {
							var fwRoot = Path.GetDirectoryName (Path.GetDirectoryName (fx.ModuleMap))!;
							var fwHeaders = Path.Combine (fwRoot, "Headers");
							var headers = map.RootModule.GetHeaders (fwRoot, false, false).ToArray ();
							foreach (var header in headers) {
								var relativePath = Path.GetRelativePath (fwHeaders, header);
								writer.WriteLine ($"#include <{fx.Name}/{relativePath}> // from module map in {fx.Location}");
							}
						}
					}
				}
			}

			foreach (var import in extraHashImports)
				writer.WriteLine ("#import <{0}> // extra import", import);
		}

		foreach (var fx in allExcludedFrameworks.OrderBy (v => v))
			BindingResult.Log (1, $"The framework '{fx}' was excluded because it was specified in the excluded frameworks list.");
		var excludedNotFound = excludedFrameworks.Except (allExcludedFrameworks).ToList ();
		foreach (var fx in excludedNotFound)
			BindingResult.ReportWarning (14 /* The framework '{0}' was specified in the excluded frameworks list but was not found in the SDK. */, fx);

		return importFile;
	}
}
