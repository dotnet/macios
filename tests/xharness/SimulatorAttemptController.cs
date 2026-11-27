// Copyright (c) Microsoft Corporation.
// Licensed under the MIT License.

using System;
using System.IO;
using System.Threading.Tasks;
using Microsoft.DotNet.XHarness.Common.Logging;

namespace Xharness {
	public enum SimulatorAttemptDecision {
		// Report the result of the attempt as-is.
		Report,
		// The attempt ended after a launch-time SpringBoard crash; launch the app again.
		Retry,
		// The launch-time SpringBoard crash happened in every attempt.
		RepeatedSpringBoardCrash,
	}

	// Decides whether a simulator test run should be launched again after a launch-time SpringBoard crash.
	// There's at most one relaunch, and only if the first attempt hasn't reported any test results.
	public static class SimulatorAttemptController {
		public const int MaxAttempts = 2;

		// Checks whether any of the logs (the test log, or the app's stdout/stderr) shows a failed test or the final summary.
		// An attempt with such results is never ended early. This doesn't protect results the app hasn't written yet:
		// with wrapped XML output (used on CI) the test log only gets results at the end of the run, so only the
		// app's console output (written to stderr through NSLog) shows failures while the tests are running.
		public static bool HasTestResults (ILog log, out string? failure, params string? [] logPaths)
		{
			failure = null;
			var hasResults = false;
			foreach (var path in logPaths) {
				if (path is null || !File.Exists (path))
					continue;

				try {
					using var stream = new FileStream (path, FileMode.Open, FileAccess.Read, FileShare.ReadWrite | FileShare.Delete);
					using var reader = new StreamReader (stream);
					string? line;
					while ((line = reader.ReadLine ()) is not null) {
						if (line.Contains ("[FAIL]", StringComparison.Ordinal)) {
							failure = line;
							return true;
						}
						if (line.Contains ("Tests run:", StringComparison.Ordinal) || line.Contains ("<test-run ", StringComparison.Ordinal))
							hasResults = true;
					}
				} catch (Exception e) when (e is IOException || e is UnauthorizedAccessException) {
					log.WriteLine ($"Could not check test results in '{path}': {e.Message}");
					hasResults = true;
				}
			}
			return hasResults;
		}

		public static async Task<string?> GetProgressAfterDiagnosticsAsync (Func<string?> getProgress, Func<Task> collectDiagnostics)
		{
			var progress = getProgress ();
			if (progress is not null)
				return progress;
			await collectDiagnostics ();
			return getProgress ();
		}

		public static SimulatorAttemptDecision Decide (int attempt, bool aborted)
		{
			if (!aborted)
				return SimulatorAttemptDecision.Report;
			if (attempt < MaxAttempts)
				return SimulatorAttemptDecision.Retry;
			return SimulatorAttemptDecision.RepeatedSpringBoardCrash;
		}

		// Runs attempts until one isn't aborted, or until the maximum number of attempts is reached.
		// 'runAttempt' returns the attempt's state, and whether the attempt was aborted because of a launch-time SpringBoard crash.
		// 'discardAttempt' is called for every attempt that's followed by another attempt.
		public static async Task<(TState Final, SimulatorAttemptDecision Decision, int Attempts)> RunAsync<TState> (
			Func<int, Task<(TState State, bool Aborted)>> runAttempt,
			Func<int, TState, Task> discardAttempt,
			ILog log)
		{
			for (var attempt = 1; ; attempt++) {
				var (state, aborted) = await runAttempt (attempt);
				var decision = Decide (attempt, aborted);
				switch (decision) {
				case SimulatorAttemptDecision.Retry:
					log.WriteLine ($"Attempt {attempt} ended after a launch-time SpringBoard crash. Launching the app again (attempt {attempt + 1} of {MaxAttempts}).");
					await discardAttempt (attempt, state);
					break;
				case SimulatorAttemptDecision.RepeatedSpringBoardCrash:
					log.WriteLine ($"Attempt {attempt} also ended after a launch-time SpringBoard crash. Not launching the app again.");
					return (state, decision, attempt);
				default:
					return (state, decision, attempt);
				}
			}
		}
	}
}
