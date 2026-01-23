using System;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;

using Microsoft.Build.Utilities;
using Microsoft.Build.Framework;

using Xamarin.Messaging.Build.Client;
using Xamarin.Utils;

namespace Xamarin.MacDev.Tasks {
	public class ScnTool : XamarinParallelTask, IHasProjectDir, IHasResourcePrefix {
		#region Inputs

		[Required]
		public string IntermediateOutputPath { get; set; } = string.Empty;

		[Required]
		public ITaskItem [] ColladaAssets { get; set; } = Array.Empty<ITaskItem> ();

		[Required]
		public string DeviceSpecificIntermediateOutputPath { get; set; } = string.Empty;

		public bool IsWatchApp { get; set; }

		[Required]
		public string ProjectDir { get; set; } = string.Empty;

		[Required]
		public string ResourcePrefix { get; set; } = string.Empty;

		[Required]
		public string SdkPlatform { get; set; } = string.Empty;

		[Required]
		public string SdkRoot { get; set; } = string.Empty;

		[Required]
		public string SdkVersion { get; set; } = string.Empty;

		#endregion

		#region Outputs
		[Output]
		public ITaskItem [] BundleResources { get; set; } = Array.Empty<ITaskItem> ();
		#endregion

		IList<string> GenerateCommandLineCommands (string inputScene, string outputScene)
		{
			var args = new List<string> ();

			args.Add ("scntool");
			args.Add ("--compress");
			args.Add (inputScene);
			args.Add ("-o");
			args.Add (outputScene);
			args.Add ($"--sdk-root={SdkRoot}");
			args.Add ($"--target-build-dir={IntermediateOutputPath}");
			args.Add ($"--target-version={SdkVersion}");
			args.Add ($"--target-platform={PlatformUtils.GetTargetPlatform (SdkPlatform, IsWatchApp)}");

			return args;
		}

		public override bool Execute ()
		{
			if (ShouldExecuteRemotely ())
				return ExecuteRemotely ();

			var colladaAssets = CollectBundleResources.ComputeLogicalNameAndDetectDuplicates (this, ColladaAssets, ProjectDir, ResourcePrefix, "Collada");
			var listOfArguments = new List<(IList<string> Arguments, ITaskItem Input)> ();
			var bundleResources = new List<ITaskItem> ();
			foreach (var asset in colladaAssets) {
				var inputScene = asset.ItemSpec;
				var logicalName = asset.GetMetadata ("LogicalName");
				var outputScene = Path.Combine (DeviceSpecificIntermediateOutputPath, logicalName);
				var args = GenerateCommandLineCommands (inputScene, outputScene);
				listOfArguments.Add (new (args, asset));

				Directory.CreateDirectory (Path.GetDirectoryName (outputScene)!);

				var bundleResource = new TaskItem (outputScene);
				asset.CopyMetadataTo (bundleResource);
				bundleResource.SetMetadata ("Optimize", "false");
				bundleResource.SetMetadata ("LogicalName", logicalName);
				bundleResources.Add (bundleResource);
			}

			ForEach (listOfArguments, (arg) => {
				ExecuteAsync ("xcrun", arg.Arguments).Wait ();
			});

			BundleResources = bundleResources.ToArray ();

			return !Log.HasLoggedErrors;
		}

		public void Cancel ()
		{
			if (ShouldExecuteRemotely ())
				BuildConnection.CancelAsync (BuildEngine4).Wait ();
		}
	}
}
