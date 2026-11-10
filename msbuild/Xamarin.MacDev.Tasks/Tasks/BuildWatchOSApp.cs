// Copyright (c) Microsoft Corporation.
// Licensed under the MIT License.

using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.Json;

using Microsoft.Build.Framework;
using Microsoft.Build.Utilities;

using Xamarin.Localization.MSBuild;
using Xamarin.Messaging.Build.Client;
using Xamarin.Utils;

#nullable enable

namespace Xamarin.MacDev.Tasks {
	public class BuildWatchOSApp : XcodeBuildTask, ITaskCallback {
		[Required]
		public string ProjectPath { get; set; } = "";

		public string SchemeName { get; set; } = "";
		public string Configuration { get; set; } = "Release";

		[Required]
		public string RuntimeIdentifier { get; set; } = "";

		public string [] InputExcludeDirectories { get; set; } = [];

		[Output]
		public string ArtifactDirectory { get; set; } = "";

		[Output]
		public string AppBundlePath { get; set; } = "";

		[Output]
		public ITaskItem [] FileWrites { get; set; } = [];

		protected override string Command { get; set; } = "build";

		public static bool IsSupportedRuntimeIdentifier (string rid)
		{
			return rid == "ios-arm64" || rid == "iossimulator-arm64";
		}

		protected override IList<string> GenerateCommandLineCommands ()
		{
			var simulator = RuntimeIdentifier.StartsWith ("iossimulator-", StringComparison.Ordinal);
			var args = new List<string> {
				"-project", Path.GetFullPath (ProjectPath),
				"-scheme", SchemeName,
				"-configuration", Configuration,
				"-sdk", simulator ? "watchsimulator" : "watchos",
				"-destination", simulator ? "generic/platform=watchOS Simulator" : "generic/platform=watchOS",
				"-derivedDataPath", Path.GetFullPath (Path.Combine (OutputPath, "DerivedData")),
				"CONFIGURATION_BUILD_DIR=" + Path.GetFullPath (Path.Combine (OutputPath, "Products")),
				"CODE_SIGNING_ALLOWED=NO",
				"DEBUG_INFORMATION_FORMAT=dwarf-with-dsym",
				"ONLY_ACTIVE_ARCH=NO",
			};
			if (simulator)
				args.Add ("ARCHS=arm64");
			return args;
		}

		public override bool Execute ()
		{
			if (ShouldExecuteRemotely ()) {
				ProjectPath = GetRemotePath (ProjectPath);
				return base.Execute ();
			}
			try {
				return ExecuteLocally ();
			} catch (Exception ex) when (ex is IOException || ex is UnauthorizedAccessException || ex is ArgumentException || ex is JsonException) {
				Log.LogError (7209, null, MSBStrings.E7209, ProjectPath, ex.Message);
				return false;
			}
		}

