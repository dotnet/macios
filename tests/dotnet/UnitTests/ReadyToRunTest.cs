// Copyright (c) Microsoft Corporation.
// Licensed under the MIT License.

#nullable enable

namespace Xamarin.Tests {
	[TestFixture]
	public class ReadyToRunTest : TestBaseClass {
		[TestCase ("Debug", "CoreLib", false)]
		[TestCase ("Release", "Full", false)]
		[TestCase ("Release", "FilteredRelease", true)]
		public void AssemblySelection (string configuration, string expectedReadyToRunConfiguration, bool filterReadyToRunAssemblies)
		{
			var platform = ApplePlatform.iOS;
			var runtimeIdentifier = "iossimulator-arm64";
			Configuration.IgnoreIfIgnoredPlatform (platform);
			Configuration.AssertRuntimeIdentifiersAvailable (platform, runtimeIdentifier);

			var projectPath = GetProjectPath ("ReadyToRunTestApp", runtimeIdentifiers: runtimeIdentifier, platform: platform, out _, configuration: configuration);
			Clean (projectPath);

			var properties = GetDefaultProperties (runtimeIdentifier);
			properties ["Configuration"] = configuration;
			properties ["UseMonoRuntime"] = "false";
			properties ["ExpectedReadyToRunConfiguration"] = expectedReadyToRunConfiguration;
			if (filterReadyToRunAssemblies)
				properties ["FilterReadyToRunAssemblies"] = "true";

			var rv = DotNet.AssertBuildFailure (projectPath, properties);
			var errors = BinLog.GetBuildLogErrors (rv.BinLogPath).ToArray ();
			AssertErrorMessages (errors, "All good!");
		}
	}
}
