#nullable enable

using System;
using System.Runtime.InteropServices;

using Foundation;

#if !COREBUILD
namespace ObjCRuntime {
	internal static class SystemVersion {
#if __MACOS__
		// NSProcessInfo.ProcessInfo.OperatingSystemVersion is only available
		// in macOS 10.10, which means we can only use it in .NET (we support
		// macOS 10.14+), and not legacy (where we support macOS 10.9+)
		static NSOperatingSystemVersion? osx_version;
		internal static bool CheckmacOS (int major, int minor)
		{
			if (osx_version is null)
				osx_version = NSProcessInfo.ProcessInfo.OperatingSystemVersion;

			var osx_major = osx_version.Value.Major;
			var osx_minor = osx_version.Value.Minor;
			return osx_major > major || (osx_major == major && osx_minor >= minor);
		}
#elif __IOS__ || __MACCATALYST__ || __TVOS__
		// These three can be used interchangeably, the OS versions are the same.
		internal static bool CheckiOS (int major, int minor)
		{
			return UIKit.UIDevice.CurrentDevice.CheckSystemVersion (major, minor);
		}

		internal static bool ChecktvOS (int major, int minor)
		{
			return UIKit.UIDevice.CurrentDevice.CheckSystemVersion (major, minor);
		}

		internal static bool CheckMacCatalyst (int major, int minor)
		{
			return UIKit.UIDevice.CurrentDevice.CheckSystemVersion (major, minor);
		}
#else
#error Unknown platform
#endif

		// Due to an issue with the analyzer (https://github.com/dotnet/roslyn-analyzers/issues/7665),
		// always use a versioned string with "SupportedOSPlatformGuard". Also, this version can't be
		// lower than the minimum OS version.
		// https://github.com/dotnet/roslyn-analyzers/issues/7665

		[SupportedOSPlatformGuard ("ios13.0")]
		[SupportedOSPlatformGuard (Constants.MinimumVersions.MacCatalyst)]
		[SupportedOSPlatformGuard (Constants.MinimumVersions.macOS)]
		[SupportedOSPlatformGuard ("tvos13.0")]
		internal static bool IsAtLeastXcode11 {
			get {
				if (is_at_least_xcode_11 is null) {
#if __MACOS__
					is_at_least_xcode_11 = true;
#elif __MACCATALYST__
					is_at_least_xcode_11 = true;
#elif __IOS__
					is_at_least_xcode_11 = OperatingSystem.IsIOSVersionAtLeast (13, 0);
#elif __TVOS__
					is_at_least_xcode_11 = OperatingSystem.IsTvOSVersionAtLeast (13, 0);
#else
#error Unknown platform
#endif
				}
				return is_at_least_xcode_11.Value;
			}
		}
		static bool? is_at_least_xcode_11;

		[SupportedOSPlatformGuard ("ios14.0")]
		[SupportedOSPlatformGuard (Constants.MinimumVersions.MacCatalyst)]
		[SupportedOSPlatformGuard (Constants.MinimumVersions.macOS)]
		[SupportedOSPlatformGuard ("tvos14.0")]
		internal static bool IsAtLeastXcode12 {
			get {
				if (is_at_least_xcode_12 is null) {
#if __MACOS__
					is_at_least_xcode_12 = true;
#elif __MACCATALYST__
					is_at_least_xcode_12 = true;
#elif __IOS__
					is_at_least_xcode_12 = OperatingSystem.IsIOSVersionAtLeast (14, 0);
#elif __TVOS__
					is_at_least_xcode_12 = OperatingSystem.IsTvOSVersionAtLeast (14, 0);
#else
#error Unknown platform
#endif
				}
				return is_at_least_xcode_12.Value;
			}
		}
		static bool? is_at_least_xcode_12;


