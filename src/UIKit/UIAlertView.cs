//
// UIAlertView.cs: Eventsion to UIAlertView
//
// Authors:
//   Miguel de Icaza
//
// Copyright 2009, Novell, Inc.
// Copyright 2015 Xamarin Inc.
//

#if IOS

using System;
using ObjCRuntime;
using Foundation;

namespace UIKit {
	public partial class UIAlertView {
		/// <summary>Constructor to initialize an alert view.</summary>
		/// <param name="title">The string that is displayed in the alert view's title bar.</param>
		/// <param name="message">A more desriptive string that appears in the alert view below the title.</param>
		/// <param name="del">The alert view's delegate.</param>
		/// <param name="cancelButtonTitle">The string that appears in the cancel button.</param>
		/// <param name="otherButtons">Titles of any additional buttons.</param>
		/// <remarks>This constructor is provided to make it possible to fully initialize an alert view when it is created.</remarks>
		public UIAlertView (string title, string message, IUIAlertViewDelegate del, string cancelButtonTitle, params string [] otherButtons)
			: this (title, message, del, cancelButtonTitle, otherButtons is null || otherButtons.Length == 0 ? IntPtr.Zero : new NSString (otherButtons [0]).DangerousRetain().DangerousAutorelease().Handle, IntPtr.Zero, IntPtr.Zero)
		{
			if (otherButtons is null)
				return;

			// first button, if present, was already added
			for (int i = 1; i < otherButtons.Length; i++)
				AddButton (otherButtons [i]);
		}
	}
}

#endif // IOS
