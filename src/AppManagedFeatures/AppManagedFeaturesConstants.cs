#nullable enable

namespace AppManagedFeatures {

	public static partial class AppManagedFeaturesConstants {
		/// <summary>Gets the AppManagedFeatures framework version string.</summary>
		[SupportedOSPlatform ("ios27.0")]
		[UnsupportedSimulator ("ios")]
		[Field ("AppManagedFeaturesVersionString", "AppManagedFeatures")]
		public static string AppManagedFeaturesVersionString {
			get {
				// This symbol is an inline C array, so dlsym already points at its contents.
				var symbol = Dlfcn.GetIndirect (Libraries.AppManagedFeatures.Handle, "AppManagedFeaturesVersionString");
				return Marshal.PtrToStringUTF8 (symbol)
					?? throw new PlatformNotSupportedException ("The AppManagedFeatures version string is unavailable.");
			}
		}
	}
}
