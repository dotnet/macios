// arguments are:
//   --shorten long=short
//   --platform <platform> <version>
//   --windows-platform <platform>

using System.IO;
using System.Security.Cryptography;
using System.Text;
using System.Xml;

var shorten = new Dictionary<string, string> ();
var platforms = new List<(string, string)> ();
var windowsPlatforms = new List<string> ();
var tfm = string.Empty;
var xcodeName = string.Empty;
var xcodeVersion = string.Empty;
var outputPath = string.Empty;
var commitDistances = new Dictionary<string, int> ();
var queue = new Queue<string> (args);

while (queue.Any ()) {
	var arg = queue.Dequeue ();
	switch (arg) {
	case "--shorten":
		var values = queue.Dequeue ().Split ('=');
		shorten [values [0]] = values [1];
		break;
	case "--platform":
		var platform = queue.Dequeue ();
		var version = queue.Dequeue ();
		platforms.Add ((platform, version));
		break;
	case "--windows-platform":
		windowsPlatforms.Add (queue.Dequeue ());
		break;
	case "--output":
		outputPath = queue.Dequeue ();
		break;
	case "--tfm":
		tfm = queue.Dequeue ();
		break;
	case "--xcode":
		xcodeVersion = queue.Dequeue ();
		xcodeName = $"xcode{xcodeVersion}";
		break;
	case string s when s.StartsWith ("--commit-distance-"):
		var distance = queue.Dequeue ();
		var pl = s ["--commit-distance-".Length..];
		commitDistances [pl] = int.Parse (distance);
		break;
	default:
		Console.Error.WriteLine ($"Unknown argument: {arg}");
		return 1;
	}
}

