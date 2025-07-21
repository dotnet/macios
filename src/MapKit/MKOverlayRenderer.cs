using System;
using System.Runtime.InteropServices;
using Foundation;
using ObjCRuntime;
using MapKit;

#nullable enable

namespace MapKit {
	public partial class MKOverlayRenderer {
		[SupportedOSPlatform ("tvos")]
		[SupportedOSPlatform ("macos")]
		[SupportedOSPlatform ("ios")]
		[SupportedOSPlatform ("maccatalyst")]
		[DllImport (Constants.MapKitLibrary)]
		public static extern nfloat MKRoadWidthAtZoomScale (/* MKZoomScale */ nfloat zoomScale);
	}
}
