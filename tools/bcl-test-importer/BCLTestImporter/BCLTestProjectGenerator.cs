using System;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Collections.Generic;
using System.ComponentModel;
using System.Net;
using System.Security.Cryptography.X509Certificates;

namespace BCLTestImporter {
	/// <summary>
	/// Class that knows how to generate .csproj files based on a BCLTestProjectDefinition.
	/// </summary>
	public class BCLTestProjectGenerator {

		static string NUnitPattern = "monotouch_*_test.dll"; 
		static string xUnitPattern = "monotouch_*_xunit-test.dll";
		internal static readonly string NameKey = "%NAME%";
		internal static readonly string ReferencesKey = "%REFERENCES%";
		internal static readonly string RegisterTypeKey = "%REGISTER TYPE%";
		internal static readonly string PlistKey = "%PLIST PATH%";
		internal static readonly string WatchOSTemplatePathKey = "%TEMPLATE PATH%";
		internal static readonly string WatchOSCsporjAppKey = "%WATCH APP PROJECT PATH%";
		internal static readonly string WatchOSCsporjExtensionKey  ="%WATCH EXTENSION PROJECT PATH%";
		static readonly Dictionary<Platform, string> plistTemplateMatches = new Dictionary<Platform, string> {
			{Platform.iOS, "Info.plist.in"},
			{Platform.TvOS, "Info-tv.plist.in"},
			{Platform.WatchOS, "Info-watchos.plist.in"},
			{Platform.MacOS, "Info-mac.plist.in"},
		};
		static readonly Dictionary<Platform, string> projectTemplateMatches = new Dictionary<Platform, string> {
			{Platform.iOS, "BCLTests.csproj.in"},
			{Platform.TvOS, "BCLTests-tv.csproj.in"},
			{Platform.WatchOS, "BCLTests-watchos.csproj.in"},
			{Platform.MacOS, "BCLTests-mac.csproj.in"},
		};
		static readonly Dictionary<WatchAppType, string> watchOSProjectTemplateMatches = new Dictionary<WatchAppType, string>
		{
			{ WatchAppType.App, "BCLTests-watchos-app.csproj.in"},
			{ WatchAppType.Extension, "BCLTests-watchos-extension.csproj.in"}
		};

		public enum WatchAppType {
			App,
			Extension
		}

		static readonly Dictionary<WatchAppType, string> watchOSPlistTemplateMatches = new Dictionary<WatchAppType, string> {
			{WatchAppType.App, "Info-watchos-app.plist.in"},
			{WatchAppType.Extension, "Info-watchos-extension.plist.in"}
		};

		//list of reference that we are already adding, and we do not want to readd (although it is just a warning)
		static readonly List<string> excludeDlls = new List<string> {
			"mscorlib",
			"nunitlite",
			"System",
			"System.Xml",
			"System.Xml.Linq",
		};

		// we have two different types of list, those that are for the iOS like projects (ios, tvos and watch os) and those 
		// for mac
		static readonly List<(string name, string[] assemblies)> commoniOSTestProjects = new List<(string name, string[] assemblies)> {
			// NUNIT TESTS

			(name:"SystemTests", assemblies: new[] {"monotouch_System_test.dll"}),
			(name:"SystemCoreTests", assemblies: new [] {"monotouch_System.Core_test.dll"}),
			(name:"SystemDataTests", assemblies: new [] {"monotouch_System.Data_test.dll"}),
			(name:"SystemNetHttpTests", assemblies: new [] {"monotouch_System.Net.Http_test.dll"}),
			(name:"SystemNumericsTests", assemblies: new [] {"monotouch_System.Numerics_test.dll"}),
			(name:"SystemRuntimeSerializationTests", assemblies: new [] {"monotouch_System.Runtime.Serialization_test.dll"}),
			(name:"SystemTransactionsTests", assemblies: new [] {"monotouch_System.Transactions_test.dll"}),
			(name:"SystemXmlTests", assemblies: new [] {"monotouch_System.Xml_test.dll"}),
			(name:"SystemXmlLinqTests", assemblies: new [] {"monotouch_System.Xml.Linq_test.dll"}),
			(name:"MonoSecurityTests", assemblies: new [] {"monotouch_Mono.Security_test.dll"}),
			(name:"SystemComponentModelDataAnnotationTests", assemblies: new [] {"monotouch_System.ComponentModel.DataAnnotations_test.dll"}),
			(name:"SystemJsonTests", assemblies: new [] {"monotouch_System.Json_test.dll"}),
			(name:"SystemServiceModelWebTests", assemblies: new [] {"monotouch_System.ServiceModel.Web_test.dll"}),
			(name:"MonoDataTdsTests", assemblies: new [] {"monotouch_Mono.Data.Tds_test.dll"}),
			(name:"SystemIOCompressionTests", assemblies: new [] {"monotouch_System.IO.Compression_test.dll"}),
			(name:"SystemIOCompressionFileSystemTests", assemblies: new [] {"monotouch_System.IO.Compression.FileSystem_test.dll"}),
			(name:"MonoCSharpTests", assemblies: new [] {"monotouch_Mono.CSharp_test.dll"}),
			(name:"SystemSecurityTests", assemblies: new [] {"monotouch_System.Security_test.dll"}),
			(name:"SystemServiceModelTests", assemblies: new [] {"monotouch_System.ServiceModel_test.dll"}),
			(name:"SystemJsonMicrosoftTests", assemblies: new [] {"monotouch_System.Json.Microsoft_test.dll"}),
			(name:"SystemDataDataSetExtensionTests", assemblies: new [] {"monotouch_System.Data.DataSetExtensions_test.dll"}),
			(name:"SystemRuntimeSerializationFormattersSoapTests", assemblies: new [] {"monotouch_System.Runtime.Serialization.Formatters.Soap_test.dll"}),
			(name:"CorlibTests", assemblies: new [] {"monotouch_corlib_test.dll"}),
			(name:"MonoParallelTests", assemblies: new [] {"monotouch_Mono.Parallel_test.dll"}),
			(name:"MonoRuntimeTests", assemblies: new [] {"monotouch_Mono.Runtime.Tests_test.dll"}),
			(name:"MonoTaskletsTests", assemblies: new [] {"monotouch_Mono.Tasklets_test.dll"}),
			(name:"SystemThreadingTasksDataflowTests", assemblies: new [] {"monotouch_System.Threading.Tasks.Dataflow_test.dll"}),

			// XUNIT TESTS 

			(name:"SystemDataXunit", assemblies: new [] {"monotouch_System.Data_xunit-test.dll"}),
			(name:"SystemJsonXunit", assemblies: new [] {"monotouch_System.Json_xunit-test.dll"}),
			(name:"SystemNumericsXunit", assemblies: new [] {"monotouch_System.Numerics_xunit-test.dll"}),
			(name:"SystemSecurityXunit", assemblies: new [] {"monotouch_System.Security_xunit-test.dll"}),
			(name:"SystemThreadingTaskXunit", assemblies: new [] {"monotouch_System.Threading.Tasks.Dataflow_xunit-test.dll"}),
			(name:"SystemLinqXunit", assemblies: new [] {"monotouch_System.Xml.Linq_xunit-test.dll"}),
			(name:"SystemRuntimeCompilerServicesUnsafeXunit", assemblies: new [] {"monotouch_System.Runtime.CompilerServices.Unsafe_xunit-test.dll"}),
		};
			
