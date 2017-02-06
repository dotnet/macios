using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;

using Xamarin.MacDev;
using Xamarin.Utils;

namespace Xamarin.Bundler
{
	public abstract class ProcessTask : BuildTask
	{
		public ProcessStartInfo ProcessStartInfo;
		protected StringBuilder Output;

		protected string Command {
			get {
				var result = new StringBuilder ();
				if (ProcessStartInfo.EnvironmentVariables.ContainsKey ("MONO_PATH")) {
					result.Append ("MONO_PATH=");
					result.Append (ProcessStartInfo.EnvironmentVariables ["MONO_PATH"]);
					result.Append (' ');
				}
				result.Append (ProcessStartInfo.FileName);
				result.Append (' ');
				result.Append (ProcessStartInfo.Arguments);
				return result.ToString ();
			}
		}

		protected int Start ()
		{
			if (Driver.Verbosity > 0)
				Console.WriteLine (Command);

			var info = ProcessStartInfo;
			var stdout_completed = new ManualResetEvent (false);
			var stderr_completed = new ManualResetEvent (false);

			Output = new StringBuilder ();

			using (var p = Process.Start (info)) {
				p.OutputDataReceived += (sender, e) =>
				{
					if (e.Data != null) {
						lock (Output)
							Output.AppendLine (e.Data);
					} else {
						stdout_completed.Set ();
					}
				};

				p.ErrorDataReceived += (sender, e) =>
				{
					if (e.Data != null) {
						lock (Output)
							Output.AppendLine (e.Data);
					} else {
						stderr_completed.Set ();
					}
				};

				p.BeginOutputReadLine ();
				p.BeginErrorReadLine ();

				p.WaitForExit ();

				stderr_completed.WaitOne (TimeSpan.FromSeconds (1));
				stdout_completed.WaitOne (TimeSpan.FromSeconds (1));

				GC.Collect (); // Workaround for: https://bugzilla.xamarin.com/show_bug.cgi?id=43462#c14

				if (p.ExitCode != 0)
					return p.ExitCode;

				if (Driver.Verbosity >= 2 && Output.Length > 0)
					Console.Error.WriteLine (Output.ToString ());
			}

			return 0;
		}
	}

	internal class MainTask : CompileTask
	{
		public static void Create (List<BuildTask> tasks, Target target, Abi abi, IEnumerable<Assembly> assemblies, string assemblyName, IList<string> registration_methods)
		{
			var app = target.App;
			var arch = abi.AsArchString ();
			var ofile = Path.Combine (app.Cache.Location, "main." + arch + ".o");
			var ifile = Path.Combine (app.Cache.Location, "main." + arch + ".m");

			var files = assemblies.Select (v => v.FullPath);

			if (!Application.IsUptodate (files, new string [] { ifile })) {
				Driver.GenerateMain (target.App, assemblies, assemblyName, abi, ifile, registration_methods);
			} else {
				Driver.Log (3, "Target '{0}' is up-to-date.", ifile);
			}

			if (!Application.IsUptodate (ifile, ofile)) {
				var main = new MainTask ()
				{
					Target = target,
					Abi = abi,
					AssemblyName = assemblyName,
					InputFile = ifile,
					OutputFile = ofile,
					SharedLibrary = false,
					Language = "objective-c++",
				};
				main.CompilerFlags.AddDefine ("MONOTOUCH");
				tasks.Add (main);
			} else {
				Driver.Log (3, "Target '{0}' is up-to-date.", ofile);
			}

			target.LinkWith (ofile);
		}

		protected override void Build ()
		{
			if (Compile () != 0)
				throw new MonoTouchException (5103, true, "Failed to compile the file '{0}'. Please file a bug report at http://bugzilla.xamarin.com", InputFile);
		}
	}

	internal class PinvokesTask : CompileTask
	{
		public static void Create (List<BuildTask> tasks, IEnumerable<Abi> abis, Target target, string ifile)
		{
			foreach (var abi in abis)
				Create (tasks, abi, target, ifile);
		}

		public static void Create (List<BuildTask> tasks, Abi abi, Target target, string ifile)
		{
			var arch = abi.AsArchString ();
			var ext = target.App.FastDev ? ".dylib" : ".o";
			var ofile = Path.Combine (target.App.Cache.Location, "lib" + Path.GetFileNameWithoutExtension (ifile) + "." + arch + ext);

			if (!Application.IsUptodate (ifile, ofile)) {
				var task = new PinvokesTask ()
				{
					Target = target,
					Abi = abi,
					InputFile = ifile,
					OutputFile = ofile,
					SharedLibrary = target.App.FastDev,
					Language = "objective-c++",
				};
				if (target.App.FastDev) {
					task.InstallName = "lib" + Path.GetFileNameWithoutExtension (ifile) + ext;
					task.CompilerFlags.AddFramework ("Foundation");
					task.CompilerFlags.LinkWithXamarin ();
				}
				tasks.Add (task);
			} else {
				Driver.Log (3, "Target '{0}' is up-to-date.", ofile);
			}

			target.LinkWith (ofile);
			target.LinkWithAndShip (ofile);
		}

