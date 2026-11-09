using System;
using System.IO;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

using Microsoft.Build.Framework;
using Microsoft.Build.Globbing;
using Microsoft.Build.Utilities;

using Xamarin.Localization.MSBuild;
using Xamarin.Messaging.Build.Client;
using Xamarin.Utils;

#nullable enable

namespace Xamarin.MacDev.Tasks {
	//
	// This task is responsible for computing everything we need to know for code
	// signing.
	//
	// For each app bundle to be signed, a few more items to be code signed:
	//
	// * All *.dylib and *.metallib files
	// * All *.framework directories
	// * All *.xpc directories
	//
	// In both cases we iterate over what we find in the app bundle instead of
	// relying on what the msbuild tasks built and copied in the app bundle,
	// because customer logic might have added additional frameworks, and those
	// need to be signed too.
	//
	// This task will also figure out a stamp file path we use to determine if
	// something needs (re-)signing.
	//
	public class ComputeCodesignItems : XamarinTask, ITaskCallback, ICancelableTask {

		[Required]
		public string AppBundleDir { get; set; } = string.Empty;

		[Required]
		public ITaskItem [] CodesignBundle { get; set; } = Array.Empty<ITaskItem> ();

		[Required]
		public ITaskItem [] CodesignItems { get; set; } = Array.Empty<ITaskItem> ();

		[Required]
		public string CodesignStampPath { get; set; } = string.Empty;

		public ITaskItem [] GenerateDSymItems { get; set; } = Array.Empty<ITaskItem> ();

		public ITaskItem [] NativeStripItems { get; set; } = Array.Empty<ITaskItem> ();

		[Output]
		public ITaskItem [] OutputCodesignItems { get; set; } = Array.Empty<ITaskItem> ();

		public ITaskItem [] SkipCodesignItems { get; set; } = Array.Empty<ITaskItem> ();

