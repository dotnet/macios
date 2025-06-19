using System;
using System.IO;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;

using Microsoft.Build.Framework;
using Microsoft.Build.Utilities;
using Xamarin.Localization.MSBuild;
using Xamarin.Messaging.Build.Client;
using Xamarin.Utils;

namespace Xamarin.MacDev.Tasks {
	public class CollectBundleResources : XamarinTask, ICancelableTask, IHasProjectDir, IHasResourcePrefix {
		#region Inputs

		public ITaskItem [] BundleResources { get; set; } = Array.Empty<ITaskItem> ();

		public bool OptimizePropertyLists { get; set; }

		public bool OptimizePNGs { get; set; }

		[Required]
		public string ProjectDir { get; set; } = string.Empty;

		[Required]
		public string ResourcePrefix { get; set; } = string.Empty;

		#endregion

		#region Outputs

		[Output]
		public ITaskItem [] BundleResourcesWithLogicalNames { get; set; } = Array.Empty<ITaskItem> ();

		public ITaskItem [] UnpackedResources { get; set; } = Array.Empty<ITaskItem> ();

		#endregion

		static bool CanOptimize (string path)
		{
			switch (Path.GetExtension (path).ToLowerInvariant ()) {
			case ".png": case ".plist": case ".strings": return true;
			default: return false;
			}
		}

		public override bool Execute ()
		{
			try {
				if (ShouldExecuteRemotely ()) {
					// Copy the bundle files to the build server
					new TaskRunner (SessionId, BuildEngine4).CopyInputsAsync (this).Wait ();
				}

				// But execute locally
				return ExecuteImpl ();
			} catch (PathTooLongException ptle) when (Environment.OSVersion.Platform == PlatformID.Win32NT && !PathUtils.OSSupportsLongPaths) {
				Log.LogError (MSBStrings.E7122 /* A path exceeding max path was detected. Enabling long paths in Windows may help. For more information see https://learn.microsoft.com/en-us/windows/win32/fileio/maximum-file-path-limitation. */);
				Log.LogErrorFromException (ptle); // report the original exception too.
				return false;
			} catch (Exception ex) {
				Log.LogErrorFromException (ex);

				return false;
			}
		}

		bool ExecuteImpl ()
		{
			var bundleResources = new List<ITaskItem> ();

			foreach (var item in BundleResources) {
				if (!TryCreateItemWithLogicalName (this, item, out var bundleResource))
					continue;

				bool optimize = false;

				if (CanOptimize (item.ItemSpec)) {
					var metadata = item.GetMetadata ("Optimize");

					// fall back to old metadata name
					if (string.IsNullOrEmpty (metadata))
						metadata = item.GetMetadata ("OptimizeImage");

					if (string.IsNullOrEmpty (metadata) || !bool.TryParse (metadata, out optimize)) {
						switch (Path.GetExtension (item.ItemSpec).ToLowerInvariant ()) {
						case ".plist": case ".strings": optimize = OptimizePropertyLists; break;
						case ".png": optimize = OptimizePNGs; break;
						}
					}
				}

				bundleResource.SetMetadata ("Optimize", optimize.ToString ());

				bundleResources.Add (bundleResource);
			}

			bundleResources.AddRange (UnpackedResources);

			var distinctBundleResources = VerifyLogicalNameUniqueness (Log, bundleResources, "BundleResource");

			BundleResourcesWithLogicalNames = distinctBundleResources.ToArray ();

			return !Log.HasLoggedErrors;
		}

