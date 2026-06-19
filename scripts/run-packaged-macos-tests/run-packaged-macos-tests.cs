// Copyright (c) Microsoft Corporation.
// Licensed under the MIT License.

// run-packaged-macos-tests: Runs pre-built packaged macOS / Mac Catalyst test suites.
//
// Usage:
//   dotnet exec run-packaged-macos-tests.dll
//       --tests-directory <path>            Path to the extracted mac-test-package/tests directory
//       --dotnet-tfm <tfm>                  Target framework moniker (e.g. net10.0)
//       [--include-mac]                     Include macOS tests
//       [--include-maccatalyst]             Include Mac Catalyst tests
//       [--configuration <Debug|Release>]   Build configuration (default: Debug)
//       [--title <title>]                   Title for the HTML report
//       [--html-report-path <path>]         Path for the vsdrops HTML report
//       [--test-summary-path <path>]        Path for TestSummary.md
//       [--crash-reports-dir <path>]        Path to crash reports directory
//       [--test-output-dir <path>]          Path for per-test stdout/stderr files
//       [--vsdrops-uri <uri>]              VSDrops URI prefix for link rewriting
//       [--timeout <seconds>]              Default timeout per test in seconds (default: 300)
//       [--timeout-longer <seconds>]       Longer timeout for heavy tests (default: 600)
//       [--launch-arguments <args>]        Extra arguments passed to test executables

using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading;
using System.Web;

// Parse arguments
var testsDirectory = "";
var dotnetTfm = "";
var includeMac = false;
var includeMacCatalyst = false;
var configuration = "Debug";
var title = "macOS Tests";
var htmlReportPath = "";
var testSummaryPath = "";
var crashReportsDir = "";
var testOutputDir = "";
var vsdropsUri = "";
var defaultTimeout = 300;
var longerTimeout = 600;
var launchArguments = "--autostart --autoexit";

for (int i = 0; i < args.Length; i++) {
	switch (args [i]) {
	case "--tests-directory":
		testsDirectory = args [++i];
		break;
	case "--dotnet-tfm":
		dotnetTfm = args [++i];
		break;
	case "--include-mac":
		includeMac = true;
		break;
	case "--include-maccatalyst":
		includeMacCatalyst = true;
		break;
	case "--configuration":
		configuration = args [++i];
		break;
	case "--title":
		title = args [++i];
		break;
	case "--html-report-path":
		htmlReportPath = args [++i];
		break;
	case "--test-summary-path":
		testSummaryPath = args [++i];
		break;
	case "--crash-reports-dir":
		crashReportsDir = args [++i];
		break;
	case "--test-output-dir":
		testOutputDir = args [++i];
		break;
	case "--vsdrops-uri":
		vsdropsUri = args [++i];
		break;
	case "--timeout":
		defaultTimeout = int.Parse (args [++i]);
		break;
	case "--timeout-longer":
		longerTimeout = int.Parse (args [++i]);
		break;
	case "--launch-arguments":
		launchArguments = args [++i];
		break;
	default:
		Console.Error.WriteLine ($"Unknown argument: {args [i]}");
		return 1;
	}
}

if (string.IsNullOrEmpty (testsDirectory) || string.IsNullOrEmpty (dotnetTfm)) {
	Console.Error.WriteLine ("Required arguments: --tests-directory <path> --dotnet-tfm <tfm>");
	Console.Error.WriteLine ("At least one of --include-mac or --include-maccatalyst must be specified.");
	return 1;
}

if (!includeMac && !includeMacCatalyst) {
	Console.Error.WriteLine ("At least one of --include-mac or --include-maccatalyst must be specified.");
	return 1;
}

testsDirectory = Path.GetFullPath (testsDirectory);
if (!Directory.Exists (testsDirectory)) {
	Console.Error.WriteLine ($"Tests directory does not exist: {testsDirectory}");
	return 1;
}

if (!string.IsNullOrEmpty (testOutputDir))
	Directory.CreateDirectory (testOutputDir);

var isAppleSilicon = RuntimeInformation.ProcessArchitecture == Architecture.Arm64 ||
	Environment.GetEnvironmentVariable ("PROCESSOR_ARCHITECTURE")?.Contains ("ARM", StringComparison.OrdinalIgnoreCase) == true;

// Define test suites
var normalTests = new[] {
	new TestSuite ("monotouch-test", "monotouchtest", true, false),
	new TestSuite ("introspection", "introspection", true, false),
};

