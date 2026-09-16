#nullable enable

using System;
using System.Runtime.Versioning;
using Foundation;
using MapKit;

namespace CarPlay {

	public partial class CPNavigationWaypoint {

		[SupportedOSPlatform ("ios26.4")]
		[UnsupportedOSPlatform ("tvos")]
		[UnsupportedOSPlatform ("macos")]
		[UnsupportedOSPlatform ("maccatalyst")]
		[ObsoletedOSPlatform ("ios27.0", "Use 'CreateWithVariants' instead.")]
		public static unsafe CPNavigationWaypoint Create (CPLocationCoordinate3D centerPoint, NSMeasurement<NSUnitLength>? locationThreshold, string? name, string? address, CPLocationCoordinate3D []? entryPoints, NSTimeZone? timeZone)
		{
			fixed (CPLocationCoordinate3D* first = entryPoints) {
				var obj = new CPNavigationWaypoint (NSObjectFlag.Empty);
				obj.InitializeHandle (obj._InitWithCenterPoint (centerPoint, locationThreshold, name, address, (IntPtr) first, (nuint) (entryPoints?.Length ?? 0), timeZone), "initWithCenterPoint:locationThreshold:name:address:entryPoints:entryPointsCount:timeZone:");
				return obj;
			}
		}

		/// <summary>Creates a waypoint with display name and address variants.</summary>
		/// <param name="centerPoint">The center point of the waypoint.</param>
		/// <param name="locationThreshold">The optional distance threshold for reaching the waypoint.</param>
		/// <param name="nameVariants">The waypoint display names, ordered from most to least preferred.</param>
		/// <param name="addressVariants">The waypoint addresses, ordered from most to least preferred.</param>
		/// <param name="entryPoints">The optional entry points for the waypoint.</param>
		/// <param name="timeZone">The optional time zone for the waypoint.</param>
		/// <returns>A new waypoint.</returns>
		[SupportedOSPlatform ("ios27.0")]
		[UnsupportedOSPlatform ("tvos")]
		[UnsupportedOSPlatform ("macos")]
		[UnsupportedOSPlatform ("maccatalyst")]
		public static unsafe CPNavigationWaypoint CreateWithVariants (CPLocationCoordinate3D centerPoint, NSMeasurement<NSUnitLength>? locationThreshold, string [] nameVariants, string [] addressVariants, CPLocationCoordinate3D []? entryPoints, NSTimeZone? timeZone)
		{
			ArgumentNullException.ThrowIfNull (nameVariants);
			ArgumentNullException.ThrowIfNull (addressVariants);

			fixed (CPLocationCoordinate3D* first = entryPoints) {
				return new CPNavigationWaypoint (centerPoint, locationThreshold, nameVariants, addressVariants, (IntPtr) first, (nuint) (entryPoints?.Length ?? 0), timeZone);
			}
		}

		[SupportedOSPlatform ("ios26.4")]
		[UnsupportedOSPlatform ("tvos")]
		[UnsupportedOSPlatform ("macos")]
		[UnsupportedOSPlatform ("maccatalyst")]
		public static unsafe CPNavigationWaypoint Create (MKMapItem mapItem, NSMeasurement<NSUnitLength>? locationThreshold, CPLocationCoordinate3D []? entryPoints)
		{
			fixed (CPLocationCoordinate3D* first = entryPoints) {
				var obj = new CPNavigationWaypoint (NSObjectFlag.Empty);
				obj.InitializeHandle (obj._InitWithMapItem (mapItem, locationThreshold, (IntPtr) first, (nuint) (entryPoints?.Length ?? 0)), "initWithMapItem:locationThreshold:entryPoints:entryPointsCount:");
				return obj;
			}
		}

		[SupportedOSPlatform ("ios26.4")]
		[UnsupportedOSPlatform ("tvos")]
		[UnsupportedOSPlatform ("macos")]
		[UnsupportedOSPlatform ("maccatalyst")]
		public unsafe CPLocationCoordinate3D [] EntryPoints {
			get {
				var source = (CPLocationCoordinate3D*) _EntryPoints;
				if (source is null)
					return [];
				nuint n = EntryPointsCount;
				var result = new CPLocationCoordinate3D [(int) n];
				for (int i = 0; i < (int) n; i++)
					result [i] = source [i];
				return result;
			}
		}
	}
}
