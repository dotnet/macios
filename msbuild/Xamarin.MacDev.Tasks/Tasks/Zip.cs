using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

using Microsoft.Build.Framework;
using Microsoft.Build.Utilities;

using Xamarin.Messaging.Build.Client;
using Xamarin.Utils;

#nullable enable

namespace Xamarin.MacDev.Tasks {
	public class Zip : XamarinTask, ICancelableTask, ITaskCallback {
		#region Inputs

		[Output]
		[Required]
		public ITaskItem? OutputFile { get; set; }

		[Required]
		public ITaskItem [] Sources { get; set; } = Array.Empty<ITaskItem> ();

		[Required]
		public ITaskItem? WorkingDirectory { get; set; }

		public string ZipPath { get; set; } = string.Empty;

		#endregion

		string GetWorkingDirectory ()
		{
			return WorkingDirectory!.GetMetadata ("FullPath");
		}

		public override bool Execute ()
		{
			if (ShouldExecuteRemotely ()) {
				var taskRunner = new TaskRunner (SessionId, BuildEngine4);
				var rv = taskRunner.RunAsync (this).Result;

				// Copy the zipped file back to Windows.
				if (rv)
					taskRunner.GetFileAsync (this, OutputFile!.ItemSpec).Wait ();

				return rv;
			}

			var zip = OutputFile!.GetMetadata ("FullPath");
			var workingDirectory = GetWorkingDirectory ();
			var sources = new List<string> ();
			for (int i = 0; i < Sources.Length; i++)
				sources.Add (Sources [i].GetMetadata ("FullPath"));

			if (!CompressionHelper.TryCompress (Log, zip, sources, false, workingDirectory, false))
				return false;

			return !Log.HasLoggedErrors;
		}

		public void Cancel ()
		{
			if (ShouldExecuteRemotely ()) {
				BuildConnection.CancelAsync (BuildEngine4).Wait ();
			}
		}

		//We don't want the inputs to be copied to the Mac since when zipping remotely, we are expecting the files to be already present in the Mac
		public bool ShouldCopyToBuildServer (ITaskItem item) => false;

		//We don't want empty output files to be created in Windows since we are already copying the real output file as part of the task execution
		public bool ShouldCreateOutputFile (ITaskItem item) => false;

		public IEnumerable<ITaskItem> GetAdditionalItemsToBeCopied () => Enumerable.Empty<ITaskItem> ();
	}
}