var linkerTests = new[] {
	new TestSuite ("dontlink", "dont link", false, true),
	new TestSuite ("linksdk", "link sdk", false, true),
	new TestSuite ("linkall", "link all", false, true),
};

var allTests = normalTests.Concat (linkerTests).ToArray ();

// Define platforms and architectures
var testConfigs = new List<TestConfig> ();
foreach (var test in allTests) {
	if (includeMac) {
		testConfigs.Add (new TestConfig (test, "macOS", "osx-x64", "macos"));
		testConfigs.Add (new TestConfig (test, "macOS", "osx-arm64", "macos"));
	}
	if (includeMacCatalyst) {
		testConfigs.Add (new TestConfig (test, "MacCatalyst", "maccatalyst-x64", "maccatalyst"));
		testConfigs.Add (new TestConfig (test, "MacCatalyst", "maccatalyst-arm64", "maccatalyst"));
	}
}

// Track results per test suite (grouping all platform/arch configs)
var suiteResults = new Dictionary<string, List<TestResult>> ();
foreach (var test in allTests)
	suiteResults [test.Name] = new List<TestResult> ();

// Execute each test configuration (apps are pre-built in the test package)
Console.WriteLine ();
Console.WriteLine ("=== Executing tests ===");

foreach (var config in testConfigs) {
	// Skip arm64 configs on non-Apple Silicon
	if (config.Rid.EndsWith ("-arm64") && !isAppleSilicon) {
		Console.WriteLine ($"⚠️  Skipping {config.DisplayName} - not running on Apple Silicon");
		suiteResults [config.Suite.Name].Add (new TestResult (config, TestOutcome.Skipped, 0, "Not running on Apple Silicon"));
		continue;
	}

	Console.WriteLine ();
	Console.WriteLine ($"--- {config.DisplayName} ---");

	var executablePath = config.GetExecutablePath (testsDirectory, configuration, dotnetTfm);
	if (!File.Exists (executablePath)) {
		Console.Error.WriteLine ($"❌ Executable not found: {executablePath}");
		suiteResults [config.Suite.Name].Add (new TestResult (config, TestOutcome.Failed, 1, $"Executable not found: {executablePath}"));
		continue;
	}

	// Mac Catalyst apps need --autostart --autoexit, macOS apps don't
	var useLaunchArgs = config.Platform == "MacCatalyst";
	var execArgs = useLaunchArgs ? launchArguments : "";
	var timeout = config.Suite.IsLonger ? longerTimeout : defaultTimeout;

	Console.WriteLine ($"Executing {config.DisplayName}...");
	var (execExit, stdout, stderr) = ExecuteWithTimeout (executablePath, execArgs, timeout);

	// Save output files
	if (!string.IsNullOrEmpty (testOutputDir)) {
		var outputName = config.OutputFileName;
		File.WriteAllText (Path.Combine (testOutputDir, $"{outputName}-stdout.txt"), stdout);
		File.WriteAllText (Path.Combine (testOutputDir, $"{outputName}-stderr.txt"), stderr);
	}

	var outcome = execExit == 0 ? TestOutcome.Passed : TestOutcome.Failed;
	var resultMessage = execExit == 0 ? "Passed" : $"Failed with exit code {execExit}";
	suiteResults [config.Suite.Name].Add (new TestResult (config, outcome, execExit, resultMessage, stdout, stderr));

	var emoji = execExit == 0 ? "✅" : "❌";
	Console.WriteLine ($"{emoji} {config.DisplayName}: {resultMessage}");
}

// Aggregate results per test suite
var suiteOutcomes = new List<(string Name, bool Passed, List<TestResult> Results)> ();
foreach (var test in allTests) {
	var results = suiteResults [test.Name];
	var passed = results.All (r => r.Outcome != TestOutcome.Failed);
	suiteOutcomes.Add ((test.Name, passed, results));
}

var passedSuites = suiteOutcomes.Count (s => s.Passed);
var failedSuites = suiteOutcomes.Count (s => !s.Passed);

Console.WriteLine ();
Console.WriteLine ("=== Results Summary ===");
foreach (var (name, passed, results) in suiteOutcomes) {
	var emoji = passed ? "✅" : "❌";
	var failedConfigs = results.Where (r => r.Outcome == TestOutcome.Failed).Select (r => r.Config.DisplayName).ToList ();
	if (failedConfigs.Count > 0)
		Console.WriteLine ($"{emoji} {name}: FAILED ({string.Join (", ", failedConfigs)})");
	else
		Console.WriteLine ($"{emoji} {name}: PASSED");
}
Console.WriteLine ($"\n{passedSuites} suites passed, {failedSuites} suites failed.");

