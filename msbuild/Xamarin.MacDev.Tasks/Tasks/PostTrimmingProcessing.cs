// Copyright (c) Microsoft Corporation.
// Licensed under the MIT License.

using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

using Microsoft.Build.Framework;

using Xamarin.Bundler;
using Xamarin.Utils;

#nullable enable

namespace Xamarin.MacDev.Tasks {
	/// <summary>
	/// Performs post-trimming processing, generating native code only for symbols that survived trimming.
	/// See docs/code/native-symbols.md and docs/code/class-handles.md for an overview of native symbol handling.
	/// </summary>
	public class PostTrimmingProcessing : XamarinTask {
		[Required]
		public string Architecture { get; set; } = "";

		[Required]
		public string OutputDirectory { get; set; } = "";

		public ITaskItem [] ReferenceNativeSymbol { get; set; } = [];

		/// <summary>
		/// Files listing calls to Class.GetHandle that survived trimming. Each file contains one symbol name per line.
		/// These can come from either ILTrim (CollectPostILTrimInformation) or NativeAOT
		/// (ComputeNativeAOTSurvivingNativeSymbols).
		/// </summary>
		public ITaskItem [] SurvivingClassesFiles { get; set; } = [];

		/// <summary>
		/// Files listing native symbols that survived trimming. Each file contains one symbol name per line.
		/// These can come from either ILTrim (CollectPostILTrimInformation) or NativeAOT
		/// (ComputeNativeAOTSurvivingNativeSymbols).
		/// </summary>
		public ITaskItem [] SurvivingNativeSymbolsFiles { get; set; } = [];

		// Type map
		public string TypeMapFilePath { get; set; } = "";

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
			var nativeSourceFiles = new List<string> ();

			Directory.CreateDirectory (OutputDirectory);
			GenerateInlinedDlfcnNativeCode (nativeSourceFiles);
			GenerateInlinedClassGetHandleNativeCode (nativeSourceFiles);

			NativeSourceFiles = nativeSourceFiles.Select (path => {
				var item = new Microsoft.Build.Utilities.TaskItem (path);
				item.SetMetadata ("Arch", Architecture.ToLowerInvariant ());
				return item;
			}).ToArray ();

			return !Log.HasLoggedErrors;
		}

		static HashSet<string> ReadUniqueLinesFromFiles (IEnumerable<ITaskItem> files)
		{
			var lines = new HashSet<string> ();
			foreach (var file in files) {
				var path = file.ItemSpec;
				if (!File.Exists (path))
					continue;
				lines.UnionWith (File.ReadAllLines (path));
			}
			return lines;
		}

		List<string> FilterOutIgnoredSymbols (HashSet<string> survivingSymbols, bool filterObjectiveCClasses)
		{
			var rv = new HashSet<string> (survivingSymbols);

			foreach (var rns in ReferenceNativeSymbol) {
				var nativeSymbol = rns.ItemSpec;
				var symbolMode = rns.GetMetadata ("SymbolMode");
				if (!string.Equals (symbolMode, "Ignore", StringComparison.OrdinalIgnoreCase))
					continue;

				var symbolType = rns.GetMetadata ("SymbolType").ToLowerInvariant ();
				switch (symbolType) {
				case "objectivecclass":
					if (filterObjectiveCClasses && rv.Remove (nativeSymbol)) {
						Log.LogMessage (MessageImportance.Low, "Ignoring Objective-C class '{0}'", nativeSymbol);
					}
					break;
				case "function":
				case "field":
					if (!filterObjectiveCClasses && rv.Remove (nativeSymbol)) {
						Log.LogMessage (MessageImportance.Low, "Ignoring native symbol '{0}'", nativeSymbol);
					}
					break;
				default:
					Log.LogMessage (MessageImportance.Low, "Ignoring symbol '{0}' with unknown SymbolType '{1}'", nativeSymbol, symbolType);
					continue;
				}
			}

			rv.Remove (""); // no empty symbols

			return rv.OrderBy (v => v).ToList ();
		}

