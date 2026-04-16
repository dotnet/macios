// Copyright (c) Microsoft Corporation.
// Licensed under the MIT License.

using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Runtime.Serialization.Json;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Xml;
using Microsoft.DotNet.XHarness.Common;
using Microsoft.DotNet.XHarness.Common.Execution;
using Microsoft.DotNet.XHarness.Common.Logging;
using Microsoft.DotNet.XHarness.Common.Utilities;
using Microsoft.DotNet.XHarness.iOS.Shared;
using Microsoft.DotNet.XHarness.iOS.Shared.Execution;
using Microsoft.DotNet.XHarness.iOS.Shared.Listeners;
using Microsoft.DotNet.XHarness.iOS.Shared.Logging;
using Microsoft.DotNet.XHarness.iOS.Shared.XmlResults;

using ExceptionLogger = System.Action<int, string>;

namespace Xharness {
	public class TestReporter : ITestReporter {
		const string timeoutMessage = "Test run timed out after {0} minute(s).";
		const string completionMessage = "Test run completed";
		const string failureMessage = "Test run failed";

		readonly ISimpleListener listener;
		readonly IFileBackedLog mainLog;
		readonly ILogs crashLogs;
		readonly IReadableLog runLog;
		readonly ILogs logs;
		readonly ICrashSnapshotReporter crashReporter;
		readonly IResultParser resultParser;
		readonly AppBundleInformation appInfo;
		readonly RunMode runMode;
		readonly XmlResultJargon xmlJargon;
		readonly IMlaunchProcessManager processManager;
		readonly string deviceName;
		readonly TimeSpan timeout;
		readonly Stopwatch timeoutWatch;
		readonly string additionalLogsDirectory;
		readonly CancellationTokenSource cancellationTokenSource = new ();
		readonly ExceptionLogger exceptionLogger;
		readonly bool generateHtml;

		bool waitedForExit = true;
		bool launchFailure;
		bool isSimulatorTest;
		bool timedout;

		public ILog CallbackLog { get; private set; }

		public bool? Success { get; private set; }

		public CancellationToken CancellationToken => cancellationTokenSource.Token;

		public bool ResultsUseXml => xmlJargon != XmlResultJargon.Missing;

		bool TestExecutionStarted => listener.ConnectedTask.IsCompletedSuccessfully && listener.ConnectedTask.Result;

		public TestReporter (
			IMlaunchProcessManager processManager,
			IFileBackedLog mainLog,
			IReadableLog runLog,
			ILogs logs,
			ICrashSnapshotReporter crashReporter,
			ISimpleListener simpleListener,
			IResultParser parser,
			AppBundleInformation appInformation,
			RunMode runMode,
			XmlResultJargon xmlJargon,
			string device,
			TimeSpan timeout,
			string additionalLogsDirectory = null,
			ExceptionLogger exceptionLogger = null,
			bool generateHtml = false)
		{
			this.processManager = processManager ?? throw new ArgumentNullException (nameof (processManager));
			deviceName = device;
			listener = simpleListener ?? throw new ArgumentNullException (nameof (simpleListener));
			this.mainLog = mainLog ?? throw new ArgumentNullException (nameof (mainLog));
			this.runLog = runLog ?? throw new ArgumentNullException (nameof (runLog));
			this.logs = logs ?? throw new ArgumentNullException (nameof (logs));
			this.crashReporter = crashReporter ?? throw new ArgumentNullException (nameof (crashReporter));
			crashLogs = new Logs (logs.Directory);
			resultParser = parser ?? throw new ArgumentNullException (nameof (parser));
			appInfo = appInformation ?? throw new ArgumentNullException (nameof (appInformation));
			this.runMode = runMode;
			this.xmlJargon = xmlJargon;
			this.timeout = timeout;
			this.additionalLogsDirectory = additionalLogsDirectory;
			this.exceptionLogger = exceptionLogger;
			timeoutWatch = Stopwatch.StartNew ();
			this.generateHtml = generateHtml;

			CallbackLog = new CallbackLog ((line) => {
				waitedForExit &= line?.Contains ("MT1111: ") != true;
				if (line?.Contains ("error MT1007") == true)
					launchFailure = true;
			});
		}

