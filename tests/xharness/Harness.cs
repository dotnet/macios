using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using System.Xml;
using Xharness.BCLTestImporter;
using Xharness.Logging;
using Xharness.Execution;
using Xharness.Targets;
using Xharness.Utilities;
using Xharness.Hardware;
using Xharness.Listeners;

namespace Xharness
{
	public enum HarnessAction
	{
		None,
		Configure,
		Run,
		Install,
		Uninstall,
		Jenkins,
	}

	public class HarnessConfiguration {
		public bool AutoConf { get; set; }
		public string BuildConfiguration { get; set; } = "Debug";
		public bool DryRun { get; set; }
		public Dictionary<string, string> EnvironmentVariables { get; set; } = new Dictionary<string, string> ();
		public bool? IncludeSystemPermissionTests { get; set; }
		public List<iOSTestProject> IOSTestProjects { get; set; } = new List<iOSTestProject> ();
		public string JenkinsConfiguration { get; set; }
		public HashSet<string> Labels { get; set; } = new HashSet<string> ();
		public string LogDirectory { get; set; } = Environment.CurrentDirectory;
		public bool Mac { get; set; }
		public string MarkdownSummaryPath { get; set; }
		public string PeriodicCommand { get; set; }
		public string PeriodicCommandArguments { get; set; }
		public TimeSpan PeriodicCommandInterval { get; set; }
		public string SdkRoot { get; set; }
		public AppRunnerTarget Target { get; set; }
		public double TimeoutInMinutes { get; set; } = 15;
		public bool UseSystemXamarinIOSMac { get; set; }
		public int Verbosity { get; set; }
		public string WatchOSAppTemplate { get; set; }
		public string WatchOSContainerTemplate { get; set; }
		public XmlResultJargon XmlJargon { get; set; } = XmlResultJargon.NUnitV3;
	}

	public interface IHarness {
		HarnessAction Action { get; }
		bool DisableWatchOSOnWrench { get; }
		string DOTNET { get; }
		bool DryRun { get; }
		bool ENABLE_XAMARIN { get; }
		Dictionary<string, string> EnvironmentVariables { get; }
		ILog HarnessLog { get; set; }
		string GetStandardErrorTty ();
		bool InCI { get; }
		bool INCLUDE_IOS { get; }
		bool INCLUDE_MAC { get; }
		bool INCLUDE_TVOS { get; }
		bool INCLUDE_WATCH { get; }
		bool? IncludeSystemPermissionTests { get; set; }
		string IOS_DESTDIR { get; }
		List<iOSTestProject> IOSTestProjects { get; }
		string JENKINS_RESULTS_DIRECTORY { get; }
		string JenkinsConfiguration { get; }
		HashSet<string> Labels { get; }
		double LaunchTimeout { get; }
		string LogDirectory { get; }
		string MAC_DESTDIR { get; }
		List<MacTestProject> MacTestProjects { get; }
		string MarkdownSummaryPath { get; }
		string MlaunchPath { get; }
		string MONO_IOS_SDK_DESTDIR { get; }
		string MONO_MAC_SDK_DESTDIR { get; }
		string MONO_PATH { get; }
		string PeriodicCommand { get; }
		string PeriodicCommandArguments { get; }
		TimeSpan PeriodicCommandInterval { get; }
		IProcessManager ProcessManager { get; }
		double Timeout { get; }
		string TodayContainerTemplate { get; }
		string TodayExtensionTemplate { get; }
		bool UseGroupedApps { get; }
		int Verbosity { get; }
		string WatchOSAppTemplate { get; }
		string WatchOSContainerTemplate { get; }
		string WatchOSExtensionTemplate { get; }
		string XcodeRoot { get; }
		Version XcodeVersion { get; }
		XmlResultJargon XmlJargon { get; }
		Task<ProcessExecutionResult> ExecuteXcodeCommandAsync (string executable, IList<string> args, ILog log, TimeSpan timeout);
		bool GetIncludeSystemPermissionTests (TestPlatform platform, bool device);
		void Log (int min_level, string message, params object [] args);
		void Log (string message);
		void Log (string message, params object [] args);
		void Save (StringWriter doc, string path);
	}

	public class Harness : IHarness {
		readonly AppRunnerTarget target;
		readonly string buildConfiguration = "Debug";

