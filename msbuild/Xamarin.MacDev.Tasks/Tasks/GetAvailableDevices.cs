using System;
using System.IO;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json;
using System.Threading;

using Microsoft.Build.Framework;
using Microsoft.Build.Utilities;

using Xamarin.Localization.MSBuild;
using Xamarin.Messaging.Build.Client;
using Xamarin.Utils;

namespace Xamarin.MacDev.Tasks;

public class GetAvailableDevices : XamarinTask, ICancelableTask {
	// The app manifest is optional, we won't compile it if it hasn't already been compiled,
	// but we'll read it if it exist (in the app bundle).
	public string AppBundleManifestPath { get; set; } = string.Empty;

	[Output]
	public ITaskItem [] Devices { get; set; } = Array.Empty<ITaskItem> ();

	[Output]
	public ITaskItem [] DiscardedDevices { get; set; } = Array.Empty<ITaskItem> ();

	public string RuntimeIdentifier { get; set; } = "";

	public bool Verbose { get; set; }

	CancellationTokenSource? cancellationTokenSource;

	public override bool Execute ()
	{
		if (ShouldExecuteRemotely ())
			return ExecuteRemotely ();

		cancellationTokenSource = new CancellationTokenSource ();

		// run devicectl and simctl in parallel
		var devicectlTask = RunDeviceCtlAsync ();
		var simctlTask = RunSimCtlAsync ();

		System.Threading.Tasks.Task.WhenAll (new [] { devicectlTask, simctlTask }).Wait (cancellationTokenSource.Token);

		var devices = new List<DeviceInfo> ();
		devices.AddRange (devicectlTask.Result);
		devices.AddRange (simctlTask.Result);

		// filter to the current platform
		foreach (var d in devices.Where (d => !d.Discarded && d.Platform != Platform))
			d.DiscardedReason = $"Device platform '{d.Platform.AsString ()}' does not match the requested platform '{Platform.AsString ()}'";

		// filter to the current RuntimeIdentifier, if specified
		if (!string.IsNullOrEmpty (RuntimeIdentifier)) {
			foreach (var d in devices.Where (d => string.IsNullOrEmpty (d.DiscardedReason) && d.RuntimeIdentifiers.Any () && !d.RuntimeIdentifiers.Contains (RuntimeIdentifier)))
				d.DiscardedReason = $"Device runtime identifier(s) '{string.Join ("', '", d.RuntimeIdentifiers)}' incompatible with the requested runtime identifier '{RuntimeIdentifier}'";
		}

		// filter to values we find in the app manifest, if it exists
		if (File.Exists (AppBundleManifestPath)) {
			var appManifest = PDictionary.FromFile (AppBundleManifestPath)!;

			var uiDeviceFamily = appManifest.GetUIDeviceFamily ();
			// an iPhone app can run on an iPad, but an iPad app cannot run on an iPhone
			var onlyShowIPads = uiDeviceFamily == IPhoneDeviceType.IPad;
			if (onlyShowIPads) {
				foreach (var d in devices.Where (d => !d.Discarded && d.DeviceType != IPhoneDeviceType.IPad))
					d.DiscardedReason = "Device is not an iPad, but the app only supports iPads";
			}

			string minimumOSVersionString;
			switch (Platform) {
			case ApplePlatform.iOS:
			case ApplePlatform.TVOS:
				minimumOSVersionString = appManifest.GetMinimumOSVersion ();
				break;
			case ApplePlatform.MacCatalyst:
			case ApplePlatform.MacOSX:
				minimumOSVersionString = appManifest.GetMinimumSystemVersion ();
				break;
			default:
				throw new InvalidOperationException (string.Format (MSBStrings.InvalidPlatform, Platform));
			}
			if (Version.TryParse (minimumOSVersionString, out var minimumOSVersion)) {
				foreach (var d in devices.Where (d => !d.Discarded && d.MinimumOSVersion < minimumOSVersion))
					d.DiscardedReason = $"Device OS version '{d.MinimumOSVersion}' is lower than the app's minimum OS version '{minimumOSVersion}'";
				foreach (var d in devices.Where (d => !d.Discarded && d.MaximumOSVersion < minimumOSVersion))
					d.DiscardedReason = $"Device maximum OS version '{d.MaximumOSVersion}' is lower than the app's minimum OS version '{minimumOSVersion}'";
			}
		}

		// Set the 'RuntimeIdentifier' metadata on every device
		foreach (var d in devices) {
			if (!d.RuntimeIdentifiers.Any ())
				continue;
			if (d.RuntimeIdentifiers.Count () == 1) {
				d.Item.SetMetadata ("RuntimeIdentifier", d.RuntimeIdentifiers.First ());
				continue;
			}
			// if we have multiple runtime identifiers, we're running in the simulator, and one is x64 and the other is arm64.
			// if we can run on arm64, then pick the arm64 simulator, otherwise pick the x64 simulator
			d.Item.SetMetadata ("RuntimeIdentifier", d.RuntimeIdentifiers.Single (v => v.Contains ("arm64") == CanRunArm64));
		}

		DiscardedDevices = devices.Where (d => d.Discarded).Select (v => {
			v.Item.SetMetadata ("DiscardedReason", v.DiscardedReason);
			return v.Item;
		}).ToArray ();

		// sort the devices, so we return them in a stable order
		var sortedDevices = devices
			.Where (d => !d.Discarded) // discard discarded devices
			.OrderByDescending (d => d.MinimumOSVersion) // newer devices first (probably has duplicates)
			.ThenBy (d => d.Item.GetMetadata ("Name")) // then sort by name (may have duplicates)
			.ThenBy (d => d.Item.ItemSpec); // and finally by UDID, which should be unique

		Devices = sortedDevices.Select (d => d.Item).ToArray ();

		if (Verbose) {
			foreach (var dd in DiscardedDevices)
				Log.LogWarning ("Discarded '{0}' because: {1}", dd.ItemSpec, dd.GetMetadata ("DiscardedReason"));
		}

		return true;
	}