		static readonly List <string> CommonIgnoredAssemblies = new List <string> {
			"monotouch_System.Data_xunit-test.dll", // issue https://github.com/xamarin/maccore/issues/1131
			"monotouch_System.Security_xunit-test.dll",// issue https://github.com/xamarin/maccore/issues/1128
			"monotouch_System.Threading.Tasks.Dataflow_xunit-test.dll", // issue https://github.com/xamarin/maccore/issues/1132
			"monotouch_System.Xml_test.dll", // issue https://github.com/xamarin/maccore/issues/1133
			"monotouch_System.Transactions_test.dll", // issue https://github.com/xamarin/maccore/issues/1134
			"monotouch_System_test.dll", // issues https://github.com/xamarin/maccore/issues/1135
			"monotouch_System.ServiceModel.Web_test.dll", // issue https://github.com/xamarin/maccore/issues/1137
			"monotouch_System.ServiceModel_test.dll", // issue https://github.com/xamarin/maccore/issues/1138
			"monotouch_System.Security_test.dll", // issue https://github.com/xamarin/maccore/issues/1139
			"monotouch_System.Runtime.Serialization.Formatters.Soap_test.dll", // issue https://github.com/xamarin/maccore/issues/1140
			"monotouch_System.Net.Http_test.dll", // issue https://github.com/xamarin/maccore/issues/1144 and https://github.com/xamarin/maccore/issues/1145
			"monotouch_System.IO.Compression_test.dll", // issue https://github.com/xamarin/maccore/issues/1146
			"monotouch_System.IO.Compression.FileSystem_test.dll", // issue https://github.com/xamarin/maccore/issues/1147 and https://github.com/xamarin/maccore/issues/1148
			"monotouch_System.Data_test.dll", // issue https://github.com/xamarin/maccore/issues/1149
			"monotouch_System.Data.DataSetExtensions_test.dll", // issue https://github.com/xamarin/maccore/issues/1150 and https://github.com/xamarin/maccore/issues/1151
			"monotouch_System.Core_test.dll", // issue https://github.com/xamarin/maccore/issues/1143
			"monotouch_Mono.Runtime.Tests_test.dll", // issue https://github.com/xamarin/maccore/issues/1141
			"monotouch_corlib_test.dll", // issue https://github.com/xamarin/maccore/issues/1153
			"monotouch_Commons.Xml.Relaxng_test.dll", // not supported by xamarin
			"monotouch_Cscompmgd_test.dll", // not supported by xamarin
			"monotouch_I18N.CJK_test.dll",
			"monotouch_I18N.MidEast_test.dll",
			"monotouch_I18N.Other_test.dll",
			"monotouch_I18N.Rare_test.dll",
			"monotouch_I18N.West_test.dll",
			"monotouch_Mono.C5_test.dll", // not supported by xamarin
			"monotouch_Mono.CodeContracts_test.dll", // not supported by xamarin
			"monotouch_Novell.Directory.Ldap_test.dll", // not supported by xamarin
			"monotouch_Mono.Profiler.Log_xunit-test.dll", // special tests that need an extra app to connect as a profiler
		};
		
