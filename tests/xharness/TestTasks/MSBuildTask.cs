﻿using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Threading.Tasks;
using System.Xml;
using Microsoft.DotNet.XHarness.iOS.Shared;
using Microsoft.DotNet.XHarness.iOS.Shared.Execution;
using Microsoft.DotNet.XHarness.iOS.Shared.Logging;
using Microsoft.DotNet.XHarness.iOS.Shared.Utilities;

namespace Xharness.TestTasks {

	public class MSBuildTask : BuildProjectTask {
		readonly IErrorKnowledgeBase errorKnowledgeBase;
		readonly string msbuildPath;

		public virtual List<string> GetToolArguments (string projectPlatform, string projectConfiguration, string projectFile, ILog buildLog) {
			var binlogPath = buildLog.FullPath.Replace (".txt", ".binlog");

			var args = new List<string> ();
			args.Add ("--");
			args.Add ("/verbosity:diagnostic");
			args.Add ($"/bl:{binlogPath}");
			if (SpecifyPlatform)
				args.Add ($"/p:Platform={projectPlatform}");
			if (SpecifyConfiguration)
				args.Add ($"/p:Configuration={projectConfiguration}");
			args.Add (projectFile);
			return args;
		}

		public MSBuildTask (string msbuildPath,
							IProcessManager processManager,
							IResourceManager resourceManager,
							IEventLogger eventLogger,
							IEnvManager envManager,
							IErrorKnowledgeBase errorKnowledgeBase) : base (processManager, resourceManager, eventLogger, envManager)
		{
			this.msbuildPath = msbuildPath ?? throw new ArgumentNullException (nameof (msbuildPath));
			this.errorKnowledgeBase = errorKnowledgeBase ?? throw new ArgumentNullException (nameof (errorKnowledgeBase));
		}

		public async Task<(TestExecutingResult ExecutionResult, string KnownFailure)> ExecuteAsync (string projectPlatform, 
																									string projectConfiguration,
																									string projectFile,
																									IAcquiredResource resource,
																									bool dryRun,
																									ILog buildLog,
																									ILog mainLog)
		{
			(TestExecutingResult ExecutionResult, string KnownFailure) result = (TestExecutingResult.NotStarted, (string) null);
			await RestoreNugetsAsync (buildLog, resource, useXIBuild: true);

			using (var xbuild = new Process ()) {
				xbuild.StartInfo.FileName = msbuildPath;
				xbuild.StartInfo.Arguments = StringUtils.FormatArguments (GetToolArguments (projectPlatform, projectConfiguration, projectFile, buildLog));
				EnviromentManager.SetEnvironmentVariables (xbuild);
				xbuild.StartInfo.EnvironmentVariables ["MSBuildExtensionsPath"] = null;
				EventLogger.LogEvent (buildLog, "Building {0} ({1})", TestName, Mode);
				if (!dryRun) {
					var timeout = TimeSpan.FromMinutes (60);
					var processResult = await ProcessManager.RunAsync (xbuild, buildLog, timeout);
					if (processResult.TimedOut) {
						result.ExecutionResult = TestExecutingResult.TimedOut;
						buildLog.WriteLine ("Build timed out after {0} seconds.", timeout.TotalSeconds);
					} else if (processResult.Succeeded) {
						result.ExecutionResult = TestExecutingResult.Succeeded;
					} else {
						result.ExecutionResult = TestExecutingResult.Failed;
						if (errorKnowledgeBase.IsKnownBuildIssue (buildLog, out result.KnownFailure))
							buildLog.WriteLine ($"Build has a known failure: '{result.KnownFailure}'");
					}
				}
				mainLog.WriteLine ("Built {0} ({1})", TestName, Mode);
			}
			return result;
		}

		async Task CleanProjectAsync (string project_file, string project_platform, string project_configuration, ILog log, ILog mainLog)
		{
			// Don't require the desktop resource here, this shouldn't be that resource sensitive
			using (var xbuild = new Process ()) {
				xbuild.StartInfo.FileName = Harness.XIBuildPath;
				var args = new List<string> ();
				args.Add ("--");
				args.Add ("/verbosity:diagnostic");
				if (project_platform != null)
					args.Add ($"/p:Platform={project_platform}");
				if (project_configuration != null)
					args.Add ($"/p:Configuration={project_configuration}");
				args.Add (project_file);
				args.Add ("/t:Clean");
				xbuild.StartInfo.Arguments = StringUtils.FormatArguments (args);
				EnviromentManager.SetEnvironmentVariables (xbuild);
				EventLogger.LogEvent (log, "Cleaning {0} ({1}) - {2}", TestName, Mode, project_file);
				var timeout = TimeSpan.FromMinutes (1);
				await ProcessManager.RunAsync (xbuild, log, timeout);
				log.WriteLine ("Clean timed out after {0} seconds.", timeout.TotalSeconds);
				mainLog.WriteLine ("Cleaned {0} ({1})", TestName, Mode);
			}
		}

		public async Task CleanAsync (string projectPlatform, string projectConfiguration, string projectFile, ILog cleanLog, ILog mainLog)
		{
			await CleanProjectAsync (projectFile, SpecifyPlatform ? projectPlatform : null, SpecifyConfiguration ? projectConfiguration : null, cleanLog, mainLog);

			// Iterate over all the project references as well.
			var doc = new XmlDocument ();
			doc.LoadWithoutNetworkAccess (projectFile);
			foreach (var pr in doc.GetProjectReferences ()) {
				var path = pr.Replace ('\\', '/');
				await CleanProjectAsync (path, SpecifyPlatform ? projectPlatform : null, SpecifyConfiguration ? projectConfiguration : null, cleanLog, mainLog);
			}
		}
	}
}
