// Copyright (c) Microsoft Corporation.
// Licensed under the MIT License.

using System;
using Microsoft.DotNet.XHarness.Common;
using Microsoft.DotNet.XHarness.Common.Execution;
using Microsoft.DotNet.XHarness.Common.Logging;
using Microsoft.DotNet.XHarness.iOS.Shared;
using Microsoft.DotNet.XHarness.iOS.Shared.Execution;
using Microsoft.DotNet.XHarness.iOS.Shared.Listeners;
using Microsoft.DotNet.XHarness.iOS.Shared.Logging;
using Microsoft.DotNet.XHarness.iOS.Shared.XmlResults;

using ExceptionLogger = System.Action<int, string>;

namespace Xharness {
	public class TestReporterFactory : ITestReporterFactory {
		readonly IMlaunchProcessManager processManager;

		public TestReporterFactory (IMlaunchProcessManager processManager)
		{
			this.processManager = processManager ?? throw new ArgumentNullException (nameof (processManager));
		}

		public ITestReporter Create (
			IFileBackedLog mainLog,
			IReadableLog runLog,
			ILogs logs,
			ICrashSnapshotReporter crashSnapshotReporter,
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
			return new TestReporter (
				processManager,
				mainLog,
				runLog,
				logs,
				crashSnapshotReporter,
				simpleListener,
				parser,
				appInformation,
				runMode,
				xmlJargon,
				device,
				timeout,
				additionalLogsDirectory,
				exceptionLogger,
				generateHtml);
		}
	}
}
