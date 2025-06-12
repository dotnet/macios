//
// ApplePlatform.cs
//
// Copyright 2020 Microsoft Corp. All Rights Reserved.

#nullable enable

using System;
using System.Text;

namespace Xamarin.Utils {
	[Flags]
	public enum ApplePlatform {
		None,
		MacOSX = 1,
		iOS = 2,
		[System.Obsolete ("Do not use")]
		WatchOS = 4,
		TVOS = 8,
		MacCatalyst = 16,
	}

	public static class ApplePlatformExtensions {
		public static string AsString (this ApplePlatform @this)
		{
			switch (@this) {
			case ApplePlatform.iOS:
				return "iOS";
			case ApplePlatform.MacOSX:
				return "macOS";
			case ApplePlatform.TVOS:
				return "tvOS";
			case ApplePlatform.MacCatalyst:
				return "MacCatalyst";
			case ApplePlatform.None:
				return "None";
			default:
				if (PopCount (unchecked((uint) @this)) == 1)
					return "Unknown";

				var value = @this;
				var sb = new StringBuilder ();
#if NET
				foreach (var e in Enum.GetValues<ApplePlatform> ()) {
#else
				foreach (var e in Enum.GetValues (typeof (ApplePlatform))) {
#endif
					var element = (ApplePlatform) e;
					if (element == ApplePlatform.None)
						continue;
					if ((value & element) == element) {
						if (sb.Length > 0)
							sb.Append (", ");
						sb.Append (element.AsString ());
						value &= ~element;
					}
				}
				return sb.ToString ();
			}
		}

		static int PopCount (uint value)
		{
#if NET
			return System.Numerics.BitOperations.PopCount (value);
#else
			int rv = 0;
			while (value != 0) {
				rv++;
				value &= value - 1;
			}
			return rv;
#endif
		}

		public static string ToFramework (this ApplePlatform @this, string? netVersion = null)
		{
			if (netVersion is null)
				netVersion = Xamarin.DotNetVersions.Tfm;

			switch (@this) {
			case ApplePlatform.iOS:
				return netVersion + "-ios";
			case ApplePlatform.MacOSX:
				return netVersion + "-macos";
			case ApplePlatform.TVOS:
				return netVersion + "-tvos";
			case ApplePlatform.MacCatalyst:
				return netVersion + "-maccatalyst";
			default:
				return "Unknown";
			}
		}

		public static ApplePlatform Parse (string platform)
		{
			switch (platform.ToLowerInvariant ()) {
			case "ios":
				return ApplePlatform.iOS;
			case "tvos":
				return ApplePlatform.TVOS;
			case "macos":
				return ApplePlatform.MacOSX;
			case "maccatalyst":
				return ApplePlatform.MacCatalyst;
			default:
				throw new System.InvalidOperationException ($"Unknown platform: {platform}");
			}
		}

		public static bool IsDesktop (this ApplePlatform @this)
		{
			switch (@this) {
			case ApplePlatform.iOS:
			case ApplePlatform.TVOS:
				return false;
			case ApplePlatform.MacOSX:
			case ApplePlatform.MacCatalyst:
				return true;
			default:
				throw new System.InvalidOperationException ($"Unknown platform: {@this}");
			}
		}
	}
}
