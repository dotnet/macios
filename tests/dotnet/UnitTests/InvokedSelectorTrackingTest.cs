// Copyright (c) Microsoft Corporation.
// Licensed under the MIT License.

#nullable enable

namespace Xamarin.Tests {
	[TestFixture]
	public class InvokedSelectorTrackingTest : TestBaseClass {
		[TestCase (true, true, false)]
		[TestCase (true, false, false)]
		[TestCase (false, false, false)]
		[TestCase (true, true, true)]
		[TestCase (true, false, true)]
		[TestCase (false, false, true)]
		public void IncrementalBuild (bool prepare, bool postprocess, bool referenced)
		{
			var platform = ApplePlatform.MacCatalyst;
			var runtimeIdentifier = RuntimeInformation.ProcessArchitecture == Architecture.Arm64 ? "maccatalyst-arm64" : "maccatalyst-x64";
			Configuration.IgnoreIfIgnoredPlatform (platform);
			Configuration.AssertRuntimeIdentifiersAvailable (platform, runtimeIdentifier);
			var projectPath = GetProjectPath ("MySimpleApp", platform: platform);
			Clean (projectPath);
			var properties = GetDefaultProperties (runtimeIdentifier);
			properties ["PrepareAssemblies"] = prepare.ToString ();
			properties ["PostProcessAssemblies"] = postprocess.ToString ();
			properties ["MtouchLink"] = "SdkOnly";
			properties ["Registrar"] = "static";
			if (referenced)
				properties ["AdditionalDefineConstants"] = "INVOKED_SELECTOR_TRACKING";

			var objDir = GetObjDir (projectPath, platform, runtimeIdentifier, "Debug");
			foreach (var enabled in new string? [] { null, "true", "false" }) {
				if (enabled is not null)
					properties ["EnableInvokedSelectorTracking"] = enabled;
				if (referenced && enabled == "false") {
					var failure = DotNet.AssertBuildFailure (projectPath, properties);
					Assert.That (BinLog.GetBuildLogErrors (failure.BinLogPath).Any (error => error.Code == "MT2388"), Is.True, "Disabled reference error");
					continue;
				}
				DotNet.AssertBuild (projectPath, properties);
				AssertGeneratedTracking (objDir, referenced || enabled == "true");

				var result = DotNet.AssertBuild (projectPath, properties);
				AssertGeneratedTracking (objDir, referenced || enabled == "true");
				if (prepare)
					AssertTargetNotExecuted (BinLog.GetAllTargets (result.BinLogPath), "_ExecutePrepareAssemblies", "Incremental preparation");
				if (postprocess)
					AssertTargetNotExecuted (BinLog.GetAllTargets (result.BinLogPath), "_ExecutePostprocessAssemblies", "Incremental post-processing");
			}
		}

		[TestCase ("")]
		[TestCase ("true")]
		[TestCase ("false")]
		public void MissingPreparationOutput (string enabled)
		{
			Configuration.IgnoreIfIgnoredPlatform (ApplePlatform.MacCatalyst);
			var projectPath = GetProjectPath ("MySimpleApp", platform: ApplePlatform.MacCatalyst);
			var properties = GetDefaultProperties ();
			properties ["PrepareAssemblies"] = "true";
			properties ["EnableInvokedSelectorTracking"] = enabled;
			properties ["_AssemblyPreparerMSBuildOutputFile"] = Path.Combine (Cache.CreateTemporaryDirectory (), "missing.txt");
			DotNet.AssertBuild (projectPath, properties, target: "_SetInvokedSelectorTracking");
		}

		static void AssertGeneratedTracking (string objDir, bool expected)
		{
			if (Configuration.IsBuildingRemotely)
				return;
			var cache = Path.Combine (objDir, "linker-cache");
			var main = Directory.GetFiles (cache, "main.*.mm").Single ();
			Assert.That (File.ReadAllText (main).Contains ("xamarin_enable_invoked_selector_tracking ();"), Is.EqualTo (expected), "Startup tracking");
			Assert.That (File.ReadAllText (Path.Combine (cache, "registrar.mm")).Contains ("xamarin_set_invoked_selector (_cmd)"), Is.EqualTo (expected), "Registrar tracking");
		}
	}
}
