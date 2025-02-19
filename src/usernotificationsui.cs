//
// UserNotificationsUI bindings
//
// Authors:
//	Alex Soto  <alex.soto@xamarin.com>
//
// Copyright 2016 Xamarin Inc. All rights reserved.
//

using System;
using CoreGraphics;
using Foundation;
using ObjCRuntime;
#if MONOMAC
using AppKit;
using UIColor = AppKit.NSColor;
#else
using UIKit;
#endif
using UserNotifications;

namespace UserNotificationsUI {

	/// <summary>Enumerates the kinds of media control button sets that the system will provide for playable notification content.</summary>
	[Introduced (PlatformName.MacCatalyst, 14, 0)]
	[Unavailable (PlatformName.TvOS)]
	[Native]
	public enum UNNotificationContentExtensionMediaPlayPauseButtonType : ulong {
		/// <summary>Indicates that no playback controls should be overlayed on top of the notification content.</summary>
		None,
		/// <summary>Indicates that the default playback control display method should be used.</summary>
		Default,
		/// <summary>Indicates that playback controls should be overlayed on top of the notification content.</summary>
		Overlay,
	}

	/// <summary>Enumerates preferred notification responses.</summary>
	[Introduced (PlatformName.MacCatalyst, 14, 0)]
	[Unavailable (PlatformName.TvOS)]
	[Native]
	public enum UNNotificationContentExtensionResponseOption : ulong {
		/// <summary>Indicates that the notification interface will not be dismissed, and that the content extension will handle the action.</summary>
		DoNotDismiss,
		/// <summary>Indicates that the notification interface will be dismissed, and that the content extension will handle the action.</summary>
		Dismiss,
		/// <summary>Indicates that the notification interface will be dismissed, and that the notification will be forwarded to the app for it to respond.</summary>
		DismissAndForwardAction,
	}

	interface IUNNotificationContentExtension { }

	/// <summary>Interface that contains the mandatory methods, if any, for the <see cref="T:UserNotifications.UNNotificationContent" /> protocol, which is required for displaying notification content from a view controller's view with custom UI.</summary>
	[Introduced (PlatformName.MacCatalyst, 14, 0)]
	[Unavailable (PlatformName.TvOS)]
	[Protocol]
	interface UNNotificationContentExtension {

		[Abstract]
		[Export ("didReceiveNotification:")]
		void DidReceiveNotification (UNNotification notification);

		[Export ("didReceiveNotificationResponse:completionHandler:")]
		void DidReceiveNotificationResponse (UNNotificationResponse response, Action<UNNotificationContentExtensionResponseOption> completion);

		[Export ("mediaPlayPauseButtonType", ArgumentSemantic.Assign)]
		UNNotificationContentExtensionMediaPlayPauseButtonType MediaPlayPauseButtonType { get; }

		[Export ("mediaPlayPauseButtonFrame", ArgumentSemantic.Assign)]
		CGRect MediaPlayPauseButtonFrame { get; }

		[Export ("mediaPlayPauseButtonTintColor", ArgumentSemantic.Copy)]
		UIColor MediaPlayPauseButtonTintColor { get; }

		[Export ("mediaPlay")]
		void PlayMedia ();

		[Export ("mediaPause")]
		void PauseMedia ();
	}

	/// <summary>Content extension for presenting notification UI, including UI for playable content.</summary>
	[Introduced (PlatformName.MacCatalyst, 14, 0)]
	[Unavailable (PlatformName.TvOS)]
	[Category]
	[BaseType (typeof (NSExtensionContext))]
	interface NSExtensionContext_UNNotificationContentExtension {

		[Export ("mediaPlayingStarted")]
		void MediaPlayingStarted ();

		[Export ("mediaPlayingPaused")]
		void MediaPlayingPaused ();

		[MacCatalyst (14, 0)]
		[Export ("performNotificationDefaultAction")]
		void PerformNotificationDefaultAction ();

		[MacCatalyst (14, 0)]
		[Export ("dismissNotificationContentExtension")]
		void DismissNotificationContentExtension ();

		// property, but we have to add the two methods since it is a category.
		[MacCatalyst (14, 0)]
		[Export ("notificationActions")]
		UNNotificationAction [] GetNotificationActions ();

		[MacCatalyst (14, 0)]
		[Export ("setNotificationActions:")]
		void SetNotificationActions (UNNotificationAction [] actions);
	}
}
