// Copyright (c) Microsoft Corporation.
// Licensed under the MIT License.

using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

if (!OperatingSystem.IsMacOS ()) {
	Console.Error.WriteLine ("This script only supports macOS hosts.");
	return 1;
}

Options options;
try {
	options = Options.Parse (args);
} catch (Exception ex) {
	Console.Error.WriteLine (ex.Message);
	PrintUsage ();
	return 1;
}

if (options.ShowHelp) {
	PrintUsage ();
	return 0;
}

var runner = new AudioUnitExtensionTestRunner (options);
return await runner.RunAsync ();

static void PrintUsage ()
{
	Console.WriteLine ("Usage:");
	Console.WriteLine ("  run-audio-unit-extension-tests --platform <platform> --rid <rid> --config <config> --app <app path> --extension <appex path> --executable <host executable> --log-file <log file> --timeout-seconds <seconds> [--test-filter <test name>] [--lsregister <path>]");
}

sealed class AudioUnitExtensionTestRunner {
	const string BundleIdentifier = "com.xamarin.monotouch-test.AudioUnitExtension";
	const string TestFilterFileName = "monotouch-extension-test-filter.txt";
	const string LogPredicate = "process == \"AppExtension\" OR eventMessage CONTAINS[c] \"monotouch-test-audio-unit-extension\" OR eventMessage CONTAINS[c] \"AppExtensionSmokeTest\"";
	const string ZzzzMarker = "ZZZZ ";

	static readonly Regex CompletionRegex = new ("\\[monotouch-test-audio-unit-extension\\] Finished monotouch-test audio unit extension test run\\.|\\[monotouch-test-audio-unit-extension\\] Extension test run failed:", RegexOptions.Compiled);
	static readonly Regex ExecutedTestRegex = new ("\\[PASS\\]|\\[FAIL\\]|Tests run: [1-9]", RegexOptions.Compiled);
	static readonly Regex FilteredSuccessRegex = new ("\\[monotouch-test-audio-unit-extension\\] Finished monotouch-test audio unit extension test run\\. Passed: [0-9]+ Failed: 0", RegexOptions.Compiled);
	static readonly Regex ZzzzLineRegex = new ("ZZZZ (.*)$", RegexOptions.Compiled);

	readonly Options options;
	readonly StringBuilder transcript = new ();
	readonly object logLock = new ();

	string ExtensionFilterFile => Path.Combine (options.ExtensionPath, "Contents", "Resources", TestFilterFileName);
	string HostFilterFile => Path.Combine (options.AppPath, "Contents", "Resources", TestFilterFileName);
	string TemporaryFilterFile => Path.Combine (Path.GetTempPath (), "monotouch-test", "extensions", "audio-unit", "test-filter.txt");

	public AudioUnitExtensionTestRunner (Options options)
	{
		this.options = options;
	}

	public async Task<int> RunAsync ()
	{
		Directory.CreateDirectory (Path.GetDirectoryName (options.LogFilePath)!);
		File.WriteAllText (options.LogFilePath, "");

		Log ($"Platform: {options.Platform}");
		Log ($"RID: {options.Rid}");
		Log ($"Config: {options.Config}");
		Log ($"App: {options.AppPath}");
		Log ($"Extension: {options.ExtensionPath}");
		Log ($"Log file: {options.LogFilePath}");
		if (!string.IsNullOrEmpty (options.TestFilter))
			Log ($"Test filter: {options.TestFilter}");
		Log ("");

		var exitCode = 0;
		var logStart = DateTime.Now;
		var logEnd = logStart;
		Process? hostProcess = null;
		Process? testProcess = null;

		try {
			await ConfigureFilterAsync ();

			await RunToolAsync (options.LsRegisterPath, "-f", options.AppPath);
			Log ("");
			await RunToolAsync ("pluginkit", "-a", options.ExtensionPath);
			Log ("");

			var existingTestPids = await GetExtensionProcessIdsAsync ();
			hostProcess = StartHostProcess ();
			var deadline = DateTime.UtcNow + options.Timeout;
			testProcess = await WaitForExtensionProcessAsync (existingTestPids, hostProcess, deadline);

			if (testProcess is null) {
				if (hostProcess.HasExited) {
					Log ("The container host exited before the extension completed its test run.");
				} else {
					Log ("Timed out waiting for the extension test process to start.");
				}
				exitCode = 1;
			} else {
				Log ($"Detected extension test process PID {testProcess.Id}.");
				if (!await WaitForExtensionProcessExitAsync (testProcess, hostProcess, logStart, deadline))
					exitCode = 1;
			}
		} catch (Exception ex) {
			Log (ex.ToString ());
			exitCode = 1;
		} finally {
			await CleanupAsync (hostProcess, testProcess);
			logEnd = DateTime.Now;

			Log ("");
			Log ($"Executing: log show --style compact --predicate {LogPredicate} --start {FormatTimestamp (logStart)} --end {FormatTimestamp (logEnd)}");
			var finalLog = await GetSystemLogAsync (logStart, logEnd);
			if (!string.IsNullOrEmpty (finalLog))
				WriteRaw (finalLog);
		}

		exitCode = Math.Max (exitCode, ValidateRun ());
		return exitCode;
	}

