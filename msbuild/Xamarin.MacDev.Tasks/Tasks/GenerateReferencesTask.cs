using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

using Microsoft.Build.Framework;
using Microsoft.Build.Utilities;

#nullable enable

namespace Xamarin.MacDev.Tasks {
	// This task replaces the GenerateReferencesStep ILLink step.
	// It processes native symbols collected during linking and either:
	// - Generates an Objective-C reference source file (Code mode), or
	// - Passes symbols through as MSBuild items for the native linker (Linker mode)
	public class GenerateReferencesTask : XamarinTask {

		#region Inputs

		// Native symbols collected during linking (from _RequiredNativeReference items).
		// Each item has Identity = "_" + symbolName, and metadata: SymbolType (Function/ObjectiveCClass/Field), SymbolMode (Default/Linker/Code/Ignore)
		public ITaskItem [] RequiredSymbols { get; set; } = Array.Empty<ITaskItem> ();

		// The symbol mode: "Linker", "Code", or "Ignore".
		[Required]
		public string SymbolMode { get; set; } = string.Empty;

		// The directory to write the reference.m file to (used in Code mode).
		public string CacheDirectory { get; set; } = string.Empty;

		#endregion

		#region Outputs

		// In Code mode: the generated reference.m file path.
		[Output]
		public ITaskItem [] ReferencesFile { get; set; } = Array.Empty<ITaskItem> ();

		// In Linker mode: the native symbols to pass to the native linker.
		[Output]
		public ITaskItem [] NativeSymbols { get; set; } = Array.Empty<ITaskItem> ();

		#endregion

		public override bool Execute ()
		{
			// Filter out symbols with SymbolMode == Ignore
			var symbols = RequiredSymbols
				.Where (s => !string.Equals (s.GetMetadata ("SymbolMode"), "Ignore", StringComparison.OrdinalIgnoreCase))
				.ToArray ();

			switch (SymbolMode) {
			case "Ignore":
				break;
			case "Code":
				ExecuteCodeMode (symbols);
				break;
			case "Linker":
				ExecuteLinkerMode (symbols);
				break;
			default:
				Log.LogError ("Invalid symbol mode: {0}", SymbolMode);
				return false;
			}

			return !Log.HasLoggedErrors;
		}

		void ExecuteLinkerMode (ITaskItem [] symbols)
		{
			// In Linker mode, pass through the symbols as ReferenceNativeSymbol items.
			// The Identity is already in "_symbolName" format from the linker output.
			var items = new List<TaskItem> ();
			foreach (var symbol in symbols) {
				var item = new TaskItem (symbol.ItemSpec);
				item.SetMetadata ("SymbolType", symbol.GetMetadata ("SymbolType"));
				items.Add (item);
			}
			NativeSymbols = items.ToArray ();
		}

		void ExecuteCodeMode (ITaskItem [] symbols)
		{
			var reference_m = Path.Combine (CacheDirectory, "reference.m");

			if (symbols.Length == 0) {
				if (File.Exists (reference_m))
					File.Delete (reference_m);
				ReferencesFile = Array.Empty<ITaskItem> ();
				return;
			}

			var sb = new StringBuilder ();
			sb.AppendLine ("#import <Foundation/Foundation.h>");

			// Emit declarations
			foreach (var symbol in symbols) {
				var symbolType = symbol.GetMetadata ("SymbolType");
				var name = GetSymbolName (symbol);
				var objcName = GetObjectiveCName (symbol);

				switch (symbolType) {
				case "Function":
				case "Field":
					sb.Append ("extern void * ").Append (name).AppendLine (";");
					break;
				case "ObjectiveCClass":
					sb.AppendLine ($"@interface {objcName} : NSObject @end");
					break;
				default:
					Log.LogError ("Invalid symbol type {0} for symbol {1}", symbolType, symbol.ItemSpec);
					return;
				}
			}

			// Emit the referencing function
			sb.AppendLine ("static void __xamarin_symbol_referencer () __attribute__ ((used)) __attribute__ ((optnone));");
			sb.AppendLine ("void __xamarin_symbol_referencer ()");
			sb.AppendLine ("{");
			sb.AppendLine ("\tvoid *value;");

			foreach (var symbol in symbols) {
				var symbolType = symbol.GetMetadata ("SymbolType");
				var name = GetSymbolName (symbol);
				var objcName = GetObjectiveCName (symbol);

				switch (symbolType) {
				case "Function":
				case "Field":
					sb.AppendLine ($"\tvalue = {name};");
					break;
				case "ObjectiveCClass":
					sb.AppendLine ($"\tvalue = [{objcName} class];");
					break;
				default:
					Log.LogError ("Invalid symbol type {0} for symbol {1}", symbolType, symbol.ItemSpec);
					return;
				}
			}

			sb.AppendLine ("}");
			sb.AppendLine ();

			Directory.CreateDirectory (CacheDirectory);
			WriteIfDifferent (reference_m, sb.ToString ());

			ReferencesFile = new ITaskItem [] { new TaskItem (reference_m) };
		}

		// The item Identity is in "_symbolName" format (with "_" prefix).
		// For the .m file, we need the name WITHOUT the leading "_" prefix.
		static string GetSymbolName (ITaskItem symbol)
		{
			var identity = symbol.ItemSpec;
			// Strip the "_" prefix that was added by the linker step
			if (identity.StartsWith ("_", StringComparison.Ordinal))
				return identity.Substring (1);
			return identity;
		}

		const string ObjectiveCPrefix = "OBJC_CLASS_$_";

		// Extract the Objective-C class name from a symbol like "_OBJC_CLASS_$_ClassName"
		static string GetObjectiveCName (ITaskItem symbol)
		{
			var name = GetSymbolName (symbol);
			if (name.StartsWith (ObjectiveCPrefix, StringComparison.Ordinal))
				return name.Substring (ObjectiveCPrefix.Length);
			return name;
		}

		static void WriteIfDifferent (string path, string contents)
		{
			if (File.Exists (path)) {
				var existing = File.ReadAllText (path);
				if (existing == contents)
					return;
			}
			File.WriteAllText (path, contents);
		}
	}
}
