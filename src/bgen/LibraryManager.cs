using System.Collections.Generic;
using ObjCRuntime;
using Xamarin.Utils;

#nullable enable

public class LibraryManager {
	public List<string> Libraries = new ();
	public string GetAttributeLibraryPath (LibraryInfo libraryInfo, PlatformName currentPlatform)
	{
		if (!string.IsNullOrEmpty (libraryInfo.AttributeDll))
			return libraryInfo.AttributeDll!;

		return currentPlatform.GetPath ("lib", "Xamarin.Apple.BindingAttributes.dll");
	}

	public IEnumerable<string> GetLibraryDirectories (LibraryInfo libraryInfo, PlatformName currentPlatform)
	{
		foreach (var lib in Libraries)
			yield return lib;
	}

	public static bool DetermineSkipSystemDrawing (TargetFramework targetFramework) =>
		 targetFramework.Platform is ApplePlatform.MacOSX &&
			   (targetFramework == TargetFramework.Xamarin_Mac_2_0_Mobile ||
				targetFramework == TargetFramework.Xamarin_Mac_4_5_Full);

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

