// Copyright (c) Microsoft Corporation.
// Licensed under the MIT License.

using Xamarin.MacDev;
using Xamarin.Utils;

namespace Sharpie.Bind;

public sealed class Sdk {
	BindingResult bindingResult;

	public string Platform { get; private set; }
	public string Location { get; private set; }
	public string Identifier { get; private set; }
	public Version? Version { get; private set; }
	public string? Variant { get; private set; }

	readonly List<string> architectures;
	public IReadOnlyList<string> Architectures {
		get { return architectures; }
	}

	public string RecommendedArchitecture {
		get { return Architectures.First (); }
	}

	readonly Lazy<IReadOnlyList<Framework>> frameworks;
	public IReadOnlyList<Framework> Frameworks {
		get { return frameworks.Value; }
	}

	bool IsMacCatalyst {
		get { return Identifier.Contains ("macabi"); }
	}

	public Sdk (BindingResult bindingResult, string identifier)
	{
		this.bindingResult = bindingResult;
		this.Identifier = identifier;

		var platform = identifier;
		if (platform.EndsWith ("-macabi", StringComparison.OrdinalIgnoreCase)) {
			Variant = "Mac Catalyst";
			platform = platform.Substring (0, platform.Length - "-macabi".Length);
			platform = platform.Replace ("ios", "macosx"); // the version number is still the iOS version
		}

		var firstDigit = platform.IndexOfAny (['0', '1', '2', '3', '4', '5', '6', '7', '8', '9']);
		if (firstDigit >= 0) {
			if (!Version.TryParse (platform.Substring (firstDigit), out var version))
				throw new InvalidOperationException ($"Unable to parse version from SDK identifier '{Identifier}'");
			Version = version;
			Platform = platform [0..firstDigit];
		} else {
			Platform = platform;
		}
		Location = GetSdkPath ($"{Platform}{Version}");

		if (!string.IsNullOrEmpty (Variant)) {
			LoadVersionMaps ();

			if (Version is not null) {
				if (!macOS_iOSMac_map.TryGetValue (Version.ToString (), out var iOSVersion))
					throw new NotSupportedException ($"Unable to map the macOS version '{Version}' to an iOS version for Mac Catalyst.");

				Version = Version.Parse (iOSVersion);
			}

			switch (Variant) {
			case "Mac Catalyst":
				break;
			default:
				throw bindingResult.CreateInternalError ($"Unknown variant: {Variant}");
			}
		}

		frameworks = new Lazy<IReadOnlyList<Framework>> (LoadFrameworks);
		architectures = LoadArchitectures ();
	}

	public static string GetSdkPath (string sdk)
	{
		var rv = Execution.RunAsync ("xcrun", ["--show-sdk-path", "--sdk", sdk]).Result;
		if (rv.ExitCode != 0)
			throw new BindingException (new BindingMessage (9, /* Failed to the SDK path ('xcrun --show-sdk-path' failed): {0} */ rv.Output.MergedOutput));
		return rv.Output.StandardOutput.Trim ();
	}

	Dictionary<string, string>? macOS_iOSMac_map;
	Dictionary<string, string>? iOSMac_macOS_map;

	[MemberNotNull (nameof (macOS_iOSMac_map))]
	[MemberNotNull (nameof (iOSMac_macOS_map))]
	void LoadVersionMaps ()
	{
		var sdkSettingsPath = Path.Combine (Location, "SDKSettings.plist");
		var plist = PDictionary.FromFile (sdkSettingsPath)!;

		macOS_iOSMac_map = new Dictionary<string, string> ();
		iOSMac_macOS_map = new Dictionary<string, string> ();

		if (plist.TryGetValue ("VersionMap", out PDictionary? dict)) {
			if (dict.TryGetValue ("macOS_iOSMac", out PDictionary? map)) {
				foreach (var kvp in map) {
					macOS_iOSMac_map [kvp.Key] = (string) (PString) kvp.Value;
				}
			}
			if (dict.TryGetValue ("iOSMac_macOS", out map)) {
				foreach (var kvp in map) {
					iOSMac_macOS_map [kvp.Key] = (string) (PString) kvp.Value;
				}
			}
		}
	}

