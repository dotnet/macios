//
// Expected files:
//
//  $Env:BUILD_SOURCESDIRECTORY/$Env:BUILD_REPOSITORY_TITLE/jenkins-results/windows-remote-dotnet-tests.trx
//  $Env:BUILD_SOURCESDIRECTORY/$Env:BUILD_REPOSITORY_TITLE/jenkins-results/windows-dotnet-tests.trx
//  $Env:BUILD_SOURCESDIRECTORY/$Env:BUILD_REPOSITORY_TITLE/jenkins-results/windows/bgen-tests/results.trx
//  $Env:BUILD_SOURCESDIRECTORY/$Env:BUILD_REPOSITORY_TITLE/jenkins-results/windows-remote-logs.zip
//  $Env:BUILD_SOURCESDIRECTORY/$Env:BUILD_REPOSITORY_TITLE/jenkins-results/tests/**/*.binlog
//

using System.IO;
using System.Text;
using System.Xml;

public class Program {
	static string GetOutcomeColor (string outcome)
	{
		switch (outcome.ToLower ()) {
		case "passed":
		case "completed":
			return "green";
		case "notexecuted":
			return "orange";
		default:
			return "red";
		}
	}

	static string FormatHtml (string text)
	{
		text = text.Replace ("\r", "");
		text = text.Replace ("&", "&amp;");
		text = text.Replace ("<", "&lt;");
		text = text.Replace (">", "&gt;");
		text = text.Replace ("  ", "&nbsp;&nbsp;");
		text = text.Replace (" &nbsp;", "&nbsp;&nbsp;");
		text = text.Replace ("&nbsp; ", "&nbsp;&nbsp;");
		text = text.Replace ("\n", "<br />\n");
		return text;
	}

	static string GetSourcesDirectory ()
	{
		var pwd = Environment.CurrentDirectory!;
		var dir = pwd;
		while (true) {
			if (Directory.Exists (Path.Combine (dir, ".git")))
				return dir;
			var parentDir = Path.GetDirectoryName (dir);
			if (string.IsNullOrEmpty (parentDir) || parentDir == dir || parentDir.Length <= 2)
				throw new Exception ($"Unable to find a .git subdirectory in any directory up the directory hierarchy from {pwd}");
			dir = parentDir;
		}
		throw new Exception ($"Unable to find a .git subdirectory in any directory up the directory hierarchy from {pwd}");
	}

