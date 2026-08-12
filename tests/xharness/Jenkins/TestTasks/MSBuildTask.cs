using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using System.Xml;
using Microsoft.DotNet.XHarness.Common.Execution;
using Microsoft.DotNet.XHarness.Common.Logging;
using Microsoft.DotNet.XHarness.iOS.Shared;
using Microsoft.DotNet.XHarness.iOS.Shared.Logging;
using Microsoft.DotNet.XHarness.iOS.Shared.Utilities;
using Xamarin;
using Xamarin.Utils;

namespace Xharness.Jenkins.TestTasks {
	public class MSBuildTask : BuildProjectTask {
		protected virtual string ToolName {
			get {
				return Jenkins.Harness.GetDotNetExecutable (Path.GetDirectoryName (ProjectFile)!);
			}
		}

		public override void SetEnvironmentVariables (Process process)
		{
			base.SetEnvironmentVariables (process);
			// modify those env vars that we do care about

			process.StartInfo.EnvironmentVariables.Remove ("MSBUILD_EXE_PATH");
			process.StartInfo.EnvironmentVariables.Remove ("MSBuildExtensionsPathFallbackPathsOverride");
			process.StartInfo.EnvironmentVariables.Remove ("MSBuildSDKsPath");
			process.StartInfo.EnvironmentVariables.Remove ("TargetFrameworkFallbackSearchPaths");
			process.StartInfo.EnvironmentVariables.Remove ("MSBuildExtensionsPathFallbackPathsOverride");
		}

		protected virtual List<string> ToolArguments =>
				MSBuild.GetToolArguments (ProjectPlatform, ProjectConfiguration, ProjectFile, BuildLog!);

		MSBuild MSBuild => (MSBuild) buildToolTask;

		public MSBuildTask (Jenkins jenkins, TestProject testProject, IProcessManager processManager)
			: base (jenkins, testProject, processManager) { }

		protected override void InitializeTool ()
		{
			buildToolTask = new DotNetBuild (
				msbuildPath: () => ToolName,
				processManager: ProcessManager,
				resourceManager: ResourceManager,
				eventLogger: this,
				envManager: this,
				errorKnowledgeBase: Jenkins.ErrorKnowledgeBase);
		}

		protected override async Task ExecuteAsync ()
		{
			using var resource = await NotifyAndAcquireDesktopResourceAsync ();
			BuildLog = Logs.Create ($"build-{Platform}-{Timestamp}.txt", LogType.BuildLog.ToString ());
			(ExecutionResult, KnownFailure) = await MSBuild.ExecuteAsync (
				projectPlatform: ProjectPlatform!,
				projectConfiguration: ProjectConfiguration!,
				projectFile: ProjectFile,
				resource: resource,
				dryRun: Jenkins.Harness.DryRun,
				buildLog: BuildLog,
				mainLog: Jenkins.MainLog);

			BuildLog.Dispose ();
		}

		public override IEnumerable<ILog> AggregatedLogs
			=> BuildLog is null ? base.AggregatedLogs : base.AggregatedLogs.Union (new [] { BuildLog });

