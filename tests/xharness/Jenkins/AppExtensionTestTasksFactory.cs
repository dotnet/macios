// Copyright (c) Microsoft Corporation.
// Licensed under the MIT License.

using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.DotNet.XHarness.iOS.Shared.Execution;
using Xharness.Jenkins.TestTasks;

namespace Xharness.Jenkins {
	class AppExtensionTestTasksFactory : TaskFactory {
		public AppExtensionTestTasksFactory (Jenkins jenkins, IMlaunchProcessManager processManager, TestVariationsFactory testVariationsFactory)
			: base (jenkins, processManager, testVariationsFactory)
		{
		}

		public override Task<IEnumerable<AppleTestTask>> CreateTasksAsync ()
		{
			var tasks = new List<AppleTestTask> ();
			var selected = Jenkins.TestSelection.IsEnabled (TestLabel.Monotouch);

			if (Jenkins.Harness.INCLUDE_MAC && Jenkins.TestSelection.IsEnabled (PlatformLabel.Mac)) {
				tasks.Add (CreateTask (
					TestPlatform.Mac,
					"Debug (CoreCLR, managed static registrar)",
					"coreclr|managed-static-registrar",
					!selected));
			}

			if (Jenkins.Harness.INCLUDE_MACCATALYST && Jenkins.TestSelection.IsEnabled (PlatformLabel.MacCatalyst)) {
				tasks.Add (CreateTask (
					TestPlatform.MacCatalyst,
					"Debug (MonoVM, managed static registrar)",
					"monovm|managed-static-registrar",
					!selected || !Jenkins.Harness.DOTNET_MONOVM_SUPPORTED));
				tasks.Add (CreateTask (
					TestPlatform.MacCatalyst,
					"Debug (CoreCLR, managed static registrar)",
					"coreclr|managed-static-registrar",
					!selected));
			}

			return Task.FromResult<IEnumerable<AppleTestTask>> (tasks);
		}

		AppExtensionTestTask CreateTask (TestPlatform platform, string variation, string testVariation, bool ignored)
		{
			return new AppExtensionTestTask (Jenkins, ProcessManager, platform, testVariation) {
				TestName = "monotouch-test app extension",
				Variation = variation,
				Ignored = ignored,
			};
		}
	}
}
