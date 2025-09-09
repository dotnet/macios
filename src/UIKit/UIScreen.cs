// 
// UIScreen.cs: Helper methods for UIScreen.
//
// Authors:
//   Miguel de Icaza
//     
// Copyright 2010 Novell, Inc
// Copyright 2014 Xamarin Inc.
//

using System;
using System.Collections;
using Foundation;
using ObjCRuntime;
using CoreGraphics;

// Disable until we get around to enable + fix any issues.
#nullable disable

namespace UIKit {
	public partial class UIScreen {

		/// <param name="action">Delegate method to invoke when the screen needs to be updated.</param>
		///         <summary>Registers a method to be invoked whenever the display screen needs to be updated.</summary>
		///         <returns>The active display link that can be configured, read from and scheduled to deliver events.</returns>
		///         <remarks>To be added.</remarks>
		public CoreAnimation.CADisplayLink CreateDisplayLink (Action action)
		{
			if (action is null)
				throw new ArgumentNullException ("action");
			var d = new NSActionDispatcher (action);
			return CreateDisplayLink (d, NSActionDispatcher.Selector);
		}

		/// <summary>Captures a screenshot of the entire screen.</summary>
		/// <returns>A screenshot as a <see cref="UIKit.UIImage" />.</returns>
		/// <remarks>
		///   <para>
		///   This API will only capture UIKit and Quartz drawing,
		///   because it uses the screen's CALayer's RenderInContext
		///   method to perform the screenshot. It will not capture
		///   OpenGL ES or video content.
		///   </para>
		/// </remarks>
		public UIImage Capture ()
		{
			if (SystemVersion.CheckiOS (7, 0)) {
				// This is from https://developer.apple.com/library/content/qa/qa1817/_index.html
				try {
					var view = UIApplication.SharedApplication.KeyWindow;
					UIGraphics.BeginImageContextWithOptions (view.Bounds.Size, view.Opaque, 0);
					view.DrawViewHierarchy (view.Bounds, true);
					return UIGraphics.GetImageFromCurrentImageContext ();
				} finally {
					UIGraphics.EndImageContext ();
				}
			}

			// This is from: https://developer.apple.com/library/ios/#qa/qa2010/qa1703.html
			var selScreen = new Selector ("screen");
			var size = Bounds.Size;

			UIGraphics.BeginImageContextWithOptions (size, false, 0);

			try {
				var context = UIGraphics.GetCurrentContext ();

				foreach (var window in UIApplication.SharedApplication.Windows) {
					if (window.RespondsToSelector (selScreen) && window.Screen != this)
						continue;

					context.SaveState ();
					context.TranslateCTM (window.Center.X, window.Center.Y);
					context.ConcatCTM (window.Transform);
					context.TranslateCTM (-window.Bounds.Size.Width * window.Layer.AnchorPoint.X, -window.Bounds.Size.Height * window.Layer.AnchorPoint.Y);

					window.Layer.RenderInContext (context);
					context.RestoreState ();
				}

				return UIGraphics.GetImageFromCurrentImageContext ();
			} finally {
				UIGraphics.EndImageContext ();
			}
		}
	}
}