		public static async Task BuildInParallelAsync (IReadOnlyList<MSBuildTask> tasks, IFileBackedLog buildLog, ILog mainLog, bool dryRun)
		{
			if (tasks.Count == 0)
				return;

			var buildDirectory = Cache.CreateTemporaryDirectory ("parallel-msbuild");
			var projectFile = Path.Combine (buildDirectory, "build.proj");
			var buildProjects = new List<(MSBuildTask Task, string Project, string SuccessMarker, string FailureMarker, string RestoreFailureMarker)> ();

			for (var i = 0; i < tasks.Count; i++) {
				var task = tasks [i];
				var wrapperProject = Path.Combine (buildDirectory, $"build-{i}.proj");
				var successMarker = Path.Combine (buildDirectory, $"build-{i}.success");
				var failureMarker = Path.Combine (buildDirectory, $"build-{i}.failure");
				var restoreFailureMarker = Path.Combine (buildDirectory, $"restore-{i}.failure");
				WriteBuildProject (wrapperProject, successMarker, failureMarker, restoreFailureMarker, task);
				buildProjects.Add ((task, wrapperProject, successMarker, failureMarker, restoreFailureMarker));
			}
			WriteRootBuildProject (projectFile, buildProjects.Select (v => v.Project));

			var binlogPath = Path.ChangeExtension (buildLog.FullPath, ".binlog");
			var restoreBinlogPath = Path.ChangeExtension (buildLog.FullPath, ".restore.binlog");
			var firstTask = tasks [0];

			mainLog.WriteLine ($"Building {tasks.Count} projects in parallel");
			ProcessExecutionResult? processResult = null;
			if (!dryRun) {
				var timeout = TimeSpan.FromMinutes (60);
				using (await firstTask.ResourceManager.DesktopResource.AcquireExclusiveAsync ()) {
					ProcessExecutionResult restoreResult;
					using (await firstTask.ResourceManager.NugetResource.AcquireExclusiveAsync ()) {
						mainLog.WriteLine ($"Restoring {tasks.Count} projects serially");
						restoreResult = await RunMSBuildAsync (firstTask, projectFile, "Restore", restoreBinlogPath, false, buildLog, timeout);
					}
					processResult = restoreResult.TimedOut
						? restoreResult
						: await RunMSBuildAsync (firstTask, projectFile, "Build", binlogPath, true, buildLog, timeout);
				}
			}

			var failedTasks = new List<MSBuildTask> ();
			foreach (var buildProject in buildProjects) {
				var succeeded = dryRun || File.Exists (buildProject.SuccessMarker);
				var failed = File.Exists (buildProject.FailureMarker) || File.Exists (buildProject.RestoreFailureMarker);
				var task = buildProject.Task;
				task.BuildLog = buildLog;
				task.KnownFailure = null;
				task.ExecutionResult = (succeeded ? TestExecutingResult.Succeeded : failed ? TestExecutingResult.Failed : processResult?.TimedOut == true ? TestExecutingResult.TimedOut : TestExecutingResult.Failed) | TestExecutingResult.Finished;
				if (!succeeded) {
					task.FailureMessage = failed || processResult?.TimedOut != true ? "Project failed in the parallel build." : "Parallel build timed out.";
					failedTasks.Add (task);
				}
			}

			if (failedTasks.Count == 1 && firstTask.Jenkins.ErrorKnowledgeBase.IsKnownBuildIssue (buildLog, out var knownFailure)) {
				failedTasks [0].KnownFailure = knownFailure;
			}
			mainLog.WriteLine ($"Built {(dryRun ? tasks.Count : buildProjects.Count (v => File.Exists (v.SuccessMarker)))} of {tasks.Count} projects in parallel");
		}

		static async Task<ProcessExecutionResult> RunMSBuildAsync (MSBuildTask task, string projectFile, string target, string binlogPath, bool buildInParallel, ILog buildLog, TimeSpan timeout)
		{
			using var process = new Process ();
			process.StartInfo.FileName = task.ToolName;
			var arguments = new List<string> {
				"msbuild",
				$"/t:{target}",
				"/verbosity:diagnostic",
				$"/bl:{binlogPath}",
				projectFile,
			};
			if (buildInParallel)
				arguments.Insert (1, "/m");
			process.StartInfo.Arguments = StringUtils.FormatArguments (arguments);
			process.StartInfo.WorkingDirectory = Path.GetDirectoryName (projectFile);
			task.SetEnvironmentVariables (process);
			return await task.ProcessManager.RunAsync (process, buildLog, timeout);
		}

