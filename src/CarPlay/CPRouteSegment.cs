#nullable enable

using System;

namespace CarPlay {

	public partial class CPRouteSegment {

		public static unsafe CPRouteSegment Create (CPNavigationWaypoint origin, CPNavigationWaypoint destination, CPManeuver [] maneuvers, CPLaneGuidance [] laneGuidances, CPManeuver [] currentManeuvers, CPLaneGuidance currentLaneGuidance, CPTravelEstimates tripTravelEstimates, CPTravelEstimates maneuverTravelEstimates, CPLocationCoordinate3D [] coordinates)
		{
			if (coordinates is null)
				ObjCRuntime.ThrowHelper.ThrowArgumentNullException (nameof (coordinates));
			if (coordinates.Length == 0)
				return new CPRouteSegment (origin, destination, maneuvers, laneGuidances, currentManeuvers, currentLaneGuidance, tripTravelEstimates, maneuverTravelEstimates, IntPtr.Zero, 0);

			fixed (CPLocationCoordinate3D* first = coordinates) {
				return new CPRouteSegment (origin, destination, maneuvers, laneGuidances, currentManeuvers, currentLaneGuidance, tripTravelEstimates, maneuverTravelEstimates, (IntPtr) first, (nint) coordinates.Length);
			}
		}

		public unsafe CPLocationCoordinate3D [] Coordinates {
			get {
				nint n = CoordinatesCount;
				if (n == 0)
					return [];
				var source = (CPLocationCoordinate3D*) _Coordinates;
				var result = new CPLocationCoordinate3D [(int) n];
				for (int i = 0; i < (int) n; i++)
					result [i] = source [i];
				return result;
			}
		}
	}
}