		public override bool Execute ()
		{
			if (ShouldExecuteRemotely ())
				return ExecuteRemotely ();

			var output = new List<ITaskItem> ();

			// Make sure AppBundleDir has a trailing slash
			var appBundlePath = PathUtils.EnsureTrailingSlash (Path.GetFullPath (AppBundleDir)!);
			var appBundleStampPath = Path.Combine (CodesignStampPath, Path.GetFileName (appBundlePath.TrimEnd (Path.DirectorySeparatorChar)));
			var codesignItems = ExpandCodesignItems (appBundlePath, appBundleStampPath);
			if (Log.HasLoggedErrors)
				return false;
			var explicitPaths = new HashSet<string> (codesignItems.Select (v => PathUtils.ResolveSymbolicLinks (Path.GetFullPath (v.ItemSpec))), StringComparer.Ordinal);

			// Add the app bundles themselves
			foreach (var bundle in CodesignBundle) {
				// An app bundle is signed if either 'EnableCodeSigning' is true
				// or a 'CodesignSigningKey' has been provided.
				var enableCodeSigning = bundle.GetMetadata ("EnableCodeSigning");
				if (string.IsNullOrEmpty (enableCodeSigning))
					enableCodeSigning = bundle.GetMetadata ("RequireCodeSigning");
				var codesignSigningKey = bundle.GetMetadata ("CodesignSigningKey");
				if (!string.Equals (enableCodeSigning, "true", StringComparison.OrdinalIgnoreCase) && string.IsNullOrEmpty (codesignSigningKey))
					continue;

				// Create a new item for the app bundle, and copy any metadata over.
				var bundlePath = Path.Combine (Path.GetDirectoryName (AppBundleDir)!, bundle.ItemSpec);
				var item = new TaskItem (bundlePath);
				bundle.CopyMetadataTo (item);

				// Compute the stamp file to use
				item.SetMetadataIfNotSet ("CodesignStampFile", Path.Combine (CodesignStampPath, Path.GetFileName (bundle.ItemSpec), ".stampfile"));

				// Get any additional stamp files we must touch when the item is signed.
				var additionalStampFiles = new List<string> ();
				// We must touch the dSYM directory's Info.plist, to ensure that we don't end up running dsymutil again after codesigning in the next build
				var generateDSymItem = GenerateDSymItems.FirstOrDefault (v => {
					return string.Equals (Path.Combine (Path.GetDirectoryName (AppBundleDir)!, Path.GetDirectoryName (v.ItemSpec)!), item.ItemSpec, StringComparison.OrdinalIgnoreCase);
				});
				if (generateDSymItem is not null)
					additionalStampFiles.Add (generateDSymItem.GetMetadata ("dSYMUtilStampFile"));
				// We must touch the stamp file for native stripping, to ensure that we don't want to run strip again after codesigning in the next build
				var nativeStripItem = NativeStripItems.FirstOrDefault (v => string.Equals (Path.Combine (Path.GetDirectoryName (AppBundleDir)!, Path.GetDirectoryName (v.ItemSpec)!), item.ItemSpec, StringComparison.OrdinalIgnoreCase));
				if (nativeStripItem is not null)
					additionalStampFiles.Add (nativeStripItem.GetMetadata ("StripStampFile"));
				// Set the CodesignAdditionalFilesToTouch metadata
				if (additionalStampFiles.Count > 0) {
					additionalStampFiles.AddRange (item.GetMetadata ("CodesignAdditionalFilesToTouch").Split (';'));
					additionalStampFiles.RemoveAll (v => string.IsNullOrEmpty (v));
					item.SetMetadata ("CodesignAdditionalFilesToTouch", string.Join (";", additionalStampFiles));
				}

				output.Add (item);
			}

			// Find all:
			//	- *.dylib and *.metallib files
			//	- *.framework directories
			//  - *.xpc directories
			foreach (var bundle in CodesignBundle) {
				var bundlePath = Path.Combine (Path.GetDirectoryName (Path.GetDirectoryName (appBundlePath)!)!, bundle.ItemSpec);
				var filesToSign = FindFilesToSign (bundlePath);
				foreach (var lib in filesToSign) {
					if (explicitPaths.Contains (PathUtils.ResolveSymbolicLinks (Path.GetFullPath (lib))))
						continue;
					var pathInBundle = lib.Substring (appBundlePath.Length);
					var relativeLib = Path.Combine (AppBundleDir, pathInBundle);
					var stampFile = Path.Combine (appBundleStampPath, pathInBundle);
					var item = new TaskItem (relativeLib);
					bundle.CopyMetadataTo (item);

					// These items must not use the entitlements for the app
					item.RemoveMetadata ("CodesignEntitlements");

					// These files are a bit special, because they're always signed. This is done
					// by setting the signing key to '-' if it's not set.
					item.SetMetadataIfNotSet ("CodesignSigningKey", "-");

					// Set the stamp file even if already set (because any existing values would be copied from
					// the bundle, which would be the wrong stamp file, so it must be overridden)
					if (Directory.Exists (relativeLib)) {
						item.SetMetadata ("CodesignStampFile", Path.Combine (stampFile, ".stampfile"));
					} else {
						item.SetMetadata ("CodesignStampFile", stampFile);
					}

					output.Add (item);
				}
			}

			// Add all additional items
			foreach (var item in codesignItems) {
				// Set the stamp file if not already set.
				item.SetMetadataIfNotSet ("CodesignStampFile", Path.Combine (CodesignStampPath, item.ItemSpec));

				output.Add (item);
			}

			// Remove any items we were asked to remove
			RemoveFilesToNotSign (output, appBundlePath);

			// We may be asked to sign the same item multiple times (in particular for universal .NET builds)
			// Here we de-duplicate (based on itemspec). We also verify that the metadata is the same between
			// all deduplicated items, and if not, we show a warning.
			var grouped = output.GroupBy (v => PathUtils.ResolveSymbolicLinks (Path.GetFullPath (v.ItemSpec)), StringComparer.Ordinal);
			foreach (var group in grouped) {
				if (group.Count () < 2)
					continue;

				var all = group.ToArray ();
				var firstMetadata = all [0].CloneCustomMetadataToDictionary ();
				for (var i = 1; i < all.Length; i++) {
					var nextMetadata = all [i].CloneCustomMetadataToDictionary ();
					if (nextMetadata.Count != firstMetadata.Count) {
						Log.LogWarning (MSBStrings.W7095, /* Code signing has been requested multiple times for '{0}', with different metadata. The metadata for one are: '{1}', while the metadata for the other are: '{2}' */ all [0].ItemSpec, string.Join (", ", firstMetadata.Keys.OrderBy (v => v)), string.Join (", ", nextMetadata.Keys.OrderBy (v => v)));
					} else {
						foreach (var kvp in firstMetadata) {
							if (!nextMetadata.TryGetValue (kvp.Key, out var nextValue)) {
								Log.LogWarning (MSBStrings.W7096, /* Code signing has been requested multiple times for '{0}', with different metadata. The metadata '{1}={2}' has been set for one item, but not the other. */ all [0].ItemSpec, kvp.Key, kvp.Value);
								continue;
							}
							if (nextValue != kvp.Value) {
								Log.LogWarning (MSBStrings.W7097, /* Code signing has been requested multiple times for '{0}', with different metadata. The metadata '{1}' has different values for each item (once it's '{2}', another time it's '{3}'). */ all [0].ItemSpec, kvp.Key, kvp.Value, nextValue);
								continue;
							}
						}
					}
					output.Remove (all [i]);
				}
			}

			OutputCodesignItems = output.ToArray ();

			return !Log.HasLoggedErrors;
		}