		public HarnessAction Action { get; }
		public int Verbosity { get; }
		public ILog HarnessLog { get; set; }
		public HashSet<string> Labels { get; }
		public XmlResultJargon XmlJargon { get; }
		public IProcessManager ProcessManager { get; }
		public IResultParser ResultParser { get; }

		// This is the maccore/tests directory.
		static string root_directory;
		public static string RootDirectory {
			get {
				if (root_directory == null) {
					var testAssemblyDirectory = Path.GetDirectoryName (System.Reflection.Assembly.GetExecutingAssembly ().Location);
					var dir = testAssemblyDirectory;
					var path = Path.Combine (testAssemblyDirectory, ".git");
					while (!Directory.Exists (path) && path.Length > 3) {
						dir = Path.GetDirectoryName (dir);
						path = Path.Combine (dir, ".git");
					}
					if (!Directory.Exists (path))
						throw new Exception ("Could not find the xamarin-macios repo.");
					path = Path.Combine (Path.GetDirectoryName (path), "tests");
					if (!Directory.Exists (path))
						throw new Exception ("Could not find the tests directory.");
					root_directory = path;
				}
				return root_directory;
			}
			set {
				root_directory = value;
				if (root_directory != null)
					root_directory = Path.GetFullPath (root_directory).TrimEnd ('/');
			}
		}

		string sdkRoot;
		string SdkRoot {
			get => sdkRoot;
			set {
				sdkRoot = value;
				XcodeRoot = FindXcode (sdkRoot);
			}
		}

		public List<iOSTestProject> IOSTestProjects { get; }
		public List<MacTestProject> MacTestProjects { get; } = new List<MacTestProject> ();

		// Configure
		readonly bool useSystemXamarinIOSMac; // if the system XI/XM should be used, or the locally build XI/XM.
		readonly bool autoConf;
		readonly bool mac;

		public string WatchOSContainerTemplate { get; private set; }
		public string WatchOSAppTemplate { get; private set; }
		public string WatchOSExtensionTemplate { get; private set; }
		public string TodayContainerTemplate { get; private set; }
		public string TodayExtensionTemplate { get; private set; }
		public string BCLTodayExtensionTemplate { get; private set; }
		public string MONO_PATH { get; private set; } // Use same name as in Makefiles, so that a grep finds it.
		public string TVOS_MONO_PATH { get; private set; } // Use same name as in Makefiles, so that a grep finds it.
		public bool INCLUDE_IOS { get; private set; }
		public bool INCLUDE_TVOS { get; private set; }
		public bool INCLUDE_WATCH { get; private set; }
		public bool INCLUDE_MAC { get; private set; }
		public string JENKINS_RESULTS_DIRECTORY { get; private set; } // Use same name as in Makefiles, so that a grep finds it.
		public string MAC_DESTDIR { get; private set; }
		public string IOS_DESTDIR { get; private set; }
		public string MONO_IOS_SDK_DESTDIR { get; private set; }
		public string MONO_MAC_SDK_DESTDIR { get; private set; }
		public bool ENABLE_XAMARIN { get; private set; }
		public string DOTNET { get; private set; }

		// Run

		public string XcodeRoot { get; private set; }
		public string LogDirectory { get; } = Environment.CurrentDirectory;
		public double Timeout { get; } = 15; // in minutes
		public double LaunchTimeout { get; } // in minutes
		public bool DryRun { get; } // Most things don't support this. If you need it somewhere, implement it!
		public string JenkinsConfiguration { get; }
		public Dictionary<string, string> EnvironmentVariables { get; } = new Dictionary<string, string> ();
		public string MarkdownSummaryPath { get; }
		public string PeriodicCommand { get; }
		public string PeriodicCommandArguments { get; }
		public TimeSpan PeriodicCommandInterval { get; }
		// whether tests that require access to system resources (system contacts, photo library, etc) should be executed or not
		public bool? IncludeSystemPermissionTests { get; set; }

		public string GetStandardErrorTty () => Helpers.GetTerminalName (2);

