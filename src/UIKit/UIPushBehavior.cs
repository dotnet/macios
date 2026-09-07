namespace UIKit {
	public partial class UIPushBehavior {
		/// <summary>Creates a push behavior with the specified mode and dynamic items.</summary>
		/// <param name="mode">The push behavior mode (continuous or instantaneous).</param>
		/// <param name="items">The dynamic items to apply the push behavior to.</param>
		public UIPushBehavior (UIPushBehaviorMode mode, params IUIDynamicItem [] items) : this (items, mode) { }
	}
}
