using System;
using System.IO;
using System.Collections.Generic;
using System.Linq;

using Parallel = System.Threading.Tasks.Parallel;
using ParallelOptions = System.Threading.Tasks.ParallelOptions;

using Microsoft.Build.Framework;
using Xamarin.Messaging.Build.Client;

#nullable enable

namespace Xamarin.MacDev.Tasks {
	public class SymbolStrip : XamarinParallelTask, ITaskCallback {
		#region Inputs

		[Required]
		public ITaskItem [] Executable { get; set; } = Array.Empty<ITaskItem> ();

		public string StripPath { get; set; } = string.Empty;

		// This can also be specified as metadata on the Executable item (as 'SymbolFile')
		public string SymbolFile { get; set; } = string.Empty;

		// The local path to the symbol file (used to transfer it to the remote Mac).
		// This can also be specified as metadata on the Executable item (as 'SymbolFileLocalPath')
		public string SymbolFileLocalPath { get; set; } = string.Empty;

		// This can also be specified as metadata on the Executable item (as 'Kind')
		public string Kind { get; set; } = string.Empty;
		#endregion

		bool GetIsFrameworkOrDynamicLibrary (ITaskItem item)
		{
			var value = GetNonEmptyStringOrFallback (item, "Kind", Kind);
			if (string.Equals (value, "Framework", StringComparison.OrdinalIgnoreCase))
				return true;

			// A framework's executable lives inside a '*.framework' directory. Detect that even when
			// the 'Kind' metadata is missing, so we never do a full strip on a framework (which fails
			// for dynamic libraries). Ref: https://github.com/dotnet/macios/issues/25952
			var directory = Path.GetDirectoryName (item.ItemSpec);
			if (!string.IsNullOrEmpty (directory) && directory.EndsWith (".framework", StringComparison.OrdinalIgnoreCase))
				return true;

			if (string.Equals (value, "Dynamic", StringComparison.OrdinalIgnoreCase) || item.ItemSpec.EndsWith (".dylib", StringComparison.OrdinalIgnoreCase))
				return true;

			return false;
		}

		void ExecuteStrip (ITaskItem item)
		{
			var args = new List<string> ();
			var executable = GetExecutable (args, "strip", StripPath);

			var symbolFile = GetNonEmptyStringOrFallback (item, "SymbolFile", SymbolFile);
			if (!string.IsNullOrEmpty (symbolFile)) {
				args.Add ("-i");
				args.Add ("-s");
				args.Add (symbolFile);
			}

			if (GetIsFrameworkOrDynamicLibrary (item)) {
				// Only remove debug symbols from frameworks.
				args.Add ("-S");
				args.Add ("-x");
			}

			args.Add (Path.GetFullPath (item.ItemSpec));

			ExecuteAsync (executable, args).Wait ();
		}

		public override bool Execute ()
		{
			if (ShouldExecuteRemotely ())
				return ExecuteRemotely ();

			ForEach (Executable, (item) => {
				ExecuteStrip (item);
			});

			return !Log.HasLoggedErrors;
		}

		public bool ShouldCopyToBuildServer (ITaskItem item) => false;

		public bool ShouldCreateOutputFile (ITaskItem item) => false;

		public IEnumerable<ITaskItem> GetAdditionalItemsToBeCopied ()
		{
			if (!string.IsNullOrEmpty (SymbolFileLocalPath))
				yield return new Microsoft.Build.Utilities.TaskItem (SymbolFileLocalPath);

			foreach (var item in Executable) {
				var symbolFileLocalPath = item.GetMetadata ("SymbolFileLocalPath");
				if (!string.IsNullOrEmpty (symbolFileLocalPath))
					yield return new Microsoft.Build.Utilities.TaskItem (symbolFileLocalPath);
			}
		}
	}
}
