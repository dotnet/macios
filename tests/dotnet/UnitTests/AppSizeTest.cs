// Copyright (c) Microsoft Corporation.
// Licensed under the MIT License.

using Cecil.Tests;
using Mono.Cecil;

#nullable enable

namespace Xamarin.Tests {
	[TestFixture]
	public class AppSizeTest : TestBaseClass {

		[TestCase (ApplePlatform.iOS, "ios-arm64")]
		[TestCase (ApplePlatform.TVOS, "tvos-arm64")]
		[TestCase (ApplePlatform.MacCatalyst, "maccatalyst-arm64")]
		public void MonoVM (ApplePlatform platform, string runtimeIdentifiers)
		{
			var dict = new Dictionary<string, string> () {
				{ "UseMonoRuntime", "true" },
				{ "NoDSymUtil", "false" },
			};
			Run (platform, runtimeIdentifiers, "Release", $"{platform}-MonoVM", true, dict);
		}

		[TestCase (ApplePlatform.iOS, "ios-arm64")]
		[TestCase (ApplePlatform.TVOS, "tvos-arm64")]
		[TestCase (ApplePlatform.MacCatalyst, "maccatalyst-arm64")]
		public void MonoVM_Interpreter (ApplePlatform platform, string runtimeIdentifiers)
		{
			var dict = new Dictionary<string, string> () {
				{ "UseInterpreter", "true" },
				{ "UseMonoRuntime", "true" },
				{ "NoDSymUtil", "false" },
			};
			Run (platform, runtimeIdentifiers, "Release", $"{platform}-MonoVM-interpreter", true, dict);
		}

		[TestCase (ApplePlatform.iOS, "ios-arm64")]
		[TestCase (ApplePlatform.TVOS, "tvos-arm64")]
		[TestCase (ApplePlatform.MacCatalyst, "maccatalyst-arm64")]
		[TestCase (ApplePlatform.MacOSX, "osx-arm64;osx-x64")]
		public void NativeAOT (ApplePlatform platform, string runtimeIdentifiers)
		{
			var dict = new Dictionary<string, string> () {
				{ "PublishAot", "true" },
				{ "_IsPublishing", "true" },
				{ "NoDSymUtil", "false" }, // off by default for macOS, but we want to test it, so enable it
			};
			Run (platform, runtimeIdentifiers, "Release", $"{platform}-NativeAOT", false, dict);
		}

		[TestCase (ApplePlatform.MacOSX, "osx-arm64;osx-x64", false)]
		public void CoreCLR_Interpreter (ApplePlatform platform, string runtimeIdentifiers, bool isTrimmed)
		{
			var dict = new Dictionary<string, string> () {
				{ "UseMonoRuntime", "false" },
				{ "PublishReadyToRun", "false" },
				{ "NoDSymUtil", "false" }, // off by default for macOS, but we want to test it, so enable it
			};
			Run (platform, runtimeIdentifiers, "Release", $"{platform}-CoreCLR-Interpreter", isTrimmed, dict);
		}

		[TestCase (ApplePlatform.iOS, "ios-arm64")]
		[TestCase (ApplePlatform.TVOS, "tvos-arm64")]
		[TestCase (ApplePlatform.MacCatalyst, "maccatalyst-arm64")]
		[TestCase (ApplePlatform.MacOSX, "osx-arm64;osx-x64")]
		public void NativeAOT_TrimmableStatic (ApplePlatform platform, string runtimeIdentifiers)
		{
			var dict = new Dictionary<string, string> () {
				{ "PublishAot", "true" },
				{ "_IsPublishing", "true" },
				{ "NoDSymUtil", "false" }, // off by default for macOS, but we want to test it, so enable it
				{ "Registrar", "trimmable-static" },
			};
			Run (platform, runtimeIdentifiers, "Release", $"{platform}-NativeAOT-TrimmableStatic", false, dict);
		}