		// list of assemblies that are going to be ignored, any project with an assemblies that is ignored will
		// be ignored

		static readonly List<string> iOSIgnoredAssemblies = new List<string> {};

		static readonly List<string> tvOSIgnoredAssemblies = new List<string> {
			"monotouch_System.Xml.Linq_xunit-test.dll", // issue https://github.com/xamarin/maccore/issues/1130
			"monotouch_System.Numerics_xunit-test.dll", // issue https://github.com/xamarin/maccore/issues/1129
		};

		static readonly List<string> watcOSIgnoredAssemblies = new List<string> {
			"monotouch_System.Xml.Linq_xunit-test.dll", // issue https://github.com/xamarin/maccore/issues/1130
			"monotouch_System.Numerics_xunit-test.dll", // issue https://github.com/xamarin/maccore/issues/1129
			"monotouch_Mono.Security_test.dll", // issue https://github.com/xamarin/maccore/issues/1142
			"monotouch_Mono.Data.Tds_test.dll", // issue https://gist.github.com/mandel-macaque/d97fa28f8a73c3016d1328567da77a0b
		};

		private static readonly List<(string name, string[] assemblies)> macTestProjects = new List<(string name, string[] assemblies)> {
		
			// NUNIT Projects
			(name:"MonoCSharp", assemblies: new [] {"xammac_net_4_5_Mono.CSharp_test.dll"}),
			(name:"MonoDataSqilte", assemblies: new [] {"xammac_net_4_5_Mono.Data.Sqlite_test.dll"}),
			(name:"MonoDataTds", assemblies: new [] {"xammac_net_4_5_Mono.Data.Tds_test.dll"}),
			(name:"MonoPoxis", assemblies: new [] {"xammac_net_4_5_Mono.Posix_test.dll"}),
			(name:"MonoSecurtiy", assemblies: new [] {"xammac_net_4_5_Mono.Security_test.dll"}),
			(name:"SystemComponentModelDataAnnotations", assemblies: new [] {"xammac_net_4_5_System.ComponentModel.DataAnnotations_test.dll"}),
			(name:"SystemConfiguration", assemblies: new [] {"xammac_net_4_5_System.Configuration_test.dll"}),
			(name:"SystemCore", assemblies: new [] {"xammac_net_4_5_System.Core_test.dll"}),
			(name:"SystemDataLinq", assemblies: new [] {"xammac_net_4_5_System.Data.Linq_test.dll"}),
			(name:"SystemData", assemblies: new [] {"xammac_net_4_5_System.Data_test.dll"}),
			(name:"SystemIOCompressionFileSystem", assemblies: new [] {"xammac_net_4_5_System.IO.Compression.FileSystem_test.dll"}),
			(name:"SystemIOCompression", assemblies: new [] {"xammac_net_4_5_System.IO.Compression_test.dll"}),
			(name:"SystemIdentityModel", assemblies: new [] {"xammac_net_4_5_System.IdentityModel_test.dll"}),
			(name:"SystemJson", assemblies: new [] {"xammac_net_4_5_System.Json_test.dll"}),
			(name:"SystemNetHttp", assemblies: new [] {"xammac_net_4_5_System.Net.Http_test.dll"}),
			(name:"SystemNumerics", assemblies: new [] {"xammac_net_4_5_System.Numerics_test.dll"}),
			(name:"SystemRuntimeSerializationFormattersSoap", assemblies: new [] {"xammac_net_4_5_System.Runtime.Serialization.Formatters.Soap_test.dll"}),
			(name:"SystemSecurity", assemblies: new [] {"xammac_net_4_5_System.Security_test.dll"}),
			(name:"SystemServiceModel", assemblies: new [] {"xammac_net_4_5_System.ServiceModel_test.dll"}),
			(name:"SystemTransactions", assemblies: new [] {"xammac_net_4_5_System.Transactions_test.dll"}),
			(name:"SystemXmlLinq", assemblies: new [] {"xammac_net_4_5_System.Xml.Linq_test.dll"}),
			(name:"SystemXml", assemblies: new [] {"xammac_net_4_5_System.Xml_test.dll"}),
			(name:"System", assemblies: new [] {"xammac_net_4_5_System_test.dll"}),
			
			// xUnit Projects
			(name:"MicrosoftCSharp", assemblies: new [] {"xammac_net_4_5_Microsoft.CSharp_xunit-test.dll"}),
			(name:"SystemCore", assemblies: new [] {"xammac_net_4_5_System.Core_xunit-test.dll"}),
			(name:"SystemData", assemblies: new [] {"xammac_net_4_5_System.Data_xunit-test.dll"}),
			(name:"SystemJson", assemblies: new [] {"xammac_net_4_5_System.Json_xunit-test.dll"}),
			(name:"SystemNumerics", assemblies: new [] {"xammac_net_4_5_System.Numerics_xunit-test.dll"}),
			(name:"SystemRuntimeCompilerServices", assemblies: new [] {"xammac_net_4_5_System.Runtime.CompilerServices.Unsafe_xunit-test.dll"}),
			(name:"SystemSecurity", assemblies: new [] {"xammac_net_4_5_System.Security_xunit-test.dll"}),
			(name:"SystemXmlLinq", assemblies: new [] {"xammac_net_4_5_System.Xml.Linq_xunit-test.dll"}),
			(name:"SystemXunit", assemblies: new [] {"xammac_net_4_5_System_xunit-test.dll"}),
			(name:"Corlib", assemblies: new [] {"xammac_net_4_5_corlib_xunit-test.dll"}),
		};
		
