/*
 * Copyright 2014 Xamarin Inc. All rights reserved.
 *
 * Authors:
 *   Rolf Bjarne Kvinge <rolf@xamarin.com>
 *
 */

using System.Diagnostics;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text;

using Xamarin.MacDev;
using Xamarin.Utils;

namespace Xamarin.Bundler {
	public partial class Driver {
		public static bool Force { get; set; }

#if LEGACY_TOOLS
		public static int Main (string [] args)
		{
			try {
				Console.OutputEncoding = new UTF8Encoding (false, false);
				SetCurrentLanguage ();
				return Main2 (args);
			} catch (Exception e) {
				ErrorHelper.Show (e);
			} finally {
				Watch ("Total time", 0);
			}
			return 0;
		}

		// Returns true if the process should exit (with a 0 exit code; failures are propagated using exceptions)
		static void ParseOptions (Application app, Mono.Options.OptionSet options, string [] args)
		{
			options.Add ("v|verbose", "Specify how verbose the output should be. This can be passed multiple times to increase the verbosity.", v => Verbosity++);
			options.Add ("q|quiet", "Specify how quiet the output should be. This can be passed multiple times to increase the silence.", v => Verbosity--);
			options.Add ("reference=", "Add an assembly to be processed.", v => app.References.Add (v));
			options.Add ("sdkroot=", "Specify the location of Apple SDKs, default to 'xcode-select' value.", v => sdk_root = v);
			options.Add ("sdk=", "Specifies the SDK version to compile against (version, for example \"10.9\"). For Mac Catalyst, this is the macOS version of the SDK.", v => {
				try {
					app.SdkVersion = StringUtils.ParseVersion (v);
					app.NativeSdkVersion = app.SdkVersion;
				} catch (Exception ex) {
					throw ErrorHelper.CreateError (26, ex, Errors.MX0026, $"sdk:{v}", ex.Message);
				}
			});
			options.Add ("target-framework=", "Specify target framework to use. Currently supported: '" + string.Join ("', '", TargetFramework.ValidFrameworks.Select ((v) => v.ToString ())) + "'.", v => {
				targetFramework = TargetFramework.Parse (v);
			});
			options.Add ("abi=", "Comma-separated list of ABIs to target.", v => app.ParseAbi (v));
			options.Add ("runregistrar:", "Runs the registrar on the input assembly and outputs a corresponding native library.",
				v => {
					app.RegistrarOutputLibrary = v;
				}
			);
			options.Add ("xamarin-runtime=", "Which runtime to use (MonoVM or CoreCLR).", v => {
				if (!Enum.TryParse<XamarinRuntime> (v, out var rv))
					throw new InvalidOperationException ($"Invalid XamarinRuntime '{v}'");
				app.XamarinRuntime = rv;
			});
			options.Add ("rid=", "The runtime identifier we're building for", v => {
				app.RuntimeIdentifier = v;
			});

			try {
				app.RootAssemblies.AddRange (options.Parse (args));
			} catch (ProductException) {
				throw;
			} catch (Exception e) {
				throw ErrorHelper.CreateError (10, e, Errors.MX0010, e);
			}
		}
#endif // !LEGACY_TOOLS

		public static int Verbosity {
			get { return ErrorHelper.Verbosity; }
			set { ErrorHelper.Verbosity = value; }
		}

		static Driver ()
		{
			Verbosity = GetDefaultVerbosity ();
		}

		static int GetDefaultVerbosity ()
		{
			var v = 0;
			var fn = Path.Combine (Environment.GetFolderPath (Environment.SpecialFolder.UserProfile), $".{NAME}-verbosity");
			if (File.Exists (fn)) {
				v = (int) new FileInfo (fn).Length;
				if (v == 0)
					v = 4; // this is the magic verbosity level we give everybody.
			}
			return v;
		}

		public static void Log (string value)
		{
			Log (0, value);
		}

		public static void Log (string format, params object [] args)
		{
			Log (0, format, args);
		}

		public static void Log (int min_verbosity, string value)
		{
			if (min_verbosity > Verbosity)
				return;

			Console.WriteLine (value);
		}