	class DeviceInfo {
		public ITaskItem Item { get; set; }
		public IEnumerable<string> RuntimeIdentifiers { get; set; }
		public ApplePlatform Platform { get; set; }
		public IPhoneDeviceType DeviceType { get; set; }
		public Version MinimumOSVersion { get; set; }
		public Version MaximumOSVersion { get; set; }
		public string DiscardedReason { get; set; }
		public bool Discarded { get => !string.IsNullOrEmpty (DiscardedReason); }
		public DeviceInfo (ITaskItem item, IEnumerable<string> runtimeIdentifiers, ApplePlatform platform, IPhoneDeviceType deviceType, Version minimumOSVersion, Version maximumOSVersion, string discardedReason)
		{
			Item = item;
			RuntimeIdentifiers = runtimeIdentifiers;
			Platform = platform;
			DeviceType = deviceType;
			MinimumOSVersion = minimumOSVersion;
			MaximumOSVersion = maximumOSVersion;
			DiscardedReason = discardedReason;
		}
	}

	protected virtual async System.Threading.Tasks.Task<string> ExecuteCtlAsync (params string [] args)
	{
		var tmpfile = Path.GetTempFileName ();
		try {
			var arguments = new List<string> (args) {
				"--json-output=" + tmpfile
			};
			await ExecuteAsync ("xcrun", arguments, cancellationToken: cancellationTokenSource!.Token);
			return File.ReadAllText (tmpfile);
		} finally {
			File.Delete (tmpfile);
		}
	}

	async System.Threading.Tasks.Task<JsonDocument> ExecuteCtlToJsonAsync (params string [] args)
	{
		var json = await ExecuteCtlAsync (args);
		var options = new JsonDocumentOptions {
			AllowTrailingCommas = true,
			CommentHandling = JsonCommentHandling.Skip,
		};
		return JsonDocument.Parse (string.IsNullOrEmpty (json) ? "{}" : json, options);
	}