	async Task ConfigureFilterAsync ()
	{
		Directory.CreateDirectory (Path.GetDirectoryName (ExtensionFilterFile)!);
		Directory.CreateDirectory (Path.GetDirectoryName (HostFilterFile)!);
		Directory.CreateDirectory (Path.GetDirectoryName (TemporaryFilterFile)!);
		await RunToolAsync ("defaults", "write", BundleIdentifier, "log.file", "-string", options.LogFilePath);

		if (string.IsNullOrEmpty (options.TestFilter)) {
			await RunBestEffortAsync ("defaults", "delete", BundleIdentifier, "test.name");
			DeleteFileIfExists (ExtensionFilterFile);
			DeleteFileIfExists (HostFilterFile);
			DeleteFileIfExists (TemporaryFilterFile);
			return;
		}

		await RunToolAsync ("defaults", "write", BundleIdentifier, "test.name", "-string", options.TestFilter);
		File.WriteAllText (ExtensionFilterFile, options.TestFilter + Environment.NewLine);
		File.WriteAllText (HostFilterFile, options.TestFilter + Environment.NewLine);
		File.WriteAllText (TemporaryFilterFile, options.TestFilter + Environment.NewLine);
	}

	async Task CleanupAsync (Process? hostProcess, Process? testProcess)
	{
		await RunBestEffortAsync ("defaults", "delete", BundleIdentifier, "log.file");
		await RunBestEffortAsync ("defaults", "delete", BundleIdentifier, "test.name");
		DeleteFileIfExists (ExtensionFilterFile);
		DeleteFileIfExists (HostFilterFile);
		DeleteFileIfExists (TemporaryFilterFile);

		TryKillProcess (testProcess);

		if (hostProcess is null)
			return;

		TryKillProcess (hostProcess);
		try {
			await hostProcess.WaitForExitAsync ();
		} catch {
		}
		hostProcess.Dispose ();
	}

	Process StartHostProcess ()
	{
		var process = new Process ();
		process.StartInfo.FileName = options.ExecutablePath;
		process.StartInfo.UseShellExecute = false;
		process.StartInfo.RedirectStandardOutput = true;
		process.StartInfo.RedirectStandardError = true;
		process.StartInfo.Environment ["RUN_EXTENSION_TESTS"] = "1";

		var commandText = new StringBuilder ();
		if (!string.IsNullOrEmpty (options.TestFilter)) {
			process.StartInfo.Environment ["NUNIT_TEST_NAME"] = options.TestFilter;
			commandText.Append ($"NUNIT_TEST_NAME={options.TestFilter} ");
		}
		commandText.Append ("RUN_EXTENSION_TESTS=1 ");
		commandText.Append (options.ExecutablePath);
		Log ($"Executing: {commandText}");

		process.OutputDataReceived += (_, e) => {
			if (!string.IsNullOrEmpty (e.Data))
				Log (e.Data);
		};
		process.ErrorDataReceived += (_, e) => {
			if (!string.IsNullOrEmpty (e.Data))
				Log (e.Data);
		};

		if (!process.Start ())
			throw new InvalidOperationException ($"Failed to start '{options.ExecutablePath}'.");

		process.BeginOutputReadLine ();
		process.BeginErrorReadLine ();
		return process;
	}