		[SupportedOSPlatformGuard ("ios14.2")]
		[SupportedOSPlatformGuard (Constants.MinimumVersions.MacCatalyst)]
		[SupportedOSPlatformGuard (Constants.MinimumVersions.macOS)]
		[SupportedOSPlatformGuard ("tvos14.2")]
		internal static bool IsAtLeastXcode12_2 {
			get {
				if (is_at_least_xcode_12_2 is null) {
#if __MACOS__
					is_at_least_xcode_12_2 = true;
#elif __MACCATALYST__
					is_at_least_xcode_12_2 = true;
#elif __IOS__
					is_at_least_xcode_12_2 = OperatingSystem.IsIOSVersionAtLeast (14, 2);
#elif __TVOS__
					is_at_least_xcode_12_2 = OperatingSystem.IsTvOSVersionAtLeast (14, 2);
#else
#error Unknown platform
#endif
				}
				return is_at_least_xcode_12_2.Value;
			}
		}
		static bool? is_at_least_xcode_12_2;

		[SupportedOSPlatformGuard ("ios15.0")]
		[SupportedOSPlatformGuard (Constants.MinimumVersions.MacCatalyst)]
		[SupportedOSPlatformGuard (Constants.MinimumVersions.macOS)]
		[SupportedOSPlatformGuard ("tvos15.0")]
		internal static bool IsAtLeastXcode13 {
			get {
				if (is_at_least_xcode_13 is null) {
#if __MACOS__
					is_at_least_xcode_13 = true;
#elif __MACCATALYST__
					is_at_least_xcode_13 = true;
#elif __IOS__
					is_at_least_xcode_13 = OperatingSystem.IsIOSVersionAtLeast (15, 0);
#elif __TVOS__
					is_at_least_xcode_13 = OperatingSystem.IsTvOSVersionAtLeast (15, 0);
#else
#error Unknown platform
#endif
				}
				return is_at_least_xcode_13.Value;
			}
		}
		static bool? is_at_least_xcode_13;

		[SupportedOSPlatformGuard ("ios18.0")]
		[SupportedOSPlatformGuard ("maccatalyst18.0")]
		[SupportedOSPlatformGuard ("macos15.0")]
		[SupportedOSPlatformGuard ("tvos18.0")]
		internal static bool IsAtLeastXcode16 {
			get {
				if (is_at_least_xcode_16 is null) {
#if __MACOS__
					is_at_least_xcode_16 = OperatingSystem.IsMacOSVersionAtLeast (15, 0);
#elif __MACCATALYST__
					is_at_least_xcode_16 = OperatingSystem.IsMacCatalystVersionAtLeast (18, 0);
#elif __IOS__
					is_at_least_xcode_16 = OperatingSystem.IsIOSVersionAtLeast (18, 0);
#elif __TVOS__
					is_at_least_xcode_16 = OperatingSystem.IsTvOSVersionAtLeast (18, 0);
#else
#error Unknown platform
#endif
				}
				return is_at_least_xcode_16.Value;
			}
		}
		static bool? is_at_least_xcode_16;

		[SupportedOSPlatformGuard ("ios18.5")]
		[SupportedOSPlatformGuard ("maccatalyst18.5")]
		[SupportedOSPlatformGuard ("macos15.5")]
		[SupportedOSPlatformGuard ("tvos18.5")]
		internal static bool IsAtLeastXcode16_4 {
			get {
				if (is_at_least_xcode_16_4 is null) {
#if __MACOS__
					is_at_least_xcode_16_4 = OperatingSystem.IsMacOSVersionAtLeast (15, 5);
#elif __MACCATALYST__
					is_at_least_xcode_16_4 = OperatingSystem.IsMacCatalystVersionAtLeast (18, 5);
#elif __IOS__
					is_at_least_xcode_16_4 = OperatingSystem.IsIOSVersionAtLeast (18, 5);
#elif __TVOS__
					is_at_least_xcode_16_4 = OperatingSystem.IsTvOSVersionAtLeast (18, 5);
#else
#error Unknown platform
#endif
				}
				return is_at_least_xcode_16_4.Value;
			}
		}
		static bool? is_at_least_xcode_16_4;
	}
}
#endif
