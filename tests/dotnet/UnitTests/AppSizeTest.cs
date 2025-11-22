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
		//
		// List of failure modes:
		// * Files added or removed from app bundle
		// * Total app size changed >10kb
		// * For those apps where assembly APIs can be compared, any API was added or removed.
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

			Assert.Multiple (() => {
				AssertAppSize (platform, name, appPath, update, forceUpdate, expectedDirectory);

				if (supportsAssemblyInspection)
					AssertAssemblyReport (platform, name, appPath, update, expectedDirectory);

			});
		}

		static void AssertAppSize (ApplePlatform platform, string name, string appPath, bool update, bool forceUpdate, string expectedDirectory)
		{
			// Compute the size of the app bundle, and compare it to the stored version on disk.
			var allFiles = Directory.GetFiles (appPath, "*", SearchOption.AllDirectories).
								Select (v => new FileInfo (v)).
								// skip 'embedded.mobileprovision', because its size depends on the provisioning profile chosen during the build, which may very well differ between CI builds and local builds
								Where (v => v.Name != "embedded.mobileprovision");
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
				expectedAppBundleSize = long.Parse (expectedSizeReport.SplitLines ().First ().Replace ("AppBundleSize: ", "").Replace (",", "").Replace (".", "").RemoveAfterFirstSpace ());
			}

			var appSizeDifference = appBundleSize - expectedAppBundleSize;
			var toleranceInBytes = 1024 * 10; // 10kb
			var withinTolerance = toleranceInBytes >= Math.Abs (appSizeDifference);

			string msg;

			if (appSizeDifference == 0) {
				msg = $"App size did not change. Expected app size: {FormatBytes (expectedAppBundleSize)}, actual app size: {FormatBytes (appBundleSize)}.";
			} else if (withinTolerance) {
				msg = $"App size changed, but not significantly: ({FormatBytes (appSizeDifference, true)} different <= tolerance of +-{FormatBytes (toleranceInBytes)}). Expected app size: {FormatBytes (expectedAppBundleSize)}, actual app size: {FormatBytes (appBundleSize)}.";
			} else {
				msg = $"App size changed significantly ({FormatBytes (appSizeDifference, true)} different > tolerance of +-{FormatBytes (toleranceInBytes)}). Expected app size: {FormatBytes (expectedAppBundleSize)}, actual app size: {FormatBytes (appBundleSize)}.";
			}

			var updated = false;
			if (forceUpdate || (update && !withinTolerance)) {
				Directory.CreateDirectory (expectedDirectory);
				File.WriteAllText (expectedSizeReportPath, report.ToString ());
				msg += " Check the modified files for more information.";
				updated = true;
			} else if (!withinTolerance) {
				msg += " Set the environment variable WRITE_KNOWN_FAILURES=1, run the test again, and verify the modified files for more information.";
			}

			Console.WriteLine ($"    {msg}");

			var expectedLines = expectedSizeReport.SplitLines ().Skip (2).Where (v => v.IndexOf (':') >= 0).ToDictionary (v => v [..v.IndexOf (':')], v => v [(v.IndexOf (':') + 1)..]);
			var actualLines = report.ToString ().SplitLines ().Skip (2).Where (v => v.IndexOf (':') >= 0).ToDictionary (v => v [..v.IndexOf (':')], v => v [(v.IndexOf (':') + 1)..]);
			var allKeys = expectedLines.Keys.Union (actualLines.Keys).OrderBy (v => v);
			foreach (var key in allKeys) {
				if (!expectedLines.TryGetValue (key, out var expectedLine)) {
					Console.WriteLine ($"        File '{key}' was removed from app bundle: {actualLines [key]}");
					Assert.Fail ($"The file '{key}' was removed from the app bundle.");
				} else if (!actualLines.TryGetValue (key, out var actualLine)) {
					Console.WriteLine ($"        File '{key}' was added to app bundle: {expectedLine}");
					Assert.Fail ($"The file '{key}' was added to the app bundle.");
				} else if (expectedLine != actualLine) {
					Console.WriteLine ($"        File '{key}' changed in app bundle:");
					Console.WriteLine ($"            -{expectedLine}");
					Console.WriteLine ($"            +{actualLine}");
				}
			}

			if (!updated && !withinTolerance)
				Assert.Fail (msg);
		}

		// Create a file with all the APIs that survived the trimmer; this can be useful to determine what is not trimmed away.
		// Note that any changes in this list when the test fails might be due to unrelated earlier changes, that didn't trigger the test
		// to fail, because the corresponding app size difference was within the tolerance for app size changes.
		void AssertAssemblyReport (ApplePlatform platform, string name, string appPath, bool update, string expectedDirectory)
		{
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
			var expectedAPIs = File.ReadAllLines (expectedFile);
			var addedAPIs = preservedAPIs.Except (expectedAPIs).ToList ();
			var removedAPIs = expectedAPIs.Except (preservedAPIs).ToList ();

			if (addedAPIs.Count () > 0) {
				Console.WriteLine ($"    {addedAPIs.Count ()} additional APIs present:");
				foreach (var line in addedAPIs)
					Console.WriteLine ($"        {line}");
			}
			if (removedAPIs.Count () > 0) {
				Console.WriteLine ($"    {removedAPIs.Count ()} APIs not present anymore:");
				foreach (var line in removedAPIs)
					Console.WriteLine ($"        {line}");
			}

			if (update) {
				File.WriteAllLines (expectedFile, preservedAPIs);
			}

			if (!update) {
				Assert.That (addedAPIs, Is.Empty, "No added APIs (set the environment variable WRITE_KNOWN_FAILURES=1 and run the test again to update the expected set of APIs)");
				Assert.That (removedAPIs, Is.Empty, "No removed APIs (set the environment variable WRITE_KNOWN_FAILURES=1 and run the test again to update the expected set of APIs)");
			}
		}

		static string FormatBytes (long bytes, bool alwaysShowSign = false)
		{
			return $"{(alwaysShowSign && bytes > 0 ? "+" : "")}{bytes:N0} bytes ({bytes / 1024.0:N1} KB = {bytes / (1024.0 * 1024.0):N1} MB)";
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