		public static void Log (int min_verbosity, string format, params object [] args)
		{
			if (min_verbosity > Verbosity)
				return;

			if (args.Length > 0)
				Console.WriteLine (format, args);
			else
				Console.WriteLine (format);
		}

		static TargetFramework targetFramework;

		public static TargetFramework TargetFramework {
			get { return targetFramework; }
			set { targetFramework = value; }
		}

		static void FileMove (string source, string target)
		{
			File.Delete (target);
			File.Move (source, target);
		}

		static void MoveIfDifferent (string path, string tmp, bool use_stamp = false)
		{
			// Don't read the entire file into memory, it can be quite big in certain cases.

			bool move = false;

			using (var fs1 = new FileStream (path, FileMode.Open, FileAccess.Read)) {
				using (var fs2 = new FileStream (tmp, FileMode.Open, FileAccess.Read)) {
					if (fs1.Length != fs2.Length) {
						Log (3, "New file '{0}' has different length, writing new file.", path);
						move = true;
					} else {
						move = !Cache.CompareStreams (fs1, fs2);
					}
				}
			}

			if (move) {
				FileMove (tmp, path);
			} else {
				Log (3, "Target {0} is up-to-date.", path);
				if (use_stamp)
					Driver.Touch (path + ".stamp");
			}
		}

		public static void WriteIfDifferent (string path, string contents, bool use_stamp = false)
		{
			var tmp = path + ".tmp";

			try {
				if (!File.Exists (path)) {
					Directory.CreateDirectory (Path.GetDirectoryName (path));
					File.WriteAllText (path, contents);
					Log (3, "File '{0}' does not exist, creating it.", path);
					return;
				}

				File.WriteAllText (tmp, contents);
				MoveIfDifferent (path, tmp, use_stamp);
			} catch (Exception e) {
				File.WriteAllText (path, contents);
				ErrorHelper.Warning (1014, e, Errors.MT1014, path, e.Message);
			} finally {
				File.Delete (tmp);
			}
		}

		public static void WriteIfDifferent (string path, byte [] contents, bool use_stamp = false)
		{
			var tmp = path + ".tmp";

			try {
				if (!File.Exists (path)) {
					File.WriteAllBytes (path, contents);
					Log (3, "File '{0}' does not exist, creating it.", path);
					return;
				}

				File.WriteAllBytes (tmp, contents);
				MoveIfDifferent (path, tmp, use_stamp);
			} catch (Exception e) {
				File.WriteAllBytes (path, contents);
				ErrorHelper.Warning (1014, e, Errors.MT1014, path, e.Message);
			} finally {
				File.Delete (tmp);
			}
		}

		internal static string GetFullPath ()
		{
			return System.Reflection.Assembly.GetExecutingAssembly ().Location;
		}

		static string xcode_product_version;
		public static string XcodeProductVersion {
			get {
				return xcode_product_version;
			}
		}

		static Version xcode_version;
		public static Version XcodeVersion {
			get {
				return xcode_version;
			}
		}

		static void SetCurrentLanguage ()
		{
			// There's no way to change the current culture from the command-line
			// without changing the system settings, so honor LANG if set.
			// This eases testing mtouch/mmp with different locales significantly,
			// and won't run into issues where changing the system language leaves
			// the tester with an incomprehensible system.
			var lang_variable = Environment.GetEnvironmentVariable ("LANG");
			if (string.IsNullOrEmpty (lang_variable))
				return;

			// Mimic how mono transforms LANG into a culture name:
			// https://github.com/mono/mono/blob/fc6e8a27fc55319141ceb29fbb7b5c63a9030b5e/mono/metadata/locales.c#L568-L576
			var lang = lang_variable;
			var idx = lang.IndexOf ('.');
			if (idx >= 0)
				lang = lang.Substring (0, idx);
			idx = lang.IndexOf ('@');
			if (idx >= 0)
				lang = lang.Substring (0, idx);
			lang = lang.Replace ('_', '-');
			try {
				var culture = CultureInfo.GetCultureInfo (lang);
				if (culture is not null) {
					CultureInfo.DefaultThreadCurrentCulture = culture;
					Log (2, $"The current language was set to '{culture.DisplayName}' according to the LANG environment variable (LANG={lang_variable}).");
				}
			} catch (Exception e) {
				ErrorHelper.Warning (124, e, Errors.MT0124, lang, lang_variable, e.Message);
			}
		}

