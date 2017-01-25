// Copyright 2013 Xamarin Inc. All rights reserved.

using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Xml.Linq;
using System.IO;
using System.Text;

using MonoTouch.Tuner;

using Mono.Tuner;
using Xamarin.Linker;

using Xamarin.Utils;
using Xamarin.MacDev;

namespace Xamarin.Bundler {

	public enum BitCodeMode {
		None = 0,
		ASMOnly = 1,
		LLVMOnly = 2,
		MarkerOnly = 3,
	}

	[Flags]
	public enum Abi {
		None   =   0,
		i386   =   1,
		ARMv6  =   2,
		ARMv7  =   4,
		ARMv7s =   8,
		ARM64 =   16,
		x86_64 =  32,
		Thumb  =  64,
		LLVM   = 128,
		ARMv7k = 256,
		SimulatorArchMask = i386 | x86_64,
		DeviceArchMask = ARMv6 | ARMv7 | ARMv7s | ARMv7k | ARM64,
		ArchMask = SimulatorArchMask | DeviceArchMask,
		Arch64Mask = x86_64 | ARM64,
		Arch32Mask = i386 | ARMv6 | ARMv7 | ARMv7s | ARMv7k,
	}

	public static class AbiExtensions {
		public static string AsString (this Abi self)
		{
			var rv = (self & Abi.ArchMask).ToString ();
			if ((self & Abi.LLVM) == Abi.LLVM)
				rv += "+LLVM";
			if ((self & Abi.Thumb) == Abi.Thumb)
				rv += "+Thumb";
			return rv;
		}

		public static string AsArchString (this Abi self)
		{
			return (self & Abi.ArchMask).ToString ().ToLowerInvariant ();
		}
	}

	public enum RegistrarMode {
		Default,
		Dynamic,
		Static,
	}

	public enum BuildTarget {
		Simulator,
		Device,
	}

	public enum DlsymOptions
	{
		Default,
		All,
		None,
		Custom,
	}

	public partial class Application
	{
		public string ExecutableName;
		public BuildTarget BuildTarget;

		public bool EnableCxx;
		public bool EnableProfiling;
		bool? package_mdb;
		public bool PackageMdb {
			get { return package_mdb.Value; }
			set { package_mdb = value; }
		}
		bool? enable_msym;
		public bool EnableMSym {
			get { return enable_msym.Value; }
			set { enable_msym = value; }
		}
		public bool EnableRepl;

		public bool IsExtension;
		public List<string> Extensions = new List<string> (); // A list of the extensions this app contains.
		public List<Application> AppExtensions = new List<Application> ();

		public bool FastDev;

		public bool? EnablePie;
		public bool NativeStrip = true;
		public string SymbolList;
		public bool ManagedStrip = true;
		public List<string> NoSymbolStrip = new List<string> ();
		
		public bool? ThreadCheck;
		public DlsymOptions DlsymOptions;
		public List<Tuple<string, bool>> DlsymAssemblies;
		public bool? UseMonoFramework;
		public bool? PackageMonoFramework;

		public bool NoFastSim;

		// The list of assemblies that we do generate debugging info for.
		public bool DebugAll;
		public List<string> DebugAssemblies = new List<string> ();

		public bool? DebugTrack;

		public string Compiler = string.Empty;
		public string CompilerPath;

		public string AotArguments = "static,asmonly,direct-icalls,";
		public string AotOtherArguments = string.Empty;
		public bool? LLVMAsmWriter;
		public Dictionary<string, string> LLVMOptimizations = new Dictionary<string, string> ();

		public Dictionary<string, string> EnvironmentVariables = new Dictionary<string, string> ();

		//
		// Linker config
		//

		public bool LinkAway = true;
		public bool LinkerDumpDependencies { get; set; }
		public List<string> References = new List<string> ();
		
		public bool? BuildDSym;
		public bool Is32Build { get { return IsArchEnabled (Abi.Arch32Mask); } } // If we're targetting a 32 bit arch.
		public bool Is64Build { get { return IsArchEnabled (Abi.Arch64Mask); } } // If we're targetting a 64 bit arch.
		public bool IsDualBuild { get { return Is32Build && Is64Build; } } // if we're building both a 32 and a 64 bit version.
		public bool IsLLVM { get { return IsArchEnabled (Abi.LLVM); } }

		public List<Target> Targets = new List<Target> ();

		public string UserGccFlags;

		// If we didn't link the final executable because the existing binary is up-to-date.
		bool cached_executable; 

		List<Abi> abis;
		HashSet<Abi> all_architectures; // all Abis used in the app, including extensions.

		public string GetLLVMOptimizations (Assembly assembly)
		{
			string opt;
			if (LLVMOptimizations.TryGetValue (assembly.FileName, out opt))
				return opt;
			if (LLVMOptimizations.TryGetValue ("all", out opt))
				return opt;
			return null;
		}

		public void SetDlsymOption (string asm, bool dlsym)
		{
			if (DlsymAssemblies == null)
				DlsymAssemblies = new List<Tuple<string, bool>> ();

			DlsymAssemblies.Add (new Tuple<string, bool> (Path.GetFileNameWithoutExtension (asm), dlsym));

			DlsymOptions = DlsymOptions.Custom;
		}

		public void ParseDlsymOptions (string options)
		{
			bool dlsym;
			if (Driver.TryParseBool (options, out dlsym)) {
				DlsymOptions = dlsym ? DlsymOptions.All : DlsymOptions.None;
			} else {
				DlsymAssemblies = new List<Tuple<string, bool>> ();

				var assemblies = options.Split (',');
				foreach (var assembly in assemblies) {
					var asm = assembly;
					if (assembly.StartsWith ("+", StringComparison.Ordinal)) {
						dlsym = true;
						asm = assembly.Substring (1);
					} else if (assembly.StartsWith ("-", StringComparison.Ordinal)) {
						dlsym = false;
						asm = assembly.Substring (1);
					} else {
						dlsym = true;
					}
					DlsymAssemblies.Add (new Tuple<string, bool> (Path.GetFileNameWithoutExtension (asm), dlsym));
				}

				DlsymOptions = DlsymOptions.Custom;
			}
		}

		public bool UseDlsym (string assembly)
		{
			string asm;

			if (DlsymAssemblies != null) {
				asm = Path.GetFileNameWithoutExtension (assembly);
				foreach (var tuple in DlsymAssemblies) {
					if (string.Equals (tuple.Item1, asm, StringComparison.Ordinal))
						return tuple.Item2;
				}
			}

			switch (DlsymOptions) {
			case DlsymOptions.All:
				return true;
			case DlsymOptions.None:
				return false;
			}

			if (EnableLLVMOnlyBitCode)
				return false;

			switch (Platform) {
			case ApplePlatform.iOS:
				return !Profile.IsSdkAssembly (Path.GetFileNameWithoutExtension (assembly));
			case ApplePlatform.TVOS:
			case ApplePlatform.WatchOS:
				return false;
			default:
				throw ErrorHelper.CreateError (71, "Unknown platform: {0}. This usually indicates a bug in Xamarin.iOS; please file a bug report at http://bugzilla.xamarin.com with a test case.", Platform);
			}
		}

