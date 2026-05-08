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
			ClassicAssert.Null (pvc.DataSource, "DataSource");
			ClassicAssert.Null (pvc.Delegate, "Delegate");
			ClassicAssert.False (pvc.DoubleSided, "DoubleSided");
			Assert.That (pvc.GestureRecognizers.Length, Is.EqualTo (2), "GestureRecognizers");
			ClassicAssert.Null (pvc.GetNextViewController, "GetNextViewController");
			ClassicAssert.Null (pvc.GetPreviousViewController, "GetPreviousViewController");
#if !__TVOS__
			ClassicAssert.Null (pvc.GetSpineLocation, "GetSpineLocation");
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
			ClassicAssert.Null (pvc, "pvc");
		}

		void Complete (bool finished)
		{
			ClassicAssert.True (finished, "finished");
			pvc = null;
		}
	}
}

#endif // !MONOMAC
