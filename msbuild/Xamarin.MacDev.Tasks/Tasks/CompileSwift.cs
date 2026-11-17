// Copyright (c) Microsoft Corporation.
// Licensed under the MIT License.

using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

using Microsoft.Build.Framework;
using Microsoft.Build.Utilities;

using Xamarin.Localization.MSBuild;
using Xamarin.Messaging.Build.Client;
using Xamarin.Utils;

#nullable enable

namespace Xamarin.MacDev.Tasks {
	// Compiles Swift source files into object files (one per Swift module), which can be linked into the app's executable.
	// Source files are grouped into modules using the 'ModuleName' metadata, and all the source files in a module
	// must have the same 'BridgingHeader' and 'AdditionalArguments' metadata.
	public class CompileSwift : XamarinTask, ICancelableTask, ITaskCallback {
		#region Inputs
		public bool IsAppExtension { get; set; }

		[Required]
		public string MinimumOSVersion { get; set; } = "";

		// The object file for each module is named '<OutputDirectory>/<ModuleName>.o'.
		[Required]
		public string OutputDirectory { get; set; } = "";

		[Required]
		public bool SdkIsSimulator { get; set; }

		[Required]
		public string SdkRoot { get; set; } = "";

		[Required]
		public ITaskItem [] Sources { get; set; } = [];

		[Required]
		public string TargetArchitectures { get; set; } = "";
		#endregion

		#region Outputs
		// This is an output so that (empty) placeholder files are created on Windows for remote builds,
		// otherwise the target that compiles the Swift code (and the native link) would always run.
		[Output]
		public ITaskItem [] CompiledOutputFiles { get; set; } = [];
		#endregion

		public override bool Execute ()
		{
			if (ShouldExecuteRemotely ())
				return ExecuteRemotely ();

			if (!Enum.TryParse<TargetArchitecture> (TargetArchitectures, out var architectures)) {
				Log.LogError (12, null, MSBStrings.E0012, TargetArchitectures);
				return false;
			}

			var abis = architectures.ToArray ();
			if (abis.Count != 1) {
				Log.LogError (7070, null, MSBStrings.E7070, TargetArchitectures);
				return false;
			}
			var arch = abis [0].ToNativeArchitecture ();

			string target;
			switch (Platform) {
			case ApplePlatform.iOS:
				target = $"{arch}-apple-ios{MinimumOSVersion}" + (SdkIsSimulator ? "-simulator" : "");
				break;
			case ApplePlatform.TVOS:
				target = $"{arch}-apple-tvos{MinimumOSVersion}" + (SdkIsSimulator ? "-simulator" : "");
				break;
			case ApplePlatform.MacCatalyst:
				target = $"{arch}-apple-ios{MinimumOSVersion}-macabi";
				break;
			case ApplePlatform.MacOSX:
				target = $"{arch}-apple-macos{MinimumOSVersion}";
				break;
			default:
				throw new InvalidOperationException (string.Format (MSBStrings.InvalidPlatform, Platform));
			}

			var outputDirectory = Path.GetFullPath (OutputDirectory);
			Directory.CreateDirectory (outputDirectory);

			var modules = Sources.GroupBy (v => v.GetMetadata ("ModuleName")).ToArray ();
			var processes = new List<System.Threading.Tasks.Task<Execution>> ();
			var outputFiles = new List<ITaskItem> ();
			foreach (var module in modules) {
				var moduleName = module.Key;
				if (!TryGetSameMetadata (moduleName, module, "BridgingHeader", out var bridgingHeader))
					continue;
				if (!TryGetSameMetadata (moduleName, module, "AdditionalArguments", out var additionalArguments))
					continue;
				if (!StringUtils.TryParseArguments (additionalArguments, out var parsedAdditionalArguments, out var ex)) {
					Log.LogError (7202, null, MSBStrings.E7202, additionalArguments, moduleName, ex.Message);
					continue;
				}

				var outputFile = Path.Combine (OutputDirectory, moduleName + ".o");
				var arguments = new List<string> {
					"swiftc",
					"-c",
					"-g",
					"-parse-as-library",
					"-whole-module-optimization",
					"-swift-version", "5",
					"-module-name", moduleName,
					"-sdk", SdkRoot,
					"-target", target,
				};
				// The app's executable already has a 'main' function (which initializes .NET), so rename the function Swift
				// creates for '@main' (if there's no '@main' this has no effect). ExtensionKit extensions (where 'main' calls
				// NSExtensionMain) still find and call the renamed function, because it's referenced from the '__swift5_entry' section.
				arguments.AddRange (["-Xfrontend", "-entry-point-function-name", "-Xfrontend", "xamarin_swift_main"]);
				if (IsAppExtension)
					arguments.Add ("-application-extension");
				if (!string.IsNullOrEmpty (bridgingHeader)) {
					arguments.Add ("-import-objc-header");
					arguments.Add (Path.GetFullPath (bridgingHeader));
				}
				arguments.AddRange (parsedAdditionalArguments);
				arguments.Add ("-o");
				arguments.Add (Path.GetFullPath (outputFile));
				arguments.AddRange (module.Select (v => Path.GetFullPath (v.ItemSpec)));

				processes.Add (ExecuteAsync ("xcrun", arguments));
				outputFiles.Add (new TaskItem (outputFile));
			}

			System.Threading.Tasks.Task.WaitAll (processes.ToArray ());

			CompiledOutputFiles = outputFiles.ToArray ();

			return !Log.HasLoggedErrors;
		}

		bool TryGetSameMetadata (string moduleName, IEnumerable<ITaskItem> items, string metadataName, out string value)
		{
			var values = items.Select (v => v.GetMetadata (metadataName)).Distinct ().ToArray ();
			value = values [0];
			if (values.Length == 1)
				return true;
			Log.LogError (7201, null, MSBStrings.E7201, moduleName, metadataName, values [0], values [1]);
			return false;
		}

		public bool ShouldCopyToBuildServer (ITaskItem item) => true;

		public bool ShouldCreateOutputFile (ITaskItem item) => true;

		public IEnumerable<ITaskItem> GetAdditionalItemsToBeCopied ()
		{
			foreach (var header in Sources.Select (v => v.GetMetadata ("BridgingHeader")).Where (v => !string.IsNullOrEmpty (v)).Distinct ())
				yield return new TaskItem (header);
		}

		public void Cancel ()
		{
			if (ShouldExecuteRemotely ())
				BuildConnection.CancelAsync (BuildEngine4).Wait ();
		}
	}
}