		public Harness (IProcessManager processManager, IResultParser resultParser, HarnessAction action, HarnessConfiguration configuration)
		{
			ProcessManager = processManager ?? throw new ArgumentNullException (nameof (processManager));
			ResultParser = resultParser ?? throw new ArgumentNullException (nameof (resultParser));
			Action = action;

			if (configuration is null)
				throw new ArgumentNullException (nameof (configuration));

			autoConf = configuration.AutoConf;
			buildConfiguration = configuration.BuildConfiguration ?? throw new ArgumentNullException (nameof (configuration));
			DryRun = configuration.DryRun;
			IncludeSystemPermissionTests = configuration.IncludeSystemPermissionTests;
			IOSTestProjects = configuration.IOSTestProjects;
			JenkinsConfiguration = configuration.JenkinsConfiguration;
			LogDirectory = configuration.LogDirectory ?? throw new ArgumentNullException (nameof (configuration.LogDirectory));
			mac = configuration.Mac;
			MarkdownSummaryPath = configuration.MarkdownSummaryPath;
			PeriodicCommand = configuration.PeriodicCommand;
			PeriodicCommandArguments = configuration.PeriodicCommandArguments;
			PeriodicCommandInterval = configuration.PeriodicCommandInterval;
			SdkRoot = configuration.SdkRoot;
			target = configuration.Target;
			Timeout = configuration.TimeoutInMinutes;
			useSystemXamarinIOSMac = configuration.UseSystemXamarinIOSMac;
			Verbosity = configuration.Verbosity;
			WatchOSAppTemplate = configuration.WatchOSAppTemplate;
			WatchOSContainerTemplate = configuration.WatchOSContainerTemplate;
			XmlJargon = configuration.XmlJargon;

			if (configuration.Labels != null)
				Labels = new HashSet<string> (configuration.Labels);

			if (configuration.EnvironmentVariables != null)
				EnvironmentVariables = new Dictionary<string, string> (configuration.EnvironmentVariables);

			LaunchTimeout = InCI ? 3 : 120;
		}

		public bool GetIncludeSystemPermissionTests (TestPlatform platform, bool device)
		{
			// If we've been told something in particular, that takes precedence.
			if (IncludeSystemPermissionTests.HasValue)
				return IncludeSystemPermissionTests.Value;

			// If we haven't been told, try to be smart.
			switch (platform) {
			case TestPlatform.iOS:
			case TestPlatform.Mac:
			case TestPlatform.Mac_Full:
			case TestPlatform.Mac_Modern:
			case TestPlatform.Mac_System:
				// On macOS we can't edit the TCC database easily
				// (it requires adding the mac has to be using MDM: https://carlashley.com/2018/09/28/tcc-round-up/)
				// So by default ignore any tests that would pop up permission dialogs in CI.
				return !InCI;
			default:
				// On device we have the same issue as on the mac: we can't edit the TCC database.
				if (device)
					return !InCI;
				// But in the simulator we can just write to the simulator's TCC database (and we do)
				return true;
			}
		}

		static string FindXcode (string path)
		{
			if (string.IsNullOrEmpty (path))
				return path;

			do {
				if (path == "/") {
					throw new Exception (string.Format ("Could not find Xcode.app in {0}", path));
				} else if (File.Exists (Path.Combine (path, "Contents", "MacOS", "Xcode"))) {
					return path;
				}

				path = Path.GetDirectoryName (path);
			} while (true);
		}

		Version xcode_version;
		public Version XcodeVersion {
			get {
				if (xcode_version == null) {
					var doc = new XmlDocument ();
					doc.Load (Path.Combine (XcodeRoot, "Contents", "version.plist"));
					xcode_version = Version.Parse (doc.SelectSingleNode ("//key[text() = 'CFBundleShortVersionString']/following-sibling::string").InnerText);
				}
				return xcode_version;
			}
		}

		public string MlaunchPath {
			get {
				return Path.Combine (IOS_DESTDIR, "Library", "Frameworks", "Xamarin.iOS.framework", "Versions", "Current", "bin", "mlaunch");
			}
		}

