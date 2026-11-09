using System;
using System.Collections.Generic;
using System.Threading;

using Microsoft.Build.Framework;

using Xamarin.Messaging.Build.Client;
using Xamarin.Utils;

namespace Xamarin.MacDev.Tasks {
	public abstract class XcodeBuildTask : XamarinTask, ICancelableTask {
		readonly CancellationTokenSource cancellationTokenSource = new CancellationTokenSource ();
		protected CancellationToken CancellationToken => cancellationTokenSource.Token;

		public string WorkingDirectory { get; set; } = string.Empty;

		public string OutputPath { get; set; } = string.Empty;


		protected virtual string Command { get; set; } = string.Empty;

		protected virtual IList<string> GenerateCommandLineCommands () => new List<string> ();

		protected static string GetRemotePath (string path)
		{
			return string.IsNullOrEmpty (path) ? path : PathUtils.AbsoluteToRelative (Environment.CurrentDirectory, path).Replace ('\\', '/');
		}

		public override bool Execute ()
		{
			if (ShouldExecuteRemotely ()) {
				WorkingDirectory = GetRemotePath (WorkingDirectory);
				OutputPath = GetRemotePath (OutputPath);
				return ExecuteRemotely ();
			}

			var args = new List<string> ();
			args.Add ("xcodebuild");
			if (!string.IsNullOrEmpty (Command)) {
				args.Add (Command);
			}
			args.AddRange (GenerateCommandLineCommands ());

			ExecuteAsync ("xcrun", args, workingDirectory: WorkingDirectory, cancellationToken: cancellationTokenSource.Token).Wait ();
			return !Log.HasLoggedErrors;
		}

		public void Cancel ()
		{
			if (ShouldExecuteRemotely ())
				BuildConnection.CancelAsync (BuildEngine4).Wait ();
			else
				cancellationTokenSource.Cancel ();
		}

	}
}