		static readonly List<string> macIgnoredAssemblies = new List<string> {
			"xammac_net_4_5_corlib_test.dll	", // exception when loading the image via refection
			"xammac_net_4_5_I18N.CJK_test.dll",
			"xammac_net_4_5_I18N.MidEast_test.dll",
			"xammac_net_4_5_I18N.Other_test.dll",
			"xammac_net_4_5_I18N.Rare_test.dll",
			"xammac_net_4_5_I18N.West_test.dll",
		};

		readonly bool isCodeGeneration;
		public bool Override { get; set; }
		public string OutputDirectoryPath { get; private  set; }
		public string MonoRootPath { get; private set; }
		public string ProjectTemplateRootPath { get; private set; }
		public string PlistTemplateRootPath{ get; private set; }
		public string RegisterTypesTemplatePath { get; private set; }
		string GeneratedCodePathRoot => Path.Combine (OutputDirectoryPath, "generated");
		string WatchContainerTemplatePath => Path.Combine (OutputDirectoryPath, "templates", "watchOS", "Container").Replace ("/", "\\");
		string WatchAppTemplatePath => Path.Combine (OutputDirectoryPath, "templates", "watchOS", "App").Replace ("/", "\\");
		string WatchExtensionTemplatePath => Path.Combine (OutputDirectoryPath, "templates", "watchOS", "Extension").Replace ("/", "\\");

		public BCLTestProjectGenerator (string outputDirectory)
		{
			OutputDirectoryPath = outputDirectory ?? throw new ArgumentNullException (nameof (outputDirectory));
		}
		
		public BCLTestProjectGenerator (string outputDirectory, string monoRootPath, string projectTemplatePath, string registerTypesTemplatePath, string plistTemplatePath)
		{
			isCodeGeneration = true;
			OutputDirectoryPath = outputDirectory ?? throw new ArgumentNullException (nameof (outputDirectory));
			MonoRootPath = monoRootPath ?? throw new ArgumentNullException (nameof (monoRootPath));
			ProjectTemplateRootPath = projectTemplatePath ?? throw new ArgumentNullException (nameof (projectTemplatePath));
			PlistTemplateRootPath = plistTemplatePath ?? throw new ArgumentNullException (nameof (plistTemplatePath));
			RegisterTypesTemplatePath = registerTypesTemplatePath ?? throw new ArgumentNullException (nameof (registerTypesTemplatePath));
		}

		/// <summary>
		/// Returns the path to be used to store the project file depending on the platform.
		/// </summary>
		/// <param name="projectName">The name of the project being generated.</param>
		/// <param name="platform">The supported platform by the project.</param>
		/// <returns></returns>
		internal string GetProjectPath (string projectName, Platform platform)
		{
			switch (platform) {
			case Platform.iOS:
				return Path.Combine (OutputDirectoryPath, $"{projectName}.csproj");
			case Platform.TvOS:
				return Path.Combine (OutputDirectoryPath, $"{projectName}-tvos.csproj");
			case Platform.WatchOS:
				return Path.Combine (OutputDirectoryPath, $"{projectName}-watchos.csproj");
			case Platform.MacOS:
				return Path.Combine (OutputDirectoryPath, $"{projectName}-mac.csproj");
			default:
				return null;
			}
		}
		
		internal string GetProjectPath (string projectName, WatchAppType appType)
		{
			switch (appType) {
			case WatchAppType.App:
				return Path.Combine (OutputDirectoryPath, $"{projectName}-watchos-app.csproj");
			default:
				return Path.Combine (OutputDirectoryPath, $"{projectName}-watchos-extension.csproj");
			}
		}

		/// <summary>
		/// Returns the path to be used to store the projects plist file depending on the platform.
		/// </summary>
		/// <param name="rootDir">The root dir to use.</param>
		/// <param name="platform">The platform that is supported by the project.</param>
		/// <returns></returns>
		internal static string GetPListPath (string rootDir, Platform platform)
		{
			switch (platform) {
			case Platform.iOS:
				return Path.Combine (rootDir, "Info.plist");
			case Platform.TvOS:
				return Path.Combine (rootDir, "Info-tv.plist");
			case Platform.WatchOS:
				return Path.Combine (rootDir, "Info-watchos.plist");
			case Platform.MacOS:
				return Path.Combine (rootDir, "Info-mac.plist");
			default:
				return Path.Combine (rootDir, "Info.plist");
			}
		}

		internal static string GetPListPath (string rootDir, WatchAppType appType)
		{
			switch (appType) {
				case WatchAppType.App:
					return Path.Combine (rootDir, "Info-watchos-app.plist");
				default:
					return Path.Combine (rootDir, "Info-watchos-extension.plist");
			}
		}
		
