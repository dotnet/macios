﻿using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Threading.Tasks;
using Microsoft.DotNet.XHarness.iOS.Shared.Logging;

namespace Microsoft.DotNet.XHarness.iOS.Shared.Tasks {
	public interface ITestTask {
		bool HasCustomTestName { get; }
		bool BuildSucceeded { get; }
		bool Succeeded { get; }
		bool Failed { get; }
		bool Ignored { get; set; }
		bool TimedOut { get; }
		bool Finished { get; }
		bool BuildOnly { get; set; }

		string KnownFailure { get; set; }
		string ProjectConfiguration { get; set; }
		string ProjectPlatform { get; set; }
		string ProjectFile { get; }
		string Mode { get; set; }
		string Variation { get; set; }
		string TestName { get; }
		string FailureMessage { get; set; }
		string LogDirectory { get; }


		string GuessFailureReason (ILog log);
		TimeSpan Duration { get; }
		TestPlatform Platform { get; set; }
		Task InitialTask { get; set; }
		TestExecutingResult ExecutionResult { get; set; }
		IEnumerable<ILog> AggregatedLogs { get; }
		ILogs Logs { get; }
		Stopwatch DurationStopWatch { get; }


		Task RunAsync ();
		Task VerifyRunAsync ();
		void Reset ();
		Task<IAcquiredResource> NotifyBlockingWaitAsync (Task<IAcquiredResource> task);
	}
}
