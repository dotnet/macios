using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;

using Mono.Cecil;
using Mono.Cecil.Cil;
using Mono.Linker;
using Mono.Tuner;

using Xamarin;
using Xamarin.Linker;
using Xamarin.MacDev;
using Xamarin.Utils;

using ObjCRuntime;

using Registrar;

#if !LEGACY_TOOLS
using ClassRedirector;
#endif

#if LEGACY_TOOLS
using PlatformResolver = MonoTouch.Tuner.MonoTouchResolver;
#elif NET
using PlatformResolver = Xamarin.Linker.DotNetResolver;
#else
#error Invalid defines
#endif

#nullable enable

#if LEGACY_TOOLS
namespace Mono.Linker {
	public class LinkContext {
		public IEnumerable<AssemblyDefinition> GetAssemblies ()
		{
			throw new System.NotImplementedException ();
		}
	}
}
#endif // LEGACY_TOOLS

namespace Xamarin.Bundler {

	public enum BuildTarget {
		None,
		Simulator,
		Device,
	}

	[Flags]
	public enum RegistrarOptions {
		Default = 0,
		Trace = 1,
	}

	public enum RegistrarMode {
		Default,
		Dynamic,
		PartialStatic,
		Static,
		ManagedStatic,
	}

	public partial class Application {
		public Cache? Cache;
		public string AppDirectory = ".";
		public bool DeadStrip = true;
		public bool EnableDebug;
		// The list of assemblies that we do generate debugging info for.
		public bool DebugAll;
		public bool UseInterpreter; // Only applicable to mobile platforms.
		public List<string> DebugAssemblies = new List<string> ();
		public Optimizations Optimizations = new Optimizations ();
#if LEGACY_TOOLS
		public readonly RegistrarMode Registrar = RegistrarMode.Static;
#else
		public RegistrarMode Registrar = RegistrarMode.Default;
#endif
		public RegistrarOptions RegistrarOptions = RegistrarOptions.Default;
		public SymbolMode SymbolMode;
		public HashSet<string> IgnoredSymbols = new HashSet<string> ();

		// The AOT arguments are currently not used for macOS, but they could eventually be used there as well (there's no mmp option to set these yet).
		public List<string> AotArguments = new List<string> ();
		public List<string>? AotOtherArguments = null;
		public bool? AotFloat32 = null;

#if !LEGACY_TOOLS
		public DlsymOptions DlsymOptions;
		public List<Tuple<string, bool>>? DlsymAssemblies;
#endif // !LEGACY_TOOLS
		public List<string>? CustomLinkFlags;

		public HashSet<string> Frameworks = new HashSet<string> ();
		public HashSet<string> WeakFrameworks = new HashSet<string> ();

		public bool IsExtension;
		public ApplePlatform Platform { get { return Driver.TargetFramework.Platform; } }

		public List<string> MonoLibraries = new List<string> ();
		public List<string> InterpretedAssemblies = new List<string> ();

		// Linker config
#if LEGACY_TOOLS
		public LinkMode LinkMode = LinkMode.Full;
#endif
		bool? are_any_assemblies_trimmed;
		public bool AreAnyAssembliesTrimmed {
			get {
				if (are_any_assemblies_trimmed.HasValue)
					return are_any_assemblies_trimmed.Value;
#if !LEGACY_TOOLS
				// This shouldn't happen, we should always set AreAnyAssembliesTrimmed to some value for .NET.
				throw ErrorHelper.CreateError (99, "A custom LinkMode value is not supported for .NET");
#else
				return LinkMode != LinkMode.None;
#endif
			}
			set {
				are_any_assemblies_trimmed = value;
			}
		}
		public bool EnableSGenConc;

		public Dictionary<string, (string Value, bool Overwrite)> EnvironmentVariables = new Dictionary<string, (string Value, bool Overwrite)> ();

		public MarshalObjectiveCExceptionMode MarshalObjectiveCExceptions;
		public MarshalManagedExceptionMode MarshalManagedExceptions;

		bool is_default_marshal_managed_exception_mode;
		public bool IsDefaultMarshalManagedExceptionMode {
			get { return is_default_marshal_managed_exception_mode || MarshalManagedExceptions == MarshalManagedExceptionMode.Default; }
			set { is_default_marshal_managed_exception_mode = value; }
		}
		public List<string> RootAssemblies = new List<string> ();
		public List<string> References = new List<string> ();
		public string? RegistrarOutputLibrary;

		public BuildTarget BuildTarget;

		public XamarinRuntime XamarinRuntime;
		public string? RuntimeIdentifier; // Only used for build-time --run-registrar support

		public bool SkipMarkingNSObjectsInUserAssemblies { get; set; }

		// How Mono should be embedded into the app.
		AssemblyBuildTarget? libmono_link_mode;
		public AssemblyBuildTarget LibMonoLinkMode {
			get {
				if (!libmono_link_mode.HasValue)
					throw new InvalidOperationException ("LibMonoLinkMode has not been set.");
				return libmono_link_mode.Value;
			}
			set {
				libmono_link_mode = value;
			}
		}