		protected override void Build ()
		{
			if (Compile () != 0)
				throw new MonoTouchException (4002, true, "Failed to compile the generated code for P/Invoke methods. Please file a bug report at http://bugzilla.xamarin.com");
		}
	}

	internal class RegistrarTask : CompileTask
	{
		public static void Create (List<BuildTask> tasks, IEnumerable<Abi> abis, Target target, string ifile)
		{
			foreach (var abi in abis)
				Create (tasks, abi, target, ifile);
		}

		public static void Create (List<BuildTask> tasks, Abi abi, Target target, string ifile)
		{
			var app = target.App;
			var arch = abi.AsArchString ();
			var ofile = Path.Combine (app.Cache.Location, Path.GetFileNameWithoutExtension (ifile) + "." + arch + ".o");

			if (!Application.IsUptodate (ifile, ofile)) {
				tasks.Add (new RegistrarTask ()
				{
					Target = target,
					Abi = abi,
					InputFile = ifile,
					OutputFile = ofile,
					SharedLibrary = false,
					Language = "objective-c++",
				});
			} else {
				Driver.Log (3, "Target '{0}' is up-to-date.", ofile);
			}

			target.LinkWith (ofile);
		}

		protected override void Build ()
		{
			if (Driver.IsUsingClang (App)) {
				// This is because iOS has a forward declaration of NSPortMessage, but no actual declaration.
				// They still use NSPortMessage in other API though, so it can't just be removed from our bindings.
				CompilerFlags.AddOtherFlag ("-Wno-receiver-forward-class");
			}

			if (Compile () != 0)
				throw new MonoTouchException (4109, true, "Failed to compile the generated registrar code. Please file a bug report at http://bugzilla.xamarin.com");
		}
	}

	public class AOTTask : ProcessTask
	{
		public string AssemblyName;
		public bool AddBitcodeMarkerSection;
		public string AssemblyPath; // path to the .s file.

		// executed with Parallel.ForEach
		protected override void Build ()
		{
			var exit_code = base.Start ();

			if (exit_code == 0) {
				if (AddBitcodeMarkerSection)
					File.AppendAllText (AssemblyPath, @"
.section __LLVM, __bitcode
.byte 0
.section __LLVM, __cmdline
.byte 0
");
				return;
			}

			Console.Error.WriteLine ("AOT Compilation exited with code {0}, command:\n{1}{2}", exit_code, Command, Output.Length > 0 ? ("\n" + Output.ToString ()) : string.Empty);
			if (Output.Length > 0) {
				List<Exception> exceptions = new List<Exception> ();
				foreach (var line in Output.ToString ().Split ('\n')) {
					if (line.StartsWith ("AOT restriction: Method '", StringComparison.Ordinal) && line.Contains ("must be static since it is decorated with [MonoPInvokeCallback]")) {
						exceptions.Add (new MonoTouchException (3002, true, line));
					}
				}
				if (exceptions.Count > 0)
					throw new AggregateException (exceptions.ToArray ());
			}

			throw new MonoTouchException (3001, true, "Could not AOT the assembly '{0}'", AssemblyName);
		}
	}

	public class LinkTask : CompileTask
	{
	}

	public class CompileTask : BuildTask
	{
		public Target Target;
		public Application App { get { return Target.App; } }
		public bool SharedLibrary;
		public string InputFile;
		public string OutputFile;
		public Abi Abi;
		public string AssemblyName;
		public string InstallName;
		public string Language;

		CompilerFlags compiler_flags;
		public CompilerFlags CompilerFlags {
			get { return compiler_flags ?? (compiler_flags = new CompilerFlags () { Target = Target }); }
			set { compiler_flags = value; }
		}

		public static void GetArchFlags (CompilerFlags flags, params Abi [] abis)
		{
			GetArchFlags (flags, (IEnumerable<Abi>) abis);
		}

		public static void GetArchFlags (CompilerFlags flags, IEnumerable<Abi> abis)
		{
			bool enable_thumb = false;

			foreach (var abi in abis) {
				var arch = abi.AsArchString ();
				flags.AddOtherFlag ($"-arch {arch}");

				enable_thumb |= (abi & Abi.Thumb) != 0;
			}

			if (enable_thumb)
				flags.AddOtherFlag ("-mthumb");
		}

		public static void GetCompilerFlags (Application app, CompilerFlags flags, string ifile, string language = null)
		{
			if (string.IsNullOrEmpty (ifile) || !ifile.EndsWith (".s", StringComparison.Ordinal))
				flags.AddOtherFlag ("-gdwarf-2");

			if (!string.IsNullOrEmpty (ifile) && !ifile.EndsWith (".s", StringComparison.Ordinal)) {
				if (string.IsNullOrEmpty (language) || !language.Contains ("++")) {
					// error: invalid argument '-std=c99' not allowed with 'C++/ObjC++'
					flags.AddOtherFlag ("-std=c99");
				}
				flags.AddOtherFlag ($"-I{Driver.Quote (Path.Combine (Driver.GetProductSdkDirectory (app), "usr", "include"))}");
			}
			flags.AddOtherFlag ($"-isysroot {Driver.Quote (Driver.GetFrameworkDirectory (app))}");
			flags.AddOtherFlag ("-Qunused-arguments"); // don't complain about unused arguments (clang reports -std=c99 and -Isomething as unused).
		}

