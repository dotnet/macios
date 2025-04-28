#nullable enable

using System;

namespace HealthKit {
	public partial class HKSampleQuery {
		// #define HKObjectQueryNoLimit (0)
		// in iOS 9.3 SDK this was changed to `static const NSUInteger`
		/// <summary>Indicates to not limit the number of returned results.</summary>
		///         <remarks>To be added.</remarks>
		public const int NoLimit = 0;
	}
}
