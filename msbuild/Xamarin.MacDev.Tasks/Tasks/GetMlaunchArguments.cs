using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Xml;

using Microsoft.Build.Framework;

using Xamarin.Localization.MSBuild;
using Xamarin.MacDev;
using Xamarin.MacDev.Tasks;
using Xamarin.Messaging.Build.Client;
using Xamarin.Utils;

#nullable enable

namespace Xamarin.MacDev.Tasks {
	public class GetMlaunchArguments : XamarinTask, ICancelableTask {

		[Required]
		public bool SdkIsSimulator { get; set; }

		[Required]
		public string SdkVersion { get; set; } = string.Empty;

		public string AppManifestPath { get; set; } = string.Empty;

		[Required]
		public string SdkDevPath { get; set; } = string.Empty;

		public ITaskItem [] AdditionalArguments { get; set; } = Array.Empty<ITaskItem> ();
		public string DeviceName { get; set; } = string.Empty;
		public ITaskItem [] EnvironmentVariables { get; set; } = Array.Empty<ITaskItem> ();
		public string Help { get; set; } = string.Empty;
		public string LaunchApp { get; set; } = string.Empty;
		public string InstallApp { get; set; } = string.Empty;
		public bool CaptureOutput { get; set; } // Set to true to capture output. If StandardOutput|ErrorPath is not set, write to the current terminal's stdout/stderr (requires WaitForExit)
		public string StandardOutputPath { get; set; } = string.Empty; // Set to a path to capture output there
		public string StandardErrorPath { get; set; } = string.Empty;// Set to a path to capture output there
		public string SupportedOSPlatformVersion { get; set; } = string.Empty;
		public bool WaitForExit { get; set; } // Required for capturing stdout/stderr output

		[Required]
		public string MlaunchPath { get; set; } = string.Empty;

		[Output]
		public string MlaunchArguments { get; set; } = string.Empty;

		public IPhoneDeviceType DeviceType {
			get {
				switch (Platform) {
				case ApplePlatform.iOS:
				case ApplePlatform.TVOS:
					var plist = PDictionary.FromFile (AppManifestPath);
					return plist.GetUIDeviceFamily ();
				default:
					throw new InvalidOperationException (string.Format (MSBStrings.InvalidPlatform, Platform));
				}
			}
		}

		List<string>? GetDeviceTypes ()
		{
			var output = GetSimulatorList ();
			if (output is null)
				return null;

			// Which product family are we looking for?
			string productFamily;
			switch (DeviceType) {
			case IPhoneDeviceType.IPhone:
			case IPhoneDeviceType.IPad:
			case IPhoneDeviceType.TV:
				productFamily = DeviceType.ToString ();
				break;
			case IPhoneDeviceType.IPhoneAndIPad:
				productFamily = "IPad";
				break;
			default:
				throw new InvalidOperationException ($"Invalid device type: {DeviceType}");
			}

			// Load mlaunch's output
			var xml = new XmlDocument ();
			xml.LoadXml (output);
			// Get the device types for the product family we're looking for
			var nodes = xml.SelectNodes ($"/MTouch/Simulator/SupportedDeviceTypes/SimDeviceType[ProductFamilyId='{productFamily}']").Cast<XmlNode> ();
			// Create a list of them all
			var deviceTypes = new List<(long Min, long Max, string Identifier)> ();
			foreach (var node in nodes) {
				var minRuntimeVersionValue = node.SelectSingleNode ("MinRuntimeVersion").InnerText;
				var maxRuntimeVersionValue = node.SelectSingleNode ("MaxRuntimeVersion").InnerText;
				var identifier = node.SelectSingleNode ("Identifier").InnerText;
				if (!long.TryParse (minRuntimeVersionValue, out var minRuntimeVersion))
					continue;
				if (!long.TryParse (maxRuntimeVersionValue, out var maxRuntimeVersion))
					continue;
				deviceTypes.Add ((minRuntimeVersion, maxRuntimeVersion, identifier));
			}
			// Sort by minRuntimeVersion, this is a rudimentary way of sorting so that the last device is at the end.
			deviceTypes.Sort ((a, b) => a.Min.CompareTo (b.Min));
			// Return the sorted list
			return deviceTypes.Select (v => v.Identifier).ToList ();
		}

		string? simulator_list;
		string? GetSimulatorList ()
		{
			if (simulator_list is null) {
				var tmpfile = Path.GetTempFileName ();
				try {
					var output = new StringBuilder ();
					var result = ExecuteAsync (MlaunchPath, new string [] { "--listsim", tmpfile }, SdkDevPath).Result;
					if (result.ExitCode != 0)
						return string.Empty;
					simulator_list = File.ReadAllText (tmpfile);
				} finally {
					File.Delete (tmpfile);
				}
			}
			return simulator_list;
		}

