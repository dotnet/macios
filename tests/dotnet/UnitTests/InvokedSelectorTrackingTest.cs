// Copyright (c) Microsoft Corporation.
// Licensed under the MIT License.

#nullable enable

namespace Xamarin.Tests {
	[TestFixture]
	public class InvokedSelectorTrackingTest : TestBaseClass {
		[TestCase (true, true)]
		[TestCase (true, false)]
		[TestCase (false, false)]
		public void IncrementalBuild (bool prepare, bool postprocess)
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

			var objDir = GetObjDir (projectPath, platform, runtimeIdentifier, "Debug");
			foreach (var enabled in new string? [] { null, "true", "false" }) {
				if (enabled is not null)
					properties ["EnableInvokedSelectorTracking"] = enabled;
				DotNet.AssertBuild (projectPath, properties);
				AssertGeneratedTracking (objDir, enabled == "true");

				var result = DotNet.AssertBuild (projectPath, properties);
				AssertGeneratedTracking (objDir, enabled == "true");
				if (prepare)
					AssertTargetNotExecuted (BinLog.GetAllTargets (result.BinLogPath), "_ExecutePrepareAssemblies", "Incremental preparation");
				if (postprocess)
					AssertTargetNotExecuted (BinLog.GetAllTargets (result.BinLogPath), "_ExecutePostprocessAssemblies", "Incremental post-processing");
			}
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
