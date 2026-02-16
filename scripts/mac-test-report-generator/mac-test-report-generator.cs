using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Web;

// mac-test-report-generator: Generates HTML reports for macOS test runs.
// Usage: mac-test-report-generator --title <title> --output <path>
//        --test <name>:<pass|fail> [--test <name>:<pass|fail> ...]
//        [--test-output-dir <path>] [--crash-reports-dir <path>]
//        [--vsdrops-uri <uri>]

var title = "";
var outputPath = "";
var tests = new List<(string Name, bool Passed)> ();
var crashReportsDir = "";
var testOutputDir = "";
var vsdropsUri = "";

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
	case "--crash-reports-dir":
		crashReportsDir = args [++i];
		break;
	case "--test-output-dir":
		testOutputDir = args [++i];
		break;
	case "--vsdrops-uri":
		vsdropsUri = args [++i];
		break;
	default:
		Console.Error.WriteLine ($"Unknown argument: {args [i]}");
		return 1;
	}
}

if (string.IsNullOrEmpty (title) || string.IsNullOrEmpty (outputPath)) {
	Console.Error.WriteLine ("Usage: mac-test-report-generator --title <title> --output <path> --test <name>:<pass|fail> [...]");
	return 1;
}

var outputDir = Path.GetDirectoryName (outputPath);
if (!string.IsNullOrEmpty (outputDir))
	Directory.CreateDirectory (outputDir);

// Sort tests alphabetically
tests.Sort ((a, b) => string.Compare (a.Name, b.Name, StringComparison.OrdinalIgnoreCase));

var passedCount = tests.Count (t => t.Passed);
var failedCount = tests.Count - passedCount;

