extern alias Microsoft_Build_Tasks_Core;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Build.Framework;
using Xamarin.MacDev.Tasks;
using Xamarin.Messaging.Build.Client;

namespace Microsoft.Build.Tasks {
	public class MakeDir : Microsoft_Build_Tasks_Core::Microsoft.Build.Tasks.MakeDir, ITaskCallback, IHasSessionId {
		public string SessionId { get; set; } = string.Empty;
		public override bool Execute ()
		{
			var result = base.Execute ();

			if (this.ShouldExecuteRemotely (SessionId))
				result = XamarinTask.ExecuteRemotely (this);

			return result;
		}

		public bool ShouldCopyToBuildServer (ITaskItem item) => true;

		public bool ShouldCreateOutputFile (ITaskItem item) => false;

		public IEnumerable<ITaskItem> GetAdditionalItemsToBeCopied () => Enumerable.Empty<ITaskItem> ();
	}
}
