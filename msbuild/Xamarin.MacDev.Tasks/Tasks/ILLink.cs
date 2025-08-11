using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Microsoft.Build.Framework;
using Microsoft.Build.Tasks;
using Microsoft.Build.Utilities;

using Xamarin.Messaging.Build.Client;

#nullable enable

namespace Xamarin.MacDev.Tasks {
	public class ILLink : global::ILLink.Tasks.ILLink, ITaskCallback {
		public string SessionId { get; set; } = string.Empty;

		public ITaskItem [] DebugSymbols { get; set; } = Array.Empty<ITaskItem> ();

		[Required]
		public string LinkerItemsDirectory { get; set; } = string.Empty;

		[Required]
		public string LinkerCacheDirectory { get; set; } = string.Empty;

		[Output]
		public ITaskItem [] LinkerOutputItems { get; set; } = Array.Empty<ITaskItem> ();

		[Output]
		public ITaskItem [] LinkerCacheItems { get; set; } = Array.Empty<ITaskItem> ();

		[Output]
		public ITaskItem [] LinkedItems { get; set; } = Array.Empty<ITaskItem> ();

		public override bool Execute ()
		{
			if (this.ShouldExecuteRemotely (SessionId))
				return new TaskRunner (SessionId, BuildEngine4).RunAsync (this).Result;

			// Capture execution start time for Mac-side detection
			var executionStartTime = DateTime.UtcNow;
			var result = base.Execute ();

			if (result) {
				// Collect all files and tag those modified during this execution
				LinkerOutputItems = GetAllFilesWithMetadata (LinkerItemsDirectory, executionStartTime);
				LinkedItems = GetAllFilesWithMetadata (OutputDirectory.ItemSpec, executionStartTime);
				LinkerCacheItems = GetAllFilesWithMetadata (LinkerCacheDirectory, executionStartTime);
			}

			return result;
		}

		public override void Cancel ()
		{
			if (this.ShouldExecuteRemotely (SessionId))
				BuildConnection.CancelAsync (BuildEngine4).Wait ();
			else
				base.Cancel ();
		}

		ITaskItem [] GetAllFilesWithMetadata (string directory, DateTime executionStartTime)
		{
			if (string.IsNullOrEmpty (directory) || !Directory.Exists (directory))
				return Array.Empty<ITaskItem> ();

			return Directory.EnumerateFiles (directory, "*", SearchOption.AllDirectories)
				.Select (file => {
					var fileInfo = new FileInfo (file);
					var item = new TaskItem (file);

					// Check if file was created or modified during this execution
					var wasModified = fileInfo.CreationTimeUtc >= executionStartTime ||
									  fileInfo.LastWriteTimeUtc >= executionStartTime;

					// Tag files that were modified during this execution
					item.SetMetadata ("Modified", wasModified.ToString ());

					return item;
				})
				.ToArray ();
		}

		// ITaskCallback implementation
		public bool ShouldCopyToBuildServer (ITaskItem item) => true;

		public bool ShouldCreateOutputFile (ITaskItem item)
		{
			var modifiedMetadata = item.GetMetadata ("Modified");
			var wasModified = bool.TryParse (modifiedMetadata, out var modified) && modified;

			// Create output file if it was modified during this execution
			if (wasModified) {
				Log.LogMessage (MessageImportance.Low, "Output file '{0}' was modified during execution", item.ItemSpec);
				return true;
			}

			// Create output file if it doesn't exist on Windows. We assume if it exists on the Mac we also need it on Windows.
			if (!File.Exists (item.ItemSpec)) {
				Log.LogMessage (MessageImportance.Low, "Output file '{0}' does not exist", item.ItemSpec);
				return true;
			}

			Log.LogMessage (MessageImportance.Low, "Output file '{0}' exists and was not modified", item.ItemSpec);
			return false;
		}

		public IEnumerable<ITaskItem> GetAdditionalItemsToBeCopied () => Array.Empty<ITaskItem> ();
	}
}