		public static void Touch (IEnumerable<string> filenames, DateTime? timestamp = null)
		{
			if (timestamp is null)
				timestamp = DateTime.Now;
			foreach (var filename in filenames) {
				try {
					var fi = new FileInfo (filename);
					if (!fi.Exists) {
						using (var fo = fi.OpenWrite ()) {
							// Create an empty file.
						}
					}
					fi.LastWriteTime = timestamp.Value;
				} catch (Exception e) {
					ErrorHelper.Warning (128, Errors.MT0128, filename, e.Message);
				}
			}
		}

		public static void Touch (params string [] filenames)
		{
			Touch ((IEnumerable<string>) filenames);
		}

		static int watch_level;
		static Stopwatch watch;

		public static int WatchLevel {
			get { return watch_level; }
			set {
				watch_level = value;
				if ((watch_level > 0) && (watch is null)) {
					watch = new Stopwatch ();
					watch.Start ();
				}
			}
		}

		public static void Watch (string msg, int level)
		{
			if ((watch is null) || (level > WatchLevel))
				return;
			for (int i = 0; i < level; i++)
				Console.Write ("!");
			Console.WriteLine ("Timestamp {0}: {1} ms", msg, watch.ElapsedMilliseconds);
		}

		internal static PDictionary FromPList (string name)
		{
			if (!File.Exists (name))
				throw ErrorHelper.CreateError (24, Errors.MT0024, name);
			return PDictionary.FromFile (name);
		}

		const string XcodeDefault = "/Applications/Xcode.app";

		static string FindSystemXcode ()
		{
			var output = new StringBuilder ();
			if (Driver.RunCommand ("xcode-select", new [] { "-p" }, output: output) != 0) {
				ErrorHelper.Warning (59, Errors.MX0059, output.ToString ());
				return null;
			}
			return output.ToString ().Trim ();
		}

		static string sdk_root;
		static string developer_directory;

		public static string SdkRoot {
			get => sdk_root;
			set => sdk_root = value;
		}

		public static string DeveloperDirectory {
			get {
				return developer_directory;
			}
		}

		// This returns the /Applications/Xcode*.app/Contents/Developer/Platforms directory
		public static string PlatformsDirectory {
			get {
				return Path.Combine (DeveloperDirectory, "Platforms");
			}
		}

		// This returns the /Applications/Xcode*.app/Contents/Developer/Platforms/*.platform directory
		public static string GetPlatformDirectory (Application app)
		{
			return Path.Combine (PlatformsDirectory, GetPlatform (app) + ".platform");
		}

		static string framework_dir;
		public static string GetFrameworkCurrentDirectory (Application app)
		{
			if (framework_dir is null)
				throw new InvalidOperationException ($"Teh current framework directory hasn't been set.");
			return framework_dir;
		}

		public static void SetFrameworkCurrentDirectory (string value)
		{
			framework_dir = value;
		}

		// This returns the platform to use in /Applications/Xcode*.app/Contents/Developer/Platforms/*.platform
		public static string GetPlatform (Application app)
		{
			switch (app.Platform) {
			case ApplePlatform.iOS:
				return app.IsDeviceBuild ? "iPhoneOS" : "iPhoneSimulator";
			case ApplePlatform.TVOS:
				return app.IsDeviceBuild ? "AppleTVOS" : "AppleTVSimulator";
			case ApplePlatform.MacOSX:
			case ApplePlatform.MacCatalyst:
				return "MacOSX";
			default:
				throw ErrorHelper.CreateError (71, Errors.MX0071, app.Platform, app.ProductName);
			}
		}