		List<ITaskItem> ExpandCodesignItems (string appBundlePath, string appBundleStampPath)
		{
			var output = new List<ITaskItem> ();
			var explicitPaths = new HashSet<string> (StringComparer.Ordinal);
			foreach (var item in CodesignItems.OrderBy (v => string.IsNullOrEmpty (v.GetMetadata ("FilePattern")) ? 0 : 1)) {
				var owner = item.GetMetadata ("CodesignItemAppBundle");
				if (string.IsNullOrEmpty (owner)) {
					output.Add (item);
					continue;
				}

				var path = PathUtils.ConvertToMacPath (item.GetMetadata ("CodesignItemPath"));
				var pattern = PathUtils.ConvertToMacPath (item.GetMetadata ("FilePattern"));
				var ownerPath = Path.Combine (Path.GetDirectoryName (AppBundleDir)!, PathUtils.ConvertToMacPath (owner));
				var canonicalOwner = PathUtils.EnsureTrailingSlash (PathUtils.ResolveSymbolicLinks (Path.GetFullPath (ownerPath)));
				if (!string.Equals (canonicalOwner, PathUtils.EnsureTrailingSlash (PathUtils.ResolveSymbolicLinks (appBundlePath)), StringComparison.Ordinal) && !ValidateCodesignPath (appBundlePath, ownerPath))
					continue;
				if (!IsRelativeCodesignPath (path) || (!string.IsNullOrEmpty (pattern) && !IsRelativeCodesignPath (pattern))) {
					Log.LogError (MSBStrings.E7229, item.GetMetadata ("CodesignItemPath"), item.GetMetadata ("FilePattern"));
					continue;
				}

				path = Path.Combine (ownerPath, path);
				if (!ValidateCodesignPath (ownerPath, path))
					continue;

				IEnumerable<string> files;
				if (string.IsNullOrEmpty (pattern)) {
					if (!File.Exists (path)) {
						Log.LogError (MSBStrings.E7230, path);
						continue;
					}
					files = new [] { path };
				} else {
					if (!Directory.Exists (path)) {
						Log.LogError (MSBStrings.E7231, path);
						continue;
					}
					var glob = MSBuildGlob.Parse (Path.GetFullPath (path), pattern);
					if (!glob.IsLegal) {
						Log.LogError (MSBStrings.E7234, item.GetMetadata ("FilePattern"));
						continue;
					}
					files = EnumerateCodesignFiles (ownerPath, path).Where (glob.IsMatch);
				}

				foreach (var file in files) {
					MachOFile [] slices;
					try {
						slices = MachO.IsMachOFile (file) ? MachO.Read (file).ToArray () : [];
					} catch (Exception ex) when (ex is IOException || ex is UnauthorizedAccessException || ex is Xamarin.Bundler.ProductException) {
						Log.LogError (MSBStrings.E7235, file, ex.Message);
						continue;
					}
					if (slices.Length == 0 || !slices.All (v => v.IsExecutable || v.IsDynamicLibrary || v.IsBundle)) {
						if (string.IsNullOrEmpty (pattern))
							Log.LogError (MSBStrings.E7232, file);
						continue;
					}

					var canonicalPath = PathUtils.ResolveSymbolicLinks (Path.GetFullPath (file));
					// Explicit files take precedence over directory discovery, even through symlinks.
					if (!string.IsNullOrEmpty (pattern) && explicitPaths.Contains (canonicalPath))
						continue;
					if (string.IsNullOrEmpty (pattern))
						explicitPaths.Add (canonicalPath);
					var result = new TaskItem (canonicalPath);
					item.CopyMetadataTo (result);
					result.RemoveMetadata ("CodesignItemAppBundle");
					result.RemoveMetadata ("CodesignItemPath");
					result.RemoveMetadata ("FilePattern");
					result.RemoveMetadata ("OriginalItemSpec");
					var relativePath = PathUtils.AbsoluteToRelative (PathUtils.ResolveSymbolicLinks (appBundlePath), canonicalPath);
					result.SetMetadata ("CodesignStampFile", Path.Combine (appBundleStampPath, relativePath));
					output.Add (result);
				}
			}
			return output;
		}

