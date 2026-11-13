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
		public void AdditionalAppExtensionTest (ApplePlatform platform, string runtimeIdentifiers, string? entitlements)
		{
			var project = "AdditionalAppExtensionConsumer";
			var extensionProjects = new [] { "NativeIntentsExtension", "SecondNativeIntentsExtension", "NativeIntentsExtensionWithoutEntitlements" };
			var configuration = "Debug";
			Configuration.IgnoreIfIgnoredPlatform (platform);

			var project_path = GetProjectPath (project, runtimeIdentifiers, platform, out var appPath);

			// Manually build the native extension project first using xcodebuild
			var projectDirectory = Path.GetDirectoryName (Path.GetDirectoryName (project_path)) ?? throw new InvalidOperationException ("The project must have a parent directory.");
			var xcodeProjectSource = Path.Combine (projectDirectory, "nativeextension", platform.AsString ());
			var xcodeProjectFolder = Cache.CreateTemporaryDirectory ();
			var xcodeBuildArgs = new [] {
				"-configuration", configuration,
				"-target", "NativeIntentsExtension",
			};
			var env = new Dictionary<string, string?> {
				{ "DEVELOPER_DIR", Configuration.XcodeLocation },
			};
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

			var buildOutput = $"build/{configuration}{buildPlatform}";
			var desktop = platform == ApplePlatform.MacOSX;
			var profilePath = Path.Combine (Configuration.SourceRoot, "tests", "msbuild", "Xamarin.MacDev.Tasks.Tests", "Resources",
				desktop ? "WildCardMacAppDevelopment.provisionprofile" : "profile.mobileprovision");
			var profile = MobileProvision.LoadFromFile (profilePath);
			var sourceEntitlements = new PDictionary {
				{ "keychain-access-groups", new PArray { new PString ("$(AppIdentifierPrefix)$(CFBundleIdentifier)") } },
			}.ToXml ();
			foreach (var extensionProject in extensionProjects) {
				var extensionProjectFolder = Path.Combine (xcodeProjectFolder, extensionProject);
				FileHelpers.CopyDirectory (xcodeProjectSource, extensionProjectFolder);
				var bundleIdentifier = "com.xamarin.NativeContainer." + extensionProject;
				var args = xcodeBuildArgs.Concat (new [] {
					"-project", Path.Combine (extensionProjectFolder, "NativeContainer.xcodeproj"),
					"build",
					"PRODUCT_NAME=" + extensionProject,
					"PRODUCT_BUNDLE_IDENTIFIER=" + bundleIdentifier,
					"CODE_SIGNING_ALLOWED=NO",
				}).ToArray ();
				ExecutionHelper.Execute ("/usr/bin/xcodebuild", args, environmentVariables: env, timeout: TimeSpan.FromMinutes (1), throwOnError: true, hide_output: true);
				var bundlePath = Path.Combine (extensionProjectFolder, buildOutput, extensionProject + ".appex");
				File.Copy (profilePath, Path.Combine (bundlePath, desktop ? "Contents/embedded.provisionprofile" : "embedded.mobileprovision"), true);
				if (extensionProject != "NativeIntentsExtensionWithoutEntitlements")
					File.WriteAllText (Path.Combine (extensionProjectFolder, extensionProject == extensionProjects [0] ? "CustomEntitlements.plist" : extensionProject + ".entitlements"), sourceEntitlements);
			}

			// Now build the containing project
			var properties = GetDefaultProperties (runtimeIdentifiers);
			properties.Add ("AdditionalAppExtensionPath", xcodeProjectFolder);
			properties.Add ("AdditionalAppExtensionBuildOutput", buildOutput);
			properties.Add ("AdditionalAppExtensionEntitlements", entitlements ?? Path.Combine (xcodeProjectFolder, extensionProjects [0], "CustomEntitlements.plist"));
			if (!string.IsNullOrEmpty (entitlements)) {
				var rv = DotNet.AssertBuildFailure (project_path, properties);
				var errors = BinLog.GetBuildLogErrors (rv.BinLogPath).ToArray ();
				AssertErrorMessages (errors, "Entitlements.plist template '/does/not/exist' not found.");
				return;
			} else {
				var rv = DotNet.AssertBuild (project_path, properties);
				var warnings = BinLog.GetBuildLogWarnings (rv.BinLogPath)
					.FilterWarnings (platform)
					.ToArray ();
				var extensionPath = Path.Combine (appPath, GetPlugInsRelativePath (platform), "NativeIntentsExtensionWithoutEntitlements.appex");
				AssertWarningMessages (warnings, $"No entitlements set for {extensionPath}.");
			}

			var expectedDirectories = new List<string> ();
			expectedDirectories.Add (Path.Combine (appPath, GetRelativeCodesignDirectory (platform), "_CodeSignature"));
			foreach (var extensionProject in extensionProjects) {
				var extensionPath = Path.Combine (appPath, GetPlugInsRelativePath (platform), extensionProject + ".appex");
				expectedDirectories.Add (Path.Combine (extensionPath, GetRelativeCodesignDirectory (platform), "_CodeSignature"));
				ExecutionHelper.Execute ("/usr/bin/codesign", new [] { "--verify", "--strict", extensionPath }, timeout: TimeSpan.FromSeconds (15), throwOnError: true);
				if (extensionProject == "NativeIntentsExtensionWithoutEntitlements")
					continue;
				Assert.That (TryGetEntitlements (GetNativeExecutable (platform, extensionPath), out var signedEntitlements), Is.True, extensionProject + " signed entitlements");
				Assert.That (signedEntitlements, Is.Not.Null, extensionProject);
				var applicationIdentifier = profile.ApplicationIdentifierPrefix [0] + ".com.xamarin.NativeContainer." + extensionProject;
				Assert.That (signedEntitlements?.GetString (desktop ? "com.apple.application-identifier" : "application-identifier")?.Value,
					Is.EqualTo (applicationIdentifier), extensionProject + " application identifier");
				Assert.That (signedEntitlements?.GetArray ("keychain-access-groups").ToStringArray (),
					Is.EqualTo (new [] { applicationIdentifier }), extensionProject + " custom entitlements");
				Assert.That (File.ReadAllText (Path.Combine (xcodeProjectFolder, extensionProject, extensionProject == extensionProjects [0] ? "CustomEntitlements.plist" : extensionProject + ".entitlements")),
					Is.EqualTo (sourceEntitlements), extensionProject + " source entitlements");
			}

			foreach (var dir in expectedDirectories)
				Assert.That (dir, Does.Exist, "Directory should exist.");
		}
	}
}