	async Task<HashSet<int>> GetExtensionProcessIdsAsync ()
	{
		var result = await ExecuteAsync ("ps", "-axo", "pid=,command=");
		if (result.ExitCode != 0)
			throw new InvalidOperationException ($"ps exited with code {result.ExitCode}:{Environment.NewLine}{result.CombinedOutput}");

		var pids = new HashSet<int> ();
		var extensionExecutable = GetExtensionExecutablePath ();
		var extensionExecutableName = Path.GetFileName (extensionExecutable);

		using var reader = new StringReader (result.StandardOutput);
		string? line;
		while ((line = reader.ReadLine ()) is not null) {
			var trimmed = line.Trim ();
			if (string.IsNullOrEmpty (trimmed))
				continue;

			var firstSpace = trimmed.IndexOf (' ');
			if (firstSpace <= 0)
				continue;

			if (!int.TryParse (trimmed.Substring (0, firstSpace), out var pid))
				continue;

			var command = trimmed.Substring (firstSpace).Trim ();
			if (command.Contains (extensionExecutable, StringComparison.Ordinal) ||
				command.EndsWith ("/" + extensionExecutableName, StringComparison.Ordinal) ||
				command.Equals (extensionExecutableName, StringComparison.Ordinal))
				pids.Add (pid);
		}

		return pids;
	}

	async Task<Process?> WaitForExtensionProcessAsync (HashSet<int> existingPids, Process hostProcess, DateTime deadline)
	{
		while (DateTime.UtcNow < deadline) {
			var currentPids = await GetExtensionProcessIdsAsync ();
			foreach (var pid in currentPids) {
				if (existingPids.Contains (pid))
					continue;

				try {
					return Process.GetProcessById (pid);
				} catch (ArgumentException) {
					existingPids.Add (pid);
				}
			}

			if (hostProcess.HasExited)
				return null;

			await Task.Delay (TimeSpan.FromSeconds (1));
		}

		return null;
	}

	async Task<bool> WaitForExtensionProcessExitAsync (Process testProcess, Process hostProcess, DateTime logStart, DateTime deadline)
	{
		// Use 'log stream' to get real-time test output instead of polling with 'log show'.
		Process? logStream = null;
		var completionDetected = false;
		var requestedHostShutdown = false;

		try {
			logStream = new Process ();
			logStream.StartInfo.FileName = "log";
			logStream.StartInfo.ArgumentList.Add ("stream");
			logStream.StartInfo.ArgumentList.Add ("--style");
			logStream.StartInfo.ArgumentList.Add ("compact");
			logStream.StartInfo.ArgumentList.Add ("--predicate");
			logStream.StartInfo.ArgumentList.Add (LogPredicate);
			logStream.StartInfo.UseShellExecute = false;
			logStream.StartInfo.RedirectStandardOutput = true;
			logStream.StartInfo.RedirectStandardError = true;

			if (!logStream.Start ())
				throw new InvalidOperationException ("Failed to start 'log stream'.");

			logStream.BeginErrorReadLine ();

			// Read log stream output on a background thread.
			var streamReader = Task.Run (() => ReadLogStream (logStream, ref completionDetected));

			while (true) {
				if (HasExited (testProcess, out var exitCode)) {
					if (exitCode.HasValue)
						Log ($"Extension test process PID {testProcess.Id} exited with code {exitCode.Value}.");
					else
						Log ($"Extension test process PID {testProcess.Id} exited.");
					return true;
				}

				if (!requestedHostShutdown && completionDetected) {
					Log ("Detected the extension completion marker. Stopping the container host so the test process can exit.");
					TryKillProcess (hostProcess);
					requestedHostShutdown = true;
				}

				if (DateTime.UtcNow >= deadline) {
					Log ($"Timed out waiting for the extension test process PID {testProcess.Id} to finish after {options.Timeout.TotalMinutes:0} minutes.");
					TryKillProcess (testProcess);
					TryKillProcess (hostProcess);
					return false;
				}

				await Task.Delay (TimeSpan.FromMilliseconds (500));
			}
		} finally {
			TryKillProcess (logStream);
			logStream?.Dispose ();
		}
	}

	void ReadLogStream (Process logStream, ref bool completionDetected)
	{
		try {
			string? line;
			while ((line = logStream.StandardOutput.ReadLine ()) is not null) {
				// Extract test progress from ZZZZ-prefixed lines and print to stdout.
				var match = ZzzzLineRegex.Match (line);
				if (match.Success) {
					var testOutput = match.Groups [1].Value;
					Console.WriteLine (testOutput);
					lock (logLock) {
						transcript.AppendLine (testOutput);
						File.AppendAllText (options.LogFilePath, line + Environment.NewLine);
					}
				} else {
					// Write full line to log file for non-ZZZZ output.
					lock (logLock) {
						File.AppendAllText (options.LogFilePath, line + Environment.NewLine);
					}
				}

				if (CompletionRegex.IsMatch (line))
					completionDetected = true;
			}
		} catch (Exception ex) {
			Log ($"Error reading log stream: {ex.Message}");
		}
	}

