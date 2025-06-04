using System.Collections.Generic;
using ObjCRuntime;
using Xamarin.Utils;

#nullable enable

public class LibraryManager {
	public List<string> Libraries = new ();
	public string GetAttributeLibraryPath (LibraryInfo libraryInfo, PlatformName currentPlatform)
	{
		return libraryInfo.AttributeDll!;
	}

	public IEnumerable<string> GetLibraryDirectories (LibraryInfo libraryInfo, PlatformName currentPlatform)
	{
		foreach (var lib in Libraries)
			yield return lib;
	}

	public static PlatformName DetermineCurrentPlatform (ApplePlatform applePlatform)
	{
		switch (applePlatform) {
		case ApplePlatform.iOS:
			return PlatformName.iOS;
		case ApplePlatform.TVOS:
			return PlatformName.TvOS;
		case ApplePlatform.MacCatalyst:
			return PlatformName.MacCatalyst;
		case ApplePlatform.MacOSX:
			return PlatformName.MacOSX;
		default:
			return PlatformName.None;
		}
	}
}

