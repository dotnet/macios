// Copyright (c) Microsoft Corporation.
// Licensed under the MIT License.

using Cecil.Tests;
using Mono.Cecil;

#nullable enable

namespace Xamarin.Tests {
	[TestFixture]
	public class AppSizeTest : TestBaseClass {

		[TestCase (ApplePlatform.iOS, "ios-arm64")]
		public void MonoVM (ApplePlatform platform, string runtimeIdentifiers)
		{
			Run (platform, runtimeIdentifiers, "Release", $"{platform}-MonoVM", true);
		}

		[TestCase (ApplePlatform.iOS, "ios-arm64")]
		public void MonoVM_Interpreter (ApplePlatform platform, string runtimeIdentifiers)
		{
			Run (platform, runtimeIdentifiers, "Release", $"{platform}-MonoVM-interpreter", true, new Dictionary<string, string> () { { "UseInterpreter", "true" } });
		}

		[TestCase (ApplePlatform.iOS, "ios-arm64")]
		public void NativeAOT (ApplePlatform platform, string runtimeIdentifiers)
		{
			Run (platform, runtimeIdentifiers, "Release", $"{platform}-NativeAOT", false, new Dictionary<string, string> () { { "PublishAot", "true" } });
		}

		// This test will build the SizeTestApp, and capture the resulting app size.
		// The app size is stored in a file on disk, so we can make sure app size doesn't change (or at least we notice it and we can update the known state).
		// There's a tolerance in the test for minor app size variances, so if this test fails, the current change might not mean there's a big change,
		// there might just be many cumulative unnoticed/minor app size differences eventually triggering the test.
		// The test fails even if app size goes down; this way we can also keep track of good news! And additionally we won't miss it if the app size first goes down, then back up again.
		void Run (ApplePlatform platform, string runtimeIdentifiers, string configuration, string name, bool supportsAssemblyInspection, Dictionary<string, string>? extraProperties = null)
		{
			Configuration.IgnoreIfIgnoredPlatform (platform);
			Configuration.AssertRuntimeIdentifiersAvailable (platform, runtimeIdentifiers);

			var project = "SizeTestApp";
			var project_path = GetProjectPath (project, runtimeIdentifiers: runtimeIdentifiers, platform: platform, out var appPath, configuration: configuration);

			Clean (project_path);

			var properties = GetDefaultProperties (runtimeIdentifiers, extraProperties: extraProperties);
			properties ["Configuration"] = configuration;

			DotNet.AssertBuild (project_path, properties);

			// FORCE_UPDATE_KNOWN_FAILURES will update the known failures files even if the test doesn't actually fail
			// WRITE_KNOWN_FAILURES will only update the known failures files if the test fails

			var forceUpdate = !string.IsNullOrEmpty (Environment.GetEnvironmentVariable ("FORCE_UPDATE_KNOWN_FAILURES"));
			var update = forceUpdate || !string.IsNullOrEmpty (Environment.GetEnvironmentVariable ("WRITE_KNOWN_FAILURES"));
			var expectedDirectory = Path.Combine (Configuration.SourceRoot, "tests", "dotnet", "UnitTests", "expected");

			// Compute the size of the app bundle, and compare it to the stored version on disk.
			var allFiles = Directory.GetFiles (appPath, "*", SearchOption.AllDirectories).Select (v => new FileInfo (v));
			var appBundleSize = allFiles.Sum (v => v.Length);
			var report = new StringBuilder ();
			report.AppendLine ($"AppBundleSize: {FormatBytes (appBundleSize)}");
			report.AppendLine ($"# The following list of files and their sizes is just informational / for review, and isn't used in the test:");
			foreach (var file in allFiles.OrderBy (v => v.FullName))
				report.AppendLine ($"{file.FullName [(appPath.Length + 1)..]}: {FormatBytes (file.Length)}");
			var expectedSizeReportPath = Path.Combine (expectedDirectory, $"{name}-size.txt");
			var expectedSizeReport = "";
			var expectedAppBundleSize = 0L;
			if (File.Exists (expectedSizeReportPath)) {
				expectedSizeReport = File.ReadAllText (expectedSizeReportPath);
				expectedAppBundleSize = long.Parse (expectedSizeReport.SplitLines ().First ().Replace ("AppBundleSize: ", "").RemoveAfterFirstSpace ());
			}

			var appSizeDifference = appBundleSize - expectedAppBundleSize;
			if (appSizeDifference == 0 && !forceUpdate)
				return;

			var toleranceInBytes = 1024 * 10; // 10kb
			if (toleranceInBytes >= Math.Abs (appSizeDifference)) {
				Console.WriteLine ($"App size difference is {FormatBytes (appSizeDifference)}, which is less than the tolerance ({toleranceInBytes}), so nothing will be reported.");
				if (!forceUpdate)
					return;
			}

			if (update) {
				Directory.CreateDirectory (expectedDirectory);
				File.WriteAllText (expectedSizeReportPath, report.ToString ());

				// Create a file with all the APIs that survived the trimmer; this can be useful to determine what is not trimmed away.
				// Note that any changes in this list when the test fails might be due to unrelated earlier changes, that didn't trigger the test
				// to fail, because the corresponding app size difference was within the tolerance for app size changes.
				if (supportsAssemblyInspection) {
					var asmDir = Path.Combine (appPath, GetRelativeAssemblyDirectory (platform));
					var preservedAPIs = new List<string> ();
					foreach (var dll in Directory.GetFiles (asmDir, "*.dll", SearchOption.AllDirectories)) {
						var relativePath = dll [(asmDir.Length + 1)..];
						using var ad = AssemblyDefinition.ReadAssembly (dll, new ReaderParameters { ReadingMode = ReadingMode.Deferred });
						foreach (var member in ad.EnumerateMembers ()) {
							preservedAPIs.Add ($"{relativePath}:{((ICustomAttributeProvider) member).AsFullName ()}");
						}
					}
					preservedAPIs.Sort ();
					var expectedFile = Path.Combine (expectedDirectory, $"{name}-preservedapis.txt");
					File.WriteAllLines (expectedFile, preservedAPIs);
					Console.WriteLine ($"Updated expected results: {expectedFile}");
				}
			} else {
				Assert.Fail ($"App size changed significantly ( {FormatBytes (appSizeDifference)} different > tolerance of  {FormatBytes (appSizeDifference)}). Expected app size: {FormatBytes (expectedAppBundleSize)}, actual app size: {FormatBytes (appBundleSize)}. Set the environment variable WRITE_KNOWN_FAILURES=1, run the test again, and verify the modified files for more information");
			}
		}

		static string FormatBytes (long bytes)
		{
			return $"{bytes} bytes ({bytes / 1024.0:0.0} KB = {bytes / (1024.0 * 1024.0):0.0} MB)";
		}
	}

	static class StringExtensions {
		public static string RemoveAfterFirstSpace (this string value)
		{
			var sp = value.IndexOf (' ');
			if (sp == -1)
				return value;
			return value [..sp];
		}
	}
}
