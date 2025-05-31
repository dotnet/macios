#nullable enable

using System.IO;
using System.IO.Compression;

namespace Xamarin.Tests {
	public class XcodeVersion : TestBaseClass {

		public static object [] GetTestCases ()
		{
			var platforms = Configuration.GetAllPlatforms ();
			var xcodes = Configuration.GetAllXcodes ();
			var rv = new List<object []> ();
			foreach (var platform in platforms) {
				foreach (var xcode in xcodes)
					rv.Add (new object [] { platform, xcode.Path, xcode.Version });
			}
			return rv.ToArray ();
		}

		[Test]
		[TestCaseSource (nameof (GetTestCases))]
		public void Test (ApplePlatform platform, string xcodePath, Version xcodeVersion)
		{
			var project = "MySimpleApp";
			Configuration.IgnoreIfIgnoredPlatform (platform);

			var isCurrentXcode = xcodeVersion == Configuration.XcodeVersion;

			var runtimeIdentifiers = GetDefaultRuntimeIdentifier (platform);
			var project_path = GetProjectPath (project, runtimeIdentifiers: runtimeIdentifiers, platform: platform, out var appPath);
			Clean (project_path);
			var properties = GetDefaultProperties (runtimeIdentifiers);
			if (isCurrentXcode) {

			} else {

				var existingDeveloperDir = Environment.GetEnvironmentVariable ("MD_APPLE_SDK_ROOT");
				try {
					Environment.SetEnvironmentVariable ("MD_APPLE_SDK_ROOT", Path.GetDirectoryName (Path.GetDirectoryName (xcodePath)));
					var rv = DotNet.AssertBuildFailure (project_path, properties);
					var errors = BinLog.GetBuildLogErrors (rv.BinLogPath).ToArray ();
					AssertErrorMessages (errors, $"This version of .NET for {platform.AsString ()} ({Configuration.GetNuGetVersionNoMetadata (platform)}) requires Xcode {Configuration.XcodeVersion}. The current version of Xcode is {xcodeVersion}. Either install Xcode {Configuration.XcodeVersion}, or use a different version of .NET for {platform.AsString ()}. See https://aka.ms/xcode-requirement for more information.");
				} finally {
					Environment.SetEnvironmentVariable ("MD_APPLE_SDK_ROOT", existingDeveloperDir);
				}
			}
		}
	}
}