		// How libxamarin should be embedded into the app.
		AssemblyBuildTarget? libxamarin_link_mode;
		public AssemblyBuildTarget LibXamarinLinkMode {
			get {
				if (!libxamarin_link_mode.HasValue)
					throw new InvalidOperationException ("LibXamarinLinkMode has not been set.");
				return libxamarin_link_mode.Value;
			}
			set {
				libxamarin_link_mode = value;
			}
		}

		// How the libmononative library should be linked into the app.
		public AssemblyBuildTarget LibMonoNativeLinkMode {
			get {
				// if there's a specific way libmono is being linked, use the same way.
				if (!libmono_link_mode.HasValue)
					throw new InvalidOperationException ("LibMonoNativeLinkMode has not been set.");
				return libmono_link_mode.Value;
			}
		}

		bool RequiresXcodeHeaders {
			get {
				switch (Platform) {
				case ApplePlatform.iOS:
				case ApplePlatform.TVOS:
				case ApplePlatform.MacCatalyst:
					return !AreAnyAssembliesTrimmed;
				case ApplePlatform.MacOSX:
					return (Registrar == RegistrarMode.Static || Registrar == RegistrarMode.ManagedStatic) && !AreAnyAssembliesTrimmed;
				default:
					throw ErrorHelper.CreateError (71, Errors.MX0071, Platform, ProductName);
				}
			}
		}

		public bool IsDeviceBuild {
			get {
				if (!string.IsNullOrEmpty (RuntimeIdentifier))
					return !IsSimulatorBuild;

				switch (Platform) {
				case ApplePlatform.iOS:
				case ApplePlatform.TVOS:
					return BuildTarget == BuildTarget.Device;
				case ApplePlatform.MacOSX:
				case ApplePlatform.MacCatalyst:
					return false;
				default:
					throw ErrorHelper.CreateError (71, Errors.MX0071, Platform, ProductName);
				}
			}
		}

		public bool IsSimulatorBuild {
			get {
				if (!string.IsNullOrEmpty (RuntimeIdentifier))
					return RuntimeIdentifier.IndexOf ("simulator", StringComparison.OrdinalIgnoreCase) >= 0;

				switch (Platform) {
				case ApplePlatform.iOS:
				case ApplePlatform.TVOS:
					return BuildTarget == BuildTarget.Simulator;
				case ApplePlatform.MacOSX:
				case ApplePlatform.MacCatalyst:
					return false;
				default:
					throw ErrorHelper.CreateError (71, Errors.MX0071, Platform, ProductName);
				}
			}
		}

		public Version? DeploymentTarget;
		public Version? SdkVersion; // for Mac Catalyst this is the iOS version
		public Version? NativeSdkVersion; // this is the same as SdkVersion, except that for Mac Catalyst it's the macOS SDK version.

		Abi abi;
		public bool IsLLVM { get { return IsArchEnabled (Abi.LLVM); } }

		bool? package_managed_debug_symbols;
		public bool PackageManagedDebugSymbols {
			get {
				if (!package_managed_debug_symbols.HasValue)
					throw new InvalidOperationException ("PackageManagedDebugSymbols has not been set.");
				return package_managed_debug_symbols.Value;
			}
			set { package_managed_debug_symbols = value; }
		}

		public Version GetMacCatalystiOSVersion (Version macOSVersion)
		{
			if (!MacCatalystSupport.TryGetiOSVersion (Driver.GetFrameworkDirectory (this), macOSVersion, out var value, out var knownMacOSVersions))
				throw ErrorHelper.CreateError (184, Errors.MX0184 /* Could not map the macOS version {0} to a corresponding Mac Catalyst version. Valid macOS versions are: {1} */, macOSVersion.ToString (), string.Join (", ", knownMacOSVersions.OrderBy (v => v)));

			return value;
		}

		public Application ()
		{
			this.StaticRegistrar = new StaticRegistrar (this);
		}

		public void CreateCache (string [] arguments)
		{
			Cache = new Cache (arguments);
		}

		public bool DynamicRegistrationSupported {
			get {
				return Optimizations.RemoveDynamicRegistrar != true;
			}
		}

		public void ParseCustomLinkFlags (string value, string value_name)
		{
			if (!StringUtils.TryParseArguments (value, out var lf, out var ex))
				throw ErrorHelper.CreateError (26, ex, Errors.MX0026, $"-{value_name}={value}", ex.Message);
			if (CustomLinkFlags is null)
				CustomLinkFlags = new List<string> ();
			CustomLinkFlags.AddRange (lf);
		}

		public void ParseInterpreter (string value)
		{
			UseInterpreter = true;
			if (!string.IsNullOrEmpty (value))
				InterpretedAssemblies.AddRange (value.Split (new char [] { ',' }, StringSplitOptions.RemoveEmptyEntries));
		}

		public void UnsetInterpreter ()
		{
			UseInterpreter = false;
			InterpretedAssemblies.Clear ();
		}