		string? device_list;
		string? GetDeviceList ()
		{
			if (device_list is null) {
				var tmpfile = Path.GetTempFileName ();
				try {
					var output = new StringBuilder ();
					var result = ExecuteAsync (MlaunchPath, new string [] { $"--listdev:{tmpfile}", "--output-format:xml", "--use-amdevice:false" }, SdkDevPath).Result;
					if (result.ExitCode != 0)
						return string.Empty;
					device_list = File.ReadAllText (tmpfile);
				} finally {
					File.Delete (tmpfile);
				}
			}
			return device_list;
		}

		List<(string Identifier, string Name, string? NotApplicableBecause)> GetDeviceListForSimulator ()
		{
			var rv = new List<(string Identifier, string Name, string? NotApplicableBecause)> ();

			var output = GetSimulatorList ();
			if (string.IsNullOrEmpty (output))
				return rv;

			var deviceTypes = GetDeviceTypes ();
			if (deviceTypes is null)
				return rv;

			// Which product family are we looking for?
			string productFamily;
			switch (DeviceType) {
			case IPhoneDeviceType.IPhone:
			case IPhoneDeviceType.IPad:
			case IPhoneDeviceType.TV:
				productFamily = DeviceType.ToString ();
				break;
			case IPhoneDeviceType.IPhoneAndIPad:
				productFamily = "IPad";
				break;
			default:
				throw new InvalidOperationException ($"Invalid device type: {DeviceType}");
			}

			// Load mlaunch's output
			var xml = new XmlDocument ();
			xml.LoadXml (output);
			// Get the device types for the product family we're looking for
			var nodes = xml.SelectNodes ($"/MTouch/Simulator/AvailableDevices/SimDevice").Cast<XmlNode> ();
			foreach (var node in nodes) {
				var simDeviceType = node.SelectSingleNode ("SimDeviceType").InnerText;
				if (!deviceTypes.Contains (simDeviceType))
					continue;
				var udid = node.Attributes? ["UDID"]?.Value ?? string.Empty;
				var name = node.Attributes? ["Name"]?.Value ?? string.Empty;
				string? notApplicableBecause = null;

				var simRuntime = node.SelectSingleNode ("SimRuntime")?.InnerText;
				if (!string.IsNullOrEmpty (simRuntime)) {
					var simRuntimeVersionString = xml.SelectSingleNode ($"/MTouch/Simulator/SupportedRuntimes/SimRuntime[Identifier='{simRuntime}']/Version")?.InnerText;
					if (int.TryParse (simRuntimeVersionString, out var simRuntimeVersionNumber)) {
						var simRuntimeVersionMajor = (simRuntimeVersionNumber >> 16) & 0xFF;
						var simRuntimeVersionMinor = (simRuntimeVersionNumber >> 8) & 0xFF;
						var simRuntimeVersion = new Version (simRuntimeVersionMajor, simRuntimeVersionMinor);
						if (Version.TryParse (SupportedOSPlatformVersion, out var supportedOSPlatformVersion) && simRuntimeVersion < supportedOSPlatformVersion)
							notApplicableBecause = $" [OS version ({simRuntimeVersion}) lower than minimum supported platform version ({SupportedOSPlatformVersion}) for this app]";
					}
				}
				rv.Add ((udid, name, notApplicableBecause));
			}
			return rv;
		}