		void LoadConfig ()
		{
			ParseConfigFiles ();
			var src_root = Path.GetDirectoryName (Path.GetFullPath (RootDirectory));
			MONO_PATH = Path.GetFullPath (Path.Combine (src_root, "external", "mono"));
			TVOS_MONO_PATH = MONO_PATH;
			INCLUDE_IOS = make_config.ContainsKey ("INCLUDE_IOS") && !string.IsNullOrEmpty (make_config ["INCLUDE_IOS"]);
			INCLUDE_TVOS = make_config.ContainsKey ("INCLUDE_TVOS") && !string.IsNullOrEmpty (make_config ["INCLUDE_TVOS"]);
			JENKINS_RESULTS_DIRECTORY = make_config ["JENKINS_RESULTS_DIRECTORY"];
			INCLUDE_WATCH = make_config.ContainsKey ("INCLUDE_WATCH") && !string.IsNullOrEmpty (make_config ["INCLUDE_WATCH"]);
			INCLUDE_MAC = make_config.ContainsKey ("INCLUDE_MAC") && !string.IsNullOrEmpty (make_config ["INCLUDE_MAC"]);
			MAC_DESTDIR = make_config ["MAC_DESTDIR"];
			IOS_DESTDIR = make_config ["IOS_DESTDIR"];
			if (string.IsNullOrEmpty (SdkRoot))
				SdkRoot = make_config ["XCODE_DEVELOPER_ROOT"];
			MONO_IOS_SDK_DESTDIR = make_config ["MONO_IOS_SDK_DESTDIR"];
			MONO_MAC_SDK_DESTDIR = make_config ["MONO_MAC_SDK_DESTDIR"];
			ENABLE_XAMARIN = make_config.ContainsKey ("ENABLE_XAMARIN") && !string.IsNullOrEmpty (make_config ["ENABLE_XAMARIN"]);
			DOTNET = make_config ["DOTNET"];
		}