		public bool IsTodayExtension {
			get {
				return ExtensionIdentifier == "com.apple.widget-extension";
			}
		}

		public bool IsTVExtension {
			get {
				return ExtensionIdentifier == "com.apple.tv-services";
			}
		}

		public string? ExtensionIdentifier {
			get {
				if (!IsExtension)
					return null;

				var plist = Driver.FromPList (InfoPListPath);
				if (plist is null)
					return null;
				var dict = plist.Get<PDictionary> ("NSExtension");
				if (dict is null)
					return null;
				return dict.GetString ("NSExtensionPointIdentifier");
			}
		}

		string? info_plistpath;
		public string InfoPListPath {
			get {
				if (info_plistpath is not null)
					return info_plistpath;

				switch (Platform) {
				case ApplePlatform.iOS:
				case ApplePlatform.TVOS:
					return Path.Combine (AppDirectory, "Info.plist");
				case ApplePlatform.MacCatalyst:
				case ApplePlatform.MacOSX:
					return Path.Combine (AppDirectory, "Contents", "Info.plist");
				default:
					throw ErrorHelper.CreateError (71, Errors.MX0071, Platform, ProductName);
				}
			}
			set {
				info_plistpath = value;
			}
		}

		// This is just a name for this app to show in log/error messages, etc.
		public string Name {
			get { return Path.GetFileNameWithoutExtension (AppDirectory); }
		}

		bool? requires_pinvoke_wrappers;
		public bool RequiresPInvokeWrappers {
			get {
				if (requires_pinvoke_wrappers.HasValue)
					return requires_pinvoke_wrappers.Value;

				return false;
			}
			set {
				requires_pinvoke_wrappers = value;
			}
		}

#if !LEGACY_TOOLS
		public bool RequireLinkWithAttributeForObjectiveCClassSearch;
#else
		public bool RequireLinkWithAttributeForObjectiveCClassSearch = true;
#endif

		public string PlatformName {
			get {
				switch (Platform) {
				case ApplePlatform.iOS:
					return "iOS";
				case ApplePlatform.TVOS:
					return "tvOS";
				case ApplePlatform.MacOSX:
					return "macOS";
				case ApplePlatform.MacCatalyst:
					return "MacCatalyst";
				default:
					throw new NotImplementedException ();
				}
			}
		}

		public static bool IsUptodate (string source, string target, bool check_contents = false, bool check_stamp = true)
		{
			return FileCopier.IsUptodate (source, target, check_contents, check_stamp);
		}

		public static void RemoveResource (ModuleDefinition module, string name)
		{
			for (int i = 0; i < module.Resources.Count; i++) {
				var embedded = module.Resources [i] as EmbeddedResource;

				if (embedded is null || embedded.Name != name)
					continue;

				module.Resources.RemoveAt (i);
				break;
			}
		}

#if !LEGACY_TOOLS
		public static void SaveAssembly (AssemblyDefinition assembly, string destination)
		{
			var main = assembly.MainModule;
			bool symbols = main.HasSymbols;
			if (symbols) {
				var provider = new DefaultSymbolReaderProvider ();
				main.ReadSymbols (provider.GetSymbolReader (main, main.FileName));
			}

			var wp = new WriterParameters () {
				WriteSymbols = symbols,
				SymbolWriterProvider = symbols ? new CustomSymbolWriterProvider () : null,
			};

			// re-write symbols, if available, so the new tokens will match
			assembly.Write (destination, wp);

			if (!symbols) {
				// if we're not saving the symbols then we must not leave stale/old files to be used by other tools
				string dest_pdb = Path.ChangeExtension (destination, ".pdb");
				if (File.Exists (dest_pdb))
					File.Delete (dest_pdb);
			}
		}
#endif // !LEGACY_TOOLS

		public static bool ExtractResource (ModuleDefinition module, string name, string path, bool remove)
		{
			for (int i = 0; i < module.Resources.Count; i++) {
				var embedded = module.Resources [i] as EmbeddedResource;

				if (embedded is null || embedded.Name != name)
					continue;

				var dirname = Path.GetDirectoryName (path);
				if (!string.IsNullOrEmpty (dirname) && !Directory.Exists (dirname))
					Directory.CreateDirectory (dirname);

				using (Stream ostream = File.OpenWrite (path)) {
					embedded.GetResourceStream ().CopyTo (ostream);
				}

				if (remove)
					module.Resources.RemoveAt (i);

				return true;
			}

			return false;
		}

		// Checks if any of the source files have a time stamp later than any of the target files.
		//
		// If check_stamp is true, the function will use the timestamp of a "target".stamp file
		// if it's later than the timestamp of the "target" file itself.
		public static bool IsUptodate (IEnumerable<string> sources, IEnumerable<string> targets, bool check_stamp = true)
		{
			return FileCopier.IsUptodate (sources, targets, check_stamp);
		}

		public static void UpdateDirectory (string source, string target)
		{
			FileCopier.UpdateDirectory (source, target);
		}

