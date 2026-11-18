// Copyright (c) Microsoft Corporation.
// Licensed under the MIT License.

using System;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;
using System.Xml.Linq;

using Microsoft.Build.Framework;
using Microsoft.Build.Utilities;

using NUnit.Framework;

using Xamarin.Tests;

namespace Xamarin.MacDev.Tasks {
	[TestFixture]
	public class BannedLoggingApiTests {
		[Test]
		public void EnforcesCodedLogging ()
		{
			var taskDirectory = Path.Combine (Configuration.RootPath, "msbuild", "Xamarin.MacDev.Tasks");
			var taskProject = XDocument.Load (Path.Combine (taskDirectory, "Xamarin.MacDev.Tasks.csproj"));
			var package = taskProject.Descendants ("PackageReference").Single (v => (string?) v.Attribute ("Include") == "Microsoft.CodeAnalysis.BannedApiAnalyzers");
			var warningsAsErrors = taskProject.Descendants ("WarningsAsErrors").Single ();
			var bannedSymbols = taskProject.Descendants ("AdditionalFiles").Single (v => (string?) v.Attribute ("Include") == "BannedSymbols.txt");
			var directory = Cache.CreateTemporaryDirectory ();
			var project = Path.Combine (directory, "Probe.csproj");
			new XDocument (
				new XElement ("Project", new XAttribute ("Sdk", "Microsoft.NET.Sdk"),
					new XElement ("PropertyGroup",
						new XElement ("TargetFramework", Configuration.DotNetTfm),
						new XElement ("WarningsAsErrors", warningsAsErrors.Value)),
					new XElement ("ItemGroup",
						new XElement (package),
						new XElement ("AdditionalFiles", new XAttribute ("Include", Path.Combine (taskDirectory, bannedSymbols.Attribute ("Include")?.Value ?? ""))),
						new XElement ("Compile", new XAttribute ("Include", Path.Combine (taskDirectory, "LoggingExtensions.cs"))),
						new XElement ("Reference", new XAttribute ("Include", "Microsoft.Build.Framework"),
							new XAttribute ("HintPath", typeof (IBuildEngine).Assembly.Location)),
						new XElement ("Reference", new XAttribute ("Include", "Microsoft.Build.Utilities.Core"),
							new XAttribute ("HintPath", typeof (TaskLoggingHelper).Assembly.Location)))))
				.Save (project);
			File.WriteAllText (Path.Combine (directory, "Probe.cs"), """
				using Microsoft.Build.Utilities;
				using Xamarin.MacDev.Tasks;
				public class Probe {
					public void Log (TaskLoggingHelper log)
					{
						log.LogWarning (51, "input", "warning");
						log.LogError (10, "input", 12, "error");
						log.LogMessage ("message");
					}
				}
				""");
			var result = ExecutionHelper.Execute (DotNet.Executable,
				["build", project, "--nologo", "-v:quiet"],
				out string? output, TimeSpan.FromMinutes (2));
			Assert.That (result, Is.EqualTo (0), output);

			File.WriteAllText (Path.Combine (directory, "Probe.cs"), """
				using Microsoft.Build.Utilities;
				public class Probe {
					public void Log (TaskLoggingHelper log)
					{
				#line 100 "Probe.cs"
						log.LogWarning ("warning");
				#line 200 "Probe.cs"
						log.LogError ("error");
				#line 300 "Probe.cs"
						log.LogWarning (null, null, null, "input", 1, 2, 3, 4, "warning");
				#line 400 "Probe.cs"
						log.LogError (null, "MT0010", null, "input", 1, 2, 3, 4, "error");
					}
				}
				""");
			result = ExecutionHelper.Execute (DotNet.Executable,
				["build", project, "--no-restore", "--nologo", "-v:quiet"],
				out output, TimeSpan.FromMinutes (2));
			Assert.That (result, Is.Not.EqualTo (0), output);
			foreach (var line in new [] { 100, 200, 300, 400 })
				Assert.That (Regex.IsMatch (output ?? "", $@"Probe.cs\({line},\d+\): error RS0030:"), Is.True, $"Missing banned API error at line {line}:\n{output}");
		}
	}
}