		static bool IsRelativeCodesignPath (string path)
		{
			return !string.IsNullOrEmpty (path) && !Path.IsPathRooted (path) && path.IndexOf (':') < 0 && !path.Split ('/').Contains ("..");
		}

		bool ValidateCodesignPath (string ownerPath, string path)
		{
			if (PathUtils.IsPathContained (ownerPath, path))
				return true;
			Log.LogError (MSBStrings.E7233, path, ownerPath);
			return false;
		}

		IEnumerable<string> EnumerateCodesignFiles (string ownerPath, string directory)
		{
			foreach (var entry in Directory.EnumerateFileSystemEntries (directory)) {
				if (!ValidateCodesignPath (ownerPath, entry))
					continue;
				if (Directory.Exists (entry)) {
					if (PathUtils.IsSymlink (entry))
						continue;
					foreach (var file in EnumerateCodesignFiles (ownerPath, entry))
						yield return file;
				} else {
					yield return entry;
				}
			}
		}

		string CodeSignatureRelativePath {
			get {
				switch (Platform) {
				case ApplePlatform.iOS:
				case ApplePlatform.TVOS:
					return string.Empty;
				case ApplePlatform.MacOSX:
				case ApplePlatform.MacCatalyst:
					return "Contents";
				default:
					throw new InvalidOperationException (string.Format (MSBStrings.InvalidPlatform, Platform));
				}
			}
		}

		void RemoveFilesToNotSign (List<ITaskItem> output, string appBundlePath)
		{
			if (SkipCodesignItems.Length == 0)
				return;

			// Canonicalize the paths and split into files and directories
			var canonicalizedItemsToSkip = SkipCodesignItems
				.Select (v => Path.Combine (Path.GetDirectoryName (appBundlePath)!, v.ItemSpec))
				.ToArray ();
			var canonicalizedDirectoriesToSkip = canonicalizedItemsToSkip
				.Where (v => Directory.Exists (v))
				.Select (v => PathUtils.ResolveSymbolicLinks (v))
				.Select (v => PathUtils.EnsureTrailingSlash (v))
				.ToArray ();
			var canonicalizedFilesToSkip = canonicalizedItemsToSkip
				.Where (File.Exists)
				.Select (PathUtils.ResolveSymbolicLinks)
				.ToArray ();

			for (var i = output.Count - 1; i >= 0; i--) {
				var item = output [i];
				var outputPath = Path.GetFullPath (item.ItemSpec); // item.ItemSpec is relative to the project directory
				outputPath = PathUtils.ResolveSymbolicLinks (outputPath); // Canonicalize

				if (canonicalizedFilesToSkip.Contains (outputPath)) {
					Log.LogMessage (MessageImportance.Low, $"Not signing '{output [i].ItemSpec}' because it's in the list of files that skips signing");
					output.RemoveAt (i);
					continue;
				}

				if (Directory.Exists (outputPath))
					outputPath = PathUtils.EnsureTrailingSlash (outputPath);

				var matchingDirectory = canonicalizedDirectoriesToSkip.FirstOrDefault (v => outputPath.StartsWith (v, StringComparison.OrdinalIgnoreCase));
				if (matchingDirectory is not null) {
					Log.LogMessage (MessageImportance.Low, $"Not signing '{output [i].ItemSpec}' because it's in the list of directories that skips signing ({matchingDirectory})");
					output.RemoveAt (i);
					continue;
				}
			}
		}