	public IReadOnlyList<string> GetCflags (string architecture = "arm64")
	{
		var cflags = new List<string> {
			"-isysroot", Location,
		};

		var minVersion = GetMinVersionFlag ();
		if (minVersion is not null)
			cflags.Add (minVersion);

		if (IsMacCatalyst) {
			// -target
			if (string.IsNullOrEmpty (architecture))
				throw new InvalidOperationException ($"Architecture must be specified when getting the C flags for Mac Catalyst");

			cflags.Add ("-target");
			cflags.Add ($"{architecture}-apple-{Identifier}");

			// -isystem
			cflags.Add ("-isystem");
			cflags.Add (Path.Combine (Location, "System", "iOSSupport", "usr", "include"));

			// -iframework
			cflags.Add ("-iframework");
			cflags.Add (Path.Combine (Location, "System", "iOSSupport", "System", "Library", "Frameworks"));
		}

		return cflags;
	}

	string? GetMinVersionFlag ()
	{
		string platform;

		if (Version is null)
			return null;

		switch (this.Platform) {
		case "iphonesimulator":
			platform = "ios-simulator";
			break;
		case "appletvos":
			platform = "tvos";
			break;
		case "iphoneos":
		case "watchos":
			platform = this.Platform;
			break;
		case "macosx":
			if (IsMacCatalyst)
				return null;

			platform = this.Platform;
			break;
		default:
			return null;
		}

		return "-m" + platform + "-version-min=" + Version;
	}

	string GetUmbrellaHeader (string frameworkName)
	{
		switch (frameworkName) {
		case "ThreadNetwork":
			return "THClient.h";
		default:
			return frameworkName + ".h";
		}
	}

	IReadOnlyList<Framework> LoadFrameworks ()
	{
		var frameworks = new Dictionary<string, Framework> ();
		var fxRoots = new List<string> ();
		if (IsMacCatalyst)
			fxRoots.Add (Path.Combine (Location, "System", "iOSSupport", "System", "Library", "Frameworks"));
		fxRoots.Add (Path.Combine (Location, "System", "Library", "Frameworks"));

		foreach (var fxRootDir in fxRoots) {
			foreach (var fxDir in Directory.EnumerateDirectories (fxRootDir, "*.framework")) {
				var fxName = Path.GetFileNameWithoutExtension (fxDir);
				if (frameworks.ContainsKey (fxName))
					continue;

				var moduleMap = Path.Combine (fxDir, "module.map");
				var headersDir = Path.Combine (fxDir, "Headers");
				var umbrellaHeader = Path.Combine (headersDir, GetUmbrellaHeader (fxName));
				if (Directory.Exists (headersDir) && (File.Exists (moduleMap) || File.Exists (umbrellaHeader))) {
					frameworks.Add (fxName, new Framework {
						Sdk = this,
						UmbrellaHeaderName = Path.GetFileNameWithoutExtension (umbrellaHeader),
						Name = fxName,
						ModuleMap = Path.Combine (fxDir, "Modules", "module.modulemap"),
						Location = fxDir
					});
					bindingResult.Log (1, $"Loaded {fxName} framework from {fxDir}");
				}
			}
		}
		return frameworks.Values.ToList ();
	}

	public Framework? GetFramework (string name)
	{
		return Frameworks.FirstOrDefault (f => f.Name == name);
	}

	List<string> LoadArchitectures ()
	{
		var architectures = new List<string> ();

		switch (Platform) {
		case "macosx":
			architectures.Add ("x86_64");
			architectures.Add ("arm64");
			break;
		case "iphonesimulator":
			architectures.Add ("x86_64");
			architectures.Add ("arm64");
			break;
		case "iphoneos":
			architectures.Add ("arm64");
			break;
		case "watchos":
			architectures.Add ("arm64");
			break;
		case "appletvos":
			architectures.Add ("arm64");
			break;
		default:
			throw new InvalidOperationException ($"Platform '{Platform}' is not supported");
		}

		return architectures;
	}

	public bool IsSupportedBySharpie {
		get {
			if (Version is null)
				return false;

			if (Architectures.Count <= 0)
				return false;

			if (Identifier.Contains ("driverkit"))
				return false;

			return true;
		}
	}
}
