//
// Authors:
//   Miguel de Icaza
//
// Copyright 2011-2014 Xamarin Inc.
//

#if !TVOS


using Foundation;
using CoreLocation;
using ObjCRuntime;

#nullable enable

namespace MapKit {

	// it's similar to MKDirectionsTransportType values but it's something only used on the managed side
	// to replace NSString fields
	/// <summary>An enumeration of travel methods for which directions can be provided.</summary>
	///     <remarks>
	///     </remarks>
	public enum MKDirectionsMode {
		/// <summary>Driving directions.</summary>
		Driving,
		/// <summary>Walking directions.</summary>
		Walking,
		/// <summary>Transit directions.</summary>
		Transit,
#if NET
		/// <summary>The user's preferred direction type.</summary>
		[SupportedOSPlatform ("ios")]
		[SupportedOSPlatform ("macos")]
		[SupportedOSPlatform ("maccatalyst")]
		[UnsupportedOSPlatform ("tvos")]
#else
		[NoTV]
#endif
		Default,
	}

#if NET
	/// <summary>Encapsulates properties to be used with <see cref="MapKit.MKMapItem.OpenInMaps(MapKit.MKLaunchOptions)" />.</summary>
	///     <remarks>
	///     </remarks>
	[SupportedOSPlatform ("ios")]
	[SupportedOSPlatform ("maccatalyst")]
	[SupportedOSPlatform ("macos")]
#endif
	public class MKLaunchOptions {
		/// <summary>The kind of directions that you want to show the user (walking, driving)</summary>
		///         <value>If specified, the map items provided represent a starting and ending point.</value>
		///         <remarks>
		///         </remarks>
		public MKDirectionsMode? DirectionsMode { get; set; }
		/// <summary>Specifies the desired type of map to render (standard, satellite, hybrid).</summary>
		///         <value>
		///         </value>
		///         <remarks>
		///         </remarks>
		public MKMapType? MapType { get; set; }
		/// <summary>The location where the map should be centered</summary>
		///         <value>
		///         </value>
		///         <remarks>
		///         </remarks>
		public CLLocationCoordinate2D? MapCenter { get; set; }
		/// <summary>Coordinate span for the region to be displayed by the maps app.</summary>
		///         <value>To be added.</value>
		///         <remarks>To be added.</remarks>
		public MKCoordinateSpan? MapSpan { get; set; }
		/// <summary>Controls whether to display traffic information on the map.</summary>
		///         <value>
		///         </value>
		///         <remarks>
		///         </remarks>
		public bool? ShowTraffic { get; set; }


#if NET
		/// <summary>Virtual camera, used to show a 3D perspective of the map.</summary>
		///         <value>If not set, the Maps app will use the current defaults for the camera.</value>
		///         <remarks>
		///         </remarks>
		[SupportedOSPlatform ("ios")]
		[SupportedOSPlatform ("maccatalyst")]
		[SupportedOSPlatform ("macos")]
#endif
		public MKMapCamera? Camera { get; set; }

		internal NSDictionary? ToDictionary ()
		{
			int n = 0;
			if (DirectionsMode.HasValue) n++;
			if (MapType.HasValue) n++;
			if (MapCenter.HasValue) n++;
			if (MapSpan.HasValue) n++;
			if (ShowTraffic.HasValue) n++;
			if (Camera is not null) n++;
			if (n == 0)
				return null;

			var keys = new NSObject [n];
			var values = new NSObject [n];
			int i = 0;
			if (DirectionsMode.HasValue) {
				keys [i] = MKMapItem.MKLaunchOptionsDirectionsModeKey;
				NSString v = MKMapItem.MKLaunchOptionsDirectionsModeDriving;
				switch (DirectionsMode.Value) {
				case MKDirectionsMode.Driving:
					v = MKMapItem.MKLaunchOptionsDirectionsModeDriving;
					break;
				case MKDirectionsMode.Transit:
					v = MKMapItem.MKLaunchOptionsDirectionsModeTransit;
					break;
				case MKDirectionsMode.Walking:
					v = MKMapItem.MKLaunchOptionsDirectionsModeWalking;
					break;
#if !TV
				case MKDirectionsMode.Default:
					v = MKMapItem.MKLaunchOptionsDirectionsModeDefault;
					break;
#endif
				}
				values [i++] = v;
			}

			if (MapType.HasValue) {
				keys [i] = MKMapItem.MKLaunchOptionsMapTypeKey;
				values [i++] = new NSNumber ((int) MapType.Value);
			}
			if (MapCenter.HasValue) {
				keys [i] = MKMapItem.MKLaunchOptionsMapCenterKey;
				values [i++] = NSValue.FromMKCoordinate (MapCenter.Value);
			}
			if (MapSpan.HasValue) {
				keys [i] = MKMapItem.MKLaunchOptionsMapSpanKey;
				values [i++] = NSValue.FromMKCoordinateSpan (MapSpan.Value);
			}
			if (ShowTraffic.HasValue) {
				keys [i] = MKMapItem.MKLaunchOptionsShowsTrafficKey;
				values [i++] = new NSNumber (ShowTraffic.Value);
			}
			if (Camera is not null) {
				keys [i] = MKMapItem.MKLaunchOptionsCameraKey;
				values [i++] = Camera;
			}
			return NSDictionary.FromObjectsAndKeys (values, keys);
		}
	}

	public partial class MKMapItem {
		/// <param name="launchOptions">To be added.</param>
		///         <summary>To be added.</summary>
		///         <remarks>To be added.</remarks>
		public void OpenInMaps (MKLaunchOptions? launchOptions = null)
		{
			_OpenInMaps (launchOptions?.ToDictionary ());
		}

		/// <param name="mapItems">To be added.</param>
		///         <param name="launchOptions">To be added.</param>
		///         <summary>To be added.</summary>
		///         <returns>To be added.</returns>
		///         <remarks>To be added.</remarks>
		public static bool OpenMaps (MKMapItem [] mapItems, MKLaunchOptions? launchOptions = null)
		{
			return _OpenMaps (mapItems, launchOptions?.ToDictionary ());
		}
	}

}
#endif