		// creates the reference node
		internal static string GetReferenceNode (string assemblyName, string hintPath = null)
		{
			// lets not complicate our life with Xml, we just need to replace two things
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

		internal static string GetRegisterTypeNode (string registerPath)
		{
			var sb = new StringBuilder ();
			sb.AppendLine ($"<Compile Include=\"{registerPath}\">");
			sb.AppendLine ($"<Link>{Path.GetFileName (registerPath)}</Link>");
			sb.AppendLine ("</Compile>");
			return sb.ToString ();
		}

		/// <summary>
		/// Returns is a project should be ignored in a platform. A project is ignored in one of the assemblies in the
		/// project is ignored in the platform.
		/// </summary>
		/// <param name="project">The project which is under test.</param>
		/// <param name="platform">The platform to which we are testing against.</param>
		/// <returns>If the project should be ignored in a platform or not.</returns>
		bool IsIgnored(BCLTestProjectDefinition project, Platform platform)
		{
			foreach (var a in project.TestAssemblies){
				if (CommonIgnoredAssemblies.Contains (a.Name))
					return true;
				switch (platform){
				case Platform.iOS:
					return iOSIgnoredAssemblies.Contains (a.Name);
				case Platform.TvOS:
					return tvOSIgnoredAssemblies.Contains (a.Name);
				case Platform.WatchOS:
					return watcOSIgnoredAssemblies.Contains (a.Name);
				case Platform.MacOS:
					return macIgnoredAssemblies.Contains (a.Name);
				}
			}
			return false;
		}

		async Task<List<(string name, string path, bool xunit)>> GenerateWatchOSTestProjectsAsync (
			IEnumerable<(string name, string[] assemblies)> projects, string generatedDir)
		{
			var projectPaths = new List<(string name, string path, bool xunit)> ();
			foreach (var def in projects) {
				// each watch os project requires 3 different ones:
				// 1. The app
				// 2. The container
				// 3. The extensions
				// TODO: The following is very similar to what is done in the iOS generation. Must be grouped
				var projectDefinition = new BCLTestProjectDefinition (def.name, def.assemblies);
				if (IsIgnored (projectDefinition, Platform.WatchOS)) // if it is ignored, continue
					continue;

				if (!projectDefinition.Validate ())
					throw new InvalidOperationException ("xUnit and NUnit assemblies cannot be mixed in a test project.");
				var generatedCodeDir = Path.Combine (generatedDir, projectDefinition.Name);
				if (!Directory.Exists (generatedCodeDir)) {
					Directory.CreateDirectory (generatedCodeDir);
				}
				var registerTypePath = Path.Combine (generatedCodeDir, "RegisterType.cs");
				var typesPerAssembly = projectDefinition.GetTypeForAssemblies (MonoRootPath, Platform.WatchOS);
				var registerCode = await RegisterTypeGenerator.GenerateCodeAsync (def.name, projectDefinition.IsXUnit,
					RegisterTypesTemplatePath);
				using (var file = new StreamWriter (registerTypePath, false)) { // false is do not append
					await file.WriteAsync (registerCode);
				}
				// create the plist for each of the apps
				var projectData = new Dictionary<WatchAppType, (string plist, string project)> ();
				foreach (var appType in new [] {WatchAppType.Extension, WatchAppType.App}) {
					(string plist, string project) data;
					var plistTemplate = Path.Combine (PlistTemplateRootPath, watchOSPlistTemplateMatches[appType]);
					var plist = await BCLTestInfoPlistGenerator.GenerateCodeAsync (plistTemplate, projectDefinition.Name);
					data.plist = GetPListPath (generatedCodeDir, appType);
					using (var file = new StreamWriter (data.plist, false)) { // false is do not append
						await file.WriteAsync (plist);
					}

					string generatedProject;
					var projetTemplate = Path.Combine (ProjectTemplateRootPath, watchOSProjectTemplateMatches[appType]);
					switch (appType) {
						case WatchAppType.App:
							generatedProject = await GenerateWatchAppAsync (projectDefinition.Name, projetTemplate, data.plist);
							break;
						default:
							generatedProject = await GenerateWatchExtensionAsync (projectDefinition.Name, projetTemplate, data.plist, registerTypePath, projectDefinition.GetCachedAssemblyInclusionInformation (MonoRootPath, Platform.WatchOS));
							break;
					}
					data.project = GetProjectPath (projectDefinition.Name, appType);
					using (var file = new StreamWriter (data.project, false)) { // false is do not append
						await file.WriteAsync (generatedProject);
					}

					projectData[appType] = data;
				} // foreach app type
				
				var rootPlistTemplate = Path.Combine (PlistTemplateRootPath, plistTemplateMatches[Platform.WatchOS]);
				var rootPlist = await BCLTestInfoPlistGenerator.GenerateCodeAsync (rootPlistTemplate, projectDefinition.Name);
				var infoPlistPath = GetPListPath (generatedCodeDir, Platform.WatchOS);
				using (var file = new StreamWriter (infoPlistPath, false)) { // false is do not append
					await file.WriteAsync (rootPlist);
				}
				
				var projectTemplatePath = Path.Combine (ProjectTemplateRootPath, projectTemplateMatches[Platform.WatchOS]);
				var rootProjectPath = GetProjectPath (projectDefinition.Name, Platform.WatchOS);
				using (var file = new StreamWriter (rootProjectPath, false)) // false is do not append
				using (var reader = new StreamReader (projectTemplatePath)){
					var template = await reader.ReadToEndAsync ();
					var generatedRootProject = GenerateWatchProject (def.name, template, infoPlistPath);
					await file.WriteAsync (generatedRootProject);
				}

				// we have the 3 projects we depend on, we need the root one, the one that will be used by harness
				projectPaths.Add ((name: projectDefinition.Name, path: rootProjectPath, xunit: projectDefinition.IsXUnit));
			} // foreach project

			return projectPaths;
		}
		
		async Task<List<(string name, string path, bool xunit)>> GenerateiOSTestProjectsAsync (
			IEnumerable<(string name, string[] assemblies)> projects, Platform platform, string generatedDir)
		{
			if (platform == Platform.WatchOS) 
				throw new ArgumentException (nameof (platform));
			var projectPaths = new List<(string name, string path, bool xunit)> ();
			foreach (var def in projects) {
				var projectDefinition = new BCLTestProjectDefinition (def.name, def.assemblies);
				if (IsIgnored (projectDefinition, platform)) // some projects are ignored, so we just continue
					continue;

				if (!projectDefinition.Validate ())
					throw new InvalidOperationException ("xUnit and NUnit assemblies cannot be mixed in a test project.");
				// generate the required type registration info
				var generatedCodeDir = Path.Combine (generatedDir, projectDefinition.Name);
				if (!Directory.Exists (generatedCodeDir)) {
					Directory.CreateDirectory (generatedCodeDir);
				}
				var registerTypePath = Path.Combine (generatedCodeDir, "RegisterType.cs");

				var typesPerAssembly = projectDefinition.GetTypeForAssemblies (MonoRootPath, platform);
				var registerCode = await RegisterTypeGenerator.GenerateCodeAsync (def.name, projectDefinition.IsXUnit,
					RegisterTypesTemplatePath);

				using (var file = new StreamWriter (registerTypePath, false)) { // false is do not append
					await file.WriteAsync (registerCode);
				}

				var plistTemplate = Path.Combine (PlistTemplateRootPath, plistTemplateMatches[platform]);
				var plist = await BCLTestInfoPlistGenerator.GenerateCodeAsync (plistTemplate, projectDefinition.Name);
				var infoPlistPath = GetPListPath (generatedCodeDir, platform);
				using (var file = new StreamWriter (infoPlistPath, false)) { // false is do not append
					await file.WriteAsync (plist);
				}

				var projectTemplatePath = Path.Combine (ProjectTemplateRootPath, projectTemplateMatches[platform]);
				var generatedProject = await GenerateAsync (projectDefinition.Name, registerTypePath,
					projectDefinition.GetCachedAssemblyInclusionInformation (MonoRootPath, platform), projectTemplatePath, infoPlistPath);
				var projectPath = GetProjectPath (projectDefinition.Name, platform);
				projectPaths.Add ((name: projectDefinition.Name, path: projectPath, xunit: projectDefinition.IsXUnit));
				using (var file = new StreamWriter (projectPath, false)) { // false is do not append
					await file.WriteAsync (generatedProject);
				}
			} // foreach project

			return projectPaths;
		}
		
		async Task<List<(string name, string path, bool xunit)>> GenerateMacTestProjectsAsync (
			IEnumerable<(string name, string[] assemblies)> projects, string generatedDir)
		{
			var projectPaths = new List<(string name, string path, bool xunit)> ();
			foreach (var def in projects) {
				var projectDefinition = new BCLTestProjectDefinition (def.name, def.assemblies);
				if (IsIgnored (projectDefinition, Platform.MacOS)) // some projects are ignored, so we just continue
					continue;

				if (!projectDefinition.Validate ())
					throw new InvalidOperationException ("xUnit and NUnit assemblies cannot be mixed in a test project.");
				// generate the required type registration info
				var generatedCodeDir = Path.Combine (generatedDir, projectDefinition.Name);
				if (!Directory.Exists (generatedCodeDir)) {
					Directory.CreateDirectory (generatedCodeDir);
				}
				var registerTypePath = Path.Combine (generatedCodeDir, "RegisterType.cs");

				var typesPerAssembly = projectDefinition.GetTypeForAssemblies (MonoRootPath, Platform.MacOS);
				var registerCode = await RegisterTypeGenerator.GenerateCodeAsync (typesPerAssembly,
					projectDefinition.IsXUnit, RegisterTypesTemplatePath);

				using (var file = new StreamWriter (registerTypePath, false)) { // false is do not append
					await file.WriteAsync (registerCode);
				}
				
				var plistTemplate = Path.Combine (PlistTemplateRootPath, plistTemplateMatches[Platform.MacOS]);
				var plist = await BCLTestInfoPlistGenerator.GenerateCodeAsync (plistTemplate, projectDefinition.Name);
				var infoPlistPath = GetPListPath (generatedCodeDir, Platform.MacOS);
				using (var file = new StreamWriter (infoPlistPath, false)) { // false is do not append
					await file.WriteAsync (plist);
				}

				var projectTemplatePath = Path.Combine (ProjectTemplateRootPath, projectTemplateMatches[Platform.MacOS]);
				var generatedProject = await GenerateMacAsync (projectDefinition.Name, registerTypePath,
					projectDefinition.GetAssemblyInclusionInformation (MonoRootPath, Platform.MacOS), projectTemplatePath, infoPlistPath);
				var projectPath = GetProjectPath (projectDefinition.Name, Platform.MacOS);
				projectPaths.Add ((name: projectDefinition.Name, path: projectPath, xunit: projectDefinition.IsXUnit));
				using (var file = new StreamWriter (projectPath, false)) { // false is do not append
					await file.WriteAsync (generatedProject);
				}
			}
			return projectPaths;
		}

		/// <summary>
		/// Generates all the project files for the given projects and platform
		/// </summary>
		/// <param name="projects">The list of projects to be generated.</param>
		/// <param name="platform">The platform to which the projects have to be generated. Each platform
		/// has its own details.</param>
		/// <param name="generatedDir">The dir where the projects will be saved.</param>
		/// <returns></returns>
		async Task<List<(string name, string path, bool xunit)>> GenerateTestProjectsAsync (
			IEnumerable<(string name, string[] assemblies)> projects, Platform platform, string generatedDir)
		{
			List<(string name, string path, bool xunit)> result = new List<(string name, string path, bool xunit)> ();
			switch (platform) {
			case Platform.WatchOS:
				result = await GenerateWatchOSTestProjectsAsync (projects, generatedDir);
				break;
			case Platform.iOS:
				result = await GenerateiOSTestProjectsAsync (projects, platform, generatedDir);
				break;
			case Platform.MacOS:
				result = await GenerateMacTestProjectsAsync (projects, generatedDir);
				break;
			}
			return result;
		}
		
		// generates a project per platform of the common projects. 
		async Task<List<(string name, string path, bool xunit, List<Platform> platforms)>> GenerateAllCommonTestProjectsAsync ()
		{
			var projectPaths = new List<(string name, string path, bool xunit, List<Platform> platforms)> ();
			if (!isCodeGeneration)
				throw new InvalidOperationException ("Project generator was instantiated to delete the generated code.");
			var generatedCodePathRoot = GeneratedCodePathRoot;
			if (!Directory.Exists (generatedCodePathRoot)) {
				Directory.CreateDirectory (generatedCodePathRoot);
			}

			var projects = new Dictionary<string, (string path, bool xunit, List<Platform> platforms)> ();
			foreach (var platform in new [] {Platform.iOS, Platform.TvOS, Platform.WatchOS}) {
				var generated = await GenerateTestProjectsAsync (commoniOSTestProjects, platform, generatedCodePathRoot);
				foreach (var (name, path, xunit) in generated) {
					if (!projects.ContainsKey (name)) {
						projects [name] = (path, xunit, new List<Platform> { platform });
					} else {
						projects [name].platforms.Add (platform);
					}
				}
			} // foreach platform
			
			// return the grouped projects
			foreach (var name in projects.Keys) {
				projectPaths.Add ((name, projects[name].path, projects[name].xunit, projects[name].platforms));
			}
			return projectPaths;
		}
		
		// creates all the projects that have already been defined
		public async Task<List<(string name, string path, bool xunit, List<Platform> platforms)>> GenerateAlliOSTestProjectsAsync ()
		{
			var projectPaths = new List<(string name, string path, bool xunit, List<Platform> platforms)> ();
			if (!isCodeGeneration)
				throw new InvalidOperationException ("Project generator was instantiated to delete the generated code.");
			var generatedCodePathRoot = GeneratedCodePathRoot;
			if (!Directory.Exists (generatedCodePathRoot)) {
				Directory.CreateDirectory (generatedCodePathRoot);
			}
			// generate all the common projects
			projectPaths.AddRange (await GenerateAllCommonTestProjectsAsync ());

			return projectPaths;
		}

		public List<(string name, string path, bool xunit, List<Platform> platforms)> GenerateAlliOSTestProjects () => GenerateAlliOSTestProjectsAsync ().Result;
		
		public async Task<List<(string name, string path, bool xunit)>> GenerateAllMacTestProjectsAsync ()
		{
			if (!isCodeGeneration)
				throw new InvalidOperationException ("Project generator was instantiated to delete the generated code.");
			var generatedCodePathRoot = GeneratedCodePathRoot;
			if (!Directory.Exists (generatedCodePathRoot)) {
				Directory.CreateDirectory (generatedCodePathRoot);
			}
			
			var generated = await GenerateTestProjectsAsync (macTestProjects, Platform.MacOS, generatedCodePathRoot);
			return generated;
		}

		public List<(string name, string path, bool xunit)> GenerateAllMacTestProjects () => GenerateAllMacTestProjectsAsync ().Result;

		/// <summary>
		/// Generates an iOS project for testing purposes. The generated project will contain the references to the
		/// mono test assemblies to run.
		/// </summary>
		/// <param name="projectName">The name of the project under generation.</param>
		/// <param name="registerPath">The path to the code that register the types so that the assemblies are not linked.</param>
		/// <param name="info">The list of assemblies to be added to the project and their hint paths.</param>
		/// <param name="templatePath">A path to the template used to generate the path.</param>
		/// <param name="infoPlistPath">The path to the info plist of the project.</param>
		/// <returns></returns>
		static async Task<string> GenerateAsync (string projectName, string registerPath, List<(string assembly, string hintPath)> info, string templatePath, string infoPlistPath)
		{
			// fix possible issues with the paths to be included in the msbuild xml
			infoPlistPath = infoPlistPath.Replace ('/', '\\');
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
				result = result.Replace (PlistKey, infoPlistPath);
				return result;
			}
		}
		