		public string MonoGCParams {
			get {
				// Configure sgen to use a small nursery
				string ret = "nursery-size=512k";
				if (IsTodayExtension || Platform == ApplePlatform.WatchOS) {
					// A bit test shows different behavior
					// Sometimes apps are killed with ~100mb allocated,
					// but I've seen apps allocate up to 240+mb as well
					ret += ",soft-heap-limit=8m";
				}
				if (EnableSGenConc)
					ret += ",major=marksweep-conc";
				else
					ret += ",major=marksweep";
				return ret;
			}
		}

		public bool IsDeviceBuild { 
			get { return BuildTarget == BuildTarget.Device; } 
		}

		public bool IsSimulatorBuild { 
			get { return BuildTarget == BuildTarget.Simulator; } 
		}

		public IEnumerable<Abi> Abis {
			get { return abis; }
		}

		public BitCodeMode BitCodeMode { get; set; }

		public bool EnableAsmOnlyBitCode { get { return BitCodeMode == BitCodeMode.ASMOnly; } }
		public bool EnableLLVMOnlyBitCode { get { return BitCodeMode == BitCodeMode.LLVMOnly; } }
		public bool EnableMarkerOnlyBitCode { get { return BitCodeMode == BitCodeMode.MarkerOnly; } }
		public bool EnableBitCode { get { return BitCodeMode != BitCodeMode.None; } }

		public ICollection<Abi> AllArchitectures {
			get {
				if (all_architectures == null) {
					all_architectures = new HashSet<Abi> ();
					foreach (var abi in abis)
						all_architectures.Add (abi & Abi.ArchMask);
					foreach (var ext in AppExtensions) {
						foreach (var abi in ext.Abis)
							all_architectures.Add (abi & Abi.ArchMask);
					}
				}
				return all_architectures;
			}
		}

		public bool IsTodayExtension {
			get {
				return ExtensionIdentifier == "com.apple.widget-extension";
			}
		}

		public bool IsWatchExtension {
			get {
				return ExtensionIdentifier == "com.apple.watchkit";
			}
		}

		public bool IsTVExtension {
			get {
				return ExtensionIdentifier == "com.apple.tv-services";
			}
		}

		public string ExtensionIdentifier {
			get {
				if (!IsExtension)
					return null;

				var info_plist = Path.Combine (AppDirectory, "Info.plist");
				var plist = Driver.FromPList (info_plist);
				var dict = plist.Get<PDictionary> ("NSExtension");
				if (dict == null)
					return null;
				return dict.GetString ("NSExtensionPointIdentifier");
			}
		}

		public string BundleId {
			get {
				return GetStringFromInfoPList ("CFBundleIdentifier");
			}
		}

		string GetStringFromInfoPList (string key)
		{
			return GetStringFromInfoPList (AppDirectory, key);
		}

		string GetStringFromInfoPList (string directory, string key)
		{
			var info_plist = Path.Combine (directory, "Info.plist");
			if (!File.Exists (info_plist))
				return null;

			var plist = Driver.FromPList (info_plist);
			if (!plist.ContainsKey (key))
				return null;
			return plist.GetString (key);
		}

		public void SetDefaultAbi ()
		{
			if (abis == null)
				abis = new List<Abi> ();
			
			switch (Platform) {
			case ApplePlatform.iOS:
				if (abis.Count == 0) {
					abis.Add (IsDeviceBuild ? Abi.ARMv7 : Abi.i386);
				}
				break;
			case ApplePlatform.WatchOS:
				if (abis.Count == 0)
					throw ErrorHelper.CreateError (76, "No architecture specified (using the --abi argument). An architecture is required for {0} projects.", "Xamarin.WatchOS");
				break;
			case ApplePlatform.TVOS:
				if (abis.Count == 0)
					throw ErrorHelper.CreateError (76, "No architecture specified (using the --abi argument). An architecture is required for {0} projects.", "Xamarin.TVOS");
				break;
			default:
				throw ErrorHelper.CreateError (71, "Unknown platform: {0}. This usually indicates a bug in Xamarin.iOS; please file a bug report at http://bugzilla.xamarin.com with a test case.", Platform);
			}
		}

		public void ValidateAbi ()
		{
			var validAbis = new List<Abi> ();
			switch (Platform) {
			case ApplePlatform.iOS:
				if (IsDeviceBuild) {
					validAbis.Add (Abi.ARMv7);
					validAbis.Add (Abi.ARMv7 | Abi.Thumb);
					validAbis.Add (Abi.ARMv7 | Abi.LLVM);
					validAbis.Add (Abi.ARMv7 | Abi.LLVM | Abi.Thumb);
					validAbis.Add (Abi.ARMv7s);
					validAbis.Add (Abi.ARMv7s | Abi.Thumb);
					validAbis.Add (Abi.ARMv7s | Abi.LLVM);
					validAbis.Add (Abi.ARMv7s | Abi.LLVM | Abi.Thumb);
				} else {
					validAbis.Add (Abi.i386);
				}
				if (IsDeviceBuild) {
					validAbis.Add (Abi.ARM64);
					validAbis.Add (Abi.ARM64 | Abi.LLVM);
				} else {
					validAbis.Add (Abi.x86_64);
				}
				break;
			case ApplePlatform.WatchOS:
				if (IsDeviceBuild) {
					validAbis.Add (Abi.ARMv7k);
					validAbis.Add (Abi.ARMv7k | Abi.LLVM);
				} else {
					validAbis.Add (Abi.i386);
				}
				break;
			case ApplePlatform.TVOS:
				if (IsDeviceBuild) {
					validAbis.Add (Abi.ARM64);
					validAbis.Add (Abi.ARM64 | Abi.LLVM);
				} else {
					validAbis.Add (Abi.x86_64);
				}
				break;
			default:
				throw ErrorHelper.CreateError (71, "Unknown platform: {0}. This usually indicates a bug in Xamarin.iOS; please file a bug report at http://bugzilla.xamarin.com with a test case.", Platform);
			}

			foreach (var abi in abis) {
				if (!validAbis.Contains (abi))
					throw ErrorHelper.CreateError (75, "Invalid architecture '{0}' for {1} projects. Valid architectures are: {2}", abi, Platform, string.Join (", ", validAbis.Select ((v) => v.AsString ()).ToArray ()));
			}
		}

		public void ClearAbi ()
		{
			abis = null;
		}

		// This is to load the symbols for all assemblies, so that we can give better error messages
		// (with file name / line number information).
		public void LoadSymbols ()
		{
			foreach (var t in Targets)
				t.LoadSymbols ();
		}

