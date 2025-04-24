//
// 
//

#if !XAMCORE_5_0

using System;
using System.Runtime.InteropServices;
using Foundation;
using ObjCRuntime;
using MapKit;

#nullable enable

namespace MapKit {
	public partial class MKOverlayView {

#if NET
		/// <param name="zoomScale">The current zoom factor of the map.</param>
		/// <summary>Application developers should not use this function in iOS 6 or later.</summary>
		/// <returns>The width, in screen points, of roads at the specified zoom scale.</returns>
		/// <remarks>Although not officially deprecated, Apple specifies that this method should not be used in iOS 6 or later.</remarks>
		[SupportedOSPlatform ("tvos")]
		[SupportedOSPlatform ("macos")]
		[SupportedOSPlatform ("ios")]
		[SupportedOSPlatform ("maccatalyst")]
		[ObsoletedOSPlatform ("ios", "Use 'MKOverlayRenderer.MKRoadWidthAtZoomScale' instead.")]
		[ObsoletedOSPlatform ("macos", "Use 'MKOverlayRenderer.MKRoadWidthAtZoomScale' instead.")]
		[ObsoletedOSPlatform ("tvos", "Use 'MKOverlayRenderer.MKRoadWidthAtZoomScale' instead.")]
		[ObsoletedOSPlatform ("maccatalyst", "Use 'MKOverlayRenderer.MKRoadWidthAtZoomScale' instead.")]
#else
		[Obsolete ("Use 'MKOverlayRenderer.MKRoadWidthAtZoomScale' instead.")]
#endif
		[DllImport (Constants.MapKitLibrary)]
		public static extern nfloat MKRoadWidthAtZoomScale (/* MKZoomScale */ nfloat zoomScale);
	}
}

#endif // !XAMCORE_5_0
