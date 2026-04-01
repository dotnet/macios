#nullable enable

namespace Xamarin.Tests {
	[TestFixture]
	public class StartupHookTest : TestBaseClass {
		const string project = "StartupHookTest";

		[TestCase (ApplePlatform.MacOSX, "osx-arm64")]
		[TestCase (ApplePlatform.MacCatalyst, "maccatalyst-x64")]
		public void EnabledForDebug (ApplePlatform platform, string runtimeIdentifiers)
		{
			Configuration.IgnoreIfIgnoredPlatform (platform);

			var configuration = "Debug";
			var project_path = GetProjectPath (project, runtimeIdentifiers: runtimeIdentifiers, platform: platform, out var appPath, configuration: configuration);
			Clean (project_path);

			var properties = GetDefaultProperties (runtimeIdentifiers);
			properties ["Configuration"] = configuration;
			DotNet.AssertBuild (project_path, properties);

			if (CanExecute (platform, properties)) {
				var appExecutable = GetNativeExecutable (platform, appPath);
				var env = new Dictionary<string, string?> {
					{ "DOTNET_STARTUP_HOOKS", "StartupHookTest:StartupHookLibrary" },
				};
				ExecuteWithMagicWordAndAssert (appExecutable, env);

				env = new Dictionary<string, string?> {
					{ "DOTNET_STARTUP_HOOKS", "StartupHookLibrary" },
				};
				ExecuteWithMagicWordAndAssert (appExecutable, env, expectedExitCode: 1); // this should fail

				env = new Dictionary<string, string?> {
					{ "DOTNET_STARTUP_HOOKS", "StartupHookTest" },
				};
				ExecuteWithMagicWordAndAssert (appExecutable, env, expectedExitCode: 2); // this should fail
			}
		}

		[TestCase (ApplePlatform.MacOSX, "osx-arm64")]
		[TestCase (ApplePlatform.MacCatalyst, "maccatalyst-x64")]
		public void DisabledForRelease (ApplePlatform platform, string runtimeIdentifiers)
		{
			Configuration.IgnoreIfIgnoredPlatform (platform);

			var configuration = "Release";
			var project_path = GetProjectPath (project, runtimeIdentifiers: runtimeIdentifiers, platform: platform, out var appPath, configuration: configuration);
			Clean (project_path);

			var properties = GetDefaultProperties (runtimeIdentifiers);
			properties ["Configuration"] = configuration;
			DotNet.AssertBuild (project_path, properties);

			if (CanExecute (platform, properties)) {
				var appExecutable = GetNativeExecutable (platform, appPath);
				var env = new Dictionary<string, string?> {
					{ "DOTNET_STARTUP_HOOKS", "StartupHookTest:StartupHookLibrary" },
				};
				ExecuteWithMagicWordAndAssert (appExecutable, env, expectedExitCode: 3); // this should fail

				env = new Dictionary<string, string?> ();
				ExecuteWithMagicWordAndAssert (appExecutable, env, expectedExitCode: 3); // this should fail

				env = new Dictionary<string, string?> {
					{ "DOTNET_STARTUP_HOOKS", "StartupHookLibrary" },
				};
				ExecuteWithMagicWordAndAssert (appExecutable, env, expectedExitCode: 3); // this should fail

				env = new Dictionary<string, string?> {
					{ "DOTNET_STARTUP_HOOKS", "StartupHookTest" },
				};
				ExecuteWithMagicWordAndAssert (appExecutable, env, expectedExitCode: 3); // this should fail
			}
		}

		[TestCase (ApplePlatform.MacOSX, "osx-arm64")]
		[TestCase (ApplePlatform.MacCatalyst, "maccatalyst-x64")]
		public void Enableable (ApplePlatform platform, string runtimeIdentifiers)
		{
			Configuration.IgnoreIfIgnoredPlatform (platform);

			var configuration = "Release";
			var project_path = GetProjectPath (project, runtimeIdentifiers: runtimeIdentifiers, platform: platform, out var appPath, configuration: configuration);
			Clean (project_path);

			var properties = GetDefaultProperties (runtimeIdentifiers);
			properties ["Configuration"] = configuration;
			properties ["StartupHookSupport"] = "true";
			DotNet.AssertBuild (project_path, properties);

			if (CanExecute (platform, properties)) {
				var appExecutable = GetNativeExecutable (platform, appPath);
				var env = new Dictionary<string, string?> {
					{ "DOTNET_STARTUP_HOOKS", "StartupHookTest:StartupHookLibrary" },
				};
				ExecuteWithMagicWordAndAssert (appExecutable, env); // this should work

				env = new Dictionary<string, string?> ();
				ExecuteWithMagicWordAndAssert (appExecutable, env, expectedExitCode: 3); // this should fail

				env = new Dictionary<string, string?> {
					{ "DOTNET_STARTUP_HOOKS", "StartupHookLibrary" },
				};
				ExecuteWithMagicWordAndAssert (appExecutable, env, expectedExitCode: 1); // this should fail

				env = new Dictionary<string, string?> {
					{ "DOTNET_STARTUP_HOOKS", "StartupHookTest" },
				};
				ExecuteWithMagicWordAndAssert (appExecutable, env, expectedExitCode: 2); // this should fail
			}
		}
	}
}