		public void ParseAbi (string abi)
		{
			var res = new List<Abi> ();
			foreach (var str in abi.Split (new char [] { ',' }, StringSplitOptions.RemoveEmptyEntries)) {
				Abi value;
				switch (str) {
				case "i386":
					value = Abi.i386;
					break;
				case "x86_64":
					value = Abi.x86_64;
					break;
				case "armv7":
					value = Abi.ARMv7;
					break;
				case "armv7+llvm":
					value = Abi.ARMv7 | Abi.LLVM;
					break;
				case "armv7+llvm+thumb2":
					value = Abi.ARMv7 | Abi.LLVM | Abi.Thumb;
					break;
				case "armv7s":
					value = Abi.ARMv7s;
					break;
				case "armv7s+llvm":
					value = Abi.ARMv7s | Abi.LLVM;
					break;
				case "armv7s+llvm+thumb2":
					value = Abi.ARMv7s | Abi.LLVM | Abi.Thumb;
					break;
				case "arm64":
					value = Abi.ARM64;
					break;
				case "arm64+llvm":
					value = Abi.ARM64 | Abi.LLVM;
					break;
				case "armv7k":
					value = Abi.ARMv7k;
					break;
				case "armv7k+llvm":
					value = Abi.ARMv7k | Abi.LLVM;
					break;
				default:
					throw new MonoTouchException (15, true, "Invalid ABI: {0}. Supported ABIs are: i386, x86_64, armv7, armv7+llvm, armv7+llvm+thumb2, armv7s, armv7s+llvm, armv7s+llvm+thumb2, armv7k, armv7k+llvm, arm64 and arm64+llvm.", str);
				}

				// merge this value with any existing ARMv? already specified.
				// this is so that things like '--armv7 --thumb' work correctly.
				if (abis != null) {
					for (int i = 0; i < abis.Count; i++) {
						if ((abis [i] & Abi.ArchMask) == (value & Abi.ArchMask)) {
							value |= abis [i];
							break;
						}
					}
				}

				res.Add (value);
			}

			// We replace any existing abis, to keep the old behavior where '--armv6 --armv7' would 
			// enable only the last abi specified and disable the rest.
			abis = res;
		}

		public static string GetArchitectures (IEnumerable<Abi> abis)
		{
			var res = new List<string> ();

			foreach (var abi in abis)
				res.Add (abi.AsArchString ());

			return string.Join (", ", res.ToArray ());
		}

		public bool IsArchEnabled (Abi arch)
		{
			return IsArchEnabled (abis, arch);
		}

		public static bool IsArchEnabled (IEnumerable<Abi> abis, Abi arch)
		{
			foreach (var abi in abis) {
				if ((abi & arch) != 0)
					return true;
			}
			return false;
		}

		public void Build ()
		{
			if (Driver.Force) {
				Driver.Log (3, "A full rebuild has been forced by the command line argument -f.");
				Cache.Clean ();
			} else {
				// this will destroy the cache if invalid, which makes setting Driver.Force to true mostly unneeded
				// in fact setting it means some actions (like extract native resource) gets duplicate for fat builds
				Cache.VerifyCache ();
			}

			Initialize ();
			ValidateAbi ();
			SelectRegistrar ();
			ExtractNativeLinkInfo ();
			SelectNativeCompiler ();
			BuildApp ();
			WriteNotice ();
			BuildFatSharedLibraries ();
			CopyAotData ();
			BuildFinalExecutable ();
			BuildDsymDirectory ();
			BuildMSymDirectory ();
			StripNativeCode ();
			StripManagedCode ();
			GenerateRuntimeOptions ();

			if (Cache.IsCacheTemporary) {
				// If we used a temporary directory we created ourselves for the cache
				// (in which case it's more a temporary location where we store the 
				// temporary build products than a cache), it will not be used again,
				// so just delete it.
				try {
					Directory.Delete (Cache.Location, true);
				} catch {
					// Don't care.
				}
			} else {
				// Write the cache data as the last step, so there is no half-done/incomplete (but yet detected as valid) cache.
				Cache.ValidateCache ();
			}

			Console.WriteLine ("{0} built successfully.", AppDirectory);
		}

		bool no_framework;
		public void SetDefaultFramework ()
		{
			// If no target framework was specified, check if we're referencing Xamarin.iOS.dll.
			// It's an error if neither target framework nor Xamarin.iOS.dll is not specified
			if (!Driver.HasTargetFramework) {
				foreach (var reference in References) {
					var name = Path.GetFileName (reference);
					switch (name) {
					case "Xamarin.iOS.dll":
						Driver.TargetFramework = TargetFramework.Xamarin_iOS_1_0;
						break;
					case "Xamarin.TVOS.dll":
					case "Xamarin.WatchOS.dll":
						throw ErrorHelper.CreateError (86, "A target framework (--target-framework) must be specified when building for TVOS or WatchOS.");
					}

					if (Driver.HasTargetFramework)
						break;
				}
			}

			if (!Driver.HasTargetFramework) {
				// Set a default target framework to show errors in the least confusing order.
				Driver.TargetFramework = TargetFramework.Xamarin_iOS_1_0;
				no_framework = true;
			}
		}

