// Copyright (c) Microsoft Corporation.
// Licensed under the MIT License.

using System;
using System.Diagnostics;
using System.IO;
using System.Threading.Tasks;
using Microsoft.DotNet.XHarness.iOS.Shared;
using Microsoft.DotNet.XHarness.iOS.Shared.Execution;
using Microsoft.DotNet.XHarness.iOS.Shared.Logging;

namespace Xharness.Jenkins.TestTasks {
	class AppExtensionTestTask : AppleTestTask {
		readonly IMlaunchProcessManager processManager;
		readonly string testVariation;

		public AppExtensionTestTask (Jenkins jenkins, IMlaunchProcessManager processManager, TestPlatform platform, string testVariation)
			: base (jenkins)
		{
			this.processManager = processManager;
			this.testVariation = testVariation;
			Platform = platform;
			SupportsParallelExecution = false;
		}

		protected override async Task ExecuteAsync ()
		{
			using var resource = await NotifyAndAcquireDesktopResourceAsync ();

			var platformName = Platform.ToPlatformName ();
			if (platformName is null)
				throw new InvalidOperationException ($"App extension tests are not supported for {Platform}.");
			var projectDirectory = Path.Combine (
				HarnessConfiguration.RootDirectory,
				"monotouch-test",
				"dotnet",
				"extensions",
				"audio-unit",
				platformName);
			var extensionLogPath = Path.Combine (Logs.Directory, $"app-extension-{Platform}-{Timestamp}.log");
			var resultsPath = Path.Combine (Logs.Directory, $"vsts-app-extension-{Platform}-{Timestamp}.xml");
			var executionLog = Logs.Create ($"execute-app-extension-{Platform}-{Timestamp}.log", LogType.ExecutionLog.ToString ());

			using var process = new Process ();
			process.StartInfo.FileName = "make";
			process.StartInfo.ArgumentList.Add ("-C");
			process.StartInfo.ArgumentList.Add (projectDirectory);
			process.StartInfo.ArgumentList.Add ("run");
			process.StartInfo.ArgumentList.Add ($"TEST_VARIATION={testVariation}");
			process.StartInfo.ArgumentList.Add ("TEST_FILTER=");
			process.StartInfo.ArgumentList.Add ($"LOGFILENAME={extensionLogPath}");
			process.StartInfo.ArgumentList.Add ($"RESULTSFILENAME={resultsPath}");
			process.StartInfo.ArgumentList.Add ("RUN_TIMEOUT_SECONDS=1200");
			SetEnvironmentVariables (process);

			Jenkins.MainLog.WriteLine ($"Executing {TestName} ({Variation})");
			if (Harness.DryRun) {
				ExecutionResult = TestExecutingResult.Succeeded;
				return;
			}

			ExecutionResult = TestExecutingResult.Running;
			var result = await processManager.RunAsync (process, executionLog, TimeSpan.FromMinutes (30));
			if (File.Exists (extensionLogPath))
				Logs.AddFile (extensionLogPath, LogType.ExecutionLog.ToString ());
			if (File.Exists (resultsPath))
				Logs.AddFile (resultsPath, LogType.NUnitResult.ToString ());

			if (result.TimedOut) {
				FailureMessage = "App extension test run timed out after 30 minutes.";
				ExecutionResult = TestExecutingResult.TimedOut;
			} else if (result.Succeeded) {
				ExecutionResult = TestExecutingResult.Succeeded;
			} else {
				FailureMessage = $"App extension test run failed with exit code {result.ExitCode}.";
				ExecutionResult = TestExecutingResult.Failed;
			}
			Jenkins.MainLog.WriteLine ($"Executed {TestName} ({Variation})");
		}
	}
}
