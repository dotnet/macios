// Copyright (c) Microsoft Corporation.
// Licensed under the MIT License.

using System.Collections.Generic;
using System.IO;
using System.Linq;

using Microsoft.Build.Framework;

#nullable enable

namespace Xamarin.MacDev.Tasks {
	/// <summary>
	/// Takes the list of unresolved native symbols from a NativeAOT static library and computes
	/// which inlined dlfcn native symbols survived trimming. The output file has the same format
	/// as CollectPostILTrimInformation's surviving symbols file.
	/// See docs/code/native-symbols.md for an overview of native symbol handling.
	/// </summary>
	public class ComputeNativeAOTSurvivingNativeSymbols : XamarinTask {
		/// <summary>
		/// The file listing all unresolved native symbols from the NativeAOT static library.
		/// </summary>
		[Required]
		public string UnresolvedSymbolsFile { get; set; } = "";

		/// <summary>
		/// Output file listing the native symbols that survived NativeAOT trimming.
		/// </summary>
		[Required]
		public string SurvivingNativeSymbolsFile { get; set; } = "";

		public override bool Execute ()
		{
			if (!File.Exists (UnresolvedSymbolsFile))
				return !Log.HasLoggedErrors;

			const string prefix = "_xamarin_Dlfcn_";
			const string suffix = "_Native";
			var survivingSymbols = new HashSet<string> ();

			foreach (var sym in File.ReadAllLines (UnresolvedSymbolsFile)) {
				if (!sym.StartsWith (prefix) || !sym.EndsWith (suffix))
					continue;
				var symbolLength = sym.Length - prefix.Length - suffix.Length;
				if (symbolLength <= 0)
					continue;
				var symbolName = sym.Substring (prefix.Length, symbolLength);
				survivingSymbols.Add (symbolName);
			}

			var sorted = survivingSymbols.OrderBy (s => s).ToArray ();

			if (File.Exists (SurvivingNativeSymbolsFile)) {
				var existing = File.ReadAllLines (SurvivingNativeSymbolsFile);
				if (existing.SequenceEqual (sorted))
					return !Log.HasLoggedErrors;
			}

			var dir = Path.GetDirectoryName (SurvivingNativeSymbolsFile);
			if (!string.IsNullOrEmpty (dir))
				Directory.CreateDirectory (dir);
			File.WriteAllLines (SurvivingNativeSymbolsFile, sorted);
			Log.LogMessage (MessageImportance.Low, "Found {0} surviving native symbols from NativeAOT", survivingSymbols.Count);

			return !Log.HasLoggedErrors;
		}
	}
}