		void Initialize ()
		{
			if (EnableDebug && IsLLVM)
				ErrorHelper.Warning (3003, "Debugging is not supported when building with LLVM. Debugging has been disabled.");

			if (!IsLLVM && (EnableAsmOnlyBitCode || EnableLLVMOnlyBitCode))
				throw ErrorHelper.CreateError (3008, "Bitcode support requires the use of LLVM (--abi=arm64+llvm etc.)");

			if (EnableDebug) {
				if (!DebugTrack.HasValue) {
					DebugTrack = IsSimulatorBuild;
				}
			} else {
				if (DebugTrack.HasValue) {
					ErrorHelper.Warning (32, "The option '--debugtrack' is ignored unless '--debug' is also specified.");
				}
				DebugTrack = false;
			}

			if (EnableAsmOnlyBitCode)
				LLVMAsmWriter = true;

			if (!File.Exists (RootAssembly))
				throw new MonoTouchException (7, true, "The root assembly '{0}' does not exist", RootAssembly);
			
			if (no_framework)
				throw ErrorHelper.CreateError (96, "No reference to Xamarin.iOS.dll was found.");

			// Add a reference to the platform assembly if none has been added, and check that we're not referencing
			// any platform assemblies from another platform.
			var platformAssemblyReference = false;
			foreach (var reference in References) {
				var name = Path.GetFileNameWithoutExtension (reference);
				if (name == Driver.GetProductAssembly (this)) {
					platformAssemblyReference = true;
				} else {
					switch (name) {
					case "Xamarin.iOS":
					case "Xamarin.TVOS":
					case "Xamarin.WatchOS":
						throw ErrorHelper.CreateError (41, "Cannot reference '{0}' in a {1} app.", Path.GetFileName (reference), Driver.TargetFramework.Identifier);
					}
				}
			}
			if (!platformAssemblyReference) {
				ErrorHelper.Warning (85, "No reference to '{0}' was found. It will be added automatically.", Driver.GetProductAssembly (this) + ".dll");
				References.Add (Path.Combine (Driver.GetPlatformFrameworkDirectory (this), Driver.GetProductAssembly (this) + ".dll"));
			}

			var FrameworkDirectory = Driver.GetPlatformFrameworkDirectory (this);
			var RootDirectory = Path.GetDirectoryName (Path.GetFullPath (RootAssembly));

			((MonoTouchProfile) Profile.Current).SetProductAssembly (Driver.GetProductAssembly (this));

			string root_wo_ext = Path.GetFileNameWithoutExtension (RootAssembly);
			if (Profile.IsSdkAssembly (root_wo_ext) || Profile.IsProductAssembly (root_wo_ext))
				throw new MonoTouchException (3, true, "Application name '{0}.exe' conflicts with an SDK or product assembly (.dll) name.", root_wo_ext);

			if (IsDualBuild) {
				var target32 = new Target (this);
				var target64 = new Target (this);

				target32.ArchDirectory = Path.Combine (Cache.Location, "32");
				target32.TargetDirectory = IsSimulatorBuild ? Path.Combine (AppDirectory, ".monotouch-32") : Path.Combine (target32.ArchDirectory, "Output");
				target32.AppTargetDirectory = Path.Combine (AppDirectory, ".monotouch-32");
				target32.Resolver.ArchDirectory = Driver.GetArch32Directory (this);
				target32.Abis = SelectAbis (abis, Abi.Arch32Mask);

				target64.ArchDirectory = Path.Combine (Cache.Location, "64");
				target64.TargetDirectory = IsSimulatorBuild ? Path.Combine (AppDirectory, ".monotouch-64") : Path.Combine (target64.ArchDirectory, "Output");
				target64.AppTargetDirectory = Path.Combine (AppDirectory, ".monotouch-64");
				target64.Resolver.ArchDirectory = Driver.GetArch64Directory (this);
				target64.Abis = SelectAbis (abis, Abi.Arch64Mask);

				Targets.Add (target64);
				Targets.Add (target32);
			} else {
				var target = new Target (this);

				target.TargetDirectory = AppDirectory;
				target.AppTargetDirectory = IsSimulatorBuild ? AppDirectory : Path.Combine (AppDirectory, Is64Build ? ".monotouch-64" : ".monotouch-32");
				target.ArchDirectory = Cache.Location;
				target.Resolver.ArchDirectory = Path.Combine (FrameworkDirectory, "..", "..", Is32Build ? "32bits" : "64bits");
				target.Abis = abis;

				Targets.Add (target);

				// Make sure there aren't any lingering .monotouch-* directories.
				if (IsSimulatorBuild) {
					var dir = Path.Combine (AppDirectory, ".monotouch-32");
					if (Directory.Exists (dir))
						Directory.Delete (dir, true);
					dir = Path.Combine (AppDirectory, ".monotouch-64");
					if (Directory.Exists (dir))
						Directory.Delete (dir, true);
				}
			}

			foreach (var target in Targets) {
				target.Resolver.FrameworkDirectory = FrameworkDirectory;
				target.Resolver.RootDirectory = RootDirectory;
				target.Resolver.EnableRepl = EnableRepl;
				target.ManifestResolver.EnableRepl = EnableRepl;
				target.ManifestResolver.FrameworkDirectory = target.Resolver.FrameworkDirectory;
				target.ManifestResolver.RootDirectory = target.Resolver.RootDirectory;
				target.ManifestResolver.ArchDirectory = target.Resolver.ArchDirectory;
				target.Initialize (target == Targets [0]);

				if (!Directory.Exists (target.TargetDirectory))
					Directory.CreateDirectory (target.TargetDirectory);
			}

			if (string.IsNullOrEmpty (ExecutableName)) {
				var bundleExecutable = GetStringFromInfoPList ("CFBundleExecutable");
				ExecutableName = bundleExecutable ?? Path.GetFileNameWithoutExtension (RootAssembly);
			}

			if (ExecutableName != Path.GetFileNameWithoutExtension (AppDirectory))
				ErrorHelper.Warning (30, "The executable name ({0}) and the app name ({1}) are different, this may prevent crash logs from getting symbolicated properly.",
					ExecutableName, Path.GetFileName (AppDirectory));
			
			if (IsExtension && Platform == ApplePlatform.iOS && SdkVersion < new Version (8, 0))
				throw new MonoTouchException (45, true, "--extension is only supported when using the iOS 8.0 (or later) SDK.");

			if (IsExtension && Platform != ApplePlatform.iOS && Platform != ApplePlatform.WatchOS && Platform != ApplePlatform.TVOS)
				throw new MonoTouchException (72, true, "Extensions are not supported for the platform '{0}'.", Platform);

			if (!IsExtension && Platform == ApplePlatform.WatchOS)
				throw new MonoTouchException (77, true, "WatchOS projects must be extensions.");
		
#if ENABLE_BITCODE_ON_IOS
			if (Platform == ApplePlatform.iOS)
				DeploymentTarget = new Version (9, 0);
#endif

			if (DeploymentTarget == null) {
				DeploymentTarget = Xamarin.SdkVersions.GetVersion (Platform);
			} else if (DeploymentTarget < Xamarin.SdkVersions.GetMinVersion (Platform)) {
				throw new MonoTouchException (73, true, "Xamarin.iOS {0} does not support a deployment target of {1} for {3} (the minimum is {2}). Please select a newer deployment target in your project's Info.plist.", Constants.Version, DeploymentTarget, Xamarin.SdkVersions.GetMinVersion (Platform), PlatformName);
			} else if (DeploymentTarget > Xamarin.SdkVersions.GetVersion (Platform)) {
				throw new MonoTouchException (74, true, "Xamarin.iOS {0} does not support a deployment target of {1} for {3} (the maximum is {2}). Please select an older deployment target in your project's Info.plist or upgrade to a newer version of Xamarin.iOS.", Constants.Version, DeploymentTarget, Xamarin.SdkVersions.GetVersion (Platform), PlatformName);
			}

			if (Platform == ApplePlatform.iOS && FastDev && DeploymentTarget.Major < 8) {
				ErrorHelper.Warning (78, "Incremental builds are enabled with a deployment target < 8.0 (currently {0}). This is not supported (the resulting application will not launch on iOS 9), so the deployment target will be set to 8.0.", DeploymentTarget);
				DeploymentTarget = new Version (8, 0);
			}

			if (!package_mdb.HasValue) {
				package_mdb = EnableDebug;
			} else if (package_mdb.Value && IsLLVM) {
				ErrorHelper.Warning (3007, "Debug info files (*.mdb) will not be loaded when llvm is enabled.");
			}

			if (!enable_msym.HasValue)
				enable_msym = !EnableDebug && IsDeviceBuild;

			if (!UseMonoFramework.HasValue && DeploymentTarget >= new Version (8, 0)) {
				if (IsExtension) {
					UseMonoFramework = true;
					Driver.Log (2, "Automatically linking with Mono.framework because this is an extension");
				} else if (Extensions.Count > 0) {
					UseMonoFramework = true;
					Driver.Log (2, "Automatically linking with Mono.framework because this is an app with extensions");
				}
			}

			if (!UseMonoFramework.HasValue)
				UseMonoFramework = false;
			
			if (UseMonoFramework.Value)
				Frameworks.Add (Path.Combine (Driver.GetProductFrameworksDirectory (this), "Mono.framework"));

			if (!PackageMonoFramework.HasValue) {
				if (!IsExtension && Extensions.Count > 0 && !UseMonoFramework.Value) {
					// The main app must package the Mono framework if we have extensions, even if it's not linking with
					// it. This happens when deployment target < 8.0 for the main app.
					PackageMonoFramework = true;
				} else {
					// Package if we're not an extension and we're using the mono framework.
					PackageMonoFramework = UseMonoFramework.Value && !IsExtension;
				}
			}

			if (Frameworks.Count > 0) {
				switch (Platform) {
				case ApplePlatform.iOS:
					if (DeploymentTarget < new Version (8, 0))
						throw ErrorHelper.CreateError (65, "Xamarin.iOS only supports embedded frameworks when deployment target is at least 8.0 (current deployment target: '{0}'; embedded frameworks: '{1}')", DeploymentTarget, string.Join (", ", Frameworks.ToArray ()));
					break;
				case ApplePlatform.WatchOS:
					if (DeploymentTarget < new Version (2, 0))
						throw ErrorHelper.CreateError (65, "Xamarin.iOS only supports embedded frameworks when deployment target is at least 2.0 (current deployment target: '{0}'; embedded frameworks: '{1}')", DeploymentTarget, string.Join (", ", Frameworks.ToArray ()));
					break;
				case ApplePlatform.TVOS:
					// All versions of tvOS support extensions
					break;
				default:
					throw ErrorHelper.CreateError (71, "Unknown platform: {0}. This usually indicates a bug in Xamarin.iOS; please file a bug report at http://bugzilla.xamarin.com with a test case.", Platform);
				}
			}

			if (IsDeviceBuild) {
				switch (BitCodeMode) {
				case BitCodeMode.ASMOnly:
					if (Platform == ApplePlatform.WatchOS)
						throw ErrorHelper.CreateError (83, "asm-only bitcode is not supported on watchOS. Use either --bitcode:marker or --bitcode:full.");
					break;
				case BitCodeMode.LLVMOnly:
				case BitCodeMode.MarkerOnly:
					break;
				case BitCodeMode.None:
					// If neither llvmonly nor asmonly is enabled, enable markeronly.
					if (Platform == ApplePlatform.TVOS || Platform == ApplePlatform.WatchOS)
						BitCodeMode = BitCodeMode.MarkerOnly;
					break;
				}
			}

			if (EnableBitCode && IsSimulatorBuild)
				throw ErrorHelper.CreateError (84, "Bitcode is not supported in the simulator. Do not pass --bitcode when building for the simulator.");

			if (LinkMode == LinkMode.None && SdkVersion < SdkVersions.GetVersion (Platform))
				throw ErrorHelper.CreateError (91, "This version of Xamarin.iOS requires the {0} {1} SDK (shipped with Xcode {2}) when the managed linker is disabled. Either upgrade Xcode, or enable the managed linker by changing the Linker behaviour to Link Framework SDKs Only.", PlatformName, SdkVersions.GetVersion (Platform), SdkVersions.Xcode);

			Namespaces.Initialize ();

			InitializeCommon ();

			Driver.Watch ("Resolve References", 1);
		}
		
