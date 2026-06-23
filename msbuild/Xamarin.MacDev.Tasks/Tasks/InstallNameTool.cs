using System;
using System.IO;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

using Microsoft.Build.Framework;

using Xamarin.Messaging.Build.Client;
using Xamarin.Utils;

#nullable enable

namespace Xamarin.MacDev.Tasks {
	public class InstallNameTool : XamarinTask, ITaskCallback {
		[Required]
		public ITaskItem [] DynamicLibrary { get; set; } = [];

		// The intended output directory for reidentified native libraries. Used to make sure we never
		// write outside this directory, even if the reidentified path was influenced by metadata that
		// originates from a (passive) binding resource package manifest.
		[Required]
		public string IntermediateNativeLibraryDir { get; set; } = "";

		// This isn't consumed from the targets files, but it's needed for VSX to create corresponding
		// files on Windows.
		[Output]
		public ITaskItem [] ReidentifiedDynamicLibrary { get; set; } = [];

		public override bool Execute ()
		{
			if (ShouldExecuteRemotely ())
				return ExecuteRemotely ();

			var processes = new Task [DynamicLibrary.Length];
			ReidentifiedDynamicLibrary = new ITaskItem [DynamicLibrary.Length];

			for (var i = 0; i < DynamicLibrary.Length; i++) {
				var input = DynamicLibrary [i];
				var src = Path.GetFullPath (input.ItemSpec);
				// Make sure we use the correct path separator, these are relative paths, so it doesn't look
				// like MSBuild does the conversion automatically.
				var target = input.GetMetadata ("ReidentifiedPath").Replace ('\\', Path.DirectorySeparatorChar);

				// Defense-in-depth: the 'ReidentifiedPath' can be influenced by metadata that originates
				// from a (passive) binding resource package manifest. Make sure we
				// never create directories or write files outside the intended intermediate output
				// directory, even if the path contains traversal segments, is absolute, or uses symlinks.
				if (!IsPathContained (IntermediateNativeLibraryDir, target)) {
					Log.LogError (MSBStrings.E7181 /* The native library can't be reidentified to '{0}' because that path is outside the intended output directory '{1}'. */, target, IntermediateNativeLibraryDir);
					processes [i] = System.Threading.Tasks.Task.CompletedTask;
					continue;
				}

				var temporaryTarget = target + ".tmp";

				// install_name_tool modifies the file in-place, so copy it first to a temporary file first.
				Directory.CreateDirectory (Path.GetDirectoryName (temporaryTarget)!);
				File.Copy (src, temporaryTarget, true);

				var arguments = new List<string> ();

				arguments.Add ("install_name_tool");
				arguments.Add ("-id");
				arguments.Add (input.GetMetadata ("DynamicLibraryId"));
				arguments.Add (temporaryTarget);

				processes [i] = ExecuteAsync ("xcrun", arguments).ContinueWith ((v) => {
					if (v.IsFaulted)
						throw v.Exception;
					if (v.Status == TaskStatus.RanToCompletion && v.Result.ExitCode == 0) {
						File.Delete (target);
						File.Move (temporaryTarget, target);
					}
				});

				ReidentifiedDynamicLibrary [i] = new Microsoft.Build.Utilities.TaskItem (target);
			}

			Task.WaitAll (processes);

			// Drop any items we skipped because their reidentified path wasn't contained.
			ReidentifiedDynamicLibrary = ReidentifiedDynamicLibrary.Where (item => item is not null).ToArray ();

			return !Log.HasLoggedErrors;
		}

		// Returns true if 'target' (which may not exist yet) is located within 'root' after fully
		// canonicalizing both paths (resolving '..' segments and symbolic links). Used to make sure we
		// never write reidentified native libraries outside the intended intermediate output directory,
		// even if the path was influenced by untrusted binding resource package metadata.
		public static bool IsPathContained (string root, string target)
		{
			if (string.IsNullOrEmpty (root) || string.IsNullOrEmpty (target))
				return false;

			var canonicalRoot = CanonicalizeForContainment (root);
			var canonicalTarget = CanonicalizeForContainment (target);

			var rootWithSeparator = canonicalRoot.TrimEnd (Path.DirectorySeparatorChar) + Path.DirectorySeparatorChar;
			return canonicalTarget.StartsWith (rootWithSeparator, StringComparison.Ordinal);
		}

		// Canonicalizes a path for a containment check: makes it absolute and resolves '..'/'.' segments,
		// then resolves symbolic links for the longest existing prefix (the target file usually doesn't
		// exist yet, so we resolve symlinks on the nearest existing ancestor and re-append the remainder).
		static string CanonicalizeForContainment (string path)
		{
			// The root can come from MSBuild with Windows separators (e.g. 'obj\...\nativelibraries\')
			// while the target has already been separator-normalized, so normalize here as well.
			path = path.Replace ('\\', Path.DirectorySeparatorChar);
			var full = Path.GetFullPath (path);

			var existing = full;
			var remainder = new List<string> ();
			while (existing.Length > 0 && !Directory.Exists (existing) && !File.Exists (existing)) {
				var parent = Path.GetDirectoryName (existing);
				if (string.IsNullOrEmpty (parent) || parent == existing)
					break;
				remainder.Insert (0, Path.GetFileName (existing));
				existing = parent;
			}

			var canonical = PathUtils.ResolveSymbolicLinks (existing) ?? existing;
			foreach (var segment in remainder)
				canonical = Path.Combine (canonical, segment);

			return canonical;
		}

		public bool ShouldCopyToBuildServer (ITaskItem item) => true;
		public bool ShouldCreateOutputFile (ITaskItem item) => true;
		public IEnumerable<ITaskItem> GetAdditionalItemsToBeCopied () => Enumerable.Empty<ITaskItem> ();
	}
}