		// This returns the correct /Applications/Xcode*.app/Contents/Developer/Platforms/*.platform/Developer/SDKs/*X.Y.sdk directory
		public static string GetFrameworkDirectory (Application app)
		{
			var platform = GetPlatform (app);

			return Path.Combine (PlatformsDirectory, platform + ".platform", "Developer", "SDKs", platform + app.NativeSdkVersion.ToString () + ".sdk");
		}

		public static string GetProductAssembly (Application app)
		{
			switch (app.Platform) {
			case ApplePlatform.iOS:
				return "Microsoft.iOS";
			case ApplePlatform.TVOS:
				return "Microsoft.tvOS";
			case ApplePlatform.MacOSX:
				return "Microsoft.macOS";
			case ApplePlatform.MacCatalyst:
				return "Microsoft.MacCatalyst";
			default:
				throw ErrorHelper.CreateError (71, Errors.MX0071, app.Platform, app.ProductName);
			}
		}

		public static void ValidateXcode (Application app, bool accept_any_xcode_version, bool warn_if_not_found)
		{
			if (sdk_root is null) {
				sdk_root = FindSystemXcode ();
				if (sdk_root is null) {
					// FindSystemXcode showed a warning in this case. In particular do not use 'string.IsNullOrEmpty' here,
					// because FindSystemXcode may return an empty string (with no warning printed) if the xcode-select command
					// succeeds, but returns nothing.
					sdk_root = null;
				} else if (!Directory.Exists (sdk_root)) {
					ErrorHelper.Warning (60, Errors.MX0060, sdk_root);
					sdk_root = null;
				} else {
					if (!accept_any_xcode_version)
						ErrorHelper.Warning (61, Errors.MT0061, sdk_root);
				}
				if (sdk_root is null) {
					sdk_root = XcodeDefault;
					if (!Directory.Exists (sdk_root)) {
						if (warn_if_not_found) {
							// mmp: and now we give up, but don't throw like mtouch, because we don't want to change behavior (this sometimes worked it appears)
							ErrorHelper.Warning (56, Errors.MX0056);
							return; // Can't validate the version below if we can't even find Xcode...
						}

						throw ErrorHelper.CreateError (56, Errors.MX0056);
					}
					ErrorHelper.Warning (62, Errors.MT0062, sdk_root);
				}
			} else if (!Directory.Exists (sdk_root)) {
				throw ErrorHelper.CreateError (55, Errors.MT0055, sdk_root);
			}

			// Check what kind of path we got
			if (File.Exists (Path.Combine (sdk_root, "Contents", "MacOS", "Xcode"))) {
				// path to the Xcode.app
				developer_directory = Path.Combine (sdk_root, "Contents", "Developer");
			} else if (File.Exists (Path.Combine (sdk_root, "..", "MacOS", "Xcode"))) {
				// path to Contents/Developer
				developer_directory = Path.GetFullPath (Path.Combine (sdk_root, "..", "..", "Contents", "Developer"));
			} else {
				throw ErrorHelper.CreateError (57, Errors.MT0057, sdk_root);
			}

			var plist_path = Path.Combine (Path.GetDirectoryName (DeveloperDirectory), "version.plist");

			if (File.Exists (plist_path)) {
				var plist = FromPList (plist_path);
				var version = plist.GetString ("CFBundleShortVersionString");
				xcode_version = new Version (version);
				xcode_product_version = plist.GetString ("ProductBuildVersion");
			} else {
				throw ErrorHelper.CreateError (58, Errors.MT0058, Path.GetDirectoryName (Path.GetDirectoryName (DeveloperDirectory)), plist_path);
			}

			Driver.Log (1, "Using Xcode {0} ({2}) found in {1}", XcodeVersion, sdk_root, XcodeProductVersion);
		}

		internal static bool TryParseBool (string value, out bool result)
		{
			if (string.IsNullOrEmpty (value)) {
				result = true;
				return true;
			}

			switch (value.ToLowerInvariant ()) {
			case "1":
			case "yes":
			case "true":
			case "enable":
				result = true;
				return true;
			case "0":
			case "no":
			case "false":
			case "disable":
				result = false;
				return true;
			default:
				return bool.TryParse (value, out result);
			}
		}

