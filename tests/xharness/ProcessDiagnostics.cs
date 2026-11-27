// Copyright (c) Microsoft Corporation.
// Licensed under the MIT License.

using System;
using System.ComponentModel;
using System.Diagnostics;
using System.IO;
using System.Text.Json;
using System.Threading.Tasks;
using Microsoft.DotNet.XHarness.Common.Execution;
using Microsoft.DotNet.XHarness.Common.Logging;
using Microsoft.DotNet.XHarness.Common.Utilities;
using Microsoft.DotNet.XHarness.iOS.Shared.Execution;
using Microsoft.DotNet.XHarness.iOS.Shared.Logging;

namespace Xharness {
	static class ProcessDiagnostics {
		public static async Task CollectSimulatorLifecycleAsync (IMlaunchProcessManager processManager, ILogs logs, ILog mainLog, string udid, string bundleIdentifier, TimeSpan timeout)
		{
			try {
				using var log = logs.Create ("simulator-lifecycle.log", "Simulator SpringBoard and test app lifecycle");
				var identifier = bundleIdentifier.Replace ("\\", "\\\\").Replace ("\"", "\\\"");
				var predicate = $"process == \"SpringBoard\" OR (process == \"runningboardd\" AND eventMessage CONTAINS \"{identifier}\")";
				await RunToolAsync (processManager, log, Path.Combine (processManager.XcodeRoot, "Contents", "Developer", "usr", "bin", "simctl"),
					"spawn", udid, "log", "show", "--style", "compact", "--last", $"{Math.Ceiling (timeout.TotalSeconds)}s", "--predicate", predicate);
			} catch (IOException ex) {
				mainLog.WriteLine ($"Could not write simulator lifecycle log: {ex.Message}");
			} catch (UnauthorizedAccessException ex) {
				mainLog.WriteLine ($"Could not write simulator lifecycle log: {ex.Message}");
			}
		}

		public static bool TryParsePid (string line, string bundleIdentifier, out int pid)
		{
			pid = -1;
			if (line.StartsWith ("[", StringComparison.Ordinal)) {
				var end = line.IndexOf ("] ", StringComparison.Ordinal);
				if (end > 0 && TimeSpan.TryParse (line.Substring (1, end - 1), out _))
					line = line.Substring (end + 2);
			}

			const string launched = "Application launched. PID = ";
			if (line.StartsWith (launched, StringComparison.Ordinal))
				line = line.Substring (launched.Length);
			else if (line.StartsWith ("Xamarin.Hosting: Launched ", StringComparison.Ordinal) && line.Contains (" with pid ", StringComparison.Ordinal))
				line = line.Substring (line.LastIndexOf (" with pid ", StringComparison.Ordinal) + " with pid ".Length);
			else if (line.StartsWith (bundleIdentifier + ": ", StringComparison.Ordinal))
				line = line.Substring (bundleIdentifier.Length + 2);
			else
				return false;

			return int.TryParse (line, out pid) && pid > 0;
		}

		public static async Task CollectAndTerminateAsync (IProcessManager processManager, ILogs logs, ILog log, int pid, string? executable, DateTime started)
		{
			if (pid <= 0 || string.IsNullOrEmpty (executable)) {
				log.WriteLine ("Cannot collect hang diagnostics without a test app PID and executable.");
				return;
			}

			try {
				using var process = Process.GetProcessById (pid);
				var path = process.MainModule?.FileName;
				var startTime = process.StartTime.ToUniversalTime ();
				if (path is null || Path.GetFileName (path) != executable || startTime != started) {
					log.WriteLine ($"Skipping diagnostics for PID {pid}: it does not match the test app '{executable}' launched at {started:O}.");
					return;
				}

				try {
					using var diagnosticLog = logs.Create ($"hang-diagnostics-{pid}.log", "Hang diagnostic collection");
					await RunToolAsync (processManager, diagnosticLog, "/bin/ps", "-p", pid.ToString (), "-o", "pid,ppid,state,etime,command");
					var samplePath = logs.CreateFile ($"native-stacks-{pid}.txt", "Native stacks of hung test app");
					await RunToolAsync (processManager, diagnosticLog, "/usr/bin/sample", pid.ToString (), "3", "10", "-file", samplePath);

					if (!IsSameProcess (pid, path, startTime)) {
						log.WriteLine ($"Skipping further diagnostics for PID {pid}: process identity changed.");
						return;
					}

					var stackTool = FindTool ("dotnet-stack");
					if (stackTool is not null) {
						using var stackLog = logs.Create ($"managed-stacks-{pid}.txt", "Managed stacks of hung test app");
						await RunToolAsync (processManager, stackLog, stackTool, "report", "--process-id", pid.ToString ());
					} else {
						diagnosticLog.WriteLine ("dotnet-stack is not on PATH, so managed stacks were not collected. CoreCLR apps need that tool and diagnostics enabled. NativeAOT cannot use it.");
					}
				} catch (IOException ex) {
					log.WriteLine ($"Could not write hang diagnostics for PID {pid}: {ex.Message}");
				} catch (UnauthorizedAccessException ex) {
					log.WriteLine ($"Could not write hang diagnostics for PID {pid}: {ex.Message}");
				}

				// Recheck identity after collection: the app may have exited and its PID may have been reused.
				using var current = Process.GetProcessById (pid);
				if (current.StartTime.ToUniversalTime () != startTime || current.MainModule?.FileName != path) {
					log.WriteLine ($"Skipping termination of PID {pid}: process identity changed during diagnostic collection.");
					return;
				}

				log.WriteLine ($"Sending SIGABRT to test app PID {pid} to request a crash report.");
				await RunToolAsync (processManager, log, "/bin/kill", "-ABRT", pid.ToString ());
				if (!current.HasExited && IsSameProcess (pid, path, startTime)) {
					// A stopped app cannot handle SIGABRT until it is resumed. Its original state was captured above.
					await RunToolAsync (processManager, log, "/bin/kill", "-CONT", pid.ToString ());
				}
				if (!await current.WaitForExitAsync ().TimeoutAfter (TimeSpan.FromSeconds (5))) {
					if (!IsSameProcess (pid, path, startTime)) {
						log.WriteLine ($"Skipping SIGKILL for PID {pid}: process identity changed.");
						return;
					}
					log.WriteLine ($"Test app PID {pid} did not exit after SIGABRT; sending SIGKILL.");
					current.Kill ();
				} else {
					await WaitForCrashReportAsync (pid, executable, startTime, log);
				}
			} catch (ArgumentException ex) {
				log.WriteLine ($"Test app PID {pid} is no longer available for diagnostics or termination: {ex.Message}");
			} catch (InvalidOperationException ex) {
				log.WriteLine ($"Could not inspect or terminate test app PID {pid}: {ex.Message}");
			} catch (Win32Exception ex) {
				log.WriteLine ($"Cannot collect hang diagnostics for PID {pid}: {ex.Message}");
			}
		}