		void SelectRegistrar ()
		{
			// If the default values are changed, remember to update CanWeSymlinkTheApplication
			// and main.m (default value for xamarin_use_old_dynamic_registrar must match).
			if (Registrar == RegistrarMode.Default) {
				if (IsDeviceBuild) {
					Registrar = RegistrarMode.Static;
				} else { /* if (app.IsSimulatorBuild) */
					Registrar = RegistrarMode.Dynamic;
				}
			}

			foreach (var target in Targets)
				target.SelectStaticRegistrar ();
		}

		// Select all abi from the list matching the specified mask.
		List<Abi> SelectAbis (IEnumerable<Abi> abis, Abi mask)
		{
			var rv = new List<Abi> ();
			foreach (var abi in abis) {
				if ((abi & mask) != 0)
					rv.Add (abi);
			}
			return rv;
		}

		public string AssemblyName {
			get {
				return Path.GetFileName (RootAssembly);
			}
		}

		public string Executable {
			get {
				return Path.Combine (AppDirectory, ExecutableName);
			}
		}

		void ManagedLink ()
		{
			foreach (var target in Targets)
				target.ManagedLink ();
		}

		void BuildApp ()
		{
			foreach (var target in Targets)	{
				if (target.CanWeSymlinkTheApplication ()) {
					target.Symlink ();
				} else {
					target.ProcessAssemblies ();
				}
			}

			// Deduplicate files from the Build directory. We need to do this before the AOT
			// step, so that we can ignore timestamp/GUID in assemblies (the GUID is
			// burned into the AOT assembly, so after that we'll need the original assembly.
			if (IsDualBuild && IsDeviceBuild) {
				// All the assemblies are now in BuildDirectory.
				var t1 = Targets [0];
				var t2 = Targets [1];

				foreach (var f1 in Directory.GetFileSystemEntries (t1.BuildDirectory)) {
					var f2 = Path.Combine (t2.BuildDirectory, Path.GetFileName (f1));
					if (!File.Exists (f2))
						continue;
					var ext = Path.GetExtension (f1).ToUpperInvariant ();
					var is_assembly = ext == ".EXE" || ext == ".DLL";
					if (!is_assembly)
						continue;

					if (!Cache.CompareAssemblies (f1, f2, true))
						continue;
						
					if (Driver.Verbosity > 0)
						Console.WriteLine ("Targets {0} and {1} found to be identical", f1, f2);
					// Don't use symlinks, since it just gets more complicated
					// For instance: on rebuild, when should the symlink be updated and when
					// should the target of the symlink be updated? And all the usages
					// must be audited to ensure the right thing is done...
					Driver.CopyAssembly (f1, f2);
				}
			}

			foreach (var target in Targets) {
				if (target.CanWeSymlinkTheApplication ())
					continue;

				target.ComputeLinkerFlags ();
				target.Compile ();
				target.NativeLink ();
			}
		}

		void WriteNotice ()
		{
			if (!IsDeviceBuild)
				return;

			if (Directory.Exists (Path.Combine (AppDirectory, "NOTICE")))
				throw new MonoTouchException (1016, true, "Failed to create the NOTICE file because a directory already exists with the same name.");

			try {
				// write license information inside the .app
				StringBuilder sb = new StringBuilder ();
				sb.Append ("Xamarin built applications contain open source software.  ");
				sb.Append ("For detailed attribution and licensing notices, please visit...");
				sb.AppendLine ().AppendLine ().Append ("http://xamarin.com/mobile-licensing").AppendLine ();
				Driver.WriteIfDifferent (Path.Combine (AppDirectory, "NOTICE"), sb.ToString ());
			} catch (Exception ex) {
				throw new MonoTouchException (1017, true, ex, "Failed to create the NOTICE file: {0}", ex.Message);
			}
		}

		void BuildFatSharedLibraries ()
		{
			// Create shared fat libraries.
			if (!FastDev)
				return;

			var hash = new Dictionary<string, List<string>> ();
			foreach (var target in Targets) {
				foreach (var a in target.Assemblies) {
					List<string> dylibs;

					if (a.Dylibs == null || a.Dylibs.Count () == 0)
						continue;

					if (!hash.TryGetValue (a.Dylib, out dylibs)) {
						dylibs = new List<string> ();
						hash [a.Dylib] = dylibs;
					}
					dylibs.AddRange (a.Dylibs);

					target.LinkWith (a.Dylib);
				}

				foreach (var dylib in target.LibrariesToShip) {
					List<string> dylibs;
					var targetName = Path.GetFileNameWithoutExtension (Path.GetFileNameWithoutExtension (dylib)) + Path.GetExtension (dylib);
					var targetPath = Path.Combine (AppDirectory, targetName);
					if (!hash.TryGetValue (targetPath, out dylibs))
						hash [targetPath] = dylibs = new List<string> ();
					dylibs.Add (dylib);
				}
			}

			foreach (var kvp in hash) {
				var dylib = kvp.Key;
				var dylibs = kvp.Value;
				if (!Application.IsUptodate (dylibs, new string [] { dylib })) {
					var cmd = new StringBuilder ();
					foreach (var lib in dylibs) {
						cmd.Append (Driver.Quote (lib));
						cmd.Append (' ');
					}
					cmd.Append ("-create -output ");
					cmd.Append (Driver.Quote (dylib));
					Driver.RunLipo (cmd.ToString ());
				} else {
					Driver.Log (3, "Target '{0}' is up-to-date.", dylib);
				}
			}
		}