		internal static bool ParseBool (string value, string name, bool show_error = true)
		{
			bool result;
			if (!TryParseBool (value, out result))
				throw ErrorHelper.CreateError (26, Errors.MX0026, name, value);
			return result;
		}

		static readonly Dictionary<string, string> tools = new Dictionary<string, string> ();
		static string FindTool (Application app, string tool)
		{
			string path;

			lock (tools) {
				if (tools.TryGetValue (tool, out path))
					return path;
			}

			path = LocateTool (app, tool);
			static string LocateTool (Application app, string tool)
			{
				if (XcrunFind (app, tool, out var path))
					return path;

				// either /Developer (Xcode 4.2 and earlier), /Applications/Xcode.app/Contents/Developer (Xcode 4.3) or user override
				path = Path.Combine (DeveloperDirectory, "usr", "bin", tool);
				if (File.Exists (path))
					return path;

				// Xcode 4.3 (without command-line tools) also has a copy of 'strip'
				path = Path.Combine (DeveloperDirectory, "Toolchains", "XcodeDefault.xctoolchain", "usr", "bin", tool);
				if (File.Exists (path))
					return path;

				// Xcode "Command-Line Tools" install a copy in /usr/bin (and it can be there afterward)
				path = Path.Combine ("/usr", "bin", tool);
				if (File.Exists (path))
					return path;

				return null;
			}

			// We can end up finding the same tool multiple times.
			// That's not a problem.
			lock (tools)
				tools [tool] = path;

			if (path is null)
				throw ErrorHelper.CreateError (5307, Errors.MX5307 /* Missing '{0}' tool. Please install Xcode 'Command-Line Tools' component */, tool);

			return path;
		}

		static bool XcrunFind (Application app, string tool, out string path)
		{
			return XcrunFind (app, ApplePlatform.None, false, tool, out path);
		}

		static bool XcrunFind (Application app, ApplePlatform platform, bool is_simulator, string tool, out string path)
		{
			var env = new Dictionary<string, string> ();
			// Unset XCODE_DEVELOPER_DIR_PATH. See https://github.com/dotnet/macios/issues/3931.
			env.Add ("XCODE_DEVELOPER_DIR_PATH", null);
			// Set DEVELOPER_DIR if we have it
			if (!string.IsNullOrEmpty (DeveloperDirectory))
				env.Add ("DEVELOPER_DIR", DeveloperDirectory);

			path = null;

			var args = new List<string> ();
			if (platform != ApplePlatform.None) {
				args.Add ("-sdk");
				switch (platform) {
				case ApplePlatform.iOS:
					args.Add (is_simulator ? "iphonesimulator" : "iphoneos");
					break;
				case ApplePlatform.MacOSX:
					args.Add ("macosx");
					break;
				case ApplePlatform.TVOS:
					args.Add (is_simulator ? "appletvsimulator" : "appletvos");
					break;
				default:
					throw ErrorHelper.CreateError (71, Errors.MX0071 /* Unknown platform: {0}. This usually indicates a bug in {1}; please file a bug report at https://github.com/dotnet/macios/issues/new with a test case. */, platform.ToString (), app.ProductName);
				}
			}
			args.Add ("-f");
			args.Add (tool);

			var stdout = new StringBuilder ();
			var stderr = new StringBuilder ();
			var both = new StringBuilder ();
			// xcrun can write unrelated stuff to stderr even if it succeeds, so we need to separate stdout and stderr.
			// We also want to print out what happened if something went wrong, and in that case we don't want stdout
			// and stderr captured separately, because related lines could end up printed far from eachother in time,
			// and that's confusing. So capture stdout and stderr by themselves, and also capture both together.
			int ret = RunCommand ("xcrun", args, env,
				(v) => {
					lock (both) {
						both.AppendLine (v);
						stdout.AppendLine (v);
					}
				},
				(v) => {
					lock (both) {
						both.AppendLine (v);
						stderr.AppendLine (v);
					}
				});

			if (ret == 0) {
				path = stdout.ToString ().Trim ();
				if (!File.Exists (path)) {
					ErrorHelper.Warning (5315, Errors.MX5315 /* The tool xcrun failed to return a valid result (the file {0} does not exist). Check build log for details. */, tool, path);
					return false;
				}
			} else {
				Log (1, "Failed to locate the developer tool '{0}', 'xcrun {1}' returned with the exit code {2}:\n{3}", tool, string.Join (" ", args), ret, both.ToString ());
			}

			return ret == 0;
		}

