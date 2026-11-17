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
	// For each module this also writes a '<OutputDirectory>/<ModuleName>.dependencies' file, which lists the files
	// (such as headers included from the bridging header) the module depends on, one per line.
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
			if (ShouldExecuteRemotely ()) {
				// These paths aren't converted automatically when building remotely from Windows.
				OutputDirectory = OutputDirectory.Replace ('\\', '/');
				foreach (var source in Sources) {
					var bridgingHeader = source.GetMetadata ("BridgingHeader");
					if (!string.IsNullOrEmpty (bridgingHeader))
						source.SetMetadata ("BridgingHeader", bridgingHeader.Replace ('\\', '/'));
				}

				return ExecuteRemotely ();
			}

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
			var platformArguments = new List<string> ();
			switch (Platform) {
			case ApplePlatform.iOS:
				target = $"{arch}-apple-ios{MinimumOSVersion}" + (SdkIsSimulator ? "-simulator" : "");
				break;
			case ApplePlatform.TVOS:
				target = $"{arch}-apple-tvos{MinimumOSVersion}" + (SdkIsSimulator ? "-simulator" : "");
				break;
			case ApplePlatform.MacCatalyst:
				target = $"{arch}-apple-ios{MinimumOSVersion}-macabi";
				var iOSSupport = Path.Combine (SdkRoot, "System", "iOSSupport");
				platformArguments.AddRange ([
					"-Fsystem", Path.Combine (iOSSupport, "System", "Library", "Frameworks"),
					"-I", Path.Combine (iOSSupport, "usr", "lib", "swift"),
					"-Xcc", "-isystem", "-Xcc", Path.Combine (iOSSupport, "usr", "include"),
					"-Xcc", "-iframework", "-Xcc", Path.Combine (iOSSupport, "System", "Library", "Frameworks"),
				]);
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
			var dependencyFiles = new List<string> ();
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
				var dependencyFile = Path.GetFullPath (Path.Combine (OutputDirectory, moduleName + ".d"));
				var outputFileMap = Path.GetFullPath (Path.Combine (OutputDirectory, moduleName + ".output-file-map.json"));
				// The output file map tells swiftc where to write the (make-style) dependency file.
				File.WriteAllText (outputFileMap, $"{{ \"\": {{ \"dependencies\": \"{dependencyFile.Replace ("\\", "\\\\").Replace ("\"", "\\\"")}\" }} }}");
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
					"-emit-dependencies",
					"-output-file-map", outputFileMap,
				};
				arguments.AddRange (platformArguments);
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
				dependencyFiles.Add (dependencyFile);
			}

			System.Threading.Tasks.Task.WaitAll (processes.ToArray ());

			foreach (var dependencyFile in dependencyFiles)
				WriteDependencyList (dependencyFile);

			CompiledOutputFiles = outputFiles.ToArray ();

			return !Log.HasLoggedErrors;
		}

		// Convert the make-style dependency file from swiftc ('output: input1 input2 \<newline> input3 ...', where spaces
		// in paths are escaped with a backslash) into a list of files, one per line, which is easy to read from MSBuild.
		void WriteDependencyList (string dependencyFile)
		{
			if (!File.Exists (dependencyFile))
				return;

			var contents = File.ReadAllText (dependencyFile).Replace ("\\\n", " ");
			var dependencies = new List<string> ();
			var current = new System.Text.StringBuilder ();
			var foundTarget = false;
			for (var i = 0; i <= contents.Length; i++) {
				var c = i < contents.Length ? contents [i] : ' ';
				if (c == '\\' && i + 1 < contents.Length && contents [i + 1] == ' ') {
					current.Append (' ');
					i++;
				} else if (char.IsWhiteSpace (c)) {
					if (current.Length > 0) {
						var token = current.ToString ();
						if (foundTarget) {
							dependencies.Add (token);
						} else if (token.EndsWith (":", StringComparison.Ordinal)) {
							foundTarget = true;
						}
						current.Clear ();
					}
				} else {
					current.Append (c);
				}
			}

			File.WriteAllLines (Path.ChangeExtension (dependencyFile, ".dependencies"), dependencies.Distinct ());
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

		public bool ShouldCopyToBuildServer (ITaskItem item)
		{
			// Generated files (such as the Swift code for FSKit extensions) only exist on the Mac, and they
			// might be empty placeholder files on Windows. Don't overwrite the Mac's version with those.
			var finfo = new FileInfo (item.ItemSpec);
			return finfo.Exists && finfo.Length > 0;
		}

		public bool ShouldCreateOutputFile (ITaskItem item) => true;

		public IEnumerable<ITaskItem> GetAdditionalItemsToBeCopied ()
		{
			foreach (var header in Sources.Select (v => v.GetMetadata ("BridgingHeader")).Where (v => !string.IsNullOrEmpty (v)).Distinct ()) {
				var item = new TaskItem (header);
				if (ShouldCopyToBuildServer (item))
					yield return item;
			}
		}

		public void Cancel ()
		{
			if (ShouldExecuteRemotely ())
				BuildConnection.CancelAsync (BuildEngine4).Wait ();
		}
	}
}