		[TestCase (ApplePlatform.MacOSX, "osx-arm64;osx-x64", false)]
		public void CoreCLR_Interpreter_TrimmableStatic (ApplePlatform platform, string runtimeIdentifiers, bool isTrimmed)
		{
			var dict = new Dictionary<string, string> () {
				{ "UseMonoRuntime", "false" },
				{ "PublishReadyToRun", "false" },
				{ "NoDSymUtil", "false" }, // off by default for macOS, but we want to test it, so enable it
				{ "Registrar", "trimmable-static" },
			};
			Run (platform, runtimeIdentifiers, "Release", $"{platform}-CoreCLR-Interpreter-TrimmableStatic", isTrimmed, dict);
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

			if (!Configuration.XcodeIsStable)
				Assert.Ignore ("Using a beta version of Xcode, so disabling this test (it will need very frequent updates to the known failures, which is better delayed until the final Xcode release)");

			var project = "SizeTestApp";
			var project_path = GetProjectPath (project, runtimeIdentifiers: runtimeIdentifiers, platform: platform, out var appPath, configuration: configuration);

			Clean (project_path);

			var properties = GetDefaultProperties (runtimeIdentifiers, extraProperties: extraProperties);
			properties ["Configuration"] = configuration;

			// Disable code signing: the code signature isn't relevant to the app size we want to track, and it's not
			// deterministic between machines. In particular the code signature's hash page size (and thus the number of
			// hashes, and thus the signature size) depends on the version of the 'codesign' tool, which is part of the OS,
			// so the app size would otherwise differ depending on the macOS version of the machine that built the app.
			properties ["EnableCodeSigning"] = "false";

			DotNet.AssertBuild (project_path, properties);

			// FORCE_UPDATE_KNOWN_FAILURES will update the known failures files even if the test doesn't actually fail
			// WRITE_KNOWN_FAILURES will only update the known failures files if the test fails (and mark the test as passed)
			// If neither is set, the updated expected file is uploaded as an Azure DevOps artifact.

			var forceUpdate = !string.IsNullOrEmpty (Environment.GetEnvironmentVariable ("FORCE_UPDATE_KNOWN_FAILURES"));
			var update = forceUpdate || !string.IsNullOrEmpty (Environment.GetEnvironmentVariable ("WRITE_KNOWN_FAILURES"));
			var expectedDirectory = Path.Combine (Configuration.SourceRoot, "tests", "dotnet", "UnitTests", "expected");

			try {
				Assert.Multiple (() => {
					AssertAppSize (platform, name, appPath, update, forceUpdate, expectedDirectory);

					if (supportsAssemblyInspection)
						AssertAssemblyReport (platform, name, appPath, update, expectedDirectory);

					AssertExpectedDSyms (platform, appPath);
				});
			} catch {
				// If the test fails on CI, copy the resulting .app bundle to a location that will be uploaded as an
				// Azure DevOps artifact (zipped). This is for diagnostic purposes only.
				CopyAppBundleForDiagnostics (name, appPath);
				throw;
			}
		}

