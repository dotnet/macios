// Copyright (c) Microsoft Corporation.
// Licensed under the MIT License.

using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

using Microsoft.Build.Framework;
using Microsoft.Build.Utilities;

using Xamarin.Messaging.Build.Client;

#nullable enable

namespace Xamarin.MacDev.Tasks {
	// Creates the *.symbols files Apple expects in an .ipa's 'Symbols' directory
	// by running 'xcrun symbols' over the DWARF binaries inside each dSYM.
	public class CreateSymbolsPackage : XamarinTask, ITaskCallback {
		#region Inputs

		[Required]
		public ITaskItem [] DSymDirectories { get; set; } = [];

		[Required]
		public string SymbolsDirectory { get; set; } = "";

		#endregion

		public override bool Execute ()
		{
			if (ShouldExecuteRemotely ())
				return ExecuteRemotely ();

			Directory.CreateDirectory (SymbolsDirectory);

			foreach (var dsym in DSymDirectories)
				CreateSymbols (dsym.ItemSpec);

			return !Log.HasLoggedErrors;
		}

		void CreateSymbols (string dSymDirectory)
		{
			var dwarfDirectory = Path.Combine (dSymDirectory, "Contents", "Resources", "DWARF");
			if (!Directory.Exists (dwarfDirectory)) {
				Log.LogMessage (MessageImportance.Low, "Skipping '{0}' because it doesn't contain any DWARF binaries.", dSymDirectory);
				return;
			}

			foreach (var binary in Directory.EnumerateFiles (dwarfDirectory)) {
				var args = new List<string> {
					"symbols",
					"-noTextInSOD",
					"-noDaemon",
					"-arch",
					"all",
					"-symbolsPackageDir",
					SymbolsDirectory,
					Path.GetFullPath (binary),
				};

				ExecuteAsync ("xcrun", args).Wait ();
			}
		}

		public bool ShouldCopyToBuildServer (ITaskItem item) => false;

		public bool ShouldCreateOutputFile (ITaskItem item) => false;

		public IEnumerable<ITaskItem> GetAdditionalItemsToBeCopied () => Enumerable.Empty<ITaskItem> ();
	}
}