		public void InitializeCommon ()
		{
			InitializeDeploymentTarget ();

			if (Platform == ApplePlatform.MacCatalyst) {
				// Our input SdkVersion is the macOS SDK version, but the rest of our code expects the supporting iOS version, so convert here.
				// The macOS SDK version is still stored in NativeSdkVersion for when we need it.
				if (NativeSdkVersion is null)
					throw ErrorHelper.CreateError (183, Errors.MX0183 /* NativeSdkVersion is required for Mac Catalyst builds. */);
				SdkVersion = GetMacCatalystiOSVersion (NativeSdkVersion);
			}

			if (RequiresXcodeHeaders && SdkVersion < SdkVersions.GetVersion (this)) {
				switch (Platform) {
				case ApplePlatform.iOS:
				case ApplePlatform.TVOS:
				case ApplePlatform.MacCatalyst:
					throw ErrorHelper.CreateError (180, Errors.MX0180, ProductName, PlatformName, SdkVersions.GetVersion (this), SdkVersions.Xcode);
				case ApplePlatform.MacOSX:
					throw ErrorHelper.CreateError (179, Errors.MX0179, ProductName, PlatformName, SdkVersions.GetVersion (this), SdkVersions.Xcode);
				default:
					// Default to the iOS error message, it's better than showing MX0071 (unknown platform), which would be completely unrelated
					goto case ApplePlatform.iOS;
				}
			}

			SetObjectiveCExceptionMode ();
			SetManagedExceptionMode ();

			if (SymbolMode == SymbolMode.Default) {
				SymbolMode = SymbolMode.Linker;
			}

			if (!package_managed_debug_symbols.HasValue) {
				package_managed_debug_symbols = EnableDebug;
			} else if (package_managed_debug_symbols.Value && IsLLVM) {
				ErrorHelper.Warning (3007, Errors.MX3007);
			}

			Optimizations.Initialize (this, out var messages);
			ErrorHelper.Show (messages);
			if (Driver.Verbosity > 3)
				Driver.Log (4, $"Enabled optimizations: {Optimizations}");
		}

		void InitializeDeploymentTarget ()
		{
			if (DeploymentTarget is null)
				DeploymentTarget = SdkVersions.GetVersion (this);

			if (DeploymentTarget is not null) {
				if (DeploymentTarget < SdkVersions.GetMinVersion (this))
					throw new ProductException (73, true, Errors.MT0073, ProductConstants.Version, DeploymentTarget, Xamarin.SdkVersions.GetMinVersion (this), PlatformName, ProductName);
				if (DeploymentTarget > SdkVersions.GetVersion (this))
					throw new ProductException (74, true, Errors.MX0074, ProductConstants.Version, DeploymentTarget, Xamarin.SdkVersions.GetVersion (this), PlatformName, ProductName);
			}
		}

		public void RunRegistrar ()
		{
			// The static registrar.
			if (Registrar != RegistrarMode.Static)
				throw new ProductException (67, Errors.MT0067, Registrar); // this is only called during our own build

			if (RootAssemblies.Count < 1)
				throw ErrorHelper.CreateError (130, Errors.MX0130);

			var registrar_m = RegistrarOutputLibrary;
			if (registrar_m is null)
				throw ErrorHelper.CreateError (99, "RegistrarOutputLibrary must be specified.");
			var RootAssembly = RootAssemblies [0];
			var resolvedAssemblies = new Dictionary<string, AssemblyDefinition> ();
			var resolver = new PlatformResolver () {
				RootDirectory = Path.GetDirectoryName (RootAssembly),
			};
			resolver.Configure ();

			var ps = new ReaderParameters ();
			ps.AssemblyResolver = resolver;
			foreach (var reference in References) {
				var r = resolver.Load (reference);
				if (r is null)
					throw ErrorHelper.CreateError (2002, Errors.MT2002, reference);
			}

			var productAssembly = Driver.GetProductAssembly (this);
			bool foundProductAssembly = false;
			foreach (var asm in RootAssemblies) {
				var rootName = Path.GetFileNameWithoutExtension (asm);
				if (rootName == productAssembly)
					foundProductAssembly = true;

				try {
					AssemblyDefinition lastAssembly = ps.AssemblyResolver.Resolve (AssemblyNameReference.Parse (rootName), new ReaderParameters ());
					if (lastAssembly is null) {
						ErrorHelper.Warning (7, Errors.MX0007, rootName);
						continue;
					}

					if (resolvedAssemblies.TryGetValue (rootName, out var previousAssembly)) {
						if (lastAssembly.MainModule.RuntimeVersion != previousAssembly.MainModule.RuntimeVersion) {
							Driver.Log (2, "Attemping to load an assembly another time {0} (previous {1})", lastAssembly.FullName, previousAssembly.FullName);
						}
						continue;
					}

					resolvedAssemblies.Add (rootName, lastAssembly);
					Driver.Log (3, "Loaded {0}", lastAssembly.MainModule.FileName);
				} catch (Exception ex) {
					ErrorHelper.Warning (9, ex, Errors.MX0009, $"{rootName}: {ex.Message}");
					continue;
				}
			}

			if (!foundProductAssembly)
				throw ErrorHelper.CreateError (131, Errors.MX0131, productAssembly, string.Join ("', '", RootAssemblies.ToArray ()));

			var registrar = new Registrar.StaticRegistrar (this);
			if (RootAssemblies.Count == 1) {
				registrar.GenerateSingleAssembly (resolver, resolvedAssemblies.Values, Path.ChangeExtension (registrar_m, "h"), registrar_m, Path.GetFileNameWithoutExtension (RootAssembly), out var _);
			} else {
				registrar.Generate (resolver, resolvedAssemblies.Values, Path.ChangeExtension (registrar_m, "h"), registrar_m, out var _);
			}
		}

