using System;
using System.IO;
using System.Security;

using Microsoft.Build.Framework;

using Xamarin.Localization.MSBuild;

namespace Xamarin.MacDev.Tasks.Messaging;

// We subclass the Xamarin.Messaging.Build.Client tasks so that we have all our tasks in the same
// assembly; otherwise we run into problems with task isolation in .NET, where the tasks in one
// assembly can't access shared state from tasks in another assembly (for instance, all remoted tasks
// need to access shared state from the SayHello task).

public class GenerateBuildSessionId : Xamarin.Messaging.Build.Client.Tasks.GenerateBuildSessionId {
	[Output]
	public string? AgentVersion { get; set; }

	public override bool Execute ()
	{
		if (!base.Execute ())
			return false;

		AgentVersion = Xamarin.Messaging.MessagingContext.Version;
		return true;
	}
}

public class SayHello : Xamarin.Messaging.Build.Client.Tasks.SayHello {
	public string? ServerPasswordFile { get; set; }
	public string? SSHPassPhraseFile { get; set; }

	string? ReadCredentialFile (string path, string emptyMessage)
	{
		string value;
		try {
			value = File.ReadAllText (path);
		} catch (Exception ex) when (ex is IOException || ex is UnauthorizedAccessException || ex is ArgumentException || ex is NotSupportedException || ex is SecurityException) {
			Log.LogError (MSBStrings.E0010, path, ex.Message);
			return null;
		}

		if (value.Length == 0) {
			Log.LogError (emptyMessage, path);
			return null;
		}

		return value;
	}

	public override bool Execute ()
	{
		if (ServerPasswordFile is string passwordFile && !string.IsNullOrEmpty (passwordFile)) {
			var password = ReadCredentialFile (passwordFile, MSBStrings.ServerPasswordFileEmpty);
			if (password is null)
				return false;
			ServerPassword = password;
		}

		if (SSHPassPhraseFile is string passPhraseFile && !string.IsNullOrEmpty (passPhraseFile)) {
			var passPhrase = ReadCredentialFile (passPhraseFile, MSBStrings.SSHPassPhraseFileEmpty);
			if (passPhrase is null)
				return false;
			SSHPassPhrase = passPhrase;
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