		void CopyAotData ()
		{
			if (!IsDeviceBuild)
				return;

			foreach (var target in Targets) {
				foreach (var a in target.Assemblies) {
					foreach (var data in a.AotDataFiles) {
						Application.UpdateFile (data, Path.Combine (target.AppTargetDirectory, Path.GetFileName (data)));
					}
				}
			}
		}

		public static void CopyMSymData (string src, string dest)
		{
			if (string.IsNullOrEmpty (src) || string.IsNullOrEmpty (dest))
				return;
			if (!Directory.Exists (src)) // got no aot data
				return;

			var p = new Process ();
			p.StartInfo.UseShellExecute = false;
			p.StartInfo.RedirectStandardError = true;
			p.StartInfo.FileName = "mono-symbolicate";
			p.StartInfo.Arguments = $"store-symbols \"{src}\" \"{dest}\"";

			try {
				if (p.Start ()) {
					var error = p.StandardError.ReadToEnd();
					p.WaitForExit ();
					GC.Collect (); // Workaround for: https://bugzilla.xamarin.com/show_bug.cgi?id=43462#c14
					if (p.ExitCode == 0)
						return;
					else {
						ErrorHelper.Warning (95, $"Aot files could not be copied to the destination directory {dest}: {error}"); 
						return;
					}
				}

				ErrorHelper.Warning (95, $"Aot files could not be copied to the destination directory {dest}: Could not start process."); 
				return;
			}
			catch (Exception e) {
				ErrorHelper.Warning (95, e, $"Aot files could not be copied to the destination directory {dest}: Could not start process."); 
				return;
			}
		}

		void BuildFinalExecutable ()
		{
			if (FastDev) {
				var libdir = Path.Combine (Driver.GetProductSdkDirectory (this), "usr", "lib");
				var libmono_name = LibMono;
				if (!UseMonoFramework.Value) {
					var libmono_target = Path.Combine (AppDirectory, libmono_name);
					var libmono_source = Path.Combine (libdir, libmono_name);
					Application.UpdateFile (libmono_source, libmono_target);
				}

				var libprofiler_target = Path.Combine (AppDirectory, "libmono-profiler-log.dylib");
				var libprofiler_source = Path.Combine (libdir, "libmono-profiler-log.dylib");
				if (EnableProfiling)
					Application.UpdateFile (libprofiler_source, libprofiler_target);

				// Copy libXamarin.dylib to the app
				var libxamarin_target = Path.Combine (AppDirectory, LibXamarin);
				Application.UpdateFile (Path.Combine (Driver.GetMonoTouchLibDirectory (this), LibXamarin), libxamarin_target);

				if (UseMonoFramework.Value) {
					if (EnableProfiling)
						Driver.XcodeRun ("install_name_tool", "-change @rpath/libmonosgen-2.0.dylib @rpath/Mono.framework/Mono " + Driver.Quote (libprofiler_target));
					Driver.XcodeRun ("install_name_tool", "-change @rpath/libmonosgen-2.0.dylib @rpath/Mono.framework/Mono " + Driver.Quote (libxamarin_target));
				}
			}

			// Copy frameworks to the app bundle.
			if (!IsExtension || IsWatchExtension) {
				var all_frameworks = new HashSet<string> ();
				all_frameworks.UnionWith (Frameworks);
				all_frameworks.UnionWith (WeakFrameworks);
				foreach (var t in Targets) {
					all_frameworks.UnionWith (t.Frameworks);
					all_frameworks.UnionWith (t.WeakFrameworks);
					foreach (var a in t.Assemblies) {
						if (a.Frameworks != null)
							all_frameworks.UnionWith (a.Frameworks);
						if (a.WeakFrameworks != null)
							all_frameworks.UnionWith (a.WeakFrameworks);
					}
				}
					
				if (PackageMonoFramework.Value) {
					// We may have to copy the Mono framework to the bundle even if we're not linking with it.
					all_frameworks.Add (Path.Combine (Driver.GetProductSdkDirectory (this), "Frameworks", "Mono.framework"));
				}
				
				foreach (var appex in Extensions) {
					var f_path = Path.Combine (appex, "..", "frameworks.txt");
					if (!File.Exists (f_path))
						continue;

					foreach (var fw in File.ReadAllLines (f_path)) {
						Driver.Log (3, "Copying {0} to the app's Frameworks directory because it's used by the extension {1}", fw, Path.GetFileName (appex));
						all_frameworks.Add (fw);
					}
				}

				foreach (var fw in all_frameworks) {
					if (!fw.EndsWith (".framework", StringComparison.Ordinal))
						continue;
					if (!Xamarin.MachO.IsDynamicFramework (Path.Combine (fw, Path.GetFileNameWithoutExtension (fw)))) {
						// We can have static libraries camouflaged as frameworks. We don't want those copied to the app.
						Driver.Log (1, "The framework {0} is a framework of static libraries, and will not be copied into the app.", fw);
						continue;
					}

					if (!File.Exists (Path.Combine (fw, "Info.plist")))
						throw ErrorHelper.CreateError (1304, "The embedded framework '{0}' in {1} is invalid: it does not contain an Info.plist.", Path.GetFileNameWithoutExtension (fw), fw);
					
					Application.UpdateDirectory (fw, Path.Combine (AppDirectory, "Frameworks"));
					if (IsDeviceBuild) {
						// Remove architectures we don't care about.
						Xamarin.MachO.SelectArchitectures (Path.Combine (AppDirectory, "Frameworks", Path.GetFileName (fw), Path.GetFileNameWithoutExtension (fw)), AllArchitectures);
					}
				}
			} else {
				if (!IsWatchExtension) {
					// In extensions we need to save a list of the frameworks we need so that the main app can get them.
					var all_frameworks = new HashSet<string> (Frameworks);
					all_frameworks.UnionWith (WeakFrameworks);
					foreach (var t in Targets) {
						all_frameworks.UnionWith (t.Frameworks);
						all_frameworks.UnionWith (t.WeakFrameworks);
						foreach (var a in t.Assemblies) {
							if (a.Frameworks != null)
								all_frameworks.UnionWith (a.Frameworks);
							if (a.WeakFrameworks != null)
								all_frameworks.UnionWith (a.WeakFrameworks);
						}
					}
					all_frameworks.RemoveWhere ((v) => !v.EndsWith (".framework", StringComparison.Ordinal));
					if (all_frameworks.Count () > 0)
						Driver.WriteIfDifferent (Path.Combine (Path.GetDirectoryName (AppDirectory), "frameworks.txt"), string.Join ("\n", all_frameworks.ToArray ()));
				}
			}

			if (IsSimulatorBuild || !IsDualBuild) {
				if (IsDeviceBuild)
					cached_executable = Targets [0].cached_executable;
				return;
			}

			if (IsSimulatorBuild || !IsDualBuild) {
				if (IsDeviceBuild)
					cached_executable = Targets [0].cached_executable;
				return;
			}

			if (IsUptodate (new string [] { Targets [0].Executable, Targets [1].Executable }, new string [] { Executable })) {
				cached_executable = true;
				Driver.Log (3, "Target '{0}' is up-to-date.", Executable);
				return;
			}

			var cmd = new StringBuilder ();
			foreach (var target in Targets) {
				cmd.Append (Driver.Quote (target.Executable));
				cmd.Append (' ');
			}
			cmd.Append ("-create -output ");
			cmd.Append (Driver.Quote (Executable));
			Driver.RunLipo (cmd.ToString ());

		}
			