		async Task<int> GetPidFromRunLog ()
		{
			int pid = -1;

			using var reader = runLog.GetReader ();
			if (reader.Peek () == -1) {
				if (!listener.ConnectedTask.IsCompletedSuccessfully || !listener.ConnectedTask.Result)
					launchFailure = true;
			} else {
				string line;
				while ((line = await reader.ReadLineAsync ()) is not null) {
					if (line.StartsWith ("Application launched. PID = ", StringComparison.Ordinal)) {
						var pidstr = line.Substring ("Application launched. PID = ".Length);
						if (!int.TryParse (pidstr, out pid))
							mainLog.WriteLine ("Could not parse pid: {0}", pidstr);
					} else if (line.Contains ("Xamarin.Hosting: Launched ") && line.Contains (" with pid ")) {
						var pidstr = line.Substring (line.LastIndexOf (' '));
						if (!int.TryParse (pidstr, out pid))
							mainLog.WriteLine ("Could not parse pid: {0}", pidstr);
					} else if (line.Contains ("error MT1008")) {
						launchFailure = true;
					}
				}
			}

			return pid;
		}

		async Task<int> GetPidFromMainLog ()
		{
			int pid = -1;
			using var logReader = mainLog.GetReader ();
			string line;
			while ((line = await logReader.ReadLineAsync ()) is not null) {
				const string str = "was launched with pid '";
				var idx = line.IndexOf (str, StringComparison.Ordinal);
				if (idx > 0) {
					idx += str.Length;
					var nextIdx = line.IndexOf ('\'', idx);
					if (nextIdx > idx)
						int.TryParse (line.Substring (idx, nextIdx - idx), out pid);
				}
				if (pid != -1)
					return pid;
			}
			return pid;
		}

		void GetCrashReason (int pid, IReadableLog crashLog, out string crashReason)
		{
			crashReason = null;
			using var crashReader = crashLog.GetReader ();
			var text = crashReader.ReadToEnd ();

			var reader = JsonReaderWriterFactory.CreateJsonReader (Encoding.UTF8.GetBytes (text), new XmlDictionaryReaderQuotas ());
			var doc = new XmlDocument ();
			doc.Load (reader);
			foreach (XmlNode node in doc.SelectNodes ($"/root/processes/item[pid = '" + pid + "']")) {
				Console.WriteLine (node?.InnerXml);
				Console.WriteLine (node?.SelectSingleNode ("reason")?.InnerText);
				crashReason = node?.SelectSingleNode ("reason")?.InnerText;
			}
		}

		async Task<bool> TcpConnectionFailed ()
		{
			using var reader = new StreamReader (mainLog.FullPath);
			string line;
			while ((line = await reader.ReadLineAsync ()) is not null) {
				if (line.Contains ("Couldn't establish a TCP connection with any of the hostnames"))
					return true;
			}
			return false;
		}

		Task KillAppProcess (int pid, CancellationTokenSource cancellationSource)
		{
			var launchTimedout = cancellationSource.IsCancellationRequested;
			var timeoutType = launchTimedout ? "Launch" : "Completion";
			mainLog.WriteLine ($"{timeoutType} timed out after {timeoutWatch.Elapsed.TotalSeconds} seconds");
			return processManager.KillTreeAsync (pid, mainLog, true);
		}

		async Task CollectResult (ProcessExecutionResult runResult)
		{
			if (!waitedForExit && !runResult.TimedOut) {
				mainLog.WriteLine ("Waiting for listener to complete, since mlaunch won't tell.");
				if (!await listener.CompletionTask.TimeoutAfter (timeout - timeoutWatch.Elapsed))
					runResult.TimedOut = true;
			}

			if (runResult.TimedOut) {
				timedout = true;
				Success = false;
				mainLog.WriteLine (timeoutMessage, timeout.TotalMinutes);
			} else if (runResult.Succeeded) {
				mainLog.WriteLine (completionMessage);
				Success = true;
			} else {
				mainLog.WriteLine (failureMessage);
				Success = false;
			}
		}

