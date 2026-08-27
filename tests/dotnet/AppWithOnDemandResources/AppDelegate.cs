// Copyright (c) Microsoft Corporation.
// Licensed under the MIT License.

using System;

using Foundation;
using UIKit;

namespace AppWithOnDemandResources {
	// The app tries to access the on-demand resource tagged "MusicTag" (the SoundBank.bin file).
	// While the request is in progress the screen is yellow; it turns green if the resource could be
	// accessed and read, and red if anything went wrong. This makes it easy to verify at runtime that
	// on-demand resources actually work (in particular on the simulator).
	public partial class AppDelegate : UIApplicationDelegate {
		const string ResourceTag = "MusicTag";
		const string ResourceName = "SoundBank";
		const string ResourceExtension = "bin";

		UIWindow? window;
		UIViewController? viewController;
		NSBundleResourceRequest? request;

		public override bool FinishedLaunching (UIApplication app, NSDictionary? options)
		{
#pragma warning disable CA1422
			window = new UIWindow (UIScreen.MainScreen.Bounds);
#pragma warning restore CA1422

			viewController = new UIViewController ();
			SetColor (UIColor.Yellow);

			window.RootViewController = viewController;
			window.MakeKeyAndVisible ();

			BeginAccessingResources ();

			return true;
		}

		void BeginAccessingResources ()
		{
			var tags = new NSSet<NSString> ((NSString) ResourceTag);
			request = new NSBundleResourceRequest (tags);
			request.BeginAccessingResources (error => {
				var success = error is null && CanReadResource ();
				BeginInvokeOnMainThread (() => SetColor (success ? UIColor.Green : UIColor.Red));
			});
		}

		static bool CanReadResource ()
		{
			var path = NSBundle.MainBundle.PathForResource (ResourceName, ResourceExtension);
			if (string.IsNullOrEmpty (path))
				return false;
			using var data = NSData.FromFile (path);
			return data is not null;
		}

		void SetColor (UIColor color)
		{
			if (viewController?.View is UIView view)
				view.BackgroundColor = color;
		}
	}
}
