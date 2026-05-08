//
// Unit tests for UIPageViewController
//
// Authors:
//	Sebastien Pouliot  <sebastien@xamarin.com>
//
// Copyright 2012 Xamarin Inc. All rights reserved.
//

#if !MONOMAC

using System.Reflection;
using UIKit;

namespace MonoTouchFixtures.UIKit {
	[TestFixture]
	[Preserve (AllMembers = true)]
	public class PageViewControllerTest {

		[Test]
		public void Defaults ()
		{
			UIPageViewController pvc = new UIPageViewController ();
			Assert.That (pvc.DataSource, Is.Null, "DataSource");
			Assert.That (pvc.Delegate, Is.Null, "Delegate");
			Assert.That (pvc.DoubleSided, Is.False, "DoubleSided");
			Assert.That (pvc.GestureRecognizers.Length, Is.EqualTo (2), "GestureRecognizers");
			Assert.That (pvc.GetNextViewController, Is.Null, "GetNextViewController");
			Assert.That (pvc.GetPreviousViewController, Is.Null, "GetPreviousViewController");
#if !__TVOS__
			Assert.That (pvc.GetSpineLocation, Is.Null, "GetSpineLocation");
#endif
			Assert.That (pvc.NavigationOrientation, Is.EqualTo (UIPageViewControllerNavigationOrientation.Horizontal), "NavigationOrientation");
			Assert.That (pvc.SpineLocation, Is.EqualTo (UIPageViewControllerSpineLocation.Min), "SpineLocation");
			Assert.That (pvc.TransitionStyle, Is.EqualTo (UIPageViewControllerTransitionStyle.PageCurl), "TransitionStyle");
			Assert.That (pvc.ViewControllers.Length, Is.EqualTo (0), "ViewControllers");
		}

		UIPageViewController pvc;

		[Test]
		public void SetViewControllers ()
		{
			pvc = new UIPageViewController ();
			// note: Complete is called synchronously
			pvc.SetViewControllers (pvc.ViewControllers, UIPageViewControllerNavigationDirection.Forward, false, Complete);
			Assert.That (pvc, Is.Null, "pvc");
		}

		void Complete (bool finished)
		{
			Assert.That (finished, Is.True, "finished");
			pvc = null;
		}
	}
}

#endif // !MONOMAC