		public void LaunchCallback (Task<bool> launchResult)
		{
			if (launchResult.IsFaulted) {
				mainLog.WriteLine ($"Test execution failed: {launchResult.Exception}");
				return;
			}

			if (launchResult.IsCanceled) {
				mainLog.WriteLine ("Test execution was cancelled");
				return;
			}

			if (launchResult.Result) {
				mainLog.WriteLine ("Test execution started");
				return;
			}

			cancellationTokenSource.Cancel ();
			timedout = true;

			if (TestExecutionStarted) {
				mainLog.WriteLine ($"Test execution timed out after {timeoutWatch.Elapsed.TotalMinutes:0.##} minutes");
				return;
			}

			mainLog.WriteLine ($"Test failed to start in {timeoutWatch.Elapsed.TotalMinutes:0.##} minutes");
		}

		public async Task CollectSimulatorResult (ProcessExecutionResult runResult)
		{
			isSimulatorTest = true;
			await CollectResult (runResult);

			if (Success is not null && !Success.Value) {
				var pid = await GetPidFromRunLog ();
				if (pid > 0) {
					await KillAppProcess (pid, cancellationTokenSource);
				} else {
					mainLog.WriteLine ("Could not find pid in mtouch output.");
				}
			}
		}

		public async Task CollectDeviceResult (ProcessExecutionResult runResult)
		{
			isSimulatorTest = false;
			await CollectResult (runResult);
		}

		async Task<(string ResultLine, bool Failed)> GetResultLine (string logPath)
		{
			string resultLine = null;
			bool failed = false;
			using var reader = new StreamReader (logPath);
			string line;
			while ((line = await reader.ReadLineAsync ()) is not null) {
				if (line.Contains ("Tests run:")) {
					Console.WriteLine (line);
					resultLine = line;
					break;
				} else if (line.Contains ("[FAIL]")) {
					Console.WriteLine (line);
					failed = true;
				}
			}
			return (ResultLine: resultLine, Failed: failed);
		}

		async Task<(string resultLine, bool failed, bool crashed)> ParseResultFile (AppBundleInformation appInfo, string testLogPath, bool timedOut)
		{
			(string resultLine, bool failed, bool crashed) parseResult = (null, false, false);
			if (!File.Exists (testLogPath)) {
				parseResult.crashed = true;
				return parseResult;
			}

			var path = Path.ChangeExtension (testLogPath, "xml");
			if (path == testLogPath)
				path = Path.Combine (Path.GetDirectoryName (path), Path.GetFileNameWithoutExtension (path) + "-clean.xml");

			resultParser.CleanXml (testLogPath, path);

			if (ResultsUseXml && resultParser.IsValidXml (path, out var xmlType)) {
				try {
					var newFilename = resultParser.GetXmlFilePath (path, xmlType);
					var testRunName = $"{appInfo.AppName} {appInfo.Variation}";
					if (xmlType == XmlResultJargon.NUnitV3) {
						var logFiles = new List<string> ();
						logFiles.AddRange (Directory.GetFiles (crashLogs.Directory));
						if (additionalLogsDirectory is not null)
							logFiles.AddRange (Directory.GetFiles (additionalLogsDirectory));
						newFilename = XmlResultParser.GetVSTSFilename (newFilename);
						resultParser.UpdateMissingData (path, newFilename, testRunName, logFiles);
					} else {
						File.Move (path, newFilename);
					}
					path = newFilename;

					if (generateHtml) {
						var humanReadableLog = logs.CreateFile (Path.GetFileNameWithoutExtension (testLogPath) + ".log", LogType.NUnitResult);
						(parseResult.resultLine, parseResult.failed) = resultParser.ParseResults (path, xmlType, humanReadableLog);
					} else {
						(parseResult.resultLine, parseResult.failed) = resultParser.ParseResults (path, xmlType, (StreamWriter) null);
					}

					logs.AddFile (path, LogType.XmlLog.ToString ());
					return parseResult;
				} catch (Exception e) {
					mainLog.WriteLine ("Could not parse xml result file: {0}", e);
					mainLog.WriteLine ("File data is:");
					mainLog.WriteLine (new string ('#', 10));
					using (var stream = new StreamReader (path)) {
						string line;
						while ((line = await stream.ReadLineAsync ()) is not null)
							mainLog.WriteLine (line);
					}
					mainLog.WriteLine (new string ('#', 10));
					mainLog.WriteLine ("End of xml results.");
					if (timedOut) {
						WrenchLog.WriteLine ($"AddSummary: <b><i>{runMode} timed out</i></b><br/>");
						return parseResult;
					} else {
						WrenchLog.WriteLine ($"AddSummary: <b><i>{runMode} crashed</i></b><br/>");
						mainLog.WriteLine ("Test run crashed");
						parseResult.crashed = true;
						return parseResult;
					}
				}
			}

			File.Delete (path);
			(parseResult.resultLine, parseResult.failed) = await GetResultLine (testLogPath);
			return parseResult;
		}