		int AutoConfigureMac (bool generate_projects)
		{
			int rv = 0;

			var test_suites = new [] {
				new { Directory = "apitest", ProjectFile = "apitest", Name = "apitest", Flavors = MacFlavors.Full | MacFlavors.Modern },
				new { Directory = "linker/mac/dont link", ProjectFile = "dont link-mac", Name = "dont link", Flavors = MacFlavors.Modern | MacFlavors.Full | MacFlavors.System },
			};
			foreach (var p in test_suites) {
				MacTestProjects.Add (new MacTestProject (Path.GetFullPath (Path.Combine (RootDirectory, p.Directory, p.ProjectFile + ".csproj"))) {
					Name = p.Name,
					TargetFrameworkFlavors = p.Flavors,
				});
			}

			MacTestProjects.Add (new MacTestProject (Path.GetFullPath (Path.Combine (RootDirectory, "introspection", "Mac", "introspection-mac.csproj")), targetFrameworkFlavor: MacFlavors.Modern) { Name = "introspection" });

			var hard_coded_test_suites = new [] {
				new { Directory = "mmptest", ProjectFile = "mmptest", Name = "mmptest", IsNUnit = true, Configurations = (string[]) null, Platform = "x86", Flavors = MacFlavors.Console, },
				new { Directory = "msbuild-mac", ProjectFile = "msbuild-mac", Name = "MSBuild tests", IsNUnit = true, Configurations = (string[]) null, Platform = "x86", Flavors = MacFlavors.Console, },
				new { Directory = "xammac_tests", ProjectFile = "xammac_tests", Name = "xammac tests", IsNUnit = false, Configurations = new string [] { "Debug", "Release" }, Platform = "AnyCPU", Flavors = MacFlavors.Modern, },
				new { Directory = "linker/mac/link all", ProjectFile = "link all-mac", Name = "link all", IsNUnit = false, Configurations = new string [] { "Debug", "Release" }, Platform = "x86", Flavors = MacFlavors.Modern, },
				new { Directory = "linker/mac/link sdk", ProjectFile = "link sdk-mac", Name = "link sdk", IsNUnit = false, Configurations = new string [] { "Debug", "Release" }, Platform = "x86", Flavors = MacFlavors.Modern, },
			};
			foreach (var p in hard_coded_test_suites) {
				MacTestProjects.Add (new MacTestProject (Path.GetFullPath (Path.Combine (RootDirectory, p.Directory, p.ProjectFile + ".csproj")), targetFrameworkFlavor: p.Flavors) {
					Name = p.Name,
					IsNUnitProject = p.IsNUnit,
					SolutionPath = Path.GetFullPath (Path.Combine (RootDirectory, "tests-mac.sln")),
					Configurations = p.Configurations,
					Platform = p.Platform,
				});
			}

			foreach (var flavor in new MonoNativeFlavor [] { MonoNativeFlavor.Compat, MonoNativeFlavor.Unified }) {
				var monoNativeInfo = new MacMonoNativeInfo (this, flavor);
				var macTestProject = new MacTestProject (monoNativeInfo.ProjectPath, targetFrameworkFlavor: MacFlavors.Modern | MacFlavors.Full) {
					MonoNativeInfo = monoNativeInfo,
					Name = monoNativeInfo.ProjectName,
					Platform = "AnyCPU",

				};

				MacTestProjects.Add (macTestProject);
			}

			var monoImportTestFactory = new BCLTestImportTargetFactory (this);
			MacTestProjects.AddRange (monoImportTestFactory.GetMacBclTargets ());

			// Generate test projects from templates (bcl/mono-native templates)
			if (generate_projects) {
				foreach (var mtp in MacTestProjects.Where (x => x.MonoNativeInfo != null).Select (x => x.MonoNativeInfo))
					mtp.Convert ();
			}

			// All test projects should be either Modern projects or NUnit/console executables at this point.
			// If we need to generate Full/System variations, we do that here.
			var unified_targets = new List<MacTarget> ();

			Action<MacTarget, string, bool, bool> configureTarget = (MacTarget target, string file, bool isNUnitProject, bool skip_generation) => {
				target.TemplateProjectPath = file;
				target.Harness = this;
				target.IsNUnitProject = isNUnitProject;
				if (!generate_projects || skip_generation)
					target.ShouldSkipProjectGeneration = true;
				target.Execute ();
			};

			foreach (var proj in MacTestProjects) {
				var target = new MacTarget (MacFlavors.Modern);
				target.MonoNativeInfo = proj.MonoNativeInfo;
				configureTarget (target, proj.Path, proj.IsNUnitProject, true);
				unified_targets.Add (target);
			}

			foreach (var proj in MacTestProjects.Where ((v) => v.GenerateVariations).ToArray ()) {
				var file = proj.Path;
				if (!File.Exists (file)) {
					Console.WriteLine ($"Can't find the project file {file}.");
					rv = 1;
					continue;
				}

				// Generate variations if requested
				if (proj.GenerateFull) {
					var target = new MacTarget (MacFlavors.Full);
					target.MonoNativeInfo = proj.MonoNativeInfo;
					configureTarget (target, file, proj.IsNUnitProject, false);
					unified_targets.Add (target);

					var cloned_project = (MacTestProject) proj.Clone ();
					cloned_project.TargetFrameworkFlavors = MacFlavors.Full;
					cloned_project.Path = target.ProjectPath;
					MacTestProjects.Add (cloned_project);
				}

				if (proj.GenerateSystem) {
					var target = new MacTarget (MacFlavors.System);
					target.MonoNativeInfo = proj.MonoNativeInfo;
					configureTarget (target, file, proj.IsNUnitProject, false);
					unified_targets.Add (target);

					var cloned_project = (MacTestProject) proj.Clone ();
					cloned_project.TargetFrameworkFlavors = MacFlavors.System;
					cloned_project.Path = target.ProjectPath;
					MacTestProjects.Add (cloned_project);
				}

				// We're done generating now
				// Re-use the existing TestProject instance instead of creating a new one.
				proj.TargetFrameworkFlavors = MacFlavors.Modern; // the default/template flavor is 'Modern'
			}

			if (generate_projects)
				MakefileGenerator.CreateMacMakefile (this, unified_targets);

			return rv;
		}

