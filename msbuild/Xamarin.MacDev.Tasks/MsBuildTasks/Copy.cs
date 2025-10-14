extern alias Microsoft_Build_Tasks_Core;

using System.Linq;

using Microsoft.Build.Framework;

using Xamarin.Localization.MSBuild;
using Xamarin.MacDev.Tasks;
using Xamarin.Messaging.Build.Client;

namespace Microsoft.Build.Tasks {
	public class Copy : Microsoft_Build_Tasks_Core::Microsoft.Build.Tasks.Copy, IHasSessionId {
		public string SessionId { get; set; } = string.Empty;
		public override bool Execute ()
		{
			if (SourceFiles?.Any () != true) {
				Log.LogMessage (MessageImportance.Low, MSBStrings.M7159 /* Skipping {0} - {1} is empty. */, nameof (Copy), nameof (SourceFiles));
				return true;
			}

			if (!this.ShouldExecuteRemotely (SessionId))
				return base.Execute ();

			return XamarinTask.ExecuteRemotely (this, out var _, (taskRunner) => {
				if (SourceFiles?.Any () == true) {
					taskRunner.FixReferencedItems (this, SourceFiles);
				}
			});
		}
	}
}
