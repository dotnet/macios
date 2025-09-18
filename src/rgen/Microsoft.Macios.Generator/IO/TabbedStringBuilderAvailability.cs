// Copyright (c) Microsoft Corporation.
// Licensed under the MIT License.

using System.IO;
using System.Threading.Tasks;
using Microsoft.Macios.Generator.Availability;
using Xamarin.Utils;

namespace Microsoft.Macios.Generator.IO;

static class TabbedStringBuilderAvailability {

	public static TabbedWriter<StringWriter> AppendMemberAvailability (this TabbedWriter<StringWriter> self, in SymbolAvailability allPlatformsAvailability)
	{
		foreach (var availability in allPlatformsAvailability.PlatformAvailabilities) {
			var platformName = availability.Platform.AsString ().ToLower ();
			if (availability.SupportedVersion is not null) {
				var versionStr = (PlatformAvailability.IsDefaultVersion (availability.SupportedVersion.Value.Version))
					? string.Empty
					: availability.SupportedVersion.Value.Version.ToString ();
				self.WriteLine ($"[SupportedOSPlatform (\"{platformName}{versionStr}\")]");
			}

			// loop over the unsupported versions of the platform 
			foreach (var (version, message) in availability.UnsupportedVersions) {
				var versionStr = (PlatformAvailability.IsDefaultVersion (version.Version)) ? string.Empty : version.Version.ToString ();
				if (message is null) {
					self.WriteLine ($"[UnsupportedOSPlatform (\"{platformName}{versionStr}\")]");
				} else {
					self.WriteLine ($"[UnsupportedOSPlatform (\"{platformName}{versionStr}\", \"{message}\")]");
				}
			}

			// loop over the obsolete versions of the platform 
			foreach (var (version, obsoleteInfo) in availability.ObsoletedVersions) {
				var versionStr = (PlatformAvailability.IsDefaultVersion (version)) ? string.Empty : version.ToString ();

				switch (obsoleteInfo) {
				case (null, null):
					self.WriteLine ($"[ObsoletedOSPlatform (\"{platformName}{versionStr}\")]");
					break;
				case (not null, null):
					self.WriteLine ($"[ObsoletedOSPlatform (\"{platformName}{versionStr}\", \"{obsoleteInfo.Message}\")]");
					break;
				case (null, not null):
					self.WriteLine ($"[ObsoletedOSPlatform (\"{platformName}{versionStr}\", Url=\"{obsoleteInfo.Url}\")]");
					break;
				case (not null, not null):
					self.WriteLine (
						$"[ObsoletedOSPlatform (\"{platformName}{versionStr}\", \"{obsoleteInfo.Message}\", Url=\"{obsoleteInfo.Url}\")]");
					break;
				}
			}
		}

		return self;
	}

	public static async Task<TabbedWriter<StreamWriter>> AppendMemberAvailabilityAsync (this TabbedWriter<StreamWriter> self, SymbolAvailability allPlatformsAvailability)
	{
		foreach (var availability in allPlatformsAvailability.PlatformAvailabilities) {
			var platformName = availability.Platform.AsString ().ToLower ();
			if (availability.SupportedVersion is not null) {
				var versionStr = (PlatformAvailability.IsDefaultVersion (availability.SupportedVersion.Value.Version))
					? string.Empty
					: availability.SupportedVersion.Value.Version.ToString ();
				await self.WriteLineAsync ($"[SupportedOSPlatform (\"{platformName}{versionStr}\")]");
			}

			// loop over the unsupported versions of the platform
			foreach (var (version, message) in availability.UnsupportedVersions) {
				var versionStr = (PlatformAvailability.IsDefaultVersion (version.Version)) ? string.Empty : version.Version.ToString ();
				if (message is null) {
					await self.WriteLineAsync ($"[UnsupportedOSPlatform (\"{platformName}{versionStr}\")]");
				} else {
					await self.WriteLineAsync ($"[UnsupportedOSPlatform (\"{platformName}{versionStr}\", \"{message}\")]");
				}
			}

			// loop over the obsolete versions of the platform
			foreach (var (version, obsoleteInfo) in availability.ObsoletedVersions) {
				var versionStr = (PlatformAvailability.IsDefaultVersion (version)) ? string.Empty : version.ToString ();

				switch (obsoleteInfo) {
				case (null, null):
					await self.WriteLineAsync ($"[ObsoletedOSPlatform (\"{platformName}{versionStr}\")]");
					break;
				case (not null, null):
					await self.WriteLineAsync ($"[ObsoletedOSPlatform (\"{platformName}{versionStr}\", \"{obsoleteInfo.Message}\")]");
					break;
				case (null, not null):
					await self.WriteLineAsync ($"[ObsoletedOSPlatform (\"{platformName}{versionStr}\", Url=\"{obsoleteInfo.Url}\")]");
					break;
				case (not null, not null):
					await self.WriteLineAsync (
						$"[ObsoletedOSPlatform (\"{platformName}{versionStr}\", \"{obsoleteInfo.Message}\", Url=\"{obsoleteInfo.Url}\")]");
					break;
				}
			}
		}

		return self;
	}
}
