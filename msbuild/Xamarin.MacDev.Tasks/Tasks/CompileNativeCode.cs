using System;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;

using Microsoft.Build.Framework;
using Microsoft.Build.Utilities;

using Xamarin.Localization.MSBuild;
using Xamarin.Messaging.Build.Client;
using Xamarin.Utils;

// Disable until we get around to enable + fix any issues.
#nullable disable

namespace Xamarin.MacDev.Tasks {
	public class CompileNativeCode : XamarinTask, ICancelableTask, ITaskCallback {

		#region Inputs
		[Required]
		public ITaskItem [] CompileInfo { get; set; }

		public ITaskItem [] IncludeDirectories { get; set; }

		[Required]
		public string MinimumOSVersion { get; set; }

		[Required]
		public string SdkDevPath { get; set; }

		[Required]
		public string SdkRoot { get; set; }

		[Required]
		public bool SdkIsSimulator { get; set; }

		public string DotNetRoot { get; set; }
		#endregion

		public override bool Execute ()
		{
			if (ShouldExecuteRemotely ()) {
				foreach (var info in CompileInfo) {
					var outputFile = info.GetMetadata ("OutputFile");

					if (!string.IsNullOrEmpty (outputFile))
						info.SetMetadata ("OutputFile", outputFile.Replace ("\\", "/"));
				}

				return new TaskRunner (SessionId, BuildEngine4).RunAsync (this).Result;
			}

			var processes = new Task<Execution> [CompileInfo.Length];

			for (var i = 0; i < CompileInfo.Length; i++) {
				var info = CompileInfo [i];
				var src = Path.GetFullPath (info.ItemSpec);
				var arguments = new List<string> ();

				arguments.Add ("clang");
				arguments.Add ("-g");

				var arch = info.GetMetadata ("Arch");

				switch (Platform) {
				case ApplePlatform.iOS:
				case ApplePlatform.TVOS:
				case ApplePlatform.MacOSX:
					arguments.Add (PlatformFrameworkHelper.GetMinimumVersionArgument (TargetFrameworkMoniker, SdkIsSimulator, MinimumOSVersion));

					if (!string.IsNullOrEmpty (arch)) {
						arguments.Add ("-arch");
						arguments.Add (arch);
					}

					break;
				case ApplePlatform.MacCatalyst:
					arguments.Add ($"-target");
					arguments.Add ($"{arch}-apple-ios{MinimumOSVersion}-macabi");
					arguments.Add ("-isystem");
					arguments.Add (Path.Combine (SdkRoot, "System", "iOSSupport", "usr", "include"));
					arguments.Add ("-iframework");
					arguments.Add (Path.Combine (SdkRoot, "System", "iOSSupport", "System", "Library", "Frameworks"));
					break;
				default:
					throw new InvalidOperationException (string.Format (MSBStrings.InvalidPlatform, Platform));
				}

				arguments.Add ("-isysroot");
				arguments.Add (SdkRoot);

				if (IncludeDirectories is not null) {
					foreach (var inc in IncludeDirectories) {
						var incPath = GetIncludeDirectory (inc);

						arguments.Add ("-I" + incPath);
					}
				}

				var args = info.GetMetadata ("Arguments");
				if (!StringUtils.TryParseArguments (args, out var parsed_args, out var ex)) {
					Log.LogError ("Could not parse the arguments '{0}': {1}", args, ex.Message);
					return false;
				}
				arguments.AddRange (parsed_args);


				var outputFile = info.GetMetadata ("OutputFile");
				if (string.IsNullOrEmpty (outputFile))
					outputFile = Path.ChangeExtension (src, ".o");
				outputFile = Path.GetFullPath (outputFile);
				arguments.Add ("-o");
				arguments.Add (outputFile);

				arguments.Add ("-c");
				arguments.Add (src);

				processes [i] = ExecuteAsync ("xcrun", arguments, sdkDevPath: SdkDevPath);
			}

			System.Threading.Tasks.Task.WaitAll (processes);

			return !Log.HasLoggedErrors;
		}

		public bool ShouldCopyToBuildServer (ITaskItem item) => false;

		public bool ShouldCreateOutputFile (ITaskItem item) => true;

		public IEnumerable<ITaskItem> GetAdditionalItemsToBeCopied ()
		{
			if (IncludeDirectories is not null) {
				foreach (var dir in IncludeDirectories) {
					foreach (var file in Directory.EnumerateFiles (dir.ItemSpec, "*.*", SearchOption.AllDirectories)) {
						yield return new TaskItem (file);
					}
				}
			}
		}

		public void Cancel ()
		{
			if (ShouldExecuteRemotely ())
				BuildConnection.CancelAsync (BuildEngine4).Wait ();
		}

		string GetIncludeDirectory(ITaskItem item)
		{
			var path = Path.GetFullPath (item.ItemSpec);

			if (string.IsNullOrEmpty (DotNetRoot)) {
				return path;
			}
				
			var packsIdentifier = "packs";
			var dotnetPacksIdentifier = Path.Combine ("dotnet", packsIdentifier);

			//If the directory points to a dotnet pack, we want to ensure the full path 
			//is actually pointing to a sub-folder in the dotnet SDK
			if (path.Contains (dotnetPacksIdentifier) && !path.StartsWith (DotNetRoot)) {
				var relativePath = path.Substring (path.IndexOf (packsIdentifier));
				//We combine the relative pack dir (starting from "packs") with the dotnet root to get the full path
				var newPath = Path.Combine (DotNetRoot, relativePath);

				Log.LogMessage (MessageImportance.Low, MSBStrings.M0169, path, newPath);
				path = newPath;
			}

			return path;
		}
	}
}
