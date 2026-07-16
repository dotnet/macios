// Copyright (c) Microsoft Corporation.
// Licensed under the MIT License.

using System.IO;

#nullable enable

namespace Xamarin.Tests {
	[TestFixture]
	public class EnableCrashReportTest : TestBaseClass {
		[Test]
		[TestCase (ApplePlatform.iOS, "iossimulator-arm64")]
		public void Enabled (ApplePlatform platform, string runtimeIdentifiers)
		{
			// When $(EnableCrashReport) is set to true, the generated native main must
			// set the DOTNET_EnableCrashReport environment variable to 1 at startup.
			var project = "MySimpleApp";
			Configuration.IgnoreIfIgnoredPlatform (platform);
			Configuration.AssertRuntimeIdentifiersAvailable (platform, runtimeIdentifiers);

			var project_path = GetProjectPath (project, platform: platform);
			Clean (project_path);
			var properties = GetDefaultProperties (runtimeIdentifiers);
			properties ["EnableCrashReport"] = "true";

			DotNet.AssertBuild (project_path, properties);

			var mainFiles = GetGeneratedMainFiles (project_path, platform, runtimeIdentifiers);
			Assert.That (mainFiles, Is.Not.Empty, "The generated native main file must exist.");
			foreach (var mainFile in mainFiles) {
				var contents = File.ReadAllText (mainFile);
				Assert.That (contents, Does.Contain ("setenv (\"DOTNET_EnableCrashReport\", \"1\""), $"The generated main file '{mainFile}' must set DOTNET_EnableCrashReport.");
			}
		}

		[Test]
		[TestCase (ApplePlatform.iOS, "iossimulator-arm64")]
		public void Disabled (ApplePlatform platform, string runtimeIdentifiers)
		{
			// When $(EnableCrashReport) is not set, the generated native main must not
			// set the DOTNET_EnableCrashReport environment variable.
			var project = "MySimpleApp";
			Configuration.IgnoreIfIgnoredPlatform (platform);
			Configuration.AssertRuntimeIdentifiersAvailable (platform, runtimeIdentifiers);

			var project_path = GetProjectPath (project, platform: platform);
			Clean (project_path);
			var properties = GetDefaultProperties (runtimeIdentifiers);

			DotNet.AssertBuild (project_path, properties);

			var mainFiles = GetGeneratedMainFiles (project_path, platform, runtimeIdentifiers);
			Assert.That (mainFiles, Is.Not.Empty, "The generated native main file must exist.");
			foreach (var mainFile in mainFiles) {
				var contents = File.ReadAllText (mainFile);
				Assert.That (contents, Does.Not.Contain ("DOTNET_EnableCrashReport"), $"The generated main file '{mainFile}' must not set DOTNET_EnableCrashReport.");
			}
		}

		static string [] GetGeneratedMainFiles (string project_path, ApplePlatform platform, string runtimeIdentifiers)
		{
			var objDir = GetObjDir (project_path, platform, runtimeIdentifiers);
			return Directory.GetFiles (objDir, "main.*.mm", SearchOption.AllDirectories);
		}
	}
}