		public void ExtractNativeLinkInfo ()
		{
			var exceptions = new List<Exception> ();

			foreach (var target in Targets)
				target.ExtractNativeLinkInfo (exceptions);

			if (exceptions.Count > 0)
				throw new AggregateException (exceptions);

			Driver.Watch ("Extracted native link info", 1);
		}

		public void SelectNativeCompiler ()
		{
			foreach (var t in Targets) {
				foreach (var a in t.Assemblies) {
					if (a.EnableCxx) {	
						EnableCxx = true;
						break;
					}
				}
			}

			Driver.CalculateCompilerPath (this);
		}

		public string LibMono {
			get {
				if (FastDev) {
					return "libmonosgen-2.0.dylib";
				} else {
					return "libmonosgen-2.0.a";
				}
			}
		}

		public string LibXamarin {
			get {
				if (FastDev) {
					return EnableDebug ? "libxamarin-debug.dylib" : "libxamarin.dylib";
				} else {
					return EnableDebug ? "libxamarin-debug.a" : "libxamarin.a";
				}
			}
		}

		public void NativeLink ()
		{
			foreach (var target in Targets)
				target.NativeLink ();
		}
		
		// this will filter/remove warnings that are not helpful (e.g. complaining about non-matching armv6-6 then armv7-6 on fat binaries)
		// and turn the remaining of the warnings into MT5203 that MonoDevelop will be able to report as real warnings (not just logs)
		// it will also look for symbol-not-found errors and try to provide useful error messages.
		public static void ProcessNativeLinkerOutput (Target target, string output, IList<string> inputs, List<Exception> errors, bool error)
		{
			List<string> lines = new List<string> (output.Split (new string[] { Environment.NewLine }, StringSplitOptions.RemoveEmptyEntries));

			// filter
			for (int i = 0; i < lines.Count; i++) {
				string line = lines [i];

				if (errors.Count > 100)
					return;

				if (line.Contains ("ld: warning: ignoring file ") && 
					line.Contains ("file was built for") && 
					line.Contains ("which is not the architecture being linked") &&
				// Only ignore warnings related to the object files we've built ourselves (assemblies, main.m, registrar.m)
					inputs.Any ((v) => line.Contains (v))) {
					continue;
				} else if (line.Contains ("ld: symbol(s) not found for architecture") && errors.Count > 0) {
					continue;
				} else if (line.Contains ("clang: error: linker command failed with exit code 1")) {
					continue;
				} else if (line.Contains ("was built for newer iOS version (5.1.1) than being linked (5.1)")) {
					continue;
				}

				if (line.Contains ("Undefined symbols for architecture")) {
					while (++i < lines.Count) {
						line = lines [i];
						if (!line.EndsWith (", referenced from:", StringComparison.Ordinal))
							break;

						var symbol = line.Replace (", referenced from:", "").Trim ('\"', ' ');
						if (symbol.StartsWith ("_OBJC_CLASS_$_", StringComparison.Ordinal)) {
							errors.Add (new MonoTouchException (5211, error, 
																"Native linking failed, undefined Objective-C class: {0}. The symbol '{1}' could not be found in any of the libraries or frameworks linked with your application.",
							                                    symbol.Replace ("_OBJC_CLASS_$_", ""), symbol));
						} else {
							var members = target.GetMembersForSymbol (symbol.Substring (1));
							if (members != null && members.Count > 0) {
								var member = members.First (); // Just report the first one.
								// Neither P/Invokes nor fields have IL, so we can't find the source code location.
								errors.Add (new MonoTouchException (5214, error,
									"Native linking failed, undefined symbol: {0}. " +
									"This symbol was referenced by the managed member {1}.{2}. " +
									"Please verify that all the necessary frameworks have been referenced and native libraries linked.",
									symbol, member.DeclaringType.FullName, member.Name));
							} else {
								errors.Add (new MonoTouchException (5210, error, 
							                                    "Native linking failed, undefined symbol: {0}. " +
																"Please verify that all the necessary frameworks have been referenced and native libraries are properly linked in.",
							                                    symbol));
							}
						}

						// skip all subsequent lines related to the same error.
						// we skip all subsequent lines with more indentation than the initial line.
						var indent = GetIndentation (line);
						while (i + 1 < lines.Count) {
							line = lines [i + 1];
							if (GetIndentation (lines [i + 1]) <= indent)
								break;
							i++;
						}
					}
				} else if (line.StartsWith ("duplicate symbol", StringComparison.Ordinal) && line.EndsWith (" in:", StringComparison.Ordinal)) {
					var symbol = line.Replace ("duplicate symbol ", "").Replace (" in:", "").Trim ();
					errors.Add (new MonoTouchException (5212, error, "Native linking failed, duplicate symbol: '{0}'.", symbol));

					var indent = GetIndentation (line);
					while (i + 1 < lines.Count) {
						line = lines [i + 1];
						if (GetIndentation (lines [i + 1]) <= indent)
							break;
						i++;
						errors.Add (new MonoTouchException (5213, error, "Duplicate symbol in: {0} (Location related to previous error)", line.Trim ()));
					}
				} else {
					if (line.StartsWith ("ld: ", StringComparison.Ordinal))
						line = line.Substring (4);

					line = line.Trim ();

					if (error) {
						errors.Add (new MonoTouchException (5209, error, "Native linking error: {0}", line));
					} else {
						errors.Add (new MonoTouchException (5203, error, "Native linking warning: {0}", line));
					}
				}
			}
		}

		static int GetIndentation (string line)
		{
			int rv = 0;
			if (line.Length == 0)
				return 0;

			while (true) {
				switch (line [rv]) {
				case ' ':
				case '\t':
					rv++;
					break;
				default:
					return rv;
				}
			};
		}

		// return the ids found in a macho file
		List<Guid> GetUuids (MachOFile file)
		{
			var result = new List<Guid> ();
			foreach (var cmd in file.load_commands) {
				if (cmd is UuidCommand) {
					var uuidCmd = cmd as UuidCommand;
					result.Add (new Guid (uuidCmd.uuid));
				}
			}
			return result;
		}

		// This method generates the manifest that is required by the symbolication in order to be able to debug the application, 
		// The following is an example of the manifest to be generated:
		// <mono-debug version=”1”>
		//	<app-id>com.foo.bar</app-id>
		//	<build-date>datetime</build-date>
		//	<build-id>build-id</build-id>
		//	<build-id>build-id</build-id>
		// </mono-debug>
		// where:
		// 
		// app-id: iOS/Android/Mac app/package ID. Currently for verification and user info only but in future may be used to find symbols automatically.
		// build-date: Local time in DateTime “O” format. For user info only.
		// build-id: The build UUID. Needed for HockeyApp to find the mSYM folder matching the app build. There may be more than one, as in the case of iOS multi-arch.
		void GenerateMSymManifest (Target target, string target_directory)
		{
			var manifestPath = Path.Combine (target_directory, "manifest.xml");
			if (String.IsNullOrEmpty (target_directory))
				throw new ArgumentNullException (nameof (target_directory));
			var root = new XElement ("mono-debug",
				new XAttribute("version", 1),
				new XElement ("app-id", BundleId),
				new XElement ("build-date", DateTime.Now.ToString ("O")));
				
			var file = MachO.Read (target.Executable);
			
			if (file is MachO) {
				var mfile = file as MachOFile;
				var uuids = GetUuids (mfile);
				foreach (var str in uuids) {
					root.Add (new XElement ("build-id", str));
				}
			} else if (file is IEnumerable<MachOFile>) {
				var ffile = file as IEnumerable<MachOFile>;
				foreach (var fentry in ffile) {
					var uuids = GetUuids (fentry);
					foreach (var str in uuids) {
						root.Add (new XElement ("build-id", str));
					}
				}
				
			} else {
				// do not write a manifest
				return;
			}

			// Write only if we need to update the manifest
			Driver.WriteIfDifferent (manifestPath, root.ToString ());
		}

