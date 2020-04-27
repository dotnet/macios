using System;

using Microsoft.Build.Framework;
using Microsoft.Build.Utilities;

using Xamarin.Utils;

namespace Xamarin.MacDev.Tasks {
	public abstract class XamarinTask : Task {

		public string SessionId { get; set; }

		[Required]
		public string TargetFrameworkMoniker { get; set; }

		public string Product {
			get {
				switch (Platform) {
				case ApplePlatform.iOS:
				case ApplePlatform.TVOS:
				case ApplePlatform.WatchOS:
					return "Xamarin.iOS";
				case ApplePlatform.MacOSX:
					return "Xamarin.Mac";
				default:
					throw new InvalidOperationException ($"Invalid platform: {Platform}");
				}
			}
		}

		ApplePlatform? platform;
		public ApplePlatform Platform {
			get {
				if (!platform.HasValue)
					platform = PlatformFrameworkHelper.GetFramework (TargetFrameworkMoniker);
				return platform.Value;
			}
		}

		TargetFramework? target_framework;
		public TargetFramework TargetFramework {
			get {
				if (!target_framework.HasValue)
					target_framework = TargetFramework.Parse (TargetFrameworkMoniker);
				return target_framework.Value;
			}
		}

		public string PlatformName {
			get {
				switch (Platform) {
				case ApplePlatform.iOS:
					return "iOS";
				case ApplePlatform.TVOS:
					return "tvOS";
				case ApplePlatform.WatchOS:
					return "watchOS";
				case ApplePlatform.MacOSX:
					return "macOS";
				default:
					throw new InvalidOperationException ($"Invalid platform: {Platform}");
				}
			}
		}

		protected string GetSdkPlatform (bool isSimulator)
		{
			switch (Platform) {
			case ApplePlatform.iOS:
				return isSimulator ? "iPhoneSimulator" : "iPhoneOS";
			case ApplePlatform.TVOS:
				return isSimulator ? "AppleTVSimulator" : "AppleTVOS";
			case ApplePlatform.WatchOS:
				return isSimulator ? "WatchSimulator" : "WatchOS";
			case ApplePlatform.MacOSX:
				return "MacOSX";
			default:
				throw new InvalidOperationException ($"Invalid platform: {Platform}");
			}
		}
	}
}

