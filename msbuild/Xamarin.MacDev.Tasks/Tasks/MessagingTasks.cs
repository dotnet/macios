using System;
using System.IO;
using System.Security;

using Xamarin.Localization.MSBuild;

namespace Xamarin.MacDev.Tasks.Messaging;

// We subclass the Xamarin.Messaging.Build.Client tasks so that we have all our tasks in the same
// assembly; otherwise we run into problems with task isolation in .NET, where the tasks in one
// assembly can't access shared state from tasks in another assembly (for instance, all remoted tasks
// need to access shared state from the SayHello task).

public class GenerateBuildSessionId : Xamarin.Messaging.Build.Client.Tasks.GenerateBuildSessionId {
}

public class SayHello : Xamarin.Messaging.Build.Client.Tasks.SayHello {
	public string? ServerPasswordFile { get; set; }

	public override bool Execute ()
	{
		if (!string.IsNullOrEmpty (ServerPasswordFile)) {
			try {
				ServerPassword = File.ReadAllText (ServerPasswordFile);
			} catch (Exception ex) when (ex is IOException || ex is UnauthorizedAccessException || ex is ArgumentException || ex is NotSupportedException || ex is SecurityException) {
				Log.LogError (MSBStrings.E0010, ServerPasswordFile, ex.Message);
				return false;
			}

			if (ServerPassword.Length == 0) {
				Log.LogError (MSBStrings.ServerPasswordFileEmpty, ServerPasswordFile);
				return false;
			}
		}

		return base.Execute ();
	}
}

public class CopyFileFromBuildServer : Xamarin.Messaging.Build.Client.Tasks.CopyFileFromBuildServer {
}

public class CopyFilesToBuildServer : Xamarin.Messaging.Build.Client.Tasks.CopyFilesToBuildServer {
}

public class CopyLongPaths : Xamarin.Messaging.Build.Client.Tasks.CopyLongPaths {
}

public class VerifyBuildSignature : Xamarin.Messaging.Build.Client.Tasks.VerifyBuildSignature {
}

public class SayGoodbye : Xamarin.Messaging.Build.Client.Tasks.SayGoodbye {
}
