using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Build.Framework;
using Xamarin.Localization.MSBuild;
using Xamarin.Utils;

#nullable enable

namespace Xamarin.MacDev.Tasks {
	/// <summary>
	/// Computes the minimum instruction set required for a given OS version and platform.
	/// This is used to configure the R2R/NAOT compiler (crossgen2) with the appropriate --instruction-set argument.
	/// </summary>
	public class ComputeInstructionSet : XamarinTask {
		#region Inputs

		[Required]
		public string RuntimeIdentifier { get; set; } = "";

		[Required]
		public string SupportedOSPlatformVersion { get; set; } = "";

		#endregion

		#region Outputs

		[Output]
		public string InstructionSet { get; set; } = "";

		#endregion

		// Device information structure
		class DeviceInfo {
			public string Cpu { get; }
			public string MaxOSVersion { get; }

			public DeviceInfo (string cpu, string maxOSVersion)
			{
				Cpu = cpu;
				MaxOSVersion = maxOSVersion;
			}
		}

		// Device information mapping
		// This maps each device model to its CPU model and maximum supported OS version.
		//
		// Sources for device information:
		// - iPhone models and chips: https://en.wikipedia.org/wiki/List_of_iPhone_models
		// - iPad models and chips: https://en.wikipedia.org/wiki/List_of_iPad_models
		// - Apple TV models and chips: https://en.wikipedia.org/wiki/Apple_TV#Specifications
		// - Apple's technical specifications pages for each device (e.g., https://support.apple.com/kb/SP714 for iPhone 6s)
		// - iOS compatibility: https://en.wikipedia.org/wiki/IOS_version_history#Overview
		// - iPadOS compatibility: https://en.wikipedia.org/wiki/IPadOS_version_history#Overview
		// - tvOS compatibility: https://en.wikipedia.org/wiki/TvOS_version_history#Overview
		// - https://support.apple.com/en-us/120256 (iOS and iPadOS compatibility)
		//
		// To update: Cross-reference Wikipedia articles with Apple's official tech specs when new devices are released.
		// Note: For devices that support the latest OS version, use SdkVersions constants instead of hardcoding the version number.
		static Dictionary<string, DeviceInfo> GetDeviceInfo ()
		{
			// Use SdkVersions for the latest OS versions
			var latestIOSVersion = SdkVersions.iOS;
			var latestTVOSVersion = SdkVersions.TVOS;

			return new Dictionary<string, DeviceInfo> {
				// iOS devices
				{ "iPhone6s", new DeviceInfo ("A9", "15.8") },
				{ "iPhone6sPlus", new DeviceInfo ("A9", "15.8") },
				{ "iPhoneSE", new DeviceInfo ("A9", "15.8") },
				{ "iPhone7", new DeviceInfo ("A10", "15.8") },
				{ "iPhone7Plus", new DeviceInfo ("A10", "15.8") },
				{ "iPhone8", new DeviceInfo ("A11", "16.7") },
				{ "iPhone8Plus", new DeviceInfo ("A11", "16.7") },
				{ "iPhoneX", new DeviceInfo ("A11", "16.7") },
				{ "iPhoneXR", new DeviceInfo ("A12", latestIOSVersion) },
				{ "iPhoneXS", new DeviceInfo ("A12", latestIOSVersion) },
				{ "iPhoneXSMax", new DeviceInfo ("A12", latestIOSVersion) },
				{ "iPhone11", new DeviceInfo ("A13", latestIOSVersion) },
				{ "iPhone11Pro", new DeviceInfo ("A13", latestIOSVersion) },
				{ "iPhone11ProMax", new DeviceInfo ("A13", latestIOSVersion) },
				{ "iPhoneSE2", new DeviceInfo ("A13", latestIOSVersion) },
				{ "iPhone12mini", new DeviceInfo ("A14", latestIOSVersion) },
				{ "iPhone12", new DeviceInfo ("A14", latestIOSVersion) },
				{ "iPhone12Pro", new DeviceInfo ("A14", latestIOSVersion) },
				{ "iPhone12ProMax", new DeviceInfo ("A14", latestIOSVersion) },
				{ "iPhone13mini", new DeviceInfo ("A15", latestIOSVersion) },
				{ "iPhone13", new DeviceInfo ("A15", latestIOSVersion) },
				{ "iPhone13Pro", new DeviceInfo ("A15", latestIOSVersion) },
				{ "iPhone13ProMax", new DeviceInfo ("A15", latestIOSVersion) },
				{ "iPhoneSE3", new DeviceInfo ("A15", latestIOSVersion) },
				{ "iPhone14", new DeviceInfo ("A15", latestIOSVersion) },
				{ "iPhone14Plus", new DeviceInfo ("A15", latestIOSVersion) },
				{ "iPhone14Pro", new DeviceInfo ("A16", latestIOSVersion) },
				{ "iPhone14ProMax", new DeviceInfo ("A16", latestIOSVersion) },
				{ "iPhone15", new DeviceInfo ("A16", latestIOSVersion) },
				{ "iPhone15Plus", new DeviceInfo ("A16", latestIOSVersion) },
				{ "iPhone15Pro", new DeviceInfo ("A17Pro", latestIOSVersion) },
				{ "iPhone15ProMax", new DeviceInfo ("A17Pro", latestIOSVersion) },
				{ "iPhone16", new DeviceInfo ("A18", latestIOSVersion) },
				{ "iPhone16Plus", new DeviceInfo ("A18", latestIOSVersion) },
				{ "iPhone16Pro", new DeviceInfo ("A18Pro", latestIOSVersion) },
				{ "iPhone16ProMax", new DeviceInfo ("A18Pro", latestIOSVersion) },

				// iPad models
				{ "iPadAir2", new DeviceInfo ("A8X", "15.8") },
				{ "iPadMini4", new DeviceInfo ("A8", "15.8") },
				{ "iPadPro9_7", new DeviceInfo ("A9X", "16.7") },
				{ "iPadPro12_9", new DeviceInfo ("A9X", "16.7") },
				{ "iPad5", new DeviceInfo ("A9", "16.7") },
				{ "iPadPro10_5", new DeviceInfo ("A10X", "16.7") },
				{ "iPadPro12_9_2", new DeviceInfo ("A10X", "16.7") },
				{ "iPad6", new DeviceInfo ("A10", "16.7") },
				{ "iPadAir3", new DeviceInfo ("A12", "17.7") },
				{ "iPadMini5", new DeviceInfo ("A12", "17.7") },
				{ "iPad7", new DeviceInfo ("A10", "17.7") },
				{ "iPadPro11", new DeviceInfo ("A12X", latestIOSVersion) },
				{ "iPadPro12_9_3", new DeviceInfo ("A12X", latestIOSVersion) },
				{ "iPad8", new DeviceInfo ("A12", latestIOSVersion) },
				{ "iPadAir4", new DeviceInfo ("A14", latestIOSVersion) },
				{ "iPad9", new DeviceInfo ("A13", latestIOSVersion) },
				{ "iPadMini6", new DeviceInfo ("A15", latestIOSVersion) },
				{ "iPadAir5", new DeviceInfo ("M1", latestIOSVersion) },
				{ "iPadPro11_3", new DeviceInfo ("M1", latestIOSVersion) },
				{ "iPadPro12_9_5", new DeviceInfo ("M1", latestIOSVersion) },
				{ "iPad10", new DeviceInfo ("A14", latestIOSVersion) },
				{ "iPadPro11_4", new DeviceInfo ("M2", latestIOSVersion) },
				{ "iPadPro12_9_6", new DeviceInfo ("M2", latestIOSVersion) },
				{ "iPadAir6", new DeviceInfo ("M2", latestIOSVersion) },
				{ "iPadPro11_M4", new DeviceInfo ("M4", latestIOSVersion) },
				{ "iPadPro13_M4", new DeviceInfo ("M4", latestIOSVersion) },

				// Apple TV models
				{ "AppleTV4", new DeviceInfo ("A8", "15.6") },
				{ "AppleTV4K", new DeviceInfo ("A10X", latestTVOSVersion) },
				{ "AppleTV4K2", new DeviceInfo ("A12", latestTVOSVersion) },
				{ "AppleTV4K3", new DeviceInfo ("A15", latestTVOSVersion) },
			};
		}

