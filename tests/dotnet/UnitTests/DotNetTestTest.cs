using System.Linq;

using Xamarin.MacDev;
using Xamarin.MacDev.Models;
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

			// Boot a simulator so that ComputeRunArguments can find a device
			var log = ConsoleLogger.Instance;
			var simService = new SimulatorService (log);
			var runtimeService = new RuntimeService (log);
			var device = GetOrCreateDevice (platform, simService, runtimeService);

			if (!device.IsBooted)
				Assert.That (simService.Boot (device.Udid), Is.True, $"Failed to boot simulator {device.Udid}.");

			try {
				// dotnet test internally calls ComputeRunArguments via MSBuild API without
				// forwarding /p: properties, so we must set them in the project file directly.
				var csproj = File.ReadAllText (proj);
				csproj = csproj.Replace (
					"</PropertyGroup>",
					$"  <UseFloatingTargetPlatformVersion>true</UseFloatingTargetPlatformVersion>\n    <Device>{device.Udid}</Device>\n  </PropertyGroup>");
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
				var testArgs = new List<string> {
					"test",
					proj,
					$"--device:{device.Udid}",
					$"/bl:{binlog}"
				};
				var testResult = Execution.RunAsync (DotNet.Executable, testArgs, env, Console.Out, workingDirectory: outputDir, timeout: TimeSpan.FromMinutes (10)).Result;
				Assert.That (testResult.ExitCode, Is.EqualTo (0), $"'dotnet test' failed with exit code {testResult.ExitCode}.\nBinlog: {binlog}\nOutput:\n{testResult.Output.MergedOutput}");
			} finally {
				simService.Shutdown (device.Udid);
			}
		}

		static SimulatorDeviceInfo GetOrCreateDevice (ApplePlatform platform, SimulatorService simService, RuntimeService runtimeService)
		{
			var runtimePlatform = platform switch {
				ApplePlatform.iOS => "iOS",
				ApplePlatform.TVOS => "tvOS",
				_ => throw new ArgumentException ($"Unsupported platform: {platform}"),
			};

			// Find an existing available device for this platform
			var devices = simService.List (availableOnly: true);
			var platformDevices = devices
				.Where (d => string.Equals (d.Platform, runtimePlatform, StringComparison.OrdinalIgnoreCase))
				.OrderByDescending (d => d.OSVersion)
				.ToList ();

			if (platformDevices.Count > 0)
				return platformDevices [0];

			// No devices exist — find the best runtime and create one
			var runtimes = runtimeService.ListByPlatform (runtimePlatform, availableOnly: true);
			Assert.That (runtimes, Is.Not.Empty, $"No available {runtimePlatform} simulator runtimes found.");

			var bestRuntime = runtimes.OrderByDescending (r => r.Version).First ();

			var defaultDeviceType = platform switch {
				ApplePlatform.iOS => "com.apple.CoreSimulator.SimDeviceType.iPhone-16",
				ApplePlatform.TVOS => "com.apple.CoreSimulator.SimDeviceType.Apple-TV-4K-3rd-generation-4K",
				_ => throw new ArgumentException ($"Unsupported platform: {platform}"),
			};

			var udid = simService.Create ("test-device", defaultDeviceType, bestRuntime.Identifier);
			Assert.That (udid, Is.Not.Null, $"Failed to create {runtimePlatform} simulator device.");

			// Re-fetch the device info so we have the full object
			var created = simService.List (availableOnly: true).FirstOrDefault (d => d.Udid == udid);
			Assert.That (created, Is.Not.Null, $"Created simulator {udid} not found in device list.");
			return created!;
		}
	}
}
