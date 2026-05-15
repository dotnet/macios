// Copyright (c) Microsoft Corporation.
// Licensed under the MIT License.

using System.Collections.Generic;
using System.IO;
using System.Linq;

using Microsoft.Build.Framework;

using Mono.Cecil;

#nullable enable

namespace Xamarin.MacDev.Tasks {
	/// <summary>
	/// Scans trimmed assemblies to collect information that survived trimming.
	/// See docs/code/native-symbols.md for an overview of native symbol handling.
	/// </summary>
	public class CollectPostILTrimInformation : XamarinTask {
		[Required]
		public ITaskItem [] TrimmedAssemblies { get; set; } = [];

		/// <summary>
		/// Output file listing the inlined dlfcn symbols that survived trimming.
		/// </summary>
		[Required]
		public string SurvivingNativeSymbolsFile { get; set; } = "";

		/// <summary>
		/// Directory for per-assembly cache files, to avoid re-scanning unchanged assemblies.
		/// </summary>
		[Required]
		public string CacheDirectory { get; set; } = "";

		public override bool Execute ()
		{
			Directory.CreateDirectory (CacheDirectory);

			CollectSurvivingNativeSymbols ();

			return !Log.HasLoggedErrors;
		}

		void CollectSurvivingNativeSymbols ()
		{
			// Scan trimmed assemblies for surviving P/Invoke methods, using per-assembly caching.
			var survivingSymbols = new HashSet<string> ();
			foreach (var item in TrimmedAssemblies) {
				var assemblyPath = item.ItemSpec;
				if (!File.Exists (assemblyPath))
					continue;

				var assemblyName = Path.GetFileNameWithoutExtension (assemblyPath);
				var cacheFile = Path.Combine (CacheDirectory, assemblyName + ".dlfcn-symbols.cache");

				string []? cachedSymbols = null;
				if (File.Exists (cacheFile) && File.GetLastWriteTimeUtc (cacheFile) >= File.GetLastWriteTimeUtc (assemblyPath)) {
					cachedSymbols = File.ReadAllLines (cacheFile);
					Log.LogMessage (MessageImportance.Low, "Using cached dlfcn symbols for {0}", assemblyName);

					survivingSymbols.UnionWith (cachedSymbols);
				} else {
					var assemblySymbols = new HashSet<string> ();
					CollectDlfcnSymbolsFromAssembly (assemblyPath, assemblySymbols);

					// Write per-assembly cache (sorted for stability).
					var sortedAssemblySymbols = assemblySymbols.OrderBy (s => s).ToArray ();
					File.WriteAllLines (cacheFile, sortedAssemblySymbols);

					survivingSymbols.UnionWith (assemblySymbols);
				}
			}

			// Write the combined results only if contents changed (sorted for stability).
			var sorted = survivingSymbols.OrderBy (s => s).ToArray ();

			if (File.Exists (SurvivingNativeSymbolsFile)) {
				var existing = File.ReadAllLines (SurvivingNativeSymbolsFile);
				if (existing.SequenceEqual (sorted))
					return;
			}

			var dir = Path.GetDirectoryName (SurvivingNativeSymbolsFile);
			if (!string.IsNullOrEmpty (dir))
				Directory.CreateDirectory (dir);
			File.WriteAllLines (SurvivingNativeSymbolsFile, sorted);
			Log.LogMessage (MessageImportance.Low, "Found {0} surviving inlined dlfcn symbols", survivingSymbols.Count);
		}

		static void CollectDlfcnSymbolsFromAssembly (string assemblyPath, HashSet<string> survivingSymbols)
		{
			const string prefix = "xamarin_Dlfcn_";
			const string suffix = "_Native";

			using var assembly = AssemblyDefinition.ReadAssembly (assemblyPath, new ReaderParameters { ReadSymbols = false });
			foreach (var module in assembly.Modules) {
				foreach (var type in module.Types) {
					if (!type.HasMethods)
						continue;
					foreach (var method in type.Methods) {
						if (!method.IsPInvokeImpl)
							continue;
						if (method.PInvokeInfo?.Module?.Name != "__Internal")
							continue;
						var name = method.Name;
						if (!name.StartsWith (prefix) || !name.EndsWith (suffix))
							continue;
						var symbolLength = name.Length - prefix.Length - suffix.Length;
						if (symbolLength <= 0)
							continue;
						var symbolName = name.Substring (prefix.Length, symbolLength);
						survivingSymbols.Add (symbolName);
					}
				}
			}
		}
	}
}
