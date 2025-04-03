using System;
using System.Runtime.InteropServices;
using Foundation;
using CoreLocation;
using ObjCRuntime;

#nullable enable

namespace MapKit {

	public partial class MKPolygon {

		/// <param name="points">An array of <see cref="T:MapKit.MKMapPoint" />s that define the polygon.</param>
		///         <summary>Creates an <see cref="T:MapKit.MKPolygon" /> from the specified <paramref name="points" />.</summary>
		///         <returns>To be added.</returns>
		///         <remarks>
		///           <para>(More documentation for this node is coming)</para>
		///           <para tool="threads">This can be used from a background thread.</para>
		///         </remarks>
		public static unsafe MKPolygon FromPoints (MKMapPoint [] points)
		{
			if (points is null)
				ObjCRuntime.ThrowHelper.ThrowArgumentNullException (nameof (points));
			if (points.Length == 0)
				return _FromPoints (IntPtr.Zero, 0);

			fixed (MKMapPoint* first = points) {
				return _FromPoints ((IntPtr) first, points.Length);
			}
		}

		/// <param name="points">An array of <see cref="T:MapKit.MKMapPoint" />s that define the polygon.</param>
		///         <param name="interiorPolygons">An array of <see cref="T:MapKit.MKPolygon" />s that should be excluded from the polygon's interior.</param>
		///         <summary>Creates an <see cref="T:MapKit.MKPolygon" /> from the specified <paramref name="points" />, excluding the specified <paramref name="interiorPolygons" />.</summary>
		///         <returns>To be added.</returns>
		///         <remarks>
		///           <para>(More documentation for this node is coming)</para>
		///           <para tool="threads">This can be used from a background thread.</para>
		///         </remarks>
		public static unsafe MKPolygon FromPoints (MKMapPoint [] points, MKPolygon [] interiorPolygons)
		{
			if (points is null)
				ObjCRuntime.ThrowHelper.ThrowArgumentNullException (nameof (points));
			if (points.Length == 0)
				return _FromPoints (IntPtr.Zero, 0);

			fixed (MKMapPoint* first = points) {
				return _FromPoints ((IntPtr) first, points.Length, interiorPolygons);
			}
		}

		/// <param name="coords">An array of <see cref="T:CoreLocation.CLLocationCoordinate2D" />s that define the desired polygon.</param>
		///         <summary>Creates an <see cref="T:MapKit.MKPolygon" /> from the specified <paramref name="coords" />.</summary>
		///         <returns>To be added.</returns>
		///         <remarks>
		///           <para>(More documentation for this node is coming)</para>
		///           <para tool="threads">This can be used from a background thread.</para>
		///         </remarks>
		public static unsafe MKPolygon FromCoordinates (CLLocationCoordinate2D [] coords)
		{
			if (coords is null)
				ObjCRuntime.ThrowHelper.ThrowArgumentNullException (nameof (coords));
			if (coords.Length == 0)
				return _FromCoordinates (IntPtr.Zero, 0);

			fixed (CLLocationCoordinate2D* first = coords) {
				return _FromCoordinates ((IntPtr) first, coords.Length);
			}
		}

		/// <param name="coords">An array of <see cref="T:CoreLocation.CLLocationCoordinate2D" />s that define the desired polygon.</param>
		///         <param name="interiorPolygons">An array of <see cref="T:MapKit.MKPolygon" />s that should be excluded from the polygon's interior.</param>
		///         <summary>Creates an <see cref="T:MapKit.MKPolygon" /> from the specified <paramref name="coords" />, excluding the specified <paramref name="interiorPolygons" />.</summary>
		///         <returns>To be added.</returns>
		///         <remarks>
		///           <para>(More documentation for this node is coming)</para>
		///           <para tool="threads">This can be used from a background thread.</para>
		///         </remarks>
		public static unsafe MKPolygon FromCoordinates (CLLocationCoordinate2D [] coords, MKPolygon [] interiorPolygons)
		{
			if (coords is null)
				ObjCRuntime.ThrowHelper.ThrowArgumentNullException (nameof (coords));
			if (coords.Length == 0)
				return _FromCoordinates (IntPtr.Zero, 0);

			fixed (CLLocationCoordinate2D* first = coords) {
				return _FromCoordinates ((IntPtr) first, coords.Length, interiorPolygons);
			}
		}

	}
}
