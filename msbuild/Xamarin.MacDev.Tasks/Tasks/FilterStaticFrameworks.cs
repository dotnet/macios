#nullable enable

using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

using Microsoft.Build.Framework;
using Microsoft.Build.Utilities;

using Xamarin.Localization.MSBuild;
using Xamarin.Messaging.Build.Client;
using Xamarin.Utils;

namespace Xamarin.MacDev.Tasks {
	// This task takes an itemgroup of frameworks, and filters out frameworks that aren't dynamic libraries.
	public class FilterStaticFrameworks : XamarinTask, ITaskCallback {
		public bool OnlyFilterFrameworks { get; set; }

		[Output]
		public ITaskItem []? FrameworkToPublish { get; set; }

		static string GetFrameworkExecutablePath (string frameworkPath, TaskLoggingHelper? log = null)
		{
			if (!(frameworkPath.EndsWith (".framework", StringComparison.OrdinalIgnoreCase) && Directory.Exists (frameworkPath)))
				return frameworkPath;

			// Try to read the CFBundleExecutable from Info.plist
			// Check multiple locations where Info.plist might be located
			var infoPlistPaths = new string [] {
				Path.Combine (frameworkPath, "Info.plist"), // iOS, tvOS
				Path.Combine (frameworkPath, "Resources", "Info.plist") // macOS, MacCatalyst
			};

			foreach (var infoPlistPath in infoPlistPaths) {
				if (File.Exists (infoPlistPath)) {
					try {
						var plist = PDictionary.FromFile (infoPlistPath);
						if (plist is not null) {
							var bundleExecutable = plist.GetCFBundleExecutable ();
							if (!string.IsNullOrEmpty (bundleExecutable)) {
								return Path.Combine (frameworkPath, bundleExecutable);
							}
						}
					} catch (Exception ex) {
						// Log exceptions from malformed plist files and fall back to default behavior
						log?.LogMessage (MessageImportance.Low, $"Failed to parse Info.plist for framework '{frameworkPath}': {ex.Message}");
					}
					break; // Found Info.plist but no CFBundleExecutable, stop looking
				}
			}

			// Fall back to the default assumption: framework name without extension
			return Path.Combine (frameworkPath, Path.GetFileNameWithoutExtension (frameworkPath));
		}

		public override bool Execute ()
		{
			if (ShouldExecuteRemotely ())
				return new TaskRunner (SessionId, BuildEngine4).RunAsync (this).Result;

			if (FrameworkToPublish is not null && FrameworkToPublish.Length > 0) {
				var list = FrameworkToPublish.ToList ();
				for (var i = list.Count - 1; i >= 0; i--) {
					var item = list [i];
					var frameworkExecutablePath = PathUtils.ConvertToMacPath (item.ItemSpec);
					try {
						if (frameworkExecutablePath.EndsWith (".framework", StringComparison.OrdinalIgnoreCase) && Directory.Exists (frameworkExecutablePath)) {
							frameworkExecutablePath = GetFrameworkExecutablePath (frameworkExecutablePath, Log);
						}

						if (OnlyFilterFrameworks && !Path.GetDirectoryName (frameworkExecutablePath).EndsWith (".framework", StringComparison.OrdinalIgnoreCase)) {
							Log.LogMessage (MessageImportance.Low, $"Skipped processing {item.ItemSpec} because it's not a framework");
							continue;
						}

						if (!File.Exists (frameworkExecutablePath)) {
							Log.LogError (158, frameworkExecutablePath, MSBStrings.E0158 /* The file '{0}' does not exist. */, frameworkExecutablePath);
							continue;
						}

						if (MachO.IsDynamicFramework (frameworkExecutablePath))
							continue;
					} catch (Exception e) {
						Log.LogError (7091, frameworkExecutablePath, MSBStrings.E7092 /* File '{0}' is not a valid framework: {1} */, frameworkExecutablePath, e.Message);
						continue;
					}

					Log.LogMessage (MessageImportance.Low, MSBStrings.W7091 /* "The framework {0} is a framework of static libraries, and will not be copied to the app." */, Path.GetDirectoryName (frameworkExecutablePath));
					list.RemoveAt (i);
				}

				// Copy back the list if anything was removed from it
				if (FrameworkToPublish.Length != list.Count)
					FrameworkToPublish = list.ToArray ();
			}

			return !Log.HasLoggedErrors;
		}

		public bool ShouldCopyToBuildServer (ITaskItem item) => true;

		public bool ShouldCreateOutputFile (ITaskItem item) => false;

		public IEnumerable<ITaskItem> GetAdditionalItemsToBeCopied ()
		{
			if (FrameworkToPublish is not null) {
				foreach (var item in FrameworkToPublish) {
					var fw = item.ItemSpec;
					// Copy all the files from the framework to the mac (copying only the executable won't work if it's just a symlink to elsewhere)
					if (File.Exists (fw))
						fw = Path.GetDirectoryName (fw);
					if (!Directory.Exists (fw))
						continue;
					foreach (var file in Directory.EnumerateFiles (fw, "*.*", SearchOption.AllDirectories)) {
						yield return new TaskItem (file);
					}
				}
			}
		}
	}
}