// Generate TestSummary.md
if (!string.IsNullOrEmpty (testSummaryPath)) {
	GenerateTestSummary (testSummaryPath, suiteOutcomes);
	Console.WriteLine ($"TestSummary written to {testSummaryPath}");
}

// Generate HTML report
if (!string.IsNullOrEmpty (htmlReportPath)) {
	GenerateHtmlReport (htmlReportPath, title, suiteOutcomes, testOutputDir, crashReportsDir, vsdropsUri);
	Console.WriteLine ($"HTML report written to {htmlReportPath}");
}

return failedSuites > 0 ? 1 : 0;

// ===== Helper methods =====

(int ExitCode, string Stdout, string Stderr) ExecuteWithTimeout (string executable, string arguments, int timeoutSeconds)
{
	var launchTimeout = TimeSpan.FromSeconds (10);
	var executionTimeout = TimeSpan.FromSeconds (timeoutSeconds);
	var maxLaunchAttempts = 10;
	var pid = Process.GetCurrentProcess ().Id;

	for (var attempt = 0; attempt < maxLaunchAttempts; attempt++) {
		var launchTimeoutFile = Path.GetFullPath ($"launch-timeout-sentinel-{pid}-{attempt}.txt");
		var launchTimedOut = new ManualResetEvent (false);

		var stdoutSb = new StringBuilder ();
		var stderrSb = new StringBuilder ();

		var p = new Process ();
		p.StartInfo.FileName = executable;
		p.StartInfo.Arguments = arguments;
		p.StartInfo.UseShellExecute = false;
		p.StartInfo.RedirectStandardOutput = true;
		p.StartInfo.RedirectStandardError = true;
		p.StartInfo.EnvironmentVariables ["LAUNCH_SENTINEL_FILE"] = launchTimeoutFile;

		p.OutputDataReceived += (_, e) => {
			if (e.Data is not null)
				stdoutSb.AppendLine (e.Data);
		};
		p.ErrorDataReceived += (_, e) => {
			if (e.Data is not null)
				stderrSb.AppendLine (e.Data);
		};

		var launchTimer = new Thread (() => {
			if (p.WaitForExit ((int) launchTimeout.TotalMilliseconds)) {
				// App finished before launch timeout
			} else if (!File.Exists (launchTimeoutFile)) {
				Console.WriteLine ($"Launch timed out after {launchTimeout.TotalSeconds} seconds.");
				launchTimedOut.Set ();
				AbortProcess (p);
			}
		}) {
			IsBackground = true,
		};

		try {
			Console.WriteLine ($"Launching (attempt #{attempt + 1}): {executable} {arguments}");
			p.Start ();
			p.BeginOutputReadLine ();
			p.BeginErrorReadLine ();

			launchTimer.Start ();

			if (!p.WaitForExit ((int) executionTimeout.TotalMilliseconds)) {
				Console.WriteLine ($"Execution timed out after {executionTimeout.TotalSeconds} seconds.");
				AbortProcess (p);
				p.WaitForExit ();
			}

			launchTimer.Join ();

			if (launchTimedOut.WaitOne (0)) {
				Console.WriteLine ("Launching again since the launch timeout triggered.");
				continue;
			}

			Console.WriteLine ($"Execution completed with exit code {p.ExitCode}");
			return (p.ExitCode, stdoutSb.ToString (), stderrSb.ToString ());
		} finally {
			File.Delete (launchTimeoutFile);
			p.Dispose ();
		}
	}

	return (-1, "", "Failed to launch after maximum attempts");
}

void AbortProcess (Process process)
{
	var exitTimeout = TimeSpan.FromSeconds (60);
	var pid = process.Id;

	Console.WriteLine ($"kill ({pid}, 6);");
	var rv = NativeMethods.kill (pid, 6 /* SIGABRT */);
	if (rv != 0) {
		Console.WriteLine ($"Failed to execute 'kill -6 {pid}'. errno = {Marshal.GetLastWin32Error ()} - process already exited?");
		return;
	}

	var watch = Stopwatch.StartNew ();
	while (watch.Elapsed < exitTimeout) {
		rv = NativeMethods.kill (pid, 0);
		if (rv != 0)
			return;
		Thread.Sleep (50);
	}

	// SIGKILL
	Console.WriteLine ($"kill ({pid}, 9);");
	NativeMethods.kill (pid, 9);
}

