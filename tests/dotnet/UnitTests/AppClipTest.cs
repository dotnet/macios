// Copyright (c) Microsoft Corporation.
// Licensed under the MIT License.

#nullable enable

namespace Xamarin.Tests {
	[TestFixture]
	public class AppClipTest : TestBaseClass {
		[TestCase ("iossimulator-arm64")]
		public void BuildAppWithAppClip (string runtimeIdentifier)
		{
			var platform = ApplePlatform.iOS;
			Configuration.IgnoreIfIgnoredPlatform (platform);

			var consumingProjectDir = GetProjectPath ("AppClipConsumer", runtimeIdentifier, platform, out var appPath);
			Clean (GetProjectPath ("AppClipProject", platform: platform));
			Clean (consumingProjectDir);

			DotNet.AssertBuild (consumingProjectDir, GetDefaultProperties (runtimeIdentifier));

			var clipPath = Path.Combine (appPath, "AppClips", "AppClipProject.app");
			Assert.That (clipPath, Does.Exist, "App Clip directory");
			Assert.That (Path.Combine (clipPath, "Info.plist"), Does.Exist, "App Clip Info.plist");
			Assert.That (Path.Combine (clipPath, "AppClipProject"), Does.Exist, "App Clip executable");
			Assert.That (Path.Combine (appPath, "PlugIns", "AppClipProject.app"), Does.Not.Exist, "App Clip must not be in PlugIns");
		}

		[Test]
		public void AppClipWithoutNativeAot ()
		{
			var platform = ApplePlatform.iOS;
			var runtimeIdentifier = "ios-arm64";
			Configuration.IgnoreIfIgnoredPlatform (platform);

			var projectPath = GetProjectPath ("AppClipProject", runtimeIdentifier, platform, out _);
			Clean (projectPath);

			var properties = GetDefaultProperties (runtimeIdentifier);
			properties ["Configuration"] = "Release";
			var rv = DotNet.AssertBuildFailure (projectPath, properties);
			var errors = BinLog.GetBuildLogErrors (rv.BinLogPath).ToArray ();
			AssertErrorMessages (errors, "App Clips require NativeAOT in Release builds (to stay within the App Store's size limit). Set 'PublishAot=true' in the project file.");
		}
	}
}
