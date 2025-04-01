using System;
using System.Globalization;
using ObjCRuntime;
using Foundation;

namespace UIKit {

	public partial class UIDevice {

#if NET
		/// <param name="major">To be added.</param>
		///         <param name="minor">To be added.</param>
		///         <summary>Whether the system version is greater than or equal to the specified major and minor values.</summary>
		///         <returns>
		///           <see langword="true" /> if the current system version is equal or greater than that specified in the arguments.</returns>
		///         <remarks>
		///           <para>This method returns <see langword="true" /> if the current version on the device is equal or greater than the version specified by <paramref name="major" /> and <paramref name="minor" />.</para>
		///           <para tool="threads">This can be used from a background thread.</para>
		///         </remarks>
		[SupportedOSPlatformGuard ("ios")]
		[SupportedOSPlatformGuard ("tvos")]
		[SupportedOSPlatformGuard ("maccatalyst")]
#endif
		public bool CheckSystemVersion (int major, int minor)
		{
#if __MACCATALYST__
			return Runtime.CheckSystemVersion (major, minor, Runtime.iOSSupportVersion);
#else
			return Runtime.CheckSystemVersion (major, minor, SystemVersion);
#endif
		}
	}
}