void GenerateTestSummary (string path, List<(string Name, bool Passed, List<TestResult> Results)> outcomes)
{
	var dir = Path.GetDirectoryName (path);
	if (!string.IsNullOrEmpty (dir))
		Directory.CreateDirectory (dir);

	var failed = outcomes.Where (o => !o.Passed).ToList ();
	var passed = outcomes.Count (o => o.Passed);

	if (failed.Count == 0) {
		File.WriteAllText (path, $"# :tada: All {passed} tests passed :tada:\n");
		return;
	}

	var sb = new StringBuilder ();
	sb.AppendLine ("# Test results");
	sb.AppendLine ("<details>");
	sb.AppendLine ($"<summary>{failed.Count} tests failed, {passed} tests passed.</summary>");
	sb.AppendLine ();
	sb.AppendLine ("## Failed tests");
	sb.AppendLine ();

	foreach (var (name, _, results) in failed) {
		var failedResults = results.Where (r => r.Outcome == TestOutcome.Failed).ToList ();
		foreach (var result in failedResults) {
			sb.AppendLine ($"* {result.Config.DisplayName}: Failed (exit code {result.ExitCode})");

			// Show [FAIL] lines
			var failLines = ExtractFailLines (result.Stdout, result.Stderr);
			if (failLines.Count > 0) {
				var maxShow = Math.Min (failLines.Count, 3);
				for (var j = 0; j < maxShow; j++)
					sb.AppendLine ($"    * `{failLines [j]}`");
				if (failLines.Count > 3)
					sb.AppendLine ($"    * ... and {failLines.Count - 3} more failures");
			} else {
				// Show stderr tail for context
				var stderrLines = result.Stderr.Split ('\n', StringSplitOptions.RemoveEmptyEntries);
				if (stderrLines.Length > 0) {
					sb.AppendLine ("    * No test failure details available. stderr output:");
					foreach (var line in stderrLines.TakeLast (10))
						sb.AppendLine ($"        * `{line}`");
				} else {
					sb.AppendLine ("    * No test failure details available.");
				}
			}
		}
	}

	sb.AppendLine ("</details>");
	File.WriteAllText (path, sb.ToString ());
}

List<string> ExtractFailLines (string stdout, string stderr)
{
	var failLines = new List<string> ();
	foreach (var text in new[] { stdout, stderr }) {
		if (string.IsNullOrEmpty (text))
			continue;
		foreach (var line in text.Split ('\n')) {
			var idx = line.IndexOf ("[FAIL]", StringComparison.Ordinal);
			if (idx >= 0)
				failLines.Add (line.Substring (idx + "[FAIL]".Length).TrimStart ());
		}
	}
	return failLines;
}

