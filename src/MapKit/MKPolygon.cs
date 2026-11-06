using CoreLocation;

#nullable enable

namespace MapKit {

	public partial class MKPolygon {

		/// <summary>Creates a polygon from map points.</summary>
		/// <param name="points">The ordered vertices of the polygon.</param>
		/// <returns>A polygon containing the specified vertices, or an empty polygon if <paramref name="points" /> is empty.</returns>
		/// <exception cref="ArgumentNullException"><paramref name="points" /> is <see langword="null" />.</exception>
		/// <remarks>
		///   <para>The points are copied during this call, so the array can be modified afterward without changing the polygon.</para>
		///   <para tool="threads">This method can be called from a background thread.</para>
		/// </remarks>
		public static MKPolygon FromPoints (MKMapPoint [] points)
		{
			if (points is null)
				ObjCRuntime.ThrowHelper.ThrowArgumentNullException (nameof (points));
			return _FromPoints (points, points.Length);
		}

		/// <summary>Creates a polygon from map points, excluding the specified interior polygons.</summary>
		/// <param name="points">The ordered vertices of the polygon.</param>
		/// <param name="interiorPolygons">The polygons to exclude from the interior, or <see langword="null" /> for no exclusions.</param>
		/// <returns>A polygon containing the specified vertices and interior polygons.</returns>
		/// <exception cref="ArgumentNullException"><paramref name="points" /> is <see langword="null" />.</exception>
		/// <remarks>
		///   <para>The points are copied during this call. If <paramref name="points" /> is empty, the result is an empty polygon and <paramref name="interiorPolygons" /> is ignored.</para>
		///   <para tool="threads">This method can be called from a background thread.</para>
		/// </remarks>
		public static MKPolygon FromPoints (MKMapPoint [] points, MKPolygon [] interiorPolygons)
		{
			if (points is null)
				ObjCRuntime.ThrowHelper.ThrowArgumentNullException (nameof (points));
			if (points.Length == 0)
				return _FromPoints (points, 0);

			return _FromPoints (points, points.Length, interiorPolygons);
		}

		/// <summary>Creates a polygon from geographic coordinates.</summary>
		/// <param name="coords">The ordered coordinates of the polygon's vertices.</param>
		/// <returns>A polygon containing the specified coordinates, or an empty polygon if <paramref name="coords" /> is empty.</returns>
		/// <exception cref="ArgumentNullException"><paramref name="coords" /> is <see langword="null" />.</exception>
		/// <remarks>
		///   <para>The coordinates are copied during this call, so the array can be modified afterward without changing the polygon.</para>
		///   <para tool="threads">This method can be called from a background thread.</para>
		/// </remarks>
		public static MKPolygon FromCoordinates (CLLocationCoordinate2D [] coords)
		{
			if (coords is null)
				ObjCRuntime.ThrowHelper.ThrowArgumentNullException (nameof (coords));
			return _FromCoordinates (coords, coords.Length);
		}

		/// <summary>Creates a polygon from geographic coordinates, excluding the specified interior polygons.</summary>
		/// <param name="coords">The ordered coordinates of the polygon's vertices.</param>
		/// <param name="interiorPolygons">The polygons to exclude from the interior, or <see langword="null" /> for no exclusions.</param>
		/// <returns>A polygon containing the specified coordinates and interior polygons.</returns>
		/// <exception cref="ArgumentNullException"><paramref name="coords" /> is <see langword="null" />.</exception>
		/// <remarks>
		///   <para>The coordinates are copied during this call. If <paramref name="coords" /> is empty, the result is an empty polygon and <paramref name="interiorPolygons" /> is ignored.</para>
		///   <para tool="threads">This method can be called from a background thread.</para>
		/// </remarks>
		public static MKPolygon FromCoordinates (CLLocationCoordinate2D [] coords, MKPolygon [] interiorPolygons)
		{
			if (coords is null)
				ObjCRuntime.ThrowHelper.ThrowArgumentNullException (nameof (coords));
			if (coords.Length == 0)
				return _FromCoordinates (coords, 0);

			return _FromCoordinates (coords, coords.Length, interiorPolygons);
		}

	}
}