	async System.Threading.Tasks.Task<IEnumerable<DeviceInfo>> RunDeviceCtlAsync ()
	{
		var doc = await ExecuteCtlToJsonAsync ("devicectl", "list", "devices");
		var array = doc.FindProperty ("result", "devices")?.EnumerateIfArray ();
		var rv = new List<DeviceInfo> ();
		if (array is not null) {
			foreach (var device in array) {
				var name = device.GetStringPropertyOrEmpty ("deviceProperties", "name");
				var udid = device.GetStringPropertyOrEmpty ("hardwareProperties", "udid");

				var deviceProperties = device.GetNullableProperty ("deviceProperties");
				var buildVersion = deviceProperties.GetStringPropertyOrEmpty ("osBuildUpdate");
				var productVersion = deviceProperties.GetStringPropertyOrEmpty ("osVersionNumber");

				var hardwareProperties = device.GetNullableProperty ("hardwareProperties");
				var deviceClass = hardwareProperties.GetStringPropertyOrEmpty ("deviceType");
				var hardwareModel = hardwareProperties.GetStringPropertyOrEmpty ("hardwareModel");
				var hardwarePlatform = hardwareProperties.GetStringPropertyOrEmpty ("platform");
				var productType = hardwareProperties.GetStringPropertyOrEmpty ("productType");
				var serialNumber = hardwareProperties.GetStringPropertyOrEmpty ("serialNumber");
				var uniqueChipID = hardwareProperties.GetUInt64Property ("ecid");

				var cpuType = hardwareProperties.GetNullableProperty ("cpuType");
				var cpuArchitecture = cpuType.GetStringPropertyOrEmpty ("name");

				var connectionProperties = device.GetNullableProperty ("connectionProperties");
				var transportType = connectionProperties.GetStringPropertyOrEmpty ("transportType");
				var pairingState = connectionProperties.GetStringPropertyOrEmpty ("pairingState");

				var item = new TaskItem (udid);
				item.SetMetadata ("Name", name);
				item.SetMetadata ("BuildVersion", buildVersion);
				item.SetMetadata ("DeviceClass", deviceClass);
				item.SetMetadata ("HardwareModel", hardwareModel);
				item.SetMetadata ("Platform", hardwarePlatform);
				item.SetMetadata ("ProductType", productType);
				item.SetMetadata ("SerialNumber", serialNumber);
				item.SetMetadata ("UniqueChipID", uniqueChipID?.ToString () ?? string.Empty);
				item.SetMetadata ("CPUArchitecture", cpuArchitecture);
				item.SetMetadata ("TransportType", transportType);
				item.SetMetadata ("PairingState", pairingState);

				// we provide the following metadata for both simulator and device
				item.SetMetadata ("Description", name);
				item.SetMetadata ("Type", "Device");
				item.SetMetadata ("OSVersion", productVersion);
				item.SetMetadata ("UDID", udid);

				// compute the platform and runtime identifier
				var runtimeIdentifier = "";
				ApplePlatform platform;
				IPhoneDeviceType deviceType;
				var discardedReason = "";
				switch (deviceClass.ToLowerInvariant ()) {
				case "iphone":
				case "ipod":
					runtimeIdentifier += "ios-";
					platform = ApplePlatform.iOS;
					deviceType = IPhoneDeviceType.IPhone;
					break;
				case "ipad":
					runtimeIdentifier += "ios-";
					platform = ApplePlatform.iOS;
					deviceType = IPhoneDeviceType.IPad;
					break;
				case "appletv":
					runtimeIdentifier += "tvos-";
					platform = ApplePlatform.TVOS;
					deviceType = IPhoneDeviceType.TV;
					break;
				case "applewatch":
				case "visionos":
				default:
					platform = ApplePlatform.None;
					deviceType = IPhoneDeviceType.NotSet;
					discardedReason = $"'{deviceClass}' devices are not supported";
					break;
				}

				if (string.IsNullOrEmpty (discardedReason)) {
					switch (cpuArchitecture.ToLowerInvariant ()) {
					case "arm64":
					case "arm64e":
						// arm64 and arm64e are both arm64 for our purposes
						runtimeIdentifier += "arm64";
						break;
					default:
						discardedReason = $"Unknown CPU architecture '{cpuArchitecture}'";
						break;
					}
				}

				Version.TryParse (productVersion, out var minimumOSVersion);
				var maximumOSVersion = new Version (65535, 255, 255);

				rv.Add (new DeviceInfo (item, [runtimeIdentifier], platform, deviceType, minimumOSVersion ?? new Version (0, 0), maximumOSVersion, discardedReason));
			}
		}
		return rv;
	}

