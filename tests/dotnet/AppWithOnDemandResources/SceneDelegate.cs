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
	[Register ("SceneDelegate")]
	public class SceneDelegate : UIResponder, IUIWindowSceneDelegate {
		const string ResourceTag = "MusicTag";
		const string ResourceName = "SoundBank";
		const string ResourceExtension = "bin";

		UIViewController? viewController;
		NSBundleResourceRequest? request;

		[Export ("window")]
		public UIWindow? Window { get; set; }

		[Export ("scene:willConnectToSession:options:")]
		public void WillConnect (UIScene scene, UISceneSession session, UISceneConnectionOptions connectionOptions)
		{
			if (scene is not UIWindowScene windowScene)
				return;

			Window ??= new UIWindow (windowScene);

			viewController = new UIViewController ();
			SetColor (UIColor.Yellow);

			Window.RootViewController = viewController;
			Window.MakeKeyAndVisible ();

			BeginAccessingResources ();
		}

		void BeginAccessingResources ()
		{
			var tags = new NSSet<NSString> ((NSString) ResourceTag);
			request = new NSBundleResourceRequest (tags);
			request.BeginAccessingResources (error => {
				var success = error is null && CanReadResource ();
				Console.WriteLine ($"Accessing the on-demand resources completed. Error: {error?.LocalizedDescription ?? "none"} Success: {success}");
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
