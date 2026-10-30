using CoreLocation;

#nullable enable

namespace MapKit {

	public partial class MKPolyline {

		/// <summary>Creates a polyline from map points.</summary>
		/// <param name="points">The ordered vertices of the polyline.</param>
		/// <returns>A polyline containing the specified vertices, or an empty polyline if <paramref name="points" /> is empty.</returns>
		/// <exception cref="ArgumentNullException"><paramref name="points" /> is <see langword="null" />.</exception>
		/// <remarks>
		///   <para>The points are copied during this call, so the array can be modified afterward without changing the polyline.</para>
		///   <para tool="threads">This method can be called from a background thread.</para>
		/// </remarks>
		public static MKPolyline FromPoints (MKMapPoint [] points)
		{
			if (points is null)
				ObjCRuntime.ThrowHelper.ThrowArgumentNullException (nameof (points));
			return _FromPoints (points, points.Length);
		}

		/// <summary>Creates a polyline from geographic coordinates.</summary>
		/// <param name="coords">The ordered coordinates of the polyline's vertices.</param>
		/// <returns>A polyline containing the specified coordinates, or an empty polyline if <paramref name="coords" /> is empty.</returns>
		/// <exception cref="ArgumentNullException"><paramref name="coords" /> is <see langword="null" />.</exception>
		/// <remarks>
		///   <para>The coordinates are copied during this call, so the array can be modified afterward without changing the polyline.</para>
		///   <para tool="threads">This method can be called from a background thread.</para>
		/// </remarks>
		public static MKPolyline FromCoordinates (CLLocationCoordinate2D [] coords)
		{
			if (coords is null)
				ObjCRuntime.ThrowHelper.ThrowArgumentNullException (nameof (coords));
			return _FromCoordinates (coords, coords.Length);
		}
	}
}
