using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

using Microsoft.Build.Utilities;
using Microsoft.Build.Framework;

using Xamarin.Messaging.Build.Client;

#nullable enable

namespace Xamarin.MacDev.Tasks {
	public class OptimizePropertyList : XamarinTask, ICancelableTask, ITaskCallback {
		CancellationTokenSource? cancellationTokenSource;
		#region Inputs

		[Required]
		public ITaskItem? Input { get; set; }

		[Required]
		[Output]
		public ITaskItem? Output { get; set; }

		public string PlutilPath { get; set; } = string.Empty;

		#endregion

		List<string> GenerateCommandLineCommands ()
		{
			var args = new List<string> ();

			args.Add ("-convert");
			args.Add ("binary1");
			args.Add ("-o");
			args.Add (Output!.ItemSpec);
			args.Add (Input!.ItemSpec);

			return args;
		}

		public override bool Execute ()
		{
			if (string.Equals (Path.GetExtension (Input!.ItemSpec), ".plist", StringComparison.OrdinalIgnoreCase)) {
				var plist = PObject.FromFile (Input.ItemSpec);
				if (plist is null)
					throw new FormatException ($"Could not parse the property list '{Input.ItemSpec}'.");

				Directory.CreateDirectory (Path.GetDirectoryName (Output!.ItemSpec)!);
				plist.Save (Output!.ItemSpec, binary: true);

				if (ShouldExecuteRemotely ())
					return CopyInputsToRemoteServerAsync (this);

				return true;
			}

			if (ShouldExecuteRemotely ())
				return ExecuteRemotely ();

			Directory.CreateDirectory (Path.GetDirectoryName (Output!.ItemSpec)!);
			var args = GenerateCommandLineCommands ();
			var executable = GetExecutable (args, "plutil", PlutilPath);
			cancellationTokenSource = new CancellationTokenSource ();
			ExecuteAsync (executable, args, cancellationToken: cancellationTokenSource.Token).Wait ();
			return !Log.HasLoggedErrors;
		}

		public bool ShouldCopyToBuildServer (Microsoft.Build.Framework.ITaskItem item) => Output is not null && item.ItemSpec == Output.ItemSpec;

		public bool ShouldCreateOutputFile (Microsoft.Build.Framework.ITaskItem item) => true;

		public IEnumerable<ITaskItem> GetAdditionalItemsToBeCopied () => Output is null ? [] : [Output];

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