		void AutoConfigureIOS ()
		{
			var test_suites = new string [] { "monotouch-test", "framework-test", "interdependent-binding-projects" };
			var library_projects = new string [] { "BundledResources", "EmbeddedResources", "bindings-test", "bindings-test2", "bindings-framework-test" };
			var fsharp_test_suites = new string [] { "fsharp" };
			var fsharp_library_projects = new string [] { "fsharplibrary" };

			foreach (var p in test_suites)
				IOSTestProjects.Add (new iOSTestProject (Path.GetFullPath (Path.Combine (RootDirectory, p + "/" + p + ".csproj"))) { Name = p });
			foreach (var p in fsharp_test_suites)
				IOSTestProjects.Add (new iOSTestProject (Path.GetFullPath (Path.Combine (RootDirectory, p + "/" + p + ".fsproj"))) { Name = p });
			foreach (var p in library_projects)
				IOSTestProjects.Add (new iOSTestProject (Path.GetFullPath (Path.Combine (RootDirectory, p + "/" + p + ".csproj")), false) { Name = p });
			foreach (var p in fsharp_library_projects)
				IOSTestProjects.Add (new iOSTestProject (Path.GetFullPath (Path.Combine (RootDirectory, p + "/" + p + ".fsproj")), false) { Name = p });

			IOSTestProjects.Add (new iOSTestProject (Path.GetFullPath (Path.Combine (RootDirectory, "introspection", "iOS", "introspection-ios.csproj"))) { Name = "introspection" });
			IOSTestProjects.Add (new iOSTestProject (Path.GetFullPath (Path.Combine (RootDirectory, "linker", "ios", "dont link", "dont link.csproj"))) { Configurations = new string [] { "Debug", "Release" } });
			IOSTestProjects.Add (new iOSTestProject (Path.GetFullPath (Path.Combine (RootDirectory, "linker", "ios", "link all", "link all.csproj"))) { Configurations = new string [] { "Debug", "Release" } });
			IOSTestProjects.Add (new iOSTestProject (Path.GetFullPath (Path.Combine (RootDirectory, "linker", "ios", "link sdk", "link sdk.csproj"))) { Configurations = new string [] { "Debug", "Release" } });

			foreach (var flavor in new MonoNativeFlavor [] { MonoNativeFlavor.Compat, MonoNativeFlavor.Unified }) {
				var monoNativeInfo = new MonoNativeInfo (this, flavor);
				var iosTestProject = new iOSTestProject (monoNativeInfo.ProjectPath) {
					MonoNativeInfo = monoNativeInfo,
					Name = monoNativeInfo.ProjectName,
					SkipwatchOSARM64_32Variation = monoNativeInfo.ProjectName.Contains ("compat"),
				};

				IOSTestProjects.Add (iosTestProject);
			}

			// add all the tests that are using the precompiled mono assemblies
			var monoImportTestFactory = new BCLTestImportTargetFactory (this);
			IOSTestProjects.AddRange (monoImportTestFactory.GetiOSBclTargets ());

			WatchOSContainerTemplate = Path.GetFullPath (Path.Combine (RootDirectory, "templates/WatchContainer"));
			WatchOSAppTemplate = Path.GetFullPath (Path.Combine (RootDirectory, "templates/WatchApp"));
			WatchOSExtensionTemplate = Path.GetFullPath (Path.Combine (RootDirectory, "templates/WatchExtension"));

			TodayContainerTemplate = Path.GetFullPath (Path.Combine (RootDirectory, "templates", "TodayContainer"));
			TodayExtensionTemplate = Path.GetFullPath (Path.Combine (RootDirectory, "templates", "TodayExtension"));
			BCLTodayExtensionTemplate = Path.GetFullPath (Path.Combine (RootDirectory, "bcl-test", "templates", "today"));
		}

		Dictionary<string, string> make_config = new Dictionary<string, string> ();
		IEnumerable<string> FindConfigFiles (string name)
		{
			var dir = Path.GetFullPath (RootDirectory);
			while (dir != "/") {
				var file = Path.Combine (dir, name);
				if (File.Exists (file))
					yield return file;
				dir = Path.GetDirectoryName (dir);
			}
		}

		void ParseConfigFiles ()
		{
			ParseConfigFiles (FindConfigFiles (useSystemXamarinIOSMac ? "test-system.config" : "test.config"));
			ParseConfigFiles (FindConfigFiles ("Make.config.local"));
			ParseConfigFiles (FindConfigFiles ("Make.config"));
		}

		void ParseConfigFiles (IEnumerable<string> files)
		{
			foreach (var file in files)
				ParseConfigFile (file);
		}

		void ParseConfigFile (string file)
		{
			if (string.IsNullOrEmpty (file))
				return;

			foreach (var line in File.ReadAllLines (file)) {
				var eq = line.IndexOf ('=');
				if (eq == -1)
					continue;
				var key = line.Substring (0, eq);
				if (!make_config.ContainsKey (key))
					make_config [key] = line.Substring (eq + 1);
			}
		}