		public Abi Abi {
			get { return abi; }
			set { abi = value; }
		}

		public bool IsArchEnabled (Abi arch)
		{
			return IsArchEnabled (abi, arch);
		}

		public static bool IsArchEnabled (Abi abi, Abi arch)
		{
			return (abi & arch) != 0;
		}

		public void ValidateAbi ()
		{
			var validAbis = new List<Abi> ();
			switch (Platform) {
			case ApplePlatform.iOS:
				if (IsDeviceBuild) {
					validAbis.Add (Abi.ARM64);
					validAbis.Add (Abi.ARM64 | Abi.LLVM);
				} else {
					validAbis.Add (Abi.x86_64);
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
			case ApplePlatform.MacOSX:
			case ApplePlatform.MacCatalyst:
				validAbis.Add (Abi.x86_64);
				validAbis.Add (Abi.ARM64);
				break;
			default:
				throw ErrorHelper.CreateError (71, Errors.MX0071, Platform, ProductName);
			}

			if (!validAbis.Contains (abi))
				throw ErrorHelper.CreateError (75, Errors.MT0075, abi, Platform, string.Join (", ", validAbis.Select ((v) => v.AsString ()).ToArray ()));
		}

		public void ClearAbi ()
		{
			abi = default;
		}

		public void ParseAbi (string abi)
		{
			Abi value;
			switch (abi) {
			case "x86_64":
				value = Abi.x86_64;
				break;
			case "arm64":
				value = Abi.ARM64;
				break;
			case "arm64+llvm":
				value = Abi.ARM64 | Abi.LLVM;
				break;
			default:
				throw ErrorHelper.CreateError (15, Errors.MT0015, abi);
			}

			this.abi = value;
		}

#if !LEGACY_TOOLS
		public void ParseRegistrar (string v)
		{
			var split = v.Split ('=');
			var name = split [0];
			var value = split.Length > 1 ? split [1] : string.Empty;
			switch (name) {
			case "static":
				Registrar = RegistrarMode.Static;
				break;
			case "dynamic":
				Registrar = RegistrarMode.Dynamic;
				break;
			case "default":
				Registrar = RegistrarMode.Default;
				break;
			case "partial":
			case "partial-static":
				Registrar = RegistrarMode.PartialStatic;
				break;
			case "managed-static":
				Registrar = RegistrarMode.ManagedStatic;
				break;
			default:
				throw ErrorHelper.CreateError (20, Errors.MX0020, "--registrar", "managed-static, static, dynamic or default");
			}

			switch (value) {
			case "trace":
				RegistrarOptions = RegistrarOptions.Trace;
				break;
			case "default":
			case "":
				RegistrarOptions = RegistrarOptions.Default;
				break;
			default:
				throw ErrorHelper.CreateError (20, Errors.MX0020, "--registrar", "static, dynamic or default");
			}
		}
#endif // !LEGACY_TOOLS

		public static string GetArchitectures (IEnumerable<Abi> abis)
		{
			var res = new List<string> ();

			foreach (var abi in abis)
				res.Add (abi.AsArchString ());

			return string.Join (", ", res.ToArray ());
		}

		public string MonoGCParams {
			get {
				switch (Platform) {
				case ApplePlatform.iOS:
				case ApplePlatform.TVOS:
					// Configure sgen to use a small nursery
					string ret = "nursery-size=512k";
					if (IsTodayExtension) {
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
				case ApplePlatform.MacCatalyst:
				case ApplePlatform.MacOSX:
					return EnableSGenConc ? "major=marksweep-conc" : "major=marksweep";
				default:
					throw ErrorHelper.CreateError (71, Errors.MX0071, Platform, ProductName);
				}
			}
		}

		public bool IsFrameworkAvailableInSimulator (string framework)
		{
			if (!Driver.GetFrameworks (this).TryGetValue (framework, out var fw))
				return true; // Unknown framework, assume it's valid for the simulator

			return fw.IsFrameworkAvailableInSimulator (this);
		}

		public static bool TryParseManagedExceptionMode (string value, out MarshalManagedExceptionMode mode)
		{
			mode = MarshalManagedExceptionMode.Default;

			switch (value) {
			case "default":
				mode = MarshalManagedExceptionMode.Default;
				break;
			case "unwindnative":
			case "unwindnativecode":
				mode = MarshalManagedExceptionMode.UnwindNativeCode;
				break;
			case "throwobjectivec":
			case "throwobjectivecexception":
				mode = MarshalManagedExceptionMode.ThrowObjectiveCException;
				break;
			case "abort":
				mode = MarshalManagedExceptionMode.Abort;
				break;
			case "disable":
				mode = MarshalManagedExceptionMode.Disable;
				break;
			default:
				return false;
			}

			return true;
		}

		public static bool TryParseObjectiveCExceptionMode (string value, out MarshalObjectiveCExceptionMode mode)
		{
			mode = MarshalObjectiveCExceptionMode.Default;
			switch (value) {
			case "default":
				mode = MarshalObjectiveCExceptionMode.Default;
				break;
			case "unwindmanaged":
			case "unwindmanagedcode":
				mode = MarshalObjectiveCExceptionMode.UnwindManagedCode;
				break;
			case "throwmanaged":
			case "throwmanagedexception":
				mode = MarshalObjectiveCExceptionMode.ThrowManagedException;
				break;
			case "abort":
				mode = MarshalObjectiveCExceptionMode.Abort;
				break;
			case "disable":
				mode = MarshalObjectiveCExceptionMode.Disable;
				break;
			default:
				return false;
			}
			return true;
		}

		public void SetManagedExceptionMode ()
		{
			switch (MarshalManagedExceptions) {
			case MarshalManagedExceptionMode.Default:
				MarshalManagedExceptions = MarshalManagedExceptionMode.ThrowObjectiveCException;
				IsDefaultMarshalManagedExceptionMode = true;
				break;
			case MarshalManagedExceptionMode.UnwindNativeCode:
			case MarshalManagedExceptionMode.Disable:
				if (XamarinRuntime == XamarinRuntime.CoreCLR)
					throw ErrorHelper.CreateError (185, Errors.MX0185 /* The option '{0}' cannot take the value '{1}' when using CoreCLR. */, "--marshal-managed-exceptions", MarshalManagedExceptions.ToString ().ToLowerInvariant ());
				break;
			}
		}

		public void SetObjectiveCExceptionMode ()
		{
			switch (MarshalObjectiveCExceptions) {
			case MarshalObjectiveCExceptionMode.Default:
				MarshalObjectiveCExceptions = MarshalObjectiveCExceptionMode.ThrowManagedException;
				break;
			case MarshalObjectiveCExceptionMode.UnwindManagedCode:
			case MarshalObjectiveCExceptionMode.Disable:
				if (XamarinRuntime == XamarinRuntime.CoreCLR)
					throw ErrorHelper.CreateError (185, Errors.MX0185 /* The option '{0}' cannot take the value '{1}' when using CoreCLR. */, "--marshal-objectivec-exceptions", MarshalObjectiveCExceptions.ToString ().ToLowerInvariant ());
				break;
			}
		}

		// For mobile device builds: returns whether an assembly is interpreted.
		// For macOS: N/A
		public bool IsInterpreted (string assembly)
		{
			if (Platform == ApplePlatform.MacOSX)
				throw ErrorHelper.CreateError (99, Errors.MX0099, "IsInterpreted isn't a valid operation for macOS apps.");

#if LEGACY_TOOLS
			if (IsSimulatorBuild)
				return false;
#endif

			// IsAOTCompiled and IsInterpreted are not opposites: mscorlib.dll can be both.
			if (!UseInterpreter)
				return false;

			// Go through the list of assemblies to interpret in reverse order,
			// so that the last option passed to mtouch takes precedence.
			for (int i = InterpretedAssemblies.Count - 1; i >= 0; i--) {
				var opt = InterpretedAssemblies [i];
				if (opt == "all")
					return true;
				else if (opt == "-all")
					return false;
				else if (opt == assembly)
					return true;
				else if (opt [0] == '-' && opt.Substring (1) == assembly)
					return false;
			}

			// There's an implicit 'all' at the start of the list.
			return true;
		}

		// Returns whether an assembly is Mono AOT-compiled.
		public bool IsAOTCompiled (string assembly)
		{
			// We're never AOT compiled if we're using CoreCLR
			if (XamarinRuntime == XamarinRuntime.CoreCLR)
				return false;

			if (!UseInterpreter) {
				if (Platform == ApplePlatform.MacCatalyst)
					return IsArchEnabled (Abi.ARM64);

				if (IsSimulatorBuild && IsArchEnabled (Abi.ARM64))
					return true;

				return IsDeviceBuild;
			}

			// IsAOTCompiled and IsInterpreted are not opposites: mscorlib.dll can be both:
			// - mscorlib will always be processed by the AOT compiler to generate required wrapper functions for the interpreter to work
			// - mscorlib might also be fully AOT-compiled (both when the interpreter is enabled and when it's not)
			if (assembly == Driver.CorlibName)
				return true;

			return !IsInterpreted (assembly);
		}

#if !LEGACY_TOOLS
		public IList<string> GetAotArguments (string filename, Abi abi, string outputDir, string outputFile, string llvmOutputFile, string dataFile)
		{
			GetAotArguments (filename, abi, outputDir, outputFile, llvmOutputFile, dataFile, null, out var processArguments, out var aotArguments);
			processArguments.Add (string.Join (",", aotArguments));
			processArguments.Add (filename);
			return processArguments;
		}

		public void GetAotArguments (string filename, Abi abi, string outputDir, string outputFile, string llvmOutputFile, string dataFile, bool? isDedupAssembly, out List<string> processArguments, out List<string> aotArguments, string? llvm_path = null)
		{
			string fname = Path.GetFileName (filename);
			processArguments = new List<string> ();
			var app = this;
			bool enable_llvm = (abi & Abi.LLVM) != 0;
			bool enable_thumb = (abi & Abi.Thumb) != 0;
			bool enable_debug = app.EnableDebug;
			bool enable_debug_symbols = app.PackageManagedDebugSymbols;
			bool interp = app.IsInterpreted (Assembly.GetIdentity (filename)) && !(isDedupAssembly.HasValue && isDedupAssembly.Value);
			bool interp_full = !interp && app.UseInterpreter;
			bool is32bit = (abi & Abi.Arch32Mask) > 0;
			string arch = abi.AsArchString ();

			processArguments.Add ("--debug");

			if (enable_llvm)
				processArguments.Add ("--llvm");

			if (!interp)
				processArguments.Add ("-O=gsharedvt");
			if (app.AotOtherArguments is not null)
				processArguments.AddRange (app.AotOtherArguments);
			if (app.AotFloat32.HasValue)
				processArguments.Add (app.AotFloat32.Value ? "-O=float32" : "-O=-float32");
			aotArguments = new List<string> ();
			if (Platform == ApplePlatform.MacCatalyst) {
				aotArguments.Add ($"--aot=mtriple={arch}-apple-ios{DeploymentTarget}-macabi");
			} else {
				aotArguments.Add ($"--aot=mtriple={(enable_thumb ? arch.Replace ("arm", "thumb") : arch)}-ios");
			}
			aotArguments.Add ($"data-outfile={dataFile}");
			aotArguments.Add ("static");
			aotArguments.Add ("asmonly");
			// This method is used in legacy build as well, where dedup is not supported. 
			// Variable isDedupAssembly could have the following values:
			// - NULL means that dedup is not enabled
			// - FALSE means that dedup-skip flag should be passed for all assemblies except a container assemblt
			// - TRUE means that dedup-include flag should be passed for the container assembly
			if (isDedupAssembly.HasValue) {
				if (isDedupAssembly.Value) {
					aotArguments.Add ($"dedup-include={fname}");
				} else {
					aotArguments.Add ($"dedup-skip");
				}
			}
			if (app.LibMonoLinkMode == AssemblyBuildTarget.StaticObject)
				aotArguments.Add ("direct-icalls");
			aotArguments.AddRange (app.AotArguments);
			if (interp) {
				if (fname != Driver.CorlibName + ".dll")
					throw ErrorHelper.CreateError (99, Errors.MX0099, fname);
				aotArguments.Add ("interp");
			} else if (interp_full) {
				aotArguments.Add ("interp");
				aotArguments.Add ("full");
			} else
				aotArguments.Add ("full");

			if (IsDeviceBuild) {
				aotArguments.Add ("readonly-value=ObjCRuntime.Runtime.Arch=i4/0");
			} else if (IsSimulatorBuild) {
				aotArguments.Add ("readonly-value=ObjCRuntime.Runtime.Arch=i4/1");
			}

			var aname = Path.GetFileNameWithoutExtension (fname);
			var sdk_or_product = Profile.IsSdkAssembly (aname) || Profile.IsProductAssembly (aname);

			if (enable_llvm)
				aotArguments.Add ("nodebug");
			else if (!(enable_debug || enable_debug_symbols))
				aotArguments.Add ("nodebug");
			else if (app.DebugAll || app.DebugAssemblies.Contains (fname) || !sdk_or_product)
				aotArguments.Add ("soft-debug");

			aotArguments.Add ("dwarfdebug");

			/* Needed for #4587 */
			if (enable_debug && !enable_llvm)
				aotArguments.Add ("no-direct-calls");

			if (!app.UseDlsym (filename))
				aotArguments.Add ("direct-pinvoke");

			if (enable_llvm) {
				if (!string.IsNullOrEmpty (llvm_path)) {
					aotArguments.Add ($"llvm-path={llvm_path}");
				} else {
					aotArguments.Add ($"llvm-path={Driver.GetFrameworkCurrentDirectory (app)}/LLVM/bin/");
				}
			}

			aotArguments.Add ($"outfile={outputFile}");
			if (enable_llvm)
				aotArguments.Add ($"llvm-outfile={llvmOutputFile}");

			// If the interpreter is enabled, and we're building for x86_64, we're AOT-compiling but we
			// don't have access to infinite trampolines. So we're bumping the trampoline count (unless
			// the developer has already set a value) to something higher than the default.
			//
			// Ref:
			// * https://github.com/dotnet/macios/issues/14887
			// * https://github.com/dotnet/runtime/issues/68808
			if (interp && (abi & Abi.x86_64) == Abi.x86_64) {
				// The default values are here: https://github.com/dotnet/runtime/blob/main/src/mono/mono/mini/aot-compiler.c#L13945-L13953
				// Let's try 4x the default values.
				var trampolines = new []
				{
					(Name: "ntrampolines", Default: 4096),
					(Name: "nrgctx-trampolines", Default: 4096),
					(Name: "nimt-trampolines", Default: 512),
					(Name: "nrgctx-fetch-trampolines", Default: 128),
					(Name: "ngsharedvt-trampolines", Default: 512),
					(Name: "nftnptr-arg-trampolines", Default: 128),
					(Name: "nunbox-arbitrary-trampolines", Default: 256),
				};
				foreach (var tramp in trampolines) {
					var nameWithEq = tramp.Name + "=";
					if (!aotArguments.Any (v => v.StartsWith (nameWithEq, StringComparison.Ordinal)))
						aotArguments.Add (nameWithEq + (tramp.Default * 4).ToString (CultureInfo.InvariantCulture));
				}
			}
		}
#endif // !LEGACY_TOOLS

		public string AssemblyName {
			get {
				return Path.GetFileName (RootAssemblies [0]);
			}
		}

		internal ProductConstants ProductConstants {
			get {
				switch (Platform) {
				case ApplePlatform.iOS:
					return ProductConstants.iOS;
				case ApplePlatform.MacCatalyst:
					return ProductConstants.MacCatalyst;
				case ApplePlatform.TVOS:
					return ProductConstants.tvOS;
				case ApplePlatform.MacOSX:
					return ProductConstants.macOS;
				default:
					throw ErrorHelper.CreateError (71, Errors.MX0071, Platform, ProductName);
				}
			}
		}

#if !LEGACY_TOOLS
		public void SetDlsymOption (string asm, bool dlsym)
		{
			if (DlsymAssemblies is null)
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
				if (DlsymAssemblies is null)
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

			if (DlsymAssemblies is not null) {
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

			// Even if this assembly is aot'ed, if we are using the interpreter we can't yet
			// guarantee that code in this assembly won't be executed in interpreted mode,
			// which can happen for virtual calls between assemblies, during exception handling
			// etc. We make sure we don't strip away symbols needed for pinvoke calls.
			// https://github.com/mono/mono/issues/14206
			if (UseInterpreter)
				return true;

			// There are native frameworks which aren't available in the simulator, and we have
			// bound P/Invokes to those native frameworks. This means that AOT-compiling for
			// the simulator will fail because the corresponding native functions don't exist.
			// So default to dlsym for the simulator.
			if (IsSimulatorBuild && Profile.IsProductAssembly (Path.GetFileNameWithoutExtension (assembly)))
				return true;

			switch (Platform) {
			case ApplePlatform.iOS:
				return !Profile.IsSdkAssembly (Path.GetFileNameWithoutExtension (assembly));
			case ApplePlatform.TVOS:
				return false;
			case ApplePlatform.MacCatalyst:
				// https://github.com/dotnet/macios/issues/14437
				return true;
			default:
				throw ErrorHelper.CreateError (71, Errors.MX0071, Platform, ProductName);
			}
		}
#endif // !LEGACY_TOOLS

		public bool VerifyDynamicFramework (string framework_path)
		{
			var framework_filename = Path.Combine (framework_path, Path.GetFileNameWithoutExtension (framework_path));
			var dynamic = false;

			try {
				dynamic = MachO.IsDynamicFramework (framework_filename);
			} catch (Exception e) {
				throw ErrorHelper.CreateError (140, e, Errors.MT0140, framework_filename);
			}

			if (!dynamic)
				Driver.Log (1, "The framework {0} is a framework of static libraries, and will not be copied to the app.", framework_path);

			return dynamic;
		}

		static Application ()
		{
			SetDefaultHiddenWarnings ();
		}

		public static void SetDefaultHiddenWarnings ()
		{
			// People don't like these warnings (#20670), and they also complicate our tests, so ignore them.
			ErrorHelper.ParseWarningLevel (ErrorHelper.WarningLevel.Disable, "4178"); // The class '{0}' will not be registered because the {1} framework has been removed from the {2} SDK.
			ErrorHelper.ParseWarningLevel (ErrorHelper.WarningLevel.Disable, "4189"); // The class '{0}' will not be registered because it has been removed from the {1} SDK.
			ErrorHelper.ParseWarningLevel (ErrorHelper.WarningLevel.Disable, "4190"); // The class '{0}' will not be registered because the {1} framework has been deprecated from the {2} SDK.
		}
	}
}
