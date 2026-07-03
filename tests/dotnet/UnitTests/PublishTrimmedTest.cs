namespace Xamarin.Tests {
	[TestFixture]
	public class PublishTrimmedTest : TestBaseClass {
		[Test]
		[TestCase (ApplePlatform.iOS, "ios-arm64", "false")]
		[TestCase (ApplePlatform.TVOS, "tvos-arm64", "false")]
		[TestCase (ApplePlatform.MacCatalyst, "maccatalyst-arm64", "false")]
		[TestCase (ApplePlatform.MacCatalyst, "maccatalyst-arm64;maccatalyst-x64", "false")]
		[TestCase (ApplePlatform.MacOSX, "osx-x64", "false")]
		[TestCase (ApplePlatform.MacOSX, "osx-arm64;osx-x64", "false")]

		[TestCase (ApplePlatform.iOS, "ios-arm64", "true")]
		[TestCase (ApplePlatform.TVOS, "tvos-arm64", "true")]
		[TestCase (ApplePlatform.MacCatalyst, "maccatalyst-arm64", "true")]
		[TestCase (ApplePlatform.MacOSX, "osx-arm64;osx-x64", "true")]
		public void PublishTrimmedNotSupported (ApplePlatform platform, string runtimeIdentifiers, string value)
		{
			var project = "MySimpleApp";
			Configuration.IgnoreIfIgnoredPlatform (platform);
			Configuration.AssertRuntimeIdentifiersAvailable (platform, runtimeIdentifiers);

			var project_path = GetProjectPath (project, platform: platform);
			Clean (project_path);
			var properties = GetDefaultProperties (runtimeIdentifiers);
			properties ["PublishTrimmed"] = value;

			var rv = DotNet.AssertBuildFailure (project_path, properties);
			var errors = BinLog.GetBuildLogErrors (rv.BinLogPath).ToArray ();
			var linkModeName = platform == ApplePlatform.MacOSX ? "LinkMode" : "MtouchLink";
			AssertErrorMessages (errors, $"{platform.AsString ()} projects do not support setting 'PublishTrimmed' to any value (current value: {value}). Use the '{linkModeName}' property to configure trimming behavior instead.");
		}

		[Test]
		[TestCase (ApplePlatform.iOS, "iossimulator-arm64")]
		public void SkipTrimmerWhenNotTrimming (ApplePlatform platform, string runtimeIdentifiers)
		{
			// When we're not trimming anything (the link mode is 'None') and we're not running any custom
			// trimmer steps (which is the case when both PrepareAssemblies and PostProcessAssemblies are
			// 'true'), then there's nothing for the trimmer to do, so PublishTrimmed defaults to false.
			var project = "MySimpleApp";
			Configuration.IgnoreIfIgnoredPlatform (platform);
			Configuration.AssertRuntimeIdentifiersAvailable (platform, runtimeIdentifiers);

			var project_path = GetProjectPath (project, platform: platform);
			Clean (project_path);
			var properties = GetDefaultProperties (runtimeIdentifiers);
			properties ["MtouchLink"] = "None";
			properties ["PrepareAssemblies"] = "true";
			properties ["PostProcessAssemblies"] = "true";

			var rv = DotNet.AssertBuild (project_path, properties);

			// Verify that the trimmer didn't run: when there's nothing for the trimmer to do, the
			// 'ILLink' target isn't executed. We check the executed targets instead of the
			// 'PublishTrimmed' property value, because 'PublishTrimmed' is computed inside a target,
			// and target-assigned property values are only logged in the binlog when property tracking
			// is enabled (the 'MsBuildLogPropertyTracking' environment variable), which isn't the case
			// on CI.
			var targets = BinLog.GetAllTargets (rv.BinLogPath);
			AssertTargetNotExecuted (targets, "ILLink", "The trimmer should not have executed.");
		}
	}
}
