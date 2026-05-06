#nullable enable

using System;
using Foundation;
using MapKit;

namespace CarPlay {

	public partial class CPNavigationWaypoint {

		public static unsafe CPNavigationWaypoint Create (CPLocationCoordinate3D centerPoint, NSMeasurement<NSUnitLength>? locationThreshold, string? name, string? address, CPLocationCoordinate3D []? entryPoints, NSTimeZone? timeZone)
		{
			fixed (CPLocationCoordinate3D* first = entryPoints) {
				var obj = new CPNavigationWaypoint (NSObjectFlag.Empty);
				obj.InitializeHandle (obj._InitWithCenterPoint (centerPoint, locationThreshold, name, address, (IntPtr) first, (nuint) (entryPoints?.Length ?? 0), timeZone), "initWithCenterPoint:locationThreshold:name:address:entryPoints:entryPointsCount:timeZone:");
				return obj;
			}
		}

		public static unsafe CPNavigationWaypoint Create (MKMapItem mapItem, NSMeasurement<NSUnitLength>? locationThreshold, CPLocationCoordinate3D []? entryPoints)
		{
			fixed (CPLocationCoordinate3D* first = entryPoints) {
				var obj = new CPNavigationWaypoint (NSObjectFlag.Empty);
				obj.InitializeHandle (obj._InitWithMapItem (mapItem, locationThreshold, (IntPtr) first, (nuint) (entryPoints?.Length ?? 0)), "initWithMapItem:locationThreshold:entryPoints:entryPointsCount:");
				return obj;
			}
		}

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
