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

		// Only end an attempt if the test run hasn't finished, and hasn't reported any results:
		// that way a relaunch can never hide a test result.
		public static bool ShouldAbort (bool listenerCompleted, bool hasTestResults)
		{
			return !listenerCompleted && !hasTestResults;
		}

		public static bool HasTestResults (string testLogPath)
		{
			if (!File.Exists (testLogPath))
				return false;

			using var stream = new FileStream (testLogPath, FileMode.Open, FileAccess.Read, FileShare.ReadWrite | FileShare.Delete);
			using var reader = new StreamReader (stream);
			string? line;
			while ((line = reader.ReadLine ()) is not null) {
				if (line.Contains ("[FAIL]", StringComparison.Ordinal) || line.Contains ("Tests run:", StringComparison.Ordinal))
					return true;
			}
			return false;
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