	async Task<string> GetSystemLogAsync (DateTime start, DateTime end)
	{
		var result = await ExecuteAsync ("log", "show", "--style", "compact", "--predicate", LogPredicate, "--start", FormatTimestamp (start), "--end", FormatTimestamp (end));
		if (result.ExitCode != 0)
			throw new InvalidOperationException ($"log show exited with code {result.ExitCode}:{Environment.NewLine}{result.CombinedOutput}");

		return result.CombinedOutput;
	}

	async Task RunToolAsync (string fileName, params string [] arguments)
	{
		Log ($"Executing: {FormatCommand (fileName, arguments)}");
		var result = await ExecuteAsync (fileName, arguments);
		WriteRaw (result.CombinedOutput);
		if (result.ExitCode != 0)
			throw new InvalidOperationException ($"'{fileName}' exited with code {result.ExitCode}.");
	}

	async Task RunBestEffortAsync (string fileName, params string [] arguments)
	{
		var result = await ExecuteAsync (fileName, arguments);
		if (!string.IsNullOrEmpty (result.CombinedOutput))
			WriteRaw (result.CombinedOutput);
	}

	int ValidateRun ()
	{
		var logText = File.Exists (options.LogFilePath) ? File.ReadAllText (options.LogFilePath) : transcript.ToString ();

		if (!logText.Contains ("[monotouch-test-audio-unit-extension] Starting monotouch-test audio unit extension test run")) {
			Log ("Did not find the monotouch-test extension start marker.");
			return 1;
		}

		if (!ExecutedTestRegex.IsMatch (logText)) {
			Log ("The extension run never reached actual test execution.");
			return 1;
		}

		if (!string.IsNullOrEmpty (options.TestFilter)) {
			if (!FilteredSuccessRegex.IsMatch (logText)) {
				Log ("Did not find the monotouch-test extension completion marker.");
				return 1;
			}
		} else {
			if (!logText.Contains ("(all monotouch-test tests)")) {
				Log ("The extension run did not start the full monotouch-test suite.");
				return 1;
			}

			if (!logText.Contains ("[monotouch-test-audio-unit-extension] Finished monotouch-test audio unit extension test run.")) {
				Log ("Did not find the monotouch-test extension completion marker.");
				return 1;
			}
		}

		Log ("✅ Extension test run succeeded");
		if (!string.IsNullOrEmpty (options.TestFilter))
			WriteMatchingLine (logText, "Finished monotouch-test audio unit extension test run.");
		else
			WriteMatchingLine (logText, "Starting monotouch-test audio unit extension test run");

		return 0;
	}

	void WriteMatchingLine (string logText, string marker)
	{
		using var reader = new StringReader (logText);
		string? line;
		while ((line = reader.ReadLine ()) is not null) {
			if (line.Contains (marker)) {
				Console.WriteLine (line);
				return;
			}
		}
	}

	void Log (string line)
	{
		lock (logLock) {
			transcript.AppendLine (line);
			File.AppendAllText (options.LogFilePath, line + Environment.NewLine);
		}
		Console.WriteLine (line);
	}

	void WriteRaw (string text)
	{
		if (string.IsNullOrEmpty (text))
			return;

		lock (logLock) {
			transcript.Append (text);
			if (!text.EndsWith (Environment.NewLine, StringComparison.Ordinal))
				transcript.AppendLine ();
			File.AppendAllText (options.LogFilePath, text.EndsWith (Environment.NewLine, StringComparison.Ordinal) ? text : text + Environment.NewLine);
		}
	}

	string GetExtensionExecutablePath ()
	{
		var executableDirectory = Path.Combine (options.ExtensionPath, "Contents", "MacOS");
		if (!Directory.Exists (executableDirectory))
			return Path.Combine (executableDirectory, "AppExtension");

		var files = Directory.GetFiles (executableDirectory);
		if (files.Length == 1)
			return files [0];

		return Path.Combine (executableDirectory, "AppExtension");
	}

	static bool HasExited (Process process, out int? exitCode)
	{
		try {
			if (process.HasExited) {
				exitCode = process.ExitCode;
				return true;
			}
		} catch (ArgumentException) {
			exitCode = null;
			return true;
		} catch (InvalidOperationException) {
			exitCode = null;
			return true;
		}

		exitCode = null;
		return false;
	}

	static void TryKillProcess (Process? process)
	{
		if (process is null)
			return;

		try {
			if (!process.HasExited)
				process.Kill ();
		} catch {
		}
	}

	static void DeleteFileIfExists (string path)
	{
		if (File.Exists (path))
			File.Delete (path);
	}