		public static void RunXcodeTool (Application app, string tool, params string [] arguments)
		{
			RunXcodeTool (app, tool, (IList<string>) arguments);
		}

		public static void RunXcodeTool (Application app, string tool, IList<string> arguments)
		{
			var executable = FindTool (app, tool);
			var rv = RunCommand (executable, arguments);
			if (rv != 0)
				throw ErrorHelper.CreateError (5309, Errors.MX5309 /* Failed to execute the tool '{0}', it failed with an error code '{1}'. Please check the build log for details. */, tool, rv);
		}

		public static void RunClang (Application app, IList<string> arguments)
		{
			RunXcodeTool (app, "clang", arguments);
		}

		public static void RunInstallNameTool (Application app, IList<string> arguments)
		{
			RunXcodeTool (app, "install_name_tool", arguments);
		}

		public static void RunBitcodeStrip (Application app, IList<string> arguments)
		{
			RunXcodeTool (app, "bitcode_strip", arguments);
		}

		public static void RunLipo (Application app, string output, IEnumerable<string> inputs)
		{
			var sb = new List<string> ();
			sb.AddRange (inputs);
			sb.Add ("-create");
			sb.Add ("-output");
			sb.Add (output);
			RunLipo (app, sb);
		}

		public static void RunLipoAndCreateDsym (Application app, string output, IEnumerable<string> inputs)
		{
			RunLipo (app, output, inputs);

			var dsymFolders = inputs.Select (input => input + ".dSYM").Where (Directory.Exists).ToArray ();
			if (dsymFolders.Length > 1) {
				// Lipo the dSYMs into one big happy dSYM
				var dsymLibsDir = dsymFolders.Select (dsym => Path.Combine (dsym, "Contents", "Resources", "DWARF")).ToArray ();
				var allLibs = dsymLibsDir.Where (Directory.Exists).SelectMany (dir => Directory.EnumerateFiles (dir)).Select (dir => Path.GetFileName (dir)).Distinct ().ToArray ();

				foreach (var lib in allLibs) {
					var outputLib = Path.Combine (dsymLibsDir [0], lib);
					var allDsymInputs = dsymLibsDir.Select (libDir => Path.Combine (libDir, lib)).Where (File.Exists).ToArray ();
					Driver.RunLipo (app, outputLib, allDsymInputs);
				}
			}

			// Move the dSYM next to its executable
			if (dsymFolders.Length > 0) {
				var outputDsymDir = output + ".dSYM";
				if (Directory.Exists (outputDsymDir))
					Directory.Delete (outputDsymDir, true);
				Directory.Move (dsymFolders [0], outputDsymDir);
				RunCommand ("/usr/bin/mdimport", outputDsymDir);
			}
		}

		public static void RunLipo (Application app, IList<string> options)
		{
			RunXcodeTool (app, "lipo", options);
		}

		public static void CreateDsym (Application app, string output_dir, string appname, string dsym_dir)
		{
			RunDsymUtil (app, Path.Combine (output_dir, appname), "-num-threads", "4", "-z", "-o", dsym_dir);
			RunCommand ("/usr/bin/mdimport", dsym_dir);
		}

		public static void RunDsymUtil (Application app, params string [] options)
		{
			RunXcodeTool (app, "dsymutil", options);
		}

		public static void RunStrip (Application app, IList<string> options)
		{
			RunXcodeTool (app, "strip", options);
		}

		public static string CorlibName {
			get {
				return "System.Private.CoreLib";
			}
		}

		public static Frameworks GetFrameworks (Application app)
		{
			var rv = Frameworks.GetFrameworks (app.Platform, app.IsSimulatorBuild);
			if (rv is null)
				throw ErrorHelper.CreateError (71, Errors.MX0071, app.Platform, app.ProductName);
			return rv;
		}
	}
}
