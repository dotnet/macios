using System.Text.Json;

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

			// Boot a simulator so that ComputeRunArguments can find a device
			var deviceUdid = GetOrCreateDeviceUdid (platform);
			BootSimulator (deviceUdid);

			// Run 'dotnet test' directly using Execution.RunAsync.
			// dotnet test's MTP flow doesn't forward /p: properties to its internal
			// ComputeRunArguments MSBuild API call, so properties must be in the csproj.
			var env = new Dictionary<string, string?> ();
			env ["MSBuildSDKsPath"] = null;
			env ["MSBUILD_EXE_PATH"] = null;
			var binlog = Path.Combine (outputDir, "log-test.binlog");
			var testArgs = new List<string> { "test", proj, $"/bl:{binlog}" };
			var testResult = Execution.RunAsync (DotNet.Executable, testArgs, env, Console.Out, workingDirectory: outputDir, timeout: TimeSpan.FromMinutes (10)).Result;
			Assert.That (testResult.ExitCode, Is.EqualTo (0), $"'dotnet test' failed with exit code {testResult.ExitCode}.\nBinlog: {binlog}\nOutput:\n{testResult.Output.MergedOutput}");
		}

		static string GetOrCreateDeviceUdid (ApplePlatform platform)
		{
			// Use xcrun simctl directly to find available simulator devices.
			var rv = Execution.RunAsync ("xcrun", new List<string> { "simctl", "list", "devices", "available", "--json" }, timeout: TimeSpan.FromMinutes (1)).Result;
			Assert.That (rv.ExitCode, Is.EqualTo (0), $"Failed to list simulators. Output:\n{rv.Output.MergedOutput}");

			var runtimePrefix = platform switch {
				ApplePlatform.iOS => "com.apple.CoreSimulator.SimRuntime.iOS-",
				ApplePlatform.TVOS => "com.apple.CoreSimulator.SimRuntime.tvOS-",
				_ => throw new ArgumentException ($"Unsupported platform: {platform}"),
			};

			var doc = JsonDocument.Parse (rv.Output.MergedOutput);
			var devicesObj = doc.RootElement.GetProperty ("devices");
			string? bestRuntime = null;
			var allDevices = new List<(string Udid, string Runtime)> ();
			foreach (var runtimeProp in devicesObj.EnumerateObject ()) {
				if (!runtimeProp.Name.StartsWith (runtimePrefix, StringComparison.Ordinal))
					continue;
				if (bestRuntime is null || string.Compare (runtimeProp.Name, bestRuntime, StringComparison.Ordinal) > 0)
					bestRuntime = runtimeProp.Name;
				foreach (var device in runtimeProp.Value.EnumerateArray ()) {
					var udid = device.GetProperty ("udid").GetString ()!;
					allDevices.Add ((udid, runtimeProp.Name));
				}
			}

			if (allDevices.Count > 0)
				return allDevices.OrderByDescending (d => d.Runtime).First ().Udid;

			// No devices exist — create one. CI agents may have runtimes but no devices.
			Assert.That (bestRuntime, Is.Not.Null, $"No {platform} simulator runtimes found. Output:\n{rv.Output.MergedOutput}");

			var defaultDeviceType = platform switch {
				ApplePlatform.iOS => "com.apple.CoreSimulator.SimDeviceType.iPhone-16",
				ApplePlatform.TVOS => "com.apple.CoreSimulator.SimDeviceType.Apple-TV-4K-3rd-generation-4K",
				_ => throw new ArgumentException ($"Unsupported platform: {platform}"),
			};

			var createResult = Execution.RunAsync ("xcrun", new List<string> { "simctl", "create", "test-device", defaultDeviceType, bestRuntime! }, timeout: TimeSpan.FromMinutes (2)).Result;
			Assert.That (createResult.ExitCode, Is.EqualTo (0), $"Failed to create simulator. Output:\n{createResult.Output.MergedOutput}");
			return createResult.Output.MergedOutput.Trim ();
		}

		static void BootSimulator (string udid)
		{
			var rv = Execution.RunAsync ("xcrun", new List<string> { "simctl", "boot", udid }, timeout: TimeSpan.FromMinutes (1)).Result;
			// Exit code 149 means "already booted", which is fine
			if (rv.ExitCode != 0 && rv.ExitCode != 149)
				Assert.Fail ($"Failed to boot simulator {udid}. Exit code: {rv.ExitCode}\nOutput:\n{rv.Output.MergedOutput}");
		}
	}
}