	static string FormatTimestamp (DateTime timestamp)
		=> timestamp.ToString ("yyyy-MM-dd HH:mm:ss");

	static string FormatCommand (string fileName, IReadOnlyList<string> arguments)
	{
		var builder = new StringBuilder (fileName);
		foreach (var argument in arguments) {
			builder.Append (' ');
			builder.Append (Quote (argument));
		}
		return builder.ToString ();
	}

	static string Quote (string value)
	{
		if (string.IsNullOrEmpty (value))
			return "\"\"";

		if (value.IndexOfAny ([' ', '\t', '"']) < 0)
			return value;

		return "\"" + value.Replace ("\\", "\\\\").Replace ("\"", "\\\"") + "\"";
	}

	static async Task<CommandResult> ExecuteAsync (string fileName, params string [] arguments)
	{
		using var process = new Process ();
		process.StartInfo.FileName = fileName;
		process.StartInfo.UseShellExecute = false;
		process.StartInfo.RedirectStandardOutput = true;
		process.StartInfo.RedirectStandardError = true;
		foreach (var argument in arguments)
			process.StartInfo.ArgumentList.Add (argument);

		if (!process.Start ())
			throw new InvalidOperationException ($"Failed to start '{fileName}'.");

		var stdoutTask = process.StandardOutput.ReadToEndAsync ();
		var stderrTask = process.StandardError.ReadToEndAsync ();
		await process.WaitForExitAsync ();
		return new CommandResult (process.ExitCode, await stdoutTask, await stderrTask);
	}

	readonly record struct CommandResult (int ExitCode, string StandardOutput, string StandardError)
	{
		public string CombinedOutput => string.Concat (StandardOutput, StandardError);
	}
}

sealed class Options {
	public bool ShowHelp { get; private init; }
	public string Platform { get; private init; } = "";
	public string Rid { get; private init; } = "";
	public string Config { get; private init; } = "";
	public string AppPath { get; private init; } = "";
	public string ExtensionPath { get; private init; } = "";
	public string ExecutablePath { get; private init; } = "";
	public string LogFilePath { get; private init; } = "";
	public string LsRegisterPath { get; private init; } = "/System/Library/Frameworks/CoreServices.framework/Frameworks/LaunchServices.framework/Support/lsregister";
	public string? TestFilter { get; private init; }
	public TimeSpan Timeout { get; private init; }

	public static Options Parse (string [] args)
	{
		var parsed = new Dictionary<string, string> (StringComparer.Ordinal);

		for (var i = 0; i < args.Length; i++) {
			var argument = args [i];
			if (argument is "--help" or "-h")
				return new Options { ShowHelp = true };
			if (!argument.StartsWith ("--", StringComparison.Ordinal))
				throw new ArgumentException ($"Unknown argument: {argument}");
			if (i + 1 >= args.Length)
				throw new ArgumentException ($"Missing value for argument: {argument}");
			parsed [argument] = args [++i];
		}

		var timeoutSeconds = int.Parse (GetRequired (parsed, "--timeout-seconds"));
		if (timeoutSeconds <= 0)
			throw new ArgumentOutOfRangeException (nameof (args), "The timeout must be a positive number of seconds.");

		return new Options {
			Platform = GetRequired (parsed, "--platform"),
			Rid = GetRequired (parsed, "--rid"),
			Config = GetRequired (parsed, "--config"),
			AppPath = Path.GetFullPath (GetRequired (parsed, "--app")),
			ExtensionPath = Path.GetFullPath (GetRequired (parsed, "--extension")),
			ExecutablePath = Path.GetFullPath (GetRequired (parsed, "--executable")),
			LogFilePath = Path.GetFullPath (GetRequired (parsed, "--log-file")),
			LsRegisterPath = GetOptional (parsed, "--lsregister") ?? "/System/Library/Frameworks/CoreServices.framework/Frameworks/LaunchServices.framework/Support/lsregister",
			TestFilter = GetOptional (parsed, "--test-filter"),
			Timeout = TimeSpan.FromSeconds (timeoutSeconds),
		};
	}

	static string GetRequired (Dictionary<string, string> parsed, string key)
	{
		if (!parsed.TryGetValue (key, out var value) || string.IsNullOrEmpty (value))
			throw new ArgumentException ($"Missing required argument: {key}");
		return value;
	}

	static string? GetOptional (Dictionary<string, string> parsed, string key)
	{
		parsed.TryGetValue (key, out var value);
		return string.IsNullOrEmpty (value) ? null : value;
	}
}