		async Task<(bool Succeeded, bool Crashed, string ResultLine)> TestsSucceeded (AppBundleInformation appInfo, string testLogPath, bool timedOut)
		{
			var (resultLine, failed, crashed) = await ParseResultFile (appInfo, testLogPath, timedOut);
			if (resultLine is not null) {
				var testsRun = resultLine.Replace ("Tests run: ", "");
				if (failed) {
					WrenchLog.WriteLine ("AddSummary: <b>{0} failed: {1}</b><br/>", runMode, testsRun);
					mainLog.WriteLine ("Test run failed");
					return (false, crashed, resultLine);
				} else {
					WrenchLog.WriteLine ("AddSummary: {0} succeeded: {1}<br/>", runMode, testsRun);
					mainLog.WriteLine ("Test run succeeded");
					return (true, crashed, resultLine);
				}
			} else if (timedOut) {
				WrenchLog.WriteLine ("AddSummary: <b><i>{0} timed out</i></b><br/>", runMode);
				mainLog.WriteLine ("Test run timed out");
				return (false, false, "Test run timed out");
			} else {
				WrenchLog.WriteLine ("AddSummary: <b><i>{0} crashed</i></b><br/>", runMode);
				mainLog.WriteLine ("Test run crashed");
				return (false, true, "Test run crashed");
			}
		}

		async Task GenerateXmlFailures (string failure, bool crashed, string crashReason)
		{
			if (!ResultsUseXml)
				return;

			if (!string.IsNullOrEmpty (crashReason)) {
				resultParser.GenerateFailure (
					logs,
					"crash",
					appInfo.AppName,
					appInfo.Variation,
					$"App Crash {appInfo.AppName} {appInfo.Variation}",
					$"App crashed: {failure}",
					mainLog.FullPath,
					xmlJargon);

				return;
			}

			if (launchFailure) {
				resultParser.GenerateFailure (
					logs,
					"launch",
					appInfo.AppName,
					appInfo.Variation,
					$"App Launch {appInfo.AppName} {appInfo.Variation} on {deviceName}",
					$"{failure} on {deviceName}",
					mainLog.FullPath,
					xmlJargon);

				return;
			}

			if (!isSimulatorTest && crashed && string.IsNullOrEmpty (crashReason)) {
				if (await TcpConnectionFailed ()) {
					resultParser.GenerateFailure (
						logs,
						"tcp-connection",
						appInfo.AppName,
						appInfo.Variation,
						$"TcpConnection on {deviceName}",
						$"Device {deviceName} could not reach the host over tcp.",
						mainLog.FullPath,
						xmlJargon);
				}
			} else if (timedout) {
				resultParser.GenerateFailure (
					logs,
					"timeout",
					appInfo.AppName,
					appInfo.Variation,
					$"App Timeout {appInfo.AppName} {appInfo.Variation} on bot {deviceName}",
					$"{appInfo.AppName} {appInfo.Variation} Test run timed out after {timeout.TotalMinutes} minute(s) on bot {deviceName}.",
					mainLog.FullPath,
					xmlJargon);
			}
		}

