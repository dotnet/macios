// Copyright (c) Microsoft Corporation.
// Licensed under the MIT License.

using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

using Microsoft.Build.Framework;

using Xamarin.Messaging.Build.Client;

#nullable enable

namespace Xamarin.MacDev.Tasks {
	public class OptimizeLocalization : XamarinTask, ICancelableTask {
		CancellationTokenSource? cancellationTokenSource;

		#region Inputs

		[Required]
		public ITaskItem? Input { get; set; }

		[Required]
		[Output]
		public ITaskItem? Output { get; set; }

		public string PlutilPath { get; set; } = "";

		#endregion

		List<string> GenerateCommandLineCommands (string input, string output)
		{
			var args = new List<string> ();

			args.Add ("-convert");
			args.Add ("binary1");
			args.Add ("-o");
			args.Add (output);
			args.Add (input);

			return args;
		}

		public override bool Execute ()
		{
			if (ShouldExecuteRemotely ())
				return ExecuteRemotely ();

			var input = Input!.ItemSpec;
			var output = Output!.ItemSpec;

			var outputDirectory = Path.GetDirectoryName (output);
			if (!string.IsNullOrEmpty (outputDirectory))
				Directory.CreateDirectory (outputDirectory);
			var args = GenerateCommandLineCommands (input, output);
			var executable = GetExecutable (args, "plutil", PlutilPath);
			cancellationTokenSource = new CancellationTokenSource ();
			ExecuteAsync (executable, args, cancellationToken: cancellationTokenSource.Token).Wait ();
			return !Log.HasLoggedErrors;
		}

		public bool ShouldCopyToBuildServer (ITaskItem item) => false;

		public bool ShouldCreateOutputFile (ITaskItem item) => true;

		public IEnumerable<ITaskItem> GetAdditionalItemsToBeCopied () => Enumerable.Empty<ITaskItem> ();

		public void Cancel ()
		{
			if (ShouldExecuteRemotely ()) {
				BuildConnection.CancelAsync (BuildEngine4).Wait ();
			} else {
				cancellationTokenSource?.Cancel ();
			}
		}
	}
}