	async System.Threading.Tasks.Task<IEnumerable<DeviceInfo>> RunSimCtlAsync ()
	{
		var doc = await ExecuteCtlToJsonAsync ("simctl", "list", "--json");
		var rv = new List<DeviceInfo> ();

		var runtimes = new Dictionary<string, JsonElement> ();
		if (doc.TryGetProperty ("runtimes", out var runtimesElement)) {
			foreach (var runtime in runtimesElement.EnumerateIfArray ()) {
				var name = runtime.GetStringProperty ("identifier") ?? string.Empty;
				runtimes [name] = runtime;
			}
		}

		var deviceTypes = new Dictionary<string, JsonElement> ();
		if (doc.TryGetProperty ("devicetypes", out var deviceTypesElement)) {
			foreach (var deviceType in deviceTypesElement.EnumerateIfArray ()) {
				var name = deviceType.GetStringProperty ("identifier") ?? string.Empty;
				deviceTypes [name] = deviceType;
			}
		}

		if (doc.TryGetProperty ("devices", out var devicesElement)) {
			foreach (var runtime in devicesElement.EnumerateObject ()) {
				var runtimeName = runtime.Name;
				var hasRuntime = runtimes.TryGetValue (runtimeName, out var runtimeElement);
				var runtimePlatform = hasRuntime ? runtimeElement.GetStringProperty ("platform") ?? string.Empty : string.Empty;
				var runtimeVersion = hasRuntime ? runtimeElement.GetStringProperty ("version") ?? string.Empty : string.Empty;
				var supportedArchitectures = hasRuntime ? runtimeElement.GetProperty ("supportedArchitectures").EnumerateIfArray ().Select (v => v.GetString () ?? "") : Enumerable.Empty<string> ();
				foreach (var element in runtime.Value.EnumerateIfArray ()) {
					var udid = element.GetStringProperty ("udid") ?? string.Empty;
					var isAvailable = element.GetBooleanProperty ("isAvailable") ?? false;
					var availabilityError = element.GetStringProperty ("availabilityError") ?? string.Empty;
					var deviceTypeIdentifier = element.GetStringProperty ("deviceTypeIdentifier") ?? string.Empty;
					var state = element.GetStringProperty ("state") ?? string.Empty;
					var name = element.GetStringProperty ("name") ?? string.Empty;

					var item = new TaskItem (udid);
					item.SetMetadata ("Runtime", runtimeName);
					item.SetMetadata ("IsAvailable", isAvailable.ToString ());
					item.SetMetadata ("AvailabilityError", availabilityError);
					item.SetMetadata ("DeviceTypeIdentifier", deviceTypeIdentifier);
					item.SetMetadata ("State", state);
					item.SetMetadata ("Name", name);
					item.SetMetadata ("SupportedArchitectures", string.Join (",", supportedArchitectures));

					// we provide the following metadata for both simulator and device
					item.SetMetadata ("Description", name);
					item.SetMetadata ("Type", "Simulator");
					item.SetMetadata ("OSVersion", runtimeVersion);
					item.SetMetadata ("UDID", udid);

					var discardedReason = "";
					var runtimeIdentifier = "";
					var runtimeIdentifiers = new List<string> ();
					if (isAvailable) {
						switch (runtimePlatform.ToLowerInvariant ()) {
						case "ios":
							runtimeIdentifier += "iossimulator-";
							break;
						case "tvos":
							runtimeIdentifier += "tvossimulator-";
							break;
						default:
							discardedReason = $"'{runtimePlatform}' simulators are not supported";
							break;
						}

						// pick the first architecture as the simulator architecture
						if (string.IsNullOrEmpty (discardedReason)) {
							foreach (var arch in supportedArchitectures) {
								switch (arch.ToLowerInvariant ()) {
								case "x64":
								case "x86_64":
									runtimeIdentifiers.Add (runtimeIdentifier + "x64");
									break;
								case "arm64":
									runtimeIdentifiers.Add (runtimeIdentifier + "arm64");
									if (!CanRunArm64)
										discardedReason = $"Can't run an arm64 simulator on an x86_64 macOS desktop.";
									break;
								default:
									discardedReason = $"Unknown CPU architecture '{arch}'";
									break;
								}
							}
						}
					} else {
						discardedReason = $"Device is not available: {availabilityError}";
					}

					var platformName = runtimeName.Replace ("com.apple.CoreSimulator.SimRuntime.", "").Split ('-') [0];
					var platform = ApplePlatform.None;
					if (string.IsNullOrEmpty (discardedReason)) {
						switch (platformName.ToLowerInvariant ()) {
						case "ios":
							platform = ApplePlatform.iOS;
							break;
						case "tvos":
							platform = ApplePlatform.TVOS;
							break;
						case "watchos":
						case "visionos":
						default:
							discardedReason = $"'{platformName}' simulators are not supported";
							break;
						}
					}
					var deviceType = IPhoneDeviceType.NotSet;
					var minimumOSVersion = new Version (0, 0);
					var maximumOSVersion = new Version (65535, 255, 255);
					if (string.IsNullOrEmpty (discardedReason)) {
						if (deviceTypes.TryGetValue (deviceTypeIdentifier, out var deviceTypeElement)) {
							var productFamily = deviceTypeElement.GetStringProperty ("productFamily") ?? string.Empty;
							switch (productFamily.ToLowerInvariant ()) {
							case "iphone":
							case "ipod":
								deviceType = IPhoneDeviceType.IPhone;
								break;
							case "ipad":
								deviceType = IPhoneDeviceType.IPad;
								break;
							case "appletv":
							case "apple tv":
								deviceType = IPhoneDeviceType.TV;
								break;
							default:
								discardedReason = $"Unknown product family '{productFamily}'";
								break;
							}
							if (Version.TryParse (deviceTypeElement.GetStringProperty ("minRuntimeVersionString"), out var parsedMinimumOSVersion))
								minimumOSVersion = parsedMinimumOSVersion;
							if (Version.TryParse (deviceTypeElement.GetStringProperty ("maxRuntimeVersionString"), out var parsedMaximumOSVersion))
								maximumOSVersion = parsedMaximumOSVersion;
						} else {
							discardedReason = $"Unknown device type identifier '{deviceTypeIdentifier}'";
						}
					}

					rv.Add (new DeviceInfo (item, runtimeIdentifiers, platform, deviceType, minimumOSVersion, maximumOSVersion, discardedReason));
				}
			}
		}
		return rv;
	}

	public void Cancel ()
	{
		if (ShouldExecuteRemotely ())
			BuildConnection.CancelAsync (BuildEngine4).Wait ();

		cancellationTokenSource?.Cancel ();
	}

	// Return true if the current machine can run ARM64 binaries.
	static bool? canRunArm64;
	public static bool CanRunArm64 {
		get {
			if (!canRunArm64.HasValue) {
				int rv = 0;
				IntPtr size = (IntPtr) sizeof (int);
				if (sysctlbyname ("hw.optional.arm64", ref rv, ref size, IntPtr.Zero, IntPtr.Zero) == 0) {
					canRunArm64 = rv == 1;
				} else {
					canRunArm64 = false;
				}
			}
			return canRunArm64.Value;
		}
	}

	[DllImport ("libc")]
	static extern int sysctlbyname (string name, ref int value, ref IntPtr size, IntPtr zero, IntPtr zeroAgain);
}