	public static int Main (string [] args)
	{
		var sourcesDirectory = GetSourcesDirectory ();
		var allTestsSucceeded = true;
		var outputDirectory = Path.Combine (sourcesDirectory, "jenkins-results");
		var indexFile = Path.Combine (outputDirectory, "index.html");
		var summaryFile = Path.Combine (sourcesDirectory, "tests", "TestSummary.md");
		var vsdropsDirectory = Path.Combine (outputDirectory, "tests");
		var vsdropsFile = Path.Combine (vsdropsDirectory, "vsdrops_index.html");

		var vsdropsPrefix = Environment.GetEnvironmentVariable ("VSDROPSPREFIX") ?? string.Empty;
		var vsdropsBuildNumber = Environment.GetEnvironmentVariable ("BUILD_BUILDNUMBER") ?? string.Empty;
		var vsdropsBuildId = Environment.GetEnvironmentVariable ("BUILD_BUILDID") ?? string.Empty;
		var vsdropsJobAttempt = Environment.GetEnvironmentVariable ("SYSTEM_JOBATTEMPT") ?? string.Empty;
		var vsdropsUri = Path.Combine (vsdropsPrefix, vsdropsBuildNumber, vsdropsBuildId, $"windows_integrationwindows-{vsdropsJobAttempt}").Replace ('\\', '/') + "/;";

		var trxFiles = new [] {
			new { Name = "Remote .NET tests", TestResults = Path.Combine (outputDirectory, "windows-remote-dotnet-tests.trx") },
			new { Name = "Local .NET tests", TestResults = Path.Combine (outputDirectory, "windows-local-dotnet-tests.trx") },
			new { Name = "BGen tests", TestResults = Path.Combine (outputDirectory, "windows", "bgen-tests", "results.trx") },
		};

		var extraFiles = new List<string> () {
			Path.Combine (outputDirectory, "windows-remote-logs.zip"),
		};
		extraFiles.AddRange (Directory.GetFiles (outputDirectory, "*.binlog", SearchOption.AllDirectories));

		var indexContents = new StringBuilder ();
		var summaryContents = new StringBuilder ();

		indexContents.AppendLine ($"<!DOCTYPE html>");
		indexContents.AppendLine ($"<html>");
		indexContents.AppendLine ($"  <head>");
		indexContents.AppendLine ($"    <meta charset=\"utf-8\"/>");
		indexContents.AppendLine ($"    <title>Test results</title>");
		indexContents.AppendLine ($"    <style>");
		indexContents.AppendLine ($"      .pdiv {{");
		indexContents.AppendLine ($"        display: table;");
		indexContents.AppendLine ($"        padding-top: 10px;");
		indexContents.AppendLine ($"      }}");
		indexContents.AppendLine ($"    </style>");
		indexContents.AppendLine ($"  </head>");
		indexContents.AppendLine ($"  <body>");
		indexContents.AppendLine ($"    <h1>Test results</h1>");

		indexContents.AppendLine ($"    <div>");
		var stepUrl = $"{Environment.GetEnvironmentVariable ("SYSTEM_TEAMFOUNDATIONCOLLECTIONURI")}" +
						$"{Environment.GetEnvironmentVariable ("SYSTEM_TEAMPROJECT")}" +
						$"/_build" +
						$"/results?buildId={Environment.GetEnvironmentVariable ("BUILD_BUILDID")}" +
						$"&view=logs" +
						$"&j={Environment.GetEnvironmentVariable ("SYSTEM_JOBID")}";
		indexContents.AppendLine ($"        Step: <a href='{stepUrl}'>{stepUrl}</a> <br />");
		var artifactsUrl = $"{Environment.GetEnvironmentVariable ("SYSTEM_TEAMFOUNDATIONCOLLECTIONURI")}" +
						$"{Environment.GetEnvironmentVariable ("SYSTEM_TEAMPROJECT")}" +
						$"/_build" +
						$"/results?buildId={Environment.GetEnvironmentVariable ("BUILD_BUILDID")}" +
						$"&view=artifacts" +
						$"&pathAsName=false" +
						$"&type=publishedArtifacts";
		indexContents.AppendLine ($"        Artifacts: <a href='{artifactsUrl}'>{artifactsUrl}</a> <br />");
		indexContents.AppendLine ($"    </div>");

		foreach (var trx in trxFiles) {
			var name = trx.Name;
			var path = trx.TestResults;
			string? outcome;
			var messageLines = new List<string> ();

			try {
				var xml = new XmlDocument ();
				xml.Load (path);
				outcome = xml.SelectSingleNode ("/*[local-name() = 'TestRun']/*[local-name() = 'ResultSummary']")?.Attributes? ["outcome"]?.Value;
				if (outcome is null) {
					outcome = $"Could not find outcome in trx file {path}";
				} else {
					var failedTests = xml.SelectNodes ("/*[local-name() = 'TestRun']/*[local-name() = 'Results']/*[local-name() = 'UnitTestResult'][@outcome != 'Passed']")?.Cast<XmlNode> ();
					if (failedTests?.Any () == true) {
						messageLines.Add ("        <ul>");
						foreach (var node in failedTests) {
							var testName = node.Attributes? ["testName"]?.Value ?? "<unknown test name>";
							var testOutcome = node.Attributes? ["outcome"]?.Value ?? "<unknown test outcome>";
							var testMessage = node.SelectSingleNode ("*[local-name() = 'Output']/*[local-name() = 'ErrorInfo']/*[local-name() = 'Message']")?.InnerText;

							var testId = node.Attributes? ["testId"]?.Value;
							if (!string.IsNullOrEmpty (testId)) {
								var testMethod = xml.SelectSingleNode ($"/*[local-name() = 'TestRun']/*[local-name() = 'TestDefinitions']/*[local-name() = 'UnitTest'][@id='{testId}']/*[local-name() = 'TestMethod']");
								var className = testMethod?.Attributes? ["className"]?.Value ?? string.Empty;
								if (!string.IsNullOrEmpty (className))
									testName = className + "." + testName;
							}

							if (string.IsNullOrEmpty (testMessage)) {
								messageLines.Add ($"        <li>{testName} (<span style='color: {GetOutcomeColor (testOutcome)}'>{testOutcome}</span>)</li>");
							} else if (testMessage.Split ('\n').Length == 1) {
								messageLines.Add ($"        <li>{testName} (<span style='color: {GetOutcomeColor (testOutcome)}'>{testOutcome}</span>): {FormatHtml (testMessage)}</li>");
							} else {
								messageLines.Add ($"        <li>{testName} (<span style='color: {GetOutcomeColor (testOutcome)}'>{testOutcome}</span>)");
								messageLines.Add ($"            <div class='pdiv' style='margin-left: 20px;'>");
								messageLines.Add (FormatHtml (testMessage));
								messageLines.Add ($"            </div>");
								messageLines.Add ($"        </li>");
							}
						}
						messageLines.Add ("        </ul>");
						allTestsSucceeded = false;
					} else if (outcome != "Completed" && outcome != "Passed") {
						messageLines.Add ($"    Failed to find any test failures in the trx file {path}");
					}
				}
				var htmlPath = Path.ChangeExtension (path, "html");
				if (File.Exists (htmlPath)) {
					var relativeHtmlPath = Path.GetRelativePath (outputDirectory, htmlPath);
					messageLines.Add ($"Html results: <a href='{relativeHtmlPath}'>{Path.GetFileName (relativeHtmlPath).Replace ('\\', '/')}</a> <br />");
				}
				var relativeTrxPath = Path.GetRelativePath (outputDirectory, path);
				messageLines.Add ($"Trx results: <a href='{relativeTrxPath}'>{Path.GetFileName (relativeTrxPath).Replace ('\\', '/')}</a> <br />");

			} catch (Exception e) {
				outcome = "Failed to parse test results";
				messageLines.Add ($"<div>{FormatHtml (e.ToString ())}</div>");
				allTestsSucceeded = false;
			}

			indexContents.AppendLine ($"    <div class='pdiv'><span>{name} (</span><span style='color: {GetOutcomeColor (outcome)}'>{outcome}</span><span>)</span></div>");
			if (messageLines.Any ()) {
				indexContents.AppendLine ("    <div class='pdiv' style='margin-left: 20px;'>");
				foreach (var line in messageLines)
					indexContents.AppendLine ($"      {line}");
				indexContents.AppendLine ("    </div>");
			}
		}
		var existingExtraFiles = extraFiles.Where (File.Exists).ToList ();
		if (existingExtraFiles.Any ()) {
			indexContents.AppendLine ($"    <div class='pdiv'>Extra files:</div>");
			indexContents.AppendLine ($"    <ul>");
			foreach (var ef in existingExtraFiles) {
				var relative = Path.GetRelativePath (outputDirectory, ef);
				indexContents.AppendLine ($"      <li><a href='{relative}'>{relative.Replace ('\\', '/')}</a></li>");
			}
			indexContents.AppendLine ($"    </ul>");
		}
		indexContents.AppendLine ($"  </body>");
		indexContents.AppendLine ($"</html>");

		if (allTestsSucceeded) {
			summaryContents.AppendLine ($"# :tada: All {trxFiles.Length} tests passed :tada:");
		} else {
			summaryContents.AppendLine ($"# :tada: All {trxFiles.Length} tests passed :tada:");
		}

		Directory.CreateDirectory (outputDirectory);
		var indexContentsValue = indexContents.ToString ();
		File.WriteAllText (indexFile, indexContents.ToString ());
		File.WriteAllText (summaryFile, summaryContents.ToString ());

		var vstsIndexContents = indexContentsValue
								.Replace ("a href='https", "a href=@https") // we don't want to rewrite https links, so make them look like something else
								.Replace ("a href='", "a href='" + vsdropsUri) // rewrite local links to vsdrops
								.Replace ("a href=@https", "a href='https"); // rewrite https links back to normal

		Directory.CreateDirectory (vsdropsDirectory);
		File.WriteAllText (vsdropsFile, vstsIndexContents);

		Console.WriteLine ($"Created {indexFile} successfully.");
		Console.WriteLine ($"Created {summaryFile} successfully.");
		Console.WriteLine ($"Created {vsdropsFile} successfully.");
		Console.WriteLine ($"All tests succeeded: {allTestsSucceeded}");
		return 0;
	}
}
