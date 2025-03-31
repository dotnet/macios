//
// CoreMotion's extra methods
//

#nullable enable

using Foundation;
using System;

namespace CoreMotion {

	public partial class CMAccelerometerData {
		/// <summary>Returns a string representation of the value of the current instance.</summary>
		///         <returns>
		///         </returns>
		///         <remarks>
		///         </remarks>
		public override string ToString ()
		{
			return String.Format ("t={0} {1}", Acceleration.ToString (), Timestamp);
		}
	}
}