		public static void GetSimulatorCompilerFlags (CompilerFlags flags, string ifile, Application app, string language = null)
		{
			GetCompilerFlags (app, flags, ifile, language);

			string sim_platform = Driver.GetPlatformDirectory (app);
			string plist = Path.Combine (sim_platform, "Info.plist");

			var dict = Driver.FromPList (plist);
			var dp = dict.Get<PDictionary> ("DefaultProperties");
			if (dp.GetString ("GCC_OBJC_LEGACY_DISPATCH") == "YES")
				flags.AddOtherFlag ("-fobjc-legacy-dispatch");
			string objc_abi = dp.GetString ("OBJC_ABI_VERSION");
			if (!String.IsNullOrWhiteSpace (objc_abi))
				flags.AddOtherFlag ($"-fobjc-abi-version={objc_abi}");

			plist = Path.Combine (Driver.GetFrameworkDirectory (app), "SDKSettings.plist");
			string min_prefix = app.CompilerPath.Contains ("clang") ? Driver.GetTargetMinSdkName (app) : "iphoneos";
			dict = Driver.FromPList (plist);
			dp = dict.Get<PDictionary> ("DefaultProperties");
			if (app.DeploymentTarget == new Version ()) {
				string target = dp.GetString ("IPHONEOS_DEPLOYMENT_TARGET");
				if (!String.IsNullOrWhiteSpace (target))
					flags.AddOtherFlag ($"-m{min_prefix}-version-min={target}");
			} else {
				flags.AddOtherFlag ($"-m{min_prefix}-version-min={app.DeploymentTarget}");
			}
			string defines = dp.GetString ("GCC_PRODUCT_TYPE_PREPROCESSOR_DEFINITIONS");
			if (!String.IsNullOrWhiteSpace (defines))
				flags.AddDefine (defines.Replace (" ", String.Empty));
		}

		void GetDeviceCompilerFlags (CompilerFlags flags, string ifile)
		{
			GetCompilerFlags (App, flags, ifile, Language);

			flags.AddOtherFlag ($"-m{Driver.GetTargetMinSdkName (App)}-version-min={App.DeploymentTarget.ToString ()}");
		}

		void GetSharedCompilerFlags (CompilerFlags flags, string install_name)
		{
			if (string.IsNullOrEmpty (install_name))
				throw new ArgumentNullException (nameof (install_name));

			flags.AddOtherFlag ("-shared");
			if (!App.EnableMarkerOnlyBitCode)
				flags.AddOtherFlag ("-read_only_relocs suppress");
			flags.LinkWithMono ();
			flags.AddOtherFlag ("-install_name " + Driver.Quote ($"@rpath/{install_name}"));
			flags.AddOtherFlag ("-fapplication-extension"); // fixes this: warning MT5203: Native linking warning: warning: linking against dylib not safe for use in application extensions: [..]/actionextension.dll.arm64.dylib
		}

		void GetStaticCompilerFlags (CompilerFlags flags)
		{
			flags.AddOtherFlag ("-c");
		}

		void GetBitcodeCompilerFlags (CompilerFlags flags)
		{
			flags.AddOtherFlag (App.EnableMarkerOnlyBitCode ? "-fembed-bitcode-marker" : "-fembed-bitcode");
		}

		protected override void Build ()
		{
			if (Compile () != 0)
				throw new MonoTouchException (3001, true, "Could not AOT the assembly '{0}'", AssemblyName);
		}

		public int Compile ()
		{
			if (App.IsDeviceBuild) {
				GetDeviceCompilerFlags (CompilerFlags, InputFile);
			} else {
				GetSimulatorCompilerFlags (CompilerFlags, InputFile, App, Language);
			}

			if (App.EnableBitCode)
				GetBitcodeCompilerFlags (CompilerFlags);
			GetArchFlags (CompilerFlags, Abi);

			if (SharedLibrary) {
				GetSharedCompilerFlags (CompilerFlags, InstallName);
			} else {
				GetStaticCompilerFlags (CompilerFlags);
			}

			if (App.EnableDebug)
				CompilerFlags.AddDefine ("DEBUG");

			CompilerFlags.AddOtherFlag ($"-o {Driver.Quote (OutputFile)}");

			if (!string.IsNullOrEmpty (Language))
				CompilerFlags.AddOtherFlag ($"-x {Language}");

			CompilerFlags.AddOtherFlag (Driver.Quote (InputFile));

			var rv = Driver.RunCommand (App.CompilerPath, CompilerFlags.ToString (), null, null);

			return rv;
		}
	}

	public class BitCodeify : BuildTask
	{
		public string Input { get; set; }
		public string OutputFile { get; set; }
		public ApplePlatform Platform { get; set; }
		public Abi Abi { get; set; }
		public Version DeploymentTarget { get; set; }

		protected override void Build ()
		{
			new BitcodeConverter (Input, OutputFile, Platform, Abi, DeploymentTarget).Convert ();
		}
	}
}