		// CPU-to-instruction-set mapping
		// This maps each CPU model to the instruction set it supports.
		//
		// Sources for CPU instruction set architecture information:
		// - Apple A-series chips: https://en.wikipedia.org/wiki/Apple_silicon#A_series
		// - Apple M-series chips: https://en.wikipedia.org/wiki/Apple_silicon#M_series
		// - ARM architecture versions: https://en.wikipedia.org/wiki/ARM_architecture_family#Cores
		// - Apple's LLVM source and documentation for architecture features
		// - ARM Architecture Reference Manuals: https://developer.arm.com/documentation/
		//
		// Crossgen2 instruction set compatibility:
		// Run `crossgen2 --help` to see supported instruction sets (current list):
		// x86-64-v2, x86-64-v3, x86-64-v4, armv8-a, armv8.1-a, armv8.2-a, armv8.3-a, armv8.4-a, armv8.5-a, armv8.6-a, apple-m1
		//
		// To update: Verify new Apple chip architectures against ARM documentation and crossgen2 supported instruction sets.
		static readonly Dictionary<string, string> CpuToInstructionSet = new Dictionary<string, string> {
			// ARM chips
			{ "A8", "armv8-a" },         // ARMv8.0-A (iPhone 6, iPad Air 2, Apple TV 4)
			{ "A8X", "armv8-a" },        // ARMv8.0-A (iPad Air 2)
			{ "A9", "armv8-a" },         // ARMv8.0-A (iPhone 6s, iPad 5, iPad Pro 9.7)
			{ "A9X", "armv8-a" },        // ARMv8.0-A (iPad Pro 12.9 1st gen, iPad Pro 9.7)
			{ "A10", "armv8-a" },        // ARMv8.0-A (iPhone 7)
			{ "A10X", "armv8.1-a" },     // ARMv8.1-A (iPad Pro 10.5, iPad Pro 12.9 2nd gen, Apple TV 4K)
			{ "A11", "armv8.2-a" },      // ARMv8.2-A (iPhone 8, iPhone X)
			{ "A12", "armv8.3-a" },      // ARMv8.3-A (iPhone XS, iPad Air 3, Apple TV 4K 2nd gen)
			{ "A12X", "armv8.3-a" },     // ARMv8.3-A (iPad Pro 11, iPad Pro 12.9 3rd gen)
			{ "A13", "armv8.4-a" },      // ARMv8.4-A (iPhone 11, iPhone SE 2nd gen)
			{ "A14", "armv8.4-a" },      // ARMv8.4-A with additional features (iPhone 12, iPad Air 4)
			{ "A15", "armv8.5-a" },      // ARMv8.5-A (iPhone 13, Apple TV 4K 3rd gen)
			{ "A16", "armv8.6-a" },      // ARMv8.6-A (iPhone 14 Pro)
			{ "A17Pro", "armv8.6-a" },   // ARMv8.6-A+ (iPhone 15 Pro)
			{ "A18", "armv8.6-a" },      // ARMv8.6-A+ (iPhone 16)
			{ "A18Pro", "armv8.6-a" },   // ARMv8.6-A+ (iPhone 16 Pro)

			// Apple Silicon (M-series) for macOS/Mac Catalyst
			{ "M1", "apple-m1" },        // Apple M1 (Mac, iPad Air 5, iPad Pro)
			{ "M2", "apple-m1" },        // Apple M2 (similar to M1 in instruction support for crossgen2)
			{ "M3", "apple-m1" },        // Apple M3 (similar to M1 in instruction support for crossgen2)
			{ "M4", "apple-m1" },        // Apple M4 (similar to M1 in instruction support for crossgen2)

			// Intel chips for macOS
			{ "Intel", "x86-64-v2" },    // Default Intel instruction set
		};

