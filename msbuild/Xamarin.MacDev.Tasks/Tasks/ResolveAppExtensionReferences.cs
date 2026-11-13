// Copyright (c) Microsoft Corporation.
// Licensed under the MIT License.

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
	public class ResolveAppExtensionReferences : XamarinTask, ITaskCallback {
		[Required]
		public string AppBundlePath { get; set; } = "";

		public ITaskItem [] AppExtensionReferences { get; set; } = [];

		[Output]
		public ITaskItem [] ResolvedAppExtensionReferences { get; set; } = [];

		public override bool Execute ()
		{
			if (ShouldExecuteRemotely ()) {
				var references = AppExtensionReferences.Select (v => new TaskItem (v)).ToArray ();
				if (!ExecuteRemotely (out var _, runner => runner.FixReferencedItems (this, AppExtensionReferences)))
					return false;

				for (var i = 0; i < references.Length; i++) {
					// Only placement metadata crosses back: signing imports still need the Windows paths.
					references [i].SetMetadata ("ContainerName", ResolvedAppExtensionReferences [i].GetMetadata ("ContainerName"));
					references [i].SetMetadata ("BundleRelativePath", ResolvedAppExtensionReferences [i].GetMetadata ("BundleRelativePath"));
				}
				ResolvedAppExtensionReferences = references;
				return true;
			}

			var resolved = new List<ITaskItem> ();
			foreach (var reference in AppExtensionReferences) {
				var item = new TaskItem (reference);
				var container = "XPCServices";
				if (Path.GetExtension (item.ItemSpec) != ".xpc") {
					var manifestPath = PlatformFrameworkHelper.GetAppManifestPath (Platform, item.ItemSpec);
					PDictionary plist;
					try {
						plist = PDictionary.OpenFile (manifestPath);
					} catch (Exception ex) {
						Log.LogError (MSBStrings.E0010, manifestPath, ex.Message);
						continue;
					}

					var legacy = plist.Get<PDictionary> ("NSExtension");
					var extensionKit = plist.Get<PDictionary> ("EXAppExtensionAttributes");
					if ((legacy is null) == (extensionKit is null)) {
						Log.LogError (7236, manifestPath, MSBStrings.E7236, manifestPath);
						continue;
					}

					container = extensionKit is null ? "PlugIns" : "Extensions";
					item.SetMetadata ("ManifestPath", manifestPath);
				}

				item.SetMetadata ("ContainerName", container);
				var relativePath = Platform == ApplePlatform.MacOSX || Platform == ApplePlatform.MacCatalyst
					? Path.Combine ("Contents", container)
					: container;
				item.SetMetadata ("BundleRelativePath", relativePath + Path.DirectorySeparatorChar);
				if (Directory.Exists (item.ItemSpec)) {
					var files = Directory.GetFiles (item.ItemSpec, "*", SearchOption.AllDirectories);
					Array.Sort (files, StringComparer.Ordinal);
					item.SetMetadata ("InputFiles", string.Join (";", files));
					var destination = Path.Combine (AppBundlePath, relativePath, Path.GetFileName (item.ItemSpec));
					var sourcePrefix = item.ItemSpec.TrimEnd (Path.DirectorySeparatorChar) + Path.DirectorySeparatorChar;
					var outputs = files.Select (v => v.Substring (sourcePrefix.Length))
						.Where (v => !IsRemovedAfterCopy (v))
						.Select (v => Path.Combine (destination, v));
					item.SetMetadata ("OutputFiles", string.Join (";", outputs));
				}
				resolved.Add (item);
			}

			ResolvedAppExtensionReferences = resolved.ToArray ();
			return !Log.HasLoggedErrors;
		}

		public bool ShouldCopyToBuildServer (ITaskItem item) => false;

		public bool ShouldCreateOutputFile (ITaskItem item) => false;

		static bool IsRemovedAfterCopy (string relativePath)
		{
			var components = relativePath.Split (Path.DirectorySeparatorChar);
			for (var i = 0; i < components.Length; i++) {
				if (components [i] == "_CodeSignature")
					return true;
				if (!components [i].EndsWith (".framework", StringComparison.OrdinalIgnoreCase))
					continue;
				var directory = i + 1;
				if (directory + 2 < components.Length && components [directory] == "Versions")
					directory += 2;
				if (directory < components.Length && (components [directory] == "Headers" || components [directory] == "Modules" || components [directory] == "PrivateHeaders"))
					return true;
			}
			return false;
		}

		public IEnumerable<ITaskItem> GetAdditionalItemsToBeCopied ()
		{
			return AppExtensionReferences
				.Where (v => v.GetMetadata ("IsAdditionalAppExtension") == "true" && Directory.Exists (v.ItemSpec))
				.SelectMany (v => Directory.GetFiles (v.ItemSpec, "*", SearchOption.AllDirectories))
				.Select (v => new TaskItem (v));
		}
	}
}
