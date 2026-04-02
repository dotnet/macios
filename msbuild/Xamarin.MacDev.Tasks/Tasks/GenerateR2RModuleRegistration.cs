using System.IO;
using System.Text;

using Microsoft.Build.Framework;

#nullable enable

namespace Xamarin.MacDev.Tasks {
	public class GenerateR2RModuleRegistration : XamarinTask {

		#region Inputs
		[Required]
		public ITaskItem [] R2RModules { get; set; } = [];

		[Required]
		public string OutputFile { get; set; } = "";
		#endregion

		public override bool Execute ()
		{
			var sb = new StringBuilder ();

			sb.AppendLine ("#include \"xamarin/xamarin.h\"");
			sb.AppendLine ();

			foreach (var module in R2RModules) {
				var symbolName = module.GetMetadata ("SymbolName");
				sb.AppendLine ($"extern void* {symbolName};");
			}

			sb.AppendLine ();
			sb.AppendLine ("static struct xamarin_r2r_module r2r_module_entries [] = {");

			foreach (var module in R2RModules) {
				var moduleName = module.GetMetadata ("ModuleName");
				var symbolName = module.GetMetadata ("SymbolName");
				sb.AppendLine ($"\t{{ \"{moduleName}\", &{symbolName} }},");
			}

			sb.AppendLine ("};");
			sb.AppendLine ();

			sb.AppendLine ("__attribute__ ((constructor))");
			sb.AppendLine ("static void xamarin_register_r2r_modules ()");
			sb.AppendLine ("{");
			sb.AppendLine ("\txamarin_r2r_modules = r2r_module_entries;");
			sb.AppendLine ("\txamarin_r2r_module_count = sizeof (r2r_module_entries) / sizeof (r2r_module_entries [0]);");
			sb.AppendLine ("}");

			var content = sb.ToString ();
			var outputDir = Path.GetDirectoryName (OutputFile);
			if (!string.IsNullOrEmpty (outputDir))
				Directory.CreateDirectory (outputDir);

			if (!File.Exists (OutputFile) || File.ReadAllText (OutputFile) != content)
				File.WriteAllText (OutputFile, content);

			return !Log.HasLoggedErrors;
		}
	}
}
