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
using Xamarin.Localization.MSBuild;
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

		public bool PostProcessing { get; set; }

		public bool? TrimExportAttributes { get; set; }

		// The original assemblies from before preparation and trimming, used during post-processing to read
		// selected registrar attributes removed during trimming.
		public ITaskItem [] PreTrimAssemblies { get; set; } = [];

		// When set (to ILC's output object file), the defined symbols in this file are used to determine
		// which UnmanagedCallersOnly trampolines survived the NativeAOT compiler (ILC). This is passed only
		// when postprocessing runs after ILC (trimmable-static registrar + NativeAOT), so that the native
		// registrar code doesn't emit direct references to trampolines ILC trimmed away.
		public string NativeAOTObjectFile { get; set; } = "";

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
			var success = false;
			var msbuildOutputFile = "";

			try {
				var infos = InputAssemblies.Select (GetAssemblyInfo).ToArray ();
				using var preparer = new AssemblyPreparer (this, infos, OptionsFile?.ItemSpec ?? "");
				msbuildOutputFile = PostProcessing ? preparer.Configuration.MSBuildPostProcessOutputFile : preparer.Configuration.MSBuildOutputFile;
				preparer.MakeReproPath = MakeReproPath;
				preparer.TrimExportAttributes = TrimExportAttributes;
				preparer.PreTrimAssemblies.AddRange (PreTrimAssemblies.Select (v => v.ItemSpec));
				bool rv;
				List<ProductException> exceptions;

				if (PostProcessing) {
					if (!string.IsNullOrEmpty (NativeAOTObjectFile)) {
						// Determine which UnmanagedCallersOnly trampolines survived ILC by inspecting the
						// defined symbols in ILC's output object file. The native symbols have a leading
						// underscore that we strip to match the managed entry-point names.
						var survivingSymbols = new HashSet<string> ();
						foreach (var symbol in Xamarin.StaticLibrary.GetDefinedSymbols (NativeAOTObjectFile)) {
							var name = symbol.StartsWith ("_", StringComparison.Ordinal) ? symbol.Substring (1) : symbol;
							survivingSymbols.Add (name);
						}
						preparer.Configuration.Application.SurvivingTrampolineSymbols = survivingSymbols;
					}
					rv = preparer.PostProcess (out exceptions);
				} else {
					rv = preparer.Prepare (out exceptions);
				}

				var totalDuration = TimeSpan.Zero;
				foreach (var step in preparer.StepExecutions) {
					totalDuration += step.Duration;
					Log.LogMessage (MessageImportance.Low, $"{step.Duration.ToString (@"hh\:mm\:ss\.fffffff")} {step.Name}: {(step.ModifiedAssemblies ? " ✏️ modified one or more assemblies" : " ✅ did not modify any assemblies")}");
				}
				Log.LogMessage (MessageImportance.Low, $"{totalDuration.ToString (@"hh\:mm\:ss\.fffffff")} Total for all steps");

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
					var relativePath = preparer.Configuration.AssemblyPublishDir + Path.GetFileName (v.Path);
					rv.SetMetadata ("PostprocessAssembly", "true");
					rv.SetMetadata ("OriginalRelativePath", relativePath);
					rv.SetMetadata ("RelativePath", relativePath);
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
				if (!rv && !Log.HasLoggedErrors)
					Log.LogError (MSBStrings.E0192);
				success = rv && !Log.HasLoggedErrors;
				return success;
			} catch (Exception e) {
				((IToolLog) this).LogException (e);
				return false;
			} finally {
				if (!success && !string.IsNullOrEmpty (msbuildOutputFile))
					File.Delete (msbuildOutputFile);
			}
		}
	}
}
