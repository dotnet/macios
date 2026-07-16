// Copyright (c) Microsoft Corporation.
// Licensed under the MIT License.

#nullable enable

namespace Xamarin.Tests {
	[TestFixture]
	public class EnableCrashReportTest : TestBaseClass {
		[Test]
		[TestCase (ApplePlatform.MacOSX, "osx-arm64")]
		[TestCase (ApplePlatform.MacCatalyst, "maccatalyst-arm64")]
		public void Enabled (ApplePlatform platform, string runtimeIdentifiers)
		{
			// When $(EnableCrashReport) is set to true, the DOTNET_EnableCrashReport=1 environment
			// variable is set at startup, which makes the .NET runtime write a JSON crash report
			// when the app crashes. Verify this by crashing the app on launch and checking that a
			// crash report file was created.
			var project = "MySimpleApp";
			Configuration.IgnoreIfIgnoredPlatform (platform);
			Configuration.AssertRuntimeIdentifiersAvailable (platform, runtimeIdentifiers);

			var project_path = GetProjectPath (project, runtimeIdentifiers, platform, out var appPath);
			Clean (project_path);
			var properties = GetDefaultProperties (runtimeIdentifiers);
			properties ["EnableCrashReport"] = "true";

			DotNet.AssertBuild (project_path, properties);

			if (!CanExecute (platform, runtimeIdentifiers))
				return;

			// Use a known, writable location for the crash report. The runtime only sets
			// DOTNET_CrashReportRootPath if it's not already set (it uses setenv with overwrite=0),
			// so setting it here takes precedence and makes it easy to find the crash report.
			var crashReportDir = Cache.CreateTemporaryDirectory ("crash-reports");

			var appExecutable = GetNativeExecutable (platform, appPath);
			var env = new Dictionary<string, string?> {
				{ "CRASH_ON_LAUNCH", "1" },
				{ "DOTNET_CrashReportRootPath", crashReportDir },
			};

			var rv = Execute (appExecutable, out var output, out var _, env);
			Assert.That (rv.ExitCode, Is.Not.EqualTo (0), $"The app should have crashed:\n{output}");

			var crashReports = Directory.GetFiles (crashReportDir, "*.crashreport.json", SearchOption.AllDirectories);
			Assert.That (crashReports, Is.Not.Empty, $"A crash report should have been created in '{crashReportDir}':\n{output}");
		}
	}
}
