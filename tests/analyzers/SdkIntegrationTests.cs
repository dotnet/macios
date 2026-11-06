// Copyright (c) Microsoft Corporation.
// Licensed under the MIT License.

using System;
using System.Diagnostics;
using System.IO;
using System.Runtime.Versioning;
using System.Text;
using NUnit.Framework;

namespace Microsoft.Macios.Analyzers.Tests {
	[TestFixture]
	public class SdkIntegrationTests {
		[TestCase ("", true, 0)]
		[TestCase ("<NoWarn>APL1000</NoWarn>", false, 0)]
		[TestCase ("<RunAnalyzers>false</RunAnalyzers>", false, 0)]
		[TestCase ("<WarningsAsErrors>APL1000</WarningsAsErrors>", true, 1)]
		public void CompilerLoadsSdkAnalyzer (string properties, bool expectDiagnostic, int expectedExitCode)
		{
			var directory = Path.Combine (TestContext.CurrentContext.WorkDirectory, "obj", "analyzer-integration", Guid.NewGuid ().ToString ());
			var targetsDirectory = Path.Combine (directory, "sdk", "targets");
			var analyzerDirectory = Path.Combine (directory, "sdk", "analyzers", "dotnet", "cs");
			Directory.CreateDirectory (targetsDirectory);
			Directory.CreateDirectory (analyzerDirectory);
			try {
				File.Copy (Path.Combine (AppContext.BaseDirectory, "Microsoft.MaciOS.Analyzers.targets"), Path.Combine (targetsDirectory, "Microsoft.MaciOS.Analyzers.targets"));
				File.Copy (Path.Combine (AppContext.BaseDirectory, "Microsoft.Macios.Analyzers.dll"), Path.Combine (analyzerDirectory, "Microsoft.Macios.Analyzers.dll"));
				var framework = new FrameworkName (AppContext.TargetFrameworkName ?? throw new InvalidOperationException ("The test target framework is unavailable.")).Version;
				File.WriteAllText (Path.Combine (directory, "Test.csproj"), $$"""
					<Project Sdk="Microsoft.NET.Sdk">
					  <PropertyGroup>
					    <TargetFramework>net{{framework.Major}}.{{framework.Minor}}</TargetFramework>
					    <TreatWarningsAsErrors>false</TreatWarningsAsErrors>
					    {{properties}}
					  </PropertyGroup>
					  <Import Project="sdk/targets/Microsoft.MaciOS.Analyzers.targets" />
					</Project>
					""");
				File.WriteAllText (Path.Combine (directory, "Test.cs"), """
					namespace UIKit {
						public enum UIButtonType { Custom }
						public class UIButton {
							public UIButton (UIButtonType type) {}
						}
					}
					class Derived : UIKit.UIButton {
						public Derived () : base (UIKit.UIButtonType.Custom) {}
					}
					""");

				var output = new StringBuilder ();
				var error = new StringBuilder ();
				using var process = new Process {
					StartInfo = new ProcessStartInfo {
						FileName = Environment.GetEnvironmentVariable ("DOTNET_HOST_PATH") ?? "dotnet",
						Arguments = "build Test.csproj --nologo --disable-build-servers -v:minimal",
						WorkingDirectory = directory,
						UseShellExecute = false,
						RedirectStandardOutput = true,
						RedirectStandardError = true,
					},
				};
				process.OutputDataReceived += (_, e) => { if (e.Data is not null) output.AppendLine (e.Data); };
				process.ErrorDataReceived += (_, e) => { if (e.Data is not null) error.AppendLine (e.Data); };
				process.Start ();
				process.BeginOutputReadLine ();
				process.BeginErrorReadLine ();
				var exited = process.WaitForExit (120_000);
				if (!exited)
					process.Kill (entireProcessTree: true);
				process.WaitForExit ();
				var log = output.ToString () + error.ToString ();
				Assert.That (exited, Is.True, log);
				Assert.That (process.ExitCode, Is.EqualTo (expectedExitCode), log);
				Assert.That (log.Contains ("APL1000"), Is.EqualTo (expectDiagnostic), log);
				Assert.That (log, Does.Not.Contain ("CS8032").And.Not.Contain ("AD0001"), log);
			} finally {
				Directory.Delete (directory, recursive: true);
			}
		}
	}
}
