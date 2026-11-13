#nullable enable

using System;

namespace CarPlay {

	public partial class CPRouteSegment {

		/// <summary>Creates a navigation route segment with maneuvers, lane guidance, travel estimates, and route coordinates.</summary>
		/// <param name="origin">The starting waypoint.</param>
		/// <param name="destination">The ending waypoint.</param>
		/// <param name="maneuvers">The maneuvers along the segment.</param>
		/// <param name="laneGuidances">The lane guidance instructions along the segment.</param>
		/// <param name="currentManeuvers">The maneuvers currently presented to the driver.</param>
		/// <param name="currentLaneGuidance">The lane guidance currently presented to the driver.</param>
		/// <param name="tripTravelEstimates">The remaining travel estimates for the trip.</param>
		/// <param name="maneuverTravelEstimates">The travel estimates for the current maneuver.</param>
		/// <param name="coordinates">The ordered geographic coordinates and altitudes of the route segment. An empty array specifies no coordinates.</param>
		/// <returns>A new route segment.</returns>
		/// <exception cref="ArgumentNullException">Any parameter is <see langword="null" />.</exception>
		/// <remarks>The coordinates are copied during this call, so the array can be modified afterward without changing the segment.</remarks>
		public static CPRouteSegment Create (CPNavigationWaypoint origin, CPNavigationWaypoint destination, CPManeuver [] maneuvers, CPLaneGuidance [] laneGuidances, CPManeuver [] currentManeuvers, CPLaneGuidance currentLaneGuidance, CPTravelEstimates tripTravelEstimates, CPTravelEstimates maneuverTravelEstimates, CPLocationCoordinate3D [] coordinates)
		{
			if (coordinates is null)
				ObjCRuntime.ThrowHelper.ThrowArgumentNullException (nameof (coordinates));

			return new CPRouteSegment (origin, destination, maneuvers, laneGuidances, currentManeuvers, currentLaneGuidance, tripTravelEstimates, maneuverTravelEstimates, coordinates, (nint) coordinates.Length);
		}

		public unsafe CPLocationCoordinate3D [] Coordinates {
			get {
				var source = (CPLocationCoordinate3D*) _Coordinates;
				if (source is null)
					return [];
				nint n = CoordinatesCount;
				var result = new CPLocationCoordinate3D [(int) n];
				for (int i = 0; i < (int) n; i++)
					result [i] = source [i];
				return result;
			}
		}
	}
}
