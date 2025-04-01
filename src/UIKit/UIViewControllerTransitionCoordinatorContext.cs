//
// UIViewControllerTransitionCoordinatorContext.cs: Helper methods to make the class more usable
//
// Authors: miguel de icaza
//
// Copyright 2014 Xamarin
//

// Disable until we get around to enable + fix any issues.
#nullable disable

namespace UIKit {
	public static partial class UIViewControllerTransitionCoordinatorContext_Extensions {
		/// <param name="This">The instance on which this method operates.</param>
		///         <param name="kind">To be added.</param>
		///         <summary>Gets a view controller that controls a transition.</summary>
		///         <returns>To be added.</returns>
		///         <remarks>To be added.</remarks>
		public static UIView GetTransitionViewController (this IUIViewControllerTransitionCoordinatorContext This, UITransitionViewControllerKind kind)
		{
			switch (kind) {
			case UITransitionViewControllerKind.ToView:
				return This.GetTransitionViewControllerForKey (UITransitionContext.ToViewKey);
			case UITransitionViewControllerKind.FromView:
				return This.GetTransitionViewControllerForKey (UITransitionContext.FromViewKey);
			default:
				return null;
			}
		}
	}
}
