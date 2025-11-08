// 
// UIScreen.cs: Helper methods for UIScreen.
//
// Authors:
//   Miguel de Icaza
//     
// Copyright 2010 Novell, Inc
// Copyright 2014 Xamarin Inc.
//

using System.Collections;
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
			// This is from https://developer.apple.com/library/content/qa/qa1817/_index.html
			// Updated to use UIGraphicsImageRenderer to avoid deprecated UIGraphicsBeginImageContextWithOptions
			var view = UIApplication.SharedApplication.KeyWindow;
			using var renderer = new UIGraphicsImageRenderer (view.Bounds.Size);
			return renderer.CreateImage ((context) => {
				view.DrawViewHierarchy (view.Bounds, true);
			});
		}
	}
}
