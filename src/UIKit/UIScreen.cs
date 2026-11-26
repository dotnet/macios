// 
// UIScreen.cs: Helper methods for UIScreen.
//
// Authors:
//   Miguel de Icaza
//     
// Copyright 2010 Novell, Inc
// Copyright 2014 Xamarin Inc.
//

#nullable enable

namespace UIKit {
	public partial class UIScreen {

		/// <summary>Registers a method to be invoked whenever the display screen needs to be updated.</summary>
		/// <param name="action">Delegate method to invoke when the screen needs to be updated.</param>
		/// <returns>The active <see cref="CoreAnimation.CADisplayLink" /> that can be configured, read from, and scheduled to deliver events, or <see langword="null" /> if the display link could not be created.</returns>
		[SupportedOSPlatform ("ios")]
		[SupportedOSPlatform ("tvos")]
		[SupportedOSPlatform ("maccatalyst")]
		[ObsoletedOSPlatform ("ios27.0", "Use the equivalent display link API on 'UIWindowScene' instead.")]
		[ObsoletedOSPlatform ("tvos27.0", "Use the equivalent display link API on 'UIWindowScene' instead.")]
		[ObsoletedOSPlatform ("maccatalyst27.0", "Use the equivalent display link API on 'UIWindowScene' instead.")]
		public CoreAnimation.CADisplayLink? CreateDisplayLink (Action action)
		{
			ArgumentNullException.ThrowIfNull (action);
			var d = new NSActionDispatcher (action);
			return CreateDisplayLink (d, NSActionDispatcher.Selector);
		}

		/// <summary>Captures a screenshot of the entire screen.</summary>
		/// <returns>A screenshot as a <see cref="UIImage" />.</returns>
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
#pragma warning disable CA1422 // This call site is reachable on: 'ios' 15.0 and later, 'maccatalyst' 15.0 and later. 'UIApplication.KeyWindow' is obsoleted on: 'ios' 13.0 and later (Should not be used for applications that support multiple scenes because it returns a key window across all connected scenes.), 'maccatalyst' 13.0 and later (Should not be used for applications that support multiple scenes because it returns a key window across all connected scenes.).
			var view = UIApplication.SharedApplication.KeyWindow;
#pragma warning restore CA1422
			if (view is null)
				throw new InvalidOperationException ("No key window is available to capture.");
			using var renderer = new UIGraphicsImageRenderer (view.Bounds.Size);
			return renderer.CreateImage ((context) => {
				view.DrawViewHierarchy (view.Bounds, true);
			});
		}
	}
}
