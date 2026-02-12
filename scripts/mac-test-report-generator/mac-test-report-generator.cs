using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Web;

// MacTestReportGenerator: Generates HTML reports for macOS test runs.
// Usage: MacTestReportGenerator --title <title> --output <path>
//        --test <name>:<pass|fail> [--test <name>:<pass|fail> ...]
//        [--stdout <path>] [--stderr <path>] [--crash-reports-dir <path>]
//        [--test-output-dir <path>]

var title = "";
var outputPath = "";
var tests = new List<(string Name, bool Passed)> ();
var stdoutPath = "";
var stderrPath = "";
var crashReportsDir = "";
var testOutputDir = "";

for (int i = 0; i < args.Length; i++) {
	switch (args [i]) {
	case "--title":
		title = args [++i];
		break;
	case "--output":
		outputPath = args [++i];
		break;
	case "--test":
		var parts = args [++i].Split (':', 2);
		tests.Add ((parts [0], parts [1].Equals ("pass", StringComparison.OrdinalIgnoreCase)));
		break;
	case "--stdout":
		stdoutPath = args [++i];
		break;
	case "--stderr":
		stderrPath = args [++i];
		break;
	case "--crash-reports-dir":
		crashReportsDir = args [++i];
		break;
	case "--test-output-dir":
		testOutputDir = args [++i];
		break;
	default:
		Console.Error.WriteLine ($"Unknown argument: {args [i]}");
		return 1;
	}
}

if (string.IsNullOrEmpty (title) || string.IsNullOrEmpty (outputPath)) {
	Console.Error.WriteLine ("Usage: MacTestReportGenerator --title <title> --output <path> --test <name>:<pass|fail> [...]");
	return 1;
}

var outputDir = Path.GetDirectoryName (outputPath);
if (!string.IsNullOrEmpty (outputDir))
	Directory.CreateDirectory (outputDir);

var passedCount = tests.FindAll (t => t.Passed).Count;
var failedCount = tests.Count - passedCount;

// Copy stdout/stderr to output directory as downloadable files
var downloadableFiles = new List<(string DisplayName, string FileName)> ();

if (!string.IsNullOrEmpty (stdoutPath) && File.Exists (stdoutPath)) {
	var destPath = Path.Combine (outputDir!, "stdout.txt");
	File.Copy (stdoutPath, destPath, overwrite: true);
	downloadableFiles.Add (("Standard Output (stdout)", "stdout.txt"));
}

if (!string.IsNullOrEmpty (stderrPath) && File.Exists (stderrPath)) {
	var destPath = Path.Combine (outputDir!, "stderr.txt");
	File.Copy (stderrPath, destPath, overwrite: true);
	downloadableFiles.Add (("Standard Error (stderr)", "stderr.txt"));
}

// Copy per-test output files if available
foreach (var test in tests) {
	if (string.IsNullOrEmpty (testOutputDir))
		continue;
	var testStdout = Path.Combine (testOutputDir, $"{test.Name}-stdout.txt");
	var testStderr = Path.Combine (testOutputDir, $"{test.Name}-stderr.txt");
	if (File.Exists (testStdout)) {
		var destName = $"{test.Name}-stdout.txt";
		File.Copy (testStdout, Path.Combine (outputDir!, destName), overwrite: true);
		downloadableFiles.Add (($"{test.Name} stdout", destName));
	}
	if (File.Exists (testStderr)) {
		var destName = $"{test.Name}-stderr.txt";
		File.Copy (testStderr, Path.Combine (outputDir!, destName), overwrite: true);
		downloadableFiles.Add (($"{test.Name} stderr", destName));
	}
}

// Collect crash reports
var crashReports = new List<(string DisplayName, string FileName)> ();
if (!string.IsNullOrEmpty (crashReportsDir) && Directory.Exists (crashReportsDir)) {
	foreach (var crashFile in Directory.GetFiles (crashReportsDir)) {
		var fileName = Path.GetFileName (crashFile);
		var destPath = Path.Combine (outputDir!, fileName);
		File.Copy (crashFile, destPath, overwrite: true);
		crashReports.Add ((fileName, fileName));
	}
}

