extern alias Microsoft_Build_Tasks_Core;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Build.Framework;
using Xamarin.MacDev.Tasks;
using Xamarin.Messaging.Build.Client;

namespace Microsoft.Build.Tasks {
	public class Move : Microsoft_Build_Tasks_Core::Microsoft.Build.Tasks.Move, ITaskCallback, IHasSessionId {
		public string SessionId { get; set; } = string.Empty;
		public override bool Execute ()
		{
			if (this.ShouldExecuteRemotely (SessionId))
				return XamarinTask.ExecuteRemotely (this);

			return base.Execute ();
		}

		public bool ShouldCopyToBuildServer (ITaskItem item) => false;

		public bool ShouldCreateOutputFile (ITaskItem item) => true;

		public IEnumerable<ITaskItem> GetAdditionalItemsToBeCopied () => Enumerable.Empty<ITaskItem> ();
	}
}
