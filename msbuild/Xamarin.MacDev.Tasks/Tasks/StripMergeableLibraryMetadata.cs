using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

using Microsoft.Build.Framework;
using Xamarin.Messaging.Build.Client;

#nullable enable

namespace Xamarin.MacDev.Tasks {
	// Strips LC_ATOM_INFO (mergeable library metadata) from all frameworks in a directory.
	public class StripMergeableLibraryMetadata : XamarinTask, ITaskCallback {
		#region Inputs

		[Required]
		public string FrameworksDirectory { get; set; } = string.Empty;

		public string StripPath { get; set; } = string.Empty;

		#endregion

		public override bool Execute ()
		{
			if (ShouldExecuteRemotely ())
				return ExecuteRemotely ();

			if (!Directory.Exists (FrameworksDirectory)) {
				Log.LogMessage (MessageImportance.Low, $"Frameworks directory does not exist: {FrameworksDirectory}");
				return true;
			}

			var frameworks = Directory.GetDirectories (FrameworksDirectory, "*.framework");
			if (frameworks.Length == 0) {
				Log.LogMessage (MessageImportance.Low, $"No frameworks found in: {FrameworksDirectory}");
				return true;
			}

			foreach (var framework in frameworks) {
				var name = Path.GetFileNameWithoutExtension (framework);
				var executable = Path.Combine (framework, name);
				if (!File.Exists (executable)) {
					Log.LogMessage (MessageImportance.Low, $"Framework executable does not exist: {executable}");
					continue;
				}

				if (!MachO.IsMergeableLibrary (executable)) {
					Log.LogMessage (MessageImportance.Low, $"Framework is not a mergeable library: {executable}");
					continue;
				}

				Log.LogMessage (MessageImportance.Normal, $"Stripping mergeable library metadata from: {executable}");

				var args = new List<string> ();
				var stripExecutable = GetExecutable (args, "strip", StripPath);
				args.Add ("-no_atom_info");
				args.Add (Path.GetFullPath (executable));
				ExecuteAsync (stripExecutable, args).Wait ();
			}

			return !Log.HasLoggedErrors;
		}

		public bool ShouldCopyToBuildServer (ITaskItem item) => false;

		public bool ShouldCreateOutputFile (ITaskItem item) => false;

		public IEnumerable<ITaskItem> GetAdditionalItemsToBeCopied () => Enumerable.Empty<ITaskItem> ();
	}
}
