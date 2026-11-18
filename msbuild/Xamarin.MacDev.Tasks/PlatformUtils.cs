#nullable enable

namespace Xamarin.MacDev.Tasks {
	public static class PlatformUtils {
		public static string? GetTargetPlatform (string sdkPlatform)
		{
			switch (sdkPlatform) {
			case "iPhoneSimulator":
				return "iphonesimulator";
			case "iPhoneOS":
				return "iphoneos";
			case "MacOSX":
				return "macosx";
			case "AppleTVSimulator":
				return "appletvsimulator";
			case "AppleTVOS":
				return "appletvos";
			case "MacCatalyst":
				return "macosx";
			}

			return null;
		}
	}
}