		static async Task<string> GenerateMacAsync (string projectName, string registerPath, List<(string assembly, string hintPath)> info, string templatePath, string infoPlistPath)
		{
			infoPlistPath = infoPlistPath.Replace ('/', '\\');
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
				result = result.Replace (PlistKey, infoPlistPath);
				return result;
			}
		}

		internal string GenerateWatchProject (string projectName, string template, string infoPlistPath)
		{
				var result = template.Replace (NameKey, projectName);
				result = result.Replace (WatchOSTemplatePathKey, WatchContainerTemplatePath);
				result = result.Replace (PlistKey, infoPlistPath);
				result = result.Replace (WatchOSCsporjAppKey, GetProjectPath (projectName, WatchAppType.App).Replace ("/", "\\"));
				return result;
		}

		async Task<string> GenerateWatchAppAsync (string projectName, string templatePath, string infoPlistPath)
		{
			using (var reader = new StreamReader(templatePath)) {
				var result = await reader.ReadToEndAsync ();
				result = result.Replace (NameKey, projectName);
				result = result.Replace (WatchOSTemplatePathKey, WatchAppTemplatePath);
				result = result.Replace (PlistKey, infoPlistPath);
				result = result.Replace (WatchOSCsporjExtensionKey, GetProjectPath (projectName, WatchAppType.Extension).Replace ("/", "\\"));
				return result;
			}
		}

