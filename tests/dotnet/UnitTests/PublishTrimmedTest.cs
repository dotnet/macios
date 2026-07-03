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
	}
}
