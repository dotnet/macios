// Copyright (c) Microsoft Corporation.
// Licensed under the MIT License.

using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

using Microsoft.Build.Framework;
using Microsoft.Build.Utilities;

using Xamarin.Build;
using Xamarin.Bundler;
using Xamarin.Utils;

#nullable enable

namespace Xamarin.MacDev.Tasks {
	// This task is not supposed to be remoted (it doesn't need to run on a Mac).
	public class PrepareAssemblies : XamarinTask {
		const string ErrorPrefix = "MX";

		#region Inputs
		[Required]
		public ITaskItem [] InputAssemblies { get; set; } = [];

		public string MakeReproPath { get; set; } = "";

		public string OutputDirectory { get; set; } = "";

		[Required]
		public ITaskItem? OptionsFile { get; set; }
		#endregion

		#region Outputs
		[Output]
		public ITaskItem [] OutputAssemblies { get; set; } = [];
		#endregion

		Dictionary<AssemblyPreparerInfo, ITaskItem> map = new ();

		AssemblyPreparerInfo GetAssemblyInfo (ITaskItem item)
		{
			var inputPath = item.ItemSpec;
			var outputPath = Path.Combine (OutputDirectory, Path.GetFileName (inputPath));
			var isTrimmableString = item.GetMetadata ("IsTrimmable");
			var isTrimmable = string.IsNullOrEmpty (isTrimmableString) ? (bool?) null : string.Equals (isTrimmableString, "true", StringComparison.OrdinalIgnoreCase);
			var trimMode = item.GetMetadata ("TrimMode");
			var rv = new AssemblyPreparerInfo (inputPath, outputPath, isTrimmable, trimMode);
			map [rv] = item;
			return rv;
		}

		public override bool Execute ()
		{
			// Capture Console usage and show an error if anything uses Console.[Error.]Write*
			using var consoleToLog = ConsoleToTaskWriter.EnsureNoConsoleUsage (Log);

			try {
				var infos = InputAssemblies.Select (GetAssemblyInfo).ToArray ();
				using var preparer = new AssemblyPreparer (this, infos, OptionsFile?.ItemSpec ?? "");
				preparer.MakeReproPath = MakeReproPath;
				var rv = preparer.Prepare (out var exceptions);

				foreach (var pe in exceptions) {
					if (pe.IsError (this)) {
						((IToolLog) this).LogError (pe);
					} else {
						((IToolLog) this).LogWarning (pe);
					}
				}

				var outputAssemblies = preparer.Assemblies.Select (v => {
					var item = new TaskItem (v.OutputPath);
					map [v].CopyMetadataTo (item);
					item.SetMetadata ("BeforePrepareAssembliesPath", v.InputPath);
					return (ITaskItem) item;
				}).ToList ();

				outputAssemblies.AddRange (preparer.AddedAssemblies.Select (v => {
					var rv = new TaskItem (v.Path);
					rv.SetMetadata ("PostprocessAssembly", "true");
					rv.SetMetadata ("RelativePath", preparer.Configuration.AssemblyPublishDir + Path.GetFileName (v.Path));
					if (v.OriginatingAssembly is not null) {
						var originatingItem = map.SingleOrDefault (kvp => Path.GetFileName (kvp.Key.InputPath) == Path.GetFileName (v.OriginatingAssembly)).Value;
						if (originatingItem is null) {
							Log.LogMessage (MessageImportance.Low, $"Could not find originating assembly for {v.Path} with originating assembly name {v.OriginatingAssembly}");
						} else {
							var metadata = originatingItem.MetadataNames.Cast<string> ().ToList ();
							if (metadata.Contains ("TrimMode"))
								rv.SetMetadata ("TrimMode", originatingItem.GetMetadata ("TrimMode"));
							if (metadata.Contains ("IsTrimmable"))
								rv.SetMetadata ("IsTrimmable", originatingItem.GetMetadata ("IsTrimmable"));
						}
					}
					return rv;
				}));

				OutputAssemblies = outputAssemblies.ToArray ();
				return rv && !Log.HasLoggedErrors;
			} catch (Exception e) {
				((IToolLog) this).LogException (e);
				return false;
			}
		}
	}
}