void GenerateHtmlReport (
	string reportPath,
	string reportTitle,
	List<(string Name, bool Passed, List<TestResult> Results)> outcomes,
	string outputDir,
	string crashDir,
	string vsdrops)
{
	var htmlDir = Path.GetDirectoryName (reportPath);
	if (!string.IsNullOrEmpty (htmlDir))
		Directory.CreateDirectory (htmlDir);

	var passedCount = outcomes.Count (o => o.Passed);
	var failedCount = outcomes.Count (o => !o.Passed);

	// Copy per-test output files
	var perTestFiles = new Dictionary<string, List<(string DisplayName, string FileName)>> ();
	var perTestFailures = new Dictionary<string, List<string>> ();

	foreach (var (name, _, results) in outcomes) {
		var files = new List<(string DisplayName, string FileName)> ();
		var failLines = new List<string> ();

		foreach (var result in results) {
			if (!string.IsNullOrEmpty (outputDir)) {
				var baseName = result.Config.OutputFileName;
				var stdoutFile = Path.Combine (outputDir, $"{baseName}-stdout.txt");
				var stderrFile = Path.Combine (outputDir, $"{baseName}-stderr.txt");

				if (File.Exists (stdoutFile) && !string.IsNullOrEmpty (htmlDir)) {
					var destName = $"{baseName}-stdout.txt";
					File.Copy (stdoutFile, Path.Combine (htmlDir, destName), overwrite: true);
					files.Add (($"{result.Config.DisplayName} stdout", destName));
					failLines.AddRange (ExtractFailLinesFromFile (stdoutFile));
				}
				if (File.Exists (stderrFile) && !string.IsNullOrEmpty (htmlDir)) {
					var destName = $"{baseName}-stderr.txt";
					File.Copy (stderrFile, Path.Combine (htmlDir, destName), overwrite: true);
					files.Add (($"{result.Config.DisplayName} stderr", destName));
					failLines.AddRange (ExtractFailLinesFromFile (stderrFile));
				}
			}
		}

		perTestFiles [name] = files;
		perTestFailures [name] = failLines;
	}

	// Collect crash reports
	var crashReports = new List<(string DisplayName, string FileName)> ();
	if (!string.IsNullOrEmpty (crashDir) && Directory.Exists (crashDir) && !string.IsNullOrEmpty (htmlDir)) {
		foreach (var crashFile in Directory.GetFiles (crashDir)) {
			var fileName = Path.GetFileName (crashFile);
			File.Copy (crashFile, Path.Combine (htmlDir, fileName), overwrite: true);
			crashReports.Add ((fileName, fileName));
		}
	}

	// Generate HTML
	var sb = new StringBuilder ();
	sb.AppendLine ("<!DOCTYPE html>");
	sb.AppendLine ("<html>");
	sb.AppendLine ($"<head><title>macOS Test Results - {HttpUtility.HtmlEncode (reportTitle)}</title>");
	sb.AppendLine ("<style>");
	sb.AppendLine ("body { font-family: -apple-system, BlinkMacSystemFont, 'Segoe UI', Helvetica, Arial, sans-serif; margin: 40px; }");
	sb.AppendLine (".passed { color: #1a7f37; font-weight: 600; }");
	sb.AppendLine (".failed { color: #cf222e; font-weight: 600; }");
	sb.AppendLine (".skipped { color: #9a6700; font-weight: 600; }");
	sb.AppendLine ("h1 { border-bottom: 1px solid #d0d7de; padding-bottom: 8px; }");
	sb.AppendLine ("h2 { margin-top: 32px; }");
	sb.AppendLine ("h3 { margin-top: 24px; }");
	sb.AppendLine (".summary { margin: 16px 0; padding: 12px; border-radius: 6px; }");
	sb.AppendLine (".summary.pass { background-color: #dafbe1; }");
	sb.AppendLine (".summary.fail { background-color: #ffebe9; }");
	sb.AppendLine ("table { border-collapse: collapse; margin: 8px 0; }");
	sb.AppendLine ("th, td { border: 1px solid #d0d7de; padding: 6px 12px; text-align: left; }");
	sb.AppendLine ("th { background-color: #f6f8fa; }");
	sb.AppendLine ("ul { padding-left: 20px; }");
	sb.AppendLine ("ul li { padding: 4px 0; }");
	sb.AppendLine ("a { color: #0969da; text-decoration: none; }");
	sb.AppendLine ("a:hover { text-decoration: underline; }");
	sb.AppendLine ("</style>");
	sb.AppendLine ("</head>");
	sb.AppendLine ("<body>");
	sb.AppendLine ($"<h1>macOS Test Results - {HttpUtility.HtmlEncode (reportTitle)}</h1>");

	if (failedCount == 0)
		sb.AppendLine ($"<div class='summary pass'>&#x2705; All {passedCount} test suites passed.</div>");
	else
		sb.AppendLine ($"<div class='summary fail'>&#x274C; {failedCount} test suites failed, {passedCount} test suites passed.</div>");

	// Per-suite sections with per-config breakdown
	sb.AppendLine ("<h2>Test Suites</h2>");
	foreach (var (name, passed, results) in outcomes) {
		var statusEmoji = passed ? "&#x2705;" : "&#x274C;";
		var cssClass = passed ? "passed" : "failed";
		var resultText = passed ? "Passed" : "Failed";
		sb.AppendLine ($"<h3>{statusEmoji} {HttpUtility.HtmlEncode (name)} — <span class='{cssClass}'>{resultText}</span></h3>");

		// Per-config table
		sb.AppendLine ("<table>");
		sb.AppendLine ("<tr><th>Platform</th><th>Architecture</th><th>Result</th><th>Exit Code</th><th>Details</th></tr>");
		foreach (var result in results) {
			var configCss = result.Outcome switch {
				TestOutcome.Passed => "passed",
				TestOutcome.Skipped => "skipped",
				_ => "failed",
			};
			var configText = result.Outcome switch {
				TestOutcome.Passed => "Passed",
				TestOutcome.Skipped => "Skipped",
				_ => "Failed",
			};
			var arch = result.Config.Rid.Split ('-').Last ();
			sb.AppendLine ($"<tr><td>{HttpUtility.HtmlEncode (result.Config.Platform)}</td><td>{arch}</td>" +
				$"<td class='{configCss}'>{configText}</td><td>{result.ExitCode}</td>" +
				$"<td>{HttpUtility.HtmlEncode (result.Message)}</td></tr>");
		}
		sb.AppendLine ("</table>");

		// Show [FAIL] lines
		var failLines = perTestFailures.GetValueOrDefault (name);
		if (failLines is not null && failLines.Count > 0) {
			sb.AppendLine ("<ul style='color: #cf222e; font-family: monospace; font-size: 0.9em;'>");
			var maxFails = Math.Min (failLines.Count, 10);
			for (var j = 0; j < maxFails; j++)
				sb.AppendLine ($"<li>{HttpUtility.HtmlEncode (failLines [j])}</li>");
			if (failLines.Count > 10)
				sb.AppendLine ($"<li>... and {failLines.Count - 10} more failures</li>");
			sb.AppendLine ("</ul>");
		}

		// Output file links
		var files = perTestFiles.GetValueOrDefault (name);
		if (files is not null && files.Count > 0) {
			sb.AppendLine ("<ul>");
			foreach (var file in files)
				sb.AppendLine ($"<li><a href='{HttpUtility.HtmlAttributeEncode (file.FileName)}'>{HttpUtility.HtmlEncode (file.DisplayName)}</a></li>");
			sb.AppendLine ("</ul>");
		}
	}

	// Crash reports
	if (crashReports.Count > 0) {
		sb.AppendLine ("<h2>Crash Reports</h2>");
		sb.AppendLine ("<ul>");
		foreach (var report in crashReports)
			sb.AppendLine ($"<li><a href='{HttpUtility.HtmlAttributeEncode (report.FileName)}'>{HttpUtility.HtmlEncode (report.DisplayName)}</a></li>");
		sb.AppendLine ("</ul>");
	} else {
		sb.AppendLine ("<h2>Crash Reports</h2>");
		sb.AppendLine ("<p>No crash reports found.</p>");
	}

	sb.AppendLine ("</body></html>");

	// Write index.html with relative links
	var indexPath = Path.Combine (htmlDir!, "index.html");
	var htmlContent = sb.ToString ();
	File.WriteAllText (indexPath, htmlContent);

	// Write vsdrops_index.html with rewritten links
	if (!string.IsNullOrEmpty (vsdrops)) {
		var vsdropsContent = htmlContent
			.Replace ("a href='https", "a href=@https")
			.Replace ("a href='", "a href='" + vsdrops)
			.Replace ("a href=@https", "a href='https");
		File.WriteAllText (reportPath, vsdropsContent);
	} else {
		File.WriteAllText (reportPath, htmlContent);
	}
}

