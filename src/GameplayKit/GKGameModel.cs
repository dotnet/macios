#nullable enable

using System;

namespace GameplayKit {

	/// <summary>Describes gameplay in a way that can be optimized with a <see cref="T:GameplayKit.GKMinMaxStrategist" />.</summary>
	///     <remarks>To be added.</remarks>
	public partial class GKGameModel {

		/// <summary>The maximum allowable score.</summary>
		///         <remarks>To be added.</remarks>
		public const int MaxScore = (1 << 24);
		/// <summary>The minimum allowable score.</summary>
		///         <remarks>To be added.</remarks>
		public const int MinScore = (-(1 << 24));
	}
}
