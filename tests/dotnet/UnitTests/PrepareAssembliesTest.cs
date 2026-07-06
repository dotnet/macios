// Copyright (c) Microsoft Corporation.
// Licensed under the MIT License.

#nullable enable

namespace Xamarin.Tests {
	[TestFixture]
	public class PrepareAssembliesTest : TestBaseClass {
		[Test]
		[TestCase (ApplePlatform.iOS, "iossimulator-arm64")]
		public void IncrementalBuild (ApplePlatform platform, string runtimeIdentifiers)
		{
			// An incremental (second, no-source-change) build with PrepareAssemblies=true must not fail.
			// The '_PrepareAssemblies' and '_PostprocessAssemblies' targets must not run as partial
			// incremental builds, because the assembly-preparer needs the complete set of assemblies to
			// resolve inter-assembly references. See https://github.com/dotnet/macios/issues/25938.
			var project = "MySimpleApp";
			Configuration.IgnoreIfIgnoredPlatform (platform);
			Configuration.AssertRuntimeIdentifiersAvailable (platform, runtimeIdentifiers);

			var configuration = "Release";
			var project_path = GetProjectPath (project, runtimeIdentifiers: runtimeIdentifiers, platform: platform, out var appPath, configuration: configuration);
			Clean (project_path);
			var properties = GetDefaultProperties (runtimeIdentifiers);
			properties ["Configuration"] = configuration;
			properties ["PrepareAssemblies"] = "true";
			properties ["MtouchLink"] = "SdkOnly";
			properties ["Registrar"] = "managed-static";

			// The first (clean) build must succeed.
			DotNet.AssertBuild (project_path, properties);

			// The second (incremental) build, without any changes, must also succeed.
			DotNet.AssertBuild (project_path, properties);
		}
	}
}
