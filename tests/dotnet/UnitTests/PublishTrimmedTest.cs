namespace Xamarin.Tests {
	[TestFixture]
	public class PublishTrimmedTest : TestBaseClass {
		[Test]
		[TestCase (ApplePlatform.iOS, "ios-arm64")]
		[TestCase (ApplePlatform.TVOS, "tvos-arm64")]
		[TestCase (ApplePlatform.MacCatalyst, "maccatalyst-arm64")]
		[TestCase (ApplePlatform.MacCatalyst, "maccatalyst-arm64;maccatalyst-x64")]
		[TestCase (ApplePlatform.MacOSX, "osx-x64")]
		[TestCase (ApplePlatform.MacOSX, "osx-arm64;osx-x64")]
		public void DisableLinker (ApplePlatform platform, string runtimeIdentifiers)
		{
			var project = "MySimpleApp";
			Configuration.IgnoreIfIgnoredPlatform (platform);
			Configuration.AssertRuntimeIdentifiersAvailable (platform, runtimeIdentifiers);

			var project_path = GetProjectPath (project, platform: platform);
			Clean (project_path);
			var properties = GetDefaultProperties (runtimeIdentifiers);
			properties ["PublishTrimmed"] = "false";

			var rv = DotNet.AssertBuildFailure (project_path, properties);
			var errors = BinLog.GetBuildLogErrors (rv.BinLogPath).ToArray ();
			Assert.That (errors.Length, Is.EqualTo (1), "Error count");
			var linkModeName = platform == ApplePlatform.MacOSX ? "LinkMode" : "MtouchLink";
			Assert.That (errors [0].Message, Is.EqualTo ($"{platform.AsString ()} projects must build with PublishTrimmed=true. Current value: false. Set '{linkModeName}=None' instead to disable trimming for all assemblies."), "Error message");
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
