// Copyright (c) Microsoft Corporation.
// Licensed under the MIT License.

using System.Collections.Generic;
using System.IO;
using System.Linq;

using Microsoft.Build.Framework;

#nullable enable

namespace Xamarin.MacDev.Tasks {
	// See docs/code/native-symbols.md and docs/code/class-handles.md for an overview of native symbol handling.
	public class CollectUnresolvedNativeSymbols : XamarinTask {
		public ITaskItem? StaticLibrary { get; set; }

		[Required]
		public string OutputFile { get; set; } = "";

		public override bool Execute ()
		{
			if (StaticLibrary is null) {
				Log.LogError ("StaticLibrary is required.");
				return false;
			}

			var path = StaticLibrary.ItemSpec;
			if (!File.Exists (path)) {
				Log.LogError ("Static library not found: {0}", path);
				return false;
			}

			var symbols = Xamarin.StaticLibrary.GetUnresolvedSymbols (path);
			Log.LogMessage (MessageImportance.Low, "Found {0} unresolved symbols in {1}", symbols.Count, path);

			var lines = symbols.OrderBy (s => s).ToArray ();
			if (File.Exists (OutputFile)) {
				var existing = File.ReadAllLines (OutputFile);
				if (existing.SequenceEqual (lines))
					return !Log.HasLoggedErrors;
			}

			var dir = Path.GetDirectoryName (OutputFile);
			if (!string.IsNullOrEmpty (dir))
				Directory.CreateDirectory (dir);
			File.WriteAllLines (OutputFile, lines);

			return !Log.HasLoggedErrors;
		}
	}
}
