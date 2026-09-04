using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

using Microsoft.Build.Framework;
using Xamarin.Messaging.Build.Client;

#nullable enable

namespace Xamarin.MacDev.Tasks {
	// Strips LC_ATOM_INFO (mergeable library metadata) from frameworks and dylibs in an app bundle.
	public class StripMergeableLibraryMetadata : XamarinTask, ITaskCallback {
		#region Inputs

		// The Frameworks directory inside the app bundle.
		public string FrameworksDirectory { get; set; } = string.Empty;

		// Additional directories to scan for mergeable dylibs (e.g. MonoBundle).
		public string [] DylibDirectories { get; set; } = [];

		public string StripPath { get; set; } = string.Empty;

		#endregion

		public override bool Execute ()
		{
			if (ShouldExecuteRemotely ())
				return ExecuteRemotely ();

			StripFrameworks ();
			StripDylibs ();

			return !Log.HasLoggedErrors;
		}

		void StripFrameworks ()
		{
			if (string.IsNullOrEmpty (FrameworksDirectory) || !Directory.Exists (FrameworksDirectory))
				return;

			foreach (var framework in Directory.GetDirectories (FrameworksDirectory, "*.framework")) {
				var name = Path.GetFileNameWithoutExtension (framework);
				var executable = Path.Combine (framework, name);
				StripIfMergeable (executable);
			}
		}

		void StripDylibs ()
		{
			if (DylibDirectories is null)
				return;

			foreach (var dir in DylibDirectories) {
				if (string.IsNullOrEmpty (dir) || !Directory.Exists (dir))
					continue;

				foreach (var dylib in Directory.GetFiles (dir, "*.dylib")) {
					StripIfMergeable (dylib);
				}
			}
		}

		void StripIfMergeable (string path)
		{
			if (!File.Exists (path))
				return;

			if (!MachO.IsMergeableLibrary (path)) {
				Log.LogMessage (MessageImportance.Low, $"Not a mergeable library: {path}");
				return;
			}

			Log.LogMessage (MessageImportance.Normal, $"Stripping mergeable library metadata from: {path}");

			var args = new List<string> ();
			var stripExecutable = GetExecutable (args, "strip", StripPath);
			args.Add ("-no_atom_info");
			args.Add ("-S");
			args.Add (Path.GetFullPath (path));
			ExecuteAsync (stripExecutable, args).Wait ();
		}

		public bool ShouldCopyToBuildServer (ITaskItem item) => false;

		public bool ShouldCreateOutputFile (ITaskItem item) => false;

		public IEnumerable<ITaskItem> GetAdditionalItemsToBeCopied () => Enumerable.Empty<ITaskItem> ();
	}
}
