//
// uikit.cs: Extensions for MonoTouch.UINavigationBar
//
// Copyright 2011, Xamarin, Inc.
//
// Author:
//  Miguel de Icaza
//

#if IOS

using System;
using Foundation;
using ObjCRuntime;
using CoreAnimation;
using CoreGraphics;
using MediaPlayer;

namespace UIKit {
	public partial class UINavigationBar {
		public partial class UINavigationBarAppearance {
			/// <summary>Display attributes for the bar's title.</summary>
			///         <returns>To be added.</returns>
			///         <remarks>To be added.</remarks>
			public virtual UITextAttributes GetTitleTextAttributes ()
			{
				return new UITextAttributes (_TitleTextAttributes);
			}

			/// <param name="attributes">To be added.</param>
			///         <summary>Sets the text attributes used by the title text.</summary>
			///         <remarks>To be added.</remarks>
			public virtual void SetTitleTextAttributes (UITextAttributes attributes)
			{
				if (attributes is null)
					throw new ArgumentNullException ("attributes");

				_TitleTextAttributes = attributes.ToDictionary ();
			}
		}
	}
}

#endif // IOS
