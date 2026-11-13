using System;
using System.IO;

using Microsoft.Build.Utilities;
using Microsoft.Build.Framework;

using Xamarin.MacDev;
using Xamarin.MacDev.Tasks;
using Xamarin.Utils;
using Xamarin.Localization.MSBuild;

#nullable enable

namespace Xamarin.MacDev.Tasks {
	public class ParseDeviceSpecificBuildInformation : XamarinTask {
		#region Inputs

		[Required]
		public string Architectures { get; set; } = "";

		[Required]
		public string IntermediateOutputPath { get; set; } = "";

		[Required]
		public string OutputPath { get; set; } = "";

		[Required]
		public string TargetiOSDevice { get; set; } = "";

		#endregion

		#region Outputs

		[Output]
		public string DeviceSpecificIntermediateOutputPath { get; set; } = "";

		[Output]
		public string DeviceSpecificOutputPath { get; set; } = "";

		[Output]
		public string TargetArchitectures { get; set; } = "";

		[Output]
		public string TargetDeviceModel { get; set; } = "";

		[Output]
		public string TargetDeviceOSVersion { get; set; } = "";

		#endregion

		public override bool Execute ()
		{
			var target = TargetArchitecture.Default;
			string targetOperatingSystem;

			switch (Platform) {
			case ApplePlatform.TVOS:
				targetOperatingSystem = "tvOS";
				break;
			case ApplePlatform.iOS:
				targetOperatingSystem = "iOS";
				break;
			default:
				throw new InvalidOperationException (string.Format (MSBStrings.InvalidPlatform, Platform));
			}

			if (!Enum.TryParse<TargetArchitecture> (Architectures, out var architectures)) {
				Log.LogError (57, null, MSBStrings.E0057, Architectures); // Invalid architectures: '{0}'.
				return false;
			}

			var plist = PObject.FromString (TargetiOSDevice) as PDictionary;
			if (plist is null) {
				Log.LogError (58, null, MSBStrings.E0058); // Failed to parse the target device information.
				return false;
			}

			if (!plist.TryGetValue<PDictionary> ("device", out var device)) {
				Log.LogError (59, null, MSBStrings.E0059); // No target device found.
				return false;
			}

			if (!device.TryGetValue<PString> ("architecture", out var value)) {
				Log.LogError (60, null, MSBStrings.E0060); // No device architecture information found.
				return false;
			}

			if (!Enum.TryParse<TargetArchitecture> (value.Value, out var deviceArchitectures) || deviceArchitectures == TargetArchitecture.Default) {
				Log.LogError (61, null, MSBStrings.E0061, value.Value); // Invalid target architecture: '{0}'
				return false;
			}

			if (!device.TryGetValue<PString> ("os", out var os)) {
				Log.LogError (62, null, MSBStrings.E0062); // No device operating system information found.
				return false;
			}

			if (os.Value != targetOperatingSystem || (architectures & deviceArchitectures) == 0) {
				// the TargetiOSDevice property conflicts with the build configuration (*.user file?), do not build this project for a specific device
				DeviceSpecificIntermediateOutputPath = IntermediateOutputPath;
				DeviceSpecificOutputPath = OutputPath;
				TargetArchitectures = Architectures;
				TargetDeviceOSVersion = string.Empty;
				TargetDeviceModel = string.Empty;

				return !Log.HasLoggedErrors;
			}

			for (int bit = 0; bit < 32; bit++) {
				var architecture = (TargetArchitecture) (1 << bit);

				if ((architectures & architecture) == 0)
					continue;

				if ((deviceArchitectures & architecture) != 0)
					target = architecture;
			}

			TargetArchitectures = target.ToString ();

			if (!device.TryGetValue ("model", out value)) {
				Log.LogError (63, null, MSBStrings.E0063); // No device model information found.
				return false;
			}

			TargetDeviceModel = value.Value;

			if (!device.TryGetValue ("os-version", out value)) {
				Log.LogError (64, null, MSBStrings.E0064); // No iOS version information found.
				return false;
			}

			TargetDeviceOSVersion = value.Value;

			// Note: we replace ',' with '.' because the ',' breaks the Mono AOT compiler which tries to treat arguments with ','s in them as options.
			var dirName = TargetDeviceModel.ToLowerInvariant ().Replace (",", ".") + "-" + TargetDeviceOSVersion;

			DeviceSpecificIntermediateOutputPath = Path.Combine (IntermediateOutputPath, "device-builds", dirName) + "/";
			DeviceSpecificOutputPath = Path.Combine (OutputPath, "device-builds", dirName) + "/";

			return !Log.HasLoggedErrors;
		}
	}
}
