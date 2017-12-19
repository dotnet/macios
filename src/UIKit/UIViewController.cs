// 
// UIViewController.cs: Implements some nicer methods for UIViewController
//
// Authors:
//   Miguel de Icaza.
//     
// Copyright 2009 Novell, Inc
// Copyright 2013 Xamarin Inc. (http://xamarin.com)
//

#if !WATCH

using System;
using System.Collections;
using System.Collections.Generic;
using XamCore.Foundation; 
#if IOS
using XamCore.iAd;
#endif
using XamCore.ObjCRuntime;
using XamCore.CoreGraphics;

namespace XamCore.UIKit {
	public partial class UIViewController : IEnumerable {
		
		// https://bugzilla.xamarin.com/show_bug.cgi?id=3189
		static Stack<UIViewController> modal;
		
		static void PushModal (UIViewController controller)
		{
			if (modal == null)
				modal = new Stack<UIViewController> ();
			modal.Push (controller);
		}

		// DismissModalViewControllerAnimated can be called on on any controller in the hierarchy
		// note: if you dismiss something that is not in the hierarchy then you remove references to everything :(
		static void PopModal (UIViewController controller)
		{
			// handle the dismiss from the presenter
			// https://bugzilla.xamarin.com/show_bug.cgi?id=3489#c2
			if (modal == null || (modal.Count == 0))
				return;
			
			UIViewController pop = modal.Pop ();
			while (pop != controller && (modal.Count > 0)) {
				pop = modal.Pop ();
			}
		}

		public void Add (UIView view)
		{
			View.AddSubview (view);
		}

		public IEnumerator GetEnumerator ()
		{
			UIView [] subviews = View.Subviews;
			if (subviews == null)
				yield break;
			foreach (UIView uiv in subviews)
				yield return uiv;
		}

#if IOS
		// This is a [Category] -> C# extension method (see adlib.cs) but it targets on static selector
		// the resulting syntax does not look good in user code so we provide a better looking API
		// https://trello.com/c/iQpXOxCd/227-category-and-static-methods-selectors
		// note: we cannot reuse the same method name - as it would break compilation of existing apps
		static public void PrepareForInterstitialAds ()
		{
			(null as UIViewController).PrepareInterstitialAds ();
		}
#endif
	}
}

#endif // !WATCH
