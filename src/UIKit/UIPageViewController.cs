//
// UIPageViewController.cs: Helper methods for the UIPageViewController
//
// Copyright 2011-2012, Xamarin, Inc.
//
// Author:
//  Miguel de Icaza
//

using Foundation;

namespace UIKit {
	public partial class UIPageViewController {
		/// <param name="style">To be added.</param>
		///         <param name="navigationOrientation">To be added.</param>
		///         <param name="spineLocation">To be added.</param>
		///         <summary>Creates an initialized <see cref="UIKit.UIPageViewController" /> object by using a <see cref="UIKit.UIPageViewControllerTransitionStyle" /> of transition between pages, a <see cref="UIKit.UIPageViewControllerNavigationOrientation" /> orientation of navigation, and a <paramref name="spineLocation" />.</summary>
		///         <remarks>To be added.</remarks>
		public UIPageViewController (UIPageViewControllerTransitionStyle style, UIPageViewControllerNavigationOrientation navigationOrientation, UIPageViewControllerSpineLocation spineLocation) : this (style, navigationOrientation, NSDictionary.FromObjectsAndKeys (new object [] { spineLocation }, new object [] { OptionSpineLocationKey }))
		{
		}

		/// <param name="style">To be added.</param>
		///         <param name="navigationOrientation">To be added.</param>
		///         <param name="spineLocation">To be added.</param>
		///         <param name="interPageSpacing">To be added.</param>
		///         <summary>To be added.</summary>
		///         <remarks>To be added.</remarks>
		public UIPageViewController (UIPageViewControllerTransitionStyle style, UIPageViewControllerNavigationOrientation navigationOrientation, UIPageViewControllerSpineLocation spineLocation, float interPageSpacing) : this (style, navigationOrientation, NSDictionary.FromObjectsAndKeys (new object [] { spineLocation, interPageSpacing }, new object [] { OptionSpineLocationKey, OptionInterPageSpacingKey }))
		{
		}

		/// <param name="style">To be added.</param>
		///         <param name="navigationOrientation">To be added.</param>
		///         <summary>To be added.</summary>
		///         <remarks>To be added.</remarks>
		public UIPageViewController (UIPageViewControllerTransitionStyle style, UIPageViewControllerNavigationOrientation navigationOrientation) : this (style, navigationOrientation, NSDictionary.FromObjectsAndKeys (new object [] { UIPageViewControllerSpineLocation.Mid }, new object [] { OptionSpineLocationKey }))
		{
		}

	}
}
