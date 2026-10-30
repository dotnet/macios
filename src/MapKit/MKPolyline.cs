using CoreLocation;

#nullable enable

namespace MapKit {

	public partial class MKPolyline {

		/// <param name="points">To be added.</param>
		///         <summary>To be added.</summary>
		///         <returns>To be added.</returns>
		///         <remarks>
		///           <para>(More documentation for this node is coming)</para>
		///           <para tool="threads">This can be used from a background thread.</para>
		///         </remarks>
		public static MKPolyline FromPoints (MKMapPoint [] points)
		{
			if (points is null)
				ObjCRuntime.ThrowHelper.ThrowArgumentNullException (nameof (points));
			return _FromPoints (points, points.Length);
		}

		/// <param name="coords">To be added.</param>
		///         <summary>To be added.</summary>
		///         <returns>To be added.</returns>
		///         <remarks>
		///           <para>(More documentation for this node is coming)</para>
		///           <para tool="threads">This can be used from a background thread.</para>
		///         </remarks>
		public static MKPolyline FromCoordinates (CLLocationCoordinate2D [] coords)
		{
			if (coords is null)
				ObjCRuntime.ThrowHelper.ThrowArgumentNullException (nameof (coords));
			return _FromCoordinates (coords, coords.Length);
		}
	}
}
