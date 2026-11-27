// Copyright (c) Microsoft Corporation.
// Licensed under the MIT License.

using System;
using System.Diagnostics;
using System.IO;
using System.Threading.Tasks;
using Microsoft.DotNet.XHarness.Common.Execution;
using Microsoft.DotNet.XHarness.Common.Utilities;
using Microsoft.DotNet.XHarness.iOS.Shared.Logging;
using NUnit.Framework;

namespace Xharness {
	[TestFixture]
	public class ProcessDiagnosticsTests {
		[TestCase ("[04:31:13.1203790] com.xamarin.monotouch-test: 42615", true, 42615)]
		[TestCase ("com.xamarin.monotouch-test: 42615", true, 42615)]
		[TestCase ("[04:31:13.1203790] Application launched. PID = 42615", true, 42615)]
		[TestCase ("Xamarin.Hosting: Launched app with pid 42615", true, 42615)]
		[TestCase ("com.other.app: 42615", false, -1)]
		[TestCase ("xcrun simctl launch com.xamarin.monotouch-test: 42615", false, -1)]
		[TestCase ("com.xamarin.monotouch-test: -1", false, -1)]
		[TestCase ("com.xamarin.monotouch-test: 0", false, 0)]
		[TestCase ("com.xamarin.monotouch-test: not-a-pid", false, 0)]
		public void ParseLaunchPid (string line, bool expected, int pid)
		{
			Assert.That (ProcessDiagnostics.TryParsePid (line, "com.xamarin.monotouch-test", out var actual), Is.EqualTo (expected));
			if (expected)
				Assert.That (actual, Is.EqualTo (pid));
		}

		[TestCase (true, false, true)]
		[TestCase (true, true, true)]
		[TestCase (false, false, true)]
		[TestCase (true, false, false)]
		public async Task CollectHungProcess (bool matches, bool suspended, bool matchesIdentity)
		{
			if (!OperatingSystem.IsMacOS ())
				Assert.Ignore ("Native hang diagnostics use macOS sample.");

			var directory = Path.Combine (Path.GetTempPath (), "xharness-hang-" + Guid.NewGuid ().ToString ("N"));
			Directory.CreateDirectory (directory);
			using var logs = new Logs (directory);
			using var log = logs.Create ("test.log", "Test log");
			using var process = new Process ();
			process.StartInfo.FileName = "/bin/sleep";
			process.StartInfo.ArgumentList.Add ("120");
			process.Start ();
			var started = process.StartTime.ToUniversalTime ();

			try {
				if (suspended) {
					var stopped = await new MacOSProcessManager ().ExecuteCommandAsync ("/bin/kill", new [] { "-STOP", process.Id.ToString () }, log, TimeSpan.FromSeconds (5));
					Assert.That (stopped.Succeeded, Is.True, "Controlled hung process must be suspended");
				}
				await ProcessDiagnostics.CollectAndTerminateAsync (new MacOSProcessManager (), logs, log, process.Id, matches ? "sleep" : "another-test-app", matchesIdentity ? started : started.AddSeconds (-1));
				if (matches && matchesIdentity) {
					Assert.That (await process.WaitForExitAsync ().TimeoutAfter (TimeSpan.FromSeconds (10)), Is.True, "Hung process must terminate");
					var samples = Directory.GetFiles (directory, "native-stacks-*.txt");
					Assert.That (samples, Has.Length.EqualTo (1));
					Assert.That (File.ReadAllText (samples [0]), Does.Contain ("Call graph:"));
					var artifact = Path.Combine (TestContext.CurrentContext.TestDirectory, suspended ? "suspended-native-stacks.txt" : "native-stacks.txt");
					File.Copy (samples [0], artifact, overwrite: true);
					TestContext.AddTestAttachment (artifact, "Native stacks from controlled hung process");
				} else {
					Assert.That (process.HasExited, Is.False, "Unrelated process must not be terminated");
					Assert.That (Directory.GetFiles (directory, "native-stacks-*"), Is.Empty);
				}
			} finally {
				if (!process.HasExited)
					process.Kill ();
				await process.WaitForExitAsync ();
				logs.Dispose ();
				Directory.Delete (directory, recursive: true);
			}
		}
	}
}
