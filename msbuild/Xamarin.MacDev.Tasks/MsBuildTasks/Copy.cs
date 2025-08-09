extern alias Microsoft_Build_Tasks_Core;

using System.Linq;

using Microsoft.Build.Framework;

using Xamarin.Localization.MSBuild;
using Xamarin.Messaging.Build.Client;

namespace Microsoft.Build.Tasks {
	public class Copy : Microsoft_Build_Tasks_Core::Microsoft.Build.Tasks.Copy {
		public string SessionId { get; set; } = string.Empty;
		public override bool Execute ()
		{
			if (SourceFiles?.Any () != true) {
				Log.LogMessage (MessageImportance.Low, MSBStrings.M7159 /* Skipping {0} - {1} is empty. */, nameof (Copy), nameof (SourceFiles));
				return true;
			}

			if (!this.ShouldExecuteRemotely (SessionId))
				return base.Execute ();

			var taskRunner = new TaskRunner (SessionId, BuildEngine4);

			if (SourceFiles?.Any () == true) {
				taskRunner.FixReferencedItems (this, SourceFiles);
			}

			return taskRunner.RunAsync (this).Result;
		}
	}
}
