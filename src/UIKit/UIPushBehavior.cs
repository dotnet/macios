namespace UIKit {
	public partial class UIPushBehavior {
		/// <param name="mode">The push behavior mode (continuous or instantaneous).</param>
		///         <param name="items">The dynamic items to apply the push behavior to.</param>
		///         <summary>To be added.</summary>
		public UIPushBehavior (UIPushBehaviorMode mode, params IUIDynamicItem [] items) : this (items, mode) { }
	}
}
