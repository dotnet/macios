using Microsoft.Build.Framework;
using Microsoft.Build.Utilities;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Xamarin.MacDev.Tasks;
using Xamarin.Messaging.Build.Client;

namespace Xamarin.MacDev.Tasks {
	public class PrepareObjCBindingNativeFrameworks : XamarinTask, ITaskCallback, ICancelableTask {
		public ITaskItem [] ObjCBindingNativeFrameworks { get; set; } = Array.Empty<ITaskItem> ();

		public override bool Execute ()
		{
			//This task runs locally, and its purpose is just to copy the ObjCBindingNativeFrameworks to the build server
			return CopyInputsToRemoteServerAsync (this);
		}

		public bool ShouldCopyToBuildServer (ITaskItem item) => true;

		public bool ShouldCreateOutputFile (ITaskItem item) => false;

		public IEnumerable<ITaskItem> GetAdditionalItemsToBeCopied ()
		{
			return CreateItemsForAllFilesRecursively (ObjCBindingNativeFrameworks);
		}

		public void Cancel () => BuildConnection.CancelAsync (BuildEngine4).Wait ();
	}
}
