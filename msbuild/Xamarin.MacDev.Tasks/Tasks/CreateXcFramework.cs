using System;
using System.IO;
using System.Collections.Generic;

using Microsoft.Build.Framework;

namespace Xamarin.MacDev.Tasks {
	public class CreateXcFramework : XcodeBuildTask {

		// Task input parameters
		[Required]
		public string XcArchivePath { get; set; } = string.Empty;

		[Required]
		public string FrameworkName { get; set; } = string.Empty;


		protected override string Command { get; set; } = "-create-xcframework";

		protected override IList<string> GenerateCommandLineCommands ()
		{
			var args = new List<string> ();

			var fullXcArchivePath = Path.GetFullPath (XcArchivePath);
			if (Directory.Exists (fullXcArchivePath)) {
				foreach (var frameworkArchive in Directory.EnumerateDirectories (fullXcArchivePath, "*.xcarchive")) {
					args.Add ("-archive");
					args.Add (frameworkArchive);
					args.Add ("-framework");
					args.Add (FrameworkName);
				}
			}

			if (!string.IsNullOrEmpty (OutputPath)) {
				args.Add ("-output");
				args.Add (Path.GetFullPath (OutputPath));
			}

			return args;
		}

	}
}
