/*
 * Copyright 2014 Xamarin Inc. All rights reserved.
 *
 * Authors:
 *   Rolf Bjarne Kvinge <rolf@xamarin.com>
 *
 */

using System.Diagnostics;
using System.IO;
using System.Globalization;
using System.Linq;
using System.Text;

using Xamarin.Bundler;
using Xamarin.MacDev;
using Xamarin.Utils;

#nullable enable

namespace Xamarin.Bundler {
	public partial class Driver {
#if LEGACY_TOOLS
		public static int Main (string [] args)
		{
			try {
				Console.OutputEncoding = new UTF8Encoding (false, false);
				SetCurrentLanguage (ConsoleLog.Instance);
				return Main2 (args);
			} catch (Exception e) {
				ErrorHelper.Show (ConsoleLog.Instance, e);
			}
			return 0;
		}

		// Returns true if the process should exit (with a 0 exit code; failures are propagated using exceptions)
		static void ParseOptions (Application app, Mono.Options.OptionSet options, string [] args)
		{
			options.Add ("v|verbose", "Specify how verbose the output should be. This can be passed multiple times to increase the verbosity.", v => app.Verbosity++);
			options.Add ("q|quiet", "Specify how quiet the output should be. This can be passed multiple times to increase the silence.", v => app.Verbosity--);
			options.Add ("reference=", "Add an assembly to be processed.", v => app.References.Add (v));
			options.Add ("sdkroot=", "Specify the location of Apple SDKs, default to 'xcode-select' value.", v => app.SdkRoot = v);
			options.Add ("sdk=", "Specifies the SDK version to compile against (version, for example \"10.9\"). For Mac Catalyst, this is the macOS version of the SDK.", v => {
				try {
					app.SdkVersion = StringUtils.ParseVersion (v);
					app.NativeSdkVersion = app.SdkVersion;
				} catch (Exception ex) {
					throw ErrorHelper.CreateError (26, ex, Errors.MX0026, $"sdk:{v}", ex.Message);
				}
			});
			options.Add ("target-framework=", "Specify target framework to use. Currently supported: '" + string.Join ("', '", TargetFramework.ValidFrameworks.Select ((v) => v.ToString ())) + "'.", v => {
				app.TargetFramework = TargetFramework.Parse (v);
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
			options.Add ("xcode-version=", "The Xcode version we're building with", v => {
				if (!Version.TryParse (v, out var xcodeVersion))
					throw ErrorHelper.CreateError (26, Errors.MX0026, $"xcode-version:{v}", "Expected a valid version string.");
				app.XcodeVersion = xcodeVersion;
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

		public static int GetDefaultVerbosity (string toolName)
		{
			var v = 0;
			var fn = Path.Combine (Environment.GetFolderPath (Environment.SpecialFolder.UserProfile), $".{toolName}-verbosity");
			if (File.Exists (fn)) {
				v = (int) new FileInfo (fn).Length;
				if (v == 0)
					v = 4; // this is the magic verbosity level we give everybody if the file exists, but has no size.
			}
			return v;
		}

		static void FileMove (string source, string target)
		{
			File.Delete (target);
			File.Move (source, target);
		}

		static void MoveIfDifferent (IToolLog log, string path, string tmp, bool use_stamp = false)
		{
			// Don't read the entire file into memory, it can be quite big in certain cases.

			bool move = false;

			using (var fs1 = new FileStream (path, FileMode.Open, FileAccess.Read)) {
				using (var fs2 = new FileStream (tmp, FileMode.Open, FileAccess.Read)) {
					if (fs1.Length != fs2.Length) {
						log.Log (3, "New file '{0}' has different length, writing new file.", path);
						move = true;
					} else {
						move = !Cache.CompareStreams (log, fs1, fs2);
					}
				}
			}

			if (move) {
				FileMove (tmp, path);
			} else {
				log.Log (3, "Target {0} is up-to-date.", path);
				if (use_stamp)
					Driver.Touch (log, path + ".stamp");
			}
		}

		public static void WriteIfDifferent (IToolLog log, string path, string contents, bool use_stamp = false)
		{
			var tmp = path + ".tmp";

			try {
				if (!File.Exists (path)) {
					var dir = Path.GetDirectoryName (path);
					if (!string.IsNullOrEmpty (dir))
						Directory.CreateDirectory (dir);
					File.WriteAllText (path, contents);
					log.Log (3, "File '{0}' does not exist, creating it.", path);
					return;
				}

				File.WriteAllText (tmp, contents);
				MoveIfDifferent (log, path, tmp, use_stamp);
			} catch (Exception e) {
				File.WriteAllText (path, contents);
				ErrorHelper.Warning (log, 1014, e, Errors.MT1014, path, e.Message);
			} finally {
				File.Delete (tmp);
			}
		}

		public static void WriteIfDifferent (IToolLog log, string path, byte [] contents, bool use_stamp = false)
		{
			var tmp = path + ".tmp";

			try {
				if (!File.Exists (path)) {
					File.WriteAllBytes (path, contents);
					log.Log (3, "File '{0}' does not exist, creating it.", path);
					return;
				}

				File.WriteAllBytes (tmp, contents);
				MoveIfDifferent (log, path, tmp, use_stamp);
			} catch (Exception e) {
				File.WriteAllBytes (path, contents);
				ErrorHelper.Warning (log, 1014, e, Errors.MT1014, path, e.Message);
			} finally {
				File.Delete (tmp);
			}
		}

		internal static string GetFullPath ()
		{
			return System.Reflection.Assembly.GetExecutingAssembly ().Location;
		}

#if LEGACY_TOOLS
		static void SetCurrentLanguage (IToolLog log)
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
					log.Log (2, $"The current language was set to '{culture.DisplayName}' according to the LANG environment variable (LANG={lang_variable}).");
				}
			} catch (Exception e) {
				ErrorHelper.Warning (log, 124, e, Errors.MT0124, lang, lang_variable, e.Message);
			}
		}
#endif

		public static void Touch (IToolLog log, IEnumerable<string> filenames, DateTime? timestamp = null)
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
					ErrorHelper.Warning (log, 128, Errors.MT0128, filename, e.Message);
				}
			}
		}

