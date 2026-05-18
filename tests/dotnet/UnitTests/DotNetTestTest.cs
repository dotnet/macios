using Xamarin.Tests;
using Xamarin.Utils;

#nullable enable

namespace Xamarin.Tests {
	[TestFixture]
	public class DotNetTestTest : TestBaseClass {
		[Test]
		[TestCase (ApplePlatform.iOS, "iostest")]
		[TestCase (ApplePlatform.TVOS, "tvostest")]
		// macOS and Mac Catalyst don't use mlaunch (they use 'open' via Desktop.targets),
		// so MTP support requires a different approach for these platforms.
		// [TestCase (ApplePlatform.MacCatalyst, "maccatalysttest")]
		// [TestCase (ApplePlatform.MacOSX, "macostest")]
		public void DotNetTest (ApplePlatform platform, string template)
		{
			Configuration.IgnoreIfIgnoredPlatform (platform);

			var tmpDir = Cache.CreateTemporaryDirectory ();
			var outputDir = Path.Combine (tmpDir, template);
			DotNet.AssertNew (outputDir, template);
			var proj = Path.Combine (outputDir, $"{template}.csproj");

			// dotnet test internally calls ComputeRunArguments via MSBuild API without
			// forwarding /p: properties, so we must set them in the project file directly.
			var csproj = File.ReadAllText (proj);
			csproj = csproj.Replace ("</PropertyGroup>", "  <UseFloatingTargetPlatformVersion>true</UseFloatingTargetPlatformVersion>\n  </PropertyGroup>");
			File.WriteAllText (proj, csproj);

			// Replace generated tests with a single passing test
			var testFile = Path.Combine (outputDir, "Test1.cs");
			File.WriteAllText (testFile, $@"namespace {template};

[TestClass]
public sealed class Test1 {{
	[TestMethod]
	public void TestMethod1 ()
	{{
	}}
}}
");

			// Run 'dotnet test' directly using Execution.RunAsync.
			// dotnet test's MTP flow doesn't forward /p: properties to its internal
			// ComputeRunArguments MSBuild API call, so properties must be in the csproj.
			var env = new Dictionary<string, string?> ();
			env ["MSBuildSDKsPath"] = null;
			env ["MSBUILD_EXE_PATH"] = null;
			var binlog = Path.Combine (outputDir, "log-test.binlog");
			var testArgs = new List<string> { "test", proj, $"/bl:{binlog}" };
			var testResult = Execution.RunAsync (DotNet.Executable, testArgs, env, Console.Out, workingDirectory: outputDir, timeout: TimeSpan.FromMinutes (10)).Result;
			Assert.AreEqual (0, testResult.ExitCode, $"'dotnet test' failed with exit code {testResult.ExitCode}.\nBinlog: {binlog}\nOutput:\n{testResult.Output.MergedOutput}");
		}
	}
}