		[return: NotNullIfNotNull (nameof (items))]
		public static ITaskItem []? VerifyLogicalNameUniqueness (TaskLoggingHelper Log, IEnumerable<ITaskItem>? items, string itemName)
		{
			if (items is null)
				return null;

			var rv = new List<ITaskItem> ();
			var groupedBundleResources = items.GroupBy (item => item.GetMetadata ("LogicalName"));
			var reportedItems = new HashSet<string> (); // Keep track of items we've shown warnings for, to not show multiple warnings.

			foreach (var group in groupedBundleResources) {
				// No/empty LogicalName is not OK.
				if (string.IsNullOrEmpty (group.Key)) {
					foreach (var item in group)
						Log.LogError (7157, item.ItemSpec, MSBStrings.E7157 /* The {0} item '{0}' does not have a 'LogicalName' metadata. */, itemName, item.ItemSpec);
					continue;
				}

				// One item per LogicalName is OK.
				if (group.Count () == 1) {
					rv.AddRange (group);
					continue;
				}

				// More than one item per LogicalName is not good at all.
				var notBundledInAssembly = group.Where (item => string.IsNullOrEmpty (item.GetMetadata ("BundledInAssembly"))).ToArray ();
				var bundledInAssembly = group.Where (item => !string.IsNullOrEmpty (item.GetMetadata ("BundledInAssembly"))).ToArray ();
				if (notBundledInAssembly.Length == 1) {
					// Only one not from a library
					rv.AddRange (notBundledInAssembly);
					// warn about ignoring all the other imported ones.
					foreach (var item in bundledInAssembly) {
						if (reportedItems.Add (item.ItemSpec)) {
							Log.LogWarning (7154, item.ItemSpec, MSBStrings.W7154 /* The {0} item '{1}' imported from '{2}' was ignored, because there's already an existing item from the current project with the same LogicalName ('{3}'). */, itemName, item.ItemSpec, item.GetMetadata ("BundledInAssembly"), group.Key);
						}
					}
					continue;
				} else if (notBundledInAssembly.Length == 0) {
					// none from the current assembly, but multiple imported ones. Don't add any of them (to have a predictable build).
					// warn about ignoring all the other ones
					foreach (var item in bundledInAssembly) {
						if (reportedItems.Add (item.ItemSpec)) {
							var others = bundledInAssembly.
											Where (i => !object.ReferenceEquals (i, item)).
											Select (i => Path.GetFileName (i.GetMetadata ("BundledInAssembly"))).
											ToArray ();
							Log.LogWarning (7155, item.ItemSpec, MSBStrings.W7155 /* The {0} item '{1}' imported from '{2}' was ignored, because there's another item from a different assembly ({4}) with the same LogicalName ('{3}'). */, itemName, item.ItemSpec, item.GetMetadata ("BundledInAssembly"), group.Key, string.Join (", ", others));
						}
					}
					continue;
				} else {
					// more than one for the current project?
					// don't add any of them (to have a predictable build).
					// warn about them all.
					foreach (var item in notBundledInAssembly) {
						if (reportedItems.Add (item.ItemSpec)) {
							Log.LogWarning (7156, item.ItemSpec, MSBStrings.W7156 /* The {0} item '{1}' was ignored, because there's another item with the same LogicalName ('{2}'). */, itemName, item.ItemSpec, group.Key);
						}
					}
				}
			}

			return rv.ToArray ();
		}

		[return: NotNullIfNotNull (nameof (items))]
		public static ITaskItem []? ComputeLogicalNameAndDetectDuplicates<U> (U task, IEnumerable<ITaskItem>? items, string projectDir, string resourcePrefix, string itemName) where U : Task, IHasProjectDir, IHasResourcePrefix, IHasSessionId
		{
			if (items is null)
				return null;

			var prefixes = BundleResource.SplitResourcePrefixes (resourcePrefix);
			foreach (var item in items) {
				var logicalName = BundleResource.GetLogicalName (task, item);
				item.SetMetadata ("LogicalName", logicalName);
			}
			return VerifyLogicalNameUniqueness (task.Log, items, itemName);
		}

		public static bool TryCreateItemWithLogicalName<T> (T task, ITaskItem item, [NotNullWhen (true)] out TaskItem? itemWithLogicalName) where T : Task, IHasProjectDir, IHasResourcePrefix, IHasSessionId
		{
			itemWithLogicalName = null;

			// Skip anything with the PublishFolderType metadata, these are copied directly to the ResolvedFileToPublish item group instead.
			var publishFolderType = item.GetMetadata ("PublishFolderType");
			if (!string.IsNullOrEmpty (publishFolderType))
				return false;

			var logicalName = BundleResource.GetLogicalName (task, item);
			// We need a physical path here, ignore the Link element
			var path = item.GetMetadata ("FullPath");

			if (!File.Exists (path)) {
				task.Log.LogError (MSBStrings.E0099, logicalName, path);
				return false;
			}

			if (logicalName.StartsWith (".." + Path.DirectorySeparatorChar, StringComparison.Ordinal)) {
				task.Log.LogError (null, null, null, item.ItemSpec, 0, 0, 0, 0, MSBStrings.E0100, logicalName);
				return false;
			}

			if (logicalName == "Info.plist") {
				task.Log.LogWarning (null, null, null, item.ItemSpec, 0, 0, 0, 0, MSBStrings.E0101);
				return false;
			}

			if (BundleResource.IsIllegalName (logicalName, out var illegal)) {
				task.Log.LogError (null, null, null, item.ItemSpec, 0, 0, 0, 0, MSBStrings.E0102, illegal);
				return false;
			}

			itemWithLogicalName = new TaskItem (item);
			itemWithLogicalName.SetMetadata ("LogicalName", logicalName);
			return true;
		}

		public void Cancel ()
		{
			if (ShouldExecuteRemotely ())
				BuildConnection.CancelAsync (BuildEngine4).Wait ();
		}
	}
}
