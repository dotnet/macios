﻿using System;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Collections.Generic;

namespace BCLTestImporter {
	public class ProjectGenerator {

		static readonly string NameKey = "%NAME%";
		static readonly string ReferencesKey = "%REFERENCES%";
		static readonly string RegisterTypeKey = "%REGISTER TYPE%";

		//list of reference that we are already adding, and we do not want to readd (although it is just a warning)
		static readonly List<string> excludeDlls = new List<string> {
			"mscorlib",
			"nunitlite",
			"System",
			"System.Xml",
			"System.Xml.Linq",
		};

		// this can be grouped TODO
		static readonly List <TestProjectDefinition> iOSTestProjects = new List <TestProjectDefinition> {
			new TestProjectDefinition ("System", new List<TestAssemblyDefinition> { new TestAssemblyDefinition ("MONOTOUCH_System_test.dll")} ),
			new TestProjectDefinition ("SystemCoreTests", new List<TestAssemblyDefinition> {new TestAssemblyDefinition ("MONOTOUCH_System.Core_test.dll")} ),
			new TestProjectDefinition ("SystemDataTests", new List<TestAssemblyDefinition> {new TestAssemblyDefinition ("MONOTOUCH_System.Data_test.dll")} ),
			new TestProjectDefinition ("SystemNet.HttpTests", new List<TestAssemblyDefinition> {new TestAssemblyDefinition ("MONOTOUCH_System.Net.Http_test.dll")} ),
			new TestProjectDefinition ("SystemNumericsTests", new List<TestAssemblyDefinition> {new TestAssemblyDefinition ("MONOTOUCH_System.Numerics_test.dll")} ),
			new TestProjectDefinition ("SystemRuntimeSerializationTests", new List<TestAssemblyDefinition> {new TestAssemblyDefinition ("MONOTOUCH_System.Runtime.Serialization_test.dll")} ),
			new TestProjectDefinition ("SystemTransactionsTests", new List<TestAssemblyDefinition> {new TestAssemblyDefinition ("MONOTOUCH_System.Transactions_test.dll")} ),
			new TestProjectDefinition ("SystemXmlTests", new List<TestAssemblyDefinition> {new TestAssemblyDefinition ("MONOTOUCH_System.Xml_test.dll")} ),
			new TestProjectDefinition ("SystemXmlLinqTests", new List<TestAssemblyDefinition> {new TestAssemblyDefinition ("MONOTOUCH_System.Xml.Linq_test.dll")} ),
			new TestProjectDefinition ("MonoSecurityTests", new List<TestAssemblyDefinition> {new TestAssemblyDefinition ("MONOTOUCH_Mono.Security_test.dll")} ),
			new TestProjectDefinition ("SystemComponentModelDataAnnotationTests", new List<TestAssemblyDefinition> {new TestAssemblyDefinition ("MONOTOUCH_System.ComponentModel.DataAnnotations_test.dll")} ),
			new TestProjectDefinition ("SystemJsonTests", new List<TestAssemblyDefinition> {new TestAssemblyDefinition ("MONOTOUCH_System.Json_test.dll")} ),
			new TestProjectDefinition ("SystemServiceModelWebTests", new List<TestAssemblyDefinition> {new TestAssemblyDefinition ("MONOTOUCH_System.ServiceModel.Web_test.dll")} ),
			new TestProjectDefinition ("MonoDataTdsTests", new List<TestAssemblyDefinition> {new TestAssemblyDefinition ("MONOTOUCH_Mono.Data.Tds_test.dll")} ),
			new TestProjectDefinition ("SystemIOCompressionTests", new List<TestAssemblyDefinition> {new TestAssemblyDefinition ("MONOTOUCH_System.IO.Compression_test.dll")} ),
			new TestProjectDefinition ("SystemIOCompression.FileSystemTests", new List<TestAssemblyDefinition> {new TestAssemblyDefinition ("MONOTOUCH_System.IO.Compression.FileSystem_test.dll")} ),
			new TestProjectDefinition ("MonoCSharpTests", new List<TestAssemblyDefinition> {new TestAssemblyDefinition ("MONOTOUCH_Mono.CSharp_test.dll")} ),
			new TestProjectDefinition ("SystemSecurityTests", new List<TestAssemblyDefinition> {new TestAssemblyDefinition ("MONOTOUCH_System.Security_test.dll")} ),
			new TestProjectDefinition ("SystemServiceModelTests", new List<TestAssemblyDefinition> {new TestAssemblyDefinition ("MONOTOUCH_System.ServiceModel_test.dll")} ),
		};

		static readonly List <TestAssemblyDefinition> CommonIgnoredAssemblies = new List <TestAssemblyDefinition> {
			new TestAssemblyDefinition ("MONOTOUCH_Commons.Xml.Relaxng_test.dll"),
			new TestAssemblyDefinition ("MONOTOUCH_Cscompmgd_test.dll"),
			new TestAssemblyDefinition ("MONOTOUCH_I18N.CJK_test.dll"),
			new TestAssemblyDefinition ("MONOTOUCH_I18N.MidEast_test.dll"),
			new TestAssemblyDefinition ("MONOTOUCH_I18N.Other_test.dll"),
			new TestAssemblyDefinition ("MONOTOUCH_I18N.Rare_test.dll"),
			new TestAssemblyDefinition ("MONOTOUCH_I18N.West_test.dll"),
			new TestAssemblyDefinition ("MONOTOUCH_Mono.C5_test.dll"),
			new TestAssemblyDefinition ("MONOTOUCH_Mono.CodeContracts_test.dll"),
			new TestAssemblyDefinition ("MONOTOUCH_Mono.Parallel_test.dll"),
			new TestAssemblyDefinition ("MONOTOUCH_Mono.Runtime.Tests_test.dll"),
			new TestAssemblyDefinition ("MONOTOUCH_Mono.Tasklets_test.dll"),
			new TestAssemblyDefinition ("MONOTOUCH_Novell.Directory.Ldap_test.dll"),
			new TestAssemblyDefinition ("MONOTOUCH_System.Data.DataSetExtensions_test.dll"),
			new TestAssemblyDefinition ("MONOTOUCH_System.Json.Microsoft_test.dll"),
			new TestAssemblyDefinition ("MONOTOUCH_System.Runtime.Serialization.Formatters.Soap_test.dll"),
			new TestAssemblyDefinition ("MONOTOUCH_System.Threading.Tasks.Dataflow_test.dll"),
			new TestAssemblyDefinition ("MONOTOUCH_corlib_test.dll"),
		};