		static void WriteBuildProject (string path, string successMarker, string failureMarker, string restoreFailureMarker, MSBuildTask task)
		{
			var properties = new List<string> {
				$"RootTestsDirectory={EscapePropertyValue (HarnessConfiguration.RootDirectory)}",
			};
			if (task.SpecifyPlatform)
				properties.Add ($"Platform={EscapePropertyValue (task.ProjectPlatform ?? "")}");
			if (task.SpecifyConfiguration)
				properties.Add ($"Configuration={EscapePropertyValue (task.ProjectConfiguration ?? "")}");
			if (task.Constants.Count > 0)
				properties.Add ($"DefineConstants={EscapePropertyValue (string.Join (";", task.Constants))}");

			var settings = new XmlWriterSettings { Indent = true };
			using var writer = XmlWriter.Create (path, settings);
			writer.WriteStartElement ("Project");
			writer.WriteStartElement ("PropertyGroup");
			writer.WriteElementString ("ProjectToBuild", task.ProjectFile);
			writer.WriteElementString ("BuildProperties", string.Join (";", properties));
			writer.WriteElementString ("SuccessMarker", successMarker);
			writer.WriteElementString ("FailureMarker", failureMarker);
			writer.WriteElementString ("RestoreFailureMarker", restoreFailureMarker);
			writer.WriteEndElement ();
			writer.WriteStartElement ("Target");
			writer.WriteAttributeString ("Name", "Restore");
			writer.WriteStartElement ("MSBuild");
			writer.WriteAttributeString ("Projects", "$(ProjectToBuild)");
			writer.WriteAttributeString ("Targets", "Restore");
			writer.WriteAttributeString ("Properties", "$(BuildProperties)");
			writer.WriteEndElement ();
			writer.WriteStartElement ("OnError");
			writer.WriteAttributeString ("ExecuteTargets", "RestoreFailed");
			writer.WriteEndElement ();
			writer.WriteEndElement ();
			writer.WriteStartElement ("Target");
			writer.WriteAttributeString ("Name", "RestoreFailed");
			writer.WriteStartElement ("WriteLinesToFile");
			writer.WriteAttributeString ("File", "$(RestoreFailureMarker)");
			writer.WriteAttributeString ("Lines", "failure");
			writer.WriteAttributeString ("Overwrite", "true");
			writer.WriteEndElement ();
			writer.WriteEndElement ();
			writer.WriteStartElement ("Target");
			writer.WriteAttributeString ("Name", "Build");
			writer.WriteStartElement ("MSBuild");
			writer.WriteAttributeString ("Projects", "$(ProjectToBuild)");
			writer.WriteAttributeString ("Targets", "Build");
			writer.WriteAttributeString ("Properties", "$(BuildProperties)");
			writer.WriteEndElement ();
			writer.WriteStartElement ("WriteLinesToFile");
			writer.WriteAttributeString ("File", "$(SuccessMarker)");
			writer.WriteAttributeString ("Lines", "success");
			writer.WriteAttributeString ("Overwrite", "true");
			writer.WriteEndElement ();
			writer.WriteStartElement ("OnError");
			writer.WriteAttributeString ("ExecuteTargets", "BuildFailed");
			writer.WriteEndElement ();
			writer.WriteEndElement ();
			writer.WriteStartElement ("Target");
			writer.WriteAttributeString ("Name", "BuildFailed");
			writer.WriteStartElement ("WriteLinesToFile");
			writer.WriteAttributeString ("File", "$(FailureMarker)");
			writer.WriteAttributeString ("Lines", "failure");
			writer.WriteAttributeString ("Overwrite", "true");
			writer.WriteEndElement ();
			writer.WriteEndElement ();
			writer.WriteEndElement ();
		}

		static void WriteRootBuildProject (string path, IEnumerable<string> projects)
		{
			var settings = new XmlWriterSettings { Indent = true };
			using var writer = XmlWriter.Create (path, settings);
			writer.WriteStartElement ("Project");
			writer.WriteAttributeString ("DefaultTargets", "Build");
			writer.WriteStartElement ("ItemGroup");
			foreach (var project in projects) {
				writer.WriteStartElement ("ProjectsToBuild");
				writer.WriteAttributeString ("Include", project);
				writer.WriteEndElement ();
			}
			writer.WriteEndElement ();
			writer.WriteStartElement ("Target");
			writer.WriteAttributeString ("Name", "Restore");
			writer.WriteStartElement ("MSBuild");
			writer.WriteAttributeString ("Projects", "@(ProjectsToBuild)");
			writer.WriteAttributeString ("Targets", "Restore");
			writer.WriteAttributeString ("BuildInParallel", "false");
			writer.WriteAttributeString ("StopOnFirstFailure", "false");
			writer.WriteAttributeString ("ContinueOnError", "WarnAndContinue");
			writer.WriteEndElement ();
			writer.WriteEndElement ();
			writer.WriteStartElement ("Target");
			writer.WriteAttributeString ("Name", "Build");
			writer.WriteStartElement ("MSBuild");
			writer.WriteAttributeString ("Projects", "@(ProjectsToBuild)");
			writer.WriteAttributeString ("Targets", "Build");
			writer.WriteAttributeString ("BuildInParallel", "true");
			writer.WriteAttributeString ("StopOnFirstFailure", "false");
			writer.WriteEndElement ();
			writer.WriteEndElement ();
			writer.WriteEndElement ();
		}

		static string EscapePropertyValue (string value)
			=> value.Replace ("%", "%25").Replace (";", "%3B");

		public override Task CleanAsync () =>
			MSBuild.CleanAsync (
				projectPlatform: ProjectPlatform!,
				projectConfiguration: ProjectConfiguration!,
				projectFile: ProjectFile,
				cleanLog: Logs.Create ($"clean-{Platform}-{Timestamp}.txt", "Clean log"),
				mainLog: Jenkins.MainLog);

		public static void SetDotNetEnvironmentVariables (Dictionary<string, string?> environment)
		{
			environment ["MSBUILD_EXE_PATH"] = null;
			environment ["MSBuildExtensionsPathFallbackPathsOverride"] = null;
			environment ["MSBuildSDKsPath"] = null;
			environment ["TargetFrameworkFallbackSearchPaths"] = null;
			environment ["MSBuildExtensionsPathFallbackPathsOverride"] = null;
		}

		public override bool SupportsParallelExecution {
			get => false;
		}
	}
}
