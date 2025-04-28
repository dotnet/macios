using System;
using Xamarin.Utils;

namespace Xamarin.Tests {
	public enum Profile {
		None,
		iOS,
		tvOS,
		watchOS,
		macOSMobile,
		MacCatalyst,
	}

	public static class ProfileExtensions {
		public static ApplePlatform AsPlatform (this Profile profile)
		{
			switch (profile) {
			case Profile.iOS:
				return ApplePlatform.iOS;
			case Profile.tvOS:
				return ApplePlatform.TVOS;
			case Profile.macOSMobile:
				return ApplePlatform.MacOSX;
			case Profile.MacCatalyst:
				return ApplePlatform.MacCatalyst;
			case Profile.None:
			default:
				throw new NotImplementedException (profile.ToString ());
			}
		}

		public static Profile AsProfile (this ApplePlatform platform)
		{
			switch (platform) {
			case ApplePlatform.iOS:
				return Profile.iOS;
			case ApplePlatform.MacCatalyst:
				return Profile.MacCatalyst;
			case ApplePlatform.MacOSX:
				return Profile.macOSMobile;
			case ApplePlatform.None:
				return Profile.None;
			case ApplePlatform.TVOS:
				return Profile.tvOS;
			default:
				throw new NotImplementedException (platform.ToString ());
			}
		}
	}
}
