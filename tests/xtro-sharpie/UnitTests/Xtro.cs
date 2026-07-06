using System.Diagnostics;
using System.IO.Compression;
using System.Xml;

#nullable enable

namespace Xamarin.Tests {
	[TestFixture]
	public class Xtro {
		[Test]
		public void RunTest ()
		{
			var dir = Path.Combine (Configuration.SourceRoot, "tests", "xtro-sharpie");
			var args = new [] {
				"-C", dir,
				"report-dotnet/report.zip",
				"-j", "8",
			};
			var rv = ExecutionHelper.Execute ("make", args);

			var reportDir = Path.Combine (dir, "report-dotnet");
			var report = Path.Combine (reportDir, "index.html");
			if (File.Exists (report)) {
				Console.WriteLine ($"Added {report} as attachment.");
				TestContext.AddTestAttachment (report, "HTML report");
			}

			// Zip up the report directory ourselves if make didn't get to it
			// (make stops at the index.html target when there are unclassified entries).
			var zippedReport = Path.Combine (reportDir, "report.zip");
			if (!File.Exists (zippedReport) && Directory.Exists (reportDir)) {
				try {
					ZipFile.CreateFromDirectory (reportDir, zippedReport);
				} catch (Exception e) {
					Console.WriteLine ($"Failed to create report zip: {e.Message}");
				}
			}
			if (File.Exists (zippedReport)) {
				Console.WriteLine ($"Added {zippedReport} as attachment.");
				TestContext.AddTestAttachment (zippedReport, "HTML report (zipped)");
			}

			Assert.That (rv, Is.EqualTo (0), "ExitCode");
		}
	}
}
