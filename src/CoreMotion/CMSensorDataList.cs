//
// CMSensorDataList.cs:
//
// Copyright 2015, Xamarin Inc.
//
// Authors:
//   Rolf Bjarne Kvinge
//

#nullable enable

#if !MONOMAC

using System.Collections.Generic;
using System.Runtime.InteropServices;

using Foundation;

namespace CoreMotion {
	public partial class CMSensorDataList : IEnumerable<CMAccelerometerData> {
		#region IEnumerable implementation
		/// <summary>Gets an enumerator for iterating over accelerometer data.</summary>
		///         <returns>An enumerator for iterating over accelerometer data.</returns>
		///         <remarks>To be added.</remarks>
		public IEnumerator<CMAccelerometerData> GetEnumerator ()
		{
			return new NSFastEnumerator<CMAccelerometerData> (this);
		}
		#endregion

		#region IEnumerable implementation
		/// <summary>For internal use only.</summary>
		///         <returns>To be added.</returns>
		///         <remarks>To be added.</remarks>
		System.Collections.IEnumerator System.Collections.IEnumerable.GetEnumerator ()
		{
			return GetEnumerator ();
		}
		#endregion
	}
}

#endif
