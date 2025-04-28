using System.Collections.Generic;
using System.Linq;
using ObjCRuntime;
using Xamarin.Utils;

#nullable enable

public class LibraryInfo {
	private LibraryInfo () { }
	public string? AttributeDll { get; set; }
	public TargetFramework TargetFramework { get; set; }
	public string? BaseLibDll { get; set; }
	public bool OmitStandardLibrary { get; set; }

	public static class LibraryInfoBuilder {
		public static LibraryInfo Build (List<string> refs, BindingTouchConfig config)
		{
			LibraryInfo libraryInfo = new ();
			SetTargetFramework (config.TargetFramework, libraryInfo);
			libraryInfo.OmitStandardLibrary = DetermineOmitStdLibrary (config.OmitStandardLibrary, libraryInfo.TargetFramework.Platform);
			libraryInfo.BaseLibDll = config.Baselibdll;
			libraryInfo.AttributeDll = config.Attributedll;
			return libraryInfo;
		}

		static void SetTargetFramework (string? fx, LibraryInfo libraryInfo)
		{
			if (fx is null)
				throw ErrorHelper.CreateError (86);
			TargetFramework tf;
			if (!TargetFramework.TryParse (fx, out tf))
				throw ErrorHelper.CreateError (68, fx);

			if (!TargetFramework.IsValidFramework (tf))
				throw ErrorHelper.CreateError (70, tf,
					string.Join (" ", TargetFramework.ValidFrameworks.Select ((v) => v.ToString ()).ToArray ()));

			libraryInfo.TargetFramework = tf;
		}

		static bool DetermineOmitStdLibrary (bool? omitStandardLibary, ApplePlatform currentPlatform)
		{
			if (omitStandardLibary is not null)
				return (bool) omitStandardLibary;

			switch (currentPlatform) {
			case ApplePlatform.iOS:
			case ApplePlatform.TVOS:
			case ApplePlatform.MacCatalyst:
			case ApplePlatform.MacOSX:
				return true;
			default:
				throw ErrorHelper.CreateError (1053, currentPlatform);
			}
		}
	}
}
