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
	// Compiles Swift source files into a single object file, which can be linked into the app's executable.
	public class CompileSwift : XamarinTask, ICancelableTask, ITaskCallback {
		#region Inputs
		public string BridgingHeader { get; set; } = "";

		public bool IsAppExtension { get; set; }

		[Required]
		public string MinimumOSVersion { get; set; } = "";

		[Required]
		public string ModuleName { get; set; } = "";

		[Required]
		public string OutputFile { get; set; } = "";

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
		// This is an output so that an (empty) placeholder file is created on Windows for remote builds,
		// otherwise the target that compiles the Swift code (and the native link) would always run.
		[Output]
		public ITaskItem? CompiledOutputFile { get; set; }
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

			var outputFile = Path.GetFullPath (OutputFile);
			var outputDirectory = Path.GetDirectoryName (outputFile);
			if (!string.IsNullOrEmpty (outputDirectory))
				Directory.CreateDirectory (outputDirectory);

			var arguments = new List<string> {
				"swiftc",
				"-c",
				"-g",
				"-parse-as-library",
				"-whole-module-optimization",
				"-swift-version", "5",
				"-module-name", ModuleName,
				"-sdk", SdkRoot,
				"-target", target,
			};
			if (IsAppExtension)
				arguments.Add ("-application-extension");
			if (!string.IsNullOrEmpty (BridgingHeader)) {
				arguments.Add ("-import-objc-header");
				arguments.Add (Path.GetFullPath (BridgingHeader));
			}
			arguments.Add ("-o");
			arguments.Add (outputFile);
			arguments.AddRange (Sources.Select (v => Path.GetFullPath (v.ItemSpec)));

			ExecuteAsync ("xcrun", arguments).Wait ();

			CompiledOutputFile = new TaskItem (OutputFile);

			return !Log.HasLoggedErrors;
		}

		public bool ShouldCopyToBuildServer (ITaskItem item) => true;

		public bool ShouldCreateOutputFile (ITaskItem item) => true;

		public IEnumerable<ITaskItem> GetAdditionalItemsToBeCopied ()
		{
			if (!string.IsNullOrEmpty (BridgingHeader))
				yield return new TaskItem (BridgingHeader);
		}

		public void Cancel ()
		{
			if (ShouldExecuteRemotely ())
				BuildConnection.CancelAsync (BuildEngine4).Wait ();
		}
	}
}
