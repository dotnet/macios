using System.Diagnostics.CodeAnalysis;

public static class PlatformNameExtensions {

	public static bool TryGetApplicationClassName (this PlatformName currentPlatform, [NotNullWhen (true)] out string? className)
	{
		switch (currentPlatform) {
		case PlatformName.iOS:
		case PlatformName.WatchOS:
		case PlatformName.TvOS:
		case PlatformName.MacCatalyst:
			className = "UIApplication";
			return true;
		case PlatformName.MacOSX:
			className = "NSApplication";
			return true;
		default:
			className = null;
			return false;
		}
	}
}