		public static void Touch (IToolLog log, params string [] filenames)
		{
			Touch (log, (IEnumerable<string>) filenames);
		}

		internal static PDictionary FromPList (string name)
		{
			if (!File.Exists (name))
				throw ErrorHelper.CreateError (24, Errors.MT0024, name);
			return PDictionary.OpenFile (name);
		}

		const string XcodeDefault = "/Applications/Xcode.app";

		static string? FindSystemXcode (IToolLog log)
		{
			var output = new StringBuilder ();
			if (Driver.RunCommand (log, "xcode-select", new [] { "-p" }, output: output) != 0) {
				ErrorHelper.Warning (log, 59, Errors.MX0059, output.ToString ());
				return null;
			}
			return output.ToString ().Trim ();
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
			var sdkVersion = app.NativeSdkVersion?.ToString () ?? "";
			return Path.Combine (app.PlatformsDirectory, platform + ".platform", "Developer", "SDKs", platform + sdkVersion + ".sdk");
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
			var sdk_root = app.SdkRoot;

			if (sdk_root is null) {
				sdk_root = FindSystemXcode (app);
				if (sdk_root is null) {
					// FindSystemXcode showed a warning in this case. In particular do not use 'string.IsNullOrEmpty' here,
					// because FindSystemXcode may return an empty string (with no warning printed) if the xcode-select command
					// succeeds, but returns nothing.
					sdk_root = null;
				} else if (!Directory.Exists (sdk_root)) {
					ErrorHelper.Warning (app, 60, Errors.MX0060, sdk_root);
					sdk_root = null;
				} else {
					if (!accept_any_xcode_version)
						ErrorHelper.Warning (app, 61, Errors.MT0061, sdk_root);
				}
				if (sdk_root is null) {
					sdk_root = XcodeDefault;
					if (!Directory.Exists (sdk_root)) {
						if (warn_if_not_found) {
							// mmp: and now we give up, but don't throw like mtouch, because we don't want to change behavior (this sometimes worked it appears)
							ErrorHelper.Warning (app, 56, Errors.MX0056);
							return; // Can't validate the version below if we can't even find Xcode...
						}

						throw ErrorHelper.CreateError (56, Errors.MX0056);
					}
					ErrorHelper.Warning (app, 62, Errors.MT0062, sdk_root);
				}
			} else if (!Directory.Exists (sdk_root)) {
				throw ErrorHelper.CreateError (55, Errors.MT0055, sdk_root);
			}

			// Check what kind of path we got
			if (File.Exists (Path.Combine (sdk_root, "Contents", "MacOS", "Xcode"))) {
				// path to the Xcode.app
				app.DeveloperDirectory = Path.Combine (sdk_root, "Contents", "Developer");
			} else if (File.Exists (Path.Combine (sdk_root, "..", "MacOS", "Xcode"))) {
				// path to Contents/Developer
				app.DeveloperDirectory = Path.GetFullPath (Path.Combine (sdk_root, "..", "..", "Contents", "Developer"));
			} else {
				throw ErrorHelper.CreateError (57, Errors.MT0057, sdk_root);
			}

			var plist_path = Path.Combine (Path.GetDirectoryName (app.DeveloperDirectory)!, "version.plist");

			if (File.Exists (plist_path)) {
				var plist = FromPList (plist_path);
				var version = plist.GetString ("CFBundleShortVersionString");
				if (version is null)
					throw ErrorHelper.CreateError (58, Errors.MT0058, Path.GetDirectoryName (Path.GetDirectoryName (app.DeveloperDirectory)), plist_path);
				app.XcodeVersion = new Version (version);
				app.XcodeProductVersion = plist.GetString ("ProductBuildVersion");
			} else {
				throw ErrorHelper.CreateError (58, Errors.MT0058, Path.GetDirectoryName (Path.GetDirectoryName (app.DeveloperDirectory)), plist_path);
			}

			app.Log (1, "Using Xcode {0} ({2}) found in {1}", app.XcodeVersion, sdk_root, app.XcodeProductVersion);
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

		public static string CorlibName {
			get {
				return "System.Private.CoreLib";
			}
		}

		public static Frameworks GetFrameworks (Application app)
		{
			if (!Frameworks.TryGetFrameworks (app.Platform, out var rv))
				throw ErrorHelper.CreateError (71, Errors.MX0071, app.Platform, app.ProductName);
			return rv;
		}
	}
}