		// Crossgen2 supported instruction sets (from crossgen2 --help)
		static readonly HashSet<string> SupportedInstructionSets = new HashSet<string> {
			"x86-64-v2",
			"x86-64-v3",
			"x86-64-v4",
			"armv8-a",
			"armv8.1-a",
			"armv8.2-a",
			"armv8.3-a",
			"armv8.4-a",
			"armv8.5-a",
			"armv8.6-a",
			"apple-m1"
		};

		public override bool Execute ()
		{
			try {
				var instructionSet = ComputeMinimumInstructionSet (Platform, SupportedOSPlatformVersion);
#if NET
				if (!string.IsNullOrEmpty (instructionSet)) {
#else
				if (!string.IsNullOrEmpty (instructionSet) && instructionSet is not null) {
#endif
					InstructionSet = instructionSet;
					Log.LogMessage (MessageImportance.Low, $"Computed instruction set '{InstructionSet}' for {PlatformName} {SupportedOSPlatformVersion}");
				} else {
					Log.LogMessage (MessageImportance.Low, $"No instruction set computed for {PlatformName} {SupportedOSPlatformVersion}");
				}

				return !Log.HasLoggedErrors;
			} catch (Exception ex) {
				Log.LogError ($"Error computing instruction set: {ex.Message}");
				return false;
			}
		}

		string? ComputeMinimumInstructionSet (ApplePlatform platform, string osVersion)
		{
			// Parse the OS version
			if (!Version.TryParse (osVersion, out var targetVersion)) {
				Log.LogMessage (MessageImportance.Low, $"Could not parse OS version: {osVersion}");
				return null;
			}

			// For macOS, Mac Catalyst, and simulators (which run on Mac hardware), we need different logic
			if (platform == ApplePlatform.MacOSX || platform == ApplePlatform.MacCatalyst || RuntimeIdentifier.Contains ("simulator")) {
				return ComputeMacInstructionSet (platform, targetVersion);
			}

			// For iOS and tvOS, find the oldest device that can run this OS version
			var deviceInfo = GetDeviceInfo ();
			var devicesToCheck = platform == ApplePlatform.TVOS ?
				deviceInfo.Where (kv => kv.Key.StartsWith ("AppleTV")) :
				deviceInfo.Where (kv => !kv.Key.StartsWith ("AppleTV"));

			string? oldestCpu = null;
			string? oldestInstructionSet = null;

			foreach (var device in devicesToCheck) {
				var deviceName = device.Key;
				var info = device.Value;
				var maxOSString = info.MaxOSVersion;

				if (!Version.TryParse (maxOSString, out var maxOS))
					continue;

				// Check if this device can run the target OS version
				// A device can run the target OS if its max OS >= target OS
				if (maxOS >= targetVersion) {
					// This device can run the target OS
					var cpu = info.Cpu;
					if (CpuToInstructionSet.TryGetValue (cpu, out var instructionSet)) {
						// Keep track of the oldest instruction set we've seen
						// We want the minimum instruction set that all compatible devices support
						if (oldestCpu is null || IsOlderInstructionSet (instructionSet, oldestInstructionSet)) {
							oldestCpu = cpu;
							oldestInstructionSet = instructionSet;
						}
					}
				}
			}

			// Validate that the instruction set is supported by crossgen2
			if (oldestInstructionSet is not null && !SupportedInstructionSets.Contains (oldestInstructionSet)) {
				Log.LogMessage (MessageImportance.Low, $"Instruction set '{oldestInstructionSet}' is not supported by crossgen2, skipping");
				return null;
			}

			return oldestInstructionSet;
		}

		string? ComputeMacInstructionSet (ApplePlatform platform, Version targetVersion)
		{
			// For macOS and Mac Catalyst, we need to determine the instruction set based on RuntimeIdentifier
			// RuntimeIdentifier format: <os>-<arch> or <os>.<version>-<arch> 
			// Examples: "osx-x64", "osx-arm64", "osx.13.0-arm64", "maccatalyst-x64", "maccatalyst-arm64"

			var parts = RuntimeIdentifier.Split ('-');
			if (parts.Length < 2) {
				Log.LogMessage (MessageImportance.Low, $"RuntimeIdentifier '{RuntimeIdentifier}' has unexpected format");
				return null;
			}

			// The architecture is always the last segment after the last hyphen
			var arch = parts [parts.Length - 1];

			// Determine instruction set based on architecture
			if (string.Equals (arch, "x64", StringComparison.OrdinalIgnoreCase)) {
				// Intel/AMD x64 architecture
				return "x86-64-v2";
			} else if (string.Equals (arch, "arm64", StringComparison.OrdinalIgnoreCase)) {
				// Apple Silicon
				return "apple-m1";
			} else {
				Log.LogMessage (MessageImportance.Low, $"Unknown architecture '{arch}' in RuntimeIdentifier '{RuntimeIdentifier}'");
				return null;
			}
		}

		bool IsOlderInstructionSet (string? instructionSet1, string? instructionSet2)
		{
			if (instructionSet2 is null)
				return true;
			if (instructionSet1 is null)
				return false;

			// Order instruction sets from oldest to newest
			var order = new [] {
				"armv8-a",
				"armv8.1-a",
				"armv8.2-a",
				"armv8.3-a",
				"armv8.4-a",
				"armv8.5-a",
				"armv8.6-a",
				"apple-m1",
				"x86-64-v2",
				"x86-64-v3",
				"x86-64-v4"
			};

			var index1 = Array.IndexOf (order, instructionSet1);
			var index2 = Array.IndexOf (order, instructionSet2);

			if (index1 == -1 || index2 == -1)
				return false;

			return index1 < index2;
		}
	}
}
