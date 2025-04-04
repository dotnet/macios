using System;
using System.Runtime.InteropServices;
using Foundation;
using CoreLocation;
using ObjCRuntime;

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
		public static unsafe MKPolyline FromPoints (MKMapPoint [] points)
		{
			if (points is null)
				ObjCRuntime.ThrowHelper.ThrowArgumentNullException (nameof (points));
			if (points.Length == 0)
				return _FromPoints (IntPtr.Zero, 0);

			fixed (MKMapPoint* first = points) {
				return _FromPoints ((IntPtr) first, points.Length);
			}
		}

		/// <param name="coords">To be added.</param>
		///         <summary>To be added.</summary>
		///         <returns>To be added.</returns>
		///         <remarks>
		///           <para>(More documentation for this node is coming)</para>
		///           <para tool="threads">This can be used from a background thread.</para>
		///         </remarks>
		public static unsafe MKPolyline FromCoordinates (CLLocationCoordinate2D [] coords)
		{
			if (coords is null)
				ObjCRuntime.ThrowHelper.ThrowArgumentNullException (nameof (coords));
			if (coords.Length == 0)
				return _FromCoordinates (IntPtr.Zero, 0);

			fixed (CLLocationCoordinate2D* first = coords) {
				return _FromCoordinates ((IntPtr) first, coords.Length);
			}
		}
	}
}