		int Configure ()
		{
			return mac ? AutoConfigureMac (true) : ConfigureIOS ();
		}

		int ConfigureIOS ()
		{
			var rv = 0;
			var unified_targets = new List<UnifiedTarget> ();
			var tvos_targets = new List<TVOSTarget> ();
			var watchos_targets = new List<WatchOSTarget> ();
			var today_targets = new List<TodayExtensionTarget> ();

			if (autoConf)
				AutoConfigureIOS ();

			foreach (var monoNativeInfo in IOSTestProjects.Where (x => x.MonoNativeInfo != null).Select (x => x.MonoNativeInfo))
				monoNativeInfo.Convert ();

			foreach (var proj in IOSTestProjects) {
				var file = proj.Path;

				if (proj.MonoNativeInfo != null)
					file = proj.MonoNativeInfo.TemplatePath;

				if (!File.Exists (file)) {
					Console.WriteLine ($"Can't find the project file {file}.");
					rv = 1;
					continue;
				}

				if (!proj.SkipwatchOSVariation) {
					var watchos = new WatchOSTarget () {
						TemplateProjectPath = file,
						Harness = this,
						TestProject = proj,
					};
					watchos.Execute ();
					watchos_targets.Add (watchos);
				}

				if (!proj.SkiptvOSVariation) {
					var tvos = new TVOSTarget () {
						TemplateProjectPath = file,
						Harness = this,
						TestProject = proj,
					};
					tvos.Execute ();
					tvos_targets.Add (tvos);
				}

				if (!proj.SkipiOSVariation) {
					var unified = new UnifiedTarget () {
						TemplateProjectPath = file,
						Harness = this,
						TestProject = proj,
					};
					unified.Execute ();
					unified_targets.Add (unified);

					var today = new TodayExtensionTarget {
						TemplateProjectPath = file,
						Harness = this,
						TestProject = proj,
					};
					today.Execute ();
					today_targets.Add (today);
				}
			}

			SolutionGenerator.CreateSolution (this, watchos_targets, "watchos");
			SolutionGenerator.CreateSolution (this, tvos_targets, "tvos");
			SolutionGenerator.CreateSolution (this, today_targets, "today");
			MakefileGenerator.CreateMakefile (this, unified_targets, tvos_targets, watchos_targets, today_targets);

			return rv;
		}

		int Install ()
		{
			if (HarnessLog == null)
				HarnessLog = new ConsoleLog ();

			foreach (var project in IOSTestProjects) {
				var runner = new AppRunner (ProcessManager,
					new SimulatorsLoaderFactory (this, ProcessManager),
					new SimpleListenerFactory (),
					new DeviceLoaderFactory (this, ProcessManager),
					new CrashSnapshotReporterFactory (ProcessManager, XcodeRoot, MlaunchPath),
					new CaptureLogFactory (),
					new DeviceLogCapturerFactory (ProcessManager, XcodeRoot, MlaunchPath),
					new XmlResultParser (),
					target,
					this,
					HarnessLog,
					project.Path,
					buildConfiguration);

				using (var install_log = new AppInstallMonitorLog (runner.MainLog)) {
					var rv = runner.InstallAsync (install_log.CancellationToken).Result;
					if (!rv.Succeeded)
						return rv.ExitCode;
				}
			}
			return 0;
		}

		int Uninstall ()
		{
			if (HarnessLog == null)
				HarnessLog = new ConsoleLog ();

			foreach (var project in IOSTestProjects) {
				var runner = new AppRunner (ProcessManager,
					new SimulatorsLoaderFactory (this, ProcessManager),
					new SimpleListenerFactory (),
					new DeviceLoaderFactory (this, ProcessManager),
					new CrashSnapshotReporterFactory (ProcessManager, XcodeRoot, MlaunchPath),
					new CaptureLogFactory (),
					new DeviceLogCapturerFactory (ProcessManager, XcodeRoot, MlaunchPath),
					new XmlResultParser (),
					target,
					this,
					HarnessLog,
					project.Path,
					buildConfiguration);

				var rv = runner.UninstallAsync ().Result;
				if (!rv.Succeeded)
					return rv.ExitCode;
			}
			return 0;
		}