		async Task<string> GenerateWatchExtensionAsync (string projectName, string templatePath, string infoPlistPath, string registerPath, List<(string assembly, string hintPath)> info)
		{
			var sb = new StringBuilder ();
			foreach (var assemblyInfo in info) {
				if (!excludeDlls.Contains (assemblyInfo.assembly))
					sb.AppendLine (GetReferenceNode (assemblyInfo.assembly, assemblyInfo.hintPath));
			}
			
			using (var reader = new StreamReader(templatePath)) {
				var result = await reader.ReadToEndAsync ();
				result = result.Replace (NameKey, projectName);
				result = result.Replace (WatchOSTemplatePathKey, WatchExtensionTemplatePath);
				result = result.Replace (PlistKey, infoPlistPath);
				result = result.Replace (RegisterTypeKey, GetRegisterTypeNode (registerPath));
				result = result.Replace (ReferencesKey, sb.ToString ());
				return result;
			}
		}

		public static string Generate (string projectName, string registerPath, List<(string assembly, string hintPath)> info, string templatePath, string infoPlistPath) =>
			GenerateAsync (projectName, registerPath, info, templatePath, infoPlistPath).Result;

		/// <summary>
		/// Removes all the generated files by the tool.
		/// </summary>
		public void CleanOutput ()
		{
			if (isCodeGeneration)
				throw new InvalidOperationException ("Project generator was instantiated to project generation.");
			if (Directory.Exists (GeneratedCodePathRoot))
				Directory.Delete (GeneratedCodePathRoot, true);
			// delete all the common projects
			foreach (var platform in new [] {Platform.iOS, Platform.TvOS}) {
				foreach (var testProject in commoniOSTestProjects) {
					var projectPath = GetProjectPath (testProject.name, platform);
					if (File.Exists (projectPath))
						File.Delete (projectPath);
				}
			}
			// delete each of the generated project files
			foreach (var projectDefinition in commoniOSTestProjects) {
				var projectPath = GetProjectPath (projectDefinition.name, Platform.iOS);
				if (File.Exists (projectPath))
					File.Delete (projectPath);
			}	
		}

