using System;
using System.IO;
using Microsoft.Build.Framework;
using Microsoft.Build.Utilities;
using Xamarin.Localization.MSBuild;
using Xamarin.MacDev.Tasks;
using Xamarin.Messaging;
using Xamarin.Messaging.Build.Client;
using Xamarin.Messaging.Ssh;

#nullable enable

namespace Xamarin.MacDev.Tasks {
	public class CopyArchiveFiles : XamarinTask, ICancelableTask {
		[Required]
		public string ArchivePath { get; set; } = "";

		[Required]
		public string AppName { get; set; } = "";

		[Required]
		public string TargetPath { get; set; } = "";

		public override bool Execute ()
		{
			if (Path.GetExtension (ArchivePath) != ".xcarchive") {
				Log.LogError (186, null, MSBStrings.E0186, ArchivePath); // The provided Archive Path is not from a valid archive file (.xcarchive). Archive Path: '{0}'

				return false;
			}

			var client = GetBuildClientAsync ().Result;

			if (!client.IsConnected) {
				Log.LogError (177, null, MSBStrings.E0177, ArchivePath); // Can't get an ISshCommands instance. Please check that a valid connection to the Mac is established.

				return false;
			}

			var sshCommands = client.MessagingService.Ssh.Commands;

			if (sshCommands is null) {
				Log.LogError (177, null, MSBStrings.E0177, ArchivePath); // Can't get an ISshCommands instance. Please check that a valid connection to the Mac is established.

				return false;
			}

			try {
				CopyArchiveAsync (sshCommands).Wait ();

				return true;
			} catch (Exception ex) {
				Log.LogError (178, null, MSBStrings.E0178, ArchivePath, TargetPath, ex.Message); // An error occurred while trying to copy Archive '{0}' to '{1}'. Details: {2}

				return false;
			}
		}

		public void Cancel () => BuildConnection.CancelAsync (BuildEngine4).Wait ();

		async System.Threading.Tasks.Task<IBuildClient> GetBuildClientAsync ()
		{
			var connection = await BuildConnection
				.GetAsync (BuildEngine4)
				.ConfigureAwait (continueOnCapturedContext: false);

			return connection.GetClient (SessionId);
		}

		async System.Threading.Tasks.Task CopyArchiveAsync (ISshCommands sshCommands)
		{
			var buildPath = PlatformPath.GetServerBuildPath (AppName, SessionId, TargetPath);

			if (!Directory.Exists (buildPath)) {
				await sshCommands.CreateDirectoryAsync (buildPath).ConfigureAwait (continueOnCapturedContext: false);
			}

			await sshCommands.CopyDirectoryAsync (ArchivePath, buildPath).ConfigureAwait (continueOnCapturedContext: false);
		}
	}
}