		int Run ()
		{
			if (HarnessLog == null)
				HarnessLog = new ConsoleLog ();

			foreach (var project in IOSTestProjects) {
				var runner = new AppRunner (ProcessManager,
					new SimulatorsLoaderFactory (this, ProcessManager),
					new SimpleListenerFactory (),
					new DeviceLoaderFactory (this, ProcessManager),
					new CrashSnapshotReporterFactory (ProcessManager, XcodeRoot, MlaunchPath),
					new CaptureLogFactory (),
					new DeviceLogCapturerFactory (ProcessManager, XcodeRoot, MlaunchPath),
					new XmlResultParser (),
					target,
					this,
					HarnessLog,
					project.Path,
					buildConfiguration);

				var rv = runner.RunAsync ().Result;
				if (rv != 0)
					return rv;
			}
			return 0;
		}

		void Log (int min_level, string message)
		{
			if (Verbosity < min_level)
				return;
			Console.WriteLine (message);
			HarnessLog?.WriteLine (message);
		}

		public void Log (int min_level, string message, params object [] args)
		{
			if (Verbosity < min_level)
				return;
			Console.WriteLine (message, args);
			HarnessLog?.WriteLine (message, args);
		}

		public void Log (string message)
		{
			Log (0, message);
		}

		public void Log (string message, params object [] args)
		{
			Log (0, message, args);
		}

		public bool InCI {
			get {
				// We use the 'BUILD_REVISION' variable to detect whether we're running CI or not.
				return !string.IsNullOrEmpty (Environment.GetEnvironmentVariable ("BUILD_REVISION"));
			}
		}

		public bool UseGroupedApps {
			get {
				var groupApps = Environment.GetEnvironmentVariable ("BCL_GROUPED_APPS");
				return string.IsNullOrEmpty (groupApps) || groupApps == "grouped";
			}
		}

		public int Execute ()
		{
			LoadConfig ();
			switch (Action) {
			case HarnessAction.Configure:
				return Configure ();
			case HarnessAction.Run:
				return Run ();
			case HarnessAction.Install:
				return Install ();
			case HarnessAction.Uninstall:
				return Uninstall ();
			case HarnessAction.Jenkins:
				return Jenkins ();
			default:
				throw new NotImplementedException (Action.ToString ());
			}
		}

		int Jenkins ()
		{
			if (autoConf) {
				AutoConfigureIOS ();
				AutoConfigureMac (false);
			}

			var jenkins = new Jenkins.Jenkins (this, ProcessManager, ResultParser);
			return jenkins.Run ();
		}

		public void Save (XmlDocument doc, string path)
		{
			if (!File.Exists (path)) {
				doc.Save (path);
				Log (1, "Created {0}", path);
			} else {
				var tmpPath = path + ".tmp";
				doc.Save (tmpPath);
				var existing = File.ReadAllText (path);
				var updated = File.ReadAllText (tmpPath);

				if (existing == updated) {
					File.Delete (tmpPath);
					Log (1, "Not saved {0}, no change", path);
				} else {
					File.Delete (path);
					File.Move (tmpPath, path);
					Log (1, "Updated {0}", path);
				}
			}
		}

		public void Save (StringWriter doc, string path)
		{
			if (!File.Exists (path)) {
				File.WriteAllText (path, doc.ToString ());
				Log (1, "Created {0}", path);
			} else {
				var existing = File.ReadAllText (path);
				var updated = doc.ToString ();

				if (existing == updated) {
					Log (1, "Not saved {0}, no change", path);
				} else {
					File.WriteAllText (path, updated);
					Log (1, "Updated {0}", path);
				}
			}
		}

		bool? disable_watchos_on_wrench;

		public bool DisableWatchOSOnWrench {
			get {
				if (!disable_watchos_on_wrench.HasValue)
					disable_watchos_on_wrench = !string.IsNullOrEmpty (Environment.GetEnvironmentVariable ("DISABLE_WATCH_ON_WRENCH"));
				return disable_watchos_on_wrench.Value;
			}
		}

		public Task<ProcessExecutionResult> ExecuteXcodeCommandAsync (string executable, IList<string> args, ILog log, TimeSpan timeout)
		{
			return ProcessManager.ExecuteCommandAsync (Path.Combine (XcodeRoot, "Contents", "Developer", "usr", "bin", executable), args, log, timeout: timeout);
		}
	}
}
