using System;
using System.IO;
using System.Diagnostics;
using System.ComponentModel;
using System.Collections.Generic;
using System.Threading;

using Microsoft.Build.Framework;
using Microsoft.Build.Utilities;
using Xamarin.Localization.MSBuild;

using Xamarin.MacDev;
using Xamarin.Messaging.Build.Client;

#nullable enable

namespace Xamarin.MacDev.Tasks {
	public abstract class XcodeToolTaskBase : XamarinTask, IHasProjectDir, IHasResourcePrefix, ICancelableTask {
		CancellationTokenSource cancellationTokenSource = new ();

		#region Inputs

		[Required]
		public string IntermediateOutputPath { get; set; } = string.Empty;

		[Required]
		public string ProjectDir { get; set; } = string.Empty;

		[Required]
		public string ResourcePrefix { get; set; } = string.Empty;

		public string ToolPath { get; set; } = string.Empty;

		#endregion

		#region Outputs

		[Output]
		public ITaskItem [] BundleResources { get; set; } = Array.Empty<ITaskItem> ();

		#endregion

		protected abstract string ToolName { get; }

		protected abstract IEnumerable<ITaskItem> EnumerateInputs ();

		protected abstract void AppendCommandLineArguments (List<string> args, ITaskItem input, ITaskItem output);

		protected virtual string GetBundleRelativeOutputPath (ITaskItem input)
		{
			return BundleResource.GetLogicalName (this, input);
		}

		protected virtual IEnumerable<ITaskItem> GetCompiledBundleResources (ITaskItem input, ITaskItem output)
		{
			yield return output;
		}

		protected virtual bool NeedsBuilding (ITaskItem input, ITaskItem output)
		{
			var dest = output.GetMetadata ("FullPath");
			var src = input.GetMetadata ("FullPath");

			return !File.Exists (dest) || File.GetLastWriteTimeUtc (src) > File.GetLastWriteTimeUtc (dest);
		}

		int ExecuteTool (ITaskItem input, ITaskItem output)
		{
			var args = new List<string> ();

			AppendCommandLineArguments (args, input, output);

			var executable = GetExecutable (args, ToolName, ToolPath);
			var rv = ExecuteAsync (executable, args, cancellationToken: cancellationTokenSource.Token).Result;
			return rv.ExitCode;
		}

		public override bool Execute ()
		{
			var intermediate = Path.Combine (IntermediateOutputPath, ToolName);
			var bundleResources = new List<ITaskItem> ();

			foreach (var input in EnumerateInputs ()) {
				var relative = GetBundleRelativeOutputPath (input);
				ITaskItem output;

				if (!string.IsNullOrEmpty (relative)) {
					if (BundleResource.IsIllegalName (relative, out var illegal)) {
						Log.LogError (null, null, null, input.ItemSpec, 0, 0, 0, 0, MSBStrings.E0102, illegal);
						continue;
					}

					var rpath = Path.Combine (intermediate, relative);

					output = new TaskItem (rpath);
				} else {
					output = new TaskItem (intermediate);
				}

				output.SetMetadata ("LogicalName", relative);

				if (NeedsBuilding (input, output)) {
					Directory.CreateDirectory (Path.GetDirectoryName (output.ItemSpec)!);

					if (ExecuteTool (input, output) == -1)
						return false;
				}

				bundleResources.AddRange (GetCompiledBundleResources (input, output));
			}

			BundleResources = bundleResources.ToArray ();

			return !Log.HasLoggedErrors;
		}

		public void Cancel ()
		{
			if (ShouldExecuteRemotely ()) {
				BuildConnection.CancelAsync (BuildEngine4).Wait ();
			} else {
				cancellationTokenSource?.Cancel ();
			}
		}
	}
}
