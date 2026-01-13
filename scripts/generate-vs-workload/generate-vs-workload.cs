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
	var manifestBuildVersion = iOSPlatform.Any () ? iOSPlatform.First ().Item2 : platforms.First ().Item2;
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
			var minimumVersion = new Version (255, 220, 39248); // This is the last incorrect MSI version, we need to produce something higher than this.
																// bump minor version according to current Xcode version:
																// minor = minimumVersion.Minor + (26 - Major Xcode version) * 10 + (Minor Xcode version)
			var minorVersionBump = int.Parse (new Version (26 - Version.Parse (xcodeVersion).Major, Version.Parse (xcodeVersion).Minor).ToString ().Replace (".", ""));
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
