// Copyright (c) Microsoft Corporation.
// Licensed under the MIT License.

using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

using Microsoft.Build.Framework;

using Xamarin.Bundler;

#nullable enable

namespace Xamarin.MacDev.Tasks {
	/// <summary>
	/// Performs post-trimming processing, generating native code only for symbols that survived trimming.
	/// See docs/code/native-symbols.md for an overview of native symbol handling.
	/// </summary>
	public class PostTrimmingProcessing : XamarinTask {
		[Required]
		public string Architecture { get; set; } = "";

		[Required]
		public string OutputDirectory { get; set; } = "";

		public ITaskItem [] ReferenceNativeSymbol { get; set; } = [];

		/// <summary>
		/// Files listing native symbols that survived trimming. Each file contains one symbol name per line.
		/// These can come from either ILTrim (CollectPostILTrimInformation) or NativeAOT
		/// (ComputeNativeAOTSurvivingNativeSymbols).
		/// </summary>
		public ITaskItem [] SurvivingNativeSymbolsFiles { get; set; } = [];

		/// <summary>
		/// Output native source files to be compiled and linked.
		/// </summary>
		[Output]
		public ITaskItem []? NativeSourceFiles { get; set; }

		HashSet<string>? ignoredSymbols;

		HashSet<string> IgnoredSymbols {
			get {
				if (ignoredSymbols is null) {
					ignoredSymbols = new HashSet<string> ();
					foreach (var rns in ReferenceNativeSymbol) {
						var nativeSymbol = rns.ItemSpec;
						var symbolMode = rns.GetMetadata ("SymbolMode");
						if (!string.Equals (symbolMode, "Ignore", StringComparison.OrdinalIgnoreCase))
							continue;
						var symbolType = rns.GetMetadata ("SymbolType").ToLowerInvariant ();
						switch (symbolType) {
						case "objectivecclass":
							nativeSymbol = Symbol.ObjectiveCPrefix + nativeSymbol;
							break;
						case "function":
						case "field":
							break;
						default:
							Log.LogMessage (MessageImportance.Low, "Ignoring symbol '{0}' with unknown SymbolType '{1}'", nativeSymbol, symbolType);
							continue;
						}
						ignoredSymbols.Add (nativeSymbol);
					}
				}
				return ignoredSymbols;
			}
		}

		public override bool Execute ()
		{
			var items = new List<ITaskItem> ();

			GenerateInlinedDlfcnNativeCode (items);

			NativeSourceFiles = items.ToArray ();
			return !Log.HasLoggedErrors;
		}

		void GenerateInlinedDlfcnNativeCode (List<ITaskItem> items)
		{
			// Collect all surviving symbols from all input files.
			var survivingSymbols = new HashSet<string> ();
			foreach (var file in SurvivingNativeSymbolsFiles) {
				var path = file.ItemSpec;
				if (!File.Exists (path))
					continue;
				survivingSymbols.UnionWith (File.ReadAllLines (path));
			}

			var survivingButIgnoredSymbols = survivingSymbols.Intersect (IgnoredSymbols).ToList ();
			if (survivingButIgnoredSymbols.Count > 0) {
				Log.LogMessage (MessageImportance.Low, "The following symbols survived trimming but are marked as ignored:");
				foreach (var symbol in survivingButIgnoredSymbols)
					Log.LogMessage (MessageImportance.Low, "  {0}", symbol);
				survivingSymbols.ExceptWith (survivingButIgnoredSymbols);
			}

			if (survivingSymbols.Count == 0) {
				Log.LogMessage (MessageImportance.Low, "There were no surviving symbols that require inlined dlfcn native code.");
				return;
			}

			Directory.CreateDirectory (OutputDirectory);
			var outputPath = Path.Combine (OutputDirectory, "inlined-dlfcn.c");

			var sb = new StringBuilder ();
			// The generated C code uses 'extern void*' declarations and returns the address of the symbol.
			// This is intentional: it allows the native linker to resolve the symbol at link time, which
			// is the whole point of this optimization (avoiding dlsym at runtime).
			foreach (var field in survivingSymbols.OrderBy (s => s)) {
				// Using 'void*' as a stand-in type since we only need the address
				sb.AppendLine ($"extern void* {field};");
				sb.AppendLine ($"void* xamarin_Dlfcn_{field}_Native ();");
				sb.AppendLine ($"void* xamarin_Dlfcn_{field}_Native () {{ return &{field}; }}");
				sb.AppendLine ();
			}

			var content = sb.ToString ();
			if (File.Exists (outputPath) && File.ReadAllText (outputPath) == content) {
				Log.LogMessage (MessageImportance.Low, "Inlined dlfcn native code is up to date");
			} else {
				File.WriteAllText (outputPath, content);
				Log.LogMessage (MessageImportance.Low, "Generated inlined dlfcn native code with {0} symbols", survivingSymbols.Count);
			}

			var item = new Microsoft.Build.Utilities.TaskItem (outputPath);
			item.SetMetadata ("Arch", Architecture.ToLowerInvariant ());
			items.Add (item);
		}
	}
}
