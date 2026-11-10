// Copyright (c) Microsoft Corporation.
// Licensed under the MIT License.

using System;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;

using Microsoft.Build.Framework;
using Microsoft.Build.Utilities;

using Xamarin.Localization.MSBuild;

#nullable enable

namespace Xamarin.MacDev.Tasks {
	public class CreateWatchOSAppZip : XamarinTask {
		[Required]
		public string ProjectPath { get; set; } = "";

		[Required]
		public string ZipPath { get; set; } = "";

		[Required]
		public string IntermediateOutputPath { get; set; } = "";

		public string SchemeName { get; set; } = "";
		public string Configuration { get; set; } = "Release";
		public string [] RuntimeIdentifiers { get; set; } = new [] { "ios-arm64", "iossimulator-arm64" };

		[Output]
		public ITaskItem [] FileWrites { get; set; } = [];

		public override bool Execute ()
		{
			try {
				return ExecuteLocally ();
			} catch (Exception ex) when (ex is IOException || ex is UnauthorizedAccessException || ex is ArgumentException) {
				Log.LogError (7209, null, MSBStrings.E7209, ProjectPath, ex.Message);
				return false;
			}
		}

		bool ExecuteLocally ()
		{
			if (!RuntimeInformation.IsOSPlatform (OSPlatform.OSX) || ShouldExecuteRemotely ()) {
				Log.LogError (7208, null, MSBStrings.E7208);
				return false;
			}
			if (RuntimeIdentifiers.Length == 0 || RuntimeIdentifiers.Distinct ().Count () != RuntimeIdentifiers.Length || RuntimeIdentifiers.Any (v => !BuildWatchOSApp.IsSupportedRuntimeIdentifier (v))) {
				Log.LogError (7205, null, MSBStrings.E7205, string.Join (";", RuntimeIdentifiers));
				return false;
			}
			var package = Path.Combine (IntermediateOutputPath, "package");
			if (Directory.Exists (package))
				Directory.Delete (package, true);
			Directory.CreateDirectory (package);
			foreach (var rid in RuntimeIdentifiers) {
				var build = new BuildWatchOSApp {
					BuildEngine = BuildEngine,
					ProjectPath = ProjectPath,
					SchemeName = SchemeName,
					Configuration = Configuration,
					RuntimeIdentifier = rid,
					OutputPath = Path.Combine (IntermediateOutputPath, rid),
					SdkDevPath = SdkDevPath,
				};
				if (!build.Execute ())
					return false;
				if (ExecuteAsync ("/usr/bin/ditto", new [] { build.ArtifactDirectory, Path.Combine (package, rid) }).Result.ExitCode != 0)
					return false;
			}
			var zipDirectory = Path.GetDirectoryName (Path.GetFullPath (ZipPath));
			if (zipDirectory is null) {
				Log.LogError (7210, null, MSBStrings.E7210, ZipPath, string.Join (";", RuntimeIdentifiers));
				return false;
			}
			Directory.CreateDirectory (zipDirectory);
			var temporaryZip = Path.GetFullPath (ZipPath) + ".tmp";
			try {
				if (!CompressionHelper.TryCompress (this, temporaryZip, Directory.GetDirectories (package), true, Path.GetFullPath (package), false))
					return false;
				if (File.Exists (ZipPath))
					File.Delete (ZipPath);
				File.Move (temporaryZip, ZipPath);
			} finally {
				if (File.Exists (temporaryZip))
					File.Delete (temporaryZip);
			}
			FileWrites = Directory.GetFiles (IntermediateOutputPath, "*", SearchOption.AllDirectories).Concat (new [] { ZipPath }).Select (v => new TaskItem (v)).ToArray ();
			return !Log.HasLoggedErrors;
		}
	}
}
