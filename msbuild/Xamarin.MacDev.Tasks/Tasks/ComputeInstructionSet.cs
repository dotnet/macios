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
		public string SupportedOSPlatformVersion { get; set; } = "";

		public string RuntimeIdentifier { get; set; } = "";

		#endregion

		#region Outputs

		[Output]
		public string InstructionSet { get; set; } = "";

		#endregion

		// Device-to-CPU mapping
		// This maps each device model to its CPU model.
		//
		// Sources for device-to-CPU information:
		// - iPhone models and chips: https://en.wikipedia.org/wiki/List_of_iPhone_models
		// - iPad models and chips: https://en.wikipedia.org/wiki/List_of_iPad_models
		// - Apple TV models and chips: https://en.wikipedia.org/wiki/Apple_TV#Specifications
		// - Apple's technical specifications pages for each device (e.g., https://support.apple.com/kb/SP714 for iPhone 6s)
		//
		// To update: Cross-reference Wikipedia articles with Apple's official tech specs when new devices are released.
		static readonly Dictionary<string, string> DeviceToCpu = new Dictionary<string, string> {
			// iOS devices
			{ "iPhone6s", "A9" },
			{ "iPhone6sPlus", "A9" },
			{ "iPhoneSE", "A9" },
			{ "iPhone7", "A10" },
			{ "iPhone7Plus", "A10" },
			{ "iPhone8", "A11" },
			{ "iPhone8Plus", "A11" },
			{ "iPhoneX", "A11" },
			{ "iPhoneXR", "A12" },
			{ "iPhoneXS", "A12" },
			{ "iPhoneXSMax", "A12" },
			{ "iPhone11", "A13" },
			{ "iPhone11Pro", "A13" },
			{ "iPhone11ProMax", "A13" },
			{ "iPhoneSE2", "A13" },
			{ "iPhone12mini", "A14" },
			{ "iPhone12", "A14" },
			{ "iPhone12Pro", "A14" },
			{ "iPhone12ProMax", "A14" },
			{ "iPhone13mini", "A15" },
			{ "iPhone13", "A15" },
			{ "iPhone13Pro", "A15" },
			{ "iPhone13ProMax", "A15" },
			{ "iPhoneSE3", "A15" },
			{ "iPhone14", "A15" },
			{ "iPhone14Plus", "A15" },
			{ "iPhone14Pro", "A16" },
			{ "iPhone14ProMax", "A16" },
			{ "iPhone15", "A16" },
			{ "iPhone15Plus", "A16" },
			{ "iPhone15Pro", "A17Pro" },
			{ "iPhone15ProMax", "A17Pro" },
			{ "iPhone16", "A18" },
			{ "iPhone16Plus", "A18" },
			{ "iPhone16Pro", "A18Pro" },
			{ "iPhone16ProMax", "A18Pro" },

			// iPad models
			{ "iPadAir2", "A8X" },
			{ "iPadMini4", "A8" },
			{ "iPadPro9_7", "A9X" },
			{ "iPadPro12_9", "A9X" },
			{ "iPad5", "A9" },
			{ "iPadPro10_5", "A10X" },
			{ "iPadPro12_9_2", "A10X" },
			{ "iPad6", "A10" },
			{ "iPadAir3", "A12" },
			{ "iPadMini5", "A12" },
			{ "iPad7", "A10" },
			{ "iPadPro11", "A12X" },
			{ "iPadPro12_9_3", "A12X" },
			{ "iPad8", "A12" },
			{ "iPadAir4", "A14" },
			{ "iPad9", "A13" },
			{ "iPadMini6", "A15" },
			{ "iPadAir5", "M1" },
			{ "iPadPro11_3", "M1" },
			{ "iPadPro12_9_5", "M1" },
			{ "iPad10", "A14" },
			{ "iPadPro11_4", "M2" },
			{ "iPadPro12_9_6", "M2" },
			{ "iPadAir6", "M2" },
			{ "iPadPro11_M4", "M4" },
			{ "iPadPro13_M4", "M4" },

			// Apple TV models
			{ "AppleTV4", "A8" },
			{ "AppleTV4K", "A10X" },
			{ "AppleTV4K2", "A12" },
			{ "AppleTV4K3", "A15" },
		};

		// Device-to-max-OS mapping
		// This maps each device to its maximum supported OS version.
		//
		// Sources for maximum OS version support:
		// - iOS compatibility: https://en.wikipedia.org/wiki/IOS_version_history#Overview
		// - iPadOS compatibility: https://en.wikipedia.org/wiki/IPadOS_version_history#Overview
		// - tvOS compatibility: https://en.wikipedia.org/wiki/TvOS_version_history#Overview
		// - Apple's official iOS/iPadOS/tvOS release notes and compatibility pages
		// - https://support.apple.com/en-us/120256 (iOS and iPadOS compatibility)
		//
		// To update: Check Wikipedia compatibility tables and Apple's official support documents when new OS versions are released.
		// Note: These represent the latest known maximum versions and may need updates as Apple releases new OS versions.
		static readonly Dictionary<string, string> DeviceToMaxOS = new Dictionary<string, string> {
			// iOS devices
			{ "iPhone6s", "15.8" },
			{ "iPhone6sPlus", "15.8" },
			{ "iPhoneSE", "15.8" },
			{ "iPhone7", "15.8" },
			{ "iPhone7Plus", "15.8" },
			{ "iPhone8", "16.7" },
			{ "iPhone8Plus", "16.7" },
			{ "iPhoneX", "16.7" },
			{ "iPhoneXR", "18.2" },
			{ "iPhoneXS", "18.2" },
			{ "iPhoneXSMax", "18.2" },
			{ "iPhone11", "18.2" },
			{ "iPhone11Pro", "18.2" },
			{ "iPhone11ProMax", "18.2" },
			{ "iPhoneSE2", "18.2" },
			{ "iPhone12mini", "18.2" },
			{ "iPhone12", "18.2" },
			{ "iPhone12Pro", "18.2" },
			{ "iPhone12ProMax", "18.2" },
			{ "iPhone13mini", "18.2" },
			{ "iPhone13", "18.2" },
			{ "iPhone13Pro", "18.2" },
			{ "iPhone13ProMax", "18.2" },
			{ "iPhoneSE3", "18.2" },
			{ "iPhone14", "18.2" },
			{ "iPhone14Plus", "18.2" },
			{ "iPhone14Pro", "18.2" },
			{ "iPhone14ProMax", "18.2" },
			{ "iPhone15", "18.2" },
			{ "iPhone15Plus", "18.2" },
			{ "iPhone15Pro", "18.2" },
			{ "iPhone15ProMax", "18.2" },
			{ "iPhone16", "18.2" },
			{ "iPhone16Plus", "18.2" },
			{ "iPhone16Pro", "18.2" },
			{ "iPhone16ProMax", "18.2" },

			// iPad models
			{ "iPadAir2", "15.8" },
			{ "iPadMini4", "15.8" },
			{ "iPadPro9_7", "16.7" },
			{ "iPadPro12_9", "16.7" },
			{ "iPad5", "16.7" },
			{ "iPadPro10_5", "16.7" },
			{ "iPadPro12_9_2", "16.7" },
			{ "iPad6", "16.7" },
			{ "iPadAir3", "17.7" },
			{ "iPadMini5", "17.7" },
			{ "iPad7", "17.7" },
			{ "iPadPro11", "18.2" },
			{ "iPadPro12_9_3", "18.2" },
			{ "iPad8", "18.2" },
			{ "iPadAir4", "18.2" },
			{ "iPad9", "18.2" },
			{ "iPadMini6", "18.2" },
			{ "iPadAir5", "18.2" },
			{ "iPadPro11_3", "18.2" },
			{ "iPadPro12_9_5", "18.2" },
			{ "iPad10", "18.2" },
			{ "iPadPro11_4", "18.2" },
			{ "iPadPro12_9_6", "18.2" },
			{ "iPadAir6", "18.2" },
			{ "iPadPro11_M4", "18.2" },
			{ "iPadPro13_M4", "18.2" },

			// Apple TV models - tvOS versions
			{ "AppleTV4", "15.6" },
			{ "AppleTV4K", "18.2" },
			{ "AppleTV4K2", "18.2" },
			{ "AppleTV4K3", "18.2" },
		};

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
				if (string.IsNullOrEmpty (SupportedOSPlatformVersion)) {
					Log.LogMessage (MessageImportance.Low, "SupportedOSPlatformVersion is not set, skipping instruction set computation");
					return true;
				}

				var instructionSet = ComputeMinimumInstructionSet (Platform, SupportedOSPlatformVersion);
				if (!string.IsNullOrEmpty (instructionSet)) {
					// The null-forgiving operator is required here because the compiler doesn't understand
					// that !string.IsNullOrEmpty guarantees non-null
					InstructionSet = instructionSet!;
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

			// For macOS and Mac Catalyst, we need different logic
			if (platform == ApplePlatform.MacOSX || platform == ApplePlatform.MacCatalyst) {
				return ComputeMacInstructionSet (platform, targetVersion);
			}

			// For iOS and tvOS, find the oldest device that can run this OS version
			var devicesToCheck = platform == ApplePlatform.TVOS ?
				DeviceToMaxOS.Where (kv => kv.Key.StartsWith ("AppleTV")) :
				DeviceToMaxOS.Where (kv => !kv.Key.StartsWith ("AppleTV"));

			string? oldestCpu = null;
			string? oldestInstructionSet = null;

			foreach (var device in devicesToCheck) {
				var deviceName = device.Key;
				var maxOSString = device.Value;

				if (!Version.TryParse (maxOSString, out var maxOS))
					continue;

				// Check if this device can run the target OS version
				// A device can run the target OS if its max OS >= target OS
				if (maxOS >= targetVersion) {
					// This device can run the target OS
					if (DeviceToCpu.TryGetValue (deviceName, out var cpu)) {
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
			
			if (string.IsNullOrEmpty (RuntimeIdentifier)) {
				Log.LogMessage (MessageImportance.Low, $"RuntimeIdentifier is not set, cannot determine instruction set for {platform}");
				return null;
			}

			var parts = RuntimeIdentifier.Split ('-');
			if (parts.Length < 2) {
				Log.LogMessage (MessageImportance.Low, $"RuntimeIdentifier '{RuntimeIdentifier}' has unexpected format");
				return null;
			}

			// The architecture is always the last segment after the last hyphen
			var arch = parts [parts.Length - 1];

			// Determine instruction set based on architecture
			if (arch == "x64") {
				// Intel/AMD x64 architecture
				return "x86-64-v2";
			} else if (arch == "arm64") {
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