		IEnumerable<string> EnumerateAppBundleEntries (string appPath)
		{
			foreach (var file in Directory.EnumerateFiles (appPath, "*", SearchOption.TopDirectoryOnly)) {
				// Don't sign symlinks, just the real file (this avoids trying to sign the same file multiple times through a symlink)
				if (PathUtils.IsSymlink (file))
					continue;
				yield return file;
			}

			foreach (var dir in Directory.EnumerateDirectories (appPath, "*", SearchOption.TopDirectoryOnly)) {
				// Don't recurse into symlinks, for same reason as above.
				if (PathUtils.IsSymlink (dir))
					continue;
				yield return dir;
				foreach (var entry in EnumerateAppBundleEntries (dir))
					yield return entry;
			}
		}

		IEnumerable<string> FindFilesToSign (string appPath)
		{
			var rv = new List<string> ();

			// Canonicalize the app path, so string comparisons work later on
			appPath = PathUtils.ResolveSymbolicLinks (Path.GetFullPath (appPath))!;

			// Make sure path ends with trailing slash to ease logic
			appPath = PathUtils.EnsureTrailingSlash (appPath);

			string dylibDirectory;
			string metallibDirectory;
			string frameworksDirectory;
			string xpcDirectory;
			switch (Platform) {
			case ApplePlatform.iOS:
			case ApplePlatform.TVOS:
				dylibDirectory = appPath;
				metallibDirectory = appPath;
				frameworksDirectory = Path.Combine (appPath, "Frameworks");
				xpcDirectory = Path.Combine (appPath, "XPCServices");
				break;
			case ApplePlatform.MacOSX:
			case ApplePlatform.MacCatalyst:
				dylibDirectory = Path.Combine (appPath, "Contents");
				metallibDirectory = Path.Combine (appPath, "Contents", "Resources");
				frameworksDirectory = Path.Combine (appPath, "Contents", "Frameworks");
				xpcDirectory = Path.Combine (appPath, "Contents", "XPCServices");
				break;
			default:
				throw new InvalidOperationException (string.Format (MSBStrings.InvalidPlatform, Platform));
			}

			dylibDirectory = PathUtils.EnsureTrailingSlash (dylibDirectory);
			metallibDirectory = PathUtils.EnsureTrailingSlash (metallibDirectory);

			foreach (var entry in EnumerateAppBundleEntries (appPath)) {
				var relativePath = entry.Substring (appPath.Length);
				// Don't recurse into the PlugIns directory, that's already handled for any app bundle inside the PlugIns directory
				if (relativePath.StartsWith ("PlugIns" + Path.DirectorySeparatorChar, StringComparison.OrdinalIgnoreCase))
					continue;
				// Don't recurse into the Watch directory, for the same reason
				if (relativePath.StartsWith ("Watch" + Path.DirectorySeparatorChar, StringComparison.OrdinalIgnoreCase))
					continue;

				if (entry.EndsWith (".dylib", StringComparison.OrdinalIgnoreCase) && entry.StartsWith (dylibDirectory, StringComparison.OrdinalIgnoreCase)) {
					// We find *.dylibs in any subdirectory
					rv.Add (entry);
				} else if (entry.EndsWith (".metallib", StringComparison.OrdinalIgnoreCase) && entry.StartsWith (metallibDirectory, StringComparison.OrdinalIgnoreCase)) {
					// We find *.metallib in any subdirectory
					rv.Add (entry);
				} else if (entry.EndsWith (".framework", StringComparison.OrdinalIgnoreCase) && string.Equals (Path.GetDirectoryName (entry), frameworksDirectory, StringComparison.OrdinalIgnoreCase)) {
					// We only find *.frameworks inside the Frameworks subdirectory, not recursively
					// (not quite sure if this is the right thing to do, but it's what we've been doing so far).
					rv.Add (entry);
				} else if (entry.EndsWith (".xpc", StringComparison.OrdinalIgnoreCase)) {
					// We find *.xpc inside the XPCServices subdirectory recursively (as opposed to for *.framework just above),
					// because 'codesign' fails if there are any *.xpc bundles in a subdirectory.
					rv.Add (entry);
				}
			}

			return rv;
		}

		public IEnumerable<ITaskItem> GetAdditionalItemsToBeCopied () => Enumerable.Empty<ITaskItem> ();

		// This task does not create or modify any files, and it should only
		// deal with files that are already on the mac, so no need to copy any
		// files either way.
		public bool ShouldCopyToBuildServer (ITaskItem item) => false;
		public bool ShouldCreateOutputFile (ITaskItem item) => false;

		public void Cancel ()
		{
			if (ShouldExecuteRemotely ())
				BuildConnection.CancelAsync (BuildEngine4).Wait ();
		}
	}
}
