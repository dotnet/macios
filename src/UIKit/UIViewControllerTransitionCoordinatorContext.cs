//
// UIViewControllerTransitionCoordinatorContext.cs: Helper methods to make the class more usable
//
// Authors: miguel de icaza
//
// Copyright 2014 Xamarin
//

#nullable enable

namespace UIKit {
	public static partial class UIViewControllerTransitionCoordinatorContext_Extensions {
		/// <summary>Gets the view associated with a transition.</summary>
		/// <param name="This">The transition coordinator context.</param>
		/// <param name="kind">The kind of transition view to retrieve.</param>
		/// <returns>The transition <see cref="UIView" />, or <see langword="null" /> if <paramref name="kind" /> is not recognized.</returns>
		[SupportedOSPlatform ("tvos")]
		[SupportedOSPlatform ("ios")]
		[SupportedOSPlatform ("maccatalyst")]
		[UnsupportedOSPlatform ("macos")]
		public static UIView? GetTransitionViewController (this IUIViewControllerTransitionCoordinatorContext This, UITransitionViewControllerKind kind)
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