		public async Task<(TestExecutingResult ExecutingResult, string ResultMessage)> ParseResult ()
		{
			(TestExecutingResult ExecutingResult, string ResultMessage) result = (TestExecutingResult.Finished, null);
			var crashed = false;
			if (File.Exists (listener.TestLog.FullPath)) {
				WrenchLog.WriteLine ("AddFile: {0}", listener.TestLog.FullPath);
				(Success, crashed, result.ResultMessage) = await TestsSucceeded (appInfo, listener.TestLog.FullPath, timedout);
			} else if (timedout) {
				WrenchLog.WriteLine ("AddSummary: <b><i>{0} never launched</i></b><br/>", runMode);
				mainLog.WriteLine ("Test run never launched");
				result.ResultMessage = "Test runner never started";
				Success = false;
			} else if (launchFailure) {
				WrenchLog.WriteLine ("AddSummary: <b><i>{0} failed to launch</i></b><br/>", runMode);
				mainLog.WriteLine ("Test run failed to launch");
				result.ResultMessage = "Test runner failed to launch";
				Success = false;
			} else {
				WrenchLog.WriteLine ("AddSummary: <b><i>{0} crashed at startup (no log)</i></b><br/>", runMode);
				mainLog.WriteLine ("Test run started but crashed and no test results were reported");
				result.ResultMessage = "No test log file was produced";
				crashed = true;
				Success = false;
			}

			if (!Success.HasValue)
				Success = false;

			var crashLogWaitTime = 0;
			if (!Success.Value)
				crashLogWaitTime = 5;

			if (crashed)
				crashLogWaitTime = 30;

			await crashReporter.EndCaptureAsync (TimeSpan.FromSeconds (crashLogWaitTime));

			if (timedout) {
				if (TestExecutionStarted) {
					result.ExecutingResult = TestExecutingResult.TimedOut;
				} else {
					result.ExecutingResult = TestExecutingResult.LaunchTimedOut;
				}
			} else if (launchFailure) {
				result.ExecutingResult = TestExecutingResult.LaunchFailure;
			} else if (crashed) {
				result.ExecutingResult = TestExecutingResult.Crashed;
			} else if (Success.Value) {
				result.ExecutingResult = TestExecutingResult.Succeeded;
			} else {
				result.ExecutingResult = TestExecutingResult.Failed;
			}

			if (!Success.Value) {
				int pid = -1;
				string crashReason = null;
				foreach (var crashLog in crashLogs) {
					try {
						logs.Add (crashLog);

						if (pid == -1)
							pid = await GetPidFromMainLog ();

						GetCrashReason (pid, crashLog, out crashReason);
						if (crashReason is not null)
							break;
					} catch (Exception e) {
						var message = string.Format ("Failed to process crash report '{1}': {0}", e.Message, crashLog.Description);
						mainLog.WriteLine (message);
						exceptionLogger?.Invoke (2, message);
					}
				}

				if (!string.IsNullOrEmpty (crashReason)) {
					if (crashReason == "per-process-limit") {
						result.ResultMessage = "Killed due to using too much memory (per-process-limit).";
					} else {
						result.ResultMessage = $"Killed by the OS ({crashReason})";
					}
				} else if (launchFailure) {
					result.ResultMessage = "Launch failure";
				}

				await GenerateXmlFailures (result.ResultMessage, crashed, crashReason);
			}

			return result;
		}

		public void Dispose ()
		{
			crashLogs.Dispose ();
			GC.SuppressFinalize (this);
		}
	}
}
