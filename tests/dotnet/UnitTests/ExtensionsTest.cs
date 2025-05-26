using System.Collections.Generic;

using Microsoft.Build.Logging.StructuredLogger;

#nullable enable

namespace Xamarin.Tests {
	[TestFixture]
	public class ExtensionsTest : TestBaseClass {
		[TestCase (ApplePlatform.iOS, "ios-arm64", null)]
		[TestCase (ApplePlatform.MacOSX, "osx-x64", null)]
		[TestCase (ApplePlatform.TVOS, "tvos-arm64", null)]
		[TestCase (ApplePlatform.iOS, "ios-arm64", "/does/not/exist")]
		public void AdditionalAppExtensionTest (ApplePlatform platform, string runtimeIdentifiers, string entitlements)
		{
			var project = "AdditionalAppExtensionConsumer";
			var extensionProject = "NativeIntentsExtension";
			var configuration = "Debug";
			Configuration.IgnoreIfIgnoredPlatform (platform);

			var project_path = GetProjectPath (project, runtimeIdentifiers, platform, out var appPath);

			// Manually build the native extension project first using xcodebuild
			var xcodeProjectFolder = Path.Combine (Path.GetDirectoryName (Path.GetDirectoryName (project_path))!, "nativeextension", platform.AsString ());
			var xcodeBuildArgs = new [] {
				"-configuration", configuration,
				"-target", "NativeIntentsExtension",
				"-project", Path.Combine (xcodeProjectFolder, "NativeContainer.xcodeproj"),
			};
			var env = new Dictionary<string, string> {
				{ "DEVELOPER_DIR", Configuration.XcodeLocation },
			};
			foreach (var action in new string [] { "clean", "build" })
				ExecutionHelper.Execute ("/usr/bin/xcodebuild", xcodeBuildArgs.Concat (new [] { action }).ToArray (), environmentVariables: env, timeout: TimeSpan.FromMinutes (1), throwOnError: true, hide_output: true);

			string buildPlatform;
			switch (platform) {
			case ApplePlatform.iOS:
				buildPlatform = "-iphoneos";
				break;
			case ApplePlatform.TVOS:
				buildPlatform = "-appletvos";
				break;
			default:
				buildPlatform = string.Empty;
				break;
			}

			// Now build the containing project
			var properties = GetDefaultProperties (runtimeIdentifiers);
			properties.Add ("AdditionalAppExtensionPath", xcodeProjectFolder);
			properties.Add ("AdditionalAppExtensionBuildOutput", $"build/{configuration}{buildPlatform}");
			if (!string.IsNullOrEmpty (entitlements)) {
				properties.Add ("AdditionalAppExtensionEntitlements", entitlements);
				var rv = DotNet.AssertBuildFailure (project_path, properties);
				var errors = BinLog.GetBuildLogErrors (rv.BinLogPath).ToArray ();
				AssertErrorMessages (errors, "Entitlements.plist template '/does/not/exist' not found.");
				return;
			} else {
				var rv = DotNet.AssertBuild (project_path, properties);
				var warnings = BinLog.GetBuildLogWarnings (rv.BinLogPath)
					.Where (v => v?.Message?.Contains ("Supported iPhone orientations have not been set") != true)
					.ToArray ();
				if (IsRuntimeIdentifierSigned (runtimeIdentifiers)) {
					var extensionPath = Path.Combine (appPath, GetPlugInsRelativePath (platform), $"{extensionProject}.appex");
					AssertWarningMessages (warnings, [
						$"No entitlements set for {extensionPath}."
					]);
				} else {
					rv.AssertNoWarnings ();
				}
			}

			var expectedDirectories = new List<string> ();
			if (IsRuntimeIdentifierSigned (runtimeIdentifiers)) {
				expectedDirectories.Add (Path.Combine (appPath, "_CodeSignature"));
				expectedDirectories.Add (Path.Combine (appPath, "PlugIns", extensionProject + ".appex", "_CodeSignature"));
			}

			foreach (var dir in expectedDirectories)
				Assert.That (dir, Does.Exist, "Directory should exist.");
		}
	}
}
