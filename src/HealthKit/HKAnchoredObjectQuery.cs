#nullable enable

namespace HealthKit {

	public partial class HKAnchoredObjectQuery {

		// #define HKAnchoredObjectQueryNoAnchor
		/// <summary>A sentinel value indicating that no anchor has been set, causing the query to return all matching samples.</summary>
		public const uint NoAnchor = 0;
	}
}
