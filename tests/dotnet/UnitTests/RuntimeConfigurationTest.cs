// Copyright (c) Microsoft Corporation.
// Licensed under the MIT License.

#nullable enable

namespace Xamarin.Tests {
	[TestFixture]
	public class RuntimeConfigurationTest : TestBaseClass {
		[TestCase (ApplePlatform.iOS, "ios-arm64", true)]
		[TestCase (ApplePlatform.iOS, "ios-arm64", false)]
		[TestCase (ApplePlatform.TVOS, "tvossimulator-arm64", true)]
		[TestCase (ApplePlatform.TVOS, "tvossimulator-arm64", false)]
		[TestCase (ApplePlatform.MacCatalyst, "maccatalyst-arm64", true)]
		[TestCase (ApplePlatform.MacCatalyst, "maccatalyst-arm64", false)]
		[TestCase (ApplePlatform.MacOSX, "osx-arm64", true)]
		[TestCase (ApplePlatform.MacOSX, "osx-arm64", false)]
		public void NativeAotInvariantGlobalization (ApplePlatform platform, string runtimeIdentifiers, bool prepareAssemblies)
		{
			Configuration.IgnoreIfIgnoredPlatform (platform);
			Configuration.AssertRuntimeIdentifiersAvailable (platform, runtimeIdentifiers);

			var project_path = GetProjectPath ("MySimpleApp", platform: platform);
			Clean (project_path);
			var properties = GetDefaultProperties (runtimeIdentifiers);
			properties ["PublishAot"] = "true";
			properties ["_IsPublishing"] = "true";
			properties ["InvariantGlobalization"] = "true";
			properties ["PrepareAssemblies"] = prepareAssemblies.ToString ();

			DotNet.AssertBuild (project_path, properties);

			var appPath = GetAppPath (project_path, platform, runtimeIdentifiers);
			var appExecutable = GetNativeExecutable (platform, appPath);
			Assert.That (appExecutable, Does.Exist, "NativeAOT executable");
			ExecuteWithMagicWordAndAssert (platform, runtimeIdentifiers, appExecutable);
		}
	}
}
