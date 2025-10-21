extern alias Microsoft_Build_Tasks_Core;

using Xamarin.MacDev.Tasks;
using Xamarin.Messaging.Build.Client;

namespace Microsoft.Build.Tasks {
	public class WriteLinesToFile : Microsoft_Build_Tasks_Core::Microsoft.Build.Tasks.WriteLinesToFile, IHasSessionId {
		public string SessionId { get; set; } = string.Empty;
		public override bool Execute ()
		{
			if (this.ShouldExecuteRemotely (SessionId))
				return XamarinTask.ExecuteRemotely (this);

			return base.Execute ();
		}
	}
}