// Copy per-test output files to output directory, and extract [FAIL] lines
var perTestFiles = new Dictionary<string, List<(string DisplayName, string FileName)>> ();
var perTestFailures = new Dictionary<string, List<string>> ();
foreach (var test in tests) {
	var files = new List<(string DisplayName, string FileName)> ();
	var failLines = new List<string> ();
	if (!string.IsNullOrEmpty (testOutputDir)) {
		var testStdout = Path.Combine (testOutputDir, $"{test.Name}-stdout.txt");
		var testStderr = Path.Combine (testOutputDir, $"{test.Name}-stderr.txt");
		if (File.Exists (testStdout)) {
			var destName = $"{test.Name}-stdout.txt";
			File.Copy (testStdout, Path.Combine (outputDir!, destName), overwrite: true);
			files.Add (("stdout", destName));
			// Extract [FAIL] lines from stdout
			foreach (var line in File.ReadLines (testStdout)) {
				var idx = line.IndexOf ("[FAIL]", StringComparison.Ordinal);
				if (idx >= 0)
					failLines.Add (line.Substring (idx + "[FAIL]".Length).TrimStart ());
			}
		}
		if (File.Exists (testStderr)) {
			var destName = $"{test.Name}-stderr.txt";
			File.Copy (testStderr, Path.Combine (outputDir!, destName), overwrite: true);
			files.Add (("stderr", destName));
			// Extract [FAIL] lines from stderr too
			foreach (var line in File.ReadLines (testStderr)) {
				var idx = line.IndexOf ("[FAIL]", StringComparison.Ordinal);
				if (idx >= 0)
					failLines.Add (line.Substring (idx + "[FAIL]".Length).TrimStart ());
			}
		}
	}
	perTestFiles [test.Name] = files;
	perTestFailures [test.Name] = failLines;
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

// Generate HTML (using single-quoted hrefs so vsdrops rewriting works)
var sb = new StringBuilder ();
sb.AppendLine ("<!DOCTYPE html>");
sb.AppendLine ("<html>");
sb.AppendLine ($"<head><title>macOS Test Results - {HttpUtility.HtmlEncode (title)}</title>");
sb.AppendLine ("<style>");
sb.AppendLine ("body { font-family: -apple-system, BlinkMacSystemFont, 'Segoe UI', Helvetica, Arial, sans-serif; margin: 40px; }");
sb.AppendLine (".passed { color: #1a7f37; font-weight: 600; }");
sb.AppendLine (".failed { color: #cf222e; font-weight: 600; }");
sb.AppendLine ("h1 { border-bottom: 1px solid #d0d7de; padding-bottom: 8px; }");
sb.AppendLine ("h2 { margin-top: 32px; }");
sb.AppendLine ("h3 { margin-top: 24px; }");
sb.AppendLine (".summary { margin: 16px 0; padding: 12px; border-radius: 6px; }");
sb.AppendLine (".summary.pass { background-color: #dafbe1; }");
sb.AppendLine (".summary.fail { background-color: #ffebe9; }");
sb.AppendLine ("ul { padding-left: 20px; }");
sb.AppendLine ("ul li { padding: 4px 0; }");
sb.AppendLine ("a { color: #0969da; text-decoration: none; }");
sb.AppendLine ("a:hover { text-decoration: underline; }");
sb.AppendLine ("</style>");
sb.AppendLine ("</head>");
sb.AppendLine ("<body>");
sb.AppendLine ($"<h1>macOS Test Results - {HttpUtility.HtmlEncode (title)}</h1>");

if (failedCount == 0) {
	sb.AppendLine ($"<div class='summary pass'>&#x2705; All {passedCount} tests passed.</div>");
} else {
	sb.AppendLine ($"<div class='summary fail'>&#x274C; {failedCount} tests failed, {passedCount} tests passed.</div>");
}

// Per-test subsections
sb.AppendLine ("<h2>Test Suites</h2>");
foreach (var test in tests) {
	var statusEmoji = test.Passed ? "&#x2705;" : "&#x274C;";
	var cssClass = test.Passed ? "passed" : "failed";
	var resultText = test.Passed ? "Passed" : "Failed";
	sb.AppendLine ($"<h3>{statusEmoji} {HttpUtility.HtmlEncode (test.Name)} — <span class='{cssClass}'>{resultText}</span></h3>");

	// Show first 10 [FAIL] lines for failing tests
	var failLines = perTestFailures [test.Name];
	if (failLines.Count > 0) {
		sb.AppendLine ("<ul style='color: #cf222e; font-family: monospace; font-size: 0.9em;'>");
		var maxFails = Math.Min (failLines.Count, 10);
		for (var j = 0; j < maxFails; j++)
			sb.AppendLine ($"<li>{HttpUtility.HtmlEncode (failLines [j])}</li>");
		if (failLines.Count > 10)
			sb.AppendLine ($"<li>... and {failLines.Count - 10} more failures</li>");
		sb.AppendLine ("</ul>");
	}

	var files = perTestFiles [test.Name];
	if (files.Count > 0) {
		sb.AppendLine ("<ul>");
		foreach (var file in files)
			sb.AppendLine ($"<li><a href='{HttpUtility.HtmlAttributeEncode (file.FileName)}'>{HttpUtility.HtmlEncode (file.DisplayName)}</a></li>");
		sb.AppendLine ("</ul>");
	}
}

// Crash reports section
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

// Write index.html with relative links (works when downloaded)
var indexPath = Path.Combine (outputDir!, "index.html");
var htmlContent = sb.ToString ();
File.WriteAllText (indexPath, htmlContent);
Console.WriteLine ($"HTML report written to {indexPath}");

// Write vsdrops_index.html with rewritten links (works on VSDrops)
if (!string.IsNullOrEmpty (vsdropsUri)) {
	var vsdropsContent = htmlContent
		.Replace ("a href='https", "a href=@https")
		.Replace ("a href='", "a href='" + vsdropsUri)
		.Replace ("a href=@https", "a href='https");
	File.WriteAllText (outputPath, vsdropsContent);
	Console.WriteLine ($"VSDrops HTML report written to {outputPath}");
} else {
	File.WriteAllText (outputPath, htmlContent);
	Console.WriteLine ($"HTML report written to {outputPath}");
}

return 0;
