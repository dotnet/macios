using System;
using System.Runtime.InteropServices;
using Foundation;
using CoreLocation;
using ObjCRuntime;

#nullable enable

namespace MapKit {

	public partial class MKMultiPoint {
		/// <summary>To be added.</summary>
		///         <value>To be added.</value>
		///         <remarks>To be added.</remarks>
		public unsafe MKMapPoint [] Points {
			get {
				var source = (MKMapPoint*) _Points;
				nint n = PointCount;
				var result = new MKMapPoint [n];
				for (int i = 0; i < n; i++)
					result [i] = source [i];

				return result;
			}
		}

		/// <param name="first">To be added.</param>
		///         <param name="count">To be added.</param>
		///         <summary>To be added.</summary>
		///         <returns>To be added.</returns>
		///         <remarks>To be added.</remarks>
		public unsafe CLLocationCoordinate2D [] GetCoordinates (int first, int count)
		{
			var range = new NSRange (first, count);
			var target = new CLLocationCoordinate2D [count];
			fixed (CLLocationCoordinate2D* firstE = target) {
				GetCoords ((IntPtr) firstE, range);
			}
			return target;
		}
	}
}
