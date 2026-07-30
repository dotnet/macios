// Copyright (c) Microsoft Corporation.
// Licensed under the MIT License.

using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

using Microsoft.Build.Framework;
using Microsoft.Build.Utilities;

using Xamarin.Utils;

#nullable enable

namespace Xamarin.MacDev.Tasks {
	public class GenerateTrustedPlatformAssemblySource : XamarinTask {
		[Required]
		public ITaskItem [] Assemblies { get; set; } = [];

		[Required]
		public string AssemblyDirectory { get; set; } = "";

		public bool IsMultiRidBuild { get; set; }

		[Required]
		public ITaskItem [] MainFiles { get; set; } = [];

		[Required]
		public string OutputDirectory { get; set; } = "";

		[Output]
		public ITaskItem [] NativeSourceFiles { get; set; } = [];

		public override bool Execute ()
		{
			var assemblyDirectory = NormalizePath (AssemblyDirectory).TrimEnd ('/');
			var assemblyNames = Assemblies
				.Select (v => NormalizePath (v.GetMetadata ("RelativePath")))
				.Where (v => string.Equals (Path.GetDirectoryName (v), assemblyDirectory, StringComparison.Ordinal))
				.Where (v => {
					var extension = Path.GetExtension (v);
					return string.Equals (extension, ".dll", StringComparison.OrdinalIgnoreCase) || string.Equals (extension, ".exe", StringComparison.OrdinalIgnoreCase);
				})
				.Select (GetFileName)
				.Distinct (StringComparer.Ordinal)
				// Any .exe files must be at the end, due to https://github.com/dotnet/runtime/issues/62735
				.OrderBy (v => Path.GetExtension (v).Equals (".exe", StringComparison.OrdinalIgnoreCase))
				.ThenBy (v => v, StringComparer.Ordinal)
				.ToArray ();
			var architectures = MainFiles
				.Select (v => v.GetMetadata ("Arch"))
				.Distinct (StringComparer.Ordinal)
				.ToArray ();

			if (architectures.Any (string.IsNullOrEmpty)) {
				Log.LogError ("The generated main source file does not specify an architecture.");
				return false;
			}

			Directory.CreateDirectory (OutputDirectory);
			var sourceFiles = new List<ITaskItem> ();
			foreach (var architecture in architectures) {
				var outputPath = Path.Combine (OutputDirectory, $"trusted-platform-assemblies.{architecture}.m");
				FileUtils.WriteIfDifferent (outputPath, GenerateSource (assemblyNames), message => Log.LogMessage (MessageImportance.Low, message));

				var item = new TaskItem (outputPath);
				item.SetMetadata ("Arch", architecture);
				sourceFiles.Add (item);
			}

			NativeSourceFiles = sourceFiles.ToArray ();
			return !Log.HasLoggedErrors;
		}

		string GenerateSource (IEnumerable<string> assemblyNames)
		{
			var sb = new StringBuilder ();
			sb.AppendLine ("// Copyright (c) Microsoft Corporation.");
			sb.AppendLine ("// Licensed under the MIT License.");
			sb.AppendLine ();
			sb.AppendLine ("#include \"xamarin/xamarin.h\"");
			sb.AppendLine ();
			sb.AppendLine ("#include <stdbool.h>");
			sb.AppendLine ("#include <stddef.h>");
			sb.AppendLine ();
			sb.Append ("const char *xamarin_trusted_platform_assemblies = \"");
			sb.Append (string.Join (":", assemblyNames.Select (EscapeString)));
			sb.AppendLine ("\";");
			sb.Append ("const size_t xamarin_trusted_platform_assembly_count = ").Append (assemblyNames.Count ()).AppendLine (";");
			sb.AppendLine ();
			sb.AppendLine ("#if defined (SUPPORTS_UNIVERSAL_BUILDS)");
			sb.Append ("const bool xamarin_is_multi_rid_build = ").Append (IsMultiRidBuild ? "true" : "false").AppendLine (";");
			sb.AppendLine ("#endif");
			return sb.ToString ();
		}

		static string NormalizePath (string value)
		{
			return value.Replace ('\\', '/');
		}

		static string GetFileName (string path)
		{
			var fileName = Path.GetFileName (path);
			if (fileName is null)
				throw new InvalidOperationException ($"Could not get the file name for '{path}'.");
			return fileName;
		}

		static string EscapeString (string value)
		{
			var sb = new StringBuilder ();
			foreach (var b in Encoding.UTF8.GetBytes (value)) {
				switch (b) {
				case (byte) '\\':
					sb.Append ("\\\\");
					break;
				case (byte) '"':
					sb.Append ("\\\"");
					break;
				default:
					if (b >= 0x20 && b <= 0x7e) {
						sb.Append ((char) b);
					} else {
						sb.Append ('\\').Append (Convert.ToString (b, 8).PadLeft (3, '0'));
					}
					break;
				}
			}
			return sb.ToString ();
		}
	}
}