// Generate HTML
var sb = new StringBuilder ();
sb.AppendLine ("<!DOCTYPE html>");
sb.AppendLine ("<html>");
sb.AppendLine ($"<head><title>macOS Test Results - {HttpUtility.HtmlEncode (title)}</title>");
sb.AppendLine ("<style>");
sb.AppendLine ("body { font-family: -apple-system, BlinkMacSystemFont, 'Segoe UI', Helvetica, Arial, sans-serif; margin: 40px; }");
sb.AppendLine ("table { border-collapse: collapse; width: 100%; max-width: 800px; }");
sb.AppendLine ("th, td { border: 1px solid #ddd; padding: 12px 16px; text-align: left; }");
sb.AppendLine ("th { background-color: #f6f8fa; font-weight: 600; }");
sb.AppendLine (".passed { color: #1a7f37; font-weight: 600; }");
sb.AppendLine (".failed { color: #cf222e; font-weight: 600; }");
sb.AppendLine ("h1 { border-bottom: 1px solid #d0d7de; padding-bottom: 8px; }");
sb.AppendLine ("h2 { margin-top: 32px; }");
sb.AppendLine (".summary { margin: 16px 0; padding: 12px; border-radius: 6px; }");
sb.AppendLine (".summary.pass { background-color: #dafbe1; }");
sb.AppendLine (".summary.fail { background-color: #ffebe9; }");
sb.AppendLine ("ul { list-style-type: none; padding-left: 0; }");
sb.AppendLine ("ul li { padding: 4px 0; }");
sb.AppendLine ("ul li a { color: #0969da; text-decoration: none; }");
sb.AppendLine ("ul li a:hover { text-decoration: underline; }");
sb.AppendLine ("</style>");
sb.AppendLine ("</head>");
sb.AppendLine ("<body>");
sb.AppendLine ($"<h1>macOS Test Results - {HttpUtility.HtmlEncode (title)}</h1>");

if (failedCount == 0) {
	sb.AppendLine ($"<div class=\"summary pass\">&#x2705; All {passedCount} tests passed.</div>");
} else {
	sb.AppendLine ($"<div class=\"summary fail\">&#x274C; {failedCount} tests failed, {passedCount} tests passed.</div>");
}

sb.AppendLine ("<table>");
sb.AppendLine ("<tr><th>Test Suite</th><th>Result</th></tr>");
foreach (var test in tests) {
	var cssClass = test.Passed ? "passed" : "failed";
	var resultText = test.Passed ? "Passed" : "Failed";
	sb.AppendLine ($"<tr><td>{HttpUtility.HtmlEncode (test.Name)}</td><td class=\"{cssClass}\">{resultText}</td></tr>");
}
sb.AppendLine ("</table>");

// Downloadable files section
if (downloadableFiles.Count > 0) {
	sb.AppendLine ("<h2>Output Logs</h2>");
	sb.AppendLine ("<ul>");
	foreach (var file in downloadableFiles) {
		sb.AppendLine ($"<li><a href=\"{HttpUtility.HtmlAttributeEncode (file.FileName)}\">{HttpUtility.HtmlEncode (file.DisplayName)}</a></li>");
	}
	sb.AppendLine ("</ul>");
}

// Crash reports section
if (crashReports.Count > 0) {
	sb.AppendLine ("<h2>Crash Reports</h2>");
	sb.AppendLine ("<ul>");
	foreach (var report in crashReports) {
		sb.AppendLine ($"<li><a href=\"{HttpUtility.HtmlAttributeEncode (report.FileName)}\">{HttpUtility.HtmlEncode (report.DisplayName)}</a></li>");
	}
	sb.AppendLine ("</ul>");
} else {
	sb.AppendLine ("<h2>Crash Reports</h2>");
	sb.AppendLine ("<p>No crash reports found.</p>");
}

sb.AppendLine ("</body></html>");

File.WriteAllText (outputPath, sb.ToString ());
Console.WriteLine ($"HTML report written to {outputPath}");
return 0;