		List<(string Identifier, string Name, string? NotApplicableBecause)> GetDeviceListForDevice ()
		{
			var rv = new List<(string Identifier, string Name, string? NotApplicableBecause)> ();

			var output = GetDeviceList ();
			if (string.IsNullOrEmpty (output))
				return rv;

			// Which product family are we looking for?
			string deviceClassCondition;
			switch (DeviceType) {
			case IPhoneDeviceType.TV:
				deviceClassCondition = "[DeviceClass='AppleTV']";
				break;
			case IPhoneDeviceType.IPad:
				deviceClassCondition = "[DeviceClass='iPad']";
				break;
			case IPhoneDeviceType.IPhone:
			case IPhoneDeviceType.IPhoneAndIPad:
				deviceClassCondition = "[DeviceClass='iPhone' or DeviceClass='iPad']";
				break;
			default:
				throw new InvalidOperationException ($"Invalid device type: {DeviceType}");
			}

			// Load mlaunch's output
			var xml = new XmlDocument ();
			xml.LoadXml (output);
			// Get the device types for the device classes we're looking for
			var nodes = xml.SelectNodes ($"/MTouch/Device{deviceClassCondition}").Cast<XmlNode> ();
			foreach (var node in nodes) {
				var deviceIdentifier = node.SelectSingleNode ("DeviceIdentifier").InnerText;
				var name = node.SelectSingleNode ("Name").InnerText;
				var productVersionString = node.SelectSingleNode ("ProductVersion")?.InnerText;

				string? notApplicableBecause = null;
				if (Version.TryParse (productVersionString, out var productVersion) && Version.TryParse (SupportedOSPlatformVersion, out var supportedOSPlatformVersion) && productVersion < supportedOSPlatformVersion)
					notApplicableBecause = $" [OS version ({productVersionString}) lower than minimum supported platform version ({SupportedOSPlatformVersion}) for this app]";
				rv.Add ((deviceIdentifier, name, notApplicableBecause));
			}
			return rv;
		}

		protected string GenerateCommandLineCommands ()
		{
			var sb = new List<string> ();

			if (!string.IsNullOrEmpty (LaunchApp)) {
				sb.Add (SdkIsSimulator ? "--launchsim" : "--launchdev");
				sb.Add (LaunchApp);
			}

			if (!string.IsNullOrEmpty (InstallApp)) {
				sb.Add (SdkIsSimulator ? "--installsim" : "--installdev");
				sb.Add (InstallApp);
			}

			if (SdkIsSimulator && string.IsNullOrEmpty (DeviceName)) {
				var simruntime = $"com.apple.CoreSimulator.SimRuntime.{PlatformName}-{SdkVersion.Replace ('.', '-')}";
				var simdevicetypes = GetDeviceTypes ();
				string simdevicetype;

				if (simdevicetypes?.Count > 0) {
					// Use the latest device type we can find. This seems to be what Xcode does by default.
					simdevicetype = simdevicetypes.Last ();
				} else {
					// We couldn't find any device types, so pick one.
					switch (Platform) {
					case ApplePlatform.iOS:
						// Don't try to launch an iPad-only app on an iPhone
						if (DeviceType == IPhoneDeviceType.IPad) {
							simdevicetype = "com.apple.CoreSimulator.SimDeviceType.iPad--7th-generation-";
						} else {
							simdevicetype = "com.apple.CoreSimulator.SimDeviceType.iPhone-11";
						}
						break;
					case ApplePlatform.TVOS:
						simdevicetype = "com.apple.CoreSimulator.SimDeviceType.Apple-TV-4K-1080p";
						break;
					default:
						throw new InvalidOperationException (string.Format (MSBStrings.InvalidPlatform, Platform));
					}
				}
				DeviceName = $":v2:runtime={simruntime},devicetype={simdevicetype}";
			}

			if (!string.IsNullOrEmpty (DeviceName)) {
				if (SdkIsSimulator) {
					sb.Add ("--device");

					// Figure out whether we got the exact name of a simulator, in which case construct the corresponding argument.
					string? simulator = null;
					var deviceList = GetDeviceListForSimulator ();
					var simulatorsByIdentifier = deviceList.Where (v => v.Identifier == DeviceName).ToArray ();
					if (simulatorsByIdentifier.Length == 1) {
						simulator = simulatorsByIdentifier [0].Identifier;
					} else {
						var simulatorsByName = deviceList.Where (v => v.Name == DeviceName).ToArray ();
						if (simulatorsByName.Length == 1)
							simulator = simulatorsByName [0].Identifier;
					}
					if (!string.IsNullOrEmpty (simulator)) {
						sb.Add ($":v2:udid={simulator}");
					} else {
						sb.Add (DeviceName);
					}
				} else {
					sb.Add ("--devname");
					sb.Add (DeviceName);
				}
			}

			if (CaptureOutput && string.IsNullOrEmpty (StandardOutputPath))
				StandardOutputPath = GetTerminalName (1);

			if (CaptureOutput && string.IsNullOrEmpty (StandardErrorPath))
				StandardErrorPath = GetTerminalName (2);

			if (!string.IsNullOrEmpty (StandardOutputPath)) {
				sb.Add ("--stdout");
				sb.Add (StandardOutputPath);
			}

			if (!string.IsNullOrEmpty (StandardErrorPath)) {
				sb.Add ("--stderr");
				sb.Add (StandardErrorPath);
			}

			foreach (var envvar in EnvironmentVariables)
				sb.Add ("--setenv=" + envvar.ItemSpec);

			sb.Add (WaitForExit ? "--wait-for-exit:true" : "--wait-for-exit:false");

			// Add additional arguments at the end, so they can override any
			// other argument.
			foreach (var arg in AdditionalArguments)
				sb.Add (arg.ItemSpec);

			return StringUtils.FormatArguments (sb);
		}

