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
	/// See docs/code/native-symbols.md and docs/code/class-handles.md for an overview of native symbol handling.
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

		/// <summary>
		/// Output file listing the Class.GetHandle calls that survived trimming.
		/// </summary>
		[Required]
		public string SurvivingClassesFile { get; set; } = "";

		public override bool Execute ()
		{
			var unresolvedSymbols = File.Exists (UnresolvedSymbolsFile) ? File.ReadAllLines (UnresolvedSymbolsFile) : [];
			CollectPostILTrimInformation.WriteSymbolsToFile (this, SurvivingNativeSymbolsFile, CollectPostILTrimInformation.FilterToDlfcnSymbols (unresolvedSymbols));
			CollectPostILTrimInformation.WriteSymbolsToFile (this, SurvivingClassesFile, CollectPostILTrimInformation.FilterToClassSymbols (unresolvedSymbols));
			return !Log.HasLoggedErrors;
		}
	}
}