		public bool Override{ get; set; }
		public string OutputDirectoryPath { get; private  set; }
		public string MonoRootPath { get; private set; }
		public string ProjectTemplatePath { get; private set; }
		public string RegisterTypesTemplatePath { get; private set; }

		public ProjectGenerator (string outpudDirectory, string monoRootPath, string projectTemplatePath, string registerTypesTemplatePath)
		{
			OutputDirectoryPath = outpudDirectory ?? throw new ArgumentNullException (nameof (outpudDirectory));
			MonoRootPath = monoRootPath ?? throw new ArgumentNullException (nameof (monoRootPath));
			ProjectTemplatePath = projectTemplatePath ?? throw new ArgumentNullException (nameof (projectTemplatePath));
			RegisterTypesTemplatePath = registerTypesTemplatePath ?? throw new ArgumentNullException (nameof (registerTypesTemplatePath));
		}

		// creates the reference node
		static string GetReferenceNode (string assemblyName, string hintPath = null)
		{
			// lets not compliate our lifes with Xml, we just need to replace two things
			if (string.IsNullOrEmpty (hintPath)) {
				return $"<Reference Include=\"{assemblyName}\" />";
			} else {
				// the hint path is using unix separators, we need to use windows ones
				hintPath = hintPath.Replace ('/', '\\');
				var sb = new StringBuilder ();
				sb.AppendLine ($"<Reference Include=\"{assemblyName}\" >");
				sb.AppendLine ($"<HintPath>{hintPath}</HintPath>");
				sb.AppendLine ("</Reference>");
				return sb.ToString ();
			}
		}

		static string GetRegisterTypeNode (string registerPath)
		{
			var sb = new StringBuilder ();
			sb.AppendLine ($"<Compile Include=\"{registerPath}\">");
			sb.AppendLine ($"<Link>{Path.GetFileName (registerPath)}</Link>");
			sb.AppendLine ("</Compile>");
			return sb.ToString ();
		}

		// creates all the projects that have already been defined
		public async Task GenerateAllTestProjects ()
		{
			// TODO: Do this per platform
			var platform = "iOS";
			var generatedCodePathRoot = Path.Combine (OutputDirectoryPath, "generated");
			if (!Directory.Exists (generatedCodePathRoot)) {
				Directory.CreateDirectory (generatedCodePathRoot);
			}

			foreach (var projectDefinition in iOSTestProjects) {
				// generate the required type registration info
				var generatedCodeDir = Path.Combine (generatedCodePathRoot, projectDefinition.Name);
				if (!Directory.Exists (generatedCodeDir)) {
					Directory.CreateDirectory (generatedCodeDir);
				}

				var typesPerAssembly = projectDefinition.GetTypeForAssemblies (MonoRootPath, "iOS");
				var registerCode = await RegisterTypeGenerator.GenerateCodeAsync (typesPerAssembly,
					projectDefinition.TestAssemblies[0].IsXUnit, RegisterTypesTemplatePath);
				Console.WriteLine ($"Register code is {registerCode}");

				var filePath = Path.Combine (generatedCodeDir, "RegisterType.cs");
				using (var file = new StreamWriter (filePath, !Override)) { // false is do not append
					await file.WriteAsync (registerCode);
				}
				Console.WriteLine ($"File written to {filePath}");

				var generatedProject = await GenerateAsync (projectDefinition.Name, filePath,
					projectDefinition.GetAssemblyInclusionInformation (MonoRootPath, platform), ProjectTemplatePath);
				Console.WriteLine ($"Generated code is {generatedProject}");
				var projectPath = Path.Combine (OutputDirectoryPath, $"{projectDefinition.Name}.csproj");
				using (var file = new StreamWriter (projectPath, !Override)) { // false is do not append
					await file.WriteAsync (generatedProject);
				}
				Console.WriteLine ($"Written to {projectPath}");
			}
		}

		static async Task<string> GenerateAsync (string projectName, string registerPath, List<(string assembly, string hintPath)> info, string templatePath)
		{
			var sb = new StringBuilder ();
			foreach (var assemblyInfo in info) {
				if (!excludeDlls.Contains (assemblyInfo.assembly))
				sb.AppendLine (GetReferenceNode (assemblyInfo.assembly, assemblyInfo.hintPath));
			}

			using (var reader = new StreamReader(templatePath)) {
				var result = await reader.ReadToEndAsync ();
				result = result.Replace (NameKey, projectName);
				result = result.Replace (ReferencesKey, sb.ToString ());
				result = result.Replace (RegisterTypeKey, GetRegisterTypeNode (registerPath));
				return result;
			}
		}

		public static string Generate (string projectName, string registerPath, List<(string assembly, string hintPath)> info, string templatePath) =>
			GenerateAsync (projectName, registerPath, info, templatePath).Result;
	}
}
