// Copyright (c) Microsoft Corporation.
// Licensed under the MIT License.

namespace AssemblyPreparerTests;

[TestFixture]
public class WarningConfigurationTests {
	[TestCase ("", false, "", "", ErrorHelper.WarningLevel.Warning)]
	[TestCase ("MT1234", false, "", "", ErrorHelper.WarningLevel.Disable)]
	[TestCase ("CS1234;IL1234,MT1234", true, "MT1234", "", ErrorHelper.WarningLevel.Disable)]
	[TestCase ("mt1234", false, "", "", ErrorHelper.WarningLevel.Disable)]
	[TestCase ("MM1234;CS1234;1234", false, "", "", ErrorHelper.WarningLevel.Warning)]
	[TestCase ("", true, "", "", ErrorHelper.WarningLevel.Error)]
	[TestCase ("", false, "IL1234; MT1234", "", ErrorHelper.WarningLevel.Error)]
	[TestCase ("", true, "", "MT1234", ErrorHelper.WarningLevel.Warning)]
	[TestCase ("", true, "MT1234", "MT1234", ErrorHelper.WarningLevel.Warning)]
	[TestCase ("", false, "MT1234", "MT1234", ErrorHelper.WarningLevel.Error)]
	[TestCase ("", false, "", "MT1234", ErrorHelper.WarningLevel.Warning)]
	public void WarningProperties (string noWarn, bool treatWarningsAsErrors, string warningsAsErrors, string warningsNotAsErrors, ErrorHelper.WarningLevel expected)
	{
		var log = new TestLogger { Platform = ApplePlatform.iOS };
		ErrorHelper.ConfigureMSBuildWarnings (log, "MT", noWarn, treatWarningsAsErrors, warningsAsErrors, warningsNotAsErrors);
		var warning = new ProductException (1234, false, "Test warning");
		Assert.That (warning.GetWarningLevel (log), Is.EqualTo (expected), "Warning severity");
		var error = new ProductException (1234, true, "Test error");
		Assert.That (error.GetWarningLevel (log), Is.EqualTo (ErrorHelper.WarningLevel.Error), "Errors cannot be suppressed or demoted");
	}

	[TestCase ("MM1234", ErrorHelper.WarningLevel.Disable)]
	[TestCase ("MT1234", ErrorHelper.WarningLevel.Warning)]
	public void MacWarningPrefix (string noWarn, ErrorHelper.WarningLevel expected)
	{
		var log = new TestLogger { Platform = ApplePlatform.MacOSX };
		ErrorHelper.ConfigureMSBuildWarnings (log, "MM", noWarn, false, "", "");
		Assert.That (ErrorHelper.GetWarningLevel (log, 1234), Is.EqualTo (expected));
	}

	[TestCase (ErrorHelper.WarningLevel.Disable, "", true, ErrorHelper.WarningLevel.Disable)]
	[TestCase (ErrorHelper.WarningLevel.Error, "", false, ErrorHelper.WarningLevel.Error)]
	[TestCase (ErrorHelper.WarningLevel.Error, "MT1234", true, ErrorHelper.WarningLevel.Disable)]
	[TestCase (ErrorHelper.WarningLevel.Warning, "", true, ErrorHelper.WarningLevel.Error)]
	public void BundlerOptions (ErrorHelper.WarningLevel legacyLevel, string noWarn, bool treatWarningsAsErrors, ErrorHelper.WarningLevel expected)
	{
		var log = new TestLogger { Platform = ApplePlatform.iOS };
		ErrorHelper.SetWarningLevel (log, legacyLevel, 1234);
		ErrorHelper.ConfigureMSBuildWarnings (log, "MT", noWarn, treatWarningsAsErrors, "", "");
		Assert.That (ErrorHelper.GetWarningLevel (log, 1234), Is.EqualTo (expected));
	}

	[Test]
	public void LoggerIsolation ()
	{
		var first = new TestLogger { Platform = ApplePlatform.iOS };
		var second = new TestLogger { Platform = ApplePlatform.iOS };
		ErrorHelper.ConfigureMSBuildWarnings (first, "MT", "", true, "", "");
		Assert.That (ErrorHelper.GetWarningLevel (first, 1234), Is.EqualTo (ErrorHelper.WarningLevel.Error));
		Assert.That (ErrorHelper.GetWarningLevel (second, 1234), Is.EqualTo (ErrorHelper.WarningLevel.Warning));
	}

	[TestCase (ApplePlatform.iOS, "MT")]
	[TestCase (ApplePlatform.MacOSX, "MM")]
	public void ConfigurationRoundTrip (ApplePlatform platform, string prefix)
	{
		Configuration.IgnoreIfIgnoredPlatform (platform);
		var logger = new TestLogger { Platform = platform };
		var lines = new List<string> {
			"AreAnyAssembliesTrimmed=false",
			$"Platform={platform.AsString ()}",
			$"TargetFramework={TargetFramework.GetTargetFramework (platform)}",
			$"SdkDevPath={Configuration.XcodeLocation}",
			$"SdkVersion={Configuration.GetSdkVersion (platform)}",
			"Registrar=dynamic",
			$"MSBuildNoWarn=CS1234,{prefix}1234",
			"MSBuildTreatWarningsAsErrors=true",
			$"MSBuildWarningsAsErrors={prefix}1235",
			$"MSBuildWarningsNotAsErrors={prefix}1236",
		};
		var configuration = new Xamarin.Linker.LinkerConfiguration (logger, lines, "warnings.txt") { AssemblyInfos = [] };
		AssertLevels (configuration.Application);
		AssertLevels (logger);

		var saved = new List<string> ();
		configuration.Save (saved);
		foreach (var line in lines.Where (v => v.StartsWith ("MSBuild", StringComparison.Ordinal)))
			Assert.That (saved, Does.Contain (line), "Saved warning property");

		var replay = new Xamarin.Linker.LinkerConfiguration (ConsoleLog.Instance, saved, "warnings.txt") { AssemblyInfos = [] };
		AssertLevels (replay.Application);

		var previousError = Console.Error;
		using var errorOutput = new StringWriter ();
		try {
			Console.SetError (errorOutput);
			replay.Application.LogError (new ProductException (1235, false, "Test warning"));
		} finally {
			Console.SetError (previousError);
		}
		Assert.That (errorOutput.ToString ().Trim (), Is.EqualTo ($"error {prefix}1235: Test warning"), "Promoted console diagnostic");

		void AssertLevels (IToolLog log)
		{
			Assert.That (ErrorHelper.GetWarningLevel (log, 1234), Is.EqualTo (ErrorHelper.WarningLevel.Disable), "Suppressed warning");
			Assert.That (ErrorHelper.GetWarningLevel (log, 1235), Is.EqualTo (ErrorHelper.WarningLevel.Error), "Explicit promotion");
			Assert.That (ErrorHelper.GetWarningLevel (log, 1236), Is.EqualTo (ErrorHelper.WarningLevel.Warning), "Exempt warning");
			Assert.That (ErrorHelper.GetWarningLevel (log, 1237), Is.EqualTo (ErrorHelper.WarningLevel.Error), "Global promotion");
			Assert.That (ErrorHelper.GetWarningLevel (log, 4178), Is.EqualTo (ErrorHelper.WarningLevel.Disable), "Default suppression");
		}
	}
}
