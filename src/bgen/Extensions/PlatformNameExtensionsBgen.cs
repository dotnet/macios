using System;
using System.IO;
using Xamarin.Utils;

public static class PlatformNameExtensionsBgen {

	// wrapper that allows us to use the same code for rgen and bgen
	public static string GetApplicationClassName (this PlatformName currentPlatform)
	{
		if (currentPlatform.TryGetApplicationClassName (out var applicationClassName))
			return applicationClassName;
		throw new BindingException (1047, currentPlatform);
	}

	public static ApplePlatform AsApplePlatform (this PlatformName platform)
	{
		switch (platform) {
		case PlatformName.iOS:
			return ApplePlatform.iOS;
		case PlatformName.TvOS:
			return ApplePlatform.TVOS;
		case PlatformName.MacCatalyst:
			return ApplePlatform.MacCatalyst;
		case PlatformName.MacOSX:
			return ApplePlatform.MacOSX;
		case PlatformName.None:
			return ApplePlatform.None;
		default:
			throw new ArgumentOutOfRangeException (nameof (platform), platform, $"Unknown platform: {platform}");
		}
	}
}