		/// <summary>
		/// Returns if all the test assemblies found in the mono path 
		/// </summary>
		/// <param name="missingAssemblies"></param>
		/// <returns></returns>
		public bool AllTestAssembliesAreRan (out Dictionary<Platform, List<string>> missingAssemblies)
		{
			missingAssemblies = new Dictionary<Platform, List<string>> ();
			foreach (var platform in new [] {Platform.iOS, Platform.TvOS}) {
				var testDir = BCLTestAssemblyDefinition.GetTestDirectory (MonoRootPath, platform); 
				var missingAssembliesPlatform = Directory.GetFiles (testDir, NUnitPattern).Select (Path.GetFileName).Union (
					Directory.GetFiles (testDir, xUnitPattern).Select (Path.GetFileName)).ToList ();
				
				foreach (var assembly in CommonIgnoredAssemblies) {
					missingAssembliesPlatform.Remove (assembly);
				}
				
				// loop over the mono root path and grab all the assemblies, then intersect the found ones with the added
				// and ignored ones.
				foreach (var projectDefinition in commoniOSTestProjects) {
					foreach (var testAssembly in projectDefinition.assemblies) {
						missingAssembliesPlatform.Remove (testAssembly);
					}
				}

				if (missingAssembliesPlatform.Count != 0) {
					missingAssemblies[platform] = missingAssembliesPlatform;
				}
			}
			return missingAssemblies.Keys.Count == 0;
		}
	}
}