		void GenerateInlinedClassGetHandleNativeCode (List<string> items)
		{
			// Collect all surviving symbols from all input files.
			var classes = FilterOutIgnoredSymbols (ReadUniqueLinesFromFiles (SurvivingClassesFiles), filterObjectiveCClasses: true);

			if (classes.Count == 0) {
				Log.LogMessage (MessageImportance.Low, "There were no surviving Objective-C classes that require inlined Class.GetHandle native code.");
				return;
			}

			if (string.IsNullOrEmpty (TypeMapFilePath) || !File.Exists (TypeMapFilePath)) {
				Log.LogError ("The type map file '{0}' does not exist. This file is generated by the linker's CoreTypeMapStep. Ensure that trimming ran successfully.", TypeMapFilePath ?? "");
				return;
			}

			var typeMapEntries = File.ReadAllLines (TypeMapFilePath)
				.Select (line => {
					var parts = line.Split ('|');
					string className = "";
					string framework = "";
					string introduced = "";
					bool iswrapper = false;
					bool isstubclass = false;
					foreach (var part in parts) {
						var kvp = part.Split (new char [] { '=' }, 2);
						if (kvp.Length != 2)
							continue;
						var key = kvp [0].Trim ();
						var value = kvp [1].Trim ();
						switch (key) {
						case "Class":
							className = value;
							break;
						case "Framework":
							framework = value;
							break;
						case "Introduced":
							introduced = value;
							break;
						case "IsWrapper":
							iswrapper = string.Equals (value, "true", StringComparison.OrdinalIgnoreCase);
							break;
						case "IsStubClass":
							isstubclass = string.Equals (value, "true", StringComparison.OrdinalIgnoreCase);
							break;
						}
					}
					return (Class: className, Framework: framework, Introduced: introduced, IsWrapper: iswrapper, IsStubClass: isstubclass);
				})
				.ToArray ();

			var typeMap = new Dictionary<string, (string Class, string Framework, string Introduced, bool IsWrapper, bool IsStubClass)> ();
			foreach (var entry in typeMapEntries) {
				if (string.IsNullOrEmpty (entry.Class))
					continue;
				if (typeMap.ContainsKey (entry.Class)) {
					Log.LogError ("Duplicate class '{0}' found in the type map file '{1}'.", entry.Class, TypeMapFilePath);
					return;
				}
				typeMap [entry.Class] = entry;
			}

			var sb = new StringBuilder ();
			sb.AppendLine ($"#include <objc/runtime.h>");
			sb.AppendLine ($"#include <Foundation/Foundation.h>");
			foreach (var objectiveCClassName in classes) {
				// We don't want to import every header under the sun to find the @interface definitions for each class, so we generate
				// a forward declaration for each class. To avoid potential issues with missing classes at runtime, we mark each declaration with __attribute__((weak_import)).
				// The only exception is that we need to #include Foundation, which means we can't create declarations for Foundation classes.
				if (!typeMap.TryGetValue (objectiveCClassName, out var info)) {
					sb.AppendLine ($"__attribute__((weak_import)) @interface {objectiveCClassName} : NSObject @end // no objc type found");
				} else if (info.IsWrapper && info.Framework == "Foundation") {
					// This is a special case for wrapper classes in the Foundation framework. Since we need to #include Foundation, we can't create a forward declaration for these classes. However, since they are wrappers, we know they won't be missing at runtime, so we don't need to mark them with __attribute__((weak_import)).
					sb.AppendLine ($"// The class '{objectiveCClassName}' comes from the Foundation framework, so no generated @interface declaration.");
				} else {
					if (info.IsStubClass)
						sb.AppendLine ("__attribute__((objc_class_stub)) __attribute__((objc_subclassing_restricted))");
					sb.AppendLine ($"__attribute__((weak_import)) @interface {objectiveCClassName} : NSObject @end // is stub: {info.IsStubClass}");
				}
				sb.AppendLine ($"Class xamarin_Class_GetHandle_{objectiveCClassName}_Native ();");
				sb.AppendLine ($"Class xamarin_Class_GetHandle_{objectiveCClassName}_Native () {{ return [{objectiveCClassName} class]; }}");
				sb.AppendLine ();
			}

			var outputPath = Path.Combine (OutputDirectory, "inlined-class-gethandle.m");
			FileUtils.WriteIfDifferent (outputPath, sb.ToString (), (msg) => Log.LogMessage (MessageImportance.Low, msg));

			items.Add (outputPath);
		}

		void GenerateInlinedDlfcnNativeCode (List<string> items)
		{
			var survivingSymbols = FilterOutIgnoredSymbols (ReadUniqueLinesFromFiles (SurvivingNativeSymbolsFiles), filterObjectiveCClasses: false);

			if (survivingSymbols.Count == 0) {
				Log.LogMessage (MessageImportance.Low, "There were no surviving symbols that require inlined dlfcn native code.");
				return;
			}

			var sb = new StringBuilder ();
			// The generated C code uses 'extern void*' declarations and returns the address of the symbol.
			// This is intentional: it allows the native linker to resolve the symbol at link time, which
			// is the whole point of this optimization (avoiding dlsym at runtime).
			//
			// The symbols are declared with __attribute__((weak_import)), and we fall back to dlsym if the
			// direct reference turns out to be null at runtime. There are two reasons for this:
			//
			// * The symbol might not exist at runtime even though it existed in the SDK we built against
			//   (for example when the app runs on an older OS than the one it was built with). Without
			//   weak_import a missing symbol makes dyld abort at launch; with it the reference resolves to
			//   null instead.
			//
			// * A symbol can be exported by more than one framework in the SDK (for example
			//   UIFontTextStyleBody is exported by both UIKit and AppKit in the macOS SDK). In that case the
			//   native linker binds our reference to one of them (the order isn't specified), which might not
			//   be the framework that actually exports the symbol at runtime on older OSes. When the bound
			//   framework doesn't have it, we use dlsym (RTLD_DEFAULT) to find it in whichever loaded
			//   framework does, restoring the behavior of the non-inlined dlfcn code path.
			//
			// The fast path (a direct reference when the symbol is present) is unchanged, and the managed
			// caller caches the result, so the dlsym fallback runs at most once per symbol.
			sb.AppendLine ("#include <dlfcn.h>");
			sb.AppendLine ();
			sb.AppendLine ("static void* xamarin_dlfcn_fallback (void* ptr, const char* symbol) { return ptr ? ptr : dlsym (RTLD_DEFAULT, symbol); }");
			sb.AppendLine ();
			foreach (var field in survivingSymbols.OrderBy (s => s)) {
				// Using 'void*' as a stand-in type since we only need the address
				sb.AppendLine ($"extern void* {field} __attribute__((weak_import));");
				sb.AppendLine ($"void* xamarin_Dlfcn_{field}_Native ();");
				sb.AppendLine ($"void* xamarin_Dlfcn_{field}_Native () {{ return xamarin_dlfcn_fallback (&{field}, \"{field}\"); }}");
				sb.AppendLine ();
			}

			var outputPath = Path.Combine (OutputDirectory, "inlined-dlfcn.c");
			FileUtils.WriteIfDifferent (outputPath, sb.ToString (), (msg) => Log.LogMessage (MessageImportance.Low, msg));

			items.Add (outputPath);
		}
	}
}
