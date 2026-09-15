using System.Diagnostics;
using System.Xml;

#nullable enable

namespace Xamarin.Tests {
	[TestFixture]
	public class Xtro {
		[Test]
		public void RunTest ()
		{
			var dir = Path.Combine (Configuration.SourceRoot, "tests", "xtro-sharpie");

			// Run the report and zip as separate make invocations so that
			// report.zip is always created even when index.html's recipe
			// returns non-zero (which happens when there are unclassified entries).
			var rv = ExecutionHelper.Execute ("make", new [] { "-C", dir, "report-dotnet/index.html", "-j", "8" });
			var zipRv = ExecutionHelper.Execute ("make", new [] { "-C", dir, "report-dotnet/report.zip" });
			if (zipRv != 0)
				Console.WriteLine ($"Failed to create report.zip (exit code: {zipRv}).");

			var reportDir = Path.Combine (dir, "report-dotnet");
			var report = Path.Combine (reportDir, "index.html");
			if (File.Exists (report)) {
				Console.WriteLine ($"Added {report} as attachment.");
				TestContext.AddTestAttachment (report, "HTML report");
			}

			var zippedReport = Path.Combine (reportDir, "report.zip");
			if (File.Exists (zippedReport)) {
				Console.WriteLine ($"Added {zippedReport} as attachment.");
				TestContext.AddTestAttachment (zippedReport, "HTML report (zipped)");
			}

			Assert.That (rv, Is.EqualTo (0), "ExitCode");
		}
	}
}
