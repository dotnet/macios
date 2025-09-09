#if MTOUCH || MMP || BUNDLER
namespace Xamarin.Bundler {
#else
namespace ObjCRuntime {
#endif
	public static partial class Constants {
		/// <summary>Path to the System library to use with DllImport attributes.</summary>
		public const string libSystemLibrary = "/usr/lib/libSystem.dylib";

		/// <summary>Path to the libc library to use with DllImport attributes.</summary>
		public const string libcLibrary = "/usr/lib/libc.dylib";

		/// <summary>Path to the libobjc library to use with DllImport attributes.</summary>
		public const string ObjectiveCLibrary = "/usr/lib/libobjc.dylib";

		/// <summary>Path to the System library to use with DllImport attributes.</summary>
		public const string SystemLibrary = "/usr/lib/libSystem.dylib";

		/// <summary>Path to the libdispatch library to use with DllImport attributes.</summary>
		public const string libdispatchLibrary = "/usr/lib/system/libdispatch.dylib";

		/// <summary>Path to the libcompression library to use with DllImport attributes.</summary>
		public const string libcompressionLibrary = "/usr/lib/libcompression.dylib";

		/// <summary>Path to the vImage framework to use with DllImport attributes.</summary>
		public const string AccelerateImageLibrary = "/System/Library/Frameworks/Accelerate.framework/Frameworks/vImage.framework/vImage";

#if __MACCATALYST__ || __MACOS__
		/// <summary>Path to the ApplicationServices framework to use with DllImport attributes.</summary>
		public const string ApplicationServicesCoreGraphicsLibrary = "/System/Library/Frameworks/ApplicationServices.framework/Frameworks/CoreGraphics.framework/CoreGraphics";
#endif

		/// <summary>Path to the QuartzCore framework to use with DllImport attributes.</summary>
		public const string QuartzLibrary = "/System/Library/Frameworks/QuartzCore.framework/QuartzCore";

#if __MACOS__ && !XAMCORE_5_0
		/// <summary>Path to the InputMethodKit framework to use with DllImport attributes.</summary>
		public const string InputMethodKitLibrary = "/System/Library/Frameworks/InputMethodKit.framework/InputMethodKit";
#endif

#if __IOS__ && !__MACCATALYST__ && !XAMCORE_5_0
		// Apple removed the AssetsLibrary framework from iOS in Xcode 15.3.
		/// <summary>Path to the AssetsLibrary framework to use with DllImport attributes.</summary>
		public const string AssetsLibraryLibrary = "/System/Library/Frameworks/AssetsLibrary.framework/AssetsLibrary";

		// Apple removed the NewsstandKit framework from iOS in Xcode 15.
		/// <summary>Path to the NewsstandKit framework to use with DllImport attributes.</summary>
		public const string NewsstandKitLibrary = "/System/Library/Frameworks/NewsstandKit.framework/NewsstandKit";
#endif
	}
}
