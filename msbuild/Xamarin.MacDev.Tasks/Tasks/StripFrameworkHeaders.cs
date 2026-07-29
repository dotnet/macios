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
	public class StripFrameworkHeaders : XamarinTask, ITaskCallback {
		static readonly HashSet<string> directoriesToStrip = new HashSet<string> (StringComparer.Ordinal) {
			"Headers",
			"Modules",
			"PrivateHeaders",
		};

		[Required]
		public string AppBundleDir { get; set; } = "";

		public override bool Execute ()
		{
			if (ShouldExecuteRemotely ())
				return ExecuteRemotely ();

			StripFrameworksInDirectory (AppBundleDir);

			return !Log.HasLoggedErrors;
		}

		void StripFrameworksInDirectory (string directory)
		{
			foreach (var subdirectory in Directory.GetDirectories (directory)) {
				if (IsSymbolicLink (subdirectory))
					continue;

				if (subdirectory.EndsWith (".framework", StringComparison.OrdinalIgnoreCase))
					StripFramework (subdirectory);

				StripFrameworksInDirectory (subdirectory);
			}
		}

		void StripFramework (string framework)
		{
			foreach (var directoryName in directoriesToStrip)
				RemoveDirectory (Path.Combine (framework, directoryName));

			var versionsDirectory = Path.Combine (framework, "Versions");
			if (!Directory.Exists (versionsDirectory) || IsSymbolicLink (versionsDirectory))
				return;

			foreach (var versionDirectory in Directory.GetDirectories (versionsDirectory).Where (directory => !IsSymbolicLink (directory))) {
				foreach (var directoryName in directoriesToStrip)
					RemoveDirectory (Path.Combine (versionDirectory, directoryName));
			}
		}

		void RemoveDirectory (string directory)
		{
			if (!Directory.Exists (directory))
				return;

			Log.LogMessage (MessageImportance.Low, $"Removing framework directory '{directory}'.");
			if (IsSymbolicLink (directory))
				File.Delete (directory);
			else
				Directory.Delete (directory, true);
		}

		static bool IsSymbolicLink (string path)
		{
			return (File.GetAttributes (path) & FileAttributes.ReparsePoint) != 0;
		}

		public IEnumerable<ITaskItem> GetAdditionalItemsToBeCopied ()
		{
			return Enumerable.Empty<ITaskItem> ();
		}

		public bool ShouldCopyToBuildServer (ITaskItem item) => false;

		public bool ShouldCreateOutputFile (ITaskItem item) => false;
	}
}
