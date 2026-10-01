extern alias Microsoft_Build_Tasks_Core;

using Xamarin.MacDev.Tasks;
using Xamarin.Messaging.Build.Client;

namespace Microsoft.Build.Tasks {
	public class WriteLinesToFile : Microsoft_Build_Tasks_Core::Microsoft.Build.Tasks.WriteLinesToFile, IHasSessionId {
		public string SessionId { get; set; } = string.Empty;
		public bool CopyToWindows { get; set; }

		public override bool Execute ()
		{
			if (this.ShouldExecuteRemotely (SessionId)) {
				if (!XamarinTask.ExecuteRemotely (this, out var taskRunner))
					return false;

				// File isn't an [Output] property, so no empty placeholder is created on Windows.
				if (CopyToWindows)
					XamarinTask.CopyFilesToWindowsAsync (this, taskRunner, new [] { File }).Wait ();

				return true;
			}

			return base.Execute ();
		}
	}
}