		static void CopyAppBundleForDiagnostics (string name, string appPath)
		{
			var artifactStagingDir = Environment.GetEnvironmentVariable ("BUILD_ARTIFACTSTAGINGDIRECTORY");
			if (string.IsNullOrEmpty (artifactStagingDir))
				return; // not running in Azure DevOps CI

			try {
				var outputDir = Path.Combine (artifactStagingDir, "failed-app-size-bundles");
				Directory.CreateDirectory (outputDir);
				// Use a sortable timestamp to make the zip file name unique, so subsequent failing tests don't overwrite it.
				var timestamp = DateTime.UtcNow.ToString ("yyyyMMdd'T'HHmmss'Z'");
				var zipPath = Path.Combine (outputDir, $"{name}-{Path.GetFileName (appPath)}-{timestamp}.zip");
				Console.WriteLine ($"    Zipping app bundle '{appPath}' to '{zipPath}' for diagnostics...");
				System.IO.Compression.ZipFile.CreateFromDirectory (appPath, zipPath, System.IO.Compression.CompressionLevel.Optimal, includeBaseDirectory: true);
				Console.WriteLine ($"    Zipped app bundle to '{zipPath}'.");
			} catch (Exception e) {
				Console.WriteLine ($"    Failed to zip app bundle for diagnostics: {e}");
			}
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
			foreach (var file in allFiles.OrderBy (v => v.FullName)) {
				// Write the file length on a different line, so that it's easier to compute length changes in a diff (the file name stays the same, only the length line changes).
				// Also if files are added or removed, in addition to other files change their lengths, this will make those additions/removals stand out more in diffs.
				report.AppendLine ($"{file.FullName [(appPath.Length + 1)..]}:");
				report.AppendLine ($"    {FormatBytes (file.Length)}");
			}
			var expectedSizeReportPath = Path.Combine (expectedDirectory, $"{name}-size.txt");
			var expectedSizeReport = "";
			var expectedAppBundleSize = 0L;
			if (File.Exists (expectedSizeReportPath)) {
				expectedSizeReport = File.ReadAllText (expectedSizeReportPath);
				if (!long.TryParse (expectedSizeReport.SplitLines ().First ().Replace ("AppBundleSize: ", "").Replace (",", "").Replace (".", "").RemoveAfterFirstSpace (), out expectedAppBundleSize)) {
					expectedSizeReport = "";
				}
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

			Console.WriteLine ($"    {msg}");

			// Compare individual files in the app bundle
			var expectedLines = ParseFileSizes (expectedSizeReport);
			var actualLines = ParseFileSizes (report.ToString ());
			var allKeys = expectedLines.Keys.Union (actualLines.Keys).OrderBy (v => v);
			var filesAdded = new List<string> ();
			var filesRemoved = new List<string> ();
			foreach (var key in allKeys) {
				if (!expectedLines.TryGetValue (key, out var expectedLine)) {
					Console.WriteLine ($"        File '{key}' was added to app bundle: {actualLines [key]}");
					filesAdded.Add (key);
				} else if (!actualLines.TryGetValue (key, out var actualLine)) {
					Console.WriteLine ($"        File '{key}' was removed from app bundle: {expectedLine}");
					filesRemoved.Add (key);
				} else if (expectedLine != actualLine) {
					Console.WriteLine ($"        File '{key}' changed in app bundle:");
					Console.WriteLine ($"            -{expectedLine}");
					Console.WriteLine ($"            +{actualLine}");
				}
			}

			// Determine if there are any meaningful differences
			var hasFileDifferences = filesAdded.Count > 0 || filesRemoved.Count > 0;
			var hasSizeDifference = !withinTolerance;
			var hasDifferences = hasFileDifferences || hasSizeDifference;

			if (forceUpdate || (update && hasDifferences)) {
				Directory.CreateDirectory (expectedDirectory);
				File.WriteAllText (expectedSizeReportPath, report.ToString ());
				Console.WriteLine ($"    Updated expected file: {expectedSizeReportPath}");
			} else if (hasDifferences) {
				UploadUpdatedExpectedFile (expectedSizeReportPath, report.ToString ());
				var updateHint = GetUpdateHint ();
				if (hasFileDifferences) {
					var details = new List<string> ();
					foreach (var key in filesAdded)
						details.Add ($"added: '{key}'");
					foreach (var key in filesRemoved)
						details.Add ($"removed: '{key}'");
					Assert.Fail ($"The app bundle's file list changed ({string.Join (", ", details)}). {updateHint}");
				}
				Assert.Fail ($"{msg} {updateHint}");
			}
		}

		// Parse a size report (either an expected file or a freshly generated report) into a dictionary
		// mapping each file's relative path to its formatted size. The format is two lines per file: the
		// file name (ending with ':') followed by an indented line with the size.
		static Dictionary<string, string> ParseFileSizes (string report)
		{
			var rv = new Dictionary<string, string> ();
			var lines = report.SplitLines ();
			// Skip the first two lines: the total 'AppBundleSize' line and the '#' comment line.
			for (var i = 2; i < lines.Length; i++) {
				var line = lines [i];
				if (!line.EndsWith (":", StringComparison.Ordinal))
					continue;
				var name = line [..^1];
				var size = i + 1 < lines.Length ? lines [i + 1].Trim () : "";
				rv [name] = size;
			}
			return rv;
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
			var expectedAPIs = File.Exists (expectedFile) ? File.ReadAllLines (expectedFile) : [];
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
				if (addedAPIs.Count > 0 || removedAPIs.Count > 0) {
					UploadUpdatedExpectedFile (expectedFile, string.Join ('\n', preservedAPIs) + "\n");
					var updateHint = " " + GetUpdateHint ();
					Assert.That (addedAPIs, Is.Empty, "Unexpected APIs were added to the preserved set." + updateHint);
					Assert.That (removedAPIs, Is.Empty, "APIs were unexpectedly removed from the preserved set." + updateHint);
				}
			}
		}

		static void UploadUpdatedExpectedFile (string expectedFilePath, string content)
		{
			var fileName = Path.GetFileName (expectedFilePath);
			var artifactStagingDir = Environment.GetEnvironmentVariable ("BUILD_ARTIFACTSTAGINGDIRECTORY");
			string outputDir;
			if (!string.IsNullOrEmpty (artifactStagingDir)) {
				outputDir = Path.Combine (artifactStagingDir, "updated-expected-sizes");
			} else {
				outputDir = Path.Combine (Cache.CreateTemporaryDirectory ("AppSizeTest"), "updated-expected-sizes");
			}
			Directory.CreateDirectory (outputDir);
			var outputFile = Path.Combine (outputDir, fileName);
			File.WriteAllText (outputFile, content);
			Console.WriteLine ($"    Updated expected file written to: {outputFile}");
		}

		static string GetUpdateHint ()
		{
			if (IsInCI)
				return "The updated expected file is available as a build artifact (set WRITE_KNOWN_FAILURES=1 to update locally).";
			return "Set WRITE_KNOWN_FAILURES=1 to update the expected files in-place.";
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
