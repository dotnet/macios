// Copyright (c) Microsoft Corporation.
// Licensed under the MIT License.

#nullable enable

using System.Text.Json;
using System.Threading;
using System.Threading.Tasks;

using Xamarin.Utils;

namespace Xamarin.Tests {
	[TestFixture]
	public class DotNetWatchTest : TestBaseClass {
		[Test]
		[TestCase (ApplePlatform.MacOSX)]
		[TestCase (ApplePlatform.MacCatalyst)]
		[TestCase (ApplePlatform.iOS)]
		public void DotNetWatch (ApplePlatform platform)
		{
			Configuration.IgnoreIfIgnoredPlatform (platform);

			var projectPath = GetProjectPath ("HotReloadTestApp", platform: platform);
			Clean (projectPath);

			var projectDirectory = Path.GetDirectoryName (projectPath)!;

			var tmpdir = Cache.CreateTemporaryDirectory ();
			var additionalFile = Path.Combine (tmpdir, "AdditionalFile.cs");

			var firstContent = """
					namespace HotReloadTestApp;
					public partial class Program {
						static partial void ChangeVariable ()
						{
							Variable = "Variable will change...";
						}
					}
					""";

			var secondContent = """
					namespace HotReloadTestApp;
					public partial class Program {
						static partial void ChangeVariable ()
						{
							Variable = "Variable has changed";
							ContinueLooping = false;
						}
					}
					""";

			File.WriteAllText (additionalFile, firstContent);

			// Debug logging is annoying here, because the test runner captures stdout/stderr, so it won't be visible until the test fails,
			// which can take a while because when things go wrong here it will most likely result in timeouts.
			// So instead we log to a separate file, which can be viewed as the test is running.
			var debugLogPath = Path.Combine (tmpdir, "debug.log");
			using var debugLog = new StreamWriter (File.OpenWrite (debugLogPath)) {
				AutoFlush = true,
			};

			var output = new List<string> ();
			var appStarted = new TaskCompletionSource<bool> ();
			var waitingForChanges = new TaskCompletionSource<bool> ();
			var variableChanged = new TaskCompletionSource<bool> ();
			var cts = new CancellationTokenSource ();
			var appOutput = new List<string> ();

			var outputProcessor = new Action<string> (line => {
				if (line.Contains ("Variable has not changed")) {
					if (appStarted.TrySetResult (true))
						debugLog.WriteLine ("Got 'Variable has not changed'");
				}
				if (line.Contains ("Variable has changed")) {
					if (variableChanged.TrySetResult (true))
						debugLog.WriteLine ("Got 'Variable has changed'");
				}
				if (line.Contains ("Waiting for changes")) {
					waitingForChanges.TrySetResult (true);
					debugLog.WriteLine ("Got 'Waiting for changes'");
				}
			});

			// I'm not sure what 'dotnet watch' does with the terminal, but Console.WriteLine from the test app doesn't seem to
			// reliably be captured here, so instead we have the test app write its output to a file, and we poll that file and
			// process new lines as they are written.
			// However, for mobile platforms, test app stdout is captured correctly, so we process both the output from the file
			// and stdout we capture from 'dotnet watch' the same way, to make sure we don't miss any output.
			var logPath = Path.Combine (tmpdir, "output.log");
			var pollThread = new Thread ((v) => {
				for (var i = 0; i < 120; i++) {
					if (File.Exists (logPath)) {
						var lines = File.ReadAllLines (logPath);
						Array.ForEach (lines, outputProcessor);
						lock (appOutput) {
							appOutput.Clear ();
							appOutput.AddRange (lines);
						}
					}
					Thread.Sleep (TimeSpan.FromSeconds (1));
				}
			}) {
				IsBackground = true,
			};
			pollThread.Start ();

			Action<string> outputCallback = (line) => {
				debugLog.WriteLine ($"[dotnet watch] {line}");
				lock (output) {
					output.Add (line);
					outputProcessor (line);
				}
			};

			var args = new List<string> {
				"watch",
				"--non-interactive",
			};

			if (platform == ApplePlatform.iOS || platform == ApplePlatform.TVOS) {
				var runtimeIdentifier = GetDefaultRuntimeIdentifier (platform);
				var device = GetDeviceAsync (projectDirectory, runtimeIdentifier).GetAwaiter ().GetResult ();
				debugLog.WriteLine ($"Using device: {device}");
				args.Add ($"--device={device}");
			}

			var env = new Dictionary<string, string?> {
				{ "HOTRELOAD_TEST_APP_LOGFILE", logPath },
				{ "AdditionalFile", additionalFile },
			};

			var watchTask = Execution.RunWithCallbacksAsync (
				DotNet.Executable,
				args,
				environment: env,
				standardOutput: outputCallback,
				standardError: outputCallback,
				workingDirectory: projectDirectory,
				timeout: TimeSpan.FromMinutes (10),
				cancellationToken: cts.Token,
				log: debugLog
			);

			// Wait for the app to start and show initial output
			debugLog.WriteLine ("Waiting for app start...");
			if (!appStarted.Task.Wait (TimeSpan.FromMinutes (1)))
				Assert.Fail ($"Timed out waiting for the app to start. Output:\n{string.Join ("\n", output)}\nDebug output:\n{string.Join ("\n", File.ReadAllLines (debugLogPath))}");
			debugLog.WriteLine ("App started!");

			debugLog.WriteLine ("Waiting for 'dotnet watch' to be waiting for changes...");
			if (!waitingForChanges.Task.Wait (TimeSpan.FromMinutes (1)))
				Assert.Fail ($"Timed out waiting for the 'dotnet watch' to be waiting for changes. Output:\n{string.Join ("\n", output)}\nDebug output:\n{string.Join ("\n", File.ReadAllLines (debugLogPath))}");
			debugLog.WriteLine ("Waiting for changes!");

			// Write AdditionalFile.cs to trigger a rebuild via dotnet watch
			File.WriteAllText (additionalFile, secondContent);

			// Wait for dotnet watch to pick up the change and the app to show the updated output
			debugLog.WriteLine ("Waiting for app restart...");
			if (!variableChanged.Task.Wait (TimeSpan.FromMinutes (1)))
				Assert.Fail ($"Timed out waiting for the variable to change. Output:\n{string.Join ("\n", output)}\nDebug output:\n{string.Join ("\n", File.ReadAllLines (debugLogPath))}");
			debugLog.WriteLine ("App restarted!");

			// Cancel the watch process
			debugLog.WriteLine ("Terminating the watch process...");
			cts.Cancel ();

			try {
				debugLog.WriteLine ("Waiting for exit...");
				watchTask.Wait (TimeSpan.FromSeconds (30));
				debugLog.WriteLine ("Waited for exit");
			} catch {
				// Expected - the process was cancelled
			}
		}