		bool ExecuteLocally ()
		{
			if (!IsSupportedRuntimeIdentifier (RuntimeIdentifier)) {
				Log.LogError (7205, null, MSBStrings.E7205, RuntimeIdentifier);
				return false;
			}
			if (!Directory.Exists (ProjectPath) || Path.GetExtension (ProjectPath) != ".xcodeproj") {
				Log.LogError (MSBStrings.XcodeBuild_InvalidItem, ProjectPath);
				return false;
			}
			if (string.IsNullOrEmpty (SchemeName)) {
				var schemes = ExecuteAsync ("xcrun", new [] { "xcodebuild", "-list", "-json", "-project", Path.GetFullPath (ProjectPath) }, cancellationToken: CancellationToken).Result;
				if (schemes.ExitCode != 0)
					return false;
				using var list = JsonDocument.Parse (schemes.Output.StandardOutput.ToString ());
				var names = list.RootElement.GetProperty ("project").GetProperty ("schemes").EnumerateArray ().Select (v => v.GetString ()).Distinct (StringComparer.Ordinal).ToArray ();
				if (names.Length != 1 || string.IsNullOrEmpty (names [0])) {
					Log.LogError (7206, null, MSBStrings.E7206, ProjectPath);
					return false;
				}
				SchemeName = names [0] ?? "";
			}
			if (!base.Execute ())
				return false;

			var args = new List<string> { "xcodebuild" };
			args.AddRange (GenerateCommandLineCommands ());
			args.Add ("-showBuildSettings");
			args.Add ("-json");
			var settingsResult = ExecuteAsync ("xcrun", args, cancellationToken: CancellationToken).Result;
			if (settingsResult.ExitCode != 0)
				return false;
			using var settings = JsonDocument.Parse (settingsResult.Output.StandardOutput.ToString ());
			var apps = settings.RootElement.EnumerateArray ()
				.Select (v => v.GetProperty ("buildSettings"))
				.Where (v => v.TryGetProperty ("WRAPPER_EXTENSION", out var ext) && ext.GetString () == "app" &&
					v.TryGetProperty ("PLATFORM_NAME", out var platform) && (platform.GetString () == "watchos" || platform.GetString () == "watchsimulator"))
				.ToArray ();
			if (apps.Length != 1) {
				Log.LogError (7207, null, MSBStrings.E7207, ProjectPath);
				return false;
			}

			ArtifactDirectory = Path.Combine (OutputPath, "artifact");
			if (Directory.Exists (ArtifactDirectory))
				Directory.Delete (ArtifactDirectory, true);
			Directory.CreateDirectory (ArtifactDirectory);
			var app = apps [0];
			var product = Path.Combine (app.GetProperty ("TARGET_BUILD_DIR").GetString () ?? "", app.GetProperty ("FULL_PRODUCT_NAME").GetString () ?? "");
			AppBundlePath = Path.Combine (ArtifactDirectory, Path.GetFileName (product));
			if (ExecuteAsync ("/usr/bin/ditto", new [] { product, AppBundlePath }, cancellationToken: CancellationToken).Result.ExitCode != 0)
				return false;

			foreach (var target in settings.RootElement.EnumerateArray ()) {
				var values = target.GetProperty ("buildSettings");
				if (!values.TryGetProperty ("PRODUCT_BUNDLE_IDENTIFIER", out var identifierValue))
					continue;
				var identifier = identifierValue.GetString () ?? "";
				if (identifier.Length == 0 || identifier.IndexOfAny (new [] { '/', '\\' }) >= 0)
					continue;
				if (values.TryGetProperty ("CODE_SIGN_ENTITLEMENTS", out var entitlementValue) && !string.IsNullOrEmpty (entitlementValue.GetString ())) {
					var sourceRoot = values.GetProperty ("SRCROOT").GetString () ?? "";
					var entitlements = Path.Combine (sourceRoot, entitlementValue.GetString () ?? "");
					var directory = Path.Combine (ArtifactDirectory, "Entitlements");
					Directory.CreateDirectory (directory);
					File.Copy (entitlements, Path.Combine (directory, identifier + ".plist"), true);
				}
				if (values.TryGetProperty ("DWARF_DSYM_FOLDER_PATH", out var symbolDirectory) && values.TryGetProperty ("DWARF_DSYM_FILE_NAME", out var symbolName)) {
					var symbols = Path.Combine (symbolDirectory.GetString () ?? "", symbolName.GetString () ?? "");
					if (Directory.Exists (symbols) && ExecuteAsync ("/usr/bin/ditto", new [] { symbols, Path.Combine (ArtifactDirectory, "dSYMs", Path.GetFileName (symbols)) }, cancellationToken: CancellationToken).Result.ExitCode != 0)
						return false;
				}
			}
			FileWrites = Directory.GetFiles (OutputPath, "*", SearchOption.AllDirectories).Select (v => new TaskItem (v)).ToArray ();
			return !Log.HasLoggedErrors;
		}

		public IEnumerable<ITaskItem> GetAdditionalItemsToBeCopied ()
		{
			var directory = Path.GetDirectoryName (ProjectPath);
			if (string.IsNullOrEmpty (directory))
				directory = ".";
			return CreateItemsForAllFilesRecursively (directory)
				.Where (item => !InputExcludeDirectories.Concat (new [] { OutputPath }).Any (directory => !string.IsNullOrEmpty (directory) && PathUtils.IsPathContained (directory, item.ItemSpec)));
		}

		public bool ShouldCopyToBuildServer (ITaskItem item) => true;
		public bool ShouldCreateOutputFile (ITaskItem item) => false;
	}
}
