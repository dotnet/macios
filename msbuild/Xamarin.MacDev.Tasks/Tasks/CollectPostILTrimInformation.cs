// Copyright (c) Microsoft Corporation.
// Licensed under the MIT License.

using System.Collections.Generic;
using System.IO;
using System.Linq;

using Microsoft.Build.Framework;

using Mono.Cecil;

using Xamarin.Bundler;
using Xamarin.Utils;

#nullable enable

namespace Xamarin.MacDev.Tasks {
	/// <summary>
	/// Scans trimmed assemblies to collect information that survived trimming.
	/// See docs/code/native-symbols.md and docs/code/class-handles.md for an overview of native symbol handling.
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
		/// Output file listing the Class.GetHandle calls that survived trimming.
		/// </summary>
		[Required]
		public string SurvivingClassesFile { get; set; } = "";

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
				var cacheFile = Path.Combine (CacheDirectory, assemblyName + ".internal-symbols.cache");

				string []? cachedSymbols = null;
				if (File.Exists (cacheFile) && File.GetLastWriteTimeUtc (cacheFile) >= File.GetLastWriteTimeUtc (assemblyPath)) {
					cachedSymbols = File.ReadAllLines (cacheFile);
					Log.LogMessage (MessageImportance.Low, "Using cached internal symbols for {0}", assemblyName);

					survivingSymbols.UnionWith (cachedSymbols);
				} else {
					var assemblySymbols = new HashSet<string> ();
					CollectInternalSymbolsFromAssembly (assemblyPath, assemblySymbols);

					// Write per-assembly cache (sorted for stability).
					var sortedAssemblySymbols = assemblySymbols.OrderBy (s => s).ToArray ();
					File.WriteAllLines (cacheFile, sortedAssemblySymbols);

					survivingSymbols.UnionWith (assemblySymbols);
				}
			}

			WriteSymbolsToFile (this, SurvivingNativeSymbolsFile, FilterToSurvivingNativeSymbols (survivingSymbols));
			WriteSymbolsToFile (this, SurvivingClassesFile, FilterToClassSymbols (survivingSymbols));
		}

		public static void WriteSymbolsToFile (XamarinTask task, string file, IEnumerable<string> unsortedSymbols)
		{
			// Write the combined results only if contents changed (sorted for stability).
			var sorted = unsortedSymbols.OrderBy (s => s).ToArray ();

			if (File.Exists (file)) {
				var existing = File.ReadAllLines (file);
				if (existing.SequenceEqual (sorted)) {
					task.Log.LogMessage (MessageImportance.Low, "The file {0} is already up-to-date with {1} symbols", file, sorted.Length);
					return;
				}
			}

			PathUtils.CreateDirectoryForFile (file);
			File.WriteAllLines (file, sorted);
			task.Log.LogMessage (MessageImportance.Low, "Wrote {0} symbols to {1}", sorted.Length, file);
		}

		public static IEnumerable<string> FilterToDlfcnSymbols (IEnumerable<string> symbols)
		{
			return FilterTo (symbols, "_xamarin_Dlfcn_", "_Native");
		}

		public static IEnumerable<string> FilterToSurvivingNativeSymbols (IEnumerable<string> symbols)
		{
			return FilterToDlfcnSymbols (symbols).Concat (
				FilterTo (symbols, "_xamarin_CategoryFramework_", "_Native")
					.Select (framework => $"xamarin_CategoryFramework_{framework}_Native"));
		}

		// Category lookups are returned as "{framework}/{class}". '/' can't be part of an Objective-C class name.
		public static IEnumerable<string> FilterToClassSymbols (IEnumerable<string> symbols)
		{
			var categorySymbols = FilterTo (symbols, "_xamarin_Category_GetHandle_", "_Native")
				.Select (symbol => {
					var separator = symbol.IndexOf ('_');
					return separator > 0 && separator < symbol.Length - 1 ? symbol.Substring (0, separator) + "/" + symbol.Substring (separator + 1) : "";
				})
				.Where (symbol => symbol.Length > 0);
			return FilterTo (symbols, "_xamarin_Class_GetHandle_", "_Native").Concat (categorySymbols);
		}

		public static string GetClassName (string classSymbol)
		{
			var separator = classSymbol.IndexOf ('/');
			return separator < 0 ? classSymbol : classSymbol.Substring (separator + 1);
		}

		public static bool TryGetCategoryFramework (string classSymbol, out string framework)
		{
			var separator = classSymbol.IndexOf ('/');
			framework = separator < 0 ? "" : classSymbol.Substring (0, separator);
			return separator >= 0;
		}

		static IEnumerable<string> FilterTo (IEnumerable<string> symbols, string prefix, string suffix)
		{
			return symbols
				.Where (symbol => symbol.StartsWith (prefix, StringComparison.Ordinal) && symbol.EndsWith (suffix, StringComparison.Ordinal))
				.Select (symbol => symbol.Substring (prefix.Length, symbol.Length - prefix.Length - suffix.Length));
		}

		void CollectInternalSymbolsFromAssembly (string assemblyPath, HashSet<string> survivingSymbols)
		{
			AssemblyDefinition assembly;
			try {
				assembly = AssemblyDefinition.ReadAssembly (assemblyPath, new ReaderParameters { ReadSymbols = false });
			} catch (BadImageFormatException) {
				// The input list of assemblies can contain native libraries with a managed extension
				// (for example a native 'NativeLibrary.dll'), which aren't managed assemblies. Just skip
				// those, they can't contain any managed P/Invokes.
				Log.LogMessage (MessageImportance.Low, "Skipping '{0}' because it's not a managed assembly.", assemblyPath);
				return;
			}

			using (assembly) {
				foreach (var module in assembly.Modules) {
					if (!module.HasModuleReferences)
						continue;
					if (!module.ModuleReferences.Any (mr => mr.Name == "__Internal"))
						continue;
					foreach (var type in module.Types) {
						if (!type.HasMethods)
							continue;
						foreach (var method in type.Methods) {
							if (!method.IsPInvokeImpl)
								continue;
							if (method.PInvokeInfo?.Module?.Name != "__Internal")
								continue;
							survivingSymbols.Add (Symbol.Prefix + (method.PInvokeInfo.EntryPoint ?? method.Name));
						}
					}
				}
			}
		}
	}
}
