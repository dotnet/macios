using System;
using System.Globalization;
using ObjCRuntime;
using Foundation;

namespace UIKit {

	public partial class UIDevice {
		/// <summary>Checks whether the system version is greater than or equal to the specified major and minor values.</summary>
		/// <param name="major">The major system version to check for.</param>
		/// <param name="minor">The minor system version to check for.</param>
		/// <returns><see langword="true" /> if the current system version is equal or greater than that specified in the arguments.</returns>
		/// <remarks>
		///   <para>This method returns <see langword="true" /> if the current version on the device is equal or greater than the version specified by <paramref name="major" /> and <paramref name="minor" />.</para>
		///   <para tool="threads">This can be used from a background thread.</para>
		/// </remarks>
		[SupportedOSPlatformGuard ("ios")]
		[SupportedOSPlatformGuard ("tvos")]
		[SupportedOSPlatformGuard ("maccatalyst")]
		[SupportedOSPlatform ("ios")]
		[SupportedOSPlatform ("tvos")]
		[SupportedOSPlatform ("maccatalyst")]
		public bool CheckSystemVersion (int major, int minor)
		{
#if __MACCATALYST__
			return Runtime.CheckSystemVersion (major, minor, Runtime.iOSSupportVersion);
#else
#pragma warning disable CA1416 // This call site is reachable on all platforms. 'UIDevice.SystemVersion' is only supported on: 'ios' 12.2 and later, 'maccatalyst' 12.2 and later, 'macOS/OSX' 12.0 and later, 'tvos' 12.2 and later.
			return Runtime.CheckSystemVersion (major, minor, SystemVersion);
#pragma warning restore CA1416
#endif
		}
	}
}