		static async Task WaitForCrashReportAsync (int pid, string executable, DateTime started, ILog log)
		{
			var directory = Path.Combine (Environment.GetFolderPath (Environment.SpecialFolder.UserProfile), "Library", "Logs", "DiagnosticReports");
			if (!Directory.Exists (directory)) {
				log.WriteLine ($"Crash report directory does not exist: {directory}");
				return;
			}

			var watch = Stopwatch.StartNew ();
			while (watch.Elapsed < TimeSpan.FromSeconds (30)) {
				try {
					foreach (var path in Directory.EnumerateFiles (directory)) {
						if (!Path.GetFileName (path).StartsWith (executable + "-", StringComparison.Ordinal) || File.GetLastWriteTimeUtc (path) < started)
							continue;
						if (path.EndsWith (".ips", StringComparison.Ordinal)) {
							using var reader = File.OpenText (path);
							var header = reader.ReadLine ();
							var body = reader.ReadToEnd ();
							using var document = JsonDocument.Parse (body.Length > 0 ? body : header ?? "");
							if (!document.RootElement.TryGetProperty ("pid", out var reportPid) || reportPid.ValueKind != JsonValueKind.Number || !reportPid.TryGetInt32 (out var value) || value != pid)
								continue;
						} else if (path.EndsWith (".crash", StringComparison.Ordinal)) {
							using var reader = File.OpenText (path);
							var processLine = reader.ReadLine ();
							if (processLine is null || !processLine.StartsWith ("Process:", StringComparison.Ordinal) || !processLine.TrimEnd ().EndsWith ($"[{pid}]", StringComparison.Ordinal))
								continue;
						} else {
							continue;
						}
						log.WriteLine ($"Test app crash report is ready for collection: {path}");
						return;
					}
				} catch (IOException ex) {
					log.WriteLine ($"Could not read pending test app crash report: {ex.Message}");
				} catch (UnauthorizedAccessException ex) {
					log.WriteLine ($"Could not read pending test app crash report: {ex.Message}");
					return;
				} catch (JsonException ex) {
					log.WriteLine ($"Test app crash report is not yet readable: {ex.Message}");
				}
				await Task.Delay (TimeSpan.FromSeconds (1));
			}
			log.WriteLine ($"No crash report for test app PID {pid} was found within 30 seconds.");
		}

		static bool IsSameProcess (int pid, string path, DateTime startTime)
		{
			using var process = Process.GetProcessById (pid);
			return process.StartTime.ToUniversalTime () == startTime && process.MainModule?.FileName == path;
		}

		static string? FindTool (string tool)
		{
			foreach (var directory in (Environment.GetEnvironmentVariable ("PATH") ?? "").Split (Path.PathSeparator)) {
				if (string.IsNullOrEmpty (directory))
					continue;
				var path = Path.Combine (directory, tool);
				if (File.Exists (path))
					return path;
			}
			return null;
		}

		static async Task RunToolAsync (IProcessManager processManager, ILog log, string tool, params string [] arguments)
		{
			try {
				using var process = new Process ();
				process.StartInfo.FileName = tool;
				foreach (var argument in arguments)
					process.StartInfo.ArgumentList.Add (argument);
				var result = await processManager.RunAsync (process, log, TimeSpan.FromSeconds (15), diagnostics: false);
				if (!result.Succeeded)
					log.WriteLine ($"Diagnostic command '{tool}' failed (exit code {result.ExitCode}, timed out: {result.TimedOut}).");
			} catch (Win32Exception ex) {
				log.WriteLine ($"Could not start diagnostic command '{tool}': {ex.Message}");
			} catch (InvalidOperationException ex) {
				log.WriteLine ($"Could not run diagnostic command '{tool}': {ex.Message}");
			}
		}
	}
}
