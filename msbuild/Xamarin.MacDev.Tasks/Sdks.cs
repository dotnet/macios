using System;
using System.IO;

using Xamarin.Localization.MSBuild;
using Xamarin.Utils;
using Xamarin.MacDev.Tasks;

// Disable until we get around to enable + fix any issues.
#nullable disable

namespace Xamarin.MacDev {
	public static class Sdks {
		public static AppleIPhoneSdk IOS { get; private set; }
		public static MacOSXSdk MacOS { get; private set; }
		public static AppleTVOSSdk TVOS { get; private set; }

		static Sdks ()
		{
			Reload ();
		}

		static void Reload ()
		{
			IOS = new AppleIPhoneSdk (AppleSdkSettings.DeveloperRoot, AppleSdkSettings.DeveloperRootVersionPlist);
			TVOS = new AppleTVOSSdk (AppleSdkSettings.DeveloperRoot, AppleSdkSettings.DeveloperRootVersionPlist);
			MacOS = new MacOSXSdk (AppleSdkSettings.DeveloperRoot, AppleSdkSettings.DeveloperRootVersionPlist);
		}

		public static AppleSdk GetSdk (ApplePlatform framework)
		{
			switch (framework) {
			case ApplePlatform.iOS:
				return IOS;
			case ApplePlatform.TVOS:
				return TVOS;
			default:
				throw new InvalidOperationException (string.Format (MSBStrings.InvalidFramework, framework));
			}
		}

		public static AppleSdk GetSdk (string targetFrameworkMoniker)
		{
			return GetSdk (PlatformFrameworkHelper.GetFramework (targetFrameworkMoniker));
		}

		public static IAppleSdk GetAppleSdk (ApplePlatform framework)
		{
			switch (framework) {
			case ApplePlatform.iOS:
				return IOS;
			case ApplePlatform.TVOS:
				return TVOS;
			case ApplePlatform.MacCatalyst:
			case ApplePlatform.MacOSX:
				return MacOS;
			default:
				throw new InvalidOperationException (string.Format (MSBStrings.InvalidFramework, framework));
			}
		}

		public static IAppleSdk GetAppleSdk (string targetFrameworkMoniker)
		{
			return GetAppleSdk (PlatformFrameworkHelper.GetFramework (targetFrameworkMoniker));
		}

	}
}
