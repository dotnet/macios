namespace Xamarin.Utils;

using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.IO;
using System.Linq;
using System.Xml;

#nullable enable

public class TrxParser {
	public class TrxTestResult {
		public required string Name;
		public required string Outcome;
		public required string Message;

		public bool Passed { get => Outcome == "Passed"; }
	}

	public static bool TryParseTrxFile (string path, [NotNullWhen (true)] out IList<TrxTestResult>? failedTests, [NotNullWhen (true)] out string? outcome, out bool allTestsSucceeded, out Exception? exception)
	{
		allTestsSucceeded = false;
		failedTests = null;
		outcome = null;
		exception = null;

		if (!File.Exists (path))
			return false;

		var rv = new List<TrxTestResult> ();
		try {
			var xml = new XmlDocument ();
			xml.Load (path);
			outcome = xml.SelectSingleNode ("/*[local-name() = 'TestRun']/*[local-name() = 'ResultSummary']")?.Attributes? ["outcome"]?.Value;
			if (outcome is null) {
				outcome = $"Could not find outcome in trx file {path}";
			} else {
				var failedTestsQuery = xml.SelectNodes ("/*[local-name() = 'TestRun']/*[local-name() = 'Results']/*[local-name() = 'UnitTestResult'][@outcome != 'Passed' and @outcome != 'NotExecuted']")?.Cast<XmlNode> ();
				if (failedTestsQuery?.Any () == true) {
					foreach (var node in failedTestsQuery) {
						var testName = node.Attributes? ["testName"]?.Value ?? "<unknown test name>";
						var testOutcome = node.Attributes? ["outcome"]?.Value ?? "<unknown test outcome>";
						var testMessage = node.SelectSingleNode ("*[local-name() = 'Output']/*[local-name() = 'ErrorInfo']/*[local-name() = 'Message']")?.InnerText ?? "";

						var testId = node.Attributes? ["testId"]?.Value;
						if (!string.IsNullOrEmpty (testId)) {
							var testMethod = xml.SelectSingleNode ($"/*[local-name() = 'TestRun']/*[local-name() = 'TestDefinitions']/*[local-name() = 'UnitTest'][@id='{testId}']/*[local-name() = 'TestMethod']");
							var className = testMethod?.Attributes? ["className"]?.Value ?? string.Empty;
							if (!string.IsNullOrEmpty (className))
								testName = className + "." + testName;
						}

						rv.Add (new TrxTestResult () {
							Name = testName,
							Outcome = testOutcome,
							Message = testMessage,
						});
					}
					allTestsSucceeded = false;
				} else if (outcome != "Completed" && outcome != "Passed") {
					// failed, but no test failures?
					allTestsSucceeded = false;
				} else {
					allTestsSucceeded = true;
				}
			}
			failedTests = rv;
			return true;
		} catch (Exception e) {
			outcome = "Failed to parse test results";
			exception = e;
			allTestsSucceeded = false;
			return false;
		}
	}
}