		void CopyAotData (string src, string dest)
		{
			if (string.IsNullOrEmpty (src) || string.IsNullOrEmpty (dest)) {
				ErrorHelper.Warning (95, $"Aot files could not be copied to the destination directory {dest}"); 
				return;
			}
				
			var dir = new DirectoryInfo (src);
			if (!dir.Exists) {
				ErrorHelper.Warning (95, $"Aot files could not be copied to the destination directory {dest}"); 
				return;
			}

			var dirs = dir.GetDirectories ();
			if (!Directory.Exists (dest))
				Directory.CreateDirectory (dest);
				
			var files = dir.GetFiles ();
			foreach (var file in files) {
				var tmp = Path.Combine (dest, file.Name);
				file.CopyTo (tmp, true);
			}

			foreach (var subdir in dirs) {
				var tmp = Path.Combine (dest, subdir.Name);
				CopyAotData (subdir.FullName, tmp);
			}
		}

		public void BuildMSymDirectory ()
		{
			if (!EnableMSym)
				return;

			var target_directory = string.Format ("{0}.mSYM", AppDirectory);
			if (!Directory.Exists (target_directory))
				Directory.CreateDirectory (target_directory);

			foreach (var target in Targets) {
				GenerateMSymManifest (target, target_directory);
				var msymdir = Path.Combine (target.BuildDirectory, "Msym");
				// copy aot data must be done BEFORE we do copy the msym one
				CopyAotData (msymdir, target_directory);
				
				// copy all assemblies under mvid and with the dll and mdb
				var tmpdir =  Path.Combine (msymdir, "Msym", "tmp");
				if (!Directory.Exists (tmpdir))
					Directory.CreateDirectory (tmpdir);
					
				foreach (var asm in target.Assemblies) {
					asm.CopyToDirectory (tmpdir, reload: false, only_copy: true);
				}
				// mono-symbolicate knows best
				CopyMSymData (target_directory, tmpdir);
			}
		}

		public void BuildDsymDirectory ()
		{
			if (!BuildDSym.HasValue)
				BuildDSym = IsDeviceBuild;

			if (!BuildDSym.Value)
				return;

			string dsym_dir = string.Format ("{0}.dSYM", AppDirectory);
			bool cached_dsym = false;

			if (cached_executable)
				cached_dsym = IsUptodate (new string [] { Executable }, Directory.EnumerateFiles (dsym_dir, "*", SearchOption.AllDirectories));

			if (!cached_dsym) {
				if (Directory.Exists (dsym_dir))
					Directory.Delete (dsym_dir, true);
				
				Driver.CreateDsym (AppDirectory, ExecutableName, dsym_dir);
			} else {
				Driver.Log (3, "Target '{0}' is up-to-date.", dsym_dir);
			}
			Driver.Watch ("Linking DWARF symbols", 1);
		}

		IEnumerable<string> GetRequiredSymbols ()
		{
			foreach (var target in Targets) {
				foreach (var symbol in target.GetRequiredSymbols ())
					yield return symbol;
			}
		}

		bool WriteSymbolList (string filename)
		{
			var required_symbols = GetRequiredSymbols ().ToArray ();
			using (StreamWriter writer = new StreamWriter (filename)) {
				foreach (string symbol in required_symbols)
					writer.WriteLine ("_{0}", symbol);
				foreach (var symbol in NoSymbolStrip)
					writer.WriteLine ("_{0}", symbol);
				writer.Flush ();
				return writer.BaseStream.Position > 0;
			}
		}

		void StripNativeCode (string name)
		{
			if (NativeStrip && IsDeviceBuild && !EnableDebug && string.IsNullOrEmpty (SymbolList)) {
				string symbol_file = Path.Combine (Cache.Location, "symbol-file");
				if (WriteSymbolList (symbol_file)) {
					Driver.RunStrip (String.Format ("-i -s \"{0}\" \"{1}\"", symbol_file, Executable));
				} else {
					Driver.RunStrip (String.Format ("\"{0}\"", Executable));
				}
				Driver.Watch ("Native Strip", 1);
			}

			if (!string.IsNullOrEmpty (SymbolList))
				WriteSymbolList (SymbolList);
		}

		public void StripNativeCode ()
		{
			if (IsDualBuild) {
				bool cached = true;
				foreach (var target in Targets)
					cached &= target.cached_executable;
				if (!cached)
					StripNativeCode (Executable);
			} else {
				foreach (var target in Targets) {
					if (!target.cached_executable)
						StripNativeCode (target.Executable);
				}
			}
		}

		public void StripManagedCode ()
		{
			foreach (var target in Targets)
				target.StripManagedCode ();

			// deduplicate assemblies between the .monotouch-32 and .monotouch-64 directories
			if (IsDualBuild && IsDeviceBuild)
				DeduplicateDir ("..", Targets [0].AppTargetDirectory, Targets [1].AppTargetDirectory);
		}

		void DeduplicateDir (string base_dir, string d1, string d2)
		{
			foreach (var f1 in Directory.GetFileSystemEntries (d1)) {
				var f2 = Path.Combine (d2, Path.GetFileName (f1));
				if (Directory.Exists (f1))
					DeduplicateDir (Path.Combine (base_dir, ".."), f1, f2);
				else
					DeduplicateFile (base_dir, f1, f2);
			}
		}

		void DeduplicateFile (string base_dir, string f1, string f2)
		{
			if (!File.Exists (f2))
				return;

			if (Driver.IsSymlink (f2))
				return; // Already determined to be identical from a previous build.

			bool equal;
			var ext = Path.GetExtension (f1).ToUpperInvariant ();
			var is_assembly = ext == ".EXE" || ext == ".DLL";

			if (is_assembly) {
				equal = Cache.CompareAssemblies (f1, f2, true, true);
				if (!equal)
					Driver.Log (1, "Assemblies {0} and {1} not found to be identical, cannot replace one with a symlink to the other.", f1, f2);
			} else {
				equal = Cache.CompareFiles (f1, f2, true);
				if (!equal)
					Driver.Log (1, "Targets {0} and {1} not found to be identical, cannot replace one with a symlink to the other.", f1, f2);
			}
			if (!equal)
				return;

			var dest = Path.Combine (base_dir, f1.Substring (AppDirectory.Length + 1));
			Driver.FileDelete (f2);
			if (!Driver.Symlink (dest, f2)) {
				File.Copy (f1, f2);
			} else {
				Driver.Log (1, "Targets {0} and {1} found to be identical, the later has been replaced with a symlink to the former.", f1, f2);
			}
		}

		public void GenerateRuntimeOptions ()
		{
			// only if the linker is disabled
			if (LinkMode != LinkMode.None)
				return;

			RuntimeOptions.Write (AppDirectory);
		}
	}
}
