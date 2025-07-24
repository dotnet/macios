// arguments are: <platform> <outputPath>

using System.IO;
using System.Xml;

var expectedArgumentCount = 6;
if (args.Length != expectedArgumentCount) {
	Console.WriteLine ($"Need {expectedArgumentCount} arguments, got {args.Length}");
	return 1;
}

var argumentIndex = 0;
var platform = args [argumentIndex++];
var outputPath = args [argumentIndex++];
var windowsPlatforms = args [argumentIndex++].Split (new char [] { ' ' }, StringSplitOptions.RemoveEmptyEntries);
var hasWindows = Array.IndexOf (windowsPlatforms, platform) >= 0;
var currentApiVersion = args [argumentIndex++];
var supportedApiVersions = args [argumentIndex++].Split (' ');
var betaApiVersions = args [argumentIndex++].Split (' ');

var platformLowerCase = platform.ToLowerInvariant ();

var supportedTFMs = new List<string> ();
supportedTFMs.AddRange (supportedApiVersions.Select (v => v.Replace ('-', '_')));
supportedTFMs.Sort ();

var betaTFMs = betaApiVersions.Select (v => v.Replace ('-', '_')).OrderBy (v => v).ToList ();

var supportedTFVs = new List<string> ();

var tfmToTpvAndTfv = new Func<string, (string Tfv, string Tpv)> (tfm => {
	var tfv = tfm.Replace ("net", "");
	var sep = tfv.IndexOfAny (new char [] { '-', '_' });
	var tpv = "";
	if (sep >= 0) {
		tpv = tfv.Substring (sep + 1);
		tfv = tfv.Substring (0, sep);
	}
	return (tfv, tpv);
});

// Validate that every beta api version must also be a supported api version
var unsupportedBetaVersions = betaApiVersions.Except (supportedApiVersions);
if (unsupportedBetaVersions.Any ()) {
	Console.WriteLine ($"The variable BETA_API_VERSIONS_{platform.ToUpperInvariant ()} contains values not in SUPPORTED_API_VERSIONS_{platform.ToUpperInvariant ()}: {string.Join (", ", unsupportedBetaVersions)}");
	return 1;
}

// Find the latest TFM for each major .NET version, except beta versions.
// We import the workload for this TFM if there's no TPV specified in the TargetFramework.
var groupedByMajorDotNetVersion = supportedTFMs.
										Except (betaTFMs).
										Where (v => v.IndexOfAny (new char [] { '-', '_' }) >= 0).
										GroupBy (v => v.Split (new char [] { '-', '_' }) [0]);
var highestTpvPerMajorDotNet = groupedByMajorDotNetVersion.
			Select (gr => {
				var max = gr.OrderByDescending (el => {
					var rv = tfmToTpvAndTfv (el);
					return float.Parse (rv.Tpv, System.Globalization.CultureInfo.InvariantCulture);
				}).First ();
				return max;
			}).
			ToHashSet ();

using (var writer = new StreamWriter (outputPath)) {
	writer.WriteLine ($"<Project>");
	writer.WriteLine ($"	<!-- Load the correct workload depending on the TargetPlatformVersion -->");
	foreach (var tfm in supportedTFMs) {
		var parsed = tfmToTpvAndTfv (tfm);
		var tfv = parsed.Tfv;
		var tpv = parsed.Tpv;
		supportedTFVs.Add (tfv);
		var workloadVersion = tfm;
		writer.WriteLine ($"	<ImportGroup Condition=\" '$(TargetPlatformIdentifier)' == '{platform}' And '$(UsingAppleNETSdk)' != 'true' And $([MSBuild]::VersionEquals($(TargetFrameworkVersion), '{tfv}')) And '$(TargetPlatformVersion)' == '{tpv}'\">");
		writer.WriteLine ($"		<Import Project=\"Sdk.props\" Sdk=\"Microsoft.{platform}.Sdk.{workloadVersion}\" />");
		if (hasWindows)
			writer.WriteLine ($"		<Import Project=\"Sdk.props\" Sdk=\"Microsoft.{platform}.Windows.Sdk.Aliased.{tfm}\" Condition=\" $([MSBuild]::IsOSPlatform('windows'))\" />");
		writer.WriteLine ($"	</ImportGroup>");
		writer.WriteLine ();
	}

	writer.WriteLine ($"	<!-- If no TargetPlatformVersion is specified, load a default workload depending on the target framework version, and that workload will validate the TargetPlatformVersion value and show an error if applicable -->");
	foreach (var tfm in highestTpvPerMajorDotNet) {
		var parsed = tfmToTpvAndTfv (tfm);
		var tfv = parsed.Tfv;
		var tpv = parsed.Tpv;
		var workloadVersion = tfm;

		writer.WriteLine ($"	<ImportGroup Condition=\" '$(TargetPlatformIdentifier)' == '{platform}' And '$(UsingAppleNETSdk)' != 'true' And $([MSBuild]::VersionEquals($(TargetFrameworkVersion), '{tfv}'))\">");
		writer.WriteLine ($"		<Import Project=\"Sdk.props\" Sdk=\"Microsoft.{platform}.Sdk.{workloadVersion}\" />");
		if (hasWindows)
			writer.WriteLine ($"		<Import Project=\"Sdk.props\" Sdk=\"Microsoft.{platform}.Windows.Sdk.Aliased.{tfm}\" Condition=\" $([MSBuild]::IsOSPlatform('windows'))\" />");
		writer.WriteLine ($"	</ImportGroup>");
		writer.WriteLine ();
	}

	writer.WriteLine ($"	<!-- Detect if the target framework version is outside our supported range, and show the corresponding error -->");
	var earliestSupportedTFV = supportedTFVs.Select (v => Version.Parse (v)).OrderBy (v => v).First ();
	var latestSupportedTFV = supportedTFVs.Select (v => Version.Parse (v)).OrderBy (v => v).Last ();
	writer.WriteLine ($"	<ImportGroup Condition=\" '$(TargetPlatformIdentifier)' == '{platform}' And '$(UsingAppleNETSdk)' != 'true'\">");
	writer.WriteLine ($"		<Import Project=\"Sdk-eol.props\" Sdk=\"Microsoft.{platform}.Sdk.{currentApiVersion}\" Condition=\" $([MSBuild]::VersionLessThan($(TargetFrameworkVersion), '{earliestSupportedTFV}'))\" />");
	writer.WriteLine ($"		<Import Project=\"Sdk.props\" Sdk=\"Microsoft.{platform}.Sdk.{currentApiVersion}\" Condition=\" $([MSBuild]::VersionGreaterThan($(TargetFrameworkVersion), '{latestSupportedTFV}'))\" />");
	writer.WriteLine ($"	</ImportGroup>");
	writer.WriteLine ();
	writer.WriteLine ($"	<ItemGroup Condition=\" '$(TargetFrameworkIdentifier)' == '.NETCoreApp' and $([MSBuild]::VersionGreaterThanOrEquals($(TargetFrameworkVersion), '6.0')) \">");
	writer.WriteLine ($"		<SdkSupportedTargetPlatformIdentifier Include=\"{platformLowerCase}\" DisplayName=\"{platform}\" />");
	writer.WriteLine ($"	</ItemGroup>");

	writer.WriteLine ($"</Project>");
	writer.WriteLine ();
}

return 0;
