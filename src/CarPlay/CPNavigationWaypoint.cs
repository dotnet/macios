#nullable enable

using System;
using Foundation;
using MapKit;

namespace CarPlay {

	public partial class CPNavigationWaypoint {

		public static unsafe CPNavigationWaypoint Create (CPLocationCoordinate3D centerPoint, NSMeasurement<NSUnitLength>? locationThreshold, string? name, string? address, CPLocationCoordinate3D []? entryPoints, NSTimeZone? timeZone)
		{
			if (entryPoints is null || entryPoints.Length == 0) {
				var obj = new CPNavigationWaypoint (NSObjectFlag.Empty);
				obj.InitializeHandle (obj._InitWithCenterPoint (centerPoint, locationThreshold, name, address, IntPtr.Zero, 0, timeZone));
				return obj;
			}

			fixed (CPLocationCoordinate3D* first = entryPoints) {
				var obj = new CPNavigationWaypoint (NSObjectFlag.Empty);
				obj.InitializeHandle (obj._InitWithCenterPoint (centerPoint, locationThreshold, name, address, (IntPtr) first, (nuint) entryPoints.Length, timeZone));
				return obj;
			}
		}

		public static unsafe CPNavigationWaypoint Create (MKMapItem mapItem, NSMeasurement<NSUnitLength>? locationThreshold, CPLocationCoordinate3D []? entryPoints)
		{
			if (entryPoints is null || entryPoints.Length == 0) {
				var obj = new CPNavigationWaypoint (NSObjectFlag.Empty);
				obj.InitializeHandle (obj._InitWithMapItem (mapItem, locationThreshold, IntPtr.Zero, 0));
				return obj;
			}

			fixed (CPLocationCoordinate3D* first = entryPoints) {
				var obj = new CPNavigationWaypoint (NSObjectFlag.Empty);
				obj.InitializeHandle (obj._InitWithMapItem (mapItem, locationThreshold, (IntPtr) first, (nuint) entryPoints.Length));
				return obj;
			}
		}

		public unsafe CPLocationCoordinate3D [] EntryPoints {
			get {
				nuint n = EntryPointsCount;
				if (n == 0)
					return [];
				var source = (CPLocationCoordinate3D*) _EntryPoints;
				var result = new CPLocationCoordinate3D [(int) n];
				for (int i = 0; i < (int) n; i++)
					result [i] = source [i];
				return result;
			}
		}
	}
}
