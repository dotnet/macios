// Copyright (c) Microsoft Corporation.
// Licensed under the MIT License.

using System;
using System.ComponentModel;
using System.Diagnostics;
using System.IO;
using System.Threading.Tasks;
using Microsoft.DotNet.XHarness.Common.Execution;
using Microsoft.DotNet.XHarness.Common.Logging;
using Microsoft.DotNet.XHarness.Common.Utilities;
using Microsoft.DotNet.XHarness.iOS.Shared.Logging;

namespace Xharness {
	static class ProcessDiagnostics {
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
				if (path is null || Path.GetFileName (path) != executable || startTime < started) {
					log.WriteLine ($"Skipping diagnostics for PID {pid}: it does not match the test app '{executable}' launched after {started:O}.");
					return;
				}

				try {
					using var diagnosticLog = logs.Create ($"hang-diagnostics-{pid}.log", "Hang diagnostic collection");
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
						diagnosticLog.WriteLine ("dotnet-stack is not on PATH. Managed stacks require a CoreCLR diagnostics-enabled app and dotnet-stack; NativeAOT does not support this protocol.");
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
				if (!await current.WaitForExitAsync ().TimeoutAfter (TimeSpan.FromSeconds (5))) {
					if (!IsSameProcess (pid, path, startTime)) {
						log.WriteLine ($"Skipping SIGKILL for PID {pid}: process identity changed.");
						return;
					}
					log.WriteLine ($"Test app PID {pid} did not exit after SIGABRT; sending SIGKILL.");
					current.Kill ();
				}
			} catch (ArgumentException ex) {
				log.WriteLine ($"Cannot collect hang diagnostics for PID {pid}: {ex.Message}");
			} catch (InvalidOperationException ex) {
				log.WriteLine ($"Cannot collect hang diagnostics for PID {pid}: {ex.Message}");
			} catch (Win32Exception ex) {
				log.WriteLine ($"Cannot collect hang diagnostics for PID {pid}: {ex.Message}");
			}
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