		static string GetTerminalName (int fd)
		{
			if (isatty (fd) != 1)
				return string.Empty;

			return Marshal.PtrToStringAuto (ttyname (fd));
		}

		void ShowHelp ()
		{
			var sb = new StringBuilder ();
			var f = $"net{TargetFramework.Version}-{Platform.AsString ().ToLower ()}";
			var rid = Platform == ApplePlatform.TVOS ? "tvos-arm64" : "ios-arm64";

			sb.AppendLine ($"");
			sb.AppendLine ($"To run on physical device:");
			sb.AppendLine ($"    1. If the project has multiple target frameworks, select the desired target framework. Example: -f {f}");
			sb.AppendLine ($"    2. Pass a RuntimeIdentifier for a device. Example: -p:{rid}");
			sb.AppendLine ($"    3. Pass the name or identifier of the target device using '-p:DeviceName=<name or identifier of device>'");
			var devices = GetDeviceListForDevice ();
			if (devices.Count == 0) {
				sb.AppendLine ($"        There are no devices connected to this Mac that can be used to run this app.");
			} else {
				sb.AppendLine ($"        There are {devices.Count} device(s) connected to this Mac that can be used to run this app:");
				foreach (var d in devices)
					sb.AppendLine ($"            {d.Name} ({d.Identifier}) {d.NotApplicableBecause}");
				var firstDevice = devices.First ();
				sb.AppendLine ($"        Example: -p:DeviceName={firstDevice.Identifier} or -p:DeviceName={StringUtils.Quote (firstDevice.Name)}");
				sb.AppendLine ($"    For example:");
				var sampleDevice = firstDevice.Name == StringUtils.Quote (firstDevice.Name) ? firstDevice.Name : firstDevice.Identifier;
				sb.AppendLine ($"        dotnet run -f {f} -r {rid} -p:DeviceName={sampleDevice}");
			}

			sb.AppendLine ($"");
			sb.AppendLine ($"To run in a simulator:");
			sb.AppendLine ($"    1. If the project has multiple target frameworks, select the desired target framework. Exmaple: -f {f}");
			sb.AppendLine ($"    2. Pass the name or identifier of the target simulator using '-p:DeviceName=<name or identifier of simulator>'");
			var simulators = GetDeviceListForSimulator ();
			if (simulators.Count == 0) {
				sb.AppendLine ($"        There are no simulators available that can be used to run this app. Please open Xcode, then the menu Window -> Devices and Simulators, select Simulators on the top left, and create a new simulator clicking on the plus sign on the bottom left.");
			} else {
				sb.AppendLine ($"        There are {simulators.Count} simulators(s) on this Mac that can be used to run this app:");
				foreach (var s in simulators)
					sb.AppendLine ($"            {s.Name} ({s.Identifier}) {s.NotApplicableBecause}");
				var firstSim = simulators.First ();
				sb.AppendLine ($"        Example: -p:DeviceName={firstSim.Identifier} or -p:DeviceName={StringUtils.Quote (firstSim.Name)}");
				sb.AppendLine ($"    For example:");
				var sampleDevice = firstSim.Name == StringUtils.Quote (firstSim.Name) ? firstSim.Name : firstSim.Identifier;
				sb.AppendLine ($"        dotnet run -f {f} -p:DeviceName={sampleDevice}");
			}
			sb.AppendLine ();

			// Sadly the only way to have the help show up in the terminal reliably is to make it a warning
			Log.LogWarning (sb.ToString ());
		}

		public override bool Execute ()
		{
			if (ShouldExecuteRemotely ())
				return new TaskRunner (SessionId, BuildEngine4).RunAsync (this).Result;

			if (!string.IsNullOrEmpty (Help)) {
				ShowHelp ();
				return !Log.HasLoggedErrors;
			}

			MlaunchArguments = GenerateCommandLineCommands ();
			return !Log.HasLoggedErrors;
		}

		public void Cancel ()
		{
			if (ShouldExecuteRemotely ())
				BuildConnection.CancelAsync (BuildEngine4).Wait ();
		}

		[DllImport ("/usr/lib/libc.dylib")]
		extern static IntPtr ttyname (int filedes);

		[DllImport ("/usr/lib/libc.dylib")]
		extern static int isatty (int fd);
	}
}