		// Pick any device for the specified project, and compatible with the specified runtime identifier (if provided).
		// We just need any device to test that dotnet watch can detect it and deploy to it.
		static async Task<string> GetDeviceAsync (string projectDirectory, string? runtimeIdentifier = null)
		{
			var tmpdir = Cache.CreateTemporaryDirectory ();
			var outputFile = Path.Combine (tmpdir, "AvailableDevices.json");
			var args = new List<string> {
				"build",
				"-t:ComputeAvailableDevices",
				"-getItem:Devices",
				$"-getResultOutputFile:{outputFile}",
			};

			if (!string.IsNullOrEmpty (runtimeIdentifier))
				args.Add ($"-p:RuntimeIdentifier={runtimeIdentifier}");

			var rv = await Execution.RunWithCallbacksAsync (
				DotNet.Executable,
				args,
				workingDirectory: projectDirectory,
				timeout: TimeSpan.FromMinutes (1),
				log: Console.Out
			);
			Assert.That (rv.ExitCode, Is.EqualTo (0), "Failed to compute available devices");

			var output = File.ReadAllText (outputFile);
			var doc = JsonDocument.Parse (output);
			// The devices are ordered, so that:
			// * We get the same device each time, to make tests more reliable.
			// * We get the most recent OS version available, to make sure we're testing on a recent OS version.
			// * We get iPhones before iPads (by sorting by device type identifier), just because they take up less of the screen during a test run.
			var devices = doc.RootElement.GetProperty ("Items").GetProperty ("Devices").EnumerateArray ().Select (e => {
				var identity = e.GetProperty ("Identity").GetString ()!;
				var osVersion = Version.Parse (e.GetProperty ("OSVersion").GetString ()!);
				var deviceTypeIdentifier = e.GetProperty ("DeviceTypeIdentifier").GetString ()!;
				return (Identity: identity, OsVersion: osVersion, DeviceTypeIdentifier: deviceTypeIdentifier);
			}).OrderBy (d => d.OsVersion).ThenByDescending (d => d.DeviceTypeIdentifier).ThenBy (d => d.Identity).ToList ();
			if (!devices.Any ())
				Assert.Inconclusive ("No devices found. Output:\n" + output);
			return devices.First ().Identity;
		}
	}
}