using (TextWriter writer = new StreamWriter (outputPath)) {
	writer.WriteLine ($"<?xml version=\"1.0\" encoding=\"utf-8\"?>");
	writer.WriteLine ($"<Project>");
	writer.WriteLine ($"  <PropertyGroup>");
	var allPlatforms = string.Join (".", platforms.Select (v => v.Item1).OrderBy (v => v));
	writer.WriteLine ($"    <TargetName>{allPlatforms}.{tfm}.{xcodeName}</TargetName>");
	// Find the iOS version, otherwise use the version of the first platform listed.
	var iOSPlatform = platforms.Where (v => v.Item1 == "iOS");
	string manifestBuildVersion;
	if (iOSPlatform.Any ()) {
		manifestBuildVersion = iOSPlatform.First ().Item2;
	} else if (platforms.Any ()) {
		manifestBuildVersion = platforms.First ().Item2;
	} else {
		manifestBuildVersion = "0.0.1"; // happens when building only sharpie
	}
	writer.WriteLine ($"    <ManifestBuildVersion>{manifestBuildVersion}</ManifestBuildVersion>");
	writer.WriteLine ($"    <EnableSideBySideManifests>true</EnableSideBySideManifests>");
	writer.WriteLine ($"    <UseVisualStudioComponentPrefix>false</UseVisualStudioComponentPrefix>");
	writer.WriteLine ($"  </PropertyGroup>");
	writer.WriteLine ($"  <ItemGroup>");
	writer.WriteLine ($"    <!-- Shorten package names to avoid long path caching issues in Visual Studio -->");
	foreach (var entry in shorten) {
		var longName = entry.Key;
		var shortName = entry.Value;
		writer.WriteLine ($"    <ShortNames Include=\"{longName}\">");
		writer.WriteLine ($"      <Replacement>{shortName}</Replacement>");
		writer.WriteLine ($"    </ShortNames>");
	}
	foreach (var entry in platforms) {
		var platform = entry.Item1;
		var version = entry.Item2;
		var longPlatform = platform;
		var description = $".NET SDK Workload for building {platform} applications.";
		if (platform == "MacCatalyst") {
			longPlatform = "Mac Catalyst";
			description = ".NET SDK Workload for building macOS applications with Mac Catalyst.";
		}
		writer.WriteLine ($"    <ComponentResources Include=\"{platform.ToLower ()}\" Version=\"{version}\" Category=\".NET\" Title=\".NET SDK for {longPlatform}\" Description=\"{description}\"/>");
	}
	foreach (var entry in platforms) {
		var platform = entry.Item1;
		var version = entry.Item2;
		if (Version.Parse (tfm.Replace ("net", "")).Major == 10) {
			// Due to a bug, we've inserted packages with wildly incorrect (and random) MSI versions into VS.
			// This poses a problem, for a couple of reasons:
			// * We can't insert a new package with an MSI version lower than the existing one.
			// * The incorrect MSI version is rather close to the maximum version number - the major part is already there (255), the minor version is close (220).
			// So implement a custom version scheme for .NET 10 (in .NET 11 the inserted
			// package name will be different, so we can use any version number we want,
			// so we'll use the correct one).
			//
			// The scheme keeps the major fixed at 255 and uses the commit distance as the build component, and
			// encodes the Xcode version into the minor component so the MSI version increases monotonically every
			// time the Xcode major/minor increases (a newer Xcode must produce a higher MSI version), while staying
			// just above the last bad version (255.220.39248) and below the MSI minor ceiling of 255.
			//
			// The minor bump is computed from the Xcode version as:
			//     minorVersionBump = (Xcode major - 26) * 10 + Xcode minor
			// Xcode 26 is the first Xcode that uses Apple's year-based version numbers, so subtracting 26 gives 0
			// for the 26.x series, 1 for 27.x, 2 for 28.x, etc. Multiplying by 10 and adding the (single-digit)
			// Xcode minor yields a value that strictly increases with the Xcode version:
			//     26.5 => 5 (MSI minor 225), 27.0 => 10 (230), 27.5 => 15 (235), 28.0 => 20 (240), ...
			// The final MSI minor is 220 + minorVersionBump. For Xcode 27.x and 28.x (and any 26.x with a non-zero
			// minor) this is > 220, so the version exceeds the 255.220.39248 baseline on the minor component alone,
			// regardless of the build component (only Xcode 26.0 would map to exactly 220, where the build component
			// would instead have to exceed 39248). The minor stays <= 255 across .NET 10's supported Xcode range
			// (26-28; 28.9 => 249) and would first overflow the 255 ceiling at Xcode 29.6 (=> 256), which is one
			// reason this scheme is .NET-10-only. This relies on the Xcode minor being a single digit, which has
			// always held.
			//
			// Note: this was previously written as (26 - Xcode major), which is negative for Xcode 27 and later
			// and threw in the System.Version constructor (it rejects negative components), breaking the build.
			var minimumVersion = new Version (255, 220, 39248); // This is the last incorrect MSI version, we need to produce something higher than this.
			var minorVersionBump = (Version.Parse (xcodeVersion).Major - 26) * 10 + Version.Parse (xcodeVersion).Minor;
			// just use the commit distance for the build version, our minor version will be higher than the minimum version, so we can use any build version.
			var buildVersionBump = commitDistances [platform];
			var msiVersion = new Version (minimumVersion.Major, minimumVersion.Minor + minorVersionBump, buildVersionBump);
			writer.WriteLine ($"    <WorkloadPackages Include=\"$(NuGetPackagePath)\\Microsoft.NET.Sdk.{platform}.Manifest*.nupkg\" Version=\"{version}.0\" MsiVersion=\"{msiVersion}\" SupportsMachineArch=\"true\" />");
		} else {
			writer.WriteLine ($"    <WorkloadPackages Include=\"$(NuGetPackagePath)\\Microsoft.NET.Sdk.{platform}.Manifest*.nupkg\" Version=\"{version}.0\" MsiVersion=\"{version}\" SupportsMachineArch=\"true\" />");
		}
	}
	writer.WriteLine ("  </ItemGroup>");
	writer.WriteLine ("</Project>");
}

return 0;