List<string> ExtractFailLinesFromFile (string filePath)
{
	var failLines = new List<string> ();
	foreach (var line in File.ReadLines (filePath)) {
		var idx = line.IndexOf ("[FAIL]", StringComparison.Ordinal);
		if (idx >= 0)
			failLines.Add (line.Substring (idx + "[FAIL]".Length).TrimStart ());
	}
	return failLines;
}

// ===== Types =====

record TestSuite (string Name, string ProjectName, bool IsLonger, bool IsLinkerTest);

record TestConfig (TestSuite Suite, string Platform, string Rid, string TfmPlatform)
{
	public string DisplayName => $"{Platform}/{Rid} {Suite.Name}";
	public string OutputFileName => $"{TfmPlatform}-{Rid.Split ('-').Last ()}-{Suite.Name}";

	public string GetProjectDirectory (string testsDir)
	{
		if (Suite.IsLinkerTest)
			return Path.Combine (testsDir, "linker", Suite.ProjectName, "dotnet", Platform);
		return Path.Combine (testsDir, Suite.Name, "dotnet", Platform);
	}

	public string GetExecutablePath (string testsDir, string config, string tfm)
	{
		var projectDir = GetProjectDirectory (testsDir);
		return Path.Combine (projectDir, "bin", config, $"{tfm}-{TfmPlatform}", Rid, $"{Suite.ProjectName}.app", "Contents", "MacOS", Suite.ProjectName);
	}
}

enum TestOutcome { Passed, Failed, Skipped }

record TestResult (TestConfig Config, TestOutcome Outcome, int ExitCode, string Message, string Stdout = "", string Stderr = "");

static class NativeMethods
{
	[DllImport ("/usr/lib/libc.dylib", SetLastError = true)]
	public static extern int kill (int pid, int signal);
}
