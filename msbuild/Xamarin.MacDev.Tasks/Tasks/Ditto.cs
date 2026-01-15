#nullable enable

using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading;

using Microsoft.Build.Framework;
using Microsoft.Build.Utilities;

using Xamarin.Localization.MSBuild;
using Xamarin.Messaging.Build.Client;
using Xamarin.Utils;

namespace Xamarin.MacDev.Tasks {
	public class Ditto : XamarinTask, ITaskCallback, ICancelableTask {
		CancellationTokenSource? cancellationTokenSource;

		#region Inputs

		public string? AdditionalArguments { get; set; }

		// If the input directory should be copied from Windows to the Mac in
		// a remote build. In some cases we only maintain empty files on
		// Windows to keep track of modified files, so that we don't have to
		// transfer the entire file back to Windows, and in those cases we
		// don't want to copy the empty content back to the Mac. In other
		// cases the input comes from Windows, and in that case we want to
		// copy the entire input to the Mac - so we need an option to select
		// the mode.
		public bool CopyFromWindows { get; set; }

		public string? DittoPath { get; set; }

		[Required]
		public ITaskItem? Source { get; set; }

		[Required]
		public ITaskItem? Destination { get; set; }

		// If set, this file will be created if the source was successfully copied to the destination.
		public ITaskItem? StampFile { get; set; }

		public bool TouchDestinationFiles { get; set; }

		// Whether output files should be created on Windows when executing remotely
		public bool CreateOutputFiles { get; set; } = true;

		// This property is required for XVS to work properly, even though it's not used for anything in the targets.
		[Output]
		public ITaskItem [] CopiedFiles { get; set; } = Array.Empty<ITaskItem> ();

		#endregion

		public override bool Execute ()
		{
			var rv = ExecuteImpl ();

			if (rv) {
				// This is executed directly on Windows for remote builds
				var stampPath = StampFile?.ItemSpec;
				if (!string.IsNullOrEmpty (stampPath)) {
					Directory.CreateDirectory (Path.GetDirectoryName (stampPath)!);
					var src = Source?.ItemSpec;
					var destination = Destination?.ItemSpec;
					File.WriteAllText (stampPath, $"{src} -> {destination}");
				}
			}

			return rv;
		}

		bool ExecuteImpl ()
		{
			if (ShouldExecuteRemotely ()) {
				return ExecuteRemotely (out var _, (taskRunner) => {
					taskRunner.FixReferencedItems (this, new ITaskItem [] { Source! });
				});
			}

			var src = Source!.ItemSpec;
			if (!File.Exists (src) && !Directory.Exists (src)) {
				Log.LogError (MSBStrings.E7131 /* The source '{0}' does not exist. */, src);
				return false;
			}

			var executable = string.IsNullOrEmpty (DittoPath) ? "/usr/bin/ditto" : DittoPath!;
			var args = new List<string> ();
			args.Add (Path.GetFullPath (Source!.ItemSpec));
			args.Add (Path.GetFullPath (Destination!.ItemSpec));
#if NET
			if (!string.IsNullOrEmpty (AdditionalArguments)) {
#else
			if (AdditionalArguments is not null && !string.IsNullOrEmpty (AdditionalArguments)) {
#endif
				if (StringUtils.TryParseArguments (AdditionalArguments, out var additionalArgs, out var ex)) {
					args.AddRange (additionalArgs);
				} else {
					Log.LogError (MSBStrings.E7132 /* Unable to parse the 'AdditionalArguments' value: {0} */, AdditionalArguments);
					return false;
				}
			}

			cancellationTokenSource = new CancellationTokenSource ();
			ExecuteAsync (Log, executable, args, cancellationToken: cancellationTokenSource.Token).Wait ();

			// Create a list of all the files we've copied
			if (CreateOutputFiles) {
				var copiedFiles = new List<ITaskItem> ();
				var destination = Destination!.ItemSpec;
				if (Directory.Exists (destination)) {
					foreach (var file in Directory.EnumerateFiles (destination, "*", SearchOption.AllDirectories)) {
						if (TouchDestinationFiles)
							File.SetLastWriteTimeUtc (file, DateTime.UtcNow);
						copiedFiles.Add (new TaskItem (file));
					}
				} else {
					copiedFiles.Add (Destination);
				}
				CopiedFiles = copiedFiles.ToArray ();
			}

			return !Log.HasLoggedErrors;
		}

		public void Cancel ()
		{
			if (ShouldExecuteRemotely ()) {
				BuildConnection.CancelAsync (BuildEngine4).Wait ();
			} else {
				cancellationTokenSource?.Cancel ();
			}
		}

		public IEnumerable<ITaskItem> GetAdditionalItemsToBeCopied ()
		{
			if (!Directory.Exists (Source!.ItemSpec))
				return Enumerable.Empty<ITaskItem> ();

			if (!CopyFromWindows)
				return Enumerable.Empty<ITaskItem> ();

			// TaskRunner doesn't know how to copy directories to Mac but `ditto` can take directories (and that's why we use ditto often).
			// If Source is a directory path, let's add each file within it as an TaskItem, as TaskRunner knows how to copy files to Mac.
			return Directory.GetFiles (Source.ItemSpec, "*", SearchOption.AllDirectories)
				.Select (f => new TaskItem (f));
		}

		public bool ShouldCopyToBuildServer (ITaskItem item) => true;

		public bool ShouldCreateOutputFile (ITaskItem item) => CreateOutputFiles;
	}
}
